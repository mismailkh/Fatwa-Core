using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.Dms;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Linq;
using static FATWA_GENERAL.Helper.Response;

namespace DMS_WEB.Pages.DocumentManagement
{
    public partial class ConfigureFileUploadAttributes : ComponentBase
    {
        #region Variable Declaration 
        protected IEnumerable<DmsFileTypes> fileTypes { get; set; } = new List<DmsFileTypes>();
        public SystemSetting systemSetting { get; set; } = new SystemSetting();
        public string fileTypesString { get; set; }
        public string[] fileTypesArray { get; set; }
        protected RadzenDataGrid<DmsFileTypes>? grid;
        public bool allowRowSelectOnRowClick = true;



        #endregion

        #region  OnInitialized
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateFileTypes();
            await PopulateSystemSetting();
            if (systemSetting.FileTypes != string.Empty)
            {
                await SplitTypes();
            }
            spinnerService.Hide();
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
                await ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task PopulateSystemSetting()
        {
            var response = await systemSettingStateService.GetSystemSetting();
            if (response.IsSuccessStatusCode)
            {
                systemSetting = (SystemSetting)response.ResultData;
            }
            else
            {
                await ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }


        #endregion

        #region Redirect Function
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion

        #region Badrequest Notiication

        //<History Author = 'Zain' Date='2022-07-28' Version="1.0" Branch="master"> Handle bad request and display error messages in whole class</History>
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
                    var badRequestResponse = (BadRequestResponse)response.ResultData;
                    if (badRequestResponse.InnerException != null && badRequestResponse.InnerException.ToLower().Contains("violation of unique key"))
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Role_Name_Exists"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
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
        protected async Task FormSubmit(SystemSetting args)
        {
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
                       translationState.Translate("Sure_Submit"),
                       translationState.Translate("Confirm"),
                       new ConfirmOptions()
                       {
                           OkButtonText = @translationState.Translate("OK"),
                           CancelButtonText = @translationState.Translate("Cancel")
                       });
                if (dialogResponse == true)
                {
                    string res = string.Empty;
                    foreach (var dmsSelectedTypes in systemSetting.DmsFileTypesSelectedList)
                    {
                        systemSetting.SelectedTypesIdList.Add(dmsSelectedTypes.Type);
                    }
                    if (systemSetting.SelectedTypesIdList != null && systemSetting.SelectedTypesIdList.Any())
                    {
                        systemSetting.FileTypes = string.Join(',', fileTypes.Where(x => systemSetting.SelectedTypesIdList.Contains(x.Type)).Select(y => y.Type));
                        var response = await systemSettingStateService.UpdateSystemSetting(args);
                        spinnerService.Show();
                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Attributes_Updated_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto;"
                            });
                        }
                        else
                        {
                            await ReturnBadRequestNotification(response);
                        }
                        spinnerService.Hide();
                        Reload();
                    }
                    else
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Select_Atleast_One_File_Type"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        spinnerService.Hide();
                    }
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
    }
}
