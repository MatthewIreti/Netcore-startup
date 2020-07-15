using NCELAP.WebAPI.Models.Entities.Accounts;
using NCELAP.WebAPI.Models.Entities.Applications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCELAP.WebAPI.Models.DTO.Applications
{
    public class LicenseApplication: ApplicationInfo
    {
        public LicenseApplicationUpload FileUploads { get; set; }
        public StakeholderLocation[] StakeholderLocations { get; set; }
        public Staff[] Staffs { get; set; }
        public GasShipperTakeOffPoint[] TakeOffPoints { get; set; }
        public GasShipperCustomer[] GasShipperCustomers { get; set; }
    }

    public class LicenseApplicationUpload
    {
        public long LicenseApplicationRecId { get; set; }
        public string HoldRelatedLicenseFileName { get; set; }
        public string HoldRelatedLicenseFileExtension { get; set; }
        public string HoldRelatedLicenseBase64 { get; set; }
        public string HasRelatedLicenseFileName { get; set; }
        public string HasRelatedLicenseFileExtension { get; set; }
        public string HasRelatedLicenseBase64 { get; set; }
        public string HasLicenseRevokedFileName { get; set; }
        public string HasLicenseRevokedFileExtension { get; set; }
        public string HasLicenseRevokedLicenseBase64 { get; set; }
        public string HasLicenseRefusedFileName { get; set; }
        public string HasLicenseRefusedFileExtension { get; set; }
        public string HasLicenseRefusedLicenseBase64 { get; set; }
        public string ProposedArrangementAttachmentFileName { get; set; }
        public string ProposedArrangementAttachmentFileExtension { get; set; }
        public string ProposedArrangementAttachmentBase64 { get; set; }
        public string DeclarationSignatureFileName { get; set; }
        public string DeclarationSignatureFileExtension { get; set; }
        public string DeclarationSignatureBase64 { get; set; }
        public string OPLFileName { get; set; }
        public string OPLFileExtension { get; set; }
        public string OPLBase64 { get; set; }
        public string SafetyCaseFileName { get; set; }
        public string SafetyCaseFileExtension { get; set; }
        public string SafetyCaseBase64 { get; set; }
        public string SCADAFileName { get; set; }
        public string SCADAFileExtension { get; set; }
        public string SCADABase64 { get; set; }
        public string GTSFileName { get; set; }
        public string GTSFileExtension { get; set; }
        public string GTSBase64 { get; set; }
        public string TechnicalAttributeFileName { get; set; }
        public string TechnicalAttributeFileExtension { get; set; }
        public string TechnicalAttributeBase64 { get; set; }
        public string AuxiliarySystemFileName { get; set; }
        public string AuxiliarySystemFileExtension { get; set; }
        public string AuxiliarySystemBase64 { get; set; }
        public string TariffAndPricingFileName { get; set; }
        public string TariffAndPricingFileExtension { get; set; }
        public string TariffAndPricingBase64 { get; set; }
        public string RiskManagmentFileName { get; set; }
        public string RiskManagmentFileExtension { get; set; }
        public string RiskManagmentBase64 { get; set; }
        public string CommunityMOUFileName { get; set; }
        public string CommunityMOUFileExtension { get; set; }
        public string CommunityMOUBase64 { get; set; }
        public string NetworkAgentOPLFileName { get; set; }
        public string NetworkAgentOPLFileExtension { get; set; }
        public string NetworkAgentOPLBase64 { get; set; }
        public string GasShipperOPLFileName { get; set; }
        public string GasShipperOPLFileExtension { get; set; }
        public string GasShipperOPLBase64 { get; set; }


    }

    public class StakeholderLocation
    {
        public long CustApplication { get; set; }
        public string Customer { get; set; }
        public string Location { get; set; }
        public string UniqueId { get; set; }
    }
    public class GasShipperTakeOffPoint
    {
        public long CustApplication { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string GasShipperPointType { get; set; }
        public string UniqueId { get; set; }
    }

    public class GasShipperCustomer
    {
        public long CustApplication { get; set; }
        public string BusinessName { get; set; }
        public string Location { get; set; }
        public string GasShipperCustCategory { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string UniqueId { get; set; }
    }

    public class ApplicationInfoForSave
    {
        public long Customer { get; set; }
        public string CustomerTier { get; set; }
        public long SubmittedBy { get; set; }
        //public string CustApplicationNum { get; set; }
        public string CustLicenseType { get; set; }
        public string CustLicenseCategory { get; set; }
        public string EffectiveDate { get; set; } //date
        public bool HoldRelatedLicense { get; set; }
        public string RelatedLicenseDetail { get; set; }
        public bool HasRelatedLicense { get; set; }
        public string RelatedLicenseType { get; set; }
        public bool HasLicenseRevoked { get; set; } //bool
        public string RevokedLicenseType { get; set; }
        public bool HasGasApplicationRefused { get; set; }
        public string RefusedLicenseType { get; set; }
        public string AgentShipperName { get; set; }
        public string AgentLocationOfShipper { get; set; }
        //public string NominatedCapacityEntryPoint { get; set; }
        public string EntryExitPoint { get; set; }
        public string Location { get; set; }
        public double MaximumNominatedCapacity { get; set; }
        public string PipelineAndGasTransporterName { get; set; }
        public string GasPipelineNetwork { get; set; }
        public string InstalledCapacity { get; set; }
        public string DeclarationName { get; set; }
        public string DeclarationCapacity { get; set; }
        public string DeclarationDate { get; set; } //date 
        public string ProposedArrangementLicensingActivity { get; set; }
        public bool HasStandardModificationRequest { get; set; }
        public string ModificationRequestDetails { get; set; }
        public string ModificationRequestReason { get; set; }
        public string CustLicenseApplicationStatus { get; set; }
        public string UniqueId { get; set; }
        public DateTime SubmittedOn { get; set; }
    }
}
