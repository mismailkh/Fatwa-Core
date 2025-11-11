using FATWA_DOMAIN.Models.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace FATWA_WEB.Pages.CaseManagment.Shared
{
    public partial class RejectionReason : ComponentBase
    {
        public class Reason
        {
            public string Value { get; set; }


        }


        public Reason reason = new Reason();



        protected string typeValidationMsgReason = "";
        public async Task FormSubmit()
        {
            try
            {
                if (reason.Value != null)
                {
                    bool? dialogResponse = await dialogService.Confirm(
                         translationState.Translate("Sure_Submit"),
                         translationState.Translate("Confirm"),
                         new ConfirmOptions()
                         {
                             OkButtonText = @translationState.Translate("OK"),
                             CancelButtonText = @translationState.Translate("Cancel")
                         });

                    if (dialogResponse == true)
                    {
                        dialogService.Close(reason.Value);
                    }
                }
                else
                {
                    typeValidationMsgReason = reason.Value != null ? "" : @translationState.Translate("Required_Field_Reason");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected void ButtonCancelClick(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
    }
}
