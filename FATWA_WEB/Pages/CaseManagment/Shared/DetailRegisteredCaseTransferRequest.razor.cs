using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_WEB.Pages.CaseManagment.RegisteredCase;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
namespace FATWA_WEB.Pages.CaseManagment.Shared
{
    public partial class DetailRegisteredCaseTransferRequest : ComponentBase
    {

        #region Parameter
        [Parameter]
        public dynamic ReferenceId { get; set; }
        [Parameter]
        public string? TaskId { get; set; }
        #endregion
        #region Varriable
        public CmsRegisteredCaseTransferRequestVM caseTransferRequestVM { get; set; } = new CmsRegisteredCaseTransferRequestVM();
        protected TaskDetailVM taskDetailVM = new TaskDetailVM();
        protected CmsRegisteredCaseDetailVM registeredCase { get; set; } = new CmsRegisteredCaseDetailVM();
        #endregion
        #region ON Load Component
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateCaseTransferRequestDetails();
            await PopulateRegisteredCaseDetail();
            await PopulateTaskDetails();
            spinnerService.Hide();
        } 
        protected async Task PopulateCaseTransferRequestDetails()
        {
            var response = await cmsRegisteredCaseService.GetResgisteredCaseTansferRequestDetailById(Guid.Parse(ReferenceId));
            if (response.IsSuccessStatusCode)
            {
                caseTransferRequestVM = (CmsRegisteredCaseTransferRequestVM)response.ResultData;
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
        protected async Task PopulateRegisteredCaseDetail()
        {
            var result = await cmsRegisteredCaseService.GetRegisteredCaseDetailByIdVM(caseTransferRequestVM.CaseId);
            if (result.IsSuccessStatusCode)
            {
                registeredCase = (CmsRegisteredCaseDetailVM)result.ResultData;
                if (registeredCase.IsDissolved == null)
                    registeredCase.IsDissolved = false;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
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
        protected async Task ApproveRegisteredCaseTransferRequest(MouseEventArgs args)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<ApproveCaseTransferRequestToChamber>(translationState.Translate("Transfer_Case"),
                new Dictionary<string, object> {
                    { "ChamberId", caseTransferRequestVM.ChamberToId },
                    {"CaseId", registeredCase.CaseId },
                    {"ChamberNumberId", caseTransferRequestVM.ChamberNumberToId },
                    {"CourtId",registeredCase.CourtId },
                    {"OutComeId", caseTransferRequestVM.OutcomeId}
                },
                new DialogOptions() { Width = "30%", Resizable = true, CloseDialogOnOverlayClick = true }
                );
                if(dialogResult != null )
                {
                    spinnerService.Show();
                    caseTransferRequestVM.caseTransferHistoryVM = (CMSRegisteredCaseTransferHistoryVM)dialogResult;
                    caseTransferRequestVM.caseTransferHistoryVM.ChamberFromId = caseTransferRequestVM.ChamberFromId;
                    caseTransferRequestVM.caseTransferHistoryVM.ChamberNumberFromId = caseTransferRequestVM.ChamberNumberFromId;
                    caseTransferRequestVM.UserName = loginState.UserDetail.UserName;
                    caseTransferRequestVM.StatusId = (int)CaseFileTransferRequestStatusEnum.Approved;
                    var response = await cmsRegisteredCaseService.ApproveRegisteredCaseTransferRequest(caseTransferRequestVM);
                    if (response.IsSuccessStatusCode)
                    {
                        if (TaskId != null)
                        {
                            taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
                            taskDetailVM.SectorId = loginState.UserDetail.SectorTypeId;
                            taskDetailVM.IsMultipleTaskUpdateForSameEntity = true;
                            taskDetailVM.SystemGenTypeIdsToComplete.Add((int)TaskSystemGenTypeEnum.RegisteredCaseTranferRequest);
                            var taskResponse = await taskService.DecisionTask(taskDetailVM);
                            if (!taskResponse.IsSuccessStatusCode)
                            {
                                await invalidRequestHandlerService.ReturnBadRequestNotification(taskResponse);
                            }
                        }
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Case_Transfered_Successfully"),
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
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected async Task RejectRegisteredCaseTransferRequest(MouseEventArgs args)
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
                        caseTransferRequestVM.caseTransferHistoryVM.ChamberFromId = caseTransferRequestVM.ChamberFromId;
                        caseTransferRequestVM.caseTransferHistoryVM.ChamberNumberFromId = caseTransferRequestVM.ChamberNumberFromId;
                        caseTransferRequestVM.caseTransferHistoryVM.ChamberToId = caseTransferRequestVM.ChamberToId;
                        caseTransferRequestVM.caseTransferHistoryVM.ChamberNumberToId = caseTransferRequestVM.ChamberNumberToId;
                        caseTransferRequestVM.caseTransferHistoryVM.OutcomeId = caseTransferRequestVM.OutcomeId;
                        caseTransferRequestVM.caseTransferHistoryVM.createdBy = loginState.UserDetail.UserName;

                        caseTransferRequestVM.RejectionReason = (string)dialogResult;
                        caseTransferRequestVM.UserName = loginState.UserDetail.UserName;
                        caseTransferRequestVM.StatusId = (int)CaseFileTransferRequestStatusEnum.Rejected;
                        var response = await cmsRegisteredCaseService.RejectRegisteredCaseTransferRequest(caseTransferRequestVM);
                        if (response.IsSuccessStatusCode)
                        {
                            if (TaskId != null)
                            {
                                taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
                                taskDetailVM.SectorId = loginState.UserDetail.SectorTypeId;
                                taskDetailVM.IsMultipleTaskUpdateForSameEntity = true;
                                taskDetailVM.SystemGenTypeIdsToComplete.Add((int)TaskSystemGenTypeEnum.RegisteredCaseTranferRequest);
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
