using System;
namespace NCELAP.WebAPI.Models.Entities.Support
{
    public class SupportTicketsInput
    {
        public long CompanyRecId { get; set; }
        public long EmployeeRecId { get; set; }
        public string CompanyName { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmail { get; set; }
        public string ContactEmail { get; set; }
        public string Department { get; set; }
        public int Priority { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public string CaseOwner { get; set; }
        public string Attachment { get; set; }
        public string Response { get; set; }
        public DateTime RaisedOn { get; set; }
    }
}
