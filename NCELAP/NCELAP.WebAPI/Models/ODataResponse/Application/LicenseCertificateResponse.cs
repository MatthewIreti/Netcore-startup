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
}
