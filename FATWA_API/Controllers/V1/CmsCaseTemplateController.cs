using FATWA_API.RabbitMQ;
using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.CaseManagement;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Interfaces.PatternNumber;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Interfaces.Notification;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.WorkflowModels;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using FATWA_DOMAIN.Enums.Common;
using System.Net;

namespace FATWA_API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //<History Author = 'Hassan Abbas' Date='2022-11-02' Version="1.0" Branch="master"> Case Template API Controller for handling calls</History>
    public class CmsCaseTemplateController : ControllerBase
    {
        private readonly ICmsCaseTemplate _ICmsCaseTemplate;
        private readonly IWebHostEnvironment _environment;
        private readonly IAuditLog _auditLogs;
        private readonly IConfiguration _configuration;
        private readonly ICMSCOMSInboxOutboxRequestPatternNumber _cMSCOMSInboxOutboxRequestPatternNumber;
        private readonly ICmsRegisteredCase _iCmsRegisteredCase;
        private readonly IAccount _IAccount;
        private readonly INotification _iNotifications;
        public CmsCaseTemplateController(ICmsCaseTemplate iCmsCaseTemplate, IConfiguration configuration,
            IWebHostEnvironment environment, IAuditLog audit, ICMSCOMSInboxOutboxRequestPatternNumber cMSCOMSInboxOutboxRequestPatternNumber,
            ICmsRegisteredCase iCmsRegisteredCase, IAccount iAccount, INotification iNotifications)
        {
            _ICmsCaseTemplate = iCmsCaseTemplate;
            _environment = environment;
            _auditLogs = audit;
            _configuration = configuration;
            _cMSCOMSInboxOutboxRequestPatternNumber = cMSCOMSInboxOutboxRequestPatternNumber;
            _iCmsRegisteredCase = iCmsRegisteredCase;
            _IAccount = iAccount;
            _iNotifications = iNotifications;
        }

        [HttpGet("GetTemplateSections")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-11-02' Version="1.0" Branch="master"> Get Case Template Sections</History>
        public async Task<IActionResult> GetTemplateSections(int templateId)
        {
            try
            {
                return Ok(await _ICmsCaseTemplate.GetTemplateSections(templateId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetDraftedTemplateSections")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-11-02' Version="1.0" Branch="master"> Get Case Template Sections</History>
        public async Task<IActionResult> GetDraftedTemplateSections(Guid versionId)
        {
            try
            {
                return Ok(await _ICmsCaseTemplate.GetDraftedTemplateSections(versionId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        

        [HttpGet("GetTemplateSectionParameters")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-11-02' Version="1.0" Branch="master"> Get Case Template Parameters</History>
        public async Task<IActionResult> GetTemplateSectionParameters(Guid templateSectionId)
        {
            try
            {
                return Ok(await _ICmsCaseTemplate.GetTemplateSectionParameters(templateSectionId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetDraftedTemplateSectionParameters")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-11-02' Version="1.0" Branch="master"> Get Case Template Parameters</History>
        public async Task<IActionResult> GetDraftedTemplateSectionParameters(Guid templateSectionId)
        {
            try
            {
                return Ok(await _ICmsCaseTemplate.GetDraftedTemplateSectionParameters(templateSectionId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
       

        [HttpGet("GetCaseTemplateContent")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-11-02' Version="1.0" Branch="master"> Get Case Template Content</History>
        public async Task<IActionResult> GetCaseTemplateContent(int templateId, Guid? DraftId, Guid? VersionId)
        {
            try
            {
                return Ok(await _ICmsCaseTemplate.GetCaseTemplateContent(templateId, DraftId, VersionId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetHeaderFooter")]
        [MapToApiVersion("1.0")]
        
        public async Task<IActionResult> GetHeaderFooter()
        {
            try
            {
                return Ok(await _ICmsCaseTemplate.GetHeaderFooter());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetCaseTemplateDetail")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-11-02' Version="1.0" Branch="master"> Get Case Template Detail</History>
        public async Task<IActionResult> GetCaseTemplateDetail(int templateId)
        {
            try
            {
                return Ok(await _ICmsCaseTemplate.GetCaseTemplateDetail(templateId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetTemplateDataFromCaseFile")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-11-02' Version="1.0" Branch="master"> Get Case Template Content</History>
        public async Task<IActionResult> GetTemplateDataFromCaseFile(Guid fileId)
        {
            try
            {
                //return Ok(await _ICmsCaseTemplate.GetTemplateDataFromCaseFile(fileId));

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetDraftNumberVersionNumber")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-11-02' Version="1.0" Branch="master"> Get New Draft Number and Version</History>
        public async Task<IActionResult> GetDraftNumberVersionNumber(Guid? draftId, Guid? versionId)
        {
            try
            {
                return Ok(await _ICmsCaseTemplate.GetDraftNumberVersionNumber(draftId, versionId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
      
        [HttpGet("GetDraftDocDetailWithSectionAndParameters")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-11-02' Version="1.0" Branch="master"> Get Draft Document Detail with Section and Parameters</History>
        public async Task<IActionResult> GetDraftDocDetailWithSectionAndParameters(Guid draftId, Guid? VersionId)
        {
            try
            {
                var result = await _ICmsCaseTemplate.GetDraftDocDetailWithSectionAndParameters(draftId, VersionId);
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
        
        [HttpGet("GetDraftedTemplateDetailById")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-11-02' Version="1.0" Branch="master"> Get Draft Document detail by id/History>
        public async Task<IActionResult> GetDraftedTemplateDetailById(Guid draftId, Guid versionId)
        {
            try
            {
                return Ok(await _ICmsCaseTemplate.GetDraftedTemplateDetailById(draftId, versionId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpGet("GetDMSDocumentDetailById")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-11-02' Version="1.0" Branch="master"> Get Draft Document detail by id/History>
        public async Task<IActionResult> GetDMSDocumentDetailById(Guid documentId, Guid versionId)
        {
            try
            {
                return Ok(await _ICmsCaseTemplate.GetDMSDocumentDetailById(documentId, versionId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpPost("CreateCaseDraftDocument")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-11-02' Version="1.0" Branch="master"> Create Case Draft Document</History>
        public async Task<IActionResult> CreateCaseDraftDocument(CmsDraftedTemplate document)
        {
            try
            {
                await _ICmsCaseTemplate.CreateCaseDraftDocument(document);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for a new Case Draft",
                    Task = "To submit the request",
                    Description = "User able to Create Case Draft successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Request has been saved",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok(document);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Request for a new Case Draft Failed",
                    Body = ex.Message,
                    Category = "User unable to Create Draft Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for a new Case Draft Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpPost("CreateDraftDocumentVersion")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-11-02' Version="1.0" Branch="master"> Create Case Draft Document</History>
        public async Task<IActionResult> CreateDraftDocumentVersion(CmsDraftedTemplate DraftedTemplate)
        {
            try
            {
                await _ICmsCaseTemplate.CreateDraftDocumentVersion(DraftedTemplate);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for a new case draft version",
                    Task = "To submit the request",
                    Description = "User able to Create Case Draft Version Request successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Request has been saved",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok(DraftedTemplate);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Request for a new case draft version Failed",
                    Body = ex.Message,
                    Category = "User unable to Create Case Draft Version Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for a new case draft version Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpPost("UpdateCaseDraftDocument")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-11-02' Version="1.0" Branch="master"> Create Case Draft Document</History>
        public async Task<IActionResult> UpdateCaseDraftDocument(CmsDraftedTemplate document)
        {
            try
            {
                await _ICmsCaseTemplate.UpdateCaseDraftDocument(document);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for a update case draft",
                    Task = "To update the request",
                    Description = "User able to Update Case Draft Request Successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Request has been Updated",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok(document);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Request for a Update Case draft Failed",
                    Body = ex.Message,
                    Category = "User unable to Update Case Draft Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for a Update Case draft Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

		[HttpPost("UpdateDraftDocumentStatus")]
		[MapToApiVersion("1.0")]
		public async Task<IActionResult> UpdateDraftDocumentStatus(CmsDraftedTemplateVersions document)
		{
			try
			{
				await _ICmsCaseTemplate.UpdateDraftDocumentStatus(document);
                string SectorTypeId = await _IAccount.UserSectorTypeIdByUserEmail(document.CreatedBy);
                var user = await _iCmsRegisteredCase.GetMojBySectorId(Convert.ToInt32(SectorTypeId));
                if (document.StatusId == (int)DraftVersionStatusEnum.SendToMOJ&&user != null)
                {
                    var notificationResponse = await _iNotifications.SendNotification(new Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        DueDate = DateTime.Now.AddDays(5),
                        CreatedBy = document.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReceiverId = user.Id,// Assign To Id
                        ModuleId = (int)WorkflowModuleEnum.CaseManagement,
                    },
                    (int)NotificationEventEnum.SendToMOJ,
                    "requests",
                    "moj-registration",
                    "",
                    document.NotificationParameter);
                }
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for update draft document status",
                    Task = "To update the request",
                    Description = "User able to update draft status request successfully.",
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
                    Subject = "Request for update draft document status Failed",
                    Body = ex.Message,
                    Category = "User unable to update draft status Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for update draft document status Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
		}

		#region Get Case Draft List By ReferenceId

		[HttpGet("GetCaseDraftListByReferenceId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2022-11-02' Version="1.0" Branch="master"> Get Case Draft Document List filtered based on Case</History>
        //<History Author = 'Hassan Abbas' Date='2023-01-02' Version="1.0" Branch="master"> Moved method from CaseRequestController</History>
        public async Task<IActionResult> GetCaseDraftListByReferenceId(Guid referenceId)
        {
            try
            {
                return Ok(await _ICmsCaseTemplate.GetCaseDraftListByReferenceId(referenceId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Case Draft Reasons List By ReferenceId

        [HttpGet("GetDraftDocumentReasonsByReferenceId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2023-03-16' Version="1.0" Branch="master">Reasons By Draft</History>
        public async Task<IActionResult> GetDraftDocumentReasonsByReferenceId(Guid referenceId)
        {
            try
            {
                return Ok(await _ICmsCaseTemplate.GetDraftDocumentReasonsByReferenceId(referenceId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Case Draft Opinion List By ReferenceId

        [HttpGet("GetDraftDocumentOpinionByReferenceId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2023-09-24' Version="1.0" Branch="master">Opinions By Draft</History>
        public async Task<IActionResult> GetDraftDocumentOpinionByReferenceId(Guid referenceId)
        {
            try
            {
                return Ok(await _ICmsCaseTemplate.GetDraftDocumentOpinionByReferenceId(referenceId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Consultation Draft

        [HttpGet("GetComsDraftNumberVersionNumber")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2023-02-25' Version="1.0" Branch="master"> Get New Draft Number and Version</History>
        public async Task<IActionResult> GetComsDraftNumberVersionNumber(Guid? draftId)
        {
            try
            {
                return Ok(await _ICmsCaseTemplate.GetComsDraftNumberVersionNumber(draftId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetConsultationDraftedTemplateDetailById")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2023-02-25' Version="1.0" Branch="master"> Get Draft Document detail by id/History>
        public async Task<IActionResult> GetConsultationDraftedTemplateDetailById(Guid draftId)
        {
            try
            {
                return Ok(await _ICmsCaseTemplate.GetConsultationDraftedTemplateDetailById(draftId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetConsultationDraftDocDetailWithSectionAndParameters")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2023-02-25' Version="1.0" Branch="master"> Get Draft Document Detail with Section and Parameters</History>
        public async Task<IActionResult> GetConsultationDraftDocDetailWithSectionAndParameters(Guid draftId)
        {
            try
            {
                return Ok(await _ICmsCaseTemplate.GetConsultationDraftDocDetailWithSectionAndParameters(draftId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateConsultationDraftDocument")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2023-02-25' Version="1.0" Branch="master"> Create Consultation Draft Document</History>
        public async Task<IActionResult> CreateConsultationDraftDocument(ComsDraftedTemplate document)
        {
            try
            {
                await _ICmsCaseTemplate.CreateConsultationDraftDocument(document);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for create consultation draft",
                    Task = "To submit the request",
                    Description = "User able to create consultation draft request successfully.",
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
                    Subject = "Request for create consultation draft Failed",
                    Body = ex.Message,
                    Category = "User unable to create consultation draft Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for create consultation draft Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetComsDraftedTemplateSections")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2023-07-03' Version="1.0" Branch="master"> Get Consultation Drafted Template Sections</History>
        public async Task<IActionResult> GetComsDraftedTemplateSections(Guid draftId)
        {
            try
            {
                return Ok(await _ICmsCaseTemplate.GetComsDraftedTemplateSections(draftId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetCOMSDraftedTemplateSectionParameters")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2023-07-03' Version="1.0" Branch="master"> Get Consultation Drafted Template Section Parameters</History>
        public async Task<IActionResult> GetCOMSDraftedTemplateSectionParameters(Guid templateSectionId)
        {
            try
            {
                return Ok(await _ICmsCaseTemplate.GetCOMSDraftedTemplateSectionParameters(templateSectionId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("UpdateConsultationDraftDocument")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2023-07-03' Version="1.0" Branch="master"> Update Consultation Draft Doc</History>
        public async Task<IActionResult> UpdateConsultationDraftDocument(ComsDraftedTemplate document)
        {
            try
            {
                await _ICmsCaseTemplate.UpdateConsultationDraftDocument(document);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for update consultation draft",
                    Task = "To update the request",
                    Description = "User able to update consultation draft Request successfully.",
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
                    Subject = "Request for update consultation draft Failed",
                    Body = ex.Message,
                    Category = "User unable to update consultation draft Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for update consultation draft Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get Consultation Draft List By ReferenceId

        [HttpGet("GetConsultationDraftListByReferenceId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2023-03-02' Version="1.0" Branch="master"> Moved method from COnsultataion RequestRepo</History>
        public async Task<IActionResult> GetConsultationDraftListByReferenceId(Guid referenceId)
        {
            try
            {
                return Ok(await _ICmsCaseTemplate.GetConsultationDraftListByReferenceId(referenceId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get contract template
        //<History Author = 'Umer Zaman' Date='2023-01-02' Version="1.0" Branch="master">Populate contract template details</History>

        [HttpGet("GetSelectedConsultationTemplateSectionDetailsUsingTemplateId")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> GetSelectedConsultationTemplateSectionDetailsUsingTemplateId(int templateId)
        {
            try
            {
                var result = await _ICmsCaseTemplate.GetSelectedConsultationTemplateSectionDetailsUsingTemplateId(templateId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Save drafted consultation request

        [HttpPost("SaveDraftFileConsultationRequest")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2023-02-25' Version="1.0" Branch="master"> Create Consultation Draft Document</History>
        public async Task<IActionResult> SaveDraftFileConsultationRequest(ConsultationRequest consultationRequests)
        {
            try
            {
                var result = await _ICmsCaseTemplate.SaveDraftFileConsultationRequest(consultationRequests);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Request for save consultation draft file",
                    Task = "To save the request",
                    Description = "User able to save consultation draft file successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Request has been saved",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Request for save consultation draft file Failed",
                    Body = ex.Message,
                    Category = "User unable tosave consultation draft file Request",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Request for save consultation draft file Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Consultation Draft Reasons List By ReferenceId

        [HttpGet("GetComsDraftDocumentReasonsByReferenceId")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2023-03-22' Version="1.0" Branch="master">Reasons By Draft</History>
        public async Task<IActionResult> GetComsDraftDocumentReasonsByReferenceId(Guid referenceId)
        {
            try
            {
                return Ok(await _ICmsCaseTemplate.GetComsDraftDocumentReasonsByReferenceId(referenceId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

		#endregion

        #region Get CmsDrafted Template Version Logs List
        [HttpGet("GetCmsDraftTemplateVersionLogsList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCmsDraftTemplateVersionLogsList(Guid versionId)
        {
            try
            {
                return Ok(await _ICmsCaseTemplate.GetCmsDraftTemplateVersionLogsList(versionId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get CmsDrafted Template Version Logs List
        [HttpGet("GetDraftParentEntityDetails")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2024-07-28' Version="1.0" Branch="master"> Get Parent Entity Detail of Draft</History>
        public async Task<IActionResult> GetDraftParentEntityDetails(Guid referenceId, string userId)
        {
            try
            {
                return Ok(await _ICmsCaseTemplate.GetDraftParentEntityDetails(referenceId, userId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Cancel Draft Document
        [HttpDelete("SoftDeleteDraftDocumentById")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Noman Khan' Date='2024-22-09' Version="1.0" Branch="master"> Delete Draft Document detail by id/History>
        public async Task<IActionResult> SoftDeleteDraftDocumentById(Guid draftId, string userName)
        {
            try
            {
                return Ok(await _ICmsCaseTemplate.SoftDeleteDraftDocumentById(draftId, userName));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        
        #endregion
    }
}
