
using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.Communication;
using FATWA_DOMAIN.Interfaces.Notification;
using FATWA_DOMAIN.Interfaces.Tasks;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.MeetModels;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;
using static FATWA_DOMAIN.Enums.UserEnum;
using FATWA_DOMAIN.Models.CaseManagment;
using Newtonsoft.Json;
using FATWA_DOMAIN.Interfaces.CaseManagement;
using Itenso.TimePeriod;
using System.Net;
using Microsoft.Extensions.Options;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using MsgReader.Mime.Header;
using System.ComponentModel.DataAnnotations;
using Humanizer;
using System.Threading.Channels;

namespace FATWA_API.Controllers.V1.Communications
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme + ",FatwaApiKey")]
    public class CommunicationController : ControllerBase
    {

        private readonly IAuditLog _auditLog;
        private readonly ICommunication _ICommunication;
        private readonly INotification _iNotifications;
        private readonly ITask _ITask;
        private readonly ICMSCaseRequest _iCMSCaseRequest;
        private readonly IAccount _IAccount;

        public CommunicationController(ICommunication iCommunication, IAuditLog auditLog, IConfiguration configuration, INotification INotification, IRole iRole, ITask iTask, IAccount iAccount, ICMSCaseRequest iCMSCaseRequest)
        {
            _ICommunication = iCommunication;
            _auditLog = auditLog;
            _iNotifications = INotification;
            _ITask = iTask;
            _IAccount = iAccount;
            _iCMSCaseRequest = iCMSCaseRequest;
        }

        #region Save

        [HttpPost("SendCommunication")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SendCommunication(SendCommunicationVM sendCommunicationVM)
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
                var result = await _ICommunication.SendCommunication(sendCommunicationVM);
                if (sendCommunicationVM.Communication.CorrespondenceTypeId == (int)CommunicationCorrespondenceTypeEnum.Inbox)
                {
                    var taskCommunication = await _ICommunication.GetTaskCommunication(sendCommunicationVM);
                    GovernmentEntity AssignedBy = await _iCMSCaseRequest.GetGovtEntityId((int)sendCommunicationVM.Communication.GovtEntityId);
                    sendCommunicationVM.NotificationParameter.SenderName = AssignedBy.Name_En + "/" + AssignedBy.Name_Ar;
                    if (sendCommunicationVM.Communication.CommunicationTypeId == (int)CommunicationTypeEnum.RequestForMeeting)
                    {
                        var notificationResponse = await _iNotifications.SendNotification(new Notification
                        {
                            NotificationId = Guid.NewGuid(),
                            DueDate = DateTime.Now.AddDays(5),
                            CreatedBy = sendCommunicationVM.Communication.CreatedBy,
                            CreatedDate = DateTime.Now,
                            IsDeleted = false,
                            ReceiverId = taskCommunication.AssignedTo,// Assign To  Id
                            ModuleId = (int)WorkflowModuleEnum.Communication,
                        },
                        (int)NotificationEventEnum.RequestForMeeting,
                        "detail",
                        "request-meeting",
                        taskCommunication.ThirdUrl,
                        sendCommunicationVM.NotificationParameter);
                    }
                    else
                    {
                        // need to implement again after discussion  
                        var notificationResponse = await _iNotifications.SendNotification(new Notification
                        {
                            NotificationId = Guid.NewGuid(),
                            DueDate = DateTime.Now.AddDays(5),
                            CreatedBy = "System Generated",
                            CreatedDate = DateTime.Now,
                            IsDeleted = false,
                            ReceiverId = taskCommunication.AssignedTo,// Assign To  Id
                        },
                        (int)NotificationEventEnum.ReceiveLegalNotification,
                        "detail",
                        "request-need-more",
                        taskCommunication.ThirdUrl,
                        sendCommunicationVM.NotificationParameter);
                    }
                    var taskId = Guid.NewGuid();
                    var taskResult = await _ITask.AddTask(new SaveTaskVM
                    {
                        Task = new UserTask
                        {
                            TaskId = taskId,
                            Name = taskCommunication.TaskName,
                            Description = "",
                            Date = DateTime.Now.Date,
                            AssignedBy = "",
                            AssignedTo = taskCommunication.AssignedTo,
                            IsSystemGenerated = true,
                            TaskStatusId = (int)TaskStatusEnum.Pending,
                            ModuleId = (int)WorkflowModuleEnum.Communication,
                            SectorId = (int)sendCommunicationVM.Communication.SectorTypeId,
                            DepartmentId = (int)DepartmentEnum.Operational,
                            TypeId = (int)TaskTypeEnum.Task,
                            RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN  
                            CreatedBy = sendCommunicationVM.Communication.CreatedBy,
                            CreatedDate = DateTime.Now,
                            IsDeleted = false,
                            ReferenceId = taskCommunication.Linktarget.ReferenceId,
                            SubModuleId = taskCommunication.Linktarget.LinkTargetTypeId,
                            SystemGenTypeId = (int)TaskSystemGenTypeEnum.Communication,
                            EntityId = (int)sendCommunicationVM.Communication.GovtEntityId,
                        },
                        TaskActions = new List<TaskAction>()
                        {
                            new TaskAction()
                            {
                                ActionName = "Communication",
                                TaskId = taskId,
                            }
                        }
                    },
                    taskCommunication.SecondUrl,
                    taskCommunication.FirstUrl,
                    taskCommunication.ThirdUrl);
                }

                _auditLog.CreateProcessLog(new ProcessLog
                {
                    Process = "To Send a Message",
                    Task = "To send a message to FATWA",
                    Description = "User able to Create Case Request successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Message sent to FATWA",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok();
            }
            catch (Exception ex)
            {
                _auditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Sending a Message To Fatwa Failed",
                    Body = ex.Message,
                    Category = "User unable to Send a Message To Fatwa Failed",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Sending a Message To Fatwa Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        //<History Author = 'Hassan Iftikhar'  Version="1.0" Branch="master"> </History>
        [HttpPost("SaveCommResponse")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SaveCommResponse(CommunicationResponseMoreInfoVM communicationRequestMore)
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
                await _ICommunication.SaveCommunicationResponse(communicationRequestMore);
                _auditLog.CreateProcessLog(new ProcessLog
                {
                    Process = "Request For More Information",
                    Task = "Request For More Information",
                    Description = "User able to Generate A Request For More Information successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "User able to Generate A Request For More Information successfully.",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok(communicationRequestMore);

            }
            catch (Exception ex)
            {

                _auditLog.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Request For More Information",
                    Body = ex.Message,
                    Category = "User unable To Generate A Request For More Information",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "User unable To Generate A Request For More Information",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Communication

        [HttpGet("GetMeetingDetailByUsingCommunicationId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetMeetingDetailByUsingCommunicationId(Guid communicationId)
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

                return Ok(await _ICommunication.GetMeetingDetailByUsingCommunicationId(communicationId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpPost("StopExecutionRejectionReason")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> StopExecutionRejectionReason(StopExecutionRejectionReason stopExecutionRejectionReason)
        {
            try
            {
                var AssigneeEmail = await _ICommunication.StopExecutionRejectionReason(stopExecutionRejectionReason);
                StopExecutionPayloadVM ExePayload = JsonConvert.DeserializeObject<StopExecutionPayloadVM>(stopExecutionRejectionReason.Payload);

                User User = _IAccount.GetUserByUserEmail(AssigneeEmail);
                var taskId = Guid.NewGuid();
                var taskResult = await _ITask.AddTask(new SaveTaskVM
                {
                    Task = new UserTask
                    {
                        TaskId = taskId,
                        Name = "Review_Request_For_Stop_Execution_of_Judgment",
                        Description = "",
                        Date = DateTime.Now.Date,
                        AssignedBy = stopExecutionRejectionReason.AssignById,
                        AssignedTo = User.Id,
                        IsSystemGenerated = true,
                        TaskStatusId = (int)TaskStatusEnum.Pending,
                        ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                        SectorId = 6,
                        DepartmentId = (int)DepartmentEnum.Operational,
                        TypeId = (int)TaskTypeEnum.Task,
                        RoleId = "d6b3075c-3f70-4b44-baa4-1fdc599a6bb2", //FATWA ADMIN
                        CreatedBy = stopExecutionRejectionReason.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReferenceId = Guid.Parse(ExePayload.ReferenceId),
                        SubModuleId = ExePayload.SubModuleId,
                        SystemGenTypeId = (int)TaskSystemGenTypeEnum.Communication,
                        EntityId = null
                    },
                    TaskActions = new List<TaskAction>()
                    {
                        new TaskAction()
                        {
                            ActionName = "Review_Request_For_Stop_Execution_of_Judgment",
                            TaskId = taskId,
                        }
                    }
                },
                "need-more-detail",
                "request",
                ExePayload.ReferenceId.ToString() + '/' + ExePayload.CommunicationId + '/' + ExePayload.SubModuleId + '/' + ExePayload.CommunicationTypeId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetCommunicationDetailCommunicationId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCommunicationDetailCommunicationId(Guid communicationId)
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

                return Ok(await _ICommunication.GetCommunicationDetailCommunicationId(communicationId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetCommunicationDetail")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCommunicationDetail(string communicationId)
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

                return Ok(await _ICommunication.GetCommunicationDetail(communicationId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetCommunicationListByCaseRequestId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Hassan' Date='2022-10-25' Version="1.0" Branch="master"> </History>
        public async Task<IActionResult> GetCommunicationListByCaseRequestId(string caseRequestId)
        {
            try
            {
                var result = await _ICommunication.GetCommunicationListByCaseRequestId(caseRequestId);
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
        #region Get Communication Details by Case File ID, Case Request ID & Case ID
        [HttpGet("GetCommunicationDetailByCaseRequestId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCommunicationDetailByCaseRequestId(string caseRequestId, Guid communicationId)
        {
            try
            {
                var result = await _ICommunication.GetCommunicationDetailByCaseRequestId(caseRequestId, communicationId);
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
        [HttpGet("GetCommunicationDetailByCaseFileId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCommunicationDetailByCaseFileId(string fileId, int CorrespondenceTypeId, string communicationId)
        {
            try
            {
                var result = await _ICommunication.GetCommunicationDetailByCaseFileId(fileId, CorrespondenceTypeId, communicationId);
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
        [HttpGet("GetCommunicationDetailByCaseId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCommunicationDetailByCaseId(string caseId, int CorrespondenceTypeId, string communicationId)
        {
            try
            {
                var result = await _ICommunication.GetCommunicationDetailByCaseId(caseId, CorrespondenceTypeId, communicationId);
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

        #region Get Communication Details By Consultation File ID & Consultation Request ID
        [HttpGet("GetCommunicationDetailByConsultationFileId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCommunicationDetailByConsultationFileId(string fileId, int CorrespondenceTypeId, string communicationId)
        {
            try
            {
                var result = await _ICommunication.GetCommunicationDetailByConsultationFileId(fileId, CorrespondenceTypeId, communicationId);
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
        [HttpGet("GetCommunicationDetailByConsultationRequestId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCommunicationDetailByConsultationRequestId(string consultationRequestId, string communicationId)
        {
            try
            {
                var result = await _ICommunication.GetCommunicationDetailByConsultationRequestId(consultationRequestId, communicationId);
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

        [HttpGet("GetCommunicationListByConsultationRequestId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Hassan' Date='2022-10-25' Version="1.0" Branch="master"> </History>
        public async Task<IActionResult> GetCommunicationListByConsultationRequestId(string consultationRequestId)
        {
            try
            {
                var result = await _ICommunication.GetCommunicationListByConsultationRequestId(consultationRequestId);
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

        [HttpGet("GetCommunicationListByCaseFileId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Hassan' Date='2022-10-25' Version="1.0" Branch="master"> </History>
        public async Task<IActionResult> GetCommunicationListByCaseFileId(string fileId, int CorrespondenceTypeId)
        {
            try
            {
                var result = await _ICommunication.GetCommunicationListByCaseFileId(fileId, CorrespondenceTypeId);
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


        [HttpGet("GetCommunicationDetailByCommunicationId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Noman Khan' Date='2024-03-19' Version="1.0" Branch="master"> </History>
        public async Task<IActionResult> GetCommunicationDetailByCommunicationId(string fileId, int CorrespondenceTypeId, string communicationId)
        {
            try
            {
                var result = await _ICommunication.GetCommunicationDetailByCommunicationId(fileId, CorrespondenceTypeId, communicationId);
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

        [HttpGet("GetConslutationFileCommunication")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Hassan'  Version="1.0" Branch="master"> </History>
        public async Task<IActionResult> GetConslutationFileCommunication(Guid fileId, int CorrespondenceTypeId)
        {
            try
            {
                var result = await _ICommunication.GetConslutationFileCommunication(fileId, CorrespondenceTypeId);
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

        [HttpGet("GetCommunicationListByCaseId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Hassan' Date='2022-10-25' Version="1.0" Branch="master"> </History>
        public async Task<IActionResult> GetCommunicationListByCaseId(string caseId, int CorrespondenceTypeId)
        {
            try
            {
                var result = await _ICommunication.GetCommunicationListByCaseId(caseId, CorrespondenceTypeId);
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

        [HttpGet("GetInboxOutboxList")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-02-01' Version="1.0" Branch="master"> Inbox Outbox List</History>
        public async Task<IActionResult> GetInboxOutboxList(int correspondenceType, string userName, int PageSize, int PageNumber, int channelId)
        {
            try
            {
                var result = await _ICommunication.GetInboxOutboxList(correspondenceType, userName, PageSize, PageNumber, channelId);
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

        [HttpGet("GetInboxOutboxRequestNeedMoreDetail")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetInboxOutboxRequestNeedMoreDetail(Guid CommunicationId)
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
                var result = await _ICommunication.GetInboxOutboxRequestNeedMoreDetail(CommunicationId);
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

        #region Communication Send Massage Detail 
        [HttpGet("CommunicationSendMessageDetailbyId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2022-10-25' Version="1.0" Branch="master"> </History>
        public async Task<IActionResult> CommunicationSendMessageDetailbyId(string CommunicationId)
        {
            try
            {
                var result = await _ICommunication.CommunicationSendMessageDetailbyId(CommunicationId);
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

        #region Communication Send Response Detail 
        [HttpGet("CommunicationSendResponseDetailbyId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2022-10-25' Version="1.0" Branch="master"> </History>
        public async Task<IActionResult> CommunicationSendResponseDetailbyId(string CommunicationId)
        {
            try
            {
                var result = await _ICommunication.CommunicationSendResponseDetailbyId(CommunicationId);
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


        #region Get Meeting by Communication Id
        [HttpGet("GetMeetingIdCommunitationbyId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2022-10-25' Version="1.0" Branch="master"> </History>
        public async Task<IActionResult> GetMeetingIdCommunitationbyId(string CommunicationId, int CommunicationTypeId)
        {
            try
            {
                var result = await _ICommunication.GetMeetingIdCommunitationbyId(CommunicationId, CommunicationTypeId);
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

        [HttpGet("GetMeetinglistCommunitationbyId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2022-10-25' Version="1.0" Branch="master"> </History>
        public async Task<IActionResult> GetMeetinglistCommunitationbyId(string CommunicationId)
        {
            try
            {
                var result = await _ICommunication.GetMeetinglistCommunitationbyId(CommunicationId);
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


        [HttpGet("CommunicationDetailbyComIdAndComType")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> CommunicationDetailbyComIdAndComType(string ReferenceId, string CommunicationId, int SubModuleId, int CommunicationTypeId)
        {
            try
            {
                var result = await _ICommunication.CommunicationDetailbyComIdAndComType(ReferenceId, CommunicationId, SubModuleId, CommunicationTypeId);
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

        #region GetCommunicationMeetingDetailCommunicationId
        [HttpGet("GetCommunicationMeetingDetailCommunicationId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCommunicationMeetingDetailCommunicationId(Guid communicationId)
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

                return Ok(await _ICommunication.GetCommunicationMeetingDetailCommunicationId(communicationId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion 
        [HttpGet("GetGetAnnouncementsListByCaseId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'ijaz Ahmad' Date='2024-03-11' Version="1.0" Branch="master"> </History>
        public async Task<IActionResult> GetGetAnnouncementsListByCaseId(string caseId)
        {
            try
            {
                var result = await _ICommunication.GetGetAnnouncementsListByCaseId(caseId);
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

        [HttpPost("ForwardCorrespondenceToLawyer")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> ForwardCorrespondenceToLawyer(CommunicationHistory communicationHistory)
        {
            try
            {
                await _ICommunication.ForwardCorrespondenceToLawyer(communicationHistory);
                //NotificationParameter notification = new();
                foreach (var reciever in communicationHistory.RecieversId)
                {
                    //var checkExist = await _ICommunication.CheckUserExistByUserAndCommunicationId(Guid.Parse(reciever), communicationHistory.ReferenceId);
                    //if (checkExist == null)
                    //{
                        var notificationResponse = await _iNotifications.SendNotification(new Notification
                        {
                            NotificationId = Guid.NewGuid(),
                            DueDate = DateTime.Now.AddDays(5),
                            CreatedBy = communicationHistory.CreatedBy,
                            CreatedDate = DateTime.Now,
                            IsDeleted = false,
                            ReceiverId = reciever,// Assign To  Id
                            ModuleId = (int)WorkflowModuleEnum.Communication,
                        },
                               (int)NotificationEventEnum.CorrespondenceForwardToLawyer,
                           "detail",
                            $"request-need-more",
                            $"{Guid.Empty}/{communicationHistory.ReferenceId.ToString()}/{(int)LinkTargetTypeEnum.Communication}/{(int)CommunicationTypeEnum.G2GTarasolCorrespondence}",
                             communicationHistory.NotificationParameter);
                    //}

                }
                _auditLog.CreateProcessLog(new ProcessLog
                {
                    Process = "Correspondence Forward To Lawyer ",
                    Task = "Correspondence Forward To Lawyer ",
                    Description = "User able to Correspondence Forward To Lawyer  successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "User able to Generate A Request For More Information successfully.",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.Communication,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });

            }
        }


        [HttpPost("ForwardCorrespondenceToSector")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> ForwardCorrespondenceToSector(CommunicationHistory communicationHistory)
        {
            try
            {
                await _ICommunication.ForwardCorrespondenceToSector(communicationHistory);
                //NotificationParameter notification = new();
                var notificationResponse = await _iNotifications.SendNotification(new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    DueDate = DateTime.Now.AddDays(5),
                    CreatedBy = communicationHistory.CreatedBy,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    ReceiverId = communicationHistory.SentTo.ToString(),// Assign To  Id
                    ModuleId = (int)WorkflowModuleEnum.Communication,
                },
                       (int)NotificationEventEnum.CorrespondenceForwardToSector,
                             "detail",
                            $"request-need-more",
                            $"{Guid.Empty}/{communicationHistory.ReferenceId.ToString()}/{(int)LinkTargetTypeEnum.Communication}/{(int)CommunicationTypeEnum.G2GTarasolCorrespondence}",

                       communicationHistory.NotificationParameter);
                _auditLog.CreateProcessLog(new ProcessLog
                {
                    Process = "Correspondence Forward To Sector ",
                    Task = "Correspondence Forward To Sector ",
                    Description = "User able to Correspondence Forward To Sector  successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "User able to Generate A Request For More Information successfully.",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.Communication,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });

            }
        }


        [HttpPost("AssignBackToHos")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AssignBackToHos(CommunicationHistory communicationHistory)
        {
            try
            {
               await _ICommunication.AssignBackToHos(communicationHistory);
                //NotificationParameter notification = new();

                var notificationResponse = await _iNotifications.SendNotification(new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    DueDate = DateTime.Now.AddDays(5),
                    CreatedBy = communicationHistory.CreatedBy,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    ReceiverId = communicationHistory.SentTo.ToString(),// Assign To  Id
                    ModuleId = (int)WorkflowModuleEnum.Communication,
                },
                    (int)NotificationEventEnum.CorrespondenceSendBackToHOS,
                            "detail",
                            $"request-need-more",
                            $"{Guid.Empty}/{communicationHistory.ReferenceId.ToString()}/{(int)LinkTargetTypeEnum.Communication}/{(int)CommunicationTypeEnum.G2GTarasolCorrespondence}",

                    communicationHistory.NotificationParameter);
           
                _auditLog.CreateProcessLog(new ProcessLog
                {
                    Process = "Correspondence Forward To Lawyer ",
                    Task = "Correspondence Forward To Lawyer ",
                    Description = "User able to Correspondence Forward To Lawyer  successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "User able to Generate A Request For More Information successfully.",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.Communication,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });

            }
        }

        [HttpPost("SendBackToSender")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SendBackToSender(CommunicationHistory communicationHistory)
        {
            try
            {
                await _ICommunication.SendBackToSender(communicationHistory);
                return Ok();
                //foreach (var reciever in communicationHistory.RecieversId)
                //{
                //    var notificationResponse = await _iNotifications.SendNotification(new Notification
                //    {
                //        NotificationId = Guid.NewGuid(),
                //        DueDate = DateTime.Now.AddDays(5),
                //        CreatedBy = communicationHistory.CreatedBy,
                //        CreatedDate = DateTime.Now,
                //        IsDeleted = false,
                //        ReceiverId = reciever,// Assign To  Id
                //        ModuleId = (int)WorkflowModuleEnum.Communication,
                //    },
                //           (int)NotificationEventEnum.RequestForMeeting,
                //           "detail",
                //           "request-meeting",
                //           taskCommunication.ThirdUrl,
                //           sendCommunicationVM.NotificationParameter);
                //}
                _auditLog.CreateProcessLog(new ProcessLog
                {
                    Process = "Correspondence Forward To Lawyer ",
                    Task = "Correspondence Forward To Lawyer ",
                    Description = "User able to Correspondence Forward To Lawyer  successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "User able to Generate A Request For More Information successfully.",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.Communication,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });

            }
        }
        #region Get Correspondence History By Communication Id
        [HttpGet("GetCorrespondenceHistoryByCommunicationId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCorrespondenceHistoryByCommunicationId(Guid CommunicationId)
        {
            try
            {
                var result = await _ICommunication.GetCorrespondenceHistoryByCommunicationId(CommunicationId);
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

        #region Mobile App Endpoints

        [HttpGet("GetInboxOutboxListForMobileApp")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2025-07-02' Version="1.0" Branch="master"> Get Inbox Outbox List For Mobile App</History> 
        public async Task<IActionResult> GetInboxOutboxListForMobileApp(int correspondenceType, string userName, int top, int channelId)
        {
            try
            {
                var result = await _ICommunication.GetInboxOutboxListForMobileApp(correspondenceType, userName, top, channelId);
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

        [HttpGet("GetInboxOutboxDetailForMobileApp")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Noman Khan' Date='2024-07-14' Version="1.0" Branch="master"> Inbox Outbox List</History>
        public async Task<IActionResult> GetInboxOutboxDetailForMobileApp([Required] string ReferenceId, string CommunicationId, [Required] int CommunicationTypeId, int LinkTargetTypeId, [Required] string CultureType)
        {
            try
            {
                var result = await _ICommunication.GetInboxOutboxDetailForMobileApp(CommunicationTypeId, CommunicationId, LinkTargetTypeId, ReferenceId, CultureType);
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
    }
}
