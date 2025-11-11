using FATWA_DOMAIN.Models.ViewModel;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.Lds
{
    public partial class ApprovedLegalLegislationList : ComponentBase
    {

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(FATWA_WEB.Services.PropertyChangedEventArgs args)
        {
        }

        #region Variables Declaration
        protected RadzenDataGrid<LegalLegislationsVM> grid = new RadzenDataGrid<LegalLegislationsVM>();
        public bool allowRowSelectOnRowClick = true;
        protected int count { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; } = 1;
        public bool isGridLoaded { get; set; }
        public bool isPageSizeChangeOnFirstLastPage { get; set; }
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => grid.CurrentPage + 1;
        private int CurrentPageSize => grid.PageSize;
        string _search;
        IEnumerable<LegalLegislationsVM> GetApprovedLegislations { get; set; }
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
        private Timer debouncer;
        private const int debouncerDelay = 500;

        IEnumerable<LegalLegislationsVM> _getLdsDocumentApprovalResult;
        IEnumerable<LegalLegislationsVM> FilteredGetLdsDocumentApprovalResult { get; set; } = new List<LegalLegislationsVM>();
        protected IEnumerable<LegalLegislationsVM> FiltergetLdsDocumentApprovalResult
        {
            get
            {
                return _getLdsDocumentApprovalResult;
            }
            set
            {
                if (!object.Equals(_getLdsDocumentApprovalResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getLdsDocumentApprovalResult", NewValue = value, OldValue = _getLdsDocumentApprovalResult };
                    _getLdsDocumentApprovalResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        #endregion

        #region On Component Load

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            PageSize = systemSettingState.Grid_Pagination;
            translationState.TranslateGridFilterLabels(grid);
            spinnerService.Hide();
        }
        #endregion

        #region On Load Grid Data 
        protected async Task OnLoadData(LoadDataArgs dataArgs)
        {
            try
            {
                if (string.IsNullOrEmpty(dataArgs.OrderBy) || CurrentPage != PageNumber || CurrentPageSize != PageSize)
                {
                    if (isGridLoaded && PageSize == CurrentPageSize && !isPageSizeChangeOnFirstLastPage)
                    {
                        grid.CurrentPage = (int)PageNumber - 1;
                        isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(dataArgs);
                    spinnerService.Show();
                    var response = await legalLegislationService.GetApprovedLegislation((int)PageSize, (int)PageNumber);
                    if (response.IsSuccessStatusCode)
                    {
                        FiltergetLdsDocumentApprovalResult = GetApprovedLegislations = (IEnumerable<LegalLegislationsVM>)response.ResultData;
                        count = GetApprovedLegislations.Count();
                        await InvokeAsync(StateHasChanged);
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    spinnerService.Hide();
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
            if (PageSize != CurrentPageSize)
            {
                int oldPageCount = GetApprovedLegislations.Any() ? (GetApprovedLegislations.First().TotalCount) / ((int)PageSize) : 1;
                int oldPageNumber = (int)PageNumber - 1;
                isGridLoaded = true;
                PageNumber = CurrentPage;
                PageSize = args.Top;
                int TotalPages = GetApprovedLegislations.Any() ? (GetApprovedLegislations.First().TotalCount) / (grid.PageSize) : 1;
                if (CurrentPage > TotalPages)
                {
                    PageNumber = TotalPages + 1;
                    PageSize = args.Top;
                    grid.CurrentPage = TotalPages;
                }
                if ((PageNumber == 1 || (PageNumber == TotalPages + 1 && oldPageCount == oldPageNumber)) && oldPageCount != 0)
                {
                    isPageSizeChangeOnFirstLastPage = true;
                }
                else
                {
                    isPageSizeChangeOnFirstLastPage = false;
                }
                return;
            }
            PageNumber = CurrentPage;
            PageSize = args.Top;
        }
        #endregion

        #region On Sort Grid Data
        private async Task OnSortData(DataGridColumnSortEventArgs<LegalLegislationsVM> args)
        {
            if (args.SortOrder != null)
            {
                FiltergetLdsDocumentApprovalResult = await gridSearchExtension.Sort(FiltergetLdsDocumentApprovalResult, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
            }
            else
            {
                ColumnName = string.Empty;
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
        #endregion

        #region Radzen Button (view detail & decision) on click
        protected async Task LegislationDescision(LegalLegislationsVM args)
        {
            string legalAproved = "2";
            navigationManager.NavigateTo("legislation-decision/" + args.LegislationId + "/" + legalAproved);
        }
        protected async Task ViewLegislationDetail(LegalLegislationsVM args)
        {
            navigationManager.NavigateTo("legallegislation-detailview/" + args.LegislationId);
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
                    FiltergetLdsDocumentApprovalResult = await gridSearchExtension.Filter(GetApprovedLegislations, new Query()
                {
                    Filter = $@"i => (i.Legislation_Number != null && i.Legislation_Number.ToString().ToLower().Contains(@0)) 
                    || (i.Legislation_Type_En != null && i.Legislation_Type_En.ToString().ToLower().Contains(@0))
                    || (i.Legislation_Type_Ar != null && i.Legislation_Type_Ar.ToString().ToLower().Contains(@0))
                    || (i.IssueDate.HasValue && i.IssueDate.Value.ToString(""dd/MM/yyyy"").Contains(@0))
                    || (i.Legislation_Flow_Status_Ar != null && i.Legislation_Flow_Status_Ar.ToString().ToLower().Contains(@0))
                    || (i.Legislation_Flow_Status_En != null && i.Legislation_Flow_Status_En.ToString().ToLower().Contains(@0))
                    || (i.LegislationTitle != null && i.LegislationTitle.ToString().ToLower().Contains(@0))",
                    FilterParameters = new object[] { search }
                });
                if (!string.IsNullOrEmpty(ColumnName))
                    FiltergetLdsDocumentApprovalResult = await gridSearchExtension.Sort(FiltergetLdsDocumentApprovalResult, ColumnName, SortOrder);
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
    }
}




