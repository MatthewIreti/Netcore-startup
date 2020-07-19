using System;
using System.ComponentModel.DataAnnotations;

namespace NCELAP.WebAPI.Models.Entities.Support
{
    public class SupportTickets
    {
        [Key]
        public int Id { get; set; }
        public string Department { get; set; }
        public int Priority { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public string Message { get; set; }
        public string Status { get; set; }
        public string CaseOwner { get; set; }
        public string Attachment { get; set; }
        public string Response { get; set; }
    }
}
