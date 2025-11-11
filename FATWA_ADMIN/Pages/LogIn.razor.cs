using FATWA_ADMIN.Pages.UserManagement.Users;
using FATWA_ADMIN.Services.UserManagement;
using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using System.DirectoryServices.AccountManagement;
using Telerik.Blazor;
using Telerik.Blazor.Components;
using static FATWA_GENERAL.Helper.Request;
using static FATWA_GENERAL.Helper.Response;
using Microsoft.AspNetCore.Authorization;


namespace FATWA_ADMIN.Pages
{

    //<History Author = 'Umer Zaman' Date='2022-08-17' Version="1.0" Branch="master"> Recreate class by usinng based component approach</History>
    public partial class LogIn : ComponentBase
    {
        #region Inject


        [Inject]
        protected RoleService roleService { get; set; }

        [Inject]
        protected IJSRuntime JsInterop { get; set; }

        [Inject]
        protected SystemConfigurationService _systemConfigurationServices { get; set; }

        #endregion

        public string SelectedLoaderSize { get; set; } = ThemeConstants.Loader.Size.Medium;
        public string ThemeColor { get; set; } = "primary";
        public string OverlayThemeColor { get; set; } = "dark";
        public string LoaderContainerText { get; set; }
        public string isLoginFailed { get; set; } = "none";
        public string loginFailedMessage { get; set; } = "";
        public bool HidePassword { get; set; } = true;

        public string errorMessage = "";

        public LoaderType SelectedLoaderType { get; set; } = LoaderType.InfiniteSpinner;
        public LoaderPosition SelectedLoaderPosition { get; set; } = LoaderPosition.Top;

        public bool LoaderContainerVisible { get; set; } = false;
        public TelerikNotification notificationComponent { get; set; }
        public IdentityRequest identityUser = new IdentityRequest();

        public bool ValidSubmit { get; set; } = true;

        #region Login validate
        protected User UserNameResult { get; set; } = new User();
        protected SystemConfiguration UserGroupDetail { get; set; } = new SystemConfiguration();
        public int WrongPasswordAttemptsCount { get; private set; } = 0;
        public bool UserLoginFormLock { get; private set; }
        #endregion

        protected override void OnParametersSet()
        {
            ValidSubmit = true;
            if (loginState.IsLoggedIn)
            {
                navigationManager.NavigateTo("Index");
            }
            else
            {
                ValidSubmit = false;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            if (_config["Environment"] != "QA" && _config["Environment"] != "DPS")
            {
                var authState = await GetAuthenticationStateAsync.GetAuthenticationStateAsync();
                var user = authState.User;
                var fullName = user.Identity.Name;
                var friendlyName = fullName.Split('\\').FirstOrDefault() ?? fullName;
                var domainName = CommonUtils.GetDomainFullName(friendlyName);
                if (domainName == _config["DomainName"])
                {
                    var username = fullName.Split('\\').LastOrDefault() ?? fullName;
                    PrincipalContext context = new PrincipalContext(ContextType.Domain);
                    UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(context, username);
                    Console.WriteLine(userPrincipal.SamAccountName);
                    await OnSingleSignOn(userPrincipal.SamAccountName);
                }
                else
                {
                    LoaderContainerText = translationState.Translate("Loading");
                    isLoginFailed = "none";
                    ValidSubmit = false;
                }
            }
            else
            {
                LoaderContainerText = translationState.Translate("Loading");
                isLoginFailed = "none";
                ValidSubmit = false;
            }
        }

        protected async void HandleValidSubmit()
        {
            isLoginFailed = "none";
            LoaderContainerVisible = true;
            ValidSubmit = true;
            await OnLogin();
            StateHasChanged();
        }


        protected void HandleInvalidSubmit()
        {
            ValidSubmit = false;
        }

        #region Reveal Password Icon
        //<History Author = 'Ammaar Naveed' Date='2024-01-02' Version="" Branch="master"> RevealPassword eye button/History>
        public async Task RevealPassword()
        {
            HidePassword = false;
            await Task.Delay(700);
            HidePassword = true;
        }
        #endregion

        #region Removing White Spaces In Input
        //Removing white spaces from the entered email for improved user experience
        private void TrimEmail()
        {
            if (!string.IsNullOrEmpty(identityUser.UserName))
                identityUser.UserName = identityUser.UserName.Trim();
        }
        #endregion

        #region ForgetPassword Dialog
        public async Task forgetPassword()
        {
            await dialogService.OpenAsync<ForgetPasswordDialog>((Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? "Forgot Password? | نسيان كلمة المرور؟" : "?Forgot Password | نسيان كلمة المرور؟"),
                 new Dictionary<string, object>() { },
                 new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = false });
        }
        #endregion

        protected async Task OnSingleSignOn(string SamAccountName)
        {
            try
            {
                var user = (await BrowserStorage.GetItemAsync<string>("User")) ?? "";
                if (string.IsNullOrEmpty(user))
                {
                    var response = await new HttpClient().PostAsJsonAsync(_config["api_url"] + "/Account/SingleSignOn", SamAccountName == null ? "" : SamAccountName);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadFromJsonAsync<UserSucessResponse>();
                        await BrowserStorage.SetItemAsync("Token", data.Token);
                        await BrowserStorage.SetItemAsync("RefreshToken", data.RefreshToken);
                        await BrowserStorage.SetItemAsync("User", data.User.Email);
                        await BrowserStorage.SetItemAsync("SecurityStamp", data.UserDetail.SecurityStamp);
                        await BrowserStorage.SetItemAsync("UserDetail", data.UserDetail);
                        await BrowserStorage.SetItemAsync("ProfilePicUrl", data.ProfilePicUrl);
                        await BrowserStorage.SetItemAsync("IsSSOAuthenticated", true);
                        var apiResponse = await roleService.GetAllTranslations();
                        if (apiResponse.IsSuccessStatusCode)
                        {
                            translationState.TranslationList = (List<TranslationSucessResponse>)apiResponse.ResultData;
                        }
                        loginState.SetLoginAndClaims(data.User.UserName, data.UserDetail, true, true, data.UserClaims, data.Token, data.RefreshToken, "LoginForm");
                        loginState.IsSSOAthenticated = true;
                        LoaderContainerVisible = false;
                        await JsInterop.InvokeVoidAsync("initializeTimerOnLogin");
                        navigationManager.NavigateTo("dashboard");
                    }
                    else
                    {
                        LoaderContainerVisible = false;
                        ValidSubmit = false;
                        isLoginFailed = "block";
                        var data1 = await response.Content.ReadFromJsonAsync<RequestFailedResponse>();
                        if (data1 != null)
                        {
                            var errors = data1.Errors;
                            if (errors != null && errors.Any())
                            {
                                errorMessage = errors.FirstOrDefault();
                            }
                            else
                            {
                                errorMessage = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? "Login Failed" : "لم يتم تسجيل الدخول";
                            }
                        }
                    }
                }
                else
                {
                    LoaderContainerVisible = false;
                    navigationManager.NavigateTo("dashboard");
                }
            }
            catch (HttpRequestException)
            {
                errorMessage = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? "Login Failed" : "لم يتم تسجيل الدخول";
                LoaderContainerVisible = false;
                isLoginFailed = "block";
                ValidSubmit = false;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-02-28' Version="1.0" Branch="master"> Initialize Login State, Claims and Tokens on Login</History>
        //<History Author = 'Hassan Abbas' Date='2022-03-15' Version="2.0" Branch="master"> Change body styling</History>
        //<History Author = 'Hassan Abbas' Date='2022-03-16' Version="3.0" Branch="master"> Added Loader on Login and changed events hierarchy for smooth layout change, keep Username on login state</History>
        //<History Author = 'Hassan Abbas' Date='2022-07-06' Version="3.0" Branch="master"> Added User in browser storage and 'initializeTimerOnLogin' on login to track inactivity for session out and changes for Service Injection as Scoped</History>
        //<History Author = 'Hassan Abbas' Date='2022-07-09' Version="3.0" Branch="master"> populate translation list in new state which is Translation State</History>
        //<History Author = 'Aqeel Altaf' Date='2022-08-09' Version="3.0" Branch="master"> Add security stamp for force logout</History>
        //<History Author = 'Umer Zaman' Date='2022-08-17' Version="3.0" Branch="master"> Handle system configuration implementation</History>
        //<History Author = 'Attique Rehman' Date='2024-01-11' Version="3.0" Branch="master">AdminPortal Access for authorized person</History>
        protected async Task OnLogin()
        {
            try
            {
                var user = (await BrowserStorage.GetItemAsync<string>("User")) ?? "";
                if (String.IsNullOrEmpty(user))
                {
                    var response = await new HttpClient().PostAsJsonAsync(_config["api_url"] + "/Account/Login", identityUser);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadFromJsonAsync<UserSucessResponse>();

                        // a user without admin permission can't be enter into admin portal
                        if (!data.UserClaims.Any(x => x.Value.Contains("Admin")))
                        {
                            LoaderContainerVisible = false;
                            isLoginFailed = "block";
                            errorMessage = "Unauthorized Access / غير مصرح بالدخول";
                            ValidSubmit = false;
                            return;
                        }
                        #region Force Password Change on First Attemp Login
                        if (!(bool)data.UserDetail.IsPasswordReset)
                        {
                            var resPasswordChange = await FirstAttempLoginPasswordChange(data);
                            if (!resPasswordChange) return; return;// redirect to login page rather the password change or not
                        }
                        #endregion

                        #region System Configuration Login Validations
                        //var responseUser = await _systemConfigurationServices.GetUserDetailByUsingEmail(data.UserDetail.Email);

                        //if (responseUser.IsSuccessStatusCode)
                        //{
                        //	UserNameResult = (User)responseUser.ResultData;
                        //}
                        //if (UserNameResult != null)
                        //{

                        await BrowserStorage.SetItemAsync("SessionTimeout", _config.GetValue<int>("SessionTimeout"));
                        //}
                        #endregion

                        await BrowserStorage.SetItemAsync("Token", data.Token);
                        await BrowserStorage.SetItemAsync("RefreshToken", data.RefreshToken);
                        await BrowserStorage.SetItemAsync("User", data.User.Email);
                        await BrowserStorage.SetItemAsync("SecurityStamp", data.UserDetail.SecurityStamp);
                        await BrowserStorage.SetItemAsync("UserDetail", data.UserDetail);

                        var apiCallResponse = await roleService.GetAllTranslations();
                        if (apiCallResponse.IsSuccessStatusCode)
                        {
                            translationState.TranslationList = (List<TranslationSucessResponse>)apiCallResponse.ResultData;
                        }
                        else
                        {
                            translationState.TranslationList = new List<TranslationSucessResponse>();
                        }

                        loginState.SetLoginAndClaims(data.User.UserName, data.UserDetail, true, true, data.UserClaims, data.Token, data.RefreshToken, "LoginForm");
                        apiCallResponse = await userService.GetUserRoles(loginState.Username);
                        if (apiCallResponse.IsSuccessStatusCode)
                        {
                            loginState.UserRoles = (List<UserRole>)apiCallResponse.ResultData;
                        }
                        else
                        {
                            loginState.ClaimList = new List<ClaimSucessResponse>();
                            DeleteBrowserStorageValues();
                            loginState.IsLoggedIn = false;
                            loginState.IsStateChecked = true;
                            spinnerService.Hide();
                            return;
                        }
                        LoaderContainerVisible = false;
                        await JsInterop.InvokeVoidAsync("initializeTimerOnLogin");
                        await JsInterop.InvokeVoidAsync("RevertBodyStyle");
                        navigationManager.NavigateTo("Index");
                    }
                    else
                    {
                        #region System Configuration Login Validations
                        var responseUser = await _systemConfigurationServices.GetUserDetailByUsingEmailForPasswordCheck(identityUser.UserName);
                        if (responseUser.StatusCode == System.Net.HttpStatusCode.NoContent)
                        {
                            UserNameResult = null;
                        }
                        else if (responseUser.IsSuccessStatusCode)
                        {
                            UserNameResult = (User)responseUser.ResultData;
                        }
                        else
                        {
                            UserNameResult = null;
                        }
                        if (UserNameResult != null)
                        {
                            WrongPasswordAttemptsCount++;
                            var responseAccessCount = await _systemConfigurationServices.UserAccountAccessFailCount(UserNameResult.Email, WrongPasswordAttemptsCount);
                            if (responseAccessCount.IsSuccessStatusCode)
                            {

                            }
                            var responseGroup = await _systemConfigurationServices.GetUserGroupDetailByUsingGroupId();
                            if (responseGroup.StatusCode == System.Net.HttpStatusCode.NoContent)
                            {
                                UserGroupDetail = null;
                            }
                            else if (responseGroup.IsSuccessStatusCode)
                            {
                                UserGroupDetail = (SystemConfiguration)responseGroup.ResultData;
                            }
                            else
                            {
                                UserGroupDetail = null;
                            }
                            if (UserGroupDetail != null)
                            {
                                //var obj = UserGroupDetail.Session_Timeout_Period;
                                await BrowserStorage.SetItemAsync("SessionTimeout", _config.GetValue<int>("SessionTimeout"));
                                if (WrongPasswordAttemptsCount == UserGroupDetail.Wrong_Password_Attempts)
                                {
                                    var responseResult = await _systemConfigurationServices.LockUserAccount(UserNameResult.Email);
                                    if (responseResult.IsSuccessStatusCode)
                                    {
                                        UserLoginFormLock = true;
                                    }
                                }

                            }
                        }
                        else
                        {
                        }
                        #endregion
                        //var emailResult = GetUSerDetailByUsingEmail(identityUser.Email);
                        LoaderContainerVisible = false;
                        ValidSubmit = false;
                        var data = await response.Content.ReadFromJsonAsync<RequestFailedResponse>();
                        if (data != null)
                        {
                            var errors = data.Errors;
                            if (errors != null && errors.Any())
                            {
                                errorMessage = errors.FirstOrDefault();
                                isLoginFailed = "block";
                                loginFailedMessage = errorMessage;
                            }
                            else
                            {

                                if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                                {
                                    LoaderContainerVisible = false;
                                    isLoginFailed = "block";
                                    errorMessage = "Login Failed";
                                    loginFailedMessage = errorMessage;
                                    ValidSubmit = false;
                                }
                                else
                                {
                                    LoaderContainerVisible = false;
                                    isLoginFailed = "block";
                                    errorMessage = "لم يتم تسجيل الدخول";
                                    loginFailedMessage = errorMessage;
                                    ValidSubmit = false;
                                }

                            }
                        }
                    }
                }
                else
                {
                    await JsInterop.InvokeVoidAsync("RevertBodyStyle");
                    LoaderContainerVisible = false;
                    navigationManager.NavigateTo("Index");
                }
            }
            catch (HttpRequestException)
            {
                if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                {
                    LoaderContainerVisible = false;
                    isLoginFailed = "block";
                    errorMessage = "Login Failed";
                    loginFailedMessage = errorMessage;
                    ValidSubmit = false;
                }
                else
                {
                    LoaderContainerVisible = false;
                    isLoginFailed = "block";
                    errorMessage = "لم يتم تسجيل الدخول";
                    loginFailedMessage = errorMessage;
                    ValidSubmit = false;
                }

            }

        }
        protected async void DeleteBrowserStorageValues()
        {
            try
            {
                await BrowserStorage.RemoveItemAsync("User");
                await BrowserStorage.RemoveItemAsync("Token");
                await BrowserStorage.RemoveItemAsync("RefreshToken");
                await BrowserStorage.RemoveItemAsync("ProfilePicUrl");
                await BrowserStorage.RemoveItemAsync("SecurityStamp");
                await BrowserStorage.RemoveItemAsync("SessionTimeout");
            }
            catch (JSDisconnectedException ex)
            {

            }
        }
        //<History Author = 'Attique Rehman' Date='2024-01-19' Version="1.0" Branch="master">First Attempt login , Force user Password Change</History>
        protected async Task<bool> FirstAttempLoginPasswordChange(UserSucessResponse data)
        {
            var result = await dialogService.OpenAsync<ResetPassword>
                               //"Reset Password / إعادة تعيين كلمة المرور ",
                               (translationState.Translate("Reset_Password"),
                              new Dictionary<string, object>
                              {
                             { "UserOldPassword",identityUser.Password}
                              },
                               new DialogOptions() { Width = "31% !important", CloseDialogOnOverlayClick = false });

            if (result != null)
            {
                ResetPasswordVM ResetPasswordBody = new ResetPasswordVM
                {
                    Email = identityUser.UserName,
                    OldPassword = identityUser.Password,
                    NewPassword = result.Password,
                    EmployeeType = data.UserDetail?.UserTypeId,
                    CreatedBy = data.UserDetail?.Email
                };
                var responsePasswordChange = await new HttpClient().PostAsJsonAsync(_config["api_url"] + "/Account/ResetUserPassword", ResetPasswordBody);
                if (!responsePasswordChange.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = "Something went wrong, please try again \n لقد حدث خطأ، حاول مرة أخرى",
                        Style = "position: fixed !important; left: 0; margin: auto; white-space: pre-line;",
                        Duration = 3000
                    });
                    return false;
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = "Password changed successfully. Kindly re-login with the new password \n تم تغيير كلمة المرور، قم بإعادة تسجيل الدخول بكلمة المرور الجديدة",
                        Style = "position: fixed !important; left: 0; margin: auto; text-align: center;",
                        Duration = 3000
                    });
                    identityUser.Password = string.Empty;
                    ValidSubmit = false;
                    LoaderContainerVisible = false;
                    return true;
                }
            }
            else
            {
                ValidSubmit = false;
                LoaderContainerVisible = false;
                return false;
            }

        }
    }
}
