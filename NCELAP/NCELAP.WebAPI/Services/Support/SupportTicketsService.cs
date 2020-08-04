using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using NCELAP.WebAPI.Data;
using NCELAP.WebAPI.Models.Entities.Support;

namespace NCELAP.WebAPI.Services.Support
{
    public class SupportTicketsService : ISupportTicket
    {
        private readonly ApplicationDbContext _dbcontext;
        public SupportTicketsService(ApplicationDbContext dbcontext)
        {
            _dbcontext = dbcontext;
        }

        public async Task<GenericResponse<SupportTicketsComment>> AddSupportTicketComment(SupportTicketsComment supportTicketsComment)
        {
            try
            {
                if (supportTicketsComment == null)
                {
                    return new GenericResponse<SupportTicketsComment>
                    {
                        Data = null,
                        Message = "support ticket is null",
                        Success = false
                    };
                }
                else
                {
                    var dateRegistered = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    supportTicketsComment.UpdateTime = dateRegistered;

                    await _dbcontext.SupportComments.AddAsync(supportTicketsComment);
                    _dbcontext.SaveChanges();

                    return new GenericResponse<SupportTicketsComment>
                    {
                        Data = supportTicketsComment,
                        Message = "support ticket created successfully",
                        Success = true
                    };
                }
            }
            catch (Exception e)
            {
                return new GenericResponse<SupportTicketsComment>
                {
                    Data = null,
                    Message = e.Message,
                    Success = false
                };
            }
        }

        public async Task<GenericResponse<SupportTickets>> CreateSupportTicketAsync(SupportTickets supportTickets)
        {
            try
            {
                if (supportTickets == null)
                {
                    return new GenericResponse<SupportTickets>
                    {
                        Data = null,
                        Message = "support ticket is null",
                        Success = false
                    };
                }
                else
                {
                    var dateRegistered = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
                    supportTickets.RaisedOn = dateRegistered;

                    await _dbcontext.SupportTickets.AddAsync(supportTickets);
                    _dbcontext.SaveChanges();

                    return new GenericResponse<SupportTickets>
                    {
                        Data = supportTickets,
                        Message = "support ticket created successfully",
                        Success = true
                    };
                }
            }
            catch (Exception e)
            {
                return new GenericResponse<SupportTickets>
                {
                    Data = null,
                    Message = e.Message,
                    Success = false
                };
            }
        }


        public async Task<GenericResponse<SupportTickets>> DeleteSupportTicketAsync(int Id)
        {
            try
            {
                var supportTicket = await _dbcontext.SupportTickets.FirstOrDefaultAsync(s => s.Id == Id);
                if (supportTicket == null)
                {
                    return new GenericResponse<SupportTickets>
                    {
                        Data = null,
                        Message = "template not found",
                        Success = false
                    };
                }
                else
                {
                    _dbcontext.Remove(supportTicket);
                    _dbcontext.SaveChanges();

                    return new GenericResponse<SupportTickets>
                    {
                        Data = null,
                        Message = "template successfully deleted",
                        Success = true
                    };
                }
            }
            catch (Exception e)
            {
                return new GenericResponse<SupportTickets>
                {
                    Data = null,
                    Message = e.Message,
                    Success = false
                };
            }
        }

        public async Task<GenericResponse<SupportTickets>> EditSupportAsync(SupportTickets supportTickets)
        {
            try
            {
                var supportTicket = await _dbcontext.SupportTickets.FirstOrDefaultAsync(s => s.Id == supportTickets.Id);
                if (supportTicket != null)
                {
                    _dbcontext.SupportTickets.Update(supportTicket);
                    _dbcontext.SaveChanges();

                    return new GenericResponse<SupportTickets>
                    {
                        Data = supportTicket,
                        Message = "Template suceessfully updated",
                        Success = true

                    };
                }
                else
                {
                    return new GenericResponse<SupportTickets>
                    {
                        Data = null,
                        Message = "Template not found",
                        Success = false

                    };
                }
            }
            catch (Exception e)
            {
                return new GenericResponse<SupportTickets>
                {
                    Data = null,
                    Message = e.Message,
                    Success = false
                };
            }
        }

        public async Task<GenericResponse<IEnumerable<SupportTickets>>> GetAllSupportTicketsAsync()
        {
            try
            {
                var supportTickets = await _dbcontext.SupportTickets.ToListAsync();

                if (supportTickets != null)
                {
                    return new GenericResponse<IEnumerable<SupportTickets>>
                    {
                        Data = supportTickets,
                        Message = "All templates listed",
                        Success = true

                    };
                }
                else
                {
                    return new GenericResponse<IEnumerable<SupportTickets>>
                    {
                        Data = null,
                        Message = "templates not found",
                        Success = false
                    };
                }
            }
            catch (Exception e)
            {
                return new GenericResponse<IEnumerable<SupportTickets>>
                {
                    Data = null,
                    Message = e.Message,
                    Success = false
                };
            }
        }

        public async Task<GenericResponse<List<SupportTickets>>> GetAllSupportTicketsByEmployee(long employeeRecId, long companyRecId)
        {
            try
            {
                var supportTickets = await _dbcontext.SupportTickets.Where(s => s.CompanyRecId == companyRecId && s.EmployeeRecId == employeeRecId).
                    OrderByDescending(s => s.RaisedOn).ToListAsync();

                if (supportTickets != null)
                {
                    return new GenericResponse<List<SupportTickets>>
                    {
                        Data = supportTickets,
                        Message = "All templates listed",
                        Success = true

                    };
                }
                else
                {
                    return new GenericResponse<List<SupportTickets>>
                    {
                        Data = null,
                        Message = "templates not found",
                        Success = false
                    };
                }
            }
            catch (Exception e)
            {
                return new GenericResponse<List<SupportTickets>>
                {
                    Data = null,
                    Message = e.Message,
                    Success = false
                };
            }
        }

        public async Task<GenericResponse<SupportTickets>> GetSupportTicketByIdAsync(int Id)
        {
            try
            {
                var supportTicket = await _dbcontext.SupportTickets.SingleOrDefaultAsync(s => s.Id == Id);

                if (supportTicket != null)
                {
                    return new GenericResponse<SupportTickets>
                    {
                        Data = supportTicket,
                        Message = "ticket found",
                        Success = true

                    };
                }
                else
                {
                    return new GenericResponse<SupportTickets>
                    {
                        Data = null,
                        Message = "template not found",
                        Success = false

                    };
                }
            }
            catch (Exception e)
            {
                return new GenericResponse<SupportTickets>
                {
                    Data = null,
                    Message = e.Message,
                    Success = false
                };
            }

        }

        public async Task<GenericResponse<List<SupportTicketsComment>>> GetSupportTicketCommentByTicketId(int supportticketId)
        {
            try
            {
                var supportTicket = await _dbcontext.SupportTickets.SingleOrDefaultAsync(s => s.Id == supportticketId);

                if (supportTicket != null)
                {
                    var supportTicketComments = await _dbcontext.SupportComments.Where
                        (s => s.CaseId == supportticketId).ToListAsync();

                    return new GenericResponse<List<SupportTicketsComment>>
                    {
                        Data = supportTicketComments,
                        Message = "Ticket Comments found",
                        Success = true

                    };
                }
                else
                {
                    return new GenericResponse<List<SupportTicketsComment>>
                    {
                        Data = null,
                        Message = "Ticket Id doesnt exist",
                        Success = false

                    };
                }
            }
            catch (Exception e)
            {
                return new GenericResponse<List<SupportTicketsComment>>
                {
                    Data = null,
                    Message = e.Message,
                    Success = false
                };
            }
        }
    }
}
