using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCELAP.WebAPI.Models.Entities.Applications
{
    public class LicenseFee
    {
        public string LicenseType { get; set; }
        public string CategoryDescription { get; set; }
        public double Maximum { get; set; }
        public double Minimum { get; set; }
        public double ProcessingFee { get; set; }
        public double Statutory { get; set; }
    }
}
