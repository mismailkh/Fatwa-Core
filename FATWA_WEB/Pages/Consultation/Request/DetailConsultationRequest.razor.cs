using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_WEB.Pages.Consultation.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System.Collections.ObjectModel;
using Telerik.Blazor.Components;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;

namespace FATWA_WEB.Pages.Consultation.Request
{
    public partial class DetailConsultationRequest : ComponentBase
    {
        #region paramater
        [Parameter]
        public dynamic ConsultationRequestId { get; set; }
        [Parameter]
        public dynamic SectorTypeId { get; set; }
        public dynamic AssignConsultationLawyerType { get; set; }
        [Parameter]
        public string? TaskId { get; set; }
        #endregion

        #region variable declaration

        public int SectorTypeIdCheck = 0;
        protected bool ShowDocumentViewer { get; set; }
        protected TelerikPdfViewer PdfViewerRef { get; set; }
        protected byte[] FileData { get; set; }
        protected RadzenDataGrid<ConsultationPartyListVM>? consultationPartyGrid = new RadzenDataGrid<ConsultationPartyListVM>();
        public IEnumerable<ConsultationPartyListVM> consultationPartyVM { get; set; } = new List<ConsultationPartyListVM>();

        public ObservableCollection<TempAttachementVM> OfficialAttachments { get; set; } = new ObservableCollection<TempAttachementVM>();
        protected List<AttachmentType> AttachmentTypes { get; set; } = new List<AttachmentType>();
        protected string DocumentPath { get; set; }
        protected string ActivityEn;
        protected string ActivityAr;
        public string TransKeyHeader = string.Empty;

        public ViewConsultationVM consultationRequestVM = new ViewConsultationVM();
        protected RadzenDataGrid<CommunicationListVM> CommunicationGrid;
        protected IEnumerable<CommunicationListVM> communicationListVm = new List<CommunicationListVM>();
        protected RadzenDataGrid<ComsConsultationRequestHistoryVM> HistoryGrid;
        protected IEnumerable<ComsConsultationRequestHistoryVM> cmsConsultationRequestHistory { get; set; } = new List<ComsConsultationRequestHistoryVM>();
        public IEnumerable<ConsultationArticleByConsultationIdListVM> consultationArticleByConsultationIdListVM { get; set; } = new List<ConsultationArticleByConsultationIdListVM>();
        protected RadzenDataGrid<ConsultationArticleByConsultationIdListVM>? consultationArticleGrid = new RadzenDataGrid<ConsultationArticleByConsultationIdListVM>();
        public bool IsStatusConsultationRequest { get; set; }
        protected string RedirectURL { get; set; }
        protected RadzenDataGrid<CmsTransferHistoryVM> transferHistoryGrid;
        protected IEnumerable<CmsTransferHistoryVM> cmsCaseRequestTransferHistory = new List<CmsTransferHistoryVM>();
        protected CmsTransferHistoryVM caseRequestTransferHistoryobj = new CmsTransferHistoryVM();
        protected TaskDetailVM taskDetailVM { get; set; } = new TaskDetailVM();
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
            PopulateTranslationKey();
            var result = await consultationRequestService.GetConsultationDetailById(Guid.Parse(ConsultationRequestId));

            if (result.IsSuccessStatusCode)
            {
                consultationRequestVM = (ViewConsultationVM)result.ResultData;
                await PopulateCommunicationList(ConsultationRequestId);
                await PopulateRequestHistoryGrid(ConsultationRequestId);
                await PopulateConsltationPartyList(Guid.Parse(ConsultationRequestId));
                await PopulateConsltationArticleList(consultationRequestVM.ConsultationRequestId);
                await PopulateRequestTransferHistoryGrid((string)ConsultationRequestId);
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
        private void PopulateTranslationKey()
        {
            switch (SectorTypeIdCheck)
            {
                case (int)OperatingSectorTypeEnum.Contracts:
                    TransKeyHeader = "Contracts_Request";
                    break;
                case (int)OperatingSectorTypeEnum.LegalAdvice:
                    TransKeyHeader = "Legal_Advice_Request";
                    break;
                case (int)OperatingSectorTypeEnum.InternationalArbitration:
                    TransKeyHeader = "International_Arbitration_Request";
                    break;
                case (int)OperatingSectorTypeEnum.AdministrativeComplaints:
                    TransKeyHeader = "Administrative_Complaints_Request";
                    break;
                case (int)OperatingSectorTypeEnum.Legislations:
                    TransKeyHeader = "Legislations_Request";
                    break;
                case (int)OperatingSectorTypeEnum.PrivateOperationalSector:
                case (int)OperatingSectorTypeEnum.PublicOperationalSector:
                    switch (consultationRequestVM.RequestTypeId)
                    {
                        case (int)RequestTypeEnum.Contracts:
                            TransKeyHeader = "Contracts_Request";
                            break;
                        case (int)RequestTypeEnum.LegalAdvice:
                            TransKeyHeader = "Legal_Advice_Request";
                            break;
                        case (int)RequestTypeEnum.InternationalArbitration:
                            TransKeyHeader = "International_Arbitration_Request";
                            break;
                        case (int)RequestTypeEnum.AdministrativeComplaints:
                            TransKeyHeader = "Administrative_Complaints_Request";
                            break;
                        case (int)RequestTypeEnum.Legislations:
                            TransKeyHeader = "Legislations_Request";
                            break;
                    }
                    break;
            }
        }

        #region Populate lists 
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
                RedirectURL = "/detail-withdraw-consultationrequest/" + item.ReferenceId + "/" + (int)CommunicationTypeEnum.WithdrawRequested;
                navigationManager.NavigateTo(RedirectURL);
            }
            else
            {
                RedirectURL = "/request-need-more-detail/" + item.ReferenceId + "/" + item.CommunicationId + "/" + item.SubModuleId + "/" + item.CommunicationTypeId;
                navigationManager.NavigateTo(RedirectURL);
            }
        }
        #endregion

        #region  Buttons


        protected async Task Transfer(MouseEventArgs args)
        {
            var dialogResult = await dialogService.OpenAsync<TransferConsultationSector>(
                translationState.Translate("Transfer_Consultation"),
                new Dictionary<string, object>() {
                    { "ReferenceId", Guid.Parse(ConsultationRequestId) },
                    { "TransferConsultationType", (int)AssignCaseToLawyerTypeEnum.ConsultationRequest },
                    { "IsConfidential", consultationRequestVM.IsConfidential },
                    { "IsAssignment", false },
                    {"SectorTypeId",SectorTypeId},
                    { "RequestTypeId",consultationRequestVM.RequestTypeId }
                },
                new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
            if (dialogResult != null)
            {
                if (TaskId != null)
                    await UpdateTaskDetail();
                navigationManager.NavigateTo("/consultationrequest-list/");
            }
        }

        protected async Task MeetingRequest(MouseEventArgs args)
        {
            try
            {
                string ReferenceId = ConsultationRequestId;
                int SubModuleId = (int)SubModuleEnum.ConsultationRequest;
                //int? SectorTypeId = consultationRequestVM.SectorTypeId;
                string ReceivedBy = consultationRequestVM.CreatedBy;
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
                        { "ConsultationRequestId", Guid.Parse(ConsultationRequestId) },
                        { "AssignConsultationLawyerType", (int)AssignCaseToLawyerTypeEnum.ConsultationRequest },
                        { "SectorTypeId", SectorTypeIdCheck },
                        { "TaskId", TaskId }

               }
               ,
                new DialogOptions() { Width = "45% !important", CloseDialogOnOverlayClick = true });
            if (result != null)
            {
                if (TaskId != null)
                    await UpdateTaskDetail();
            }
        }
        protected void RequestMoreInfo(MouseEventArgs args)
        {
            navigationManager.NavigateTo("/Coms-Request-For-More-Information/" + ConsultationRequestId + "/" + true);
        }


        protected async Task SendACopy(MouseEventArgs args)
        {
            var dialogResult = await dialogService.OpenAsync<SendACopyConsultationRequest>(
                translationState.Translate("SendAcopy"),
                new Dictionary<string, object>() {
                    { "ReferenceId", Guid.Parse(ConsultationRequestId) } ,
                    { "SendACopyType", (int)AssignCaseToLawyerTypeEnum.ConsultationRequest } ,
                    { "SectorTypeId", SectorTypeId } ,
                },
                new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
            if (dialogResult != null)
            {
                if (TaskId != null)
                    await UpdateTaskDetail();
            }
        }

        protected async Task AddMeeting(CommunicationListVM item)
        {
            try
            {
                Guid CommunicationId = item.CommunicationId;
                string ReferenceId = ConsultationRequestId;
                int SubModuleId = (int)SubModuleEnum.ConsultationRequest;
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

        #region view Attachment
        protected async Task ViewAttachement(TempAttachementVM theUpdatedItem)
        {
            try
            {
                var physicalPath = Path.Combine(Directory.GetCurrentDirectory() + theUpdatedItem.StoragePath).Replace(@"\\", @"\");

                if (!String.IsNullOrEmpty(physicalPath))
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
                        Detail = translationState.Translate("File_Not_Found"),
                        Summary = translationState.Translate("Error"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Summary = translationState.Translate("Error"),
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
