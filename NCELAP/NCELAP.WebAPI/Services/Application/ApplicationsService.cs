using Microsoft.Extensions.Configuration;

using AspNetCore.Http.Extensions;
using Flurl;
using Flurl.Http;
using NCELAP.WebAPI.Models.DTO;
using NCELAP.WebAPI.Models.DTO.Applications;
using NCELAP.WebAPI.Models.Entities.Applications;
using NCELAP.WebAPI.Models.ODataResponse.Application;
using NCELAP.WebAPI.Util;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;

namespace NCELAP.WebAPI.Services.Application
{
    public class ApplicationsService
    {
        readonly IConfiguration _configuration;
        private readonly string licenseapplication, custapplicationshareholder, gasShipperTakeOffLink, gasShipperCustomerLink;
        private readonly string docupload, licensefees, custlicensebycustomerrecid, custlicenseapplicationdetails, networkcodelicenses;
        private string jsonResponse;
        private readonly AuthService _authService;
        private readonly Helper _helper;

        public ApplicationsService(IConfiguration configuration)
        {
            _configuration = configuration;
            _helper = new Helper(_configuration);
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

        public async Task<InfoReponse<BaseApplicationResponse<ApplicationInfo>>> UpdateApplication(APPlicationInfoDetails model)
        {
            InfoReponse<BaseApplicationResponse<ApplicationInfo>> response;
            try
            {
                LicenseUpdateModel item = ExtractApplicationForUpdate(model);
                var updateApplicationResponse = await _helper.GetEnvironmentUrl()
                      .AppendPathSegment($"CustLicenseApplications(UniqueId='{model.UniqueId}',dataAreaId='dpr')")
                      .WithOAuthBearerToken(_authService.GetAuthToken())
                      .PatchJsonAsync(item);

                if (updateApplicationResponse.IsSuccessStatusCode)
                {
                    //continue to update supporting records
                    if (model.CustLicenseType == BaseConstantHelper.gasShipperLicenseType)
                    {
                        if (model.GasShipperCustomers != null && model.GasShipperCustomers.Count > 0)
                        {
                            var gasShipperCustomerForSave = model.GasShipperCustomers.Where(x => string.IsNullOrEmpty(x.UniqueId)).ToArray();
                            var gasShipperCustomerForUpdate = model.GasShipperCustomers.Where(x => !string.IsNullOrEmpty(x.UniqueId)).ToArray();


                            var gasShipperCustomers = await _helper.GetEnvironmentUrl().AppendPathSegment($"GasShipperCustomer")
                               .SetQueryParam("$filter", $"CustApplication eq {model.RecordId}")
                              .WithOAuthBearerToken(_authService.GetAuthToken())
                              .GetJsonAsync<BaseApplicationResponse<GasShipperCustomerDetails>>();

                            var gasShipperFordelete = gasShipperCustomers.value.Where(x => !model.GasShipperCustomers.Any(j=> j.UniqueId.Equals(x.UniqueId)));

                            foreach (var deleteRecord in gasShipperFordelete)
                            {

                                var customerUpdate = await _helper.GetEnvironmentUrl()
                                          .AppendPathSegment($"GasShipperCustomer(UniqueId='{deleteRecord.UniqueId}',dataAreaId='dpr')")
                                          .WithOAuthBearerToken(_authService.GetAuthToken())
                                          .DeleteAsync();
                            }

                            foreach (var shipperCustomer in gasShipperCustomerForUpdate)
                            {
                                var customerUpdate = await _helper.GetEnvironmentUrl()
                                          .AppendPathSegment($"GasShipperCustomer(UniqueId='{shipperCustomer.UniqueId}',dataAreaId='dpr')")
                                          .WithOAuthBearerToken(_authService.GetAuthToken())
                                          .PatchJsonAsync(new GasShipperCustomer
                                          {
                                              CustApplication = shipperCustomer.CustApplication,
                                              BusinessName = shipperCustomer.BusinessName,
                                              Email = shipperCustomer.Email,
                                              GasShipperCustCategory = shipperCustomer.GasShipperCustCategory,
                                              Location = shipperCustomer.Location,
                                              PhoneNumber = shipperCustomer.PhoneNumber
                                          });
                            }
                            if (gasShipperCustomerForSave.Length>0)
                            {
                                await SaveGasShipperCustomers(gasShipperCustomerForSave, model.RecordId);
                            }
                        }
                        if (model.GasShipperTakeOffPoints != null && model.GasShipperTakeOffPoints.Count > 0)
                        {
                            var gasShipperForSave = model.GasShipperTakeOffPoints.Where(x => string.IsNullOrEmpty(x.UniqueId)).ToArray();
                            var gasShipperForUpdate = model.GasShipperTakeOffPoints.Where(x => !string.IsNullOrEmpty(x.UniqueId));

                            var gasShipperTakeOffPoints = await _helper.GetEnvironmentUrl().AppendPathSegment($"GasShipperDeliveryTakeOffPoint")
                                .SetQueryParam("$filter", $"CustApplication eq {model.RecordId}")
                               .WithOAuthBearerToken(_authService.GetAuthToken())
                               .GetJsonAsync<BaseApplicationResponse<GasShipperTakeOffPointDetails>>();

                            var takeOffPointsForDelete = gasShipperTakeOffPoints.value.Where(x => !model.GasShipperTakeOffPoints.Any(j => j.UniqueId.Equals(x.UniqueId)));

                            foreach (var deleteRecord in takeOffPointsForDelete)
                            {

                                var pointsDeleteResponse = await _helper.GetEnvironmentUrl()
                                          .AppendPathSegment($"GasShipperDeliveryTakeOffPoint(UniqueId='{deleteRecord.UniqueId}',dataAreaId='dpr')")
                                          .WithOAuthBearerToken(_authService.GetAuthToken())
                                          .DeleteAsync();
                            }


                            foreach (var m in gasShipperForUpdate)
                            {
                                var pointUpdate = await _helper.GetEnvironmentUrl()
                                         .AppendPathSegment($"GasShipperDeliveryTakeOffPoint(UniqueId='{m.UniqueId}',dataAreaId='dpr')")
                                         .WithOAuthBearerToken(_authService.GetAuthToken())
                                         .PatchJsonAsync(new GasShipperTakeOffPoint
                                         {
                                             CustApplication = m.CustApplication,
                                             GasShipperPointType = m.GasShipperPointType,
                                             Location = m.Location,
                                             Name = m.Name
                                         });
                            }
                            if (gasShipperForSave.Length > 0)
                            {
                                await SaveGasShipperTakeOffPoints(gasShipperForSave, model.RecordId);
                            }
                        }
                    }
                    else if (model.CustLicenseType == BaseConstantHelper.gasTransporterLicenseType)
                    {
                        if (model.StakeholderLocations != null && model.StakeholderLocations.Count > 0)
                        {
                            var stakeHolderForSave = model.StakeholderLocations.Where(x => string.IsNullOrEmpty(x.UniqueId)).ToArray();
                            var stakeHolderForUpdate = model.StakeholderLocations.Where(x => !string.IsNullOrEmpty(x.UniqueId)).ToArray();
                            var proposedShareholders = await _helper.GetEnvironmentUrl().AppendPathSegment("AppCustShareholders")
                                .SetQueryParam("$filter", $"CustApplication eq {model.RecordId}")
                                .WithOAuthBearerToken(_authService.GetAuthToken())
                                .GetJsonAsync<BaseApplicationResponse<StakeholderLocationDetails>>();

                            var shareHoldersForDelete = proposedShareholders.value.Where(x => !model.StakeholderLocations.Any(j => j.UniqueId.Equals(x.UniqueId)));

                            foreach (var deleteRecord in shareHoldersForDelete)
                            {

                                var shareHolderUpdate = await _helper.GetEnvironmentUrl()
                                          .AppendPathSegment($"AppCustShareholders(UniqueId='{deleteRecord.UniqueId}',dataAreaId='dpr')")
                                          .WithOAuthBearerToken(_authService.GetAuthToken())
                                          .DeleteAsync();
                            }

                            foreach (var r in stakeHolderForUpdate)
                            {
                                var pointUpdate = await _helper.GetEnvironmentUrl()
                                         .AppendPathSegment($"AppCustShareholders(UniqueId='{r.UniqueId}',dataAreaId='dpr')")
                                         .WithOAuthBearerToken(_authService.GetAuthToken())
                                         .PatchJsonAsync(new StakeholderLocation
                                         {
                                             CustApplication = r.CustApplication,
                                             Customer = r.Customer,
                                             Location = r.Location
                                         });
                            }
                            if (stakeHolderForSave.Length > 0)
                            {
                                await SaveLicenseApplicationShareholders(stakeHolderForSave, model.RecordId);
                            }
                        }

                    }
                    if (model.FileUploads != null)
                    {
                        var uploadResponse = await UploadLicenseApplicationDocuments(model.FileUploads, model.RecordId, model.CompanyName);
                        if (!uploadResponse)
                        {
                            throw new Exception("An error occurred during file upload");
                        }
                    }
                    
                }
                response = new InfoReponse<BaseApplicationResponse<ApplicationInfo>>
                {
                    Data = null,
                    Message = $"Successful Operation {updateApplicationResponse.StatusCode}",
                    Status = true
                };
            }
            catch (Exception ex)
            {
                response = new InfoReponse<BaseApplicationResponse<ApplicationInfo>>
                {
                    Data = null,
                    Message = ex.Message,
                    Status = false
                };
            }
            return response;
        }

        private LicenseUpdateModel ExtractApplicationForUpdate(APPlicationInfoDetails licenseApplication)
        {
            LicenseUpdateModel licenseApplicationForSave = new LicenseUpdateModel
            {
                AgentLocationOfShipper = licenseApplication.AgentLocationOfShipper,
                UniqueId = licenseApplication.UniqueId,
                AgentShipperName = licenseApplication.AgentShipperName,
                CustLicenseType = licenseApplication.CustLicenseType,
                CustLicenseCategory = licenseApplication.CustLicenseCategory,
                Customer = licenseApplication.Customer,
                CustomerTier = licenseApplication.CustomerTier,
                SubmittedBy = licenseApplication.SubmittedBy,
                DeclarationCapacity = licenseApplication.DeclarationCapacity,
                DeclarationDate = licenseApplication.DeclarationDate,
                DeclarationName = !String.IsNullOrEmpty(licenseApplication.DeclarationName) ? licenseApplication.DeclarationName.ToUpper() : "",
                EffectiveDate = licenseApplication.EffectiveDate,
                GasPipelineNetwork = licenseApplication.GasPipelineNetwork,
                HasGasApplicationRefused = licenseApplication.HasGasApplicationRefused,
                HasLicenseRevoked = licenseApplication.HasLicenseRevoked,
                HasRelatedLicense = licenseApplication.HasRelatedLicense,
                HasStandardModificationRequest = licenseApplication.HasStandardModificationRequest,
                HoldRelatedLicense = licenseApplication.HoldRelatedLicense,
                InstalledCapacity = licenseApplication.InstalledCapacity,
                Location = licenseApplication.Location,
                MaximumNominatedCapacity = licenseApplication.MaximumNominatedCapacity,
                ModificationRequestDetails = licenseApplication.ModificationRequestDetails,
                ModificationRequestReason = licenseApplication.ModificationRequestReason,
                PipelineAndGasTransporterName = licenseApplication.PipelineAndGasTransporterName,
                ProposedArrangementLicensingActivity = licenseApplication.ProposedArrangementLicensingActivity,
                RefusedLicenseType = licenseApplication.RefusedLicenseType,
                RelatedLicenseDetail = licenseApplication.RelatedLicenseDetail,
                RelatedLicenseType = licenseApplication.RelatedLicenseType,
                RevokedLicenseType = licenseApplication.RevokedLicenseType,
                EntryPoint = licenseApplication.EntryPoint,
                LicenseFeeCategory = licenseApplication.LicenseFeeCategory,
                ExitPoint = licenseApplication.ExitPoint,
                SubmittedOn = licenseApplication.SubmittedOn,
                ExitPointState = licenseApplication.ExitPointState,
                EntryPointState = licenseApplication.EntryPointState
            };
            return licenseApplicationForSave;
        }
        public async Task<InfoReponse<ApplicationInfo>> SaveApplication(LicenseApplication licenseApplication)
        {
            var response = new InfoReponse<ApplicationInfo>();
            var licenseApplicationForSave = ExtractApplicationInfo(licenseApplication);
            try
            {
                var responseMessage = await _helper.GetEnvironmentUrl()
                     .AppendPathSegment($"CustLicenseApplications")
                     .WithOAuthBearerToken(_authService.GetAuthToken())
                     .PostJsonAsync(licenseApplicationForSave)
                     .ReceiveJson<ApplicationInfo>();

                if (responseMessage != null)
                {
                    response = new InfoReponse<ApplicationInfo>
                    {
                        Data = responseMessage,
                        Status = true
                    };

                    // save  shareholders and locations
                    if (licenseApplication.CustLicenseType == BaseConstantHelper.gasShipperLicenseType)
                    {
                        var customerSaveResponse = await SaveGasShipperCustomers(licenseApplication.GasShipperCustomers, responseMessage.RecordId);
                        var takeoffPointSaveResponse = await SaveGasShipperTakeOffPoints(licenseApplication.TakeOffPoints, responseMessage.RecordId);
                    }
                    else
                    {
                        var prospectShareholdersSaveResponse = await this.SaveLicenseApplicationShareholders(licenseApplication.StakeholderLocations, responseMessage.RecordId);
                    }
                    // do file uploads
                    var licenseApplicationUploadsResponse = await this.UploadLicenseApplicationDocuments(licenseApplication.FileUploads, responseMessage.RecordId, licenseApplication.CompanyName);
                }
            }
            catch (Exception ex)
            {
                response = new InfoReponse<ApplicationInfo>
                {
                    Data = null,
                    Status = false,
                    Message = ex.Message
                };
            }

            return response;
        }


        private async Task<bool> SaveLicenseApplicationShareholders(StakeholderLocation[] stakeholderLocations, long applicationRecId)
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


        private async Task<bool> SaveGasShipperCustomers(GasShipperCustomer[] customers, long applicationRecId)
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
        private async Task<bool> SaveGasShipperTakeOffPoints(GasShipperTakeOffPoint[] model, long applicationRecId)
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
        private async Task<bool> UploadLicenseApplicationDocuments(LicenseApplicationUpload licenseApplicationUpload, long applicationRecId, string companyName)
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
                    licenseApplicationUpload.DeclarationSignatureFileName = companyName + BaseConstantHelper.DeclarationSignatureFileName;
                    licenseApplicationUpload.HasLicenseRefusedFileName = companyName + BaseConstantHelper.HasLicenseRefusedFileName;
                    licenseApplicationUpload.HasLicenseRevokedFileName = companyName + BaseConstantHelper.HasLicenseRevokedFileName;
                    licenseApplicationUpload.HasRelatedLicenseFileName = companyName + BaseConstantHelper.HasRelatedLicenseFileName;
                    licenseApplicationUpload.HoldRelatedLicenseFileName = companyName + BaseConstantHelper.HoldRelatedLicenseFileName;
                    licenseApplicationUpload.ProposedArrangementAttachmentFileName = companyName + BaseConstantHelper.ProposedArrangementAttachmentFileName;
                    licenseApplicationUpload.OPLFileName = $"{companyName}{BaseConstantHelper.OPLFileName}";
                    licenseApplicationUpload.SafetyCaseFileName = $"{companyName}{BaseConstantHelper.SafetyCaseFileName}";
                    licenseApplicationUpload.SCADAFileName = $"{companyName}{BaseConstantHelper.SCADAFileName}";
                    licenseApplicationUpload.GTSFileName = $"{companyName}{BaseConstantHelper.GTSFileName}";
                    licenseApplicationUpload.TechnicalAttributeFileName = $"{companyName}{BaseConstantHelper.TechnicalAttributeFileName}";
                    licenseApplicationUpload.AuxiliarySystemFileName = $"{companyName}{BaseConstantHelper.AuxiliarySystemFileName}";
                    licenseApplicationUpload.TariffAndPricingFileName = $"{companyName}{BaseConstantHelper.TariffAndPricingFileName}";
                    licenseApplicationUpload.RiskManagementFileName = $"{companyName}{BaseConstantHelper.RiskManagementFileName}";
                    licenseApplicationUpload.CommunityMOUFileName = $"{companyName}{BaseConstantHelper.CommunityMOUFileName}";
                    licenseApplicationUpload.NetworkAgentOPLFileName = $"{companyName}{BaseConstantHelper.OPLFileName}";
                    licenseApplicationUpload.GasShipperOPLFileName = $"{companyName}{BaseConstantHelper.OPLFileName}";

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
                        switch (item.CustLicenseCategory)
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

        private ApplicationInfoForSave ExtractApplicationInfo(LicenseApplication licenseApplication)
        {
            var licenseApplicationForSave = new ApplicationInfoForSave();
            var dateRegistered = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

            licenseApplicationForSave = new ApplicationInfoForSave
            {
                AgentLocationOfShipper = licenseApplication.AgentLocationOfShipper,
                AgentShipperName = licenseApplication.AgentShipperName,
                // CustLicenseApplicationStatus = "AwaitingProcessingFee",
                CustLicenseType = licenseApplication.CustLicenseType,
                CustLicenseCategory = "NewApplication",
                Customer = licenseApplication.Customer,
                CustomerTier = licenseApplication.CustomerTier,
                SubmittedBy = licenseApplication.SubmittedBy,
                DeclarationCapacity = licenseApplication.DeclarationCapacity,
                DeclarationDate = licenseApplication.DeclarationDate,
                DeclarationName = !String.IsNullOrEmpty(licenseApplication.DeclarationName) ? licenseApplication.DeclarationName.ToUpper() : "",
                EffectiveDate = licenseApplication.EffectiveDate,
                GasPipelineNetwork = licenseApplication.GasPipelineNetwork,
                Location = licenseApplication.Location,
                HasGasApplicationRefused = licenseApplication.HasGasApplicationRefused,
                HasLicenseRevoked = licenseApplication.HasLicenseRevoked,
                HasRelatedLicense = licenseApplication.HasRelatedLicense,
                HasStandardModificationRequest = licenseApplication.HasStandardModificationRequest,
                HoldRelatedLicense = licenseApplication.HoldRelatedLicense,
                InstalledCapacity = licenseApplication.InstalledCapacity,
                MaximumNominatedCapacity = licenseApplication.MaximumNominatedCapacity,
                ModificationRequestDetails = licenseApplication.ModificationRequestDetails,
                ModificationRequestReason = licenseApplication.ModificationRequestReason,
                PipelineAndGasTransporterName = licenseApplication.PipelineAndGasTransporterName,
                ProposedArrangementLicensingActivity = licenseApplication.ProposedArrangementLicensingActivity,
                ProposedArrangementDetail = licenseApplication.ProposedArrangementDetail,
                RefusedLicenseType = licenseApplication.RefusedLicenseType,
                RelatedLicenseDetail = licenseApplication.RelatedLicenseDetail,
                RelatedLicenseType = licenseApplication.RelatedLicenseType,
                RevokedLicenseType = licenseApplication.RevokedLicenseType,
                LicenseFeeCategory = licenseApplication.LicenseFeeCategory,
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

        public async Task<APPlicationInfoDetails> GetLicenseApplicationDetails(long licenseApplicationRecId)
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

                    var webResponse = JsonConvert.DeserializeObject<BaseApplicationResponse<APPlicationInfoDetails>>(jsonResponse);
                    applicationInfo = webResponse.value[0];

                    var proposedShareholders = await currentEnvironment.AppendPathSegment("AppCustShareholders")
                        .SetQueryParam("$filter", $"CustApplication eq {licenseApplicationRecId}")
                        .WithOAuthBearerToken(token)
                        .GetJsonAsync<BaseApplicationResponse<StakeholderLocationDetails>>();

                    applicationInfo.StakeholderLocations = proposedShareholders.value;

                    var applicationComment = await currentEnvironment.AppendPathSegment("LicenseApplicationComment")
                        .SetQueryParam("$filter", $"LicenseApplication eq {licenseApplicationRecId}")
                        .WithOAuthBearerToken(token)
                        .GetJsonAsync<BaseApplicationResponse<LicenseApplicationCommentModel>>();

                    //Get the attachments
                    var attachments = await _authService.GetApiServiceEndpoint()
                        .AppendPathSegment("NCLEAS/LicenseAttachmentService/GetAttachments")
                        .WithOAuthBearerToken(token)
                        .PostJsonAsync(new LicenseAttachment
                        {
                            contract = new LicenseCertificatePayload
                            {
                                CustLicenseApplicationId = licenseApplicationRecId
                            }
                        }).ReceiveJson<List<LicenseAttachmentModel>>();

                    applicationInfo.LicenseApplicationAttachments = attachments;
                    applicationInfo.LicenseApplicationComments = applicationComment.value;

                    if (applicationInfo.CustLicenseType == BaseConstantHelper.gasShipperLicenseType)
                    {
                        var gasShipperCustomers = await currentEnvironment.AppendPathSegment($"GasShipperCustomer")
                             .SetQueryParam("$filter", $"CustApplication eq {licenseApplicationRecId}")
                            .WithOAuthBearerToken(token)
                            .GetJsonAsync<BaseApplicationResponse<GasShipperCustomerDetails>>();
                        applicationInfo.GasShipperCustomers = gasShipperCustomers.value;

                        var gasShipperTakeOffPoints = await currentEnvironment.AppendPathSegment($"GasShipperDeliveryTakeOffPoint")
                            .SetQueryParam("$filter", $"CustApplication eq {licenseApplicationRecId}")
                           .WithOAuthBearerToken(token)
                           .GetJsonAsync<BaseApplicationResponse<GasShipperTakeOffPointDetails>>();
                        applicationInfo.GasShipperTakeOffPoints = gasShipperTakeOffPoints.value;
 

                    }

                    if (applicationInfo.CustLicenseType == "NetworkAgent")
                    {
                        applicationInfo.LicenseType = "Network Agent";
                    }

                    if (applicationInfo.CustLicenseType == "GasShipperLicense")
                    {
                        applicationInfo.LicenseType = "Gas Shipper License";
                    }

                    if (applicationInfo.CustLicenseType == "GasTransporterLicense")
                    {
                        applicationInfo.LicenseType = "Gas Transporter License";
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
