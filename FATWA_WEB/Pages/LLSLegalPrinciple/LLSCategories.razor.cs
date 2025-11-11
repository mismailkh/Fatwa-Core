using FATWA_DOMAIN.Models.LLSLegalPrinciple;
using FATWA_DOMAIN.Models.ViewModel.LLSLegalPrinciple;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Syncfusion.Blazor.Navigations;

namespace FATWA_WEB.Pages.LLSLegalPrinciple
{
    public partial class LLSCategories : ComponentBase
    {
        #region Variables

        public List<LLSLegalPrincipleCategoriesVM> lLSLegalPrincipleCategoryDetails { get; set; } = new List<LLSLegalPrincipleCategoriesVM>();
        public string[] LegalPrincipleCategoryList { get; set; } = new string[] { };
        public string[] selectedNodes = Array.Empty<string>();
        string selectedId;
        SfContextMenu<MenuItem> menu;
        public string[] expandedNodes = new string[] { };
        SfTreeView<LLSLegalPrincipleCategoriesVM> tree;
        bool isEdit = false;
        int index = 0;
        #endregion

        #region Component Load
        protected override async Task OnInitializedAsync()
        {
            selectedNodes = new string[] { };
            spinnerService.Show();
            await PopulateMethods();
            spinnerService.Hide();
        }
        #endregion

        #region Functions
        private async Task PopulateMethods()
        {
            await GetLLSLegaPrincipleCategories();
            if (lLSLegalPrincipleCategoryDetails.Count > 0)
                index = lLSLegalPrincipleCategoryDetails.Max(x => x.CategoryId) + 1;
        }

        //principle content base on CategoryId
        private async Task PopulatePrincipleContents()
        {
            var dialogResult = await dialogService.OpenAsync<ListLLSLegalPrincipleContentDialog>(
                translationState.Translate("Legal_Principe_Contents"),
                new Dictionary<string, object>() {
                    { "CategoryId", this.selectedId },
                },
                new DialogOptions() { Width = "60% !important", CloseDialogOnOverlayClick = true });
            dialogService.Close();
            return;
        }
        #endregion

        #region Tree View Context Menu

        // Datasource for menu items
        public List<MenuItem> MenuItems = new List<MenuItem>();
   
        void PopulateMenuItems()
        {
            MenuItems = new List<MenuItem>
            {
                new MenuItem { Text = translationState.Translate("Add_SubCategory") },
                new MenuItem { Text = translationState.Translate("Edit_Label") },
                new MenuItem { Text = translationState.Translate("Remove_Node") },
            };
        }

        void PopulateMenuItemForDefaultNode()
        {
            MenuItems = new List<MenuItem>
            {
                new MenuItem { Text = translationState.Translate("Add_SubCategory") }
            };
        }

        //open context menu on button click
        private async Task OpenContextMenu(MouseEventArgs e)
        {
            await menu.OpenAsync(e.ClientX, e.ClientY);
        }

        //trigger before opening the menu items
        public async Task BeforeContextOpen(BeforeOpenCloseMenuEventArgs<MenuItem> args)
        {
            var treeNode = tree.GetNode(selectedId);

            if(treeNode.ParentID == null)
            {
                PopulateMenuItemForDefaultNode();
            }
            else
            {
                PopulateMenuItems();
            }
            var node = lLSLegalPrincipleCategoryDetails.Where(x => x.CategoryId == int.Parse(treeNode.Id)).FirstOrDefault();
            if (node.PrincipleContentCount > 0)
            {
                //MenuItems.Where(x => x.Text == "Edit_Label" && x.Text == "Remove_Node").Select(x => { x.Disabled = true; return x; });

                foreach (var item in MenuItems)
                {
                    if (item.Text == translationState.Translate("Edit_Label") || item.Text == translationState.Translate("Remove_Node"))
                        item.Disabled = true;
                }
            }
            else
            {
                foreach (var item in MenuItems)
                {
                    if (item.Text == translationState.Translate("Edit_Label") || item.Text == translationState.Translate("Remove_Node"))
                        item.Disabled = false;
                }
            }
        }
        #endregion

        #region Method Triggered After Context Menu Selection

        // Triggers when context menu is selected
        public async Task MenuSelect(MenuEventArgs<MenuItem> args)
        {
            string selectedText;
            selectedText = args.Item.Text;
            if (selectedText == translationState.Translate("Edit_Label"))
            {
                isEdit = true;
                await this.RenameNodes();

            }
            else if (selectedText == translationState.Translate("Remove_Node"))
            {
                this.RemoveNodes();
            }
            else if (selectedText == translationState.Translate("Add_SubCategory"))
            {
                isEdit = false;
                await this.AddNodes();
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
        //Triggered after the node being edited
        public async Task NodeEdited(NodeEditEventArgs args)
        {
            if (!isEdit)
            {
                lLSLegalPrincipleCategoryDetails
                   .Where(x => x.CategoryId == index)
                   .Select(x => { x.Name = args.NewText; return x; }).ToList();
            }
            else
            {
                lLSLegalPrincipleCategoryDetails
                    .Where(x => x.CategoryId == Convert.ToInt32(selectedId))
                    .Select(x => { x.Name = args.NewText; return x; }).ToList();
            }
            if (string.IsNullOrWhiteSpace(args.NewText))
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Node_Name_Cannot_be_Empty"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                if (!isEdit)
                    lLSLegalPrincipleCategoryDetails.Remove(lLSLegalPrincipleCategoryDetails.FirstOrDefault(x => x.CategoryId == index));
                await PopulateMethods();
                return;
            }
            else
            {
                string Message = isEdit ? translationState.Translate("Sure_Rename_Category") : translationState.Translate("Sure_Add_Category");
                if (await dialogService.Confirm(Message, translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    if (!isEdit)
                    {
                        selectedId = args.NodeData.Id;
                        selectedNodes = new string[] { args.NodeData.Id };
                        LLSLegalPrincipleCategory llsCategory = new LLSLegalPrincipleCategory
                        {
                            ParentId = Convert.ToInt32(args.NodeData.ParentID),
                            Name = args.NewText,
                        };
                        await AddCategory(llsCategory);
                    }
                    else
                    {
                        var updateNode = lLSLegalPrincipleCategoryDetails.Where(x => x.CategoryId == Convert.ToInt32(args.NodeData.Id)).FirstOrDefault();
                        LLSLegalPrincipleCategory updateCategory = new LLSLegalPrincipleCategory
                        {
                            CategoryId = updateNode.CategoryId,
                            Name = args.NewText,
                        };
                        await UpdateCategory(updateCategory);
                    }
                    await PopulateMethods();
                    spinnerService.Hide();

                }
                else
                {
                    spinnerService.Show();
                    args.Cancel = true;
                    await PopulateMethods();
                    spinnerService.Hide();
                    return;
                }
            }
        }


        #endregion

        #region Add Legal Principle Category

        protected async Task AddCategory(LLSLegalPrincipleCategory args)
        {
            try
            {
                spinnerService.Show();
                args.IsActive = true;
                var result = await lLSLegalPrincipleService.SaveLegalPrincipleCategory(args);
                if (result.IsSuccessStatusCode && (bool)result.ResultData)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Category_Save_Success_Message"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    await PopulateMethods();
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("something_went_wrong"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
                spinnerService.Hide();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Update Legal Principle Category

        protected async Task UpdateCategory(LLSLegalPrincipleCategory args)
        {
            spinnerService.Show();
            args.IsActive = true;
            var result = await lLSLegalPrincipleService.UpdateLegalPrincipleCategory(args);
            if (result.IsSuccessStatusCode && (bool)result.ResultData)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Success,
                    Detail = translationState.Translate("Category_Updated_Successfully"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                await PopulateMethods();
            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("something_went_wrong"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
            spinnerService.Hide();
        }
        #endregion

        #region Delete Legal Principle Category
        //soft delete
        protected async Task DeleteCategory(LLSLegalPrincipleCategory args)
        {
            spinnerService.Show();
            args.IsActive = true;
            var result = await lLSLegalPrincipleService.DeleteLegalPrincipleCategory(args);
            if (result.IsSuccessStatusCode && (bool)result.ResultData)
            {
                await PopulateMethods();
                StateHasChanged();
                await Task.Delay(500);
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Success,
                    Detail = translationState.Translate("Category_Deleted_Successfully"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("something_went_wrong"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
            spinnerService.Hide();
        }
        #endregion

        #region Get Principle Categories
        private async Task GetLLSLegaPrincipleCategories()
        {
            var response = await lLSLegalPrincipleService.GetLLSLegaPrincipleCategories();
            if (response.IsSuccessStatusCode)
            {
                lLSLegalPrincipleCategoryDetails = (List<LLSLegalPrincipleCategoriesVM>)response.ResultData;
                if (lLSLegalPrincipleCategoryDetails.Count > 0)
                {
                    foreach (var item in lLSLegalPrincipleCategoryDetails)
                    {
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
        }
        #endregion

        #region Context Select Methods 

        // To add a new node
        async Task AddNodes()
        {
            // Expand the selected nodes
            expandedNodes = new string[] { this.selectedId };

            int NodeId = index;
            lLSLegalPrincipleCategoryDetails.Add(new LLSLegalPrincipleCategoriesVM
            {
                CategoryId = NodeId,
                Name = "New Category",
                ParentId = Convert.ToInt32(this.selectedId)
            });

            lLSLegalPrincipleCategoryDetails.FirstOrDefault(x => x.CategoryId == int.Parse(selectedId)).Expanded = true;
            lLSLegalPrincipleCategoryDetails.FirstOrDefault(x => x.CategoryId == int.Parse(selectedId)).HasSubChildren = true;

            await Task.Delay(100);
            StateHasChanged();

            // Edit the added node.
            await this.tree.BeginEditAsync(NodeId.ToString());
        }

        // To delete a tree node
        async void RemoveNodes()
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Remove_Category"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                var NodeToRemove = tree.GetNode(selectedId);
                LLSLegalPrincipleCategory removeNode = new LLSLegalPrincipleCategory()
                {
                    CategoryId = int.Parse(NodeToRemove.Id),
                    IsDeleted = true,
                };
                await DeleteCategory(removeNode);
            }
            else
                return;
        }

        // To edit a tree node
        async Task RenameNodes()
        {
            await this.tree.BeginEditAsync(this.selectedId);
        }
        #endregion

    }
}
