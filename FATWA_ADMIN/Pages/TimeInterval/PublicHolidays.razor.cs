using FATWA_DOMAIN.Models.ViewModel.TimeIntervalVMs;
using FATWA_DOMAIN.Models.ViewModel.WorkerServiceVMs;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace FATWA_ADMIN.Pages.TimeInterval
{

    public partial class PublicHolidays : ComponentBase
    {
        #region Properties
        protected RadzenDataGrid<PublicHolidaysVM>? grid = new RadzenDataGrid<PublicHolidaysVM>();
        protected IEnumerable<PublicHolidaysVM> publicHolidays = new List<PublicHolidaysVM>();
        protected IEnumerable<PublicHolidaysVM> FilteredpublicHolidaysResult = new List<PublicHolidaysVM>();
        private Timer debouncer;
        private const int debouncerDelay = 500;
        public int count { get; set; }
        protected string search { get; set; }

        IEnumerable<WSExecutionDetailVM> _WSExecutionDetailVMs;
        protected IEnumerable<WSExecutionDetailVM> WSExecutionDetailVMs
        {
            get
            {
                return _WSExecutionDetailVMs;
            }
            set
            {
                if (!Equals(_WSExecutionDetailVMs, value))
                {
                    var args = new FATWA_ADMIN.Services.General.PropertyChangedEventArgs() { Name = "WSExecutionDetailVMs", NewValue = value, OldValue = _WSExecutionDetailVMs };
                    _WSExecutionDetailVMs = value;

                    Reload();
                }
            }
        }

        #endregion

        #region On Initialize
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            spinnerService.Hide();
        }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        #endregion

        #region Load
        protected async Task Load()
        {
            spinnerService.Show();
            await GetPublicHolidays();
            spinnerService.Hide();
        }
        #endregion

        #region On Search Input
        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredpublicHolidaysResult = await gridSearchExtension.Filter(publicHolidays, new Query()
                    {
                        Filter = $@"i => ((i.Description != null && i.Description.ToLower().Contains(@0)) 
                    || (i.CreatedByEn != null && i.CreatedByEn.ToLower().Contains(@1)) 
                    || (i.ToDate != null && i.ToDate.ToString(""dd/MM/yyyy"").Contains(@2)) 
                    || (i.FromDate != null && i.FromDate.ToString(""dd/MM/yyyy"").Contains(@3)) 
                    || (i.CreatedByAr != null && i.CreatedByAr.ToLower().Contains(@4)))
                    || (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@5))",
                        FilterParameters = new object[] { search, search, search, search, search, search }
                    });
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

        #region Public Holiday Lists
        protected async Task GetPublicHolidays()
        {
            var response = await timeIntervalService.GetPublicHolidays();
            if (response.IsSuccessStatusCode)
            {
                publicHolidays = (IEnumerable<PublicHolidaysVM>)response.ResultData;
                FilteredpublicHolidaysResult = (IEnumerable<PublicHolidaysVM>)response.ResultData;
                count = publicHolidays.Count();
            }
            else
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);

            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region Holidays Action Button Detail
        #region Add Holidays
        protected async Task AddHoliday()
        {
            try
            {
                if (await dialogService.OpenAsync<AddPublicHolidays>(
                translationState.Translate("Add_Public_Holiday"),
               new Dictionary<string, object>()
                {
                   { "PublicHolidays", publicHolidays }
                },
                new DialogOptions() { Width = "30%", CloseDialogOnOverlayClick = false }) == true)
                {
                    await Task.Delay(100);
                    await GetPublicHolidays();
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
        #region Edit Holidays
        protected async Task UpdatePublicHoliday(PublicHolidaysVM publicHoliday)
        {
            try
            {
                if (await dialogService.OpenAsync<AddPublicHolidays>(
                translationState.Translate("Add_Public_Holiday"),
                new Dictionary<string, object>()
                {
                    { "Id", publicHoliday.Id },
                    { "PublicHolidays", publicHolidays }
                },
                new DialogOptions() { Width = "25%", CloseDialogOnOverlayClick = false }) == true)
                {
                    await Task.Delay(100);
                    await GetPublicHolidays();
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
        #region Delete Holidays
        protected async Task DeletePublicHoliday(PublicHolidaysVM publicHoliday)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Are_you_sure_you_want_to_delete_this_record"),
                translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();

                    var response = await timeIntervalService.DeletePublicHoliday(publicHoliday);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Deleted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        await GetPublicHolidays();
                        StateHasChanged();
                    }
                    else
                    {
                        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Something_went_wrong"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("An_unexpected_error_occurred") + $": {response.StatusCode}",
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                    }
                    spinnerService.Hide();
                }
            }
            catch (Exception ex)
            {
                spinnerService.Hide();
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("An_unexpected_error_occurred") + ": " + ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion
        #endregion

        #region Row Cell Render based on status highlight color 
        //protected void RowCellRender(RowRenderEventArgs<WSExecutionDetailVM> WSExecutionDetailVMs)
        //{
        //    if (WSExecutionDetailVMs.Data.ExecutionStatusEn == WorkerServiceExecutionStatusEnums.Failed.ToString()
        //        || WSExecutionDetailVMs.Data.ExecutionStatusAr == WorkerServiceExecutionStatusEnums.Failed.ToString()
        //        || WSExecutionDetailVMs.Data.ExecutionStatusEn == WorkerServiceExecutionStatusEnums.Exception.ToString()
        //        || WSExecutionDetailVMs.Data.ExecutionStatusAr == WorkerServiceExecutionStatusEnums.Exception.ToString())
        //    {
        //        WSExecutionDetailVMs.Attributes.Add("style", $"background-color: #FF7F7F;");
        //    }
        //}
        #endregion

        #region Advance Search
        //protected void ToggleAdvanceSearch()
        //{
        //    isVisible = !isVisible;
        //    if (!isVisible)
        //    {
        //        ResetForm();
        //    }
        //}
        //public async void ResetForm()
        //{
        //    advanceSearchVM = new WSExecutionAdvanceSearchVM();
        //    await Load();
        //    Keywords = false;
        //    StateHasChanged();
        //}
        //protected async Task SubmitAdvanceSearch()
        //{
        //    if (advanceSearchVM.FromDate > advanceSearchVM.ToDate)
        //    {
        //        notificationService.Notify(new NotificationMessage()
        //        {
        //            Severity = NotificationSeverity.Error,
        //            Detail = translationState.Translate("FromDate_NotGreater_ToDate"),
        //            Style = "position: fixed !important; left: 0; margin: auto; "
        //        });
        //        Keywords = true;
        //        return;
        //    }
        //    if (advanceSearchVM.StatusId == 0 /*&& string.IsNullOrWhiteSpace(advanceSearchVM.SearchKeywords*/
        //    && !advanceSearchVM.FromDate.HasValue && !advanceSearchVM.ToDate.HasValue)
        //    {
        //    }
        //    else
        //    {
        //        Keywords = true;
        //        await Load();
        //        StateHasChanged();
        //    }
        //}
        //protected async Task LoadStatuses()
        //{
        //    WSExecutionStatuses = await timeIntervalService.GetWSExecutionStatuses();
        //}
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
