using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using NCELAP.WebAPI.Models.DTO;
using NCELAP.WebAPI.Models.Entities.Accounts;
using NCELAP.WebAPI.Models.Entities.Support;
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
        public async Task<IActionResult> SaveRegisteredBusinessInformation(RegisteredBusiness registeredBusiness)
        {
            var response = await _userAccountService.SaveBusinessInformation(registeredBusiness);
            return Ok(response);
        }

        [HttpPost]
        [Route("contactsupport")]
        public async Task<IActionResult> ContactSupport(ContactSupport contactSupport)
        {
            var response = await _userAccountService.ContactSupport(contactSupport);
            return Ok(response);
        }

        [HttpPost]
        [Route("userlogin")]
        public async Task<IActionResult> SaveRegisteredBusinessInformation(NcelasUserLogin ncelasUserLogin)
        {
            var response = await _userAccountService.GetNcelasUserAsync(ncelasUserLogin);
            return Ok(response);
        }

        [HttpPost]
        [Route("activate")]
        public async Task<IActionResult> ActivateAccount(NcelasUserLogin userInfoPayload)
        {
            var response = await _userAccountService.ChangeNcelasUserPassword(userInfoPayload);
            return Ok(response);
        }

        [HttpGet]
        [Route("useraccount/{recid}")]
        public async Task<IActionResult> GetUserInfo(long recid)
        {
            var response = await _userAccountService.GetNcelasUserByRecId(recid);
            return Ok(response);
        }

        [HttpGet]
        [Route("usersbycreatorrecid/{recid}")]
        public async Task<IActionResult> GetNcelasUsersByCreatorRecId(long recid)
        {
            var response = await _userAccountService.GetNcelasUsersByCreatorRecId(recid);
            return Ok(response);
        }

        [HttpPost]
        [Route("createuser")]
        public async Task<IActionResult> CreateUser(UserToCreate userToCreate)
        {
            var response = await _userAccountService.CreateUser(userToCreate);
            return Ok(response);
        }
        //
        [HttpPost]
        [Route("emailcheck")]
        public async Task<IActionResult> GetUserInfo(UserEmail userEmail )
        {
            var response = await _userAccountService.GetNcelasUserEmailExist(userEmail.Value);
            return Ok(response);
        }
        
        [HttpPost]
        [Route("sendpassresetcode")]
        public async Task<IActionResult> SendpPsswordResetCode(UserEmail userEmail )
        {
            var response = await _userAccountService.SendPasswordResetCode(userEmail.Value);
            return Ok(response);
        }

        [HttpPost]
        [Route("resetncelasuserpassword")]
        public async Task<IActionResult> ResetNcelasPassword(EmailPasswordDto emailPasswordDto)
        {
            var response = await _userAccountService.PasswordReset(emailPasswordDto);
            return Ok(response);
        }
    }
}