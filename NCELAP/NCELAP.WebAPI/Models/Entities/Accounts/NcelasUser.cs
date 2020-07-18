using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCELAP.WebAPI.Models.Entities.Accounts
{
    public class NcelasUser
    {
        public string Email { get; set; }
        public long CreatedByRecId { get; set; }
        public string Status { get; set; }
        public DateTime ActivatedOn { get; set; }
        public string Activated { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public string Password { get; set; }
        //public string LastName { get; set; }
        public long CustTableRecId { get; set; }
    }

    public class NcelasUserLogin
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class UserToCreate
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Department { get; set; }
        public long CreatedByRecId { get; set; }
        public long CustTableRecId { get; set; }
        public string Activated { get; set; }
    }
}
