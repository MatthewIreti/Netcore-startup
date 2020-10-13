using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NCELAP.WebAPI.Models.Entities.Support;
using NCELAP.WebAPI.Services.Support;

namespace NCELAP.WebAPI.Controllers.Support
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupportTicketController : ControllerBase
    {
        private readonly ISupportTicket _supportTicketsService;
        public SupportTicketController(ISupportTicket supportTicketService)
        {
            _supportTicketsService = supportTicketService;
        }

        [HttpPost]
        [Route("postsupporttickets")]
        public async Task<ActionResult<GenericResponse<SupportTickets>>> CreateSupportTicket([FromBody] SupportTicketsInput supportTicketsInput)

        {
            if (ModelState.IsValid)
            {
                try
                {
                    var supportTicket = new SupportTickets
                    {
                        Department = supportTicketsInput.Department,
                        Priority = supportTicketsInput.Priority,
                        Subject = supportTicketsInput.Subject,
                        Description = supportTicketsInput.Description,
                        Message = supportTicketsInput.Message,
                        Status = "Open",
                        CaseOwner = supportTicketsInput.CaseOwner,
                        Attachment = supportTicketsInput.Attachment,
                        Response = supportTicketsInput.Response,
                        CompanyName = supportTicketsInput.CompanyName,
                        CompanyRecId = supportTicketsInput.CompanyRecId,
                        EmployeeRecId = supportTicketsInput.EmployeeRecId,
                        ContactEmail = supportTicketsInput.ContactEmail,
                        EmployeeEmail = supportTicketsInput.EmployeeEmail,
                        EmployeeName = supportTicketsInput.EmployeeName,
                        RaisedOn = supportTicketsInput.RaisedOn,
                    };

                    var newSupportTicket = await _supportTicketsService.CreateSupportTicketAsync(supportTicket);

                    if (newSupportTicket.Success == true)
                    {
                        return new GenericResponse<SupportTickets>
                        {
                            Data = newSupportTicket.Data,
                            Message = "Support ticket created successfully",
                            Success = true
                        };
                    }
                    else
                    {
                        return new GenericResponse<SupportTickets>
                        {
                            Data = null,
                            Message = "Support Ticket not created successfully",
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
            else
            {
                return new GenericResponse<SupportTickets>
                {
                    Data = null,
                    Message = "Invalid operation",
                    Success = false

                };
            }
        }

        [HttpPost]
        [Route("postsupportticketscomments")]
        public async Task<ActionResult<GenericResponse<SupportTicketsComment>>> CreateSupportTicketcommnet([FromBody] SupportTicketCommentInput supportTicketCommentInput)

        {
            if (ModelState.IsValid)
            {
                try
                {
                    var supportTicketComment = new SupportTicketsComment
                    {
                        CaseId = supportTicketCommentInput.CaseId,
                        CompanyRecId = supportTicketCommentInput.CompanyRecId,
                        EmployeeName = supportTicketCommentInput.EmployeeName,
                        EmployeeEmail = supportTicketCommentInput.EmployeeEmail,
                        Subject = supportTicketCommentInput.Subject,
                        Comment = supportTicketCommentInput.Comment
                    };

                    var newSupportTicketComment = await _supportTicketsService.AddSupportTicketComment(supportTicketComment);

                    if (newSupportTicketComment.Success == true)
                    {
                        return new GenericResponse<SupportTicketsComment>
                        {
                            Data = newSupportTicketComment.Data,
                            Message = "Support ticket comment created successfully",
                            Success = true
                        };
                    }
                    else
                    {
                        return new GenericResponse<SupportTicketsComment>
                        {
                            Data = null,
                            Message = "Support Ticket comment not created successfully",
                            Success = false
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
            else
            {
                return new GenericResponse<SupportTicketsComment>
                {
                    Data = null,
                    Message = "Invalid operation",
                    Success = false

                };
            }
        }

        [HttpDelete]
        [Route("deletesupportticketsbyid/{id}")]
        public async Task<ActionResult<GenericResponse<SupportTickets>>> DeleteSupportTicket(int Id)
        {
            try
            {
                var deleteSupportTicket = await _supportTicketsService.DeleteSupportTicketAsync(Id);

                if (deleteSupportTicket.Success == true)
                {
                    return new GenericResponse<SupportTickets>
                    {
                        Data = deleteSupportTicket.Data,
                        Message = "SupportTicket deleted successfully",
                        Success = true
                    };

                }
                else
                {
                    return new GenericResponse<SupportTickets>
                    {
                        Data = null,
                        Message = "Unable to delete support ticket",
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

        [HttpPost]
        [Route("updatesupporttickets/{id}")]
        public async Task<ActionResult<GenericResponse<SupportTickets>>> EditSupportTicket(int Id, SupportTicketsInput supportTicketsInput)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    var supportTicketEdit = await _supportTicketsService.GetSupportTicketByIdAsync(Id);

                    if (supportTicketEdit.Success == true)
                    {
                        supportTicketEdit.Data.Department = supportTicketsInput.Department;
                        supportTicketEdit.Data.Priority = supportTicketsInput.Priority;
                        supportTicketEdit.Data.Subject = supportTicketsInput.Subject;
                        supportTicketEdit.Data.Description = supportTicketsInput.Description;
                        supportTicketEdit.Data.Message = supportTicketsInput.Message;
                        supportTicketEdit.Data.Status = supportTicketsInput.Status;
                        supportTicketEdit.Data.CaseOwner = supportTicketsInput.CaseOwner;
                        supportTicketEdit.Data.Attachment = supportTicketsInput.Attachment;
                        supportTicketEdit.Data.Response = supportTicketsInput.Response;

                        var newSupportTicket = await _supportTicketsService.EditSupportAsync(supportTicketEdit.Data);

                        return new GenericResponse<SupportTickets>
                        {
                            Data = newSupportTicket.Data,
                            Message = "Support ticket updated successfully",
                            Success = true
                        };
                    }
                    else
                    {
                        return new GenericResponse<SupportTickets>
                        {
                            Data = null,
                            Message = "Support ticket not updated",
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
            else
            {
                return new GenericResponse<SupportTickets>
                {
                    Data = null,
                    Message = "Invalid operation",
                    Success = false

                };
            }
        }

        [HttpGet]
        [Route("getallsupporttickets")]
        public async Task<ActionResult<GenericResponse<IEnumerable<SupportTickets>>>> GetAllSupportTickets()
        {
            try
            {
                var allTickets = await _supportTicketsService.GetAllSupportTicketsAsync();

                if (allTickets.Success == true)
                {
                    return new GenericResponse<IEnumerable<SupportTickets>>
                    {
                        Data = allTickets.Data,
                        Message = "Ticketss listed",
                        Success = true
                    };
                }
                else
                {
                    return new GenericResponse<IEnumerable<SupportTickets>>
                    {
                        Data = null,
                        Message = "Tickets not found",
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

        [HttpGet]
        [Route("getsupportticketsbyid/{Id}")]
        public async Task<ActionResult<GenericResponse<SupportTickets>>> GetSupportTicketById(int Id)
        {
            try
            {
                var ticket = await _supportTicketsService.GetSupportTicketByIdAsync(Id);

                if (ticket.Success == true)
                {
                    return new GenericResponse<SupportTickets>
                    {
                        Data = ticket.Data,
                        Message = "ticket found",
                        Success = true

                    };
                }
                else
                {
                    return new GenericResponse<SupportTickets>
                    {
                        Data = null,
                        Message = "ticket not found",
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

        [HttpGet]
        [Route("getsupportticketscommentsbyid/{id}")]
        public async Task<ActionResult<GenericResponse<List<SupportTicketsComment>>>> GetSupportTicketCommentsById(int id)
        {
            try
            {
                var ticketcomments = await _supportTicketsService.GetSupportTicketCommentByTicketId(id);

                if (ticketcomments.Success == true)
                {
                    return new GenericResponse<List<SupportTicketsComment>>
                    {
                        Data = ticketcomments.Data,
                        Message = "ticket comments found",
                        Success = true

                    };
                }
                else
                {
                    return new GenericResponse<List<SupportTicketsComment>>
                    {
                        Data = null,
                        Message = "ticket comments not found",
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

        [HttpGet]
        [Route("getsupportticketsbyemployee/{employeeRecId}/{companyRecId}")]
        public async Task<ActionResult<GenericResponse<List<SupportTickets>>>> GetAllSupportTicketsByEmployee(long employeeRecId, long companyRecId)
        {
            try
            {
                var ticket = await _supportTicketsService.GetAllSupportTicketsByEmployee(employeeRecId, companyRecId);

                if (ticket.Success == true)
                {
                    return new GenericResponse<List<SupportTickets>>
                    {
                        Data = ticket.Data,
                        Message = "ticket found",
                        Success = true

                    };
                }
                else
                {
                    return new GenericResponse<List<SupportTickets>>
                    {
                        Data = null,
                        Message = "ticket not found",
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
    }
}