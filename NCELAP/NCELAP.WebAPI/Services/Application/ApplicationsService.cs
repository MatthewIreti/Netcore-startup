﻿using AspNetCore.Http.Extensions;
using Microsoft.Extensions.Configuration;
using NCELAP.WebAPI.Models.DTO.Applications;
using NCELAP.WebAPI.Models.Entities.Applications;
using NCELAP.WebAPI.Models.ODataResponse.Application;
using NCELAP.WebAPI.Util;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace NCELAP.WebAPI.Services.Application
{
    public class ApplicationsService
    {
        readonly IConfiguration _configuration;
        private readonly string licenseapplication, custapplicationshareholder, docupload, licensefees, custlicensebycustomerrecid;
        private string jsonResponse;
        private readonly AuthService _authService;

        public ApplicationsService(IConfiguration configuration)
        {
            _configuration = configuration;
            _authService = new AuthService(_configuration);
            licenseapplication = _configuration.GetSection("Endpoints").GetSection("custlicenseapplication").Value;
            custapplicationshareholder = _configuration.GetSection("Endpoints").GetSection("custapplicationshareholder").Value;
            docupload = _configuration.GetSection("Endpoints").GetSection("licenseapplicationuploads").Value;
            licensefees = _configuration.GetSection("Endpoints").GetSection("licensefee").Value;
            custlicensebycustomerrecid = _configuration.GetSection("Endpoints").GetSection("custlicensebycustomerrecid").Value;
        }

        public async Task<bool> SaveApplication(LicenseApplication licenseApplication)
        {
            bool response = false;
            long applicationRecId = 0;
            var helper = new Helper(_configuration);
            var authOperation = new AuthService(_configuration);

            string token = _authService.GetAuthToken();
            string currentEnvironment = helper.GetEnvironmentUrl();
            var applicationResponse = new ApplicationInfoRecordId();

            var licenseApplicationForSave = this.ExtractApplicationInfo(licenseApplication);

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(currentEnvironment);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");


                    var responseMessage = await client.PostAsJsonAsync(licenseapplication, licenseApplicationForSave);
                    var errorMessage = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    StreamReader sr = new StreamReader(await responseMessage.Content.ReadAsStreamAsync());
                    var applicationSaveResponse = sr.ReadToEnd();
                    applicationResponse = JsonConvert.DeserializeObject<ApplicationInfoRecordId>(applicationSaveResponse);

                    if (responseMessage.IsSuccessStatusCode)
                    {
                        response = true;
                        applicationRecId = applicationResponse.RecordId;

                       // save  shareholders and locations
                       var prospectShareholdersSaveResponse = await this.SaveLicenseApplicationShareholders(licenseApplication.StakeholderLocations, applicationRecId);

                       // do file uploads
                       var licenseApplicationUploadsResponse = await this.UploadLicenseApplicationDocuments(licenseApplication.FileUploads, applicationRecId, licenseApplication.CompanyName);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
            }

            return response;
        }


        public async Task<bool> SaveLicenseApplicationShareholders(StakeholderLocation[] stakeholderLocations, long applicationRecId)
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

                    foreach (var stakeholderLocation in stakeholderLocations)
                    {
                        stakeholderLocation.CustApplication = applicationRecId;
                        stakeholderLocation.UniqueId = Helper.RandomAlhpaNumeric(10);

                        var responseMessage = await client.PostAsJsonAsync(custapplicationshareholder, stakeholderLocation);
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

        public async Task<bool> UploadLicenseApplicationDocuments(LicenseApplicationUpload licenseApplicationUpload, long applicationRecId, string companyName)
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

                    licenseApplicationUpload.LicenseApplicationRecId = applicationRecId;
                    licenseApplicationUpload.DeclarationSignatureFileName = companyName + "_declaration_signature";
                    licenseApplicationUpload.HasLicenseRefusedFileName = companyName + "_licenserefused";
                    licenseApplicationUpload.HasLicenseRevokedFileName = companyName + "_licenserevoked";
                    licenseApplicationUpload.HasRelatedLicenseFileName = companyName + "_relatedlicense";
                    licenseApplicationUpload.HoldRelatedLicenseFileName = companyName + "_holdrelatedlicense";
                    licenseApplicationUpload.ProposedArrangementAttachmentFileName = companyName + "_proposedarrangementlicense";
                    

                    var responseMessage = await client.PostAsJsonAsync(docupload, licenseApplicationUpload);
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

        public async Task<List<ShortApplicationInfo>> GetCustomerApplications(long custrecid)
        {
            string token = _authService.GetAuthToken();
            var helper = new Helper(_configuration);
            string currentEnvironment = helper.GetEnvironmentUrl();
            string url = currentEnvironment + custlicensebycustomerrecid;
            string formattedUrl = String.Format(url, custrecid);
            var applications = new List<ShortApplicationInfo>();

            try
            {
                var webRequest = WebRequest.Create(formattedUrl);
                if (webRequest != null)
                {
                    webRequest.Method = "GET";
                    webRequest.Timeout = 120000;
                    webRequest.Headers.Add("Authorization", "Bearer " + token);

                    WebResponse response = await webRequest.GetResponseAsync();
                    Stream dataStream = response.GetResponseStream();

                    StreamReader reader = new StreamReader(dataStream);

                    var webResponse = new ShortApplicationInfoResponse();
                    jsonResponse = reader.ReadToEnd();

                    webResponse = JsonConvert.DeserializeObject<ShortApplicationInfoResponse>(jsonResponse);
                    applications = webResponse.value;


                    response.Dispose();
                    dataStream.Close();
                    dataStream.Dispose();
                    reader.Close();
                    reader.Dispose();
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
            }

            foreach (var item in applications)
            {
                switch (item.CustLicenseType)
                {
                    case "NetworkAgent":
                        item.CustLicenseType = "Network Agent";
                        break;
                    case "GasShipperLicense":
                        item.CustLicenseType = "Gas Shipper License";
                        break;
                    case "GasTransporterLicense":
                        item.CustLicenseType = "Gas Transporter License";
                        break;
                    default:
                        break;
                }
            }

            return applications;
        }

        public async Task<List<LicenseFee>> GetLicenseFees()
        {
            string token = _authService.GetAuthToken();
            var helper = new Helper(_configuration);
            string currentEnvironment = helper.GetEnvironmentUrl();
            string url = currentEnvironment + licensefees;
            var licenseFees = new List<LicenseFee>();

            try
            {
                var webRequest = WebRequest.Create(url);
                if (webRequest != null)
                {
                    webRequest.Method = "GET";
                    webRequest.Timeout = 120000;
                    webRequest.Headers.Add("Authorization", "Bearer " + token);

                    WebResponse response = await webRequest.GetResponseAsync();
                    Stream dataStream = response.GetResponseStream();

                    StreamReader reader = new StreamReader(dataStream);

                    var webResponse = new LicenseFeeResponse();
                    jsonResponse = reader.ReadToEnd();

                    webResponse = JsonConvert.DeserializeObject<LicenseFeeResponse>(jsonResponse);
                    licenseFees = webResponse.value;


                    response.Dispose();
                    dataStream.Close();
                    dataStream.Dispose();
                    reader.Close();
                    reader.Dispose();
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
            }

            return licenseFees;
        }

        public ApplicationInfoForSave ExtractApplicationInfo(LicenseApplication licenseApplication)
        {
            var dateRegistered = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

            var licenseApplicationForSave = new ApplicationInfoForSave()
            {
                AgentLocationOfShipper = licenseApplication.AgentLocationOfShipper,
                AgentShipperName = licenseApplication.AgentShipperName,
                CustLicenseApplicationStatus = "AwaitingProcessingFee",
                CustLicenseType = licenseApplication.CustLicenseType,
                CustLicenseCategory = "NewApplication",
                Customer = licenseApplication.Customer,
                DeclarationCapacity = licenseApplication.DeclarationCapacity,
                DeclarationDate = licenseApplication.DeclarationDate,
                DeclarationName = !String.IsNullOrEmpty(licenseApplication.DeclarationName) ? licenseApplication.DeclarationName.ToUpper() : "",
                EffectiveDate = licenseApplication.EffectiveDate,
                GasPipelineNetwork = licenseApplication.GasPipelineNetwork,
                HasGasApplicationRefused = String.IsNullOrEmpty(licenseApplication.HasGasApplicationRefused) ? false : Convert.ToBoolean(licenseApplication.HasGasApplicationRefused),
                HasLicenseRevoked = String.IsNullOrEmpty(licenseApplication.HasLicenseRevoked) ? false : Convert.ToBoolean(licenseApplication.HasLicenseRevoked),
                HasRelatedLicense = String.IsNullOrEmpty(licenseApplication.HasRelatedLicense) ? false : Convert.ToBoolean(licenseApplication.HasRelatedLicense),
                HasStandardModificationRequest = String.IsNullOrEmpty(licenseApplication.HasStandardModificationRequest) ? false: Convert.ToBoolean(licenseApplication.HasStandardModificationRequest),
                HoldRelatedLicense = String.IsNullOrEmpty(licenseApplication.HoldRelatedLicense) ? false : Convert.ToBoolean(licenseApplication.HoldRelatedLicense),
                InstalledCapacity = licenseApplication.InstalledCapacity,
                MaximumNominatedCapacity = licenseApplication.MaximumNominatedCapacity,
                ModificationRequestDetails = licenseApplication.ModificationRequestDetails,
                ModificationRequestReason = licenseApplication.ModificationRequestReason,
                //MaximumNominatedCapacity = licenseApplication.MaximumNominatedCapacity,
                PipelineAndGasTransporterName = licenseApplication.PipelineAndGasTransporterName,
                ProposedArrangementLicensingActivity = licenseApplication.ProposedArrangementLicensingActivity,
                RefusedLicenseType = licenseApplication.RefusedLicenseType,
                RelatedLicenseDetail = licenseApplication.RelatedLicenseDetail,
                RelatedLicenseType = licenseApplication.RelatedLicenseType,
                RevokedLicenseType = licenseApplication.RevokedLicenseType,
                SubmittedOn = dateRegistered,
                UniqueId = Helper.RandomAlhpaNumeric(15),
            };

            return licenseApplicationForSave;
        }
    }
}
