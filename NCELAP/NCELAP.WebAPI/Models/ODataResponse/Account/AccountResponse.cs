using NCELAP.WebAPI.Models.Entities.Accounts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCELAP.WebAPI.Models.ODataResponse.Account
{
    public class AccountResponse
    {
    }
    public class CustProspectSaveResponse : CustProspect
    {
        public long RecordId { get; set; }
    }

    public class PasswordResetResponse
    {
        public string Email { get; set; }
        public string ActivationCode { get; set; }
        public bool Status { get; set; }
    }
}
