using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_WEB.Pages.CaseManagment.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;

namespace FATWA_WEB.Pages.CaseManagment.Files
{
    //< History Author = 'Hassan Abbas' Date = '2022-12-29' Version = "1.0" Branch = "master" >Review Case File Assignment Task</History>
    public partial class ReviewAssignmentCaseFile : ComponentBase
    {

        #region Paramter
        [Parameter]
        public string FileId { get; set; }
        [Parameter]
        public string TaskId { get; set; }
        #endregion

        #region Variables

        protected RadzenDataGrid<CmsDraftedTemplateVM> gridTemplate;
        protected RadzenDataGrid<CmsDraftedDocumentVM> gridVersion;

        public IList<CmsDraftedDocumentVM> selectedDraftsVersion;
        protected RadzenDataGrid<CmsRegisteredCaseFileDetailVM> gridRegionalCases;
        protected RadzenDataGrid<CmsRegisteredCaseFileDetailVM> gridAppealCases;
        protected RadzenDataGrid<CmsRegisteredCaseFileDetailVM> gridSupremeCases;
        public IList<CmsRegisteredCaseFileDetailVM> RegisteredCases { get; set; } = new List<CmsRegisteredCaseFileDetailVM>();
        public bool allowRowSelectOnRowClick = true;
        protected string RedirectURL { get; set; }
        protected CaseRequestDetailVM caseRequest { get; set; }
        protected RadzenDataGrid<CmsCaseFileStatusHistoryVM> HistoryGrid;
        protected RadzenDataGrid<CmsCaseAssigneesHistoryVM> LawyersGrid;
        protected RadzenDataGrid<CmsTransferHistoryVM> transferHistoryGrid;
        protected RadzenDataGrid<CasePartyLinkVM> PartiesGrid;
        public IEnumerable<CmsCaseFileStatusHistoryVM> caseStatusHistory { get; set; } = new List<CmsCaseFileStatusHistoryVM>();
        protected IEnumerable<CmsTransferHistoryVM> cmsCaseFileTransferHistory = new List<CmsTransferHistoryVM>();
        public List<CmsCaseAssigneeVM> caseFileAssignees { get; set; } = new List<CmsCaseAssigneeVM>();
        public List<CmsDraftedTemplateVM> DraftDocuments { get; set; } = new List<CmsDraftedTemplateVM>();
        public List<CmsDraftedDocumentVM> DraftTemplate { get; set; } = new List<CmsDraftedDocumentVM>();
        public List<CmsDraftedDocumentVM> DraftedTemplateVersions { get; set; } = new List<CmsDraftedDocumentVM>();
        public IEnumerable<CmsCaseAssigneesHistoryVM> caseFileAssigneesHistory { get; set; } = new List<CmsCaseAssigneesHistoryVM>();
        protected RadzenDataGrid<CommunicationListVM> CommunicationGrid;
        public IEnumerable<CommunicationListVM> communicationListVm = new List<CommunicationListVM>();
        protected IEnumerable<CmsCaseFileVM> linkedFiles;
        protected bool RefreshFileUploadGrid { get; set; } = true;
        protected bool IsUnderfilingButtonsEnabled { get; set; } = true;
        protected CaseAssignment getcaseAssignment { get; set; } = new CaseAssignment();
        protected List<ManagerTaskReminderVM> managerTaskReminderData { get; set; } = new List<ManagerTaskReminderVM>();

        #endregion

        #region Component Load

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Component Load</History>
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();

            var result = await cmsCaseFileService.GetCaseFileDetailByIdVM(Guid.Parse(FileId));

            if (result.IsSuccessStatusCode)
            {
                dataCommunicationService.caseFile = (CmsCaseFileDetailVM)result.ResultData;
                await PopulateCaseRequestGrid();
                await PopulateCasePartyGrid();
                await PopulateCaseFileStatusHistory();
                await PopulateTransferHistoryGrid();
                await PopulateLawyerAssignmentHistory();
                await PopulateFileAssignees();
                await PopulateCommunicationList(FileId);
                await PopulateLinkedFiles();
                await PopulateDraftGrid();
                await PopulateCaseFileAssigmentbyFileIdAndLawyerId();
                await PopulateRegisteredCasesByFileId();
                if (TaskId != null)
                {
                    await GetManagerTaskReminderData();
                }

                if ((dataCommunicationService.caseFile != null && (int)dataCommunicationService.caseFile.StatusId == (int)CaseFileStatusEnum.RegisteredInMoj) && (int)loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeUnderFilingCases && (int)loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialUnderFilingCases && loginState.UserRoles.Any(u => u.RoleId == SystemRoles.HOS))
                {
                    IsUnderfilingButtonsEnabled = false;
                }
                else if (dataCommunicationService.caseFile.IsAssignedBack != null && dataCommunicationService.caseFile.IsAssignedBack == true && loginState.UserRoles.Any(u => u.RoleId == SystemRoles.HOS))
                {
                    await cmsCaseFileService.UpdateCaseFileIsAssignedBackStatus(Guid.Parse(FileId));
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }

            spinnerService.Hide();
        }

        #endregion

        #region Populate Grids

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
        public async Task PopulateCommunicationList(string FileId)
        {
            var CommunicationResponse = await communicationService.GetCommunicationListByCaseFileId(Guid.Parse(FileId));
            if (CommunicationResponse.IsSuccessStatusCode)
            {
                communicationListVm = (List<CommunicationListVM>)CommunicationResponse.ResultData;
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
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(caseRequestResponse);
            }
        }
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
        protected async Task PopulateTransferHistoryGrid()
        {
            var historyResponse = await cmsSharedService.GetCMSTransferHistory(FileId);
            if (historyResponse.IsSuccessStatusCode)
            {
                cmsCaseFileTransferHistory = (List<CmsTransferHistoryVM>)historyResponse.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(historyResponse);
            }

        }
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

        protected async Task PopulateRegisteredCasesByFileId()
        {
            var response = await cmsCaseFileService.GetAllRegisteredCasesByFileId(Guid.Parse(FileId));
            if (response.IsSuccessStatusCode)
            {
                RegisteredCases = (List<CmsRegisteredCaseFileDetailVM>)response.ResultData;
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
        #endregion

        #region Load Versions for a Draft
        protected async Task ExpandDraftVersions(CmsDraftedTemplateVM draftTemplate)
        {
            DraftedTemplateVersions = DraftTemplate.Where(x => x.Id == draftTemplate.Id).ToList();
        }
        protected async Task DetailDraft(CmsDraftedDocumentVM args)
        {
            navigationManager.NavigateTo("draftdocument-detail/" + args.Id + "/" + args.VersionId);
        }
        #endregion

        #region Redirect Function
        private void GoBack()
        {
            navigationManager.NavigateTo("/case-files");
        }
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
            else if (item.CommunicationTypeId == (int)CommunicationTypeEnum.CaseRequest)
            {
                //IsStatusCaseRequest = false;
            }
            else
            {
                RedirectURL = "/request-need-more-detail/" + item.ReferenceId + "/" + item.CommunicationId + "/" + item.SubModuleId + "/" + item.CommunicationTypeId;
                navigationManager.NavigateTo(RedirectURL);
            }
        }
        #endregion

        #region Grid Buttons
        protected async Task AssignBackToHos(MouseEventArgs args)
        {
            var dialogResult = await dialogService.OpenAsync<ApproveRejectCaseFileByLawyer>
                (
                translationState.Translate("Approve_Reject_CaseFile"),
                new Dictionary<string, object>() {
                    { "ReferenceId", Guid.Parse(FileId) },
                    { "TaskId", TaskId }
                },
                new DialogOptions() { Width = "25% !important", CloseDialogOnOverlayClick = true ,});
            if (dialogResult != null)
            {
                await Task.Delay(300);
                await RedirectBack();
            }
        }
        //<History Author = 'Ijaz Ahmad' Date='2024-02-07' Version="1.0" Branch="master"> Detail  Case Party Info</History>
        protected void DetailCaseParty(CasePartyLinkVM args)
        {
            navigationManager.NavigateTo("/caseparty-view/" + args.Id);
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
