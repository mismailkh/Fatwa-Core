using FATWA_DOMAIN.Models;
using FATWA_ADMIN.Data;
using FATWA_ADMIN.Services;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.JSInterop;
using FATWA_ADMIN.Services.General;

namespace FATWA_ADMIN.Pages.AuditLogs
{
    public partial class ErrorLogViewDetailComponent : ComponentBase
    {
        #region Service Injection
        [Inject]
        protected ErrorLogService FatwaDb { get; set; }

        [Inject]
        protected ProcessLogService processLogService { get; set; }

        [Inject]
        protected LoginState loginState { get; set; }

        [Inject]
        protected TranslationState translationState { get; set; }

        [Inject]
        protected SpinnerService spinnerService { get; set; }

        [Inject]
        protected NavigationManager navigationManager { get; set; }

        [Inject]
        protected IJSRuntime JsInterop { get; set; }

        #endregion

        #region Variable

        [Parameter]
        public dynamic ErrorLogId { get; set; }

        ErrorLog _getErrorlogsResult;
        protected ErrorLog getErrorlogsResult
        {
            get
            {
                return _getErrorlogsResult;
            }
            set
            {
                if (!Equals(_getErrorlogsResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getErrorlogsResult", NewValue = value, OldValue = getErrorlogsResult };
                    _getErrorlogsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }


        #endregion

        #region Redirect Function
        protected void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        protected void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }

        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion

        public ErrorLogViewDetailComponent()
        {
        }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();

            //  processLog.ProcessLogId = Guid.Parse(ProcessLogId);
            getErrorlogsResult = await FatwaDb.GetErrorLogDetailById(Guid.Parse(ErrorLogId));

            spinnerService.Hide();
        }

    }
}
