using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.WorkflowModels;
using FATWA_WEB.Services;
using FATWA_WEB.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.WorkflowEnums;

namespace FATWA_WEB.Pages.Workflows
{
    public partial class Workflows : ComponentBase
    {
        #region Variables Declaration
        protected static IEnumerable<WorkflowStatus> WorkflowStatuses = new List<WorkflowStatus>();
        protected static IEnumerable<Module> Modules = new List<Module>();
        public bool isVisible { get; set; }
        WorkflowStatus selectedStatus = new WorkflowStatus();
        protected RadzenDataGrid<WorkflowListVM> workflowGrid = new RadzenDataGrid<WorkflowListVM>();
        protected bool Keywords = false;
        protected Dictionary<int, RadzenDataGrid<WorkflowListVM>> myWorkflowGrids = new Dictionary<int, RadzenDataGrid<WorkflowListVM>>();
        string attachmentTypesEn = string.Empty;
        string attachmentTypesAr = string.Empty;
        protected bool AdvancedSearchResultGrid;
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => workflowGrid.CurrentPage + 1;
        protected WorkflowAdvanceSearchVM advanceSearchVM = new WorkflowAdvanceSearchVM();
        string _search;
        protected string search
        {
            get
            {
                return _search;
            }
            set
            {
                if (!object.Equals(_search, value))
                {
                    var args = new FATWA_WEB.Services.PropertyChangedEventArgs() { Name = "search", NewValue = value, OldValue = _search };
                    _search = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        private Timer debouncer;
        private const int debouncerDelay = 500;
        public IEnumerable<WorkflowVM> employees;
        protected IEnumerable<WorkflowListVM> getWorkflowsResult { get; set; } = new List<WorkflowListVM>();
        protected IEnumerable<WorkflowListVM> FilteredWorkflowsResult { get; set; }
        protected WorkflowCountVM _workflowCount { get; set; } = new WorkflowCountVM();
        private int CurrentPageSize => workflowGrid.PageSize;

        #endregion

        #region On Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            translationState.TranslateGridFilterLabels(workflowGrid);
            await LoadStatuses();
            await LoadModules();
            await LoadWorkflowsCount();
            spinnerService.Hide();
        }
        #endregion

        #region Grid Search
        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();

                    if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
                    {
                        FilteredWorkflowsResult = await gridSearchExtension.Filter(getWorkflowsResult, new Query()
                        {
                            Filter = $@"i => (i.Name != null && i.Name.ToString().ToLower().Contains(@0)) 
                                    || (i.Description != null && i.Description.ToString().ToLower().Contains(@1)) 
                                    || (i.Status_En != null && i.Status_En.ToString().ToLower().Contains(@2)) 
                                    || (i.ModuleNameEn != null && i.ModuleNameEn.ToString().ToLower().Contains(@3))
                                    || (i.SubModuleNameEn != null && i.SubModuleNameEn.ToString().ToLower().Contains(@4))
                                    || (i.DocumentTypeNameEn != null && i.DocumentTypeNameEn.ToString().ToLower().Contains(@5))
                                    || (i.CreatedDate.HasValue && i.CreatedDate.Value.ToString(""dd/MM/yyyy"").Contains(@6))
                                    || (i.TriggerName != null && i.TriggerName.ToString().ToLower().Contains(@7))",
                            FilterParameters = new object[] { search, search, search, search, search, search, search, search }
                        });
                    }
                    else
                    {
                        FilteredWorkflowsResult = await gridSearchExtension.Filter(getWorkflowsResult, new Query()
                        {
                            Filter = $@"i => (i.Name != null && i.Name.ToString().ToLower().Contains(@0)) 
                                    || (i.Description != null && i.Description.ToString().ToLower().Contains(@1)) 
                                    || (i.Status_Ar != null && i.Status_Ar.ToString().ToLower().Contains(@2)) 
                                    || (i.ModuleNameAr != null && i.ModuleNameAr.ToString().ToLower().Contains(@3))
                                    || (i.SubModuleNameAr != null && i.SubModuleNameAr.ToString().ToLower().Contains(@4))
                                    || (i.DocumentTypeNameAr != null && i.DocumentTypeNameAr.ToString().ToLower().Contains(@5))
                                    || (i.CreatedDate.HasValue && i.CreatedDate.Value.ToString(""dd/MM/yyyy"").Contains(@6))
                                    || (i.TriggerName != null && i.TriggerName.ToString().ToLower().Contains(@7))",
                            FilterParameters = new object[] { search, search, search, search, search, search, search, search }
                        });
                    }
                    if (!string.IsNullOrEmpty(ColumnName))
                        FilteredWorkflowsResult = await gridSearchExtension.Sort(FilteredWorkflowsResult, ColumnName, SortOrder);
                    await InvokeAsync(StateHasChanged);
                }, null, debouncerDelay, Timeout.Infinite);
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
        }
        #endregion

        #region Populate Statuses & Modules
        protected async Task LoadStatuses()
        {
            var WorkflowStatuse = await workflowService.GetWorkflowStatuses();
            if (WorkflowStatuse.IsSuccessStatusCode)
            {
                WorkflowStatuses = (List<WorkflowStatus>)WorkflowStatuse.ResultData;
            }
        }
        protected async Task LoadModules()
        {
            var response = await lookupService.GetModules();
            if (response.IsSuccessStatusCode)
            {
                Modules = (List<Module>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        protected async Task LoadWorkflowsCount()
        {
            var response = await workflowService.GetWorkflowsCount();
            if (response.IsSuccessStatusCode)
            {
                _workflowCount = (WorkflowCountVM)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

        #region On Load Grid Data 
        protected async Task OnLoadData(LoadDataArgs dataArgs)
        {
            try
            {
                if (string.IsNullOrEmpty(dataArgs.OrderBy) || CurrentPage != advanceSearchVM.PageNumber || CurrentPageSize != advanceSearchVM.PageSize || (Keywords && advanceSearchVM.isDataSorted))
                {
                    if (advanceSearchVM.isGridLoaded && advanceSearchVM.PageSize == CurrentPageSize && !advanceSearchVM.isPageSizeChangeOnFirstLastPage)
                    {
                        workflowGrid.CurrentPage = (int)advanceSearchVM.PageNumber - 1;
                        advanceSearchVM.isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(dataArgs);
                    spinnerService.Show();

                    var response = await workflowService.GetWorkflows(advanceSearchVM);

                    if (response.IsSuccessStatusCode)
                    {
                        getWorkflowsResult = (IEnumerable<WorkflowListVM>)response.ResultData;
                        FilteredWorkflowsResult = (IEnumerable<WorkflowListVM>)response.ResultData;
                        if (!(string.IsNullOrEmpty(search))) await OnSearchInput(search);
                        if (!(string.IsNullOrEmpty(dataArgs.OrderBy)) && (string.IsNullOrEmpty(search)))
                        {
                            FilteredWorkflowsResult = await gridSearchExtension.Sort(FilteredWorkflowsResult, ColumnName, SortOrder);
                        }
                        await InvokeAsync(StateHasChanged);
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
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
            spinnerService.Hide();
        }
        #endregion

        #region Grid Pagination Calculation
        private void SetPagingProperties(LoadDataArgs args)
        {
            if (advanceSearchVM.PageSize != null && advanceSearchVM.PageSize != CurrentPageSize)
            {
                int oldPageCount = getWorkflowsResult.Any() ? (getWorkflowsResult.First().TotalCount) / ((int)advanceSearchVM.PageSize) : 1;
                int oldPageNumber = (int)advanceSearchVM.PageNumber - 1;
                advanceSearchVM.isGridLoaded = true;
                advanceSearchVM.PageNumber = CurrentPage;
                advanceSearchVM.PageSize = args.Top;
                int TotalPages = getWorkflowsResult.Any() ? (getWorkflowsResult.First().TotalCount) / (workflowGrid.PageSize) : 1;
                if (CurrentPage > TotalPages)
                {
                    advanceSearchVM.PageNumber = TotalPages + 1;
                    advanceSearchVM.PageSize = args.Top;
                    workflowGrid.CurrentPage = TotalPages;
                }
                if ((advanceSearchVM.PageNumber == 1 || (advanceSearchVM.PageNumber == TotalPages + 1 && oldPageCount == oldPageNumber)) && oldPageCount != 0)
                {
                    advanceSearchVM.isPageSizeChangeOnFirstLastPage = true;
                }
                else
                {
                    advanceSearchVM.isPageSizeChangeOnFirstLastPage = false;
                }
                return;
            }
            advanceSearchVM.PageNumber = CurrentPage;
            advanceSearchVM.PageSize = args.Top;
        }
        #endregion

        #region On Sort Grid Data
        private async Task OnSortData(DataGridColumnSortEventArgs<WorkflowListVM> args)
        {
            if (args.SortOrder != null)
            {
                FilteredWorkflowsResult = await gridSearchExtension.Sort(FilteredWorkflowsResult, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
                advanceSearchVM.isDataSorted = false;
            }
            else
            {
                ColumnName = string.Empty;
            }
        }
        #endregion

        #region GRID Buttons

        protected async Task CreateButtonClick(MouseEventArgs args)
        {
            navigationManager.NavigateTo("/create-workflow");
        }
        //<History Author = 'Hassan Abbas' Date='2022-04-21' Version="1.0" Branch="master"> Redirect to Edit document wizard</History>
        protected Task ViewWorkflowDetail(WorkflowListVM args)
        {
            navigationManager.NavigateTo("/detail-workflow/" + args.WorkflowId);
            return Task.CompletedTask;
        }

        //<History Author = 'Muhammad Zaeem' Date='2023-29-11' Version="1.0" Branch="master"> Redirect to Edit workflow wizard</History>
        protected Task CloneWorkflow(WorkflowListVM args)
        {
            navigationManager.NavigateTo("/update-workflow/" + args.WorkflowId);
            return Task.CompletedTask;
        }

        //<History Author = 'Hassan Abbas' Date='2022-04-21' Version="1.0" Branch="master"> Redirect to Edit document wizard</History>
        protected async Task PublishWorkflow(WorkflowListVM args)
        {
            bool? dialogResponse = await dialogService.Confirm(
               translationState.Translate("Sure_Publish_Workflow"),
               translationState.Translate("Confirm"),
               new ConfirmOptions()
               {
                   OkButtonText = @translationState.Translate("OK"),
                   CancelButtonText = @translationState.Translate("Cancel")
               });

            if (dialogResponse == true)
            {
                var result = await workflowService.UpdateWorkflowStatus((int)args.WorkflowId, (int)WorkflowStatusEnum.Published);

                if (result)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = "Workflow has been published successfully",
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    workflowGrid.Reset();
                    await workflowGrid.Reload();
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Something_Went_Wrong"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-04-21' Version="1.0" Branch="master"> Redirect to Edit document wizard</History>
        protected async Task ActiveWorkflow(WorkflowListVM args)
        {
            string currentWorkflowMsgPart = string.Empty;
            var response = await workflowService.GetWorkflowforSuspend((int)args.WorkflowId, (int)WorkflowStatusEnum.Active);
            if (response.IsSuccessStatusCode)
            {
                Workflow workflow = (Workflow)response.ResultData;
                if (workflow.WorkflowId > 0)
                {
                    currentWorkflowMsgPart = translationState.Translate("Current_Workflow: ") + workflow.Name + Environment.NewLine;
                }
            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
            bool? dialogResponse = await dialogService.Confirm(string.IsNullOrEmpty(currentWorkflowMsgPart) ?
                translationState.Translate("Sure_Active_Workflow") :
               currentWorkflowMsgPart + translationState.Translate("Sure_Active_Workflow_Suspend_Current"),
               translationState.Translate("Confirm"),
               new ConfirmOptions()
               {
                   OkButtonText = @translationState.Translate("OK"),
                   CancelButtonText = @translationState.Translate("Cancel")
               });

            if (dialogResponse == true)
            {
                var result = await workflowService.UpdateWorkflowStatus((int)args.WorkflowId, (int)WorkflowStatusEnum.Active);

                if (result)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = "Workflow_Activated_Successfully",
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    workflowGrid.Reset();
                    await workflowGrid.Reload();
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Something_Went_Wrong"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-04-21' Version="1.0" Branch="master"> Redirect to Edit document wizard</History>
        protected async Task DeleteWorkflow(WorkflowListVM args)
        {
            bool? dialogResponse = await dialogService.Confirm(
               translationState.Translate("Sure_Delete_Workflow"),
               translationState.Translate("Confirm"),
               new ConfirmOptions()
               {
                   OkButtonText = @translationState.Translate("OK"),
                   CancelButtonText = @translationState.Translate("Cancel")
               });

            if (dialogResponse == true)
            {
                var result = await workflowService.UpdateWorkflowStatus((int)args.WorkflowId, (int)WorkflowStatusEnum.Deleted);

                if (result)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = "Workflow has been deleted successfully",
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    workflowGrid.Reset();
                    await workflowGrid.Reload();
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Something_Went_Wrong"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
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
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
        #endregion

        #region Advance Search
        protected async Task SubmitAdvanceSearch()
        {
            if (advanceSearchVM.FromDate > advanceSearchVM.ToDate)
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
            if (advanceSearchVM.ModuleId == 0
                && advanceSearchVM.StatusId == 0
                && !advanceSearchVM.FromDate.HasValue
                && !advanceSearchVM.ToDate.HasValue) { }
            else
            {
                Keywords = advanceSearchVM.isDataSorted = true;
                if (workflowGrid.CurrentPage > 0)
                    await workflowGrid.FirstPage();
                else
                {
                    advanceSearchVM.isGridLoaded = false;
                    await workflowGrid.Reload();
                }
                StateHasChanged();
            }
        }

        protected async void ToggleAdvanceSearch()
        {
            isVisible = !isVisible;
            if (!isVisible)
            {
                ResetForm();
            }
        }
        public async void ResetForm()
        {
            advanceSearchVM = new WorkflowAdvanceSearchVM { PageSize = workflowGrid.PageSize };
            Keywords = advanceSearchVM.isDataSorted = false;
            advanceSearchVM.StatusId = selectedStatus.StatusId;
            workflowGrid.Reset();
            await workflowGrid.Reload();
            StateHasChanged();
        }
        #endregion

    }
}
