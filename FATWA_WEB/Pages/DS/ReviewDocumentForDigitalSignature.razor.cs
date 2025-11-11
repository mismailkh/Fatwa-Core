using FATWA_DOMAIN.Enums;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.Dms;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_GENERAL.Helper;
using FATWA_WEB.Pages.CaseManagment.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Syncfusion.Blazor.PdfViewerServer;
using Telerik.Blazor.Components;
using static FATWA_DOMAIN.Enums.DmsEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;

namespace FATWA_WEB.Pages.DS
{
    public partial class ReviewDocumentForDigitalSignature : ComponentBase
    {
        #region Parameter
        [Parameter]
        public string SigningTaskId { get; set; }
        [Parameter]
        public dynamic SubModuleId { get; set; }
        [Parameter]
        public string TaskId { get; set; }
        #endregion

        #region Variables
        protected bool ShowDocumentViewer { get; set; }
        protected byte[] FileData { get; set; }
        protected TelerikPdfViewer PdfViewerRef { get; set; }
        protected bool isRecordFound = true;
        protected DsSigningRequestTaskLog taskForDS = new DsSigningRequestTaskLog();
        protected UploadedDocument attachment = new UploadedDocument();
        protected TaskDetailVM taskDetailVM { get; set; } = new TaskDetailVM();
        public int SubModule { get { return Convert.ToInt32(SubModuleId); } set { SubModuleId = value; } }
        public List<CmsDraftedDocumentReasonVM> DraftDocumentReasons { get; set; } = new List<CmsDraftedDocumentReasonVM>();
        protected RadzenDataGrid<CmsDraftedDocumentReasonVM>? grid = new RadzenDataGrid<CmsDraftedDocumentReasonVM>();
        public SfPdfViewerServer pdfViewer;
        public string DocumentPath { get; set; } = string.Empty; 
        #endregion

        #region Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            await LoadDocument();
            await PopulateTaskDetails();
            spinnerService.Hide();
        }
        #endregion

        protected async Task Load()
        {
            var response = await fileUploadService.GetTaskForSignature(Guid.Parse(SigningTaskId));
            if (response.IsSuccessStatusCode)
            {
                taskForDS = (DsSigningRequestTaskLog)response.ResultData;
                var newReason = new CmsDraftedDocumentReasonVM
                {
                    Reason = taskForDS.Remarks,
                    UserNameEn = taskForDS.SenderName_En,
                    UserNameAr = taskForDS.SenderName_Ar,
                    CreatedDate = taskForDS.CreatedDate
                };
                DraftDocumentReasons.Add(newReason);
                await grid.Reload();
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
        protected async Task LoadDocument()
        {
            try
            {
                if (isRecordFound)
                {
                    ShowDocumentViewer = false;
                    attachment = await fileUploadService.GetUploadedAttachementById(taskForDS.DocumentId);
                    if (attachment != null)
                    {
                        var physicalPath = string.Empty;
#if DEBUG
                        {
                            physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + attachment.StoragePath).Replace(@"\\", @"\");
                        }
#else
{

                            physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + attachment.StoragePath).Replace(@"\\", @"\");
                            physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
}
#endif
                        if (!string.IsNullOrEmpty(physicalPath))
                        {
                            FileData = await DocumentEncryptionService.GetDecryptedDocumentBytes(physicalPath, attachment.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
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
            }
            catch
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        protected async Task RejectToSignDocument(MouseEventArgs args)
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
                var dialogResult = await dialogService.OpenAsync<RejectionReason>(
                  translationState.Translate("Rejection_Reason"),
                  null,
                  new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true }
                );
                if (dialogResult != null)
                {
                    taskForDS.StatusId = (int)SigningTaskStatusEnum.Rejected;
                    taskForDS.RejectionReason = dialogResult;
                    taskForDS.ModifiedBy = loginState.Username;
                    taskForDS.ModifiedDate = DateTime.Now;

                    var resp = await fileUploadService.UpdateTaskForSignature(taskForDS);
                    if (resp.IsSuccessStatusCode)
                    {
                        await UpdateTaskStatus();
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(resp);
                    }
                    await RedirectBack();
                }

            }
        }
        protected async Task SignDocument()
        {
            DsSigningResponseVM dialogResult = await dialogService.OpenAsync<DigitalSignature>(
                 translationState.Translate("Digital_Signature"),
                 new Dictionary<string, object>() {
                    { "DocumentId", taskForDS.DocumentId },
                    { "AttachmentTypeId", attachment.AttachmentTypeId },
                    { "StatusId", taskForDS.StatusId }
                 },
                 new DialogOptions() { Width = "32% !important", CloseDialogOnOverlayClick = true });
            if (dialogResult != null && dialogResult.RequestStatus == SigningRequestStatusEnum.Approved.GetDisplayName())
            {
                taskForDS.StatusId = (int)SigningTaskStatusEnum.Signed;
                taskForDS.ModifiedBy = loginState.Username;
                taskForDS.ModifiedDate = DateTime.Now;
                taskForDS.SigningMethodId = dialogResult.SigningMethodId;
                var resp = await fileUploadService.UpdateTaskForSignature(taskForDS);
                if (resp.IsSuccessStatusCode)
                {
                    await UpdateTaskStatus();
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(resp);
                }
            }
            if (dialogResult != null && dialogResult.RequestStatus == SigningRequestStatusEnum.Failed.GetDisplayName())
            {
                taskForDS.StatusId = (int)SigningTaskStatusEnum.Failed;
                taskForDS.ModifiedBy = loginState.Username;
                taskForDS.ModifiedDate = DateTime.Now;
                taskForDS.SigningMethodId = dialogResult.SigningMethodId;
                var resp = await fileUploadService.UpdateTaskForSignature(taskForDS);
                if (resp.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Changes_saved_successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(resp);
                }
            }
        }
        protected async Task UpdateTaskStatus()
        {
            if (TaskId != null)
            {
                taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
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
        }
        protected void BtnView(MouseEventArgs args)
        {
            var url = "";
            if (SubModule == (int)SubModuleEnum.CaseRequest)
            {
                url = $"/caserequest-view/{taskForDS.ReferenceId}";
            }
            else if (SubModule == (int)SubModuleEnum.CaseFile)
            {
                url = $"/casefile-view/{taskForDS.ReferenceId}";
            }
            else if (SubModule == (int)SubModuleEnum.RegisteredCase)
            {
                url = $"/case-view/{taskForDS.ReferenceId}";
            }
            else if (SubModule == (int)SubModuleEnum.ConsultationFile)
            {
                url = $"/consultationfile-view/{taskForDS.ReferenceId}/" + loginState.UserDetail.SectorTypeId;
            }
            else if (SubModule == (int)SubModuleEnum.ConsultationRequest)
            {
                url = $"/consultationrequest-detail/{taskForDS.ReferenceId}/" + loginState.UserDetail.SectorTypeId;
            }
            navigationManager.NavigateTo(url);
        }

        #region Redirect Function
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
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
        #endregion

    }
}
