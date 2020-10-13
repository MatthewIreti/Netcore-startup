using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NCELAP.Models;
using NCELAP.Service;

namespace NCELAP.Controllers
{
    public class CompanyOperatorController : Controller
    {
        private readonly INCELAPClientService _clientService;
        public CompanyOperatorController(INCELAPClientService clientService)
        {
            _clientService = clientService;
        }
        public IActionResult Index()
        {
            ViewBag.Url = Request.Host;
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
        public async Task<IActionResult> PaymentSuccessful(string RRR,string orderID)
        {
            try
            {
                var status = await _clientService.GetPaymentStatus(RRR);
                if (status.Status)
                {
                    var model = new NCELAPViewModel
                    {
                        orderId = orderID,
                        RRR = RRR,
                        statusMessage = status.Message,
                        status = status.Status
                    };
                    ViewBag.ErrorMessage =string.Empty;
                    return View(model);
                }
                else
                {
                    ViewBag.ErrorMessage = status.Message;
                    return View(new NCELAPViewModel());
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}