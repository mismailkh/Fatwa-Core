using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Newtonsoft.Json;
using Radzen;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_GENERAL.Helper.Enum;

namespace FATWA_WEB.Pages
{
    //<History Author = 'Hassan Abbas' Date='2023-02-21' Version="1.0" Branch="master"> Dashboard Page for Inbox Outbox and Modules</History>
    public partial class Dashboard : ComponentBase
    {
        #region Variables
        protected string RedirectURL { get; set; }
        protected IEnumerable<CommunicationInboxOutboxVM> inboxOutboxList = new List<CommunicationInboxOutboxVM>();
        protected object grid;
        public ViewConsultationVM consultationRequestVM = new ViewConsultationVM();
        #endregion

        #region Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateInboxOutboxList((int)CommunicationCorrespondenceTypeEnum.Inbox);
            translationState.TranslateGridFilterLabels(grid);
            spinnerService.Hide();
        }

        #endregion

        public async Task PopulateInboxOutboxList(int correspondenceType)
        {
            try
            {
                var PageSize = 5;
                var PageNumber = 1;
                var CommunicationResponse = await communicationService.GetInboxOutboxList(correspondenceType, loginState.Username, PageSize, PageNumber);
                if (CommunicationResponse.IsSuccessStatusCode)
                {
                    inboxOutboxList = JsonConvert.DeserializeObject<List<CommunicationInboxOutboxVM>>(CommunicationResponse.ResultData.ToString());
                    //inboxOutboxList = (List<CommunicationInboxOutboxVM>)CommunicationResponse.ResultData;
                    StateHasChanged();
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(CommunicationResponse);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region Tab Change

        protected async Task OnChange(int index)
        {
            if (index == 0)
            {
                await PopulateInboxOutboxList((int)CommunicationCorrespondenceTypeEnum.Inbox);
                translationState.TranslateGridFilterLabels(grid);
            }
            else
            {
                await PopulateInboxOutboxList((int)CommunicationCorrespondenceTypeEnum.Outbox);
                translationState.TranslateGridFilterLabels(grid);
            }
        }
        #endregion

        #region Action Events

        protected async void ViewResponse(CommunicationInboxOutboxVM item)
        {
            loginState.ModuleId = (int)ModuleEnum.Communication;
            if (item.CommunicationTypeId == (int)CommunicationTypeEnum.CaseRequest)
            {
                navigationManager.NavigateTo("/caserequest-view/" + item.ReferenceId);
            }
            else if (item.CommunicationTypeId == (int)CommunicationTypeEnum.ContractRequest
                 || item.CommunicationTypeId == (int)CommunicationTypeEnum.LegislationRequest
                 || item.CommunicationTypeId == (int)CommunicationTypeEnum.LegalAdviceRequest
                 || item.CommunicationTypeId == (int)CommunicationTypeEnum.AdministrativeComplaintRequest
                 || item.CommunicationTypeId == (int)CommunicationTypeEnum.InternationalArbitrationRequest)
            {
                var result = await consultationRequestService.GetConsultationDetailById((Guid)item.ReferenceId);
                if (result.IsSuccessStatusCode)
                {
                    consultationRequestVM = (ViewConsultationVM)result.ResultData;
                    //navigationManager.NavigateTo("/consultationrequest-detail/" + item.ReferenceId + "/" + consultationRequestVM.RequestTypeId); // ORIGINAL
                    navigationManager.NavigateTo("/consultationrequest-detail/" + item.ReferenceId + "/" + consultationRequestVM.SectorTypeId);
                }
            }

            else if (item.CommunicationTypeId == (int)CommunicationTypeEnum.RequestForMeeting)
            {
                navigationManager.NavigateTo("/meeting-view/" + item.CommunicationId + "/" + item.ReferenceId + "/" + item.CommunicationTypeId + "/" + true + "/" + item.LinkTargetTypeId);
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
                RedirectURL = "/request-need-more-detail/" + item.ReferenceId + "/" + item.CommunicationId + "/" + item.LinkTargetTypeId + "/" + item.CommunicationTypeId;
                navigationManager.NavigateTo(RedirectURL);
            }
        }

        protected async Task AddMeeting(CommunicationInboxOutboxVM item)
        {
            try
            {
                Guid MeetingId = Guid.Empty;
                Guid CommunicationId = item.CommunicationId;
                Guid ReferenceId = (Guid)item.ReferenceId;
                int SubModuleId = item.LinkTargetTypeId;
                int SectorTypeId = 0;
                navigationManager.NavigateTo("/meeting-add/" + MeetingId + "/" + CommunicationId + "/" + ReferenceId + "/" + SubModuleId + "/" + SectorTypeId);


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

        #region Change Events
        protected async void OnModuleClick(int moduleId)
        {
            loginState.ModuleId = moduleId;
            await BrowserStorage.SetItemAsync("ModuleId", moduleId);
        }
        #endregion

        #region Logout
        //<History Author = 'Hassan Abbas' Date='2023-03-24' Version="2.0" Branch="master"> Logout</History>
        protected async Task HandleLogout(MouseEventArgs args)
        {
            await userService.RecordUserLogoutActivity(loginState.Username, loginState.UserDetail.UserId);
            loginState.SetLogout(false);
            DeleteBrowserStorageValues();
            navigationManager.NavigateTo("Login", true);
        }
        protected async void DeleteBrowserStorageValues()
        {
            try
            {
                await BrowserStorage.RemoveItemAsync("User");
                await BrowserStorage.RemoveItemAsync("Token");
                await BrowserStorage.RemoveItemAsync("RefreshToken");
                await BrowserStorage.RemoveItemAsync("ProfilePicUrl");
                await BrowserStorage.RemoveItemAsync("SecurityStamp");
                await BrowserStorage.RemoveItemAsync("SessionTimeout");
            }
            catch (Exception)
            {
            }
        }
        #endregion
    }
}
