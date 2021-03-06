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
using System.Net;
using NCELAP.WebAPI.Models.ODataResponse;
using NCELAP.WebAPI.Models.Entities.Support;
using Flurl.Http;

namespace NCELAP.WebAPI.Services.Account
{
    public class UserAccountsService
    {
        readonly IConfiguration _configuration;
        private readonly string businessaccount, businessaccountlegalstatus, businessaccountshareholder, businessaccountdirector, docupload, custpropspectstafflist;
        private readonly string ncelasloginendpoint, companynamebycustrecid, accountdetails, ncelasinfoupdate, ncelasusers, ncelasuserbyemail, ncelasusersbycreatorrecid;
        private readonly string contactsupport, passwordreset, ncelaspasswordupdate;
        private string jsonResponse;
        private readonly AuthService _authService;
        private readonly Helper _helper;

        public UserAccountsService(IConfiguration configuration)
        {
            _configuration = configuration;
            _authService = new AuthService(_configuration);
            _helper = new Helper(_configuration);
            businessaccount = _configuration.GetSection("Endpoints").GetSection("custprospect").Value;
            businessaccountlegalstatus = _configuration.GetSection("Endpoints").GetSection("custprospectlegalstatus").Value;
            businessaccountshareholder = _configuration.GetSection("Endpoints").GetSection("custprospectshareholder").Value;
            businessaccountdirector = _configuration.GetSection("Endpoints").GetSection("custprospectsdirector").Value;
            docupload = _configuration.GetSection("Endpoints").GetSection("custprospectupload").Value;
            ncelasloginendpoint = _configuration.GetSection("Endpoints").GetSection("ncelasuserlogin").Value;
            companynamebycustrecid = _configuration.GetSection("Endpoints").GetSection("companynamebycustrecid").Value;
            accountdetails = _configuration.GetSection("Endpoints").GetSection("accountdetails").Value;
            ncelasinfoupdate = _configuration.GetSection("Endpoints").GetSection("ncelasinfoupdate").Value;
            ncelasusers = _configuration.GetSection("Endpoints").GetSection("ncelasusers").Value;
            ncelasuserbyemail = _configuration.GetSection("Endpoints").GetSection("ncelasuserbyemail").Value;
            ncelasusersbycreatorrecid = _configuration.GetSection("Endpoints").GetSection("ncelasusersbycreatorrecid").Value;
            custpropspectstafflist = _configuration.GetSection("Endpoints").GetSection("custpropspectstafflist").Value;
            contactsupport = _configuration.GetSection("Endpoints").GetSection("ncelassupport").Value;
            passwordreset = _configuration.GetSection("Endpoints").GetSection("passwordreset").Value;
            ncelaspasswordupdate = _configuration.GetSection("Endpoints").GetSection("ncelasuserpasswordupdate").Value;
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
                        
                        // save staffs
                        var staffSaveResponse = await this.SaveStaffList(registeredBusiness.Staffs, custProspectResponse.RecordId, custProspectForSave.CustProspectId);

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

        public async Task<bool> SaveStaffList(Staff[] custProspectStaffs, long custProspectRecId, string custProspectId)
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

                    foreach (var custProspectStaff in custProspectStaffs)
                    {
                        custProspectStaff.CustProspectId = custProspectId;
                        custProspectStaff.CustProspect = custProspectRecId;
                        custProspectStaff.UniqueId = Helper.RandomAlhpaNumeric(15);
                        var dateRegistered = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);

                        var responseMessage = await client.PostAsJsonAsync(custpropspectstafflist, custProspectStaff);
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
                    //custProspectUploads.CertificateOfRegistrationFileName = businessName + "_Cert_of_Registration";
                    custProspectUploads.MemorandumArticlesOfAssociationFileName = businessName + "_Memo_Articles_of_Association";
                    //custProspectUploads.DeedOfPartnershipFileName = businessName + "_Deed_of_Partnership";
                    custProspectUploads.TaxClearanceFileName = businessName + "_Tax_Clearance";
                    custProspectUploads.OrganizationChartFileName = businessName + "_Organization_Chart";
                    custProspectUploads.BankStatementFileName = businessName + "_Bank_Statement";

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

        public async Task<NcelasUserResponse> GetNcelasUserAsync(NcelasUserLogin ncelasUserLogin)
        {
            string token = _authService.GetAuthToken();
            string currentEnvironment = _helper.GetEnvironmentUrl();
            string url = currentEnvironment + ncelasloginendpoint;
            string formattedUrl = String.Format(url, ncelasUserLogin.Email, Helper.ComputeSha256Hash(ncelasUserLogin.Password));
            var userAccount = new NcelasUserResponse();

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

                    var webResponse = new NcelasUsersResponse();
                    jsonResponse = reader.ReadToEnd();

                    webResponse = JsonConvert.DeserializeObject<NcelasUsersResponse>(jsonResponse);
                    userAccount = webResponse.value[0];

                    if (userAccount.CustTableRecId > 0)
                    {
                        userAccount.CompanyName = await this.GetCustomerNameByRecId(userAccount.CustTableRecId);
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


            return userAccount;
        }

        public async Task<bool> ChangeNcelasUserPassword(NcelasUserLogin ncelasUserInfoPayload)
        {
            bool response = false;
            string token = _authService.GetAuthToken();
            string currentEnvironment = _helper.GetEnvironmentUrl();
            string url = currentEnvironment + ncelasinfoupdate;
            ncelasUserInfoPayload.Password = Helper.ComputeSha256Hash(ncelasUserInfoPayload.Password);
            //string formattedUrl = String.Format(url, ncelasUserLogin.Email, Helper.ComputeSha256Hash(ncelasUserLogin.Password));
            var userAccount = new NcelasUserResponse();

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(currentEnvironment);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                    var responseMessage = await client.PostAsJsonAsync(url, ncelasUserInfoPayload);
                    var errorMessage = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    //StreamReader sr = new StreamReader(await responseMessage.Content.ReadAsStreamAsync());
                    //var reportSaveResponse = sr.ReadToEnd();
                    
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

        public async Task<NcelasUserResponse> GetNcelasUserByRecId(long userRecId)
        {
            string token = _authService.GetAuthToken();
            string currentEnvironment = _helper.GetEnvironmentUrl();
            string url = currentEnvironment + accountdetails;
            string formattedUrl = String.Format(url, userRecId);
            var userAccount = new NcelasUserResponse();

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

                    var webResponse = new NcelasUsersResponse();
                    jsonResponse = reader.ReadToEnd();

                    webResponse = JsonConvert.DeserializeObject<NcelasUsersResponse>(jsonResponse);
                    userAccount = webResponse.value[0];

                    if (userAccount.CustTableRecId > 0)
                    {
                        userAccount.CompanyName = await this.GetCustomerNameByRecId(userAccount.CustTableRecId);
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

            return userAccount;
        }

        public async Task<string> GetCustomerNameByRecId(long custRecId)
        {
            string token = _authService.GetAuthToken();
            string currentEnvironment = _helper.GetEnvironmentUrl();
            string url = currentEnvironment + companynamebycustrecid;
            string formattedUrl = String.Format(url, custRecId);
            string companyName = string.Empty;

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

                    var webResponse = new CommonResponse();
                    jsonResponse = reader.ReadToEnd();

                    webResponse = JsonConvert.DeserializeObject<CommonResponse>(jsonResponse);
                    companyName = webResponse.value[0].OrganizationName;

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

            return companyName;
        }

        public async Task<bool> CreateUser(UserToCreate userToCreate)
        {
            bool response = false;

            var helper = new Helper(_configuration);
            string token = _authService.GetAuthToken();
            string currentEnvironment = helper.GetEnvironmentUrl();
            string url = currentEnvironment + ncelasusers;

            if (userToCreate != null)
            {
                userToCreate.Activated = "No";
            }
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                    var responseMessage = await client.PostAsJsonAsync(ncelasusers, userToCreate);
                    var errorMessage = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    StreamReader sr = new StreamReader(await responseMessage.Content.ReadAsStreamAsync());
                    var reportSaveResponse = sr.ReadToEnd();

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

        public async Task<bool> ContactSupport(ContactSupport supportMessagePayload)
        {
            bool response = false;

            var helper = new Helper(_configuration);
            string token = _authService.GetAuthToken();
            string currentEnvironment = helper.GetEnvironmentUrl();
            string url = currentEnvironment;

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                    supportMessagePayload.UniqueId = Helper.RandomAlhpaNumeric(15);

                    var responseMessage = await client.PostAsJsonAsync(contactsupport, supportMessagePayload);
                    var errorMessage = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    StreamReader sr = new StreamReader(await responseMessage.Content.ReadAsStreamAsync());
                    var reportSaveResponse = sr.ReadToEnd();

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

        public async Task<bool> GetNcelasUserEmailExist(string email)
        {
            string token = _authService.GetAuthToken();
            string currentEnvironment = _helper.GetEnvironmentUrl();
            string url = currentEnvironment + ncelasuserbyemail;
            string formattedUrl = String.Format(url, email);
            var userAccount = new NcelasUserResponse();
            bool emailexist = false;

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

                    var webResponse = new NcelasUsersResponse();
                    jsonResponse = reader.ReadToEnd();

                    webResponse = JsonConvert.DeserializeObject<NcelasUsersResponse>(jsonResponse);
                    userAccount = webResponse.value[0];

                    if (userAccount.RecordId > 0)
                    {
                        // user account exists, so email exist
                        emailexist = true;
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


            return emailexist;
        }

        public async Task<List<NcelasUserResponse>> GetNcelasUsersByCreatorRecId(long creatorRecId)
        {
            string token = _authService.GetAuthToken();
            string currentEnvironment = _helper.GetEnvironmentUrl();
            string url = currentEnvironment + ncelasusersbycreatorrecid;
            string formattedUrl = String.Format(url, creatorRecId);
            var userAccounts = new List<NcelasUserResponse>();

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

                    var webResponse = new NcelasUsersResponse();
                    jsonResponse = reader.ReadToEnd();

                    webResponse = JsonConvert.DeserializeObject<NcelasUsersResponse>(jsonResponse);
                    userAccounts = webResponse.value;

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


            return userAccounts;
        }
    
        public async Task<PasswordResetResponse> SendPasswordResetCode(string email)
        {
            var helper = new Helper(_configuration);
            string token = _authService.GetAuthToken();
            string currentEnvironment = helper.GetEnvironmentUrl();
            string url = currentEnvironment + passwordreset;
            var passwordReset = new PasswordReset()
            {
                ActivationCode = Helper.RandomNumbers(6),
                Email = email
            };

            var passwordResetResponse = new PasswordResetResponse()
            {
                ActivationCode = passwordReset.ActivationCode,
                Email = email,
                Status = false
            };

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                    var responseMessage = await client.PostAsJsonAsync(passwordreset, passwordReset);
                    var errorMessage = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                    StreamReader sr = new StreamReader(await responseMessage.Content.ReadAsStreamAsync());
                    var passwordResetResponseText = sr.ReadToEnd();

                    if (responseMessage.IsSuccessStatusCode)
                    {
                        passwordResetResponse.Status = true;
                    }

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
            }

            return passwordResetResponse;
        }

        public async Task<bool> PasswordReset(EmailPasswordDto emailPasswordDto)
        {
            var helper = new Helper(_configuration);
            var authOperation = new AuthService(_configuration);

            bool userPasswordUpdateResponse = false;
            string token = authOperation.GetAuthToken();
            string currentEnvironmentEndpoint = helper.GetEnvironmentUrl();
            string requestUri = String.Format(ncelaspasswordupdate, emailPasswordDto.Email);

            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(currentEnvironmentEndpoint);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
                    client.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", "application/json");

                    var userPassPayload = new Dictionary<string, string>
                    {
                        { "Password", Helper.ComputeSha256Hash(emailPasswordDto.Password) }
                    };

                    var requestContent = JsonConvert.SerializeObject(userPassPayload, Formatting.Indented);
                    var stringContent = new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json");

                    var passwordUpdateResponse = await client.PatchAsync(requestUri, stringContent);
                    if (passwordUpdateResponse.StatusCode == HttpStatusCode.NoContent)
                    {
                        userPasswordUpdateResponse = true;

                        //Send password rest confirmation;
                        String postUrl = "https://prod-68.westeurope.logic.azure.com:443/workflows/bca8fbc56ffd4b0f874630f3e0014d6a/triggers/manual/paths/invoke?api-version=2016-06-01&sp=%2Ftriggers%2Fmanual%2Frun&sv=1.0&sig=HMUVikMKxunhFv6SVyGj_4sqCBJ9PRbqIiu3gXP7o9o";
                        var resposne = await postUrl
                            .PostJsonAsync(new{Email = emailPasswordDto.Email})
                            .ReceiveString();
                         


                    }

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
            }

            return userPasswordUpdateResponse;
        }
    }
}
