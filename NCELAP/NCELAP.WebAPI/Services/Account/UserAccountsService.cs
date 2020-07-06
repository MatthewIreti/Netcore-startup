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
        private readonly string businessaccount, businessaccountlegalstatus, businessaccountshareholder, businessaccountdirector, docupload;
        private readonly AuthService _authService;

        public UserAccountsService(IConfiguration configuration)
        {
            _configuration = configuration;
            _authService = new AuthService(_configuration);
            businessaccount = _configuration.GetSection("Endpoints").GetSection("custprospect").Value;
            businessaccountlegalstatus = _configuration.GetSection("Endpoints").GetSection("custprospectlegalstatus").Value;
            businessaccountshareholder  = _configuration.GetSection("Endpoints").GetSection("custprospectshareholder").Value;
            businessaccountdirector = _configuration.GetSection("Endpoints").GetSection("custprospectsdirector").Value;
            docupload = _configuration.GetSection("Endpoints").GetSection("custprospectupload").Value;
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
                CustProspectId = "CUSTPROSP-" + Helper.GetYearMonthDayHourMinuteSeconds(),
                Address = registeredBusiness.Address,
                BusinessName = registeredBusiness.BusinessName,
                Email = registeredBusiness.Email,
                UniqueId = Helper.RandomAlhpaNumeric(15),
                CompanyLegalStatus = "SoleProprietorship",
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
                    custProspectForSave.RegisteredOn = dateRegistered;

                    var responseMessage = await client.PostAsJsonAsync(businessaccount, custProspectForSave);
                    var errorMessage = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    StreamReader sr = new StreamReader(await responseMessage.Content.ReadAsStreamAsync());
                    var custProspectSaveResponse = sr.ReadToEnd();
                    custProspectResponse = JsonConvert.DeserializeObject<CustProspectSaveResponse>(custProspectSaveResponse);

                    if (responseMessage.IsSuccessStatusCode)
                    {
                        response = true;

                        // save business shareholders
                        var prospectShareholdersSaveResponse = await this.SaveCustProspectShareholders(registeredBusiness.Shareholders, custProspectResponse.RecordId, custProspectForSave.CustProspectId);

                        // save business directors
                        var prospectDirectorsSaveResponse = await this.SaveCustProspectDirectors(registeredBusiness.Directors, custProspectResponse.RecordId, custProspectForSave.CustProspectId);

                        // upload supporting documents
                        var supportingDocsUpload = await this.UploadCustProspectSupportingDocuments(registeredBusiness.SupportingDocuments, custProspectResponse.RecordId, custProspectForSave.BusinessName);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
            }

            return response;
        }

        public async Task<bool> SaveCustProspectShareholders(CustProspectShareholder[] custProspectShareholders, long custProspectRecId, string custProspectId)
        {
            bool response = false;

            var helper = new Helper(_configuration);
            var authOperation = new AuthService(_configuration);
            int count = 0;

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

                    foreach (var custProspectShareholder in custProspectShareholders)
                    {
                        custProspectShareholder.CustProspectId = custProspectId;
                        custProspectShareholder.CustProspectRecId = custProspectRecId;
                        custProspectShareholder.UniqueId = Helper.RandomAlhpaNumeric(15);
                        var dateRegistered = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                        var responseMessage = await client.PostAsJsonAsync(businessaccountshareholder, custProspectShareholder);
                        var errorMessage = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                        StreamReader sr = new StreamReader(await responseMessage.Content.ReadAsStreamAsync());
                        var reportSaveResponse = sr.ReadToEnd();

                        if (responseMessage.IsSuccessStatusCode)
                        {
                            count++;
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
            }

            if (count > 0)
            {
                response = true;
            }

            return response;
        }


        public async Task<bool> SaveCustProspectDirectors(CustProspectDirector[] custProspectDirectors, long custProspectRecId, string custProspectId)
        {
            bool response = false;

            var helper = new Helper(_configuration);
            var authOperation = new AuthService(_configuration);
            int count = 0;

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

                    foreach (var custProspectDirector in custProspectDirectors)
                    {
                        custProspectDirector.CustProspectId = custProspectId;
                        custProspectDirector.CustProspectRecId = custProspectRecId;
                        custProspectDirector.UniqueId = Helper.RandomAlhpaNumeric(15);
                        var dateRegistered = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                        var responseMessage = await client.PostAsJsonAsync(businessaccountdirector, custProspectDirector);
                        var errorMessage = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                        StreamReader sr = new StreamReader(await responseMessage.Content.ReadAsStreamAsync());
                        var reportSaveResponse = sr.ReadToEnd();

                        if (responseMessage.IsSuccessStatusCode)
                        {
                            count++;
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
            }

            if (count > 0)
            {
                response = true;
            }

            return response;
        }

        public async Task<bool> UploadCustProspectSupportingDocuments(CustProspectUploads custProspectUploads, long custProspectRecId, string businessName)
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

                    custProspectUploads.CustProspectRecId = custProspectRecId;
                    custProspectUploads.CertificateOfIncorporationFileName = businessName + "_Cert_of_Incorporation";
                    custProspectUploads.CertificateOfRegistrationFileName = businessName + "_Cert_of_Registration";
                    custProspectUploads.MemorandumArticlesOfAssociationFileName = businessName + "_Memo_Articles_of_Association";
                    custProspectUploads.DeedOfPartnershipFileName = businessName + "_Deed_of_Partnership";
                    custProspectUploads.DeedOfTrustFileName = businessName + "_Deed_of_Trust";

                    var dateRegistered = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                    var responseMessage = await client.PostAsJsonAsync(docupload, custProspectUploads);
                    var errorMessage = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    //StreamReader sr = new StreamReader(await responseMessage.Content.ReadAsStreamAsync());
                    //var reportSaveResponse = sr.ReadToEnd();

                    response = true;
                    //if (responseMessage.IsSuccessStatusCode)
                    //{
                    //    count++;
                    //}

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
            }

            return response;
        }
    }
}
