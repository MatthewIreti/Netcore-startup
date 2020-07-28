using System;
using System.ComponentModel.DataAnnotations;

namespace NCELAP.WebAPI.Models.Entities.Support
{
    public class SupportTicketsComment
    {
        [Key]
        public int Id { get; set; }
        public int CaseId { get; set; }
        public int CompanyRecId { get; set; }
        public string EmployeeName { get; set; }
        public string EmployeeEmail { get; set; }
        public string Subject { get; set; }
        public string Comment { get; set; }
        public DateTime UpdateTime { get; set; }
    }
}
