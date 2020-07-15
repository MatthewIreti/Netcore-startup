using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCELAP.WebAPI.Models.DTO
{
    public class Common
    {
    }

    public class Organization
    {
        public string OrganizationName { get; set; }
    }

    public static class ConstantHelper
    {
        public const string NetworkAgent = "Network Agent";
        public const string GasTransporterLicense = "Gas Transporter License";
        public const string GasShipperLicense = "Gas Shipper License";
    }
}
