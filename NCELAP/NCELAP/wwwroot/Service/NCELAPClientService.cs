using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Flurl;
using Flurl.Http;

namespace NCELAP.Service
{
    public interface INCELAPClientService
    {
        Task<InfoReponse<RemitaCollectionResponse>> GetPaymentStatus(string rrr);
    }
    public class NCELAPClientService :INCELAPClientService
    {
        private readonly AppSettings _appSettings;
        public NCELAPClientService(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public async Task<InfoReponse<RemitaCollectionResponse>> GetPaymentStatus(string rrr)
        {
            try
            {
                var url = _appSettings.ncelasApiBaseUrl;
                var response = await url.AppendPathSegment($"api/payment/status/{rrr}")
                    .GetAsync()
                    .ReceiveJson<InfoReponse<RemitaCollectionResponse>>();
                return response;   
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
    public class RemitaCollectionResponse
    {
        public string status { get; set; }
        public string RRR { get; set; }
        public string merchantId { get; set; }
        public string statusMessage { get; set; }
        public string transactiontime { get; set; }
        public string orderId { get; set; }
    }
    public class InfoReponse<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
    }
    public class AppSettings
    {
        public string ncelasApiBaseUrl { get; set; }
    }   
}
