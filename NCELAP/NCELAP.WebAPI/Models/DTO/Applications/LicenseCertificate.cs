using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCELAP.WebAPI.Models.DTO.Applications
{
    public class LicenseCertificate
    {
        public LicenseCertificatePayload payload { get; set; }
    }

    public class LicenseCertificatePayload
    {
        public long CustLicenseApplicationId { get; set; }
        public long LicenseId { get; set; }
    }
}
