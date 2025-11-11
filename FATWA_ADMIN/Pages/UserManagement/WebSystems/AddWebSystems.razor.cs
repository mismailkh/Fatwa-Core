using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;

namespace FATWA_ADMIN.Pages.UserManagement.WebSystems
{
    public partial class AddWebSystems : ComponentBase
    {
        #region Paramters

        [Parameter]
        public dynamic Id { get; set; }
        #endregion

        #region Variables
        public bool CreateAnother { get; set; } = false;

        WebSystem WebSystem = new WebSystem();
        #endregion        

        #region On Component Load

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            spinnerService.Hide();
        }
        private async Task Load()
        {
            if (Id != null)
            {
                var response = await groupService.GetWebSystemsById(Id);
                if (response.IsSuccessStatusCode)
                {
                    WebSystem = (WebSystem)response.ResultData;
                    StateHasChanged();
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
        }
        #endregion

        #region Button Events
        protected async Task CancelChanges()
        {
            dialogService.Close(false);
        }
        protected async Task SaveChanges()
        {
            if (await dialogService.Confirm(
                        translationState.Translate("Sure_Save_Changes"),
                        translationState.Translate("Confirm"),
                        new ConfirmOptions()
                        {
                            OkButtonText = @translationState.Translate("OK"),
                            CancelButtonText = @translationState.Translate("Cancel")
                        }) == true)
            {
                spinnerService.Show();
                if (Id == null)
                {
                    WebSystem.CreatedBy = loginState.Username;
                    WebSystem.CreatedDate = DateTime.Now;
                    var response = await groupService.SaveWebSystems(WebSystem);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("WebSystems_Added_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        await Task.Delay(2000);
                        if (CreateAnother)
                        {
                            await jSRuntime.InvokeVoidAsync("window.location.reload");
                        }
                        else
                        {
                            await jSRuntime.InvokeVoidAsync("history.back");
                        }
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                }
                else
                {
                    WebSystem.ModifiedBy = loginState.Username;
                    WebSystem.ModifiedDate = DateTime.Now;
                    var response = await groupService.UpdateWebSystems(WebSystem);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("WebSystems_Updated_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                }
                spinnerService.Hide();
                dialogService.Close(true);
                StateHasChanged();
            }
        }

        #endregion

        #region Toggle Create Another
        protected async Task ToggleCreateAnother()
        {
            // if it's true, set it to false and vice versa
            CreateAnother = !CreateAnother;
        }

        #endregion

        #region Redirect Functions
        public void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        public void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion
    }
}
