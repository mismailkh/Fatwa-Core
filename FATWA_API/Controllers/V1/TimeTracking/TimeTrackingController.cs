using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FATWA_GENERAL.Helper.Response;
using FATWA_DOMAIN.Interfaces.TimeTracking;
using FATWA_DOMAIN.Models.ViewModel.TimeTrackingVM;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace FATWA_API.Controllers.V1.TimeTracking
{

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TimeTrackingController : ControllerBase
    {
        #region Variables  
        private readonly ITimeTracking _iTimeTracking;
        #endregion

        #region Constractor
        public TimeTrackingController( ITimeTracking iTimeTracking)
        {
           _iTimeTracking = iTimeTracking;
        }
        #endregion

        #region Time Tracking List
        [HttpPost("GetTimeTracking")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetTimeTracking(TimeTrackingAdvanceSearchVM advanceSearchVM)
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
                var result = await _iTimeTracking.GetTimeTracking(advanceSearchVM);

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
    }
}
