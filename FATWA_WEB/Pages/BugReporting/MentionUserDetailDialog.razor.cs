using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Components;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.BugReporting
{
    public partial class MentionUserDetailDialog : ComponentBase
    {
        #region Parameter
        [Parameter]
        public dynamic UserId { get; set; }
        #endregion
        #region Variable Declaration
        public AddEmployeeVM userDetail { get; set; } = new AddEmployeeVM();
        #endregion
        #region On Initilized Async 
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            spinnerService.Hide();
        }
        protected async Task Load()
        {
            await PopulateUserDetailById();
        }
        #endregion
        #region Populate User Detail
        protected async Task PopulateUserDetailById()
        {
            try
            {
                var response = await userService.GetEmployeeDetailById(Guid.Parse(UserId));
                if (response.IsSuccessStatusCode)
                {
                    userDetail = (AddEmployeeVM)response.ResultData;
                }
                else
                {
                    await ReturnBadRequestNotification(response);
                }
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion
        #region ReturnBadRequestNotification
        protected async Task ReturnBadRequestNotification(ApiCallResponse response)
        {
            try
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Token_Expired"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    await Task.Delay(5000);
                    await BrowserStorage.RemoveItemAsync("User");
                    await BrowserStorage.RemoveItemAsync("Token");
                    await BrowserStorage.RemoveItemAsync("RefreshToken");
                    await BrowserStorage.RemoveItemAsync("UserDetail");
                    await BrowserStorage.RemoveItemAsync("SecurityStamp");
                    loginState.IsLoggedIn = false;
                    loginState.IsStateChecked = true;
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion
    }
}
