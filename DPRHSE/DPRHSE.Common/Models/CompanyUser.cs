using System;

namespace DPRHSE.Common.Models
{
    public class CompanyUser
    {
        public string CompanyName { get; set; }
        public string  Address { get; set; }
    }
    public class CompanyUserResponse : CompanyUser
    {
        public DateTime CreatedDate { get; set; }
        public string UniqueId { get; set; }
    }


}
