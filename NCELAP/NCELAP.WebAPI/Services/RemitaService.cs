using Flurl.Http;
using NCELAP.WebAPI.Models.DTO;
using NCELAP.WebAPI.Util;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NCELAP.WebAPI.Services
{
    public interface IRemitaService
    {
        Task<ReferenceResponseModel> GetRRR(RemitaReferenceRetrievalModel model);
        RemitaReferenceRetrievalModel GetRemitaRRModel(RemitaReferenceRetrievalModel model);
    }
    public class RemitaService : IRemitaService
    {
        private readonly RemitaAppSetting _remitaAppSetting;
        public RemitaService(RemitaAppSetting remitaAppSetting)
        {
            _remitaAppSetting = remitaAppSetting;

        }

        public RemitaReferenceRetrievalModel GetRemitaRRModel(RemitaReferenceRetrievalModel model)
        {
            try
            {
                var url = _remitaAppSetting.baseUrl;
                var merchantId = _remitaAppSetting.merchantId;
                string hashValue = Helper.ComputeSHA512($"{merchantId}{model.serviceTypeId}{model.orderId}{model.amount}{_remitaAppSetting.APIKey}");
                var authorization = $"remitaConsumerKey={merchantId},remitaConsumerToken={hashValue}";
                var mainUrl = $"{_remitaAppSetting.domain}/merchant/api/paymentinit";

                //model.hash = hashValue;
                model.merchantId = merchantId;
                model.url = $"{url}/{mainUrl}";
                return model;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<ReferenceResponseModel> GetRRR(RemitaReferenceRetrievalModel model)
        {
            try
            {
                model = new RemitaReferenceRetrievalModel
                {
                    amount = model.amount,
                    apiKey = model.apiKey,
                    merchantId = model.merchantId,
                    serviceTypeId = model.serviceTypeId,
                    description = model.description,
                    orderId =  model.orderId,
                    payerEmail = model.payerEmail,
                    payerName = model.payerName,
                    url = model.url
                };

                string hashValue = Helper.ComputeSHA512($"{model.merchantId}{model.serviceTypeId}{model.orderId}{model.amount}{model.apiKey}");
                var authorization = $"remitaConsumerKey={model.merchantId},remitaConsumerToken={hashValue}";

                var resps = await model.url
                    .WithHeader("Authorization", authorization)
                    .PostJsonAsync(model)
                    .ReceiveString();
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
