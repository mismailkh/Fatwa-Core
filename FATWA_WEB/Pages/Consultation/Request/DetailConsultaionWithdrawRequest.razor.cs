using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_GENERAL.Helper;
using FATWA_WEB.Pages.CaseManagment.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Syncfusion.Blazor.PdfViewerServer;
using System.Collections.ObjectModel;
using Telerik.Blazor.Components;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.Consultation.Request
{
    public partial class DetailConsultaionWithdrawRequest : ComponentBase
    {
        #region Parameter
        [Parameter]
        public dynamic ConsultationRequestId { get; set; }
        [Parameter]
        public dynamic CommunicationTypeId { get; set; }
        [Parameter]
        public dynamic TaskId { get; set; }
        [Parameter]
        public dynamic WithdrawRequestId { get; set; }
        #endregion
        public int SectorTypeId { get; set; }
        #region variables
        WithdrawRequestDetailVM getWithdrawConsultation;
        protected TelerikPdfViewer PdfViewerRef { get; set; }
        protected byte[] FileData { get; set; }
        protected string DocumentPath { get; set; }
        protected bool ShowDocumentViewer = false;
        protected TaskDetailVM taskDetailVM { get; set; } = new TaskDetailVM();
        protected List<ManagerTaskReminderVM> managerTaskReminderData { get; set; } = new List<ManagerTaskReminderVM>();
        public SfPdfViewerServer pdfViewer; 
        #endregion

        #region On Load
        protected override async Task OnInitializedAsync()
        {
            SectorTypeId = (int)loginState.UserDetail.SectorTypeId;
            spinnerService.Show();
            await Load();
            spinnerService.Hide();
        }

        protected async Task Load()
        {
            await WithdrawRequestDetailbyId(Guid.Parse(WithdrawRequestId), int.Parse(CommunicationTypeId));
            await LoadAuthorityLetter();
            if (TaskId != null)
            {
                await PopulateTaskDetails();
                await GetManagerTaskReminderData();
            }

        }
        #endregion

        #region  Detail View 

        protected async Task WithdrawRequestDetailbyId(Guid ConsultationRequestId, int CommunicationTypeId)
        {
            try
            {
                var response = await caseRequestService.GetRequestWithdrawDetailById(ConsultationRequestId, CommunicationTypeId);
                if (response.IsSuccessStatusCode)
                {
                    getWithdrawConsultation = (WithdrawRequestDetailVM)response.ResultData;
                    await InvokeAsync(StateHasChanged);
                }
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected async Task PopulateTaskDetails()
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
        #endregion

        #region Decision 

        protected async Task Decision(WithdrawRequestDetailVM request, bool isRejected)
        {
            try
            {
                string notifMessage = "";
                string successMessage = "";
                if (isRejected == false)
                    notifMessage = translationState.Translate("Sure_Accept_WithdrawRequest");
                else
                    notifMessage = translationState.Translate("Sure_Reject_WithdrawRequest");

                bool? dialogResponse = await dialogService.Confirm(
                    notifMessage,
                    translationState.Translate("Confirm"),
                    new ConfirmOptions()
                    {
                        OkButtonText = @translationState.Translate("OK"),
                        CancelButtonText = @translationState.Translate("Cancel")
                    });

                if (dialogResponse == true)
                {
                    if (isRejected == true)
                    {
                        var dialogResult = await dialogService.OpenAsync<RejectionReason>(
                         translationState.Translate("Rejection_Reason"),
                         null,
                         new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true }

                         );
                        if (dialogResult != null)
                        {
                            request.RejectionReason = dialogResult;
                            var response = await consultationRequestService.UpdateWithDrawConsultationRequest(request, isRejected);
                            if (response.IsSuccessStatusCode)
                            {
                                successMessage = translationState.Translate("WithdrawRequest_Rejected");
                                if (TaskId != null)
                                {
                                    taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
                                    taskDetailVM.Name = "Withdraw_Request_Reject";
                                    var taskResponse = await taskService.DecisionTask(taskDetailVM);
                                    if (!taskResponse.IsSuccessStatusCode)
                                    {
                                        await invalidRequestHandlerService.ReturnBadRequestNotification(taskResponse);
                                    }
                                }
                            }
                            else
                            {
                                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                            }
                        }
                    }
                    else
                    {
                        var response = await consultationRequestService.UpdateWithDrawConsultationRequest(request, isRejected);
                        if (response.IsSuccessStatusCode)
                        {
                            successMessage = translationState.Translate("WithdrawRequest_Accepted");
                            if (TaskId != null)
                            {
                                taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
                                taskDetailVM.Name = "Withdraw_Request_Accepted";
                                var taskResponse = await taskService.DecisionTask(taskDetailVM);
                                if (!taskResponse.IsSuccessStatusCode)
                                {
                                    await invalidRequestHandlerService.ReturnBadRequestNotification(taskResponse);
                                }
                                await taskService.CompleteAllPendingTasks(request.ReferenceGuid);
                            }
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }
                    }
                    if (!successMessage.IsNullOrEmpty())
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = successMessage,
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await RedirectBack();
                    }
                    await Task.Delay(300);
                    await Load();
                }
            }
            catch (Exception)
            {

                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
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
        protected async Task BtnBack(MouseEventArgs args)
        {
            await JSRuntime.InvokeVoidAsync("history.back");
        }
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion

        #region Load Authority Letter 
        protected async Task LoadAuthorityLetter()
        {
            try
            {

                var response = new ApiCallResponse();
                ObservableCollection<TempAttachementVM> attachments = await fileUploadService.GetUploadedAttachements(false, 0, Guid.Parse(WithdrawRequestId));
                var authorityLetter = attachments?.Where(t => t.AttachmentTypeId == (int)AttachmentTypeEnum.WithdrawConsultationRequest).FirstOrDefault();
                //var authorityLetter = attachments?.FirstOrDefault(t => t.AttachmentTypeId == (int)AttachmentTypeEnum.WithdrawRequest);
                if (authorityLetter != null)
                {
                    var physicalPath = string.Empty;
#if DEBUG
                    {
                        physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + authorityLetter.StoragePath).Replace(@"\\", @"\");
                    }
#else
                    {
                            // Construct the physical path of the file on the server
                            physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + authorityLetter.StoragePath).Replace(@"\\", @"\");
                            // Remove the wwwroot/Attachments part of the path to get the actual file path
                            physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
                           
                    }
#endif

                    if (!string.IsNullOrEmpty(physicalPath))
                    {
                        FileData = await DocumentEncryptionService.GetDecryptedDocumentBytes(physicalPath, authorityLetter.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
                        string base64String = Convert.ToBase64String(FileData);
                        DocumentPath = "data:application/pdf;base64," + base64String;
                        ShowDocumentViewer = true;
                        StateHasChanged();
                    }
                    else
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Authority_Letter_Not_Loaded"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
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

        #region Get Manager Task Reminder Data
        protected async Task GetManagerTaskReminderData()
        {
            try
            {
                var response = await lookupService.GetManagerTaskReminderData(Guid.Parse(TaskId));
                if (response.IsSuccessStatusCode)
                {
                    managerTaskReminderData = (List<ManagerTaskReminderVM>)response.ResultData;
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
        #endregion
    }
}
