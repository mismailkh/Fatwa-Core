using FATWA_DOMAIN.Models.LLSLegalPrinciple;
using FATWA_DOMAIN.Models.ViewModel.LLSLegalPrinciple;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.LegalPrinciple.LegalPrincipleEnum;

namespace FATWA_WEB.Pages.LLSLegalPrinciple
{
	public partial class AddLegalPrincipleRelation : ComponentBase
	{
		#region Constructor
		public AddLegalPrincipleRelation()
		{
			PrincipleRelationVMAddResult = new LLSLegalPrinciplesRelationVM();
			GlobalListForExistingPrincipleChanges = new List<LLSLegalPrincipleSystem>();
			//lLSLegalPrincipleReferences = new LLSLegalPrincipleContentSourceDocumentReference();
            FileTypeOptions = new List<object>();
        }
        #endregion

        #region Parameter
        [Parameter]
        public dynamic PrincipleContent { get; set; }
        #endregion

        #region Variable declaration
        public LLSLegalPrinciplesRelationVM PrincipleRelationVMAddResult { get; set; }
		public LLSLegalPrincipleSystem legalPrinciple { get; set; } = new LLSLegalPrincipleSystem();
		protected List<LLSLegalPrinciplesRelationVM> legalPrinciplesAddVM { get; set; }
		protected RadzenDataGrid<LLSLegalPrinciplesRelationVM>? gridRelation;
        protected List<object> FileTypeOptions { get; set; }
        protected List<LLSLegalPrincipleSystem> GlobalListForExistingPrincipleChanges { get; set; }
		//protected LLSLegalPrincipleContentSourceDocumentReference lLSLegalPrincipleReferences { get; set; }
		public bool keywords { get; set; }
		public class FileTypeEnumTemp
        {
            public int? FileTypeEnumValue { get; set; }
            public string FileTypeEnumName { get; set; }
        }
        #endregion

        #region Component Initial
        protected override async Task OnInitializedAsync()
		{
			try
			{
				spinnerService.Show();
				await Load();
				await SearchRelationButtonClick();
				spinnerService.Hide();
			}
			catch (Exception ex)
			{

			}
		}
		protected async Task Load()
		{
            //foreach (LegalPrincipleSourceFileTypeEnum item in Enum.GetValues(typeof(LegalPrincipleSourceFileTypeEnum)))
            //{
            //    FileTypeOptions.Add(new FileTypeEnumTemp { FileTypeEnumName = translationState.Translate(item.ToString()), FileTypeEnumValue = (int)item });
            //}
            //translationState.TranslateGridFilterLabels(gridRelation);
		}

		#endregion

		#region Search Relation Button Click
		protected async Task SearchRelationButtonClick()
		{
			try
			{
                if (PrincipleContent != null)
                {
                    PrincipleRelationVMAddResult.PrincipleContent = (string)PrincipleContent;

				}
                PrincipleRelationVMAddResult.FromPage = 1;
                var resultRelation = await lLSLegalPrincipleService.AdvanceSearchPrincipleRelation(PrincipleRelationVMAddResult);
                if (resultRelation.IsSuccessStatusCode)
                {
                    legalPrinciplesAddVM = (List<LLSLegalPrinciplesRelationVM>)resultRelation.ResultData;
					await gridRelation?.Reload();
                    StateHasChanged();
                }
			}
			catch (Exception)
			{

				throw;
			}
		}

        public async void ResetForm()
        {
            PrincipleRelationVMAddResult = new LLSLegalPrinciplesRelationVM();
            await Load();
            keywords = false;
            StateHasChanged();
        }
        #endregion

        #region Grid add relation button click
        protected async Task GridAddRelationButtonClick(LLSLegalPrinciplesRelationVM args)
		{
			try
			{
				if (args != null)
				{
                    var result = await dialogService.OpenAsync<AddLegalPrincipleRelationPageNumber>(translationState.Translate("Add_Relation_Page_Number"),
                new Dictionary<string, object>()
                {
                    {"PrincipleContent", args.PrincipleContent }
                });
                    var RelationLinkResult = (int)result;
                    if (RelationLinkResult != 0)
                    {
                        args.PageNumber = RelationLinkResult;
                        dialogService.Close(args);
                    }
                }
			}
			catch (Exception)
			{

				throw;
			}
		}

        protected async Task LLSLegalPrincipleDetail(Guid principleId)
        {
            var dialogResult = await dialogService.OpenAsync<DetailLLSLegalPrinciple>(translationState.Translate("Legal_Principle_Detail"),
                                new Dictionary<string, object>() 
                                {
                                    { "PrincipleId", principleId },
                                    { "IsDialog", true } 
                                },
                                new DialogOptions() { Width = "80%", CloseDialogOnOverlayClick = true });
        }
        protected async Task CloseDialog(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
        #endregion
    }
}
