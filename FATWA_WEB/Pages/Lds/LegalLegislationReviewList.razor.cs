using FATWA_DOMAIN.Models.ViewModel;
using FATWA_WEB.Services;
using FATWA_WEB.Shared;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.Lds
{
    public partial class LegalLegislationReviewList : ComponentBase
    {
        #region Variables Declaration

        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }
        protected RadzenDataGrid<LegalLegislationsVM> grid = new RadzenDataGrid<LegalLegislationsVM>();
        public bool allowRowSelectOnRowClick = true;
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => grid.CurrentPage + 1;
        private int PageNumber { get; set; } = 1;
        private int PageSize { get; set; } = 10;
        private int CurrentPageSize => grid.PageSize;
        private int? PreviousPageSize { get; set; }
        private int? PreviousPageNumber { get; set; } = 1;
        public bool isGridLoaded { get; set; }
        public bool isPageSizeChangeOnFirstLastPage { get; set; }
        IEnumerable<LegalLegislationsVM> LdsDocumentApprovalResult { get; set; } = new List<LegalLegislationsVM>();
        protected string search { get; set; }
        protected IEnumerable<LegalLegislationsVM> FiltergetLdsDocumentApprovalResult { get; set; }
        private Timer debouncer;
        private const int debouncerDelay = 500;
        #endregion

        #region On Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            translationState.TranslateGridFilterLabels(grid);

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

        #region On Load Grid Data 
        protected async Task OnLoadData(LoadDataArgs dataArgs)
        {
            if (string.IsNullOrEmpty(dataArgs.OrderBy) || CurrentPage != PreviousPageNumber || CurrentPageSize != PreviousPageSize)
            {
                try
                {
                    if (isGridLoaded && PreviousPageSize == CurrentPageSize && !isPageSizeChangeOnFirstLastPage)
                    {
                        grid.CurrentPage = (int)PreviousPageNumber - 1;
                        isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(dataArgs);
                    spinnerService.Show();
                    var response = await legalLegislationService.GetLegalLegislationApprovals((int)PreviousPageSize, (int)PreviousPageNumber);
                    spinnerService.Hide();
                    if (response.IsSuccessStatusCode)
                    {
                        LdsDocumentApprovalResult = (IEnumerable<LegalLegislationsVM>)response.ResultData;
                        FiltergetLdsDocumentApprovalResult = (IEnumerable<LegalLegislationsVM>)response.ResultData;
                        if (!(string.IsNullOrEmpty(search))) await OnSearchInput(search);
                        if (!(string.IsNullOrEmpty(dataArgs.OrderBy)) && (string.IsNullOrEmpty(search)))
                        {
                            FiltergetLdsDocumentApprovalResult = await gridSearchExtension.Sort(FiltergetLdsDocumentApprovalResult, ColumnName, SortOrder);
                        }
                        await InvokeAsync(StateHasChanged);
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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
        }
        #endregion

        #region Grid Pagination Calculation
        private void SetPagingProperties(LoadDataArgs args)
        {
            if (PreviousPageSize != CurrentPageSize)
            {
                int oldPageCount = LdsDocumentApprovalResult.Any() ? (LdsDocumentApprovalResult.First().TotalCount) / ((int)PreviousPageSize) : 1;
                int oldPageNumber = (int)PreviousPageNumber - 1;
                isGridLoaded = true;
                PreviousPageNumber = CurrentPage;
                PreviousPageSize = args.Top;
                int TotalPages = LdsDocumentApprovalResult.Any() ? (LdsDocumentApprovalResult.First().TotalCount) / (grid.PageSize) : 1;
                if (CurrentPage > TotalPages)
                {
                    PreviousPageNumber = TotalPages + 1;
                    PreviousPageSize = args.Top;
                    grid.CurrentPage = TotalPages;
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


        protected async Task LegislationDescision(LegalLegislationsVM args)
        {
            int legalReview = 1;
            navigationManager.NavigateTo("legislation-decision/" + args.LegislationId + "/" + legalReview);
        }
        protected async Task ViewLegislationDetail(LegalLegislationsVM args)
        {
            var Redirect = "legallegislationreview";
            navigationManager.NavigateTo("legallegislation-detailview/" + args.LegislationId + "/" + Redirect);

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

                    FiltergetLdsDocumentApprovalResult = await gridSearchExtension.Filter(LdsDocumentApprovalResult, new Query()
                {
                    Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US"
                    ? $@"i => (i.Legislation_Number != null && i.Legislation_Number.ToString().ToLower().Contains(@0)) 
                            || (i.LegislationTitle != null && i.LegislationTitle.ToString().ToLower().Contains(@1))
                            || (i.Legislation_Type_En != null && i.Legislation_Type_En.ToString().ToLower().Contains(@2))
                            || (i.IssueDate.HasValue && i.IssueDate.Value.ToString(""dd/MM/yyyy"").Contains(@3))
                            || (i.Legislation_Flow_Status_En != null && i.Legislation_Flow_Status_En.ToString().ToLower().Contains(@4))"

                    : $@"i => (i.Legislation_Number != null && i.Legislation_Number.ToString().ToLower().Contains(@0)) 
                            || (i.LegislationTitle != null && i.LegislationTitle.ToString().ToLower().Contains(@1))
                            || (i.Legislation_Type_Ar != null && i.Legislation_Type_Ar.ToString().ToLower().Contains(@2))
                            || (i.IssueDate.HasValue && i.IssueDate.Value.ToString(""dd/MM/yyyy"").Contains(@3))
                            || (i.Legislation_Flow_Status_Ar != null && i.Legislation_Flow_Status_Ar.ToString().ToLower().Contains(@4))",
                    FilterParameters = new object[] { search, search, search, search, search }
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
        #endregion

    }
}
