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
    //<!-- <History Author = 'Umer Zaman' Date='2022-05-26' Version="1.0" Branch="master">Create class to manage api controller</History>

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UmsGroupsController : ControllerBase
    {
        private readonly IUmsGroup _iumsGroup;

        public UmsGroupsController(IUmsGroup iUmsGroup)
        {
            _iumsGroup = iUmsGroup;
        }
        #region Get, Details & By Id


        #endregion

        #region Create

        [HttpPost("CreateUmsGroups")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CreateUmsGroups(Group usergroup)
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
                await _iumsGroup.CreateUmsGroup(usergroup);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region get User Group by Details
        [HttpGet("GetUserGroupDetailById")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetUserGroupDetailById(Guid GroupId)
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
                Group result = await _iumsGroup.GetUserGroupDetailById(GroupId);
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

        #endregion

        #region Update UserGroup
        [HttpPost("UpdateUmsGroup")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateUmsGroup(Group UserGroup)
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
                await _iumsGroup.UpdateUMSUsersGroup(UserGroup);

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Get All Claims 
        [HttpGet("GetAllClaims")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-07-20' Version="1.0" Branch="master"> returns the list of all Claims</History>
        public async Task<IActionResult> GetAllClaims(string? groupId)
        {
            try
            {
                return Ok(await _iumsGroup.GetAllClaims(groupId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

    }
}
