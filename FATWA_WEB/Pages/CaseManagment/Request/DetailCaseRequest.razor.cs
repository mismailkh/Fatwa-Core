using DocumentFormat.OpenXml.Spreadsheet;
using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_WEB.Pages.CaseManagment.Shared;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Radzen;
using Radzen.Blazor;
using System.Collections.ObjectModel;
using Telerik.Blazor.Components;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;

namespace FATWA_WEB.Pages.CaseManagment.Request
{
    public partial class DetailCaseRequest : ComponentBase
    {
        #region parameter
        [Parameter]
        public string RequestId { get; set; }
        //New Parameter add  public dynamic assignCaseToLawyerTypeEnum { get; set; }
        public dynamic AssignCaseLawyerType { get; set; }
        [Parameter]
        public string? TaskId { get; set; }
        #endregion

        #region Variable Declaration

        protected RadzenDataGrid<CasePartyLinkVM> PartiesGrid;
        protected RadzenDataGrid<CmsCaseRequestHistoryVM> HistoryGrid;
        protected RadzenDataGrid<CmsTransferHistoryVM> transferHistoryGrid;
        protected ObservableCollection<TempAttachementVM> OfficialAttachments = new ObservableCollection<TempAttachementVM>();
        protected List<AttachmentType> AttachmentTypes = new List<AttachmentType>();
        protected CaseRequestDetailVM caseRequestdetailVm = new CaseRequestDetailVM();
        protected TaskDetailVM taskDetailVM = new TaskDetailVM();

        protected IEnumerable<CmsCaseRequestHistoryVM> cmsCaseRequestHistory = new List<CmsCaseRequestHistoryVM>();
        protected IEnumerable<CmsTransferHistoryVM> cmsCaseRequestTransferHistory = new List<CmsTransferHistoryVM>();
        protected CmsTransferHistoryVM caseRequestTransferHistoryobj = new CmsTransferHistoryVM();
        protected IEnumerable<CmsCaseRequestVM> linkedRequests;
        protected DateTime Min = new DateTime(DateTime.Now.Ticks);
        protected RadzenDataGrid<CommunicationListVM> CommunicationGrid;
        protected IEnumerable<CommunicationListVM> communicationListVm = new List<CommunicationListVM>();
        protected string ActivityEn;
        protected string ActivityAr;
        protected string InboxNumber;
        protected string OutboxNumber;
        protected TelerikPdfViewer PdfViewerRef { get; set; }
        protected byte[] FileData { get; set; }
        protected string DocumentPath { get; set; }
        protected bool ShowDocumentViewer = false;
        protected string RedirectURL { get; set; }
        protected CaseRequest caseRequest { get; set; } = new CaseRequest();
        protected RadzenDataGrid<CmsDraftedDocumentVM> DraftCaseRequestGrid;
        protected List<CmsDraftedDocumentVM> cmsDraftCaseRequestVM;
        List<CommunicationMeetingDetailVM> _communicationMeetinglistVM;
        protected List<CommunicationMeetingDetailVM> communicationMeetinglistVM
        {
            get
            {
                return _communicationMeetinglistVM;
            }
            set
            {
                if (!object.Equals(_communicationMeetinglistVM, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "communicationMeetinglistVM", NewValue = value, OldValue = _communicationMeetinglistVM };
                    _communicationMeetinglistVM = value;
                    //OnPropertyChanged(args);
                    //Reload();
                }
            }
        }
        protected List<ManagerTaskReminderVM> managerTaskReminderData { get; set; } = new List<ManagerTaskReminderVM>();

        #endregion

        #region Send Response Detail View Grid Load Properties Load

        CommunicationSendResponseVM _communicationSendResponseVM;
        protected CommunicationSendResponseVM communicationSendResponseVM
        {
            get
            {
                return _communicationSendResponseVM;
            }
            set
            {
                if (!object.Equals(_communicationSendResponseVM, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "CommunicationSendResponseVM", NewValue = value, OldValue = _communicationSendResponseVM };
                    _communicationSendResponseVM = value;

                }
            }
        }

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
            await Load();
            spinnerService.Hide();
        }
        protected async Task Load()
        {
            spinnerService.Show();
            var result = await caseRequestService.GetCaseRequestDetailById(Guid.Parse(RequestId));

            if (result.IsSuccessStatusCode)
            {
                caseRequestdetailVm = JsonConvert.DeserializeObject<CaseRequestDetailVM>(result.ResultData.ToString());
                //caseRequestdetailVm = (CaseRequestDetailVM)result.ResultData;

                await PopulateRequestHistoryGrid(RequestId);
                await PopulateRequestTransferHistoryGrid(RequestId);
                await PopulateCasePartyGrid(RequestId);
                await PopulateCommunicationList(RequestId);
                await PopulateLinkedRequests(RequestId);
                //await GetDraftCasesCmsCaseRequest();
                await PopulateAttachmentTypes();
                await caseRequestService.UpdateCaseRequestViewedStatus(Guid.Parse(RequestId));
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
                if (cmsCaseRequestTransferHistory.Count() > 0)
                    caseRequestTransferHistoryobj = cmsCaseRequestTransferHistory.FirstOrDefault();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(historyResponse);
            }

        }

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Populate Subtypes data</History>
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
                    OutboxNumber = communicationListVm.FirstOrDefault().OutboxNumber;
                    InboxNumber = communicationListVm.FirstOrDefault().InboxNumber;
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(CommunicationResponse);
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

        #region  Buttons

        protected void ButtonCancelClick()
        {
            //DialogService.Close(null);
            navigationManager.NavigateTo("/caserequest-view/" + RequestId);
        }
        protected async Task Transfer(MouseEventArgs args)
        {
            var RejectedTransferIds = cmsCaseRequestTransferHistory.Where(x => x.StatusId == (int)ApprovalTrackingStatusEnum.Rejected).Select(x => x.SectorFrom).ToList();
            RejectedTransferIds.AddRange(cmsCaseRequestTransferHistory.Where(x => x.StatusId == (int)ApprovalTrackingStatusEnum.RejectedByFatwaPresident).Select(x => x.SectorTo).ToList());
            var dialogResult = await dialogService.OpenAsync<TransferSector>
                (
                translationState.Translate("Transfer"),
                new Dictionary<string, object>() {
                    { "ReferenceId", Guid.Parse(RequestId) },
                    { "TransferCaseType", (int)AssignCaseToLawyerTypeEnum.CaseRequest },
                    { "IsAssignment", false },
                    { "IsConfidential", caseRequestdetailVm.IsConfidential },
                    { "RequestTypeId", caseRequestdetailVm.RequestTypeId },
                    { "RejectedTransferIds", RejectedTransferIds },
                    { "SenderSector", cmsCaseRequestTransferHistory.Select(x => x.SectorFrom).FirstOrDefault() }
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
        }
        protected async Task MeetingRequest(MouseEventArgs args)
        {
            try
            {
                Guid ReferenceId = Guid.Parse(RequestId);
                int SubModuleId = (int)SubModuleEnum.CaseRequest;
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
        protected async Task SendCopyofRequest(MouseEventArgs args)
        {
            var dialogResult = await dialogService.OpenAsync<SendACopyCaseRequest>(
                translationState.Translate("SendAcopy"),
                new Dictionary<string, object>() {
                    { "ReferenceId", Guid.Parse(RequestId) } ,
                    { "SendACopyType", (int)AssignCaseToLawyerTypeEnum.CaseRequest } ,
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
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Open Add Party dialog</History>
        protected async Task AssignToLawyer()
        {
            var result = await dialogService.OpenAsync<AssignToLawyer>(translationState.Translate("Assign_To_Lawyer"),
               new Dictionary<string, object>()
               {
                        { "RequestId", Guid.Parse(RequestId) },
                        { "AssignCaseLawyerType", (int)AssignCaseToLawyerTypeEnum.CaseRequest },
                        { "TaskId", TaskId },

               }
               ,
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

        #endregion

        #region View Details
        protected void ViewHistoryDetail(CmsCaseRequestHistoryVM item)
        {
            navigationManager.NavigateTo("/caserequest-historydetail/" + item.HistoryId);
        }

        protected async void ViewResponse(CommunicationListVM item)
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

        #region Meeting
        protected async Task AddMeeting(CommunicationListVM item)
        {
            try
            {
                Guid CommunicationId = item.CommunicationId;
                Guid ReferenceId = Guid.Parse(RequestId);
                int SubModuleId = (int)SubModuleEnum.CaseRequest;
                string ReceivedBy = System.Net.WebUtility.UrlEncode(caseRequestdetailVm.CreatedBy).Replace(".", "%999");
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

        #region RowCellRender
        protected void RowCellRender(RowRenderEventArgs<CommunicationListVM> commuication)
        {
            if (commuication.Data.IsRead == true && commuication.Data.CommunicationTypeId != (int)CommunicationTypeEnum.CaseRequest)
            {
                commuication.Attributes.Add("style", $"background-color: #e3d4c9;");
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

