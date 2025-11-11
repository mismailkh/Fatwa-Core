using FATWA_DOMAIN.Models.LLSLegalPrinciple;
using FATWA_DOMAIN.Models.ViewModel.LLSLegalPrinciple;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Syncfusion.Blazor.Navigations;
using System.Text.RegularExpressions;
using static FATWA_DOMAIN.Enums.LegalPrinciple.LegalPrincipleEnum;

namespace FATWA_WEB.Pages.LLSLegalPrinciple
{
    public partial class LLSCategoriesTreeView : ComponentBase
    {
        #region Variables

        public List<LLSLegalPrincipleCategoriesVM> lLSLegalPrincipleCategoryDetails { get; set; } = new List<LLSLegalPrincipleCategoriesVM>();
        public IList<LLSLegalPrincipleCategoriesVM> FilterlLSLegalPrincipleCategoryDetails { get; set; } = new List<LLSLegalPrincipleCategoriesVM>();
        public string[] LegalPrincipleCategoryList { get; set; } = new string[] { };
        public string[] selectedNodes = Array.Empty<string>();
        string selectedId;
        SfContextMenu<MenuItem> menu;
        SfContextMenu<MenuItem> Pcontentmenu;
        public string[] expandedNodes = new string[] { };
        SfTreeView<LLSLegalPrincipleCategoriesVM> tree;
        bool isEdit = false;
        int index = 0;
        public bool isVisible { get; set; }
        protected bool Keywords { get; set; }
        public List<LLSLegalPrincipleCategory> lLSLegalPrincipleCategories = new List<LLSLegalPrincipleCategory>();
        LLSLegalPrincipleCategoryAdvanceSearchVm advanceSearchVM = new LLSLegalPrincipleCategoryAdvanceSearchVm();
        Guid NavigatePrincipleContentId { get; set; }
        string _search;
        protected string search;

        Guid bgContentId = default(Guid);
        int bgCategoryId = 0;

        #endregion

        #region Component Load
        protected override async Task OnInitializedAsync()
        {
            selectedNodes = new string[] { };
            spinnerService.Show();
            await Load();
            await PopulateContentMnuItems();
            spinnerService.Hide();
        }
        #endregion

        #region Functions
        private async Task Load()
        {

            await GetLLSLegaPrincipleCategories();
            if (lLSLegalPrincipleCategoryDetails.Count > 0)
                index = lLSLegalPrincipleCategoryDetails.Max(x => x.CategoryId) + 1;
        }
        #endregion

        #region Tree View Context Menu

        public List<MenuItem> contentMenuItems = new List<MenuItem>();
        async Task PopulateContentMnuItems()
        {
            contentMenuItems = new List<MenuItem>
            {
                new MenuItem { Text = translationState.Translate("View_Principle_Details") }
                //new MenuItem { Text = translationState.Translate("Edit_Principle_Content") }
            };
        }
        private async Task OpenPContentMenu(MouseEventArgs e, Guid PrincipleContentId, int categoryId)
        {
            int principleFlowStatusId = 0;
            foreach(var item in lLSLegalPrincipleCategoryDetails.Where(x=>x.CategoryId == categoryId).SelectMany(x => x.PrincipleContent.Where(y => y.PrincipleContentId == PrincipleContentId).ToList()))
            {
                item.BackgroundColor = "background-color: #cfc8c1;";
                principleFlowStatusId = (int)item.MainPrincipleFlowStatusId;
            }
            bgCategoryId = categoryId;
            bgContentId = PrincipleContentId;
            NavigatePrincipleContentId = PrincipleContentId;
            if (principleFlowStatusId == (int)PrincipleFlowStatusEnum.NeedModification || principleFlowStatusId == (int)PrincipleFlowStatusEnum.PartiallyCompleted)
            {
                if (contentMenuItems.Count() == 1)
                {
                    contentMenuItems.Add(new MenuItem { Text = translationState.Translate("Edit_Principle_Content") });
                }
            }
            else
            {
                var itemToRemove = contentMenuItems.FirstOrDefault(x => x.Text == translationState.Translate("Edit_Principle_Content"));
                if (itemToRemove != null)
                {
                    contentMenuItems.Remove(itemToRemove);
                }
            }
            await Pcontentmenu.OpenAsync(e.ClientX, e.ClientY);
            StateHasChanged();
        }

        private async Task OnClosePContentMenu(OpenCloseMenuEventArgs<MenuItem> args)
        {
            int principleFlowStatusId = 0;
            foreach (var item in lLSLegalPrincipleCategoryDetails.Where(x => x.CategoryId == bgCategoryId).SelectMany(x => x.PrincipleContent.Where(y => y.PrincipleContentId == bgContentId).ToList()))
            {
                item.BackgroundColor = "";
                bgCategoryId = 0;
                bgContentId = default(Guid);
                principleFlowStatusId = (int)item.MainPrincipleFlowStatusId;
            }
            StateHasChanged();
        }
        public async Task BeforePContentMenuOpen(BeforeOpenCloseMenuEventArgs<MenuItem> args)
        {
        }

        #endregion

        #region Method Triggered After Context Menu Selection

        public async Task PcontentMenuSelect(MenuEventArgs<MenuItem> args)
        {
            string selectedText = args.Item.Text;
            if (selectedText == translationState.Translate("View_Principle_Details"))
            {
                navigationManager.NavigateTo("/details-principle-content/" + NavigatePrincipleContentId);
            }
            else if (selectedText == translationState.Translate("Edit_Principle_Content"))
            {
                navigationManager.NavigateTo("/edit-llslegalprinciplecontent/" + NavigatePrincipleContentId);
            }
        }
        #endregion

        #region TreeView Events Calls
        // Triggers when TreeView Node is selected
        public void OnSelect(NodeSelectEventArgs args)
        {
            this.selectedId = args.NodeData.Id;
        }

        // Triggers when TreeView node is clicked
        public void nodeClicked(NodeClickEventArgs args)
        {
            selectedId = args.NodeData.Id;
            selectedNodes = new string[] { args.NodeData.Id };
        }
        #endregion

        #region Tree Search AND Advance Search
        protected async Task SubmitAdvanceSearch()
        {
            if (advanceSearchVM.StartDate > advanceSearchVM.EndDate)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("FromDate_NotGreater_ToDate"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                Keywords = true;
                return;
            }
            if (!string.IsNullOrEmpty(advanceSearchVM.PrincipleContent) || advanceSearchVM.CategoryId != null || (advanceSearchVM.StartDate.HasValue && advanceSearchVM.EndDate.HasValue))
            {
                Keywords = true;
                var response = await lLSLegalPrincipleService.GetLLSLegaPrincipleCategoriesAdvanceSearch(advanceSearchVM);
                if (response.IsSuccessStatusCode)
                {
                    FilterlLSLegalPrincipleCategoryDetails = (List<LLSLegalPrincipleCategoriesVM>)response.ResultData;
                    if (lLSLegalPrincipleCategoryDetails.Count > 0)
                    {
                        foreach (var item in lLSLegalPrincipleCategoryDetails)
                        {
                            bool IsExpanded = lLSLegalPrincipleCategoryDetails.Any(x => x.ParentId == item.CategoryId);
                            item.Expanded = IsExpanded;
                            item.HasSubChildren = IsExpanded;
                        }
                    }
                    FilterlLSLegalPrincipleCategoryDetails = FilterCatogriesContent(advanceSearchVM.PrincipleContent != null ? advanceSearchVM.PrincipleContent.ToLower() : "") ;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
        }
        public async void ResetForm()
        {

            advanceSearchVM = new LLSLegalPrincipleCategoryAdvanceSearchVm();
            await Load();
            Keywords = false;
            StateHasChanged();
        }

        protected async Task OnSearchInput()
        {
            try
            {
                search = string.IsNullOrEmpty(search) ? "" : search.TrimStart().TrimEnd().ToLower();

                FilterlLSLegalPrincipleCategoryDetails = await gridSearchExtension.Filter(lLSLegalPrincipleCategoryDetails, new Query()
                {

                    Filter = $@"i =>  (i.PrincipleContent != null && i.PrincipleContent.Any(c => c.PrincipleContent.ToLower().Contains(@0)))",
                    FilterParameters = new object[] { search }
                });

                if (!string.IsNullOrEmpty(search))
                {
                    FilterlLSLegalPrincipleCategoryDetails = FilterCatogriesContent(search);
                }
                else
                {
                    await Load();
                }
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
            await InvokeAsync(StateHasChanged);
        }
        public List<LLSLegalPrincipleCategoriesVM> FilterCatogriesContent(string SearchText)
        {

            List<LLSLegalPrincipleCategoriesVM> parentnodes = new List<LLSLegalPrincipleCategoriesVM>();
            foreach (var item in FilterlLSLegalPrincipleCategoryDetails)
            {
                parentnodes.Add(item);
                FindParentNodes(item.ParentId);
            }
            void FindParentNodes(int? parentId)
            {
                if (parentId == null || !lLSLegalPrincipleCategoryDetails.Any(x => x.CategoryId == parentId))
                    return;

                var parentNode = lLSLegalPrincipleCategoryDetails.FirstOrDefault(x => x.CategoryId == parentId);
                parentnodes.Add(parentNode);
                FindParentNodes(parentNode.ParentId);
            }
            var rootNodes = lLSLegalPrincipleCategoryDetails.Where(x => x.ParentId == null).FirstOrDefault();
            parentnodes.Add(rootNodes);
            var distinctFilteredList = parentnodes.Distinct().ToList();
            if (!string.IsNullOrEmpty(SearchText))
            {
                foreach (var item in distinctFilteredList)
                {
                    if (item.PrincipleContent.Any())
                    {
                        var itemsToRemove = new List<LLSLegalPrincipleContent>();

                        foreach (var subitem in item.PrincipleContent)
                        {
                            if (!subitem.PrincipleContent.ToLower().Contains(SearchText))
                            {
                                itemsToRemove.Add(subitem);
                            }
                        }
                        foreach (var subitemToRemove in itemsToRemove)
                        {
                            item.PrincipleContent.Remove(subitemToRemove);
                        }
                    }
                }
            }
            return distinctFilteredList;
        }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        protected async Task ToggleAdvanceSearch()
        {
            isVisible = !isVisible;
            if (!isVisible)
            {
                ResetForm();
            }
        }
        #endregion

        #region Get Principle Categories
        private async Task GetLLSLegaPrincipleCategories()
        {
            try
            {
				spinnerService.Show();
				var response = await lLSLegalPrincipleService.GetLLSLegaPrincipleCategories();
				if (response.IsSuccessStatusCode)
				{
					lLSLegalPrincipleCategoryDetails = (List<LLSLegalPrincipleCategoriesVM>)response.ResultData;
					FilterlLSLegalPrincipleCategoryDetails = lLSLegalPrincipleCategoryDetails;
					if (lLSLegalPrincipleCategoryDetails.Count > 0)
					{
						foreach (var item in lLSLegalPrincipleCategoryDetails)
						{
							item.PrincipleContent = (List<LLSLegalPrincipleContent>)ConvertHtmlConvertToPlainText(item.PrincipleContent);
							if (item.PrincipleContent.Count() != 0)
							{
								foreach (var content in item.PrincipleContent)
								{
                                    if (content != null)
                                    {
										if (content.PrincipleContent.Length > 140)
										{
											content.PrincipleContent = (content.PrincipleContent.Substring(0, 140)) + "...";
										}
									}
								}
							}
							bool IsExpanded = lLSLegalPrincipleCategoryDetails.Any(x => x.ParentId == item.CategoryId);
							item.Expanded = IsExpanded;
							item.HasSubChildren = IsExpanded;
						}
					}
				}
				else
				{
					await invalidRequestHandlerService.ReturnBadRequestNotification(response);
				}
				spinnerService.Hide();
			}
            catch (Exception)
            {

                throw;
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

        private IEnumerable<LLSLegalPrincipleContent> ConvertHtmlConvertToPlainText(List<LLSLegalPrincipleContent> principles)
        {
            try
            {
				if (principles.Any())
				{
					foreach (var content in principles)
					{
                        if (content != null)
                        {
							if (content.PrincipleContent != null)
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
						}
					}
					return principles;
				}
				return new List<LLSLegalPrincipleContent>();
			}
            catch (Exception)
            {

                throw new Exception();
            }
        }
    }
}
