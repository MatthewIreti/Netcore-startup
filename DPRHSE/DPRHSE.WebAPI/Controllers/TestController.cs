using DPRHSE.Business.Services;
using DPRHSE.WebAPI.Common;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DPRHSE.WebAPI.Controllers
{
    [Route("api/[controller]")]

    public class TestController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IAccountService _accountSvc;
        private readonly IAuthService _authService;
        public TestController(AppSettings appSettings, 
            IAccountService accountService,
            IAuthService authService)
        {
            _appSettings = appSettings;
            _accountSvc = accountService;
            _authService = authService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var resp = _accountSvc.TestMethod();
            return Ok(resp);
        }

       
    }
}
