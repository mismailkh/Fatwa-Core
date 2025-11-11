using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
namespace FATWA_WEB.Pages.CaseManagment.Shared
{
    public partial class DetailCaseFileTransferRequest : ComponentBase
    {

        #region Parameter
        [Parameter]
        public dynamic ReferenceId { get; set; }
        [Parameter]
        public string? TaskId { get; set; }
        #endregion
        #region Varriable
        public CmsCaseFileTransferRequestDetailVM cmsCaseFileTransferRequestDetailVM { get; set; } = new CmsCaseFileTransferRequestDetailVM();
        protected TaskDetailVM taskDetailVM = new TaskDetailVM();
        #endregion
        #region ON Load Component
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateTransferRequestDetails();
            //await PopulateTransferRequestRejectionDetails();
            await PopulateTaskDetails();
            spinnerService.Hide();
        } 
        protected async Task PopulateTransferRequestDetails()
        {
            var response = await cmsSharedService.GetCaseFileTransferRequestDetailById(Guid.Parse(ReferenceId));
            if (response.IsSuccessStatusCode)
            {
                cmsCaseFileTransferRequestDetailVM = (CmsCaseFileTransferRequestDetailVM)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        protected async Task PopulateTaskDetails()
        {
            if (TaskId != null)
            {
                var getTaskDetail = await taskService.GetTaskDetailById(Guid.Parse(TaskId));
                if (getTaskDetail.IsSuccessStatusCode)
                {
                    taskDetailVM = (TaskDetailVM)getTaskDetail.ResultData;
                }
                else
                {
                    taskDetailVM = new TaskDetailVM();
                }
            }
        }
        #endregion
        #region Redirect Function
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion
        #region Approve/Reject Buttons
        protected async Task ApproveCaseFileTransferRequest(MouseEventArgs args)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<TransferSector>(
                    translationState.Translate("Transfer"),
                    new Dictionary<string, object>() {
                    { "ReferenceId", Guid.Empty },
                    { "TransferCaseType", (int)AssignCaseToLawyerTypeEnum.CaseFile },
                    { "IsAssignment", false },
                    { "RejectedTransferIds", new List<int>()},
                    { "SenderSector", null},
                    { "IsViewOnly", true }
                    },
                    new DialogOptions() { Width = "80% !important", CloseDialogOnOverlayClick = true });
                if (dialogResult != null)
                {
                    spinnerService.Show();
                    cmsCaseFileTransferRequestDetailVM.UserName = loginState.UserDetail.UserName;
                    cmsCaseFileTransferRequestDetailVM.StatusId = (int)CaseFileTransferRequestStatusEnum.Approved;
                    var response = await cmsSharedService.ApproveCaseFileTransferRequest(cmsCaseFileTransferRequestDetailVM);
                    if (response.IsSuccessStatusCode)
                    {
                        if (TaskId != null)
                        {
                            taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
                            taskDetailVM.SectorId = loginState.UserDetail.SectorTypeId;
                            taskDetailVM.IsMultipleTaskUpdateForSameEntity = true;
                            taskDetailVM.SystemGenTypeIdsToComplete.Add((int)TaskSystemGenTypeEnum.CaseFileTranferRequestToSector);
                            var taskResponse = await taskService.DecisionTask(taskDetailVM);
                            if (!taskResponse.IsSuccessStatusCode)
                            {
                                await invalidRequestHandlerService.ReturnBadRequestNotification(taskResponse);
                            }
                        }
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Changes_saved_successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        await RedirectBack();
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    spinnerService.Hide();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected async Task RejectCaseFileTransferRequest(MouseEventArgs args)
        {
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
                     translationState.Translate("Sure_Reject"),
                     translationState.Translate("Confirm"),
                     new ConfirmOptions()
                     {
                         OkButtonText = @translationState.Translate("OK"),
                         CancelButtonText = @translationState.Translate("Cancel")
                     });
                if (dialogResponse == true)
                {
                    var dialogResult = await dialogService.OpenAsync<RejectionReason>
                   (
                       translationState.Translate("Rejection_Reason"),
                       null,
                       new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true }
                   );
                    if (dialogResult != null)
                    {
                        spinnerService.Show();
                        cmsCaseFileTransferRequestDetailVM.RejectionReason = (string)dialogResult;
                        cmsCaseFileTransferRequestDetailVM.UserName = loginState.UserDetail.UserName;
                        cmsCaseFileTransferRequestDetailVM.StatusId = (int)CaseFileTransferRequestStatusEnum.Rejected;
                        var response = await cmsSharedService.RejectCaseFileTransferRequest(cmsCaseFileTransferRequestDetailVM);
                        if (response.IsSuccessStatusCode)
                        {
                            if (TaskId != null)
                            {
                                taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
                                taskDetailVM.SectorId = loginState.UserDetail.SectorTypeId;
                                taskDetailVM.IsMultipleTaskUpdateForSameEntity = true;
                                taskDetailVM.SystemGenTypeIdsToComplete.Add((int)TaskSystemGenTypeEnum.CaseFileTranferRequestToSector);
                                var taskResponse = await taskService.DecisionTask(taskDetailVM);
                                if (!taskResponse.IsSuccessStatusCode)
                                {
                                    await invalidRequestHandlerService.ReturnBadRequestNotification(taskResponse);
                                }
                            }
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Changes_saved_successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            await RedirectBack();
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }
                        spinnerService.Hide();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
