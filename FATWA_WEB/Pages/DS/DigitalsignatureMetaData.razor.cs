using FATWA_GENERAL.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace FATWA_WEB.Pages.DS
{
    public partial class DigitalsignatureMetaData : ComponentBase
    {
        public class MetaData
        {
            public string Description { get; set; }
            public string Reason { get; set; }
        }
        public MetaData metaData = new MetaData();

        protected string typeValidationMsgReason = "";
        public async Task FormSubmit()
        {
            try
            {
                if (metaData != null)
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
                        dialogService.Close(metaData);
                    }
                }
                else
                {
                    typeValidationMsgReason = metaData != null ? "" : @translationState.Translate("Required_Field_Reason");
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
