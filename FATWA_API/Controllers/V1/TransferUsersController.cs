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
    //<History Author = 'Umer Zaman' Date='2022-07-26' Version="1.0" Branch="master"> Controller for handling transfer user operations</History>

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TransferUsersController : ControllerBase
    {
        #region Constructor
        public TransferUsersController(ITransferUser iTransferUser)
        {
            _iTransferUser = iTransferUser;
        }
        #endregion

        #region Variable declaration
        private readonly ITransferUser _iTransferUser;
        #endregion

        #region Get All Department List
        [HttpGet("GetAllDepartmentList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAllDepartmentList()
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
                return Ok(await _iTransferUser.GetAllDepartmentList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Save Transfer User
        //<History Author = 'Umer Zaman' Date='2022-07-28' Version="1.0" Branch="master"> Handle save transfer user operation</History>
        [HttpPost("SaveTransferUser")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SaveTransferUser(TransferUser transferUser)
        {
            try
            {
                await _iTransferUser.SaveTransferUser(transferUser);

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

