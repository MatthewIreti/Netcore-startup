﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCELAP.WebAPI.Models.Entities.Applications
{
    public class LicenseApplicationPaymentModel
    {
        public string RemitaRetrievalRef { get; set; }
        public long LicenseApplication { get; set; }
        public double Amount { get; set; }
        public long Customer { get; set; }
        public bool Status { get; set; }
        public string InvoiceDate { get; set; }
        public string PaymentDate { get; set; }
        public string StatusMessage { get; set; }
        public string OrderId { get; set; }
    }

    public class LicenseApplicationEntity
    {
        public string RemitaRetrievalRef { get; set; }
        public long LicenseApplication { get; set; }
        public double Amount { get; set; }
        public string OrderId { get; set; }
        public string Description { get; set; }
    }
}
