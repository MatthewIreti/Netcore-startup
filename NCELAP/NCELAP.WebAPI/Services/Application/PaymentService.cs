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
        Task<ReferenceResponseModel> GetRemitaRetrivalReference(RemitaReferenceRetrievalModel model);
        Task<LicenseApplicationPaymentModel> SavePaymentInformation(LicenseApplicationPaymentModel model);
        Task<List<LicenseApplicationPaymentModel>> GetAllCustomerPaymentInformation(long custRecId);
        RemitaReferenceRetrievalModel GetRemitaRRModel(RemitaReferenceRetrievalModel model);
    }
    public class PaymentService: IPaymentService
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

        public async Task<ReferenceResponseModel> GetRemitaRetrivalReference(RemitaReferenceRetrievalModel model)
        {
            try
            {
                return await _remitaService.GetRRR(model);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<LicenseApplicationPaymentModel> SavePaymentInformation(LicenseApplicationPaymentModel model)
        {
            try
            {
                var env = _helper.GetEnvironmentUrl();
                var token = _authService.GetAuthToken();
                var licensePaymentLink = _configuration.GetSection("Endpoints").GetSection("licensePayment").Value;

                model.InvoiceDate = DateTime.Now.Date.ToString();

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
                var licensePaymentLink = _configuration.GetSection("Endpoints").GetSection("licensePaymentList").Value;

                var response = await env.AppendPathSegment(string.Format(licensePaymentLink, custRecId))
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
                return  _remitaService.GetRemitaRRModel(model);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
