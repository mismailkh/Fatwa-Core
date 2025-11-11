using FATWA_DOMAIN.Common;
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
using FATWA_WEB.Pages.Shared;
using FATWA_WEB.Pages.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.Consultation.ConsultationFiles
{
    public partial class ReviewTransferConsultationFile : ComponentBase
    {
        #region Parameter
        [Parameter]
        public dynamic FileId { get; set; }
        [Parameter]
        public dynamic SectorTypeId { get; set; }
        [Parameter]
        public dynamic TaskId { get; set; }
        #endregion

        #region Varriable
        protected ConsultationFileDetailVM consultationFile { get; set; }
        protected List<ConsultationFileHistoryVM> consultationFileHistory { get; set; }
        public IEnumerable<CommunicationListVM> communicationListVm = new List<CommunicationListVM>();
        protected RadzenDataGrid<CommunicationListVM> CommunicationGrid;
        protected RadzenDataGrid<ConsultationFileHistoryVM> HistoryGrid;
        public List<ConsultationFileAssignmentVM> consultationFileAssignee = new List<ConsultationFileAssignmentVM>();
        public List<ConsultationFileAssignmentHistoryVM> consultationFileAssignmentHistoryVM;
        protected CmsApprovalTracking approvalTracking { get; set; } = new CmsApprovalTracking();
        protected RadzenDataGrid<ConsultationFileAssignmentHistoryVM> AssigneeGrid;
        protected ConsultationFile consultationFileMain { get; set; } = new ConsultationFile();
        protected RadzenDataGrid<ComsDraftedDocumentVM> DraftConsultationRequestGrid;
        public List<ComsDraftedDocumentVM> DraftDocuments { get; set; } = new List<ComsDraftedDocumentVM>();
        protected TaskDetailVM taskDetailVM { get; set; }

        public string ActivityEn;
        public string ActivityAr;
        protected string InboxNumber;
        protected string OutboxNumber;
        protected bool RefreshFileUploadGrid { get; set; } = true;
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

            var result = await consultationFileService.GetConsultationFileDetailById(Guid.Parse(FileId));

            if (result.IsSuccessStatusCode)
            {
                consultationFile = (ConsultationFileDetailVM)result.ResultData;
                await PopulateConslutationFileCommunicationGrid();
                await PopulateConslutationFileStatusHistory();
                await PopulateConsultationAssigmentHistory();
                await PopulateFileAssignees();
                await PopulateApprovalTrackingDetails();
                await PopulateDraftGrid();
                await PopulateCurrentInstanceByApprovalTrackingId();
                await PopulateRequestTransferHistoryGrid((string)FileId);
                if (TaskId != null)
                {
                    await GetManagerTaskReminderData();
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
        }
        #endregion

        #region Populate Grids
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
        protected async Task PopulateApprovalTrackingDetails()
        {
            var response = await comsSharedService.GetApprovalTrackingProcess(Guid.Parse(FileId), (int)loginState.UserDetail.SectorTypeId, (int)ApprovalProcessTypeEnum.Transfer);
            if (response.IsSuccessStatusCode)
            {
                approvalTracking = (CmsApprovalTracking)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

        }
        public async Task PopulateDraftGrid()
        {
            var response = await cmsCaseTemplateService.GetConsultationDraftListByReferenceId(Guid.Parse(FileId));
            if (response.IsSuccessStatusCode)
            {
                DraftDocuments = (List<ComsDraftedDocumentVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

        #region Redirect
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
            navigationManager.NavigateTo("draftdocument-detail/" + args.Id + "/" + TaskId);
        }
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion

        #region Buttons
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
                    var crResult = consultationFileService.GetConsultationFile(Guid.Parse(FileId));
                    if (crResult.IsSuccessStatusCode)
                    {
                        consultationFileMain = (ConsultationFile)crResult.ResultData;
                        consultationFileMain.FileId = Guid.Parse(FileId);
                        consultationFileMain.Remarks = dialogResult;
                        consultationFileMain.CreatedBy = loginState.Username;
                        consultationFileMain.SectorTypeId = loginState.UserDetail.SectorTypeId;
                        approvalTracking.StatusId = (int)ApprovalStatusEnum.Rejected;
                        approvalTracking.TransferCaseType = (int)AssignCaseToLawyerTypeEnum.ConsultationFile;
                        approvalTracking.Item = consultationFileMain;
                        approvalTracking.AssignedBy = loginState.UserDetail.UserId;
                        approvalTracking.TransferStatusId = (int)ApprovalStatusEnum.Rejected;
                        approvalTracking.SectorTypeId = loginState.UserDetail.SectorTypeId;
                        await workflowService.ProcessWorkflowActvivities(approvalTracking, (int)WorkflowModuleEnum.COMSConsultationManagement, (int)WorkflowModuleTriggerEnum.TransferConsultationFile);
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


                    //var response = await comsSharedService.RejectTransferComsSector(consultationFileMain, (int)AssignCaseToLawyerTypeEnum.ConsultationFile);
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

        protected async Task ApproveTransferComs(MouseEventArgs args)
        {
                var crResult = await consultationFileService.GetConsultationFile(Guid.Parse(FileId));
                if (crResult.IsSuccessStatusCode)
                {
                    consultationFileMain = (ConsultationFile)crResult.ResultData;
                    
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
                        approvalTracking.TransferCaseType = (int)AssignCaseToLawyerTypeEnum.ConsultationFile;
                        await workflowService.ProcessWorkflowOptionActivites(selecetedActivityOption, approvalTracking, (int)WorkflowModuleEnum.COMSConsultationManagement, (int)WorkflowModuleTriggerEnum.TransferConsultationFile);
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
                        await invalidRequestHandlerService.ReturnBadRequestNotification(crResult);
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

        protected async Task ViewHistoryDetail(ConsultationFileHistoryVM args)
        {
            navigationManager.NavigateTo("/casefile-historydetail/" + args.HistoryId);
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
            navigationManager.NavigateTo("/communication-detail/" + FileId);
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

                //if (response.IsSuccessStatusCode)
                //{
                //    notificationService.Notify(new NotificationMessage()
                //    {
                //        Severity = NotificationSeverity.Success,
                //        Detail = translationState.Translate("Task_Accept_Success"),
                //        Style = "position: fixed !important; left: 0; margin: auto;"
                //    });
                //    navigationManager.NavigateTo("/usertask-list");
                //}

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

                    //if (response.IsSuccessStatusCode)
                    //{
                    //    notificationService.Notify(new NotificationMessage()
                    //    {
                    //        Severity = NotificationSeverity.Success,
                    //        Detail = translationState.Translate("Task_Reject_Success"),
                    //        Style = "position: fixed !important; left: 0; margin: auto;"
                    //    });
                    //    navigationManager.NavigateTo("/usertask-list");
                    //}
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
