using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace FATWA_WEB.Pages.CaseManagment.Files
{
    //<History Author = 'Hassan Abbas' Date='2022-12-15' Version="1.0" Branch="master">Select Primary File for Linking selected Files </History>
    public partial class SelectPrimaryCANForLinkingPopup : ComponentBase
    {

        #region Parameter

        [Parameter]
        public dynamic? Files { get; set; }
        public List<RegisteredCaseFileVM> LinkedFiles { get { return (List<RegisteredCaseFileVM>)Files; } set { Files = value; } }

        #endregion

        #region Variables
        public List<CmsRegisteredCaseVM> registeredCases = new List<CmsRegisteredCaseVM>();
        protected LinkCANsVM linkCANs = new LinkCANsVM();
        public bool allowRowSelectOnRowClick = true;
        protected string LinkedCANs { get; set; }
        #endregion

        #region Component Load

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await ConcatCANs();
            await PopulateLinkedCANs();
            spinnerService.Hide();
        }

        #endregion

        #region Remote Dropdown Data and Dropdown Change Events

        //<History Author = 'Hassan Abbas' Date='2022-11-30' Version="1.0" Branch="master"> Concatinate Cases in selected Files</History>
        protected async Task ConcatCANs()
        {
            try
            {
                foreach (var file in LinkedFiles)
                {
                    var response = await cmsCaseFileService.GetAllRegisteredCasesByFileId(file.FileId);
                    if (response.IsSuccessStatusCode)
                    {
                        registeredCases = new List<CmsRegisteredCaseVM>(registeredCases?.Concat((List<CmsRegisteredCaseVM>)response.ResultData).ToList());
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
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


        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Populate Linked Request Nos</History>
        protected async Task PopulateLinkedCANs()
        {
            try
            {
                linkCANs.LinkedCANs = registeredCases?.Select(o => o.CANNumber).Distinct().ToList();
                var length = linkCANs.LinkedCANs?.Count();
                for (int i = 0; i < length; i++)
                {
                    var seperator = i + 1 == length ? "" : ", ";
                    LinkedCANs += linkCANs.LinkedCANs[i] + seperator;
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
        //<History Author = 'Hassan Abbas' Date='2022-11-30' Version="1.0" Branch="master"> Link Case Files</History>
        protected async Task LinkCANs()
        {
            try
            {
                if(linkCANs.LinkedCANs?.Count() < 2)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Must_Select_Two_CANs"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
                }
                if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    var response = await cmsRegisteredCaseService.LinkCANs(linkCANs);
                    if (response.IsSuccessStatusCode)
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
            catch (Exception ex)
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
