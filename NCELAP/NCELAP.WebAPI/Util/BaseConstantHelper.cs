using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCELAP.WebAPI.Util
{
    public class BaseConstantHelper
    {
        //licenseType
        public const string networkAgentLicenseType = "NetworkAgent";
        public const string gasTransporterLicenseType = "GasTransporterLicense";
        public const string gasShipperLicenseType = "GasShipperLicense";

        //CustLicenseApplication
        public const string Submitted = "Submitted";
        public const string ChangeRequested = "ChangeRequested";
        public const string Approved = "Approved";
        public const string Rejected = "Rejected";
        public const string Cancelled = "Cancelled";
        public const string Returned = "Returned";
        public const string AwaitingProcessingFee = "AwaitingProcessingFee";
        public const string AwaitingLicenseFee = "AwaitingLicenseFee";
        public const string Active = "Active";
        public const string Expired = "Expired";
        public const string DueForRenewal = "DueForRenewal";

        //CustLicenseCategory
        public const string NewApplication = "NewApplication";
        public const string Renewal = "Renewal";

    }
}
