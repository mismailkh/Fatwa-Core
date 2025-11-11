using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.ViewModel;
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
    //<History Author = 'Muhammad Zaeem' Date='2022-09-29' Version="1.0" Branch="master"> API Controller for handling request/responses</History>
    //<History Author = 'Muhammad Zaeem' Date='2022-09-29' Version="1.0" Branch="master"> API Controller for handling request/responses</History>
    public class UmsClaimsController : ControllerBase
    {
        private readonly IClaims _IClaims;
       
        public UmsClaimsController(IClaims iClaims)
        {
            _IClaims = iClaims;
        }

        #region Get Ums Claims

        [HttpGet("GetClaimUms")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2022-09-29' Version="1.0" Branch="master"> Return List of UMS CLAIMS</History>
        public async Task<IActionResult> GetClaimUms()
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
                return Ok(await _IClaims.GetClaimUms());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetClaimUmsSync")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        //<History Author = 'Muhammad Zaeem' Date='2022-09-29' Version="1.0" Branch="master"> Return List of UMS CLAIMS sync call</History>
        public List<ClaimUms> GetClaimUmsSync()
        {
            return _IClaims.GetClaimUmsSync();
        }

        #endregion

        #region get claims by ID
        [HttpGet("GetClaimsById")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        //<History Author = 'Aqeel Altaf' Date='2022-03-23' Version="1.0" Branch="master"> Get Literature Classifcation on Id</History>
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
                ClaimUms lit = await _IClaims.GetClaimsById((int)id);
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


        #region Create Claims
        [HttpPost("CreateClaims")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Aqeel Altaf' Date='2022-03-23' Version="1.0" Branch="master"> create Literature Borrow Details</History>
        public async Task<IActionResult> CreateClaims(ClaimUms ClaimUms)
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
                await _IClaims.CreateClaims(ClaimUms);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }


        #endregion

        #region Update Claims
        [HttpPost("UpdateClaims")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Aqeel Altaf' Date='2022-03-23' Version="1.0" Branch="master"> Update Literature Classifcation</History>
        public async Task<IActionResult> UpdateClaims(ClaimUms ClaimUms)
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
                await _IClaims.UpdateClaims(ClaimUms);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion


        #region Delete Claims(hard delete)
        [HttpDelete("{id}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> DeleteClaims(int id)
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
                await _IClaims.DeleteClaims(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion


        //#region Delete claims (Soft delete status change)
        ////<History Author = 'Muhammad Zaeem' Date='2022-08-01' Version="1.0" Branch="master"> Soft delete user role</History>


        //[HttpDelete("{item}")]
        //[MapToApiVersion("1.0")]
        //public async Task<IActionResult> DeleteClaims(ClaimUms item)
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
        //        await _IClaims.DeleteClaims(item);
        //        //_audit.AddProcessLog(
        //        //"Delete Role Process",
        //        //"Delete Role",
        //        //"Delete Role Successfully",
        //        //Environment.MachineName.ToString(),
        //        //"Delete Role Managed",
        //        //(int)ProcessLogEnum.Processed,
        //        //(int)ProcessLogEnum.Processed);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        //_audit.AddErrorLog(
        //        //   (int)ErrorLogEnum.Error,
        //        //   "Delete Role Failed",
        //        //   ex.Message,
        //        //   "User unable to Delete Role",
        //        //   ex.Source,
        //        //   ex.GetType().Name,
        //        //   Environment.MachineName.ToString(),
        //        //   ex.StackTrace);
        //        return BadRequest(ex.Message);
        //    }
        //}
        //#endregion

    }
}
