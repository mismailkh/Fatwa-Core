using FATWA_DOMAIN.Models.BugReporting;
using FATWA_DOMAIN.Models.ViewModel.BugReportingVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using static FATWA_DOMAIN.Enums.BugReporting.BugReportingEnum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_ADMIN.Pages.BugReporting
{
    public partial class AddCommentFeedBack : ComponentBase
    {
        #region Variables
        [Parameter]
        public Guid ReferenceId { get; set; }
        [Parameter]
        public int RemarksType { get; set; }
        public BugCommentFeedBack bugTicketComment { get; set; } = new BugCommentFeedBack();
        public TicketListVM BugTicket { get; set; } = new TicketListVM();
        public string BugReportingDate { get; set; }
        public string UserName { get; set; }
        public int bugId { get; set; }
        public string commentValidationMessage { get; set; } = "";
        #endregion

        #region OnInitialized
        protected async override Task OnInitializedAsync()
        {
        }
        #endregion
        protected async Task OnChangeRating(double rating)
        {
            bugTicketComment.Rating = Convert.ToInt32(rating);
        }
        #region Submit 
        protected async Task SubmitComment()
        {
            try
            {
                if (string.IsNullOrEmpty(bugTicketComment.Comment))
                {
                    commentValidationMessage = String.IsNullOrEmpty(bugTicketComment.Comment) ? translationState.Translate("Required_Field") : "";
                }
                else
                {
                    bool? dialogResponse = await dialogService.Confirm(
                    translationState.Translate(RemarksType == (int)RemarksTypeEnum.Comment ? "Sure_Submit_Comment" : "Sure_Submit_FeedBack"),
                    translationState.Translate("Submit"),
                    new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    });
                    if (dialogResponse != null)
                    {
                        if ((bool)dialogResponse)
                        {
                            spinnerService.Show();
                            ApiCallResponse response = new ApiCallResponse();
                            bugTicketComment.CreatedBy = loginState.Username;
                            bugTicketComment.CreatedDate = DateTime.Now;
                            bugTicketComment.ReferenceId = ReferenceId;
                            bugTicketComment.RemarkType = RemarksType;
                            response = await bugReportingService.AddCommentFeedBack(bugTicketComment);

                            if (response.IsSuccessStatusCode)
                            {
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Success,
                                    Detail = translationState.Translate(RemarksType == (int)RemarksTypeEnum.Comment ? "Comment_Added_Successfully" : "FeedBack_Added_Successfully"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                                dialogService.Close();
                                await InvokeAsync(StateHasChanged);
                            }
                            else
                            {
                                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                            }
                            spinnerService.Hide();

                        }
                    }
                }
            }
            catch (Exception)
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

        #region Cancel
        protected async Task CloseDialog(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
        #endregion

        #region Button Click Event
        protected async Task RedirectBack()
        {
            await jSRuntime.InvokeVoidAsync("RedirectToPreviousPage");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion

    }
}
