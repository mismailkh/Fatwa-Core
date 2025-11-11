using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_DOMAIN.Models.WorkflowModels;
using FATWA_WEB.Pages.CaseManagment.Shared;
using FATWA_WEB.Pages.Shared;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Radzen;
using Radzen.Blazor;
using Telerik.Blazor.Components;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;

namespace FATWA_WEB.Pages.CaseManagment.Request
{
    //<History Author = 'Hassan Abbas' Date='2022-12-15' Version="1.0" Branch="master">Reviewing Transfer Request </History>
    public partial class ReviewTransferCaseRequest : ComponentBase
    {

        #region parameter

        [Parameter]
        public string RequestId { get; set; }
        [Parameter]
        public string? TaskId { get; set; }

        #endregion

        #region Variable Declaration

        protected TaskDetailVM taskDetailVM = new TaskDetailVM();
        protected RadzenDataGrid<CasePartyLinkVM> PartiesGrid;
        protected RadzenDataGrid<CmsCaseRequestHistoryVM> HistoryGrid;

        protected RadzenDataGrid<CmsTransferHistoryVM> transferHistoryGrid;
        protected CaseRequestDetailVM caseRequestdetailVm = new CaseRequestDetailVM();
        protected CmsApprovalTracking approvalTracking { get; set; } = new CmsApprovalTracking();
        protected WorkflowInstance workflowInstance { get; set; } = new WorkflowInstance();

        protected IEnumerable<CmsCaseRequestHistoryVM> cmsCaseRequestHistory { get; set; } = new List<CmsCaseRequestHistoryVM>();
        protected IEnumerable<CmsTransferHistoryVM> cmsCaseRequestTransferHistory = new List<CmsTransferHistoryVM>();
        protected IEnumerable<CmsCaseRequestVM> linkedRequests;
        protected DateTime Min = new DateTime(DateTime.Now.Ticks);
        protected RadzenDataGrid<CommunicationListVM> CommunicationGrid;
        protected IEnumerable<CommunicationListVM> communicationListVm = new List<CommunicationListVM>();
        protected string ActivityEn;
        protected string ActivityAr;
        protected TelerikPdfViewer PdfViewerRef { get; set; }
        protected byte[] FileData { get; set; }
        protected string DocumentPath { get; set; }
        protected bool ShowDocumentViewer { get; set; }
        protected CaseRequest caseRequest { get; set; } = new CaseRequest();
        protected RadzenDataGrid<CmsDraftedDocumentVM> DraftCaseRequestGrid;

        protected List<CmsDraftedDocumentVM> cmsDraftCaseRequestVM;
        protected string RedirectURL { get; set; }
        protected int sectorTo { get; set; }
        protected int sectorFrom { get; set; }
        protected int caseRequestSector { get; set; }
        protected List<WorkflowActivityOptionVM> activityOptions { get; set; } = new List<WorkflowActivityOptionVM>();
        protected List<ManagerTaskReminderVM> managerTaskReminderData { get; set; } = new List<ManagerTaskReminderVM>();
        #endregion

        #region Draft Request Grid
        IEnumerable<CmsDraftedDocumentVM> _getCaseRequestDetail;
        protected IEnumerable<CmsDraftedDocumentVM> getCaseRequestDetail
        {
            get
            {
                return _getCaseRequestDetail;
            }
            set
            {
                if (!Equals(_getCaseRequestDetail, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getCaseRequestDetail", NewValue = value, OldValue = _getCaseRequestDetail };
                    _getCaseRequestDetail = value;

                    // Reload();
                }
            }
        }
        string _search;
        protected string search
        {
            get
            {
                return _search;
            }
            set
            {
                if (!Equals(_search, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "search", NewValue = value, OldValue = _search };
                    _search = value;

                    //Reload();
                }
            }
        }
        #endregion

        #region Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();

            var result = await caseRequestService.GetCaseRequestDetailById(Guid.Parse(RequestId));

            if (result.IsSuccessStatusCode)
            {
                caseRequestdetailVm = JsonConvert.DeserializeObject<CaseRequestDetailVM>(result.ResultData.ToString());
                //caseRequestdetailVm = (CaseRequestDetailVM)result.ResultData;
                caseRequestSector = (int)caseRequestdetailVm.SectorTypeId;

                await PopulateRequestHistoryGrid(RequestId);
                await PopulateRequestTransferHistoryGrid(RequestId);
                await PopulateCasePartyGrid(RequestId);
                await PopulateCommunicationList(RequestId);
                await PopulateLinkedRequests(RequestId);
                await PopulateApprovalTrackingDetails();
                await PopulateCurrentInstanceByApprovalTrackingId();
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

        #region Populate Grids Data

        protected async Task PopulateCasePartyGrid(string RequestId)
        {
            var partyResponse = await caseRequestService.GetCMSCasePartyDetailById(Guid.Parse(RequestId));
            if (partyResponse.IsSuccessStatusCode)
            {
                caseRequestdetailVm.CasePartyLinks = (List<CasePartyLinkVM>)partyResponse.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(partyResponse);
            }
        }

        protected async Task PopulateLinkedRequests(string RequestId)
        {
            var response = await caseRequestService.GetLinkedRequestsByPrimaryRequestId(RequestId);
            if (response.IsSuccessStatusCode)
            {
                linkedRequests = (List<CmsCaseRequestVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

        }

        protected async Task PopulateRequestHistoryGrid(string RequestId)
        {
            var historyResponse = await caseRequestService.GetCMSCaseRequestStatusHistory(RequestId);
            if (historyResponse.IsSuccessStatusCode)
            {
                cmsCaseRequestHistory = (List<CmsCaseRequestHistoryVM>)historyResponse.ResultData;

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(historyResponse);
            }

        }

        protected async Task PopulateRequestTransferHistoryGrid(string RequestId)
        {
            var historyResponse = await cmsSharedService.GetCMSTransferHistory(RequestId);
            if (historyResponse.IsSuccessStatusCode)
            {
                cmsCaseRequestTransferHistory = (List<CmsTransferHistoryVM>)historyResponse.ResultData;
                if (cmsCaseRequestTransferHistory.Any())
                {
                    sectorTo = cmsCaseRequestTransferHistory.Select(x => x.SectorTo).First();
                    sectorFrom = cmsCaseRequestTransferHistory.Select(x => x.SectorFrom).First();
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(historyResponse);
            }

        }

        //<History Author = 'Hassan Abbas' Date='2022-12-21' Version="1.0" Branch="master"Get Approval Tracking Process</History>
        protected async Task PopulateApprovalTrackingDetails()
        {
            var response = await cmsSharedService.GetApprovalTrackingProcess(Guid.Parse(RequestId), (int)loginState.UserDetail.SectorTypeId, (int)ApprovalProcessTypeEnum.Transfer);
            if (response.IsSuccessStatusCode)
            {
                approvalTracking = (CmsApprovalTracking)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

        }

        public async Task PopulateCommunicationList(string RequestId)
        {
            var CommunicationResponse = await communicationService.GetCommunicationListByCaseRequestId(Guid.Parse(RequestId));
            if (CommunicationResponse.IsSuccessStatusCode)
            {
                communicationListVm = (List<CommunicationListVM>)CommunicationResponse.ResultData;
                if (communicationListVm.Any())
                {
                    ActivityEn = communicationListVm.FirstOrDefault().Activity_En;
                    ActivityAr = communicationListVm.FirstOrDefault().Activity_Ar;
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(CommunicationResponse);
            }
        }
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
        #endregion

        #region Buttons

        protected void ButtonCancelClick(MouseEventArgs args)
        {
            navigationManager.NavigateTo(navigationState.ReturnUrl);
        }

        #endregion

        #region Approve/Reject Functions

        protected async Task ApproveTransfer(MouseEventArgs args)
        {
            var crResult = await caseRequestService.GetCaseRequestById(Guid.Parse(RequestId));
            if (crResult.IsSuccessStatusCode)
            {
                caseRequest = (CaseRequest)crResult.ResultData;

                var currentActivity = await workflowService.GetInstanceCurrentActivity(approvalTracking.Id);
                if (currentActivity != null)
                {
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
                        spinnerService.Show();
                        var SelectedOptionId = (int)result;
                        var selecetedActivityOption = activityOptions.Where(x => x.ModuleOptionId == SelectedOptionId).FirstOrDefault();
                        approvalTracking.CreatedBy = loginState.Username;
                        approvalTracking.ProcessTypeId = (int)ApprovalProcessTypeEnum.Transfer;
                        approvalTracking.SectorTypeId = loginState.UserDetail.SectorTypeId;
                        approvalTracking.AssignedBy = loginState.UserDetail.UserId;
                        approvalTracking.TransferCaseType = (int)AssignCaseToLawyerTypeEnum.CaseRequest;
                        await workflowService.ProcessWorkflowOptionActivites(selecetedActivityOption, approvalTracking, (int)WorkflowModuleEnum.CaseManagement, (int)WorkflowModuleTriggerEnum.TransferCaseRequest);
                        if (TaskId != null)
                        {
                            taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
                            if (loginState.UserDetail.RoleId == SystemRoles.ViceHOS || loginState.UserDetail.RoleId == SystemRoles.HOS)
                            {
                                taskDetailVM.IsMultipleTaskUpdateForSameEntity = true;
                                taskDetailVM.SectorId = loginState.UserDetail.SectorTypeId;
                            }
                            if (taskDetailVM.Url.StartsWith("caserequest-transfer-review"))
                            {
                                taskDetailVM.Url = taskDetailVM.Url.Replace("caserequest-transfer-review", "caserequest-view");
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
                        spinnerService.Hide();

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
                    var crResult = await caseRequestService.GetCaseRequestById(Guid.Parse(RequestId));
                    if (crResult.IsSuccessStatusCode)
                    {
                        caseRequest = (CaseRequest)crResult.ResultData;
                        caseRequest.RequestId = Guid.Parse(RequestId);
                        caseRequest.Remarks = dialogResult;
                        caseRequest.CreatedBy = loginState.Username;
                        approvalTracking.StatusId = (int)ApprovalStatusEnum.Rejected;
                        approvalTracking.TransferCaseType = (int)AssignCaseToLawyerTypeEnum.CaseRequest;
                        approvalTracking.Item = caseRequest;
                        approvalTracking.AssignedBy = loginState.UserDetail.UserId;
                        approvalTracking.TransferStatusId = (int)ApprovalStatusEnum.Rejected;
                        approvalTracking.SectorTypeId = loginState.UserDetail.SectorTypeId;
                        await workflowService.ProcessWorkflowActvivities(approvalTracking, (int)WorkflowModuleEnum.CaseManagement, (int)WorkflowModuleTriggerEnum.TransferCaseRequest);
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
                        await RedirectBack();
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(crResult);
                        spinnerService.Hide();
                        return;
                    }

                    spinnerService.Hide();
                }
            }
        }

        protected async Task Transfer(MouseEventArgs args)
        {
            var dialogResult = await dialogService.OpenAsync<TransferSector>
                (
                translationState.Translate("Transfer"),
                new Dictionary<string, object>() {
                    { "ReferenceId", Guid.Parse(RequestId) },
                    { "TransferCaseType", (int)AssignCaseToLawyerTypeEnum.CaseRequest },
                    { "IsAssignment", false },
                    { "IsConfidential", caseRequestdetailVm.IsConfidential },
                    { "RequestTypeId", caseRequestdetailVm.RequestTypeId },
                    { "RejectedTransferIds", cmsCaseRequestTransferHistory.Where( x => x.StatusId == (int)ApprovalTrackingStatusEnum.Rejected).Select(x => x.SectorFrom).ToList() },
                    { "SenderSector", cmsCaseRequestTransferHistory.Select(x => x.SectorFrom).FirstOrDefault() }
                },
                new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });

            if (dialogResult != null)
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
                approvalTracking.StatusId = (int)ApprovalStatusEnum.Rejected;
                await RedirectBack();
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

        protected void ViewHistoryDetail(CmsCaseRequestHistoryVM item)
        {
            navigationManager.NavigateTo("/caserequest-historydetail/" + item.HistoryId);
        }

        protected void ViewResponse(CommunicationListVM item)
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

        #region Linked Requests Grid Buttons

        //<History Author = 'Hassan Abbas' Date='2022-12-05' Version="1.0" Branch="master"> Redirect to View Detail page</History>
        protected async Task DetailLinkedCaseRequest(CmsCaseRequestVM args)
        {
            navigationState.ReturnUrl = "caserequest-view/" + RequestId;
            navigationManager.NavigateTo("caserequest-view/" + args.RequestId);
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
