using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCELAP.WebAPI.Models.Entities.Accounts
{
    public class CustProspect
    {
        public string BusinessName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string PostalCode { get; set; }
        public string WebAddress { get; set; }
        public string Address { get; set; }
        public string AuthorizedRepName { get; set; }
        public string AuthorizedRepEmail { get; set; }
        public string AuthorizedRepTelephone { get; set; }
        public string AuthorizedRepMobile { get; set; }
        public string AuthorizedRepPhysicalAddress { get; set; }
        public string UniqueId { get; set; }
        public DateTime RegisteredOn { get; set; }
    }

    public class SuperAdmin
    {
        public string MyProperty { get; set; }
    }
    public class CustProspectUploads
    {
        public long CustProspectRecId { get; set; }

        // Certificate Of Registration
        public string CertificateOfRegistrationFileName { get; set; }
        public string CertificateOfRegistrationFileExtension { get; set; }
        public string CertificateOfRegistrationBase64 { get; set; }

        // Certificate Of Incorporation
        public string CertificateOfIncorporationFileName { get; set; }
        public string CertificateOfIncorporationFileExtension { get; set; }
        public string CertificateOfIncorporationBase64 { get; set; }

        // Memorandum & Articles Of Association
        public string MemorandumArticlesOfAssociationFileName { get; set; }
        public string MemorandumArticlesOfAssociationFileExtension { get; set; }
        public string MemorandumArticlesOfAssociationBase64 { get; set; }

        // Deed Of Partnership
        public string DeedOfPartnershipFileName { get; set; }
        public string DeedOfPartnershipFileExtension { get; set; }
        public string DeedOfPartnershipBase64 { get; set; }

        // Deed Of Trust
        public string DeedOfTrustFileName { get; set; }
        public string DeedOfTrustFileExtension { get; set; }
        public string DeedOfTrustBase64 { get; set; }
    }
}
