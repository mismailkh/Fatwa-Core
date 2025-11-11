using FATWA_DOMAIN.Models.LLSLegalPrinciple;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using System.Text.RegularExpressions;

namespace FATWA_WEB.Pages.LLSLegalPrinciple
{
    public partial class ListLLSLegalPrincipleContentDialog : ComponentBase
    {
        #region Parameter
        [Parameter]
        public dynamic? CategoryId { get; set; }

        #endregion

        #region Variable
        protected RadzenDataGrid<LLSLegalPrincipleContent>? grid = new RadzenDataGrid<LLSLegalPrincipleContent>();
        protected IEnumerable<LLSLegalPrincipleContent> LLSLegalPrincipleContents = new List<LLSLegalPrincipleContent>();
        protected IEnumerable<LLSLegalPrincipleContent> FilteredLLSLegalPrincipleContents { get; set; } = new List<LLSLegalPrincipleContent>();
        #endregion

        #region On Initialize
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            CategoryId = int.Parse(CategoryId);
            await Load();
            spinnerService.Hide();
        }
        #endregion

        #region Load
        protected async Task Load()
        {
            await PopulateLLSLegalPrincipleContent();
        }
        #endregion

        #region Functions


        private async Task PopulateLLSLegalPrincipleContent()
        {
            try
            {
                var response = await lLSLegalPrincipleService.GetLLSPrincipleContents(CategoryId);
                if (response.IsSuccessStatusCode)
                {
                    LLSLegalPrincipleContents = (IEnumerable<LLSLegalPrincipleContent>)response.ResultData;
                    FilteredLLSLegalPrincipleContents = (IEnumerable<LLSLegalPrincipleContent>)response.ResultData;
                    if (FilteredLLSLegalPrincipleContents is not null)
                        FilteredLLSLegalPrincipleContents = ConvertHtmlConvertToPlainText(FilteredLLSLegalPrincipleContents);
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        private IEnumerable<LLSLegalPrincipleContent> ConvertHtmlConvertToPlainText(IEnumerable<LLSLegalPrincipleContent> principles)
        {
            foreach (var content in principles)
            {

                // Replace </div><div> sequences with a space
                string withoutbothDivs = Regex.Replace(content.PrincipleContent, @"</div><div>", " ");
                // Replace <div> sequences with a space
                string withoutDivs = Regex.Replace(withoutbothDivs, @"<div>", " ");
                // Replace <br> tags with spaces
                string withoutBr = Regex.Replace(withoutDivs, @"<br\s*/?>", " ");
                // Replace all html tags
                string withoutTags = Regex.Replace(withoutBr, @"<[^>]+>", "");
                // Replace HTML entities like &nbsp; with spaces
                string withoutEntities = Regex.Replace(withoutTags, @"&\S+?;", " ");
                // Remove extra spaces between strings
                string withoutExtraSpaces = Regex.Replace(withoutEntities, @"\s+", " ");
                // Trim leading and trailing spaces
                string trimmedString = withoutExtraSpaces.Trim();
                content.PrincipleContent = trimmedString;
            }
            return principles;

        }

        #region Redirect Function
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }

        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion

    }
}
