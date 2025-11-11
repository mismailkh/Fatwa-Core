using FATWA_DOMAIN.Models.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_GENERAL.Helper.Response;
using Court = FATWA_DOMAIN.Models.CaseManagment.Court;

namespace FATWA_ADMIN.Pages.LookupsManagment
{

    public partial class ChamberDetailAdd : ComponentBase
    {

        #region Paramter
        [Parameter]
        public dynamic Id { get; set; }
        #endregion

        #region Variables
        public List<Court> CourtNames { get; set; } = new List<Court>();
        protected Chamber CmsChamberG2GLKP;
        ApiCallResponse response = new ApiCallResponse();
        public string moduleValidationMessageCourtName { get; set; } = "";
        public string moduleValidationMessageCourtCode { get; set; } = "";
        public string moduleValidationMessageCourtNameEnglish { get; set; } = "";
        public string moduleValidationMessageCourtNameArabic { get; set; } = "";
        #endregion

        #region On Load
        protected override async Task OnInitializedAsync()
        {
            await GetCourt();
            await Load();
        }
        protected async Task Load()
        {
            if (Id == null)
            {
                spinnerService.Show();
                CmsChamberG2GLKP = new Chamber() { };
                spinnerService.Hide();
            }
            else
            {
                spinnerService.Show();
                response = await lookupService.GetChamberById(Id);
                if (response.IsSuccessStatusCode)
                {
                    CmsChamberG2GLKP = (Chamber)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                spinnerService.Hide();
            }

        }
        #endregion

        #region populate Court Name 
        protected async Task GetCourt()
        {

            var response = await lookupService.GetCourt();
            if (response.IsSuccessStatusCode)
            {
                CourtNames = (List<Court>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

            StateHasChanged();

        }
        #endregion

        #region Form Submit
        protected async Task SaveChanges(Chamber args)
        {
            try
            {
                bool isValid = true;
                string validationMessage = translationState.Translate("Required_Field");

                
                isValid = (string.IsNullOrWhiteSpace(args.ChamberCode) || string.IsNullOrEmpty(args.ChamberCode))
                    ? (moduleValidationMessageCourtCode = validationMessage, false).Item2
                    : (moduleValidationMessageCourtCode = string.Empty, isValid).Item2;

                isValid = (args.SelectedCourtIds == null || args.SelectedCourtIds.Count() == 0)
                    ? (moduleValidationMessageCourtName = validationMessage, false).Item2
                    : (moduleValidationMessageCourtName = string.Empty, isValid).Item2;

                isValid = (string.IsNullOrWhiteSpace(args.Name_En) || string.IsNullOrEmpty(args.Name_En))
                    ? (moduleValidationMessageCourtNameEnglish = validationMessage, false).Item2
                    : (moduleValidationMessageCourtNameEnglish = string.Empty, isValid).Item2;

                isValid = (string.IsNullOrWhiteSpace(args.Name_Ar) || string.IsNullOrEmpty(args.Name_Ar))
                    ? (moduleValidationMessageCourtNameArabic = validationMessage, false).Item2
                    : (moduleValidationMessageCourtNameArabic = string.Empty, isValid).Item2;
                if (!isValid)
                {
                    StateHasChanged();
                    return;
                }
                else
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
                            var fatwaDbCreateChamberResult = await lookupService.SaveChamber(CmsChamberG2GLKP);
                            if (fatwaDbCreateChamberResult.IsSuccessStatusCode)
                            {
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Success,
                                    Detail = translationState.Translate("Chamber_Added_Successfully"),
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
                            var fatwaDbUpdateLmsLiteratureTypeResult = await lookupService.UpdateChamber(CmsChamberG2GLKP);
                            if (fatwaDbUpdateLmsLiteratureTypeResult.IsSuccessStatusCode)
                            {
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Success,
                                    Detail = translationState.Translate("Chamber_Updated_Successfully"),
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
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = Id == null ? translationState.Translate("Could_not_create_a_new_Chamber") : translationState.Translate("Chamber_could_not_be_updated"),
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
