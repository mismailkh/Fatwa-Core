using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.TimeIntervalVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace FATWA_ADMIN.Pages.TimeInterval
{
    public partial class TimeIntervals : ComponentBase
    {
        #region Radzen DataGrid Variables and Settings

        public List<Role> rolesList = new List<Role>();
        private Timer debouncer;
        private const int debouncerDelay = 500;
        protected RadzenDataGrid<TimeIntervalVM>? grid = new RadzenDataGrid<TimeIntervalVM>();
        protected RadzenDataGrid<TimeIntervalHistoryVM>? gridRemindersHistory = new RadzenDataGrid<TimeIntervalHistoryVM>();
        protected List<TimeIntervalHistoryVM> _CmsComsReminderHistory = new List<TimeIntervalHistoryVM>();

        protected bool allowRowSelectOnRowClick = true;
        string _search;
        public bool IsEdit { get; set; } = false;
        private bool isButtonDisabled = false;
        private Tooltip tooltipRef;
        IEnumerable<TimeIntervalVM> getTimeIntervals { get; set; }
        protected string search { get; set; }

        IEnumerable<TimeIntervalVM> _getTimeIntervals;
        protected IEnumerable<TimeIntervalVM> FiltergetTimeIntervals
        {
            get
            {
                return _getTimeIntervals;
            }
            set
            {
                if (!object.Equals(_getTimeIntervals, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "_getTimeIntervals", NewValue = value, OldValue = _getTimeIntervals };
                    _getTimeIntervals = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        //private void ShowTooltip()
        //{
        //	if (isButtonDisabled)
        //	{
        //		tooltipRef.Show();
        //	}
        //}

        //private void HideTooltip()
        //{
        //	if (isButtonDisabled)
        //	{
        //		tooltipRef.Hide();
        //	}
        //}

        //protected IEnumerable<CmsComsReminderHistory> _CmsComsReminderHistory
        //{
        //    get
        //    {
        //        return _CmsComsReminderHistory;
        //    }
        //    set
        //    {
        //        if (!object.Equals(_CmsComsReminderHistory, value))
        //        {
        //            var args = new PropertyChangedEventArgs() { Name = "_CmsComsReminderHistory", NewValue = value, OldValue = _CmsComsReminderHistory };
        //            _CmsComsReminderHistory = value;
        //            OnPropertyChanged(args);
        //            Reload();
        //        }
        //    }
        //}
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        #endregion

        #region On Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            translationState.TranslateGridFilterLabels(grid);

            spinnerService.Hide();

        }
        protected async Task Load()
        {
            try
            {
                var response = await timeIntervalService.GetTimeIntervals();
                if (response.IsSuccessStatusCode)
                {
                    getTimeIntervals = (IEnumerable<TimeIntervalVM>)response.ResultData;
                    FiltergetTimeIntervals = (IEnumerable<TimeIntervalVM>)response.ResultData;
                }
                else
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Time Interval History List
        protected async Task GetTimeIntervalHistoryList()
        {
            var response = await timeIntervalService.GetTimeIntervalHistoryList();
            //var result = await lookupService.GetLookupHistoryListByRefernceId(Id, IntervalTypeId);

            if (response.IsSuccessStatusCode)
                _CmsComsReminderHistory = (List<TimeIntervalHistoryVM>)response.ResultData;
            else
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);

        }


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
                        FiltergetTimeIntervals = await gridSearchExtension.Filter(getTimeIntervals, new Query()
                        {

                            Filter = $@"i => (i.SLAInterval != null && i.SLAInterval.ToString().ToLower().Contains(@0)) || (i.ExecutionTime != null && i.ExecutionTime.HasValue != null && i.ExecutionTime.Value.ToString(""hh:mm:ss"").Contains(@1)) || (i.ModifiedDate.HasValue != null && i.ModifiedDate.Value.ToString(""dd/MM/yyyy"").Contains(@2)) || (i.IntervalNameEn != null && i.IntervalNameEn.ToLower().Contains(@3)) || (i.CommunicationTypeEn != null && i.CommunicationTypeEn.ToLower().Contains(@4)) ",
                            FilterParameters = new object[] { search, search, search, search, search }
                        });
                    }
                    else
                    {
                        FiltergetTimeIntervals = await gridSearchExtension.Filter(getTimeIntervals, new Query()
                        {
                            Filter = $@"i => (i.SLAInterval != null && i.SLAInterval.ToString().ToLower().Contains(@0)) || (i.ExecutionTime != null && i.ExecutionTime.HasValue != null && i.ExecutionTime.Value.ToString(""hh:mm:ss"").Contains(@1)) || (i.ModifiedDate.HasValue != null && i.ModifiedDate.Value.ToString(""dd/MM/yyyy"").Contains(@2)) || (i.IntervalNameAr != null && i.IntervalNameAr.ToLower().Contains(@3)) || (i.CommunicationTypeAr != null && i.CommunicationTypeAr.ToLower().Contains(@4))",
                            FilterParameters = new object[] { search, search, search, search, search }
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

        #region Time Interval Action Button Details
        #region Add Time Interval
        protected async Task AddInterval(MouseEventArgs args)
        {
            // Currently this Add functionality is not used, might be later on we will have requriments to add.
            try
            {
                IsEdit = false;
                var dialogResult = await dialogService.OpenAsync<AddTimeIntervals>(
                translationState.Translate("Add_Reminders"),
                new Dictionary<string, object>()
                {
                    { "TimeIntervals", getTimeIntervals }
                },
                new DialogOptions() { CloseDialogOnOverlayClick = false });
                await Task.Delay(200);
                await Load();
                StateHasChanged();
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
        #endregion
        #region Edit Time Interval
        protected async Task EditTimeInterval(TimeIntervalVM data)
        {
            try
            {
                IsEdit = true;
                if (await dialogService.OpenAsync<AddTimeIntervals>(
                translationState.Translate("Edit_Time_Interval_(SLA)"),
                new Dictionary<string, object>()
                {
                    { "ID", data.ID } ,
                    {"IsEdit", IsEdit },
                    {"TimeIntervalVM", data }
                },
                new DialogOptions() { CloseDialogOnOverlayClick = false, Width = "48%" }) == true)
                {
                    await Task.Delay(100);
                    await Load();
                }

                StateHasChanged();
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
        #endregion
        #region Update Interval Status 
        protected async Task UpdateIntervavlStatus(TimeIntervalVM timeIntervalVM)
        {
            if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_you_want_to_update_Status"),
                translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
            {
                spinnerService.Show();
                var response = await timeIntervalService.UpdateIntervalStatus(timeIntervalVM.IsActive, timeIntervalVM.ID);
                if (response != null)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Updated_Successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto;"
                    });
                    await Load();
                    StateHasChanged();
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
                spinnerService.Hide();
            }

            else
            {
                if (timeIntervalVM.IsActive == false)
                    timeIntervalVM.IsActive = true;
                else
                    timeIntervalVM.IsActive = false;
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
        #endregion

    }
}

