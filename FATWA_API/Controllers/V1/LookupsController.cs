using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models.CaseManagment;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FATWA_GENERAL.Helper.Response;
 
namespace FATWA_API.Controllers.V1
{
    //<History Author = 'Hassan Abbas' Date='2022-09-29' Version="1.0" Branch="master"> Repository for handling Get Lookup Calls from G2G Portal</History>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LookupsController : ControllerBase
    {
        private readonly ILookups _ILookups;
        private readonly IAuditLog _auditLogs;

        public LookupsController(ILookups iLookups, IAuditLog audit)
        {
            _ILookups = iLookups;
            _auditLogs = audit;
        }

        [HttpGet("GetGovernmentEntities")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Get goverment entities</History>
        public async Task<IActionResult> GetGovernmentEntities()
        {
            try
            {
                return Ok(await _ILookups.GetGovernmentEntities());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetOperatingSectorTypes")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Get goverment entities</History>
        public async Task<IActionResult> GetOperatingSectorTypes()
        {

            try
            {
                return Ok(await _ILookups.GetOperatingSectorTypes());

            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }


        }
        [HttpGet("GetAllSectorSubtypes")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Get all sector subtypes</History>
        public async Task<IActionResult> GetAllSectorSubtypes()
        {
            try
            {
                return Ok(await _ILookups.GetAllSectorSubtypes());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpGet("GetSectorSubtypesBySector")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Get sector subtypes</History>
        public async Task<IActionResult> GetSectorSubtypesBySector(int sectorType)
        {
            try
            {
                return Ok(await _ILookups.GetSectorSubtypesBySector(sectorType));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetCaseRequestStatuses")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Get case request statuses</History>
        public async Task<IActionResult> GetCaseRequestStatuses()
        {
            try
            {
                return Ok(await _ILookups.GetCaseRequestStatuses());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetCaseFileStatuses")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Get case request statuses</History>
        public async Task<IActionResult> GetCaseFileStatuses()
        {
            try
            {
                return Ok(await _ILookups.GetCaseFileStatuses());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }
        [HttpGet("GetMeetingTypes")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetMeetingTypes()
        {
            try
            {
                return Ok(await _ILookups.GetMeetingTypes());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }

        }

        [HttpGet("GetCasePriorities")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Get case priorities</History>
        public async Task<IActionResult> GetCasePriorities()
        {
            try
            {
                return Ok(await _ILookups.GetCasePriorities());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpGet("GetMinistries")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Get Ministries</History>
        public async Task<IActionResult> GetMinistries()
        {
            try
            {
                return Ok(await _ILookups.GetMinistries());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpGet("GetDepartments")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetDepartments()
        {
            try
            {
                return Ok(await _ILookups.GetDepartments());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
         
        #region  zaeem  controllers
         
        #region Get court type
        [HttpGet("GetCourtType")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2022-11-11' Version="1.0" Branch="master">Get Chamber</History>
        public async Task<IActionResult> GetCourtType()
        {
            try
            {
                return Ok(await _ILookups.GetCourtTypes());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region get Court
        [HttpGet("GetCourt")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2022-11-11' Version="1.0" Branch="master">Get court</History>
        public async Task<IActionResult> GetCourt()
        {
            try
            {
                return Ok(await _ILookups.GetCourts());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Get Chamber
        [HttpGet("GetChamber")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2022-11-11' Version="1.0" Branch="master">Get Chamber</History>
        public async Task<IActionResult> GetChamber()
        {
            try
            {
                return Ok(await _ILookups.GetChambers());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Submit Lawyer To Court Assignment

        [HttpPost("SubmitLawyerToCourt")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2022-11-11' Version="1.0" Branch="master"> Submit Lawyer To Court Assignment</History>
        public async Task<IActionResult> SubmitLawyerToCourt(CmsAssignLawyerToCourt cmsAssignLawyerToCourt)
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
                await _ILookups.SaveAssignLawyerToCourt(cmsAssignLawyerToCourt);


                return Ok(cmsAssignLawyerToCourt);
            }
            catch (Exception ex)
            {

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        [HttpGet("GetResponseTypes")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetResponseTypes()
        {
            try
            {
                return Ok(await _ILookups.GetResponseTypes());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetCaseTemplates")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Get case priorities</History>
        public async Task<IActionResult> GetCaseTemplates(int attachmentTypeId)
        {
            try
            {
                return Ok(await _ILookups.GetCaseTemplates(attachmentTypeId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
    } 
}



