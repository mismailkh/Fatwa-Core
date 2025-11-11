using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.Lds
{
    public partial class PublishUnpublishLegislationNew : ComponentBase
    {
        #region Variables Declaration
        protected RadzenDataGrid<LegalLegislationsVM> grid = new RadzenDataGrid<LegalLegislationsVM>();
        protected AdvanceSearchLegalLegislationsVM advanceSearchVM = new AdvanceSearchLegalLegislationsVM();
        protected List<LegalLegislationStatus> statuses { get; set; } = new List<LegalLegislationStatus>();
        protected List<LegalLegislationType> legislationTypes { get; set; } = new List<LegalLegislationType>();
        public bool isVisible { get; set; }
        protected bool Keywords = false;
        string _search;
        private Timer debouncer;
        private const int debouncerDelay = 500;
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => grid.CurrentPage + 1;
        private int CurrentPageSize => grid.PageSize;
        IEnumerable<LegalLegislationsVM> LegalLegislationsForPublish { get; set; } = new List<LegalLegislationsVM>();
        protected int count { get; set; }

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
        IEnumerable<LegalLegislationsVM> _getLegalLegislations;
        protected IEnumerable<LegalLegislationsVM> FilteredLegalLegislations
        {
            get
            {
                return _getLegalLegislations;
            }
            set
            {
                if (!object.Equals(_getLegalLegislations, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getLegislationResult", NewValue = value, OldValue = _getLegalLegislations };
                    _getLegalLegislations = value;
                    Reload();
                }
            }
        }
        #endregion

        #region Initialized
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            advanceSearchVM.PageSize = systemSettingState.Grid_Pagination;
            translationState.TranslateGridFilterLabels(grid);
            spinnerService.Hide();
        }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
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
                        grid.CurrentPage = (int)advanceSearchVM.PageNumber - 1;
                        advanceSearchVM.isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(dataArgs);
                    spinnerService.Show();
                    advanceSearchVM.UserId = loginState.UserDetail.UserId;
                    var response = await legalLegislationService.GetLegalLegislationsForPublish(advanceSearchVM);
                    if (response.IsSuccessStatusCode)
                    {
                        FilteredLegalLegislations = LegalLegislationsForPublish = (IEnumerable<LegalLegislationsVM>)response.ResultData;
                        count = LegalLegislationsForPublish.Count();
                        if (!string.IsNullOrEmpty(search)) await OnSearchInput(search);
                        if (!string.IsNullOrEmpty(dataArgs.OrderBy) && string.IsNullOrEmpty(search))
                        {
                            FilteredLegalLegislations = await gridSearchExtension.Sort(FilteredLegalLegislations, ColumnName, SortOrder);
                        }
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
            if (advanceSearchVM.PageSize != null && advanceSearchVM.PageSize != CurrentPageSize)
            {
                int oldPageCount = LegalLegislationsForPublish.Any() ? (LegalLegislationsForPublish.First().TotalCount) / ((int)advanceSearchVM.PageSize) : 1;
                int oldPageNumber = (int)advanceSearchVM.PageNumber - 1;
                advanceSearchVM.isGridLoaded = true;
                advanceSearchVM.PageNumber = CurrentPage;
                advanceSearchVM.PageSize = args.Top;
                int TotalPages = LegalLegislationsForPublish.Any() ? (LegalLegislationsForPublish.First().TotalCount) / (grid.PageSize) : 1;
                if (CurrentPage > TotalPages)
                {
                    advanceSearchVM.PageNumber = TotalPages + 1;
                    advanceSearchVM.PageSize = args.Top;
                    grid.CurrentPage = TotalPages;
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
        private async Task OnSortData(DataGridColumnSortEventArgs<LegalLegislationsVM> args)
        {
            if (args.SortOrder != null)
            {
                FilteredLegalLegislations = await gridSearchExtension.Sort(FilteredLegalLegislations, args.Column.Property, (SortOrder)args.SortOrder);
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
        //<History Author = 'Nabeel ur Rehman' Date='2022-06-06' Version="1.0" Branch="master"> Redirect to View Detail page</History>

        protected async Task LegislationDescision(LegalLegislationsVM args)
        {
            string LegalPubUnPub = "3";
            navigationManager.NavigateTo("legislation-decision/" + args.LegislationId + "/" + LegalPubUnPub);
        }
        protected async Task ViewLegislationDetail(LegalLegislationsVM args)
        {
            var FromRedirect = "/legallegislationpublishunpublish";
            navigationManager.NavigateTo("legallegislation-detailview/" + args.LegislationId + "/" + FromRedirect);
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

        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredLegalLegislations = await gridSearchExtension.Filter(LegalLegislationsForPublish, new Query()
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
                    FilteredLegalLegislations = await gridSearchExtension.Sort(FilteredLegalLegislations, ColumnName, SortOrder);
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
