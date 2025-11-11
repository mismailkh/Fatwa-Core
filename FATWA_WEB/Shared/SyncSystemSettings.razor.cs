using Blazored.LocalStorage;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_WEB.Data;
using Microsoft.AspNetCore.Components;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;

namespace FATWA_WEB.Shared
{
    //<History Author = 'Hassan Abbas' Date='2022-09-12' Version="1.0" Branch="master"> System Settings page for reinitializing Singelton Services</History>
    public partial class SyncSystemSettings : ComponentBase
    {
        #region Service Injection

        
     

        #endregion

        //<History Author = 'Hassan Abbas' Date='2022-09-12' Version="1.0" Branch="master"> Invoke Login State Change Event</History>
        protected override async Task OnInitializedAsync()
        {
            await GetSystemSetting();
        }

        public async Task GetSystemSetting()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/SystemSetting/GetSystemSetting");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await BrowserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var setting = await response.Content.ReadFromJsonAsync<SystemSetting>();
                    systemSettingState.Grid_Pagination = setting.Grid_Pagination;
                    systemSettingState.Book_Copy_Count = setting.Book_Copy_Count;
                    systemSettingState.Eligible_Count = setting.Eligible_Count;
                    systemSettingState.Borrow_Period = setting.Borrow_Period;
                    systemSettingState.Extension_Period = setting.Extension_Period;
                    systemSettingState.File_Minimum_Size = setting.File_Minimum_Size;
                    systemSettingState.File_Maximum_Size = setting.File_Maximum_Size;
                    systemSettingState.IsInitialized = true;
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("System Settings Synced!"),
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
            catch(Exception ex)
            {
                
            }
        }
    }
}
