using AspNetCore.Http.Extensions;
using Flurl;
using Flurl.Http;
using NCELAP.WebAPI.Models.DTO;
using NCELAP.WebAPI.Util;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NCELAP.WebAPI.Services
{
    public interface IRemitaService
    {
        Task<ReferenceResponseModel> GetRRR(RemitaReferenceRetrievalModel model);
    }
    public class RemitaService : IRemitaService
    {
        private readonly RemitaAppSetting _remitaAppSetting;
        private readonly IHttpClientFactory _clientFactory;
        public RemitaService(RemitaAppSetting remitaAppSetting, IHttpClientFactory clientFactory)
        {
            _remitaAppSetting = remitaAppSetting;
            _clientFactory = clientFactory;

        }
        public async Task<ReferenceResponseModel> GetRRR(RemitaReferenceRetrievalModel model)
        {
            try
            {
                string resps;
                var url = _remitaAppSetting.baseUrl;
                var merchantId = _remitaAppSetting.merchantId;
                string hashValue = Helper.ComputeSHA512($"{merchantId}{model.serviceTypeId}{model.orderId}{model.amount}{_remitaAppSetting.APIKey}");
                var authorization = $"remitaConsumerKey={merchantId},remitaConsumerToken={hashValue}";
                var mainUrl = $"{_remitaAppSetting.domain}/merchant/api/paymentinit";

                var payload= new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                
                var client = _clientFactory.CreateClient("remita");
                client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", authorization);
                var responseMessage = await client.PostAsync(mainUrl, payload);
                
                StreamReader sr = new StreamReader(await responseMessage.Content.ReadAsStreamAsync());
                resps = sr.ReadToEnd();


                var jsonResult = resps.Replace("jsonp (", "")
                  .Replace(")", "");
                var response = JsonConvert.DeserializeObject<ReferenceResponseModel>(jsonResult);
                if (response == null)
                    throw new Exception("Invalid Remita response");
                if (!string.IsNullOrEmpty(response.statusCode) && response.statusCode.Equals("025"))
                {
                    return response;
                }
                else
                {
                    throw new Exception(response.statusMessage);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
