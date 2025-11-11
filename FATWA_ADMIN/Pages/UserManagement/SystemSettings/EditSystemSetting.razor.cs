using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.Dms;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

//<History Author = 'Umer Zaman' Date='2022-09-10' Version="1.0" Branch="master"> To manage system setting operation</History>

namespace FATWA_ADMIN.Pages.UserManagement.SystemSettings
{
    public partial class EditSystemSetting : ComponentBase
    {

        #region Variables
        public SystemSetting systemSetting = new SystemSetting();
        protected IEnumerable<DmsFileTypes> fileTypes { get; set; } = new List<DmsFileTypes>();
        protected RadzenDataGrid<DmsFileTypes>? grid;
        public bool allowRowSelectOnRowClick = true;



        #endregion

        #region On Component Load
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        //<History Author = 'Umer Zaman' Date='2022-07-26' Version="1.0" Branch="master"> get user details operation</History>
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateFileTypes();
            await GetSystemSetting();
            if (systemSetting.FileTypes != string.Empty)
            {
                await SplitTypes();
            }
            spinnerService.Hide();
        }


        protected async Task GetSystemSetting()
        {
            var response = await systemSettingStateService.GetSystemSetting();
            if (response.IsSuccessStatusCode)
            {
                systemSetting = (SystemSetting)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

        #region Populate File Types
        protected async Task PopulateFileTypes()
        {
            var response = await fileUploadService.GetFileTypes();
            if (response.IsSuccessStatusCode)
            {
                var fileTypesList = (List<DmsFileTypes>)response.ResultData;
                fileTypes = fileTypesList.AsEnumerable();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task SplitTypes()
        {

            foreach (string fileType in systemSetting.FileTypes.Split(','))
            {

                systemSetting.SelectedTypesIdList.Add(fileType.Trim());

            }
            foreach (var selectedTypes in systemSetting.SelectedTypesIdList)
            {
                var dmsSelectedType = fileTypes.Where(x => x.Type == selectedTypes).FirstOrDefault();
                if (dmsSelectedType != null)
                {
                    systemSetting.DmsFileTypesSelectedList.Add(dmsSelectedType);
                    grid.SelectRow(dmsSelectedType);
                }

            }
            systemSetting.SelectedTypesIdList = new List<string>();
        }
        #endregion

        #region Redirect Functions
        protected void ButtonCancelClick(MouseEventArgs args)
        {
            navigationManager.NavigateTo("/index");
        }

        public void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion

        #region Form Submit
        protected async Task FormSubmit()
        {
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
                @translationState.Translate("Setting_Confirmation_Message"),
                @translationState.Translate("Confirm"),
                new ConfirmOptions()
                {
                    OkButtonText = @translationState.Translate("Save"),
                    CancelButtonText = @translationState.Translate("Cancel")
                });

                if (dialogResponse == true)
                {
                    spinnerService.Show();
                    foreach (var dmsSelectedTypes in systemSetting.DmsFileTypesSelectedList)
                    {
                        systemSetting.SelectedTypesIdList.Add(dmsSelectedTypes.Type);
                    }
                    if (systemSetting.SelectedTypesIdList != null && systemSetting.SelectedTypesIdList.Any())
                    {
                        systemSetting.FileTypes = string.Join(',', fileTypes.Where(x => systemSetting.SelectedTypesIdList.Contains(x.Type)).Select(y => y.Type));
                        var response = await systemSettingStateService.UpdateSystemSetting(systemSetting);
                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Setting_Update_Success_Messsage"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            await OnInitializedAsync();
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }
                    }
                    else
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Select_Atleast_One_File_Type"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                    }
                    spinnerService.Hide();
                    Reload();
                }

            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Contact_Administrator"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion
    }
}
