using NCELAP.WebAPI.Models.DTO.Applications;
using System;
using System.Collections.Generic;

namespace NCELAP.WebAPI.Models.Entities.Applications
{
    public class ApplicationInfo
    {
        public string ApplicationNum { get; set; }
        public long Customer { get; set; }
        public string CustomerTier { get; set; }
        public long SubmittedBy { get; set; }
        public string CompanyName { get; set; }
        //public string CustApplicationNum { get; set; }
        public string CustLicenseType { get; set; }
        public string CustLicenseCategory { get; set; }
        public string EffectiveDate { get; set; } //date
        public string HoldRelatedLicense { get; set; }
        public string RelatedLicenseDetail { get; set; }
        public string HasRelatedLicense { get; set; }
        public string RelatedLicenseType { get; set; }
        public string HasLicenseRevoked { get; set; } //bool
        public string RevokedLicenseType { get; set; }
        public string HasGasApplicationRefused { get; set; }
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
        public string HasStandardModificationRequest { get; set; } 
        public string ModificationRequestDetails { get; set; } 
        public string ModificationRequestReason { get; set; } 
        public string CustLicenseApplicationStatus { get; set; }
        public string ExitPoint { get; set; }
        public string EntryPoint { get; set; }
        public string UniqueId { get; set; }
        public DateTime SubmittedOn { get; set; }

       
    }
    public class APPlicationInfoDetails : ApplicationInfo
    {
        public List<GasShipperCustomer> GasShipperCustomers { get; set; }
        public List<GasShipperTakeOffPoint> GasShipperTakeOffPoints { get; set; }
        public List<StakeholderLocation> StakeholderLocations { get; set; }
        public List<LicenseApplicationPaymentModel> LicenseApplicationPayments { get; set; }
        public List<LicenseApplicationCommentModel> LicenseApplicationComments { get; set; }
    }

    
}
