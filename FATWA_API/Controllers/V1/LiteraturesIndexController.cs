using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using FATWA_GENERAL.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_API.Controllers.V1
{
    //<!-- <History Author = 'Umer Zaman' Date='2022-03-25' Version="1.0" Branch="master">Create class for manage api controller</History>

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LmsLiteratureIndexController : ControllerBase
    {
        private readonly ILmsLiteratureIndex _iLmsLiteratureIndex;
        private readonly IAuditLog _auditLogs;

        public LmsLiteratureIndexController(ILmsLiteratureIndex iLmsLiteratureIndex, IAuditLog audit)
        {
            _iLmsLiteratureIndex = iLmsLiteratureIndex;
            _auditLogs = audit; 
        }
        #region Get Lms Literature Index's Details / By Id

        [HttpGet("GetLmsLiteratureIndexs")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLmsLiteraturesIndexs()
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
                return Ok(await _iLmsLiteratureIndex.GetLmsLiteratureIndexs());

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetLmsLiteraturesIndexSync")]
        [MapToApiVersion("1.0")]
        public IActionResult GetLmsLiteraturesIndexSync()
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
                return Ok(_iLmsLiteratureIndex.GetLmsLiteratureIndexSync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetLmsLiteratureIndexById")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLmsLiteratureIndexById(int indexId)
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

                LmsLiteratureIndex lit = await _iLmsLiteratureIndex.GetLiteratureIndexDetail(indexId);
                if (lit != null)
                {
                    return Ok(lit);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetLmsLiteratureIndexesIdByIndexNumber")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLmsLiteratureIndexesIdByIndexNumber(string indexNumber)
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
                return Ok(await _iLmsLiteratureIndex.GetLmsLiteratureIndexesIdByIndexNumber(indexNumber));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetLmsLiteratureIndexDetailByUsingNameAndIndexNumber")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLmsLiteratureIndexDetailByUsingNameAndIndexNumber(string indexNumber, string name_en, string name_ar)
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
                var obj = await _iLmsLiteratureIndex.GetLmsLiteratureIndexDetailByUsingNameAndIndexNumber(indexNumber, name_en, name_ar);
                if (obj != null)
                {
                    return Ok(obj);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetLmsLiteratureIndexDetilsByUsingParentIdAndNumber")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLmsLiteratureIndexDetilsByUsingParentIdAndNumber(int parentIndexId, string parentIndexNumber)
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
                var obj = await _iLmsLiteratureIndex.GetLmsLiteratureIndexDetilsByUsingParentIdAndNumber(parentIndexId, parentIndexNumber);
                return Ok(obj);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetLmsLiteratureIndexDivisions")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLmsLiteratureIndexDivisions(int indexId)
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
                return Ok(await _iLmsLiteratureIndex.GetLmsLiteratureIndexDivisions(indexId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetLmsLiteratureIndexNumberDetailsForAddDivision")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLmsLiteratureIndexNumberDetailsForAddDivision(string indexNumber)
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
                return Ok(await _iLmsLiteratureIndex.GetLmsLiteratureIndexNumberDetailsForAddDivision((string)indexNumber));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetLiteratureIndexDetailByUsingIndexId")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLiteratureIndexDetailByUsingIndexId(int indexId)
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
                return Ok(await _iLmsLiteratureIndex.GetLiteratureIndexDetailByUsingIndexId((int)indexId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetLiteratureIndexByIndexIdAndNumber")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLiteratureIndexByIndexIdAndNumber(int indexId, string indexNumber)
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
                var obj = await _iLmsLiteratureIndex.GetLiteratureIndexByIndexIdAndNumber(indexId, indexNumber);
                if (obj != null)
                {
                    return Ok(obj);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Create

        [HttpPost("CreateLmsLiteratureIndex")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CreateLmsLiteratureIndex(LmsLiteratureIndex lmsLiteraturesIndex)
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
                await _iLmsLiteratureIndex.CreateLmsLiteratureIndex(lmsLiteraturesIndex);

                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Adding Book Index Data",
                    Task = "Adding Book Index Data",
                    Description = "User able to Add Book Index Data successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Adding Book Index Data executed Successfully",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok(lmsLiteraturesIndex);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Adding Book Index Data Failed",
                    Body = ex.Message,
                    Category = "User unable Add Book Index Data",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Adding Book Index Data Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });

            }
        }
        #endregion

        #region Update

        [HttpPost("UpdateLmsLiteratureIndex")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateLmsLiteratureIndex(LmsLiteratureIndex lmsLiteraturesIndex)
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
                await _iLmsLiteratureIndex.UpdateLmsLiteratureIndex(lmsLiteraturesIndex);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Updating Book Index Data",
                    Task = "Updating Book Index Data",
                    Description = "User able to Update Book Index Data successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Update Book Index Data executed Successfully",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return Ok(lmsLiteraturesIndex);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogId = Guid.NewGuid(),
                    ErrorLogTypeId = Guid.NewGuid(),
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Updating Book Index Data Failed",
                    Body = ex.Message,
                    LogDate = DateTime.Now,
                    Category = "User unable to Update Book Index Data",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Computer = Environment.MachineName.ToString(),
                    Message = "Update Book Index Data Failed",
                    UserName = "",
                    TerminalID = "C0-B6-F9-1A-D8-89/ hostname/MAC address",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ChannelName = "Web",
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });

            }
        }
        #endregion

        #region Delete

        [HttpPost("SoftDeleteLiteratureIndex")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SoftDeleteLiteratureIndex(LmsLiteratureIndex UniqueLiteratureIndex)
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
                await _iLmsLiteratureIndex.DeleteLmsLiteratureIndex(UniqueLiteratureIndex);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion


        #region Check IndexId Associate with Literatuer

        [HttpGet("CheckLmsLiteratureIndexIdAssociatedWithLiteratures")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public IActionResult CheckLmsLiteratureIndexIdAssociatedWithLiteratures(int indexId)
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
                return Ok(_iLmsLiteratureIndex.CheckLmsLiteratureIndexIdAssociatedWithLiteratures(indexId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
