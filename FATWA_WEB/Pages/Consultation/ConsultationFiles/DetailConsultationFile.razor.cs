using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_DOMAIN.Models.ViewModel.ContactManagmentVMs;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_WEB.Pages.Consultation.Shared;
using FATWA_WEB.Pages.ContactManagment;
using FATWA_WEB.Pages.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;

namespace FATWA_WEB.Pages.Consultation.ConsultationFiles
{
    public partial class DetailConsultationFile : ComponentBase
    {
        #region Parameter
        [Parameter]
        public dynamic FileId { get; set; }
        [Parameter]
        public dynamic SectorTypeId { get; set; }
        [Parameter]
        public string? TaskId { get; set; }
        #endregion

        #region Varriable
        protected RadzenDataGrid<CmsDraftedTemplateVM> gridTemplate;
        protected RadzenDataGrid<CmsDraftedDocumentVM> gridVersion;
        public List<CmsDraftedTemplateVM> DraftDocuments { get; set; } = new List<CmsDraftedTemplateVM>();
        public List<CmsDraftedDocumentVM> DraftTemplate { get; set; } = new List<CmsDraftedDocumentVM>();
        public List<CmsDraftedDocumentVM> DraftedTemplateVersions { get; set; } = new List<CmsDraftedDocumentVM>();
        public IList<CmsDraftedDocumentVM> selectedDraftsVersion;

        public bool allowRowSelectOnRowClick = true;
        protected ConsultationFileDetailVM consultationFile { get; set; }
        protected List<ConsultationFileHistoryVM> consultationFileHistory { get; set; }
        int SectorTypeIdCheck = 0;
        public IEnumerable<CommunicationListVM> communicationListVm = new List<CommunicationListVM>();
        protected RadzenDataGrid<CommunicationListVM> CommunicationGrid;
        protected RadzenDataGrid<ConsultationFileHistoryVM> HistoryGrid;
        public List<ConsultationFileAssignmentVM> consultationFileAssignee = new List<ConsultationFileAssignmentVM>();
        public List<ConsultationFileAssignmentHistoryVM> consultationFileAssignmentHistoryVM;
        protected RadzenDataGrid<ConsultationFileAssignmentHistoryVM> AssigneeGrid;
        public ViewConsultationVM consultationRequestVM;
        public string ActivityEn;
        public string ActivityAr;
        protected string InboxNumber;
        protected string OutboxNumber;
        protected bool RefreshFileUploadGrid { get; set; } = true;
        protected string RedirectURL { get; set; }
        protected RadzenDataGrid<ConsultationPartyListVM>? consultationPartyGrid = new RadzenDataGrid<ConsultationPartyListVM>();
        protected RadzenDataGrid<ConsultationArticleByConsultationIdListVM>? consultationArticleGrid = new RadzenDataGrid<ConsultationArticleByConsultationIdListVM>();
        public string TransKeyHeader = string.Empty;
        protected RadzenDataGrid<CmsTransferHistoryVM> transferHistoryGrid;
        protected IEnumerable<CmsTransferHistoryVM> cmsCaseRequestTransferHistory = new List<CmsTransferHistoryVM>();
        protected CmsTransferHistoryVM caseRequestTransferHistoryobj = new CmsTransferHistoryVM();

        protected RadzenDataGrid<ContactFileLinkVM> ContactGrid = new RadzenDataGrid<ContactFileLinkVM>();
        protected IList<ContactFileLinkVM> contactListForFileVm { get; set; } = new List<ContactFileLinkVM>();
        protected TaskDetailVM taskDetailVM { get; set; }
        protected List<ManagerTaskReminderVM> managerTaskReminderData { get; set; } = new List<ManagerTaskReminderVM>();

        public bool IsReqeustLoad { get; set; }
        #endregion

        #region On Load Component
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            spinnerService.Hide();
        }

        protected async Task Load()
        {
            spinnerService.Show();

            SectorTypeIdCheck = Convert.ToInt32(SectorTypeId);
            PopulateTranslationKey();
            await GetConsultationFileDetail();
            await PopulateConslutationFileCommunicationGrid();
            await PopulateConslutationFileStatusHistory();
            await PopulateConsultationAssigmentHistory();
            await PopulateFileAssignees();
            await PopulateConsultationRequestGrid();
            await PopulateRequestTransferHistoryGrid((string)FileId);

            translationState.TranslateGridFilterLabels(CommunicationGrid);
            translationState.TranslateGridFilterLabels(HistoryGrid);
            translationState.TranslateGridFilterLabels(AssigneeGrid);
            await GetContactDetailsForFile(Guid.Parse(FileId));
            await PopulateDraftGrid();
            if (TaskId != null)
            {
                await PopulateTaskDetails();
                await GetManagerTaskReminderData();
            }
            spinnerService.Hide();

        }
        #endregion

        #region Populate Grids
        protected async Task GetConsultationFileDetail()
        {
            try
            {
                var result = await consultationFileService.GetConsultationFileDetailById(Guid.Parse(FileId));
                if (result.IsSuccessStatusCode)
                {
                    consultationFile = (ConsultationFileDetailVM)result.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(result);

                }
            }
            catch (Exception ex)
            {

            }
        }
        protected async Task PopulateRequestTransferHistoryGrid(string FileId)
        {
            var historyResponse = await cmsSharedService.GetCMSTransferHistory(FileId);
            if (historyResponse.IsSuccessStatusCode)
            {
                cmsCaseRequestTransferHistory = (List<CmsTransferHistoryVM>)historyResponse.ResultData;
                if (cmsCaseRequestTransferHistory.Count() > 0)
                    caseRequestTransferHistoryobj = cmsCaseRequestTransferHistory.FirstOrDefault();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(historyResponse);
            }

        }
        protected async Task PopulateConsultationRequestGrid()
        {
            if (consultationFile != null)
            {
                var consultationRequestResponse = await consultationRequestService.GetConsultationDetailById(consultationFile.ConsultationRequestId);
                if (consultationRequestResponse.IsSuccessStatusCode)
                {
                    consultationRequestVM = (ViewConsultationVM)consultationRequestResponse.ResultData;
                    if (consultationRequestVM != null)
                    {
                        IsReqeustLoad = true;
                    }
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(consultationRequestResponse);
                }
            }
        }
        protected async Task PopulateConslutationFileStatusHistory()
        {
            var response = await consultationFileService.GetConslutationFileStatusHistory(Guid.Parse(FileId));
            if (response.IsSuccessStatusCode)
            {
                consultationFileHistory = (List<ConsultationFileHistoryVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        protected async Task PopulateConslutationFileCommunicationGrid()
        {
            var response = await communicationService.GetConslutationFileCommunication(Guid.Parse(FileId));
            if (response.IsSuccessStatusCode)
            {
                communicationListVm = (List<CommunicationListVM>)response.ResultData;
                if (communicationListVm.Any())
                {
                    ActivityEn = communicationListVm.FirstOrDefault().Activity_En;
                    ActivityAr = communicationListVm.FirstOrDefault().Activity_Ar;
                    InboxNumber = communicationListVm.FirstOrDefault().InboxNumber;
                    OutboxNumber = communicationListVm.FirstOrDefault().OutboxNumber;
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        protected async Task PopulateFileAssignees()
        {
            var response = await consultationFileService.GetConsultationAssigneeList(Guid.Parse(FileId));
            if (response.IsSuccessStatusCode)
            {
                consultationFileAssignee = (List<ConsultationFileAssignmentVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        protected async Task PopulateConsultationAssigmentHistory()
        {
            var response = await consultationFileService.GetConsultationAssigmentHistory(Guid.Parse(FileId));
            if (response.IsSuccessStatusCode)
            {
                consultationFileAssignmentHistoryVM = (List<ConsultationFileAssignmentHistoryVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        public async Task PopulateDraftGrid()
        {
            var response = await cmsCaseTemplateService.GetCaseDraftListByReferenceId(Guid.Parse(FileId));
            if (response.IsSuccessStatusCode)
            {
                DraftTemplate = (List<CmsDraftedDocumentVM>)response.ResultData;
                DraftDocuments = DraftTemplate.Select(c => new CmsDraftedTemplateVM
                {
                    Id = c.Id,
                    TypeEn = c.TypeEn,
                    TypeAr = c.TypeAr,
                    Name = c.Name,
                    DraftNumber = c.DraftNumber,
                    CreatedDate = c.TempCreatedDate,
                    AttachmentTypeId = c.AttachmentTypeId,
                    StatusId = c.StatusId
                }).ToList();
                DraftDocuments = DraftDocuments.DistinctBy(x => x.Id).ToList();
                var docResponse = await fileUploadService.GetAllAttachmentTypes();
                if (docResponse.IsSuccessStatusCode)
                {
                    var attachmentTypes = (List<AttachmentType>)docResponse.ResultData;
                    foreach (var draftedDocument in DraftDocuments)
                    {
                        var attachmentType = attachmentTypes.FirstOrDefault(x => x.AttachmentTypeId == draftedDocument.AttachmentTypeId);
                        if (attachmentType is not null)
                        {
                            draftedDocument.TypeAr = attachmentType.Type_Ar;
                            draftedDocument.TypeEn = attachmentType.Type_En;
                        }
                    }
                }

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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

        #region Load Versions for a Draft
        protected async Task ExpandDraftVersions(CmsDraftedTemplateVM draftTemplate)
        {
            DraftedTemplateVersions = DraftTemplate.Where(x => x.Id == draftTemplate.Id).ToList();
        }

        //<History Author = 'Hassan Abbas' Date='2022-01-06' Version="1.0" Branch="master"> Redirect to View Detail page</History>
        protected async Task DetailDraft(CmsDraftedDocumentVM args)
        {
            navigationManager.NavigateTo("draftdocument-detail/" + args.Id + "/" + args.VersionId);
        }

        protected async Task CompareVersion(MouseEventArgs args)
        {
            try
            {
                if (selectedDraftsVersion != null && selectedDraftsVersion.Any())
                {
                    navigationManager.NavigateTo("/fileDraft-comparison/" + selectedDraftsVersion[0].Id + "/" + selectedDraftsVersion[0].VersionId + "/" + selectedDraftsVersion[1].VersionId);
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

        #region Redirect
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
        //<History Author = 'Muhammad Zaeem' Date='2022-03-02' Version="1.0" Branch="master"> Redirect to View Detail page</History>
        protected async Task DetailDraft(ComsDraftedDocumentVM args)
        {
            navigationManager.NavigateTo("draftdocument-detail/" + args.Id + "/" + Guid.Empty);
        }
        #endregion

        #region Buttons
        protected async Task Transfer(MouseEventArgs args)
        {
            var dialogResult = await dialogService.OpenAsync<TransferConsultationSector>(
                translationState.Translate("Transfer_Consultation"),
                new Dictionary<string, object>() {
                    { "ReferenceId", Guid.Parse(FileId) },
                    { "TransferConsultationType", (int)AssignCaseToLawyerTypeEnum.ConsultationFile },
                    { "IsAssignment", false },
                    { "IsConfidential", consultationFile.IsConfidential },
                    { "SectorTypeId",consultationFile.SectorTypeId },
                    { "RequestTypeId",consultationFile.RequestTypeId}
                },
                new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
            if (dialogResult != null)
            {
                if (TaskId != null && taskDetailVM.TaskStatusId != (int)TaskStatusEnum.Done)
                    await UpdateTaskDetail();
                navigationManager.NavigateTo("/consultationfile-list/");
            }
        }
        protected async Task MeetingRequest(MouseEventArgs args)
        {
            try
            {
                string ReferenceId = FileId;
                int SubModuleId = (int)SubModuleEnum.ConsultationFile;
                //int SectorTypeId = (int)consultationFile.SectorTypeId;
                string ReceivedBy = consultationRequestVM?.CreatedBy;
                ReceivedBy = System.Net.WebUtility.UrlEncode(ReceivedBy).Replace(".", "%999");
                if (TaskId == null)
                {
                    navigationManager.NavigateTo("/meeting-add/" + ReferenceId + "/" + SubModuleId + "/" + ReceivedBy);
                }
                else
                {
                    navigationManager.NavigateTo("/meeting-add/" + ReferenceId + "/" + SubModuleId + "/" + ReceivedBy + "/" + TaskId + "/" + true);

                }
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
        protected async Task AssignToLawyer()
        {
            var result = await dialogService.OpenAsync<AssignConsultationToLawyer>(translationState.Translate("Assign_To_Lawyer"),
               new Dictionary<string, object>()
               {
                        { "ReferenceId", Guid.Parse(FileId) },
                        { "AssignConsultationLawyerType", (int)AssignCaseToLawyerTypeEnum.ConsultationFile },

               }
               ,
                new DialogOptions() { Width = "45% !important", CloseDialogOnOverlayClick = true });
            if (result != null)
            {
                if (TaskId != null && taskDetailVM.TaskStatusId != (int)TaskStatusEnum.Done)
                    await UpdateTaskDetail();
            }

        }
        //protected async Task ViewHistoryDetail(ConsultationFileHistoryVM args)
        //{
        //    navigationManager.NavigateTo("/consultationfile-historydetail/" + args.HistoryId);
        //}
        protected async Task SendACopy(MouseEventArgs args)
        {
            var dialogResult = await dialogService.OpenAsync<SendACopyConsultationRequest>(
            translationState.Translate("SendAcopy"),
            new Dictionary<string, object>() {
                { "ReferenceId", Guid.Parse(FileId) } ,
                { "SendACopyType", (int)AssignCaseToLawyerTypeEnum.ConsultationFile } ,
                { "SectorTypeId", consultationFile.SectorTypeId } ,
            },
            new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
            if (dialogResult != null)
            {
                if (TaskId != null && taskDetailVM.TaskStatusId != (int)TaskStatusEnum.Done)
                    await UpdateTaskDetail();
            }
        }
        protected async Task DraftAFile(MouseEventArgs args)
        {
            int attachmentType = 0;
            if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.LegalAdvice)
            {
                attachmentType = (int)AttachmentTypeEnum.ComsLegalAdvice;
            }
            if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.Legislations)
            {
                attachmentType = (int)AttachmentTypeEnum.ComsLegisltation;
            }
            if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.InternationalArbitration)
            {
                attachmentType = (int)AttachmentTypeEnum.comsInternationArbitration;
            }
            if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.Contracts)
            {
                attachmentType = (int)AttachmentTypeEnum.ContractReview;
            }
            if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeComplaints)
            {
                attachmentType = (int)AttachmentTypeEnum.ComsAdministrativeComplaints;
            }


            var result = await dialogService.OpenAsync<SelectDraftTemplatePopup>(translationState.Translate("Add_Draft"),
            new Dictionary<string, object>()
            {
                    {"ReferenceId", FileId },
                    {"ModuleId", (int)WorkflowModuleEnum.COMSConsultationManagement },
                    {"DraftEntityType",  (int)DraftEntityTypeEnum.ConsultationFile},
                    {"Document_Type",  attachmentType},
            },
            new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
            if (result != null)
            {
                if (TaskId != null && taskDetailVM.TaskStatusId != (int)TaskStatusEnum.Done)
                    await UpdateTaskDetail();
            }
        }
        protected void RequestMoreInfo(MouseEventArgs args)
        {
            navigationManager.NavigateTo("/Coms-Request-For-More-Information/" + FileId + "/" + false);
        }


        #endregion

        #region Document
        protected async Task AddDocument()
        {
            try
            {
                var result = await dialogService.OpenAsync<FileUpload>(translationState.Translate("Upload_Documents"),
                new Dictionary<string, object>()
                    {
                        { "ReferenceGuid", Guid.Parse(FileId) },
                        { "IsViewOnly", false },
                        { "IsUploadPopup", true },
                        { "FileTypes", systemSettingState.FileTypes },
                        { "MaxFileSize", systemSettingState.File_Maximum_Size },
                        { "Multiple", false },
                        { "UploadFrom", "ConsultationManagement" },
                        { "ModuleId", (int)WorkflowModuleEnum.COMSConsultationManagement },
                        { "AutoSave", true },
                    }
                    ,
                    new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
                var uploadedAttachment = (TempAttachementVM)result;
                if (uploadedAttachment != null)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Document_Added_Successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    RefreshFileUploadGrid = false;
                    StateHasChanged();
                    RefreshFileUploadGrid = true;
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region view response
        protected async Task ViewResponse(CommunicationListVM item)
        {
            if (item.CommunicationTypeId == (int)CommunicationTypeEnum.RequestForMeeting)
            {
                navigationManager.NavigateTo("/meeting-view/" + item.CommunicationId + "/" + item.ReferenceId + "/" + item.CommunicationTypeId + "/" + true + "/" + item.SubModuleId);
            }
            else if (item.CommunicationTypeId == (int)CommunicationTypeEnum.MeetingScheduled)
            {
                navigationManager.NavigateTo("/meeting-view/" + item.CommunicationId + "/" + item.CommunicationTypeId + "/" + true);
            }
            else if (item.CommunicationTypeId == (int)CommunicationTypeEnum.WithdrawRequested)
            {
                RedirectURL = "/detail-withdraw-request/" + item.ReferenceId + "/" + (int)CommunicationTypeEnum.WithdrawRequested;
                navigationManager.NavigateTo(RedirectURL);
            }
            else
            {
                RedirectURL = "/request-need-more-detail/" + item.ReferenceId + "/" + item.CommunicationId + "/" + item.SubModuleId + "/" + item.CommunicationTypeId;
                navigationManager.NavigateTo(RedirectURL);
            }
        }
        #endregion

        #region View Request More Info
        protected void ViewRequestMoreInfo(MouseEventArgs args)
        {
            RedirectURL = "/request-need-information-detail/" + FileId + "/" + false + "/" + (int)CommunicationTypeEnum.RequestMoreInfo;
            navigationManager.NavigateTo(RedirectURL);
        }

        #endregion

        #region RowCellRender
        protected void RowCellRender(RowRenderEventArgs<CommunicationListVM> commuication)
        {
            if (commuication.Data.IsRead == true && commuication.Data.CommunicationTypeId != (int)CommunicationTypeEnum.CaseRequest)
            {
                commuication.Attributes.Add("style", $"background-color: #e3d4c9;");
            }
        }

        #endregion

        #region Add Meeting
        protected async Task AddMeeting(CommunicationListVM item)
        {
            try
            {
                Guid CommunicationId = (Guid)item.CommunicationId;
                string ReferenceId = FileId;
                int SubModuleId = (int)SubModuleEnum.ConsultationFile;
                string ReceivedBy = consultationRequestVM.CreatedBy;
                if (TaskId == null)
                {
                    navigationManager.NavigateTo("/meeting-add/" + CommunicationId + "/" + ReferenceId + "/" + SubModuleId + "/" + ReceivedBy);

                }
                else
                {
                    navigationManager.NavigateTo("/meeting-add/" + CommunicationId + "/" + ReferenceId + "/" + SubModuleId + "/" + ReceivedBy + "/" + TaskId + "/" + true);

                }
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
        #endregion

        #region Update Task Detail
        protected async Task UpdateTaskDetail()
        {
            taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
            if (loginState.UserDetail.RoleId == SystemRoles.ViceHOS || loginState.UserDetail.RoleId == SystemRoles.HOS || loginState.UserDetail.RoleId == SystemRoles.ComsHOS)
            {
                taskDetailVM.IsMultipleTaskUpdateForSameEntity = true;
                taskDetailVM.SectorId = loginState.UserDetail.SectorTypeId;
            }
            var taskResponse = await taskService.DecisionTask(taskDetailVM);
            if (!taskResponse.IsSuccessStatusCode)
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(taskResponse);
            }
        }
        #endregion

        protected void PopulateTranslationKey()
        {
            if (SectorTypeIdCheck == (int)OperatingSectorTypeEnum.Contracts)
            {
                TransKeyHeader = "Contracts_File";


            }
            else if (SectorTypeIdCheck == (int)OperatingSectorTypeEnum.LegalAdvice)
            {
                TransKeyHeader = "Legal_Advice_File";


            }
            else if (SectorTypeIdCheck == (int)OperatingSectorTypeEnum.InternationalArbitration)
            {
                TransKeyHeader = "International_Arbitration_File";

            }
            else if (SectorTypeIdCheck == (int)OperatingSectorTypeEnum.AdministrativeComplaints)
            {
                TransKeyHeader = "Administrative_Complaints_File";

            }
            else if (SectorTypeIdCheck == (int)OperatingSectorTypeEnum.Legislations)
            {
                TransKeyHeader = "List_Legislations_File";


            }
            else
            {
                if (consultationFile.RequestTypeId == (int)RequestTypeEnum.Legislations)
                {
                    TransKeyHeader = "List_Legislations_File";

                }
                if (consultationFile.RequestTypeId == (int)RequestTypeEnum.Contracts)
                {
                    TransKeyHeader = "Contracts_File";

                }
                if (consultationFile.RequestTypeId == (int)RequestTypeEnum.InternationalArbitration)
                {
                    TransKeyHeader = "International_Arbitration_File";

                }
                if (consultationFile.RequestTypeId == (int)RequestTypeEnum.AdministrativeComplaints)
                {
                    TransKeyHeader = "Administrative_Complaints_File";

                }
                if (consultationFile.RequestTypeId == (int)RequestTypeEnum.LegalAdvice)
                {
                    TransKeyHeader = "Legal_Advice_File";

                }
            }
        }
        protected void ViewRequestMoreInfoList(MouseEventArgs args)
        {
            navigationManager.NavigateTo("/list-needformoreinfo/" + FileId + "/" + (int)SubModuleEnum.ConsultationFile + "/" + consultationFile.SectorTypeId);
        }

        #region Get Contact Details For File
        private async Task GetContactDetailsForFile(Guid fileId)
        {
            try
            {
                var response = await lookupService.GetContactDetailsForFile(fileId);
                if (response.IsSuccessStatusCode)
                {
                    contactListForFileVm = (List<ContactFileLinkVM>)response.ResultData;
                }
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }

        }
        #endregion

        #region Contact File Grid Action Buttons
        protected async Task DetailContact(ContactFileLinkVM args)
        {
            int Module = (int)WorkflowModuleEnum.COMSConsultationManagement;
            Guid File = Guid.Parse(FileId);
            int SectorId = Convert.ToInt32(SectorTypeId);
            navigationManager.NavigateTo("contact-view/" + args.ContactId + "/" + Module + "/" + File + "/" + SectorId);
        }

        protected async Task RemoveContact(ContactFileLinkVM args)
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Delete_The_Record"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                var response = await lookupService.RemoveContact(args);
                if (response.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Contact_Delete_Success"),
                        Style = "position: fixed !important; left: 0; margin: auto;"
                    });
                    await GetContactDetailsForFile(Guid.Parse(FileId));
                    await ContactGrid.Reload();
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
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
        #endregion

        #region Add Contact With File
        public async Task AddContactWithFile()
        {
            var dialogResult = await dialogService.OpenAsync<ListContact>(
                translationState.Translate("Contact_List"),
                new Dictionary<string, object>()
                {
                    {"ContactListForFileLink", true },
                    {"FileId", Guid.Parse(FileId) },
                    {"FileModule", (int)WorkflowModuleEnum.COMSConsultationManagement }
                },
                new DialogOptions() { Width = "80% !important", CloseDialogOnOverlayClick = true });
            await Task.Delay(100);
            var resultContact = (string)dialogResult;
            if (resultContact != null)
            {
                await GetContactDetailsForFile(Guid.Parse(FileId));
                await ContactGrid.Reload();
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
