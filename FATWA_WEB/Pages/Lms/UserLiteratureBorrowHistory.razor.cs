using FATWA_DOMAIN.Models.ViewModel.Lms;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace FATWA_WEB.Pages.Lms
{
    public partial class UserLiteratureBorrowHistory : ComponentBase
    {
        [Parameter]
        public dynamic Id { get; set; }
        public List<UserBorrowedHistoryVM> UserHistory = new();
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            spinnerService.Hide();
        }
        protected async Task Load()
        {
            await GetTasKEntityHistoryByReferenceId();
        }
        protected async Task GetTasKEntityHistoryByReferenceId()
        {
            try
            {
                var response = await lmsLiteratureBorrowDetailService.GetUserBorrowHistoryByUserId(Id);
                if (response.IsSuccessStatusCode)
                {
                    UserHistory = (List<UserBorrowedHistoryVM>)response.ResultData;

                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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
