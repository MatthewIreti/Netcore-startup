using NCELAP.WebAPI.Models.Entities.Applications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCELAP.WebAPI.Models.ODataResponse.Application
{
    public class LicenseCertificateResponse
    {
        [JsonProperty("$id")]
        public int Id { get; set; }
        public string companyId { get; set; }
        public string companyName { get; set; }
        public string license { get; set; }
    }

    public class NetworkCodeResponse
    {
        [JsonProperty("@odata.context")]
        public string odatacontext { get; set; }
        [JsonProperty("value")]
        public List<NetworkCodeLicense> value { get; set; }
    }
}
