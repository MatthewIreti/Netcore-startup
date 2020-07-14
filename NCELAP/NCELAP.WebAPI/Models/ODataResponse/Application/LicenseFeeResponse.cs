using NCELAP.WebAPI.Models.Entities.Applications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCELAP.WebAPI.Models.ODataResponse.Application
{
    public class LicenseFeeResponse
    {
        [JsonProperty("@odata.context")]
        public string odatacontext { get; set; }
        [JsonProperty("value")]
        public List<LicenseFee> value { get; set; }
    }
}
