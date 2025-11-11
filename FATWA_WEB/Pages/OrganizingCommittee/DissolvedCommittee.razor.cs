using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel.GeneralVMs;
using FATWA_DOMAIN.Models.ViewModel.OrganizingCommitteeVMs;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using static FATWA_DOMAIN.Enums.OrganizingCommittee.OrganizingCommitteeEnum;

namespace FATWA_WEB.Pages.OrganizingCommittee
{
    public partial class DissolvedCommittee:ComponentBase
    {
        #region Inject
        [Inject]
        public ProcessLogService processLogService { get; set; }
        [Inject]
        public ErrorLogService errorLogService { get; set; }
        #endregion

        #region Parameters
        [Parameter]
        public Guid CommitteId { get; set; }
        #endregion

        #region Variables
        public CommitteeDecisionStatusVM decisionStatusVM = new CommitteeDecisionStatusVM();

        public string? descriptionValidationMessage { get; set; } = "";
        #endregion

        #region OnInitialized
        protected override Task OnInitializedAsync()
        {
            return base.OnInitializedAsync();
        }
        #endregion

        #region Form Submit
        protected async Task FormSubmit(CommitteeDecisionStatusVM decisionStatus)
        {
            try
            {
                if (string.IsNullOrEmpty(decisionStatus.Reason))
                {
                    descriptionValidationMessage = String.IsNullOrEmpty(decisionStatus.Reason) ? translationState.Translate("Required_Field") : "";
                }
                else
                {
                    bool? dialogResponse = await dialogService.Confirm(
                    translationState.Translate("Sure_Submit"),
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
                            decisionStatus.ReferenceId = CommitteId;
                            decisionStatus.StatusId = (int)CommitteeStatusEnum.Dissolved;
                            var response = await organizingCommitteeService.DissolveCommittee(decisionStatus);
                            if (response.IsSuccessStatusCode)
                            {
                                var apiResponse = (ApiReturnTaskNotifAuditLogVM)response.ResultData;
                                if (apiResponse.sendNotifications.Count > 0)
                                {
                                    await notificationDetailService.SendNotification(apiResponse.sendNotifications);
                                }
                                if (apiResponse.processLog != null)
                                {
                                    await processLogService.CreateProcessLog(apiResponse.processLog);
                                }
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Success,
                                    Detail = translationState.Translate("Committee_Disolved_Successfully"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                                dialogService.Close();
                            }
                            else
                            {
                                var errorLog = (ErrorLog)response.ResultData;
                                if (errorLog != null)
                                {
                                    await errorLogService.CreateErrorLog(errorLog);
                                }
                                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                            }
                            spinnerService.Hide();
                        }
                    }
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
        #endregion

        #region Cancel
        protected async Task CloseDialog(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
        #endregion

        #region Reload Page
        protected async Task ReloadPage()
        {
            await JsInterop.InvokeVoidAsync("refreshPage");
        }
        #endregion
    }
}
