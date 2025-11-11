using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.CaseManagment;
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
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;

namespace FATWA_WEB.Pages.CaseManagment.Files
{
    //< History Author = 'Hassan Abbas' Date = '2022-10-29' Version = "1.0" Branch = "master" >Review Transfer Case File</History>
    public partial class ReviewTransferCaseFile : ComponentBase
    {
        #region Parameters
        [Parameter]
        public string FileId { get; set; }

        [Parameter]
        public string? TaskId { get; set; }
        #endregion

        #region Variables

        protected RadzenDataGrid<CmsDraftedTemplateVM> gridTemplate;
        protected RadzenDataGrid<CmsDraftedDocumentVM> gridVersion;
        public IList<CmsDraftedDocumentVM> selectedDraftsVersion;
        public bool allowRowSelectOnRowClick = true;
        protected string RedirectURL { get; set; }
        protected TaskDetailVM taskDetailVM = new TaskDetailVM();
        protected CaseRequestDetailVM caseRequest = new CaseRequestDetailVM();
        protected CaseFile caseFile { get; set; }
        protected RadzenDataGrid<CmsRegisteredCaseFileDetailVM> gridRegionalCases;
        protected RadzenDataGrid<CmsRegisteredCaseFileDetailVM> gridAppealCases;
        protected RadzenDataGrid<CmsRegisteredCaseFileDetailVM> gridSupremeCases;
        public IList<CmsRegisteredCaseFileDetailVM> RegisteredCases { get; set; } = new List<CmsRegisteredCaseFileDetailVM>();
        protected RadzenDataGrid<CmsCaseFileStatusHistoryVM> HistoryGrid;
        protected RadzenDataGrid<CmsCaseAssigneesHistoryVM> LawyersGrid;
        protected RadzenDataGrid<CasePartyLinkVM> PartiesGrid;
        public IEnumerable<CmsCaseFileStatusHistoryVM> caseStatusHistory { get; set; } = new List<CmsCaseFileStatusHistoryVM>();
        public List<CmsCaseAssigneeVM> caseFileAssignees { get; set; } = new List<CmsCaseAssigneeVM>();
        public List<CmsDraftedTemplateVM> DraftDocuments { get; set; } = new List<CmsDraftedTemplateVM>();
        public List<CmsDraftedDocumentVM> DraftTemplate { get; set; } = new List<CmsDraftedDocumentVM>();
        public List<CmsDraftedDocumentVM> DraftedTemplateVersions { get; set; } = new List<CmsDraftedDocumentVM>();
        public IEnumerable<CmsCaseAssigneesHistoryVM> caseFileAssigneesHistory { get; set; } = new List<CmsCaseAssigneesHistoryVM>();
        protected RadzenDataGrid<CommunicationListVM> CommunicationGrid;
        public IEnumerable<CommunicationListVM> communicationListVm = new List<CommunicationListVM>();
        protected IEnumerable<CmsCaseFileVM> linkedFiles;
        protected bool RefreshFileUploadGrid { get; set; } = true;
        protected CmsApprovalTracking approvalTracking { get; set; } = new CmsApprovalTracking();
        protected RadzenDataGrid<CmsTransferHistoryVM> transferHistoryGrid;
        protected IEnumerable<CmsTransferHistoryVM> cmsCaseFileTransferHistory = new List<CmsTransferHistoryVM>();

        protected int sectorTo { get; set; }
        protected int sectorFrom { get; set; }
        protected List<WorkflowActivityOptionVM> activityOptions { get; set; } = new List<WorkflowActivityOptionVM>();
        protected WorkflowInstance workflowInstance { get; set; } = new WorkflowInstance();
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
                await PopulateCommunicationList(FileId);
                await PopulateApprovalTrackingDetails();
                await PopulateLinkedFiles();
                await PopulateDraftGrid();
                await PopulateRegisteredCasesByFileId();
                if (TaskId != null)
                {
                    await PopulateTaskDetails();
                    await GetManagerTaskReminderData();
                }

                if (approvalTracking != null && approvalTracking.ProcessTypeId == (int)ApprovalProcessTypeEnum.Transfer)
                {
                    await PopulateCurrentInstanceByApprovalTrackingId();
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
        protected async Task PopulateCurrentInstanceByApprovalTrackingId()
        {
            if (approvalTracking != null)
            {
                var response = await workflowService.GetCurrentInstanceByReferneceId(approvalTracking.Id);
                if (response.IsSuccessStatusCode)
                {
                    workflowInstance = (WorkflowInstance)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
        }

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

        //<History Author = 'Hassan Abbas' Date='2022-12-21' Version="1.0" Branch="master"Get Approval Tracking Process</History>
        protected async Task PopulateApprovalTrackingDetails()
        {
            var response = await cmsSharedService.GetApprovalTrackingProcess(Guid.Parse(FileId), (int)loginState.UserDetail.SectorTypeId, (int)ApprovalProcessTypeEnum.FileAssignment);
            if (response.IsSuccessStatusCode)
            {
                approvalTracking = (CmsApprovalTracking)response.ResultData;
                if (approvalTracking == null)
                {
                    response = await cmsSharedService.GetApprovalTrackingProcess(Guid.Parse(FileId), (int)loginState.UserDetail.SectorTypeId, (int)ApprovalProcessTypeEnum.Transfer);
                    if (response.IsSuccessStatusCode)

                    {
                        approvalTracking = (CmsApprovalTracking)response.ResultData;
                    }
                    else

                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }

                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

        }
        //<History Author = 'Hassan Abbas' Date='2023-03-10' Version="1.0" Branch="master">Populate Task Detail to be Approved or Rejected</History>
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

        protected async Task PopulateTransferHistoryGrid()
        {
            try
            {
                var historyResponse = await cmsSharedService.GetCMSTransferHistory(FileId);
                if (historyResponse.IsSuccessStatusCode)
                {
                    cmsCaseFileTransferHistory = (List<CmsTransferHistoryVM>)historyResponse.ResultData;
                    if (cmsCaseFileTransferHistory.Count() > 0)
                    {
                        sectorTo = cmsCaseFileTransferHistory.Select(x => x.SectorTo).First();
                        sectorFrom = cmsCaseFileTransferHistory.Select(x => x.SectorFrom).First();
                    }
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(historyResponse);
                }
            }
            catch (Exception ex)
            {

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
        #region Load Versions for a Draft
        protected async Task ExpandDraftVersions(CmsDraftedTemplateVM draftTemplate)
        {
            DraftedTemplateVersions = DraftTemplate.Where(x => x.Id == draftTemplate.Id).ToList();
        }
        #endregion
        #endregion

        #region Redirect Function
        //<History Author = 'Hassan Abbas' Date='2022-11-25' Version="1.0" Branch="master">Redirect to respective page</History>

        //private void GoBack()
        //{
        //    navigationManager.NavigateTo("/usertask-list");
        //}
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

        protected async Task DetailDraft(CmsDraftedDocumentVM args)
        {
            navigationManager.NavigateTo("draftdocument-detail/" + args.Id + "/" + args.VersionId);
        }
        #endregion

        #region Approve/Reject Functions

        protected async Task SelectOptions(MouseEventArgs args)
        {

            var crResult = await cmsCaseFileService.GetCaseFileById(Guid.Parse(FileId));
            if (crResult.IsSuccessStatusCode)
            {
                caseFile = (CaseFile)crResult.ResultData;

                var currentActivity = await workflowService.GetInstanceCurrentActivity(approvalTracking.Id);
                var response = await workflowService.GetWorkflowActivityOptionsByActivityId(currentActivity.WorkflowActivityId);
                if (response.IsSuccessStatusCode)
                {
                    activityOptions = (List<WorkflowActivityOptionVM>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    return;
                }
                if (activityOptions.Count > 0)
                {
                    var result = await dialogService.OpenAsync<SelectConditionOptionPopup>(translationState.Translate("Select_Option"),
                    new Dictionary<string, object>()
                        {
                                    { "Options", activityOptions},
                                    { "isActivity", true}

                        },
                        new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
                    if (result != null)
                    {
                        var SelectedOptionId = (int)result;
                        var selecetedActivityOption = activityOptions.Where(x => x.ModuleOptionId == SelectedOptionId).FirstOrDefault();
                        approvalTracking.CreatedBy = loginState.Username;
                        approvalTracking.ProcessTypeId = (int)ApprovalProcessTypeEnum.Transfer;
                        approvalTracking.SectorTypeId = loginState.UserDetail.SectorTypeId;
                        approvalTracking.AssignedBy = loginState.UserDetail.UserId;
                        approvalTracking.TransferCaseType = (int)AssignCaseToLawyerTypeEnum.CaseFile;
                        await workflowService.ProcessWorkflowOptionActivites(selecetedActivityOption, approvalTracking, (int)WorkflowModuleEnum.CaseManagement, (int)WorkflowModuleTriggerEnum.TransferCaseFile);
                        if (TaskId != null)
                        {
                            taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
                            if (loginState.UserDetail.RoleId == SystemRoles.ViceHOS || loginState.UserDetail.RoleId == SystemRoles.HOS)
                            {
                                taskDetailVM.IsMultipleTaskUpdateForSameEntity = true;
                                taskDetailVM.SectorId = loginState.UserDetail.SectorTypeId;
                            }
                            if (taskDetailVM.Url.StartsWith("casefile-transfer-review"))
                            {
                                taskDetailVM.Url = taskDetailVM.Url.Replace("casefile-transfer-review", "casefile-view");
                            }
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
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Please_Select_Any_Option"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        return;
                    }
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("No_Options_For_Current_Activity"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(crResult);
                return;
            }

        }

        protected async Task ApproveTransfer(MouseEventArgs args)
        {
            bool? dialogResponse = await dialogService.Confirm(
                 translationState.Translate("Sure_Approve"),
                 translationState.Translate("Confirm"),
                 new ConfirmOptions()
                 {
                     OkButtonText = @translationState.Translate("OK"),
                     CancelButtonText = @translationState.Translate("Cancel")
                 });


            if (dialogResponse == true)
            {
                spinnerService.Show();
                var crResult = await cmsCaseFileService.GetCaseFileById(Guid.Parse(FileId));
                if (crResult.IsSuccessStatusCode)
                {
                    caseFile = (CaseFile)crResult.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(crResult);
                    spinnerService.Hide();
                    return;
                }
                caseFile.FileId = Guid.Parse(FileId);
                caseFile.Remarks = approvalTracking.Remarks;
                caseFile.ModifiedBy = loginState.UserDetail.UserName;
                caseFile.SectorTypeId = loginState.UserDetail.SectorTypeId;
                caseFile.SectorFrom = approvalTracking.SectorFrom;
                caseFile.SectorTo = approvalTracking.SectorTo;
                caseFile.LoggedInUserId = loginState.UserDetail.UserId;
                approvalTracking.StatusId = (int)ApprovalStatusEnum.Approved;
                var response = await cmsSharedService.ApproveTransferSector(caseFile, (int)AssignCaseToLawyerTypeEnum.CaseFile);
                if (response.IsSuccessStatusCode)
                {
                    if (TaskId != null)
                    {
                        if (taskDetailVM.Url.StartsWith("casefile-transfer-review"))
                        {
                            taskDetailVM.Url = taskDetailVM.Url.Replace("casefile-transfer-review", "casefile-view");
                        }
                        taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
                        taskDetailVM.SectorId = loginState.UserDetail.SectorTypeId;
                        taskDetailVM.IsMultipleTaskUpdateForSameEntity = true;
                        var taskResponse = await taskService.DecisionTask(taskDetailVM);
                        if (!taskResponse.IsSuccessStatusCode)
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(taskResponse);
                        }
                        if (loginState.UserDetail.RoleId == SystemRoles.ViceHOS || loginState.UserDetail.RoleId == SystemRoles.HOS)
                        {
                            taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
                            taskDetailVM.IsMultipleTaskUpdateForSameEntity = true;
                            taskDetailVM.SectorId = approvalTracking.SectorFrom;
                            taskDetailVM.SystemGenTypeIdsToComplete.Add((int)TaskSystemGenTypeEnum.CaseFileAssignmentApproval);
                            taskDetailVM.SystemGenTypeIdsToComplete.Add((int)TaskSystemGenTypeEnum.FinalJudgementIssued);
                            var taskResponseAssignment = await taskService.DecisionTask(taskDetailVM);
                            if (!taskResponseAssignment.IsSuccessStatusCode)
                            {
                                await invalidRequestHandlerService.ReturnBadRequestNotification(taskResponse);
                            }
                        }
                        if (RegisteredCases.Any(x => x.IsFinalJudgement == true))
                        {
                            List<CmsRegisteredCaseFileDetailVM> filteredRegisteredCases = RegisteredCases.Where(r => r.IsFinalJudgement == true).ToList();
                            foreach (var item in filteredRegisteredCases)
                            {
                                taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
                                taskDetailVM.IsMultipleTaskUpdateForSameEntity = false;
                                taskDetailVM.ReferenceId = item.CaseId;
                                taskDetailVM.SectorId = approvalTracking.SectorFrom;
                                taskDetailVM.IsFinalJudgementTrue = true;
                                var taskResponses = await taskService.DecisionTask(taskDetailVM);
                                if (!taskResponses.IsSuccessStatusCode)
                                {
                                    await invalidRequestHandlerService.ReturnBadRequestNotification(taskResponse);
                                }
                            }
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


        protected async Task RejectTransfer(MouseEventArgs args)
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
                    var crResult = await cmsCaseFileService.GetCaseFileById(Guid.Parse(FileId));
                    if (crResult.IsSuccessStatusCode)
                    {
                        caseFile = (CaseFile)crResult.ResultData;
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(crResult);
                        spinnerService.Hide();
                        return;
                    }
                    caseFile.Remarks = (string)dialogResult;
                    caseFile.ModifiedBy = loginState.UserDetail.UserName;
                    caseFile.SectorTypeId = loginState.UserDetail.SectorTypeId;
                    caseFile.SectorFrom = approvalTracking.SectorFrom;
                    caseFile.SectorTo = approvalTracking.SectorTo;
                    var response = await cmsSharedService.RejectTransferSector(caseFile, (int)AssignCaseToLawyerTypeEnum.CaseFile);
                    if (response.IsSuccessStatusCode)
                    {
                        if (TaskId != null)
                        {
                            taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
                            taskDetailVM.SectorId = loginState.UserDetail.SectorTypeId;
                            taskDetailVM.IsMultipleTaskUpdateForSameEntity = true;
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


        #endregion
        #region Grid Button
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
