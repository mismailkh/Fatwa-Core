using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Telerik.Blazor;
using Telerik.Blazor.Components;
using static FATWA_GENERAL.Helper.Request;
using static FATWA_GENERAL.Helper.Response;
using FATWA_DOMAIN.Common;
using System.DirectoryServices.AccountManagement;

namespace DMS_WEB.Pages
{

    //<History Author = 'Umer Zaman' Date='2022-08-17' Version="1.0" Branch="master"> Recreate class by usinng based component approach</History>
    public partial class LogIn : ComponentBase
    {
        string SelectedLoaderSize { get; set; } = ThemeConstants.Loader.Size.Medium;
        string ThemeColor { get; set; } = "primary";
        string OverlayThemeColor { get; set; } = "dark";
        string LoaderContainerText { get; set; }
        string isLoginFailed { get; set; } = "none";
        string loginFailedMessage { get; set; } = "";
        string errorMessage = "";


        LoaderType SelectedLoaderType { get; set; } = LoaderType.InfiniteSpinner;
        LoaderPosition SelectedLoaderPosition { get; set; } = LoaderPosition.Top;

        public bool LoaderContainerVisible { get; set; } = false;
        TelerikNotification notificationComponent { get; set; }
        IdentityRequest identityUser = new IdentityRequest();

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
                navigationManager.NavigateTo("dashboard");
            }
            else
            {
                ValidSubmit = false;
            }
        }

        protected override async Task OnInitializedAsync()
        {
            if (_config["Environment"] != "QA")
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

        protected async Task HandleValidSubmit()
        {
            isLoginFailed = "none";
            LoaderContainerVisible = true;
            ValidSubmit = true;
            await OnLogin();
            StateHasChanged();
        }


        void HandleInvalidSubmit()
        {
            ValidSubmit = false;
        }

        protected async Task OnSingleSignOn(string SamAccountName)
        {
            try
            {
                var user = (await BrowserStorage.GetItemAsync<string>("User")) ?? "";
                if (string.IsNullOrEmpty(user))
                {
                    var response = await new HttpClient().PostAsJsonAsync(_config["fatwa_api_url"] + "/Account/SingleSignOn", SamAccountName == null ? "" : SamAccountName);
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
                        loginState.SetLoginAndClaims(data.User.UserName, data.UserDetail, true, true, data.UserClaims, data.Token, data.RefreshToken, "LoginForm", data.ProfilePicUrl);
				        loginState.IsSSOAthenticated = true;
                        apiResponse = await userService.GetUserRoles(loginState.Username);
                        if (apiResponse.IsSuccessStatusCode)
                        {
                            loginState.UserRoles = (List<UserRole>)apiResponse.ResultData;
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
        protected async Task OnLogin()
        {
            try
            {
                var user = (await BrowserStorage.GetItemAsync<string>("User")) ?? "";
                if (string.IsNullOrEmpty(user))
                {
                    var response = await new HttpClient().PostAsJsonAsync(_config["fatwa_api_url"] + "/Account/Login", identityUser);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadFromJsonAsync<UserSucessResponse>();

                        #region System Configuration Login Validations
      //                  var responseUser = await systemConfigurationService.GetUserDetailByUsingEmail(data.UserDetail.Email);

						//if (responseUser.IsSuccessStatusCode)
						//{
						//	UserNameResult = (User)responseUser.ResultData;
						//}
						//if (UserNameResult != null)
						//{
                            var responseGroup = await systemConfigurationService.GetUserGroupDetailByUsingGroupId();
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
                                await BrowserStorage.SetItemAsync("SessionTimeout", UserGroupDetail.Session_Timeout_Period);
                            }
                        //}
                        #endregion


                        await BrowserStorage.SetItemAsync("Token", data.Token);
                        await BrowserStorage.SetItemAsync("RefreshToken", data.RefreshToken);
                        await BrowserStorage.SetItemAsync("User", data.User.Email);
                        await BrowserStorage.SetItemAsync("SecurityStamp", data.UserDetail.SecurityStamp);
                        await BrowserStorage.SetItemAsync("UserDetail", data.UserDetail);
                        await BrowserStorage.SetItemAsync("ProfilePicUrl", data.ProfilePicUrl);

                        var apiResponse = await roleService.GetAllTranslations();
                        if (apiResponse.IsSuccessStatusCode)
                        {
                            translationState.TranslationList = (List<TranslationSucessResponse>)apiResponse.ResultData;
                        }

                        loginState.SetLoginAndClaims(data.User.UserName, data.UserDetail, true, true, data.UserClaims, data.Token, data.RefreshToken, "LoginForm", data.ProfilePicUrl);

                        apiResponse = await userService.GetUserRoles(loginState.Username);
                        if (apiResponse.IsSuccessStatusCode)
                        {
                            loginState.UserRoles = (List<UserRole>)apiResponse.ResultData;
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
                        navigationManager.NavigateTo("dashboard");
                    }
                    else
                    {
                        #region System Configuration Login Validations
                        var responseUser = await systemConfigurationService.GetUserDetailByUsingEmailForPasswordCheck(identityUser.UserName);
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
                            var responseAccessCount = await systemConfigurationService.UserAccountAccessFailCount(UserNameResult.Email, WrongPasswordAttemptsCount);
                            if (responseAccessCount.IsSuccessStatusCode)
                            {

							}
                            var responseGroup = await systemConfigurationService.GetUserGroupDetailByUsingGroupId();
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
                                var obj = UserGroupDetail.Session_Timeout_Period;
                                await BrowserStorage.SetItemAsync("SessionTimeout", obj);
                                if (WrongPasswordAttemptsCount == UserGroupDetail.Wrong_Password_Attempts)
                                {
                                    var responseResult = await systemConfigurationService.LockUserAccount(UserNameResult.Email);
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

                        LoaderContainerVisible = false;
                        ValidSubmit = false;
                        var data1 = await response.Content.ReadFromJsonAsync<RequestFailedResponse>();
                        if (data1 != null)
                        {
                            var errors = data1.Errors;
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
                    LoaderContainerVisible = false;
                    navigationManager.NavigateTo("dashboard");
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
    }
}
