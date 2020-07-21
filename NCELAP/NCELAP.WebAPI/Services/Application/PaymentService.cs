using Flurl;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using NCELAP.WebAPI.Models.DTO;
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
using System.Threading.Tasks;

namespace NCELAP.WebAPI.Services.Application
{
    public interface IPaymentService
    {
        Task<LicenseApplicationPaymentModel> SavePaymentInformation(LicenseApplicationEntity model);
        Task<List<LicenseApplicationPaymentModel>> GetAllCustomerPaymentInformation(long custRecId);
        Task<ReferenceResponseModel> GetRemitaRetrivalReference(long applicationId);
    }
    public class PaymentService : IPaymentService
    {
        private readonly IConfiguration _configuration;
        private readonly AuthService _authService;
        private readonly Helper _helper;
        //IConfigurationSection section;
        private readonly IRemitaService _remitaService;
        public PaymentService(IRemitaService remitaService, IConfiguration configuration)
        {
            _configuration = configuration;
            _authService = new AuthService(configuration);
            _helper = new Helper(configuration);
            //section = configuration.GetSection("Remita");
            _remitaService = remitaService;
        }
        public async Task<RemitaReferenceRetrievalModel> FetchRRRPayload(long applicationId)
        {
            try
            {
                var data = new
                {
                    model = new
                    {
                        custLicenseApplicationId = applicationId
                    }
                };
                var url = _helper.GetEnvironmentUrl().Replace("/data", "");
                var response = await url
                    .AppendPathSegment("/api/services/NCLEAS/Remita/generateRRR")
                    .WithOAuthBearerToken(_authService.GetAuthToken())
                    .PostJsonAsync(data)
                    .ReceiveJson<RemitaReferenceRetrievalModel>();
                return response;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<ReferenceResponseModel> GetRemitaRetrivalReference(long applicationId)
        {
            try
            {
                ReferenceResponseModel response;
                LicenseApplicationPaymentModel paymentInfo = null;
                var payload = await FetchRRRPayload(applicationId);
                if (payload == null)
                    throw new Exception("Invalid RRR Payload");

                //check if has been generated for the current orderId
                var licensePayment = await _helper.GetEnvironmentUrl()
                    .AppendPathSegment("LicenseApplicationPayment")
                    .WithOAuthBearerToken(_authService.GetAuthToken())
                    .SetQueryParam("$filter", $"OrderId eq '{payload.orderId}'")
                    .GetJsonAsync<BaseApplicationResponse<LicenseApplicationPaymentModel>>();
                if (licensePayment.value.Count > 0)
                {
                     paymentInfo = licensePayment.value[0]; 
                    response = new ReferenceResponseModel
                    {
                        RRR = paymentInfo.RemitaRetrievalRef,
                        status = paymentInfo.StatusMessage
                    };
                     
                }
                else
                {
                     response = await _remitaService.GetRRR(payload);
                    if (response != null)
                    {
                        //submit the response to server.
                        paymentInfo =  await SavePaymentInformation(new LicenseApplicationEntity
                        {
                            Amount = double.Parse(payload.amount),
                            LicenseApplication = applicationId,
                            RemitaRetrievalRef = response.RRR,
                            OrderId = payload.orderId
                        });
                    }
                }
                response.metadata = payload;
                response.metadata.apiKey = "";
                response.metadata.url = "";
                response.metadata.merchantId = "";
                response.paymentInfo = paymentInfo;

                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<LicenseApplicationPaymentModel> SavePaymentInformation(LicenseApplicationEntity model)
        {
            try
            {
                var env = _helper.GetEnvironmentUrl();
                var token = _authService.GetAuthToken();
                var licensePaymentLink = _configuration.GetSection("Endpoints").GetSection("licensePayment").Value;
                var response = await env.AppendPathSegment(licensePaymentLink)
                    .WithOAuthBearerToken(token)
                    .PostJsonAsync(model)
                    .ReceiveJson<LicenseApplicationPaymentModel>();
                if (response == null) throw new Exception("No response");
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<LicenseApplicationPaymentModel>> GetAllCustomerPaymentInformation(long custRecId)
        {
            try
            {
                var env = _helper.GetEnvironmentUrl();
                var token = _authService.GetAuthToken();
                var response = await env.AppendPathSegment("LicenseApplicationPayment")
                    .SetQueryParam("$filter","Customer eq "+ custRecId)
                    .WithOAuthBearerToken(token)
                    .GetJsonAsync<BaseApplicationResponse<LicenseApplicationPaymentModel>>();
                if (response == null) throw new Exception("No response");
                return response.value;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public RemitaReferenceRetrievalModel GetRemitaRRModel(RemitaReferenceRetrievalModel model)
        {
            try
            {
                return _remitaService.GetRemitaRRModel(model);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

    }
}
