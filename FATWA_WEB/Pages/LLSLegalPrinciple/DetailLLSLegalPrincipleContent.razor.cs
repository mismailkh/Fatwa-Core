using FATWA_DOMAIN.Models.LLSLegalPrinciple;
using FATWA_DOMAIN.Models.ViewModel.LLSLegalPrinciple;
using FATWA_DOMAIN.Models.WorkflowModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using System.Text;
namespace FATWA_WEB.Pages.LLSLegalPrinciple
{
    public partial class DetailLLSLegalPrincipleContent : ComponentBase
    {
        #region Parameter
        [Parameter]
        public dynamic PrincipleContentId { get; set; }
        [Parameter]
        public bool IsPrincipleContent { get; set; } = false;
        #endregion

        #region Constructor
        public DetailLLSLegalPrincipleContent()
        {
            LegalPrincipleContent = new LLSLegalPrincipleContent();
        }
        #endregion

        #region Variables declaration
       
        string PrincipleContent = string.Empty;
        public LLSLegalPrincipleContent LegalPrincipleContent { get; set; }
        public List<LLSLegalPrincipleContentCategoriesVM> LegalPrincipleContentCategories { get; set; }
        public LLSLegalPrincipleContentCategoriesVM PrincipleContentCategories { get; set; }
        StringBuilder categoriesEn = new StringBuilder();
        StringBuilder categoriesAr = new StringBuilder();
        int pageNumber;
        #endregion

        #region On Iitialize
        protected async override Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            spinnerService.Hide();
        }
        #endregion
        #region Load
        private async Task Load()
        {
            if (PrincipleContentId is not null)
            {
                PrincipleContentId = PrincipleContentId.ToString();
                PrincipleContentId = Guid.Parse(PrincipleContentId);
				await GetLLSLegalPrincipleContentCategories();
				await GetLLSLegalPrincipleContentById();
			}
			else
			{
				notificationService.Notify(new NotificationMessage()
				{
					Severity = NotificationSeverity.Error,
					Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
					Style = "position: fixed !important; left: 0; margin: auto; "
				});
			}
		}
        #endregion

        #region Functions
        private async Task GetLLSLegalPrincipleContentById()
        {
            var response = await lLSLegalPrincipleService.GetLLSLegalPrincipleContentById(PrincipleContentId);
            if (response.IsSuccessStatusCode)
            {
                LegalPrincipleContent = (LLSLegalPrincipleContent)response.ResultData;
                var markupContent = new MarkupString(LegalPrincipleContent.PrincipleContent);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        private async Task GetLLSLegalPrincipleContentCategories()
        {
            var response = await lLSLegalPrincipleService.GetLLSLegalPrincipleContentCategories(PrincipleContentId);
            if (response.IsSuccessStatusCode)
            {
                LegalPrincipleContentCategories = (List<LLSLegalPrincipleContentCategoriesVM>)response.ResultData;
                foreach(var items in LegalPrincipleContentCategories)
                {
                    categoriesEn.AppendLine(items.CategoryPathEn);
                    categoriesAr.AppendLine(items.CategoryPathAr);
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

        #region Redirect Buttons
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion

    }
}
