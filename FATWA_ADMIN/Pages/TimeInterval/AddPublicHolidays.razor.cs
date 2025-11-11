using FATWA_DOMAIN.Models.TimeInterval;
using FATWA_DOMAIN.Models.ViewModel.TimeIntervalVMs;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace FATWA_ADMIN.Pages.TimeInterval
{
    public partial class AddPublicHolidays : ComponentBase
    {
        #region Parameter
        [Parameter]
        public int? Id { get; set; }
        [Parameter]
        public IEnumerable<PublicHolidaysVM> PublicHolidays { get; set; }
        #endregion

        #region Variables
        public PublicHoliday PublicHoliday = new PublicHoliday();
        #endregion

        #region On Initialize
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            spinnerService.Hide();
        }

        #endregion

        #region Load
        private async Task Load()
        {
            if (Id != null)
            {
                await GetPublicHolidayById(Id);
            }
        }
        protected async Task GetPublicHolidayById(int? Id)
        {
            var response = await timeIntervalService.GetPublicHolidayById(Id);
            if (response.IsSuccessStatusCode)
            {
                PublicHoliday = (PublicHoliday)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        #endregion

        #region Holiday Date Validation
        private bool IsHolidayDateRangeValid()
        {
            return PublicHolidays.Any(h =>
                (h.FromDate.Date <= PublicHoliday.FromDate.Date && h.ToDate.Date >= PublicHoliday.FromDate.Date)
                || (h.FromDate.Date <= PublicHoliday.ToDate?.Date && h.ToDate.Date >= PublicHoliday.ToDate?.Date)
                || (PublicHoliday.FromDate.Date <= h.FromDate.Date && PublicHoliday.ToDate?.Date >= h.FromDate.Date)
                || (PublicHoliday.FromDate.Date <= h.ToDate.Date && PublicHoliday.ToDate?.Date >= h.ToDate.Date));

        }
        #endregion

        #region Add Public Holidays
        protected async Task AddHoliday(PublicHoliday publicHoliday)
        {
            try
            {
                if (string.IsNullOrEmpty(publicHoliday.Description) || publicHoliday.Description.Length > 1000)
                    return;

                if (IsHolidayDateRangeValid())
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("selected_date_range_conflicts_with_existing_holidays"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
                }
                bool? dialogResponse = await dialogService.Confirm(
                            translationState.Translate("Sure_Submit"),
                            translationState.Translate("Confirm"),
                            new ConfirmOptions()
                            {
                                OkButtonText = translationState.Translate("OK"),
                                CancelButtonText = translationState.Translate("Cancel")
                            });
                if (dialogResponse == true)
                {
                    spinnerService.Show();
                    if (publicHoliday.Id == 0)
                    {
                        var response = await timeIntervalService.AddPublicHolidays(publicHoliday);

                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Successfully_Added_Holidays"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }
                    }
                    else
                    {
                        var response = await timeIntervalService.UpdatePublicHoliday(publicHoliday);
                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Successfully_Updated"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            dialogService.Close(true);
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }
                    }
                    PublicHolidays = new List<PublicHolidaysVM>();
                    dialogService.Close(true);
                    StateHasChanged();
                    spinnerService.Hide();
                }
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                spinnerService.Hide();
            }
        }

        #endregion

        #region Functions
        protected void OnCancel()
        {
            PublicHolidays = new List<PublicHolidaysVM>();
            dialogService.Close(false);
        }
        public async Task DateValidityCheck()

        {
            if (PublicHoliday.FromDate > PublicHoliday.ToDate)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("FromDate_NotGreater_ToDate"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                PublicHoliday.FromDate = DateTime.Today.AddDays(1);
                PublicHoliday.ToDate = DateTime.Today.AddDays(2);
                return;
            }
        }
        #endregion
    }
}
