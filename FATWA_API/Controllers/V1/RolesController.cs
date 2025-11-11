using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_GENERAL.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Net;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Permissions;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_API.Controllers.V1
{
    //<History Author = 'Hassan Abbas' Date='2022-07-20' Version="1.0" Branch="master"> Controller for handling Role and Claims operations</History>

    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RolesController : ControllerBase
    {
        private readonly IRole _IRole;
        private readonly IAuditLog _auditLogs;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;
        public RolesController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IRole iRole, IAuditLog audit)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _IRole = iRole;
            _auditLogs = audit;
        }

        #region Get Roles
        //<History Author = 'Hassan Abbas' Date='2022-07-20' Version="1.0" Branch="master"> Return role details by id</History>
        [HttpGet("GetRoleById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetRoleById(string roleId)
        {
            try
            {
                return Ok(await _IRole.GetRoleById(roleId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Get Roles Details
        //<History Author = 'Umer Zaman' Date='2022-07-21' Version="1.0" Branch="master"> Get complete list of roles</History>

        [HttpGet("GetRoleDetails")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetRoleDetails()
        {
            try
            {
                var result = await _IRole.GetRoleDetails();

                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Get Role List",
                    Task = "Get Role List",
                    Description = "User able to Get Role List successfully.",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Get Role List executed Successfully",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return Ok(result);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Get Role List Failed",
                    Body = ex.Message,
                    Category = "User unable to Get Role List",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Get Role List Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                return BadRequest(ex.Message);
            }
        }
        #endregion

        [HttpGet("UserRoleList")]
        [AllowAnonymous]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UserRoleList()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            return Ok(roles);
        }

        [HttpPost("AddRole")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (roleName != null)
            {
                await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));
            }
            return RedirectToAction("Index");
        }

        #region Create Update Role
        //<History Author = 'Hassan Abbas' Date='2022-07-20' Version="1.0" Branch="master"> Handle create role operation</History>
        [HttpPost("CreateRole")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CreateRole(Role role)
        {
            try
            {
                await _IRole.CreateRole(role);

                //_audit.AddProcessLog(
                //   "Create Role",
                //   "Create Role",
                //   "Created Role Successfully",
                //   Environment.MachineName.ToString(),
                //   "Create Role Successfully",
                //   (int)ProcessLogEnum.Processed,
                //   (int)ProcessLogEnum.Processed);
                return Ok();
            }
            catch (Exception ex)
            {
                //_audit.AddErrorLog(
                //   (int)ErrorLogEnum.Error,
                //   "Create Role Failed",
                //   ex.Message,
                //   "User unable to Create Role",
                //   ex.Source,
                //   ex.GetType().Name,
                //   Environment.MachineName.ToString(),
                //   ex.StackTrace);

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-07-20' Version="1.0" Branch="master"> Handle update role operation</History>
        [HttpPost("UpdateRole")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateRole(Role role)
        {
            try
            {
                await _IRole.UpdateRole(role);

                //_audit.AddProcessLog(
                //   "Update Role",
                //   "Update Role",
                //   "Updated Role Successfully",
                //   Environment.MachineName.ToString(),
                //   "CrUpdateeate Role Successfully",
                //   (int)ProcessLogEnum.Processed,
                //   (int)ProcessLogEnum.Processed);
                return Ok();
            }
            catch (Exception ex)
            {
                //_audit.AddErrorLog(
                //   (int)ErrorLogEnum.Error,
                //   "Update Role Failed",
                //   ex.Message,
                //   "User unable to Create Role",
                //   ex.Source,
                //   ex.GetType().Name,
                //   Environment.MachineName.ToString(),
                //   ex.StackTrace);

                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion

        #region Role DDL

        [HttpGet("GetRoleData")]
        [MapToApiVersion("1.0")]

        public async Task<IActionResult> GetRoleData()
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
                var result = await _IRole.GetRoleData();
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

        #region Role Claims


        [HttpGet("GetRoleClaims")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-07-05' Version="1.0" Branch="master"> returns the list of Claims by userId</History>
        public async Task<IActionResult> GetRoleClaims([FromForm] string userId)
        {
            try
            {
                return Ok(await _IRole.GetRoleClaims(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetAllClaims")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-07-20' Version="1.0" Branch="master"> returns the list of all Claims</History>
        public async Task<IActionResult> GetAllClaims(string? roleId)
        {
            try
            {
                return Ok(await _IRole.GetAllClaims(roleId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Delete Role (Soft delete status change)
        //<History Author = 'Umer Zaman' Date='2022-08-01' Version="1.0" Branch="master"> Soft delete user role</History>

        //[HttpPost("DeleteRole")]
        [HttpDelete("{item}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> DeleteRole(Role item)
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
                await _IRole.DeleteRole(item);
                //_audit.AddProcessLog(
                //"Delete Role Process",
                //"Delete Role",
                //"Delete Role Successfully",
                //Environment.MachineName.ToString(),
                //"Delete Role Managed",
                //(int)ProcessLogEnum.Processed,
                //(int)ProcessLogEnum.Processed);
                return Ok();
            }
            catch (Exception ex)
            {
                //_audit.AddErrorLog(
                //   (int)ErrorLogEnum.Error,
                //   "Delete Role Failed",
                //   ex.Message,
                //   "User unable to Delete Role",
                //   ex.Source,
                //   ex.GetType().Name,
                //   Environment.MachineName.ToString(),
                //   ex.StackTrace);
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get Sector HOS
        //<History Author = 'Hassan Abbas' Date='2022-12-21' Version="1.0" Branch="master"> Get HOS By Sector</History>
        [HttpGet("GetHOSBySectorId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetHOSBySectorId(int sectorTypeId)
        {
            try
            {
                var result = await _IRole.GetHOSBySectorId(sectorTypeId);

                return result == null ? BadRequest(result) : Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion

    }
}

