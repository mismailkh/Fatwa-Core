using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Extensions;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.DigitalSignature;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.ContactManagmentVMs;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_WEB.Pages.CaseManagment.MOJ;
using FATWA_WEB.Pages.CaseManagment.Shared;
using FATWA_WEB.Pages.ContactManagment;
using FATWA_WEB.Pages.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Radzen;
using Radzen.Blazor;
using System.Collections.ObjectModel;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.CaseManagment.Files
{
    //< History Author = 'Hassan Abbas' Date = '2022-10-29' Version = "1.0" Branch = "master" >Detail Case File</History>
    public partial class DetailCaseFile : ComponentBase
    {
        [Parameter]
        public string FileId { get; set; }
        [Parameter]
        public string? TaskId { get; set; }

        #region Variables

        protected RadzenDataGrid<CmsDraftedTemplateVM> gridTemplate;
        protected RadzenDataGrid<CmsDraftedDocumentVM> gridVersion;
        protected RadzenDataGrid<CmsRegisteredCaseFileDetailVM> gridRegionalCases;
        protected RadzenDataGrid<CmsRegisteredCaseFileDetailVM> gridAppealCases;
        protected RadzenDataGrid<CmsRegisteredCaseFileDetailVM> gridSupremeCases;
        //public IList<CmsRegisteredCaseVM> RegisteredCases { get; set; } = new List<CmsRegisteredCaseVM>();
        public IList<CmsRegisteredCaseFileDetailVM> RegisteredCases { get; set; } = new List<CmsRegisteredCaseFileDetailVM>();
        public IList<CmsDraftedDocumentVM> selectedDraftsVersion;

        public bool allowRowSelectOnRowClick = true;
        public bool IsFinalJudgementIssued = true;
        protected string RedirectURL { get; set; }
        protected CaseRequestDetailVM caseRequest { get; set; } = new CaseRequestDetailVM();
        protected RadzenDataGrid<CmsCaseFileStatusHistoryVM> HistoryGrid;
        protected RadzenDataGrid<CmsCaseAssigneesHistoryVM> LawyersGrid;
        protected RadzenDataGrid<CmsTransferHistoryVM> transferHistoryGrid;
        protected RadzenDataGrid<CasePartyLinkVM> PartiesGrid;
        public IEnumerable<CmsCaseFileStatusHistoryVM> caseStatusHistory { get; set; } = new List<CmsCaseFileStatusHistoryVM>();
        protected IEnumerable<CmsTransferHistoryVM> cmsCaseFileTransferHistory = new List<CmsTransferHistoryVM>();
        protected CmsTransferHistoryVM caseFileTransferHistoryobj = new CmsTransferHistoryVM();
        public List<CmsCaseAssigneeVM> caseFileAssignees { get; set; } = new List<CmsCaseAssigneeVM>();
        public List<CmsDraftedTemplateVM> DraftDocuments { get; set; } = new List<CmsDraftedTemplateVM>();
        public List<CmsDraftedDocumentVM> DraftTemplate { get; set; } = new List<CmsDraftedDocumentVM>();
        public List<CmsDraftedDocumentVM> DraftedTemplateVersions { get; set; } = new List<CmsDraftedDocumentVM>();
        public List<MojRegistrationRequest> MojRegistrationRequestList { get; set; } = new List<MojRegistrationRequest>();
        protected TaskDetailVM taskDetailVM { get; set; } = new TaskDetailVM();
        public IEnumerable<CmsCaseAssigneesHistoryVM> caseFileAssigneesHistory { get; set; } = new List<CmsCaseAssigneesHistoryVM>();
        protected RadzenDataGrid<CommunicationListVM> CommunicationGrid;
        protected IEnumerable<CmsCaseFileVM> linkedFiles;
        protected IEnumerable<CmsCaseFileSectorAssignment> caseFileSectorAssignment = new List<CmsCaseFileSectorAssignment>();
        public IEnumerable<CommunicationListVM> communicationListVm = new List<CommunicationListVM>();
        public string ActivityEn;
        public string ActivityAr;
        protected string InboxNumber;
        protected string OutboxNumber;
        public bool IsSentToMOJ = false;
        protected bool RefreshFileUploadGrid { get; set; } = true;
        protected bool IsUnderfilingButtonsEnabled { get; set; } = true;
        protected Guid CommunicationId { get; set; } = Guid.Empty;
        public string TransKeyHeader = string.Empty;
        public string TaskCreatedBy { get; set; }
        public string LoginUserEmail { get; set; }
        protected CaseAssignment getcaseAssignment { get; set; } = new CaseAssignment();
        protected RadzenDataGrid<ContactFileLinkVM> ContactGrid = new();
        protected IList<ContactFileLinkVM> contactListForFileVm { get; set; } = new List<ContactFileLinkVM>();
        protected ObservableCollection<TempAttachementVM> documents = new ObservableCollection<TempAttachementVM>();
        ExternalSigningRequest externalSigningRequest = new ExternalSigningRequest();
        protected List<ManagerTaskReminderVM> managerTaskReminderData { get; set; } = new List<ManagerTaskReminderVM>();


        #endregion

        #region Component Load

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Component Load</History>
        protected override async Task OnInitializedAsync()
        {
            LoginUserEmail = loginState.UserDetail.Email;
            spinnerService.Show();

            var result = await cmsCaseFileService.GetCaseFileDetailByIdVM(Guid.Parse(FileId));
            if (result.IsSuccessStatusCode)
            {
                dataCommunicationService.caseFile = (CmsCaseFileDetailVM)result.ResultData;
                documents = await fileUploadService.GetUploadedAttachements(false, 0, Guid.Parse(FileId));
                await PopulateCaseRequestGrid();
                await PopulateCasePartyGrid();
                await PopulateCaseFileStatusHistory();
                await PopulateLawyerAssignmentHistory();
                await PopulateTransferHistoryGrid();
                await PopulateFileAssignees();
                await PopulateCommunicationList();
                await PopulateDraftGrid();
                await PopulateLinkedFiles();
                await IsSenttoMOj();
                await PopulateCaseFileAssigmentbyFileIdAndLawyerId();
                await PopulateRegisteredCasesByFileId();
                if ((dataCommunicationService.caseFile != null && (int)dataCommunicationService.caseFile.StatusId == (int)CaseFileStatusEnum.RegisteredInMoj) && (int)loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeUnderFilingCases && (int)loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialUnderFilingCases && loginState.UserRoles.Any(u => u.RoleId == SystemRoles.HOS))
                {
                    IsUnderfilingButtonsEnabled = false;
                }
                else if (dataCommunicationService.caseFile.IsAssignedBack != null && dataCommunicationService.caseFile.IsAssignedBack == true && loginState.UserRoles.Any(u => u.RoleId == SystemRoles.HOS))
                {
                    await cmsCaseFileService.UpdateCaseFileIsAssignedBackStatus(Guid.Parse(FileId));
                }
                await GetContactDetailsForFile(Guid.Parse(FileId));
                await GetCaseFileSectorAssigmentByFileId();

                if (TaskId != null)
                {
                    await PopulateTaskDetails();
                    await GetManagerTaskReminderData();
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
            if ((bool)dataCommunicationService?.caseFile?.IsCaseRegistered)
            {
                RedirectURL = "/case-files";
                TransKeyHeader = "Case_Files";
            }
            else
            {
                RedirectURL = "/registerd-requests";
                TransKeyHeader = "Under_Filing";
            }
            spinnerService.Hide();
        }
        #endregion

        #region Populate Grids

        protected async Task PopulateCaseFileAssigmentbyFileIdAndLawyerId()
        {
            var userresponse = await cmsSharedService.GetCaseAssigmentByLawyerIdAndFileId(Guid.Parse(FileId), loginState.UserDetail.UserId);
            if (userresponse.IsSuccessStatusCode)
            {
                getcaseAssignment = (CaseAssignment)userresponse.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(userresponse);
            }
        }
        protected async Task PopulateRegisteredCasesByFileId()
        {
            var response = await cmsCaseFileService.GetAllRegisteredCasesByFileId(Guid.Parse(FileId));
            if (response.IsSuccessStatusCode)
            {
                RegisteredCases = (List<CmsRegisteredCaseFileDetailVM>)response.ResultData;
                int courtTypeId = CaseConsultationExtension.GetCourtTypeIdBasedOnSectorId((int)loginState.UserDetail.SectorTypeId);
                if (RegisteredCases.Any(c => c.CourtTypeId == courtTypeId))
                {
                    IsFinalJudgementIssued = !RegisteredCases.Any(c => c.CourtTypeId == courtTypeId && !c.IsFinalJudgement);
                }
                else
                {
                    IsFinalJudgementIssued = false;
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

            if (gridRegionalCases != null)
                translationState.TranslateGridFilterLabels(gridRegionalCases);
            if (gridAppealCases != null)
                translationState.TranslateGridFilterLabels(gridAppealCases);
            if (gridSupremeCases != null)
                translationState.TranslateGridFilterLabels(gridSupremeCases);
        }

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Populate Case Parties</History>
        protected async Task PopulateCasePartyGrid()
        {
            var partyResponse = await caseRequestService.GetCMSCasePartyDetailById(Guid.Parse(FileId));
            if (partyResponse.IsSuccessStatusCode)
            {
                dataCommunicationService.caseFile.CasePartyLinks = (List<CasePartyLinkVM>)partyResponse.ResultData;
                dataCommunicationService.caseFile.CasePartyLinks = dataCommunicationService.caseFile?.CasePartyLinks?.Select(c => { c.CasePartyCategory = (CasePartyCategoryEnum)c.CategoryId; c.CasePartyType = (CasePartyTypeEnum)c.TypeId; return c; }).ToList();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(partyResponse);
            }
        }
        public async Task PopulateCommunicationList()
        {
            var CommunicationResponse = await communicationService.GetCommunicationListByCaseFileId(Guid.Parse(FileId));
            if (CommunicationResponse.IsSuccessStatusCode)
            {
                communicationListVm = (List<CommunicationListVM>)CommunicationResponse.ResultData;
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
                await invalidRequestHandlerService.ReturnBadRequestNotification(CommunicationResponse);
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Populate Request Grid</History>
        protected async Task PopulateCaseFileStatusHistory()
        {
            var response = await cmsCaseFileService.GetCaseFileStatusHistory(Guid.Parse(FileId));
            if (response.IsSuccessStatusCode)
            {
                caseStatusHistory = (List<CmsCaseFileStatusHistoryVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Populate Request Grid</History>
        protected async Task PopulateLawyerAssignmentHistory()
        {
            var response = await cmsCaseFileService.GetCaseAssigmentHistory(Guid.Parse(FileId));
            if (response.IsSuccessStatusCode)
            {
                caseFileAssigneesHistory = (List<CmsCaseAssigneesHistoryVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Populate Request Grid</History>
        protected async Task PopulateFileAssignees()
        {
            var response = await cmsCaseFileService.GetCaseAssigeeList(Guid.Parse(FileId));
            if (response.IsSuccessStatusCode)
            {
                caseFileAssignees = (List<CmsCaseAssigneeVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Populate Request Grid</History>
        protected async Task PopulateCaseRequestGrid()
        {
            var caseRequestResponse = await caseRequestService.GetCaseRequestDetailById(dataCommunicationService.caseFile.RequestId);
            if (caseRequestResponse.IsSuccessStatusCode)
            {
                caseRequest = JsonConvert.DeserializeObject<CaseRequestDetailVM>(caseRequestResponse.ResultData.ToString());
                //caseRequest = (CaseRequestDetailVM)caseRequestResponse.ResultData;
                dataCommunicationService.caseFile.CaseRequest.Add(caseRequest);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(caseRequestResponse);
            }
        }
        protected async Task PopulateTransferHistoryGrid()
        {
            var historyResponse = await cmsSharedService.GetCMSTransferHistory(FileId);
            if (historyResponse.IsSuccessStatusCode)
            {
                cmsCaseFileTransferHistory = (List<CmsTransferHistoryVM>)historyResponse.ResultData;
                if (cmsCaseFileTransferHistory.Count() > 0)
                    caseFileTransferHistoryobj = cmsCaseFileTransferHistory.FirstOrDefault();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(historyResponse);
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
        //<History Author = 'Hassan Abbas' Date='2022-12-30' Version="1.0" Branch="master"> Populate Linked Files Grid</History>
        public async Task PopulateLinkedFiles()
        {
            var response = await cmsCaseFileService.GetLinkedFilesByPrimaryFileId(Guid.Parse(FileId));
            if (response.IsSuccessStatusCode)
            {
                linkedFiles = (List<CmsCaseFileVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        public async Task GetCaseFileSectorAssigmentByFileId()
        {
            var response = await cmsCaseFileService.GetCaseFileSectorAssigmentByFileId(Guid.Parse(FileId), (int)loginState.UserDetail.SectorTypeId);
            if (response.IsSuccessStatusCode)
            {
                caseFileSectorAssignment = (List<CmsCaseFileSectorAssignment>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        //<History Author = 'Hassan Abbas' Date='2023-03-11' Version="1.0" Branch="master">Check if Attachments</History>
        public void PartyRowRender(RowRenderEventArgs<CasePartyLinkVM> args)
        {
            try
            {
                if (args.Data.AttachmentCount <= 0)
                {
                    args.Attributes.Add("class", "no-party-attachment");
                }
            }
            catch (Exception ex)
            {
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-12-30' Version="1.0" Branch="master"> Populate Draft Grid</History>
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
                    StatusId = c.StatusId,
                    GovtEntityNamesEn = c.GovtEntityNamesEn,
                    GovtEntityNamesAr = c.GovtEntityNamesAr,
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
        protected async Task IsSenttoMOj()
        {
            if (documents != null && documents.Where(x => (x.AttachmentTypeId == (int)AttachmentTypeEnum.ClaimStatement
                                                        || x.AttachmentTypeId == (int)AttachmentTypeEnum.PerformOrderNotes
                                                        || x.AttachmentTypeId == (int)AttachmentTypeEnum.OrderOnPetitionNotes
                                                        || x.AttachmentTypeId == (int)AttachmentTypeEnum.StopExecutionOfJudgment)
                                                        && x.IsMOJRegistered == false).Any())
                IsSentToMOJ = true;
            else
                IsSentToMOJ = false;
        }
        #endregion

        #region Load Versions for a Draft
        protected async Task ExpandDraftVersions(CmsDraftedTemplateVM draftTemplate)
        {
            DraftedTemplateVersions = DraftTemplate.Where(x => x.Id == draftTemplate.Id).ToList();

            List<CmsDraftedDocumentVM> excludedDraftVersions = new List<CmsDraftedDocumentVM>();
            if (loginState.UserRoles.Any(u => u.RoleId != SystemRoles.Lawyer) && loginState.UserRoles.Any(u => u.RoleId != SystemRoles.ComsLawyer))
            {
                excludedDraftVersions = DraftedTemplateVersions.Where(x => x.StatusId == (int)DraftVersionStatusEnum.Draft && x.CreatedBy != loginState.Username).ToList();
            }
            else
            {
                foreach (var version in DraftedTemplateVersions.Where(x => x.StatusId == (int)DraftVersionStatusEnum.Draft).ToList())
                {
                    List<string> roleList = new List<string>();
                    roleList = await GetUserRoleByVersionCreatedBy(version.CreatedBy);
                    if (!roleList.Contains(SystemRoles.Lawyer) && !roleList.Contains(SystemRoles.ComsLawyer))
                    {
                        excludedDraftVersions.Add(version);
                    }
                }
            }
            DraftedTemplateVersions = DraftedTemplateVersions.Except(excludedDraftVersions).ToList();
        }
        protected async Task<List<string>> GetUserRoleByVersionCreatedBy(string Username)
        {
            ApiCallResponse response = await userService.GetUserRoles(Username);
            if (response.IsSuccessStatusCode)
            {
                var roleList = (List<UserRole>)response.ResultData;

                return roleList.Select(x => x.RoleId).ToList();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                return new List<string>();
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-01-06' Version="1.0" Branch="master"> Redirect to View Detail page</History>
        protected async Task DetailDraft(CmsDraftedDocumentVM args)
        {
            navigationManager.NavigateTo("draftdocument-detail/" + args.Id + "/" + args.VersionId);
        }
        protected async Task ViewItemHistory(CmsDraftedDocumentVM args)
        {
            var dialogResult = await dialogService.OpenAsync<ListDraftTemplateVersionLogs>
                (
                translationState.Translate("Draft_Version_History"),
                new Dictionary<string, object>()
                {
                    { "versionId", args.VersionId.ToString() },
                },
                new DialogOptions() { Width = "60% !important", CloseDialogOnOverlayClick = true, ShowClose = true });
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

        #region Add Party, Delete Party

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Open Add Party dialog</History>
        protected async Task AddParty(int categoryId)
        {
            try
            {
                var result = await dialogService.OpenAsync<AddCaseParty>(translationState.Translate("Add_Case_Party"),
                    new Dictionary<string, object>()
                    {
                        { "CategoryId", categoryId },
                        { "ReferenceId", FileId },
                    },
                    new DialogOptions() { Width = "40% !important", CloseDialogOnOverlayClick = true }
                );
                var party = (CasePartyLinkVM)result;
                if (party != null)
                {
                    await PopulateCasePartyGrid();
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {

            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Delete Party</History>
        protected async Task DeleteParty(CasePartyLinkVM party)
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Delete"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                spinnerService.Show();
                var response = await cmsCaseFileService.DeleteCaseParty(party);
                if (response.IsSuccessStatusCode)
                {
                    var docResponse = await fileUploadService.RemoveDocument(party.Id.ToString(), true);
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Party_Deleted_Successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    await PopulateCasePartyGrid();
                    StateHasChanged();
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                spinnerService.Hide();
            }
        }

        #endregion

        #region Add Document

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Open Add Party dialog</History>
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
                        { "FileTypes", systemSettingState.FileTypes},
                        { "MaxFileSize", systemSettingState.File_Maximum_Size },
                        { "Multiple", false },
                        { "UploadFrom", "CaseManagement" },
                        { "ModuleId", (int)WorkflowModuleEnum.CaseManagement },
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

        #region  Buttons

        //<History Author = 'Hassan Abbas' Date='2022-10-30' Version="1.0" Branch="master"> Transfer Case File to Another sector</History>
        protected async Task Transfer(MouseEventArgs args)
        {
            var RejectedTransferIds = cmsCaseFileTransferHistory.Where(x => x.StatusId == (int)ApprovalTrackingStatusEnum.Rejected).Select(x => x.SectorFrom).ToList();
            RejectedTransferIds.AddRange(cmsCaseFileTransferHistory.Where(x => x.StatusId == (int)ApprovalTrackingStatusEnum.RejectedByFatwaPresident).Select(x => x.SectorTo).ToList());
            var dialogResult = await dialogService.OpenAsync<TransferSector>(
                translationState.Translate("Transfer"),
                new Dictionary<string, object>() {
                    { "ReferenceId", Guid.Parse(FileId) },
                    { "TransferCaseType", (int)AssignCaseToLawyerTypeEnum.CaseFile },
                    { "IsAssignment", false },
                    { "IsConfidential", caseRequest.IsConfidential },
                    { "RequestTypeId", caseRequest.RequestTypeId },
                    { "RejectedTransferIds", RejectedTransferIds},
                    { "SenderSector", cmsCaseFileTransferHistory.Select(x => x.SectorFrom).FirstOrDefault() }
                },
                new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
            if (dialogResult != null)
            {
                if (TaskId != null)
                {
                    taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
                    if (loginState.UserDetail.RoleId == SystemRoles.ViceHOS || loginState.UserDetail.RoleId == SystemRoles.HOS)
                    {
                        taskDetailVM.IsMultipleTaskUpdateForSameEntity = true;
                        taskDetailVM.SectorId = loginState.UserDetail.SectorTypeId;
                        taskDetailVM.SystemGenTypeIdsToComplete.Add((int)TaskSystemGenTypeEnum.CaseFileAssignmentApproval);
                    }
                    var taskResponse = await taskService.DecisionTask(taskDetailVM);
                    if (!taskResponse.IsSuccessStatusCode)
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(taskResponse);
                    }
                }
                await Task.Delay(300);
                await RedirectBack();
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-10-30' Version="1.0" Branch="master"> Transfer Case File to Another sector</History>
        protected async Task Assign(MouseEventArgs args)
        {
            var dialogResult = await dialogService.OpenAsync<TransferSector>(
                translationState.Translate("Assign_Case_File"),
                new Dictionary<string, object>() {
                    { "ReferenceId", Guid.Parse(FileId) },
                    { "TransferCaseType", (int)AssignCaseToLawyerTypeEnum.CaseFile },
                    { "IsAssignment", true },
                    { "IsConfidential", caseRequest.IsConfidential  },
                    { "RequestTypeId", caseRequest.RequestTypeId },
                    { "RejectedTransferIds", new List<int>() },
                    { "SenderSector", 0 }
                },
                new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
            if (dialogResult != null)
            {
                await Task.Delay(300);
                await RedirectBack();
            }
        }
        //<History Author = 'Hassan Abbas' Date='2024-02-30' Version="1.0" Branch="master"> Assign Moj Migrated Case File to Selected Sector</History>
        protected async Task AssignMojMigratedFileToSector(MouseEventArgs args)
        {
            var result = await dialogService.OpenAsync<SelectSectorForMojAssignment>(translationState.Translate("Assign_Migrated_CaseFile_To_Sector"),
            new Dictionary<string, object>()
            {
                { "SelectedFileIds", new List<Guid>{ Guid.Parse(FileId)} },
            },
            new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true }
              );
            if (result != null)
            {
                await RedirectBack();
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-10-30' Version="1.0" Branch="master"> Open Add Draft Popup</History>
        protected async Task DraftAFile(MouseEventArgs args)
        {
            if (documents != null && documents.Where(x => (x.AttachmentTypeId == (int)AttachmentTypeEnum.PerformOrderNotes
                                                        || x.AttachmentTypeId == (int)AttachmentTypeEnum.OrderOnPetitionNotes)
                                                        && x.IsMOJRegistered == true).Any())
                dataCommunicationService.caseFile.ShowClaimStatement = true;
            var dialogResult = await dialogService.OpenAsync<SelectDraftTemplatePopup>(translationState.Translate("Add_Draft"),
            new Dictionary<string, object>()
            {
                {"ReferenceId", FileId },
                {"ModuleId", (int)WorkflowModuleEnum.CaseManagement },
                {"DraftEntityType",  (int)DraftEntityTypeEnum.CaseFile},
                {"DocumentTypes", await identifyDocumentTypes.GetDocumentTypes(dataCommunicationService.caseFile,(int)DraftEntityTypeEnum.CaseFile,loginState.UserDetail.SectorTypeId != null ? (int)loginState.UserDetail.SectorTypeId : 0)},
            },
            new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });

        }
        //<History Author = 'Hassan Abbas' Date='2022-10-30' Version="1.0" Branch="master"> Redirect to Response of More Info</History>
        protected void ViewRequestMoreInfo(MouseEventArgs args)
        {
            navigationManager.NavigateTo("/list-needformoreinfo/" + FileId + "/" + (int)SubModuleEnum.CaseFile + "/" + 0);
        }
        //<History Author = 'Hassan Abbas' Date='2022-10-30' Version="1.0" Branch="master"> Send a Copy</History>
        protected async Task SendACopy(MouseEventArgs args)
        {
            var dialogResult = await dialogService.OpenAsync<SendACopyCaseRequest>(
                translationState.Translate("SendAcopy"),
                new Dictionary<string, object>() {
                    { "ReferenceId", Guid.Parse(FileId) } ,
                    { "SendACopyType", (int)AssignCaseToLawyerTypeEnum.CaseFile } ,
                },
                new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
        }
        //<History Author = 'Hassan Abbas' Date='2022-10-30' Version="1.0" Branch="master"> Request More Info</History>
        protected async Task MeetingRequest(MouseEventArgs args)
        {
            try
            {
                string ReferenceId = FileId;
                int SubModuleId = (int)SubModuleEnum.CaseFile;
                string ReceivedBy = System.Net.WebUtility.UrlEncode(caseRequest.CreatedBy).Replace(".", "%999");
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
        //<History Author = 'Ijaz Ahmad' Date='2024-02-07' Version="1.0" Branch="master"> Detail  Case Party Info</History>
        protected void DetailCaseParty(CasePartyLinkVM args)
        {
            navigationManager.NavigateTo("/caseparty-view/" + args.Id);
        }
        //<History Author = 'Hassan Abbas' Date='2022-10-30' Version="1.0" Branch="master"> Send to Moj Team for Registration</History>
        protected async Task SendToMojTeam(MouseEventArgs args)
        {

            if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.Execution)
                {
                    var response = await cmsCaseFileService.GetExecutionCasesByFileId(Guid.Parse(FileId));
                    if (response.IsSuccessStatusCode)
                    {
                        List<CmsRegisteredCaseVM> executionCases = (List<CmsRegisteredCaseVM>)response.ResultData;
                        if (executionCases.Count() > 1)
                        {
                            await dialogService.OpenAsync<SelectCasesForExecution>(translationState.Translate("Select_Cases_Execution"),
                                new Dictionary<string, object>()
                                {
                                    { "FileId", FileId },
                                },
                                new DialogOptions() { Width = "80% !important", CloseDialogOnOverlayClick = true }
                            );
                        }
                        else if (executionCases.Count() > 0)
                        {
                            MojExecutionRequest mojExecutionRequest = new MojExecutionRequest { CaseId = executionCases.FirstOrDefault().CaseId, CreatedBy = loginState.Username, CreatedDate = DateTime.Now };
                            var execResponse = await cmsCaseFileService.CreateMojExecutionRequest(mojExecutionRequest);
                            if (execResponse.IsSuccessStatusCode)
                            {
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Success,
                                    Detail = translationState.Translate("Sent_Execution_To_Moj_Successfully"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                            }
                            else
                            {
                                await invalidRequestHandlerService.ReturnBadRequestNotification(execResponse);
                            }
                        }
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("No_Execution_Case_Found"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                }
                else
                {
                    if (documents != null && documents.Where(x => (x.AttachmentTypeId == (int)AttachmentTypeEnum.ClaimStatement
                                                                || x.AttachmentTypeId == (int)AttachmentTypeEnum.PerformOrderNotes
                                                                || x.AttachmentTypeId == (int)AttachmentTypeEnum.OrderOnPetitionNotes
                                                                || x.AttachmentTypeId == (int)AttachmentTypeEnum.StopExecutionOfJudgment)
                                                                && x.IsMOJRegistered == false).Any())
                    {
                        spinnerService.Show();
                        await ObjectPreparationOfMojRegistrationRequest(documents);
                        var resp = await cmsCaseFileService.CreateMojRegistrationRequest(MojRegistrationRequestList);
                        if (resp.IsSuccessStatusCode)
                        {
                            foreach (var request in MojRegistrationRequestList)
                            {
                                await fileUploadService.UpdateUploadedAttachementMojFlagById(request.DocumentId);
                                var VersionId = documents.Where(x => x.UploadedDocumentId == request.DocumentId).Select(x => x.VersionId).FirstOrDefault();
                                CmsDraftedTemplateVersions draftedTemplateVersion = new CmsDraftedTemplateVersions();
                                draftedTemplateVersion.VersionId = Guid.NewGuid();
                                draftedTemplateVersion.OldVersionId = (Guid)VersionId;
                                draftedTemplateVersion.StatusId = (int)DraftVersionStatusEnum.SendToMOJ;
                                draftedTemplateVersion.CreatedBy = loginState.UserDetail.UserName;
                                draftedTemplateVersion.CreatedDate = DateTime.Now;
                                draftedTemplateVersion.FileId = Guid.Parse(FileId);
                                await cmsCaseTemplateService.UpdateDraftDocumentStatus(draftedTemplateVersion);
                            }
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Sent_To_Moj_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            dataCommunicationService.caseFile.StatusId = (int)CaseFileStatusEnum.PendingForRegistrationAtMoj;
                            IsSentToMOJ = false;
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(resp);
                        }
                        await PopulateDraftGrid();
                        StateHasChanged();
                        spinnerService.Hide();
                    }
                    else
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Must_Atleast_One_ClaimStatement"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                }
            }
            StateHasChanged();
        }
        protected async Task ObjectPreparationOfMojRegistrationRequest(ObservableCollection<TempAttachementVM> documents)
        {
            var documentsToSend = documents.Where(x => x.AttachmentTypeId == (int)AttachmentTypeEnum.ClaimStatement && x.IsMOJRegistered == false).ToList();
            if (documentsToSend.Any())
            {
                foreach (var document in documentsToSend)
                {
                    var mojRegistrationRequest = new MojRegistrationRequest
                    {
                        FileId = Guid.Parse(FileId),
                        SectorTypeId = (int)loginState.UserDetail.SectorTypeId,
                        DocumentId = (int)document.UploadedDocumentId,
                        CreatedBy = loginState.Username,   //await browserStorage.GetItemAsync<string>("User"),
                        CreatedDate = DateTime.Now,
                    };
                    MojRegistrationRequestList.Add(mojRegistrationRequest);
                }
                return;
            }
            var documentToSend = documents.Where(x => (x.AttachmentTypeId == (int)AttachmentTypeEnum.PerformOrderNotes
                                                    || x.AttachmentTypeId == (int)AttachmentTypeEnum.OrderOnPetitionNotes)
                                                    && x.IsMOJRegistered == false).FirstOrDefault();
            if (documentToSend != null)
            {

                var mojRegistrationRequest = new MojRegistrationRequest
                {
                    FileId = Guid.Parse(FileId),
                    SectorTypeId = (int)loginState.UserDetail.SectorTypeId,
                    DocumentId = (int)documentToSend.UploadedDocumentId,
                    CreatedBy = loginState.Username,
                    CreatedDate = DateTime.Now,
                };
                MojRegistrationRequestList.Add(mojRegistrationRequest);
                return;
            }
            var documentToSendd = documents.Where(x => (x.AttachmentTypeId == (int)AttachmentTypeEnum.StopExecutionOfJudgment)
                                                        && x.IsMOJRegistered == false).FirstOrDefault();
            if (documentToSendd != null)
            {
                var mojRegistrationRequest = new MojRegistrationRequest
                {
                    FileId = Guid.Parse(FileId),
                    SectorTypeId = (int)loginState.UserDetail.SectorTypeId,
                    DocumentId = (int)documentToSendd.UploadedDocumentId,
                    CreatedBy = loginState.Username,
                    CreatedDate = DateTime.Now,
                };
                MojRegistrationRequestList.Add(mojRegistrationRequest);
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-10-30' Version="1.0" Branch="master"> Request More Info</History>
        //<History Author = 'ijaz Ahmad' Date='2022-11-30' Version="1.0" Branch="master"> Assign to Lawyer Case File</History>
        protected async Task AssignToLawyer()
        {
            var result = await dialogService.OpenAsync<AssignToLawyer>(translationState.Translate("Assign_To_Lawyer"),
                new Dictionary<string, object>()
               {
                        { "ReferenceId", Guid.Parse(FileId) },
                        { "AssignCaseLawyerType", (int)AssignCaseToLawyerTypeEnum.CaseFile },
               },
                new DialogOptions() { Width = "45% !important", CloseDialogOnOverlayClick = true });
            if (result != null)
            {
                if (TaskId != null)
                {
                    taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
                    if (loginState.UserDetail.RoleId == SystemRoles.ViceHOS || loginState.UserDetail.RoleId == SystemRoles.HOS)
                    {
                        taskDetailVM.IsMultipleTaskUpdateForSameEntity = true;
                        taskDetailVM.SectorId = loginState.UserDetail.SectorTypeId;
                        taskDetailVM.SystemGenTypeIdsToComplete.Add((int)TaskSystemGenTypeEnum.CaseFileAssignmentApproval);
                    }
                    var taskResponse = await taskService.DecisionTask(taskDetailVM);
                    if (!taskResponse.IsSuccessStatusCode)
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(taskResponse);
                    }
                }
                await RedirectBack();
            }
        }

        protected async Task ViewCaseDetail(MouseEventArgs args)
        {
            var response = await cmsCaseFileService.GetAllRegisteredCasesByFileId(Guid.Parse(FileId));
            if (response.IsSuccessStatusCode)
            {
                var RegisteredCases = (List<CmsRegisteredCaseVM>)response.ResultData;
                navigationManager.NavigateTo("/case-view/" + RegisteredCases.FirstOrDefault().CaseId);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        protected async Task DetailRegisteredCase(CmsRegisteredCaseFileDetailVM args)
        {
            navigationManager.NavigateTo("/case-view/" + args.CaseId);
        }
        protected async Task LegalNotificationList(CmsRegisteredCaseFileDetailVM args)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<ListLegalNotification>(

                translationState.Translate("Legal_Notifications"),
                 new Dictionary<string, object>()
                 {
               { "ReferenceId", args.CaseId  },
               { "SubModuleId", (int)SubModuleEnum.RegisteredCase },
                 },
               new DialogOptions() { Width = "40 !important", CloseDialogOnOverlayClick = true, });
                await Task.Delay(200);
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

        #region RowCellRender
        protected void RowCellRender(RowRenderEventArgs<CommunicationListVM> commuication)
        {
            if (commuication.Data.IsRead == true && commuication.Data.CommunicationTypeId != (int)CommunicationTypeEnum.CaseRequest)
            {
                commuication.Attributes.Add("style", $"background-color: #e3d4c9;");
            }
        }

        #endregion

        #region Redirect Function
        //<History Author = 'Hassan Abbas' Date='2022-11-25' Version="1.0" Branch="master">Redirect to respective page</History>
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

        protected async Task AddMeeting(CommunicationListVM item)
        {
            try
            {
                Guid CommunicationId = item.CommunicationId;
                string ReferenceId = FileId;
                int SubModuleId = (int)SubModuleEnum.CaseFile;
                string ReceivedBy = caseRequest.CreatedBy;
                navigationManager.NavigateTo("/meeting-add/" + CommunicationId + "/" + ReferenceId + "/" + SubModuleId + "/" + ReceivedBy);
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

        #region Get Contact Details For File
        private async Task GetContactDetailsForFile(Guid fileId)
        {
            var response = await lookupService.GetContactDetailsForFile(fileId);
            if (response.IsSuccessStatusCode)
            {
                contactListForFileVm = (List<ContactFileLinkVM>)response.ResultData;
            }
            else await invalidRequestHandlerService.ReturnBadRequestNotification(response);
        }
        #endregion

        #region Contact File Grid Action Buttons
        protected async Task DetailContact(ContactFileLinkVM args)
        {
            int Module = (int)WorkflowModuleEnum.CaseManagement;
            Guid File = Guid.Parse(FileId);
            int SectorId = 0;
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
                    {"FileModule", (int)WorkflowModuleEnum.CaseManagement }
                },
                new DialogOptions() { Width = "80% !important", CloseDialogOnOverlayClick = true });
            var resultContact = (string)dialogResult;
            if (resultContact != null)
            {
                await GetContactDetailsForFile(Guid.Parse(FileId));
            }
        }
        #endregion

        #region
        public async Task SaveImportantCase()
        {
            try
            {
                bool? dialogResponse = false;


                dialogResponse = await dialogService.Confirm(
                    translationState.Translate("Sure_Submit_Mark_As_Important"),
                    translationState.Translate("Confirm_Action"),
                    new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("Yes"),
                        CancelButtonText = translationState.Translate("No")
                    });
                if (dialogResponse == true)
                {
                    CaseUserImportant importantCase = new();

                    var user = Guid.Parse(loginState.UserDetail.UserId);
                    importantCase.UserId = user;
                    importantCase.ReferenceId = Guid.Parse(FileId);
                    var response = await cmsRegisteredCaseService.SaveImportantCase(importantCase);
                    if (response.IsSuccessStatusCode)
                    {
                        string successMessage = translationState.Translate("Mark_As_Important_Done");

                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = successMessage,
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        dataCommunicationService.caseFile.IsImportant = true;

                    }
                }
                else
                {
                }
            }
            catch (Exception)
            {

                throw;
            }

        }
        public async Task DeleteImportantCase()
        {
            try
            {
                bool? dialogResponse = false;
                dialogResponse = await dialogService.Confirm(
                    translationState.Translate("Sure_Submit_Mark_As_Un_Important"),
                    translationState.Translate("Confirm"),
                    new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("Yes"),
                        CancelButtonText = translationState.Translate("No")
                    });
                if (dialogResponse == true)
                {
                    CaseUserImportant importantCase = new();

                    var user = Guid.Parse(loginState.UserDetail.UserId);
                    importantCase.UserId = user;
                    importantCase.ReferenceId = Guid.Parse(FileId);
                    var response = await cmsRegisteredCaseService.DeleteImportantCase(importantCase);
                    if (response.IsSuccessStatusCode)
                    {
                        string successMessage = translationState.Translate("Mark_As_Un_Important_Done");

                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = successMessage,
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        dataCommunicationService.caseFile.IsImportant = false;

                    }
                }
                else
                {

                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        #endregion
        #region Request For Transfer
        protected async Task RequestForTransfer(MouseEventArgs args)
        {
            navigationManager.NavigateTo("/transfer-requests");
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
