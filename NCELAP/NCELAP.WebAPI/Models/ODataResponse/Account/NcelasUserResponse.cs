using NCELAP.WebAPI.Models.Entities.Accounts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCELAP.WebAPI.Models.ODataResponse.Account
{
    public class NcelasUserResponse : NcelasUser
    {
        public long RecordId { get; set; }
        public string CompanyName { get; set; }
    }

    public class NcelasUsersResponse
    {
        [JsonProperty("@odata.context")]
        public string odatacontext { get; set; }
        [JsonProperty("value")]
        public List<NcelasUserResponse> value { get; set; }
    }

    
}
