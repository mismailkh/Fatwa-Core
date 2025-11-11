using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_DOMAIN.Models.WorkflowModels;
using FATWA_WEB.Pages.CaseManagment.Shared;
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

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Detail of Registered Case</History>
    public partial class DetailRegisteredCase : ComponentBase
    {
        #region Parameters
        [Parameter]
        public string CaseId { get; set; }
        [Parameter]
        public string? TaskId { get; set; }
        #endregion

        #region Variables
        public Guid? FileId { get; set; }
        public Guid FileIdForViewDetail { get; set; }

        public Guid referenceId { get; set; }
        protected CmsRegisteredCaseDetailVM registeredCase { get; set; } = new CmsRegisteredCaseDetailVM() { IsAssigned = false };
        protected CmsCaseFileDetailVM caseFile { get; set; }
        protected RadzenDataGrid<CmsRegisteredCaseStatusHistoryVM> HistoryGrid;
        protected RadzenDataGrid<CmsCaseAssigneesHistoryVM> LawyersGrid;
        protected RadzenDataGrid<CasePartyLinkVM> PartiesGrid;
        protected RadzenDataGrid<HearingVM> HearingGrid;
        protected List<CasePartyLinkVM> CasePartyLinks;
        protected List<CmsRegisteredCaseVM> Subcases;
        protected List<CmsRegisteredCaseVM> MergedCases;
        protected List<HearingVM> Hearings;
        protected List<OutcomeHearingVM> OutcomeHearings;
        protected List<OutcomeAndHearingVM> OutcomeAndHearings;
        protected List<JudgementVM> Judgements = new List<JudgementVM>();
        protected List<CmsJudgmentExecutionVM> CmsJudgmentExecutions = new List<CmsJudgmentExecutionVM>();
        public IEnumerable<CmsRegisteredCaseStatusHistoryVM> caseStatusHistory { get; set; } = new List<CmsRegisteredCaseStatusHistoryVM>();
        protected List<TransferHistoryVM> TransferHistoryVMs = new List<TransferHistoryVM>();
        public List<CmsCaseAssigneeVM> caseFileAssignees { get; set; } = new List<CmsCaseAssigneeVM>();
        protected IEnumerable<CmsCaseFileSectorAssignment> caseFileSectorAssignment = new List<CmsCaseFileSectorAssignment>();
        public IEnumerable<CmsCaseAssigneesHistoryVM> caseFileAssigneesHistory { get; set; } = new List<CmsCaseAssigneesHistoryVM>();
        protected RadzenDataGrid<CommunicationListVM>? communicationListGrid;
        protected RadzenDataGrid<CmsAnnouncementVM>? announcementsListGrid;
        public IEnumerable<CommunicationListVM> communicationListVm = new List<CommunicationListVM>();
        public IEnumerable<CmsAnnouncementVM> announcementsListVm = new List<CmsAnnouncementVM>();
        public string ActivityEn;
        public string ActivityAr;
        protected string InboxNumber;
        protected string OutboxNumber;
        protected bool RefreshFileUploadGrid { get; set; } = true;
        protected bool IsRespectiveSector { get; set; }

        protected RadzenDataGrid<CmsDraftedDocumentVM> DraftCaseRequestGrid;
        protected CaseRequestDetailVM caseRequestdetailVm = new CaseRequestDetailVM();
        protected string RedirectURL { get; set; }

        protected RadzenDataGrid<CmsDraftedTemplateVM> gridTemplate;
        protected RadzenDataGrid<CmsDraftedDocumentVM> gridVersion;
        public IList<CmsDraftedDocumentVM> selectedDraftsVersion;

        public bool allowRowSelectOnRowClick = true;
        public List<CmsDraftedDocumentVM> DraftTemplate { get; set; } = new List<CmsDraftedDocumentVM>();
        public List<CmsDraftedTemplateVM> DraftDocuments { get; set; } = new List<CmsDraftedTemplateVM>();
        public List<CmsDraftedDocumentVM> DraftedTemplateVersions { get; set; } = new List<CmsDraftedDocumentVM>();
        protected TaskDetailVM taskDetailVM = new TaskDetailVM();

        public bool IsSentToMOJ = false;
        protected ObservableCollection<TempAttachementVM> documents = new ObservableCollection<TempAttachementVM>();
        public List<MojRegistrationRequest> MojRegistrationRequestList { get; set; } = new List<MojRegistrationRequest>();
        protected List<ManagerTaskReminderVM> managerTaskReminderData { get; set; } = new List<ManagerTaskReminderVM>();
        int caseDetailsTabsIndex;
        #endregion

        #region Component Load

        //<History Author = 'Hassan Abbas' Date='2025-07-01' Version="1.0" Branch="master"> Component Parameters Set</History>
        protected override async Task OnParametersSetAsync()
        {
            if (caseDetailsTabsIndex > 0 || registeredCase != null)
            {
                caseDetailsTabsIndex = 0;
                await OnInitializedAsync();
            }
        }

        #endregion

        #region Component Load

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Component Load</History>
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();

            var result = await cmsRegisteredCaseService.GetRegisteredCaseDetailByIdVM(Guid.Parse(CaseId));

            if (result.IsSuccessStatusCode)
            {
                documents = await fileUploadService.GetUploadedAttachements(false, 0, Guid.Parse(CaseId));
                registeredCase = (CmsRegisteredCaseDetailVM)result.ResultData;

                if (registeredCase.IsDissolved == null)
                    registeredCase.IsDissolved = false;

                FileIdForViewDetail = (Guid)registeredCase.FileId;
                await PopulateCaseFileGrid();
                await GetCaseFileSectorAssigmentByFileId();
                await PopulateRegisteredCasePartyGrid();
                await PopulateRegisteredCaseStatusHistory();
                await PopulateTransferHistoryDetail();
                await PopulateLawyerAssignmentHistory();
                await PopulateCaseAssignees();
                await PopulateCommunicationList(CaseId);
                await PopulateOutcomesGrid();
                await PopulateJudgementsGrid();
                await PopulateSubcasesGrid();
                await PopulateMergedCasesGrid();
                await PopulateDraftGrid();
                await ValidateSector();
                await PopulateHearingsGrid();
                await PopulateExecutionsGrid();
                await PopulateCaseRequestGrid();
                await PopulateOutcomesAndHearingGrid();
                await PopulateAnnouncementsList(CaseId);
                await IsSenttoMOj();
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

            spinnerService.Hide();
        }

        #endregion

        #region Populate Grids

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Populate Case Parties</History>
        protected async Task PopulateRegisteredCaseDetail()
        {
            var result = await cmsRegisteredCaseService.GetRegisteredCaseDetailByIdVM(Guid.Parse(CaseId));
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

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Populate Case Parties</History>
        protected async Task PopulateRegisteredCasePartyGrid()
        {
            var partyResponse = await caseRequestService.GetCMSCasePartyDetailById(Guid.Parse(CaseId));
            if (partyResponse.IsSuccessStatusCode)
            {
                CasePartyLinks = (List<CasePartyLinkVM>)partyResponse.ResultData;
                CasePartyLinks = CasePartyLinks?.Select(c => { c.CasePartyCategory = (CasePartyCategoryEnum)c.CategoryId; c.CasePartyType = (CasePartyTypeEnum)c.TypeId; return c; }).ToList();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(partyResponse);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Populate Hearings of a Case</History>
        protected async Task PopulateHearingsGrid()
        {
            var response = await cmsRegisteredCaseService.GetHearingsByCase(Guid.Parse(CaseId));
            if (response.IsSuccessStatusCode)
            {
                Hearings = (List<HearingVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Populate Outcoems of Hearing Hearing</History>
        protected async Task PopulateOutcomesAndHearingGrid()
        {
            var response = await cmsRegisteredCaseService.GetOutcomesAndHearingByCase(Guid.Parse(CaseId));
            if (response.IsSuccessStatusCode)
            {
                OutcomeAndHearings = (List<OutcomeAndHearingVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        protected async Task PopulateOutcomesGrid()
        {
            var response = await cmsRegisteredCaseService.GetOutcomesHearingByCase(Guid.Parse(CaseId));
            if (response.IsSuccessStatusCode)
            {
                OutcomeHearings = (List<OutcomeHearingVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        //<History Author = 'Danish' Date='2022-01-03' Version="1.0" Branch="master"> Populate Execution Grid</History>
        protected async Task PopulateExecutionsGrid()
        {
            var response = await cmsRegisteredCaseService.GetJudgmentExecutions(Guid.Parse(CaseId));
            if (response.IsSuccessStatusCode)
            {
                CmsJudgmentExecutions = (List<CmsJudgmentExecutionVM>)response.ResultData;

                foreach (var execution in CmsJudgmentExecutions)
                {
                    if (string.IsNullOrEmpty(execution.PayerName))
                    {
                        if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
                            execution.PayerName = execution.GovtEntityPayer_En;
                        else
                            execution.PayerName = execution.GovtEntityPayer_Ar;
                    }
                    if (string.IsNullOrEmpty(execution.ReceiverName))
                    {
                        if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
                            execution.ReceiverName = execution.GovtEntityReceiver_En;
                        else
                            execution.ReceiverName = execution.GovtEntityReceiver_Ar;
                    }
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Populate Judgements Hearing</History>
        protected async Task PopulateJudgementsGrid()
        {
            var response = await cmsRegisteredCaseService.GetJudgementsByCase(Guid.Parse(CaseId));
            if (response.IsSuccessStatusCode)
            {
                Judgements = (List<JudgementVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Populate Subcases Grid</History>
        protected async Task PopulateSubcasesGrid()
        {
            var response = await cmsRegisteredCaseService.GetSubcasesByCase(Guid.Parse(CaseId));
            if (response.IsSuccessStatusCode)
            {
                Subcases = (List<CmsRegisteredCaseVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Populate Merged Cases Grid</History>
        protected async Task PopulateMergedCasesGrid()
        {
            var response = await cmsRegisteredCaseService.GetMergedCasesByCase(Guid.Parse(CaseId));
            if (response.IsSuccessStatusCode)
            {
                MergedCases = (List<CmsRegisteredCaseVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Populate Merged Cases Grid</History>
        protected async Task ValidateSector()
        {
            if ((loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeRegionalCases || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialRegionalCases) && registeredCase.CourtTypeId == (int)CourtTypeEnum.Regional)
            {
                IsRespectiveSector = true;
            }
            else if ((loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeAppealCases || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialAppealCases) && registeredCase.CourtTypeId == (int)CourtTypeEnum.Appeal)
            {
                IsRespectiveSector = true;
            }
            else if ((loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeSupremeCases || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialSupremeCases) && registeredCase.CourtTypeId == (int)CourtTypeEnum.Supreme)
            {
                IsRespectiveSector = true;
            }
            else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases) //&& registeredCase.CourtTypeId == (int)CourtTypeEnum.PartialUrgent)
            {
                IsRespectiveSector = true;
            }
            else
            {
                IsRespectiveSector = false;
            }
        }

        public async Task PopulateCommunicationList(string caseId)
        {
            var CommunicationResponse = await communicationService.GetCommunicationListByCaseId(Guid.Parse(caseId));
            if (CommunicationResponse.IsSuccessStatusCode)
            {
                communicationListVm = (List<CommunicationListVM>)CommunicationResponse.ResultData;

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(CommunicationResponse);
            }
        }
        public async Task PopulateAnnouncementsList(string caseId)
        {
            var AnnouncementResponse = await communicationService.GetAnnouncementsListByCaseId(Guid.Parse(caseId));
            if (AnnouncementResponse.IsSuccessStatusCode)
            {
                announcementsListVm = (List<CmsAnnouncementVM>)AnnouncementResponse.ResultData;

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(AnnouncementResponse);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Populate Request Grid</History>
        protected async Task PopulateRegisteredCaseStatusHistory()
        {
            var response = await cmsRegisteredCaseService.GetRegisteredCaseStatusHistory(Guid.Parse(CaseId));
            if (response.IsSuccessStatusCode)
            {
                caseStatusHistory = (List<CmsRegisteredCaseStatusHistoryVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        protected async Task PopulateTransferHistoryDetail()
        {
            try
            {
                var response = await cmsRegisteredCaseService.GetTransferHistoryByOutcome(Guid.Empty, Guid.Parse(CaseId));
                if (response.IsSuccessStatusCode)
                {
                    TransferHistoryVMs = (List<TransferHistoryVM>)response.ResultData;
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
        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Populate Request Grid</History>
        protected async Task PopulateLawyerAssignmentHistory()
        {
            var response = await cmsCaseFileService.GetCaseAssigmentHistory(Guid.Parse(CaseId));
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
        public async Task GetCaseFileSectorAssigmentByFileId()
        {
            var response = await cmsCaseFileService.GetCaseFileSectorAssigmentByFileId((Guid)registeredCase.FileId, (int)loginState.UserDetail.SectorTypeId);
            if (response.IsSuccessStatusCode)
            {
                caseFileSectorAssignment = (List<CmsCaseFileSectorAssignment>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Populate Request Grid</History>
        protected async Task PopulateCaseFileGrid()
        {
            var caseRequestResponse = await cmsCaseFileService.GetCaseFileDetailByIdVM((Guid)registeredCase.FileId);
            if (caseRequestResponse.IsSuccessStatusCode)
            {
                caseFile = (CmsCaseFileDetailVM)caseRequestResponse.ResultData;

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(caseRequestResponse);
            }
        }

        protected async Task PopulateCaseRequestGrid()
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

        //<History Author = 'Hassan Abbas' Date='2023-03-11' Version="1.0" Branch="master">Check if Attachments</History>
        public void ExecutionRowRender(RowRenderEventArgs<CmsJudgmentExecutionVM> args)
        {
            try
            {
                if (args.Data.AttachmentCount <= 0)
                {
                    args.Attributes.Add("class", "no-execution-attachment");
                }
            }
            catch (Exception ex)
            {
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
                        { "ReferenceId", CaseId },
                    },
                    new DialogOptions() { Width = "40% !important", CloseDialogOnOverlayClick = true }
                );
                var party = (CasePartyLinkVM)result;
                if (party != null)
                {
                    await PopulateRegisteredCasePartyGrid();
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
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Party_Deleted_Successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    await PopulateRegisteredCasePartyGrid();
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

        //<History Author = 'Danish' Date='2022-09-30' Version="1.0" Branch="master"> Open Add Party dialog</History>
        protected async Task RequestForDocument()
        {
            try
            {
                var result = await dialogService.OpenAsync<CreateRequestForDocument>(translationState.Translate("Request_For_Portfolio"),
                new Dictionary<string, object>()
               {
                        { "CaseId", Guid.Parse(CaseId) },
               }
               ,
               new DialogOptions() { Width = "50% !important", CloseDialogOnOverlayClick = true });
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Add Hearing

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Add Hearing</History>
        protected async Task AddHearing()
        {
            try
            {
                navigationManager.NavigateTo("/add-hearing/" + Guid.Parse(CaseId));
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

        #region Add Prepare Execution
        //<History Author = 'Danish' Date='2023-01-03' Version="1.0" Branch="master">Add Prepare Execution</History>
        protected async Task AddPrepareExecution()
        {
            try
            {
                var result = await dialogService.OpenAsync<AddExecution>(translationState.Translate("Prepare_Execution"),
                    new Dictionary<string, object>()
                     {
                        { "CaseId", Guid.Parse(CaseId) },
                     }
                    , new DialogOptions()
                    {
                        CloseDialogOnOverlayClick = true,
                        Width = "50% !important"


                    });
                await Task.Delay(300);
                await PopulateExecutionsGrid();


            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Edit Prepare Execution
        //<History Author = 'Danish' Date='2023-01-03' Version="1.0" Branch="master">Edit Prepare Execution</History>
        //protected async Task EditPrepareExecution(CmsJudgmentExecutionVM cmsJudgmentExecution)
        //{
        //    try
        //    {
        //        await dialogService.OpenAsync<AddExecution>(
        //            translationState.Translate("Prepare_Execution"),
        //            new Dictionary<string, object>()
        //            {
        //                { "ExecutionId",  cmsJudgmentExecution.Id },
        //            },
        //            new DialogOptions() { CloseDialogOnOverlayClick = true, Width = "50% !important" });
        //        await Task.Delay(300);
        //        await PopulateExecutionsGrid();

        //    }
        //    catch (Exception ex)
        //    {
        //        notificationService.Notify(new NotificationMessage()
        //        {
        //            Severity = NotificationSeverity.Error,
        //            Detail = ex.Message,
        //            Style = "position: fixed !important; left: 0; margin: auto; "
        //        });
        //    }
        //}
        #endregion

        #region  Buttons

        //<History Author = 'Hassan Abbas' Date='2022-10-30' Version="1.0" Branch="master"> Open Add Draft Popup</History>
        protected async Task DraftAFile(MouseEventArgs args)
        {

            var result = await dialogService.OpenAsync<SelectDraftTemplatePopup>(translationState.Translate("Add_Draft"),
            new Dictionary<string, object>()
                {
                        { "ReferenceId", CaseId },
                        { "ModuleId", (int)WorkflowModuleEnum.CaseManagement },
                        { "DraftEntityType", (int)DraftEntityTypeEnum.Case},
                        { "DocumentTypes", await identifyDocumentTypes.GetDocumentTypes(registeredCase,(int)DraftEntityTypeEnum.Case, loginState.UserDetail.SectorTypeId != null ? (int)loginState.UserDetail.SectorTypeId : 0)},
                }
                ,
                new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
        }

        //<History Author = 'Hassan Abbas' Date='2023-04-05' Version="1.0" Branch="master"> Create Execution Request</History>
        protected async Task OpenNewWindow(MouseEventArgs args)
        {
            JsInterop.InvokeVoidAsync("openNewWindow", "https://localhost:7214/case-view/f5884642-5cb3-4096-86bf-735ec7806180");
        }
        protected async Task CreateExecutionRequest(MouseEventArgs args)
        {
            navigationManager.NavigateTo("/add-execution-request/" + CaseId);
        }
        //<History Author = 'Hassan Abbas' Date='2022-10-30' Version="1.0" Branch="master"> Request More Info</History>
        protected async Task MeetingRequest(MouseEventArgs args)
        {
            try
            {
                Guid ReferenceId = registeredCase.CaseId;
                int SubModuleId = (int)SubModuleEnum.RegisteredCase;
                string ReceivedBy = caseRequestdetailVm.CreatedBy;
                navigationManager.NavigateTo("/meeting-add/" + ReferenceId + "/" + SubModuleId + "/" + ReceivedBy);
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
        //<History Author = 'Hassan Abbas' Date='2022-10-30' Version="1.0" Branch="master"> Request More Info</History>
        //<History Author = 'ijaz Ahmad' Date='2022-11-30' Version="1.0" Branch="master"> Assign Registered Case To Lawyer</History>
        protected async Task AssignToLawyer()
        {
            var result = await dialogService.OpenAsync<AssignToLawyer>(translationState.Translate("Assign_To_Lawyer"),
                new Dictionary<string, object>()
                {
                        { "ReferenceId", Guid.Parse(CaseId) },
                        { "AssignCaseLawyerType", (int)AssignCaseToLawyerTypeEnum.RegisteredCase },

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

        protected async Task SendToMojTeam(MouseEventArgs args)
        {

            if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                if (documents != null && documents.Where(x => (x.AttachmentTypeId == (int)AttachmentTypeEnum.StopExecutionOfJudgment)
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
                            draftedTemplateVersion.FileId = FileId;
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
            StateHasChanged();
        }
        protected async Task ObjectPreparationOfMojRegistrationRequest(ObservableCollection<TempAttachementVM> documents)
        {

            var documentToSendd = documents.Where(x => (x.AttachmentTypeId == (int)AttachmentTypeEnum.StopExecutionOfJudgment)
                                                        && x.IsMOJRegistered == false).FirstOrDefault();
            if (documentToSendd != null)
            {
                var mojRegistrationRequest = new MojRegistrationRequest
                {
                    FileId = FileIdForViewDetail,
                    SectorTypeId = (int)loginState.UserDetail.SectorTypeId,
                    DocumentId = (int)documentToSendd.UploadedDocumentId,
                    CreatedBy = loginState.Username,
                    CreatedDate = DateTime.Now,
                };
                MojRegistrationRequestList.Add(mojRegistrationRequest);
            }
        }
        protected async Task IsSenttoMOj()
        {
            if (documents != null && documents.Where(x => (x.AttachmentTypeId == (int)AttachmentTypeEnum.StopExecutionOfJudgment)
                                                        && x.IsMOJRegistered == false).Any())
                IsSentToMOJ = true;
            else
                IsSentToMOJ = false;
        }
        //<History Author = 'Hassan Abbas' Date='2022-10-30' Version="1.0" Branch="master"> Request More Info</History>
        protected void NeedMoreInfo(MouseEventArgs args)
        {
            navigationManager.NavigateTo("/Case-Request-For-More-Information/" + CaseId);
        }

        //<History Author = 'Hassan Abbas' Date='2022-10-30' Version="1.0" Branch="master"> Request More Info</History>
        protected void CreateSubCase(MouseEventArgs args)
        {
            if (registeredCase.FileId != null)
            {
                FileId = registeredCase.FileId;
                navigationManager.NavigateTo("/Add-Sub-Case/" + CaseId + "/" + FileId);
            }
            else
            {

            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-10-30' Version="1.0" Branch="master"> Request More Info</History>
        protected void MergeCases(MouseEventArgs args)
        {
            //navigationManager.NavigateTo("/Request-For-More-Information/" + CaseId + "/" + false);
        }

        //<History Author = 'Hassan Abbas' Date='2022-10-30' Version="1.0" Branch="master"> Request More Info</History>
        protected void NotifyGE(MouseEventArgs args)
        {
            //navigationManager.NavigateTo("/Request-For-More-Information/" + CaseId + "/" + false);
        }

        //<History Author = 'Hassan Abbas' Date='2022-10-30' Version="1.0" Branch="master"> Request More Info</History>
        protected void SaveCloseSase(MouseEventArgs args)
        {

            navigationManager.NavigateTo("/save-closecase-file/" + CaseId);
            navigationState.ReturnUrl = "/case-view/" + CaseId;
        }

        //<History Author = 'Hassan Abbas' Date='2022-10-29' Version="1.0" Branch="master"> Redirect to Case Detail page</History>
        protected async Task DetailCase(CmsRegisteredCaseVM args)
        {
            navigationManager.NavigateTo("/case-view/" + args.CaseId);
        }
        //<History Author = 'Hassan Abbas' Date='2022-01-06' Version="1.0" Branch="master"> Redirect to View Detail page</History>
        protected async Task DetailDraft(CmsDraftedDocumentVM args)
        {
            navigationState.ReturnUrl = "/case-view/" + CaseId;
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
        protected async Task AddMeeting(CommunicationListVM item)
        {
            try
            {
                Guid CommunicationId = item.CommunicationId;
                string ReferenceId = CaseId;
                int SubModuleId = (int)SubModuleEnum.RegisteredCase;
                string ReceivedBy = caseRequestdetailVm.CreatedBy;
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
        protected void ViewRequestMoreInfo(MouseEventArgs args)
        {
            navigationManager.NavigateTo("/list-needformoreinfo/" + CaseId + "/" + (int)SubModuleEnum.RegisteredCase + "/" + 0);
        }
        #endregion

        #region Hearing Grid Actions

        //<History Author = 'Hassan Abbas' Date='2022-10-30' Version="1.0" Branch="master"> Add Outcome</History>
        protected async Task AddOutcome(OutcomeAndHearingVM args)
        {
            dataCommunicationService.outcomeHearing = null;
            navigationManager.NavigateTo("add-outcome/" + args.HearingId + "/" + Guid.Parse(CaseId));
        }
        //<History Author = 'Muhammad Zaeem' Date='2024-10-03' Version="1.0" Branch="master"> Edit Outcome</History>
        protected async Task EditOutcome(OutcomeAndHearingVM args)
        {
            dataCommunicationService.outcomeHearing = null;
            navigationManager.NavigateTo("add-outcome/" + args.HearingId + "/" + Guid.Parse(CaseId) + "/" + args.OutcomeId);
        }
        //<History Author = 'Hassan Abbas' Date='2022-10-30' Version="1.0" Branch="master"> Postpone Hearing</History>
        protected async Task PostponeHearing(HearingVM args)
        {
            var result = await dialogService.OpenAsync<AddPostponeHearingRequest>(translationState.Translate("Postpone_Hearing"),
                new Dictionary<string, object>()
               {
                    { "HearingId", args.Id },
                    { "CaseId", Guid.Parse(CaseId) },
               },
                new DialogOptions() { Width = "75% !important", CloseDialogOnOverlayClick = true });
            if (result != null)
            {
                await PopulateHearingsGrid();
            }
        }

        //<History Author = 'Hassan Abbas' Date='2023-03-19' Version="1.0" Branch="master"> Document Portfolio</History>
        protected void CreateDocumentPortfolio(HearingVM args)
        {
            navigationManager.NavigateTo("/document-portofolio/" + args.Id + "/" + CaseId + "/" + args.CreatedDate.ToString("dd-MM-yyyy"));
        }

        //<History Author = 'Hassan Abbas' Date='2023-11-19' Version="1.0" Branch="master"> Hearing Detail</History>
        protected void DetailHearing(OutcomeAndHearingVM args)
        {
            navigationManager.NavigateTo("/hearing-view/" + args.HearingId + "/" + args.OutcomeId);
        }
        protected void DetailExecution(CmsJudgmentExecutionVM args)
        {
            navigationManager.NavigateTo("/execution-view/" + args.Id);
        }
        protected void DetailCaseParty(CasePartyLinkVM args)
        {
            navigationManager.NavigateTo("/caseparty-view/" + args.Id);
        }
        #endregion

        #region Outcome Grid Actions

        //<History Author = 'Hassan Abbas' Date='2023-11-25' Version="1.0" Branch="master"> Detail Outcome</History>
        protected async Task DetailJudgement(JudgementVM args)
        {
            navigationManager.NavigateTo("judgement-view/" + args.Id);
        }
        protected async Task EditJudgement(JudgementVM args)
        {
            navigationManager.NavigateTo("add-judgement/" + args.Id + "/" + args.OutcomeId + "/" + args.CaseId);
        }
        protected async Task EditHearing(OutcomeAndHearingVM args)
        {
            navigationManager.NavigateTo("add-hearing/" + args.HearingId + "/" + true);
        }
        protected async Task EditExecution(CmsJudgmentExecutionVM args)
        {
            try
            {
                var result = await dialogService.OpenAsync<AddExecution>(translationState.Translate("Prepare_Execution"),
                    new Dictionary<string, object>()
                    {
                        { "ExecutionId", args.Id }
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
        //<History Author = 'Hassan Abbas' Date='2023-11-25' Version="1.0" Branch="master"> Detail Outcome</History>
        protected async Task DetailOutcome(OutcomeAndHearingVM args)
        {
            navigationManager.NavigateTo("outcome-view/" + args.OutcomeId);
        }

        //<History Author = 'Hassan Abbas' Date='2022-10-30' Version="1.0" Branch="master"> Add Judgement</History>
        protected async Task AddJudgement(OutcomeAndHearingVM args)
        {
            navigationManager.NavigateTo("add-judgement/" + args.OutcomeId + "/" + Guid.Parse(CaseId));
        }
        //<History Author = 'Hassan Abbas' Date='2022-10-30' Version="1.0" Branch="master"> Add Outcome Document</History>
        protected async Task AddOutcomeDocument()
        {
            var result = await dialogService.OpenAsync<FileUpload>(translationState.Translate("Upload_Documents"),
            new Dictionary<string, object>()
                {
                        { "ReferenceGuid", Guid.Parse(CaseId) },
                        { "IsViewOnly", false },
                        { "IsUploadPopup", true },
                        { "FileTypes", systemSettingState.FileTypes },
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

        //<History Author = 'Hassan Abbas' Date='2022-12-10' Version="1.0" Branch="master"> Update Parties</History>
        protected async Task UpdateParties(OutcomeHearingVM args)
        {
            var result = await dialogService.OpenAsync<UpdateCaseParties>(translationState.Translate("Update_Parties"),
            new Dictionary<string, object>()
                {
                        { "ReferenceId", Guid.Parse(CaseId) },
                },
                new DialogOptions() { Width = "80% !important", CloseDialogOnOverlayClick = true });
            await PopulateRegisteredCasePartyGrid();
        }
        #endregion

        #region SelectChamberNumberDialog
        protected async Task Transfer(OutcomeAndHearingVM args)
        {
            var result = await dialogService.OpenAsync<TransferCaseToChamber>(translationState.Translate("Transfer_Case"),
                new Dictionary<string, object> {
                    { "ChamberId", registeredCase.ChamberId },
                    {"CaseId", registeredCase.CaseId },
                    {"ChamberNumberId", registeredCase.ChamberNumberId },
                    {"CourtId",registeredCase.CourtId },
                    {"OutComeId", args.OutcomeId}
                },
                new DialogOptions() { Width = "30%", Resizable = true, CloseDialogOnOverlayClick = true }
                );

            await PopulateRegisteredCaseDetail();
        }

        #endregion

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
        protected async Task ViewCaseFileDetails(Guid FileId)
        {
            navigationManager.NavigateTo("/casefile-view/" + FileId);
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
            else
            {
                RedirectURL = "/request-need-more-detail/" + item.ReferenceId + "/" + item.CommunicationId + "/" + item.SubModuleId + "/" + item.CommunicationTypeId;
                navigationManager.NavigateTo(RedirectURL);
            }
        }
        #endregion

        #region Get Case Draft List By Reference Id
        public async Task PopulateDraftGrid()
        {
            var response = await cmsCaseTemplateService.GetCaseDraftListByReferenceId(Guid.Parse(CaseId));
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
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }


        }
        #endregion

        #region Load Versions for a Draft
        protected async Task ExpandDraftVersions(CmsDraftedTemplateVM draftTemplate)
        {
            DraftedTemplateVersions = DraftTemplate.Where(x => x.Id == draftTemplate.Id).ToList();
        }
        #endregion

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

        #region Expand list Scheduliing Court Visit        
        //<History Author = 'Danish' Date='2022-11-29' Version="1.0" Branch="master">Expand list Scheduliing Court Visit</History>        
        protected async Task ExpandCourtVisit(HearingVM hearing)
        {
            var response = await cmsRegisteredCaseService.GetSchedulCourtVisitByHearingId(hearing.Id);
            if (response.IsSuccessStatusCode)
            {
                hearing.ScheduleCourtVisit = (List<SchedulingCourtVisitVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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
                    importantCase.ReferenceId = Guid.Parse(CaseId);
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
                        registeredCase.IsImportant = true;
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
                    importantCase.ReferenceId = Guid.Parse(CaseId);
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
                        registeredCase.IsImportant = false;

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
