using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.CaseManagment.MOJ
{
    //<!-- <History Author = 'Hassan Abbas' Date='2024-02-28' Version="1.0" Branch="master">List for Unassigned MOJ Case Files</History> -->
    public partial class ListUnassignedMigratedCases : ComponentBase
    {
        #region Variables
        protected RadzenDataGrid<MojUnassignedCaseFileVM> grid;
        protected RadzenDataGrid<CmsRegisteredCaseFileDetailVM> gridRegionalCases;
        protected RadzenDataGrid<CmsRegisteredCaseFileDetailVM> gridAppealCases;
        protected RadzenDataGrid<CmsRegisteredCaseFileDetailVM> gridSupremeCases;
        protected RadzenDataGrid<CmsCaseFileVM> gridlinkedFiles;
        protected bool Keywords = false;
        public int selectedIndex { get; set; } = 0;
        public int selectedSubIndex { get; set; } = 0;
        public bool isVisible { get; set; }
        protected AdvanceSearchCmsCaseFileVM advanceSearchVM = new AdvanceSearchCmsCaseFileVM();
        protected AdvanceSearchCmsCaseFileVM args;
        protected List<CaseFileStatus> Statuses { get; set; } = new List<CaseFileStatus>();
        public bool allowRowSelectOnRowClick = true;
        public IList<MojUnassignedCaseFileVM> selectedFiles;
        protected IEnumerable<CmsCaseFileVM> linkedFiles;
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => grid.CurrentPage + 1;
        private int CurrentPageSize => grid.PageSize;
        private Timer debouncer;
        private const int debouncerDelay = 500;
        #endregion

        #region Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            if (grid != null)
                translationState.TranslateGridFilterLabels(grid);
            spinnerService.Hide();
        }
        #endregion

        #region Grid Events

        IEnumerable<MojUnassignedCaseFileVM> FilteredGetRegisteredCasefile;
        protected IEnumerable<MojUnassignedCaseFileVM> getRegisteredCasefile { get; set; } = new List<MojUnassignedCaseFileVM>();

        protected string search { get; set; }


        #region On Load Grid Data
        /*<History Author='Ammaar Naveed' Date='2025-02-05' Version="1.0" Branch="master">Load grid data based on pagination</History>*/
        protected async Task OnLoadData(LoadDataArgs args)
        {
            try
            {
                if (string.IsNullOrEmpty(args.OrderBy) || CurrentPage != advanceSearchVM.PageNumber || CurrentPageSize != advanceSearchVM.PageSize || (Keywords && advanceSearchVM.isDataSorted))
                {
                    if (advanceSearchVM.isGridLoaded && advanceSearchVM.PageSize == CurrentPageSize && !advanceSearchVM.isPageSizeChangeOnFirstLastPage)
                    {
                        grid.CurrentPage = (int)advanceSearchVM.PageNumber - 1;
                        advanceSearchVM.isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(args);
                    spinnerService.Show();
                    var response = await cmsCaseFileService.GetUnassignedMigratedCaseFilesList(advanceSearchVM);
                    if (response.IsSuccessStatusCode)
                    {
                        getRegisteredCasefile = (IEnumerable<MojUnassignedCaseFileVM>)response.ResultData;
                        FilteredGetRegisteredCasefile = (IEnumerable<MojUnassignedCaseFileVM>)response.ResultData;
                        if (!string.IsNullOrEmpty(search)) await OnSearchInput(search);
                        if (!string.IsNullOrEmpty(args.OrderBy) && string.IsNullOrEmpty(search))
                        {
                            FilteredGetRegisteredCasefile = await gridSearchExtension.Sort(FilteredGetRegisteredCasefile, ColumnName, SortOrder);
                        }
                        await InvokeAsync(StateHasChanged);
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                }
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
            spinnerService.Hide();
        }
        #endregion

        #region On Sort Grid Data
        /*<History Author='Ammaar Naveed' Date='2025-02-05' Version="1.0" Branch="master">Sort grid data</History>*/
        private async Task OnSortData(DataGridColumnSortEventArgs<MojUnassignedCaseFileVM> args)
        {
            if (args.SortOrder != null)
            {
                FilteredGetRegisteredCasefile = await gridSearchExtension.Sort(FilteredGetRegisteredCasefile, args.Column.Property, (SortOrder)args.SortOrder);
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

        #region Grid Pagination Calculation
        private void SetPagingProperties(LoadDataArgs args)
        {
            if (advanceSearchVM.PageSize != null && advanceSearchVM.PageSize != CurrentPageSize)
            {
                int oldPageCount = getRegisteredCasefile.Any() ? (getRegisteredCasefile.First().TotalCount) / ((int)advanceSearchVM.PageSize) : 1;
                int oldPageNumber = (int)advanceSearchVM.PageNumber - 1;
                advanceSearchVM.isGridLoaded = true;
                advanceSearchVM.PageNumber = CurrentPage;
                advanceSearchVM.PageSize = args.Top;
                int TotalPages = getRegisteredCasefile.Any() ? (getRegisteredCasefile.First().TotalCount) / (grid.PageSize) : 1;
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
                    FilteredGetRegisteredCasefile = await gridSearchExtension.Filter(getRegisteredCasefile, new Query()
                    {
                        Filter = $@"i => ( i.FileNumber != null && i.FileNumber.ToString().ToLower().Contains(@0) ) || ( i.PriorityNameEn != null && i.PriorityNameEn.ToLower().Contains(@1) ) || ( i.GovernmentEntityNameEn != null && i.GovernmentEntityNameEn.ToLower().Contains(@2) ) ||(i.StatusNameEn!=null && i.StatusNameEn.ToLower().Contains(@3))",
                        FilterParameters = new object[] { search, search, search, search }
                    });
                }
                else
                {
                    FilteredGetRegisteredCasefile = await gridSearchExtension.Filter(getRegisteredCasefile, new Query()
                    {
                        Filter = $@"i => ( i.FileNumber != null && i.FileNumber.ToString().ToLower().Contains(@0) ) || ( i.PriorityNameAr != null && i.PriorityNameAr.ToLower().Contains(@1) )|| ( i.GovernmentEntityNameAr != null && i.GovernmentEntityNameAr.ToLower().Contains(@2) )||(i.StatusNameEn!=null && i.StatusNameEn.ToLower().Contains(@3)",
                        FilterParameters = new object[] { search, search, search, search }
                    });
                }
                if (!string.IsNullOrEmpty(ColumnName))
                    FilteredGetRegisteredCasefile = await gridSearchExtension.Sort(FilteredGetRegisteredCasefile, ColumnName, SortOrder);
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

        #region Advance Search
        //<History Author = 'Hassan Abbas' Date='2022-11-29' Version="1.0" Branch="master"> Advance Search </History>
        protected async Task SubmitAdvanceSearch()
        {
            if (advanceSearchVM.CreatedFrom > advanceSearchVM.CreatedTo)
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
            if (advanceSearchVM.ModifiedFrom > advanceSearchVM.ModifiedTo)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("ModifiedFrom_NotGreater_ModifiedTo"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                Keywords = true;
                return;
            }
            if (string.IsNullOrEmpty(advanceSearchVM.FileNumber) && !advanceSearchVM.CreatedFrom.HasValue && !advanceSearchVM.CreatedTo.HasValue)
            {

            }
            else
            {
                spinnerService.Show();
                Keywords = advanceSearchVM.isDataSorted = true;

                if (grid.CurrentPage > 0)
                    await grid.FirstPage();
                else
                {
                    advanceSearchVM.isGridLoaded = false;
                    await grid.Reload();
                }
                spinnerService.Hide();
                StateHasChanged();
            }
        }

        public async void ResetForm()
        {
            advanceSearchVM = new AdvanceSearchCmsCaseFileVM { PageSize = grid.PageSize };
            Keywords = advanceSearchVM.isDataSorted = false;
            await Task.Delay(100);
            grid.Reset();
            await grid.Reload();
            StateHasChanged();
        }

        //<History Author = 'Hassan Abbas' Date='2022-09-28' Version="1.0" Branch="master">Open advance search popup </History>
        protected async Task ToggleAdvanceSearch()
        {
            isVisible = !isVisible;
            if (!isVisible)
            {
                ResetForm();
            }
        }

        #endregion

        #region GRID Buttons
        //<History Author = 'Hassan Abbas' Date='2024-02-29' Version="1.0" Branch="master"> Redirect </History>
        protected async Task Grid0RowSelect(MojUnassignedCaseFileVM args)
        {
            navigationManager.NavigateTo("/casefile-view/" + args.FileId);
        }

        //<History Author = 'Hassan Abbas' Date='2024-02-29' Version="1.0" Branch="master"> Redirect to Case Detail page</History>
        protected async Task DetailRegisteredCase(CmsRegisteredCaseFileDetailVM args)
        {
            navigationManager.NavigateTo("/case-view/" + args.CaseId);
        }

        //<History Author = 'Hassan Abbas' Date='2024-02-29' Version="1.0" Branch="master"> Assign Selected Files to Sector</History>
        protected async Task AssignToSector()
        {
            var result = await dialogService.OpenAsync<SelectSectorForMojAssignment>(translationState.Translate("Assign_Migrated_CaseFile_To_Sector"),
                  new Dictionary<string, object>()
                  {
                            { "SelectedFileIds", selectedFiles.Select(x => x.FileId).ToList() },
                  },
                  new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true }
              );
            if (result != null)
            {
                selectedFiles = null;
                await grid.Reload();
            }
        }
        public void RowRender(RowRenderEventArgs<CmsRegisteredCaseFileDetailVM> args)
        {
            //if (args.Data.SubcaseCount <= 0)
            //{
            //    args.Attributes.Add("class", "no-subcase");
            //}
        }
        protected void CellRender(RowRenderEventArgs<MojUnassignedCaseFileVM> args)
        {
            if (args.Data.IsAssignedBack == true)
            {
                args.Attributes.Add("style", $"background-color: #e3d4c9;");
            }
        }

        #endregion

        #region Redirect Function
        //<History Author = 'Hassan Abbas' Date='2022-11-29' Version="1.0" Branch="master"> Redirect Function </History>
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion

        #region Load Cases for a File
        //<History Author = 'Hassan Abbas' Date='2022-11-29' Version="1.0" Branch="master"> Load Cases for a File </History>

        protected async Task ExpandCases(MojUnassignedCaseFileVM file)
        {
            var response = await cmsCaseFileService.GetAllRegisteredCasesByFileId(file.FileId);
            if (response.IsSuccessStatusCode)
            {
                file.RegisteredCases = (List<CmsRegisteredCaseFileDetailVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

            if (gridRegionalCases != null)
                translationState.TranslateGridFilterLabels(gridRegionalCases);
            if (gridAppealCases != null)
                translationState.TranslateGridFilterLabels(gridAppealCases);
            if (gridSupremeCases != null)
                translationState.TranslateGridFilterLabels(gridSupremeCases);
            if (gridlinkedFiles != null)
                translationState.TranslateGridFilterLabels(gridlinkedFiles);
        }
        #endregion
    }
}
