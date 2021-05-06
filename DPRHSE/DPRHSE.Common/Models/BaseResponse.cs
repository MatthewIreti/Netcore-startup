using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DPRHSE.Common.Models
{
    public class BaseResponse<T>
    {
        [JsonProperty("@odata.context")]
        public string  Context{ get; set; }

        [JsonProperty("@odata.count")]
        public int TotalCount { get; set; }

        [JsonProperty("value")]
        public T Value { get; set; }
    }
}
