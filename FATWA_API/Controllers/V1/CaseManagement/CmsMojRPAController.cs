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
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_DOMAIN.Models.ViewModel.MojRollsVM;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_DOMAIN.Enums.UserEnum;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_API.Controllers.V1.CaseManagement
{
    //<!-- <History Author = 'Hassan Abbas' Date='2024-02-01' Version="1.0" Branch="master">API Controller For Data Migration of MOJ</History> -->
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme + ",FatwaApiKey")]
    public class CmsMojRPAController : ControllerBase
    {
        #region Variable Declaration
        private readonly ICmsMojRPA _ICmsMojRPA;
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
        public CmsMojRPAController(ICmsMojRPA iCmsMojRPA, ICMSCaseRequest iCMSCaseRequest, ICmsCaseFile iCmsCaseFile, IAuditLog audit, ITask iTask, IAccount iAccount,
            IRole iRole, IConfiguration configuration, INotification iNotifications, IMapper mapper, RabbitMQClient client)
        {
            _ICmsMojRPA = iCmsMojRPA;
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

        #region Add Case Data

        [HttpPost("AddCaseData")]
        [MapToApiVersion("1.0")]
        //[AllowAnonymous]
        //[ApiKeyAuthorize]
        //<History Author = 'Hassan Abbas' Date='2024-02-01' Version="1.0" Branch="master"> Endpoint to Migrate Case Data from MOJ</History>
        public async Task<IActionResult> AddCaseData(CmsMojRPAPayloadVM requestPayload)
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
                CmsMojRPACaseData caseData = await _ICmsMojRPA.AddCaseData(requestPayload);

                if (caseData.RegisteredCase.SectorTypeId > 0)
                {
                    await _ITask.AddTaskAndNotificationForHOSAndViceHOSOfSector(new SaveTaskVM
                    {
                        Task = new UserTask
                        {
                            Name = "Case_Migrated_From_MOJ",
                            Description = "",
                            Date = DateTime.Now.Date,
                            AssignedBy = "MOJ RPA",
                            IsSystemGenerated = true,
                            TaskStatusId = (int)TaskStatusEnum.Pending,
                            ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                            SectorId = (int)caseData.RegisteredCase.SectorTypeId,
                            DepartmentId = (int)DepartmentEnum.Operational,
                            TypeId = (int)TaskTypeEnum.Task,
                            RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                            CreatedBy = "System Generated",
                            CreatedDate = DateTime.Now,
                            IsDeleted = false,
                            ReferenceId = caseData.RegisteredCase.CaseId,
                            SubModuleId = (int)SubModuleEnum.RegisteredCase,
                            SystemGenTypeId = (int)TaskSystemGenTypeEnum.CaseMigratedFromMOJ,
                        },
                        Action = "view",
                        EntityName = "case",
                        EntityId = caseData.RegisteredCase.CaseId.ToString()
                    },
                    new Notification
                    {
                        DueDate = DateTime.Now.AddDays(5),
                        CreatedBy = "System Generated",
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                        EventId = (int)NotificationEventEnum.CaseDataPushedFromRPA,
                        Action = "view",
                        EntityName = "case",
                        EntityId = caseData.RegisteredCase.CaseId.ToString(),
                        NotificationParameter = requestPayload.NotificationParameter
                    },
                    (int)caseData.RegisteredCase.SectorTypeId,
                    false,//Send True if required to Check Vice HOS Responsibility for All or for only Manager, send False if need to send Tasks/Notifications for All Vice HOS
                    true,//Send True if need to Include HOS as well along Vice HOSs
                    caseData.RegisteredCase.ChamberNumberId//Send Chamber Number Id if need to send Tasks for Vice HOSs of specific Chamber Number
                    );
                }
                if (requestPayload != null)
                {
                    //Rabbit MQ send Messages
                    _client.SendMessage(caseData, RabbitMQKeys.MojCaseDataSyncKey);
                }
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "To Migrate Case Data",
                    Task = "To Migrate Case Data",
                    Description = "Case data migrated Added.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Case data migrated Added",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return Ok(new { Status = "Success", Message = "Case details has been saved successfully.", CaseId = caseData.RegisteredCase.CaseId });
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
                    ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Migrated Unassigned Cases from MOJ 

        [HttpPost("GetUnassignedMigratedCaseFilesList")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2024-02-26' Version="1.0" Branch="master"> Get cases which were migrated from MOJ but are not assigned to any sector</History>
        public async Task<IActionResult> GetUnassignedMigratedCaseFilesList(AdvanceSearchCmsCaseFileVM advanceSearchVM)
        {
            try
            {
                return Ok(await _ICmsMojRPA.GetUnassignedMigratedCaseFilesList(advanceSearchVM));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Assign Unassigned Cases to Sector

        [HttpPost("AssignUnassignedFilesToSector")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2024-02-26' Version="1.0" Branch="master"> Get cases which were migrated from MOJ but are not assigned to any sector</History>
        public async Task<IActionResult> AssignUnassignedFilesToSector(AssignMojCaseFileToSectorVM sectorAssignment)
        {
            try
            {
                var result = await _ICmsMojRPA.AssignUnassignedFilesToSector(sectorAssignment);
                if(result != null)
                {
                    _client.SendMessage(result, RabbitMQKeys.MojAssignCaseFileToSectorKey);
                }
                foreach (var fileId in sectorAssignment.FileIds)
                {
                    User assignedTo = await _IRole.GetHOSBySectorId((int)sectorAssignment.SectorTypeId);
                    var taskId = Guid.NewGuid();
                    var taskResult = await _ITask.AddTask(new SaveTaskVM
                    {
                        Task = new UserTask
                        {
                            TaskId = taskId,
                            Name = "Case_Migrated_From_MOJ",
                            Description = "",
                            Date = DateTime.Now.Date,
                            AssignedBy = "System Generated",
                            AssignedTo = assignedTo?.Id,
                            IsSystemGenerated = true,
                            TaskStatusId = (int)TaskStatusEnum.Pending,
                            ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                            SectorId = (int)sectorAssignment.SectorTypeId,
                            DepartmentId = (int)DepartmentEnum.Operational,
                            TypeId = (int)TaskTypeEnum.Task,
                            RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                            CreatedBy = "System Generated",
                            CreatedDate = DateTime.Now,
                            IsDeleted = false,
                            ReferenceId = fileId,
                            SubModuleId = (int)SubModuleEnum.RegisteredCase,
                            SystemGenTypeId = (int)TaskSystemGenTypeEnum.CaseMigratedFromMOJ,
                        },
                        TaskActions = new List<TaskAction>()
                        {
                            new TaskAction()
                            {
                                ActionName = "Case Migrated fom MOJ",
                                TaskId = taskId,
                            }
                        }
                    },
                    "view",
                    "casefile",
                    fileId.ToString());
                    var notificationResponse = await _iNotifications.SendNotification(new Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        DueDate = DateTime.Now.AddDays(5),
                        CreatedBy = "System Generated",
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReceiverId = assignedTo?.Id,// Assign To Sactor Id
                        ModuleId = (int)WorkflowModuleEnum.CaseManagement
                    },
                   (int)NotificationEventEnum.AssigUnassignedCasestoSector,
                   "view",
                   "casefile",
                   fileId.ToString(),
                   sectorAssignment.NotificationParameter);

                }
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "To Assign Migrated Case Data From MOJ to Sector",
                    Task = "To Assign Migrated Case Data From MOJ to Sector",
                    Description = "Migrated Case Data From MOJ Assigned to Sector",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Migrated Case Data From MOJ Assigned to Sector",
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
                    Subject = "To Assign Migrated Case Data From MOJ to Sector",
                    Body = ex.Message,
                    Category = "Unable To Assign Migrated Case Data From MOJ to Sector",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Unable To Assign Migrated Case Data From MOJ to Sector",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region MOJ Add  Hearings & OutcomeHearing Data 
        [MapToApiVersion("1.0")]
        [HttpPost("AddHearingData")]
        //[AllowAnonymous]
        //[ApiKeyAuthorize]
        //<History Author = 'Ijaz Ahmad' Date='2024-03-25' Version="1.0" Branch="master"> MOJ RPA Add Hearing data</History>
        public async Task<IActionResult> AddHearingData(CmsMojRPAHearing mojRPAHearing)
        {
            try
            {
                var CanAndCaseNumber = await _ICmsMojRPA.AddHearingData(mojRPAHearing.Cases, mojRPAHearing.HearingDate, mojRPAHearing.DocumentId);
                if (CanAndCaseNumber != null && CanAndCaseNumber.Count > 0)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Add Hearing",
                        Task = "To Add Hearing for a Case",
                        Description = "To Add  Hearing for a Case",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = " Hearing added.",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });
                    return Ok(new { Status = "Success", Message = "The Hearing  created successfully for following cases.", CANS = CanAndCaseNumber });
                }
                else
                {
                    return NotFound(new { success = false, Message = "The given Case & CAN numbers dose not found " });
                }

            }
            catch (Exception ex)
            {

                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Add Hearing",
                    Body = ex.Message,
                    Category = "User unable to add the  Hearing for a case",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "User unable to add the  Hearing for a case",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }
        [MapToApiVersion("1.0")]
        [HttpPost("AddOutcomeHearingData")]
        //[AllowAnonymous]
        //[ApiKeyAuthorize]
        //<History Author = 'Ijaz Ahmad' Date='2024-03-25' Version="1.0" Branch="master"> MOJ RPA Add out Come hearing data</History>
        public async Task<IActionResult> AddOutcomeHearingData(CmsMojRPAOutComeHearing mojRPAOutcomeHearing)
        {
            try
            {
                var canAndCaseNumbers = await _ICmsMojRPA.AddOutcomeHearingData(mojRPAOutcomeHearing.Cases, mojRPAOutcomeHearing.DocumentId, mojRPAOutcomeHearing.HearingDate, mojRPAOutcomeHearing.NextHearingDate);
                if (canAndCaseNumbers != null && canAndCaseNumbers.Count > 0)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Add OutcomeHearing",
                        Task = "To Add OutcomeHearing for a Case",
                        Description = "To Add  OutcomeHearing for a Case",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = " OutcomeHearing added.",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });
                    return Ok(new { Status = "Success", Message = "The OutcomeHearing was created successfully for the following cases.", CANS = canAndCaseNumbers });
                }
                else
                {
                    return NotFound(new { success = false, Message = "The given hearing date does not exist against CANS & Case Number!" });

                }

            }
            catch (Exception ex)
            {

                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Add OutcomeHearing",
                    Body = ex.Message,
                    Category = "User unable to add the  OutcomeHearing for a case",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "User unable to add the  OutcomeHearing for a case",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }

        [MapToApiVersion("1.0")]
        [HttpPost("AssignHearingRollsToLawyer")]
        //<History Author = 'Ijaz Ahmad' Date='2024-03-25' Version="1.0" Branch="master"> Assign Hearing Rolls To Lawyer</History>
        public async Task<IActionResult> AssignHearingRollsToLawyer(AssignHearingRollToLawyerVM hearingRollToLawyerVM)
        {
            try
            {
                await _ICmsMojRPA.AssignHearingRollsToLawyer(hearingRollToLawyerVM);
                return Ok();
            }
            catch (Exception ex)
            {


                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }

        #endregion

        #region Get Hearing Roll Detail

        [MapToApiVersion("1.0")]
        [HttpPost("GetHearingRollDetailForPrintingAndOutcome")]
        //<History Author = 'Hassan Abbas' Date='2024-04-05' Version="1.0" Branch="master"> Get Hearing Roll Detail for Printing and Outcome</History>
        public async Task<IActionResult> GetHearingRollDetailForPrintingAndOutcome(CmsHearingRollDetailSearchVM cmsHearingRollDetailSearch)
        {
            try
            {
                return Ok(await _ICmsMojRPA.GetHearingRollDetailForPrintingAndOutcome(cmsHearingRollDetailSearch));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Save Hearing Roll Outcome As Draft

        [MapToApiVersion("1.0")]
        [HttpPost("SaveHearingRollOutcomesAsDraft")]
        //<History Author = 'Hassan Abbas' Date='2024-04-15' Version="1.0" Branch="master"> Save Hearing Roll Outcomes as Draft</History>
        public async Task<IActionResult> SaveHearingRollOutcomesAsDraft(CmsHearingRollOutcomeDraftPayload payload)
        {
            try
            {
                await _ICmsMojRPA.SaveHearingRollOutcomesAsDraft(payload);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Save Hearing Roll Outcomes

        [MapToApiVersion("1.0")]
        [HttpPost("SaveHearingRollOutcomes")]
        //<History Author = 'Hassan Abbas' Date='2024-04-15' Version="1.0" Branch="master"> Save Hearing Roll Outcomes</History>
        public async Task<IActionResult> SaveHearingRollOutcomes(List<CmsPrintHearingRollDetailVM> hearings)
        {
            try
            {
                await _ICmsMojRPA.SaveHearingRollOutcomes(hearings);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
    }
}
