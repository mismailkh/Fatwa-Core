using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_DOMAIN.Models.WorkflowModels;
using FATWA_WEB.Pages.CaseManagment.Shared;
using FATWA_WEB.Pages.Meet;
using FATWA_WEB.Pages.Shared;
using FATWA_WEB.Pages.Tasks;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Telerik.Blazor.Components;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.Consultation.Request
{
    public partial class ReviewTransferConsultationRequest : ComponentBase
    {
        #region Parmater
        [Parameter]
        public dynamic ConsultationRequestId { get; set; }
        [Parameter]

        public dynamic SectorTypeId { get; set; }
        [Parameter]

        public dynamic TaskId { get; set; }

        #endregion

        #region variable declaration

        public int SectorTypeIdCheck = 0;
        protected bool ShowDocumentViewer { get; set; }
        protected TelerikPdfViewer PdfViewerRef { get; set; }
        protected byte[] FileData { get; set; }
        protected RadzenDataGrid<ConsultationPartyListVM>? consultationPartyGrid = new RadzenDataGrid<ConsultationPartyListVM>();
        public IEnumerable<ConsultationPartyListVM> consultationPartyVM { get; set; } = new List<ConsultationPartyListVM>();
        protected CmsApprovalTracking approvalTracking { get; set; } = new CmsApprovalTracking();

        public ObservableCollection<TempAttachementVM> OfficialAttachments { get; set; } = new ObservableCollection<TempAttachementVM>();
        protected List<AttachmentType> AttachmentTypes { get; set; } = new List<AttachmentType>();
        protected string DocumentPath { get; set; }
        protected string ActivityEn;
        protected string ActivityAr;
        public ViewConsultationVM consultationRequestVM = new ViewConsultationVM();
        protected RadzenDataGrid<CommunicationListVM> CommunicationGrid;
        protected IEnumerable<CommunicationListVM> communicationListVm = new List<CommunicationListVM>();
        protected RadzenDataGrid<ComsConsultationRequestHistoryVM> HistoryGrid;
        protected IEnumerable<ComsConsultationRequestHistoryVM> cmsConsultationRequestHistory { get; set; } = new List<ComsConsultationRequestHistoryVM>();
        protected ConsultationRequest consultationRequest { get; set; } = new ConsultationRequest();
        protected RadzenDataGrid<ConsultationArticleByConsultationIdListVM>? consultationArticleGrid = new RadzenDataGrid<ConsultationArticleByConsultationIdListVM>();
        public IEnumerable<ConsultationArticleByConsultationIdListVM> consultationArticleByConsultationIdListVM { get; set; } = new List<ConsultationArticleByConsultationIdListVM>();
        protected TaskDetailVM taskDetailVM { get; set; }
        protected string RedirectURL { get; set; }
        public bool IsStatusConsultationRequest { get; set; }
        protected List<WorkflowActivityOptionVM> activityOptions { get; set; } = new List<WorkflowActivityOptionVM>();
        protected WorkflowInstance workflowInstance { get; set; } = new WorkflowInstance();

        protected RadzenDataGrid<CmsTransferHistoryVM> transferHistoryGrid;
        protected IEnumerable<CmsTransferHistoryVM> cmsCaseRequestTransferHistory = new List<CmsTransferHistoryVM>();
        protected CmsTransferHistoryVM caseRequestTransferHistoryobj = new CmsTransferHistoryVM();
        protected List<ManagerTaskReminderVM> managerTaskReminderData { get; set; } = new List<ManagerTaskReminderVM>();


        #endregion

        #region On Load Component
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            if (SectorTypeId != null)
            {
                SectorTypeIdCheck = Convert.ToInt32(SectorTypeId);
            }

            var result = await consultationRequestService.GetConsultationDetailById(Guid.Parse(ConsultationRequestId));


            if (result.IsSuccessStatusCode)
            {
                consultationRequestVM = (ViewConsultationVM)result.ResultData;
                await PopulateCommunicationList((Guid.Parse(ConsultationRequestId)).ToString());
                await PopulateRequestHistoryGrid((Guid.Parse(ConsultationRequestId)).ToString());
                await PopulateConsltationPartyList(Guid.Parse(ConsultationRequestId));
                await PopulateApprovalTrackingDetails();
                await PopulateCurrentInstanceByApprovalTrackingId();
                await PopulateRequestTransferHistoryGrid((string)ConsultationRequestId);
                if (TaskId != null)
                {
                    await GetManagerTaskReminderData();
                }

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }

            var getTaskDetail = await taskService.GetTaskDetailById(Guid.Parse(TaskId));
            if (getTaskDetail.IsSuccessStatusCode)
            {
                taskDetailVM = (TaskDetailVM)getTaskDetail.ResultData;

            }
            else
            {
                taskDetailVM = new TaskDetailVM();
            }

            spinnerService.Hide();
        }

        #endregion

        #region Component After Render

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Load Authority Letter after component render</History>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                //load authority letter
                await LoadAuthorityLetter();
            }
        }

        #endregion

        #region attachment grid show
        protected async Task LoadAuthorityLetter()
        {
            try
            {
                var response = new ApiCallResponse();
                OfficialAttachments = await fileUploadService.GetOfficialDocuments(Guid.Parse(ConsultationRequestId));
                var authorityLetter = OfficialAttachments?.Where(t => t.AttachmentTypeId == (int)AttachmentTypeEnum.AuthorityLetter).FirstOrDefault();
                if (authorityLetter != null)
                {
#if DEBUG
                    {

                        var physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + authorityLetter.StoragePath).Replace(@"\\", @"\");

                        if (File.Exists(physicalPath))
                        {
                            FileData = File.ReadAllBytes(physicalPath);
                            DocumentPath = "data:application/pdf;base64," + Convert.ToBase64String(FileData);
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
#else
{

                            // Construct the physical path of the file on the server
                            var physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + authorityLetter.StoragePath).Replace(@"\\", @"\");
                            // Remove the wwwroot/Attachments part of the path to get the actual file path
                            physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
                            // Create a new HttpClient instance to download the file
                            using var httpClient = new HttpClient();
                            var httpresponse = await httpClient.GetAsync(physicalPath);
                            // Check if the file was downloaded successfully
                            if (httpresponse.IsSuccessStatusCode)
                            {
                                // Read the file content as a byte array
                                var fileData = await httpresponse.Content.ReadAsByteArrayAsync();
                                FileData = fileData;
                                DocumentPath = "data:application/pdf;base64," + Convert.ToBase64String(FileData);
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
#endif

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

        #region Populate functions
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

        public async Task PopulateCommunicationList(string ConsultationRequestId)
        {
            var CommunicationResponse = await communicationService.GetCommunicationListByConsultationRequestId(Guid.Parse(ConsultationRequestId));
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
        protected async Task PopulateRequestHistoryGrid(string ConsultationRequestId)
        {
            var historyResponse = await consultationRequestService.GetCOMSConsultationRequestStatusHistory(ConsultationRequestId);
            if (historyResponse.IsSuccessStatusCode)
            {
                cmsConsultationRequestHistory = (List<ComsConsultationRequestHistoryVM>)historyResponse.ResultData;

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(historyResponse);
            }

        }
        protected async Task PopulateConsltationPartyList(Guid ConsultationRequestId)
        {
            var response = await consultationRequestService.GetConsultationPartyByConsultationId(ConsultationRequestId);
            if (response.IsSuccessStatusCode)
            {
                consultationPartyVM = (List<ConsultationPartyListVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        //<History Author = 'Muhammad Zaeem' Date='2022-1-18' Version="1.0" Branch="master"Get Approval Tracking Process</History>
        protected async Task PopulateApprovalTrackingDetails()
        {
            var response = await comsSharedService.GetApprovalTrackingProcess(Guid.Parse(ConsultationRequestId), (int)loginState.UserDetail.SectorTypeId, (int)ApprovalProcessTypeEnum.Transfer);
            if (response.IsSuccessStatusCode)
            {
                approvalTracking = (CmsApprovalTracking)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

        }
        protected async Task PopulateConsltationArticleList(Guid ConsultationRequestId)
        {
            var response = await consultationRequestService.GetConsultationArticleByConsultationId(ConsultationRequestId);
            if (response.IsSuccessStatusCode)
            {
                consultationArticleByConsultationIdListVM = (List<ConsultationArticleByConsultationIdListVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

        #region Redirect Function
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
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
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion

        #region Approve/Reject Functions

        protected async Task ApproveTransferComs(MouseEventArgs args)
        {
            var crResult = await consultationRequestService.GetConsultationById(Guid.Parse(ConsultationRequestId));
            if (crResult.IsSuccessStatusCode)
            {
                consultationRequest = (ConsultationRequest)crResult.ResultData;
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
                        await invalidRequestHandlerService.ReturnBadRequestNotification(crResult);
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
                        approvalTracking.TransferCaseType = (int)AssignCaseToLawyerTypeEnum.ConsultationRequest;
                        await workflowService.ProcessWorkflowOptionActivites(selecetedActivityOption, approvalTracking, (int)WorkflowModuleEnum.COMSConsultationManagement, (int)WorkflowModuleTriggerEnum.TransferConsultationRequest);
                        if (TaskId != null)
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
                spinnerService.Hide();
                return;
            }

        }



        protected async Task RejectTransferComs(MouseEventArgs args)
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
                    var crResult = await consultationRequestService.GetConsultationById(Guid.Parse(ConsultationRequestId));
                    if (crResult.IsSuccessStatusCode)
                    {
                        consultationRequest = (ConsultationRequest)crResult.ResultData;
                        consultationRequest.ConsultationRequestId = Guid.Parse(ConsultationRequestId);
                        consultationRequest.Remarks = approvalTracking.Remarks;
                        consultationRequest.CreatedBy = loginState.Username;
                        consultationRequest.SectorTypeId = loginState.UserDetail.SectorTypeId;
                        approvalTracking.StatusId = (int)ApprovalStatusEnum.Rejected;
                        approvalTracking.TransferCaseType = (int)AssignCaseToLawyerTypeEnum.ConsultationRequest;
                        approvalTracking.Item = consultationRequest;
                        approvalTracking.AssignedBy = loginState.UserDetail.UserId;
                        approvalTracking.TransferStatusId = (int)ApprovalStatusEnum.Rejected;
                        approvalTracking.SectorTypeId = loginState.UserDetail.SectorTypeId;
                        await workflowService.ProcessWorkflowActvivities(approvalTracking, (int)WorkflowModuleEnum.COMSConsultationManagement, (int)WorkflowModuleTriggerEnum.TransferConsultationRequest);
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
                    //var response = await comsSharedService.RejectTransferComsSector(consultationRequest, (int)AssignCaseToLawyerTypeEnum.ConsultationRequest);
                    //if (response.IsSuccessStatusCode)
                    //{
                    //    await ButtonReject();

                    //}
                    //else
                    //{
                    //    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    //}
                    spinnerService.Hide();
                }
            }
        }

        #endregion

        #region Accept Task Button 
        private async Task ButtonAccept()
        {
            try
            {
                taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;

                ApiCallResponse response = null;

                if ((taskDetailVM.ModuleId == (int)WorkflowModuleEnum.CaseManagement) && (taskDetailVM.TypeId == (int)TaskTypeEnum.Assignment))
                {
                    response = await taskService.SaveCaseAssignment(taskDetailVM);
                }
                //if ((taskDetailVM.DocumentModuleId == (int)WorkflowModuleEnum.COMSConsultationManagement) && (taskDetailVM.TypeId == (int)TaskTypeEnum.Assignment))
                //{
                //    response = await taskService.SaveConsultationAssignment(taskDetailVM);
                //}

                else
                {
                    response = await taskService.DecisionTask(taskDetailVM);
                }

                if (response.IsSuccessStatusCode)
                {
                    //notificationService.Notify(new NotificationMessage()
                    //{
                    //    Severity = NotificationSeverity.Success,
                    //    Detail = translationState.Translate("Task_Accept_Success"),
                    //    Style = "position: fixed !important; left: 0; margin: auto;"
                    //});
                    navigationManager.NavigateTo("/usertask-list");
                }

            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }

        }
        #endregion

        #region Reject Task Button 
        protected async Task ButtonReject()
        {
            try
            {
                taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;




                var dialogResult = await dialogService.OpenAsync<RejectAndReason>(
                     translationState.Translate("Reject_Reason"),
                     new Dictionary<string, object>() { { "ReferenceId", taskDetailVM.TaskId } },
                     new DialogOptions() { CloseDialogOnOverlayClick = true });

                if (dialogResult is not null)
                {
                    ApiCallResponse response = null;

                    if ((taskDetailVM.ModuleId == (int)WorkflowModuleEnum.CaseManagement) && (taskDetailVM.TypeId == (int)TaskTypeEnum.Assignment))
                    {
                        response = await taskService.NotifyTaskAssignedBy(taskDetailVM);
                    }
                    if ((taskDetailVM.ModuleId == (int)WorkflowModuleEnum.COMSConsultationManagement) && (taskDetailVM.TypeId == (int)TaskTypeEnum.Assignment))
                    {
                        response = await taskService.ConsultationTaskRejection(taskDetailVM);
                    }


                    else
                    {
                        response = await taskService.DecisionTask(taskDetailVM);
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        //notificationService.Notify(new NotificationMessage()
                        //{
                        //    Severity = NotificationSeverity.Success,
                        //    Detail = translationState.Translate("Task_Reject_Success"),
                        //    Style = "position: fixed !important; left: 0; margin: auto;"
                        //});
                        navigationManager.NavigateTo("/usertask-list");
                    }
                }


            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
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
