using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
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
    public class SystemSettingController : ControllerBase
    {
        #region Constructor
        public SystemSettingController(ISystemSetting iSystemSetting)
        {
            _iSystemSetting = iSystemSetting;
        }
        #endregion

        #region Variable declaration
        private readonly ISystemSetting _iSystemSetting;
        #endregion

        #region Get System setting
        [HttpGet("GetSystemSetting")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetSystemSetting()
        {
            try
            {
                return Ok(await _iSystemSetting.GetSystemSetting());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Update system setting
        //<History Author = 'Umer Zaman' Date='2022-08-09' Version="1.0" Branch="master"> Handle Save system configuration operation</History>
        [HttpPost("UpdateSystemSetting")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateSystemSetting(SystemSetting systemsetting)
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
                await _iSystemSetting.UpdateSystemSetting(systemsetting);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
    }
}

