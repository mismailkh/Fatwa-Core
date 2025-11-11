using FATWA_DOMAIN.Interfaces.ArchivedCases;
using FATWA_DOMAIN.Models.ViewModel.ArchivedCases;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_API.Controllers.V1.ArchivedCases
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme + ",FatwaApiKey")]
    public class ArchivedCasesController : ControllerBase
    {
        #region Variables Declaration
        private readonly IArchivedCase _IArchivedCase;
        #endregion

        #region Constructor
        public ArchivedCasesController(IArchivedCase archivedCase) 
        {
            _IArchivedCase = archivedCase;
        }
        #endregion

        #region Endpoints

        #region Add Archived Case Data
        //[AllowAnonymous]
        //[ApiKeyAuthorize]
        [HttpPost("AddArchivedCaseData")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddArchivedCaseData([FromBody] AddArchivedCaseDataRequestPayload addArchivedCaseData)
        {
            try
            {
                await _IArchivedCase.AddArchivedCaseData(addArchivedCaseData);
                return Ok(new { Status = "Success", Message = "The case data has been added successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = "Could not add case data due to " + ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Archived Cases List
        [HttpGet("GetArchivedCaseList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetArchivedCaseList(ArchivedCaseAdvanceSearchVM archivedCaseAdvanceSearchVM)
        {
            try
            {
                return Ok(await _IArchivedCase.GetArchivedCaseList(archivedCaseAdvanceSearchVM));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Archived Case Detail By Case Number
        [HttpGet("GetArchivedCaseDetailByCaseId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetArchivedCaseDetailByCaseId(Guid CaseId)
        {
            try
            {
                return Ok(await _IArchivedCase.GetArchivedCaseDetailByCaseId(CaseId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Archived Case Document Detail
        [HttpGet("GetArchivedCaseDocumentDetailsById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetArchivedCaseDocumentDetailById(Guid documentId)
        {
            try
            {
                return Ok(await _IArchivedCase.GetArchivedCaseDocumentDetailById(documentId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #endregion

    }
}
