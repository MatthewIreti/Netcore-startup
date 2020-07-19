using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NCELAP.WebAPI.Models.DTO
{
    public class PaymentModel
    {
        public string serviceTypeId { get; set; }
        public double amount { get; set; }
        public string orderId { get; set; }
        public string payerName { get; set; }
        public string payerEmail { get; set; }
        public string payerPhone { get; set; }
        public string description { get; set; }

    }
    public class ReferenceResponseModel
    {
        public string statusCode { get; set; }
        public string RRR { get; set; }
        public string status { get; set; }
        public string statusMessage { get; set; }
        public string uniqueReference { get; set; }
    }

    public class RemitaReferenceRetrievalModel
    {
        public string serviceTypeId { get; set; }
        public double amount { get; set; }
        public string orderId { get; set; }
        public string payerName { get; set; }
        public string payerEmail { get; set; }
        public string payerPhone { get; set; }
        public string description { get; set; }
    }


}
