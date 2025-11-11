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
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_DOMAIN.Models.WorkflowModels;
using FATWA_INFRASTRUCTURE.Repository.CaseManagement;
using FirebaseAdmin.Messaging;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using MsgReader.Mime.Header;
using System.Reflection;
using System.Threading.Tasks;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_DOMAIN.Enums.UserEnum;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;
using static FATWA_INFRASTRUCTURE.Repository.MeetRepo.MeetingRepository;
using Notification = FATWA_DOMAIN.Models.Notifications.Notification;
using NotificationParameter = FATWA_DOMAIN.Models.Notifications.ViewModel.NotificationParameter;

namespace FATWA_API.Controllers.V1.CaseManagement
{
    //<!-- <History Author = 'Hassan Abbas' Date='2022-12-24' Version="1.0" Branch="master">Shared API Controller For Case Management</History> -->
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme + ",FatwaApiKey")]
    public class CmsSharedController : ControllerBase
    {
        #region Variable Declaration
        private readonly ICmsShared _ICmsShared;
        private readonly ICMSCaseRequest _ICMSCaseRequest;
        private readonly ICmsCaseFile _ICmsCaseFile;
        private readonly IAuditLog _auditLogs;
        private readonly ITask _ITask;
        private readonly IAccount _IAccount;
        private readonly IRole _IRole;
        private readonly IConfiguration _configuration;
        private readonly INotification _iNotifications;
        private readonly IMapper _mapper;
        private readonly RabbitMQClient _client;

        #endregion

        #region Constructor
        public CmsSharedController(ICmsShared iCmsShared, ICMSCaseRequest iCMSCaseRequest, ICmsCaseFile iCmsCaseFile, IAuditLog audit, ITask iTask, IAccount iAccount,
            IRole iRole, IConfiguration configuration, INotification iNotifications, IMapper mapper, RabbitMQClient client)
        {
            _ICmsShared = iCmsShared;
            _ICMSCaseRequest = iCMSCaseRequest;
            _ICmsCaseFile = iCmsCaseFile;
            _auditLogs = audit;
            _ITask = iTask;
            _IAccount = iAccount;
            _IRole = iRole;
            _configuration = configuration;
            _iNotifications = iNotifications;
            _mapper = mapper;
            _client = client;
        }

        #endregion

        #region Get Approval Tracking

        [HttpGet("GetApprovalTrackingProcess")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetApprovalTrackingProcess(Guid referenceId, int sectorTypeId, int processTypeId)
        {
            try
            {
                return Ok(await _ICmsShared.GetApprovalTrackingProcess(referenceId, sectorTypeId, processTypeId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }

        #endregion

        #region Get Case Request Transfer History
        [HttpGet("GetCMSTransferHistory")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2022-09-30' Version="1.0" Branch="master"> Get Case Request status history</History>
        public async Task<IActionResult> GetCMSCaseRequestTransferHistory(string RequestId)
        {
            try
            {
                var result = await _ICmsShared.GetCMSTransferHistory(RequestId);
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

        #region Transfer Sector

        [HttpPost("AddTransferSectorTask")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddTransferSectorTask(CmsApprovalTracking approvalTracking, int TransferCaseType)
        {
            try
            {
                await _ICmsShared.SaveApprovalTrackingProcess(approvalTracking);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }


        [HttpPost("AddTransferTaskToPartialUrgentSector")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddTransferTaskToPartialUrgentSector(CmsApprovalTracking approvalTracking, int TransferCaseType)
        {
            try
            {
                User assignedTo = await _IRole.GetHOSBySectorId(approvalTracking.SectorTo);
                User assignedFrom = await _IRole.GetHOSBySectorId(approvalTracking.SectorFrom);
                approvalTracking.CreatedBy = assignedFrom.Email;
                await _ICmsShared.SaveApprovalTrackingProcessForCivilPartialUrgentSector(approvalTracking, TransferCaseType);

                //Find HOS of Transfer Sector here to Create task for it

                var taskId = Guid.NewGuid();
                var taskResult = await _ITask.AddTask(new SaveTaskVM
                {
                    Task = new UserTask
                    {
                        TaskId = taskId,
                        Name = "Assignment_of_Sector_Task",
                        Description = approvalTracking.Remarks,
                        Date = DateTime.Now.Date,
                        AssignedBy = assignedFrom?.Id,
                        AssignedTo = assignedTo?.Id,
                        IsSystemGenerated = true,
                        TaskStatusId = (int)TaskStatusEnum.Pending,
                        ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                        SectorId = approvalTracking.SectorTo,
                        DepartmentId = (int)DepartmentEnum.Operational,
                        TypeId = (int)TaskTypeEnum.Task,
                        RoleId = "d6b3075c-3f70-4b44-baa4-1fdc599a6bb2", //FATWA ADMIN
                        CreatedBy = approvalTracking.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReferenceId = approvalTracking.ReferenceId,
                        SubModuleId = (int)SubModuleEnum.CaseFile,
                        SystemGenTypeId = (int)TaskSystemGenTypeEnum.CaseFileAssignToSector,
                    },
                    TaskActions = new List<TaskAction>()
                    {
                        new TaskAction()
                        {
                            ActionName = "Transfer of Sector Action",
                            TaskId = taskId,
                        }
                    }
                },
                "view",
                TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? new CaseRequest().GetType().Name : new CaseFile().GetType().Name,
                approvalTracking.ReferenceId.ToString());
                // dependent notification module
                var notificationResponse = await _iNotifications.SendNotification(new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    DueDate = DateTime.Now.AddDays(5),
                    CreatedBy = approvalTracking.CreatedBy,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    ReceiverId = assignedTo?.Id,// Assign To Sactor Id
                    ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                },
                (int)NotificationEventEnum.AssigntoSector,
                "list",
                new UserTask().GetType().Name,
                assignedTo?.Id.ToString(),
                approvalTracking.NotificationParameter);

                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        #endregion

        #region Send Copy to Another Sector

        [HttpPost("AddSendACopyTask")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddSendACopyTask(CmsApprovalTracking approvalTracking, int TransferCaseType)
        {
            try
            {
                await _ICmsShared.SaveApprovalTrackingProcess(approvalTracking);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("RejectSendACopy")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> RejectSendACopy(dynamic Item, int TransferCaseType)
        {
            try
            {
                await _ICmsShared.RejectSendACopy(Item, TransferCaseType);

                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Reject Send A Copy Request",
                    Task = "To Reject Send A Copy Request",
                    Description = "To Reject Send A Copy Request.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Reject Send A Copy Request",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return Ok(Item);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "To Reject Send A Copy Request",
                    Body = ex.Message,
                    Category = "User unable To Reject Send A Copy Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Unable To Reject Send A Copy Request",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }

        #endregion

        #region Assign To Lawyer

        [HttpPost("AssignCaseToLawyer")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-04-22' Version="1.0" Branch="master"> Handle create document request</History>
        public async Task<IActionResult> AssignCaseToLawyer(CaseAssignment casefileAssignment)
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
                Guid? referenceId = await _ICmsShared.AssignCaseToLawyer(casefileAssignment);
                if (referenceId != null)
                {
                    if (casefileAssignment.AssignCaseToLawyerType == (int)AssignCaseToLawyerTypeEnum.CaseRequest)
                    {
                        await TaskUpdate((Guid)casefileAssignment.RequestId, casefileAssignment.TaskUserId);
                    }
                    if (casefileAssignment.AssignCaseToLawyerType == (int)AssignCaseToLawyerTypeEnum.RegisteredCase)
                    {
                        await TaskUpdate((Guid)casefileAssignment.ReferenceId, casefileAssignment.TaskUserId);
                    }
                    if (casefileAssignment.AssignCaseToLawyerType == (int)AssignCaseToLawyerTypeEnum.CaseFile)
                    {
                        await TaskUpdate((Guid)casefileAssignment.ReferenceId, casefileAssignment.TaskUserId);
                    }
                }

                string entityName;
                if ((int)AssignCaseToLawyerTypeEnum.CaseRequest == casefileAssignment.AssignCaseToLawyerType || (int)AssignCaseToLawyerTypeEnum.CaseFile == casefileAssignment.AssignCaseToLawyerType)
                {
                    entityName = new CaseFile().GetType().Name;
                    casefileAssignment.NotificationParameter.Entity = entityName;
                }
                else
                {
                    entityName = "Case";
                    casefileAssignment.NotificationParameter.Entity = entityName;
                    foreach (var additionalLawyers in casefileAssignment.SelectedUsers.Where(x => x.Id != (string)casefileAssignment.PrimaryLawyerId).ToList())
                    {
                        await CreateTaskAndNotificationForAdditionalLawyers(casefileAssignment, additionalLawyers, referenceId, entityName);

                    }
                }
                var taskId = Guid.NewGuid();
                var taskResult = await _ITask.AddTask(new SaveTaskVM
                {
                    Task = new UserTask
                    {
                        TaskId = taskId,
                        Name = "Request_Assigned_To_Lawyer_Task",
                        Date = DateTime.Now.Date,
                        AssignedBy = await _IAccount.UserIdByUserEmail(casefileAssignment.CreatedBy),
                        AssignedTo = (bool)casefileAssignment.SelectedUsers?.Any() ? casefileAssignment.PrimaryLawyerId : casefileAssignment.LawyerId,
                        IsSystemGenerated = true,
                        TaskStatusId = (int)TaskStatusEnum.Pending,
                        ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                        SectorId = (int)casefileAssignment.SectorTypeId,
                        DepartmentId = (int)DepartmentEnum.Operational,
                        TypeId = (int)TaskTypeEnum.Assignment,
                        RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                        CreatedBy = casefileAssignment.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReferenceId = referenceId,
                        SubModuleId = casefileAssignment.AssignCaseToLawyerType == (int)AssignCaseToLawyerTypeEnum.RegisteredCase ? (int)SubModuleEnum.RegisteredCase : (int)SubModuleEnum.CaseFile,
                        SystemGenTypeId = casefileAssignment.AssignCaseToLawyerType == (int)AssignCaseToLawyerTypeEnum.RegisteredCase ? (int)TaskSystemGenTypeEnum.RegisteredCaseAssignToLawyer : (int)TaskSystemGenTypeEnum.CaseFileAssignToLawyer,
                    },
                    TaskActions = new List<TaskAction>()
                    {
                        new TaskAction()
                        {
                            ActionName = "Assigned To Lawyer Action",
                            TaskId = taskId,
                        }
                    }
                },
                casefileAssignment.AssignCaseToLawyerType == (int)AssignCaseToLawyerTypeEnum.RegisteredCase ? "view" : "review-assignment",
                entityName,
                referenceId.ToString());

                var notificationResult = await _iNotifications.SendNotification(new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    DueDate = DateTime.Now.AddDays(5),
                    CreatedBy = casefileAssignment.CreatedBy,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    ReceiverId = (bool)casefileAssignment.SelectedUsers?.Any() ? casefileAssignment.PrimaryLawyerId : casefileAssignment.LawyerId,// Assign To Lawyer Id
                    ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                },
                (int)NotificationEventEnum.AssignToLawyer,
                casefileAssignment.AssignCaseToLawyerType == (int)AssignCaseToLawyerTypeEnum.RegisteredCase ? "view" : "review-assignment",
                entityName,
                referenceId.ToString() + "/" + taskId,
                casefileAssignment.NotificationParameter);
                casefileAssignment.ReferenceId = (Guid)referenceId;

                if (casefileAssignment.AssignCaseToLawyerType == (int)AssignCaseToLawyerTypeEnum.CaseRequest)
                {
                    var caseFile = await _ICmsCaseFile.CaseFileDetailWithPartiesAndAttachments((Guid)referenceId);
                    //Rabbit MQ send Messages
                    // Update Case Request Status
                    UpdateEntityStatusVM updateEntity = new UpdateEntityStatusVM();
                    updateEntity.ReferenceId = (Guid)casefileAssignment.RequestId;
                    updateEntity.StatusId = (int)CaseRequestStatusEnum.ConvertedToFile;
                    updateEntity.SubModuleId = (int)SubModuleEnum.CaseRequest;
                    _client.SendMessage(updateEntity, RabbitMQKeys.RequestStatusKey);
                    // CreateCaseFile
                    _client.SendMessage(caseFile, RabbitMQKeys.CreateCaseFileKey);
                }

                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "View the list of Case Requests",
                    Task = "To assign the request to a lawyer",
                    Description = "Request assigned to lawyer.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Request assigned to lawyer",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok(casefileAssignment);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "To assign the request to a lawyer",
                    Body = ex.Message,
                    Category = "User unable to assign the request to a lawyer",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Unable to assign the request",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Create Task For Additional Lawyers
        protected async Task CreateTaskAndNotificationForAdditionalLawyers(CaseAssignment casefileAssignment, LawyerVM additionalLawyer, Guid? referenceId, string entityName)
        {
            var taskGuid = Guid.NewGuid();
            var addTaskResult = await _ITask.AddTask(new SaveTaskVM
            {
                Task = new UserTask
                {
                    TaskId = taskGuid,
                    Name = "Request_Assigned_To_Lawyer_Task",
                    Date = DateTime.Now.Date,
                    AssignedBy = await _IAccount.UserIdByUserEmail(casefileAssignment.CreatedBy),
                    AssignedTo = additionalLawyer.Id,
                    IsSystemGenerated = true,
                    TaskStatusId = (int)TaskStatusEnum.Pending,
                    ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                    SectorId = (int)casefileAssignment.SectorTypeId,
                    DepartmentId = (int)DepartmentEnum.Operational,
                    TypeId = (int)TaskTypeEnum.Assignment,
                    RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                    CreatedBy = casefileAssignment.CreatedBy,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    ReferenceId = referenceId,
                    SubModuleId = (int)SubModuleEnum.RegisteredCase,
                    SystemGenTypeId = (int)TaskSystemGenTypeEnum.RegisteredCaseAssignToLawyer,
                },
                TaskActions = new List<TaskAction>()
                            {
                                new TaskAction()
                                {
                                    ActionName = "Assigned To Lawyer Action",
                                    TaskId = taskGuid,
                                }
                            }
            },
            "view",
            entityName,
            referenceId.ToString());

            var notificationResult = await _iNotifications.SendNotification(new Notification
            {
                NotificationId = Guid.NewGuid(),
                DueDate = DateTime.Now.AddDays(5),
                CreatedBy = casefileAssignment.CreatedBy,
                CreatedDate = DateTime.Now,
                IsDeleted = false,
                ReceiverId = additionalLawyer.Id,// Assign To Lawyer Id
                ModuleId = (int)WorkflowModuleEnum.CaseManagement,
            },
                (int)NotificationEventEnum.AssignToLawyer,
                "view",
                entityName,
                referenceId.ToString() + "/" + taskGuid,
                casefileAssignment.NotificationParameter);
        }
        #endregion

        #region Create Ge Representative

        [HttpPost("CreateGeRepresentative")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-02-27' Version="1.0" Branch="master"> Handle create ge represntative request</History>
        public async Task<IActionResult> CreateGeRepresentative(GovernmentEntityRepresentative geRepresentative)
        {
            try
            {
                bool isNew = false;
                if (geRepresentative.Id == null || geRepresentative.Id == Guid.Empty)
                {
                    isNew = true;
                }
                GovernmentEntityRepresentative geRep = await _ICmsShared.CreateGeRepresentative(geRepresentative);
                if (geRep != null && isNew)
                {
                    //Rabbit MQ send Messages
                    _client.SendMessage(geRep, RabbitMQKeys.GERepresentativeFATWAKey);
                }
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "To create Ge Representative",
                    Task = "To create Ge Representative",
                    Description = "GE Representative Added.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "GE Representative Added",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return Ok(geRep);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "To create Ge Representative",
                    Body = ex.Message,
                    Category = "User unable To create Ge Representative",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Unable To create Ge Representative",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Execution Request Approve/Reject

        //< History Author = 'Hassan Abbas' Date = '2023-04-05' Version = "1.0" Branch = "master" >Add Execution Request</History>
        [HttpPost("SendExecutionRequestToMOJExecution")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SendExecutionRequestToMOJExecution(MojExecutionRequest executionRequest)
        {
            try
            {
                string messengerId = await _ICmsShared.SendExecutionRequestToMOJExecution(executionRequest);
                await TaskUpdate((Guid)executionRequest.Id, executionRequest.TaskUserId);

                var taskId = Guid.NewGuid();
                var taskResult = await _ITask.AddTask(new SaveTaskVM
                {
                    Task = new UserTask
                    {
                        TaskId = taskId,
                        Name = "Request_Sent_For_Execution",
                        Date = DateTime.Now.Date,
                        AssignedBy = executionRequest.TaskUserId,
                        AssignedTo = messengerId,
                        IsSystemGenerated = true,
                        TaskStatusId = (int)TaskStatusEnum.Pending,
                        ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                        SectorId = (int)executionRequest.SectorTypeId,
                        DepartmentId = (int)DepartmentEnum.Operational,
                        TypeId = (int)TaskTypeEnum.Transfer,
                        RoleId = SystemRoles.FatwaAdmin,
                        CreatedBy = executionRequest.ModifiedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReferenceId = executionRequest.Id,
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
                executionRequest.Id.ToString());

                var notificationResult = await _iNotifications.SendNotification(new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    DueDate = DateTime.Now.AddDays(5),
                    CreatedBy = executionRequest.ModifiedBy,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    ReceiverId = messengerId,// Assign To Lawyer Id
                    ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                },
                (int)NotificationEventEnum.SendExecutionRequestToMojExecution,
                "detail",
                "executionrequest",
                 executionRequest.Id.ToString(),
                 executionRequest.NotificationParameter);
                //Rabbit MQ send Messages
                //_client.SendMessage(cmsJudgmentExecution, JudgementExecutionKey);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //< History Author = 'Hassan Abbas' Date = '2023-04-05' Version = "1.0" Branch = "master" >Add Execution Request</History>
        [HttpPost("ApproveExecutionRequest")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> ApproveExecutionRequest(MojExecutionRequest executionRequest)
        {
            try
            {
                await _ICmsShared.ApproveExecutionRequest(executionRequest);
                await TaskUpdate((Guid)executionRequest.Id, executionRequest.TaskUserId);
                User assignedTo = await _IRole.GetHOSBySectorId((int)OperatingSectorTypeEnum.Execution);

                CmsApprovalTracking approvalTracking = new CmsApprovalTracking
                {
                    Id = new Guid(),
                    ReferenceId = executionRequest.Id,
                    SectorTo = (int)OperatingSectorTypeEnum.Execution,
                    SectorFrom = executionRequest.SectorTypeId != null ? (int)executionRequest.SectorTypeId : 0,
                    ProcessTypeId = (int)ApprovalProcessTypeEnum.ExecutionRequest,
                    StatusId = (int)ApprovalStatusEnum.Pending,
                    CreatedDate = DateTime.Now,
                    CreatedBy = executionRequest.ModifiedBy
                };

                await _ICmsShared.SaveApprovalTrackingProcess(approvalTracking);

                var taskId = Guid.NewGuid();
                var taskResult = await _ITask.AddTask(new SaveTaskVM
                {
                    Task = new UserTask
                    {
                        TaskId = taskId,
                        Name = "Execution_Request_Created_For_Review",
                        Date = DateTime.Now.Date,
                        AssignedBy = await _IAccount.UserIdByUserEmail(executionRequest.CreatedBy),
                        AssignedTo = assignedTo?.Id,
                        IsSystemGenerated = true,
                        TaskStatusId = (int)TaskStatusEnum.Pending,
                        ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                        SectorId = (int)OperatingSectorTypeEnum.Execution,
                        DepartmentId = (int)DepartmentEnum.Operational,
                        TypeId = (int)TaskTypeEnum.Transfer,
                        RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                        CreatedBy = executionRequest.ModifiedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReferenceId = executionRequest.Id,
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
                executionRequest.Id.ToString());

                var notificationResult = await _iNotifications.SendNotification(new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    DueDate = DateTime.Now.AddDays(5),
                    CreatedBy = executionRequest.ModifiedBy,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    ReceiverId = assignedTo.Id,// Assign To Lawyer Id
                    ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                },
                (int)NotificationEventEnum.ApproveExecutionRequest,
                "detail",
                "executionrequest",
                 executionRequest.Id.ToString(),
                 executionRequest.NotificationParameter);

                //Rabbit MQ send Messages
                //_client.SendMessage(cmsJudgmentExecution, JudgementExecutionKey);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //< History Author = 'Hassan Abbas' Date = '2023-04-05' Version = "1.0" Branch = "master" >Add Execution Request</History>
        [HttpPost("RejectExecutionRequest")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> RejectExecutionRequest(MojExecutionRequest executionRequest)
        {
            try
            {
                await _ICmsShared.RejectExecutionRequest(executionRequest);
                await TaskUpdate((Guid)executionRequest.Id, executionRequest.TaskUserId);
                var notificationResult = await _iNotifications.SendNotification(new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    DueDate = DateTime.Now.AddDays(5),
                    CreatedBy = executionRequest.ModifiedBy,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    ReceiverId = await _IAccount.UserIdByUserEmail(executionRequest.CreatedBy),// Assign To Lawyer Id
                    ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                },
                (int)NotificationEventEnum.RejectExecutionRequest,
                "detail",
                "executionrequest",
                 executionRequest.Id.ToString(),
                 executionRequest.NotificationParameter);
                //Rabbit MQ send Messages
                //_client.SendMessage(cmsJudgmentExecution, JudgementExecutionKey);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Assign Decision Request To Lawyer

        [HttpPost("AssignDecisionRequestToLawyer")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-04-22' Version="1.0" Branch="master"> Handle create document request</History>
        public async Task<IActionResult> AssignDecisionRequestToLawyer(CmsCaseDecisionAssignee casedecisionAssignment)
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
                result = await _ICmsShared.AssignDecisionRequestToLawyer(casedecisionAssignment);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "View the list of Decision Requests",
                    Task = "To assign the request to a lawyer",
                    Description = "Request assigned to lawyer.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Request assigned to lawyer",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                //var notificationResult = await _iNotifications.SendNotification(new Notification
                //{
                //    NotificationId = Guid.NewGuid(),
                //    DueDate = DateTime.Now.AddDays(5),
                //    CreatedBy = casedecisionAssignment.CreatedBy,
                //    CreatedDate = DateTime.Now,
                //    IsDeleted = false,
                //    ReceiverId = Convert.ToString(casedecisionAssignment.UserId),// Assign To Lawyer Id
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
                //"judgmentdecision",
                //Convert.ToString(casedecisionAssignment.DecisionId));

                return Ok(result);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "To assign the request to a lawyer",
                    Body = ex.Message,
                    Category = "User unable to assign the request to a lawyer",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Unable to assign the request",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Assign Back To Hos

        [HttpPost("AssignCaseFileBackToHos")]
        [MapToApiVersion("1.0")]
        //<History Author = 'ijaz Ahmad' Date='2023-04-17' Version="1.0" Branch="master"> Assign Back to Hos</History>
        public async Task<IActionResult> AssignCaseFileBackToHos(CmsAssignCaseFileBackToHos cmsAssignBackToHos)
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

                await _ICmsCaseFile.AssignCaseFileBackToHos(cmsAssignBackToHos);
                //await _ICmsCaseFile.UpdatCaseFileaAssignedBackToHos(cmsAssignBackToHos.ReferenceId, true, cmsAssignBackToHos.CreatedBy);
                //Find HOS by Sector type Id
                User assignedTo = await _IRole.GetHOSBySectorId((int)cmsAssignBackToHos.SectorTypeId);

                string entityName = "CaseFile";
                var taskId = Guid.NewGuid();
                var taskResult = await _ITask.AddTask(new SaveTaskVM
                {
                    Task = new UserTask
                    {
                        TaskId = taskId,
                        Name = "Case_File_Rejected_By_Lawyer",
                        Date = DateTime.Now.Date,
                        AssignedBy = await _IAccount.UserIdByUserEmail(cmsAssignBackToHos.CreatedBy),
                        AssignedTo = assignedTo.Id,
                        IsSystemGenerated = true,
                        TaskStatusId = (int)TaskStatusEnum.Pending,
                        ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                        SectorId = (int)cmsAssignBackToHos.SectorTypeId,
                        DepartmentId = (int)DepartmentEnum.Operational,
                        TypeId = (int)TaskTypeEnum.Assignment,
                        RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                        CreatedBy = cmsAssignBackToHos.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReferenceId = cmsAssignBackToHos.ReferenceId,
                        SubModuleId = (int)SubModuleEnum.CaseFile,
                        SystemGenTypeId = (int)TaskSystemGenTypeEnum.CaseFileRejectTransfer,
                    },
                    TaskActions = new List<TaskAction>()
                    {
                        new TaskAction()
                        {
                            ActionName = "Case_File_Rejected_By_Lawyer",
                            TaskId = taskId,
                        }
                    }
                },
                "view",
                entityName,
                cmsAssignBackToHos.ReferenceId.ToString());
                var notificationResult = await _iNotifications.SendNotification(new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    DueDate = DateTime.Now.AddDays(5),
                    CreatedBy = cmsAssignBackToHos.CreatedBy,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    ReceiverId = assignedTo.Id,// Send  Id
                    ModuleId = (int)WorkflowModuleEnum.CaseManagement
                },
                (int)NotificationEventEnum.AssignBackToHos,
                "view",
                entityName,
                cmsAssignBackToHos.ReferenceId.ToString(),
                cmsAssignBackToHos.NotificationParameter);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Case File Assigned Back To HOS",
                    Task = "To assign the request to a HOS",
                    Description = "Request assigned to HOS.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Request assigned to HOS",
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
                    Subject = "Case File Assigned Back To HOS",
                    Body = ex.Message,
                    Category = "User unable to assign the request Back to a HOS",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Unable to assign the  request Back to a HOS",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Govt Entity By reference Id
        [HttpGet("GetGovtEnityByReferencId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2023-01-27' Version="1.0" Branch="master"> Get Govt Entity Id</History>
        public async Task<IActionResult> GetGovtEnityByReferencId(Guid ReferenceId, int SubModulId)
        {
            try
            {
                return Ok(await _ICmsShared.GetGovtEnityByReferencId(ReferenceId, SubModulId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }
        [HttpGet("GetSendBackToHosByReferenceId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2023-01-27' Version="1.0" Branch="master"> Get Govt Entity Id</History>
        public async Task<IActionResult> GetSendBackToHosByReferenceId(Guid ReferenceId, string LawyerId)
        {
            try
            {

                return Ok(await _ICmsShared.GetSendBackToHosByReferenceId(ReferenceId, LawyerId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }
        [HttpGet("GetCaseAssigmentByLawyerIdAndFileId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2023-01-27' Version="1.0" Branch="master"> Get Govt Entity Id</History>
        public async Task<IActionResult> GetCaseAssigmentByLawyerIdAndFileId(Guid FileId, string UserId)
        {
            try
            {

                return Ok(await _ICmsShared.GetCaseAssigmentByLawyerIdAndFileId(FileId, UserId));
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

        #region Approve case file

        [HttpPost("ApproveCaseFile")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> ApproveCaseFile(CmsCaseFileDetailVM item)
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
                var result = await _ICmsShared.ApproveCaseFile(item);
                if (result != null)
                {
                    item.NotificationParameter.Entity = new CaseFile().GetType().Name;
                    item.NotificationParameter.ReferenceNumber = item.FileNumber;
                    var LawyerIds = await _ICmsShared.GetCaseAssignmentListByReferenceId(item.FileId);
                    if (LawyerIds != null)
                    {
                        foreach (var AssignedTo in LawyerIds)
                        {
                            var taskId = Guid.NewGuid();
                            var taskResult = await _ITask.AddTask(new SaveTaskVM
                            {
                                Task = new UserTask
                                {
                                    TaskId = taskId,
                                    Name = "Request_Assigned_To_Lawyer_Task",
                                    Date = DateTime.Now.Date,
                                    AssignedBy = await _IAccount.UserIdByUserEmail(item.CreatedBy),
                                    AssignedTo = AssignedTo,
                                    IsSystemGenerated = true,
                                    TaskStatusId = (int)TaskStatusEnum.Pending,
                                    ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                                    SectorId = (int)item.SectorTypeId,
                                    DepartmentId = (int)DepartmentEnum.Operational,
                                    TypeId = (int)TaskTypeEnum.Assignment,
                                    RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                                    CreatedBy = item.CreatedBy,
                                    CreatedDate = DateTime.Now,
                                    IsDeleted = false,
                                    ReferenceId = item.FileId,
                                    SubModuleId = (int)SubModuleEnum.CaseFile,
                                    SystemGenTypeId = (int)TaskSystemGenTypeEnum.CaseFileAssignToLawyer,
                                },
                                TaskActions = new List<TaskAction>()
                                {
                                    new TaskAction()
                                    {
                                        ActionName = "Assigned To Lawyer Action",
                                        TaskId = taskId,
                                    }
                                }
                            },
                            "view",
                            new CaseFile().GetType().Name,
                            item.FileId.ToString());

                            var notificationResult = await _iNotifications.SendNotification(new Notification
                            {
                                NotificationId = Guid.NewGuid(),
                                DueDate = DateTime.Now.AddDays(5),
                                CreatedBy = item.CreatedBy,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReceiverId = AssignedTo,// Assign To Lawyer Id
                                ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                            },
                            (int)NotificationEventEnum.AssignToLawyer,
                            "view",
                            new CaseFile().GetType().Name,
                            item.FileId.ToString() + "/" + taskId,
                            item.NotificationParameter);
                        }
                    }
                    //Rabbit MQ send Messages
                    var mapObj = _mapper.Map<UpdateEntityHistoryVM>(result);
                    mapObj.ReferenceId = result.FileId;
                    mapObj.SubModuleId = (int)SubModuleEnum.CaseFile;
                    _client.SendMessage(mapObj, RabbitMQKeys.HistoryKey);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region GetCaseConsultationRequestList
        [HttpPost("GetCaseConsultationRequestList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCaseConsultationRequestList(AdvanceSearchConsultationRequestVM advanceSearchVM)
        {
            try
            {
                return Ok(await _ICmsShared.GetCaseConsultationRequestList(advanceSearchVM));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region get user id by user email
        [HttpGet("UserIdByUserEmail")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UserIdByUserEmail(string email)
        {
            try
            {
                return Ok(await _IAccount.UserIdByUserEmail(email));

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region UpdateTransferHistory
        [HttpPost("UpdateTransferHistory")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateTransferHistory(CmsTransferHistoryVM transferHistory)
        {
            try
            {
                await _ICMSCaseRequest.UpdateTransferHistory(transferHistory);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Update Tansfer History",
                    Task = "To Update The Transfer History",
                    Description = "To Update The Transfer History",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Update Tansfer History",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return Ok(transferHistory);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "To Update The Transfer History",
                    Body = ex.Message,
                    Category = "User unable To Update The Transfer History",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Unable To Update The Transfer History",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }
        #endregion

        #region Assign Case File To Sector
        [HttpPost("AddAssignSectorTask")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddAssignSectorTask(CmsApprovalTracking approvalTracking, int TransferCaseType)
        {
            try
            {
                await _ICmsShared.SaveApprovalTrackingProcessForAssign(approvalTracking);


                User assignedFrom = await _IRole.GetHOSBySectorId(approvalTracking.SectorFrom);
                //Find HOS of Transfer Sector here to Create task for it
                User assignedTo = await _IRole.GetHOSBySectorId(approvalTracking.SectorTo);

                // OLD CODE START
                string viewPage;
                viewPage = "transfer-review";
                var taskId = Guid.NewGuid();
                





                //NEW CODE START


                SaveTaskVM taskObj = null;
                Notification notificationObj = null;
                
                    taskObj = new SaveTaskVM
                    {
                        Task = new UserTask
                        {
                            TaskId = taskId,
                            Name = "Assignment_of_Sector_Task",
                            Description = approvalTracking.Remarks,
                            Date = DateTime.Now.Date,
                            AssignedBy = approvalTracking?.AssignedBy,
                            IsSystemGenerated = true,
                            TaskStatusId = (int)TaskStatusEnum.Pending,
                            ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                            SectorId = (int)approvalTracking.SectorTo,
                            DepartmentId = (int)DepartmentEnum.Operational,
                            TypeId = (int)TaskTypeEnum.Task,
                            RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                            CreatedBy = approvalTracking.CreatedBy,
                            CreatedDate = DateTime.Now,
                            IsDeleted = false,
                            ReferenceId = approvalTracking.ReferenceId,
                            SubModuleId = (int)SubModuleEnum.CaseFile,
                            SystemGenTypeId = (int)TaskSystemGenTypeEnum.CaseFileAssignToSector,
                        },
                        Action = viewPage,
                        EntityName = new CaseFile().GetType().Name,
                        EntityId = approvalTracking.ReferenceId.ToString()
                    };







                
                    notificationObj =
                        new Notification
                        {
                            NotificationId = Guid.NewGuid(),
                            DueDate = DateTime.Now.AddDays(5),
                            CreatedBy = approvalTracking.CreatedBy,
                            CreatedDate = DateTime.Now,
                            IsDeleted = false,
                            ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                            EventId = (int)NotificationEventEnum.TransferOfSector,
                            Action = viewPage,
                            EntityName = new CaseFile().GetType().Name,
                            EntityId = approvalTracking.ReferenceId.ToString(),
                            NotificationParameter = approvalTracking.NotificationParameter
                        };
                
                await _ITask.AddTaskAndNotificationForHOSAndViceHOSOfSector(taskObj, notificationObj, (int)approvalTracking.SectorTo,
                false,//Send True if required to Check Vice HOS Responsibility for All or for only Manager, send False if need to send Tasks/Notifications for All Vice HOS
                true,//Send True if need to Include HOS as well along Vice HOSs
                0//Send Chamber Number Id if need to send Tasks for Vice HOSs of specific Chamber Number
                );
                //NEW CODE END
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = " Assign To Sector",
                    Task = "To  Assign To The Sector Type",
                    Description = "To  Assign To The Sector Type.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = " Assign To The Sector Type",
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
                    Subject = "To  Assign To The Sector Type",
                    Body = ex.Message,
                    Category = "User unable To  Assign To The Sector Type",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Unable To  Assign To The Sector Type",
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
        [HttpPost("ApproveTransferSector")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> ApproveTransferSector(dynamic Item, int TransferCaseType)
        {
            try
            {
                var historyObject = await _ICmsShared.ApproveTransferSector(Item, TransferCaseType);
                CaseFile caseFiledetail = System.Text.Json.JsonSerializer.Deserialize<CaseFile>(Item);
                string viewPage;
                viewPage = "view";
                var taskId = Guid.NewGuid();
                SaveTaskVM taskObj = null;
                Notification notificationObj = null;
                var UserId = await _IAccount.UserIdByUserEmail(caseFiledetail.CreatedBy);
                taskObj = new SaveTaskVM
                {
                    Task = new UserTask
                    {
                        TaskId = taskId,
                        Name = "Assignment_of_Sector_Task",
                        Description = caseFiledetail.Remarks,
                        Date = DateTime.Now.Date,
                        AssignedBy = UserId == null ? caseFiledetail.LoggedInUserId : UserId,
                        IsSystemGenerated = true,
                        TaskStatusId = (int)TaskStatusEnum.Pending,
                        ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                        SectorId = (int)caseFiledetail.SectorTo,
                        DepartmentId = (int)DepartmentEnum.Operational,
                        TypeId = (int)TaskTypeEnum.Task,
                        RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                        CreatedBy = caseFiledetail.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReferenceId = caseFiledetail.FileId,
                        SubModuleId = (int)SubModuleEnum.CaseFile,
                        SystemGenTypeId = (int)TaskSystemGenTypeEnum.CaseFileAssignmentApproval,
                    },
                    Action = viewPage,
                    EntityName = new CaseFile().GetType().Name,
                    EntityId = caseFiledetail.FileId.ToString()
                };
                await _ITask.AddTaskAndNotificationForHOSAndViceHOSOfSector(taskObj, notificationObj, (int)caseFiledetail.SectorTo,
                false,//Send True if required to Check Vice HOS Responsibility for All or for only Manager, send False if need to send Tasks/Notifications for All Vice HOS
                true,//Send True if need to Include HOS as well along Vice HOSs
                0//Send Chamber Number Id if need to send Tasks for Vice HOSs of specific Chamber Number
                );
                if (TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseFile && historyObject != null)
                {
                    CaseFile caseFile = System.Text.Json.JsonSerializer.Deserialize<CaseFile>(Item);
                    //Rabbit MQ send Messages
                    // Add Case File Status History
                    var mapObj = _mapper.Map<UpdateEntityHistoryVM>(historyObject);
                    mapObj.ReferenceId = historyObject.FileId;
                    mapObj.SubModuleId = (int)SubModuleEnum.CaseFile;
                    _client.SendMessage(mapObj, RabbitMQKeys.HistoryKey);
                }
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Approve Assign To Sector",
                    Task = "To Approve Assign To The Sector Type",
                    Description = "To Approve Assign To The Sector Type.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Assign To The Sector Type",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok(Item);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "To Approve Assign To The Sector Type",
                    Body = ex.Message,
                    Category = "User unable To Approve Assign To The Sector Type",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Unable To Approve Assign To The Sector Type",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }

        [HttpPost("RejectTransferSector")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> RejectTransferSector(dynamic Item, int TransferCaseType)
        {
            try
            {
                await _ICmsShared.RejectTransferSector(Item, TransferCaseType);
                NotificationParameter notificationParameter = new NotificationParameter();
                CaseFile caseFile = null;
                int SectorFrom = 0;
                string userName = "", Remarks = "";
                if (TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseFile)
                {
                    caseFile = System.Text.Json.JsonSerializer.Deserialize<CaseFile>(Item);
                    SectorFrom = caseFile.SectorTypeId != null ? (int)caseFile.SectorTypeId : 0;
                    userName = caseFile.CreatedBy;
                    Remarks = caseFile.Remarks;
                    notificationParameter.FileNumber = caseFile.FileNumber;
                }
                //Find HOS of Transfer Sector here to Create task for it
                User assignedTo = await _IRole.GetHOSBySectorId((int)caseFile.SectorFrom);
                var notificationResponse = await _iNotifications.SendNotification(new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    DueDate = DateTime.Now.AddDays(5),
                    CreatedBy = caseFile.CreatedBy,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    ReceiverId = assignedTo?.Id,// Assign To Sactor Id
                    ModuleId = (int)WorkflowModuleEnum.CaseManagement
                },
                (int)NotificationEventEnum.RejectToAcceptAssignFile,
                "view",
                new CaseFile().GetType().Name,
                caseFile.FileId.ToString(),
                notificationParameter);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Reject Assign To Sector",
                    Task = "To Reject Assign To The Sector Type",
                    Description = "To Reject Assign To The Sector Type.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Reject Assign To The Sector Type",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return Ok(Item);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "To Reject Assign To The Sector Type",
                    Body = ex.Message,
                    Category = "User unable To Reject Assign To The Sector Type",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Unable To Reject Assign To The Sector Type",
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
        #endregion

        [HttpGet("GetCaseParties")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-04-22' Version="1.0" Branch="master"> Handle create document request</History>
        public async Task<IActionResult> GetCaseParties(Guid caseId)
        {

            try
            {
                return Ok(await _ICmsCaseFile.GetCMSCasePartyDetailByGuid(caseId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }

        }
        [HttpGet("GetCasePartiesForExecution")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-04-22' Version="1.0" Branch="master"> Handle create document request</History>
        public async Task<IActionResult> GetCasePartiesForExecution(Guid caseId)
        {

            try
            {
                return Ok(await _ICmsShared.GetCasePartiesByCaseIdForExecution(caseId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }

        }

        [HttpGet("GetUsersByRoleAndSector")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetUsersByRoleAndSector(string RoleId, int? SectorTypeId)
        {
            try
            {
                return Ok(await _ICmsShared.GetUsersByRoleAndSector(RoleId, SectorTypeId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetSectorUsersList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetSectorUsersList(string RoleId, int? SectorTypeId, int? pageNumber, int? pageSize,string UserId)
        {
            try
            {
                return Ok(await _ICmsShared.GetSectorUsersList(RoleId, SectorTypeId, pageNumber, pageSize,UserId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #region Add Request For TransferTask
        [HttpPost("AddCaseFileTransferRequest")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddCaseFileTransferRequest(CmsCaseFileTranferRequest cmsCaseFileTranferRequest)
        {
            try
            {
                await _ICmsShared.AddCaseFileTransferRequest(cmsCaseFileTranferRequest);
                User assignedFrom = await _IRole.GetHOSBySectorId(cmsCaseFileTranferRequest.SectorFrom);
                User assignedTo = await _IRole.GetHOSBySectorId(cmsCaseFileTranferRequest.SectorTo);
                var taskId = Guid.NewGuid();
                SaveTaskVM taskObj = null;
                Notification notificationObj = null;
                taskObj = new SaveTaskVM
                {
                    Task = new UserTask
                    {
                        TaskId = taskId,
                        Name = "Request_Transfer_To_Sector_Task",
                        Description = cmsCaseFileTranferRequest.Description,
                        Date = DateTime.Now.Date,
                        AssignedBy = await _IAccount.UserIdByUserEmail(cmsCaseFileTranferRequest?.CreatedBy),
                        IsSystemGenerated = true,
                        TaskStatusId = (int)TaskStatusEnum.Pending,
                        ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                        SectorId = (int)cmsCaseFileTranferRequest.SectorTo,
                        DepartmentId = (int)DepartmentEnum.Operational,
                        TypeId = (int)TaskTypeEnum.Task,
                        RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                        CreatedBy = cmsCaseFileTranferRequest.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReferenceId = cmsCaseFileTranferRequest.Id,
                        SubModuleId = (int)SubModuleEnum.CaseFile,
                        SystemGenTypeId = (int)TaskSystemGenTypeEnum.CaseFileTranferRequestToSector,
                    },
                    EntityName = "transferrequest",
                    Action = "view",
                    EntityId = cmsCaseFileTranferRequest.Id.ToString()
                };
                notificationObj =
                    new Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        DueDate = DateTime.Now.AddDays(5),
                        CreatedBy = cmsCaseFileTranferRequest.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                        EventId = (int)NotificationEventEnum.RequestForTransferToSector,
                        EntityName = "transferrequest",
                        Action = "view",
                        EntityId = cmsCaseFileTranferRequest.Id.ToString(),
                        NotificationParameter = cmsCaseFileTranferRequest.NotificationParameter
                    };
                await _ITask.AddTaskAndNotificationForHOSAndViceHOSOfSector(taskObj, notificationObj, (int)cmsCaseFileTranferRequest.SectorTo,
                false,//Send True if required to Check Vice HOS Responsibility for All or for only Manager, send False if need to send Tasks/Notifications for All Vice HOS
                true,//Send True if need to Include HOS as well along Vice HOSs
                0//Send Chamber Number Id if need to send Tasks for Vice HOSs of specific Chamber Number
                );
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Request For Transfer",
                    Task = "Request Transfer To The Sector Type",
                    Description = "Request For Transfer To The Sector Type.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Request For Transfer",
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
                    Subject = "Request For Transfer",
                    Body = ex.Message,
                    Category = "User unable To Request Transfer To Sector Type",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Unable To Transfer To The Sector Type",
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
        [HttpPost("RejectCaseFileTransferRequest")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> RejectCaseFileTransferRequest(CmsCaseFileTransferRequestDetailVM cmsCaseFileTransferRequestDetailVM)
        {
            try
            {
                await _ICmsShared.RejectCaseFileTransferRequest(cmsCaseFileTransferRequestDetailVM);
                NotificationParameter notificationParameter = new NotificationParameter();
                notificationParameter.Entity = new CmsCaseFileTranferRequest().GetType().Name;
                notificationParameter.SectorFrom = cmsCaseFileTransferRequestDetailVM.SectorFromNameEn + "/" + cmsCaseFileTransferRequestDetailVM.SectorFromNameAr;
                notificationParameter.SectorTo = cmsCaseFileTransferRequestDetailVM.SectorToNameEn + "/" + cmsCaseFileTransferRequestDetailVM.SectorToNameAr;
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
                (int)NotificationEventEnum.RejectToAcceptTransferRequest,
                "view",
                "transferrequest",
                cmsCaseFileTransferRequestDetailVM.Id.ToString(),
                notificationParameter);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Reject Request For Transfer To Sector",
                    Task = "To Reject Request For Transfer To Sector Type",
                    Description = "To Reject Request For Transfer To Sector Type.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Reject Request For Transfer To The Sector Type",
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
                    Subject = "To Reject Request For Transfer To The Sector Type",
                    Body = ex.Message,
                    Category = "User unable To Reject Request For Transfer To The Sector Type",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Unable To Reject Request For Transfer To The Sector Type",
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
        [HttpPost("ApproveCaseFileTransferRequest")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> ApproveCaseFileTransferRequest(CmsCaseFileTransferRequestDetailVM cmsCaseFileTransferRequestDetailVM)
        {
            try
            {
                await _ICmsShared.UpdateCaseFileTransferRequestForStatus(cmsCaseFileTransferRequestDetailVM);
                NotificationParameter notificationParameter = new NotificationParameter();
                notificationParameter.Entity = new CmsCaseFileTranferRequest().GetType().Name;
                notificationParameter.SectorFrom = cmsCaseFileTransferRequestDetailVM.SectorFromNameEn + "/" + cmsCaseFileTransferRequestDetailVM.SectorFromNameAr;
                notificationParameter.SectorTo = cmsCaseFileTransferRequestDetailVM.SectorToNameEn + "/" + cmsCaseFileTransferRequestDetailVM.SectorToNameAr;
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
                (int)NotificationEventEnum.ApprovedRequestForTransfer,
                "view",
                "transferrequest",
                cmsCaseFileTransferRequestDetailVM.Id.ToString(),
                notificationParameter);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Reject Request For Transfer To Sector",
                    Task = "To Reject Request For Transfer To Sector Type",
                    Description = "To Reject Request For Transfer To Sector Type.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Reject Request For Transfer To The Sector Type",
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
                    Subject = "To Reject Request For Transfer To The Sector Type",
                    Body = ex.Message,
                    Category = "User unable To Reject Request For Transfer To The Sector Type",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Unable To Reject Request For Transfer To The Sector Type",
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
        [HttpGet("GetCaseFileTransferRequestList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCaseFileTransferRequestList(int sectorTypeId)
        {
            try
            {
                return Ok(await _ICmsShared.GetCaseFileTransferRequestList(sectorTypeId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #region Request Transfer Detail
        [HttpGet("GetCaseFileTransferRequestDetailById")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Noman Khan' Date='2024-21-08' Version="1.0" Branch="master"> Get Case Party detail By Id</History>
        public async Task<IActionResult> GetCaseFileTransferRequestDetailById(Guid ReferenceId)
        {
            try
            {
                var result = await _ICmsShared.GetCaseFileTransferRequestDetailById(ReferenceId);
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
        #endregion
    }
}
