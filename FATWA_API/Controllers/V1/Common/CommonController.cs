using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;
using FATWA_INFRASTRUCTURE.Repository.CommonRepos;
using FATWA_DOMAIN.Common.Service;
using FATWA_DOMAIN.Models.Email;
using System.Net;
using FATWA_DOMAIN.Interfaces.Common;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Interfaces.PatternNumber;

namespace FATWA_API.Controllers.V1.Common
{

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme + ",FatwaApiKey")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]

    public class CommonController : ControllerBase
    {

        private readonly IAuditLog _auditLogs;
        public CommonRepository _commonRepo { get; }
        private readonly ICMSCOMSInboxOutboxRequestPatternNumber _cMSCOMSInboxOutboxRequestPatternNumber;
        public EmailService emailService = new EmailService();

        public CommonController(IAuditLog auditLogs, CommonRepository commonRepo, ICMSCOMSInboxOutboxRequestPatternNumber cMSCOMSInboxOutboxRequestPatternNumber)
        {

            _auditLogs = auditLogs;
            _commonRepo = commonRepo;
            _cMSCOMSInboxOutboxRequestPatternNumber = cMSCOMSInboxOutboxRequestPatternNumber;
        }

        [HttpGet("GetEmailConfiguration")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetEmailConfiguration(int ApplicationId)
        {
            try
            {
                return Ok(await _commonRepo.GetEmailConfiguration(ApplicationId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #region Update Entity status
        [HttpPost("UpdateEntityStatus")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nadia Gull' Date='2023-01-13' Version="1.0" Branch="master">  Update CaseRequest / CaseFile Status </History>
        public async Task<IActionResult> UpdateEntityStatus(UpdateEntityStatusVM updateEntity)
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
                var result = await _commonRepo.UpdateEntityStatus(updateEntity);
                if (result)
                {
                    _auditLogs.CreateProcessLog(new ProcessLog
                    {
                        Process = "To edit the Case Request",
                        Task = "To update the request",
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
                return BadRequest();
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Edit the Case Request Failed",
                    Body = ex.Message,
                    Category = "User unable to edit the Case Request",
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

        #region GenerateNumberPattern

        [HttpGet("GenerateNumberPattern")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GenerateNumberPattern(int govtEntityId, int pattrenTypeId)
        {
            try
            {
                return Ok(await _cMSCOMSInboxOutboxRequestPatternNumber.GenerateNumberPattern(govtEntityId, pattrenTypeId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion



    }
}
