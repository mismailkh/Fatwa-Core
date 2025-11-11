using FATWA_API.RabbitMQ;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.DigitalSignature;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_GENERAL.Helper;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Itenso.TimePeriod;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RemoteSigningServiceReference;
using System.Net;
using System.Security.Claims;
using System.ServiceModel.Channels;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Request;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_API.Controllers.V1
{

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Route("api/[controller]")] // for backward compatibility

    //<History Author = 'Aqeel Altaf Abbasi' Date='2022-02-25' Version="1.0" Branch="master"> Add basic Authentication and Authorization with JWT</History>

    //<History Author = 'Umer Zaman' Date='2022-03-30' Version="1.0" Branch="master">Add Process and Error Log's functionality</History>
    public class AccountController : ControllerBase
    {
        private readonly IAuthorizationService _AuthorizationService;
        private readonly IAccount _IAccount;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AccountController> _logger;
        private readonly IAuditLog _auditLogs;

        public AccountController(JwtSettings jwtSettings, IAccount iAccount, IAuthorizationService AuthorizationService,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ILogger<AccountController> logger,
            IHttpContextAccessor httpContextAccessor,
            IAuditLog audit) //Logging logging
        {
            _IAccount = iAccount;
            _AuthorizationService = AuthorizationService;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _auditLogs = audit;
        }

        #region Register

        [HttpPost("Register")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        //<History Author = 'Aqeel Altaf Abbasi' Date='2022-02-25' Version="1.0" Branch="master"> Some Generic fixes when stopped working after role rights</History>
        public async Task<IActionResult> Register([FromBody] IdentityRequest request)
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
                var authResponse = await _IAccount.RegisterAsync(request.UserName, request.Password);
                if (!authResponse.Success)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = authResponse.Errors
                    });

                }
                return Ok(new Response.UserSucessResponse
                {
                    Token = authResponse.Token,
                    RefreshToken = authResponse.RefreshToken,
                    User = authResponse.User
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        #endregion

        #region Login

        [HttpPost("Login")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        //<History Author = 'Aqeel Altaf Abbasi' Date='2022-02-25' Version="1.0" Branch="master"> Some Generic fixes when stopped working after role rights</History>
        public async Task<IActionResult> Login([FromBody] IdentityRequest request)
        {
            try
            {
                _logger.LogInformation("login is  executing...");
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }
                var authResponse = await _IAccount.LoginAsync(request.UserName, request.Password, request.CultureValue);
                if (!authResponse.Success)
                {
                    await RecordLoginException(authResponse, null);
                    if (request.ChannelId > 0)
                    {
                        return BadRequest(new ApiCallResponse
                        {
                            StatusCode = HttpStatusCode.BadRequest,
                            IsSuccessStatusCode = false,
                            Message = string.Join('\n', authResponse.Errors).ToString(),
                        });
                    }
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = authResponse.Errors
                    });
                }
                if (request.ChannelId > 0)
                {
                    UserSucessResponse userSucessResponse = new UserSucessResponse()
                    {
                        Token = authResponse.Token,
                        RefreshToken = authResponse.RefreshToken,
                        UserClaims = authResponse.ClaimsResultList,
                        TranslationsList = authResponse.TranslationsList,
                        User = authResponse.User,
                        UserDetail = authResponse.UserDetail,
                        ProfilePicUrl = authResponse.ProfilePicUrl
                    };
                    return Ok(new ApiCallResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        IsSuccessStatusCode = true,
                        ResultData = userSucessResponse,
                        Message = "success"
                    });
                }
                return Ok(new UserSucessResponse
                {
                    Token = authResponse.Token,
                    RefreshToken = authResponse.RefreshToken,
                    UserClaims = authResponse.ClaimsResultList,
                    TranslationsList = authResponse.TranslationsList,
                    User = authResponse.User,
                    UserDetail = authResponse.UserDetail,
                    ProfilePicUrl = authResponse.ProfilePicUrl
                });
            }
            catch (Exception ex)
            {
                if (request.ChannelId > 0)
                {
                    return BadRequest(new ApiCallResponse
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        IsSuccessStatusCode = false,
                        ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message },
                        Message = "Error_Ocurred"
                    });
                }
                var response = new BadRequestResponse { Message = ex.Message };
                if (ex.InnerException != null)
                {
                    response.InnerException = ex.InnerException.Message;
                }
                await RecordLoginException(null, ex.Message);
                return BadRequest(response);
            }
        }

        #endregion

        #region Record login exception in User Activity table
        private async Task RecordLoginException(AuthenticationResult authResponse, string innerException)
        {
            try
            {
                string exceptionMessage = null;

                if (authResponse != null)
                {
                    exceptionMessage = authResponse.Errors.FirstOrDefault();
                }
                else if (!string.IsNullOrEmpty(innerException))
                {
                    exceptionMessage = innerException;
                }

                await _IAccount.RecordLoginExceptions(exceptionMessage, null);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion


        #region SingleSignOn

        [HttpPost("SingleSignOn")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        //<History Author = 'Nadia Gull' Date='2024-01-21' Version="1.0" Branch="master"> Assign token,Claims,User details after SSO</History>
        public async Task<IActionResult> SingleSignOn([FromBody] string? SamAccountName)
        {
            try
            {
                _logger.LogInformation("login is  executing...");
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }
                var authResponse = await _IAccount.SingleSignOn(SamAccountName);
                if (!authResponse.Success)
                {
                    await RecordLoginException(authResponse, null);
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = authResponse.Errors
                    });
                }

                return Ok(new UserSucessResponse
                {
                    Token = authResponse.Token,
                    RefreshToken = authResponse.RefreshToken,
                    UserClaims = authResponse.ClaimsResultList,
                    TranslationsList = authResponse.TranslationsList,
                    User = authResponse.User,
                    UserDetail = authResponse.UserDetail,
                    ProfilePicUrl = authResponse.ProfilePicUrl
                });
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

        #endregion

        #region GetList

        [HttpGet("UserListByUserId")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        //<History Author = 'Zain Ul Islam' Date='2022-03-28' Version="1.0" Branch="master"> returns the user by id</History>
        public async Task<IActionResult> UserListByUserId(string userId)
        {
            try
            {
                var user = await _IAccount.UserDetailByUserId(userId);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("UserList")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Aqeel Altaf Abbasi' Date='2022-02-28' Version="1.0" Branch="master"> returns the list of authenticated users</History>
        public async Task<IActionResult> UserList()
        {
            try
            {
                _logger.LogInformation("login is  executing...");

                var currentUser = await _userManager.GetUserAsync(HttpContext.User);
                var allUsers = await _userManager.Users.ToListAsync();

                return Ok(allUsers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("UserListBySearchTerm")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Aqeel Altaf Abbasi' Date='2022-02-28' Version="1.0" Branch="master"> returns the list of  users</History>
        public async Task<IActionResult> UserListBySearchTerm(string? searchTerm)
        {
            try
            {
                var allUsers = await _IAccount.UserListBySearchTerm(searchTerm);
                return Ok(allUsers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        

        [HttpGet("UserGroupListBySearchTerm")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Aqeel Altaf Abbasi' Date='2022-02-28' Version="1.0" Branch="master"> returns the list of  users</History>
        public async Task<IActionResult> UserGroupListBySearchTerm(string? searchTerm)
        {
            try
            {
                var allUsers = await _IAccount.UserGroupListBySearchTerm(searchTerm);
                return Ok(allUsers);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("UserBorrowLiteratures")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nadia Gull' Date='2022-11-3' Version="1.0" Branch="master"> returns the list of User Borrow Literatures</History>
        public async Task<IActionResult> UserBorrowLiteratures(string? userId)
        {
            try
            {
                var borrowLiteratures = await _IAccount.UserBorrowLiteratures(userId);
                return Ok(borrowLiteratures);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }



        [HttpGet("UserGroupListByUserGroupId")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        //<History Author = 'Zain Ul Islam' Date='2022-03-28' Version="1.0" Branch="master"> returns the user by id</History>
        public async Task<IActionResult> UserGroupListByUserGroupId(Guid userGroupId)
        {
            try
            {
                var user = await _IAccount.UserGroupListByUserGroupId(userGroupId);

                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        [HttpGet("GetSecurityStampByEmail")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        //<History Author = 'Aqeel Altaf' Date='2022-03-28' Version="1.0" Branch="master"> returns the security stamp</History>
        public async Task<IActionResult> GetSecurityStampByEmail(string emailId)
        {
            try
            {
                var securityStamp = await _IAccount.GetSecurityStampByEmail(emailId);

                return Ok(securityStamp);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #region Get Roles Details
        [HttpGet("GetUserRolesByUserName")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetUserRolesByUserName(string userName)
        {
            try
            {
                var roles = await _IAccount.GetUserRolesByUserName(userName);
                return Ok(roles);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }

        #endregion



        [HttpGet("GetTaskStatuses")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Aqeel Altaf Abbasi' Date='2022-02-28' Version="1.0" Branch="master">Get Task Statuses</History>
        public async Task<IActionResult> GetTasktSatuses()
        {
            try
            {
                var result = await _IAccount.GetTasktSatuses();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        #endregion


        [HttpGet("CheckEmailExists")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Zain Ul Islam' Date='2022-03-28' Version="1.0" Branch="master"> Check Email Exists</History>
        public async Task<IActionResult> CheckEmailExists(string email)
        {
            try
            {
                bool result = await _IAccount.CheckEmailExists(email);
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
        [HttpPost("ResetUserPassword")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        //<History Author = 'Attique Rehman' Date='2023-12-19' Version="1.0" Branch="master"> Added Funcationality for user Reset Password </History>
        public async Task<IActionResult> ResetUserPassword([FromBody] ResetPasswordVM requestBody)
        {
            try
            {
                var user = await _IAccount.ResetUserPasswordAsync(requestBody);
                if (string.IsNullOrEmpty(user))
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        ErrorMessage = " لقد حدث خطأ، يرجى المحاولة مرة أخرى!"
                    });
                }
                _auditLogs.CreateProcessLog(new ProcessLog
                {
                    Process = "Password Change",
                    Task = "Reset Password",
                    Description = "Password for " + user + " is Change successfully",
                    ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    Message = "Password Change successfully ",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.UMS,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return Ok();
            }
            catch (Exception ex)
            {
                var response = new BadRequestResponse { Message = ex.Message };
                if (ex.InnerException != null)
                {
                    response.InnerException = ex.InnerException.Message;
                }
                _auditLogs.CreateErrorLog(new ErrorLog
                {
                    ErrorLogEventId = (int)ErrorLogEnum.Error,
                    Subject = "Reset Password Changed Failed",
                    Body = ex.Message,
                    Category = "unable to Reset User password",
                    Source = ex.Source,
                    Type = ex.GetType().Name,
                    Message = "Password Change Failed",
                    IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    ApplicationID = (int)PortalEnum.FatwaPortal,
                    ModuleID = (int)WorkflowModuleEnum.UMS,
                    Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                });
                return BadRequest(response);
            }
        }


        [HttpGet("GetUsersList")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetUsersList()
        {
            try
            {
                return Ok(await _IAccount.GetUsersList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("UserIdByUserEmail")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Nabeel ur Rehman' Date='2022-02-28' Version="1.0" Branch="master">Get Task Statuses</History>
        public async Task<IActionResult> UserIdByUserEmail(string email)
        {
            try
            {
                var result = await _IAccount.UserIdByUserEmail(email);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        #region Role & Rights

        [HttpPost("HasPermission")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Aqeel Altaf Abbasi' Date='2022-02-28' Version="1.0" Branch="master"> General function to check permissions</History>
        public IActionResult HasPermission(string permission)
        {
            try
            {
                string token = Request.Headers["Authorization"];
                var actualToken = token.Replace("Bearer ", "");
                ClaimsPrincipal claimInfo = _IAccount.GetClaimsFromTokenAsync(actualToken);
                var claim = claimInfo.Claims.FirstOrDefault(x => x.Value == permission);
                if (claim != null)
                {
                    return new JsonResult(new { StatusCode = HttpStatusCode.OK, Result = true });
                }
                else
                {
                    return new JsonResult(new { StatusCode = HttpStatusCode.OK, Result = false });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new { StatusCode = HttpStatusCode.BadRequest, Result = false, Message = ex.Message });
            }
        }

        [HttpGet("GetClaimsList")]
        //<History Author = 'Aqeel Altaf Abbasi' Date='2022-02-28' Version="1.0" Branch="master"> Get all claims based on user</History>
        public IActionResult GetClaimsList()
        {
            try
            {
                List<ClaimSucessResponse> claimlist = new List<ClaimSucessResponse>();
                string token = Request.Headers["Authorization"];
                var actualToken = token.Replace("Bearer ", "");
                ClaimsPrincipal claimInfo = _IAccount.GetClaimsFromTokenAsync(actualToken);
                var claimslistdb = claimInfo.Claims.ToList();
                foreach (var claimItem in claimslistdb)
                {
                    ClaimSucessResponse claims = new ClaimSucessResponse();
                    claims.Type = claimItem.Type;
                    claims.Value = claimItem.Value;
                    claimlist.Add(claims);
                }
                if (claimlist != null)
                {
                    return Ok(claimlist);
                }
                else
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        ErrorMessage = "No Claim Exist for this user"
                    });
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("GetRoleList")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        //<History Author = 'Hassan Abbas' Date='2022-06-20' Version="1.0" Branch="master"> returns the list of authenticated users</History>
        public async Task<IActionResult> GetRoleList()
        {
            try
            {
                return Ok(await _roleManager.Roles.ToListAsync());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet("GetAllTranslations")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Hassan Abbas' Date='2022-07-05' Version="1.0" Branch="master"> returns the list of Translations</History>
        public async Task<IActionResult> GetAllTranslations()
        {
            try
            {
                return Ok(await _IAccount.GetAllTranslations());
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #endregion
        [HttpGet("GetAttendeeUser")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetAttendeeUser()
        {
            try
            {
                return Ok(await _IAccount.GetAttendeeUser());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        
        //<History Author = 'Ammaar Naveed' Date='2024-01-24' Version="1.0" Branch="master">Controller to get employee password history</History>
        [HttpGet("GetUserPasswordResetHistory")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetEmployeePasswordHistory(string userId)
        {
            try
            {
                return Ok(await _IAccount.GetEmployeePasswordHistory(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //<History Author = 'Ammaar Naveed' Date='2024-01-24' Version="1.0" Branch="master">Gets employee login/logout activities.</History>
        [HttpGet("GetEmployeeActivities")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetEmployeeActivities(string userId)
        {
            try
            {
                return Ok(await _IAccount.GetEmployeeActivities(userId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #region Mobile App End Points
        //<History Author = 'Noman Khan' Date='2024-05-09' Version="1.0" Branch="master">Gets employee login/logout activities.</History>
        [HttpPost("SaveDeviceRegisteredFCMToken")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> SaveDeviceRegisteredFCMToken([FromBody] SaveDeviceTokenVM saveDeviceTokenVM)
        {
            try
            {
                ApiCallResponse response = new ApiCallResponse();
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }
                response = await _IAccount.SaveDeviceRegisteredFCMToken(saveDeviceTokenVM);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiCallResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccessStatusCode = false,
                    ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message },
                    Message = "Error_Ocurred"
                });
            }
        }
        //<History Author = 'Noman Khan' Date='2024-05-21' Version="1.0" Branch="master">Gets employee login/logout activities.</History>
        [HttpDelete("DeleteRegisteredDeviceToken")]
        [MapToApiVersion("1.0")]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteRegisteredDeviceToken(string token, string userId, int channelId, string cultureValue)
        {
            try
            {
                ApiCallResponse response = new ApiCallResponse();
                if (!ModelState.IsValid)
                {
                    return BadRequest(new RequestFailedResponse
                    {
                        Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                    });
                }
                response = await _IAccount.DeleteRegisteredDeviceToken(token, userId, channelId, cultureValue);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiCallResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccessStatusCode = false,
                    ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message },
                    Message = "Error_Ocurred"
                });
            }
        }
        #endregion
        #region Get Get Committee Members By ReferenceId
        [HttpGet("GetCommitteeMembersByReferenceId")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> GetCommitteeMembersByReferenceId(Guid ReferenceId)
        {
            try
            {
                return Ok(await _IAccount.GetCommitteeMembersByReferenceId(ReferenceId));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        #endregion

        [HttpGet("GetAllUserList")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Aqeel Altaf Abbasi' Date='2022-02-28' Version="1.0" Branch="master">Get Task Statuses</History>
        public async Task<IActionResult> GetAllUserList()
        {
            try
            {
                var result = await _IAccount.GetTasktSatuses();
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }
        #region Get Legal Cultural Center Users List
        [HttpGet("GetLegalCulturalCenterUsersList")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2024-09-04' Version="1.0" Branch="master"> returns the list of  users</History>
        public async Task<IActionResult> GetLegalCulturalCenterUsersList(string? searchTerm)
        {
            try
            {
                var allUsers = await _IAccount.GetLegalCulturalCenterUsersList(searchTerm);
                return Ok(allUsers);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
        #region Get Library Users List
        [HttpGet("GetLibraryUsersList")]
        [MapToApiVersion("1.0")]
        //<History Author = 'Muhammad Zaeem' Date='2024-09-04' Version="1.0" Branch="master"> returns the list of  users</History>
        public async Task<IActionResult> GetLibraryUsersList(string? searchTerm)
        {
            try
            {
                var allUsers = await _IAccount.GetLibraryUsersList(searchTerm);
                return Ok(allUsers);
            }
            catch (Exception ex)
            {
                return BadRequest(new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message });
            }
        }
        #endregion
    }
}