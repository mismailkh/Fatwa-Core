using FATWA_DOMAIN.Models;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace FATWA_WEB.Pages.Lds
{
    public partial class AddLegalLegislationTags : ComponentBase
    {
       
        #region Variables

        protected LegalLegislationTag legalLegislationTag { get; set; }

        #endregion

        #region Functions

        protected override async Task OnInitializedAsync()
        {
            await Load();
        }
        protected async Task Load()
        {
          
            spinnerService.Show();

            legalLegislationTag = new LegalLegislationTag() { };

            spinnerService.Hide();
        }

        protected async Task FormSubmit(LegalLegislationTag args)
        {
            try
            {
                spinnerService.Show();

                var result = await legalLegislationService.SavelegalLegislationTags(legalLegislationTag);
                if (result.IsSuccessStatusCode)
                {
                    var resultBool = (LegalLegislationTag)result.ResultData;
                    dialogService.Close(resultBool);
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Tags_Save_Success_Message"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
                else
                {
                    dialogService.Close(new LegalLegislationTag());
                }
                spinnerService.Hide();
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Sure_Cancel"), 
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        protected void ButtonCancelClick()
        {
            try
            {
                dialogService.Close(null);

            }
            catch (Exception)
            { 
                throw;
            }
        }

        #endregion
    }
}
