using DPRHSE.Business.Services;
using DPRHSE.Common.Models;
using DPRHSE.Common.Models.ViewModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DPRHSE.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ICompanyService _service;

        public CompanyController(ICompanyService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("get-all-companies")]
        public async Task<ActionResult<Response<IReadOnlyList<CompanyViewModel>>>> GetAllCompanies(int page = 1, int size = 30)
        {
            try
            {
                var response = await _service.GetAllCompanies(page, size);
                if (response.Status)
                    return Ok(response);
                return BadRequest(Response<IReadOnlyList<CompanyViewModel>>.Failed(response.Message));
            }
            catch (Exception exception)
            {
                return BadRequest(Response<IReadOnlyList<CompanyViewModel>>.Failed(exception.Message));
            }
        }
    }
}
