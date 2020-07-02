using Microsoft.Extensions.Configuration;
using NCELAP.WebAPI.Models.Entities.Accounts;
using NCELAP.WebAPI.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net.Http;
using AspNetCore.Http.Extensions;
using Newtonsoft.Json;
using System.IO;
using Serilog;
using NCELAP.WebAPI.Models.DTO;
using NCELAP.WebAPI.Models.ODataResponse.Account;

namespace NCELAP.WebAPI.Services.Account
{
    public class UserAccountsService
    {
        readonly IConfiguration _configuration;
        private readonly string businessaccount, businessaccountlegalstatus;
        private readonly AuthService _authService;

        public UserAccountsService(IConfiguration configuration)
        {
            _configuration = configuration;
            _authService = new AuthService(_configuration);
            businessaccount = _configuration.GetSection("Endpoints").GetSection("custprospect").Value;
            businessaccountlegalstatus = _configuration.GetSection("Endpoints").GetSection("custprospectlegalstatus").Value;
        }

        public async Task<bool> SaveBusinessInformation(RegisteredBusiness registeredBusiness)
        {
            bool response = false;

            var helper = new Helper(_configuration);
            var authOperation = new AuthService(_configuration);

            string token = _authService.GetAuthToken();
            string currentEnvironment = helper.GetEnvironmentUrl();
            var custProspectResponse = new CustProspectSaveResponse();

            var custProspectForSave = new RegisteredBusinessForSave()
            {
                Address = registeredBusiness.Address,
                BusinessName = registeredBusiness.BusinessName,
                Email = registeredBusiness.Email,
                Mobile = registeredBusiness.Mobile,
                PostalCode = registeredBusiness.PostalCode,
                Telephone = registeredBusiness.Telephone,
                WebAddress = registeredBusiness.WebAddress,
                AuthorizedRepEmail = registeredBusiness.AuthorizedRepEmail,
                AuthorizedRepMobile = registeredBusiness.AuthorizedRepMobile,
                AuthorizedRepName = registeredBusiness.AuthorizedRepName,
                AuthorizedRepPhysicalAddress = registeredBusiness.AuthorizedRepPhysicalAddress,
                AuthorizedRepTelephone = registeredBusiness.AuthorizedRepTelephone
            };

            // saves registered business info
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(currentEnvironment);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                    var dateRegistered = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    custProspectForSave.CustProspectId = Helper.GetYearMonthDayHourMinuteSeconds();
                    custProspectForSave.RegisteredOn = dateRegistered;

                    var responseMessage = await client.PostAsJsonAsync(businessaccount, custProspectForSave);
                    var errorMessage = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    StreamReader sr = new StreamReader(await responseMessage.Content.ReadAsStreamAsync());
                    var custProspectSaveResponse = sr.ReadToEnd();
                    custProspectResponse = JsonConvert.DeserializeObject<CustProspectSaveResponse>(custProspectSaveResponse);

                    if (responseMessage.IsSuccessStatusCode)
                    {
                        response = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
            }

            return response;
        }

        public async Task<bool> SaveLegalStatusInformation(CustProspectLegalStatus custProspectLegalStatus)
        {
            bool response = false;

            var helper = new Helper(_configuration);
            var authOperation = new AuthService(_configuration);

            string token = _authService.GetAuthToken();
            string currentEnvironment = helper.GetEnvironmentUrl();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(currentEnvironment);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                    var dateRegistered = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                    var responseMessage = await client.PostAsJsonAsync(businessaccountlegalstatus, custProspectLegalStatus);
                    var errorMessage = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    StreamReader sr = new StreamReader(await responseMessage.Content.ReadAsStreamAsync());
                    var reportSaveResponse = sr.ReadToEnd();
                    //reportResponse = JsonConvert.DeserializeObject<ReportInfoResponse>(reportSaveResponse);

                    if (responseMessage.IsSuccessStatusCode)
                    {
                        response = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
            }

            return response;
        }

        // create super admin account
        // send out email informing the super admin an account has been created

    }
}
