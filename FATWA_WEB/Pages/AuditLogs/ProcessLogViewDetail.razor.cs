using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using FATWA_WEB.Services;
using FATWA_DOMAIN.Models;
using FATWA_WEB.Data;
using FATWA_DOMAIN.Models.ViewModel;
using Microsoft.JSInterop;
using Syncfusion.Blazor.PdfViewerServer;


namespace FATWA_WEB.Pages.AuditLogs
{
    public partial class ProcessLogViewDetailComponent : ComponentBase
    { 
        #region Service Injection

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
        public dynamic ProcessLogId { get; set; }

        ProcessLog _getProcesslogsResult;
        protected ProcessLog getProcesslogsResult
        {
            get
            {
                return _getProcesslogsResult;
            }
            set
            {
                if (!object.Equals(_getProcesslogsResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getProcesslogsResult", NewValue = value, OldValue = getProcesslogsResult };
                    _getProcesslogsResult = value;
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

        public ProcessLogViewDetailComponent()
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
            getProcesslogsResult = await processLogService.GetProcessLogDetailById(Guid.Parse(ProcessLogId));

            spinnerService.Hide();
        }

    }
}
