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
    public class ApplicationsController : ControllerBase
    {
        IConfiguration _configuration;
        private readonly ApplicationsService _applicationsService;

        public ApplicationsController(IConfiguration configuration)
        {
            _configuration = configuration;
            _applicationsService = new ApplicationsService(_configuration);
        }

        [HttpPost]
        [Route("savelicenseapplication")]
        public async Task<IActionResult> SaveLicenseApplicationInformation(LicenseApplication licenseApplication)
        {
            var response = await _applicationsService.SaveApplication(licenseApplication);
            return Ok(response);
        }

        [HttpGet]
        [Route("licensefees")]
        public async Task<IActionResult> LicenseFees()
        {
            var response = await _applicationsService.GetLicenseFees();
            return Ok(response);
        }

        [HttpGet]
        [Route("customer/{custrecid}")]
        public async Task<IActionResult> CustomerApplications(long custrecid)
        {
            var response = await _applicationsService.GetCustomerApplications(custrecid);
            return Ok(response);
        }

        [HttpGet]
        [Route("licenseapplicationdetails/{licenseapplicationrecid}")]
        public async Task<IActionResult> LicenseApplicationDetails(long licenseapplicationrecid)
        {
            var response = await _applicationsService.GetLicenseApplicationDetails(licenseapplicationrecid);
            return Ok(response);
        }
    }
}