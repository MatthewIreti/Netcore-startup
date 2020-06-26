using Microsoft.Extensions.Configuration;
using NCELAP.WebAPI.Models.ODataResponse;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace NCELAP.WebAPI.Services
{
    public class AuthService
    {
        IConfiguration _configuration;
        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetAuthToken()
        {
            string environmentAuthConfig;
            string currentEnv = _configuration.GetSection("Environments").GetSection("current").Value;

            if (currentEnv == "dev")
            {
                environmentAuthConfig = "DevAuthConfig";
            }
            else if (currentEnv == "sat")
            {
                environmentAuthConfig = "SatAuthConfig";
            }
            else if (currentEnv == "cherry")
            {
                environmentAuthConfig = "CherryDevAuthConfig";
            }
            else
            {
                environmentAuthConfig = "ProdAuthConfig";
            }

            var authResponse = new AuthResponse();

            try
            {
                using (var wb = new WebClient())
                {
                    var data = new NameValueCollection
                    {
                        ["grant_type"] = _configuration.GetSection(environmentAuthConfig).GetSection("grant_type").Value,
                        ["client_id"] = _configuration.GetSection(environmentAuthConfig).GetSection("client_id").Value,
                        ["client_secret"] = _configuration.GetSection(environmentAuthConfig).GetSection("client_secret").Value,
                        ["resource"] = _configuration.GetSection(environmentAuthConfig).GetSection("resource").Value
                    };

                    string url = _configuration.GetSection(environmentAuthConfig).GetSection("url").Value;

                    var response = wb.UploadValues(url, "POST", data);
                    string responseInString = Encoding.UTF8.GetString(response);

                    authResponse = JsonConvert.DeserializeObject<AuthResponse>(responseInString);

                    return authResponse.Access_Token.Trim();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.StackTrace);
            }

            return authResponse.Access_Token.Trim();
        }
    }
}
