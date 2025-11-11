using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_ADMIN.Pages.LookupsManagment
{
    public partial class ChambersNumberAdd : ComponentBase
    {
        #region Paramter
        [Parameter]
        public dynamic Id { get; set; }
        #endregion

        #region Variable
        public List<Chamber> ChamberNames { get; set; } = new List<Chamber>();
        public List<ChamberShift> Shifts { get; set; } = new List<ChamberShift>();
        ApiCallResponse response = new ApiCallResponse();
        ChamberNumber _chamberNumber;
        protected ChamberNumber chamberNumber
        {
            get
            {
                return _chamberNumber;
            }
            set
            {
                if (!object.Equals(_chamberNumber, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "chamberNumber", NewValue = value, OldValue = _chamberNumber };
                    _chamberNumber = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        #endregion

        #region On Load
        protected override async Task OnInitializedAsync()
        {
            await Load();
            await GetChamber();
            await GetShift();
        }
        protected async Task Load()
        {
            if (Id == null)
            {
                spinnerService.Show();
                chamberNumber = new ChamberNumber() { };
                chamberNumber.Id = new int();
                spinnerService.Hide();
            }
            else
            {
                spinnerService.Show();

                response = await lookupService.GetChamberNumberById(Id);
                if (response.IsSuccessStatusCode)
                {
                    chamberNumber = (ChamberNumber)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                spinnerService.Hide();
            }

        }
        #endregion

        #region populate Chambers Name and Shifts
        protected async Task GetChamber()
        {

            var response = await lookupService.GetChamber();
            if (response.IsSuccessStatusCode)
            {
                ChamberNames = (List<Chamber>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

            StateHasChanged();

        }
        protected async Task GetShift()
        {

            var response = await lookupService.GetShift();
            if (response.IsSuccessStatusCode)
            {
                Shifts = (List<ChamberShift>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

            StateHasChanged();

        }
        #endregion

        #region Form Submit

        protected async Task SaveChanges(ChamberNumber args)
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
                    if (Id == null)
                    {
                        var fatwaDbCreatelegalPrincipleTagResult = await lookupService.SaveChamberNumber(chamberNumber);
                        if (fatwaDbCreatelegalPrincipleTagResult.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Chamber_Number_Added_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }
                    }
                    else
                    {
                        var fatwaDbUpdateLmsLiteratureTypeResult = await lookupService.UpdateChamberNumber(chamberNumber);
                        if (fatwaDbUpdateLmsLiteratureTypeResult.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Chamber_Number_Updated_Successfully"),
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
                    Detail = Id == null ? translationState.Translate("Could_not_create_a_new_Chamber_Number") : translationState.Translate("Chamber_Number_could_not_be_updated"),
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

        #region Funtions
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
        #endregion
    }
}
