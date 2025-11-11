using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.LLSLegalPrinciple;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.LegalPrinciple.LegalPrincipleEnum;

namespace FATWA_WEB.Pages.LLSLegalPrinciple
{
    public partial class LLSLegalPrincipleListComponent : ComponentBase
    {
        [Parameter]
        public int? FlowStatusId { get; set; }
        [Parameter]
        public string? UserId { get; set; }
        [Parameter]
        public bool IsPublishUnPublish { get; set; }


        #region Variables Declaration
        protected RadzenDataGrid<LLSLegalPrinciplesReviewVM> grid = new RadzenDataGrid<LLSLegalPrinciplesReviewVM>();
        protected int count { get; set; }
        IEnumerable<LLSLegalPrinciplesReviewVM> getLegalPrinciples { get; set; } = new List<LLSLegalPrinciplesReviewVM>();
        IEnumerable<LLSLegalPrinciplesReviewVM> _FiltergetLegalPrinciples;
        LLSLegalPrincipleAdvanceSearchVM advanceSearch = new LLSLegalPrincipleAdvanceSearchVM();
        string? headings;
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => grid.CurrentPage + 1;
        private int CurrentPageSize => grid.PageSize;
        protected IEnumerable<LLSLegalPrinciplesReviewVM> FiltergetLegalPrinciples
        {
            get
            {
                return _FiltergetLegalPrinciples;
            }
            set
            {
                if (!object.Equals(_FiltergetLegalPrinciples, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "FilteredLegalPrinciples", NewValue = value, OldValue = _FiltergetLegalPrinciples };
                    _FiltergetLegalPrinciples = value;
                    Reload();

                }
            }
        }
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
                    Reload();
                }
            }
        }
        private Timer debouncer;
        private const int debouncerDelay = 500;
        #endregion

        #region Initialized
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            advanceSearch.PageSize = systemSettingState.Grid_Pagination;
            translationState.TranslateGridFilterLabels(grid);
            SetPageSettings();
            spinnerService.Hide();
        }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        private void SetPageSettings()
        {
            advanceSearch.FlowStatusId = FlowStatusId;
            advanceSearch.UserId = UserId;
            advanceSearch.IsPublishUnPublish = IsPublishUnPublish;

            if (!IsPublishUnPublish)
            {
                switch (FlowStatusId)
                {
                    case null:
                        headings = translationState.Translate("Legal_Principle_List");
                        break;

                    case (int)PrincipleFlowStatusEnum.Approve:
                        headings = translationState.Translate("Approved_Legal_Principles");
                        break;

                    case (int)PrincipleFlowStatusEnum.InReview:
                        headings = translationState.Translate("Review_Legal_Principles");
                        break;

                    default:
                        break;
                }
            }
            else
                headings = translationState.Translate("Publish_Unpublish_Legal_Principles");
        }

        #region OnSearchInput  
        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FiltergetLegalPrinciples = await gridSearchExtension.Filter(getLegalPrinciples, new Query()
                    {
                        Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? $@"i => 
                    (i.FlowStatusEn != null && i.FlowStatusEn.ToString().ToLower().Contains(@0)) 
                    || (i.PrincipleNumber != null && i.PrincipleNumber.ToString().ToLower().Contains(@0))  
                    || (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").ToLower().Contains(@0))
                    || (i.NumberOfPrincipleContents != null && i.NumberOfPrincipleContents.ToString().ToLower().Contains(@0))  
                    || (i.TypeEn != null && i.TypeEn.ToString().ToLower().Contains(@0))" :
                    $@"i => 
                    (i.FlowStatusAr != null && i.FlowStatusAr.ToString().ToLower().Contains(@0)) 
                    || (i.PrincipleNumber != null && i.PrincipleNumber.ToString().ToLower().Contains(@0)) 
                    || (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").ToLower().Contains(@0))
                    || (i.NumberOfPrincipleContents != null && i.NumberOfPrincipleContents.ToString().ToLower().Contains(@0))
                    || (i.TypeAr != null && i.TypeAr.ToString().ToLower().Contains(@0))",
                        FilterParameters = new object[] { search }
                    });
                    if (!string.IsNullOrEmpty(ColumnName))
                        FiltergetLegalPrinciples = await gridSearchExtension.Sort(FiltergetLegalPrinciples, ColumnName, SortOrder);
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
        #endregion

        #region On Load Grid Data 
        protected async Task OnLoadData(LoadDataArgs dataArgs)
        {
            try
            {
                if (string.IsNullOrEmpty(dataArgs.OrderBy) || CurrentPage != advanceSearch.PageNumber || CurrentPageSize != advanceSearch.PageSize)
                {

                    if (advanceSearch.isGridLoaded && advanceSearch.PageSize == CurrentPageSize && !advanceSearch.isPageSizeChangeOnFirstLastPage)
                    {
                        grid.CurrentPage = (int)advanceSearch.PageNumber - 1;
                        advanceSearch.isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(dataArgs);
                    spinnerService.Show();
                    var response = await lLSLegalPrincipleService.GetLegalPrinciplesReviewList(advanceSearch);
                    if (response.IsSuccessStatusCode)
                    {
                        FiltergetLegalPrinciples = getLegalPrinciples = (IEnumerable<LLSLegalPrinciplesReviewVM>)response.ResultData;
                        if (!string.IsNullOrEmpty(search)) await OnSearchInput(search);
                        if (!string.IsNullOrEmpty(dataArgs.OrderBy) && string.IsNullOrEmpty(search))
                        {
                            FiltergetLegalPrinciples = await gridSearchExtension.Sort(FiltergetLegalPrinciples, ColumnName, SortOrder);
                        }
                        count = getLegalPrinciples.Count();
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
                throw new Exception(ex.Message);
            }
        }
        #endregion
        #region Grid Pagination Calculation
        private void SetPagingProperties(LoadDataArgs args)
        {
            if (advanceSearch.PageSize != null && advanceSearch.PageSize != CurrentPageSize)
            {
                int oldPageCount = getLegalPrinciples.Any() ? (getLegalPrinciples.First().TotalCount) / ((int)advanceSearch.PageSize) : 1;
                int oldPageNumber = (int)advanceSearch.PageNumber - 1;
                advanceSearch.isGridLoaded = true;
                advanceSearch.PageNumber = CurrentPage;
                advanceSearch.PageSize = args.Top;
                int TotalPages = getLegalPrinciples.Any() ? (getLegalPrinciples.First().TotalCount) / (grid.PageSize) : 1;
                if (CurrentPage > TotalPages)
                {
                    advanceSearch.PageNumber = TotalPages + 1;
                    advanceSearch.PageSize = args.Top;
                    grid.CurrentPage = TotalPages;
                }
                if ((advanceSearch.PageNumber == 1 || (advanceSearch.PageNumber == TotalPages + 1 && oldPageCount == oldPageNumber)) && oldPageCount != 0)
                {
                    advanceSearch.isPageSizeChangeOnFirstLastPage = true;
                }
                else
                {
                    advanceSearch.isPageSizeChangeOnFirstLastPage = false;
                }
                return;
            }
            advanceSearch.PageNumber = CurrentPage;
            advanceSearch.PageSize = args.Top;
        }
        #endregion

        #region On Sort Grid Data
        private async Task OnSortData(DataGridColumnSortEventArgs<LLSLegalPrinciplesReviewVM> args)
        {
            if (args.SortOrder != null)
            {
                FiltergetLegalPrinciples = await gridSearchExtension.Sort(FiltergetLegalPrinciples, args.Column.Property, (SortOrder)args.SortOrder);
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

        #region Action Button
        protected async Task ViewPrincipleDetail(LLSLegalPrinciplesReviewVM args)
        {
            navigationManager.NavigateTo("detail-llsleagalprinciple/" + args.PrincipleId);
        }

        protected async Task LegalPrincipleDescision(LLSLegalPrinciplesReviewVM args)
        {
            if (FlowStatusId == (int)PrincipleFlowStatusEnum.InReview)
            {
                var legalReview = "1";
                navigationManager.NavigateTo("lls-legal-principle-decision/" + args.PrincipleId + "/" + legalReview);
            }
            else if (FlowStatusId == (int)PrincipleFlowStatusEnum.Approve)
            {
                var legalReview = "2";
                navigationManager.NavigateTo("lls-legal-principle-decision/" + args.PrincipleId + "/" + legalReview);
            }
            else
            {
                var legalReview = "3";
                navigationManager.NavigateTo("lls-legal-principle-decision/" + args.PrincipleId + "/" + legalReview);
            }

        }
        #endregion
    }
}
