using NCELAP.WebAPI.Models.Entities.Applications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCELAP.WebAPI.Models.ODataResponse.Application
{
    public class ApplicationInfoRecordId : ApplicationInfo
    {
        public long RecordId { get; set; }
    }
    public class ApplicationResponse
    {

        [JsonProperty("@odata.context")]
        public string odatacontext { get; set; }
        [JsonProperty("value")]
        public List<ApplicationInfoRecordId> value { get; set; }
    }

    public class ApplicationInfoResponse
    {

        [JsonProperty("@odata.context")]
        public string odatacontext { get; set; }
        [JsonProperty("value")]
        public List<ApplicationInfo> value { get; set; }
    }
     
    public class ShortApplicationInfo
    {

        public long RecordId { get; set; }
        public string ApplicationNum { get; set; }
        public DateTime DeclarationDate { get; set; }
        public string CustLicenseType { get; set; }
        public DateTime SubmittedOn { get; set; }
        public string CustLicenseApplicationStatus { get; set;}
        public string CustLicenseCategory { get; set; }
    }

    public class CustApplicationExtension: ShortApplicationInfo
    {
        public string CustLicenseName { get; set; }
        public string CustLicenseApplicationStatusName   { get; set; }
        public string CustLicenseCategoryName { get; internal set; }
    }

    public class ShortApplicationInfoResponse
    {

        [JsonProperty("@odata.context")]
        public string odatacontext { get; set; }
        [JsonProperty("value")]
        public List<CustApplicationExtension> value { get; set; }
    }

    public class BaseApplicationResponse<T>
    {
        [JsonProperty("@odata.context")]
        public string odatacontext { get; set; }
        [JsonProperty("value")]
        public List<T> value { get; set; }
    }
}
