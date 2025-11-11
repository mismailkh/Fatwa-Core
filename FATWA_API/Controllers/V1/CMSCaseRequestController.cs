using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.CaseManagement;
using FATWA_DOMAIN.Interfaces.Tasks;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Interfaces.Communication;
using FATWA_API.RabbitMQ;
using FATWA_DOMAIN.Interfaces.Notification;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Interfaces.PatternNumber;
using FATWA_INFRASTRUCTURE.Repository.PatternNumber;
using static FATWA_DOMAIN.Enums.UserEnum;
using System.Net;

namespace FATWA_API.Controllers.V1
{
    //<!-- <History Author = 'Nabeel ur Rehman' Date='2022-05-26' Version="1.0" Branch="master">Create class to manage api controller</History>

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme + ",FatwaApiKey")]
    public class CMSCaseRequestController : ControllerBase
    {
        private readonly ICMSCaseRequest _iCMSCaseRequest;
        private readonly IAuditLog _auditLogs;
        private readonly ITask _ITask;
        private readonly IAccount _IAccount;
        private readonly ICommunication _iCommunicationRepo;
        private readonly IConfiguration _configuration;
        private readonly INotification _INotification;
        private readonly IRole _IRole;
        private readonly ICMSCOMSInboxOutboxRequestPatternNumber _iCMSCOMSInboxOutboxRequestPatternNumber;
        private readonly RabbitMQClient _client;

        #region Constructor

        public CMSCaseRequestController(ICMSCaseRequest iCmsCaseRequest, IAuditLog audit, ITask iTask, IAccount iAccount, ICommunication iCommunicationRepo, 
            IConfiguration configuration, INotification iNotification, IRole iRole, ICMSCOMSInboxOutboxRequestPatternNumber iCMSCOMSInboxOutboxRequestPatternNumber,
            RabbitMQClient client)
        {
            _iCMSCaseRequest = iCmsCaseRequest;
            _auditLogs = audit;
            _ITask = iTask;
            _IAccount = iAccount;
            _iCommunicationRepo = iCommunicationRepo;
            _configuration = configuration;
            _INotification = iNotification;
            _IRole = iRole;
            _iCMSCOMSInboxOutboxRequestPatternNumber = iCMSCOMSInboxOutboxRequestPatternNumber;
            _client = client;
        }

        #endregion

        #region Get Case Request List
        [HttpPost("GetCMSCaseRequest")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCmsCaseRequest(AdvanceSearchCmsCaseRequestVM advanceSearchVM)
        {
            try
            {
                return Ok(await _iCMSCaseRequest.GetCMSCaseRequests(advanceSearchVM));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Case Request List by Sector
        [HttpPost("GetAllCaseRequestsBySectorTypeId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllCaseRequestsBySectorTypeId(int sectorTypeId)
        {
            try
            {
                return Ok(await _iCMSCaseRequest.GetAllCaseRequestsBySectorTypeId(sectorTypeId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Case request Detail

        [HttpGet("GetCMSCaseRequestDetailById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCMSCaseRequestDetailById(string RequestId, int channelId)
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
                var result = await _iCMSCaseRequest.GetCMSCaseRequestsDetailById(RequestId, channelId);
                if (result != null)
                {
                    return Ok(new ApiCallResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        IsSuccessStatusCode = true,
                        ResultData = result,
                        Message = "success"
                    });
                }
                return NotFound(new ApiCallResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccessStatusCode = false,
                    ResultData = result,
                    Message = "No_record_found"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiCallResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccessStatusCode = false,
                    ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message },
                    Message = "Error_Ocurred"
                });
            }
        }

        #endregion

        #region Get Linked Requests By Pimary Request

        [HttpGet("GetLinkedRequestsByPrimaryRequestId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-12-21' Version="1.0" Branch="master"> Get Linked Case Requests by Primary Request</History>
        public async Task<IActionResult> GetLinkedRequestsByPrimaryRequestId(Guid RequestId)
        {
            try
            {
                return Ok(await _iCMSCaseRequest.GetLinkedRequestsByPrimaryRequestId(RequestId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }
        #endregion

        #region Create
        [HttpPost("CreateCaseRequest")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CreateCaseRequest(CaseRequestCommunicationVM caseRequestCommunication)
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
                await _iCMSCaseRequest.CreateCMSCaseRequest(caseRequestCommunication);
                await _iCMSCaseRequest.CommunicationForViceHos(caseRequestCommunication);
                await _ITask.AddTaskAndNotificationForHOSAndViceHOSOfSector(new SaveTaskVM
                {
                    Task = new UserTask
                    {
                        Name = "Request_Created",
                        Description = "",
                        Date = DateTime.Now.Date,
                        AssignedBy = "System Generated",
                        IsSystemGenerated = true,
                        TaskStatusId = (int)TaskStatusEnum.Pending,
                        ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                        SectorId = (int)caseRequestCommunication.CaseRequest.SectorTypeId,
                        DepartmentId = (int)DepartmentEnum.Operational,
                        TypeId = (int)TaskTypeEnum.Task,
                        RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                        CreatedBy = caseRequestCommunication.CaseRequest.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReferenceId = caseRequestCommunication.CaseRequest.RequestId,
                        SubModuleId = (int)SubModuleEnum.CaseRequest,
                        SystemGenTypeId = (int)TaskSystemGenTypeEnum.CreateCaseRequest,
                        EntityId = (int)caseRequestCommunication.CaseRequest.GovtEntityId,
                    },
                    Action = "view",
                    EntityName = "caserequest",
                    EntityId = caseRequestCommunication.CaseRequest.RequestId.ToString()
                },
                new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    DueDate = DateTime.Now.AddDays(5),
                    CreatedBy = "System Generated",
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                    EventId = (int)NotificationEventEnum.NewRequest,
                    Action = "view",
                    EntityName = new CaseRequest().GetType().Name,
                    EntityId = caseRequestCommunication.CaseRequest.RequestId.ToString(),
                    NotificationParameter = caseRequestCommunication.CaseRequest.NotificationParameter
                },
                (int)caseRequestCommunication.CaseRequest.SectorTypeId,
                false,//Send True if required to Check Vice HOS Responsibility for All or for only Manager, send False if need to send Tasks/Notifications for All Vice HOS
                true,//Send True if need to Include HOS as well along Vice HOSs
                0//Send Chamber Number Id if need to send Tasks for Vice HOSs of specific Chamber Number
                );

                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for a new case",
                    Task = "To submit the request",
                    Description = "User able to Create Case Request successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Request has been saved",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return Ok();
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Request for a new case Failed",
                    Body = ex.Message,
                    Category = "User unable to Create Case Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for a new case Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Update

        [HttpPost("UpdateCaseRequest")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nadia Gull' Date='2022-12-14' Version="1.0" Branch="master"> Update Case Request</History>
        public async Task<IActionResult> UpdateCaseRequest(CaseRequestCommunicationVM caseRequestCommunication)
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
                await _iCMSCaseRequest.UpdatCMSeCaseRequest(caseRequestCommunication);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "To edit the Case Request",
                    Task = "To update the request",
                    Description = "Request has been updated.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Request has been updated",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok();
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Edit the Case Request Failed",
                    Body = ex.Message,
                    Category = "User unable to edit the Case Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "The request could not be updated",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });
            }
        }
        #endregion

        #region Cms Case Request Response by Id 
        [HttpGet("GetCaseRequestResponsebyRequestId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCaseRequestResponsebyRequestId(Guid RequestId, Guid CommunicationId)
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
                var result = await _iCMSCaseRequest.GetCaseRequestResponsebyRequestId(RequestId, CommunicationId);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }

        [HttpGet("GetFileRequestNeedMoreDetail")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetFileRequestNeedMoreDetail(Guid FileId, Guid CommunicationId)
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
                var result = await _iCMSCaseRequest.GetFileRequestNeedMoreDetail(FileId, CommunicationId);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get Sub Case By CaseId
        [HttpGet("GetSubCaseByCaseId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetSubCaseByCaseId(Guid CaseId)
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
                var result = await _iCMSCaseRequest.GetSubCaseByCaseId(CaseId);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get  Case Party Detail By Id
        [HttpGet("GetCMSCasePartyDetailById")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2022-09-30' Version="1.0" Branch="master"> Get Case Party detail By Id</History>
        public async Task<IActionResult> GetCMSCasePartyDetailById(string Id)
        {
            try
            {
                var result = await _iCMSCaseRequest.GetCMSCasePartyDetailById(Id);
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
        [HttpGet("GetCasePartyDetailById")]
        [MapToApiVersion("1.0")]
        //<History Author = 'ijaz Ahmad' Date='2022-09-30' Version="1.0" Branch="master"> Get Case Party detail By Id</History>
        public async Task<IActionResult> GetCasePartyDetailById(string Id)
        {
            try
            {
                var result = await _iCMSCaseRequest.GetCasePartyDetailById(Id);
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


        #region Get Case Request Status History
        [HttpGet("GetCMSCaseRequestStatusHistory")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2022-09-30' Version="1.0" Branch="master"> Get Case Request status history</History>
        public async Task<IActionResult> GetCMSCaseRequestStatusHistory(string RequestId)
        {
            try
            {
                var result = await _iCMSCaseRequest.GetCMSCaseRequestStatusHistory(RequestId);
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

        #region Get case request by Id
        [HttpGet("GetCaseRequestById")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-03-16' Version="1.0" Branch="master"> Get Literature Classifcation on Id</History>
        public async Task<IActionResult> GetCaseRequestById(Guid RequestId)
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
                CaseRequest caseRequest = await _iCMSCaseRequest.GetCaseRequestById((Guid)RequestId);
                if (caseRequest != null)
                {
                    return Ok(caseRequest);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });

            }
        }
        #endregion

        #region Send a Copy
        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master">Create a new copy of the Case Request and Send it to another Sector </History>
        [HttpPost("SendACopyCaseRequest")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SendACopyCaseRequest(CaseRequest caseRequest)
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
                //await _iCMSCaseRequest.SendACopyCaseRequest(caseRequest);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Send a Copy of Case Request",
                    Task = "Send a Copy of Case Request",
                    Description = "Send a Copy of Case Request",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Copy of he request Sent.",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return Ok();
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Send a Copy of Case Request",
                    Body = ex.Message,
                    Category = "Unable To Send a Copy of Case Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "User Unable To Send a Copy of Case Request",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }

        #endregion

        #region Link Case Requests with Primary Request

        //<History Author = 'Hassan Abbas' Date='2022-12-15' Version="1.0" Branch="master"> Link Selected Case Requests with Primary Request </History>
        [HttpPost("LinkCaseRequests")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> LinkCaseRequests(LinkCaseRequestsVM linkCaseRequest)
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
                await _iCMSCaseRequest.LinkCaseRequests(linkCaseRequest);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Link Case Requests",
                    Task = "To Link Case Requests",
                    Description = "To Link Case Requests",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Case Requests Linked.",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return Ok();
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "To Link Case Requests",
                    Body = ex.Message,
                    Category = "User unable To Link Case Requests",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Unable To Link Case Requests",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }

        #endregion

        #region Get WithDraw Cases Request by RequestId

        [HttpGet("GetWithDrawCaseRequestByRequestId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2022-01-09' Version="1.0" Branch="master"> Get With Draw Case Request By Request Id</History>
        public async Task<IActionResult> GetWithDrawCaseRequestByRequestId(Guid RequestId)
        {
            try
            {
                return Ok(await _iCMSCaseRequest.GetWithDrawCaseRequestByRequestId(RequestId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Create Withdraw Case Request

        [HttpPost("WithdrawCaseRequest")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nadia Gull' Date='2023-01-18' Version="1.0" Branch="master"> Withdraw Case Request </History>
        public async Task<IActionResult> CreateWithDrawCaseRequest(WithdrawRequestCommunicationVM cmsWithdrawRequestCommunication)
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
                await _iCMSCaseRequest.CreateWithDrawCaseRequest(cmsWithdrawRequestCommunication);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "To withdraw the Case Request ",
                    Task = "To withdraw a case request",
                    Description = "Case Request has been withdrawn.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Case Request has been withdrawn",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                User assignedTo = await _IRole.GetHOSBySectorId((int)cmsWithdrawRequestCommunication.WithdrawRequest.SectorTypeId);
                var taskId = Guid.NewGuid();
                var taskResult = await _ITask.AddTask(new SaveTaskVM
                {
                    Task = new UserTask
                    {
                        TaskId = taskId,
                        Name = "Withdraw_Request",
                        Description = "",
                        Date = DateTime.Now.Date,
                        AssignedBy = "",
                        AssignedTo = assignedTo?.Id,
                        IsSystemGenerated = true,
                        TaskStatusId = (int)TaskStatusEnum.Pending,
                        ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                        SectorId = (int)cmsWithdrawRequestCommunication.WithdrawRequest.SectorTypeId,
                        DepartmentId = (int)DepartmentEnum.Operational,
                        TypeId = (int)TaskTypeEnum.Task,
                        RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                        CreatedBy = cmsWithdrawRequestCommunication.WithdrawRequest.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReferenceId = cmsWithdrawRequestCommunication.WithdrawRequest.Id,
                        SubModuleId = (int)SubModuleEnum.CaseRequest,
                        SystemGenTypeId = (int)TaskSystemGenTypeEnum.WithdrawCaseRequest,
                        EntityId = (int)cmsWithdrawRequestCommunication.WithdrawRequest.GovtEntityId,
                    },
                    TaskActions = new List<TaskAction>()
                    {
                        new TaskAction()
                        {
                            ActionName = "Withdraw Request",
                            TaskId = taskId,
                        }
                    }
                },
                "withdraw-request",
                "detail",
                cmsWithdrawRequestCommunication.WithdrawRequest.Id.ToString() + "/" + (int)CommunicationTypeEnum.WithdrawRequested);
                return Ok();
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "To withdraw the Case Request ",
                    Body = ex.Message,
                    Category = "To withdraw the Case Request ",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Case Request could not been withdrawn",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });
            }
        }

        #endregion

        #region GetCaseRequestHistoryDetailByHistoryId

        [HttpGet("GetCaseRequestHistoryDetailByHistoryId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Zain Ul Islam' Date='2023-01-18' Version="1.0" Branch="master"> Get Case Request History Detail By HistoryId</History>
        public async Task<IActionResult> GetCaseRequestHistoryDetailByHistoryId(Guid historyId)
        {
            try
            {
                var result = await _iCMSCaseRequest.GetCaseRequestHistoryDetailByHistoryId(historyId);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse
                {
                    Message = ex.Message,
                    InnerException = ex.InnerException?.Message
                });
            }
        }

        #endregion

        #region Update Withdraw Case Request 

        //<History Author = 'Danish' Date='2023-01-23' Version="1.0" Branch="master"> Update Withdraw Case Request </History>
        [HttpPost("UpdateWithdrawCaseRequestStatus")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateWithdrawCaseRequestStatus(WithdrawRequestDetailVM caseRequest, bool isRejected)
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
                var result = await _iCMSCaseRequest.UpdateWithdrawCaseRequestStatus(caseRequest, isRejected);
                if (result != null)
                {
                    // Update Withdraw CaseRequest Status
                    UpdateEntityStatusVM updateEntity = new UpdateEntityStatusVM();

                    updateEntity.ReferenceId = caseRequest.ReferenceGuid;
                    updateEntity.Reason = caseRequest.RejectionReason;
                    if (isRejected == false)
                        updateEntity.StatusId = (int)WithdrawRequestStatusEnum.WithdrawnByGE;
                    else
                        updateEntity.StatusId = (int)WithdrawRequestStatusEnum.Rejected;
                    //Rabbit MQ send Messages
                    _client.SendMessage(updateEntity, RabbitMQKeys.RequestStatusKey);
                    foreach (var res in result)
                    {
                        //Rabbit MQ send Messages
                        _client.SendMessage(res, RabbitMQKeys.HistoryKey);
                    }

                }
                return Ok();

            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });
            }
        }
        #endregion

        #region View detail Withdraw request

        [HttpGet("GetCaseRequestWithdrawDetailById")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Danish' Date='2022-03-20' Version="1.0" Branch="master"> Get With Draw Case Request By Request Id</History>
        public async Task<IActionResult> GetCaseRequestWithdrawDetailById(Guid WithdrawRequestId, int CommunicationTypeId)
        {
            try
            {
                return Ok(await _iCMSCaseRequest.GetRequestWithdrawDetailById(WithdrawRequestId, CommunicationTypeId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Update

        [HttpPost("UpdateCaseRequestViewedStatus")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nadia Gull' Date='2022-12-14' Version="1.0" Branch="master"> Update Case Request</History>
        public async Task<IActionResult> UpdateCaseRequestViewedStatus(Guid RequestId)
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
                await _iCMSCaseRequest.UpdateCaseRequestViewedStatus(RequestId);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "To edit the Case Request status",
                    Task = "To update the request status",
                    Description = "Request has been updated.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Request has been updated",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok();
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Edit the Case Request status Failed",
                    Body = ex.Message,
                    Category = "User unable to edit the Case Request status",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "The request could not be updated",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });
            }
        }
        #endregion

        #region Task Update Status
        private async Task TaskUpdate(Guid ReferenceId, string UserId)
        {
            try
            {
                var task = await _ITask.GetTaskDetailByReferenceAndAssignedToId(ReferenceId, UserId);
                if (task != null)
                {
                    if (task.TaskStatusId == (int)TaskStatusEnum.Pending)
                    {
                        var taskstatus = await _ITask.UpdateTaskStatus(task.TaskId, (int)TaskStatusEnum.Done);
                    }
                }

            }
            catch (Exception)
            {

            }
        }
        #endregion

        #region Get New Case Request Number

        [HttpGet("GetNewCaseRequestNumber")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-04-22' Version="1.0" Branch="master"> Get new document number</History>
        public async Task<IActionResult> GetNewCaseRequestNumber(int govtEntityId)
        {
            try
            {
                var result = await _iCMSCOMSInboxOutboxRequestPatternNumber.GenerateNumberPattern(govtEntityId, (int)CmsComsNumPatternTypeEnum.CaseRequestNumber);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}


