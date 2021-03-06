using NCELAP.WebAPI.Models.DTO.Applications;
using System;
using System.Collections.Generic;

namespace NCELAP.WebAPI.Models.Entities.Applications
{
    public class ApplicationInfo
    {
        public long LicenseFeeCategory { get; set; }
        public long RecordId { get; set; }
        public string ApplicationNum { get; set; }
        public long Customer { get; set; }
        public string CustomerTier { get; set; }
        public long SubmittedBy { get; set; }
        public string CompanyName { get; set; }
        //public string CustApplicationNum { get; set; }
        public string CustLicenseType { get; set; }
        public string LicenseType    { get; set; }
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
        public double MaximumNominatedCapacity { get; set; }
        public string PipelineAndGasTransporterName { get; set; }
        public string GasPipelineNetwork { get; set; }
        public string InstalledCapacity { get; set; }
        public string DeclarationName { get; set; }
        public string DeclarationCapacity { get; set; }
        public string DeclarationDate { get; set; } //date 
        public string ProposedArrangementLicensingActivity { get; set; }
        public string ProposedArrangementDetail { get; set; }
        public string Location { get; set; }
        public bool HasStandardModificationRequest { get; set; }
        public string ModificationRequestDetails { get; set; }
        public string ModificationRequestReason { get; set; }
        public string CustLicenseApplicationStatus { get; set; }
        public string ExitPoint { get; set; }
        public string EntryPoint { get; set; }
        public long EntryPointState { get; set; }
        public long ExitPointState { get; set; }
        public string UniqueId { get; set; }
        public DateTime SubmittedOn { get; set; }


    }
    public class APPlicationInfoDetails : ApplicationInfo
    {
        public List<GasShipperCustomerDetails> GasShipperCustomers { get; set; }
        public List<GasShipperTakeOffPointDetails> GasShipperTakeOffPoints { get; set; }
        public List<StakeholderLocationDetails> StakeholderLocations { get; set; }
        public List<LicenseApplicationPaymentModel> LicenseApplicationPayments { get; set; }
        public List<LicenseApplicationCommentModel> LicenseApplicationComments { get; set; }
        public List<LicenseAttachmentModel> LicenseApplicationAttachments { get; set; }
        public LicenseApplicationUpload FileUploads { get; set; }
    }

    public class LicenseAttachmentModel
    {
        public long ApplicationId { get; set; }
        public string FileType { get; set; }
        public string ApplicationNum { get; set; }
        public string FileName { get; set; }
    }

    public class DPRZoneStates
    {
        public string State { get; set; }
        public long RecordId { get; set; }

    }

}
