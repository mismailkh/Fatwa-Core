using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_GENERAL.Helper;
using FATWA_WEB.Pages.Shared;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Syncfusion.Blazor.PdfViewerServer;
using System.Collections.ObjectModel;
using Telerik.Blazor.Components;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.CaseManagment.Shared
{
    public partial class DetailCommunication : ComponentBase
    {
        #region Parameter

        [Parameter]
        public dynamic ReferenceId { get; set; }
        [Parameter]
        public dynamic CommunicationId { get; set; }

        [Parameter]
        public dynamic SubModuleId { get; set; }

        [Parameter]
        public dynamic ByActivity { get; set; }
        [Parameter]
        public dynamic TaskId { get; set; }
        #endregion

        #region Variables
        public int SubModule { get { return Convert.ToInt32(SubModuleId); } set { SubModuleId = value; } }
        public int Activity { get { return Convert.ToInt32(ByActivity); } set { ByActivity = value; } }
        protected bool ShowDocumentViewer { get; set; }
        public ObservableCollection<TempAttachementVM> OfficialAttachments { get; set; } = new ObservableCollection<TempAttachementVM>();
        protected byte[] FileData { get; set; }
        protected string DocumentPath { get; set; }
        protected TelerikPdfViewer PdfViewerRef { get; set; }
        public List<CmsDraftedTemplateVM> DraftDocuments { get; set; } = new List<CmsDraftedTemplateVM>();
        public List<CmsDraftedDocumentVM> DraftTemplate { get; set; } = new List<CmsDraftedDocumentVM>();
        protected List<AttachmentType> AttachmentTypes { get; set; } = new List<AttachmentType>();
        public bool Render = false;
        protected bool isRecordFound = true;
        public string TransKeyHeader = string.Empty;
        public string TransKeyUrl = string.Empty;
        protected CommunicationDetailVM communicationDetail;
        protected SaveTaskVM saveTaskVm { get; set; }
        protected TaskDetailVM taskDetailVM { get; set; } = new TaskDetailVM();
        protected ObservableCollection<TempAttachementVM> PublishedDocument = new ObservableCollection<TempAttachementVM>();
        protected byte[] PublishedDocumentFileData { get; set; }
        public CmsApprovalTracking approvalTracking { get; set; } = new CmsApprovalTracking { StatusId = (int)ApprovalStatusEnum.Pending, CreatedDate = DateTime.Now };
        public bool IsHOS = false;
        protected RadzenDataGrid<CorrespondenceHistoryVM> CorrespondenceHistoryGrid = new RadzenDataGrid<CorrespondenceHistoryVM>();
        protected List<CorrespondenceHistoryVM> CorrespondenceHistory = new();
        protected List<ManagerTaskReminderVM> managerTaskReminderData { get; set; } = new List<ManagerTaskReminderVM>();
        public SfPdfViewerServer pdfViewer; 
        #endregion

        #region Send Response Detail View Grid Load Properties Load

        CommunicationSendResponseVM _communicationResponseVM;
        protected CommunicationSendResponseVM communicationResponseVM
        {
            get
            {
                return _communicationResponseVM;
            }
            set
            {
                if (!object.Equals(_communicationResponseVM, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "CommunicationSendResponseVM", NewValue = value, OldValue = _communicationResponseVM };
                    _communicationResponseVM = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        #endregion

        #region Model full property Instance

        public string CaseRequestUrlS { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {

        }
        #endregion

        #region Component Load

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            if (loginState.UserRoles.Any(r => SystemRoles.HOS.Contains(r.RoleId)) || loginState.UserRoles.Any(r => SystemRoles.ComsHOS.Contains(r.RoleId)) || loginState.UserRoles.Any(r => SystemRoles.ViceHOS.Contains(r.RoleId)))
            {
                IsHOS = true;
            }
            PopulateTranslationKey();
            await PopulateAttachmentTypes();
            await CommunicationDetailbyComIdAndComType();
            await ViewDraftSendByHOS();
            if (TaskId != null)
            {
                await PopulateTaskDetails();
                await GetManagerTaskReminderData();
            }
            if (Activity == (int)CommunicationTypeEnum.G2GTarasolCorrespondence)
            {
                await GetCorrespondenceHistory();
            }
            await LoadAuthorityLetter();
            spinnerService.Hide();
        }

        protected async Task PopulateAttachmentTypes()
        {
            var response = await lookupService.GetAttachmentTypes(0);
            if (response.IsSuccessStatusCode)
            {
                AttachmentTypes = (List<AttachmentType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        public void PopulateTranslationKey() {

            if (SubModule == (int)SubModuleEnum.CaseFile)
            {
                CaseRequestUrlS = @"/case-files";
                TransKeyUrl = "Case_Files";
            }
            else if (SubModule == (int)SubModuleEnum.CaseRequest)
            {
                CaseRequestUrlS = @"/case-requests";
                TransKeyUrl = "Case_Requests";
            }
            else if (SubModule == (int)SubModuleEnum.RegisteredCase)
            {
                CaseRequestUrlS = @"/case-files";
                TransKeyUrl = "Case_Files";
            }
            else if (SubModule == (int)SubModuleEnum.ConsultationFile)
            {
                CaseRequestUrlS = @"/consultationfile-list";
                TransKeyUrl = "Consultation_File";
            }
            else if (SubModule == (int)SubModuleEnum.ConsultationRequest)
            {
                CaseRequestUrlS = @"/consultationrequest-list";
                TransKeyUrl = "Consultation_Request";
            }
            else
            {
                CaseRequestUrlS = @"/inboxOutbox-list";
                TransKeyUrl = "Correspondences";
            }
            if (Activity == (int)CommunicationTypeEnum.SendResponse)
            {
                TransKeyHeader = "Communication_Response_Send_Response_Detail";
            }
            else if (Activity == (int)CommunicationTypeEnum.SendMessage)
            {
                TransKeyHeader = "Communication_Response_Send_Message_Detail";
            }
            else if (Activity == (int)CommunicationTypeEnum.CaseRequest)
            {
                TransKeyHeader = "Case_Request_Correspondence_Detail";
            }
            else
            {
                TransKeyHeader = "Request_Need_More_Details";
            }
            StateHasChanged();
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

        //<History Author = 'Hassan Abbas' Date='2023-07-05' Version="1.0" Branch="master"> Modified the function to Decrypt the attachment and then view, also added functionality to convert .msg file to .pdf for preview</History>
        protected async Task LoadAuthorityLetter()
        {
            try
            {
                if (isRecordFound)
                {
                    ShowDocumentViewer = false;
                    OfficialAttachments = await fileUploadService.GetUploadedAttachements(false, 0, Guid.Parse(CommunicationId));
                    var attachment = OfficialAttachments.Where(x => x.AttachmentTypeId == (int)AttachmentTypeEnum.CmsLegalNotification
                                                                || x.AttachmentTypeId == (int)AttachmentTypeEnum.LegalNotificationResponse
                                                                || x.AttachmentTypeId == (int)AttachmentTypeEnum.OfficialLetter)?.FirstOrDefault();
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

        protected async Task CommunicationDetailbyComIdAndComType()
        {
            try
            {
                ApiCallResponse res = new ApiCallResponse();
                //if (SubModule == (int)LinkTargetTypeEnum.Communication)

                //{
                //    res = await communicationService.CommunicationDetailbyComIdAndComType(ReferenceId, Guid.Parse(CommunicationId), SubModule, Activity);

                //}

                res = await communicationService.CommunicationDetailbyComIdAndComType(Guid.Parse(ReferenceId), Guid.Parse(CommunicationId), SubModule, Activity);


                if (res.IsSuccessStatusCode)
                {
                    communicationDetail = (CommunicationDetailVM)res.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(res);
                }
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region FILE PREVIEW

        //<History Author = 'Hassan Abbas' Date='2023-07-05' Version="1.0" Branch="master"> Modified the function to Decrypt the attachment and then view, also added functionality to convert .msg file to .pdf for preview</History>
        protected async Task ViewAttachement(TempAttachementVM theUpdatedItem)
        {
            try
            {
                ShowDocumentViewer = false;
                var physicalPath = string.Empty;
#if DEBUG
                {
                    physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + theUpdatedItem.StoragePath).Replace(@"\\", @"\");
                }
#else
                {
                    physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + theUpdatedItem.StoragePath).Replace(@"\\", @"\");
                    physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
                }
#endif
                if (!String.IsNullOrEmpty(physicalPath))
                {
                    FileData = await DocumentEncryptionService.GetDecryptedDocumentBytes(physicalPath, theUpdatedItem.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
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
                        Detail = translationState.Translate("File_Not_Found"),
                        Summary = translationState.Translate("Error"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
            }
            catch (Exception)
            {

                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Summary = translationState.Translate("Error"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }


        }

        protected async Task ViewDraftSendByHOS()
        {
            try
            {
                PublishedDocument = await fileUploadService.GetUploadedAttachements(false, 0, Guid.Parse(ReferenceId));

                if (PublishedDocument != null)
                {
                    var DocForView = PublishedDocument.Where(x => x.AttachmentTypeId == (int)AttachmentTypeEnum.RequestForStopExecutionOfJudgment).First();

                    var physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + DocForView.StoragePath).Replace(@"\\", @"\");
                    PublishedDocumentFileData = await DocumentEncryptionService.GetDecryptedDocumentBytes(physicalPath, DocForView.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
                    StateHasChanged();
                }
            }
            catch
            {

            }
        }
        #endregion

        #region Redirect Functions

        protected async Task TarassolReply(MouseEventArgs args)
        {
            navigationManager.NavigateTo($"/communicationtarassol-add/{CommunicationId}/{communicationDetail.GovtEntityId}/{communicationDetail.DepartmentId}");
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
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        protected void BtnView(MouseEventArgs args)
        {
            string url = "";
            if (SubModule == (int)SubModuleEnum.CaseRequest)
            {
                url = $"/caserequest-view/{ReferenceId}";
            }
            else if (SubModule == (int)SubModuleEnum.CaseFile)
            {
                url = $"/casefile-view/{ReferenceId}";
            }
            else if (SubModule == (int)SubModuleEnum.RegisteredCase)
            {
                url = $"/case-view/{ReferenceId}";
            }
            else if (SubModule == (int)SubModuleEnum.ConsultationFile)
            {
                url = $"/consultationfile-view/{ReferenceId}/" + loginState.UserDetail.SectorTypeId;
            }
            else if (SubModule == (int)SubModuleEnum.ConsultationRequest)
            {
                url = $"/consultationrequest-detail/{ReferenceId}/" + loginState.UserDetail.SectorTypeId;

            }
            navigationManager.NavigateTo(url);
        }
        // NEW CODE
        protected void TarassolResponse(MouseEventArgs args)
        {

        }
        protected void NeedMoreInfo(MouseEventArgs args)
        {
            if (SubModule == (int)SubModuleEnum.CaseRequest)
            {
                navigationManager.NavigateTo("/Request-For-More-Information/" + ReferenceId + "/" + true + "/" + CommunicationId + "/" + false + "/" + TaskId);
            }
            if (SubModule == (int)SubModuleEnum.CaseFile)
            {
                navigationManager.NavigateTo("/Request-For-More-Information/" + ReferenceId + "/" + false + "/" + CommunicationId + "/" + false + "/" + TaskId);
            }
            else if (SubModule == (int)SubModuleEnum.RegisteredCase)
            {
                navigationManager.NavigateTo("/Case-Request-For-More-Information/" + ReferenceId + "/" + CommunicationId + "/" + TaskId);
            }
            else if (SubModule == (int)SubModuleEnum.ConsultationRequest)
            {
                navigationManager.NavigateTo("/Request-For-More-Information/" + ReferenceId + "/" + false + "/" + CommunicationId + "/" + false + "/" + TaskId);
            }
            else if (SubModule == (int)SubModuleEnum.ConsultationFile)
            {
                navigationManager.NavigateTo("/Coms-Request-For-More-Information/" + ReferenceId + "/" + false + "/" + CommunicationId + "/" + false + "/" + TaskId);

            }
        }
        protected async Task DraftAFile(MouseEventArgs args)
        {
            StopExecutionPayloadVM ApnaPayload = new StopExecutionPayloadVM
            {
                CommunicationId = CommunicationId,
                ReferenceId = ReferenceId,
                CommunicationTypeId = ByActivity,
                SubModuleId = SubModule
            };
            var result = await dialogService.OpenAsync<SelectDraftTemplatePopup>(translationState.Translate("Add_Draft"),
             new Dictionary<string, object>()
                 {
                        { "ReferenceId", ReferenceId },
                        { "ModuleId", (int)WorkflowModuleEnum.CaseManagement },
                        { "DraftEntityType",  (int)DraftEntityTypeEnum.StopExecutionOfJudgment},
                        { "Payload", Newtonsoft.Json.JsonConvert.SerializeObject(ApnaPayload) },
                        { "DocumentTypes", await identifyDocumentTypes.GetDocumentTypes(null,(int)DraftEntityTypeEnum.StopExecutionOfJudgment, loginState.UserDetail.SectorTypeId != null ? (int)loginState.UserDetail.SectorTypeId : 0)},
                 }
                 ,
                 new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
        }
        protected async Task ApproveFile(MouseEventArgs args)
        {
            approvalTracking.SectorFrom = (int)OperatingSectorTypeEnum.CivilCommercialRegionalCases;
            approvalTracking.SectorTo = (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases;
            approvalTracking.ReferenceId = Guid.Parse(ReferenceId);
            approvalTracking.CreatedBy = loginState.Username;
            approvalTracking.ProcessTypeId = (int)ApprovalProcessTypeEnum.Transfer;

            var transferResponse = await cmsSharedService.AddTransferTaskToPartialUrgentSector(approvalTracking, (int)AssignCaseToLawyerTypeEnum.CaseFile);
            if (transferResponse.IsSuccessStatusCode)
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
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(transferResponse);
                return;
            }

        }
        protected async Task RejectFile(MouseEventArgs args)
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
                    StopExecutionPayloadVM payload = new StopExecutionPayloadVM
                    {
                        CommunicationId = CommunicationId,
                        ReferenceId = ReferenceId,
                        CommunicationTypeId = ByActivity,
                        SubModuleId = SubModule,
                    };

                    var stopExecutionRejectionReason = new StopExecutionRejectionReason
                    {
                        Id = Guid.NewGuid(),
                        CommunicationId = (string)CommunicationId,
                        Reason = dialogResult,
                        Payload = Newtonsoft.Json.JsonConvert.SerializeObject(
                            new StopExecutionPayloadVM
                            {
                                CommunicationId = CommunicationId,
                                ReferenceId = ReferenceId,
                                CommunicationTypeId = ByActivity,
                                SubModuleId = SubModule,
                            }),
                        CreatedBy = loginState.UserDetail.UserName,
                        AssignById = loginState.UserDetail.UserId
                    };

                    var resp = await communicationService.StopExecutionRejectionReason(stopExecutionRejectionReason);
                    if (resp.IsSuccessStatusCode)
                    {
                        /*   if (TaskId != null)
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
                           });*/
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(resp);
                    }
                    await RedirectBack();
                }

            }
        }
        #endregion

        #region Task Status update
        protected async Task CompleteTask(MouseEventArgs args)
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Complete"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                if (TaskId != null)
                {
                    taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
                    var taskResponse = await taskService.DecisionTask(taskDetailVM);
                    if (!taskResponse.IsSuccessStatusCode)
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(taskResponse);
                    }
                    await RedirectBack();
                }
            }

        }
        protected async Task SaveAndCloseFile(MouseEventArgs args)
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Save_and_Close_File"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }
            ) == true)
            {
                if (TaskId != null)
                {
                    string userName = loginState.Username;
                    var taskResponse = await cmsCaseFileService.UpdateCaseFileStatusandAddHistory(Guid.Parse(ReferenceId), userName);

                    taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
                    var taskResponse2 = await taskService.DecisionTask(taskDetailVM);
                    if (!taskResponse.IsSuccessStatusCode || !taskResponse2.IsSuccessStatusCode)
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(taskResponse);
                    }
                    await RedirectBack();
                }
            }

        }
        #endregion

        #region 
        protected async Task ForwardToLawyer(MouseEventArgs args)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<CorrespondenceForwardToLawyer>(
                    translationState.Translate("Forward_To_Lawyer"),
                    new Dictionary<string, object>() { { "CommunicationId", CommunicationId } },
                    new DialogOptions() { CloseDialogOnOverlayClick = true });
                await Task.Delay(300);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected async Task ForwardToSector(MouseEventArgs args)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<CorrespondenceForwardToSector>(
                    translationState.Translate("Forward_To_Sector"),
                    new Dictionary<string, object>() { { "CommunicationId", CommunicationId } },
                    new DialogOptions() { CloseDialogOnOverlayClick = true });
                await Task.Delay(300);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected async Task SendBackToHos(MouseEventArgs args)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<CorrespondenceReturnToHos>(
                    translationState.Translate("Forward_To_Sector"),
                    new Dictionary<string, object>() { { "CommunicationId", CommunicationId } },
                    new DialogOptions() { CloseDialogOnOverlayClick = true });
                await Task.Delay(300);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected async Task SendBackToSender(MouseEventArgs args)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<CorrespondenceSendBackToSender>(
                    translationState.Translate("Forward_To_Sector"),
                    new Dictionary<string, object>() { { "CommunicationId", CommunicationId } },
                    new DialogOptions() { CloseDialogOnOverlayClick = true });
                await Task.Delay(300);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task GetCorrespondenceHistory()
        {
            var history = await communicationService.GetCorrespondenceHistoryByCommunicationId(Guid.Parse(CommunicationId));
            if (history.IsSuccessStatusCode)
            {
                CorrespondenceHistory = (List<CorrespondenceHistoryVM>)history.ResultData;
            }
            else
            {
                CorrespondenceHistory = new List<CorrespondenceHistoryVM>();
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


