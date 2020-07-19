using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NCELAP.WebAPI.Models.Entities.Support;

namespace NCELAP.WebAPI.Services.Support
{
    public interface ISupportTicket
    {
        Task<GenericResponse<SupportTickets>> CreateSupportTicketAsync(SupportTickets supportTickets);
        Task<GenericResponse<SupportTickets>> DeleteSupportTicketAsync(int Id);
        Task<GenericResponse<SupportTickets>> EditSupportAsync(SupportTickets supportTickets);
        Task<GenericResponse<SupportTickets>> GetSupportTicketByIdAsync(int Id);
        Task<GenericResponse<IEnumerable<SupportTickets>>> GetAllSupportTicketsAsync();
    }
}
