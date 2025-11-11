using FATWA_API.RabbitMQ;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.Notification;
using FATWA_DOMAIN.Interfaces.Tasks;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.WorkflowModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.LegalLegislationEnum;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_DOMAIN.Enums.UserEnum;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
using Newtonsoft.Json;
using FATWA_DOMAIN.Models.Consultation;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using FATWA_DOMAIN.Interfaces.CaseManagement;
using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models.Dms;
using static FATWA_GENERAL.Helper.Response;
using AutoMapper;
using FATWA_DOMAIN.Interfaces.Common;
using System.Threading.Tasks;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using G2GTarasolServiceReference;
using FATWA_DOMAIN.Interfaces.Consultation;
using System.Data;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_GENERAL.Helper;
using static Org.BouncyCastle.Math.EC.ECCurve;
using FATWA_DOMAIN.Models.LLSLegalPrinciple;
using static FATWA_DOMAIN.Enums.LegalPrinciple.LegalPrincipleEnum;
using FATWA_DOMAIN.Models.ViewModel.Lms;
using System.Reflection.Metadata;
using static MsgReader.Outlook.Storage.Flag;
using DSPExternalAuthenticationService;
using System.Net.Http.Headers;
using System.Text;

namespace FATWA_API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Workflow API Controller for handling calls</History>
    public class WorkflowController : ControllerBase
    {
        #region Variable Declaration

        private readonly IWorkflow _IWorkflow;
        private readonly IWebHostEnvironment _environment;
        private readonly IAuditLog _auditLogs;
        private readonly IConfiguration _configuration;
        private readonly ITask _ITask;
        private readonly INotification _iNotifications;
        private readonly IAccount _IAccount;
        private readonly ICmsRegisteredCase _iCmsRegisteredCase;
        private readonly IRole _IRole;
        private readonly ITempFileUpload _IFileUpload;
        private readonly IMapper _mapper;
        private readonly ILookups _ILookups;
        private readonly IConfiguration _config;
        private readonly RabbitMQClient _client;

        #endregion

        #region Constructor
        public WorkflowController(IWorkflow iWorkflow, ICmsRegisteredCase iCmsRegisteredCase,
            IWebHostEnvironment environment, IAuditLog audit, IConfiguration configuration, ITask iTask, INotification iNotifications, IAccount iAccount,
            IRole iRole, IMapper mapper, ILookups ILookups, IConfiguration config, ITempFileUpload IFileUpload,
            RabbitMQClient client)
        {
            _IWorkflow = iWorkflow;
            _iCmsRegisteredCase = iCmsRegisteredCase;
            _environment = environment;
            _auditLogs = audit;
            _configuration = configuration;
            _ITask = iTask;
            _iNotifications = iNotifications;
            _IAccount = iAccount;
            _IRole = iRole;
            _mapper = mapper;
            _ILookups = ILookups;
            _config = config;
            _IFileUpload = IFileUpload;
            _client = client;
        }
        #endregion

        #region Get Workflows

        [HttpPost("GetWorkflows")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Get Workflows</History>
        public async Task<IActionResult> GetWorkflows(WorkflowAdvanceSearchVM advanceSearchVM)
        {
            try
            {
                return Ok(await _IWorkflow.GetWorkflows(advanceSearchVM));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetWorkflowsCount")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2022-06-02' Version="1.0" Branch="master"> Get Workflow count</History>

        public async Task<IActionResult> GetWorkflowsCount()
        {
            try
            {
                return Ok(await _IWorkflow.GetWorkflowsCount());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetWorkflowsInstanceCount")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetWorkflowsInstanceCount(int workflowId)
        {
            try
            {
                return Ok(await _IWorkflow.GetWorkflowsInstanceCount(workflowId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });

            }
        }

        [HttpGet("GetWorkflowDetailById")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Get Workflow detail</History>
        public async Task<IActionResult> GetWorkflowDetailById([FromForm] int workflowId)
        {
            try
            {
                return Ok(await _IWorkflow.GetWorkflowDetailById(workflowId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetWorkflowTriggerByWorkflowId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Get Workflow trigger detail</History>
        public async Task<IActionResult> GetWorkflowTriggerByWorkflowId([FromForm] int workflowId)
        {
            try
            {
                return Ok(await _IWorkflow.GetWorkflowTriggerByWorkflowId(workflowId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetActiveWorkflows")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Get active Workflows</History>
        public async Task<IActionResult> GetActiveWorkflows([FromForm] int moduleId, [FromForm] int moduleTriggerId, [FromForm] int? attachmentTypeId, [FromForm] int? submoduleId)
        {
            try
            {
                return Ok(await _IWorkflow.GetActiveWorkflows(moduleTriggerId, attachmentTypeId, submoduleId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetWorkflowModules")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Get Workflow modules</History>
        public async Task<IActionResult> GetWorkflowModules()
        {
            try
            {
                return Ok(await _IWorkflow.GetWorkflowModules());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetModuleTriggers")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Get Module Triggers</History>
        public async Task<IActionResult> GetModuleTriggers(int submoduleId)
        {
            try
            {
                return Ok(await _IWorkflow.GetModuleTriggers(submoduleId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetSubModuleTriggers")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Noman khan' Date='2022-06-02' Version="1.0" Branch="master"> Get Submodule Module Triggers</History>
        public async Task<IActionResult> GetSubModuleTriggers(int moduleId)
        {
            try
            {
                return Ok(await _IWorkflow.GetSubModuleTriggers(moduleId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetModuleTriggerById")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2023-01-12'  Version="1.0" Branch="master"> Get Module Triggers by Id</History>
        public async Task<IActionResult> GetModuleTriggerById(int triggerId)
        {
            try
            {
                return Ok(await _IWorkflow.GetModuleTriggerById(triggerId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetAttachementTypesById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAttachementTypesById(int workflowId)
        {
            try
            {
                return Ok(await _IWorkflow.GetAttachementTypesById(workflowId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetModuleConditions")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2023-01-12'  Version="1.0" Branch="master"> Get Module Conditions</History>
        public async Task<IActionResult> GetModuleConditions(int triggerId)
        {
            try
            {
                return Ok(await _IWorkflow.GetModuleConditions(triggerId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetModuleActvities")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2023-01-12'  Version="1.0" Branch="master"> Get Module Activities</History>
        public async Task<IActionResult> GetModuleActvities(int triggerId)
        {
            try
            {
                return Ok(await _IWorkflow.GetModuleActvities(triggerId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetModuleActvitiesByCategory")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2023-01-12'  Version="1.0" Branch="master"> Get Module Activities By Category</History>
        public async Task<IActionResult> GetModuleActvitiesByCategory([FromForm] int triggerId, [FromForm] int categoryId)
        {
            try
            {
                return Ok(await _IWorkflow.GetModuleActvitiesByCategory(triggerId, categoryId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetModuleActivityParameters")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2023-01-12' Version="1.0" Branch="master"> Get Module Activity Parameters</History>
        public async Task<IActionResult> GetModuleActivityParameters([FromForm] int activityId)
        {
            try
            {
                return Ok(await _IWorkflow.GetModuleActivityParameters(activityId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetWorkflowStatuses")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Get Workflow Statuses</History>
        public async Task<IActionResult> GetWorkflowStatuses()
        {
            try
            {
                return Ok(await _IWorkflow.GetWorkflowStatuses());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetModuleOptionsByTriggerId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2023-04-12' Version="1.0" Branch="master"> Get module options by trigger id</History>
        public async Task<IActionResult> GetModuleOptionsByTriggerId(int triggerId)
        {
            try
            {
                return Ok(await _IWorkflow.GetModuleOptionsByTriggerId(triggerId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetWorkflowSectorTransferOptions")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Get Workflow sector transfer option</History>
        public async Task<IActionResult> GetWorkflowSectorTransferOptions(int workflowTriggerId, int sectorTypeId)
        {
            try
            {
                return Ok(await _IWorkflow.GetWorkflowSectorTransferOptions(workflowTriggerId, sectorTypeId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpGet("GetWorkflowTriggerConditionsByTriggerId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2023-04-12' Version="1.0" Branch="master"> Get Workflow trigger conditions by trigger id</History>
        public async Task<IActionResult> GetWorkflowTriggerConditionsByTriggerId(int TriggerId)
        {
            try
            {
                return Ok(await _IWorkflow.GetWorkflowTriggerConditionsByTriggerId(TriggerId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetWorkflowTriggerSectorOptions")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2023-04-12' Version="1.0" Branch="master"> Get Workflow trigger sector options for edit</History>
        public async Task<IActionResult> GetWorkflowTriggerSectorOptions(int TriggerId)
        {
            try
            {
                return Ok(await _IWorkflow.GetWorkflowTriggerSectorOptions(TriggerId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetWorkflowTriggerSectorTransferOptions")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2023-04-12' Version="1.0" Branch="master"> Get Workflow triigger  sector transfer options for edit</History>
        public async Task<IActionResult> GetWorkflowTriggerSectorTransferOptions(int TriggerOptionId)
        {
            try
            {
                return Ok(await _IWorkflow.GetWorkflowTriggerSectorTransferOptions(TriggerOptionId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetActivtySlAsByActivityId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2023-04-12' Version="1.0" Branch="master"> Get Workflow Activity SLAs for edit </History>
        public async Task<IActionResult> GetActivtySlAsByActivityId(int WorkflowActivityId)
        {
            try
            {
                return Ok(await _IWorkflow.GetActivtySlAsByActivityId(WorkflowActivityId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetActivtySLAsActionParameterBySLAId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2023-04-12' Version="1.0" Branch="master"> Get Workflow Activity SLAs Action Parameters for edit</History>
        public async Task<IActionResult> GetActivtySLAsActionParameterBySLAId(int WorkflowSLAId)
        {
            try
            {
                return Ok(await _IWorkflow.GetActivtySLAsActionParameterBySLAId(WorkflowSLAId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        #endregion

        #region Create Workflow

        [HttpPost("CreateWorkflow")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Create Workflow</History>
        public async Task<IActionResult> CreateWorkflow(Workflow workflow)
        {
            try
            {
                await _IWorkflow.CreateWorkflow(workflow);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "To create the workflow",
                    Task = "To create the workflow",
                    Description = "Workflow has been created",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Workflow created successfully",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.Workflow,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok();
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "To create the workflow",
                    Body = ex.Message,
                    Category = "User unable to create the workflow",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "The workflow could not be created",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.Workflow,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });
            }
        }

        #endregion

        #region SLA

        [HttpGet("GetSlaActionParameters")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Create Sla parameters</History>
        public async Task<IActionResult> GetSlaActionParameters([FromForm] int actionId)
        {
            try
            {
                return Ok(await _IWorkflow.GetSlaActionParameters(actionId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Workflow Activities

        [HttpGet("GetWorkflowActivities")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Get workflow activities</History>
        public async Task<IActionResult> GetWorkflowActivities([FromForm] int workflowId)
        {
            try
            {
                return Ok(await _IWorkflow.GetWorkflowActivities(workflowId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetWorkflowTriggerConditions")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2023-10-29' Version="1.0" Branch="master"> Get workflow Trigger Conditions</History>
        public async Task<IActionResult> GetWorkflowTriggerConditions(int workflowTriggerId)
        {
            try
            {
                return Ok(await _IWorkflow.GetWorkflowTriggerConditions(workflowTriggerId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetWorkflowActivitiesByWorkflowId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Get workflow activities by Workflow Id</History>
        public async Task<IActionResult> GetWorkflowActivitiesByWorkflowId([FromForm] int workflowId)
        {
            try
            {
                return Ok(await _IWorkflow.GetWorkflowActivitiesByWorkflowId(workflowId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetToDoWorkflowActivities")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Get to do workflow activities</History>
        public async Task<IActionResult> GetToDoWorkflowActivities([FromForm] int workflowId, [FromForm] int sequenceNumber)
        {
            try
            {
                return Ok(await _IWorkflow.GetToDoWorkflowActivities(workflowId, sequenceNumber));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetWorkflowActivityById")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Get workflow activity by Id</History>
        public async Task<IActionResult> GetWorkflowActivityById([FromForm] int workflowActivityId)
        {
            try
            {
                return Ok(await _IWorkflow.GetWorkflowActivityById(workflowActivityId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetWorkflowActivityBySequenceNumber")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Get workflow activity by Sequence Number</History>
        public async Task<IActionResult> GetWorkflowActivityBySequenceNumber([FromForm] int workflowId, [FromForm] int sequenceNumber)
        {
            try
            {
                return Ok(await _IWorkflow.GetWorkflowActivityBySequenceNumber(workflowId, sequenceNumber));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetWorkflowConditions")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Get Workflow Conditions</History>
        public async Task<IActionResult> GetWorkflowConditions([FromForm] int workflowActivityId)
        {
            try
            {
                return Ok(await _IWorkflow.GetWorkflowConditions(workflowActivityId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetWorkflowOptions")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Get Workflow Conditions</History>
        public async Task<IActionResult> GetWorkflowOptions([FromForm] int workflowActivityId)
        {
            try
            {
                return Ok(await _IWorkflow.GetWorkflowOption(workflowActivityId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetWorkflowConditionOptionsList")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Get Workflow Conditions</History>
        public async Task<IActionResult> GetWorkflowConditionOptionsList([FromForm] int workflowConditionId)
        {
            try
            {
                return Ok(await _IWorkflow.GetWorkflowConditionOptionList(workflowConditionId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("GetWorkflowActivityParameters")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Get Workflow Activity Parameters</History>
        public async Task<IActionResult> GetWorkflowActivityParameters(int workflowActivityId, int? TriggerId, dynamic entity)
        {
            try
            {
                return Ok(await _IWorkflow.GetWorkflowActivityParameters(workflowActivityId, TriggerId, entity));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetWorkflowActivityParametersForUpdate")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Get Workflow Activity Parameters</History>
        public async Task<IActionResult> GetWorkflowActivityParametersForUpdate(int workflowActivityId)
        {
            try
            {
                return Ok(await _IWorkflow.GetWorkflowActivityParameters(workflowActivityId, 0, null));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Workflow Instance


        [HttpGet("GetWorkflowInstanceDocuments")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Get Workflow Document Instances</History>
        public async Task<IActionResult> GetWorkflowInstanceDocuments(int PageSize, int PageNumber)
        {
            try
            {
                return Ok(await _IWorkflow.GetWorkflowInstanceDocuments(PageSize, PageNumber));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetInstanceCurrentActivity")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Get Current Activity of Instance</History>
        public async Task<IActionResult> GetInstanceCurrentActivity([FromForm] Guid referenceId)
        {
            try
            {
                return Ok(await _IWorkflow.GetInstanceCurrentActivity(referenceId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("UpdateWorkflowInstanceStatus")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Update status of workflow instance</History>
        public async Task<IActionResult> UpdateWorkflowInstanceStatus([FromForm] Guid referenceId, [FromForm] int statusId)
        {
            try
            {
                await _IWorkflow.UpdateWorkflowInstanceStatus(referenceId, statusId);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for update workflow instance status",
                    Task = "To update the request",
                    Description = "User able to update workflow status Request successfully.",
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
                    Subject = "Request for update workflow instance status Failed",
                    Body = ex.Message,
                    Category = "User unable to update workflow status Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for update workflow status Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetCurrentInstanceByReferneceId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2022-06-02' Version="1.0" Branch="master"> Update status of workflow instance</History>
        public async Task<IActionResult> GetCurrentInstanceByReferneceId(Guid referenceId)
        {
            try
            {
                return Ok(await _IWorkflow.GetCurrentInstanceByReferneceId(referenceId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetWorkflowConditionOptions")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Get Workflow Activity Parameters</History>
        public async Task<IActionResult> GetWorkflowConditionOptions(Guid ReferneceId, int StatusId)
        {
            try
            {
                return Ok(await _IWorkflow.GetWorkflowConditionOptions(ReferneceId, StatusId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetWorkflowActivityOptions")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Get Workflow Activity Parameters</History>
        public async Task<IActionResult> GetWorkflowActivityOptions(int ActivityId)
        {
            try
            {
                return Ok(await _IWorkflow.GetWorkflowActivityOptions(ActivityId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Update Workflow Status

        [HttpPost("UpdateWorkflowStatus")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Update status of workflow</History>
        public async Task<IActionResult> UpdateWorkflowStatus([FromForm] int workflowId, [FromForm] int statusId)
        {
            try
            {
                await _IWorkflow.UpdateWorkflowStatus(workflowId, statusId);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for update workflow status",
                    Task = "To update the request",
                    Description = "User able to update workflow status Request successfully.",
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
                    Subject = "Request for update workflow status Failed",
                    Body = ex.Message,
                    Category = "User unable update workflow status Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for update workflow status Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetWorkflowforSuspend")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetWorkflowforSuspend(int workflowId, int statusId)
        {
            try
            {
                return Ok(await _IWorkflow.GetActiveWorkflowforSuspend(workflowId, statusId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region legal  legislation

        [HttpPost("UpdateDocumentInstance")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Update document instance</History>
        public async Task<IActionResult> UpdateDocumentInstance(LegalLegislation document)
        {
            try
            {
                await _IWorkflow.UpdateDocumentInstance(document);
                var UsersList = await _IAccount.GetUsersByRoleId(document.RoleId);
                if (document.Legislation_Flow_Status == (int)LegislationFlowStatusEnum.InReview)
                {
                    foreach (var user in UsersList)
                    {
                        var notificationResponse = await _iNotifications.SendNotification(new Notification
                        {
                            NotificationId = Guid.NewGuid(),
                            DueDate = DateTime.Now.AddDays(5),
                            CreatedBy = document.SenderEmail != null ? document.SenderEmail : document.ModifiedBy != null ? document.ModifiedBy : document.AddedBy,
                            CreatedDate = DateTime.Now,
                            IsDeleted = false,
                            ReceiverId = user,// Send  Id
                            ModuleId = (int)WorkflowModuleEnum.LDSDocument,
                        },
                        (int)NotificationEventEnum.SaveLegalLegislation,
                        "detailview",
                        document.GetType().Name,
                        document.LegislationId.ToString(),
                        document.NotificationParameter);
                    }
                }
                else if (document.Legislation_Flow_Status == (int)LegislationFlowStatusEnum.NeedModification)
                {
                    var notificationResult = await _iNotifications.SendNotification(new Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        DueDate = DateTime.Now.AddDays(5),
                        CreatedBy = document.SenderEmail != null ? document.SenderEmail : document.ModifiedBy != null ? document.ModifiedBy : document.AddedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReceiverId = await _IAccount.UserIdByUserEmail(document.AddedBy),
                        ModuleId = (int)WorkflowModuleEnum.LDSDocument,
                    },
                        (int)NotificationEventEnum.UpdateLegalLegislation,
                       "detailview",
                       document.GetType().Name,
                       document.LegislationId.ToString(),
                       document.NotificationParameter);
                }
                else if (document.Legislation_Flow_Status == (int)LegislationFlowStatusEnum.Published)
                {
                    if (UsersList != null)
                    {
                        foreach (var user in UsersList)
                        {
                            var notificationResponse = await _iNotifications.SendNotification(new Notification
                            {
                                NotificationId = Guid.NewGuid(),
                                DueDate = DateTime.Now.AddDays(5),
                                CreatedBy = document.SenderEmail != null ? document.SenderEmail : document.ModifiedBy != null ? document.ModifiedBy : document.AddedBy,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReceiverId = user,// Send  Id
                                ModuleId = (int)WorkflowModuleEnum.LDSDocument,
                            },
                            (int)NotificationEventEnum.SaveLegalLegislation,
                            "detailview",
                            document.GetType().Name,
                            document.LegislationId.ToString(),
                            document.NotificationParameter);
                        }
                    }

                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Publish Legal Legislation",
                        Task = "Publish Legal Legislation",
                        Description = "User able to Publish Legal Legislation successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Publish Legal Legislation executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)WorkflowModuleEnum.LDSDocument,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });
                }
                else if (document.Legislation_Flow_Status == (int)LegislationFlowStatusEnum.Unpublished)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "UnPublish Legal Legislation",
                        Task = "UnPublish Legal Legislation",
                        Description = "User able to Publish Legal Legislation successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "UnPublish Legal Legislation executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)WorkflowModuleEnum.LDSDocument,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });

                }
                else if (document.Legislation_Flow_Status == (int)LegislationFlowStatusEnum.Approved)
                {
                    if (UsersList != null)
                    {
                        foreach (var user in UsersList)
                        {
                            var notificationResponse = await _iNotifications.SendNotification(new Notification
                            {
                                NotificationId = Guid.NewGuid(),
                                DueDate = DateTime.Now.AddDays(5),
                                CreatedBy = document.SenderEmail != null ? document.SenderEmail : document.ModifiedBy != null ? document.ModifiedBy : document.AddedBy,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReceiverId = user,// Send  Id
                                ModuleId = (int)WorkflowModuleEnum.LDSDocument,
                            },
                            (int)NotificationEventEnum.SaveLegalLegislation,
                            "detailview",
                            document.GetType().Name,
                            document.LegislationId.ToString(),
                            document.NotificationParameter);
                        }
                    }
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Approve Legal Legislation",
                        Task = "Approve Legal Legislation",
                        Description = "User able to Approve Legal Legislation successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Approve Legal Legislation executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)WorkflowModuleEnum.LDSDocument,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });
                }
                else if (document.Legislation_Flow_Status == (int)LegislationFlowStatusEnum.Rejected)
                {
                    var notificationResult = await _iNotifications.SendNotification(new Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        DueDate = DateTime.Now.AddDays(5),
                        CreatedBy = document.SenderEmail != null ? document.SenderEmail : document.ModifiedBy != null ? document.ModifiedBy : document.AddedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReceiverId = await _IAccount.UserIdByUserEmail(document.AddedBy),
                        ModuleId = (int)WorkflowModuleEnum.LDSDocument,
                    },
                        (int)NotificationEventEnum.UpdateLegalLegislation,
                    "detailview",
                    document.GetType().Name,
                       document.LegislationId.ToString(),
                       document.NotificationParameter);

                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Reject Legal Legislation",
                        Task = "Reject Legal Legislation",
                        Description = "User able to Reject Legal Legislation successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Reject Legal Legislation executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)WorkflowModuleEnum.LDSDocument,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });
                }
                else if (document.Legislation_Flow_Status == (int)LegislationFlowStatusEnum.NeedToModify || document.Legislation_Flow_Status == (int)LegislationFlowStatusEnum.SendAComment)
                {
                    var notificationResult = await _iNotifications.SendNotification(new Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        DueDate = DateTime.Now.AddDays(5),
                        CreatedBy = document.SenderEmail != null ? document.SenderEmail : document.ModifiedBy != null ? document.ModifiedBy : document.AddedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReceiverId = await _IAccount.UserIdByUserEmail(document.AddedBy),
                        ModuleId = (int)WorkflowModuleEnum.LDSDocument,
                    },
                        (int)NotificationEventEnum.UpdateLegalLegislation,
                    "detailview",
                    document.GetType().Name,
                       document.LegislationId.ToString(),
                       document.NotificationParameter);

                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Reviewed Legal Legislation and Send to need modification",
                        Task = "Reviewed Legal Legislation and Send to need modification",
                        Description = "Reviewed Legal Legislation and Sent to need modification successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Review Legal Legislation and Send to need modification executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)WorkflowModuleEnum.LDSDocument,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Take Decision On Legal Legislation Failed",
                    Body = ex.Message,
                    Category = "User unable to Take Decision On LDS Document",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Take Decision On Legal Legislation Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LDSDocument,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Lps Principle

        //[AllowAnonymous]
        [HttpPost("UpdatePrincipleInstance")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Update principle instance</History>
        public async Task<IActionResult> UpdatePrincipleInstance(LLSLegalPrincipleSystem principle)
        {
            try
            {
                await _IWorkflow.UpdatePrincipleInstance(principle);
                var UsersList = await _IAccount.GetUsersByRoleId(principle.RoleId);
                if (principle.FlowStatus == (int)PrincipleFlowStatusEnum.InReview)
                {
                    foreach (var user in UsersList)
                    {
                        var notificationResponse = await _iNotifications.SendNotification(new Notification
                        {
                            NotificationId = Guid.NewGuid(),
                            DueDate = DateTime.Now.AddDays(5),
                            CreatedBy = principle.SenderEmail,
                            CreatedDate = DateTime.Now,
                            IsDeleted = false,
                            ReceiverId = user,// Send  Id
                            ModuleId = (int)WorkflowModuleEnum.LDSDocument,
                        },
                        (int)NotificationEventEnum.SaveLegalPrinciple,
                        "llsleagalprinciple",
                        "detail",
                        principle.PrincipleId.ToString(),
                        principle.NotificationParameter);
                    }
                }
                else if (principle.FlowStatus == (int)PrincipleFlowStatusEnum.NeedModification)
                {
                    var entity = "principle-content";
                    var notificationResult = await _iNotifications.SendNotification(new Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        DueDate = DateTime.Now.AddDays(5),
                        CreatedBy = principle.SenderEmail,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReceiverId = await _IAccount.UserIdByUserEmail(principle.CreatedBy),
                        ModuleId = (int)WorkflowModuleEnum.LDSDocument,
                    },
                       (int)NotificationEventEnum.UpdateLegalPrinciple,
                        entity,
                        "details",
                        principle.PrincipleId.ToString(),
                        principle.NotificationParameter);
                }
                else if (principle.FlowStatus == (int)PrincipleFlowStatusEnum.Publish)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Publish LPS Principle",
                        Task = "Publish LPS Principle",
                        Description = "User able to Publish LPS Principle successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Publish LPS Principle executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });
                }
                else if (principle.FlowStatus == (int)PrincipleFlowStatusEnum.Unpublished)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "UnPublish LPS Principle",
                        Task = "UnPublish LPS Principle",
                        Description = "User able to Unpublish LPS Principle successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "UnPublish LPS Principle executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });

                }
                else if (principle.FlowStatus == (int)PrincipleFlowStatusEnum.Approve)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Approve LPS Principle",
                        Task = "Approve LPS Principle",
                        Description = "User able to Approve LPS Principle successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Approve LPS Principle executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });
                }
                else if (principle.FlowStatus == (int)PrincipleFlowStatusEnum.Reject)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Reject LPS Principle",
                        Task = "Reject LPS Principle",
                        Description = "User able to Reject LPS Principle successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Reject LPS Principle executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });
                }
                else if (principle.FlowStatus == (int)PrincipleFlowStatusEnum.NeedToModify || principle.FlowStatus == (int)PrincipleFlowStatusEnum.SendAComment)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Reviewed LPS Principle and Send to need modification",
                        Task = "Reviewed LPS Principle and Send to need modification",
                        Description = "Reviewed LPS Principle and Sent to need modification successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Review LPS Principle and Send to need modification executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });
                }

                return Ok();
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Take Decision On LPS Principle Failed",
                    Body = ex.Message,
                    Category = "User unable to Take Decision On LPS Principle",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Take Decision On LPS Principle Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region cms draft insatance

        //[AllowAnonymous]
        [HttpPost("UpdateCaseDraftInstance")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Update principle instance</History>
        public async Task<IActionResult> UpdateCaseDraftInstance(CmsDraftedTemplate draft, string userName)
        {
            try
            {
                userName = draft.userName;
                string userId = await _IAccount.UserIdByUserEmail(userName);
                WorkflowActivity workflowActivity = new WorkflowActivity();
                if (draft.WorkflowActivityId > 0)
                {
                    workflowActivity = await _IWorkflow.GetWorkflowActivityById((int)draft.WorkflowActivityId);

                }
                draft.LawyerId = await _IAccount.UserIdByUserEmail(draft?.DraftedTemplateVersion.CreatedBy);
                draft.Payload = await _IWorkflow.UpdateCaseDraftInstance(draft, userName);

                if (!String.IsNullOrEmpty(draft.DraftedTemplateVersion.ReviewerUserId) || !String.IsNullOrEmpty(draft.DraftedTemplateVersion.ReviewerRoleId))
                {
                    if (!String.IsNullOrEmpty(draft.DraftedTemplateVersion.ReviewerRoleId))
                    {
                        List<User> viceHosList = await _IWorkflow.GetViceHOSBySectorId(draft.SectorTypeId);
                        foreach (var reviewUser in viceHosList)
                        {
                            var taskIds = Guid.NewGuid();
                            if (workflowActivity.IsTask == true)
                            {
                                var taskResult = await _ITask.AddTask(new SaveTaskVM
                                {
                                    Task = new UserTask
                                    {
                                        TaskId = taskIds,
                                        Name = "Review_Draft_Document_Task",
                                        Description = "",
                                        Date = DateTime.Now.Date,
                                        AssignedBy = userId,
                                        AssignedTo = reviewUser.Id,
                                        IsSystemGenerated = true,
                                        TaskStatusId = (int)TaskStatusEnum.Pending,
                                        ModuleId = draft.ModuleId,
                                        SectorId = draft.SectorTypeId,
                                        DepartmentId = (int)DepartmentEnum.Operational,
                                        TypeId = (int)TaskTypeEnum.Task,
                                        RoleId = "d6b3075c-3f70-4b44-baa4-1fdc599a6bb2", //FATWA ADMIN
                                        CreatedBy = draft.userName,
                                        CreatedDate = DateTime.Now,
                                        IsDeleted = false,
                                        ReferenceId = draft.Id,
                                        SubModuleId = CalculateSubmodule(draft.DraftEntityType != null ? (int)draft.DraftEntityType : 0, draft.ModuleId),
                                        SystemGenTypeId = CalculateSystemGenTypeId(draft.DraftEntityType != null ? (int)draft.DraftEntityType : 0, draft.ModuleId),
                                        EntityId = null
                                    },
                                    TaskActions = new List<TaskAction>()
                                    {
                                new TaskAction()
                                {
                                    ActionName = "Review Draft Document",
                                    TaskId = taskIds,
                                }
                                    }
                                },
                                "detail",
                                "draftdocument",
                                draft.Id.ToString() + '/' + draft.DraftedTemplateVersion.VersionId);

                            }
                            if (workflowActivity.IsNotification == true)
                            {
                                var notificationResponse = await _iNotifications.SendNotification(new Notification
                                {
                                    NotificationId = Guid.NewGuid(),
                                    DueDate = DateTime.Now.AddDays(5),
                                    CreatedBy = userName,
                                    CreatedDate = DateTime.Now,
                                    IsDeleted = false,
                                    ReceiverId = reviewUser.Id,// Assign To  Id
                                    ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                                },
                                workflowActivity.ActivityId == (int)WorkflowActivityEnum.LawyerDraftModification ? (int)NotificationEventEnum.ModifyDraft : (int)NotificationEventEnum.ReviewDraft,
                                "detail",
                                "draftdocument",
                                draft.Id.ToString() + '/' + draft.DraftedTemplateVersion.VersionId + '/' + taskIds,
                                draft.NotificationParameter);
                            }
                        }
                    }
                    else
                    {
                        var taskId = Guid.NewGuid();
                        if (workflowActivity.IsTask == true)
                        {
                            var taskResult = await _ITask.AddTask(new SaveTaskVM
                            {
                                Task = new UserTask
                                {
                                    TaskId = taskId,
                                    Name = "Review_Draft_Document_Task",
                                    Description = "",
                                    Date = DateTime.Now.Date,
                                    AssignedBy = userId,
                                    AssignedTo = draft?.DraftedTemplateVersion.ReviewerUserId,
                                    IsSystemGenerated = true,
                                    TaskStatusId = (int)TaskStatusEnum.Pending,
                                    ModuleId = draft.ModuleId,
                                    SectorId = draft.SectorTypeId,
                                    DepartmentId = (int)DepartmentEnum.Operational,
                                    TypeId = (int)TaskTypeEnum.Task,
                                    RoleId = "d6b3075c-3f70-4b44-baa4-1fdc599a6bb2", //FATWA ADMIN
                                    CreatedBy = draft.userName,
                                    CreatedDate = DateTime.Now,
                                    IsDeleted = false,
                                    ReferenceId = draft.Id,
                                    SubModuleId = CalculateSubmodule(draft.DraftEntityType != null ? (int)draft.DraftEntityType : 0, draft.ModuleId),
                                    SystemGenTypeId = CalculateSystemGenTypeId(draft.DraftEntityType != null ? (int)draft.DraftEntityType : 0, draft.ModuleId),
                                    EntityId = null
                                },
                                TaskActions = new List<TaskAction>()
                            {
                                new TaskAction()
                                {
                                    ActionName = "Review Draft Document",
                                    TaskId = taskId,
                                }
                            }
                            },
                            "detail",
                            "draftdocument",
                            draft.Id.ToString() + '/' + draft.DraftedTemplateVersion.VersionId);
                        }
                        if (workflowActivity.IsNotification == true)
                        {
                            var notificationResponse = await _iNotifications.SendNotification(new Notification
                            {
                                NotificationId = Guid.NewGuid(),
                                DueDate = DateTime.Now.AddDays(5),
                                CreatedBy = userName,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReceiverId = draft?.DraftedTemplateVersion.ReviewerUserId,// Assign To  Id
                                ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                            },
                            workflowActivity.ActivityId == (int)WorkflowActivityEnum.LawyerDraftModification ? (int)NotificationEventEnum.ModifyDraft : (int)NotificationEventEnum.ReviewDraft,
                            "detail",
                            "draftdocument",
                            draft.Id.ToString() + '/' + draft.DraftedTemplateVersion.VersionId + '/' + taskId,
                            draft.NotificationParameter);
                        }
                    }
                }
                if (draft.IsEndofFlow == true)
                {
                    string userInitiatorId = await _IAccount.UserIdByUserEmail(draft?.CreatedBy);
                    var notificationResponse = await _iNotifications.SendNotification(new Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        DueDate = DateTime.Now.AddDays(5),
                        CreatedBy = draft.ModifiedBy != null ? draft.ModifiedBy : draft.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReceiverId = userInitiatorId,// Assign To  Id
                        ModuleId = draft.ModuleId,
                    },
                    (int)NotificationEventEnum.PublishedDraft,
                    "detail",
                    "draftdocument",
                    draft.Id.ToString() + '/' + draft.DraftedTemplateVersion.VersionId,
                    draft.NotificationParameter);
                }
                if (draft.IsLawyerTask == true)
                {
                    string AssignedTo = await _IAccount.UserIdByUserEmail(draft?.CreatedBy);
                    string AssignedBy = await _IAccount.UserIdByUserEmail(draft?.DraftedTemplateVersion.CreatedBy);
                    var taskId = Guid.NewGuid();
                    var taskResult = await _ITask.AddTask(new SaveTaskVM
                    {
                        Task = new UserTask
                        {
                            TaskId = taskId,
                            Name = "Reject_Review_Draft_Document",
                            Description = "",
                            Date = DateTime.Now.Date,
                            AssignedBy = AssignedBy,
                            AssignedTo = AssignedTo,
                            IsSystemGenerated = true,
                            TaskStatusId = (int)TaskStatusEnum.Pending,
                            ModuleId = draft.ModuleId,
                            SectorId = draft.SectorTypeId,
                            DepartmentId = (int)DepartmentEnum.Operational,
                            TypeId = (int)TaskTypeEnum.Task,
                            RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                            CreatedBy = draft.userName,
                            CreatedDate = DateTime.Now,
                            IsDeleted = false,
                            ReferenceId = draft.Id,
                            SubModuleId = CalculateSubmodule(draft.DraftEntityType != null ? (int)draft.DraftEntityType : 0, draft.ModuleId),
                            SystemGenTypeId = CalculateSystemGenTypeId(draft.DraftEntityType != null ? (int)draft.DraftEntityType : 0, draft.ModuleId),
                            EntityId = null
                        },
                        TaskActions = new List<TaskAction>()
                        {
                            new TaskAction()
                            {
                                ActionName = "Review Draft Document",
                                TaskId = taskId,
                            }
                        }
                    },
                    "detail",
                    "draftdocument",
                    draft.Id.ToString() + '/' + draft.DraftedTemplateVersion.VersionId);
                }

                if (draft.DraftedTemplateVersion.StatusId == (int)DraftVersionStatusEnum.Published && draft.IsG2GSend == true)//&& (draft.DraftEntityType == (int)DraftEntityTypeEnum.RequestNeedMoreInfo || draft.DraftEntityType == (int)DraftEntityTypeEnum.FileNeedMoreInfo || draft.DraftEntityType == (int)DraftEntityTypeEnum.CaseNeedMoreInfo)
                {
                    if (draft.Payload != null)
                    {
                        List<SendCommunicationVM> sendCommunication = JsonConvert.DeserializeObject<List<SendCommunicationVM>>(draft.Payload);
                        await Task.Delay(2000);

                        //send G2G Tarasol Correspondence
#if !DEBUG
                        try
                        {
                            if (_config["Environment"] != "QA" && _config["Environment"] != "DPS")
                            {
                                var comm = sendCommunication.FirstOrDefault();
                                string subject = string.Empty;
                                if (comm != null)
                                {
                                    subject = await _iCmsRegisteredCase.GetFileAndCommunicationTypeInfo(comm.Communication.CommunicationTypeId, draft.ReferenceId);
                                }
                                await SendCommunicationToTarassol(sendCommunication, subject);
                            }
                        }
                        catch (Exception)
                        { 
                        }
#endif
                        foreach (SendCommunicationVM sendCommunicationVM in sendCommunication)
                        {
                            //Rabbit MQ send Messages
                            _client.SendMessage(sendCommunicationVM, RabbitMQKeys.CommunicationKey);
                            // Update Case Request Status
                            _client.SendMessage(draft.UpdateEntity, RabbitMQKeys.RequestStatusKey);
                        }
                    }
                }

                if (draft.DraftedTemplateVersion.StatusId == (int)DraftVersionStatusEnum.Published && draft.DraftEntityType == (int)DraftEntityTypeEnum.HearingSchedulingCourtVisit)
                {
                    Hearing hearing = JsonConvert.DeserializeObject<Hearing>(draft.Payload);

                    if (hearing.SendPortfolioRequestMoj == true)
                    {

                        string assignedBy = await _IAccount.UserIdByUserEmail(hearing.CreatedBy);
                        string SectorTypeId = await _IAccount.UserSectorTypeIdByUserEmail(hearing.CreatedBy);
                        //string assignedTo = await _IAccount.UserIdByUserEmail(.ClaimStatementCreatedBy);
                        var document = await _iCmsRegisteredCase.GetMojBySectorId(Convert.ToInt32(SectorTypeId));
                        var casenumber = await _iCmsRegisteredCase.GetRegisteredCaseDetailByIdVM(hearing.CaseId, document.Id);
                        var taskId = Guid.NewGuid();
                        var taskResult = await _ITask.AddTask(new SaveTaskVM
                        {
                            Task = new UserTask
                            {
                                TaskId = taskId,
                                Name = "Document_Portfolio_Request_Created",
                                Description = "",
                                Date = DateTime.Now.Date,
                                AssignedBy = assignedBy,
                                AssignedTo = document.Id,
                                IsSystemGenerated = true,
                                TaskStatusId = (int)TaskStatusEnum.Pending,
                                ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                                SectorId = 1,
                                DepartmentId = 1,
                                TypeId = (int)TaskTypeEnum.Task,
                                RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                                CreatedBy = assignedBy,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReferenceId = hearing.RequestForDocument.Id,
                                SubModuleId = (int)SubModuleEnum.CaseRequest,
                                SystemGenTypeId = (int)TaskSystemGenTypeEnum.DraftDocumentReview,
                                EntityId = 1,
                            },
                            TaskActions = new List<TaskAction>()
                            {
                                new TaskAction()
                                {
                                    ActionName = "portfolio document request",
                                    TaskId = taskId,
                                }
                            }
                        },
                        "request",
                        "detail-portfolio",
                          hearing.RequestForDocument.Id.ToString() + "\\" + casenumber.CaseNumber.ToString());

                    }
                    await CreateHearingTaskForLawyer(hearing);
                    //Rabbit MQ send Messages
                    _client.SendMessage(hearing, RabbitMQKeys.HearingKey);
                }
                if (draft.AttachmentTypeId == (int)AttachmentTypeEnum.RequestForStopExecutionOfJudgment && draft.DraftedTemplateVersion.StatusId == (int)DraftVersionStatusEnum.Published)
                {
                    StopExecutionPayloadVM ExePayload = JsonConvert.DeserializeObject<StopExecutionPayloadVM>(draft.Payload);
                    User assignedTo = await _IRole.GetHOSBySectorId((int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases);
                    var taskId = Guid.NewGuid();
                    var taskResult = await _ITask.AddTask(new SaveTaskVM
                    {
                        Task = new UserTask
                        {
                            TaskId = taskId,
                            Name = "Review_Request_For_Stop_Execution_of_Judgment",
                            Description = "",
                            Date = DateTime.Now.Date,
                            AssignedBy = userId,
                            AssignedTo = assignedTo.Id,
                            IsSystemGenerated = true,
                            TaskStatusId = (int)TaskStatusEnum.Pending,
                            ModuleId = draft.ModuleId,
                            SectorId = draft.SectorTypeId,
                            DepartmentId = (int)DepartmentEnum.Operational,
                            TypeId = (int)TaskTypeEnum.Task,
                            RoleId = "d6b3075c-3f70-4b44-baa4-1fdc599a6bb2", //FATWA ADMIN
                            CreatedBy = draft.userName,
                            CreatedDate = DateTime.Now,
                            IsDeleted = false,
                            ReferenceId = draft.Id,
                            SubModuleId = CalculateSubmodule(draft.DraftEntityType != null ? (int)draft.DraftEntityType : 0, draft.ModuleId),
                            SystemGenTypeId = CalculateSystemGenTypeId(draft.DraftEntityType != null ? (int)draft.DraftEntityType : 0, draft.ModuleId),
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
                }
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "To update the case draft  instance",
                    Task = "To update the case draft instance",
                    Description = "Case draft instance has been updated.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Draft has been updated sucessfully",
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
                    Subject = "Update case draft  instance Failed",
                    Body = ex.Message,
                    Category = "User unable to update the case  draft  instance",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "The draft could not be updated",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });
            }
        }
        #endregion

        #region Function To call Tarassol Service
        private async Task SendCommunicationToTarassol(List<SendCommunicationVM> sendCommunication, string subject)
        {
            try
            {
                CommunicationTarassolSendVM tarassolSend = new();
                var communication = sendCommunication.FirstOrDefault();
                var depts = await _IRole.GetDepartmentsByGEId(sendCommunication);
                var attachments = await GetAttachmentsForTarasolCommunication(communication.Communication.CommunicationId);
                foreach (var department in depts)
                {
                    var user = _IAccount.GetUserByUserEmail(communication.Communication.CreatedBy);
                    tarassolSend.RBrSiteId = department.G2GBRSiteID != null ? (int)department.G2GBRSiteID : 0;
                    tarassolSend.SenderUser = user.ADUserName != null ? user.ADUserName : "";
                    tarassolSend.SBrSiteId = department.SenderBranchId != null ? (int)department.SenderBranchId : 0;
                    var client = new G2GIWSSoapClient(G2GTarasolServiceReference.G2GIWSSoapClient.EndpointConfiguration.G2GIWSSoap);
                    var result = await client.G2G_SendOutGoingDocument(tarassolSend.SenderUser, "إدارة الفتوى والتشريع", department.RecieverSiteName, "", communication.Communication.OutboxNumber, subject, tarassolSend.SBrSiteId, tarassolSend.RBrSiteId, attachments);

                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Sent Outgoing Correspondence to G2G Tarasol",
                        Task = "Sent Outgoing Correspondence to G2G Tarasol",
                        Description = "Send Outgoing Correspondence to G2G Tarasol",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = result,
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)ModuleEnum.CaseManagement,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });
                }
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Unable to send Outgoing Correspondence to G2G Tarasol",
                    Body = ex.Message,
                    Category = "Unable to send Outgoing Correspondence to G2G Tarasol",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = ex.InnerException?.Message,
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
            }

        }

        private async Task<DataTable> GetAttachmentsForTarasolCommunication(Guid communicationId)
        {
            DataTable DT = new DataTable("Attachments");
            DataColumn colString = new DataColumn("AttName");
            colString.DataType = System.Type.GetType("System.String");
            DT.Columns.Add(colString);
            DataColumn colByteArray = new DataColumn("Attachment");
            colByteArray.DataType = System.Type.GetType("System.Byte[]");
            DT.Columns.Add(colByteArray);

            var attachments = await _IFileUpload.GetUploadedAttachementsByReferenceGuid(communicationId);

            if (attachments.Any())
            {
                foreach (var attachment in attachments)
                {
                    DataRow DR = DT.NewRow();
                    DR["AttName"] = attachment.FileName;

#if DEBUG
                    var physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + attachment.StoragePath).Replace(@"\\", @"\");
                    DR["Attachment"] = await DocumentEncryptionService.GetDecryptedDocumentBytes(physicalPath, attachment.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
#else
                    var physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + attachment.StoragePath).Replace(@"\\", @"\");
                    physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
                    DR["Attachment"] = await DocumentEncryptionService.GetDecryptedDocumentBytes(physicalPath, attachment.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
#endif

                    DT.Rows.Add(DR);
                }
            }
            return DT;
        }

        #endregion
        #region DMS Document Instance
        //[AllowAnonymous]
        [HttpPost("UpdateDMSDocumentInstance")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateDMSDocumentInstance(DmsAddedDocument document)
        {
            try
            {
                WorkflowActivity workflowActivity = new WorkflowActivity();
                if (document.WorkflowActivityId > 0)
                {
                    workflowActivity = await _IWorkflow.GetWorkflowActivityById((int)document.WorkflowActivityId);
                }
                document.Payload = await _IWorkflow.UpdateDMSDocumentInstance(document);
                if (!String.IsNullOrEmpty(document.DocumentVersion.ReviewerUserId))
                {
                    if (workflowActivity.IsNotification == true)
                    {
                        var notificationResponse = await _iNotifications.SendNotification(new Notification
                        {
                            NotificationId = Guid.NewGuid(),
                            DueDate = DateTime.Now.AddDays(5),
                            CreatedBy = document.CreatedBy,
                            CreatedDate = DateTime.Now,
                            IsDeleted = false,
                            ReceiverId = document?.DocumentVersion.ReviewerUserId,// Assign To  Id
                            ModuleId = document.ModuleId,
                        },
                        workflowActivity.ActivityId == (int)WorkflowActivityEnum.InitiatorDocumentModification ? (int)NotificationEventEnum.ModifyDocument : (int)NotificationEventEnum.ReviewDocument,
                        "view",
                        "document",
                        document.Id.ToString() + '/' + document.DocumentVersion.Id,
                        document.NotificationParameter);
                    }
                }
                if (!String.IsNullOrEmpty(document.DocumentVersion.ReviewerRoleId))
                {
                    if (workflowActivity.IsNotification == true)
                    {
                        var UsersList = await _IAccount.GetUsersByRoleId(document.DocumentVersion.ReviewerRoleId);
                        if (document.ModuleId == (int)MainWorkflowModuleEnum.CaseManagement || document.ModuleId == (int)MainWorkflowModuleEnum.ConsultationManagement)
                        {
                            UsersList = await _IWorkflow.GetUsersByRoleIdandSectorId(document.DocumentVersion.ReviewerRoleId, document.SectorTypeId);
                        }
                        foreach (var user in UsersList)
                        {
                            var notificationResponse = await _iNotifications.SendNotification(new Notification
                            {
                                NotificationId = Guid.NewGuid(),
                                DueDate = DateTime.Now.AddDays(5),
                                CreatedBy = document.CreatedBy,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReceiverId = user,// Assign To  Id
                                ModuleId = document.ModuleId
                            },
                            workflowActivity.ActivityId == (int)WorkflowActivityEnum.InitiatorDocumentModification ? (int)NotificationEventEnum.ModifyDocument : (int)NotificationEventEnum.ReviewDocument,
                            "view",
                            "document",
                            document.Id.ToString() + '/' + document.DocumentVersion.Id,
                            document.NotificationParameter);
                        }
                    }
                }
                if (document.IsEndofFlow == true)
                {
                    string userInitiatorId = await _IAccount.UserIdByUserEmail(document?.CreatedBy);
                    var notificationResponse = await _iNotifications.SendNotification(new Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        DueDate = DateTime.Now.AddDays(5),
                        CreatedBy = document.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReceiverId = userInitiatorId,// Assign To  Id
                        ModuleId = document.ModuleId,
                    },
                    (int)NotificationEventEnum.PublishedDocument,
                    "view",
                    "document",
                    document.Id.ToString() + '/' + document.DocumentVersion.Id,
                    document.NotificationParameter);
                }
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for a update DMS Instance",
                    Task = "To update the request",
                    Description = "User able to update DMS instance Request successfully.",
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
                    Subject = "Request for update DMS Instance Failed",
                    Body = ex.Message,
                    Category = "User unable to update DMS Instance Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for a update DMS Instance Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Hearing Task Creation
        private async Task CreateHearingTaskForLawyer(Hearing hearing)
        {
            try
            {
                var taskId = Guid.NewGuid();
                string user = await _IAccount.UserIdByUserEmail(hearing?.CreatedBy);
                int SectorId = (int)await _IAccount.GetSectorIdByEmail(hearing?.CreatedBy);

                var taskResult = await _ITask.AddTask(new SaveTaskVM
                {
                    Task = new UserTask
                    {
                        TaskId = taskId,
                        Name = "Add_Hearing",
                        Description = "",
                        Date = DateTime.Now.Date,
                        AssignedBy = user,
                        AssignedTo = hearing.LawyerId,
                        IsSystemGenerated = true,
                        TaskStatusId = (int)TaskStatusEnum.Pending,
                        ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                        SectorId = SectorId,
                        DepartmentId = (int)DepartmentEnum.Operational,
                        TypeId = (int)TaskTypeEnum.Task,
                        RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                        CreatedBy = hearing.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReferenceId = hearing.CaseId,
                        SubModuleId = (int)SubModuleEnum.RegisteredCase,
                        SystemGenTypeId = (int)TaskSystemGenTypeEnum.RegisteredCaseAssignToLawyer,
                        EntityId = null
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
                "case",
                hearing.CaseId.ToString());
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for a new hearing task For Lawyer",
                    Task = "To submit the request",
                    Description = "User able to Create hearing task Request successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Request has been saved",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Request for a new hearing task For Lawyer Failed",
                    Body = ex.Message,
                    Category = "User unable to Create hearing task Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for a new hearing task Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                throw ex;
            }
        }
        #endregion

        #region Calculate Submodule
        private int CalculateSubmodule(int DraftEntityType, int ModuleId)
        {
            int SubmoduleId = 0;
            if (ModuleId == (int)WorkflowModuleEnum.CaseManagement)
            {
                SubmoduleId = DraftEntityType == (int)DraftEntityTypeEnum.RequestNeedMoreInfo
                    ? (int)SubModuleEnum.CaseRequest : ((DraftEntityType == (int)DraftEntityTypeEnum.FileNeedMoreInfo
                    || DraftEntityType == (int)DraftEntityTypeEnum.CaseFile) ? (int)SubModuleEnum.CaseFile : (int)SubModuleEnum.RegisteredCase);
            }
            else if (ModuleId == (int)WorkflowModuleEnum.COMSConsultationManagement)
            {
                SubmoduleId = DraftEntityType == (int)DraftEntityTypeEnum.RequestNeedMoreInfo
                    ? (int)SubModuleEnum.ConsultationRequest : ((DraftEntityType == (int)DraftEntityTypeEnum.FileNeedMoreInfo
                    || DraftEntityType == (int)DraftEntityTypeEnum.ConsultationFile) ? (int)SubModuleEnum.ConsultationFile : (int)SubModuleEnum.ConsultationRequest);
            }
            return SubmoduleId;
        }
        #endregion

        #region Calculate SystemGenType Id
        private int CalculateSystemGenTypeId(int DraftEntityType, int ModuleId)
        {
            int SystemGenTypeId = 0;
            if (ModuleId == (int)WorkflowModuleEnum.CaseManagement)
            {
                SystemGenTypeId = DraftEntityType == (int)DraftEntityTypeEnum.RequestNeedMoreInfo
                    ? (int)TaskSystemGenTypeEnum.CaseRequestDraftDocumentReview : (int)TaskSystemGenTypeEnum.DraftDocumentReview;
            }
            else if (ModuleId == (int)WorkflowModuleEnum.COMSConsultationManagement)
            {
                SystemGenTypeId = DraftEntityType == (int)DraftEntityTypeEnum.RequestNeedMoreInfo
                    ? (int)TaskSystemGenTypeEnum.ConsultationRequestDraftDocumentReview : (int)TaskSystemGenTypeEnum.ConsultationDraftDocumentReview;
            }
            return SystemGenTypeId;
        }
        #endregion

        #region Update Approval tracking instance(Transfer)
        //<History Author = 'Muhammad Zaeem' Date='2023-04-12' Version="1.0" Branch="master"> Update Consultation Approval tracking instance for transfer </History>

        [HttpPost("UpdateApprovalTrackingInstance")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateApprovalTrackingInstance(CmsApprovalTracking approvalTracking)
        {
            try
            {
                WorkflowActivity workflowActivity = new WorkflowActivity();
                if (approvalTracking.WorkflowActivityId > 0)
                {
                    workflowActivity = await _IWorkflow.GetWorkflowActivityById((int)approvalTracking.WorkflowActivityId);
                }
                var historyObject = await _IWorkflow.UpdateApprovalTrackingInstance(approvalTracking);
                if (!String.IsNullOrEmpty(approvalTracking.ReviewerUserId))
                {
                    var user = await _IAccount.GetUserEmploymentInfoByUserId(approvalTracking.ReviewerUserId);
                    string viewPage = "";
                    viewPage = approvalTracking.StatusId == (int)ApprovalStatusEnum.Pending ? "transfer-review" : "view";
                    if (!(workflowActivity.ActivityId >= (int)WorkflowActivityEnum.CmsSendToGS &&
                            workflowActivity.ActivityId <= (int)WorkflowActivityEnum.CmsTransferToPOButSendToFPForDecision))
                    {
                        SaveTaskVM taskObj = null;
                        Notification notificationObj = null;
                        if (workflowActivity.IsTask == true)
                        {
                            taskObj = new SaveTaskVM
                            {
                                Task = new UserTask
                                {
                                    Name = approvalTracking.ProcessTypeId == (int)ApprovalProcessTypeEnum.Transfer ? "Transfer_of_Sector_Task" : "Assignment_of_Sector_Task",
                                    Description = approvalTracking.Remarks,
                                    Date = DateTime.Now.Date,
                                    AssignedBy = approvalTracking?.AssignedBy,
                                    IsSystemGenerated = true,
                                    TaskStatusId = (int)TaskStatusEnum.Pending,
                                    ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                                    SectorId = (int)user.SectorTypeId,
                                    DepartmentId = (int)DepartmentEnum.Operational,
                                    TypeId = (int)TaskTypeEnum.Task,
                                    RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                                    CreatedBy = approvalTracking.CreatedBy,
                                    CreatedDate = DateTime.Now,
                                    IsDeleted = false,
                                    ReferenceId = approvalTracking.ReferenceId,
                                    SubModuleId = approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? (int)SubModuleEnum.CaseRequest : (int)SubModuleEnum.CaseFile,
                                    SystemGenTypeId = approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? (int)TaskSystemGenTypeEnum.CaseRequestTransfer : (approvalTracking.ProcessTypeId == (int)ApprovalProcessTypeEnum.FileAssignment ? (int)TaskSystemGenTypeEnum.CaseFileAssignToSector : (int)TaskSystemGenTypeEnum.CaseFileTransfer),
                                },
                                Action = viewPage,
                                EntityName = approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? new CaseRequest().GetType().Name : new CaseFile().GetType().Name,
                                EntityId = approvalTracking.ReferenceId.ToString()
                            };
                        }

                        if (workflowActivity.IsNotification == true)
                        {
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
                                    EntityName = approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? new CaseRequest().GetType().Name : new CaseFile().GetType().Name,
                                    EntityId = approvalTracking.ReferenceId.ToString(),
                                    NotificationParameter = approvalTracking.NotificationParameter
                                };
                        }

                        await _ITask.AddTaskAndNotificationForHOSAndViceHOSOfSector(taskObj, notificationObj, (int)user?.SectorTypeId,
                        false,//Send True if required to Check Vice HOS Responsibility for All or for only Manager, send False if need to send Tasks/Notifications for All Vice HOS
                        true,//Send True if need to Include HOS as well along Vice HOSs
                        0//Send Chamber Number Id if need to send Tasks for Vice HOSs of specific Chamber Number
                        );
                    }
                    else
                    {
                        if (workflowActivity.IsTask == true)
                        {
                            var taskId = Guid.NewGuid();
                            var taskResult = await _ITask.AddTask(new SaveTaskVM
                            {
                                Task = new UserTask
                                {
                                    TaskId = taskId,
                                    Name = approvalTracking.ProcessTypeId == (int)ApprovalProcessTypeEnum.Transfer ? "Transfer_of_Sector_Task" : "Assignment_of_Sector_Task",
                                    Description = approvalTracking.Remarks,
                                    Date = DateTime.Now.Date,
                                    AssignedBy = approvalTracking?.AssignedBy,
                                    AssignedTo = approvalTracking?.ReviewerUserId,
                                    IsSystemGenerated = true,
                                    TaskStatusId = (int)TaskStatusEnum.Pending,
                                    ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                                    SectorId = (int)user.SectorTypeId,
                                    DepartmentId = (int)DepartmentEnum.Operational,
                                    TypeId = (int)TaskTypeEnum.Task,
                                    RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                                    CreatedBy = approvalTracking.CreatedBy,
                                    CreatedDate = DateTime.Now,
                                    IsDeleted = false,
                                    ReferenceId = approvalTracking.ReferenceId,
                                    SubModuleId = approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? (int)SubModuleEnum.CaseRequest : (int)SubModuleEnum.CaseFile,
                                    SystemGenTypeId = approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? (int)TaskSystemGenTypeEnum.CaseRequestTransfer : (approvalTracking.ProcessTypeId == (int)ApprovalProcessTypeEnum.FileAssignment ? (int)TaskSystemGenTypeEnum.CaseFileAssignToSector : (int)TaskSystemGenTypeEnum.CaseFileTransfer),
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
                            viewPage,
                            approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? new CaseRequest().GetType().Name : new CaseFile().GetType().Name,
                            approvalTracking.ReferenceId.ToString());
                        }
                        if (workflowActivity.IsNotification == true)
                        {
                            var notificationResponse = await _iNotifications.SendNotification(new Notification
                            {
                                NotificationId = Guid.NewGuid(),
                                DueDate = DateTime.Now.AddDays(5),
                                CreatedBy = approvalTracking.CreatedBy,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReceiverId = approvalTracking?.ReviewerUserId,// Assign To Sactor Id
                                ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                            },
                            (int)NotificationEventEnum.TransferOfSector,
                            viewPage,
                            approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? new CaseRequest().GetType().Name : new CaseFile().GetType().Name,
                            approvalTracking.ReferenceId.ToString(),
                            approvalTracking.NotificationParameter);
                        }
                    }
                }
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "To update  the approval  tracking instance ",
                    Task = "To update the approval  tracking instance ",
                    Description = "Approval tracking  has been updated.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Approval tracking has been updated",
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
                    Subject = "Update the approval  tracking instance  Failed",
                    Body = ex.Message,
                    Category = "User unable to update the approval  tracking instance ",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "The approval tracking instance could not be updated",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });
            }
        }



        #endregion

        #region Update Consultation Approval tracking instance(Transfer)
        //<History Author = 'Muhammad Zaeem' Date='2023-04-12' Version="1.0" Branch="master"> Update Consultation Approval tracking instance for transfer </History>

        [HttpPost("UpdateApprovalTrackingConsultationInstance")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateApprovalTrackingConsultationInstance(CmsApprovalTracking approvalTracking)
        {
            try
            {
                WorkflowActivity workflowActivity = new WorkflowActivity();
                if (approvalTracking.WorkflowActivityId > 0)
                {
                    workflowActivity = await _IWorkflow.GetWorkflowActivityById((int)approvalTracking.WorkflowActivityId);

                }
                await _IWorkflow.UpdateApprovalTrackingConsultationInstance(approvalTracking);
                if (!String.IsNullOrEmpty(approvalTracking.ReviewerUserId))
                {
                    string viewPage;
                    if (approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest)
                        viewPage = approvalTracking.StatusId == (int)ApprovalStatusEnum.Pending ? "transfer-review" : "detail";
                    else
                        viewPage = approvalTracking.StatusId == (int)ApprovalStatusEnum.Pending ? "transfer-review" : "view";
                    var user = await _IAccount.GetUserEmploymentInfoByUserId(approvalTracking.ReviewerUserId);
                    if (!(workflowActivity.ActivityId >= (int)WorkflowActivityEnum.CmsSendToGS &&
                            workflowActivity.ActivityId <= (int)WorkflowActivityEnum.CmsTransferToPOButSendToFPForDecision))
                    {
                        SaveTaskVM taskObj = null;
                        Notification notificationObj = null;
                        if (workflowActivity.IsTask == true)
                        {
                            taskObj = new SaveTaskVM
                            {
                                Task = new UserTask
                                {
                                    Name = approvalTracking.ProcessTypeId == (int)ApprovalProcessTypeEnum.Transfer ? "Transfer_of_Sector_Task" : "Assignment_of_Sector_Task",
                                    Description = approvalTracking.Remarks,
                                    Date = DateTime.Now.Date,
                                    AssignedBy = approvalTracking?.AssignedBy,
                                    AssignedTo = approvalTracking?.ReviewerUserId,
                                    IsSystemGenerated = true,
                                    TaskStatusId = (int)TaskStatusEnum.Pending,
                                    ModuleId = (int)WorkflowModuleEnum.COMSConsultationManagement,
                                    SectorId = (int)user.SectorTypeId,
                                    DepartmentId = (int)DepartmentEnum.Operational,
                                    TypeId = (int)TaskTypeEnum.Task,
                                    RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                                    CreatedBy = approvalTracking.CreatedBy,
                                    CreatedDate = DateTime.Now,
                                    IsDeleted = false,
                                    ReferenceId = approvalTracking.ReferenceId,
                                    SubModuleId = approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest ? (int)SubModuleEnum.ConsultationRequest : (int)SubModuleEnum.ConsultationFile,
                                    SystemGenTypeId = approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest ? (int)TaskSystemGenTypeEnum.ConsultationRequestTransfer : (approvalTracking.ProcessTypeId == (int)ApprovalProcessTypeEnum.FileAssignment ? (int)TaskSystemGenTypeEnum.ConsultationFileAssignToSector : (int)TaskSystemGenTypeEnum.ConsultationFileTransfer),
                                },

                                Action = viewPage,
                                EntityName = approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest ? new ConsultationRequest().GetType().Name : new ConsultationFile().GetType().Name,
                                EntityId = approvalTracking.ReferenceId.ToString() + "/" + user.SectorTypeId.ToString()
                            };
                        }
                        if (workflowActivity.IsNotification == true)
                        {
                            notificationObj =
                                new Notification
                                {
                                    NotificationId = Guid.NewGuid(),
                                    DueDate = DateTime.Now.AddDays(5),
                                    CreatedBy = approvalTracking.CreatedBy,
                                    CreatedDate = DateTime.Now,
                                    IsDeleted = false,
                                    ModuleId = (int)WorkflowModuleEnum.COMSConsultationManagement,
                                    EventId = (int)NotificationEventEnum.TransferOfSector,
                                    Action = viewPage,
                                    EntityName = approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest ? new ConsultationRequest().GetType().Name : new ConsultationFile().GetType().Name,
                                    EntityId = approvalTracking.ReferenceId.ToString() + "/" + user.SectorTypeId.ToString(),
                                    NotificationParameter = approvalTracking.NotificationParameter
                                };
                        }
                        await _ITask.AddTaskAndNotificationForHOSAndViceHOSOfSector(taskObj, notificationObj, (int)user?.SectorTypeId,
                        false,//Send True if required to Check Vice HOS Responsibility for All or for only Manager, send False if need to send Tasks/Notifications for All Vice HOS
                        true,//Send True if need to Include HOS as well along Vice HOSs
                        0//Send Chamber Number Id if need to send Tasks for Vice HOSs of specific Chamber Number
                        );
                    }
                    else
                    {
                        Guid taskIdForNotification = Guid.Empty;
                        if (workflowActivity.IsTask == true)
                        {
                            var taskId = Guid.NewGuid();
                            taskIdForNotification = taskId;
                            var taskResult = await _ITask.AddTask(new SaveTaskVM
                            {
                                Task = new UserTask
                                {
                                    TaskId = taskId,
                                    Name = approvalTracking.ProcessTypeId == (int)ApprovalProcessTypeEnum.Transfer ? "Transfer_of_Sector_Task" : "Assignment_of_Sector_Task",
                                    Description = approvalTracking.Remarks,
                                    Date = DateTime.Now.Date,
                                    AssignedBy = approvalTracking?.AssignedBy,
                                    AssignedTo = approvalTracking?.ReviewerUserId,
                                    IsSystemGenerated = true,
                                    TaskStatusId = (int)TaskStatusEnum.Pending,
                                    ModuleId = (int)WorkflowModuleEnum.COMSConsultationManagement,
                                    SectorId = (int)user.SectorTypeId,
                                    DepartmentId = (int)DepartmentEnum.Operational,
                                    TypeId = (int)TaskTypeEnum.Task,
                                    RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                                    CreatedBy = approvalTracking.CreatedBy,
                                    CreatedDate = DateTime.Now,
                                    IsDeleted = false,
                                    ReferenceId = approvalTracking.ReferenceId,
                                    SubModuleId = approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest ? (int)SubModuleEnum.ConsultationRequest : (int)SubModuleEnum.ConsultationFile,
                                    SystemGenTypeId = approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest ? (int)TaskSystemGenTypeEnum.ConsultationRequestTransfer : (approvalTracking.ProcessTypeId == (int)ApprovalProcessTypeEnum.FileAssignment ? (int)TaskSystemGenTypeEnum.ConsultationFileAssignToSector : (int)TaskSystemGenTypeEnum.ConsultationFileTransfer),
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
                            viewPage,
                            approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest ? new ConsultationRequest().GetType().Name : new ConsultationFile().GetType().Name,
                           approvalTracking.ReferenceId.ToString() + "/" + user.SectorTypeId.ToString());
                        }
                        if (workflowActivity.IsNotification == true)
                        {
                            var notificationResponse = await _iNotifications.SendNotification(new Notification
                            {
                                NotificationId = Guid.NewGuid(),
                                DueDate = DateTime.Now.AddDays(5),
                                CreatedBy = approvalTracking.CreatedBy,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReceiverId = approvalTracking?.ReviewerUserId,// Assign To Sactor Id
                                ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                            },
                            (int)NotificationEventEnum.TransferOfSector,
                            viewPage,
                            approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest ? new ConsultationRequest().GetType().Name : new ConsultationFile().GetType().Name,
                           approvalTracking.ReferenceId.ToString() + "/" + user.SectorTypeId + "/" + taskIdForNotification.ToString(),
                            approvalTracking.NotificationParameter);
                        }
                    }
                }
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "To update  the consultation approval  tracking instance ",
                    Task = "To update the consultation approval  tracking instance ",
                    Description = "Consultation Approval tracking  has been updated.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Consultation Approval tracking has been updated",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.ConsultationManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok();
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Update the consultation approval  tracking instance  Failed",
                    Body = ex.Message,
                    Category = "User unable to update the consultation approval  tracking instance ",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "The consultation approval tracking instance could not be updated",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.ConsultationManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });
            }
        }



        #endregion

        #region Update Copy Tracking Instance
        //<History Author = 'Muhammad Zaeem' Date='2023-04-12' Version="1.0" Branch="master"> Update case Approval tracking instance for pending Send copy </History>

        [HttpPost("UpdateCopyTrackingInstance")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateCopyTrackingInstance(CmsApprovalTracking approvalTracking)
        {
            try
            {
                WorkflowActivity workflowActivity = new WorkflowActivity();
                if (approvalTracking.WorkflowActivityId > 0)
                {
                    workflowActivity = await _IWorkflow.GetWorkflowActivityById((int)approvalTracking.WorkflowActivityId);

                }
                await _IWorkflow.UpdateCopyTrackingInstance(approvalTracking);
                if (!String.IsNullOrEmpty(approvalTracking.ReviewerUserId))
                {
                    var user = await _IAccount.GetUserEmploymentInfoByUserId(approvalTracking.ReviewerUserId);
                    string viewPage = "";
                    viewPage = approvalTracking.StatusId == (int)ApprovalStatusEnum.Pending ? "copy-review" : "view";
                    if (workflowActivity.IsTask == true)
                    {
                        var taskId = Guid.NewGuid();
                        var taskResult = await _ITask.AddTask(new SaveTaskVM
                        {
                            Task = new UserTask
                            {
                                TaskId = taskId,
                                Name = approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? "Review_Recieved_Copy_of_Case_Request" : "Review_Recieved_Copy_of_Case_File",
                                Description = approvalTracking.Remarks,
                                Date = DateTime.Now.Date,
                                AssignedBy = approvalTracking.AssignedBy,
                                AssignedTo = approvalTracking?.ReviewerUserId,
                                IsSystemGenerated = true,
                                TaskStatusId = (int)TaskStatusEnum.Pending,
                                ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                                SectorId = (int)user.SectorTypeId,
                                DepartmentId = (int)DepartmentEnum.Operational,
                                TypeId = (int)TaskTypeEnum.Task,
                                RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                                CreatedBy = approvalTracking.CreatedBy,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReferenceId = approvalTracking.ReferenceId,
                                SubModuleId = approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? (int)SubModuleEnum.CaseRequest : (int)SubModuleEnum.CaseFile,
                                SystemGenTypeId = approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? (int)TaskSystemGenTypeEnum.CaseRequestSendCopy : (int)TaskSystemGenTypeEnum.CaseFileSendCopy,
                            },
                            TaskActions = new List<TaskAction>()
                            {
                                new TaskAction()
                                {
                                    ActionName = approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? "Review Recieved Copy of Case Request":"Review_Recieved_Copy_of_Case_File",
                                    TaskId = taskId,
                                }
                            }
                        },
                        viewPage,
                        approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? new CaseRequest().GetType().Name : new CaseFile().GetType().Name,
                        approvalTracking.ReferenceId.ToString());
                    }
                    if (workflowActivity.IsNotification == true)
                    {
                        var notificationResponse = await _iNotifications.SendNotification(new Notification
                        {
                            NotificationId = Guid.NewGuid(),
                            DueDate = DateTime.Now.AddDays(5),
                            CreatedBy = approvalTracking.CreatedBy,
                            CreatedDate = DateTime.Now,
                            IsDeleted = false,
                            ReceiverId = approvalTracking.ReviewerUserId,// Send  Id
                            ModuleId = (int)WorkflowModuleEnum.CaseManagement
                        },
                        (int)NotificationEventEnum.SendACopyReview,
                        viewPage,
                        approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? new CaseRequest().GetType().Name : new CaseFile().GetType().Name,
                        approvalTracking.ReferenceId.ToString(),
                        approvalTracking.NotificationParameter);
                    }
                }
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? "To send the copy of case request" : "To send the copy of case file",
                    Task = approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? "To send a copy of Request" : "To send a copy of file",
                    Description = approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? "User send the copy of the request." : "User send the copy of the file.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Copy has been sent to the sector successfully",
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
                    Subject = approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? "To send the copy of case request" : "To send the copy of case file",
                    Body = ex.Message,
                    Category = "User unable To  send the Copy ",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Unable to send the Copy ",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });

            }
        }
        #endregion

        #region Update Copy Approved Tracking Instance
        //<History Author = 'Muhammad Zaeem' Date='2023-04-12' Version="1.0" Branch="master"> Update Case Approval tracking instance for approved Send copy </History>

        [HttpPost("UpdateCopyApprovedTrackingInstance")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateCopyApprovedTrackingInstance(CmsApprovalTracking approvalTracking)
        {
            try
            {
                WorkflowActivity workflowActivity = new WorkflowActivity();
                if (approvalTracking.WorkflowActivityId > 0)
                {
                    workflowActivity = await _IWorkflow.GetWorkflowActivityById((int)approvalTracking.WorkflowActivityId);

                }
                var result = await _IWorkflow.UpdateCopyApprovedTrackingInstance(approvalTracking);
                if (approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest && result != null)
                {
                    approvalTracking.ReferenceId = result.RequestId;
                }
                else if (approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseFile && result != null)
                {
                    approvalTracking.ReferenceId = result.FileId;
                }
                if (!String.IsNullOrEmpty(approvalTracking.ReviewerUserId))
                {
                    string viewPage = "view";
                    if (workflowActivity.IsTask == true)
                    {
                        var user = await _IAccount.GetUserEmploymentInfoByUserId(approvalTracking.ReviewerUserId);
                        var taskId = Guid.NewGuid();
                        var taskResult = await _ITask.AddTask(new SaveTaskVM
                        {
                            Task = new UserTask
                            {
                                TaskId = taskId,
                                Name = approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? "Review_Recieved_Copy_of_Case_Request" : "Review_Recieved_Copy_of_Case_File",
                                Description = approvalTracking.Remarks,
                                Date = DateTime.Now.Date,
                                AssignedBy = approvalTracking.AssignedBy,
                                AssignedTo = approvalTracking?.ReviewerUserId,
                                IsSystemGenerated = true,
                                TaskStatusId = (int)TaskStatusEnum.Pending,
                                ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                                SectorId = (int)user.SectorTypeId,
                                DepartmentId = (int)DepartmentEnum.Operational,
                                TypeId = (int)TaskTypeEnum.Task,
                                RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                                CreatedBy = approvalTracking.CreatedBy,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReferenceId = approvalTracking.ReferenceId,
                                SubModuleId = approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? (int)SubModuleEnum.CaseRequest : (int)SubModuleEnum.CaseFile,
                                SystemGenTypeId = approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? (int)TaskSystemGenTypeEnum.CaseRequestSendCopy : (int)TaskSystemGenTypeEnum.CaseFileSendCopy,
                            },
                            TaskActions = new List<TaskAction>()
                            {
                                new TaskAction()
                                {
                                    ActionName = approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? "Review Recieved Copy of Case Request":"Review_Recieved_Copy_of_Case_File",
                                    TaskId = taskId,
                                }
                            }
                        },
                        viewPage,
                        approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? new CaseRequest().GetType().Name : new CaseFile().GetType().Name,
                        approvalTracking.ReferenceId.ToString());
                    }
                    if (workflowActivity.IsNotification == true)
                    {
                        var notificationResponse = await _iNotifications.SendNotification(new Notification
                        {
                            NotificationId = Guid.NewGuid(),
                            DueDate = DateTime.Now.AddDays(5),
                            CreatedBy = approvalTracking.CreatedBy,
                            CreatedDate = DateTime.Now,
                            IsDeleted = false,
                            ReceiverId = approvalTracking.ReviewerUserId,// Send  Id
                            ModuleId = (int)WorkflowModuleEnum.CaseManagement
                        },
                        (int)NotificationEventEnum.SendACopyApproved,
                        viewPage,
                        approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? new CaseRequest().GetType().Name : new CaseFile().GetType().Name,
                        approvalTracking.ReferenceId.ToString(),
                        approvalTracking.NotificationParameter);

                    }
                    if (result != null && approvalTracking.StatusId != (int)ApprovalStatusEnum.Pending)
                    {
                        _client.SendMessage(result, approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? RabbitMQKeys.CopyRequestKey : RabbitMQKeys.CopyFileKey);
                    }
                }
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? "To send the copy of case request" : "To send the copy of case file",
                    Task = approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? "To send a copy of Request" : "To send a copy of file",
                    Description = approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? "User send the copy of the request." : "User send the copy of the file.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Copy has been sent to the sector successfully",
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
                    Subject = approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? "To send the copy of case request" : "To send the copy of case file",
                    Body = ex.Message,
                    Category = "User unable To  send the Copy ",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Unable to send the Copy ",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.CaseManagement,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(new BadRequestResponse { Message = ex?.Message, InnerException = ex?.InnerException?.Message });

            }
        }
        #endregion

        #region Get users by role id and sector id
        [HttpGet("GetUsersByRoleIdandSectorId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetUsersByRoleAndSector(string RoleId , int SectorTypeId)
        {
            try
            {
                var result = await _IWorkflow.GetUsersByRoleIdandSectorId(RoleId, SectorTypeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        

        [HttpGet("GetNextWorrkflowActivity")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetNextWorrkflowActivity(Guid draftId)
        {
            try
            {
                return Ok(await _IWorkflow.GetNextWorrkflowActivity(draftId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #region Workflow Trigger Condition Options
        [HttpGet("GetWorkflowTriggerConditionsOptions")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2023-10-29' Version="1.0" Branch="master"> Get workflow Trigger Conditions</History>
        public async Task<IActionResult> GetWorkflowTriggerConditionsOptions(int TriggerConditionId, Guid ReferenceId)
        {
            try
            {
                return Ok(await _IWorkflow.GetWorkflowTriggerConditionsOptions(TriggerConditionId, ReferenceId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
