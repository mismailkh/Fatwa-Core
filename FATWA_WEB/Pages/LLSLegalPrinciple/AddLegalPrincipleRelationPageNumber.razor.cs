using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace FATWA_WEB.Pages.LLSLegalPrinciple
{
    public partial class AddLegalPrincipleRelationPageNumber : ComponentBase
    {
        #region Constructor
        public AddLegalPrincipleRelationPageNumber()
        {
            pageNumberTemp = new PageNumberTemp() 
            {
                PageNumber = 0,
                PrincipleContentDetail = string.Empty
            };
        }
        #endregion

        #region Parameter
        [Parameter]
        public dynamic PrincipleContent { get; set; }
        #endregion

        #region Variable declaration
        public class PageNumberTemp
        {
            public int PageNumber { get; set; }
            public string PrincipleContentDetail { get; set; }
        }
        public PageNumberTemp pageNumberTemp { get; set; }

        #endregion

        #region Component Initial
        protected override async Task OnInitializedAsync()
        {
            try
            {
                spinnerService.Show();
                await Load();
                spinnerService.Hide();
            }
            catch (Exception ex)
            {

            }
        }
        protected async Task Load()
        {
            if (PrincipleContent != null)
            {
                pageNumberTemp.PrincipleContentDetail = Convert.ToString(PrincipleContent);
            }
        }

        #endregion

        #region Form submit button click
        protected async Task Form0Submit(PageNumberTemp args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Display_Suggested_Principle_Submit_Message"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    if (args.PageNumber != 0)
                    {
                        dialogService.Close(args.PageNumber);
                    }
                    spinnerService.Hide();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected async Task CloseDialog(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
        #endregion
    }
}
