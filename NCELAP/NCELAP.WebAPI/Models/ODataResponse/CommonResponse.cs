using NCELAP.WebAPI.Models.DTO;
using NCELAP.WebAPI.Models.Entities.Accounts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCELAP.WebAPI.Models.ODataResponse
{
    public class CommonResponse
    {
        [JsonProperty("@odata.context")]
        public string odatacontext { get; set; }
        [JsonProperty("value")]
        public List<Organization> value { get; set; }
    }

    //public class NcelasUserResponse
    //{
    //    [JsonProperty("@odata.context")]
    //    public string odatacontext { get; set; }
    //    [JsonProperty("value")]
    //    public List<NcelasUser> value { get; set; }
    //}
}
