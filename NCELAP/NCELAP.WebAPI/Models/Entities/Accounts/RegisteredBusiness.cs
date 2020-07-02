using NCELAP.WebAPI.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCELAP.WebAPI.Models.Entities.Accounts
{
    public class RegisteredBusiness : RegisteredBusinessForSave
    {
        public CustProspectLegalStatus[] LegalStatus { get; set; }
    }

    public class RegisteredBusinessForSave
    {
        public string CustProspectId { get; set; }
        public string BusinessName { get; set; }
        public string Email { get; set; }
        //public string Password { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }
        public string PostalCode { get; set; }
        public string WebAddress { get; set; }
        public string Address { get; set; }
        public DateTime RegisteredOn { get; set; }
        public string AuthorizedRepName { get; set; }
        public string AuthorizedRepEmail { get; set; }
        public string AuthorizedRepTelephone { get; set; }
        public string AuthorizedRepMobile { get; set; }
        public string AuthorizedRepPhysicalAddress { get; set; }
    }
}
