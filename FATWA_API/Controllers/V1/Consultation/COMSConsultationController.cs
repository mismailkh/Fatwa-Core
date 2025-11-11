using FATWA_API.RabbitMQ;
using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.CaseManagement;
using FATWA_DOMAIN.Interfaces.Consultation;
using FATWA_DOMAIN.Interfaces.Notification;
using FATWA_DOMAIN.Interfaces.PatternNumber;
using FATWA_DOMAIN.Interfaces.Tasks;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_DOMAIN.Enums.UserEnum;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_API.Controllers.V1.Consultation
{
    //<!-- <History Author = 'Muhammad Zaeem' Date='2022-1-2' Version="1.0" Branch="master">Create API controller for handling consultation request calls</History>

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme + ",FatwaApiKey")]
    public class COMSConsultationController : ControllerBase
    {
        private readonly ICOMSConsultation _ICOMSConsultation;

        private readonly IConfiguration _configuration;
        private readonly IAuditLog _auditLogs;
        private readonly IRole _IRole;
        private readonly INotification _INotification;
        private readonly ITask _ITask;
        private readonly ICMSCaseRequest _iCMSCaseRequest;
        private readonly RabbitMQClient _client;
        private const string CreateConsultationRequestfromFatwaKey = "routingkeys.CreateConsultationRequestfromFatwaKey";
        private readonly ICMSCOMSInboxOutboxRequestPatternNumber _iCMSCOMSInboxOutboxRequestPatternNumber;

        public COMSConsultationController(ICOMSConsultation iComsConsultationRequest, IConfiguration configuration, IAuditLog auditLogs, IRole iRole,
            INotification iNotification, ITask iTask, ICMSCaseRequest iCMSCaseRequest, RabbitMQClient client, ICMSCOMSInboxOutboxRequestPatternNumber iCMSCOMSInboxOutboxRequestPatternNumber)
        {
            _ICOMSConsultation = iComsConsultationRequest;
            _configuration = configuration;
            _auditLogs = auditLogs;
            _IRole = iRole;
            _INotification = iNotification;
            _ITask = iTask;
            _iCMSCaseRequest = iCMSCaseRequest;
            _client = client;
            _iCMSCOMSInboxOutboxRequestPatternNumber = iCMSCOMSInboxOutboxRequestPatternNumber;

        }

        #region GetConsultationRequestList
        [HttpPost("GetConsultationRequestList")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2023-02-15' Version="1.0" Branch="master"> Get consultation Request By RequestId</History>
        public async Task<IActionResult> GetConsultationRequestList(AdvanceSearchConsultationRequestVM advanceSearchVM)
        {
            try
            {
                return Ok(await _ICOMSConsultation.GetConsultationRequestList(advanceSearchVM));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Consultation Request By Id(VM)
        //Author: Hassan Iftikhar
        [HttpGet("GetConsultationRequest")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> GetConsultationRequest(Guid consultationId)
        {
            try
            {
                return Ok(await _ICOMSConsultation.GetConsultationRequest(consultationId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion 

        #region Get Consultation Request Response By Id
        //Author: Hassan Iftikhar
        [HttpGet("GetConsultationFileResponseById")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> GetConsultationFileResponseById(Guid consultationId)
        {
            try
            {
                return Ok(await _ICOMSConsultation.GetConsultationFileResponseById(consultationId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Consultation Request Response By Id
        //Author: Hassan Iftikhar
        [HttpGet("GetConsultationRequestResponseById")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> GetConsultationRequestResponseById(Guid consultationId)
        {
            try
            {
                return Ok(await _ICOMSConsultation.GetConsultationRequestResponseById(consultationId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Consultation Request By Id(Main Model)
        //Author: Hassan Iftikhar
        [HttpGet("GetConsultationRequestById")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> GetConsultationRequestById(Guid consultationId)
        {
            try
            {
                return Ok(await _ICOMSConsultation.GetConsultationRequestById(consultationId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion


        #region Get Consultation PARTY By Id
        //Author: Hassan Iftikhar
        [HttpGet("GetConsultationPartyByConsultationId")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> GetConsultationPartyByConsultationId(Guid consultationId)
        {
            try
            {
                return Ok(await _ICOMSConsultation.GetConsultationPartyByConsultationId(consultationId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Consultation ARTICLE By Id
        //Author: Hassan Iftikhar
        [HttpGet("GetConsultationArticleByConsultationId")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> GetConsultationArticleByConsultationId(Guid consultationId)
        {
            try
            {
                return Ok(await _ICOMSConsultation.GetConsultationArticleByConsultationId(consultationId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get consultation Request Status History
        [HttpGet("GetCOMSConsultationRequestStatusHistory")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2022-09-30' Version="1.0" Branch="master"> Get Case Request status history</History>
        public async Task<IActionResult> GetCOMSConsultationRequestStatusHistory(string ConsultationRequestId)
        {
            try
            {
                var result = await _ICOMSConsultation.GetCOMSConsultationRequestStatusHistory(ConsultationRequestId);
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

        #region Get  Consultation Party Detail By Id
        [HttpGet("GetCOMSCosnultationPartyDetailById")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2022-09-30' Version="1.0" Branch="master"> Get Case Party detail By Id</History>
        public async Task<IActionResult> GetCOMSConsultationPartyDetailById(string Id)
        {
            try
            {
                var result = await _ICOMSConsultation.GetCOMSCosnultationPartyDetailById(Id);
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

        #region Create

        [HttpPost("CreateConsultationRequest")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Umer Zaman' Date='2023-01-05' Version="1.0" Branch="master"> Create Consultation Request</History>
        public async Task<IActionResult> CreateConsultationRequest(CaseRequestCommunicationVM consultationRequestCommunication)
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
                await _ICOMSConsultation.CreateConsultationRequest(consultationRequestCommunication);


                #region tasks
                await _iCMSCaseRequest.CommunicationForViceHos(consultationRequestCommunication);
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
                        ModuleId = (int)WorkflowModuleEnum.COMSConsultationManagement,
                        SectorId = (int)consultationRequestCommunication.ConsultationRequests.SectorTypeId,
                        DepartmentId = (int)DepartmentEnum.Operational,
                        TypeId = (int)TaskTypeEnum.Task,
                        RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                        CreatedBy = consultationRequestCommunication.ConsultationRequests.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReferenceId = consultationRequestCommunication.ConsultationRequests.ConsultationRequestId,
                        SubModuleId = (int)SubModuleEnum.ConsultationRequest,
                        SystemGenTypeId = (int)TaskSystemGenTypeEnum.CreateConsultationRequest,
                        EntityId = (int)consultationRequestCommunication.ConsultationRequests.GovtEntityId,
                    },
                    Action = "detail",
                    EntityName = "consultationrequest",
                    EntityId = consultationRequestCommunication.ConsultationRequests.ConsultationRequestId + "/" + consultationRequestCommunication.ConsultationRequests.SectorTypeId
                },
                      new Notification
                      {
                          NotificationId = Guid.NewGuid(),
                          DueDate = DateTime.Now.AddDays(5),
                          CreatedBy = "System Generated",
                          CreatedDate = DateTime.Now,
                          IsDeleted = false,
                          ModuleId = (int)WorkflowModuleEnum.COMSConsultationManagement,
                          EventId = (int)NotificationEventEnum.NewConsultationRequest,
                          Action = "detail",
                          EntityName = new ConsultationRequest().GetType().Name,
                          EntityId = consultationRequestCommunication.ConsultationRequests.ConsultationRequestId.ToString() + "/" + (int)consultationRequestCommunication.ConsultationRequests.SectorTypeId,
                          NotificationParameter = consultationRequestCommunication.ConsultationRequests.NotificationParameter
                      },
                (int)consultationRequestCommunication.ConsultationRequests.SectorTypeId,
                false,//Send True if required to Check Vice HOS Responsibility for All or for only Manager, send False if need to send Tasks/Notifications for All Vice HOS
                true,//Send True if need to Include HOS as well along Vice HOSs
                0//Send Chamber Number Id if need to send Tasks for Vice HOSs of specific Chamber Number
                );
                #endregion

                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for a new consultation",
                    Task = "To submit the request",
                    Description = "User able to Create Consultation Request successfully.",
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
                    Subject = "Request for a new consultation Failed",
                    Body = ex.Message,
                    Category = "User unable to Create Consultation Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for a new consultation Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(ex.Message);
            }
        }
         #endregion

        #region Update consultation request

        [HttpPost("UpdateConsultationRequest")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Umer Zaman' Date='2023-01-17' Version="1.0" Branch="master"> Update consultation request</History>
        public async Task<IActionResult> UpdateConsultationRequest(CaseRequestCommunicationVM consultationRequestCommunication)
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
                await _ICOMSConsultation.UpdateConsultationRequest(consultationRequestCommunication);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Update the Consultation Request",
                    Task = "Update the request",
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
                    Subject = "Update the Consultation Request Failed",
                    Body = ex.Message,
                    Category = "User unable to edit the Consultation Request",
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

        #region Withdraw consultation request

        [HttpPost("CreateConsultationWithDrawRequest")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Umer Zaman' Date='2023-02-09' Version="1.0" Branch="master"> withdraw Consultation Request </History>
        public async Task<IActionResult> CreateConsultationWithDrawRequest(WithdrawRequestCommunicationVM cmsWithdrawRequestCommunication)
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
                await _ICOMSConsultation.CreateConsultationWithDrawRequest(cmsWithdrawRequestCommunication);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "To withdraw the Consultation Request ",
                    Task = "To withdraw a consultation request",
                    Description = "Consultation Request has been withdrawn.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Consultation Request has been withdrawn",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                User assignedTo = await _IRole.GetHOSBySectorId((int)cmsWithdrawRequestCommunication.ComsWithdrawRequest.SectorTypeId);
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
                        ModuleId = (int)WorkflowModuleEnum.COMSConsultationManagement,
                        SectorId = (int)cmsWithdrawRequestCommunication.ComsWithdrawRequest.SectorTypeId,
                        DepartmentId = (int)DepartmentEnum.Operational,
                        TypeId = (int)TaskTypeEnum.Task,
                        RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                        CreatedBy = "",
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReferenceId = cmsWithdrawRequestCommunication.ComsWithdrawRequest.Id,
                        SubModuleId = (int)SubModuleEnum.ConsultationRequest,
                        SystemGenTypeId = (int)TaskSystemGenTypeEnum.WithdrawConsultationRequest,
                        EntityId = (int)cmsWithdrawRequestCommunication.ComsWithdrawRequest.GovtEntityId,
                    },
                    TaskActions = new List<TaskAction>()
                    {
                        new TaskAction()
                        {
                            ActionName = " Withdraw Request",
                            TaskId = taskId,
                        }
                    }
                },
                "withdraw-consultationrequest",
                "detail",
                cmsWithdrawRequestCommunication.ComsWithdrawRequest.Id.ToString() + "/" + (int)CommunicationTypeEnum.WithdrawRequested);
                return Ok();
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "To withdraw the Consultation Request ",
                    Body = ex.Message,
                    Category = "To withdraw the Consultation Request ",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Consultation Request could not been withdrawn",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });
            }
        }
        #endregion

        #region Get WithDraw Consultation Request by RequestId

        [HttpGet("GetWithDrawConsultationRequestByRequestId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2023-02-15' Version="1.0" Branch="master"> Get with Draw consultation Request By RequestId</History>
        public async Task<IActionResult> GetWithDrawConsultationRequestByRequestId(Guid RequestId)
        {
            try
            {
                return Ok(await _ICOMSConsultation.GetWithDrawConsultationRequestByRequestId(RequestId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Update Withdraw Consultation Request 

        //<History Author = 'Danish' Date='2023-01-23' Version="1.0" Branch="master"> Update Withdraw Case Request </History>
        [HttpPost("UpdateWithDrawConsultationRequest")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateWithDrawConsultationRequest(WithdrawRequestDetailVM consultationRequest, bool isRejected)
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
                var result = await _ICOMSConsultation.UpdateWithdrawConsultationRequestStatus(consultationRequest, isRejected);
                if (result != null)
                {
                    // Update Withdraw CaseRequest Status
                    UpdateEntityStatusVM updateEntity = new UpdateEntityStatusVM();
                    updateEntity.ReferenceId = consultationRequest.ReferenceGuid;
                    updateEntity.Reason = consultationRequest.RejectionReason;

                    if (isRejected == false)
                        updateEntity.StatusId = (int)WithdrawRequestStatusEnum.WithdrawnByGE;
                    else
                        updateEntity.StatusId = (int)WithdrawRequestStatusEnum.Rejected;
                    //Rabbit MQ send Messages
                    _client.SendMessage(updateEntity, RabbitMQKeys.ComsWithdrawRequestStatusKey);
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

        #region Get Article Status
        [HttpGet("GetArticleStatusList")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2022-09-30' Version="1.0" Branch="master"> Get all Court types</History>
        public async Task<IActionResult> GetArticleStatusList()
        {
            try
            {
                return Ok(await _ICOMSConsultation.GetArticleStatusList());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpGet("GetArticleNewNumber")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetArticleNewNumber(Guid consultationRequestId)
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

                var result = await _ICOMSConsultation.GetArticleNewNumber(consultationRequestId);
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

        #region Get consultation request details by usinivg consultationrequestid

        [HttpGet("GetConsultationRequestByReferenceId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2022-03-16' Version="1.0" Branch="master"> Get consultation request details by usinivg consultationrequestid for edit request form</History>
        public async Task<IActionResult> GetConsultationRequestByReferenceId(Guid consultationRequestId)
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
                ConsultationRequest consultationRequest = await _ICOMSConsultation.GetConsultationRequestByReferenceId(consultationRequestId);
                if (consultationRequest != null)
                {
                    return Ok(consultationRequest);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });

            }
        }
        #endregion

        #region Get section list
        [HttpGet("GetSectionList")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2022-09-30' Version="1.0" Branch="master"> Get all Court types</History>
        public async Task<IActionResult> GetSectionList()
        {
            try
            {
                return Ok(await _ICOMSConsultation.GetSectionList());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        [HttpGet("GetSectionParentList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetSectionParentList()
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

                var result = await _ICOMSConsultation.GetSectionParentList();
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

        [HttpGet("GetConsultationFileDetailsByReferenceId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2022-03-16' Version="1.0" Branch="master"> Get consultation request details by usinivg consultationrequestid for edit request form</History>
        public async Task<IActionResult> GetConsultationFileDetailsByReferenceId(Guid fileId)
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
                ConsultationFile consultationFile = await _ICOMSConsultation.GetConsultationFileDetailsByReferenceId(fileId);
                return Ok(consultationFile);

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });

            }
        }

        #region Get All Consultation Request By SectorTypeId
        [HttpGet("GetAllConsultationBySectorTypeId")]
        [MapToApiVersion("1.0")]
        //<History Author = Ijaz Ahmad' Date='2023-02-15' Version="1.0" Branch="master"> Get All consultation Request SectorTypeId</History>
        public async Task<IActionResult> GetAllConsultationBySectorTypeId(int sectorTypeId)
        {
            try
            {
                return Ok(await _ICOMSConsultation.GetAllConsultationBySectorTypeId(sectorTypeId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region Create Consultation Request From Fatwa
        [HttpPost("CreateConsultationRequestFromFatwa")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2024-13-10' Version="1.0" Branch="master"> Create Consultation Request From Fatwa</History>
        public async Task<IActionResult> CreateConsultationRequestFromFatwa(ConsultationRequest consultationRequest)
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
                var consultationRequestCommunication = await _ICOMSConsultation.CreateConsultationRequestFromFatwa(consultationRequest);
                if (consultationRequestCommunication != null && consultationRequest.RequestStatusId != (int)CaseRequestStatusEnum.Draft)
                {
                    //Rabbit MQ send Messages
                    _client.SendMessage(consultationRequestCommunication, CreateConsultationRequestfromFatwaKey);
                }
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for a new consultation",
                    Task = "To submit the request",
                    Description = "User able to Create Consultation Request successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Request has been saved",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok(consultationRequestCommunication);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Request for a new consultation Failed",
                    Body = ex.Message,
                    Category = "User unable to Create Consultation Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for a new consultation Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion
        #region Get contract template
        //<History Author = 'Umer Zaman' Date='2023-01-02' Version="1.0" Branch="master">Populate contract template details</History>

        [HttpGet("GetConsultationTemplate")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> GetConsultationTemplate()
        {
            try
            {
                var result = await _ICOMSConsultation.GetConsultationTemplate();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetSelectedConsultationTemplateSectionDetailsUsingTemplateId")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> GetSelectedConsultationTemplateSectionDetailsUsingTemplateId(int templateId)
        {
            try
            {
                var result = await _ICOMSConsultation.GetSelectedConsultationTemplateSectionDetailsUsingTemplateId(templateId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get New Consultation Request Number

        [HttpGet("GetConsultationRequestFileNumber")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2024-15-24' Version="1.0" Branch="master"> Get new consultation request number</History>
        public async Task<IActionResult> GetConsultationRequestFileNumber(int govtEntityId , int NumberTypeId)
        {
            try
            {
                var result = await _iCMSCOMSInboxOutboxRequestPatternNumber.GenerateNumberPattern(govtEntityId, NumberTypeId);
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
