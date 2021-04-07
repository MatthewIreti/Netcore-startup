using AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using NCELAP.WebAPI.Models.DTO.Applications;
using NCELAP.WebAPI.Models.ODataResponse.Application;
using NCELAP.WebAPI.Util;
using Newtonsoft.Json;
using Serilog;
using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace NCELAP.WebAPI.Services.Application
{
    public class LicenseService
    {
        readonly IConfiguration _configuration;
        private readonly AuthService _authService;
        private string jsonResponse;
        public LicenseService(IConfiguration configuration)
        {
            _configuration = configuration;
            _authService = new AuthService(_configuration);
            //licenseapplication = _configuration.GetSection("Endpoints").GetSection("custlicenseapplication").Value;
        }

        public async Task<string> GetLicenseBase64(LicenseCertificate licenseCertificate)
        {
            string token = _authService.GetAuthToken();
            string licenseBase64 = string.Empty;
            string licenseServiceEndpoint = _authService.GetApiServiceEndpoint();
            var helper = new Helper(_configuration);

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(licenseServiceEndpoint);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                    var responseMessage = await client.PostAsJsonAsync("NCLEAS/NcleasService/getLicense", licenseCertificate);
                    var errorMessage = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    StreamReader sr = new StreamReader(await responseMessage.Content.ReadAsStreamAsync());
                    jsonResponse = sr.ReadToEnd();

                    var webResponse = new LicenseCertificateResponse();
                    webResponse = JsonConvert.DeserializeObject<LicenseCertificateResponse>(jsonResponse);
                    licenseBase64 = webResponse.license;
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
                throw;
            }

            return licenseBase64;
        }
    }
}
