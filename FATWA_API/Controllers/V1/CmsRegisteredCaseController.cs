using AutoMapper;
using FATWA_API.RabbitMQ;
using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.CaseManagement;
using FATWA_DOMAIN.Interfaces.Notification;
using FATWA_DOMAIN.Interfaces.Tasks;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_DOMAIN.Models.WorkflowModels;
using FATWA_INFRASTRUCTURE.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.Net;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_DOMAIN.Enums.UserEnum;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_API.Controllers.V1
{
    //<!-- <History Author = ijaz Ahmad' Date='2022-05-26' Version="1.0" Branch="master">Create class to manage api controller</History>

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme + ",FatwaApiKey")]
    public class CmsRegisteredCaseController : ControllerBase
    {
        private readonly ICmsRegisteredCase _iCmsRegisteredCase;
        private readonly IAuditLog _auditLogs;
        private readonly ITask _ITask;
        private readonly ICmsCaseFile _ICmsCaseFile;
        private readonly IAccount _IAccount;
        private readonly INotification _iNotifications;
        private readonly IConfiguration _configuration;
        private readonly IRole _IRole;
        private readonly ICmsShared _ICmsShared;
        private readonly IMapper _mapper;
        private readonly IWorkflow _Iworkflow;
        private readonly RabbitMQClient _client;


        public CmsRegisteredCaseController(ICmsRegisteredCase icmsregisteredcase, IAuditLog audit, ITask iTask, ICmsCaseFile iCmsCaseFile, IAccount iAccount, 
            IConfiguration configuration, INotification iNotifications, IRole iRole, ICmsShared iCmsShared, IMapper mapper, IWorkflow iworkflow, RabbitMQClient client)
        {
            _iCmsRegisteredCase = icmsregisteredcase;
            _auditLogs = audit;
            _ITask = iTask;
            _ICmsCaseFile = iCmsCaseFile;
            _IAccount = iAccount;
            _configuration = configuration;
            _iNotifications = iNotifications;
            _IRole = iRole;
            _ICmsShared = iCmsShared;
            _mapper = mapper;
            _Iworkflow = iworkflow;
            _client = client;
        }

        #region Create Registered Case

        [HttpPost("CreateRegisteredCase")]
        [MapToApiVersion("1.0")]

        //<History Author = 'Hassan Abbas' Date='2022-11-22' Version="1.0" Branch="master"> Handle Create Registered Case</History>
        public async Task<IActionResult> CreateRegisteredCase(CmsRegisteredCase registeredCase)
        {
            try
            {
                var result = await _iCmsRegisteredCase.CreateRegisteredCase(registeredCase);
                if (registeredCase.AttachmentTypeId != (int)AttachmentTypeEnum.PerformOrderNotes
                    && registeredCase.AttachmentTypeId != (int)AttachmentTypeEnum.OrderOnPetitionNotes)
                {
                    var user = _IAccount.GetUserByUserEmail(registeredCase.CreatedBy);
                    User assignedTo = await _IRole.GetHOSBySectorId((int)user.SectorTypeId);

                    await _ITask.AddTaskAndNotificationForHOSAndViceHOSOfSector(new SaveTaskVM
                    {
                        Task = new UserTask
                        {
                            Name = "Registered_Case_To_Moj",
                            Description = "",
                            Date = DateTime.Now.Date,
                            AssignedBy = user.Id.ToString(),
                            IsSystemGenerated = true,
                            TaskStatusId = (int)TaskStatusEnum.Pending,
                            ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                            SectorId = (int)user.SectorTypeId,
                            DepartmentId = (int)DepartmentEnum.Operational,
                            TypeId = (int)TaskTypeEnum.Transfer,
                            RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                            CreatedBy = registeredCase.CreatedBy,
                            CreatedDate = DateTime.Now,
                            IsDeleted = false,
                            ReferenceId = (user.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeAppealCases ||
                            user.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialAppealCases ||
                            user.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeSupremeCases ||
                            user.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialSupremeCases) ? registeredCase.CaseId : registeredCase.FileId,
                            SubModuleId = (user.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeAppealCases ||
                            user.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialAppealCases ||
                            user.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeSupremeCases ||
                            user.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialSupremeCases) ? (int)SubModuleEnum.RegisteredCase : (int)SubModuleEnum.CaseFile,
                            SystemGenTypeId = (int)TaskSystemGenTypeEnum.RegisteredCaseToMoj,
                        },
                        Action = "view",
                        EntityName =(user.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeAppealCases||
                        user.SectorTypeId== (int)OperatingSectorTypeEnum.CivilCommercialAppealCases||
                        user.SectorTypeId== (int)OperatingSectorTypeEnum.AdministrativeSupremeCases||
                        user.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialSupremeCases) ? "case" : "casefile",
                        EntityId = (user.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeAppealCases ||
                        user.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialAppealCases ||
                        user.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeSupremeCases ||
                        user.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialSupremeCases) ? registeredCase.CaseId.ToString() : registeredCase.FileId.ToString()
                    },
                    new Notification
                    {
                        DueDate = DateTime.Now.AddDays(5),
                        CreatedBy = registeredCase.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                        EventId = (int)NotificationEventEnum.CaseDataPushedFromRPA,
                        Action = "view",
                        EntityName = (user.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeAppealCases ||
                        user.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialAppealCases ||
                        user.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeSupremeCases ||
                        user.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialSupremeCases) ? "case" : "casefile",
                        EntityId = (user.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeAppealCases ||
                        user.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialAppealCases ||
                        user.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeSupremeCases ||
                        user.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialSupremeCases) ? registeredCase.CaseId.ToString() : registeredCase.FileId.ToString(),
                        NotificationParameter = registeredCase.NotificationParameter
                    },
                    (int)user.SectorTypeId,
                    false,//Send True if required to Check Vice HOS Responsibility for All or for only Manager, send False if need to send Tasks/Notifications for All Vice HOS
                    true,//Send True if need to Include HOS as well along Vice HOSs
                    registeredCase.ChamberNumberId//Send Chamber Number Id if need to send Tasks for Vice HOSs of specific Chamber Number
                    );
                }
                if (result != null)
                {
                    //Rabbit MQ send Messages
                    _client.SendMessage(result, RabbitMQKeys.RegisteredCaseKey);
                    // Update Case Request Status
                    UpdateEntityHistoryVM updateEntityHistory = new UpdateEntityHistoryVM();
                    updateEntityHistory.HistoryId = Guid.NewGuid();
                    updateEntityHistory.ReferenceId = registeredCase.FileId;
                    updateEntityHistory.StatusId = (int)CaseFileStatusEnum.RegisteredInMoj;
                    updateEntityHistory.CreatedDate = DateTime.Now;
                    updateEntityHistory.CreatedBy = registeredCase.CreatedBy;
                    updateEntityHistory.EventId = (int)CaseFileEventEnum.RegisteredAtMoj;
                    updateEntityHistory.SubModuleId = (int)SubModuleEnum.CaseFile;
                    _client.SendMessage(updateEntityHistory, RabbitMQKeys.HistoryKey);
                }
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Create Registered Case",
                    Task = "To add case details received from Moj",
                    Description = "To add case details received from Moj",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Registered Case Added.",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok(registeredCase);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Create Registered Case",
                    Body = ex.Message,
                    Category = "User unable to add the Registered Case details",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Unable to assign the create Registered Case",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Create Merge Request

        [HttpPost("CreateMergeRequest")]
        [MapToApiVersion("1.0")]

        //<History Author = 'Hassan Abbas' Date='2022-11-22' Version="1.0" Branch="master"> Handle Create Merge Request</History>
        public async Task<IActionResult> CreateMergeRequest(MergeRequest mergeRequest)
        {
            try
            {
                await _iCmsRegisteredCase.CreateMergeRequest(mergeRequest);

                foreach (var registeredCases in mergeRequest.RegisteredCases.Where(x => x.CaseId != mergeRequest.PrimaryId))
                {
                    var caseAssignment = _ICmsCaseFile.GetPrimaryCaseAssignmentByCaseId(registeredCases.CaseId);
                    if (caseAssignment.Result is not null)
                    {
                        caseAssignment.Result.NotificationParameter.PrimaryCaseNumber = mergeRequest.NotificationParameter.CaseNumber;
                        string assignedTo = caseAssignment.Result.LawyerId;
                        string assignedBy = await _IAccount.UserIdByUserEmail(mergeRequest.CreatedBy);

                        var taskId = Guid.NewGuid();
                        var taskResult = await _ITask.AddTask(new SaveTaskVM
                        {
                            Task = new UserTask
                            {
                                TaskId = taskId,
                                Name = "Case_Merge_Request_Task",
                                Description = mergeRequest.Reason,
                                Date = DateTime.Now.Date,
                                AssignedBy = assignedBy,
                                AssignedTo = assignedTo,
                                IsSystemGenerated = true,
                                TaskStatusId = (int)TaskStatusEnum.Pending,
                                ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                                SectorId = (int)await _IAccount.GetSectorIdByEmail(mergeRequest.CreatedBy),
                                DepartmentId = (int)DepartmentEnum.Operational,
                                TypeId = (int)TaskTypeEnum.Task,
                                RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                                CreatedBy = mergeRequest.CreatedBy,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReferenceId = registeredCases.CaseId,
                                SubModuleId = (int)SubModuleEnum.RegisteredCase,
                                SystemGenTypeId = (int)TaskSystemGenTypeEnum.CaseMergeRequest,
                            },
                            TaskActions = new List<TaskAction>()
                            {
                                new TaskAction()
                                {
                                    ActionName = "Case Merge Request Action",
                                    TaskId = taskId,
                                }
                            }
                        },
                        "view",
                        new MergeRequest().GetType().Name,
                        mergeRequest.Id.ToString());

                        var notificationResult = await _iNotifications.SendNotification(new Notification
                        {
                            NotificationId = Guid.NewGuid(),
                            DueDate = DateTime.Now.AddDays(5),
                            CreatedBy = mergeRequest.CreatedBy,
                            CreatedDate = DateTime.Now,
                            IsDeleted = false,
                            ReceiverId = assignedTo,
                            ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                        },
                        (int)NotificationEventEnum.CreateMergeRequest,
                        "view",
                        new MergeRequest().GetType().Name,
                        mergeRequest.Id.ToString(),
                        caseAssignment.Result.NotificationParameter);
                    }
                }
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Create Merge Request",
                    Task = "To Merge multiple Cases into one",
                    Description = "To Merge multiple Cases into one",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Merge Request created.",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok();
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Create Merge Request",
                    Body = ex.Message,
                    Category = "User unable to add the Merge Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "User unable to add the Merge Request",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Approve Merge Request

        [HttpPost("ApproveMergeRequest")]
        [MapToApiVersion("1.0")]

        //<History Author = 'Hassan Abbas' Date='2022-11-22' Version="1.0" Branch="master"> Approve Merge Request</History>
        public async Task<IActionResult> ApproveMergeRequest(Guid mergeRequestId, string loggedInUser)
        {
            try
            {
                await _iCmsRegisteredCase.ApproveMergeRequest(mergeRequestId, loggedInUser);

                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Approve Merge Request",
                    Task = "To approve merge request",
                    Description = "To Approve merge request",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Merge Request approved.",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                var notificationResponse = await _iNotifications.SendNotification(new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    DueDate = DateTime.Now.AddDays(5),
                    CreatedBy = loggedInUser,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    ReceiverId = loggedInUser,// Assign To Sactor Id
                    ModuleId = (int)WorkflowModuleEnum.CaseManagement
                },
                (int)NotificationEventEnum.ApproveMergeRequest,
                "view",
                new MergeRequest().GetType().Name,
                loggedInUser,
                new NotificationParameter());
                return Ok();
            }
            catch (Exception ex)
            {

                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Approve Merge Request",
                    Body = ex.Message,
                    Category = "User unable to Approve Merge Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "User unable to Approve Merge Request",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Reject Merge Request

        [HttpPost("RejectMergeRequest")]
        [MapToApiVersion("1.0")]

        //<History Author = 'Hassan Abbas' Date='2022-11-22' Version="1.0" Branch="master"> Reject Merge Request</History>
        public async Task<IActionResult> RejectMergeRequest(Guid mergeRequestId)
        {
            try
            {
                await _iCmsRegisteredCase.RejectMergeRequest(mergeRequestId);

                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Reject Merge Request",
                    Task = "To Reject merge request",
                    Description = "To Reject merge request",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Merge Request rejected.",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok();
            }
            catch (Exception ex)
            {

                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Reject Merge Request",
                    Body = ex.Message,
                    Category = "User unable to Reject Merge Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "User unable to Reject Merge Request",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get All Registered Cases By Sector/Court Type
        [HttpGet("GetAllRegisteredCasesByCourtTypeId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2023-08-03' Version="1.0" Branch="master"> Get All Registered Cases By Sector/CourtType</History>
        public async Task<IActionResult> GetAllRegisteredCasesByCourtTypeId(int courtTypeId, string userId)
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetAllRegisteredCasesByCourtTypeId(courtTypeId, userId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Registered Case VM Detail by Id

        [HttpGet("GetRegisteredCaseDetailByIdVM")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Get Registered Case detail by Id</History>
        public async Task<IActionResult> GetRegisteredCaseDetailByIdVM(Guid caseId, string userId)
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetRegisteredCaseDetailByIdVM(caseId, userId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Add Hearing

        [HttpPost("AddHearing")]
        [MapToApiVersion("1.0")]

        //<History Author = 'Hassan Abbas' Date='2022-11-22' Version="1.0" Branch="master"> Handle Add Hearing Request</History>
        //<History Author = 'Hassan Abbas' Date='2023-03-23' Version="1.0" Branch="master">Send Portfolio Request to MOJ for Hearing </History>
        public async Task<IActionResult> AddHearing(Hearing hearing)
        {
            try
            {
                var result = await _iCmsRegisteredCase.AddHearing(hearing);
                if (result)
                {
                    //Rabbit MQ send Messages
                    _client.SendMessage(hearing, RabbitMQKeys.HearingKey);
                }
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Add Hearing",
                    Task = "To Add Hearing for a Case",
                    Description = "To Add Hearing for a Case",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Hearing added.",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                if (hearing.IsUpdated == false)
                {
                    var RecieverId = await _IAccount.UserIdByUserEmail(hearing.CreatedBy);
                    if (hearing.LawyerId != RecieverId)
                    {
                        var taskId = Guid.NewGuid();
                        var taskResult = await _ITask.AddTask(new SaveTaskVM
                        {
                            Task = new UserTask
                            {
                                TaskId = taskId,
                                Name = "Add_Hearing",
                                Date = DateTime.Now.Date,
                                AssignedBy = RecieverId,
                                AssignedTo = hearing.LawyerId,
                                IsSystemGenerated = true,
                                TaskStatusId = (int)TaskStatusEnum.Done,
                                ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                                SectorId = (int)hearing.SectorTypeId,
                                DepartmentId = (int)DepartmentEnum.Operational,
                                TypeId = (int)TaskTypeEnum.Transfer,
                                RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                                CreatedBy = hearing.CreatedBy,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReferenceId = hearing.Id,
                                SubModuleId = (int)SubModuleEnum.RegisteredCase,
                                SystemGenTypeId = (int)TaskSystemGenTypeEnum.Hearing,
                                ModifiedDate = DateTime.Now,
                                ModifiedBy = hearing.CreatedBy,
                            },
                            TaskActions = new List<TaskAction>()
                    {
                        new TaskAction()
                        {
                            ActionName = "Add Hearing",
                            TaskId = taskId,
                        }
                    }
                        },
                        "view",
                        "hearing",
                        hearing.Id.ToString());
                    }
                }
                return Ok();
            }
            catch (Exception ex)
            {

                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Add Hearing",
                    Body = ex.Message,
                    Category = "User unable to add the Hearing for a case",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "User unable to add the Hearing for a case",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Hearings By Case

        [HttpGet("GetHearingsByCase")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Get Hearings by Case Id</History>
        public async Task<IActionResult> GetHearingsByCase(Guid caseId)
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetHearingsByCase(caseId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Hearings Detail

        [HttpGet("GetHearingDetail")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2023-11-21' Version="1.0" Branch="master"> Get Hearing detail</History>
        public async Task<IActionResult> GetHearingDetail(Guid hearingId)
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetHearingDetail(hearingId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpGet("GetHearingDetailByHearingId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2023-11-21' Version="1.0" Branch="master"> Get Hearing detail</History>
        public async Task<IActionResult> GetHearingDetailByHearingId(Guid hearingId)
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetHearingDetailByHearingId(hearingId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpGet("GetExecutionDetail")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2023-11-21' Version="1.0" Branch="master"> Get Hearing detail</History>
        public async Task<IActionResult> GetExecutionDetail(Guid executionId)
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetExecutionDetail(executionId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion


        #region Get Hearings By Case

        [HttpGet("GetOutcomesHearingByCase")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Get Outcomes of Hearing By Case Id</History>
        public async Task<IActionResult> GetOutcomesHearingByCase(Guid caseId)
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetOutcomesHearingByCase(caseId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region Get hearing and Outcome By Case

        [HttpGet("GetOutcomesAndHearingByCase")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Get Outcomes of Hearing By Case Id</History>
        public async Task<IActionResult> GetOutcomesAndHearingByCase(Guid caseId)
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetOutcomesAndHearingByCase(caseId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get upcoming hearings of a lawyer
        [HttpGet("GetUpcomingHearingsOfLawyer")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ammaar Naveed' Date='2024-04-03' Version="1.0" Branch="master"> Get upcoming + previous hearings of lawyer on condition</History>
        //<History Author = 'Ammaar Naveed' Date='2024-03-26' Version="1.0" Branch="master"> Get upcoming hearings against userId==LawyerId</History>
        public async Task<IActionResult> GetUpcomingHearingsOfLawyer(string LawyerId, bool isPrevious)
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetHearingsOfLawyer(LawyerId, isPrevious));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Judgements By Case

        [HttpGet("GetJudgementsByCase")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Get Judgements By Case Id</History>
        public async Task<IActionResult> GetJudgementsByCase(Guid caseId)
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetJudgementsByCase(caseId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Judgements By Outcome

        [HttpGet("GetJudgementsByOutcome")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2023-12-06' Version="1.0" Branch="master"> Get Judgements By Outcome</History>
        public async Task<IActionResult> GetJudgementsByOutcome(Guid outcomeId)
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetJudgementsByOutcome(outcomeId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Transfer History
        [HttpGet("GetTransferHistoryByOutcome")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2023-12-06' Version="1.0" Branch="master"> Get Judgements By Outcome</History>
        public async Task<IActionResult> GetTransferHistoryByOutcome(Guid outcomeId, Guid CaseId)
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetTransferHistoryByOutcome(outcomeId, CaseId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Registered Case history status

        [HttpGet("GetRegisteredCaseStatusHistory")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-12-10' Version="1.0" Branch="master"> Get Registered Case status history</History>
        public async Task<IActionResult> GetRegisteredCaseStatusHistory(Guid caseId)
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetRegisteredCaseStatusHistory(caseId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Add Outcome Hearing

        [HttpPost("AddOutcomeHearing")]
        [MapToApiVersion("1.0")]

        //<History Author = 'Hassan Abbas' Date='2022-11-22' Version="1.0" Branch="master"> Handle Add Hearing Request</History>
        public async Task<IActionResult> AddOutcomeHearing(OutcomeHearing outcomeHearing)
        {
            try
            {
                await _iCmsRegisteredCase.AddOutcomeHearing(outcomeHearing);
                //Rabbit MQ send Messages
                _client.SendMessage(outcomeHearing, RabbitMQKeys.OutcomeHearingKey);
                if (outcomeHearing.outcomeJudgement.Count > 0)
                {
                    foreach (var outcomeJudgement in outcomeHearing.outcomeJudgement)
                    {
                        await AddJudgementNotificationAndSendToG2G(outcomeJudgement, outcomeHearing.NotificationParameter);
                        if (outcomeJudgement.mojExecutionRequest != null)
                        {
                            await AddExecutionTaskAndNotification(outcomeJudgement.mojExecutionRequest, outcomeHearing.NotificationParameter);
                        }
                    }
                }
                if (outcomeHearing.caseTransferRequestsVM.Any(x => x.IsAlreadyExist == false))
                {
                    foreach (var registeredCaseTransferRequest in outcomeHearing.caseTransferRequestsVM)
                    {
                        await AddCaseTransferRequestTaskAndNotification(registeredCaseTransferRequest, outcomeHearing.NotificationParameter);
                    }
                }
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Add Outcome of Hearing",
                    Task = "To Add Outcome of Hearing for a Case",
                    Description = "To Add Outcome of Hearing for a Case",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Outcome of a Hearing added.",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok();
            }
            catch (Exception ex)
            {

                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Add Outcome of Hearing",
                    Body = ex.Message,
                    Category = "User unable to add Outcome of Hearing for a case",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "User unable to add Outcome of Hearing for a case",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Outcome Hearings Detail

        [HttpGet("GetOutcomeDetail")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2023-11-25' Version="1.0" Branch="master"> Get Outcome detail</History>
        public async Task<IActionResult> GetOutcomeDetail(Guid outcomeId)
        {
            try
            {
                var result = await _iCmsRegisteredCase.GetOutcomeDetail(outcomeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region Create Task and notification for Execution
        protected async Task AddExecutionTaskAndNotification(MojExecutionRequest executionRequest, NotificationParameter NotificationParameter)
        {
            User assignedTo = await _IRole.GetHOSBySectorId(executionRequest.SectorTypeId);

            CmsApprovalTracking approvalTracking = new CmsApprovalTracking
            {
                Id = new Guid(),
                ReferenceId = executionRequest.Id,
                SectorTo = executionRequest.SectorTypeId != null ? (int)executionRequest.SectorTypeId : 0,
                SectorFrom = executionRequest.SectorTypeId != null ? (int)executionRequest.SectorTypeId : 0,
                ProcessTypeId = (int)ApprovalProcessTypeEnum.ExecutionRequest,
                StatusId = (int)ApprovalStatusEnum.Pending,
                CreatedDate = DateTime.Now,
                CreatedBy = executionRequest.CreatedBy
            };

            await _ICmsShared.SaveApprovalTrackingProcess(approvalTracking);

            await _ITask.AddTaskAndNotificationForHOSAndViceHOSOfSector(new SaveTaskVM
            {
                Task = new UserTask
                {
                    Name = "Execution_Request_Created_For_Review",
                    Date = DateTime.Now.Date,
                    AssignedBy = await _IAccount.UserIdByUserEmail(executionRequest.CreatedBy),
                    IsSystemGenerated = true,
                    TaskStatusId = (int)TaskStatusEnum.Pending,
                    ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                    SectorId = executionRequest.SectorTypeId,
                    DepartmentId = (int)DepartmentEnum.Operational,
                    TypeId = (int)TaskTypeEnum.Transfer,
                    RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                    CreatedBy = executionRequest.CreatedBy,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    ReferenceId = executionRequest.Id,
                    SubModuleId = (int)SubModuleEnum.RegisteredCase,
                    SystemGenTypeId = (int)TaskSystemGenTypeEnum.ExecutionRequest,
                },
                Action = "detail",
                EntityName = "executionrequest",
                EntityId = executionRequest.Id.ToString()
            },
                new Notification
                {
                    DueDate = DateTime.Now.AddDays(5),
                    CreatedBy = executionRequest.CreatedBy,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                    EventId = (int)NotificationEventEnum.CreateExecutionRequest,
                    Action = "detail",
                    EntityName = "executionrequest",
                    EntityId = executionRequest.Id.ToString(),
                    NotificationParameter = executionRequest.NotificationParameter
                },
                executionRequest.SectorTypeId,
                true,//Send True if required to Check Vice HOS Responsibility for All or for only Manager, send False if need to send Tasks/Notifications for All Vice HOS
                true,
                0//Send True if need to Include HOS as well along Vice HOSs
                );
        }
        #endregion
        #region Create Task and notification for Judgement
        protected async Task AddJudgementNotificationAndSendToG2G(JudgementVM outcomeJudgement, NotificationParameter NotificationParameter)
        {
            Judgement judgement = new Judgement();
            judgement = _mapper.Map<Judgement>(outcomeJudgement);
            _client.SendMessage(judgement, RabbitMQKeys.JudgementKey);
            if (outcomeJudgement.IsFinal = true)
            {
                var notificationResult = await _iNotifications.SendNotification(new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    DueDate = DateTime.Now.AddDays(5),
                    CreatedBy = outcomeJudgement.CreatedBy,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    ReceiverId = await _IAccount.UserIdByUserEmail(outcomeJudgement.CreatedBy),
                    ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                },
                (int)NotificationEventEnum.AddJudgement,
                "view",
                "case",
                outcomeJudgement?.CaseId.ToString(),
                NotificationParameter);
                await AddTaskAndNotificationForFinalJudgement(outcomeJudgement, NotificationParameter);
            }

        }
        #endregion
        #region Create Task and Notification For Case Transfer Request
        protected async Task AddCaseTransferRequestTaskAndNotification(CmsRegisteredCaseTransferRequestVM cmsRegisteredCaseTransferRequest, NotificationParameter NotificationParameter)
        {
            try
            {
                var taskId = Guid.NewGuid();
                SaveTaskVM taskObj = null;
                Notification notificationObj = null;
                taskObj = new SaveTaskVM
                {
                    Task = new UserTask
                    {
                        TaskId = taskId,
                        Name = "Registered_Case_Transfer_Request_Task",
                        Description = cmsRegisteredCaseTransferRequest.Remarks,
                        Date = DateTime.Now.Date,
                        AssignedBy = await _IAccount.UserIdByUserEmail(cmsRegisteredCaseTransferRequest?.CreatedBy),
                        IsSystemGenerated = true,
                        TaskStatusId = (int)TaskStatusEnum.Pending,
                        ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                        SectorId = (int)cmsRegisteredCaseTransferRequest.SectorTypeId,
                        DepartmentId = (int)DepartmentEnum.Operational,
                        TypeId = (int)TaskTypeEnum.Task,
                        RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                        CreatedBy = cmsRegisteredCaseTransferRequest.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReferenceId = cmsRegisteredCaseTransferRequest.Id,
                        SubModuleId = (int)SubModuleEnum.CaseFile,
                        SystemGenTypeId = (int)TaskSystemGenTypeEnum.RegisteredCaseTranferRequest,
                    },
                    EntityName = "casetransferrequest",
                    Action = "view",
                    EntityId = cmsRegisteredCaseTransferRequest.Id.ToString()
                };
                notificationObj =
                    new Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        DueDate = DateTime.Now.AddDays(5),
                        CreatedBy = cmsRegisteredCaseTransferRequest.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                        EventId = (int)NotificationEventEnum.RegisteredCaseTransferRequest,
                        EntityName = "casetransferrequest",
                        Action = "view",
                        EntityId = cmsRegisteredCaseTransferRequest.Id.ToString(),
                        NotificationParameter = NotificationParameter
                    };
                await _ITask.AddTaskAndNotificationForHOSAndViceHOSOfSector(taskObj, notificationObj, (int)cmsRegisteredCaseTransferRequest.SectorTypeId,
                true,//Send True if required to Check Vice HOS Responsibility for All or for only Manager, send False if need to send Tasks/Notifications for All Vice HOS
                true,//Send True if need to Include HOS as well along Vice HOSs
                0//Send Chamber Number Id if need to send Tasks for Vice HOSs of specific Chamber Number
                );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Create Task and Notification When final Judgement Issued
        protected async Task AddTaskAndNotificationForFinalJudgement(JudgementVM outcomeJudgement, NotificationParameter NotificationParameter)
        {
            try
            {
                var taskId = Guid.NewGuid();
                SaveTaskVM taskObj = null;
                Notification notificationObj = null;
                var caseDetail = await _iCmsRegisteredCase.GetRegisteredCaseById((Guid)outcomeJudgement.CaseId);
                taskObj = new SaveTaskVM
                {
                    Task = new UserTask
                    {
                        TaskId = taskId,
                        Name = "Registered_Case_FinalJudgement_Issued_Task",
                        Description = outcomeJudgement.Remarks,
                        Date = DateTime.Now.Date,
                        AssignedBy = await _IAccount.UserIdByUserEmail(outcomeJudgement?.CreatedBy),
                        IsSystemGenerated = true,
                        TaskStatusId = (int)TaskStatusEnum.Pending,
                        ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                        SectorId = (int)outcomeJudgement.SectorTypeId,
                        DepartmentId = (int)DepartmentEnum.Operational,
                        TypeId = (int)TaskTypeEnum.Task,
                        RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                        CreatedBy = outcomeJudgement.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReferenceId = caseDetail.FileId,
                        SubModuleId = (int)SubModuleEnum.CaseFile,
                        SystemGenTypeId = (int)TaskSystemGenTypeEnum.FinalJudgementIssued,
                    },
                    EntityName = new CaseFile().GetType().Name,
                    Action = "view",
                    EntityId = caseDetail.FileId.ToString()
                };
                notificationObj =
                    new Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        DueDate = DateTime.Now.AddDays(5),
                        CreatedBy = outcomeJudgement.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                        EventId = (int)NotificationEventEnum.FinalJudgementIssued,
                        EntityName = new CaseFile().GetType().Name,
                        Action = "view",
                        EntityId = caseDetail.FileId.ToString(),
                        NotificationParameter = NotificationParameter
                    };
                await _ITask.AddTaskAndNotificationForHOSAndViceHOSOfSector(taskObj, notificationObj, (int)outcomeJudgement.SectorTypeId,
                true,//Send True if required to Check Vice HOS Responsibility for All or for only Manager, send False if need to send Tasks/Notifications for All Vice HOS
                true,//Send True if need to Include HOS as well along Vice HOSs
                0//Send Chamber Number Id if need to send Tasks for Vice HOSs of specific Chamber Number
                );
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Get Registered Case  by Id

        [HttpGet("GetRegisteredCaseById")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2022-10-21' Version="1.0" Branch="master"> Get Registered Case  Id</History>
        public async Task<IActionResult> GetRegisteredCaseById(Guid caseId)
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
                CmsRegisteredCase registeredCase = await _iCmsRegisteredCase.GetRegisteredCaseById(caseId);
                if (registeredCase != null)
                {
                    return Ok(registeredCase);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Merge Request Detail by Id

        [HttpGet("GetMergeRequestDetailById")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Get Merge Request detail by Id</History>
        public async Task<IActionResult> GetMergeRequestDetailById(Guid id)
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetMergeRequestDetailById(id));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Merged Cases By Merge Request Id

        [HttpGet("GetMergedCasesByMergeRequestId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Get Merge Request detail by Id</History>
        public async Task<IActionResult> GetMergedCasesByMergeRequestId(Guid mergeRequestId)
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetMergedCasesByMergeRequestId(mergeRequestId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Merge Requests For Approval

        [HttpGet("GetMergeRequestsForApproval")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-11-21' Version="1.0" Branch="master"> Get Merge Requests for Approval</History>
        public async Task<IActionResult> GetMergeRequestsForApproval()
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetMergeRequestsForApproval());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Sub cases by Case Id 
        [HttpGet("GetSubCasesByCaseId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2022-11-12' Version="1.0" Branch="master"> Get Sub  Cases by Case Id</History>
        public async Task<IActionResult> GetSubCasesByCaseId(Guid caseId)
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetSubCasesByCaseId(caseId));
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }
        #endregion

        #region Get Judgment Decision 
        [HttpGet("GetJudgmentDecision")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2023-03-31' Version="1.0" Branch="master">Get Judgment Decision </History>
        public async Task<IActionResult> GetJudgmentDecision(Guid caseId)
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetJudgmentDecision(caseId));
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }
        #endregion

        #region Get Registered Cases 

        [HttpGet("GetAllRegisteredCases")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Danish' Date='2022-11-29' Version="1.0" Branch="master"> Get All Registered Cases</History>
        public async Task<IActionResult> GetAllRegisteredCases()
        {
            try
            {
                var response = (await _iCmsRegisteredCase.GetAllRegisteredCases());
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Add Judgement

        [HttpPost("AddJudgement")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-11-22' Version="1.0" Branch="master"> Handle Add Judgement Request</History>
        public async Task<IActionResult> AddJudgement(Judgement judgement)
        {
            try
            {
                await _iCmsRegisteredCase.AddJudgement(judgement);
                #region Task & Notification
                //var taskId = Guid.NewGuid();
                //var taskResult = await _ITask.AddTask(new SaveTaskVM
                //{
                //    Task = new UserTask
                //    {
                //        TaskId = taskId,
                //        Name = "Add_Judgement",
                //        Date = DateTime.Now.Date,
                //        AssignedBy = await _IAccount.UserIdByUserEmail(judgement.CreatedBy),
                //        AssignedTo = await _IAccount.UserIdByUserEmail(judgement.CreatedBy),
                //        IsSystemGenerated = true,
                //        TaskStatusId = (int)TaskStatusEnum.Done,
                //        ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                //        SectorId = (int)judgement.SectorTypeId,
                //        DepartmentId = (int)DepartmentEnum.Operational,
                //        TypeId = (int)TaskTypeEnum.Transfer,
                //        RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                //        CreatedBy = judgement.CreatedBy,
                //        CreatedDate = DateTime.Now,
                //        IsDeleted = false,
                //        ReferenceId = judgement.Id,
                //        SubModuleId = (int)SubModuleEnum.RegisteredCase,
                //        SystemGenTypeId = null,
                //        ModifiedDate = DateTime.Now,
                //        ModifiedBy = judgement.CreatedBy,
                //    },
                //    TaskActions = new List<TaskAction>()
                //    {
                //        new TaskAction()
                //        {
                //            ActionName = "Add Jdgement",
                //            TaskId = taskId,
                //        }
                //    }
                //},
                //" ",
                //" ",
                //" ");

                //if (judgement.IsFinal = true)
                //{
                //    var notificationResult = await _iNotifications.SendNotification(new Notification
                //    {
                //        NotificationId = Guid.NewGuid(),
                //        DueDate = DateTime.Now.AddDays(5),
                //        CreatedBy = judgement.CreatedBy,
                //        CreatedDate = DateTime.Now,
                //        IsDeleted = false,
                //        ReceiverId = await _IAccount.UserIdByUserEmail(judgement.CreatedBy),
                //        ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                //    },
                //    (int)NotificationEventEnum.AddJudgement,
                //    "view",
                //    "case",
                //    judgement?.CaseId.ToString(),
                //    judgement.NotificationParameter);
                //}
                #endregion
                //Rabbit MQ send Messages
                _client.SendMessage(judgement, RabbitMQKeys.JudgementKey);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Add Judgement",
                    Task = "To Add Judgement for a Case",
                    Description = "To Add Judgement for a Case",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Judgement added.",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok();
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Add Judgement",
                    Body = ex.Message,
                    Category = "User unable to add the Judgement for a case",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "User unable to add the Judgement for a case",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Postpone Hearing Request

        [HttpPost("AddPostponeHearingRequest")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-11-22' Version="1.0" Branch="master"> Handle Add Postpone Hearing Request</History>
        public async Task<IActionResult> AddPostponeHearingRequest(PostponeHearing postponeHearing)
        {
            try
            {
                await _iCmsRegisteredCase.AddPostponeHearingRequest(postponeHearing);

                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Add Postpone Hearing Request",
                    Task = "To Add Postpone Hearing Request for a Case",
                    Description = "To Add Postpone Hearing Request for a Case",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Postpone Hearing Request added.",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                var taskId = Guid.NewGuid();
                var taskResult = await _ITask.AddTask(new SaveTaskVM
                {
                    Task = new UserTask
                    {
                        TaskId = taskId,
                        Name = "Postpone_Hearing",
                        Date = DateTime.Now.Date,
                        AssignedBy = await _IAccount.UserIdByUserEmail(postponeHearing.CreatedBy),
                        AssignedTo = await _IAccount.UserIdByUserEmail(postponeHearing.CreatedBy),
                        IsSystemGenerated = true,
                        TaskStatusId = (int)TaskStatusEnum.Done,
                        ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                        SectorId = (int)postponeHearing.SectorTypeId,
                        DepartmentId = (int)DepartmentEnum.Operational,
                        TypeId = (int)TaskTypeEnum.Transfer,
                        RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                        CreatedBy = postponeHearing.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReferenceId = postponeHearing.HearingId,
                        SubModuleId = (int)SubModuleEnum.RegisteredCase,
                        SystemGenTypeId = null,
                        ModifiedDate = DateTime.Now,
                        ModifiedBy = postponeHearing.CreatedBy,
                    },
                    TaskActions = new List<TaskAction>()
                    {
                        new TaskAction()
                        {
                            ActionName = "Postpone Hearing",
                            TaskId = taskId,
                        }
                    }
                },
                " ",
                " ",
                " ");
                return Ok();
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Add Postpone Hearing Request",
                    Body = ex.Message,
                    Category = "User unable to add Postpone Hearing Request for a case",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "User unable to add Postpone Hearing Request for a case",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Merged Cases By Case Id

        [HttpGet("GetMergedCasesbyCaseId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2022-12-12' Version="1.0" Branch="master">Get Merged Cases By Case Id </History>
        public async Task<IActionResult> GetMergedCasesbyCaseId(Guid caseId)
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetMergedCasesbyCaseId(caseId));
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }
        #endregion

        #region Get Requested Documents
        [HttpGet("GetRequestedDocuments")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2022-12-12' Version="1.0" Branch="master">Get Requested  Documents </History>
        public async Task<IActionResult> GetRequestedDocuments()
        {
            try
            {

                return Ok(await _iCmsRegisteredCase.GetRequestedDocuments());
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }
        #endregion

        #region Create Request For Document
        //< History Author = 'Danish' Date = '2022-12-09' Version = "1.0" Branch = "master" >Request For  Document</History>
        [HttpPost("CreateRequestForDocument")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CreateRequestForDocument(MojRequestForDocument item)
        {
            try
            {
                await _iCmsRegisteredCase.CreateRequestForDocument(item);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Create Sub Case Detail

        [HttpPost("CreateSubCase")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel Ur Rehman' Date='2022-10-21' Version="1.0" Branch="master"> Get Registered Case detail by Id</History>
        public async Task<IActionResult> CreateSubCase(CmsRegisteredCase cmsRegisteredCase, int SectorTypeId)
        {
            try
            {
                var result = await _iCmsRegisteredCase.CreateSubCase(cmsRegisteredCase);
                // To Inform About Subcase, to The Lawyer of Regional who initiate the Stop Execution Request from GE
                if (SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases && cmsRegisteredCase.LawyerId != null)
                {
                    // dependent notification module
                    var notificationResult = await _iNotifications.SendNotification(new Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        DueDate = DateTime.Now.AddDays(5),
                        CreatedBy = cmsRegisteredCase.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReceiverId = cmsRegisteredCase.LawyerId,// Assign To Lawyer Id
                        ModuleId = (int)WorkflowModuleEnum.CaseManagement
                    },
                    (int)NotificationEventEnum.AddSubCase,
                    "view",
                    "case",
                     cmsRegisteredCase.CaseId.ToString(),
                     cmsRegisteredCase.NotificationParameter);

                    //Rabbit MQ send Messages
                    _client.SendMessage(cmsRegisteredCase, RabbitMQKeys.SubCaseRegister);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Create Scheduling Court Vists
        //< History Author = 'Danish' Date = '2022-12-14' Version = "1.0" Branch = "master" >Create Scheduling Court Vists</History>
        [HttpPost("CreateSchedulingCourtVists")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CreateSchedulingCourtVists(SchedulingCourtVisits item)
        {
            try
            {
                await _iCmsRegisteredCase.CreateSchedulingCourtVists(item);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Link CANs

        //<History Author = 'Hassan Abbas' Date='2022-12-15' Version="1.0" Branch="master"> Link CANs</History>
        [HttpPost("LinkCANs")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> LinkCANs(LinkCANsVM linkCAN)
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
                await _iCmsRegisteredCase.LinkCANs(linkCAN);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Link CANs",
                    Task = "To Link CANs",
                    Description = "To Link CANs",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "CANs Linked.",
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
                    Subject = "To Link CANs",
                    Body = ex.Message,
                    Category = "User unable To Link CANs",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Unable To Link CANs",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }

        #endregion

        #region Save & Close Case Files

        [HttpPost("SaveAndCloseCaseFiles")]
        [MapToApiVersion("1.0")]

        //<History Author = 'ijaz Ahmad' Date='2022-12-22' Version="1.0" Branch="master"> Save&Close Case Files</History>
        public async Task<IActionResult> SaveAndCloseCaseFiles(CmsSaveCloseCaseFile saveCloseCaseFile)
        {
            try
            {
                await _iCmsRegisteredCase.SaveAndCloseCaseFiles(saveCloseCaseFile);

                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Save & Close Case Files ",
                    Task = "To Save & Close  Case Files",
                    Description = "To Save & Close  Case Files",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Save&Close Case Files.",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok();
            }
            catch (Exception ex)
            {

                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Save&Close Case Files",
                    Body = ex.Message,
                    Category = "User unable to Save&Close Case Files",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "User unable to Save&Close Case Files",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Schedule Court Visit by Id        
        [HttpGet("GetSchedulCourtVisitByHearingId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Danish' Date='2022-12-26' Version="1.0" Branch="master">Get Schedule Court Visit by Id</History>        
        public async Task<IActionResult> GetSchedulCourtVisitByHearingId(Guid HearingId)
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetSchedulCourtVisitByHearingId(HearingId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Judgment Execution

        [HttpGet("GetJudgmentExecutions")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Danish' Date='2022-11-29' Version="1.0" Branch="master">Get Judgment Execution</History>
        public async Task<IActionResult> GetJudgmentExecutions(Guid caseId)
        {
            try
            {
                var response = (await _iCmsRegisteredCase.GetJudgmentExecutions(caseId));
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Add Judgment Execution
        //< History Author = 'Danish' Date = '2022-12-14' Version = "1.0" Branch = "master" >Add Judgment Execution</History>
        [HttpPost("AddJudgmentExecutions")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddJudgmentExecution(CmsJudgmentExecution cmsJudgmentExecution)
        {
            try
            {
                await _iCmsRegisteredCase.AddJudgmentExecution(cmsJudgmentExecution);
                if (cmsJudgmentExecution.IsUpdated == false)
                {
                    if (cmsJudgmentExecution.ExecutionRequestId != null && cmsJudgmentExecution.ExecutionRequestId != Guid.Empty)
                    {
                        User assignedTo = await _IRole.GetHOSBySectorId((int)OperatingSectorTypeEnum.Execution);

                        CmsApprovalTracking approvalTracking = new CmsApprovalTracking
                        {
                            Id = new Guid(),
                            ReferenceId = cmsJudgmentExecution.ExecutionRequest.Id,
                            SectorTo = (int)OperatingSectorTypeEnum.Execution,
                            SectorFrom = cmsJudgmentExecution.ExecutionRequest.SectorTypeId != null ? (int)cmsJudgmentExecution.ExecutionRequest.SectorTypeId : 0,
                            ProcessTypeId = (int)ApprovalProcessTypeEnum.ExecutionRequest,
                            StatusId = (int)ApprovalStatusEnum.Pending,
                            CreatedDate = DateTime.Now,
                            CreatedBy = cmsJudgmentExecution.ExecutionRequest.ModifiedBy
                        };

                        await _ICmsShared.SaveApprovalTrackingProcess(approvalTracking);

                        var taskId = Guid.NewGuid();
                        var taskResult = await _ITask.AddTask(new SaveTaskVM
                        {
                            Task = new UserTask
                            {
                                TaskId = taskId,
                                Name = "Execution_File_Details_Added_By_MOJ",
                                Date = DateTime.Now.Date,
                                AssignedBy = await _IAccount.UserIdByUserEmail(cmsJudgmentExecution.ExecutionRequest.ModifiedBy),
                                AssignedTo = assignedTo?.Id,
                                IsSystemGenerated = true,
                                TaskStatusId = (int)TaskStatusEnum.Pending,
                                ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                                SectorId = (int)OperatingSectorTypeEnum.Execution,
                                DepartmentId = (int)DepartmentEnum.Operational,
                                TypeId = (int)TaskTypeEnum.Transfer,
                                RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                                CreatedBy = cmsJudgmentExecution.ExecutionRequest.ModifiedBy,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReferenceId = cmsJudgmentExecution.ExecutionRequest.Id,
                                SubModuleId = (int)SubModuleEnum.RegisteredCase,
                                SystemGenTypeId = (int)TaskSystemGenTypeEnum.ExecutionRequest,
                            },
                            TaskActions = new List<TaskAction>()
                            {
                                new TaskAction()
                                {
                                    ActionName = "Execution Request Action",
                                    TaskId = taskId,
                                }
                            }
                        },
                        "detail",
                        "executionrequest",
                        cmsJudgmentExecution.ExecutionRequest.Id.ToString());


                        var notificationResult = await _iNotifications.SendNotification(new Notification
                        {
                            NotificationId = Guid.NewGuid(),
                            DueDate = DateTime.Now.AddDays(5),
                            CreatedBy = cmsJudgmentExecution.ExecutionRequest.ModifiedBy,
                            CreatedDate = DateTime.Now,
                            IsDeleted = false,
                            ReceiverId = assignedTo.Id,
                            ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                        },
                        (int)NotificationEventEnum.AddJudgmentExecution,
                        "detail",
                        "executionrequest",
                        cmsJudgmentExecution.ExecutionRequest.Id.ToString(),
                        cmsJudgmentExecution.NotificationParameter);
                    }
                }
                else if (cmsJudgmentExecution.CaseId != null && cmsJudgmentExecution.CaseId != Guid.Empty)
                {
                    CmsRegisteredCaseDetailVM caseDetails = await _iCmsRegisteredCase.GetRegisteredCaseDetailByIdVM((Guid)cmsJudgmentExecution.CaseId, cmsJudgmentExecution.CreatedBy);
                    if (caseDetails != null)
                    {
                        User reciever = _IAccount.GetUserByUserEmail(caseDetails.CreatedBy);
                        User assignedTo = await _IRole.GetHOSBySectorId(reciever.SectorTypeId != null ? (int)reciever.SectorTypeId : 0);
                        if (assignedTo != null)
                        {
                            var notificationResult = await _iNotifications.SendNotification(new Notification
                            {
                                NotificationId = Guid.NewGuid(),
                                DueDate = DateTime.Now.AddDays(5),
                                CreatedBy = cmsJudgmentExecution.CreatedBy,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReceiverId = assignedTo.Id,
                                ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                            },
                            (int)NotificationEventEnum.AddJudgmentExecution,
                            "view",
                            "case",
                            Convert.ToString(caseDetails.CaseId),
                            cmsJudgmentExecution.NotificationParameter);
                        }
                    }
                }
                //Rabbit MQ send Messages
                _client.SendMessage(cmsJudgmentExecution, RabbitMQKeys.JudgementExecutionKey);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Add Judgment Execution
        //< History Author = 'Hassan Abbas' Date = '2023-04-05' Version = "1.0" Branch = "master" >Add Execution Request</History>
        [HttpPost("AddExecutionRequest")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddExecutionRequest(MojExecutionRequest executionRequest)
        {
            try
            {
                Guid taskId = Guid.Empty;
                User assignedTo = new User();

                await _iCmsRegisteredCase.AddExecutionRequest(executionRequest);

                CmsApprovalTracking approvalTracking = new CmsApprovalTracking
                {
                    Id = new Guid(),
                    ReferenceId = executionRequest.Id,
                    SectorTo = executionRequest.SectorTypeId != null ? (int)executionRequest.SectorTypeId : 0,
                    SectorFrom = executionRequest.SectorTypeId != null ? (int)executionRequest.SectorTypeId : 0,
                    ProcessTypeId = (int)ApprovalProcessTypeEnum.ExecutionRequest,
                    StatusId = (int)ApprovalStatusEnum.Pending,
                    CreatedDate = DateTime.Now,
                    CreatedBy = executionRequest.CreatedBy
                };

                await _ICmsShared.SaveApprovalTrackingProcess(approvalTracking);

                await _ITask.AddTaskAndNotificationForHOSAndViceHOSOfSector(new SaveTaskVM
                {
                    Task = new UserTask
                    {
                        Name = "Execution_Request_Created_For_Review",
                        Date = DateTime.Now.Date,
                        AssignedBy = await _IAccount.UserIdByUserEmail(executionRequest.CreatedBy),
                        IsSystemGenerated = true,
                        TaskStatusId = (int)TaskStatusEnum.Pending,
                        ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                        SectorId = executionRequest.SectorTypeId,
                        DepartmentId = (int)DepartmentEnum.Operational,
                        TypeId = (int)TaskTypeEnum.Transfer,
                        RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                        CreatedBy = executionRequest.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReferenceId = executionRequest.Id,
                        SubModuleId = (int)SubModuleEnum.RegisteredCase,
                        SystemGenTypeId = (int)TaskSystemGenTypeEnum.ExecutionRequest,
                    },
                    Action = "detail",
                    EntityName = "executionrequest",
                    EntityId = executionRequest.Id.ToString()
                },
                new Notification
                {
                    DueDate = DateTime.Now.AddDays(5),
                    CreatedBy = executionRequest.CreatedBy,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                    EventId = (int)NotificationEventEnum.CreateExecutionRequest,
                    Action = "detail",
                    EntityName = "executionrequest",
                    EntityId = executionRequest.Id.ToString(),
                    NotificationParameter = executionRequest.NotificationParameter
                },
                executionRequest.SectorTypeId,
                true,//Send True if required to Check Vice HOS Responsibility for All or for only Manager, send False if need to send Tasks/Notifications for All Vice HOS
                true,//Send True if need to Include HOS as well along Vice HOSs
                0//Send Chamber Number Id if need to send Tasks for Vice HOSs of specific Chamber Number
                );

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get Execution Request By Id
        //< History Author = 'Hassan Abbas' Date = '2023-04-05' Version = "1.0" Branch = "master" >Get Execution Request By Id</History>
        [HttpGet("GetExecutionRequestById")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> GetExecutionRequestById(Guid ExecutionId)
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetExecutionRequestById(ExecutionId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Execution By Id
        //< History Author = 'Danish' Date = '2022-12-14' Version = "1.0" Branch = "master" >Get Execution By Id</History>
        [HttpGet("GetExecutionById")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> GetExecutionById(Guid ExecutionId)
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetExecutionById(ExecutionId));

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Execution By Id
        //< History Author = 'Hassan Abbas' Date = '2023-04-06' Version = "1.0" Branch = "master" >Get Judgement Execution Request By Id</History>
        [HttpGet("GetJudgementExecutionByExecutionRequestId")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> GetJudgementExecutionByExecutionRequestId(Guid ExecutionRequestId)
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetJudgementExecutionByExecutionRequestId(ExecutionRequestId));

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Edit Judgment Execution
        //< History Author = 'Danish' Date = '2022-12-14' Version = "1.0" Branch = "master" >Edit Judgment Execution</History>
        [HttpPost("EditJudgmentExecution")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> EditJudgmentExecution(CmsJudgmentExecution cmsJudgmentExecution)
        {
            try
            {
                await _iCmsRegisteredCase.EditJudgmentExecution(cmsJudgmentExecution);

                await TaskUpdate((Guid)cmsJudgmentExecution.Id, cmsJudgmentExecution.TaskUserId);
                var notificationResult = await _iNotifications.SendNotification(new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    DueDate = DateTime.Now.AddDays(5),
                    CreatedBy = cmsJudgmentExecution.ModifiedBy,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    ReceiverId = await _IAccount.UserIdByUserEmail(cmsJudgmentExecution.LawyerUserName),
                    ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                },
                (int)NotificationEventEnum.AddJudgmentExecution,
                "view",
                "case",
                 cmsJudgmentExecution.CaseId.ToString(),
                 cmsJudgmentExecution.NotificationParameter);

                //Rabbit MQ send Messages
                _client.SendMessage(cmsJudgmentExecution, RabbitMQKeys.JudgementExecutionKey);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Request For Document By Id
        //< History Author = 'Hassan Abbas' Date = '2023-03-24' Version = "1.0" Branch = "master" >Get Request For Document By Id</History>
        [HttpGet("GetRequestForDocumentById")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> GetRequestForDocumentById(Guid Id)
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetRequestForDocumentById(Id));

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Update Document Portfolio Request
        //< History Author = 'Hassan Abbas' Date = '2023-03-24' Version = "1.0" Branch = "master" >Update Request For Document Portfolio</History>
        [HttpGet("UpdateDocumentPortfolioRequest")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> UpdateDocumentPortfolioRequest(Guid Id)
        {
            try
            {
                await _iCmsRegisteredCase.UpdateDocumentPortfolioRequest(Id);
                return Ok();

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Add Judgement Decision
        [HttpPost("AddJudgementDecision")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddJudgementDecision(CmsCaseDecision cmsCaseDecision)
        {
            try
            {

                CmsCaseDecision caseDecision = await _iCmsRegisteredCase.AddJudgementDecision(cmsCaseDecision);
                User RescverId = await _IRole.GetHOSBySectorId((int)cmsCaseDecision.SectorTypeId);
                if (cmsCaseDecision.DecisionTypeId == (int)CaseDecisionTypeEnum.StopJudgementExecution)
                {
                    //notificationkey = "Stop_Judgment_Execution_Added";
                    //    var notificationResponse = await _iNotifications.SendNotification(new Notification
                    //    {
                    //        NotificationId = Guid.NewGuid(),
                    //        DueDate = DateTime.Now.AddDays(5),
                    //        CreatedBy = RescverId.CreatedBy,
                    //        CreatedDate = DateTime.Now,
                    //        IsDeleted = false,
                    //        ReceiverId = RescverId.Id,// Assign To  Id
                    //        ReceiverTypeId = (int)NotificationReceiverTypeEnum.User,
                    //        ModuleId = (int)WorkflowModuleEnum.Communication,
                    //        NotificationTypeId = (int)NotificationTypeEnum.Asynchronous,
                    //        NotificationCommunicationMethodId = (int)NotificationChannelEnum.Browser,
                    //        NotificationLinkId = Guid.NewGuid(),
                    //        NotificationEventId = (int)NotificationEventEnum.SubmitRequest,
                    //        NotificationCategoryId = (int)NotificationCategoryEnum.Important
                    //    },
                    //  notificationkey,
                    //  "detail",
                    //  "judgmentdecision",
                    //Convert.ToString(cmsCaseDecision.Id));
                }

                if (cmsCaseDecision.DecisionTypeId == (int)CaseDecisionTypeEnum.RegenerationOfJudgmentExecution)
                {
                    //notificationkey = "Regeneration_Decision_Judgment_Added";
                    //    var notificationResponse = await _iNotifications.SendNotification(new Notification
                    //    {
                    //        NotificationId = Guid.NewGuid(),
                    //        DueDate = DateTime.Now.AddDays(5),
                    //        CreatedBy = RescverId.CreatedBy,
                    //        CreatedDate = DateTime.Now,
                    //        IsDeleted = false,
                    //        ReceiverId = RescverId.Id,// Assign To  Id
                    //        ReceiverTypeId = (int)NotificationReceiverTypeEnum.User,
                    //        ModuleId = (int)WorkflowModuleEnum.Communication,
                    //        NotificationTypeId = (int)NotificationTypeEnum.Asynchronous,
                    //        NotificationCommunicationMethodId = (int)NotificationChannelEnum.Browser,
                    //        NotificationLinkId = Guid.NewGuid(),
                    //        NotificationEventId = (int)NotificationEventEnum.SubmitRequest,
                    //        NotificationCategoryId = (int)NotificationCategoryEnum.Important
                    //    },
                    //  notificationkey,
                    //  "detail",
                    //  "judgmentdecision",
                    //Convert.ToString(cmsCaseDecision.Id));
                }
                if (cmsCaseDecision.DecisionTypeId == (int)CaseDecisionTypeEnum.InvalidityofJudgementExecution)
                {
                    CaseAssignment user = await _ICmsCaseFile.GetPrimaryCaseAssignmentByCaseId(cmsCaseDecision.CaseId);
                    //notificationkey = "Invalidatiy_Of_Judgment_Decision_Judgment_Added";
                    //    var notificationResponse = await _iNotifications.SendNotification(new Notification
                    //    {
                    //        NotificationId = Guid.NewGuid(),
                    //        DueDate = DateTime.Now.AddDays(5),
                    //        CreatedBy = RescverId.CreatedBy,
                    //        CreatedDate = DateTime.Now,
                    //        IsDeleted = false,
                    //        ReceiverId = user.LawyerId,// Assign To  Id
                    //        ReceiverTypeId = (int)NotificationReceiverTypeEnum.User,
                    //        ModuleId = (int)WorkflowModuleEnum.Communication,
                    //        NotificationTypeId = (int)NotificationTypeEnum.Asynchronous,
                    //        NotificationCommunicationMethodId = (int)NotificationChannelEnum.Browser,
                    //        NotificationLinkId = Guid.NewGuid(),
                    //        NotificationEventId = (int)NotificationEventEnum.SubmitRequest,
                    //        NotificationCategoryId = (int)NotificationCategoryEnum.Important
                    //    },
                    //  notificationkey,
                    //  "detail",
                    //  "judgmentdecision",
                    //Convert.ToString(cmsCaseDecision.Id));
                }
                if (cmsCaseDecision.DecisionTypeId == (int)CaseDecisionTypeEnum.InterpretationofJudgement)
                {
                    CaseAssignment user = await _ICmsCaseFile.GetPrimaryCaseAssignmentByCaseId(cmsCaseDecision.CaseId);
                    //notificationkey = "Interpretation_Of_Judgment_Decision_Judgment_Added";
                    //    var notificationResponse = await _iNotifications.SendNotification(new Notification
                    //    {
                    //        NotificationId = Guid.NewGuid(),
                    //        DueDate = DateTime.Now.AddDays(5),
                    //        CreatedBy = RescverId.CreatedBy,
                    //        CreatedDate = DateTime.Now,
                    //        IsDeleted = false,
                    //        ReceiverId = user.LawyerId,// Assign To  Id
                    //        ReceiverTypeId = (int)NotificationReceiverTypeEnum.User,
                    //        ModuleId = (int)WorkflowModuleEnum.Communication,
                    //        NotificationTypeId = (int)NotificationTypeEnum.Asynchronous,
                    //        NotificationCommunicationMethodId = (int)NotificationChannelEnum.Browser,
                    //        NotificationLinkId = Guid.NewGuid(),
                    //        NotificationEventId = (int)NotificationEventEnum.SubmitRequest,
                    //        NotificationCategoryId = (int)NotificationCategoryEnum.Important
                    //    },
                    //  notificationkey,
                    //  "detail",
                    //  "judgmentdecision",
                    //Convert.ToString(cmsCaseDecision.Id));
                }
                return Ok(caseDecision);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Judgment Decision Detail by Id

        [HttpGet("GetJudgmentDecisionDetailbyId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2023-1-04' Version="1.0" Branch="master">Get Judgment Decision Detail by Id</History>
        public async Task<IActionResult> GetJudgmentDecisionDetailbyId(Guid decisionId)
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetJudgmentDecisionDetailbyId(decisionId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Approve/Reject Decision
        [HttpPost("RejectDecision")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> RejectDecision(CmsJugdmentDecisionVM cmsJugdmentDecisionVM)
        {
            bool result = false;
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }

                result = await _iCmsRegisteredCase.RejectDecision(cmsJugdmentDecisionVM);



                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpPost("ApproveDecision")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> ApproveDecision(CmsJugdmentDecisionVM cmsJugdmentDecisionVM)
        {
            bool result = false;
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }

                result = await _iCmsRegisteredCase.ApproveDecision(cmsJugdmentDecisionVM);
                User assignedTo = await _IRole.GetHOSBySectorId((int)OperatingSectorTypeEnum.Execution);
                //var notificationResponse = await _iNotifications.SendNotification(new Notification
                //{
                //    NotificationId = Guid.NewGuid(),
                //    DueDate = DateTime.Now.AddDays(5),
                //    CreatedBy = cmsJugdmentDecisionVM.ModifiedBy,
                //    CreatedDate = DateTime.Now,
                //    IsDeleted = false,
                //    ReceiverId = assignedTo.Id,// Assign To Sactor Id
                //    ReceiverTypeId = (int)NotificationReceiverTypeEnum.User,
                //    ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                //    NotificationTypeId = (int)NotificationTypeEnum.Asynchronous,
                //    NotificationCommunicationMethodId = (int)NotificationChannelEnum.Browser,
                //    NotificationLinkId = Guid.NewGuid(),
                //    NotificationEventId = (int)NotificationEventEnum.SubmitRequest,
                //    NotificationCategoryId = (int)NotificationCategoryEnum.Important
                //},
                //"Regeneration_Decision_Judgment_Added",
                //"detail",
                //  "judgmentdecision",
                //Convert.ToString(cmsJugdmentDecisionVM.Id));



                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Judgment Decision  List
        [HttpGet("GetJudgmentDecisionList")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2023-03-31' Version="1.0" Branch="master">Get Judgment Decision </History>
        public async Task<IActionResult> GetJudgmentDecisionList(Guid userId, int pageNumber, int pageSize)
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetJudgmentDecisionList(userId, pageNumber, pageSize));
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }
        #endregion

        #region Get Moj By SectorId
        [HttpGet("GetMojBySectorId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2023-03-31' Version="1.0" Branch="master">Get </History>
        public async Task<IActionResult> GetMojBySectorId(int userId)
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetMojBySectorId(userId));
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }
        #endregion

        #region Send Decision To Moj
        [HttpPost("SendDecisionToMoj")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SendDecisionToMoj(CmsJugdmentDecisionVM cmsJugdmentDecisionVM)
        {
            bool result = false;
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }

                result = await _iCmsRegisteredCase.SendDecisionToMoj(cmsJugdmentDecisionVM);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Send Decision Request To Moj",
                    Task = "To Send Decision Request To Moj",
                    Description = "To Send Decision Request To Moj",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Decision Request added.",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                User assignedTo = await _IRole.GetMojBySectorId((int)OperatingSectorTypeEnum.Execution);
                //var notificationResponse = await _iNotifications.SendNotification(new Notification
                //{
                //    NotificationId = Guid.NewGuid(),
                //    DueDate = DateTime.Now.AddDays(5),
                //    CreatedBy = cmsJugdmentDecisionVM.ModifiedBy,
                //    CreatedDate = DateTime.Now,
                //    IsDeleted = false,
                //    ReceiverId = assignedTo.Id,// Assign To Sactor Id
                //    ReceiverTypeId = (int)NotificationReceiverTypeEnum.User,
                //    ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                //    NotificationTypeId = (int)NotificationTypeEnum.Asynchronous,
                //    NotificationCommunicationMethodId = (int)NotificationChannelEnum.Browser,
                //    NotificationLinkId = Guid.NewGuid(),
                //    NotificationEventId = (int)NotificationEventEnum.SubmitRequest,
                //    NotificationCategoryId = (int)NotificationCategoryEnum.Important
                //},
                //"Regeneration_Decision_Judgment_Added",
                //"detail",
                //  "judgmentdecision",
                //Convert.ToString(cmsJugdmentDecisionVM.Id));


                return Ok(result);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Send Decision Request to MOJ",
                    Body = ex.Message,
                    Category = "User unable to Send Decision Request to MOJ",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "User unable to Send Decision Request to MOJ",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Registered Case Need More detail
        [HttpGet("GetRegisteredCaseNeedMoreDetail")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetRegisteredCaseNeedMoreDetail(Guid CaseId, Guid CommunicationId)
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
                var result = await _iCmsRegisteredCase.GetRegisteredCaseNeedMoreDetail(CaseId, CommunicationId);
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

        #region Add Document Portfolio Task 

        [HttpPost("GenrateDocumentPortfolioToDocumentTask")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GenrateDocumentPortfolioToDocumentTask(CmsDocumentPortfolio documentPortfolio)
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


                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Judgements By Case

        [HttpGet("GetRequestfordocumentbyCaseId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur rehman' Date='2022-10-21' Version="1.0" Branch="master"> Get Requestfordocument by CaseId</History>
        public async Task<IActionResult> GetRequestfordocumentbyCaseId(Guid caseId)
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetRequestfordocumentbyCaseId(caseId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
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

        #region Update Registered Case Chamber Number
        [HttpGet("UpdateRegisteredCaseChamberNumber")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> UpdateRegisteredCaseChamberNumber(CMSRegisteredCaseTransferHistoryVM cMSRegisteredCaseTransferHistoryVM)
        {
            try
            {
                await _iCmsRegisteredCase.UpdateRegisteredCaseChamberNumber(cMSRegisteredCaseTransferHistoryVM);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Update Chamber Number",
                    Task = "Update Registered Case Chamber Number",
                    Description = "Update Registered Case Chamber Number.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Chamber Number Updated",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok();
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Update Registered Case Chamber Number",
                    Body = ex.Message,
                    Category = "User unable To Update Chamber Number",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Unable To Update Chamber Number",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Judgement Detail By Judgement Id
        [HttpGet("GetJudgementDetailById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetJudgementDetailById(Guid judgementId)
        {
            try
            {
                var result = await _iCmsRegisteredCase.GetJudgementDetailById(judgementId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpGet("GetJudgementDetailByJudgementId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetJudgementDetailByJudgementId(Guid judgementId)
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetJudgementDetailByJudgementId(judgementId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Execution File Status
        [HttpGet("GetExecutionFileStatus")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Abuzar' Date='2023-12-05' Version="1.0" Branch="master"> Get Execution File Status</History>
        public async Task<IActionResult> GetExecutionFileStatus()
        {

            try
            {
                return Ok(await _iCmsRegisteredCase.GetExecutionFileStatus());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }
        #endregion

        #region Get case outcome  party history detail by Outcome Id
        [HttpGet("GetCMSCaseOutcomePartyHistoryDetailById")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2023-12-29' Version="1.0" Branch="master">Get case outcome  party history detail by Outcome Id </History>
        public async Task<IActionResult> GetCMSCaseOutcomePartyHistoryDetailById(string Id)
        {
            try
            {
                var result = await _iCmsRegisteredCase.GetCMSCaseOutcomePartyHistoryDetailById(Id);
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

        #region Get Outcome By Id
        [HttpGet("GetOutcomeByOutcomeId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetOutcomeByOutcomeId(Guid outcomeId)
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetOutcomeByOutcomeId(outcomeId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get RegisteredCases By Chamber Number Id
        [HttpGet("GetRegisteredCasesByChamberNumberId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetRegisteredCasesByChamberNumberId(int chamberNumberId)
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetRegisteredCasesByChamberNumberId(chamberNumberId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        [HttpPost("SaveImportantCase")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SaveImportantCase(CaseUserImportant ImportantCase)
        {
            try
            {
                await _iCmsRegisteredCase.SaveImportantCase(ImportantCase);



                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("DeleteImportantCase")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> DeleteImportantCase(CaseUserImportant ImportantCase)
        {
            try
            {
                await _iCmsRegisteredCase.DeleteImportantCase(ImportantCase);



                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #region Mobile App End Point
        [HttpGet("GetHearingListForMobileApp")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetHearingListForMobileApp([Required] string userId)
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
                var result = await _iCmsRegisteredCase.GetHearingListForMobileApp(userId);
                if (result != null && result.Count > 0)
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
        [HttpGet("GetHearingDetailsForMobileApp")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetHearingDetailsForMobileApp([Required] string hearingId)
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
                var result = await _iCmsRegisteredCase.GetHearingDetailsForMobileApp(hearingId);
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
        #region CMS Register Case Transfer Request
        [HttpGet("GetRegisterdCaseTransferRequestList")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Noman Khan' Date='2024-21-08' Version="1.0" Branch="master"> Get Judgements By Outcome</History>
        public async Task<IActionResult> GetRegisterdCaseTransferRequestList(Guid outcomeId)
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetRegisterdCaseTransferRequestList(outcomeId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpGet("GetResgisteredCaseTansferRequestDetailById")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Noman Khan' Date='2024-21-08' Version="1.0" Branch="master"> Get Case Party detail By Id</History>
        public async Task<IActionResult> GetResgisteredCaseTansferRequestDetailById(Guid ReferenceId)
        {
            try
            {
                var result = await _iCmsRegisteredCase.GetResgisteredCaseTansferRequestDetailById(ReferenceId);
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
        [HttpPost("RejectRegisteredCaseTransferRequest")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> RejectRegisteredCaseTransferRequest(CmsRegisteredCaseTransferRequestVM cmsCaseFileTransferRequestDetailVM)
        {
            try
            {
                await _iCmsRegisteredCase.RejectRegisteredCaseTransferRequest(cmsCaseFileTransferRequestDetailVM);
                NotificationParameter notificationParameter = new NotificationParameter();
                notificationParameter.Entity = new CmsRegisteredCaseTransferRequest().GetType().Name;
                var assignedTo = await _IAccount.UserIdByUserEmail(cmsCaseFileTransferRequestDetailVM.CreatedBy.ToString());
                var notificationResponse = await _iNotifications.SendNotification(new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    DueDate = DateTime.Now.AddDays(5),
                    CreatedBy = cmsCaseFileTransferRequestDetailVM.CreatedBy,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    ReceiverId = assignedTo,// Assign To Initiator Id
                    ModuleId = (int)WorkflowModuleEnum.CaseManagement
                },
                (int)NotificationEventEnum.RejectToAcceptCaseTransferRequest,
                "view",
                "casetransferrequest",
                cmsCaseFileTransferRequestDetailVM.Id.ToString(),
                notificationParameter);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Reject Registered Case Request For Transfer",
                    Task = "To Reject Registered Case Request For Transfer",
                    Description = "To Reject Registered Case Request For Transfer.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "RejectRegistered Case  Request For Transfer",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok(cmsCaseFileTransferRequestDetailVM);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "To Reject Registered Case Request For Transfer",
                    Body = ex.Message,
                    Category = "User unable To Reject Registered Case Request For Transfer",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Unable To Reject Registered Case Request For Transfer",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")

                });
                return BadRequest(new BadRequestResponse
                {
                    Message = ex?.Message,
                    InnerException = ex?.InnerException?.Message
                });
            }
        }
        [HttpPost("ApproveRegisteredCaseTransferRequest")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> ApproveRegisteredCaseTransferRequest(CmsRegisteredCaseTransferRequestVM cmsCaseFileTransferRequestDetailVM)
        {
            try
            {
                await _iCmsRegisteredCase.ApproveRegisteredCaseTransferRequest(cmsCaseFileTransferRequestDetailVM);
                NotificationParameter notificationParameter = new NotificationParameter();
                notificationParameter.Entity = new CmsRegisteredCaseTransferRequest().GetType().Name;
                var assignedTo = await _IAccount.UserIdByUserEmail(cmsCaseFileTransferRequestDetailVM.CreatedBy.ToString());
                var notificationResponse = await _iNotifications.SendNotification(new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    DueDate = DateTime.Now.AddDays(5),
                    CreatedBy = cmsCaseFileTransferRequestDetailVM.CreatedBy,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    ReceiverId = assignedTo,// Assign To Initiator Id
                    ModuleId = (int)WorkflowModuleEnum.CaseManagement
                },
                (int)NotificationEventEnum.ApprovedCaseTransferRequest,
                "view",
                "casetransferrequest",
                cmsCaseFileTransferRequestDetailVM.Id.ToString(),
                notificationParameter);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Approved Registered Case Request For Transfer",
                    Task = "To Approve Registered Case Request For Transfer",
                    Description = "To Approve Registered Case Request For Transfer.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "RejectRegistered Case Request For Transfer",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok(cmsCaseFileTransferRequestDetailVM);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "To Approve Registered Case Request For Transfer",
                    Body = ex.Message,
                    Category = "User unable To Approve Registered Case Request For Transfer",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Unable To Approve Registered Case Request For Transfer",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")

                });
                return BadRequest(new BadRequestResponse
                {
                    Message = ex?.Message,
                    InnerException = ex?.InnerException?.Message
                });
            }
        }
        [HttpGet("SoftDeleteCaseTransferRequest")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Noman Khan' Date='2024-02-09' Version="1.0" Branch="master"> Get Case Party detail By Id</History>
        public async Task<IActionResult> SoftDeleteCaseTransferRequest(Guid outcomeId, string userName)
        {
            try
            {
                var result = await _iCmsRegisteredCase.SoftDeleteCaseTransferRequest(outcomeId, userName);
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


        #region Case Detail for Moj by Case Id

        [HttpGet("GetCaseDetailForMOJ")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Danish' Date='2024-09-10' Version="1.0" Branch="master">Case detail</History>
        public async Task<IActionResult> GetCaseDetailForMOJ(Guid caseId)
        {
            try
            {
                return Ok(await _iCmsRegisteredCase.GetCaseDetailForMOJ(caseId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
    }
}
