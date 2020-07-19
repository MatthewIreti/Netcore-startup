using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NCELAP.WebAPI.Models.DTO;
using NCELAP.WebAPI.Models.Entities.Applications;
using NCELAP.WebAPI.Services.Application;

namespace NCELAP.WebAPI.Controllers.Application
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        IConfiguration _configuration;
        private readonly IPaymentService _service;

        public PaymentController(IPaymentService paymentService)
        {
            _service = paymentService;
        }


        [HttpPost]
        [Route("generateRRR")]
        public async Task<IActionResult> GenerateRemitaRetrievalReference(RemitaReferenceRetrievalModel model)
        {
            try
            {
                var response = await _service.GetRemitaRetrivalReference(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("submitPaymentInformation")]
        public async Task<IActionResult> SaveLicensePaymentInformation(LicenseApplicationPayment model)
        {
            try
            {
                var response = await _service.SavePaymentInformation(model);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("customerPayments/{custRecId}")]
        public async Task<IActionResult> GetAllCustomerPayments(long custRecId)
        {
            try
            {
                var response = await _service.GetAllCustomerPaymentInformation(custRecId);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}