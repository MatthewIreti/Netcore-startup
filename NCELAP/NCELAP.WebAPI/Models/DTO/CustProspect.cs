using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCELAP.WebAPI.Models.DTO
{
    public class CustProspect
    {
    }

    public class CustProspectLegalStatus
    {
        public string Name { get; set; }
        public bool Checked { get; set; }
    }

    public class CustProspectShareholder
    {
        public long CustProspectRecId { get; set; }
        public string CustProspectId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Nationality { get; set; }
        public string CountryOfUsualResidence { get; set; }
        public string UniqueId { get; set; }
    }

    public class CustProspectDirector
    {
        public long CustProspectRecId { get; set; }
        public string CustProspectId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Nationality { get; set; }
        public string CountryOfUsualResidence { get; set; }
        public string UniqueId { get; set; }
        
    }


    public class CustProspectLegalStatusForSave
    {
        public string CustProspectId { get; set; }
        public string LegalStatus { get; set; }
    }
}
