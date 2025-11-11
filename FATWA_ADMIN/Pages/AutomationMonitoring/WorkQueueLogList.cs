using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.AdminModels.AutomationMonitoring;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.AutomationMonitoring;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;

namespace FATWA_ADMIN.Pages.AutomationMonitoring
{
    public partial class WorkQueueLogList : ComponentBase
    {
        #region Varriables
        [Parameter]
        public dynamic ItemId { get; set; }
        protected bool Keywords = false;
        public bool isVisible { get; set; }
        public string? CaseNumber { get; set; }
        public string? CANNumber { get; set; }
        [Inject]
        protected IJSRuntime JsInterop { get; set; }
        protected RadzenDataGrid<AMSItemLogVM>? ItemLogsGrid;
        public AMSItemLogVM ItemLogVM { get; set; }
        protected AdvanceSearchQueueVM advanceSearchVM = new AdvanceSearchQueueVM();
        protected List<AMSQueueItemStatus> Statuses { get; set; } = new List<AMSQueueItemStatus>();
        protected bool isLoading { get; set; }
        public int count { get; set; }
        IEnumerable<AMSItemLogVM> _Itemloglist;
        IEnumerable<AMSItemLogVM> FilteredItemLog { get; set; } = new List<AMSItemLogVM>();
        protected IEnumerable<AMSItemLogVM> Itemloglist
        {
            get
            {
                return _Itemloglist;
            }
            set
            {
                if (!object.Equals(_Itemloglist, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "FilteredItemLog", NewValue = value, OldValue = _Itemloglist };
                    _Itemloglist = value;

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

            await Load();
            await PopulateStatuses();
            spinnerService.Hide();
        }
        public async Task Load()
        {
            isLoading = true;
            var response = await automationmonitoringService.GetItemLogsList(Convert.ToInt32(ItemId));
            if (response.IsSuccessStatusCode)
            {
                _Itemloglist = (IEnumerable<AMSItemLogVM>)response.ResultData;
                FilteredItemLog = (IEnumerable<AMSItemLogVM>)response.ResultData;

                var result = _Itemloglist.FirstOrDefault();
                if (result != null)
                {
                    CANNumber = result.CANumber;
                    CaseNumber = result.CaseNumber;

                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            isLoading = false;

            StateHasChanged();
        }
        protected async Task PopulateStatuses()
        {
            var response = await automationmonitoringService.GetQueueItemStatuses();
            if (response.IsSuccessStatusCode)
            {
                Statuses = (List<AMSQueueItemStatus>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
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
                    //Summary = $"خطأ!",
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                Keywords = true;
                return;
            }
            if (advanceSearchVM.StatusId == 0 && !advanceSearchVM.CreatedFrom.HasValue && !advanceSearchVM.CreatedTo.HasValue)
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
            advanceSearchVM = new AdvanceSearchQueueVM();
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

        #region  List Filtration -> SearchFunctionality

        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
                    {
                        FilteredItemLog = await gridSearchExtension.Filter(_Itemloglist, new Query()
                        {
                            Filter = $@"i => (i.LogType != null && i.LogType.ToLower().Contains(@0))",
                            FilterParameters = new object[] { search }
                        });
                    }
                    else
                    {
                        FilteredItemLog = await gridSearchExtension.Filter(_Itemloglist, new Query()
                        {
                            Filter = $@"i => (i.LogType != null && i.LogType.ToLower().Contains(@0))",
                            FilterParameters = new object[] { search }
                        });
                    }
                    await InvokeAsync(StateHasChanged);
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
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion
    }
}