using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.Notification;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.BugReporting;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.BugReportingVMs;
using FATWA_DOMAIN.Models.WorkflowModels;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;
using static SkiaSharp.HarfBuzz.SKShaper;

namespace FATWA_API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsersController : ControllerBase
    {
        private readonly IUsers _IUsers;
        private readonly IAuditLog _auditLogs;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly INotification _iNotifications;
        private readonly IAccount _IAccount;

        public UsersController(IUsers iUsers, UserManager<IdentityUser> userManager, IAuditLog auditLogs, INotification iNotifications, IAccount iAccount)
        {
            _userManager = userManager;
            _IUsers = iUsers;
            _auditLogs = auditLogs;
            _iNotifications = iNotifications;
            _IAccount = iAccount;
        }

        [HttpGet("GetTransferUsers")]
        [MapToApiVersion("1.0")]
        public async Task<List<UserTransferVM>> GetTransferUsers(string userId)
        {
            return await _IUsers.GetUmsUserTransfer(userId);
        }

        #region GetUsers

        [HttpGet("GetUsersList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetUsersList()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var allUsersExceptCurrentUser = await _userManager.Users.Where(a => a.Id != currentUser.Id).ToListAsync();
            return Ok(allUsersExceptCurrentUser);
        }


        [HttpGet("GetUmsUser")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetUmsUser(string GroupId, bool IsView)
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
                return Ok(await _IUsers.GetUmsUser(GroupId, IsView));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Get user detail by id (for Detail View)
        // <History Author = 'Umer Zaman' Date='2022-07-28' Version="1.0" Branch="master">Get user details for detail view page</History>
        [HttpGet("GetUserDetailsById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetUserDetailsById(string userId)
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
                return Ok(await _IUsers.GetUserDetailsById(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        #endregion

        #region UMS Functions

        [HttpGet("GetNationality")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetNationality()
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
                return Ok(await _IUsers.GetNationality());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("CheckCivilIdExists")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CheckCivilIdExists(string civilId)
        {
            try
            {
                bool result = await _IUsers.CheckCivilIdExists(civilId);
                if (result != false)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetGenders")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetGenders()
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
                return Ok(await _IUsers.GetGenders());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetUserAdress")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetUserAdress()
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
                return Ok(await _IUsers.GetUserAdress());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetCities")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCities()
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
                return Ok(await _IUsers.GetCities());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetCountries")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCountries()
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
                return Ok(await _IUsers.GetCountries());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetContactTypes")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetContactTypes()
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
                return Ok(await _IUsers.GetContactTypes());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetDesignations")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetDesignations()
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
                return Ok(await _IUsers.GetDesignations());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetEmployeeType")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetEmployeeType()
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
                return Ok(await _IUsers.GetEmployeeType());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetCompanies")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCompanies()
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
                return Ok(await _IUsers.GetCompanies());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetEmployeeDepartment")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetEmployeeDepartment()
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
                return Ok(await _IUsers.GetEmployeeDepartment());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetEmployeeSectortype")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetEmployeeSectortype()
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
                return Ok(await _IUsers.GetEmployeeSectortype());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetEmployeeGrade")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetEmployeeGrade()
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
                return Ok(await _IUsers.GetEmployeeGrade());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetGradeTypes")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetGradeTypes()
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
                return Ok(await _IUsers.GetGradeTypes());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetContractTypes")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetContractTypes()
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
                return Ok(await _IUsers.GetContractTypes());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetWorkingTime")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetWorkingTime()
        {
            try
            {
                return Ok(await _IUsers.GetWorkingTime());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetEmployeeStatus")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetEmployeeStatus()
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
                return Ok(await _IUsers.GetEmployeeStatus());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetEmployeesByRoleSectorAndDesignation")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetEmployeesByRoleSectorAndDesignation(int SectorTypeId, string RoleId, int DesignationId)
        {
            try
            {
                return Ok(await _IUsers.GetEmployeesByRoleSectorAndDesignation(SectorTypeId, RoleId, DesignationId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetGovernorates")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetGovernorates()
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
                return Ok(await _IUsers.GetGovernorates());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet("GetEmployeeDetailById")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetEmployeeDetailById(Guid Id)
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
                return Ok(await _IUsers.GetEmployeeDetailById(Id));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("EditEmployee")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> EditEmployee(AddEmployeeVM updatedEmployee)
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
                await _IUsers.EditEmployee(updatedEmployee);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPost("AddEmployee")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddEmployee(AddEmployeeVM user)
        {
            try
            {
                var response = await _IUsers.AddEmployee(user);
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Create New Employee",
                    Task = "Create New Employee",
                    Description = "User Created successfully .  ",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "New Employee Added Successfully",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.UMS,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });

                List<string> NotificationReceivers = new List<string>();
                var fatwaAdminUsers = await _IAccount.GetUsersByRoleId(SystemRoles.FatwaAdmin);
                NotificationReceivers.AddRange(fatwaAdminUsers);

                foreach (var receiverId in NotificationReceivers)
                {
                    var notificationResult = await _iNotifications.SendNotification(new Notification
                    {
                        NotificationId = Guid.NewGuid(),
                        DueDate = DateTime.Now.AddDays(5),
                        CreatedBy = user.CreatedBy,
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        ReceiverId = receiverId,
                        ModuleId = (int)WorkflowModuleEnum.UMS,
                    },
                    (int)NotificationEventEnum.AddingNewEmployee,
                    "list",
                    "employees",
                    null,
                    user.NotificationParameters);
                }

                return Ok(response);
            }
            catch (Exception ex)
            {
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Create New Employee Failed",
                    Body = ex.Message,
                    Category = "User unable to Create New Employee",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Creating New User Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.UMS,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpPost("AddBulkEmployees")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AddBulkEmployees(List<ImportEmployeeTemplate> employees, bool cultureEn)
        {
            try
            {
                var result = await _IUsers.AddBulkEmployees(employees, cultureEn);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }


        #endregion

        [HttpPost("GetEmployeeList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetEmployeeList(UserListAdvanceSearchVM advanceSearchVM)
        {
            try
            {
                return Ok(await _IUsers.GetEmployeeList(advanceSearchVM));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("GetEmployeeListBySectorTypeId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetEmployeeList(int sectorTypeId, int attachementId, int documentId)
        {
            try
            {
                return Ok(await _IUsers.GetEmployeeList(sectorTypeId, attachementId, documentId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("GetUserData")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetUserData()
        {
            try
            {
                var result = await _IUsers.GetUserData();
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
        //<History Author = 'Ammaar Naveed' Date='2024-04-29' Version="1.0" Branch="master"> Check employee current leave status</History>
        [HttpGet("CheckEmployeeLeaveStatus")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> CheckEmployeeLeaveStatus(string UserId, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                bool result = await _IUsers.CheckEmployeeLeaveStatus(UserId, FromDate, ToDate);
                if (result != false)
                {
                    return Ok(result);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        //<History Author = 'Ammaar Naveed' Date='2024-04-25' Version="1.0" Branch="master">Get alternate Vice HOS</History>
        [HttpGet("GetAlternateViceHos")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAlternateEmployeesList(int SectorTypeId, string RoleId, DateTime FromDate, DateTime ToDate)
        {
            try
            {
                var result = await _IUsers.GetAlternateEmployeesList(SectorTypeId, RoleId, FromDate, ToDate);
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
        //<History Author = 'Ammaar Naveed' Date='2024-04-25' Version="1.0" Branch="master">Assign delegated user to selected user.</History>
        [HttpPost("SaveDelegatedEmployee")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SaveDelegatedEmployee(EmployeeLeaveDelegationInformation delegatedEmployeeInformation)
        {
            try
            {
                await _IUsers.SaveDelegatedEmployee(delegatedEmployeeInformation);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetUsersByBugTypeId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetUsersByBugTypeId(int TypeId)
        {
            try
            {
                var result = await _IUsers.GetUsersByBugTypeId(TypeId);
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
        //<History Author = 'Ammaar Naveed' Date='2024-09-30' Version="1.0" Branch="master">Save delegated user and assign tasks for deactivated employee</History>
        [HttpPost("SaveDelegatedEmployeeForDeactivatedEmployee")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SaveDelegatedEmployeeForDeactivatedEmployee(EmployeeDelegationVM employeeDelegation)
        {
            try
            {
                await _IUsers.SaveDelegatedEmployeeForDeactivatedEmployee(employeeDelegation);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpGet("DepartmentList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> DepartmentList()
        {
            try
            {
                return Ok(await _IUsers.Department());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("DeactivateEmployees")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> DeactivateEmployees(DeactivateEmployeesVM deactivateEmployeesVMList, string EmployeeId, string loggedInUser)
        {
            try
            {
                await _IUsers.DeactivateEmployee(deactivateEmployeesVMList, EmployeeId, loggedInUser);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpPost("RecordUserLogoutActivity")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> RecordUserLogoutActivity(string username, string userId)
        {
            try
            {
                await _IUsers.RecordUserLogoutActivity(username, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpGet("GetEmployeeDelegationsInformation")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetEmployeeDelegationsInformation(string userId)
        {
            try
            {
                return Ok(await _IUsers.GetEmployeeDelegationsInformation(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #region Get users by role id and sector id
        [HttpGet("GetUsersByRoleIdandSectorId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetUsersByRoleAndSector(string RoleId, int SectorTypeId)
        {
            try
            {
                var result = await _IUsers.GetUsersByRoleIdandSectorId(RoleId, SectorTypeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region Get Groups by role id and sector id
        [HttpGet("GetGroupsByRoleIdandSectorId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetGroupsByRoleAndSector(string RoleId, int SectorTypeId)
        {
            try
            {
                var result = await _IUsers.GetGroupsByRoleIdandSectorId(RoleId, SectorTypeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        //< History Author = "Ammaar Naveed" Date = "07/05/2024" Version = "1.0" Branch = "master" >Get managers by sector type id and hierarchy definition against role</ History >
        [HttpGet("GetManagersList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetManagersList(int? SectorTypeId, int DesignationId)
        {
            try
            {
                var result = await _IUsers.GetManagersList(SectorTypeId, DesignationId);
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

        [HttpPost("SaveEmployeeRole")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SaveEmployeeRole(UserRoleAssignmentVM userRoleAssignmentVM)
        {
            try
            {
                await _IUsers.AddEditEmployeeRole(userRoleAssignmentVM);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpPost("SaveUserClaims")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> SaveUserClaims(UserClaimsVM userClaimsVM)
        {
            try
            {
                await _IUsers.SaveUserClaims(userClaimsVM);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        [HttpPost("AllowBulkDigitalSign")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> AllowBulkDigitalSign(EmployeeVMForDropDown data)
        {
            try
            {
                await _IUsers.AllowBulkDigitalSign(data);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #region Get User List By SectorId
        [HttpGet("GetCommitteeUsersListBySectorId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCommitteeUsersListBySectorId(int SectorTypeId)
        {
            try
            {
                var result = await _IUsers.GetCommitteeUsersListBySectorId(SectorTypeId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        #region Get Employee Working Hours

        [HttpGet(nameof(GetEmployeeWorkingHours))]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetEmployeeWorkingHours(string userId)
        {
            try
            {
                var result = await _IUsers.GetEmployeeWorkingHours(userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Update default correspondence receiver status
        [HttpPost("UpdateDefaultReceiverStatus")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> UpdateDefaultReceiverStatus(bool isDefaultCorrespondenceReceiver, string userId)
        {
            try
            {
                await _IUsers.UpdateDefaultReceiverStatus(isDefaultCorrespondenceReceiver, userId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion


        #region Get ManagerBy Role And Sector
        [HttpPost(nameof(GetManagerByRoleAndSector))]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetManagerByRoleAndSector([FromForm] int sectorId, [FromForm] string roleId)
        {
            try
            {
                var result = await _IUsers.GetManagerByRoleAndSector(sectorId, roleId);
                if (result == null)
                    return BadRequest(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Get Employees List By Designation Id
        [HttpGet("GetEmployeesByDesignationId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetEmployeesByDesignationId(int? DesignationId)
        {
            try
            {
                var result = await _IUsers.GetEmployeesByDesignationId(DesignationId);
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

        #region Get UMS Claims List By Module Id
        [HttpGet("GetUmsClaimsByModuleId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetUmsClaimsByModuleId(int? moduleId)
        {
            try
            {
                var result = await _IUsers.GetUmsClaimsByModuleId(moduleId);
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

        #region Get Manager By user ID
        [HttpGet(nameof(GetManagerByuserId))]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetManagerByuserId(string userId)
        {
            try
            {
                var result = await _IUsers.GetManagerByuserId(userId);
                if (result == null)
                    return BadRequest(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion
        #region Get User List For Mention
        [HttpGet("GetUsersListForMention")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetUsersListForMention(Guid TicketId, string LoggedInUserEmail)
        {
            try
            {
                return Ok(await _IUsers.GetUsersListForMention(TicketId, LoggedInUserEmail));
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion


        #region Get Active Users By SectorTypeId

        [HttpGet(nameof(GetActiveUsersBySectorTypeId))]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetActiveUsersBySectorTypeId(int? sectorTypeId)
        {
            try
            {
                var result = await _IUsers.GetActiveUsersBySectorTypeId(sectorTypeId);
                if (result == null)
                    return BadRequest(result);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        #endregion

        #region Get Employees List For Admin
        [HttpPost("GetEmployeesListForAdmin")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetEmployeesListForAdmin(int EmployeeTypeId, int? SectorTypeId, int? DesignationId)
        {
            try
            {
                return Ok(await _IUsers.GetEmployeesListForAdmin(EmployeeTypeId, SectorTypeId, DesignationId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPost("GetEmployeesListForUserGroup")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetEmployeesListForUserGroup(UserListAdvanceSearchVM advanceSearchVM)
        {
            try
            {
                return Ok(await _IUsers.GetEmployeesListForUserGroup(advanceSearchVM));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion
    }


}
