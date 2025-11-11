using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using FATWA_GENERAL.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_API.Controllers.V1
{
    //<History Author = 'Umer Zaman' Date='2022-03-15' Version="1.0" Branch="own"> create api controller & add functionality</History>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LmsLiteratureTypeController : ControllerBase
    {
        private readonly ILiteratureTypes _iLmsLiteratureTypes;

        public LmsLiteratureTypeController(ILiteratureTypes iLmsLiteratureType)
        {
            _iLmsLiteratureTypes = iLmsLiteratureType;
        }

        [HttpGet("GetLmsLiteratureTypes")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLmsLiteratureTypes()
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

                return Ok(await _iLmsLiteratureTypes.GetLmsLiteratureTypes());
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [HttpGet("GetLmsLiteratureTypesSync")]
        [MapToApiVersion("1.0")]
        public IActionResult GetLmsLiteratureTypesSync()
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
                return Ok(_iLmsLiteratureTypes.GetLmsLiteratureTypesSync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateLmsLiteratureType")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CreateLmsLiteratureType(LmsLiteratureType lmsLiteratureType)
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
                await _iLmsLiteratureTypes.CreateLmsLiteratureType(lmsLiteratureType);

                return Ok(lmsLiteratureType);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });

            }
        }

        [HttpGet("GetLmsLiteratureTypeById")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLiteratureTypeDetails(int id)
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

                LmsLiteratureType lit = await _iLmsLiteratureTypes.GetLiteratureTypeDetails((int)id);
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

        [HttpPost("UpdateLmsLiteratureType")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateLmsLiteratureType(LmsLiteratureType lmsLiteratureType)
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
                await _iLmsLiteratureTypes.UpdateLmsLiteratureType(lmsLiteratureType);
                return Ok(lmsLiteratureType);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });

            }
        }

        [HttpDelete("{id}")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteLmsLiteratureType(int id)
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
                return Ok(await _iLmsLiteratureTypes.DeleteLmsLiteratureType(id));

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
