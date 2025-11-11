using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //<History Author = 'Hassan Abbas' Date='2022-03-16' Version="1.0" Branch="master"> API Controller for handling request/responses</History>
    //<History Author = 'Hassan Abbas' Date='2022-09-02' Version="1.0" Branch="master"> Renamed from 'ProcessLog' to 'AuditLog' Controller for handling request/responses for both Process Logs and Error Logs</History>
    public class AuditLogController : ControllerBase
    {
        private readonly IAuditLog _IAuditLog;
       
        public AuditLogController(IAuditLog iAuditLog)
        {
            _IAuditLog = iAuditLog;
        }

        #region Process Logs

        [HttpGet("GetProcessLog")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        //<History Author = 'Hassan Abbas' Date='2022-03-16' Version="1.0" Branch="master"> Return List of Literature Classifcations</History>
        public async Task<List<ProcessLog>> GetProcessLog()
        {
            return await _IAuditLog.GetProcessLogs();
        }

        [HttpGet("GetProcessLogSync")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        //<History Author = 'Hassan Abbas' Date='2022-03-16' Version="1.0" Branch="master"> Return List of Literature Classifcations Sync Call</History>
        public List<ProcessLog> GetProcessLogSync()
        {
            return _IAuditLog.GetProcessLogsSync();
        }
        [HttpGet("GetProcessLogDetailById")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProcessLogDetailById(Guid ProcessLogId)
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

                ProcessLog lit = await _IAuditLog.GetProcessLogDetailById(ProcessLogId);
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
       

        #endregion

        #region Error Logs

        [HttpPost("GetErrorLogAdvanceSearch")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetErrorLogAdvanceSearch(ErrorLogAdvanceSearchVM errorLogAdvanceSearchVM)
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
                return Ok(await _IAuditLog.GetErrorLogAdvanceSearch(errorLogAdvanceSearchVM));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpPost("GetProcessLogAdvanceSearch")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProcessLogAdvanceSearch(ProcessLogAdvanceSearchVM processLogAdvanceSearch)
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
                return Ok(await _IAuditLog.GetProcessLogAdvanceSearch(processLogAdvanceSearch));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("GetErrorLogThroughProc")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetErrorLogAdvanceSearch()
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
                return Ok(await _IAuditLog.GetErrorLogThroughProc());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet("GetProcessLogThroughProc")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetProcessLogThroughProc()
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
                return Ok(await _IAuditLog.GetProcessLogThroughProc());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpGet("GetErrorLog")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetErrorLog()
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
                return Ok(await _IAuditLog.GetErrorLogs());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }
        [HttpGet("GetErrorLogDetailById")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetErrorLogDetailById(Guid ErrorLogId)
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

                ErrorLog lit = await _IAuditLog.GetErrorLogDetailById(ErrorLogId);
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



        [HttpGet("GetErrorLogSync")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public IActionResult GetErrorLogSync()
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
                return Ok(_IAuditLog.GetErrorLogsSync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }

        }

        #endregion

        #region Create process log for OSS portal
        [HttpPost("CreateProcessLog")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CreateProcessLog(ProcessLog processLog)
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
                processLog.IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                processLog.Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                _IAuditLog.CreateProcessLog(processLog);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }           
        }
        #endregion

        #region Create error log for OSS portal
        [HttpPost("CreateErrorLog")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CreateErrorLog(ErrorLog errorLog)
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
                errorLog.IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString();
                errorLog.Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
                _IAuditLog.CreateErrorLog(errorLog);
                return Ok(true);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }
}
