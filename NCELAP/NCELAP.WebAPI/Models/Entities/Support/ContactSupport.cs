using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCELAP.WebAPI.Models.Entities.Support
{
    public class ContactSupport
    {
        public string BusinessName { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }
        public string UniqueId { get; set; }
    }
}
