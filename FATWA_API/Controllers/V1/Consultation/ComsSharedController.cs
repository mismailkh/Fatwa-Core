using FATWA_DOMAIN.Interfaces.Consultation;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_DOMAIN.Models;
using Microsoft.AspNetCore.Mvc;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_DOMAIN.Enums.UserEnum;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;
using FATWA_DOMAIN.Interfaces.Tasks;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Interfaces.Notification;
using FATWA_API.RabbitMQ;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_DOMAIN.Common;
using AutoMapper;

namespace FATWA_API.Controllers.V1.Consultation
{
    //<!-- <History Author = 'Muhammad Zaeem' Date='2023-10-03' Version="1.0" Branch="master">Shared API Controller For Consultation Management</History> -->
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class ComsSharedController : ControllerBase
    {
        #region Varaiable Declaration
        private readonly IComsShared _IComsShared;
        private readonly ICOMSConsultation _ICOMSConsultation;
        private readonly IAuditLog _auditLogs;
        private readonly ITask _ITask;
        private readonly IAccount _IAccount;
        private readonly IRole _IRole;
        private readonly INotification _iNotifications;
        private readonly IConfiguration _configuration;
        private readonly ICOMSConsultationFile _IComsConsultationFile;
        private readonly IMapper _mapper;
        private readonly RabbitMQClient _client;
        #endregion

        #region Constructor
        public ComsSharedController(IComsShared iComsShared, ICOMSConsultation iCOMSConsultationRequest, IAuditLog audit, ITask iTask, IAccount iAccount, IRole iRole,
            INotification iNotifications, IConfiguration configuration, ICOMSConsultationFile ICOMSConsultationFile, IMapper mapper,RabbitMQClient client)
        {
            _IComsShared = iComsShared;
            _ICOMSConsultation = iCOMSConsultationRequest;
            _auditLogs = audit;
            _ITask = iTask;
            _IAccount = iAccount;
            _IRole = iRole;
            _iNotifications = iNotifications;
            _configuration = configuration;
            _IComsConsultationFile = ICOMSConsultationFile;
            _mapper = mapper;
            _client = client;
        }

        #endregion

        #region Transfer sector
        [HttpPost("AddTransferComsSectorTask")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddTransferComsSectorTask(CmsApprovalTracking approvalTracking, int TransferConsultationType, int SectorTypeId)
        {
            try
            {
                await _IComsShared.SaveApprovalTrackingProcess(approvalTracking);
                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }


        #endregion

        #region Approve/Reject Transfer
        [HttpPost("ApproveTransferComsSector")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> ApproveTransferComsSector(dynamic Item, int TransferConsultationType)
        {
            try
            {
                await _IComsShared.ApproveTransferComsSector(Item, TransferConsultationType);
                if (TransferConsultationType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest)
                {
                    ConsultationRequest consultationRequest = System.Text.Json.JsonSerializer.Deserialize<ConsultationRequest>(Item);
                    consultationRequest.EventId = (int)CaseRequestEventEnum.Transfer;
                    if (consultationRequest.SectorTypeId == (int)OperatingSectorTypeEnum.PrivateOperationalSector)
                    {
                        consultationRequest.IsConfidential = true;
                    }
                    else
                    {
                        consultationRequest.IsConfidential = false;
                    }
                    //Rabbit MQ send Messages
                    _client.SendMessage(consultationRequest, RabbitMQKeys.UpdateComsRequestHistory);
                }
                if (TransferConsultationType == (int)AssignCaseToLawyerTypeEnum.ConsultationFile)
                {
                    ConsultationFile consultationFile = System.Text.Json.JsonSerializer.Deserialize<ConsultationFile>(Item);

                    // Update Case Request Status
                    UpdateEntityStatusVM updateEntity = new UpdateEntityStatusVM();
                    updateEntity.ReferenceId = consultationFile.FileId;
                    if (consultationFile.SectorTypeId == (int)OperatingSectorTypeEnum.Contracts)
                    {
                        updateEntity.StatusId = (int)CaseFileStatusEnum.AssignedToContractSector;
                    }
                    if (consultationFile.SectorTypeId == (int)OperatingSectorTypeEnum.Legislations)
                    {
                        updateEntity.StatusId = (int)CaseFileStatusEnum.AssignedToLegislationSector;

                    }
                    if (consultationFile.SectorTypeId == (int)OperatingSectorTypeEnum.LegalAdvice)
                    {
                        updateEntity.StatusId = (int)CaseFileStatusEnum.AssignedToLegalAdviceSector;

                    }
                    if (consultationFile.SectorTypeId == (int)OperatingSectorTypeEnum.InternationalArbitration)
                    {
                        updateEntity.StatusId = (int)CaseFileStatusEnum.AssignedToInternationalArbitrationSector;

                    }
                    if (consultationFile.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeComplaints)
                    {
                        updateEntity.StatusId = (int)CaseFileStatusEnum.AssignedToAdministrativeComplaintsSector;

                    }
                    //Rabbit MQ send Messages
                    _client.SendMessage(updateEntity, RabbitMQKeys.RequestStatusKey);
                }
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Approve Tansfer Sector",
                    Task = "To Approve Transfer The Sector Type",
                    Description = "To Approve Transfer The Sector Type.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Transfer The Sector Type",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.COMSConsultationManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });


                return Ok(Item);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "To Approve Transfer The Sector Type",
                    Body = ex.Message,
                    Category = "User unable To Approve Transfer The Sector Type",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Unable To Approve Transfer The Sector Type",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.COMSConsultationManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }

        [HttpPost("RejectTransferComsSector")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> RejectTransferComsSector(dynamic Item, int TransferConsultationType)
        {
            try
            {
                await _IComsShared.RejectTransferComsSector(Item, TransferConsultationType);

                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Reject Tansfer Sector",
                    Task = "To Reject Transfer The Sector Type",
                    Description = "To Reject Transfer The Sector Type.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Reject Transfer The Sector Type",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.COMSConsultationManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return Ok(Item);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "To Reject Transfer The Sector Type",
                    Body = ex.Message,
                    Category = "User unable To Reject Transfer The Sector Type",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Unable To Reject Transfer The Sector Type",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.COMSConsultationManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }

        #endregion

        #region Assign To Lawyer

        [HttpPost("AssignConsultationToLawyer")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2023-1-12' Version="1.0" Branch="master"> Handle create document request</History>
        public async Task<IActionResult> AssignConsultationToLawyer(ConsultationAssignment consultationfileAssignment)
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
                Guid? referenceId = await _IComsShared.AssignConsultationToLawyer(consultationfileAssignment);
                if (referenceId != null)
                {
                    if (consultationfileAssignment.AssignConsultationLawyerType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest)
                    {
                        User assignedFrom = await _IRole.GetHOSBySectorId((int)consultationfileAssignment.SectorTypeId);
                        await TaskUpdate((Guid)consultationfileAssignment.ConsultationRequestId, assignedFrom.Id, (int)TaskSystemGenTypeEnum.CreateConsultationRequest);
                    }
                }
                

                string entityName;
                if ((int)AssignCaseToLawyerTypeEnum.ConsultationRequest == consultationfileAssignment.AssignConsultationLawyerType || (int)AssignCaseToLawyerTypeEnum.ConsultationFile == consultationfileAssignment.AssignConsultationLawyerType)
                {
                    entityName = new ConsultationFile().GetType().Name;
                    consultationfileAssignment.NotificationParameter.Entity = entityName;
                }
                else
                {
                    entityName = "Consultation";
                    consultationfileAssignment.NotificationParameter.Entity = entityName;
                }
                var taskId = Guid.NewGuid();
                var taskResult = await _ITask.AddTask(new SaveTaskVM
                {
                    Task = new UserTask
                    {
                        TaskId = taskId,
                        Name = "Request_Assigned_To_Lawyer_Task",
                        Date = DateTime.Now.Date,
                        AssignedBy = await _IAccount.UserIdByUserEmail(consultationfileAssignment.CreatedBy),
                        AssignedTo = (bool)consultationfileAssignment.SelectedUsers?.Any() ? consultationfileAssignment.PrimaryLawyerId : consultationfileAssignment.LawyerId,
                        IsSystemGenerated = true,
                        TaskStatusId = (int)TaskStatusEnum.Pending,
                        ModuleId = (int)WorkflowModuleEnum.COMSConsultationManagement,
                        SectorId = (int)consultationfileAssignment.SectorTypeId,
                        DepartmentId = (int)DepartmentEnum.Operational,
                        TypeId = (int)TaskTypeEnum.Assignment,
                        RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                        CreatedBy = consultationfileAssignment.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReferenceId = referenceId,
                        SubModuleId = (int)SubModuleEnum.ConsultationFile,
                        SystemGenTypeId = consultationfileAssignment.AssignConsultationLawyerType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest ? (int)TaskSystemGenTypeEnum.ConsultationRequestAssignToLawyer : (int)TaskSystemGenTypeEnum.ConsultationFileAssignToLawyer,

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
                "review-assignment",
                entityName,
                referenceId.ToString());

                var notificationResult = await _iNotifications.SendNotification(new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    DueDate = DateTime.Now.AddDays(5),
                    CreatedBy = consultationfileAssignment.CreatedBy,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    ReceiverId = consultationfileAssignment.LawyerId,
                    ModuleId = (int)WorkflowModuleEnum.COMSConsultationManagement,
                },
                (int)NotificationEventEnum.AssignToLawyer,
                "review-assignment",
                entityName,
                consultationfileAssignment.FileId.ToString() + '/' + taskId,
                consultationfileAssignment.NotificationParameter);

                if (consultationfileAssignment.AssignConsultationLawyerType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest)
                {
                    var consultationFile = await _IComsConsultationFile.ConsultationFileDetailWithPartiesAndAttachments(referenceId.Value);
                    //Rabbit MQ send Messages
                    // Update Consultation Request Status
                    UpdateEntityStatusVM updateEntity = new UpdateEntityStatusVM();
                    updateEntity.ReferenceId = (Guid)consultationfileAssignment.ConsultationRequestId;
                    updateEntity.StatusId = (int)CaseRequestStatusEnum.ConvertedToFile;
                    updateEntity.SubModuleId = (int)SubModuleEnum.ConsultationRequest;
                    _client.SendMessage(updateEntity, RabbitMQKeys.RequestStatusKey);
                    // CreateConsultationFile
                    _client.SendMessage(consultationFile, RabbitMQKeys.CreateConsultationFileKey);
                }
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "View the list of consultation Requests",
                    Task = "To assign the request to a lawyer",
                    Description = "Request assigned to lawyer.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Request assigned to lawyer",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.COMSConsultationManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok(consultationfileAssignment);
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
                    ModuleID = (int)WorkflowModuleEnum.COMSConsultationManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        private async Task TaskUpdate(Guid ReferenceId, string UserId, int SystemGeneratedTaskId)
        {
            try
            {
                var task = await _ITask.GetTaskDetailBySystemGeneratedId(ReferenceId, UserId, SystemGeneratedTaskId);
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

        #region Get Approval Tracking

        [HttpGet("GetApprovalTrackingProcess")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetApprovalTrackingProcess(Guid referenceId, int sectorTypeId, int processTypeId)
        {
            try
            {
                return Ok(await _IComsShared.GetApprovalTrackingProcess(referenceId, sectorTypeId, processTypeId));

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }

        #endregion

        #region Send a copy 
        [HttpPost("AddSendACopyConsultationTask")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddSendACopyConsultationTask(CmsApprovalTracking approvalTracking, int TransferConsultationType, int SectorTypeId)
        {
            try
            {
                await _IComsShared.SaveApprovalTrackingProcess(approvalTracking);

                //Find HOS of Transfer Sector here to Create task for it
                User assignedTo = await _IRole.GetHOSBySectorId(approvalTracking.SectorTo);
                User assignedFrom = await _IRole.GetHOSBySectorId(approvalTracking.SectorFrom);

                var taskId = Guid.NewGuid();
                var taskResult = await _ITask.AddTask(new SaveTaskVM
                {
                    Task = new UserTask
                    {
                        TaskId = taskId,
                        Name = "Review_Recieved_Copy_of_Consultation_Request",
                        Description = approvalTracking.Remarks,
                        Date = DateTime.Now.Date,
                        AssignedBy = assignedFrom?.Id,
                        AssignedTo = assignedTo?.Id,
                        IsSystemGenerated = true,
                        TaskStatusId = (int)TaskStatusEnum.Pending,
                        ModuleId = (int)WorkflowModuleEnum.COMSConsultationManagement,
                        SectorId = approvalTracking.SectorTo,
                        DepartmentId = (int)DepartmentEnum.Operational,
                        TypeId = (int)TaskTypeEnum.Task,
                        RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                        CreatedBy = approvalTracking.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReferenceId = approvalTracking.ReferenceId
                    },
                    TaskActions = new List<TaskAction>()
                    {
                        new TaskAction()
                        {
                            ActionName = "Review Recieved Copy of Consultation Request",
                            TaskId = taskId,
                        }
                    }
                },
                "copy-review/" + SectorTypeId,
                TransferConsultationType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest ? new ConsultationRequest().GetType().Name : new ConsultationFile().GetType().Name,
                approvalTracking.ReferenceId.ToString());

                var notificationResult = await _iNotifications.SendNotification(new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    DueDate = DateTime.Now.AddDays(5),
                    CreatedBy = approvalTracking.CreatedBy,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    ReceiverId = assignedTo?.Id,
                    ModuleId = (int)WorkflowModuleEnum.COMSConsultationManagement,
                },
               (int)NotificationEventEnum.SendACopyApproved,
               "list",
               new UserTask().GetType().Name,
               assignedTo?.Id,
               approvalTracking.NotificationParameter);


                return Ok();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpPost("ApproveSendACopyConsultation")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> ApproveSendACopyConsultation(dynamic Item, int TransferConsultationType)
        {
            try
            {
                await _IComsShared.ApproveSendACopy(Item, TransferConsultationType);

                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Approve Send A Copy Request",
                    Task = "To Approve Send A Copy Request",
                    Description = "To Approve Send A Copy Request.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Transfer The Sector Type",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.COMSConsultationManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return Ok(Item);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "To Approve Send A Copy Request",
                    Body = ex.Message,
                    Category = "User unable To Approve Send A Copy Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Unable To Approve Send A Copy Request",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.COMSConsultationManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }

        [HttpPost("RejectSendACopyConsultation")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> RejectSendACopyConsultation(dynamic Item, int TransferConsultationType)
        {
            try
            {
                await _IComsShared.RejectSendACopy(Item, TransferConsultationType);

                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Reject Send A Copy Request",
                    Task = "To Reject Send A Copy Request",
                    Description = "To Reject Send A Copy Request.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Reject Send A Copy Request",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.COMSConsultationManagement,
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
                    ModuleID = (int)WorkflowModuleEnum.COMSConsultationManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });
            }
        }

        #endregion

        [HttpPost("ApproveConsultationFile")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> ApproveConsultationFile(ConsultationFileDetailVM item)
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
                var result = await _IComsShared.ApproveConsultationFile(item);
                if (result != null)
                {
                    //var mapObj = _mapper.Map<UpdateEntityHistoryVM>(result);
                    UpdateEntityHistoryVM mapObj = new UpdateEntityHistoryVM();
                    mapObj.HistoryId = result.HistoryId;
                    mapObj.ReferenceId = result.FileId;
                    mapObj.EventId = result.EventId;
                    mapObj.Remarks = result.Remarks;
                    mapObj.StatusId = result.StatusId;
                    mapObj.CreatedBy = result.CreatedBy;
                    mapObj.CreatedDate = result.CreatedDate;
                    mapObj.IsDeleted = result.IsDeleted;
                    mapObj.ModifiedBy = result.ModifiedBy;
                    mapObj.ModifiedDate = result.ModifiedDate;
                    mapObj.DeletedDate = result.DeletedDate;
                    mapObj.DeletedBy = result.DeletedBy;
                    mapObj.SubModuleId = (int)SubModuleEnum.ConsultationFile;
                    //Rabbit MQ send Messages
                    _client.SendMessage(mapObj, RabbitMQKeys.HistoryKey);
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpPost("GetConsultationAssigmentByLawyerIdAndFileId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetConsultationAssigmentByLawyerIdAndFileId(Guid FileId, string UserId)
        {
            try
            {

                return Ok(await _IComsShared.GetConsultationAssigmentByLawyerIdAndFileId(FileId, UserId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }

        #region Assign Back To Hos

        [HttpPost("AssignConsultationBackToHos")]
        [MapToApiVersion("1.0")]
        //<History Author = 'ijaz Ahmad' Date='2023-04-17' Version="1.0" Branch="master"> Assign Back to Hos</History>
        public async Task<IActionResult> AssignConsultationBackToHos(CmsAssignCaseFileBackToHos cmsAssignBackToHos, int SectorTypeId)
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

                await _IComsShared.AssignConsultationBackToHos(cmsAssignBackToHos);


                //await _ICmsCaseFile.UpdatCaseFileaAssignedBackToHos(cmsAssignBackToHos.ReferenceId, true, cmsAssignBackToHos.CreatedBy);
                //Find HOS by Sector type Id
                User assignedTo = await _IRole.GetHOSBySectorId((int)cmsAssignBackToHos.SectorTypeId);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Consultation File Assigned Back To HOS",
                    Task = "To assign the request to a HOS",
                    Description = "Request assigned to HOS.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Request assigned to HOS",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                string entityName;

                entityName = "ConsultationFile";

                var taskId = Guid.NewGuid();
                var taskResult = await _ITask.AddTask(new SaveTaskVM
                {
                    Task = new UserTask
                    {
                        TaskId = taskId,
                        Name = "Consultation_File_Rejected_By_Lawyer",
                        Date = DateTime.Now.Date,
                        AssignedBy = await _IAccount.UserIdByUserEmail(cmsAssignBackToHos.CreatedBy),
                        AssignedTo = assignedTo.Id,
                        IsSystemGenerated = true,
                        TaskStatusId = (int)TaskStatusEnum.Pending,
                        ModuleId = (int)WorkflowModuleEnum.COMSConsultationManagement,
                        SectorId = (int)cmsAssignBackToHos.SectorTypeId,
                        DepartmentId = (int)DepartmentEnum.Operational,
                        TypeId = (int)TaskTypeEnum.Assignment,
                        RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                        CreatedBy = cmsAssignBackToHos.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReferenceId = cmsAssignBackToHos.ReferenceId,
                        SubModuleId = (int)SubModuleEnum.ConsultationFile,
                        SystemGenTypeId = (int)TaskSystemGenTypeEnum.ConsultationFileRejectTransfer,
                    },
                    TaskActions = new List<TaskAction>()
                    {
                        new TaskAction()
                        {
                            ActionName = "Consultation_File_Rejected_By_Lawyer",
                            TaskId = taskId,
                        }
                    }
                },
                "view",
                entityName,
                cmsAssignBackToHos.ReferenceId.ToString()+"/"+SectorTypeId);
                var notificationResult = await _iNotifications.SendNotification(new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    DueDate = DateTime.Now.AddDays(5),
                    CreatedBy = cmsAssignBackToHos.CreatedBy,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    ReceiverId = assignedTo.Id,
                    ModuleId = (int)WorkflowModuleEnum.COMSConsultationManagement,
                },
              (int)NotificationEventEnum.AssignBackToHos,
              "view",
              entityName,
              cmsAssignBackToHos.ReferenceId.ToString() + "/"+SectorTypeId +"/"+taskId,
              cmsAssignBackToHos.NotificationParameter);
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

        [HttpGet("GetSendBackToHosByReferenceId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetSendBackToHosByReferenceId(Guid ReferenceId, string LawyerId)
        {
            try
            {

                return Ok(await _IComsShared.GetSendBackToHosByReferenceId(ReferenceId, LawyerId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }
    }
}
