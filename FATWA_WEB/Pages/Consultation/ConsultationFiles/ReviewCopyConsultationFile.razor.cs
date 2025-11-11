using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;

namespace FATWA_WEB.Pages.Consultation.ConsultationFiles
{
    public partial class ReviewCopyConsultationFile : ComponentBase
    {
        #region Parameter
        [Parameter]
        public dynamic FileId { get; set; }
        [Parameter]
        public dynamic SectorTypeId { get; set; }
        #endregion
        #region Varriable
        protected ConsultationFileDetailVM consultationFile { get; set; }
        protected List<ConsultationFileHistoryVM> consultationFileHistory { get; set; }
        public IEnumerable<CommunicationListVM> communicationListVm = new List<CommunicationListVM>();
        protected RadzenDataGrid<CommunicationListVM> CommunicationGrid;
        protected RadzenDataGrid<ConsultationFileHistoryVM> HistoryGrid;
        public List<ConsultationFileAssignmentVM> consultationFileAssignee = new List<ConsultationFileAssignmentVM>();
        public List<ConsultationFileAssignmentHistoryVM> consultationFileAssignmentHistoryVM;
        protected ComsApprovalTracking approvalTracking { get; set; } = new ComsApprovalTracking();
        protected RadzenDataGrid<ConsultationFileAssignmentHistoryVM> AssigneeGrid;
        protected ConsultationFile consultationFileMain { get; set; } = new ConsultationFile();
        public string ActivityEn;
        public string ActivityAr;
        protected string InboxNumber;
        public int SectorTypeIdCheck = 0; 
        protected string OutboxNumber;
        protected bool RefreshFileUploadGrid { get; set; } = true;
        #endregion
        #region On Load Component
        protected override async Task OnInitializedAsync()
        {
            SectorTypeIdCheck = Convert.ToInt32(SectorTypeId);
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
                spinnerService.Hide();

            }
        }
        #endregion
        #region Populate Grids
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
            var response = await comsSharedService.GetApprovalTrackingProcess(Guid.Parse(FileId), (int)loginState.UserDetail.SectorTypeId, (int)ApprovalProcessTypeEnum.SendaCopy);
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
        #region Redirect
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

        protected async Task ApproveSendACopy(MouseEventArgs args)
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
                var crResult = await consultationFileService.GetConsultationFile(Guid.Parse(FileId));
                if (crResult.IsSuccessStatusCode)
                {
                    consultationFileMain = (ConsultationFile)crResult.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(crResult);
                    spinnerService.Hide();
                    return;
                }
                consultationFileMain.Remarks = approvalTracking.Remarks;
                consultationFileMain.CreatedBy = loginState.Username;
                consultationFileMain.SectorTypeId = loginState.UserDetail.SectorTypeId;

                var response = await comsSharedService.ApproveSendACopyConsultation(consultationFileMain, (int)AssignCaseToLawyerTypeEnum.ConsultationFile);
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

        protected async Task RejectSendACopy(MouseEventArgs args)
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
                var crResult = await consultationFileService.GetConsultationFileDetailById(Guid.Parse(FileId));
                if (crResult.IsSuccessStatusCode)
                {
                    consultationFileMain = (ConsultationFile)crResult.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(crResult);
                    spinnerService.Hide();
                    return;
                }
                consultationFileMain.Remarks = approvalTracking.Remarks;
                consultationFileMain.CreatedBy = loginState.Username;
                consultationFileMain.SectorTypeId = loginState.UserDetail.SectorTypeId;

                var response = await comsSharedService.RejectSendACopyConsultation(consultationFileMain, (int)AssignCaseToLawyerTypeEnum.ConsultationFile);
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
        protected async Task ViewResponse(CommunicationListVM item)
        {
            navigationManager.NavigateTo("/communication-detail/" + FileId);
        }




    }
}
