using NCELAP.WebAPI.Models.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace NCELAP.WebAPI.Models.Entities.Accounts
{
    public class RegisteredBusiness : RegisteredBusinessForSave
    {
        //public CustProspectLegalStatus[] LegalStatus { get; set; }
        public CustProspectShareholder[] Shareholders { get; set; }
        public CustProspectDirector[] Directors { get; set; }
        public CustProspectUploads SupportingDocuments { get; set; }
    }

    public class RegisteredBusinessForSave
    {
        public string CustProspectId { get; set; }
        [Required]
        public string BusinessName { get; set; }
        [Required]
        public string Email { get; set; }
        public string UniqueId { get; set; }
        [Required]
        public string CompanyLegalStatus { get; set; }
        public string OtherLegalStatus { get; set; }
        //public string Password { get; set; }
        [Required]
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string PostalCode { get; set; }
        public string WebAddress { get; set; }
        [Required]
        public string Address { get; set; }
        public DateTime RegisteredOn { get; set; }
        [Required]
        public string AuthorizedRepName { get; set; }
        [Required]
        public string AuthorizedRepEmail { get; set; }
        public string AuthorizedRepTelephone { get; set; }
        public string AuthorizedRepMobile { get; set; }
        public string AuthorizedRepPhysicalAddress { get; set; }
        public string DirectorCriminalAct { get; set; }
        public string DetailsOfConviction { get; set; }
    }
}
