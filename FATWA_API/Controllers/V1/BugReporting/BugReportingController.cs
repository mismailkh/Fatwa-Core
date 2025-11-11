using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.BugReporting;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.BugReporting;
using AutoMapper;
using FATWA_DOMAIN.Models.ViewModel.BugReportingVMs;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;
using FATWA_DOMAIN.Interfaces.Notification;
using FATWA_DOMAIN.Models.Notifications;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_DOMAIN.Enums.BugReporting.BugReportingEnum;
using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_API.RabbitMQ;
using System.Text.RegularExpressions;
using FATWA_DOMAIN.Enums;
//< History Author = 'Muhammad Zaeem' Date = '2024-04-28' Version = "1.0" Branch = "master" Bug Reporting Controller</History>

namespace FATWA_API.Controllers.V1.BugReporting
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme + ",FatwaApiKey")]
    public class BugReportingController : ControllerBase
    {
        #region Variable Declaration
        private readonly IBugReporting _IBugReporting;
        private readonly IAuditLog _iAuditLog;
        private readonly IMapper _mapper;
        private readonly INotification _iNotifications;
        private readonly IUsers _iUsers;
        private readonly FATWA_DOMAIN.Interfaces.IAccount _IAccount;
        private readonly IConfiguration _configuration;
        private readonly RabbitMQClient _client;

        #endregion

        #region Constructor
        public BugReportingController(IBugReporting bugReporting, IAuditLog AuditLog, IMapper mapper, INotification iNotifications, IUsers iUsers, FATWA_DOMAIN.Interfaces.IAccount iAccount, IConfiguration configuration, RabbitMQClient client)
        {
            _IBugReporting = bugReporting;
            _iAuditLog = AuditLog;
            _mapper = mapper;
            _iNotifications = iNotifications;
            _iUsers = iUsers;
            _IAccount = iAccount;
            _configuration = configuration;
            _client = client;
        }
        #endregion

        #region Get
        [HttpGet("GetAllApplications")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllApplications()
        {
            try
            {
                var result = await _IBugReporting.GetAllApplications();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpGet("GetBugModules")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetBugModules()
        {
            try
            {
                return Ok(await _IBugReporting.GetBugModules());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetModulesByApplicationId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetModulesByApplicationId(int ApplicationId)
        {
            try
            {
                return Ok(await _IBugReporting.GetModulesByApplicationId(ApplicationId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetIssueTypes")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetIssueType()
        {
            try
            {
                return Ok(await _IBugReporting.GetIssueTypes());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpGet("GetBugStatuses")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetBugStatuses()
        {
            try
            {
                return Ok(await _IBugReporting.GetBugStatuses());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpGet("GetSeverityLevel")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetSeverityLevel()
        {
            try
            {
                return Ok(await _IBugReporting.GetSeverities());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Auto Generated BugTicketId
        [HttpGet("GetAutoGeneratedIds")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAutoGeneratedId()
        {
            try
            {
                return Ok(await _IBugReporting.GetAutoGeneratedId());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Create Bug Ticket
        [HttpPost("CreateBugTicket")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CreateBugTicket(BugTicket ticket)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }
                await _IBugReporting.CreateBugTicket(ticket);
                if (ticket.StatusId != (int)BugStatusEnum.Draft)
                {
                    List<string> AllUsers = new List<string>();
                    var BugAdminUsers = await _IAccount.GetUsersByRoleId(SystemRoles.BugAdmin);
                    if (BugAdminUsers.Count() == 0)
                    {
                        var FatwaAdminUser = await _IAccount.GetUsersByRoleId(SystemRoles.FatwaAdmin);
                        AllUsers.AddRange(FatwaAdminUser);
                    }
                    var CreatorId = await _IAccount.UserIdByUserEmail(ticket.CreatedBy);
                    AllUsers.AddRange(BugAdminUsers);
                    AllUsers.Add(CreatorId);
                    AllUsers.Remove(CreatorId);
                    foreach (var receiverId in AllUsers)
                    {
                        var notificationResult = await _iNotifications.SendNotification(new Notification
                        {
                            NotificationId = Guid.NewGuid(),
                            DueDate = DateTime.Now.AddDays(5),
                            CreatedBy = ticket.CreatedBy,
                            CreatedDate = DateTime.Now,
                            IsDeleted = false,
                            ReceiverId = receiverId,
                            ModuleId = (int)WorkflowModuleEnum.BugReporting,
                        },
                        string.IsNullOrEmpty(CreatorId) ? (int)NotificationEventEnum.G2GSaveBugTicket : (int)NotificationEventEnum.SaveBugTicket,
                        "view",
                        new BugTicket().GetType().Name,
                        ticket.Id.ToString() + "/" + true,
                        ticket.NotificationParameter);
                    }

                }
                _iAuditLog.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for a new Bug Ticket ",
                    Task = "To submit the Ticket",
                    Description = "User able to Create Bug Ticket successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Bug Ticket has been saved",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.BugReporting,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok();
            }
            catch (Exception ex)
            {
                _iAuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Request for a new Bug Ticket Failed",
                    Body = ex.Message,
                    Category = "User unable to Create Bug Ticket Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for a New Bug Ticket Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.BugReporting,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Update Bug Ticket
        [HttpPost("UpdateBugTicket")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateBugTicket(BugTicket ticket)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }
                await _IBugReporting.UpdateBugTicket(ticket);
                if (ticket.StatusId != (int)BugStatusEnum.Draft)
                {
                    List<string> AllUsers = new List<string>();
                    var BugAdminUsers = await _IAccount.GetUsersByRoleId(SystemRoles.BugAdmin);
                    if (BugAdminUsers.Count() == 0)
                    {
                        var FatwaAdminUser = await _IAccount.GetUsersByRoleId(SystemRoles.FatwaAdmin);
                        AllUsers.AddRange(FatwaAdminUser);
                    }
                    var CreatorId = await _IAccount.UserIdByUserEmail(ticket.CreatedBy);
                    AllUsers.AddRange(BugAdminUsers);
                    AllUsers.Add(CreatorId);
                    AllUsers.Remove(CreatorId);
                    foreach (var receiverId in AllUsers)
                    {
                        var notificationResult = await _iNotifications.SendNotification(new Notification
                        {
                            NotificationId = Guid.NewGuid(),
                            DueDate = DateTime.Now.AddDays(5),
                            CreatedBy = ticket.CreatedBy,
                            CreatedDate = DateTime.Now,
                            IsDeleted = false,
                            ReceiverId = receiverId,
                            ModuleId = (int)WorkflowModuleEnum.BugReporting,
                        },
                        (int)NotificationEventEnum.SaveBugTicket,
                        "view",
                        new BugTicket().GetType().Name,
                        ticket.Id.ToString() + "/" + true,
                        ticket.NotificationParameter);
                    }

                }
                _iAuditLog.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for Update Bug Ticket ",
                    Task = "To Update the Bug Ticket",
                    Description = "User able to Update Bug Ticket successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Bug Ticket has been saved",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.BugReporting,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok();
            }
            catch (Exception ex)
            {
                _iAuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Request for Update Bug Ticket Failed",
                    Body = ex.Message,
                    Category = "User unable to Update Bug Ticket Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for Update Bug Ticket Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.BugReporting,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Bug Ticket By Id 
        [HttpGet("GetBugTicketById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetBugTicketById(Guid TicketId)
        {
            try
            {
                return Ok(await _IBugReporting.GetBugTicketById(TicketId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });

            }
        }
        #endregion

        #region Get Ticket List
        [HttpPost("GetTickets")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTickets(AdvanceSearchTicketListVM advanceSearchTicketList)
        {
            try
            {
                return Ok(await _IBugReporting.GetTickets(advanceSearchTicketList));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        
        #region Get Bug Ticket Detail
        [HttpGet("GetBugTicketDetail")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetBugTicketDetail(Guid Id)
        {
            try
            {
                var result = await _IBugReporting.GetBugTicketDetail(Id);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Bug Ticket Status History
        [HttpGet("GetTicketStatusHistory")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTicketStatusHistory(Guid Id)
        {
            try
            {
                return Ok(await _IBugReporting.GetTicketStatusHistory(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Auto Generated Bug Number
        [HttpGet("GetAutoGeneratedBugNumber")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAutoGeneratedBugNumber()
        {
            try
            {
                return Ok(await _IBugReporting.GetAutoGeneratedBugNumber());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get  Reported Bug By Id 
        [HttpGet("GetReportedBugById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetReportedBugById(Guid BugId)
        {
            try
            {
                return Ok(await _IBugReporting.GetReportedBugById(BugId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });

            }
        }
        #endregion

        #region Create Bug 
        [HttpPost("CreateBug")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CreateBug(ReportedBug bug)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }
                await _IBugReporting.CreateBug(bug);
                _iAuditLog.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for a new Bug",
                    Task = "To submit the Bug",
                    Description = "User able to Create Bug successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Bug has been saved",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.BugReporting,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok();
            }
            catch (Exception ex)
            {
                _iAuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Request for a new Bug Failed",
                    Body = ex.Message,
                    Category = "User unable to Create Bug Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for a New Bug Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.BugReporting,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Update Bug 
        [HttpPost("UpdateBug")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateBug(ReportedBug bug)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }
                await _IBugReporting.UpdateBug(bug);
                _iAuditLog.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for Update Bug",
                    Task = "To Update the Bug",
                    Description = "User able to Update Bug successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Bug has been saved",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.BugReporting,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok();
            }
            catch (Exception ex)
            {
                _iAuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Request for Update Bug Failed",
                    Body = ex.Message,
                    Category = "User unable to Update Bug Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for Update Bug Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.BugReporting,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Reproted Bug List
        [HttpPost("GetListReportedBug")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetListReportedBug(AdvanceSearchBugListVM advanceSearchBugList)
        {
            try
            {
                return Ok(await _IBugReporting.GetAllReportedBug(advanceSearchBugList));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Reported Bug Detail
        [HttpGet("GetReportedBugDetail")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetReportedBugDetail(Guid Id)
        {
            try
            {
                var result = await _IBugReporting.GetReportedBugDetail(Id);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Crash Report List
        [HttpPost("GetCrashReportList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCrashReportList(AdvanceSearchBugListVM advanceSearchBugList)
        {
            try
            {
                return Ok(await _IBugReporting.GetCrashReportList(advanceSearchBugList));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Bug Type List
        [HttpGet("GetBugIssueList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetBugIssueList()
        {
            try
            {
                return Ok(await _IBugReporting.GetBugIssueList());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Remove Bug Type 
        [HttpPost("RemoveBugType")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> RemoveBugType(BugIssueTypeListVM issueType)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }
                await _IBugReporting.RemoveBugType(issueType);
                _iAuditLog.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for Remove Bug Type",
                    Task = "To Remove Bug Type",
                    Description = "User able to Remove Bug Type successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Bug has been removed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.BugReporting,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok();
            }
            catch (Exception ex)
            {
                _iAuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Request for Remove Bug Type Failed",
                    Body = ex.Message,
                    Category = "User unable to Remove Bug Type Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for Remove Bug Type Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.BugReporting,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Add Bug Type 
        [HttpPost("SaveBugType")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SaveBugType(BugIssueType issueType)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }
                await _IBugReporting.SaveBugType(issueType);
                _iAuditLog.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for Add Bug Type",
                    Task = "To Add Bug Type",
                    Description = "User able to Add Bug Type successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Bug has been added",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.BugReporting,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok();
            }
            catch (Exception ex)
            {
                _iAuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Request for Add Bug Type Failed",
                    Body = ex.Message,
                    Category = "User unable to Add Bug Type Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for Add Bug Type Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.BugReporting,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Add/Edit Bug Ticket Comment FeedBack
        [HttpPost("AddCommentFeedBack")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddCommentFeedBack(BugCommentFeedBack bugTicketComment)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }
                var result = await _IBugReporting.AddCommentFeedBack(bugTicketComment);
                List<string> AllUsers = new List<string>();
                List<string> BugAdminIds = new List<string>();
                List<string> FatwaAdminIds = new List<string>();
                int notificationEvent = 0;
                if (bugTicketComment.CommentFeedbackFrom == (int)CommentFeedbackFromTypeEnum.Ticket)
                {
                    BugAdminIds = await _IAccount.GetUsersByRoleId(SystemRoles.BugAdmin);
                    if (BugAdminIds.Count == 0)
                    {
                        FatwaAdminIds = await _IAccount.GetUsersByRoleId(SystemRoles.FatwaAdmin);
                        AllUsers.AddRange(FatwaAdminIds);
                    }
                    var creatorId = await _IAccount.UserIdByUserEmail(bugTicketComment.CreatedBy);
                    if (!string.IsNullOrEmpty(result.AssignTo))
                    {
                        AllUsers.Add(result.AssignTo);
                    }
                    var TicketCreatorId = await _IAccount.UserIdByUserEmail(result.CreatedBy);
                    AllUsers.AddRange(BugAdminIds);
                    AllUsers.Add(TicketCreatorId);
                    AllUsers.Remove(creatorId);
                    var comment = await _IBugReporting.GetBugTicketCommentById(bugTicketComment.ParentCommentId);
                    if (bugTicketComment.ParentCommentId != null && comment.CreatedBy != null && comment.CreatedBy != bugTicketComment.CreatedBy)
                    {
                        AllUsers = new List<string>();
                        var parentCommentCreator = await _IAccount.UserIdByUserEmail(comment.CreatedBy);
                        AllUsers.Add(parentCommentCreator);
                    }
                    if (bugTicketComment.FromFatwa == true)
                    {
                        notificationEvent = bugTicketComment.RemarkType == (int)RemarksTypeEnum.Comment ? bugTicketComment.ParentCommentId == null ? (int)NotificationEventEnum.AddComment : (int)NotificationEventEnum.AddCommentReply : (int)NotificationEventEnum.AddFeedback;

                    }
                    else
                    {
                        notificationEvent = bugTicketComment.RemarkType == (int)RemarksTypeEnum.Comment ? bugTicketComment.ParentCommentId == null ? (int)NotificationEventEnum.AddG2GComment : (int)NotificationEventEnum.AddG2GCommentReply : (int)NotificationEventEnum.G2GAddFeedback;

                    }
                }
                if (AllUsers.Count() > 0)
                {
                    foreach (var receiverId in AllUsers)
                    {
                        var notificationResult = await _iNotifications.SendNotification(new Notification
                        {
                            NotificationId = Guid.NewGuid(),
                            DueDate = DateTime.Now.AddDays(5),
                            CreatedBy = bugTicketComment.CreatedBy,
                            CreatedDate = DateTime.Now,
                            IsDeleted = false,
                            ReceiverId = receiverId,
                            ModuleId = (int)WorkflowModuleEnum.BugReporting,
                        },
                        notificationEvent,
                        "view",
                        result.GetType().Name,
                        bugTicketComment.ReferenceId.ToString() + "/" + true,
                        bugTicketComment.NotificationParameter);
                    }

                }
                if (bugTicketComment.MentionedUser.Count > 0 && bugTicketComment.CommentFeedbackFrom == (int)CommentFeedbackFromTypeEnum.Ticket)
                {
                    foreach (var user in bugTicketComment.MentionedUser)
                    {
                        if (user.Key != ((int)MentionUserEnum.GEUser).ToString())
                        {
                            if(user.Key != ((int)MentionUserEnum.FatwaUser).ToString())
                            {
                                var notificationResult = await _iNotifications.SendNotification(new Notification
                                {
                                    NotificationId = Guid.NewGuid(),
                                    DueDate = DateTime.Now.AddDays(5),
                                    CreatedBy = bugTicketComment.CreatedBy,
                                    CreatedDate = DateTime.Now,
                                    IsDeleted = false,
                                    ReceiverId = user.Key,
                                    ModuleId = (int)WorkflowModuleEnum.BugReporting,
                                },
                                 (int)NotificationEventEnum.MentionUserNotification,
                                "view",
                                result.GetType().Name,
                                bugTicketComment.ReferenceId.ToString() + "/" + true,
                                bugTicketComment.NotificationParameter);
                            }
                            else
                            {
                                List<string> mentionUsers = new List<string>();
                                if (!string.IsNullOrEmpty(result.AssignTo))
                                {
                                    mentionUsers.Add(result.AssignTo);
                                }
                                else if (BugAdminIds.Count > 0)
                                {
                                    mentionUsers.AddRange(BugAdminIds);
                                }
                                else if(FatwaAdminIds.Count > 0)
                                { 
                                    mentionUsers.AddRange(FatwaAdminIds); 
                                }
                                foreach(var item in mentionUsers)
                                {
                                    var notificationResult = await _iNotifications.SendNotification(new Notification
                                    {
                                        NotificationId = Guid.NewGuid(),
                                        DueDate = DateTime.Now.AddDays(5),
                                        CreatedBy = bugTicketComment.CreatedBy,
                                        CreatedDate = DateTime.Now,
                                        IsDeleted = false,
                                        ReceiverId = item,
                                        ModuleId = (int)WorkflowModuleEnum.BugReporting,
                                    },
                                 (int)NotificationEventEnum.G2GMentionUserNotification,
                                "view",
                                result.GetType().Name,
                                bugTicketComment.ReferenceId.ToString() + "/" + true,
                                bugTicketComment.NotificationParameter);
                                }
                                
                            }
                            
                        }
                    }
                }
                if (bugTicketComment.FromFatwa == true)
                {
                    var resultFatwaUsers = bugTicketComment.MentionedUser.Where(x => x.Key != ((int)MentionUserEnum.GEUser).ToString()).ToList();
                    if (resultFatwaUsers != null)
                    {
                        foreach (var item in resultFatwaUsers)
                        {
                            if (bugTicketComment.Comment.Contains(item.Value))
                            {
                                bugTicketComment.Comment = bugTicketComment.Comment.Replace(item.Value, bugTicketComment.MentionedUserTranslatedName);
                            }
                        }
                    }

                    //Rabbit MQ send Messages
                    _client.SendMessage(bugTicketComment, RabbitMQKeys.AddG2GFeedbackCommentKey);
                }
                _iAuditLog.CreateProcessLog(new ProcessLog
                {
                    Process = bugTicketComment.RemarkType == (int)RemarksTypeEnum.Comment ? "Request for a new comment" : "Request for a new feedback",
                    Task = bugTicketComment.RemarkType == (int)RemarksTypeEnum.Comment ? "To submit the Comment" : "To submit the FeedBack",
                    Description = bugTicketComment.RemarkType == (int)RemarksTypeEnum.Comment ? "User able to add the comment successfully." : "User able to add feedBack successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = bugTicketComment.RemarkType == (int)RemarksTypeEnum.Comment ? "Comment has been saved" : "FeedBack has been saved",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.BugReporting,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok();
            }
            catch (Exception ex)
            {
                _iAuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = bugTicketComment.RemarkType == (int)RemarksTypeEnum.Comment ? "Request for a new comment failed" : "Request for a new feedback failed",
                    Body = ex.Message,
                    Category = bugTicketComment.RemarkType == (int)RemarksTypeEnum.Comment ? "User unable to add the comment successfully." : "User unable to add feedBack successfully.",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = bugTicketComment.RemarkType == (int)RemarksTypeEnum.Comment ? "Request for a new comment failed" : "Request for a new feedback failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.BugReporting,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpPost("UpdateCommentFeedBack")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateCommentFeedBack(BugCommentFeedBack bugTicketComment)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }
                await _IBugReporting.UpdateCommentFeedBack(bugTicketComment);
                var result = await _IBugReporting.GetBugTicketById(bugTicketComment.ReferenceId);
                if (bugTicketComment.MentionedUser.Count > 0 && result != null)
                {
                    List<string> BugAdminIds = new List<string>();
                    List<string> FatwaAdminIds = new List<string>();
                    BugAdminIds = await _IAccount.GetUsersByRoleId(SystemRoles.BugAdmin);
                    if (BugAdminIds.Count == 0)
                    {
                        FatwaAdminIds = await _IAccount.GetUsersByRoleId(SystemRoles.FatwaAdmin);
                    }
                    foreach (var user in bugTicketComment.MentionedUser)
                    {
                        if (user.Key != ((int)MentionUserEnum.GEUser).ToString())
                        {
                            if (user.Key != ((int)MentionUserEnum.FatwaUser).ToString())
                            {
                                var notificationResult = await _iNotifications.SendNotification(new Notification
                                {
                                    NotificationId = Guid.NewGuid(),
                                    DueDate = DateTime.Now.AddDays(5),
                                    CreatedBy = bugTicketComment.CreatedBy,
                                    CreatedDate = DateTime.Now,
                                    IsDeleted = false,
                                    ReceiverId = user.Key,
                                    ModuleId = (int)WorkflowModuleEnum.BugReporting,
                                },
                                 (int)NotificationEventEnum.MentionUserNotification,
                                "view",
                                result?.GetType().Name,
                                bugTicketComment.ReferenceId.ToString() + "/" + true,
                                bugTicketComment.NotificationParameter);
                            }
                            else
                            {
                                List<string> mentionUsers = new List<string>();
                                if (!string.IsNullOrEmpty(result?.AssignTo))
                                {
                                    mentionUsers.Add(result?.AssignTo);
                                }
                                else if (BugAdminIds.Count > 0)
                                {
                                    mentionUsers.AddRange(BugAdminIds);
                                }
                                else if (FatwaAdminIds.Count > 0)
                                {
                                    mentionUsers.AddRange(FatwaAdminIds);
                                }
                                foreach (var item in mentionUsers)
                                {
                                    var notificationResult = await _iNotifications.SendNotification(new Notification
                                    {
                                        NotificationId = Guid.NewGuid(),
                                        DueDate = DateTime.Now.AddDays(5),
                                        CreatedBy = bugTicketComment.CreatedBy,
                                        CreatedDate = DateTime.Now,
                                        IsDeleted = false,
                                        ReceiverId = item,
                                        ModuleId = (int)WorkflowModuleEnum.BugReporting,
                                    },
                                 (int)NotificationEventEnum.G2GMentionUserNotification,
                                    "view",
                                result?.GetType().Name,
                                bugTicketComment.ReferenceId.ToString() + "/" + true,
                                bugTicketComment.NotificationParameter);
                                }

                            }

                        }
                    }
                }

                    if (bugTicketComment.FromFatwa == true)
                {
                    //Rabbit MQ send Messages
                    _client.SendMessage(bugTicketComment, RabbitMQKeys.UpdateG2GFeedbackCommentKey);
                }
                _iAuditLog.CreateProcessLog(new ProcessLog
                {
                    Process = bugTicketComment.RemarkType == (int)RemarksTypeEnum.Comment ? "Request for a update comment" : "Request for a update feedback",
                    Task = bugTicketComment.RemarkType == (int)RemarksTypeEnum.Comment ? "To submit the comment" : "To submit the feedback",
                    Description = bugTicketComment.RemarkType == (int)RemarksTypeEnum.Comment ? "User able to update the comment successfully." : "User able to feedBack successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = bugTicketComment.RemarkType == (int)RemarksTypeEnum.Comment ? "Comment has been saved" : "FeedBack has been saved",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.BugReporting,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok();
            }
            catch (Exception ex)
            {
                _iAuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = bugTicketComment.RemarkType == (int)RemarksTypeEnum.Comment ? "Request for update comment failed" : "Request for update feedback failed",
                    Body = ex.Message,
                    Category = bugTicketComment.RemarkType == (int)RemarksTypeEnum.Comment ? "User unable to update the comment successfully." : "User unable to update feedBack successfully.",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = bugTicketComment.RemarkType == (int)RemarksTypeEnum.Comment ? "Request for update comment failed" : "Request for update feedback failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.BugReporting,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Bug Ticket Comments By Id 
        [HttpGet("GetBugTicketCommentFeedBack")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetBugTicketCommentFeedBack(Guid Id, int RemarksType)
        {
            try
            {
                return Ok(await _IBugReporting.GetBugTicketCommentFeedBack(Id, RemarksType));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });

            }
        }
        #endregion

        #region Delete Bug Ticket Comments
        [HttpPost("DeleteBugTicketComment")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> DeleteBugTicketComment(BugTicketCommentVM bugTicketCommentVM)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }
                await _IBugReporting.DeleteBugTicketComment(bugTicketCommentVM);
                if (bugTicketCommentVM.FromFatwa == true)
                {
                    //Rabbit MQ send Messages
                    _client.SendMessage(bugTicketCommentVM, RabbitMQKeys.DeleteG2GFeedbackCommentKey);
                }
                _iAuditLog.CreateProcessLog(new ProcessLog
                {
                    Process = "Delete the comment",
                    Task = "To delete the comment",
                    Description = "User able to delete the comment successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Comment has been deleted",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.BugReporting,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok();

            }
            catch (Exception ex)
            {
                _iAuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Request for delete the comment Failed",
                    Body = ex.Message,
                    Category = "User unable to delete the comment",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for delete the comment Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.BugReporting,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Assign UnAssign Issue Type User
        [HttpPost("AssigningTypesToUser")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AssigningTypesToUser(BugUserTypeAssignment assigningType)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }
                await _IBugReporting.AssigningTypesToUser(assigningType);
                _iAuditLog.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for a new Assign Type to User ",
                    Task = "To submit Type to User",
                    Description = "User able to Assign Type to User successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Assign Type to User has been saved",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.BugReporting,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok();
            }
            catch (Exception ex)
            {
                _iAuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Request for new assign type to user failed",
                    Body = ex.Message,
                    Category = "User unable to assign type to user",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for new assign type to user failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.BugReporting,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpPost("AssigningTypesModule")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AssigningTypesModule(BugModuleTypeAssignment assigningType)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }
                await _IBugReporting.AssigningTypesModule(assigningType);
                //Rabbit MQ send Messages
                _client.SendMessage(assigningType, RabbitMQKeys.AssignBugTypeToModuleKey);
                _iAuditLog.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for new assign properties to issue type",
                    Task = "To assign properties to isseu type",
                    Description = "User able to Assign properties to issue type successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Assign proeprties to issue type has been saved",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.BugReporting,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok();
            }
            catch (Exception ex)
            {
                _iAuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Request for new assign properties to issue type failed",
                    Body = ex.Message,
                    Category = "User unable to assign properties to issue type",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for new assign properties to issue type failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.BugReporting,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpPost("UnAssigningTypesModule")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UnAssigningTypesModule(BugModuleTypeAssignment assigningType)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }
                await _IBugReporting.UnAssigningTypesModule(assigningType);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }


        [HttpGet("GetAssignTypeModulesById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAssignTypeModulesById(int Id)
        {
            try
            {
                return Ok(await _IBugReporting.GetAssignTypeModules(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpPost("UnAssigningTypesToUser")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UnAssigningTypesToUser(BugUserTypeAssignment assigningType)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }
                await _IBugReporting.UnAssigningTypesToUser(assigningType);
                _iAuditLog.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for a new UnAssign Type to User ",
                    Task = "To UnAssign Type to User",
                    Description = "User able to UnAssign Type to User successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "UnAssign Type to User has been saved",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.AuditLogs,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok();
            }
            catch (Exception ex)
            {
                _iAuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Request for unassign type to user failed",
                    Body = ex.Message,
                    Category = "User unable to unassign type to user",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for  unassign type to user failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.BugReporting,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Update Ticket Status
        [HttpPost("UpdateTicketStatus")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateTicketStatus(DecisionStatusVM decisionStatus)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage)),
                    });

                }
                await _IBugReporting.UpdateTicketStatus(decisionStatus);
                List<string> AllUsers = new List<string>();
                int NotificationEvent = 0;
                if (decisionStatus.StatusId == (int)BugStatusEnum.Rejected)
                {
                    var BugAdminUsers = await _IAccount.GetUsersByRoleId(SystemRoles.BugAdmin);
                    if (BugAdminUsers.Count() == 0)
                    {
                        var FatwaAdminUser = await _IAccount.GetUsersByRoleId(SystemRoles.FatwaAdmin);
                        AllUsers.AddRange(FatwaAdminUser);
                    }
                    AllUsers.AddRange(BugAdminUsers);
                    NotificationEvent = (int)NotificationEventEnum.RejectTicket;

                }
                if (decisionStatus.StatusId == (int)BugStatusEnum.Resolved)
                {
                    var CreatorId = await _IAccount.UserIdByUserEmail(decisionStatus.EntityCreator);
                    AllUsers.Add(CreatorId);
                    NotificationEvent = (int)NotificationEventEnum.ResolveTicket;

                }
                if (decisionStatus.StatusId == (int)BugStatusEnum.Reopened || decisionStatus.StatusId == (int)BugStatusEnum.Closed)
                {
                    var BugAdminUsers = await _IAccount.GetUsersByRoleId(SystemRoles.BugAdmin);
                    if (BugAdminUsers.Count() == 0)
                    {
                        var FatwaAdminUser = await _IAccount.GetUsersByRoleId(SystemRoles.FatwaAdmin);
                        AllUsers.AddRange(FatwaAdminUser);
                    }
                    AllUsers.AddRange(BugAdminUsers);
                    var groupUsers = decisionStatus.GroupId != null ? await _iUsers.GetUmsUser(decisionStatus.GroupId.ToString(), true) : new List<UserListGroupVM>();
                    var groupUserIds = groupUsers.Select(x => x.Id);
                    AllUsers.AddRange(groupUserIds);
                    AllUsers.Add(decisionStatus.AssignedUser != null ? decisionStatus.AssignedUser : "");
                    var CreatorId = await _IAccount.UserIdByUserEmail(decisionStatus.EntityCreator);
                    AllUsers.Remove(CreatorId);
                    if (decisionStatus.FromFatwa != true)
                    {
                        NotificationEvent = decisionStatus.StatusId == (int)BugStatusEnum.Reopened ? (int)NotificationEventEnum.G2GReOpenTicket : (int)NotificationEventEnum.G2GCloseTicket;

                    }
                    else
                    {
                        NotificationEvent = decisionStatus.StatusId == (int)BugStatusEnum.Reopened ? (int)NotificationEventEnum.ReOpenTicket : (int)NotificationEventEnum.CloseTicket;

                    }

                }
                foreach (var user in AllUsers)
                {
                    var notificationResult = await _iNotifications.SendNotification(new Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        DueDate = DateTime.Now.AddDays(5),
                        CreatedBy = decisionStatus.UserName,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReceiverId = user,
                        ModuleId = (int)WorkflowModuleEnum.BugReporting,
                    },
                  NotificationEvent,
                  "view",
                  new BugTicket().GetType().Name,
                  decisionStatus.ReferenceId.ToString() + "/" + true,
                  decisionStatus.NotificationParameter);
                }
                if (decisionStatus.FromFatwa == true)
                {
                    var resultTickets = await _IBugReporting.GetBugTicketDetail(decisionStatus.ReferenceId);
                    if (resultTickets != null && (resultTickets.PortalId == (int)ApplicationEnums.G2GPortal || resultTickets.PortalId == (int)ApplicationEnums.G2GAdminPortal))
                    {
                        //Rabbit MQ send Messages
                        _client.SendMessage(decisionStatus, RabbitMQKeys.UpdateG2GTicketStatusKey);
                    }
                }
                _iAuditLog.CreateProcessLog(new ProcessLog
                {
                    Process = "Update the status of the ticket",
                    Task = "To update the status of ticket",
                    Description = "User able to update the status of ticket successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Status has been updated",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.BugReporting,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok();
            }
            catch (Exception ex)
            {
                _iAuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Request for update ticket status Failed",
                    Body = ex.Message,
                    Category = "User unable to update the ticket status",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for update ticket stauts Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.BugReporting,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Update Bug Status
        [HttpPost("UpdateBugStatus")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateBugStatus(DecisionStatusVM UpdateBugStatus)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage)),
                    });

                }
                await _IBugReporting.UpdateBugStatus(UpdateBugStatus.ReferenceId, Convert.ToInt32(UpdateBugStatus.StatusId), UpdateBugStatus.UserName, UpdateBugStatus.Reason);
                _iAuditLog.CreateProcessLog(new ProcessLog
                {
                    Process = "Update the status of the bug",
                    Task = "To update the status of bug",
                    Description = "User able to update the status of bug successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Status has been updated",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.BugReporting,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok();
            }
            catch (Exception ex)
            {
                _iAuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Request for update bug status Failed",
                    Body = ex.Message,
                    Category = "User unable to update the bug status",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for update bug stauts Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.BugReporting,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Groups By Type Id
        [HttpGet("GetGroupByTypeId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetGroupByTypeId(int TypeId)
        {
            try
            {
                var result = await _IBugReporting.GetGroupByTypeId(TypeId);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Bug List
        [HttpGet("GetBugList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetBugList()
        {
            try
            {
                return Ok(await _IBugReporting.GetBugList());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Ticket Tagging and assignment
        [HttpPost("TicketTaggingAndAssignment")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> TicketTaggingAndAssignment(TicketAssignmentVM ticketAssignment)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }
                await _IBugReporting.TicketTaggingAndAssignment(ticketAssignment);
                if (ticketAssignment.AssignmentTypeId == (int)AssingmentOptionEnums.User)
                {
                    var notificationResult = await _iNotifications.SendNotification(new Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        DueDate = DateTime.Now.AddDays(5),
                        CreatedBy = ticketAssignment.UserName,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReceiverId = ticketAssignment.UserId,
                        ModuleId = (int)WorkflowModuleEnum.BugReporting,
                    },
                        (int)NotificationEventEnum.AssignTicket,
                        "view",
                        new BugTicket().GetType().Name,
                        ticketAssignment.TicketId.ToString() + "/" + true,
                        ticketAssignment.NotificationParameter);
                }
                else if (ticketAssignment.AssignmentTypeId == (int)AssingmentOptionEnums.Group)
                {
                    var Users = await _iUsers.GetUmsUser(ticketAssignment.GroupId.ToString(), true);
                    foreach (var user in Users)
                    {
                        var notificationResult = await _iNotifications.SendNotification(new Notification
                        {
                            NotificationId = Guid.NewGuid(),
                            DueDate = DateTime.Now.AddDays(5),
                            CreatedBy = ticketAssignment.UserName,
                            CreatedDate = DateTime.Now,
                            IsDeleted = false,
                            ReceiverId = user.Id,
                            ModuleId = (int)WorkflowModuleEnum.BugReporting,
                        },
                        (int)NotificationEventEnum.AssignTicket,
                        "view",
                        new BugTicket().GetType().Name,
                        ticketAssignment.TicketId.ToString() + "/" + true,
                        ticketAssignment.NotificationParameter);
                    }
                }
                //Rabbit MQ send Messages
                _client.SendMessage(ticketAssignment, RabbitMQKeys.TicketTaggingAndAssignmentKey);
                _iAuditLog.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for tag a ticket to bug and assign to user",
                    Task = "To tag a ticket to bug and assign to user",
                    Description = "User able tag a ticket to bug and assign to user successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Tag a ticket to bug and assign to user has been saved",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.BugReporting,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok();
            }
            catch (Exception ex)
            {
                _iAuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Request for tag a ticket to bug and assign to user Failed",
                    Body = ex.Message,
                    Category = "User unable to tag a ticket to bug and assign to user",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for tag a ticket to bug and assign to user",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.BugReporting,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Update All Selected Ticket Status
        [HttpPost("UpdateAllSelectedTicketStatus")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateAllSelectedTicketStatus(DecisionStatusVM decisionStatus)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage)),
                    });

                }
                await _IBugReporting.UpdateAllSelectedTicketStatus(decisionStatus.selectedTicketList, decisionStatus.UserName);
                foreach (var ticket in decisionStatus.selectedTicketList)
                {
                    List<string> AllUsers = new List<string>();
                    var BugAdminUsers = await _IAccount.GetUsersByRoleId(SystemRoles.BugAdmin);
                    if (BugAdminUsers.Count() == 0)
                    {
                        var FatwaAdminUser = await _IAccount.GetUsersByRoleId(SystemRoles.FatwaAdmin);
                        AllUsers.AddRange(FatwaAdminUser);
                    }
                    AllUsers.AddRange(BugAdminUsers);
                    var groupUsers = ticket.GroupId != null ? await _iUsers.GetUmsUser(ticket.GroupId.ToString(), true) : new List<UserListGroupVM>();
                    var groupUserIds = groupUsers.Select(x => x.Id);
                    AllUsers.AddRange(groupUserIds);
                    AllUsers.Add(ticket.AssignTo != null ? ticket.AssignTo : "");
                    var CreatorId = await _IAccount.UserIdByUserEmail(ticket.CreatedBy);
                    AllUsers.Remove(CreatorId);
                    foreach (var user in AllUsers)
                    {
                        var notificationResult = await _iNotifications.SendNotification(new Notification
                        {
                            NotificationId = Guid.NewGuid(),
                            DueDate = DateTime.Now.AddDays(5),
                            CreatedBy = decisionStatus.UserName,
                            CreatedDate = DateTime.Now,
                            IsDeleted = false,
                            ReceiverId = user,
                            ModuleId = (int)WorkflowModuleEnum.BugReporting,
                        },
                      (int)NotificationEventEnum.CloseTicket,
                      "view",
                      new BugTicket().GetType().Name,
                      decisionStatus.ReferenceId.ToString() + "/" + true,
                      decisionStatus.NotificationParameter);
                    }
                }

                _iAuditLog.CreateProcessLog(new ProcessLog
                {
                    Process = "Update the status of the all selected  ticket",
                    Task = "To update the status of all selected ticket",
                    Description = "User able to update the status of all selected ticket successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Status has been updated",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.BugReporting,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok();
            }
            catch (Exception ex)
            {
                _iAuditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Request for update all selected ticket status Failed",
                    Body = ex.Message,
                    Category = "User unable to update all the selected  ticket status",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for update all selected ticket stauts Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.BugReporting,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Bug List For Tagging
        [HttpPost("GetBugListForTagging")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetBugListForTagging()
        {
            try
            {
                return Ok(await _IBugReporting.GetBugListForTagging());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Tickets For Bug Detail
        [HttpPost("GetTicketsListByBugId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTicketsListByBugId(Guid bugId)
        {
            try
            {
                return Ok(await _IBugReporting.GetTicketsListByBugId(bugId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

    }
}
