using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    //<History Author = 'Hassan Abbas' Date='2022-03-16' Version="1.0" Branch="master"> API Controller for handling request/responses</History>
    public class LmsLiteratureClassificationsController : ControllerBase
    {
        private readonly ILmsLiteratureClassification _ILmsLiteratureClassifications;
        public LmsLiteratureClassificationsController(ILmsLiteratureClassification iLmsLiteratureClassifications)
        {
            _ILmsLiteratureClassifications = iLmsLiteratureClassifications;
        }

        #region CRUD Functions

        //<History Author = 'Zain Ul Islam' Date='2022-07-21' Version="1.0" Branch="master">New Generic Implementation for GET API</History>  
        [HttpGet("GetLmsLiteratureClassifications")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-03-16' Version="1.0" Branch="master"> Return List of Literature Classifcations</History>
        public async Task<IActionResult> GetLmsLiteratureClassifications()
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
                return Ok(await _ILmsLiteratureClassifications.GetLmsLiteratureClassifications());
                //var response = await _ILmsLiteratureClassifications.GetLmsLiteratureClassifications();
                //return Ok(new ApiResponse<LmsLiteratureClassification>
                //{
                //    Count = response.Count(),
                //    Value = response
                //});
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetLiteratureClassificationDetailById")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        //<History Author = 'Hassan Abbas' Date='2022-03-16' Version="1.0" Branch="master"> Get Literature Classifcation on Id</History>
        public async Task<IActionResult> Get(int id)
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
                LmsLiteratureClassification lit = await _ILmsLiteratureClassifications.GetLiteratureClassificationDetailById((int)id);
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

        //<History Author = 'Hassan Abbas' Date='2022-03-16' Version="1.0" Branch="master"> create Literature Classifcations</History> 
        [HttpPost("CreateLmsLiteratureClassification")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CreateLmsLiteratureClassification(LmsLiteratureClassification literatureClassification)
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
                Task? result = _ILmsLiteratureClassifications.CreateLmsLiteratureClassification(literatureClassification);
                if (!result.IsCompletedSuccessfully)
                    return BadRequest();
                return Ok(literatureClassification);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });

            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-03-16' Version="1.0" Branch="master"> Update Literature Classifcation</History> 
        [HttpPost("UpdateLmsLiteratureClassification")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateLmsLiteratureClassification(LmsLiteratureClassification entity)
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
                Task? result = _ILmsLiteratureClassifications.UpdateLmsLiteratureClassification(entity);
                if (!result.IsCompletedSuccessfully)
                    return BadRequest();
                return Ok(entity);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-03-16' Version="1.0" Branch="master"> Delete Literature Classifcation</History> 
        [HttpDelete("{id}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Delete(int id)
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

                return Ok(await _ILmsLiteratureClassifications.DeleteLmsLiteratureClassification(id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        //<History Author = 'Hassan Abbas' Date='2022-03-16' Version="1.0" Branch="master"> Return List of Literature Classifcations Sync Call</History> 
        [HttpGet("GetLmsLiteratureClassificationsSync")]
        [MapToApiVersion("1.0")]
        public IActionResult GetLmsLiteratureClassificationsSync()
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
                return Ok(_ILmsLiteratureClassifications.GetLmsLiteratureClassificationsSync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
