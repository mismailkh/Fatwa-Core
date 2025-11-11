using FATWA_DOMAIN.Models.ViewModel.LLSLegalPrinciple;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.LLSLegalPrinciple
{
    public partial class DetailLLSLegalPrinciple : ComponentBase
	{
        #region Parameter
        [Parameter]
        public dynamic PrincipleId { get; set; }  
        [Parameter]
        public dynamic? IsDialog { get; set; }

        #endregion

        #region Variable
        protected RadzenDataGrid<LLSLegalPrincipleDetailVM>? grid = new RadzenDataGrid<LLSLegalPrincipleDetailVM>();
        protected LLSLegalPrinciplesReviewVM LLSLegalPrincipleDetailVM { get; set; } = new LLSLegalPrinciplesReviewVM();
        protected List<LLSLegalPrinciplReferenceVM> LLSLegalPrincipleReferenceVM { get; set; } = new List<LLSLegalPrinciplReferenceVM>();
        protected List<LLSLegalPrinciplReferenceVM> FilteredLLSLegalPrincipleReferenceVM { get; set; } = new List<LLSLegalPrinciplReferenceVM>();
		//protected List<LLSLegalPrincipleDetailVM> FilteredLLSLegalPrincipleDetailVM { get; set; } = new List<LLSLegalPrincipleDetailVM>();
		protected RadzenHtmlEditor editor = new RadzenHtmlEditor();
		public bool busyPreviewBtn { get; set; }
        protected RadzenDataGrid<LLSLegalPrinciplesContentVM> gridRelation { get; set; }
        public List<LLSLegalPrinciplesContentVM> lLSLegalPrinciplesContentVMs { get; set; } = new List<LLSLegalPrinciplesContentVM>();
        #endregion

        #region On Initialize
        protected override async Task OnInitializedAsync()
        {
		
			spinnerService.Show(); 
            await Load();
            spinnerService.Hide();
        }
        #endregion

        #region Load
        protected async Task Load()
        {
            await GetLLSLegalPrincipleDetailById();
            await GetLLSLegalPrincipleContentListByPrincipleId();
        }
        #endregion

        #region Functions


        private async Task GetLLSLegalPrincipleDetailById()
        {
            try
            {
                var response = await lLSLegalPrincipleService.GetLegalPrincipleDetailById(Guid.Parse(PrincipleId));

                if (response.IsSuccessStatusCode)
                {
                    LLSLegalPrincipleDetailVM = (LLSLegalPrinciplesReviewVM)response.ResultData;
                    //var resultContentDetails = await GetLLSLegalPrincipleContentDetailsByUsingPrincipleId(Guid.Parse(PrincipleId));
                    //if (resultContentDetails.Count() != 0)
                    //{
                    //    lLSLegalPrinciplesContentVMs = resultContentDetails;
                    //}
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception)
            {

                throw;
            }
            
        }

        private async Task GetLLSLegalPrincipleContentListByPrincipleId()
        {
            try
            {
                var response = await lLSLegalPrincipleService.GetLLSLegalPrincipleContentDetailsByUsingPrincipleId(Guid.Parse(PrincipleId));
                if(response.IsSuccessStatusCode)
                {
                    lLSLegalPrinciplesContentVMs = (List<LLSLegalPrinciplesContentVM>)response.ResultData;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        #endregion

        #region Redirect Function
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

        protected async Task ViewDetail(LLSLegalPrinciplesContentVM args)
        {
            var result = await dialogService.OpenAsync<DetailLLSLegalPrincipleContent>(translationState.Translate("Legal_Principle_Detail"),
            new Dictionary<string, object>()
            {
                { "PrincipleContentId", args.PrincipleContentId},
                { "IsPrincipleContent", true },
            },
            new DialogOptions() { Width = "100% !important", CloseDialogOnOverlayClick = true }
        );
            await Task.Delay(200);
            await Load();
            StateHasChanged();
        }
        #endregion

    }
}
