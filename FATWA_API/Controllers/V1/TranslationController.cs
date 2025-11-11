using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Net;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //<History Author = 'Aqeel Altaf' Date='2022-03-23' Version="1.0" Branch="master"> API Controller for handling request/responses</History>
    public class TranslationController : ControllerBase
    {
        private readonly ITranslation _ITranslation;
        public TranslationController(ITranslation iTranslation)
        {
            _ITranslation = iTranslation;
        }

        [HttpGet("GetTranslation")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Aqeel Altaf' Date='2022-03-23' Version="1.0" Branch="master"> Return List of Literature Borrow Details</History>
        public async Task<IActionResult> GetTranslation()
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
                return Ok(await _ITranslation.GetTranslations());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message });
            }
        }
        [HttpGet("GetTranslationForMobileApp")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        //<History Author = 'Noman Khan' Date='2024-05-13' Version="1.0" Branch="master"> Return List of Literature Borrow Details</History>
        public async Task<IActionResult> GetTranslationForMobileApp([Required] string cultureValue, [Required] int channelId, [Required] string versionCode)
        {
            try
            {
                HttpResponseMessage response = null;
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }
                var data = await _ITranslation.GetTranslationsByCultureValue(cultureValue, channelId, versionCode);
                if (data != null)
                {
                    return Ok(new ApiCallResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        IsSuccessStatusCode = true,
                        ResultData = data,
                        Message = "success"
                    });
                }
                else
                {
                    return NotFound(new ApiCallResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccessStatusCode = false,
                        ResultData = data,
                        Message = "No_record_found"
                    });
                }
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
        [HttpGet("GetTranslationSync")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Aqeel Altaf' Date='2022-03-23' Version="1.0" Branch="master"> Return List of Literature Borrow Details Sync Call</History>
        public IActionResult GetTranslationSync()
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
                return Ok(_ITranslation.GetTranslationsSync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("UpdateTranslation")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Aqeel Altaf' Date='2022-03-23' Version="1.0" Branch="master"> create Literature Borrow Details</History>
        public async Task<IActionResult> UpdateTranslation(Translation Translation)
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
                await _ITranslation.UpdateTranslation(Translation);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [MapToApiVersion("1.0")] 
        //<History Author = 'Aqeel Altaf' Date='2022-03-23' Version="1.0" Branch="master"> Delete Literature Classifcation</History>
        public async Task<IActionResult> DeleteTranslation(int id)
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
                await _ITranslation.DeleteTranslation(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message });
            }
        }

    }
}
