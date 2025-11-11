using FATWA_DOMAIN.Models.ViewModel.AdminVM.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace FATWA_WEB.Pages.CaseManagment.Request
{
    //<History Author = 'Hassan Abbas' Date='2022-12-15' Version="1.0" Branch="master">Select Primary Request for Linking selected Requests </History>
    public partial class SelectPrimaryRequestForLinkingPopup : ComponentBase
    {

        #region Parameter

        [Parameter]
        public dynamic? Requests { get; set; }

        [Parameter]
        public dynamic? Files { get; set; }
        public List<CmsCaseRequestVM> LinkedRequests { get { return (List<CmsCaseRequestVM>)Requests; } set { Requests = value; } }
        public List<CmsCaseFileVM> LinkedFiles { get { return (List<CmsCaseFileVM>)Files; } set { Files = value; } }

        #endregion

        #region Variables
        protected LinkCaseRequestsVM linkCaseRequest = new LinkCaseRequestsVM();
        public bool allowRowSelectOnRowClick = true;
        protected string LinkedRequestNos { get; set; }
        #endregion

        #region Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateLinkedRequestNos();
            spinnerService.Hide();
        }

        #endregion

        #region Remote Dropdown Data and Dropdown Change Events
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Populate Linked Request Nos</History>
        protected async Task PopulateLinkedRequestNos()
        {
            try
            {
                var length = LinkedRequests?.Count();
                for (int i = 0; i < length; i++)
                {
                    var seperator = i + 1 == length ? "" : ", ";
                    LinkedRequestNos += LinkedRequests[i].RequestNumber + seperator;
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

        #region Redirect Function
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion

        #region Button Events
        //<History Author = 'Hassan Abbas' Date='2022-11-30' Version="1.0" Branch="master"> Link Case Requests</History>
        protected async Task LinkCaseRequests()
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    linkCaseRequest.LinkedRequests = LinkedRequests.Where(f => f.RequestId != linkCaseRequest.PrimaryRequestId).ToList();
                    spinnerService.Show();
                    var response = await caseRequestService.LinkCaseRequests(linkCaseRequest);
                    if(response.IsSuccessStatusCode)
                    {
                        dialogService.Close(1);
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    spinnerService.Hide();
                }
            }
            catch(Exception ex)
            {
                spinnerService.Hide();
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
