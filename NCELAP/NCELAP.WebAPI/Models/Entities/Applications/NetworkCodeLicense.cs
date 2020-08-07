using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCELAP.WebAPI.Models.Entities.Applications
{
    public class NetworkCodeLicense
    {
        public string LicenseNumber { get; set; }
        public DateTime DueDate { get; set; }
        public long LicenseApplication { get; set; }
        public long RecordId { get; set; }
        public long Customer { get; set; }
        public string LicenseType { get; set; }
        public DateTime LicenseDate { get; set; }
    }
}
