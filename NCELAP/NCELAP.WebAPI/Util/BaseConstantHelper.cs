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

        //Application uploads name extensions constants
        public const string DeclarationSignatureFileName = "_declaration_signature";
        public const string HasLicenseRefusedFileName = "_licenserefused";
        public const string HasLicenseRevokedFileName = "_licenserevoked";
        public const string HasRelatedLicenseFileName = "_relatedlicense";
        public const string HoldRelatedLicenseFileName = "_holdrelatedlicense";
        public const string ProposedArrangementAttachmentFileName = "_proposedarrangementlicense";
        public const string OPLFileName = "_OPL_License";
        public const string SafetyCaseFileName = "_SafetyCaseApproved";
        public const string SCADAFileName = "_SCADA_System";
        public const string GTSFileName = "_Gas_transmission_system";
        public const string TechnicalAttributeFileName = "_technical_attributes";
        public const string AuxiliarySystemFileName = "_Auxiliary_systems";
        public const string TariffAndPricingFileName = "_Tarrif_and_pricing";
        public const string RiskManagementFileName = "_Risk_management";
        public const string CommunityMOUFileName = "_CommunityMOU";
         


    }
}
