using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using FATWA_GENERAL.Helper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static FATWA_GENERAL.Helper.Request;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_API.Controllers.V2
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    //[Route("api/[controller]")] // for backward compatibility
    //<History Author = 'Aqeel Altaf Abbasi' Date='2022-02-25' Version="1.0" Branch="master"> Add basic Authentication and Authorization with JWT</History>
    public class AccountController : ControllerBase
    {
        private readonly IAuthorizationService _AuthorizationService;
        private readonly IAccount _IAccount;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(JwtSettings jwtSettings, IAccount iAccount, IAuthorizationService AuthorizationService,
            UserManager<IdentityUser> userManager,
            IHttpContextAccessor httpContextAccessor)
        {
            _IAccount = iAccount;
            _AuthorizationService = AuthorizationService;
            _userManager = userManager;
            _httpContextAccessor = httpContextAccessor;
        }

        #region Register

        [HttpPost("Register")]
        [MapToApiVersion("2.0")]
        [AllowAnonymous]
        //<History Author = 'Aqeel Altaf Abbasi' Date='2022-02-25' Version="1.0" Branch="master"> Some Generic fixes when stopped working after role rights</History>
        public async Task<IActionResult> Register([FromBody] IdentityRequest request)
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

        #endregion

        #region Login

        [HttpPost("Login")]
        [MapToApiVersion("2.0")]
        [AllowAnonymous]
        //<History Author = 'Aqeel Altaf Abbasi' Date='2022-02-25' Version="1.0" Branch="master"> Some Generic fixes when stopped working after role rights</History>
        public async Task<IActionResult> Login([FromBody] IdentityRequest request)
        {
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
                return BadRequest(new RequestFailedResponse
                {
                    Errors = authResponse.Errors

                });
            }

            return Ok(new Response.UserSucessResponse
            {
                Token = authResponse.Token,
                RefreshToken = authResponse.RefreshToken,
                UserClaims = authResponse.ClaimsResultList,
                TranslationsList = authResponse.TranslationsList,
                User = authResponse.User
            });
        }

        #endregion

        #region GetList

        [HttpGet("UserList")]
        [MapToApiVersion("2.0")]
        //<History Author = 'Aqeel Altaf Abbasi' Date='2022-02-28' Version="1.0" Branch="master"> returns the list of authenticated users</History>
        public async Task<IActionResult> UserList()
        {
            var currentUser = await _userManager.GetUserAsync(HttpContext.User);
            var allUsers = await _userManager.Users.ToListAsync();
            return Ok(allUsers);
        }

        #endregion

        #region Role & Rights

        [HttpPost("HasPermission")]
        [MapToApiVersion("2.0")]
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
                    return new JsonResult(new { StatusCode = 200, Result = true });
                }
                else
                {
                    return new JsonResult(new { StatusCode = 200, Result = false });
                }
            }
            catch (Exception ex)
            {
                return new JsonResult(new { StatusCode = 400, Result = false, Message = ex.Message });
            }
        }

        [HttpPost("GetClaimsList")]
        [MapToApiVersion("2.0")]
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
        #endregion
    }
}