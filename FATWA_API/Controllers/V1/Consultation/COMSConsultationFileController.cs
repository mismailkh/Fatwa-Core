using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.Consultation;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_API.Controllers.V1.Consultation
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class COMSConsultationFileController : ControllerBase
    {
        private readonly ICOMSConsultationFile _ICOMSConsultationFile;

        private readonly IConfiguration _configuration;
        private readonly IAuditLog _auditLogs;
        public COMSConsultationFileController(ICOMSConsultationFile iCOMSConsultationFile, IConfiguration configuration, IAuditLog auditLogs)
        {
            _ICOMSConsultationFile = iCOMSConsultationFile;
            _configuration = configuration;
            _auditLogs = auditLogs;
        }

        //Author: Hassan Iftikhar
        [HttpPost("GetConsultationFileList")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> GetConsultationFileList(AdvanceSearchConsultationCaseFile advanceSearchVM)
        {
            try
            {
                return Ok(await _ICOMSConsultationFile.GetConsultationFileList(advanceSearchVM));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpGet("GetConsultationFileDetailById")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> GetConsultationFileDetailById(Guid fileId)
        {
            try
            {
                return Ok(await _ICOMSConsultationFile.GetConsultationFileDetailById(fileId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpGet("GetConsultationFile")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> GetConsultationFile(Guid fileId)
        {
            try
            {
                return Ok(await _ICOMSConsultationFile.GetConsultationFile(fileId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpGet("GetConslutationFileStatusHistory")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> GetConslutationFileStatusHistory(Guid fileId)
        {
            try
            {
                return Ok(await _ICOMSConsultationFile.GetConslutationFileStatusHistory(fileId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpGet("GetConsultationAssigneeList")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> GetConsultationAssigneeList(Guid fileId)
        {
            try
            {
                return Ok(await _ICOMSConsultationFile.GetConsultationAssigneeList(fileId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpGet("GetConsultationAssigmentHistory")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> GetConsultationAssigmentHistory(Guid referenceId)
        {
            try
            {
                return Ok(await _ICOMSConsultationFile.GetConsultationAssigmentHistory(referenceId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetConsultationAssigment")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> GetConsultationAssigment(Guid referenceId)
        {
            try
            {
                return Ok(await _ICOMSConsultationFile.GetConsultationAssigment(referenceId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        //[HttpGet("GetConsultationAssigmentHistory")]
        //[MapToApiVersion("1.0")]

        //public async Task<IActionResult> GetConsultationAssigmentHistory(Guid fileId)
        //{
        //    try
        //    {
        //        return Ok(await _ICOMSConsultationFile.GetConsultationFile(fileId));
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
        //    }
        //}
        [HttpGet("GetConslutationFileStatusHistoryForList")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> GetConslutationFileStatusHistoryForList(Guid fileId)
        {
            try
            {
                return Ok(await _ICOMSConsultationFile.GetConslutationFileStatusHistoryForList(fileId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #region Get All Conslutation Case file

        [HttpGet("GetAllConsultationFileListBySectorTypeId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllConsultationFileListBySectorTypeId(int sectorTypeId, string userId)
        {
            try
            {
                return Ok(await _ICOMSConsultationFile.GetAllConsultationFileListBySectorTypeId(sectorTypeId, userId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Drafted Consultation Request List
        [HttpPost("GetDraftedConsultationRequestList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetDraftedConsultationRequestList(AdvanceSearchConsultationRequestVM advanceSearchConsultationRequestVM)
        {
            try
            {
                return Ok(await _ICOMSConsultationFile.GetDraftedConsultationRequestList(advanceSearchConsultationRequestVM));
            }
            catch(Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });

            }
        }
        #endregion
    }
}
