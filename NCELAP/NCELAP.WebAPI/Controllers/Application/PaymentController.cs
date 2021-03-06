using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NCELAP.WebAPI.Models.DTO;
using NCELAP.WebAPI.Models.Entities.Applications;
using NCELAP.WebAPI.Services;
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


        [HttpGet]
        [Route("generateRRR/{applicationId}")]
        public async Task<IActionResult> GenerateRemitaRetrievalReference(long applicationId)
        {
            try
            {
                var response = await _service.GetRemitaRetrivalReference(applicationId);
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

        [HttpGet]
        [Route("status/{rrr}")]
        public async Task<IActionResult> GetPaymentStatus(string rrr)
        {
            try
            {
                var response = await _service.GetCustomerPaymentStatus(rrr);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}