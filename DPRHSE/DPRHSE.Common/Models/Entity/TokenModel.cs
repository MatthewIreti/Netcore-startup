using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DPRHSE.Common.Models.Entity
{
    public class TokenModel
    {
        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("resource")]
        public string Resource { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }

    
}
