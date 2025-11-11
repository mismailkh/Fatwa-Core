using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_GENERAL.Helper.Response;
namespace FATWA_ADMIN.Pages.LookupsManagment
{
    public partial class CaseFileStatusAdd : ComponentBase
    {
        #region functions
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
        CaseFileStatus _casefilestatuslist;
        protected CaseFileStatus casefilestatuslist
        {
            get
            {
                return _casefilestatuslist;
            }
            set
            {
                if (!object.Equals(_casefilestatuslist, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "casefilestatuslist", NewValue = value, OldValue = _casefilestatuslist };
                    _casefilestatuslist = value;
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
                casefilestatuslist = new CaseFileStatus() { };
                spinnerService.Hide();
            }
            else
            {
                spinnerService.Show();
                response = await lookupService.GetCaseFileStatusById(Id);
                if (response.IsSuccessStatusCode)
                {
                    casefilestatuslist = (CaseFileStatus)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }

                spinnerService.Hide();
            }

        }
        #endregion

        #region Form submit
        protected async Task SaveChanges(CaseFileStatus args)
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
                        var fatwaDbUpdateLmsLiteratureTypeResult = await lookupService.UpdateCaseFileStatus(casefilestatuslist);
                        if (fatwaDbUpdateLmsLiteratureTypeResult.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Case_File_Status_Upadated_Successfully"),
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
                    Detail = Id == null ? translationState.Translate("Could_not_create_a_new_Case_File_Status") : translationState.Translate("Case_File_Status_could_not_be_updated"),
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
