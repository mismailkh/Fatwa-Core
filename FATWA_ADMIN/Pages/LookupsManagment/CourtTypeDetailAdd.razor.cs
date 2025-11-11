using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_GENERAL.Helper.Response;
using Court = FATWA_DOMAIN.Models.CaseManagment.Court;

namespace FATWA_ADMIN.Pages.LookupsManagment
{

    public partial class CourtTypeDetailAdd : ComponentBase
    {
        #region Paramter
        [Parameter]
        public dynamic Id { get; set; }
        #endregion

        #region Variable
        public List<CourtType> courtTypes { get; set; } = new List<CourtType>();
        ApiCallResponse response = new ApiCallResponse();
        Court _CmsCourtG2GLKP;
        protected Court CmsCourtG2GLKP
        {
            get
            {
                return _CmsCourtG2GLKP;
            }
            set
            {
                if (!object.Equals(_CmsCourtG2GLKP, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "CmsCourtG2GLKP", NewValue = value, OldValue = _CmsCourtG2GLKP };
                    _CmsCourtG2GLKP = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        #endregion

        #region Functions
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
        #endregion

        #region OnLoad
        protected override async Task OnInitializedAsync()
        {
            await GetCourtTypes();
            await Load();
        }
        protected async Task Load()
        {
            if (Id == null)
            {
                spinnerService.Show();
                CmsCourtG2GLKP = new Court() { };
                spinnerService.Hide();
            }
            else
            {
                spinnerService.Show();
                response = await lookupService.GetCourtTypesById(Id);
                if (response.IsSuccessStatusCode)
                {
                    CmsCourtG2GLKP = (Court)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                spinnerService.Hide();
            }

        }
        #endregion

        #region populate Court TypeID 
        protected async Task GetCourtTypes()
        {

            var response = await lookupService.GetCourtType();
            if (response.IsSuccessStatusCode)
            {
                courtTypes = (List<CourtType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

            StateHasChanged();

        }
        #endregion

        #region Save / FormSubmit
        protected async Task SaveChanges(Court args)
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
                        var fatwaDbCreateCourtResult = await lookupService.SaveCourtType(CmsCourtG2GLKP);
                        if (fatwaDbCreateCourtResult.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Court_Type_Added_Successfully"),
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
                        var fatwaDbUpdateLmsLiteratureTypeResult = await lookupService.UpdateCourtType(CmsCourtG2GLKP);
                        if (fatwaDbUpdateLmsLiteratureTypeResult.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Court_Type_Updated_Successfully"),
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
                    Detail = Id == null ? translationState.Translate("Could_not_create_a_new_Court_Type") : translationState.Translate("Court_Type_could_not_be_updated"),
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
