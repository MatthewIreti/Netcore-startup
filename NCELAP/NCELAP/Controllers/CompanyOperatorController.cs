using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace NCELAP.Controllers
{
    public class CompanyOperatorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AllApplication()
        {
            return View();
        }
        public IActionResult NewApplication()
        {
            return View();
        }

        public IActionResult ApplicationDetails()
        {
            return View();
        }

        public IActionResult Payments()
        {
            return View();
        }

        public IActionResult PaymentHistory()
        {
            return View();
        }

        public IActionResult ActiveLicences()
        {
            return View();
        }
        public IActionResult ExpiredLicences()
        {
            return View();
        }
        public IActionResult UserManagement()
        {
            return View();
        }

        public IActionResult Ticket()
        {
            return View();
        }
        public IActionResult ViewTicket()
        {
            return View();
        }

        public IActionResult Support()
        {
            return View();
        }
        public IActionResult SupportFaq()
        {
            return View();
        }
    }
}