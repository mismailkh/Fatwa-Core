using FATWA_DOMAIN.Common.Service;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.Notification;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.Lms;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;
using NotificationEventEnum = FATWA_GENERAL.Helper.Enum.NotificationEventEnum;
//using NotificationTypeEnum = FATWA_GENERAL.Helper.Enum.NotificationTypeEnum;

namespace FATWA_API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LmsLiteraturesController : ControllerBase
    {
        private readonly ILmsLiterature _ILmsLiteratures;
        private readonly LmsLiteratureService _literatureService;
        private readonly IAuditLog _auditLogs;
        private readonly IWebHostEnvironment _environment;
        private readonly INotification _INotification;

        public LmsLiteraturesController(ILmsLiterature iLmsLiteratures, LmsLiteratureService service,
            IWebHostEnvironment environment, IAuditLog audit, INotification iNotification)
        {
            _ILmsLiteratures = iLmsLiteratures;
            _literatureService = service;
            _environment = environment;
            _auditLogs = audit;
            _INotification = iNotification;
        }

        [HttpGet("GetLmsLiteratures")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLmsLiteratures()
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
                return Ok(await _ILmsLiteratures.GetLmsLiteratures());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetLMSLiteratureDetailById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLMSLiteratureDetailById(int LiteratureId)
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
                return Ok(await _ILmsLiteratures.GetLMSLiteratureDetailById(LiteratureId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetBorrowDetailById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetBorrowDetailById(int LiteratureId, string UserId, string RoleName)
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
                return Ok(await _ILmsLiteratures.GetBorrowDetailById(LiteratureId, UserId, RoleName));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetLMSLiteratureAuthorslById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLMSLiteratureAuthorslById(int LiteratureId)
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
                return Ok(await _ILmsLiteratures.GetLMSLiteratureAuthorsById(LiteratureId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllActiveLiteratureTags")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllActiveLiteratureTags()
        {
            try
            {
                return Ok(await _ILmsLiteratures.GetAllActiveLiteratureTags());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetLmsLiteraturesExport")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLmsLiteraturesExport(string ExportToken)
        {
            if (ExportToken == "Business123#@!")
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
                    return Ok(await _ILmsLiteratures.GetLmsLiteratures());
                }
                catch (Exception ex)
                {
                    return BadRequest(ex.Message);
                }
            }
            else
            {
                return BadRequest("Invalid Token you are not Authorized");
            }

        }
        [HttpPost("GetLmsLiteraturesAdvanceSearch")]

        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLmsLiteraturesAdvanceSearch(LiteratureAdvancedSearchVM advancedSearch)
        {
            try
            {
                return Ok(await _ILmsLiteratures.GetLmsLiteraturesAdvanceSearch(advancedSearch));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("GetLmsLiteraturesSync")]
        [MapToApiVersion("1.0")]
        public IActionResult GetLmsLiteraturesSync()
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
                return Ok(_ILmsLiteratures.GetLmsLiteraturesSync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateLmsLiterature")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CreateLmsLiterature(LmsLiterature literature)
        {
            try
            {
                var literatureResult = await _ILmsLiteratures.CreateLmsLiterature(literature);
                if (literatureResult.LiteratureIdList.Any())
                {
                    foreach (var item in literatureResult.LiteratureIdList)
                    {
                        _auditLogs.CreateProcessLog(new ProcessLog
                        {
                            Process = "Create LMS Literature",
                            Task = "Create LMS Literature",
                            Description = "User able to Create LMS Literature successfully.",
                            ProcessLogEventId = (int)ProcessLogEnum.Processed,
                            Message = "Creating LMS Literature executed Successfully",
                            IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                            ApplicationID = (int)PortalEnum.FatwaPortal,
                            ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                            Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                        });
                    }
                    if (literature.IsDraft == false && literature.RoleId == null)
                    {
                        var lmsAdminUsers = await _ILmsLiteratures.GetLmsAdminUser();
                        foreach (var user in lmsAdminUsers)
                        {
                            foreach (var item in literatureResult.LiteratureIdList)
                            {
                                literature.NotificationParameter.Entity = new CaseFile().GetType().Name;
                                var notificationResult = await _INotification.SendNotification(new Notification
                                {
                                    NotificationId = Guid.NewGuid(),
                                    DueDate = DateTime.Now.AddDays(5),
                                    CreatedBy = literature.CreatedBy,
                                    CreatedDate = DateTime.Now,
                                    IsDeleted = false,
                                    ReceiverId = user.UserId,
                                    ModuleId = (int)WorkflowModuleEnum.LMSLiterature,
                                },
                                (int)NotificationEventEnum.AddLiterature,
                                new LmsLiterature().GetType().Name,
                                "edit",
                                item.ToString(),
                                literature.NotificationParameter);
                            }

                        }
                        var fatwaAdminUsers = await _ILmsLiteratures.GetFatwaAdminUser();
                        foreach (var user in fatwaAdminUsers)
                        {
                            foreach (var item in literatureResult.LiteratureIdList)
                            {
                                literature.NotificationParameter.Entity = new CaseFile().GetType().Name;
                                var notificationResult = await _INotification.SendNotification(new Notification
                                {
                                    NotificationId = Guid.NewGuid(),
                                    DueDate = DateTime.Now.AddDays(5),
                                    CreatedBy = literature.CreatedBy,
                                    CreatedDate = DateTime.Now,
                                    IsDeleted = false,
                                    ReceiverId = user.UserId,
                                    ModuleId = (int)WorkflowModuleEnum.LMSLiterature,
                                },
                                (int)NotificationEventEnum.AddLiterature,
                                new LmsLiterature().GetType().Name,
                                "edit",
                                item.ToString(),
                                literature.NotificationParameter);
                            }

                        }
                    }
                }

                return Ok(literatureResult);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Create LMS Literature Failed",
                    Body = ex.Message,
                    Category = "User unable to Create LMS Literature",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Create LMS Literature Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetLmsLiteratureById")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(int id)
        {
            LmsLiterature literatureobj = await _ILmsLiteratures.GetLiteratureDetail((int)id);
            if (literatureobj != null)
            {
                return Ok(literatureobj);
            }
            return NotFound();
        }

        [HttpGet("GetLmsLiteratureTagById")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLmsLiteratureTagById(int id)
        {
            LmsLiterature literatureobj = await _ILmsLiteratures.GetLiteratureDetailTagById((int)id);
            if (literatureobj != null)
            {
                return Ok(literatureobj);
            }
            return NotFound();
        }

        [HttpGet("GetNewLmsLiteratureNumber")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetNewLmsLiteratureNumber()
        {
            try
            {
                return Ok(await _ILmsLiteratures.GetNewLmsLiteratureNumber());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateLmsLiterature")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateLmsLiterature(LmsLiterature literature)
        {
            try
            {
                var lmsliterature = await _ILmsLiteratures.UpdateLmsLiterature(literature);
                if (lmsliterature != null)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "Update LMS Literature",
                        Task = "Update LMS Literature",
                        Description = "User able to Update LMS Literature successfully.",
                        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                        Message = "Updating LMS Literature executed Successfully",
                        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                        ApplicationID = (int)PortalEnum.FatwaPortal,
                        ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    });
                }
                return Ok(lmsliterature);

            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Update LMS Literature Failed",
                    Body = ex.Message,
                    Category = "User unable to Update The LMS Literature",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Update LMS Literature Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpPost("SoftDeleteLmsLiterature")]
        [MapToApiVersion("1.0")]
        public async Task SoftDeleteLmsLiterature(LmsLiterature literature)
        {
            try
            {
                await _ILmsLiteratures.SoftDeleteLmsLiterature(literature);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Delete LMS Literature",
                    Task = "Delete LMS Literature",
                    Description = "User able to Delete LMS Literature successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Deleting LMS Literature executed Successfully",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Delete LMS Literature Failed",
                    Body = ex.Message,
                    Category = "User unable to Delete LMS Literature",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Delete LMS Literature Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                throw new Exception(ex.Message);
            }
        }

        [HttpPost("SoftDeleteLiterature")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SoftDeleteLiterature(List<LiteratureDetailVM> literatures)
        {
            try
            {
                await _ILmsLiteratures.SoftDeleteLiterature(literatures);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Delete Literature",
                    Task = "Delete Literature",
                    Description = "User able to Delete  Literature successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Deleting Literature executed Successfully",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                if (literatures.Count() > 0)
                {
                    foreach (var literature in literatures)
                    {
                        var userId = await _ILmsLiteratures.GetUserIdByEmail(literature.DeletedBy);
                        var notificationResult = await _INotification.SendNotification(new Notification
                        {
                            NotificationId = Guid.NewGuid(),
                            DueDate = DateTime.Now.AddDays(5),
                            CreatedBy = literature.DeletedBy,
                            CreatedDate = DateTime.Now,
                            IsDeleted = false,
                            ReceiverId = userId,
                            ModuleId = (int)WorkflowModuleEnum.LMSLiterature,
                        },
                        (int)NotificationEventEnum.DeleteLiterature,
                         new LmsLiterature().GetType().Name,
                        "list",
                        literature.LiteratureId.ToString(),
                        literature.NotificationParameter);
                        return Ok();
                    }
                }

                return Ok();

            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Delete Literature Failed",
                    Body = ex.Message,
                    Category = "User unable to Delete LMS Literature",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Delete  Literature Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpDelete("{id}")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task Delete(int id)
        {
            try
            {
                await _ILmsLiteratures.DeleteLmsLiterature(id);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Delete LMS Literature",
                    Task = "Delete LMS Literature",
                    Description = "User able to Delete LMS Literature successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Deleting LMS Literature executed Successfully",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Delete LMS Literature Failed",
                    Body = ex.Message,
                    Category = "User unable to Delete LMS Literature",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Delete LMS Literature Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                throw new Exception(ex.Message);
            }

        }

        [HttpGet("GenerateLiteratureBarcode")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GenerateLiteratureBarcode()
        {
            try
            {
                String Barcode = _literatureService.GenerateRandomString12DigitsLiteratureBarcocode(12);
                if (Barcode != null)
                {
                    return new JsonResult(new { StatusCode = HttpStatusCode.OK, Result = true, BarCodeNumber = Barcode });
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return new JsonResult(new { StatusCode = HttpStatusCode.BadRequest, Result = false });
        }

        [HttpGet("GenerateListofLiteratureBarcode")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GenerateListofLiteratureBarcode(int copyCount)
        {
            try
            {
                var res = await _literatureService.GenerateListof12DigitsLiteratureBarcocode(12, copyCount);


                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Generate 12 digit BarCode",
                    Task = "Generate 12 digit BarCode",
                    Description = "User able to Generate 12 digit BarCode successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Generate 12 digit BarCode executed Successfully",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok(res);
            }

            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Generate 12 digit BarCode Failed",
                    Body = ex.Message,
                    Category = "User unable to Generate 12 digit BarCode",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Generate 12 digit BarCode Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetLmsLiteraturesAuthorBySearchTerm")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLmsLiteraturesAuthorBySearchTerm(string? searchTerm = "")
        {
            try
            {
                var result = await _ILmsLiteratures.GetLmsLiteraturesAuthorBySearchTerm(searchTerm);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }


        [HttpGet("GetLmsLiteratureAuthorById")]
        [MapToApiVersion("1.0")]
        public async Task<LmsLiteratureAuthor> GetLmsLiteratureAuthorById(int authorId)
        {
            return await _ILmsLiteratures.GetLmsLiteratureAuthorById(authorId);
        }

        [HttpGet("GetLmsLiteratureCountByAuthorId")]
        [MapToApiVersion("1.0")]
        public async Task<int> GetLmsLiteratureCountByAuthorId(int authorId)
        {
            return await _ILmsLiteratures.GetLmsLiteratureCountByAuthorId(authorId);
        }

        [HttpGet("GetLmsLiteraturesBySearchTerm")]
        [MapToApiVersion("1.0")]
        public async Task<List<LiteratureDetailVM>> GetLmsLiteraturesBySearchTerm(string? searchTerm, string appCulture)
        {
            try
            {
                return await _ILmsLiteratures.GetLmsLiteraturesBySearchTerm(searchTerm, appCulture);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        // Image file upload (used by HtmlEditor components)
        [MapToApiVersion("1.0")]
        [HttpPost("UploadLiteratureFile")]
        public IActionResult FileUploadTest(List<IBrowserFile> files)
        {
            try
            {
                foreach (var file in files)
                {
                    var basePath = Path.Combine(Directory.GetCurrentDirectory() + "\\LmsLiteratureAttachments\\");
                    bool basePathExists = System.IO.Directory.Exists(basePath);
                    if (!basePathExists) Directory.CreateDirectory(basePath);
                    var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.Name)}";
                    var filePath = Path.Combine(basePath, file.Name);
                    var extension = Path.GetExtension(file.Name);
                    if (!System.IO.File.Exists(filePath))
                    {
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            file.OpenReadStream().CopyTo(stream);
                        }

                    }
                }
                return StatusCode(200);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        #region get isactive & isborrowable barcode number detail's by using literature id

        [HttpGet("GetBarcodeNumberDetailByusingLiteratureId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetBarcodeNumberDetailByusingLiteratureId(int literatureId)
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
                return Ok(await _ILmsLiteratures.GetBarcodeNumberDetailByusingLiteratureId(literatureId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Mobile Application End Point APIs
        [HttpGet("GetLMSLiteratureDetailByIdForMobileApp")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLMSLiteratureDetailByIdForMobileApp(int LiteratureId)
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
                var result = await _ILmsLiteratures.GetLMSLiteratureDetailById(LiteratureId);
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
        [HttpPost("GetLmsLiteratureListForMobileApp")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLmsLiteratureListForMobileApp(LiteratureAdvancedSearchVM advancedSearch)
        {
            try
            {
                var result = await _ILmsLiteratures.GetLmsLiteraturesForMobileApp(advancedSearch);
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
        #endregion

        #region Check RFID Value Exists

        [HttpGet("CheckRFIDValueExists")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> CheckRFIDValueExists(string barCodeNumber, string rfIdValue)
        {
            try
            {
                return Ok(await _ILmsLiteratures.CheckRFIDValueExists(barCodeNumber, rfIdValue));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Stock Taking List

        [HttpGet("GetStockTakingList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetStockTakingList(StockTakingAdvancedSearchVM advancedSearchVM)
        {
            try
            {
                return Ok(await _ILmsLiteratures.GetStockTakingList(advancedSearchVM));
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get StockTaking Detail By Id

        [HttpGet("GetStockTakingDetailById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetStockTakingDetailById(Guid Id)
        {
            try
            {
                return Ok(await _ILmsLiteratures.GetStockTakingDetailById(Id));
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion


        #region Get StockTaking Status
        [HttpGet("GetStockTakingStatuses")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetStockTakingStatuses()
        {
            try
            {
                var result = await _ILmsLiteratures.GetStockTakingStatus();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });

            }
        }
        #endregion

        #region Get Total Number of Books
        [HttpGet("GetTotalNoOfBooks")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTotalNoOfBooks()
        {
            try
            {
                var result = await _ILmsLiteratures.GetTotalNoOfBooks();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });

            }
        }
        #endregion

        #region Get Literature Books Report List Group By Barcode and StockTaking Id
        [HttpGet("GetLmsBookStockTakingReportList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLmsBookStockTakingReportList(Guid? StockTakingId)
        {
            try
            {
                return Ok(await _ILmsLiteratures.GetLmsBookStockTakingReportList(StockTakingId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message });

            }
        }
        #endregion

        #region Print Stock Taking Report
        [HttpPost("SubmitStockTakingReport")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SubmitStockTakingReport(SaveStockTakingVm saveStockTakingVm)
        {
            try
            {
                var result = await _ILmsLiteratures.SubmitStockTakingReport(saveStockTakingVm);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Submit StockTaking Report",
                    Task = "Submit StockTaking Report",
                    Description = "StockTaking report has been submitted successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "StockTaking report has been submitted successfully.",
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
                    Subject = "Submit StockTaking Report Failed",
                    Body = ex.Message,
                    Category = "User unable to Submit StockTaking Report",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Submit StockTaking Report Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Auto Generated Report Number
        [HttpGet("GetAutoGeneratedReportNumber")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAutoGeneratedReportNumber()
        {
            try
            {
                var result = await _ILmsLiteratures.GetAutoGeneratedReportNumber();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Lms StockTaking By Id 
        [HttpGet("GetLmsStockTakingById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLmsStockTakingById(Guid StockTakingId)
        {
            try
            {
                var result = await _ILmsLiteratures.GetLmsStockTakingById(StockTakingId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Approve StockTaking Report
        [HttpGet("ApproveStockTakingReport")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> ApproveStockTakingReport(Guid Id, string ApprovedBy)
        {
            try
            {
                var result = await _ILmsLiteratures.ApproveStockTakingReport(Id, ApprovedBy);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Approved StockTaking Report",
                    Task = "Approved StockTaking Report",
                    Description = "StockTaking report has been Approved successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "StockTaking report has been Approved successfully.",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok(result);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Approved StockTaking Report Failed",
                    Body = ex.Message,
                    Category = "User unable to Approved StockTaking Report",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Approved StockTaking Report Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Performers By StockTakingId
        [HttpGet("GetPerformersByStockTakingId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetPerformersByStockTakingId(Guid StockTakingId)
        {
            try
            {
                var result = await _ILmsLiteratures.GetPerformersByStockTakingId(StockTakingId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Check If Any InProgress StockTaking
        [HttpGet("CheckIfAnyInProgressStockTaking")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CheckIfAnyInProgressStockTaking()
        {
            try
            {
                var result = await _ILmsLiteratures.CheckIfAnyInProgressStockTaking();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion


        #region View Able Literature

        [HttpPost("GetLmsViewableLiteratures")]

        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLmsViewableLiteratures(LiteratureAdvancedSearchVM advancedSearch)
        {
            try
            {
                return Ok(await _ILmsLiteratures.GetLmsViewableLiteratures(advancedSearch));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Delete Lms StockTaking
        [HttpPost("DeleteLmsStockTaking")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> DeleteLmsStockTaking(LmsStockTakingListVM item)
        {
            try
            {
                await _ILmsLiteratures.DeleteLmsStockTaking(item);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse {Message = ex.Message , InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Lms StockTaking History By Id
        [HttpGet("GetLmsStockTakingHistoryById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLmsStockTakingHistoryById(Guid StockTakingId)
        {
            try
            {
                var result = await _ILmsLiteratures.GetLmsStockTakingHistoryById(StockTakingId);
                return Ok(result);
            }
            catch(Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message , InnerException = ex.InnerException?.Message});
            }
        }
        #endregion

    }
}
