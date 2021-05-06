using DPRHSE.Common.Models;
using DPRHSE.Common.Models.Entity;
using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace DPRHSE.Business.Services
{
    public interface IAuthService
    {
        Task<Response<TokenModel>> GetToken();
    }
    public class AuthService : IAuthService
    {
      //  private readonly AppSettings appSettings;
        public AuthService()
        {

        }

        public async Task<Response<TokenModel>> GetToken()
        {
            try
            {
                var tokenUrl = "https://login.microsoftonline.com/dde00ac9-104d-4c6f-af96-1adb1039445c/oauth2/token";

                var response = await tokenUrl.PostUrlEncodedAsync(new
                {
                    grant_type = "client_credentials",
                    resource = "https://dprdevenvb40388463e4b1d37devaos.cloudax.dynamics.com",
                    client_id = "25bfbea7-2651-4011-9b82-c2ce10d71ecb",
                    client_secret = "fg=_uN[Y6qD5-cd0CbYapsmeiQ1jAm39",
                }).ReceiveJson<TokenModel>();

                return Response<TokenModel>.Success(response);
            }
            catch (Exception ex)
            {
                return Response<TokenModel>.Failed(ex.Message);
            }
        }
    }
}
