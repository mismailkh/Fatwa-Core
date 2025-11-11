using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_WEB.Services;
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

namespace FATWA_WEB.Pages.Consultation.Request
{
    public partial class ReviewCopyConsultationRequest : ComponentBase
    {
        [Parameter]
        public dynamic ConsultationRequestId { get; set; }
        [Parameter]

        public dynamic SectorTypeId { get; set; }


        #region variable declaration

        public int SectorTypeIdCheck = 0;
        protected bool ShowDocumentViewer { get; set; }
        protected TelerikPdfViewer PdfViewerRef { get; set; }
        protected byte[] FileData { get; set; }
        protected RadzenDataGrid<ConsultationPartyListVM>? consultationPartyGrid = new RadzenDataGrid<ConsultationPartyListVM>();
        public IEnumerable<ConsultationPartyListVM> consultationPartyVM { get; set; } = new List<ConsultationPartyListVM>();
        protected ComsApprovalTracking approvalTracking { get; set; } = new ComsApprovalTracking();

        public ObservableCollection<TempAttachementVM> OfficialAttachments { get; set; } = new ObservableCollection<TempAttachementVM>();
        protected List<AttachmentType> AttachmentTypes { get; set; } = new List<AttachmentType>();
        protected string DocumentPath { get; set; }
        protected string ActivityEn;
        protected string ActivityAr;
        protected string RedirectURL { get; set; }
        public ViewConsultationVM consultationRequestVM = new ViewConsultationVM();
        protected RadzenDataGrid<CommunicationListVM> CommunicationGrid;
        protected IEnumerable<CommunicationListVM> communicationListVm = new List<CommunicationListVM>();
        protected RadzenDataGrid<ComsConsultationRequestHistoryVM> HistoryGrid;
        protected IEnumerable<ComsConsultationRequestHistoryVM> cmsConsultationRequestHistory { get; set; } = new List<ComsConsultationRequestHistoryVM>();
        protected ConsultationRequest consultationRequest { get; set; } = new ConsultationRequest();
        protected RadzenDataGrid<ComsDraftedDocumentVM> DraftConsultationRequestGrid;

        #endregion

        #region Draft Request Grid
        IEnumerable<ComsDraftedDocumentVM> _getConsultationRequestDetail;
        protected IEnumerable<ComsDraftedDocumentVM> getConsultationRequestDetail
        {
            get
            {
                return _getConsultationRequestDetail;
            }
            set
            {
                if (!Equals(_getConsultationRequestDetail, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getConsultationRequestDetail", NewValue = value, OldValue = _getConsultationRequestDetail };
                    _getConsultationRequestDetail = value;

                    // Reload();
                }
            }
        }

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
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }

            spinnerService.Hide();
        }

        #endregion

        #region Populate functions
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
            var response = await comsSharedService.GetApprovalTrackingProcess(Guid.Parse(ConsultationRequestId), (int)loginState.UserDetail.SectorTypeId, (int)ApprovalProcessTypeEnum.SendaCopy);
            if (response.IsSuccessStatusCode)
            {
                approvalTracking = (ComsApprovalTracking)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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
        #endregion

        #region Approve/Reject Functions

        protected async Task ApproveSendACopyComs(MouseEventArgs args)
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
                var crResult = await consultationRequestService.GetConsultationById(Guid.Parse(ConsultationRequestId));
                if (crResult.IsSuccessStatusCode)
                {
                    consultationRequest = (ConsultationRequest)crResult.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(crResult);
                    spinnerService.Hide();
                    return;
                }
                consultationRequest.ConsultationRequestId = Guid.Parse(ConsultationRequestId);
                consultationRequest.Remarks = approvalTracking.Remarks;
                consultationRequest.CreatedBy = loginState.Username;
                consultationRequest.SectorTypeId = loginState.UserDetail.SectorTypeId;

                var response = await comsSharedService.ApproveSendACopyConsultation(consultationRequest, (int)AssignCaseToLawyerTypeEnum.ConsultationRequest);
                if (response.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Changes_saved_successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    approvalTracking.StatusId = (int)ApprovalStatusEnum.Approved;
                    if (navigationState.ReturnUrl != null)
                    {
                        navigationManager.NavigateTo(navigationState.ReturnUrl);
                    }
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                spinnerService.Hide();
            }
        }

        protected async Task RejectSendACopyComs(MouseEventArgs args)
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
                var crResult = await consultationRequestService.GetConsultationById(Guid.Parse(ConsultationRequestId));
                if (crResult.IsSuccessStatusCode)
                {
                    consultationRequest = (ConsultationRequest)crResult.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(crResult);
                    spinnerService.Hide();
                    return;
                }
                consultationRequest.ConsultationRequestId = Guid.Parse(ConsultationRequestId);
                consultationRequest.Remarks = approvalTracking.Remarks;
                consultationRequest.CreatedBy = loginState.Username;
                consultationRequest.SectorTypeId = loginState.UserDetail.SectorTypeId;

                var response = await comsSharedService.RejectSendACopyConsultation(consultationRequest, (int)AssignCaseToLawyerTypeEnum.ConsultationRequest);
                if (response.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Changes_saved_successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    approvalTracking.StatusId = (int)ApprovalStatusEnum.Rejected;
                    if (navigationState.ReturnUrl != null)
                    {
                        navigationManager.NavigateTo(navigationState.ReturnUrl);
                    }
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                spinnerService.Hide();
            }
        }

        #endregion

    }
}
