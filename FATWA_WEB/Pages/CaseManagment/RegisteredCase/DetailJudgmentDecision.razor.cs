using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_GENERAL.Helper;
using FATWA_WEB.Pages.CaseManagment.Shared;
using FATWA_WEB.Pages.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Radzen;
using Syncfusion.Blazor.PdfViewerServer;
using System.Collections.ObjectModel;
using Telerik.Blazor.Components;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    public partial class DetailJudgmentDecision : ComponentBase
    {
        #region Parameter
        [Parameter]
        public dynamic DecisionId { get; set; }
        #endregion

        #region Varriable
        public CmsJugdmentDecisionVM cmsJugdmentDecisionVM { get; set; } = new CmsJugdmentDecisionVM();
        public ObservableCollection<TempAttachementVM> OfficialAttachments { get; set; } = new ObservableCollection<TempAttachementVM>();
        protected byte[] FileData { get; set; } 
        protected List<AttachmentType> AttachmentTypes { get; set; } = new List<AttachmentType>();
        protected TelerikPdfViewer PdfViewerRef { get; set; }
        protected bool ShowDocumentViewer { get; set; }
        protected bool isRecordFound = true;
        public IEnumerable<CmsCaseAssigneesHistoryVM> caseFileAssigneesHistory { get; set; } = new List<CmsCaseAssigneesHistoryVM>();
        protected CmsRegisteredCaseDetailVM registeredCase { get; set; } = new CmsRegisteredCaseDetailVM();
        protected CmsCaseFileDetailVM caseFile { get; set; } = new CmsCaseFileDetailVM();
        protected CaseRequestDetailVM caseRequestdetailVm = new CaseRequestDetailVM();
        public Guid FileIdForViewDetail { get; set; }
        public List<CmsCaseAssigneeVM> caseFileAssignees { get; set; } = new List<CmsCaseAssigneeVM>();
        public SfPdfViewerServer pdfViewer;
        public string DocumentPath { get; set; } = string.Empty;
        #endregion

        #region ON Load Component
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();

            await Load();
            await LoadAuthorityLetter();
            await PopulateAttachmentTypes();
            await PopulateLawyerAssignmentHistory();
            await PopulateRegisteredCaseDetail();
            await PopulateCaseAssignees();
            await PopulateCaseFileDetail();
            await PopulateCaseRequestDetail();
            spinnerService.Hide();
        }
        protected async Task Load()
        {
            try
            {

                var response = await cmsRegisteredCaseService.GetJudgmentDecisionDetailbyId(Guid.Parse(DecisionId));
                if (response.IsSuccessStatusCode)
                {
                    cmsJugdmentDecisionVM = (CmsJugdmentDecisionVM)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Load Authority Letter
        //<History Author = 'Hassan Abbas' Date='2023-07-05' Version="1.0" Branch="master"> Modified the function to Decrypt the attachment and then view, also added functionality to convert .msg file to .pdf for preview</History>
        protected async Task LoadAuthorityLetter()
        {
            try
            {
                if (isRecordFound)
                {
                    OfficialAttachments = await fileUploadService.GetUploadedAttachements(false, 0, Guid.Parse(DecisionId));
                    var attachment = OfficialAttachments?.FirstOrDefault();
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

        //<History Author = 'Hassan Abbas' Date='2023-07-05' Version="1.0" Branch="master"> Modified the function to Decrypt the attachment and then view, also added functionality to convert .msg file to .pdf for preview</History>
        protected async Task ViewAttachement(TempAttachementVM theUpdatedItem)
        {
            try
            {
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

        #endregion

        #region Populate attachment type grid 
        protected async Task PopulateAttachmentTypes()
        {
            var response = await lookupService.GetAttachmentTypes((int)WorkflowModuleEnum.CaseManagement);
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
        protected async Task PopulateLawyerAssignmentHistory()
        {
            var response = await cmsCaseFileService.GetCaseAssigmentHistory(cmsJugdmentDecisionVM.CaseId);
            if (response.IsSuccessStatusCode)
            {
                caseFileAssigneesHistory = (List<CmsCaseAssigneesHistoryVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        protected async Task PopulateRegisteredCaseDetail()
        {
            var result = await cmsRegisteredCaseService.GetRegisteredCaseDetailByIdVM(cmsJugdmentDecisionVM.CaseId);
            if (result.IsSuccessStatusCode)
            {
                registeredCase = (CmsRegisteredCaseDetailVM)result.ResultData;
                FileIdForViewDetail = (Guid)registeredCase.FileId;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
        }
        protected async Task PopulateCaseAssignees()
        {
            var response = await cmsCaseFileService.GetCaseAssigeeList(FileIdForViewDetail);
            if (response.IsSuccessStatusCode)
            {
                caseFileAssignees = (List<CmsCaseAssigneeVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        protected async Task PopulateCaseFileDetail()
        {
            var result = await cmsCaseFileService.GetCaseFileDetailByIdVM(FileIdForViewDetail);
            if (result.IsSuccessStatusCode)
            {
                caseFile = (CmsCaseFileDetailVM)result.ResultData;

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
        }
        protected async Task PopulateCaseRequestDetail()
        {
            var request = await caseRequestService.GetCaseRequestDetailById(caseFile.RequestId);
            if (request.IsSuccessStatusCode)
            {
                caseRequestdetailVm = JsonConvert.DeserializeObject<CaseRequestDetailVM>(request.ResultData.ToString());
                //caseRequestdetailVm = (CaseRequestDetailVM)request.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(request);
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

        #region Buttons
        protected async Task DraftAFile(MouseEventArgs args)
        {
            var result = await dialogService.OpenAsync<SelectDraftTemplatePopup>(translationState.Translate("Add_Draft"),
            new Dictionary<string, object>()
            {
                    { "ReferenceId", Convert.ToString(cmsJugdmentDecisionVM.CaseId) },
                    { "ModuleId", (int)WorkflowModuleEnum.CaseManagement },
                    { "DraftEntityType",  (int)DraftEntityTypeEnum.Case},
                    { "DocumentTypes", await identifyDocumentTypes.GetDocumentTypes(registeredCase,(int)DraftEntityTypeEnum.Case, loginState.UserDetail.SectorTypeId != null ? (int)loginState.UserDetail.SectorTypeId : 0)},
            }
            ,
            new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
        }
        protected async Task AddOpinionDocument(MouseEventArgs args)
        {
            var result = await dialogService.OpenAsync<SelectDraftTemplatePopup>(translationState.Translate("Add_Draft"),
            new Dictionary<string, object>()
                {
                        { "ReferenceId", Convert.ToString(cmsJugdmentDecisionVM.CaseId) },
                        { "DraftEntityType",  (int)DraftEntityTypeEnum.Case},
                        { "Document_Type" ,(int)AttachmentTypeEnum.OpinionDocument},
                        { "DocumentTypes", await identifyDocumentTypes.GetDocumentTypes(registeredCase,(int)DraftEntityTypeEnum.Case, loginState.UserDetail.SectorTypeId != null ? (int)loginState.UserDetail.SectorTypeId : 0)},
                }
                ,
                new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
        }
        protected async Task MeetingRequest(MouseEventArgs args)
        {
            try
            {
                Guid MeetingId = Guid.Empty;
                Guid CommunicationId = Guid.Empty;
                Guid ReferenceId = cmsJugdmentDecisionVM.CaseId;
                int SubModuleId = (int)SubModuleEnum.RegisteredCase;
                int SectorTypeId = 0;
                string ReceivedBy = caseRequestdetailVm.CreatedBy;
                navigationManager.NavigateTo("/meeting-add/" + MeetingId + "/" + CommunicationId + "/" + ReferenceId + "/" + SubModuleId + "/" + SectorTypeId + "/" + ReceivedBy);
                //await dialogService.OpenAsync<SaveMeeting>(
                //    translationState.Translate("Schedule_Meeting"),
                //    new Dictionary<string, object>()
                //    {
                //        { "ReferenceId", registeredCase.CaseId },
                //    },
                //    new DialogOptions() { CloseDialogOnOverlayClick = true, Width = "900px" });
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
        protected async Task Assign(MouseEventArgs args)
        {
            var dialogResult = await dialogService.OpenAsync<TransferSector>(
                translationState.Translate("Assign_Case_File"),
                new Dictionary<string, object>() {
                    { "ReferenceId", FileIdForViewDetail },
                    { "TransferCaseType", (int)AssignCaseToLawyerTypeEnum.CaseFile },
                    { "IsAssignment", true },
                },
                new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
            if (dialogResult != null)
            {
                await Task.Delay(300);
                await RedirectBack();
                await Load();
            }
        }
        protected async Task AssignToLawyer()
        {
            var result = await dialogService.OpenAsync<AssignDecisionToLawyer>(translationState.Translate("Assign_To_Lawyer"),
               new Dictionary<string, object>()
               {
                { "DecisionId", Guid.Parse(DecisionId) },
               }
               ,
                new DialogOptions() { Width = "50% !important", CloseDialogOnOverlayClick = true });
            await Load();


        }

        protected async Task SendToMoj()
        {
            try
            {

                bool? dialogResponse = await dialogService.Confirm(
                      translationState.Translate("Approve_Confirm"),
                      translationState.Translate("Confirm"),
                      new ConfirmOptions()
                      {
                          OkButtonText = translationState.Translate("OK"),
                          CancelButtonText = translationState.Translate("Cancel")
                      });
                if (dialogResponse == true)
                {
                    spinnerService.Show();
                    cmsJugdmentDecisionVM.StatusId = (int)CaseDecisionStatusEnum.SendToMoj;
                    var response = await cmsRegisteredCaseService.SendDecisionToMoj(cmsJugdmentDecisionVM);
                    spinnerService.Hide();
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Request_Sent_To_Moj"),
                        Style = "position: fixed !important; left: 0; margin: auto;"
                    });

                }
            }
            catch (Exception ex)
            {
                spinnerService.Hide();
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        #endregion

        #region Approve/Reject
        protected async Task RejectDecision()
        {
            try
            {

                bool? dialogResponse = await dialogService.Confirm(
                      translationState.Translate("Reject_Confirm"),
                      translationState.Translate("Confirm"),
                      new ConfirmOptions()
                      {
                          OkButtonText = translationState.Translate("OK"),
                          CancelButtonText = translationState.Translate("Cancel")
                      });
                if (dialogResponse == true)
                {
                    spinnerService.Show();
                    cmsJugdmentDecisionVM.StatusId = (int)CaseDecisionStatusEnum.Rejected;
                    var response = await cmsRegisteredCaseService.RejectDecision(cmsJugdmentDecisionVM);
                    spinnerService.Hide();
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Changes_saved_successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
            }
            catch (Exception ex)
            {
                spinnerService.Hide();
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        protected async Task ApproveDecision()
        {
            try
            {

                bool? dialogResponse = await dialogService.Confirm(
                      translationState.Translate("Approve_Confirm"),
                      translationState.Translate("Confirm"),
                      new ConfirmOptions()
                      {
                          OkButtonText = translationState.Translate("OK"),
                          CancelButtonText = translationState.Translate("Cancel")
                      });
                if (dialogResponse == true)
                {
                    spinnerService.Show();
                    cmsJugdmentDecisionVM.StatusId = (int)CaseDecisionStatusEnum.Approved;
                    var response = await cmsRegisteredCaseService.ApproveDecision(cmsJugdmentDecisionVM);
                    spinnerService.Hide();
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Changes_saved_successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
            }
            catch (Exception ex)
            {
                spinnerService.Hide();
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        #endregion

        #region Add Prepare Execution
        //<History Author = 'Hassan Abbas' Date='2023-03-04' Version="1.0" Branch="master">Add Prepare Execution</History>
        protected async Task AddPrepareExecution()
        {
            try
            {
                var result = await dialogService.OpenAsync<AddExecution>(translationState.Translate("Prepare_Execution"),
                    new Dictionary<string, object>()
                    {
                        { "DecisionId", cmsJugdmentDecisionVM.Id },
                        { "CaseId", cmsJugdmentDecisionVM.CaseId },
                    }
                    , new DialogOptions()
                    {
                        CloseDialogOnOverlayClick = true,
                        Width = "80% !important"
                    });
                await Task.Delay(300);
                if (result != null)
                {
                    await RedirectBack();
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

