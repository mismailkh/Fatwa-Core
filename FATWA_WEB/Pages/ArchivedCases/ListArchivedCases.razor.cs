using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.ArchivedCases;
using FATWA_WEB.Pages.ArchivedCases.Dialogs;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using static FATWA_GENERAL.Helper.Response;
using FATWA_WEB.Pages.ArchivedCases.Dialogs;
using Microsoft.AspNetCore.Components.Web;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.ContactManagmentVMs;

namespace FATWA_WEB.Pages.ArchivedCases
{
    public partial class ListArchivedCases : ComponentBase
    {

        #region Variables Declarations
        protected RadzenDataGrid<ArchivedCaseListVM>? grid = new RadzenDataGrid<ArchivedCaseListVM>();
        protected IEnumerable<ArchivedCaseListVM> ArchivedCaseList { get; set; } = new List<ArchivedCaseListVM>();
        protected List<ArchivedCaseListVM> FilteredArchivedCaseListVM { get; set; }
        private string _search;
        IEnumerable<ArchivedCaseListVM> _getArchivedcase;
        public bool isVisible { get; set; }
        ArchivedCaseAdvanceSearchVM ArchivedCaseAdvanceSearchVM = new ArchivedCaseAdvanceSearchVM();
        protected bool Keywords { get; set; }
        public List<Chamber> Chambers { get; set; } = new List<Chamber>();
        public List<ChamberNumber> ChamberNumbers { get; set; } = new List<ChamberNumber>();
        public IList<CourtType> CourtTypes { get; set; } = new List<CourtType>();

        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => grid.CurrentPage + 1;
        private int CurrentPageSize => grid.PageSize;
        private Timer debouncer;
        private const int debouncerDelay = 500;
        #endregion

        #region On Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            isVisible = true;
            ArchivedCaseAdvanceSearchVM.PageSize = systemSettingState.Grid_Pagination;
            translationState.TranslateGridFilterLabels(grid);
            await ToggleAdvanceSearch();
            spinnerService.Hide();
        }
        #endregion

        #region On Load Grid Data
        /*<History Author='Ammaar Naveed' Date='2025-02-03' Version="1.0" Branch="master">Fetch grid data based on pagination</History>*/
        protected async Task OnLoadData(LoadDataArgs args)
        {
            if (!(string.IsNullOrEmpty(ArchivedCaseAdvanceSearchVM.CaseAutomatedNumber) &&
                 string.IsNullOrEmpty(ArchivedCaseAdvanceSearchVM.CaseNumber) &&
                 (ArchivedCaseAdvanceSearchVM.ChamberTypeId == null || ArchivedCaseAdvanceSearchVM.ChamberTypeId == 0) &&
                 (ArchivedCaseAdvanceSearchVM.ChamberNumberId == null || ArchivedCaseAdvanceSearchVM.ChamberNumberId == 0) &&
                 (ArchivedCaseAdvanceSearchVM.CourtTypeId == null || ArchivedCaseAdvanceSearchVM.CourtTypeId == 0) &&
                 string.IsNullOrEmpty(ArchivedCaseAdvanceSearchVM.PlaintiffName) &&
                 string.IsNullOrEmpty(ArchivedCaseAdvanceSearchVM.DefendantName)))
            {
                if (string.IsNullOrEmpty(args.OrderBy) || CurrentPage != ArchivedCaseAdvanceSearchVM.PageNumber || CurrentPageSize != ArchivedCaseAdvanceSearchVM.PageSize || (Keywords && ArchivedCaseAdvanceSearchVM.isDataSorted))
                {
                    try
                    {
                        if (ArchivedCaseAdvanceSearchVM.isGridLoaded && ArchivedCaseAdvanceSearchVM.PageSize == CurrentPageSize && !ArchivedCaseAdvanceSearchVM.isPageSizeChangeOnFirstLastPage)
                        {
                            grid.CurrentPage = (int)ArchivedCaseAdvanceSearchVM.PageNumber - 1;
                            ArchivedCaseAdvanceSearchVM.isGridLoaded = false;
                            return;
                        }
                        SetPagingProperties(args);
                        var response = await archivedCasesService.GetArchivedCaseList(ArchivedCaseAdvanceSearchVM);
                        if (response.IsSuccessStatusCode)
                        {
                            ArchivedCaseList = (IEnumerable<ArchivedCaseListVM>)response.ResultData;
                            FilteredArchivedCaseListVM = (List<ArchivedCaseListVM>)ArchivedCaseList;
                            if (!string.IsNullOrEmpty(search)) await OnSearchInput(search);
                            if (!string.IsNullOrEmpty(args.OrderBy) && string.IsNullOrEmpty(search))
                            {
                                FilteredArchivedCaseListVM = await gridSearchExtension.Sort(FilteredArchivedCaseListVM, ColumnName, SortOrder);
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
                }
            }
        }
        private void SetPagingProperties(LoadDataArgs args)
        {
            if (ArchivedCaseAdvanceSearchVM.PageSize != null && ArchivedCaseAdvanceSearchVM.PageSize != CurrentPageSize)
            {
                int oldPageCount = ArchivedCaseList.Any() ? (ArchivedCaseList.First().TotalCount) / ((int)ArchivedCaseAdvanceSearchVM.PageSize) : 1;
                int oldPageNumber = (int)ArchivedCaseAdvanceSearchVM.PageNumber - 1;
                ArchivedCaseAdvanceSearchVM.isGridLoaded = true;
                ArchivedCaseAdvanceSearchVM.PageNumber = CurrentPage;
                ArchivedCaseAdvanceSearchVM.PageSize = args.Top;
                int TotalPages = ArchivedCaseList.Any() ? (ArchivedCaseList.First().TotalCount) / (grid.PageSize) : 1;
                if (CurrentPage > TotalPages)
                {
                    ArchivedCaseAdvanceSearchVM.PageNumber = TotalPages + 1;
                    ArchivedCaseAdvanceSearchVM.PageSize = args.Top;
                    grid.CurrentPage = TotalPages;
                }
                if ((ArchivedCaseAdvanceSearchVM.PageNumber == 1 || (ArchivedCaseAdvanceSearchVM.PageNumber == TotalPages + 1 && oldPageCount == oldPageNumber)) && oldPageCount != 0)
                {
                    ArchivedCaseAdvanceSearchVM.isPageSizeChangeOnFirstLastPage = true;
                }
                else
                {
                    ArchivedCaseAdvanceSearchVM.isPageSizeChangeOnFirstLastPage = false;
                }
                return;
            }
            ArchivedCaseAdvanceSearchVM.PageNumber = CurrentPage;
            ArchivedCaseAdvanceSearchVM.PageSize = args.Top;
        }
        #endregion

        #region On Sort Grid Data
        /*<History Author='Ammaar Naveed' Date='2025-02-03' Version="1.0" Branch="master">Sort grid data</History>*/
        private async Task OnSortData(DataGridColumnSortEventArgs<ArchivedCaseListVM> args)
        {
            if (args.SortOrder != null)
            {
                FilteredArchivedCaseListVM = await gridSearchExtension.Sort(FilteredArchivedCaseListVM, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
                ArchivedCaseAdvanceSearchVM.isDataSorted = false;
            }
            else
            {
                ColumnName = string.Empty;
            }
        }
        #endregion

        #region Advance Search
        protected async Task ToggleAdvanceSearch()
        {
            await PopulateCourtTypes();
            await PopulateChambers();
            await PopulateChamberNumbers();
            if (!isVisible)
            {
                ResetForm();
            }
        }
        public async void ResetForm()
        {
            ArchivedCaseAdvanceSearchVM = new ArchivedCaseAdvanceSearchVM { PageSize = grid.PageSize };
            Keywords = ArchivedCaseAdvanceSearchVM.isDataSorted = false;
            await Task.Delay(100);
            grid.Reset();
            await grid.Reload();
            StateHasChanged();
        }
        protected async Task SubmitAdvanceSearch()
        {
            if (string.IsNullOrEmpty(ArchivedCaseAdvanceSearchVM.CaseAutomatedNumber) &&
                string.IsNullOrEmpty(ArchivedCaseAdvanceSearchVM.CaseNumber) &&
                (ArchivedCaseAdvanceSearchVM.ChamberTypeId == null || ArchivedCaseAdvanceSearchVM.ChamberTypeId == 0) &&
                (ArchivedCaseAdvanceSearchVM.ChamberNumberId == null || ArchivedCaseAdvanceSearchVM.ChamberNumberId == 0) &&
                (ArchivedCaseAdvanceSearchVM.CourtTypeId == null || ArchivedCaseAdvanceSearchVM.CourtTypeId == 0) &&
                string.IsNullOrEmpty(ArchivedCaseAdvanceSearchVM.PlaintiffName) &&
                string.IsNullOrEmpty(ArchivedCaseAdvanceSearchVM.DefendantName))
            {
            }
            else
            {
                spinnerService.Show();
                Keywords = ArchivedCaseAdvanceSearchVM.isDataSorted = true;
                if (grid.CurrentPage > 0)
                {
                    await grid.FirstPage();
                }
                else
                {
                    ArchivedCaseAdvanceSearchVM.isGridLoaded = false;
                    await grid.Reload();
                }
                spinnerService.Hide();
                StateHasChanged();
            }
        }
        #endregion

        #region Populate Dropdowns
        protected async Task PopulateCourtTypes()
        {
            var response = await lookupService.GetCourtType();
            if (response.IsSuccessStatusCode)
            {
                CourtTypes = (List<CourtType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task PopulateChambers()
        {
            var response = await lookupService.GetChamber();
            if (response.IsSuccessStatusCode)
            {
                Chambers = (List<Chamber>)response.ResultData;
                ArchivedCaseAdvanceSearchVM.ChamberTypeId = 0;
                ArchivedCaseAdvanceSearchVM.ChamberNumberId = 0;

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        public async Task PopulateChamberNumbers()
        {
            var response = await lookupService.GetChamberNumber();
            if (response.IsSuccessStatusCode)
            {
                ChamberNumbers = (List<ChamberNumber>)response.ResultData;
                ArchivedCaseAdvanceSearchVM.ChamberNumberId = 0;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion        

        #region Archived Case View Action
        protected async Task OpenDetailArchivedCaseDialog(MouseEventArgs args, Guid caseId)
        {
            await dialogService.OpenAsync<DetailArchivedCaseDialog>(
                            translationState.Translate("Archived_Case_Detail"),
                            new Dictionary<string, object>()
                            {{"CaseId", caseId }},
                            new DialogOptions()
                            {
                                Width = "80% !important",
                                CloseDialogOnOverlayClick = true,
                            });
        }
        #endregion

        #region Search
        protected string search { get; set; }
         
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilteredArchivedCaseListVM = await gridSearchExtension.Filter(ArchivedCaseList, new Query()
                {
                    Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? $@"i => (i.CaseNumber != null && i.CaseNumber.ToString().Contains(@0)) || (i.CaseAutomatedNumber != null && i.CaseAutomatedNumber.ToString().ToLower().Contains(@1)) || (i.CourtNameEn != null && i.CourtNameEn.ToString().ToLower().Contains(@2)) || (i.ChamberNameEn != null && i.ChamberNameEn.ToString().ToLower().Contains(@3))"
                             : $@"i => (i.CaseNumber != null && i.CaseNumber.ToString().Contains(@0)) || (i.CaseAutomatedNumber != null && i.CaseAutomatedNumber.ToString().ToLower().Contains(@1)) || (i.CourtNameAr != null && i.CourtNameAr.ToString().ToLower().Contains(@2)) || (i.ChamberNameAr != null && i.ChamberNameAr.ToString().ToLower().Contains(@3)) ",

                    FilterParameters = new object[] { search, search, search, search }
                });
                if (!string.IsNullOrEmpty(ColumnName))
                    FilteredArchivedCaseListVM = await gridSearchExtension.Sort(FilteredArchivedCaseListVM, ColumnName, SortOrder);
                    await InvokeAsync(StateHasChanged);
                }, null, debouncerDelay, Timeout.Infinite);
            }
            catch (Exception)
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

        #region Redirect Functions
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
        protected async Task ViewDetail(ArchivedCaseListVM archivedcase)
        {
            InvokeAsync(StateHasChanged);
        }
        #endregion

    }
}
