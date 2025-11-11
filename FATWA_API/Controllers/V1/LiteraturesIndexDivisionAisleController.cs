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
    //<!-- <History Author = 'Umer Zaman' Date='2022-07-08' Version="1.0" Branch="master">Create class for manage api controller</History>

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class LmsLiteratureIndexDivisionAisleController : ControllerBase
    {
        private readonly ILmsLiteratureIndexDivisionAisle _iLmsLiteratureIndexDivisionAisle;

        public LmsLiteratureIndexDivisionAisleController(ILmsLiteratureIndexDivisionAisle iLmsLiteratureIndex)
        {
            _iLmsLiteratureIndexDivisionAisle = iLmsLiteratureIndex;
        }
        #region Get Lms Literature Division Index's Details / By Id

        [HttpGet("GetLmsLiteratureIndexDivisions")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetLmsLiteratureIndexDivisions()
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
                return Ok(await _iLmsLiteratureIndexDivisionAisle.GetLmsLiteratureIndexDivisions());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetLmsLiteratureIndexDivisionsSync")]
        [MapToApiVersion("1.0")]
        public IActionResult GetLmsLiteratureIndexDivisionsSync()
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
                return Ok(_iLmsLiteratureIndexDivisionAisle.GetLmsLiteratureIndexDivisionsSync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetLiteratureIndexDivisionDetail")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLiteratureIndexDivisionDetail(int divisionAisleId)
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

                LmsLiteratureIndexDivisionAisle? result = await _iLmsLiteratureIndexDivisionAisle.GetLiteratureIndexDivisionDetail(divisionAisleId);
                if (result != null)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetLmsLiteratureDivisionDetailsByUsingIndexId")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLmsLiteratureDivisionDetailsByUsingIndexId(int indexId)
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
                return Ok(await _iLmsLiteratureIndexDivisionAisle.GetLmsLiteratureDivisionDetailsByUsingIndexId(indexId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetLmsLiteratureDivisionDetailsByUsingIndexIdForViewPage")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLmsLiteratureDivisionDetailsByUsingIndexIdForViewPage(int indexId)
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
                return Ok(await _iLmsLiteratureIndexDivisionAisle.GetLmsLiteratureDivisionDetailsByUsingIndexIdForViewPage(indexId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetDivisionDetailsByUsingIndexAndDivisionId")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDivisionDetailsByUsingIndexAndDivisionId(int divisionAisleId, int indexId)
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
                return Ok(await _iLmsLiteratureIndexDivisionAisle.GetDivisionDetailsByUsingIndexAndDivisionId(divisionAisleId, indexId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetLmsLiteratureDivisionDetailByUsingDivisionId")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLmsLiteratureDivisionDetailByUsingDivisionId(int divisionAisleId)
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
                return Ok(await _iLmsLiteratureIndexDivisionAisle.GetLmsLiteratureDivisionDetailByUsingDivisionId(divisionAisleId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetLmsLiteratureAisleNumberDetailsByUsingIndexAndDivisionNumber")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetLmsLiteratureAisleNumberDetailsByUsingIndexAndDivisionNumber(int indexId, string divisionNumber)
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
                return Ok(await _iLmsLiteratureIndexDivisionAisle.GetLmsLiteratureAisleNumberDetailsByUsingIndexAndDivisionNumber(indexId, divisionNumber));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Create

        [HttpPost("CreateLmsLiteratureIndexDivision")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CreateLmsLiteratureIndexDivision(LmsLiteratureIndexDivisionAisle lmsLiteratureIndexDivisionAisle)
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
                await _iLmsLiteratureIndexDivisionAisle.CreateLmsLiteratureIndexDivision(lmsLiteratureIndexDivisionAisle);
                return Ok(lmsLiteratureIndexDivisionAisle);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Update

        [HttpPost("UpdateLmsLiteratureIndexDivision")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateLmsLiteratureIndexDivision(LmsLiteratureIndexDivisionAisle lmsLiteratureIndexDivision)
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
                await _iLmsLiteratureIndexDivisionAisle.UpdateLmsLiteratureIndexDivision(lmsLiteratureIndexDivision);
                
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Delete

        [HttpDelete("{divisionAisleId}")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteLmsLiteratureIndexDivisionAisle(int divisionAisleId)
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
                await _iLmsLiteratureIndexDivisionAisle.DeleteLmsLiteratureIndexDivisionAisle(divisionAisleId);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        //USE THIS LATER
        //public async Task<IActionResult> DeleteLmsLiteratureIndex(int id)
        //{  
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(new RequestFailedResponse
        //            {
        //                Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
        //            });
        //        }
        //        _logging.AddProcessLog(
        //           "Delete Literature Process",
        //           "Delete Literature",
        //           "Delete Literature Successfully",
        //           Environment.MachineName.ToString(),
        //           "Delete Literature Managed",
        //           (int)ProcessLogEnum.Processed,
        //           (int)ProcessLogEnum.Processed);

        //        _iLmsLiteratureIndexDivisionAisle.DeleteLmsLiteratureIndex(id);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        _logging.AddErrorLog(
        //           (int)ErrorLogEnum.Error,
        //           "Delete Literature Failed",
        //           ex.Message,
        //           "User unable to Delete Literature",
        //           ex.Source,
        //           ex.GetType().Name,
        //           Environment.MachineName.ToString(),
        //           ex.StackTrace);

        //        return BadRequest(ex.Message);
        //    }
        //}

        #endregion


        #region Check Division Id Associate with Literatuer

        //[HttpGet("CheckLmsLiteratureDivisionAisleIdAssociatedWithLiterature")]
        //[MapToApiVersion("1.0")]
        //[AllowAnonymous]
        //public IActionResult CheckLmsLiteratureDivisionAisleIdAssociatedWithLiterature(int divisionAisleId)
        //{
        //    try
        //    {
        //        if (!ModelState.IsValid)
        //        {
        //            return BadRequest(new RequestFailedResponse
        //            {
        //                Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
        //            });
        //        }
        //        return Ok(_iLmsLiteratureIndexDivisionAisle.CheckLmsLiteratureDivisionAisleIdAssociatedWithLiterature((int)divisionAisleId));
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
        #endregion
    }
}
