using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    public partial class ListDecisionRequests : ComponentBase
    {
        #region Variables & Property Declaration
        protected RadzenDataGrid<CmsJugdmentDecisionVM> grid = new RadzenDataGrid<CmsJugdmentDecisionVM>();
        protected bool Keywords = false;
        public bool isVisible { get; set; }

        public IList<CmsRequestDocumentsVM> selectedFiles;
        public IList<CmsJugdmentDecisionVM> jugdmentDecisionVMs;
        public string UserId { get; set; }
        IEnumerable<CmsJugdmentDecisionVM> _jugdmentDecisionVMs;
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => grid.CurrentPage + 1;
        private int CurrentPageSize => grid.PageSize;
        private int? PageNumber { get; set; } = 1;
        private int? PageSize { get; set; }
        private bool isGridLoaded { get; set; }
        private bool isPageSizeChangeOnFirstLastPage { get; set; }
        IEnumerable<CmsJugdmentDecisionVM> FilteredJudgementDecisionList { get; set; }
        protected IEnumerable<CmsJugdmentDecisionVM> getjugdmentDecisionVMs
        {
            get
            {
                return _jugdmentDecisionVMs;
            }
            set
            {
                if (!Equals(_jugdmentDecisionVMs, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getjugdmentDecisionVMs", NewValue = value, OldValue = getjugdmentDecisionVMs };
                    _jugdmentDecisionVMs = value;

                    Reload();
                }

            }
        }
        private Timer debouncer;
        private const int debouncerDelay = 500;
        #endregion

        #region On Initialized/Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            UserId = loginState.UserDetail.UserId;
            PageSize = systemSettingState.Grid_Pagination;
            translationState.TranslateGridFilterLabels(grid);
            spinnerService.Hide();
        }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        #endregion

        #region On Load Grid Data
        /*<History Author='Ammaar Naveed' Date='2025-01-29'>Load grid data based on pagination</History>*/
        private async Task OnLoadData(LoadDataArgs args)
        {
            try
            {
                if (string.IsNullOrEmpty(args.OrderBy) || CurrentPage != PageNumber || CurrentPageSize != PageSize)
                {
                    if (isGridLoaded && PageSize == CurrentPageSize && !isPageSizeChangeOnFirstLastPage)
                    {
                        grid.CurrentPage = (int)PageNumber - 1;
                        isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(args);
                    spinnerService.Show();
                    var result = await cmsRegisteredCaseService.GetJudgmentDecisionList(Guid.Parse(UserId), PageNumber, PageSize);
                    if (result.IsSuccessStatusCode)
                    {
                        FilteredJudgementDecisionList = getjugdmentDecisionVMs = (List<CmsJugdmentDecisionVM>)result.ResultData;
                        if (!string.IsNullOrEmpty(search)) await OnSearchInput(search);
                        if (!string.IsNullOrEmpty(args.OrderBy) && string.IsNullOrEmpty(search))
                        {
                            FilteredJudgementDecisionList = await gridSearchExtension.Sort(FilteredJudgementDecisionList, ColumnName, SortOrder);
                        }
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(result);
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
        private void SetPagingProperties(LoadDataArgs args)
        {
            if (PageSize != CurrentPageSize)
            {
                int oldPageCount = getjugdmentDecisionVMs.Any() ? (getjugdmentDecisionVMs.First().TotalCount) / ((int)PageSize) : 1;
                int oldPageNumber = (int)PageNumber - 1;
                isGridLoaded = true;
                PageNumber = CurrentPage;
                PageSize = args.Top;
                int TotalPages = getjugdmentDecisionVMs.Any() ? (getjugdmentDecisionVMs.First().TotalCount) / (grid.PageSize) : 1;
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
        /*<History Author='Ammaar Naveed' Date='2025-01-29'>Sort grid data</History>*/
        private async Task OnSortData(DataGridColumnSortEventArgs<CmsJugdmentDecisionVM> args)
        {
            if (args.SortOrder != null)
            {
                FilteredJudgementDecisionList = await gridSearchExtension.Sort(FilteredJudgementDecisionList, args.Column.Property, (SortOrder)args.SortOrder);
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

                    Reload();
                }
            }
        }
        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredJudgementDecisionList = await gridSearchExtension.Filter(getjugdmentDecisionVMs, new Query()
                    {
                        Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US" ?
                    $@"i => ( i.Title != null && i.Title.ToString().ToLower().Contains(@0))
                            ||(i.Description != null && i.Description.ToString().ToLower().Contains(@1))
                            ||(i.DecisionTypeEn != null && i.DecisionTypeEn.ToString().ToLower().Contains(@2)) " :
                    $@"i => ( i.Title != null && i.Title.ToString().ToLower().Contains(@0))
                            ||(i.Description != null && i.Description.ToString().ToLower().Contains(@1)) 
                            ||(i.DecisionTypeAr != null && i.DecisionTypeAr.ToString().ToLower().Contains(@2))",
                        FilterParameters = new object[] { search, search, search }
                    });
                    if (!string.IsNullOrEmpty(ColumnName))
                        FilteredJudgementDecisionList = await gridSearchExtension.Sort(FilteredJudgementDecisionList, ColumnName, SortOrder);
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

        #region Redirect Function
        //<History Author = 'Ijaz Ahmad' Date='2022-12-13' Version="1.0" Branch="master"> Redirect Function </History>

        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        protected async Task RequestedDocumentDetails(CmsRequestDocumentsVM args)
        {
            navigationManager.NavigateTo("requested-documents-list-detail/" + args.CaseId);
        }
        #endregion

        #region Grid Buttons
        protected async Task DetailJudgmentDecision(CmsJugdmentDecisionVM args)
        {
            navigationManager.NavigateTo("/judgmentdecision-detail/" + args.Id);
        }
        #endregion
    }
}
