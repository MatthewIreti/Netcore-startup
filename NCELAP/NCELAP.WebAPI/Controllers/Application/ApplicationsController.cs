using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NCELAP.WebAPI.Models.DTO.Applications;
using NCELAP.WebAPI.Models.Entities.Applications;
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
            try
            {
                var response = await _applicationsService.SaveApplication(licenseApplication);
                return Ok(response);
            }
            catch (Exception exception)
            {

                return BadRequest(exception.Message);
            }


        }
        [HttpPut]
        [Route("updatelicenseapplication")]
        public async Task<IActionResult> UpdateLicenseApplicationInformation(ApplicationInfo model)
        {
            try
            {
                var response = await _applicationsService.UpdateApplication(model);
                return Ok(response);
            }
            catch (Exception exception)
            {

                return BadRequest(exception.Message);
            }


        }


        [HttpGet]
        [Route("licensefees")]
        public async Task<IActionResult> LicenseFees()
        {
            var response = await _applicationsService.GetLicenseFees();
            return Ok(response);
        }

        [HttpGet]
        [Route("states")]
        public async Task<IActionResult> GetStates()
        {
            try
            {
                var response = await _applicationsService.GetZoneStates();
                return Ok(response);
            }
            catch (Exception exception)
            {

                return BadRequest(exception.Message);
            }
        }

        [HttpGet]
        [Route("customer/{custrecid}")]
        public async Task<IActionResult> CustomerApplications(long custrecid)
        {
            try
            {
                var response = await _applicationsService.GetCustomerApplications(custrecid);
                return Ok(response);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("licenses/{custrecid}")]
        public async Task<IActionResult> CustomerApplicationLicenses(long custrecid)
        {
            try
            {
                var response = await _applicationsService.GetCustomerApplicationLicenses(custrecid);
                return Ok(response);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
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