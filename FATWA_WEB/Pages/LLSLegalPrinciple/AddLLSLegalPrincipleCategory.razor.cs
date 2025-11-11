using FATWA_DOMAIN.Models.LLSLegalPrinciple;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace FATWA_WEB.Pages.LLSLegalPrinciple
{
    public partial class AddLLSLegalPrincipleCategory : ComponentBase
    {
        #region Constructor
        public AddLLSLegalPrincipleCategory()
        {
            lLSLegalPrincipleCategory = new LLSLegalPrincipleCategory();
            CategoryParentDetails = new List<LLSLegalPrincipleCategory>();
        }
        #endregion

        #region Variables 
        protected List<LLSLegalPrincipleCategory> CategoryParentDetails { get; set; }
        #endregion

        #region Model full property Instance
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        public LLSLegalPrincipleCategory lLSLegalPrincipleCategory { get; set; }
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
                throw ex;
            }
        }
        protected async Task Load()
        {
            //var resultNumber = await legalLegislationService.GetLegalSectionNewNumber();
            //if (resultNumber.IsSuccessStatusCode)
            //{
            //    lLSLegalPrincipleCategory.Section_Number = (int)resultNumber.ResultData;
            //}
            var resultParent = await lLSLegalPrincipleService.GetLLSLegalPrincipleCategory();
            if (resultParent.IsSuccessStatusCode)
            {
                CategoryParentDetails = (List<LLSLegalPrincipleCategory>)resultParent.ResultData;
            }
        }

        #endregion

        #region Submit button click

        protected async Task Form0Submit(LLSLegalPrincipleCategory args)
        {
            try
            {
                spinnerService.Show();
                args.IsActive = true;
				var result = await lLSLegalPrincipleService.SaveLegalPrincipleCategory(args);
				if (result.IsSuccessStatusCode)
				{
					var resultBool = (LLSLegalPrincipleCategory)result.ResultData;
					dialogService.Close(resultBool);
					notificationService.Notify(new NotificationMessage()
					{
						Severity = NotificationSeverity.Success,
						Detail = translationState.Translate("Category_Save_Success_Message"),
						Style = "position: fixed !important; left: 0; margin: auto; "
					});
				}
				else
				{
					dialogService.Close(new LLSLegalPrincipleCategory());
				}
				spinnerService.Hide();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Dialog close
        protected async Task CloseDialog(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
        #endregion

    }
}
