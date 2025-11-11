using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_ADMIN.Pages.LookupsManagment
{

    public partial class CaseStatusAdd : ComponentBase
    {

        #region Grid Function

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        #endregion

        #region Paramter
        [Parameter]
        public dynamic Id { get; set; }
        #endregion

        #region Variables
        CmsRegisteredCaseStatus _CmsRegisteredCaseStatuslist;
        protected CmsRegisteredCaseStatus CmsRegisteredCaseStatuslist
        {
            get
            {
                return _CmsRegisteredCaseStatuslist;
            }
            set
            {
                if (!object.Equals(_CmsRegisteredCaseStatuslist, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "CmsRegisteredCaseStatuslist", NewValue = value, OldValue = _CmsRegisteredCaseStatuslist };
                    _CmsRegisteredCaseStatuslist = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        ApiCallResponse response = new ApiCallResponse();
        #endregion

        #region On Load

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }
        protected async Task Load()
        {
            if (Id == null)
            {
                spinnerService.Show();
                CmsRegisteredCaseStatuslist = new CmsRegisteredCaseStatus() { };
                spinnerService.Hide();
            }
            else
            {
                spinnerService.Show();
                response = await lookupService.GetCaseStatusById(Id);
                if (response.IsSuccessStatusCode)
                {
                    CmsRegisteredCaseStatuslist = (CmsRegisteredCaseStatus)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                spinnerService.Hide();
            }

        }
        #endregion

        #region Form Submit
        protected async Task SaveChanges(CmsRegisteredCaseStatus args)
        {
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
                                    translationState.Translate("Sure_Submit"),
                                    translationState.Translate("Confirm"),
                                    new ConfirmOptions()
                                    {
                                        OkButtonText = translationState.Translate("OK"),
                                        CancelButtonText = translationState.Translate("Cancel")
                                    });
                if (dialogResponse == true)
                {
                    spinnerService.Show();
                    if (Id != null)
                    {
                        var fatwaDbUpdateLmsLiteratureTypeResult = await lookupService.UpdateCaseStatus(CmsRegisteredCaseStatuslist);
                        if (fatwaDbUpdateLmsLiteratureTypeResult.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Case_Status_Upadated_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }
                    }

                    dialogService.Close(true);
                    StateHasChanged();
                    spinnerService.Hide();
                }
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = Id == null ? translationState.Translate("Could_not_create_a_new_Case_Status") : translationState.Translate("Case_Status_could_not_be_updated"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                spinnerService.Hide();
            }
        }
        protected async Task Button2Click(MouseEventArgs args)
        {
            dialogService.Close(false);
        }
        #endregion
    }
}
