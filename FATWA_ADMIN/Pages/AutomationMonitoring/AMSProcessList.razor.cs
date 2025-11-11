using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.AutomationMonitoring;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using static FATWA_ADMIN.Pages.AutomationMonitoring.AMSProcessAction;
namespace FATWA_ADMIN.Pages.AutomationMonitoring
{
    public partial class AMSProcessList : ComponentBase
    {
        #region Varriables
        protected bool Keywords = false;
        public bool isVisible { get; set; }
        protected RadzenDataGrid<AutomationMonitoringProcessVM>? ProcessGrid;
        protected IEnumerable<AutomationMonitoringProcessVM> automationMonitoringProcessList { get; set; }
        public AutomationMonitoringProcessVM automationMonitoringProcessListVM { get; set; }
        protected AdvanceSearchProcessVM advanceSearchVM = new AdvanceSearchProcessVM();
        protected IEnumerable<StatusOption> statusOptions { get; set; }
        protected bool isLoading { get; set; }
        public int count { get; set; }
        IEnumerable<AutomationMonitoringProcessVM> _Processlist;
        IEnumerable<AutomationMonitoringProcessVM> FilteredProcess { get; set; } = new List<AutomationMonitoringProcessVM>();
        protected IEnumerable<AutomationMonitoringProcessVM> Processlist
        {
            get
            {
                return _Processlist;
            }
            set
            {
                if (!object.Equals(_Processlist, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "FilteredCaseFiles", NewValue = value, OldValue = _Processlist };
                    _Processlist = value;

                    Reload();
                }

            }
        }

        protected string search { get; set; }
        private Timer debouncer;
        private const int debouncerDelay = 500;

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
        #endregion
        #region On Initialized
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            statusOptions = new List<StatusOption>
            {
            new StatusOption { Id = true, Status = "Activate" },
            new StatusOption { Id = false, Status = "Deactivate" }
            };
            statusOptions = (IEnumerable<StatusOption>)statusOptions;
            await Load();
            spinnerService.Hide();
        }
        public async Task Load()
        {
            isLoading = true;
            var response = await automationmonitoringService.GetProcessList(advanceSearchVM);
            if (response.IsSuccessStatusCode)
            {
                _Processlist = (IEnumerable<AutomationMonitoringProcessVM>)response.ResultData;
                FilteredProcess = (IEnumerable<AutomationMonitoringProcessVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            isLoading = false;

            StateHasChanged();
        }
        #endregion

        #region  List Filtration -> SearchFunctionality
        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredProcess = await gridSearchExtension.Filter(_Processlist, new Query()
                    {
                        Filter = $@"i => ((i.processName != null && i.processName.ToLower().Contains(@0)) || (i.Resources != null && i.Resources.ToLower().Contains(@0)) || (i.LaunchDate != null && i.LaunchDate.ToString().ToLower().Contains(@0)) || (i.Description != null && i.Description.ToLower().Contains(@0)) || (i.ProcessCode != null && i.ProcessCode.ToLower().Contains(@0)))",
                        FilterParameters = new object[] { search }
                    });  await InvokeAsync(StateHasChanged);
                }, null, debouncerDelay, Timeout.Infinite);
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

        protected async Task ActiveAndInActiveProcess(AutomationMonitoringProcessVM item)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<AMSProcessAction>(
                            translationState.Translate("Process_Action"),
                            new Dictionary<string, object>() { { "ProcessId", item.Id } },
                            new DialogOptions()
                            {
                                Width = "30% !important",
                                CloseDialogOnOverlayClick = false,
                            });
                await Task.Delay(100);
                await Load();

            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }

        }
        protected void OnRowSelect(AutomationMonitoringProcessVM process)
        {
            Console.WriteLine($"Row selected: {process.ProcessName}");
        }

        #region Advance Search
        protected async Task SubmitAdvanceSearch()
        {
            if (advanceSearchVM.CreatedFrom > advanceSearchVM.CreatedTo)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("FromDate_NotGreater_ToDate"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                Keywords = true;
                return;
            }
            if (!advanceSearchVM.CreatedFrom.HasValue && !advanceSearchVM.CreatedTo.HasValue && !advanceSearchVM.IsActive.HasValue)
            {

            }
            else
            {
                Keywords = true;
                await Load();
                //await grid.Reload();
                StateHasChanged();
            }



        }

        public async void ResetForm()
        {
            advanceSearchVM = new AdvanceSearchProcessVM();
            await Load();
            Keywords = false;
            StateHasChanged();
        }

        //<History Author = 'Hassan Abbas' Date='2022-09-28' Version="1.0" Branch="master">Open advance search popup </History>
        protected async Task ToggleAdvanceSearch()
        {
            isVisible = !isVisible;
            if (!isVisible)
            {
                ResetForm();

            }
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
