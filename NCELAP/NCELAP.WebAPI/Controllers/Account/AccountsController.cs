using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NCELAP.WebAPI.Models.Entities.Accounts;
using NCELAP.WebAPI.Services.Account;

namespace NCELAP.WebAPI.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        IConfiguration _configuration;
        private readonly UserAccountsService _userAccountService;

        public AccountsController(IConfiguration configuration)
        {
            _configuration = configuration;
            _userAccountService = new UserAccountsService(_configuration);
        }

        [HttpPost]
        [Route("registeredbusiness")]
        //[Authorize]
        public async Task<IActionResult> SaveRegisteredBusinessInformation(RegisteredBusiness registeredBusiness)
        {
            var response = await _userAccountService.SaveBusinessInformation(registeredBusiness);
            return Ok(response);
        }
    }
}