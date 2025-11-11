using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_WEB.Pages.CaseManagment.RegisteredCase;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;

namespace FATWA_WEB.Pages.CaseManagment.MOJ
{
    //<History Author = 'Hassan Abbas' Date='2023-03-30' Version="1.0" Branch="master"> Case Execution Request Detail</History>
    public partial class DetailExecutionRequest : ComponentBase
    {

        #region Parameters
        [Parameter]
        public string ExecutionId { get; set; }
        [Parameter]
        public string? TaskId { get; set; }
        #endregion

        #region Variables
        public Guid FileIdForViewDetail { get; set; }

        protected TaskDetailVM taskDetailVM { get; set; }
        protected CmsJudgmentExecution judgementExecution { get; set; }
        protected MojExecutionRequest executionRequest { get; set; } = new MojExecutionRequest();
        protected CmsRegisteredCaseDetailVM registeredCase { get; set; } = new CmsRegisteredCaseDetailVM();
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
        protected List<JudgementVM> Judgements = new List<JudgementVM>();
        protected List<CmsJudgmentExecutionVM> CmsJudgmentExecutions = new List<CmsJudgmentExecutionVM>();
        protected RadzenDataGrid<CommunicationListCaseFileVM>? CommunicationGrid;
        public IEnumerable<CommunicationListCaseFileVM> communicationListVm = new List<CommunicationListCaseFileVM>();
        protected CmsApprovalTracking approvalTracking { get; set; } = new CmsApprovalTracking();
        public string ActivityEn;
        public string ActivityAr;
        protected string InboxNumber;
        protected string OutboxNumber;
        protected bool RefreshFileUploadGrid { get; set; } = true;
        protected bool IsRespectiveSector { get; set; }

        protected RadzenDataGrid<CmsDraftedDocumentVM> DraftCaseRequestGrid;

        #endregion

        #region Draft Request Grid
        IEnumerable<CmsDraftedDocumentVM> _getCmsDraftDocumentDetail;
        protected IEnumerable<CmsDraftedDocumentVM> getCmsDraftDocumentDetail
        {
            get
            {
                return _getCmsDraftDocumentDetail;
            }
            set
            {
                if (!object.Equals(_getCmsDraftDocumentDetail, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getCmsDraftDocumentDetail", NewValue = value, OldValue = _getCmsDraftDocumentDetail };
                    _getCmsDraftDocumentDetail = value;

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
                if (!object.Equals(_search, value))
                {
                    var args = new FATWA_WEB.Services.PropertyChangedEventArgs() { Name = "search", NewValue = value, OldValue = _search };
                    _search = value;

                    //Reload();
                }
            }
        }
        #endregion

        #region Component Load

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Component Load</History>
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();

            try
            {
                var result = await cmsRegisteredCaseService.GetExecutionRequestById(Guid.Parse(ExecutionId));

                if (result.IsSuccessStatusCode)
                {
                    executionRequest = (MojExecutionRequest)result.ResultData;
                    var caseResult = await cmsRegisteredCaseService.GetRegisteredCaseDetailByIdVM((Guid)executionRequest.CaseId);
                    if (caseResult.IsSuccessStatusCode)
                    {
                        registeredCase = (CmsRegisteredCaseDetailVM)caseResult.ResultData;
                        if (registeredCase.IsDissolved == null)
                            registeredCase.IsDissolved = false;
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(caseResult);
                    }

                    FileIdForViewDetail = (Guid)registeredCase.FileId;
                    await PopulateCaseFileGrid();
                    //if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.Execution)
                    //{
                        if (loginState.UserRoles.Any(u => u.RoleId == SystemRoles.HOS || u.RoleId==SystemRoles.ViceHOS))
                        {
                            await PopulateApprovalTrackingDetails();
                        }
                    //}
                    await PopulateTaskDetails();
                    await PopulateJudgementExecutionDetails();
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(result);
                }

                spinnerService.Hide();
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

        #region Populate Grids

        //<History Author = 'Hassan Abbas' Date='2023-03-10' Version="1.0" Branch="master">Populate Task Detail to be Approved or Rejected</History>
        protected async Task PopulateJudgementExecutionDetails()
        {
            var getTaskDetail = await cmsRegisteredCaseService.GetJudgementExecutionByExecutionRequestId(Guid.Parse(ExecutionId));
            if (getTaskDetail.IsSuccessStatusCode)
            {
                judgementExecution = (CmsJudgmentExecution)getTaskDetail.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(getTaskDetail);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2023-03-10' Version="1.0" Branch="master">Populate Task Detail to be Approved or Rejected</History>
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
                    await invalidRequestHandlerService.ReturnBadRequestNotification(getTaskDetail);
                }
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

        //<History Author = 'Hassan Abbas' Date='2022-12-21' Version="1.0" Branch="master"Get Approval Tracking Process</History>
        protected async Task PopulateApprovalTrackingDetails()
        {
            var response = await cmsSharedService.GetApprovalTrackingProcess(executionRequest.Id, (int)loginState.UserDetail.SectorTypeId, (int)ApprovalProcessTypeEnum.ExecutionRequest);
            if (response.IsSuccessStatusCode)
            {
                approvalTracking = (CmsApprovalTracking)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

        }
        #endregion

        #region Redirect Function
        //<History Author = 'Hassan Abbas' Date='2022-02-03' Version="1.0" Branch="master"> Redirect back to previous page from browser history</History>
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

        #region Approve/Reject Functions


        //<History Author = 'Hassan Abbas' Date='2023-04-05' Version="1.0" Branch="master"></History>
        protected async Task ApproveExecutionRequest(MouseEventArgs args)
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

                executionRequest.SectorTypeId = (int)loginState.UserDetail.SectorTypeId;
                executionRequest.ModifiedBy = loginState.Username;

                var response = await cmsSharedService.ApproveExecutionRequest(executionRequest);
                if (response.IsSuccessStatusCode)
                {
                    if (TaskId != null)
                    {
                        taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
                        if(loginState.UserDetail.RoleId==SystemRoles.HOS|| loginState.UserDetail.RoleId == SystemRoles.ViceHOS)
                        {
                            taskDetailVM.IsMultipleTaskUpdateForSameEntity = true;
                            taskDetailVM.SectorId = executionRequest.SectorTypeId;

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
                    approvalTracking.StatusId = (int)ApprovalStatusEnum.Approved;
                    await RedirectBack();
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                spinnerService.Hide();
            }
        }

        //<History Author = 'Hassan Abbas' Date='2023-04-05' Version="1.0" Branch="master"></History>

        protected async Task RejectExecutionRequest(MouseEventArgs args)
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
                spinnerService.Show();

                executionRequest.SectorTypeId = (int)loginState.UserDetail.SectorTypeId;
                executionRequest.ModifiedBy = loginState.Username;
               
                var response = await cmsSharedService.RejectExecutionRequest(executionRequest);
                if (response.IsSuccessStatusCode)
                {
                    if (TaskId != null)
                    {
                        taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
                        if (loginState.UserDetail.RoleId == SystemRoles.HOS || loginState.UserDetail.RoleId == SystemRoles.ViceHOS)
                        {
                            taskDetailVM.IsMultipleTaskUpdateForSameEntity = true;
                            taskDetailVM.SectorId = executionRequest.SectorTypeId;

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
                    approvalTracking.StatusId = (int)ApprovalStatusEnum.Rejected;
                    await RedirectBack();
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                spinnerService.Hide();
            }
        }

        //<History Author = 'Hassan Abbas' Date='2023-04-05' Version="1.0" Branch="master"></History>
        protected async Task SendExecutionRequestToMOJExecution(MouseEventArgs args)
        {
            bool? dialogResponse = await dialogService.Confirm(
                 translationState.Translate("Sure_Submit"),
                 translationState.Translate("Confirm"),
                 new ConfirmOptions()
                 {
                     OkButtonText = @translationState.Translate("OK"),
                     CancelButtonText = @translationState.Translate("Cancel")
                 });

            if (dialogResponse == true)
            {
                spinnerService.Show();

                executionRequest.SectorTypeId = (int)loginState.UserDetail.SectorTypeId;
                executionRequest.ModifiedBy = loginState.Username;

                var response = await cmsSharedService.SendExecutionRequestToMOJExecution(executionRequest);
                if (response.IsSuccessStatusCode)
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
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                spinnerService.Hide();
            }
        }
        //<History Author = 'Hassan Abbas' Date='2023-04-05' Version="1.0" Branch="master"></History>
        protected async Task SendExecutionDetailsToLawyer(MouseEventArgs args)
        {
            bool? dialogResponse = await dialogService.Confirm(
                 translationState.Translate("Sure_Submit"),
                 translationState.Translate("Confirm"),
                 new ConfirmOptions()
                 {
                     OkButtonText = @translationState.Translate("OK"),
                     CancelButtonText = @translationState.Translate("Cancel")
                 });

            if (dialogResponse == true)
            {
                spinnerService.Show();

                judgementExecution.ExecutionRequestId = Guid.Empty;
                judgementExecution.ModifiedBy = loginState.Username;
                judgementExecution.CaseId = executionRequest.CaseId;
                judgementExecution.LawyerUserName = executionRequest.CreatedBy;

                judgementExecution.ExecutionRequest = executionRequest;

                var response = await cmsRegisteredCaseService.EditJudgmentExecution(judgementExecution);
                if (response.IsSuccessStatusCode)
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
                    await CopyAttachmentsFromSourceToDestination(new CopyAttachmentVM { SourceId = judgementExecution.Id, DestinationId = (Guid)judgementExecution.CaseId, CreatedBy = loginState.Username });
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Changes_saved_successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    approvalTracking.StatusId = (int)ApprovalStatusEnum.Rejected;
                    await RedirectBack();
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                spinnerService.Hide();
            }
        }

        protected async Task CopyAttachmentsFromSourceToDestination(CopyAttachmentVM item)
        {
            try
            {
                List<CopyAttachmentVM> copyAttachments = new List<CopyAttachmentVM>();
                copyAttachments.Add(item);
                var docResponse = await fileUploadService.CopyAttachmentsFromSourceToDestination(copyAttachments);
                if (!docResponse.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Attachment_Save_Failed"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
                }
            }
            catch (Exception)
            {
                return;
                throw;
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
                        { "ExecutionRequestId", executionRequest.Id },
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
