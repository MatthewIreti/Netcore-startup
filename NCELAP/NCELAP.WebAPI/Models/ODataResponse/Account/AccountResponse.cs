using NCELAP.WebAPI.Models.Entities.Accounts;
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
}
