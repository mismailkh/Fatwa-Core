using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_GENERAL.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Rewrite;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_API.Controllers.V1
{
    //<History Author = 'Umer Zaman' Date='2022-07-22' Version="1.0" Branch="master"> Controller to handling Groups</History>

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class GroupController : ControllerBase
    {
        #region Variables declaration
        private readonly IUmsGroup _iUmsGroup;
        #endregion

        #region Constructor
        public GroupController(IUmsGroup iUmsGroup)
        {
            _iUmsGroup = iUmsGroup;
        }
        #endregion

        #region Get Group Details
        //<History Author = 'Umer Zaman' Date='2022-07-22' Version="1.0" Branch="master"> Get complete list of groups</History>
        [HttpPost("GetGroupDetails")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetGroupDetails(UserListAdvanceSearchVM advanceSearchVM)
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
                return Ok(await _iUmsGroup.GetGroupDetails(advanceSearchVM));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion 

        #region Websystems Functions
        //<History Author = 'Umer Zaman' Date='2022-07-22' Version="1.0" Branch="master"> Get complete list of groups</History>
        [HttpGet("GetWebSystems")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetWebSystems()
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
                return Ok(await _iUmsGroup.GetWebSystems());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetWebSystemsById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetWebSystemsById(int Id)
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
                return Ok(await _iUmsGroup.GetWebSystemsById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("SaveWebSystems")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SaveWebSystems(WebSystem WebSystem)
        {
            try
            {
                await _iUmsGroup.SaveWebSystems(WebSystem);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpPost("UpdateWebSystems")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateWebSystems(WebSystem webSystem)
        {
            try
            {
                await _iUmsGroup.UpdateWebSystems(webSystem);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }



        #endregion

        #region Group AccessType

        [HttpGet("GetGroupTypeById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetGroupTypeById(int Id)
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
                return Ok(await _iUmsGroup.GetGroupTypeById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetGroupAccessTypes")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetGroupAccessTypes()
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
                return Ok(await _iUmsGroup.GetGroupAccessTypes());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetGroups")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetGroups()
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
                return Ok(await _iUmsGroup.GetGroups());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("CreateGroupAccessType")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CreateGroupAccessType(GroupAccessTypeVM groupAccessTypeVM)
        {
            try
            {
                await _iUmsGroup.CreateGroupAccessType(groupAccessTypeVM);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpPost("UpdateGroupAccessType")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateGroupAccessType(GroupAccessTypeVM groupAccessTypeVM)
        {
            try
            {
                await _iUmsGroup.UpdateGroupAccessType(groupAccessTypeVM);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

        #region Delete selected groups

        [HttpPost("DeleteSelectedUserGroup")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> DeleteSelectedUserGroup(IList<Group> data)
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
                await _iUmsGroup.DeleteSelectedUserGroup(data);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Group Claims
        [HttpGet("GetGroupClaims")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nadia Gull' Date='2023-05-03' Version="1.0" Branch="master"> Returns the list of Group Claims by userId</History>
        public async Task<IActionResult> GetGroupClaims([FromForm] string userId)
        {
            try
            {
                return Ok(await _iUmsGroup.GetGroupClaims(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Assign Groups To User
        [HttpGet("AssignGroupsToUser")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nadia Gull' Date='2023-05-03' Version="1.0" Branch="master"> Returns the list of Group Claims by userId</History>
        public async Task<IActionResult> AssignGroupsToUser()
        {
            try
            {
                await _iUmsGroup.AssignGroupsToUser();
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
        
        [HttpGet("GetGroupsByGroupTypeId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetGroupsByGroupTypeId(int GroupTypeId)
        {
            try
            {
                var groups = await _iUmsGroup.GetGroupsByGroupTypeId(GroupTypeId);
                return Ok(groups);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }


    }
}

