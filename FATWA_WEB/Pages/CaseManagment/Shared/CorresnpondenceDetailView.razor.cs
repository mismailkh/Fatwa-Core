using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using static FATWA_DOMAIN.Enums.GeneralEnums;

namespace FATWA_WEB.Pages.CaseManagment.Shared
{
    public partial class CorresnpondenceDetailView : ComponentBase
    {
        #region Parameter

        [Parameter]
        public dynamic ReferenceId { get; set; }
        [Parameter]
        public dynamic CommunicationId { get; set; }

        [Parameter]
        public dynamic SubModuleId { get; set; }

        [Parameter]
        public dynamic ByActivity { get; set; }
        #endregion

        #region Variables
        public int SubModule { get { return Convert.ToInt32(SubModuleId); } set { SubModuleId = value; } }
        public int Activity { get { return Convert.ToInt32(ByActivity); } set { ByActivity = value; } }
        protected bool isRecordFound = true;
        public string TransKeyHeader = string.Empty;
        public string TransKeyUrl = string.Empty;
        protected CommunicationDetailVM communicationDetail;
        #endregion

        #region Send Response Detail View Grid Load Properties Load

        CommunicationSendResponseVM _communicationResponseVM;
        protected CommunicationSendResponseVM communicationResponseVM
        {
            get
            {
                return _communicationResponseVM;
            }
            set
            {
                if (!object.Equals(_communicationResponseVM, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "CommunicationSendResponseVM", NewValue = value, OldValue = _communicationResponseVM };
                    _communicationResponseVM = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        #endregion

        #region Model full property Instance

        public string CaseRequestUrlS { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {

        }
        #endregion

        #region Component Load

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await CommunicationDetailbyComIdAndComType();
            if (SubModule == (int)SubModuleEnum.CaseFile)
            {
                CaseRequestUrlS = @"/case-files";
                TransKeyUrl = "Case_Files";
            }
            else if (SubModule == (int)SubModuleEnum.CaseRequest)
            {
                CaseRequestUrlS = @"/case-requests";
                TransKeyUrl = "Case_Requests";
            }
            else if (SubModule == (int)SubModuleEnum.RegisteredCase)
            {
                CaseRequestUrlS = @"/case-files";
                TransKeyUrl = "Case_Files";
            }
            else if (SubModule == (int)SubModuleEnum.ConsultationFile)
            {
                CaseRequestUrlS = @"/consultationfile-list";
                TransKeyUrl = "Consultation_File";
            }
            else if (SubModule == (int)SubModuleEnum.ConsultationRequest)
            {
                CaseRequestUrlS = @"/consultationrequest-list";
                TransKeyUrl = "Consultation_Request";
            }
            TransKeyHeader = "Correspondence_Details";
            spinnerService.Hide();
        }
        protected async Task CommunicationDetailbyComIdAndComType()
        {
            try
            {
                var response = await communicationService.CommunicationDetailbyComIdAndComType(Guid.Parse(ReferenceId), Guid.Parse(CommunicationId), SubModule, Activity);
                if (response.IsSuccessStatusCode)
                {
                    communicationDetail = (CommunicationDetailVM)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        #region Redirect Functions

        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }

        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }

        protected async Task BtnBack(MouseEventArgs args)
        {
            await JSRuntime.InvokeVoidAsync("history.back");
        }
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion
      
    }
}

