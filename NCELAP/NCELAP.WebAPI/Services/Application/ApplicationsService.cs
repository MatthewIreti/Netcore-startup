using AspNetCore.Http.Extensions;
using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using NCELAP.WebAPI.Models.DTO;
using NCELAP.WebAPI.Models.DTO.Applications;
using NCELAP.WebAPI.Models.Entities.Applications;
using NCELAP.WebAPI.Models.ODataResponse.Account;
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
        private readonly string licenseapplication, custapplicationshareholder, gasShipperTakeOffLink, gasShipperCustomerLink;
        private readonly string docupload, licensefees, custlicensebycustomerrecid, custlicenseapplicationdetails, networkcodelicenses;
        private string jsonResponse;
        private readonly AuthService _authService;

        public ApplicationsService(IConfiguration configuration)
        {
            _configuration = configuration;
            _authService = new AuthService(_configuration);
            licenseapplication = _configuration.GetSection("Endpoints").GetSection("custlicenseapplication").Value;
            custapplicationshareholder = _configuration.GetSection("Endpoints").GetSection("custapplicationshareholder").Value;
            gasShipperTakeOffLink = _configuration.GetSection("Endpoints").GetSection("gasShipperTakeOffPoints").Value;
            gasShipperCustomerLink = _configuration.GetSection("Endpoints").GetSection("gasShipperCustomers").Value;
            docupload = _configuration.GetSection("Endpoints").GetSection("licenseapplicationuploads").Value;
            licensefees = _configuration.GetSection("Endpoints").GetSection("licensefee").Value;
            custlicensebycustomerrecid = _configuration.GetSection("Endpoints").GetSection("custlicensebycustomerrecid").Value;
            custlicenseapplicationdetails = _configuration.GetSection("Endpoints").GetSection("custlicenseapplicationdetails").Value;
            networkcodelicenses = _configuration.GetSection("Endpoints").GetSection("networkcodelicenses").Value;
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
                        if (licenseApplication.CustLicenseType == BaseConstantHelper.gasShipperLicenseType)
                        {
                            var customerSaveResponse = await SaveGasShipperCustomers(licenseApplication.GasShipperCustomers, applicationRecId);
                            var takeoffPointSaveResponse = await SaveGasShipperTakeOffPoints(licenseApplication.TakeOffPoints, applicationRecId);
                        }
                        else
                        {
                            var prospectShareholdersSaveResponse = await this.SaveLicenseApplicationShareholders(licenseApplication.StakeholderLocations, applicationRecId);
                        }
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
                return count > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
                throw;
            }
            
        }

        
        public async Task<bool> SaveGasShipperCustomers(GasShipperCustomer[] customers, long applicationRecId)
        {
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

                    foreach (var item in customers)
                    {
                        item.CustApplication = applicationRecId;

                        var responseMessage = await client.PostAsJsonAsync(gasShipperCustomerLink, item);
                        var errorMessage = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                        StreamReader sr = new StreamReader(await responseMessage.Content.ReadAsStreamAsync());
                        var reportSaveResponse = sr.ReadToEnd();

                        if (responseMessage.IsSuccessStatusCode)
                        {
                            count++;
                        }
                    }

                }
                return count > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
                throw;
            }
        }
        public async Task<bool> SaveGasShipperTakeOffPoints(GasShipperTakeOffPoint[] model, long applicationRecId)
        {

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

                    foreach (var item in model)
                    {
                        item.CustApplication = applicationRecId;

                        var responseMessage = await client.PostAsJsonAsync(gasShipperTakeOffLink, item);
                        var errorMessage = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                        StreamReader sr = new StreamReader(await responseMessage.Content.ReadAsStreamAsync());
                        var reportSaveResponse = sr.ReadToEnd();

                        if (responseMessage.IsSuccessStatusCode)
                        {
                            count++;
                        }
                    }

                }
                return count > 0;
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
                throw;
            }

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
                    licenseApplicationUpload.OPLFileName = $"{companyName}_OPL_License";
                    licenseApplicationUpload.SafetyCaseFileName = $"{companyName}_SafetyCaseApproved";
                    licenseApplicationUpload.SCADAFileName = $"{companyName}_SCADA_System";
                    licenseApplicationUpload.GTSFileName = $"{companyName}_Gas_transmission_system";
                    licenseApplicationUpload.TechnicalAttributeFileName = $"{companyName}_technical_attributes";
                    licenseApplicationUpload.AuxiliarySystemFileName = $"{companyName}_Auxiliary_systems";
                    licenseApplicationUpload.TariffAndPricingFileName = $"{companyName}_Tarrif_and_pricing";
                    licenseApplicationUpload.RiskManagementFileName = $"{companyName}_Risk_management";
                    licenseApplicationUpload.CommunityMOUFileName = $"{companyName}_CommunityMOU";
                    licenseApplicationUpload.NetworkAgentOPLFileName = $"{companyName}_OPL_License";
                    licenseApplicationUpload.GasShipperOPLFileName = $"{companyName}_OPL_License";

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

        public async Task<List<CustApplicationExtension>> GetCustomerApplications(long custrecid)
        {
            string token = _authService.GetAuthToken();
            var helper = new Helper(_configuration);
            string currentEnvironment = helper.GetEnvironmentUrl();
            string url = currentEnvironment + custlicensebycustomerrecid;
            string formattedUrl = String.Format(url, custrecid);
            var applications = new List<CustApplicationExtension>();

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
                    foreach (var item in applications)
                    {
                        switch (item.CustLicenseType)
                        {
                            case "NetworkAgent":
                                item.CustLicenseName = "Network Agent";
                                break;
                            case "GasShipperLicense":
                                item.CustLicenseName = "Gas Shipper License";
                                break;
                            case "GasTransporterLicense":
                                item.CustLicenseName = "Gas Transporter License";
                                break;
                            default:
                                break;
                        }
                        switch(item.CustLicenseCategory)
                        {
                            case BaseConstantHelper.NewApplication:
                                item.CustLicenseCategoryName = "New";
                                break;
                            case BaseConstantHelper.Renewal:
                                item.CustLicenseCategoryName = "Renewal";
                                break;
                            default:
                                break;
                        }
                        switch (item.CustLicenseApplicationStatus)
                        {
                            case BaseConstantHelper.Submitted:
                                item.CustLicenseApplicationStatusName = "In review";
                                break;
                            case BaseConstantHelper.ChangeRequested:
                                item.CustLicenseApplicationStatusName = "In review";
                                break;
                            case BaseConstantHelper.Approved:
                                item.CustLicenseApplicationStatusName = "Approved";
                                break;
                            case BaseConstantHelper.Rejected:
                                item.CustLicenseApplicationStatusName = "Rejected";
                                break;
                            case BaseConstantHelper.Returned:
                                item.CustLicenseApplicationStatusName = "Change requested";
                                break;
                            case BaseConstantHelper.Cancelled:
                                item.CustLicenseApplicationStatusName = "Rejected";
                                break;
                            case BaseConstantHelper.AwaitingProcessingFee:
                                item.CustLicenseApplicationStatusName = "Awaiting processing fee";
                                break;
                            case BaseConstantHelper.AwaitingLicenseFee:
                                item.CustLicenseApplicationStatusName = "Awaiting License Fee";
                                break;
                            case BaseConstantHelper.Active:
                                item.CustLicenseApplicationStatusName = "Active";
                                break;
                            case BaseConstantHelper.Expired:
                                item.CustLicenseApplicationStatusName = "Expired";
                                break;
                            case BaseConstantHelper.DueForRenewal:
                                item.CustLicenseApplicationStatusName = "Due For Renewal";
                                break;
                            default:
                                break;
                        }
                        
                    }

                    response.Dispose();
                    dataStream.Close();
                    dataStream.Dispose();
                    reader.Close();
                    reader.Dispose();
                }
                return applications;
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
                throw;
            }
        }

        public async Task<List<NetworkCodeLicense>> GetCustomerApplicationLicenses(long custrecid)
        {
            string token = _authService.GetAuthToken();
            var helper = new Helper(_configuration);
            string currentEnvironment = helper.GetEnvironmentUrl();
            string url = currentEnvironment + networkcodelicenses;
            string formattedUrl = String.Format(url, custrecid);
            var networkCodeLicenses = new List<NetworkCodeLicense>();

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

                    var webResponse = new NetworkCodeResponse();
                    jsonResponse = reader.ReadToEnd();

                    webResponse = JsonConvert.DeserializeObject<NetworkCodeResponse>(jsonResponse);
                    networkCodeLicenses = webResponse.value;
                    
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
                throw;
            }

            return networkCodeLicenses;
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
                HasStandardModificationRequest = String.IsNullOrEmpty(licenseApplication.HasStandardModificationRequest) ? false : Convert.ToBoolean(licenseApplication.HasStandardModificationRequest),
                HoldRelatedLicense = String.IsNullOrEmpty(licenseApplication.HoldRelatedLicense) ? false : Convert.ToBoolean(licenseApplication.HoldRelatedLicense),
                InstalledCapacity = licenseApplication.InstalledCapacity,
                MaximumNominatedCapacity = licenseApplication.MaximumNominatedCapacity,
                ModificationRequestDetails = licenseApplication.ModificationRequestDetails,
                ModificationRequestReason = licenseApplication.ModificationRequestReason,
                PipelineAndGasTransporterName = licenseApplication.PipelineAndGasTransporterName,
                ProposedArrangementLicensingActivity = licenseApplication.ProposedArrangementLicensingActivity,
                RefusedLicenseType = licenseApplication.RefusedLicenseType,
                RelatedLicenseDetail = licenseApplication.RelatedLicenseDetail,
                RelatedLicenseType = licenseApplication.RelatedLicenseType,
                RevokedLicenseType = licenseApplication.RevokedLicenseType,
                SubmittedOn = dateRegistered,
                UniqueId = Helper.RandomAlhpaNumeric(15),
                EntryPoint = licenseApplication.EntryPoint,
                ExitPoint = licenseApplication.ExitPoint,
                ExitPointState = licenseApplication.ExitPointState,
                EntryPointState = licenseApplication.EntryPointState
            };

            return licenseApplicationForSave;
        }

        public async Task<List<DPRZoneStates>> GetZoneStates()
        {
            try
            {
                string token = _authService.GetAuthToken();
                var helper = new Helper(_configuration);
                string currentEnvironment = helper.GetEnvironmentUrl();
                var response = await currentEnvironment.AppendPathSegment("DprZoneStates")
                    .WithOAuthBearerToken(token)
                    .GetJsonAsync<BaseApplicationResponse<DPRZoneStates>>();
                return response.value;
            }
            catch (Exception exp)
            {
                Log.Error(exp.StackTrace);
                throw new Exception(exp.Message);
            }
        }

        public async Task<ApplicationInfo> GetLicenseApplicationDetails(long licenseApplicationRecId)
        {
            string token = _authService.GetAuthToken();
            var helper = new Helper(_configuration);
            string currentEnvironment = helper.GetEnvironmentUrl();
            string url = currentEnvironment + custlicenseapplicationdetails;
            string formattedUrl = String.Format(url, licenseApplicationRecId);
            var applicationInfo = new APPlicationInfoDetails();

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

                    jsonResponse = reader.ReadToEnd();

                   var  webResponse = JsonConvert.DeserializeObject<BaseApplicationResponse<APPlicationInfoDetails>>(jsonResponse);
                    applicationInfo = webResponse.value[0];

                    var proposedShareholders = await currentEnvironment.AppendPathSegment("AppCustShareholders")
                        .SetQueryParam("$filter", $"CustApplication eq {licenseApplicationRecId}")
                        .WithOAuthBearerToken(token)
                        .GetJsonAsync<BaseApplicationResponse<StakeholderLocation>>();

                    applicationInfo.StakeholderLocations = proposedShareholders.value;

                    var applicationComment = await currentEnvironment.AppendPathSegment("LicenseApplicationComment")
                        .SetQueryParam("$filter", $"LicenseApplication eq {licenseApplicationRecId}")
                        .WithOAuthBearerToken(token)
                        .GetJsonAsync<BaseApplicationResponse<LicenseApplicationCommentModel>>();

                    applicationInfo.LicenseApplicationComments = applicationComment.value;

                    if (applicationInfo.CustLicenseType == BaseConstantHelper.gasShipperLicenseType)
                    {
                        var gasShipperCustomers = await currentEnvironment.AppendPathSegment($"GasShipperCustomer")
                             .SetQueryParam("$filter", $"CustApplication eq {licenseApplicationRecId}")
                            .WithOAuthBearerToken(token)
                            .GetJsonAsync<BaseApplicationResponse<GasShipperCustomer>>();
                        applicationInfo.GasShipperCustomers = gasShipperCustomers.value;

                        var gasShipperTakeOffPoints = await currentEnvironment.AppendPathSegment($"GasShipperDeliveryTakeOffPoint")
                            .SetQueryParam("$filter", $"CustApplication eq {licenseApplicationRecId}")
                           .WithOAuthBearerToken(token)
                           .GetJsonAsync<BaseApplicationResponse<GasShipperTakeOffPoint>>();
                        applicationInfo.GasShipperTakeOffPoints = gasShipperTakeOffPoints.value;

                    }
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

            return applicationInfo;
        }
    }
}
