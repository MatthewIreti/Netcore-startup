using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NCELAP.WebAPI.Models.DTO.Applications;
using NCELAP.WebAPI.Services.Application;

namespace NCELAP.WebAPI.Controllers.Application
{
    [Route("api/[controller]")]
    [ApiController]
    public class LicenseCertificateController : ControllerBase
    {
        IConfiguration _configuration;
        private readonly LicenseService licenseService;

        public LicenseCertificateController(IConfiguration configuration)
        {
            _configuration = configuration;
            licenseService = new LicenseService(_configuration);
        }

        [HttpPost]
        [Route("generate")]
        public async Task<IActionResult> GenerateLicenseCertificate(LicenseCertificate licenseCertificate)
        {
            var response = await licenseService.GetLicenseBase64(licenseCertificate);
            return Ok(response);
        }
    }
}