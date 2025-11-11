using System.DirectoryServices.AccountManagement;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using AutoMapper;
using Azure;
using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.Meet;
using FATWA_DOMAIN.Models.WorkflowModels;
using FATWA_GENERAL.Helper;
using FATWA_INFRASTRUCTURE.Database;
using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.AspNetCore.Builder.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using static System.Formats.Asn1.AsnWriter;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.UserEnum;
using static FATWA_GENERAL.Helper.Permissions;
using static FATWA_GENERAL.Helper.Response;
using static Google.Apis.Requests.BatchRequest;
using Group = FATWA_DOMAIN.Models.AdminModels.UserManagement.Group;

namespace FATWA_INFRASTRUCTURE.Repository
{
    public class AccountRepository : IAccount
    {
        private readonly JwtSettings _jwtSettings;
        private readonly DatabaseContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly TokenValidationParameters _tokenValidationParameters;
        public readonly IPasswordHasher<IdentityUser> _passwordHasher;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        private List<UserVM> _userList;
        private List<Group> _userGroupList;
        private List<UserBorrowLiteratureVM> _borrowLiteratureList;
        private List<ClaimVM> _ClaimVM;
        private ActiveDirectorySettings _adSettings { get; set; }
        private int FloorId { get; set; } = 0;
        private int BuildingId { get; set; } = 0;
        private int BorrowReturnDayDuration { get; set; } = 0;
        public int DepartmentId { get; set; } = 0;

        public AccountRepository(IPasswordHasher<IdentityUser> passwordHasher, JwtSettings jwtSettings,
            DatabaseContext dbContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager,
            TokenValidationParameters tokenValidationParameters, IServiceScopeFactory serviceScopeFactory,
            IMapper mapper, IOptions<ActiveDirectorySettings> ADSettings, IConfiguration config)
        {
            _jwtSettings = jwtSettings;
            _dbContext = dbContext;
            _tokenValidationParameters = tokenValidationParameters;
            _userManager = userManager;
            _roleManager = roleManager;
            _passwordHasher = passwordHasher;
            _serviceScopeFactory = serviceScopeFactory;
            _mapper = mapper;
            _adSettings = ADSettings.Value;
            _config = config;
        }

        #region Sign Up

        //<History Author = 'Aqeel Altaf Abbasi' Date='2022-02-25' Version="1.0" Branch="master"> Some Generic fixes when stopped working after role rights</History>
        //<History Author = 'Hassan Abbas' Date='2022-03-04' Version="2.0" Branch="master"> Removed existing user logic to display identity provided validations</History>
        public async Task<AuthenticationResult> RegisterAsync(string email, string password)
        {
            var newUser = new IdentityUser
            {
                Email = email,
                UserName = email
            };
            var createUser = await _userManager.CreateAsync(newUser, password);
            if (!createUser.Succeeded)
            {
                return new AuthenticationResult
                {
                    Errors = createUser.Errors.Select(x => x.Description)
                };
            }

            return await GenerateAuthenticationResultByuserAsync(newUser);

        }

        #endregion

        #region Sign In
        //<History Author = 'Ammaar Naveed' Date='2024-05-23' Version="1.0" Branch="master">Check user status before signin.</History>
        public async Task<AuthenticationResult> SingleSignOn(string SamAccountName)
        {
            var email = _dbContext.Users.Where(x => x.ADUserName == SamAccountName).Select(x => x.Email).FirstOrDefault();
            var userStatus = _dbContext.Users.Where(x => x.ADUserName == SamAccountName && x.IsActive == true).Select(x => x.IsActive).FirstOrDefault();
            var user = await _userManager.FindByEmailAsync(email);

            if (user == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "المستخدم غير موجود!" } // User does not exist!
                };
            }

            if (userStatus != true)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "المستخدم إما غير نشط أو محذوف!" } // User is either inactive or deleted!
                };
            }

            return await GenerateAuthenticationResultByuserAsync(user);
        }

        //<History Author = 'Zain Ul Islam' Date='2022-08-03' Version="1.0" Branch="master"> Deleted or InActive User should not Login</History> 
        //<History Author = 'Aqeel Altaf Abbasi' Date='2022-02-25' Version="1.0" Branch="master"> Some Generic fixes when stopped working after role rights</History>
        //<History Author = 'Attique Rehman' Date='2024-jan-12' Version="2.0" Branch="master">Advance login Funtionality ( Check release mode comments)  </History>
        public async Task<AuthenticationResult> LoginAsync(string email, string password, string culture)
        {
#if DEBUG
            try
            {
                return await AuthenticateUserViaDB(email, password, culture);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
#else
            try
            {
                    //<History Author = 'Attique Rehman' Date='2024-jan-12' Version="2.0" Branch="master">Advance login Funtionality for Internal/External Employee thru Ad/Db(release mode)  </History>
                    #region Functionality Description
                    // Check the EmployeeType on login to determine if they're Internal or External
                    // Internal employees can only log in with their UserName via Active Directory (AD)
                    // If an Internal user tries to log in, restrict access to AD UserName only, not email anymore
                    // External users (not part of AD anymore) can log in via Database (Db) only with email
            #endregion

                    int EmployeeType = 0;
                    var userFindByEmail = await _dbContext.Users.Where(x => x.Email == email || x.ADUserName == email).FirstOrDefaultAsync();
                    if (userFindByEmail != null)
                    {
                        EmployeeType = await _dbContext.UserEmploymentInformation.Where(x => x.UserId == userFindByEmail.Id).Select(x => x.EmployeeTypeId).FirstOrDefaultAsync();
                        if (EmployeeType == (int)EmployeeTypeEnum.Internal && _config["Environment"] != "DPS")
                        {
                           return await AuthenticateUserViaAD(email,password, culture);

                        }
                        else
                        {
                            return await AuthenticateUserViaDB(email, password, culture);
                        }
                    }
                    else
                    {
                        return new AuthenticationResult
                        {
                            Errors = new[] { LoginPageMessages.GetMessage("UserDoesNotExist", culture) }
                        };
                    }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
#endif
        }
        //<History Author = 'Attique Rehman' Date='2024-jan-23' Version="1.0" Branch="master"> Authentication For Internal Employee from Active Directory </History>
        public async Task<AuthenticationResult> AuthenticateUserViaAD(string email, string password, string culture)
        {
            var context = new PrincipalContext(ContextType.Domain, _adSettings.ServerIPAddress, _adSettings.Container, _adSettings.MachineAccountName, _adSettings.MachineAccountPassword);
            var userPrincipal = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, email);
            if (email.Contains("@"))
            {
                return new AuthenticationResult
                {
                    Errors = new[] { LoginPageMessages.GetMessage("UseUsernameInsteadOfEmail", culture) }
                };
            }
            if (userPrincipal == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { LoginPageMessages.GetMessage("UserDoesNotExist", culture) }
                };
            }
            else
            {
                if (!userPrincipal.IsAccountLockedOut())
                {
                    if ((bool)userPrincipal.Enabled)
                    {
                        if (context.ValidateCredentials(email, password) == false)
                        {
                            return new AuthenticationResult
                            {
                                Errors = new[] { LoginPageMessages.GetMessage("UserEmailOrPasswordIncorrect", culture) }
                            };
                        }
                        else
                        {
                            var user = await _userManager.FindByEmailAsync(userPrincipal.EmailAddress);
                            return await GenerateAuthenticationResultByuserAsync(user);
                        }
                    }
                    else
                    {
                        return new AuthenticationResult
                        {
                            Errors = new[] { LoginPageMessages.GetMessage("UserIsDeactivated", culture) }
                        };
                    }
                }
                else
                {
                    return new AuthenticationResult
                    {
                        Errors = new[] { LoginPageMessages.GetMessage("UserIsInactiveOrDeleted", culture) }
                    };
                }
            }
        }
        //<History Author = 'Attique Rehman' Date='2024-jan-23' Version="1.0" Branch="master"> Authentication For External Employee from DataBase</History>
        public async Task<AuthenticationResult> AuthenticateUserViaDB(string email, string password, string culture)
        {

            var isValidUser = _dbContext.Users.Where(x => (x.Email == email) && (x.IsDeleted == false) && (x.IsActive == true) && (x.IsLocked == false)).FirstOrDefault();
            if (isValidUser != null)
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return new AuthenticationResult
                    {
                        Errors = new[] { LoginPageMessages.GetMessage("UserDoesNotExist", culture) }
                    };
                }
                var userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);
                if (!userHasValidPassword)
                {
                    return new AuthenticationResult
                    {
                        Errors = new[] { LoginPageMessages.GetMessage("UserEmailOrPasswordIncorrect", culture) }
                    };
                }
                return await GenerateAuthenticationResultByuserAsync(user);
            }
            else
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return new AuthenticationResult
                    {
                        Errors = new[] { LoginPageMessages.GetMessage("UserDoesNotExist", culture) }
                    };
                }
                else
                {
                    return new AuthenticationResult
                    {
                        Errors = new[] { LoginPageMessages.GetMessage("UserIsInactiveOrDeleted", culture) }
                    };
                }
            }
        }



        //<History Author = 'Aqeel Altaf Abbasi' Date='2022-02-25' Version="1.0" Branch="master"> Some Generic fixes when stopped working after role rights</History>
        //<History Author = 'Hassan Abbas' Date='2022-02-25' Version="1.0" Branch="master"> Merged permissions oming from User, Roles, </History>
        private async Task<AuthenticationResult> GenerateAuthenticationResultByuserAsync(IdentityUser user)
        {
            try
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                List<ClaimSucessResponse> claimlistforResult = new List<ClaimSucessResponse>();
                IList<Claim> Addedclaims = new List<Claim>();
                IList<Claim> EmptyClaims = new List<Claim>();

                IList<TranslationSucessResponse> AddedTranslations = new List<TranslationSucessResponse>();
                // Get User Specific Claims
                await GetSpecificClaimsAndCreateClaimResultAsync(claimlistforResult, user);
                // Get Group Specific Claims
                var userGroups = await _dbContext.UserGroups.Where(x => x.UserId == user.Id).ToListAsync();
                List<Group> userGroupList = new List<Group>();
                foreach (var group in userGroups)
                {
                    var userGroup = new Group();
                    userGroup.GroupId = group.GroupId;
                    userGroupList.Add(userGroup);
                }
                await GetGroupSpecificClaimsAndAddClaims(userGroupList, claimlistforResult);

                EmptyClaims.Add(new System.Security.Claims.Claim(JwtRegisteredClaimNames.Sub, user.Email));
                EmptyClaims.Add(new System.Security.Claims.Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
                EmptyClaims.Add(new System.Security.Claims.Claim(JwtRegisteredClaimNames.Email, user.Email));
                EmptyClaims.Add(new System.Security.Claims.Claim(JwtRegisteredClaimNames.Name, user.UserName));
                EmptyClaims.Add(new System.Security.Claims.Claim("id", user.Id));
                var tokenhandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
                var tokenDescription = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(EmptyClaims),
                    Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifeTime),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                        SecurityAlgorithms.HmacSha256Signature)
                };
                var token = tokenhandler.CreateToken(tokenDescription);
                var writeToken = tokenhandler.WriteToken(token);
                Guid tokenId = Guid.NewGuid();
                var refreshToken = new TokenModel
                {
                    Token = tokenId.ToString(),
                    JwtId = token.Id,
                    UserId = user.Id,
                    CreateDate = DateTime.UtcNow,
                    ExpireDate = (DateTime)tokenDescription.Expires,
                    JwtToken = writeToken.ToString()
                };
                await _dbContext.TokenModels.AddAsync(refreshToken);
                await _dbContext.SaveChangesAsync();
                GetTranslationsAndCreateResponseObject(AddedTranslations);
                //Get Profile Picture Url
                var employmentInformation = await _dbContext.UserEmploymentInformation.Where(u => u.UserId == user.Id).FirstOrDefaultAsync();
                var personalInformation = await _dbContext.UserPersonalInformation.Where(u => u.UserId == user.Id).FirstOrDefaultAsync();
                bool isUserPasswordReset = await _dbContext.Users.Where(u => u.Id == user.Id).Select(x => x.IsPasswordReset).FirstOrDefaultAsync();
                var umsUser = await _dbContext.Users.Where(u => u.Id == user.Id).FirstOrDefaultAsync();
                var UserSectorDetails = await _dbContext.OperatingSectorType.Where(f => f.Id == employmentInformation.SectorTypeId).FirstOrDefaultAsync();
                var Group = await _dbContext.UserGroups.Where(u => u.UserId == user.Id).FirstOrDefaultAsync();

                if (UserSectorDetails != null)
                {
                    FloorId = UserSectorDetails.FloorId;
                    BuildingId = UserSectorDetails.BuildingId;
                    BorrowReturnDayDuration = await _dbContext.Departments.Where(x => x.Id == UserSectorDetails.DepartmentId).Select(f => f.Borrow_Return_Day_Duration).FirstOrDefaultAsync();
                    DepartmentId = UserSectorDetails.DepartmentId;
                }
                //Get role information that is mainly used through out the app
                IdentityRole? role = null;
                foreach (var userRole in userRoles)
                {
                    role = await _roleManager.FindByNameAsync(userRole);
                }
                var authenticationResult = new AuthenticationResult
                {
                    Success = true,
                    Token = writeToken,
                    RefreshToken = tokenId.ToString(),
                    ClaimsResultList = claimlistforResult,
                    TranslationsList = AddedTranslations,
                    User = user,
                    ProfilePicUrl = string.Empty,
                    UserDetail = new FATWA_DOMAIN.Models.ViewModel.UserDetailVM
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        SecurityStamp = user.SecurityStamp,
                        Email = user.Email,
                        RoleId = role?.Id,
                        RoleName = role?.Name,
                        SectorTypeId = employmentInformation.SectorTypeId != null ? employmentInformation.SectorTypeId : 0,
                        FullNameEn = personalInformation?.FirstName_En + " " + personalInformation?.LastName_En,
                        FullNameAr = personalInformation?.FirstName_Ar + " " + personalInformation?.LastName_Ar,
                        CivilId = personalInformation?.CivilId,
                        UserTypeId = employmentInformation.EmployeeTypeId,
                        IsPasswordReset = isUserPasswordReset,
                        ActiveDirectoryUserName = umsUser.ADUserName,
                        BuildingId = BuildingId,
                        FloorId = FloorId,
                        BorrowReturnDayDuration = BorrowReturnDayDuration,
                        DepartmentId = DepartmentId,
                        DesignationId = employmentInformation.DesignationId,
                        HasSignatureImage = umsUser.HasSignatureImage,
                        CanSignDocument = umsUser.AllowDigitalSign,
                        SectorOnlyViceHOSApprovalEnough = UserSectorDetails != null ? UserSectorDetails.IsOnlyViceHosApprovalRequired : false,
                        GroupId = Group != null ? Group.GroupId : null
                    }
                };

                if(personalInformation is not null && personalInformation.GenderId is not null)
                {
                    authenticationResult.UserDetail.GenderId = (int)personalInformation.GenderId;
                }
                    
                if (authenticationResult != null && UserSectorDetails is not null)
                {
                    Department departmentInformation = null;
                    if (UserSectorDetails is not null)
                        departmentInformation = await _dbContext.Departments.Where(x => x.Id == UserSectorDetails.DepartmentId).FirstOrDefaultAsync();
                    if (departmentInformation is not null)
                    {
                        authenticationResult.UserDetail.DepartmentNameEn = departmentInformation.Name_En;
                        authenticationResult.UserDetail.DepartmentNameAr = departmentInformation.Name_Ar;
                    }
                    authenticationResult.UserDetail.CivilId = personalInformation.CivilId;
                    authenticationResult.UserDetail.EmployeeId = employmentInformation.EmployeeId;
                }
                if (authenticationResult != null)
                {
                    await RecordUserActivity(authenticationResult.User, true);
                }
                return authenticationResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region Storing User Login and Logout Activity
        //<History Author = 'Ammaar Naveed' Date='2024-03-14' Version="1.0" Branch="master"> Stores user's activity in database.</History>
        private async Task RecordUserActivity(IdentityUser user, bool isLoggedIn)
        {
            try
            {
                var userActivity = new UserActivity
                {
                    ActivityId = Guid.NewGuid(),
                    UserId = user.Id,
                    UserName = user.UserName,
                    IPAddress = GetIPAddress(),
                    ComputerName = Environment.MachineName.ToString(),
                    IsLoggedIn = isLoggedIn,
                    LoginDateTime = isLoggedIn ? DateTime.UtcNow : (DateTime?)null,
                };
                await _dbContext.UserActivities.AddAsync(userActivity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region Get IP Address
        private string GetIPAddress()
        {
            try
            {
                string hostName = Dns.GetHostName();
                IPAddress[] localIPs = Dns.GetHostAddresses(hostName);

                foreach (IPAddress ip in localIPs)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                return "Unknown";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Record Login Exceptions
        //<History Author = 'Ammaar Naveed' Date='2024-03-20' Version="1.0" Branch="master"> Stores user's failed login activity</History>
        public async Task RecordLoginExceptions(string loginErrors, string innerException)
        {
            try
            {
                string exceptionMessage = loginErrors ?? innerException;

                var userActivity = new UserActivity
                {
                    ActivityId = Guid.NewGuid(),
                    IPAddress = GetIPAddress(),
                    ComputerName = Environment.MachineName.ToString(),
                    IsLoggedIn = false,
                    LoginDateTime = DateTime.Now,
                    ExceptionMessage = exceptionMessage
                };
                _dbContext.UserActivities.Add(userActivity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #endregion


        private async Task GetSpecificClaimsAndCreateClaimResultAsync(List<ClaimSucessResponse> claimlistforResult, IdentityUser user)
        {
            var userclaimsList = await _userManager.GetClaimsAsync(user);
            foreach (var item in userclaimsList)
            {
                ClaimSucessResponse claimsobj = new ClaimSucessResponse();
                claimsobj.Type = item.Type;
                claimsobj.Value = item.Value;
                claimlistforResult.Add(claimsobj);
            }
        }

        private async Task GetGroupSpecificClaimsAndAddClaims(IList<Group> userGroups, IList<ClaimSucessResponse> claimlistforResult)
        {
            try
            {
                foreach (var userGroup in userGroups)
                {
                    IList<GroupClaims> claimsList = new List<GroupClaims>();

                    claimsList = await _dbContext.UmsGroupClaims.Where(x => x.GroupId == userGroup.GroupId).ToListAsync();
                    foreach (var item in claimsList)
                    {
                        ClaimSucessResponse claimsobj = new ClaimSucessResponse();
                        claimsobj.Type = item.ClaimType;
                        claimsobj.Value = item.ClaimValue;
                        claimlistforResult.Add(claimsobj);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        #endregion

        #region Translations
        private void GetTranslationsAndCreateResponseObject(IList<TranslationSucessResponse> addedTranslations)
        {

            try
            {
                List<Translation> translationsList = new List<Translation>();
                string StoredProc = "exec pTranslationGetBulkDataAll ";
                translationsList = _dbContext.Translations.FromSqlRaw(StoredProc).ToList();
                foreach (var translation in translationsList)
                {
                    TranslationSucessResponse translationobj = new TranslationSucessResponse();
                    translationobj.TranslationKey = translation.TranslationKey;
                    translationobj.Value_Ar = translation.Value_Ar;
                    translationobj.Value_En = translation.Value_En;
                    addedTranslations.Add(translationobj);
                }
            }
            catch (TaskCanceledException ex)
            {
                // Check ex.CancellationToken.IsCancellationRequested here.
                // If false, it's pretty safe to assume it was a timeout.
            }

        }

        //<History Author = 'Hassan Abbas' Date='2022-07-05' Version="1.0" Branch="master"> returns the list of Translations</History>
        public async Task<List<TranslationSucessResponse>> GetAllTranslations()
        {
            try
            {
                List<TranslationSucessResponse> addedTranslations = new List<TranslationSucessResponse>();
                List<Translation> translationsList = new List<Translation>();
                string StoredProc = "exec pTranslationGetBulkDataAll ";
                translationsList = _dbContext.Translations.FromSqlRaw(StoredProc).ToList();
                foreach (var translation in translationsList)
                {
                    TranslationSucessResponse translationobj = new TranslationSucessResponse();
                    translationobj.TranslationKey = translation.TranslationKey;
                    translationobj.Value_Ar = translation.Value_Ar;
                    translationobj.Value_En = translation.Value_En;
                    addedTranslations.Add(translationobj);
                }
                return addedTranslations;
            }
            catch (TaskCanceledException ex)
            {
                throw;
            }

        }

        #endregion

        #region Get Claims Role Rights
        public ClaimsPrincipal GetClaimsFromTokenAsync(string token)
        {
            var ClaimsofValidatedToken = GetPrincipleFromToken(token);
            return ClaimsofValidatedToken;

        }
        #endregion

        #region Refresh Token
        //<History Author = 'Aqeel Altaf Abbasi' Date='2022-02-28' Version="1.0" Branch="master"> to refresh the generated token</History>

        public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken)
        {
            var validateToken = GetPrincipleFromToken(token);
            if (validateToken == null)
            {
                return new AuthenticationResult { Errors = new[] { "Invalid Token" } };
            }

            var expireDateUnix = long.Parse(validateToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);
            var ExpireDateDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expireDateUnix);
            if (ExpireDateDateTimeUtc > DateTime.UtcNow)
            {
                return new AuthenticationResult { Errors = new[] { "This Token hasn't expire Yet" } };
            }

            var jti = validateToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;
            var storeRefreshToken = await _dbContext.TokenModels.SingleOrDefaultAsync(x => x.Token == refreshToken);
            if (storeRefreshToken == null)
            {
                return new AuthenticationResult { Errors = new[] { "This Refresh token does not exist" } };
            }

            if (DateTime.UtcNow > storeRefreshToken.ExpireDate)
            {
                return new AuthenticationResult { Errors = new[] { "This Refresh token has expired" } };
            }
            if (storeRefreshToken.Invalidated)
            {
                return new AuthenticationResult { Errors = new[] { "This Refresh token has Invalidated" } };
            }
            if (storeRefreshToken.Used)
            {
                return new AuthenticationResult { Errors = new[] { "This Refresh token has Used" } };
            }
            if (storeRefreshToken.JwtId != jti)
            {
                return new AuthenticationResult { Errors = new[] { "This Refresh token does not match with this jwt" } };
            }

            storeRefreshToken.Used = true;
            _dbContext.TokenModels.Update(storeRefreshToken);
            await _dbContext.SaveChangesAsync();
            var user = await _userManager.FindByIdAsync(validateToken.Claims.Single(x => x.Type == "id").Value);
            return await GenerateAuthenticationResultByuserAsync(user);


        }
        //<History Author = 'Aqeel Altaf Abbasi' Date='2022-02-28' Version="1.0" Branch="master"> get new token from principle</History>

        private ClaimsPrincipal GetPrincipleFromToken(string token)
        {
            var tokenhandler = new JwtSecurityTokenHandler();
            try
            {
                var principle = tokenhandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                if (!IsjwtWithValidSecurityAlgorithem(validatedToken))
                    return null;
                else
                {
                    return principle;
                }
            }
            catch
            {
                return null;
            }


        }

        private bool IsjwtWithValidSecurityAlgorithem(SecurityToken validateToken)
        {
            return (validateToken is JwtSecurityToken jwtSecurityToken) &&
                    jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                        StringComparison.InvariantCultureIgnoreCase);
        }
        #endregion

        public async Task<List<UserVM>> UserListBySearchTerm(string? searchTerm)
        {
            try
            {
                if (_userList == null)
                {
                    string StoredProc = "exec pLiteratureUserSelBySearchTerm " + "@searchTerm = '" + searchTerm + "'" ;
                    _userList = await _dbContext.UserVM.FromSqlRaw(StoredProc).ToListAsync();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _userList;
        }

        public async Task<List<Group>> UserGroupListBySearchTerm(string? searchTerm)
        {
            try
            {
                if (_userGroupList == null)
                {

                    string StoredProc = "exec pLiteratureUserGroupsSelBySearchTerm " +
                    "@searchTerm = '" + searchTerm + "'," +
                    "@numberOfRows = '" + 20 + "'";
                    _userGroupList = await _dbContext.Groups.FromSqlRaw(StoredProc).ToListAsync();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _userGroupList;
        }

        //<History Author = 'Nadia Gull' Date='2022-11-3' Version="1.0" Branch="master"> returns the list of User Borrow Literatures</History>
        public async Task<List<UserBorrowLiteratureVM>> UserBorrowLiteratures(string? userId)
        {
            try
            {
                if (_borrowLiteratureList == null)
                {
                    string StoredProc = "exec pUserBorrowLiteraturesByUserId @searchTerm = '" + userId + "'";
                    _borrowLiteratureList = await _dbContext.UserBorrowLiteratureVM.FromSqlRaw(StoredProc).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _borrowLiteratureList;
        }

        public async Task<UserPersonalInformationVM> UserDetailByUserId(string userId)
        {
            UserPersonalInformationVM _user = null;
            try
            {
                if (userId != null)
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                    using (_DbContext)
                    {
                        var user = _DbContext.Users.FirstOrDefault(x => x.Id == userId);
                        _user = _mapper.Map<UserPersonalInformationVM>(_DbContext.UserPersonalInformation.FirstOrDefault(x => x.UserId == userId));
                        _user.EligibleCount = user.EligibleCount;
                        _user.UserName = user.UserName;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _user;
        }

        public async Task<UserEmploymentInformation> GetUserEmploymentInfoByUserId(string userId)
        {

            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                using (_DbContext)
                {
                    return _DbContext.UserEmploymentInformation.FirstOrDefault(x => x.UserId == userId);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }


        }

        public async Task<Group> UserGroupListByUserGroupId(Guid userGroupId)
        {
            Group _usergroup = null;
            try
            {
                if (userGroupId != null)
                {
                    using var scope = _serviceScopeFactory.CreateScope();
                    var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                    using (_DbContext)
                    {
                        _usergroup = _DbContext.Groups.FirstOrDefault(x => x.GroupId == userGroupId);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _usergroup;
        }
        //<History Author = 'Nadia Gull' Date='2022-11-3' Version="1.0" Branch="master"> GetSecurityStampByEmail</History>
        public async Task<string> GetSecurityStampByEmail(string emailId)
        {
            string _stamp = "";
            try
            {
                if (emailId != string.Empty)
                {
                    _stamp = _dbContext.Users.FirstOrDefault(x => x.Email == emailId).SecurityStamp;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _stamp;
        }
        #region Get User Roles By UserName

        //<History Author = 'Zain' Date='2022-07-28' Version="1.0" Branch="master">Get UserRoles By UserId</History>  
        public async Task<List<UserRole>> GetUserRolesByUserName(string userName)
        {
            List<UserRole>? userRoles = null;
            if (userName != null)
            {
                var user = _dbContext.Users.Where(u => u.UserName == userName).FirstOrDefault();
                userRoles = await _dbContext.UserRoles.Where(u => u.UserId == user.Id).ToListAsync();
            }

            return userRoles;
        }

        #endregion

        public async Task<string> UserIdByUserEmail(string email)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            using (_DbContext)
            {
                string? _Id = null;
                try
                {
                    var User = _DbContext.Users.FirstOrDefault(x => x.Email == email);
                    if(User != null)
                    {
                        _Id = User.Id;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return _Id;
            }
        }

        public User GetUserByUserEmail(string email)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            using (_DbContext)
            {
                User user;
                try
                {
                    user = _DbContext.Users.FirstOrDefault(x => x.Email == email);
                    user.SectorTypeId = _DbContext.UserEmploymentInformation.Where(x => x.UserId == user.Id).Select(x => x.SectorTypeId).FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return user;
            }
        }
        public async Task<string> UserSectorTypeIdByUserEmail(string email)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            using (_DbContext)
            {
                string? _SectorTypeId = "";
                try
                {
                    var UserId = _DbContext.Users.FirstOrDefault(x => x.Email == email).Id.ToString();
                    _SectorTypeId = _DbContext.UserEmploymentInformation.FirstOrDefault(x => x.UserId == UserId).SectorTypeId.ToString();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return _SectorTypeId;
            }
        }

        public async Task<int> GetSectorIdByEmail(string email)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            using (_DbContext)
            {
                try
                {
                    var UserId = (string)_DbContext.Users.FirstOrDefault(x => x.Email == email).Id;
                    return (int)_DbContext.UserEmploymentInformation.FirstOrDefault(x => x.UserId == UserId).SectorTypeId;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

        private List<IdentityUserVM> identityUsersList;
        //<History Author = 'Zain' Date='2022-07-28' Version="1.0" Branch="master">Get Identity Users By Email</History>   
        public async Task<List<IdentityUserVM>> GetIdentityUsersByEmail(string email)
        {
            try
            {
                if (identityUsersList == null)
                {
                    string StoredProc = "";
                    if (email != null)
                        StoredProc += "exec pIdentityUserSelAll @searchTerm = '" + email + "'";
                    identityUsersList = await _dbContext.IdentityUserVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return identityUsersList;
        }

        //<History Author = 'Zain' Date='2022-07-29' Version="1.0" Branch="master">Check Email Exists</History>   
        public async Task<bool> CheckEmailExists(string email)
        {
            try
            {
                bool isExist = false;
                if (email != null)
                {
                    //var result = await _userManager.FindByEmailAsync(email);
                    var result = await _dbContext.Users.Where(x => x.Email == email && x.IsDeleted == false).FirstOrDefaultAsync();
                    if (result != null)
                        isExist = true;
                }
                return isExist;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region Dropdownlist Functions
        public async Task<List<Module>> GetModules()
        {
            try
            {
                return _dbContext.Modules.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<UserTaskStatus>> GetTasktSatuses()
        {
            try
            {
                return _dbContext.TaskStatuses.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region User CRUD

        private List<UserVM> _usersList;
        //<History Author = 'Zain' Date='2022-07-28' Version="1.0" Branch="master">Get Users List</History>   
        public async Task<List<UserVM>> GetUsersList()
        {
            try
            {
                if (_usersList == null)
                {
                    string StoredProc = "exec pUserSelAll ";
                    _usersList = await _dbContext.UserVM.FromSqlRaw(StoredProc).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _usersList;
        }


        //<History Author = 'Zain' Date='2022-07-28' Version="1.0" Branch="master">Create User</History>  

        private async Task<bool> CreateUMSUser(User user)
        {
            bool isSaved = false;
            try
            {
                IdentityUser identityUser = new IdentityUser
                {
                    Email = user.Email,
                    UserName = user.UserName
                };
                var createUser = await _userManager.CreateAsync(identityUser, user.Password);
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var userDetail = GetIdentityUsersByEmail(user.Email).Result.FirstOrDefault();

                            if (createUser != null && user != null && userDetail != null)
                            {
                                user.Id = userDetail.Id;
                                user.UserName = userDetail.UserName;
                                user.NormalizedUserName = userDetail.NormalizedUserName;
                                user.Email = userDetail.Email;
                                user.NormalizedEmail = userDetail.NormalizedEmail;
                                user.EmailConfirmed = false;
                                user.PasswordHash = userDetail.PasswordHash;
                                user.SecurityStamp = userDetail.SecurityStamp;
                                user.ConcurrencyStamp = userDetail.ConcurrencyStamp;
                                user.LockoutEnd = userDetail.LockoutEnd;
                                user.LockoutEnabled = userDetail.LockoutEnabled;
                                user.AccessFailedCount = userDetail.AccessFailedCount;

                                _dbContext.Entry(user).State = EntityState.Modified;
                                await _dbContext.SaveChangesAsync();

                                //await InsertAttachmentsByUser(user, _dbContext);
                                if (user.RoleIds != null)
                                    await InsertUserRoles(user, _dbContext);
                                if (user.UserClaims != null)
                                    await InsertUserClaims(user, _dbContext);
                                isSaved = true;
                            }
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return isSaved;
        }

        private async Task InsertUserRoles(User user, DatabaseContext dbContext)
        {
            try
            {
                var userRoles = _dbContext.UserRoles.Where(x => x.UserId == user.Id).ToList();
                if (userRoles != null)
                {
                    _dbContext.UserRoles.RemoveRange(userRoles);
                    await _dbContext.SaveChangesAsync();
                }

                foreach (var role in user.RoleIds)
                {
                    UserRole ur = new UserRole()
                    {
                        UserId = user.Id,
                        RoleId = role
                    };

                    await dbContext.UserRoles.AddAsync(ur);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //<History Author = 'Zain' Date='2022-07-28' Version="1.0" Branch="master">Insert User Claims</History>   
        private async Task InsertUserClaims(User user, DatabaseContext dbContext)
        {
            try
            {
                var userClaims = _dbContext.UserClaims.Where(x => x.UserId == user.Id).ToList();
                if (userClaims != null)
                {
                    _dbContext.UserClaims.RemoveRange(userClaims);
                    await _dbContext.SaveChangesAsync();
                }

                foreach (var claim in user.UserClaims)
                {
                    IdentityUserClaim<string> cm = new IdentityUserClaim<string>();
                    cm.UserId = user.Id;
                    cm.ClaimType = claim.ClaimType;
                    cm.ClaimValue = claim.ClaimValue;
                    await _dbContext.UserClaims.AddAsync(cm);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Get User Permissions

        //<History Author = 'Zain' Date='2022-07-20' Version="1.0" Branch="master"> Get list of All Claims both if UserId is null and not null</History>
        public async Task<List<ClaimVM>> GetAllClaims(string userId)
        {
            try
            {
                if (_ClaimVM == null)
                {
                    string StoredProc = "";

                    if (userId == null)
                        StoredProc = "exec pUserClaimsList";
                    else
                        StoredProc = $"exec pUserClaimsList @userId ='{userId}'";

                    _ClaimVM = await _dbContext.ClaimVM.FromSqlRaw(StoredProc).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _ClaimVM;
        }

        #endregion
        private List<FatwaAttendeeVM> _fatwaAttendee;
        //<History Author = 'Zain' Date='2022-07-28' Version="1.0" Branch="master">Get Users List</History>   
        public async Task<List<FatwaAttendeeVM>> GetAttendeeUser()
        {
            try
            {
                if (_fatwaAttendee == null)
                {
                    string StoredProc = "exec pAttendeeSelAll ";
                    _fatwaAttendee = await _dbContext.FatwaAttendeeVM.FromSqlRaw(StoredProc).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _fatwaAttendee;
        }
        public async Task<List<string>> GetUsersByRoleId(string? RoleId)
        {
            List<UserRole> _userrole = new List<UserRole>();

            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                using (_DbContext)
                {
                    _userrole = await _DbContext.UserRoles.Where(x => x.RoleId == RoleId).ToListAsync();
                }
                return _userrole.Select(userRole => userRole.UserId).ToList();
                //return userIds;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //<History Author = 'Attique Rehman' Date='12-jan-2024' Version="1.0" Branch="master"> Change UserPassowrd Funtionality </History>   
        // In release mode, change password in Active Directory (AD) for internal users and in Database (DB) for external users.
        // In development mode, reset password in the Database (DB) for both internal and external employees.
        public async Task<string> ResetUserPasswordAsync(ResetPasswordVM resetPasswordVM)
        {
            using (var transaction = _dbContext.Database.BeginTransaction())
            {
                try
                {
                    if (resetPasswordVM != null)
                    {
#if !DEBUG
                        bool IsPasswordResetByAdmin = true;
                        {
                            if (resetPasswordVM.EmployeeType == (int)EmployeeTypeEnum.Internal)
                            {
                                if (resetPasswordVM.AdUserName == null)
                                {
                                    resetPasswordVM.AdUserName = resetPasswordVM.Email;
                                    IsPasswordResetByAdmin = false;
                                }
                                using (PrincipalContext context = new PrincipalContext(ContextType.Domain, _adSettings.ServerIPAddress, _adSettings.Container, _adSettings.MachineAccountName, _adSettings.MachineAccountPassword))
                                {
                                    UserPrincipal AdUser = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, resetPasswordVM.AdUserName);
                                    if (AdUser != null)
                                    {
                                        AdUser.SetPassword(resetPasswordVM.NewPassword);
                                        AdUser.Save();
                                        var isValidUser = _dbContext.Users.Where(x => x.Email == AdUser.EmailAddress).FirstOrDefault();
                                        if (isValidUser != null)
                                        {
                                            if (IsPasswordResetByAdmin)
                                            {
                                                isValidUser.IsPasswordReset = false;
                                            }
                                            else
                                            {
                                                isValidUser.IsPasswordReset = true;
                                            }
                                            _dbContext.Entry(isValidUser).Property(a => a.IsPasswordReset).IsModified = true;
                                            await _dbContext.SaveChangesAsync();
                                            var passwordHistory = new UserPasswordHistory
                                            {
                                                HistoryId = Guid.NewGuid(),
                                                UserId = isValidUser.Id,
                                                CreatedBy = resetPasswordVM.CreatedBy
                                            };
                                            _dbContext.UserPasswordHistory.Add(passwordHistory);
                                            await _dbContext.SaveChangesAsync();
                                            transaction.Commit();
                                            return resetPasswordVM.AdUserName;
                                        }
                                    }
                                }
                            }
                        }
#endif
                        var user = await _userManager.FindByEmailAsync(resetPasswordVM.Email);
                        if (user != null)
                        {
                            if (resetPasswordVM.OldPassword is null)
                            {    // Change the user's password by Admin --For External Emp
                                string ChangePasswordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                                var changpas = await _userManager.ResetPasswordAsync(user, ChangePasswordToken, resetPasswordVM.NewPassword);
                                if (changpas.Succeeded)
                                {
                                    var isValidUser = _dbContext.Users.Where(x => x.Email == resetPasswordVM.Email).FirstOrDefault();
                                    if (isValidUser != null)
                                    {
                                        isValidUser.IsPasswordReset = false;
                                        _dbContext.Entry(isValidUser).Property(a => a.IsPasswordReset).IsModified = true;
                                        await _dbContext.SaveChangesAsync();
                                        var passwordHistory = new UserPasswordHistory
                                        {
                                            HistoryId = Guid.NewGuid(),
                                            UserId = isValidUser.Id,
                                            CreatedBy = resetPasswordVM.CreatedBy
                                        };
                                        _dbContext.UserPasswordHistory.Add(passwordHistory);
                                        await _dbContext.SaveChangesAsync();
                                        transaction.Commit();
                                        return isValidUser.UserName;
                                    }
                                }
                                else return string.Empty;
                            }
                            // Change the user's password after first login --For External Emp
                            var ChangePassword = await _userManager.ChangePasswordAsync(user, resetPasswordVM.OldPassword, resetPasswordVM.NewPassword);
                            if (ChangePassword.Succeeded)
                            {
                                var isValidUser = _dbContext.Users.Where(x => x.Email == resetPasswordVM.Email).FirstOrDefault();
                                if (isValidUser != null)
                                {
                                    isValidUser.IsPasswordReset = true;
                                    _dbContext.Entry(isValidUser).Property(a => a.IsPasswordReset).IsModified = true;
                                    await _dbContext.SaveChangesAsync();
                                    var passwordHistory = new UserPasswordHistory
                                    {
                                        HistoryId = Guid.NewGuid(),
                                        UserId = isValidUser.Id,
                                        CreatedBy = resetPasswordVM.CreatedBy
                                    };
                                    _dbContext.UserPasswordHistory.Add(passwordHistory);
                                    await _dbContext.SaveChangesAsync();
                                    transaction.Commit();
                                    return isValidUser.UserName;
                                }
                            }
                            else return string.Empty;
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
                return string.Empty;
            }
        }

        #region Get Employee Password History
        //<History Author = 'Ammaar Naveed' Date='2024-01-24' Version="1.0" Branch="master">Getting employee password history for view detail page</History>
        private List<UserPasswordHistory> employeePasswordHistory;
        public async Task<List<UserPasswordHistory>> GetEmployeePasswordHistory(string userId)
        {
            try
            {
                if (userId != null)
                {
                    employeePasswordHistory = await _dbContext.UserPasswordHistory.Where(x => x.UserId == userId).AsNoTracking().ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return employeePasswordHistory;
        }
        #endregion

        #region Get Employee Login/Logout Activities
        //<History Author = 'Ammaar Naveed' Date='2024-03-21' Version="1.0" Branch="master">Gets employee login and logout activities.</History>
        private List<UserActivity> UserActivities;
        public async Task<List<UserActivity>> GetEmployeeActivities(string userId)
        {
            try
            {
                if (userId != null)
                {
                    UserActivities = await _dbContext.UserActivities.Where(x => x.UserId == userId).AsNoTracking().ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return UserActivities;
        }
        #endregion
        #region FCM Notification
        public async Task<dynamic> SaveDeviceRegisteredFCMToken(SaveDeviceTokenVM saveDeviceTokenVM)
        {
            try
            {
                var user = await _dbContext.Users.FindAsync(saveDeviceTokenVM.UserId);
                if (user != null)
                {
                    UmsUserDeviceToken umsUserDeviceToken = new UmsUserDeviceToken()
                    {
                        DeviceToken = saveDeviceTokenVM.DeviceToken,
                        UserId = saveDeviceTokenVM.UserId,
                        CreatedDate = DateTime.Now,
                        CreatedBy = saveDeviceTokenVM.UserId,
                        ChannelId = saveDeviceTokenVM.ChannelId
                    };
                    var response = await _dbContext.UmsUserDeviceToken.AddAsync(umsUserDeviceToken);
                    await _dbContext.SaveChangesAsync();
                    return new ApiCallResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        IsSuccessStatusCode = true,
                        Message = "Token_Registered_Successfully"
                    };
                }
                else
                {
                    return new ApiCallResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccessStatusCode = false,
                        ResultData = null,
                        Message = "User_Identity_not_Found"
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<UmsUserDeviceToken>> GetDeviceRegisteredFCMToken(string userId)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                List<UmsUserDeviceToken> umsUserDeviceToken = new List<UmsUserDeviceToken>();
                if (!string.IsNullOrEmpty(userId))
                {
                    umsUserDeviceToken = await _DbContext.UmsUserDeviceToken.Where(x => x.UserId == userId).ToListAsync();
                }
                return umsUserDeviceToken;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<dynamic> DeleteRegisteredDeviceToken(string token, string userId, int channelId, string cultureValue)
        {
            try
            {
                UmsUserDeviceToken umsUserDeviceToken = new UmsUserDeviceToken();
                umsUserDeviceToken = await _dbContext.UmsUserDeviceToken.Where(x => x.DeviceToken == token && x.UserId == userId && x.ChannelId == channelId).FirstOrDefaultAsync();
                if (umsUserDeviceToken != null)
                {
                    _dbContext.UmsUserDeviceToken.Remove(umsUserDeviceToken);
                    await _dbContext.SaveChangesAsync();
                    return new ApiCallResponse
                    {
                        StatusCode = HttpStatusCode.OK,
                        IsSuccessStatusCode = true,
                        Message = "Token_Deleted_Successfully"
                    };
                }
                else
                {
                    return new ApiCallResponse
                    {
                        StatusCode = HttpStatusCode.NotFound,
                        IsSuccessStatusCode = false,
                        ResultData = null,
                        Message = "Token_userId_Not_Found"
                    };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Committee Memebers By ReferenceId
        private List<FatwaAttendeeVM> _committeeAttendee;
        //<History Author = 'Muhammad Zaeem' Date='2024-07-30' Version="1.0" Branch="master">Get Committee memebers  List</History>   
        public async Task<List<FatwaAttendeeVM>> GetCommitteeMembersByReferenceId(Guid ReferenceId)

        {
            try
            {
                if (_committeeAttendee == null)
                {
                    string StoredProc = $"exec pOC_AttendeeForCommitteeByReferenceId @ReferenceId = '{ReferenceId}'";
                    _committeeAttendee = await _dbContext.FatwaAttendeeVM.FromSqlRaw(StoredProc).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _committeeAttendee;
        }
        #endregion
        #region Get Library Users List
        public async Task<List<UserVM>> GetLegalCulturalCenterUsersList(string? searchTerm)
        {
            try
            {
                if (_userList == null)
                {
                    string StoredProc = "exec pGetLibraryUsersListByRole " + "@searchTerm = '" + searchTerm + "'," + "@sectortype ='" + ((int)OperatingSectorTypeEnum.LegalCulturalCenter) + "' ";
                    _userList = await _dbContext.UserVM.FromSqlRaw(StoredProc).ToListAsync();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _userList;
        }

        #endregion
        #region Get Library Users List
        public async Task<List<UserVM>> GetLibraryUsersList(string? searchTerm)
        {
            try
            {
                if (_userList == null)
                {
                    string StoredProc = "exec pGetLibraryUsersListByRole " + "@searchTerm = '" + searchTerm + "'";
                    _userList = await _dbContext.UserVM.FromSqlRaw(StoredProc).ToListAsync();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _userList;
        }

        #endregion
    }
}
