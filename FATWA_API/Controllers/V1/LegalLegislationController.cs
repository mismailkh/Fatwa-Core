using FATWA_DOMAIN.Enums;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.Notification;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.Lms;
using FATWA_DOMAIN.Models.ViewModel.Meet;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection.Metadata;
using static FATWA_DOMAIN.Enums.LegalLegislationEnum;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace FATWA_API.Controllers.V1
{

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LegalLegislationController : ControllerBase
    {

        #region Constructor
        public LegalLegislationController(ILegalLegislation iLegalLegislation, IAuditLog auditLogs, INotification iNotifications, IAccount iAccount)
        {
            _iLegalLegislation = iLegalLegislation;
            _auditLogs = auditLogs;
            _iNotifications = iNotifications;
            _IAccount = iAccount;
        }
        #endregion

        #region Variables declaration
        private readonly IAuditLog _auditLogs;
        private readonly INotification _iNotifications;
        private readonly IAccount _IAccount;

        #endregion

        #region Interface initialized
        private readonly ILegalLegislation _iLegalLegislation;
        #endregion

        #region Get legislation detail by using id
        [HttpGet("GetLegalLegislationDetailByUsingId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLegalLegislationDetailByUsingId(Guid legislationId)
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

                var result = await _iLegalLegislation.GetLegalLegislationDetailByUsingId(legislationId);
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

        #region Get legislation type details

        [HttpGet("GetLegislationTypeDetails")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLegislationTypeDetails()
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

                var result = await _iLegalLegislation.GetLegislationTypeDetails();
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

        #region Get legislation status details

        [HttpGet("GetLegislationStatusDetails")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLegislationStatusDetails()
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

                var result = await _iLegalLegislation.GetLegislationStatusDetails();
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

        #region Get legislation flow status details

        [HttpGet("GetLegislationFlowStatusDetails")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLegislationFlowStatusDetails()
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

                var result = await _iLegalLegislation.GetLegislationFlowStatusDetails();
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

        #region Get legislation Tag details

        [HttpGet("GetLegislationTagDetails")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLegislationTagDetails()
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

                var result = await _iLegalLegislation.GetLegislationTagDetails();
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

        #region Get publication source name details

        [HttpGet("GetPublicationSourceNameDetails")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetPublicationSourceNameDetails()
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

                var result = await _iLegalLegislation.GetPublicationSourceNameDetails();
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

        #region Get legal section parent list, new number & add

        [HttpGet("GetLegalSectionParentList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLegalSectionParentList()
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

                var result = await _iLegalLegislation.GetLegalSectionParentList();
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

        [HttpGet("GetLegalSectionNewNumber")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLegalSectionNewNumber()
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

                var result = await _iLegalLegislation.GetLegalSectionNewNumber();
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

        [HttpPost("AddLegalSection")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddLegalSection(LegalSection item)
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

                var result = await _iLegalLegislation.AddLegalSection(item);
                if (result)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Adding legislation section",
                        Task = "Adding legislation section process",
                        Description = "User able to add legislation section successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Adding legislation section executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });

                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Adding legislation section Failed",
                    Body = ex.Message,
                    Category = "User unable to add legislation section",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Adding legislation section Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get article status details

        [HttpGet("GetLegalArticleStatusList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLegalArticleStatusList()
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

                var result = await _iLegalLegislation.GetLegalArticleStatusList();
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

        #region Advance search relation

        [HttpPost("AdvanceSearchRelation")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AdvanceSearchRelation(LegalLegislationVM item)
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

                var result = await _iLegalLegislation.AdvanceSearchRelation(item);
                if (result.Count() != 0)
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

        #region Article Effects (Existing legislation cancel, Article cancel, modify & add)

        [HttpPost("ExistingLegislationStatusChange")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> ExistingLegislationStatusChange(LegalLegislationVM args)
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

                var result = await _iLegalLegislation.ExistingLegislationStatusChange(args);
                if (result != null)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Existing legislation status change",
                        Task = "Existing legislation status change process",
                        Description = "User able to change existing legislation status successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Existing legislation status change executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });

                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Existing legislation status change Failed",
                    Body = ex.Message,
                    Category = "User unable to change existing legislation status change",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Existing legislation status change Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(ex.Message);
            }
        }
        [HttpPost("ExistingArticleStatusChange")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> ExistingArticleStatusChange(LegalArticle args)
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

                var result = await _iLegalLegislation.ExistingArticleStatusChange(args);
                if (result != null)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Existing legislation article status change",
                        Task = "Existing legislation article status change process",
                        Description = "User able to change existing legislation article status successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Existing legislation article status change executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });

                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Existing legislation article status change Failed",
                    Body = ex.Message,
                    Category = "User unable to change existing legislation article status",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Existing legislation article status change Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(ex.Message);
            }
        }
        [HttpPost("AddExistingArticleNewChilFromEffectsGrid")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddExistingArticleNewChilFromEffectsGrid(LegalArticle args)
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

                var result = await _iLegalLegislation.AddExistingArticleNewChilFromEffectsGrid(args);
                if (result != null)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Adding new legislation article from existing article",
                        Task = "Adding new legislation article from existing article process",
                        Description = "User able to adding new legislation article from existing article successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Adding new legislation article from existing article executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Adding new legislation article from existing article Failed",
                    Body = ex.Message,
                    Category = "User unable to adding new legislation article from existing article Failed",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Adding new legislation article from existing article Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get Legislation

        [HttpPost("GetLegalLegislations")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nadia Gull' Date='2022-10-14' Version="1.0" Branch="master"> Get LegalLegislations list</History>
        public async Task<IActionResult> GetLegalLegislations(AdvanceSearchLegalLegislationsVM advanceSearchVM)
        {
            try
            {
                var res = await _iLegalLegislation.GetLegalLegislations(advanceSearchVM);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("GetLegalLegislationsDms")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nadia Gull' Date='2022-10-14' Version="1.0" Branch="master"> Get LegalLegislations list</History>
        public async Task<IActionResult> GetLegalLegislationsDms()
        {
            try
            {
                var res = await _iLegalLegislation.GetLegalLegislationsDms();
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get LegislationForPublish

        [HttpPost("GetLegislationForPublish")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nadia Gull' Date='2022-10-14' Version="1.0" Branch="master"> Get LegalLegislations list</History>
        public async Task<IActionResult> GetLegislationForPublish(AdvanceSearchLegalLegislationsVM advanceSearchVM)
        {
            try
            {
                var res = await _iLegalLegislation.GetLegislationForPublish(advanceSearchVM);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get LegislationDecision

        [HttpGet("GetLegalLegislationsDecision")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Iftikhar' Date='2022-10-20' Version="1.0" Branch="master"> Get LegalLegislations Decision</History>
        public async Task<IActionResult> GetLegalLegislationsDecision(Guid LegislationId)
        {
            try
            {
                var res = await _iLegalLegislation.GetLegalLegislationsDecision(LegislationId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Post LegislationDecision

        [HttpPost("UpdateLegalLegislationDecision")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Iftikhar' Date='2022-10-21' Version="1.0" Branch="master"> Get LegalLegislations Decision</History>
        public async Task<IActionResult> UpdateLegalLegislationDecision(LegalLegislationDecisionVM legalLegislationDecisionVM)
        {
            try
            {
                await _iLegalLegislation.UpdateLegalLegislationDecision(legalLegislationDecisionVM);

                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Take legislation decision",
                    Task = "Take legislation decision process",
                    Description = "User able to take legislation decision successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Legislation decision executed Successfully",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                if (legalLegislationDecisionVM.FlowStatusId == (int)LegislationFlowStatusEnum.Unpublished)
                {
                    var notificationResult = await _iNotifications.SendNotification(new Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        DueDate = DateTime.Now.AddDays(5),
                        CreatedBy = legalLegislationDecisionVM.ModifiedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReceiverId = await _IAccount.UserIdByUserEmail(legalLegislationDecisionVM.AddedBy),
                        ModuleId = (int)WorkflowModuleEnum.LDSDocument,
                    },
                        (int)NotificationEventEnum.UpdateLegalLegislation,
                    "detailview",
                    legalLegislationDecisionVM.GetType().Name,
                       legalLegislationDecisionVM.LegislationId.ToString(),
                       legalLegislationDecisionVM.NotificationParameter);
                }
                return Ok();
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "taking legislation decision Failed",
                    Body = ex.Message,
                    Category = "User unable to take legislation decision",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Legislation decision Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("ModifiedExistingArticleNewChilFromEffectsGrid")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> ModifiedExistingArticleNewChilFromEffectsGrid(LegalArticle args)
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

                var result = await _iLegalLegislation.ModifiedExistingArticleNewChilFromEffectsGrid(args);
                if (result != null)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Existing legislation article modified",
                        Task = "Existing legislation article modified process",
                        Description = "User able to modified existing legislation article successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Existing legislation article modified executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });

                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Existing legislation article modified Failed",
                    Body = ex.Message,
                    Category = "User unable to modify existing legislation article",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Existing legislation article modified Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get all article list by using legislationId

        [HttpGet("GetLatestArticleDetailByUsingLegislationId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLatestArticleDetailByUsingLegislationId(Guid legislationId)
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

                var result = await _iLegalLegislation.GetLatestArticleDetailByUsingLegislationId(legislationId);
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

        #region Save legal legislation

        [HttpPost("SaveLegalLegislation")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SaveLegalLegislation(LegalLegislation args)
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

                var result = await _iLegalLegislation.SaveLegalLegislation(args);
                if (result)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Adding legislation",
                        Task = "Adding legislation process",
                        Description = "User able to Adding legislation successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Adding legislation executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });

                    //if(args.Legislation_Flow_Status == (int)LegislationFlowStatusEnum.InReview)
                    //{
                    //    var usersResult = await _iLegalLegislation.GetUserByRole(args.RoleId);
                    //    foreach (var user in usersResult)
                    //    {
                    //        args.NotificationParameter.Entity = new CaseFile().GetType().Name;
                    //        var notificationResult = await _iNotifications.SendNotification(new Notification
                    //        {
                    //            NotificationId = Guid.NewGuid(),
                    //            DueDate = DateTime.Now.AddDays(5),
                    //            CreatedBy = args.AddedBy,
                    //            CreatedDate = DateTime.Now,
                    //            IsDeleted = false,
                    //            ReceiverId = user.UserId,
                    //            ModuleId = (int)WorkflowModuleEnum.LMSLiterature,
                    //        },
                    //        (int)NotificationEventEnum.SaveLegalLegislation,
                    //        "detailview",
                    //        args.GetType().Name,
                    //        args.LegislationId.ToString(),
                    //        args.NotificationParameter);

                    //    }
                    //}

                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Adding legislation Failed",
                    Body = ex.Message,
                    Category = "User unable to Adding legislation",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Adding legislation Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Update legal legislation

        [HttpPost("UpdateLegalLegislation")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateLegalLegislation(LegalLegislation args)
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

                var result = await _iLegalLegislation.UpdateLegalLegislation(args);
                if (result)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Update legislation",
                        Task = "Update legislation process",
                        Description = "User able to Update legislation successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Update legislation executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Update legislation Failed",
                    Body = ex.Message,
                    Category = "User unable to Update legislation decision",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Update legislation Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Check Legislation Number Duplication

        [HttpGet("CheckLegislationNumberDuplication")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CheckLegislationNumberDuplication(int legislationType, string legislationNumber)
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

                var result = await _iLegalLegislation.CheckLegislationNumberDuplication(legislationType, legislationNumber);
                if (result)
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

        #region Legal legislation Detail View 
        [HttpGet("GetLegislationReferencebyLegislationId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLegislationReferencebyLegislationId(Guid LegislationId)
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
                List<LegalLegislationReference> result = await _iLegalLegislation.GetLegislationReferencebyLegislationId(LegislationId);
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
        //[HttpGet("GetLegislationbyLegislationId")]
        //[MapToApiVersion("1.0")]
        //[AllowAnonymous]
        //public async Task<IActionResult> GetLegislationbyLegislationId(Guid LegislationId)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(new RequestFailedResponse
        //            {
        //                Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
        //            });
        //        }

        //        LegalLegislation result = await _iLegalLegislation.GetLegislationbyLegislationId(LegislationId);
        //        if (result != null)
        //        {
        //            return Ok(result);
        //        }
        //        return NotFound();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        [HttpGet("GetLegislationNoteLegislationId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLegislationNoteLegislationId(Guid LegislationId)
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

                LegalNote result = await _iLegalLegislation.GetLegislationNoteLegislationId(LegislationId);
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
        [HttpGet("GetLegislationExplanatoryNoteLegislationId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLegislationExplanatoryNoteLegislationId(Guid LegislationId)
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

                LegalExplanatoryNote result = await _iLegalLegislation.GetLegislationExplanatoryNoteLegislationId(LegislationId);
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
        [HttpGet("GetLegislationSignaturesbyLegislationId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLegislationSignaturesbyLegislationId(Guid LegislationId)
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

                List<LegalLegislationSignature> result = await _iLegalLegislation.GetLegislationSignaturesbyLegislationId(LegislationId);
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
        [HttpGet("GetLegalArticalSectionByLegislationId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLegalArticalSectionByLegislationId(Guid LegislationId)
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

                List<LegalArticalSectionVM> result = await _iLegalLegislation.GetLegalArticalSectionByLegislationId(LegislationId);
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

        [HttpGet("GetLegalClausesSectionByLegislationId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLegalClausesSectionByLegislationId(Guid LegislationId)
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

                List<LegalClausesSectionVM> result = await _iLegalLegislation.GetLegalClausesSectionByLegislationId(LegislationId);
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

        [HttpGet("GetLegalPublicationSourceDetailByLegislationId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLegalPublicationSourceDetailByLegislationId(Guid LegislationId)
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

                LegalPublicationSourceVM result = await _iLegalLegislation.GetLegalPublicationSourceDetailByLegislationId(LegislationId);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetLegalLegislationDetailById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLegalLegislationDetailById(Guid LegislationId)
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

                LegalLegislationDetailVM result = await _iLegalLegislation.GetLegalLegislationDetailById(LegislationId);
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
        [HttpGet("GetLegalLegislationDetailPreviewById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLegalLegislationDetailPreviewById(Guid LegislationId)
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
                LegalLegislation result = await _iLegalLegislation.GetLegalLegislationDetailPreviewById(LegislationId);
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


        [HttpGet("GetlegislationDetailById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetlegislationDetailById(Guid LegislationId)
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
                LegalLegislationDetailVM result = await _iLegalLegislation.GetLegalLegislationDetailById(LegislationId);
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

        #region Get Legal Template Details

        [HttpGet("GetLegalTemplateDetails")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLegalTemplateDetails()
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

                var result = await _iLegalLegislation.GetLegalTemplateDetails();
                if (result.Count() != 0)
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

        #region Get legal template seytting details

        [HttpGet("GetLegalTemplateSettingDetails")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLegalTemplateSettingDetails()
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

                var result = await _iLegalLegislation.GetLegalTemplateSettingDetails();
                if (result.Count() != 0)
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

        #region Save legal Legislation Tags

        [HttpPost("SaveLegalLegislationTags")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2022-04-22' Version="1.0" Branch="master"> Handle create  legal Legislation Tags</History>
        public async Task<IActionResult> SaveLegalLegislationTags(LegalLegislationTag legalLegislationTag)
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
                var result = await _iLegalLegislation.CreateLegalLegislationTags(legalLegislationTag);
                if (result != null)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Adding legislation tag",
                        Task = "Adding legislation tag process",
                        Description = "User able to Adding legislation tag successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Adding legislation tag executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });

                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Adding legislation tag Failed",
                    Body = ex.Message,
                    Category = "User unable to Adding legislation tag",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Adding legislation tag Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)ModuleEnum.LegalLibrarySystem,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(legalLegislationTag);
            }
        }

        #endregion

        #region Check and get template selected value details

        [HttpGet("GetRegisteredTemplateDetailsByUsingSelectedTemplateId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetRegisteredTemplateDetailsByUsingSelectedTemplateId(Guid templateId)
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

                var result = await _iLegalLegislation.GetRegisteredTemplateDetailsByUsingSelectedTemplateId(templateId);
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

        #region Count Associated Legislation In Template By Using TemplateId

        [HttpGet("CountAssociatedLegislationInTemplateByUsingTemplateId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CountAssociatedLegislationInTemplateByUsingTemplateId(Guid templateId)
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

                var result = await _iLegalLegislation.CountAssociatedLegislationInTemplateByUsingTemplateId(templateId);
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

        #region SoftDelete LegalLegislation
        [HttpPost("SoftDeleteLegalLegislation")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2022-11-14' Version="1.0" Branch="master"> Soft Delete Legal Legislation </History>
        public async Task<IActionResult> SoftDeleteLegalLegislation(LegalLegislationsVM legalLegislationsVM)
        {
            try
            {
                await _iLegalLegislation.SoftDeleteLegalLegislation(legalLegislationsVM);

                //string assignedTo = await _IAccount.UserIdByUserEmail(legalLegislationsVM.AddedBy);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Delate legislation",
                    Task = "Delate legislation process",
                    Description = "User able to Delate legislation successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Delate legislation executed Successfully",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                // Notification
                var notificationResult = await _iNotifications.SendNotification(new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    DueDate = DateTime.Now.AddDays(5),
                    CreatedBy = legalLegislationsVM.ModifiedBy,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    ReceiverId = "436e82d2-70d8-455c-a643-7909b8689667",
                    ModuleId = (int)WorkflowModuleEnum.LDSDocument,
                },
                 (int)NotificationEventEnum.SoftDeleteLegalLegislation,
                 "detailview",
                 new LegalLegislation().GetType().Name,
                 legalLegislationsVM.LegislationId.ToString() + "/" + "Deletelegalligislation",
                 legalLegislationsVM.NotificationParameter);
                return Ok();
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Deleting legislation Failed",
                    Body = ex.Message,
                    Category = "User unable to delete legislation",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Legislation deleting Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(ex.Message);
            }


        }
        #endregion

        #region Get Legal Legislation Details By Using LegislationId For Edit Form

        [HttpGet("GetLegalLegislationDetailsByUsingLegislationIdForEditForm")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLegalLegislationDetailsByUsingLegislationIdForEditForm(Guid legislationId)
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

                var result = await _iLegalLegislation.GetLegalLegislationDetailsByUsingLegislationIdForEditForm(legislationId);
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

        #region Get Explanatory Note Attachment From Temp Table By Using ExplanatoryNoteId

        [HttpGet("GetExplanatoryNoteAttachmentFromTempTableByUsingId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetExplanatoryNoteAttachmentFromTempTableByUsingId(Guid explanatoryNoteId)
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

                var result = await _iLegalLegislation.GetExplanatoryNoteAttachmentFromTempTableByUsingId(explanatoryNoteId);
                if (result.Count() != 0)
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

        #region Update Selected Section As Parent HasChild Column

        [HttpGet("UpdateSelectedSectionAsParentHasChildColumn")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateSelectedSectionAsParentHasChildColumn(Guid sectionId)
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

                var result = await _iLegalLegislation.UpdateSelectedSectionAsParentHasChildColumn(sectionId);
                if (result)
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

        #region Delete Attachment From Temp Table
        [HttpGet("DeleteAttachmentFromTempTable")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2022-11-14' Version="1.0" Branch="master"> Soft Delete Legal Legislation </History>
        public async Task<IActionResult> DeleteAttachmentFromTempTable(Guid legislationId)
        {
            try
            {
                var result = await _iLegalLegislation.DeleteAttachmentFromTempTable(legislationId);
                return Ok(result);

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }
        #endregion

        #region Get Legal Legislation Reference By LegislationId

        [HttpGet("GetLegalLegislationReferenceByLegislationId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLegalLegislationReferenceByLegislationId(Guid legislationId)
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

                var result = await _iLegalLegislation.GetLegalLegislationReferenceByLegislationId(legislationId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Revoke Delete LegalLegislation
        [HttpPost("RevokeDeleteLegalLegislation")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2022-12-09' Version="1.0" Branch="master"> Revoke Delete Legal Legislation </History>
        public async Task<IActionResult> RevokeDeleteLegalLegislation(LegalLegislationsVM legalLegislationsVM)
        {
            try
            {
                await _iLegalLegislation.RevokeDeleteLegalLegislation(legalLegislationsVM);

                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Revoke Delate legislation",
                    Task = "Revoke Delate legislation process",
                    Description = "User able to Revoke Delate legislation successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Revoke Delate legislation executed Successfully",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LDSDocument,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                // Notification
                string assignedTo = await _IAccount.UserIdByUserEmail(legalLegislationsVM.AddedBy);
                var notificationResult = await _iNotifications.SendNotification(new Notification
                {
                    NotificationId = Guid.NewGuid(),
                    DueDate = DateTime.Now.AddDays(5),
                    CreatedBy = legalLegislationsVM.ModifiedBy,
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    ReceiverId = assignedTo,
                    ModuleId = (int)WorkflowModuleEnum.LDSDocument,
                },
                 (int)NotificationEventEnum.RevokeDeleteLegalLegislation,
                 "detailview",
                 new LegalLegislation().GetType().Name,
                 legalLegislationsVM.LegislationId.ToString(),
                 legalLegislationsVM.NotificationParameter);
                return Ok();
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Revoke Deleting legislation Failed",
                    Body = ex.Message,
                    Category = "User unable to Revoke delete legislation",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = " Revoke Legislation deleting Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LDSDocument,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(ex.Message);
            }


        }
        #endregion

        #region Get Delete Legislation

        [HttpGet("GetDeleteLegalLegislations")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Ijaz Ahmad' Date='2022-10-14' Version="1.0" Branch="master"> Get LegalLegislations list</History>
        public async Task<IActionResult> GetDeleteLegalLegislations(int PageSize, int PageNumber)
        {
            try
            {
                var res = await _iLegalLegislation.GetDeleteLegalLegislations(PageSize, PageNumber);
                return Ok(res);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region get approved legislation
        [HttpGet("GetApprovedLegislation")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetApprovedLegislation(string UserId, int PageSize, int PageNumber)
        {
            try
            {
                return Ok(await _iLegalLegislation.GetApporvedLegislation(UserId, PageSize, PageNumber));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get Article Number For Article Effect By Using LegislationId
        [HttpGet("GetArticleNumberForArticleEffectByUsingLegislationId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetArticleNumberForArticleEffectByUsingLegislationId(Guid legislationId)
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
                return Ok(await _iLegalLegislation.GetArticleNumberForArticleEffectByUsingLegislationId(legislationId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get legislation comments details
        [HttpGet("GetLegalLegislationCommentsDetailByUsingId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLegalLegislationCommentsDetailByUsingId(Guid legislationId)
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

                var result = await _iLegalLegislation.GetLegalLegislationCommentsDetailByUsingId(legislationId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get All Template Setting Details

        [HttpGet("GetAllTemplateSettingDetails")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllTemplateSettingDetails()
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

                var result = await _iLegalLegislation.GetAllTemplateSettingDetails();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Mobile Application End Point APIs
        [HttpPost("GetLegalLegislationsForMobileApp")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Noman Khan' Date='2024-08-28' Version="1.0" Branch="master"> Get LegalLegislations list</History>
        public async Task<IActionResult> GetLegalLegislationsForMobileApp(AdvanceSearchLegalLegislationsVM advanceSearchVM)
        {
            try
            {
                advanceSearchVM.Legislation_FlowStatus = (int)LegislationFlowStatusEnum.Published;
                var res = await _iLegalLegislation.GetLegalLegislations(advanceSearchVM);
                if (res != null && res.Count > 0)
                {
                    return Ok(new ApiCallResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        IsSuccessStatusCode = true,
                        ResultData = res,
                        Message = "success"
                    });
                }
                return NotFound(new ApiCallResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccessStatusCode = false,
                    ResultData = res,
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
        [HttpGet("GetLegalLegislationsDetailsForMobileApp")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Noman Khan' Date='2024-09-09' Version="1.0" Branch="master"> Get LegalLegislations Detail</History>
        public async Task<IActionResult> GetLegalLegislationsDetailsForMobileApp([Required] Guid LegislationId)
        {
            try
            {
                var response = await _iLegalLegislation.GetLegalLegislationsDetailsForMobileApp(LegislationId);
                if (response != null)
                {
                    return Ok(new ApiCallResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        IsSuccessStatusCode = true,
                        ResultData = response,
                        Message = "success"
                    });
                }
                return NotFound(new ApiCallResponse
                {
                    StatusCode = HttpStatusCode.NotFound,
                    IsSuccessStatusCode = false,
                    ResultData = response,
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

        #region Get Legal Legislation Approvals Details

        [HttpGet("GetLegalLegislationApprovals")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLegalLegislationApprovals(string UserId, int PageSize, int PageNumber)
        {
            try
            {
                return Ok(await _iLegalLegislation.GetLegalLegislationApprovals(UserId, PageSize, PageNumber));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
