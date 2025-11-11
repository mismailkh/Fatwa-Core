using FATWA_DOMAIN.Models.ViewModel;
using FATWA_WEB.Services;
using FATWA_WEB.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MimeKit.Cryptography;
using Radzen;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.Workflows
{
    public partial class WorkflowInstances : ComponentBase
    {
        #region variable declaration
        protected RadzenDataGrid<WorkflowInstanceDocumentVM> grid0 = new RadzenDataGrid<WorkflowInstanceDocumentVM>();
        public bool allowRowSelectOnRowClick = true;
        protected WorkflowInstanceCountVM _workflowInstanceCount { get; set; } = new WorkflowInstanceCountVM();
        IList<WorkflowInstanceDocumentVM> SelectedRow = new List<WorkflowInstanceDocumentVM>();
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => grid0.CurrentPage + 1;
        private int CurrentPageSize => grid0.PageSize;

        private int? PreviousPageSize { get; set; }
        private int? PreviousPageNumber { get; set; } = 1;
        public bool isGridLoaded { get; set; }
        public bool isPageSizeChangeOnFirstLastPage { get; set; }
        private Timer debouncer;
        private const int debouncerDelay = 500;
        #endregion

        #region Full Property Declaration
        IEnumerable<WorkflowInstanceDocumentVM> getdocumentInstancesResult { get; set; } = new List<WorkflowInstanceDocumentVM>();
        protected IEnumerable<WorkflowInstanceDocumentVM> filteredGetDocumentInstancesResult { get; set; }

        #endregion

        #region On Load Component
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            translationState.TranslateGridFilterLabels(grid0);
            await LoadWorkflowsInstanceCount(0);
            //ChangeGridFilterLabels(grid0);
            spinnerService.Hide();
        }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {

        }
        #endregion

        #region Load Workflow Instances
        protected async Task LoadWorkflowsInstanceCount(int workflowId)
        {

            var response = await workflowService.GetWorkflowsInstanceCount(workflowId);
            if (response.IsSuccessStatusCode)
            {
                _workflowInstanceCount = (WorkflowInstanceCountVM)response.ResultData;
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
                if (string.IsNullOrEmpty(dataArgs.OrderBy) || CurrentPage != PreviousPageNumber || CurrentPageSize != PreviousPageSize)
                {
                    if (isGridLoaded && PreviousPageSize == CurrentPageSize && !isPageSizeChangeOnFirstLastPage)
                    {
                        grid0.CurrentPage = (int)PreviousPageNumber - 1;
                        isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(dataArgs);
                    spinnerService.Show();

                    var response = await workflowService.GetWorkflowInstanceDocuments((int)PreviousPageSize, (int)PreviousPageNumber);
                    spinnerService.Hide();
                    if (response.IsSuccessStatusCode)
                    {
                        getdocumentInstancesResult = (IEnumerable<WorkflowInstanceDocumentVM>)response.ResultData;
                        filteredGetDocumentInstancesResult = getdocumentInstancesResult;

                        if (!(string.IsNullOrEmpty(search))) await OnSearchInput(search);
                        if (!(string.IsNullOrEmpty(dataArgs.OrderBy)) && (string.IsNullOrEmpty(search)))
                        {
                            filteredGetDocumentInstancesResult = await gridSearchExtension.Sort(filteredGetDocumentInstancesResult, ColumnName, SortOrder);
                        }
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
        }
        #endregion

        #region Grid Pagination Calculation
        private void SetPagingProperties(LoadDataArgs args)
        {
            if (PreviousPageSize != CurrentPageSize)
            {
                int oldPageCount = getdocumentInstancesResult.Any() ? (getdocumentInstancesResult.First().TotalCount) / ((int)PreviousPageSize) : 1;
                int oldPageNumber = (int)PreviousPageNumber - 1;
                isGridLoaded = true;
                PreviousPageNumber = CurrentPage;
                PreviousPageSize = args.Top;
                int TotalPages = getdocumentInstancesResult.Any() ? (getdocumentInstancesResult.First().TotalCount) / (grid0.PageSize) : 1;
                if (CurrentPage > TotalPages)
                {
                    PreviousPageNumber = TotalPages + 1;
                    PreviousPageSize = args.Top;
                    grid0.CurrentPage = TotalPages;
                }
                if ((PreviousPageNumber == 1 || (PreviousPageNumber == TotalPages + 1 && oldPageCount == oldPageNumber)) && oldPageCount != 0)
                {
                    isPageSizeChangeOnFirstLastPage = true;
                }
                else
                {
                    isPageSizeChangeOnFirstLastPage = false;
                }
                return;
            }
            PreviousPageNumber = CurrentPage;
            PreviousPageSize = args.Top;
        }
        #endregion

        #region On Sort Grid Data
        private async Task OnSortData(DataGridColumnSortEventArgs<WorkflowInstanceDocumentVM> args)
        {
            if (args.SortOrder != null)
            {
                filteredGetDocumentInstancesResult = await gridSearchExtension.Sort(filteredGetDocumentInstancesResult, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
            }
            else
            {
                ColumnName = string.Empty;
            }
        }
        #endregion

        #region Grid Search
        #region Search Component
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
                    var args = new PropertyChangedEventArgs() { Name = "search", NewValue = value, OldValue = _search };
                    _search = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        #endregion


        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();

                    filteredGetDocumentInstancesResult = await gridSearchExtension.Filter(getdocumentInstancesResult, new Query()
                    {
                        Filter = $@"i => (i.Title != null && i.Title.ToLower().Contains(@0)) 
                                || (i.WorkflowName != null && i.WorkflowName.ToLower().Contains(@1)) 
                                || (i.ActivityName != null && i.ActivityName.ToLower().Contains(@2)) 
                                || (i.Status != null && i.Status.ToLower().Contains(@3))",
                        FilterParameters = new object[] { search, search, search, search }
                    });
                    if (!string.IsNullOrEmpty(ColumnName))
                        filteredGetDocumentInstancesResult = await gridSearchExtension.Sort(filteredGetDocumentInstancesResult, ColumnName, SortOrder);
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

        #region Row Click Action
        protected async Task OnRowClick(WorkflowInstanceDocumentVM row)
        {
            int workflowId = 0;
            allowRowSelectOnRowClick = true;
            if (SelectedRow.Any() && SelectedRow[0] == row)
            {
                allowRowSelectOnRowClick = false;
                SelectedRow = new List<WorkflowInstanceDocumentVM>();
            }
            else if (SelectedRow.Any() && SelectedRow[0] != row)
            {
                SelectedRow = new List<WorkflowInstanceDocumentVM>();
                workflowId = (int)row.WorkflowId;
            }
            else
            {
                workflowId = (int)row.WorkflowId;
            }
            StateHasChanged();
            await LoadWorkflowsInstanceCount(workflowId);
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
        #endregion
    }
}
