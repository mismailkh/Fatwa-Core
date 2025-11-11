using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using static FATWA_GENERAL.Helper.Response;
using FATWA_ADMIN.Data;
using System.Text.RegularExpressions;

namespace FATWA_ADMIN.Pages.UserManagement.GroupAccessType
{
    public partial class AddGroupType : ComponentBase
    {
        #region Paramters

        [Parameter]
        public string? Id { get; set; }
        #endregion

        #region Variables
        protected IEnumerable<WebSystem> websystemList { get; set; }
        public IEnumerable<int> GroupAccessTypeId { get; set; }
        GroupAccessTypeVM groupAccessTypeVM = new GroupAccessTypeVM();
        #endregion

        #region Redirect Function
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion
       
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateWebSystemDropDown();
            await Load();
            spinnerService.Hide();

        }

        private async Task Load()
        {
            if (Id != null)
            {
                var response = await groupService.GetGroupTypeById(int.Parse(Id));
                if (response.IsSuccessStatusCode)
                {
                    groupAccessTypeVM = (GroupAccessTypeVM)response.ResultData;
                    StateHasChanged();
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
        }

        #region Populate DropDown
        public async Task PopulateWebSystemDropDown()
        {
            var response = await groupService.GetWebSystems();
            if (response.IsSuccessStatusCode)
            {
                websystemList = (IEnumerable<WebSystem>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();

        }
        #endregion

        #region Button Events
        protected async Task CancelChanges()
        {
           dialogService.Close(false);
        }
        protected async Task SaveChanges()
        {
            if (groupAccessTypeVM.SelectedIdz != null && groupAccessTypeVM.SelectedIdz.Count() > 0)
            {

                bool? dialogResponse = await dialogService.Confirm(
                    translationState.Translate("Sure_Save_Group_Type"),
                    translationState.Translate("Confirm"),
                    new ConfirmOptions()
                    {
                        OkButtonText = @translationState.Translate("OK"),
                        CancelButtonText = @translationState.Translate("Cancel")
                    });
                if (dialogResponse == true)
                {
                    spinnerService.Show();
                    if (Id == null)
                    {
                        groupAccessTypeVM.CreatedBy = loginState.Username;
                        groupAccessTypeVM.CreatedDate = DateTime.Now;
                        var response = await groupService.CreateGroupAccessType(groupAccessTypeVM);
                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("GroupAccessType_Added_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            await Task.Delay(2000);

                            await jSRuntime.InvokeVoidAsync("history.back");

                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }
                    }
                    else
                    {
                        groupAccessTypeVM.GroupTypeId = int.Parse(Id);
                        groupAccessTypeVM.ModifiedBy = loginState.Username;
                        groupAccessTypeVM.ModifiedDate = DateTime.Now;
                        var response = await groupService.UpdateGroupAccessType(groupAccessTypeVM);
                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("GroupAccessType_Updated_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            await Task.Delay(2000);

                            await jSRuntime.InvokeVoidAsync("history.back");

                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }
                    }
                    spinnerService.Hide();
                }
            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("WebSystems_Required"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }

        }

        #endregion

    }
}
