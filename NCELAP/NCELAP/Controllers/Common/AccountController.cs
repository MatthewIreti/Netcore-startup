using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NCELAP.Controllers.Account
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult RegistrationComplete()
        {
            return View();
        }

        public IActionResult Activation()
        {
            return View();
        }

        public IActionResult PasswordReset()
        {
            return View();
        }
    }
}