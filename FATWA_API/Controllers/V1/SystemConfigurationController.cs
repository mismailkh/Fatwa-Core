using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_GENERAL.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_API.Controllers.V1
{
    //<History Author = 'Umer Zaman' Date='2022-08-09' Version="1.0" Branch="master"> Controller for handling system configuration operations</History>

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class SystemConfigurationController : ControllerBase
    {
        #region Constructor
        public SystemConfigurationController(ISystemConfiguration iSystemConfiguration)
        {
            _iSystemConfiguration = iSystemConfiguration;
        }
        #endregion

        #region Variable declaration
        private readonly ISystemConfiguration _iSystemConfiguration;
        protected int InvalidLoginAttemptsCount = 0;
        protected int WrongPasswordAttemptsCount = 0;
        #endregion

        #region Get system configuration List
        [HttpGet("GetSystemConfigurationDetails")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetSystemConfigurationDetails()
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
                return Ok(await _iSystemConfiguration.GetSystemConfigurationDetails());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get system option List
        [HttpGet("GetSystemOptionDetails")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetSystemOptionDetails()
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
                return Ok(await _iSystemConfiguration.GetSystemOptionDetails());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Login validation
        [HttpGet("GetUserDetailByUsingEmail")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserDetailByUsingEmail(string email)
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
                var res = await _iSystemConfiguration.GetUserDetailByUsingEmail(email);
                if (res == null)
                {
                    return null;
                }
                else
                {
                    return Ok(res);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex?.InnerException.Message });
            }
        }
        [HttpGet("GetUserDetailByUsingEmailForPasswordCheck")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserDetailByUsingEmailForPasswordCheck(string email)
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
                var res = await _iSystemConfiguration.GetUserDetailByUsingEmail(email);
                if (res == null)
                {
                    InvalidLoginAttemptsCount++;
                    return BadRequest();
                }
                else
                {
                    WrongPasswordAttemptsCount++;
                    return Ok(res);
                }
            }
            catch (Exception ex)
            {
                var response = new BadRequestResponse { Message = ex.Message };
                if (ex.InnerException != null)
                {
                    response.InnerException = ex.InnerException.Message;
                }

                return BadRequest(response);
            }
        }
        [HttpGet("GetUserGroupDetailByUsingGroupId")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserGroupDetailByUsingGroupId()
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
                return Ok(await _iSystemConfiguration.GetUserGroupDetailByUsingGroupId());
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region User account lock
        //<History Author = 'Umer Zaman' Date='2022-08-23' Version="1.0" Branch="master"> lock user account after wrong password attempts count completed</History>
        [HttpGet("LockUserAccount")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> LockUserAccount(string email)
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
                await _iSystemConfiguration.LockUserAccount(email);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region User account access fail count manage
        //<History Author = 'Umer Zaman' Date='2022-08-23' Version="1.0" Branch="master"> Fail login access count</History>
        [HttpGet("UserAccountAccessFailCount")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> UserAccountAccessFailCount(string email, int wrongCount)
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
                await _iSystemConfiguration.UserAccountAccessFailCount(email, wrongCount);
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

