using AutoMapper;
using FATWA_DOMAIN.Enums;
using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.CaseManagement;
using FATWA_DOMAIN.Interfaces.Communication;
using FATWA_DOMAIN.Interfaces.Notification;
using FATWA_DOMAIN.Interfaces.Tasks;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MsgReader.Mime.Header;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_API.Controllers.V1.Communications
{
    //<!-- <History Author = 'Hassan Iftikhar' Date='2024-04-02' Version="1.0" Branch="master">API Controller For Communication From G2G Tarasol portal using RPA</History> -->
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme + ",FatwaApiKey")]


    public class CommunicationTarasolRPAController : ControllerBase
    {
        #region Variable Declaration
        private readonly ICommunicationTarasolRPA _ICommunicationTarasolRPA;

        private readonly IAuditLog _auditLogs;
        private readonly ITask _ITask;
        private readonly IAccount _IAccount;
        private readonly IRole _IRole;
        private readonly IConfiguration _configuration;
        private readonly INotification _iNotifications;
        private readonly IMapper _mapper;

        #endregion

        #region Constructor
        public CommunicationTarasolRPAController(ICommunicationTarasolRPA ICommunicationTarasolRPA, ICMSCaseRequest iCMSCaseRequest, ICmsCaseFile iCmsCaseFile, IAuditLog audit, ITask iTask, IAccount iAccount,
            IRole iRole, IConfiguration configuration, INotification iNotifications, IMapper mapper)
        {
            _ICommunicationTarasolRPA = ICommunicationTarasolRPA;

            _auditLogs = audit;
            _ITask = iTask;
            _IAccount = iAccount;
            _IRole = iRole;
            _configuration = configuration;
            _iNotifications = iNotifications;
            _mapper = mapper;
        }

        #endregion

        #region Save Communication

        [HttpPost("AddCommunicationData")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddCommunicationData(CommunicationTarasolRPAPayload communicationPayload)
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

                SendCommunicationVM communication = await _ICommunicationTarasolRPA.AddCommunicationData(communicationPayload);
                communication.NotificationParameter = new();
                communication.NotificationParameter.CorrespodenceNumber = communication.Communication.InboxNumber;
                communication.NotificationParameter.GEName = communicationPayload.SenderSiteName;
               var notificationResponse = await _iNotifications.SendNotification(new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    DueDate = DateTime.Now.AddDays(5),
                    CreatedBy = "System Generated",
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    ReceiverId = communication.RecieverId,// Assign To  Id
                    ModuleId = (int)WorkflowModuleEnum.Communication,
                },
                            (int)NotificationEventEnum.ReceiveFromTarassol,
                            "detail",
                            $"request-need-more",
                            $"{Guid.Empty}/{communicationPayload.Guid.ToString()}/{(int)LinkTargetTypeEnum.Communication}/{(int)CommunicationTypeEnum.G2GTarasolCorrespondence}",
                            communication.NotificationParameter);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Correrspondence added by RPA",
                    Task = "Correrspondence added by RPA",
                    Description = "Correrspondence added by RPA",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Correrspondence added by RPA",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.Communication,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return Ok(new { Status = "Success", Message = "The Correspondenece Added successfully.", CorrespondenceId = communication.Communication.CommunicationId });
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "To Add Correspondence by RPA",
                    Body = ex.Message,
                    Category = "Unable To Add Correspondence",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Unable To Add Correspondence by RPA",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.Communication,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpPost("AddFaultyCommunicationData")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddFaultyCommunicationData(CommunicationTarasolRpaPayload payload)
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
                await _ICommunicationTarasolRPA.AddFaultyCommunicationData(payload);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "To Migrate Case Data",
                    Task = "To Migrate Case Data",
                    Description = "Case data migrated Added.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Case data migrated Added",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.Communication,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return Ok(new { Status = "Success", Message = "The Correspondenece Added successfully." });
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "To Migrate Case Data",
                    Body = ex.Message,
                    Category = "Unable To migrate Case Data",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Unable To migrate Case Data",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.Communication,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Correspondences

        [HttpGet("GetCorrespondences")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCorrespondences()
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
                return Ok(await _ICommunicationTarasolRPA.GetCorrespondences());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        [HttpPost("SendTarassolCommunication")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SendTarassolCommunication(SendCommunicationVM sendCommunicationVM)
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
                return Ok(await _ICommunicationTarasolRPA.SendTarassolCommunication(sendCommunicationVM));

                //_auditLog.CreateProcessLog(new ProcessLog
                //{
                //	Process = "To Send a Message",
                //	Task = "To send a message to FATWA",
                //	Description = "User able to Create Case Request successfully.",
                //	ProcessLogEventId = (int)ProcessLogEnum.Processed,
                //	Message = "Message sent to FATWA",
                //	IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                //	ApplicationID = (int)PortalEnum.FatwaPortal,
                //	ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                //	Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                //});

            }
            catch (Exception ex)
            {
                //_auditLog.CreateErrorLog(new ErrorLog
                //{
                //	ErrorLogEventId = (int)ErrorLogEnum.Error,
                //	Subject = "Sending a Message To Fatwa Failed",
                //	Body = ex.Message,
                //	Category = "User unable to Send a Message To Fatwa Failed",
                //	Source = ex.Source,
                //	Type = ex.GetType().Name,
                //	Message = "Sending a Message To Fatwa Failed",
                //	IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                //	ApplicationID = (int)PortalEnum.FatwaPortal,
                //	ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                //	Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                //});
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpGet("GetGovernmentEntitiyById")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan' Date='2024-03-11' Version="1.0" Branch="master"> </History>
        public async Task<IActionResult> GetGovernmentEntitiesById(int Id)
        {
            try
            {
                var result = await _ICommunicationTarasolRPA.GetGovernmentEntitiyById(Id);
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
        [HttpGet("GetGEDepartmentById")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan' Date='2024-03-11' Version="1.0" Branch="master"> </History>
        public async Task<IActionResult> GetGEDepartmentById(int Id)
        {
            try
            {
                var result = await _ICommunicationTarasolRPA.GetGEDepartmentById(Id);
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

        [HttpGet("GetRPAFaultyMessages")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRPAFaultyMessages()
        {
            try
            {
                var result = await _ICommunicationTarasolRPA.GetRPAFaultyMessages();
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


    }
}
