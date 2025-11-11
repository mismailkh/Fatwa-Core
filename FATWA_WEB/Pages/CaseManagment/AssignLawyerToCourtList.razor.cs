using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.CaseManagment
{
    //<History Author = 'Hassan Abbas' Date='2022-11-29' Version="1.0" Branch="master"> List of Merge Requests For Cases</History>
    public partial class AssignLawyerToCourtList : ComponentBase
    {
        #region Variables Declaration
        protected RadzenDataGrid<AssignLawyerToCourtVM>? grid = new RadzenDataGrid<AssignLawyerToCourtVM>();
        protected bool Keywords = false;
        public int selectedIndex { get; set; } = 0;
        public int? sectorTypeId { get; set; }
        protected int count { get; set; }
        public bool isVisible { get; set; }
        public IList<LawyerVM> lawyers { get; set; } = new List<LawyerVM>();
        public IList<Court> courts { get; set; } = new List<Court>();
        public List<ChamberNumber> allChamberNumbers { get; set; } = new List<ChamberNumber>();
        public List<Chamber> allChambers { get; set; } = new List<Chamber>();
        public int CourtTypeId { get; set; }
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => grid.CurrentPage + 1;
        private int CurrentPageSize => grid.PageSize;
        protected AdvanceSearchVMAssignLawyerToCourt advanceSearchVM = new AdvanceSearchVMAssignLawyerToCourt();
        IEnumerable<AssignLawyerToCourtVM> GetAssignedLawyerToCourt { get; set; } = new List<AssignLawyerToCourtVM>();
        IEnumerable<AssignLawyerToCourtVM> _FilteredAssignedLawyerToCourt;
        protected IEnumerable<AssignLawyerToCourtVM> FilteredAssignedLawyerToCourt
        {
            get
            {
                return _FilteredAssignedLawyerToCourt;
            }
            set
            {
                if (!object.Equals(_FilteredAssignedLawyerToCourt, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getAssignedLawyerToCourt", NewValue = value, OldValue = _FilteredAssignedLawyerToCourt };
                    _FilteredAssignedLawyerToCourt = value;
                    Reload();
                }
            }
        }
        private Timer debouncer;
        private const int debouncerDelay = 500;
        #endregion

        #region On Initialized
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateLawyers();
            await PopulateCourts();
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
        private async Task OnLoadData(LoadDataArgs args)
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
                    var result = await lookupService.GetAssingedLawyerToCourt(advanceSearchVM);
                    if (result.IsSuccessStatusCode)
                    {
                        FilteredAssignedLawyerToCourt = GetAssignedLawyerToCourt = (IEnumerable<AssignLawyerToCourtVM>)result.ResultData;
                        if (!string.IsNullOrEmpty(search)) await OnSearchInput(search);
                        if (!string.IsNullOrEmpty(args.OrderBy) && string.IsNullOrEmpty(search))
                        {
                            FilteredAssignedLawyerToCourt = await gridSearchExtension.Sort(FilteredAssignedLawyerToCourt, ColumnName, SortOrder);
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
            if (advanceSearchVM.PageSize != null && advanceSearchVM.PageSize != CurrentPageSize)
            {
                int oldPageCount = GetAssignedLawyerToCourt.Any() ? (GetAssignedLawyerToCourt.First().TotalCount) / ((int)advanceSearchVM.PageSize) : 1;
                int oldPageNumber = (int)advanceSearchVM.PageNumber - 1;
                advanceSearchVM.isGridLoaded = true;
                advanceSearchVM.PageNumber = CurrentPage;
                advanceSearchVM.PageSize = args.Top;
                int TotalPages = GetAssignedLawyerToCourt.Any() ? (GetAssignedLawyerToCourt.First().TotalCount) / (grid.PageSize) : 1;
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
        /*<History Author='Ammaar Naveed' Date='2025-02-02'>Sort grid data</History>*/
        private async Task OnSortData(DataGridColumnSortEventArgs<AssignLawyerToCourtVM> args)
        {
            if (args.SortOrder != null)
            {
                FilteredAssignedLawyerToCourt = await gridSearchExtension.Sort(FilteredAssignedLawyerToCourt, args.Column.Property, (SortOrder)args.SortOrder);
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

        #region Advance Search
        protected void ToggleAdvanceSearch()
        {
            isVisible = !isVisible;
            if (!isVisible)
            {
                ResetForm();
            }
        }
        protected async Task SubmitAdvanceSearch()
        {
            if (string.IsNullOrEmpty(advanceSearchVM.LawyerName) && !advanceSearchVM.CourtName.HasValue && !advanceSearchVM.ChamberName.HasValue && !advanceSearchVM.ChamberNumber.HasValue)
            {
                //Do Nothing
            }
            else
            {
                spinnerService.Show();
                Keywords = advanceSearchVM.isDataSorted = true;
                if (grid.CurrentPage > 0)
                { 
                    await grid.FirstPage();
                }
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
            advanceSearchVM = new AdvanceSearchVMAssignLawyerToCourt { PageSize = grid.PageSize };
            Keywords = advanceSearchVM.isDataSorted = false;
            await Task.Delay(100);
            grid.Reset();
            await grid.Reload();
            StateHasChanged();
        }


        #region Advance Search Dropdown Data  
        protected async Task PopulateLawyers()
        {
            try
            {
                var userresponse = await lookupService.GetLawyersBySector(loginState.UserDetail.SectorTypeId);
                if (userresponse.IsSuccessStatusCode)
                {
                    lawyers = (IList<LawyerVM>)userresponse.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(userresponse);
                }
                StateHasChanged();
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
        protected async Task PopulateCourts()
        {
            try
            {
                if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialRegionalCases || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeRegionalCases)
                {
                    CourtTypeId = (int)CourtTypeEnum.Regional;
                }
                else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeAppealCases || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialAppealCases)
                {
                    CourtTypeId = (int)CourtTypeEnum.Appeal;
                }
                else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeSupremeCases || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialSupremeCases)
                {
                    CourtTypeId = (int)CourtTypeEnum.Supreme;
                }
                else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases)
                {
                    //CourtTypeId = (int)CourtTypeEnum.PartialUrgent;
                }
                var response = await lookupService.GetCourt();
                if (response.IsSuccessStatusCode)
                {
                    courts = (List<Court>)response.ResultData;
                    courts = courts.Where(c => c.TypeId == CourtTypeId).ToList();
                    advanceSearchVM.CourtType = CourtTypeId;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                StateHasChanged();
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
        protected async Task PopulateChambers()
        {
            try
            {
                if (advanceSearchVM.CourtName > 0)
                {
                    var response = await lookupService.GetChamber();
                    if (response.IsSuccessStatusCode)
                    {
                        allChambers = (List<Chamber>)response.ResultData;
                        allChambers = allChambers.Where(x => x.SelectedCourtIds.Contains((int)advanceSearchVM.CourtName)).ToList();
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    StateHasChanged();
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
        public async Task PopulateChamberNumbers()
        {
            try
            {
                if (advanceSearchVM.ChamberName.HasValue && advanceSearchVM.ChamberName.Value > 0)
                {
                    var response = await lookupService.GetChamberNumbersByChamberId(0);
                    if (response.IsSuccessStatusCode)
                    {
                        allChamberNumbers = (List<ChamberNumber>)response.ResultData;
                        allChamberNumbers = allChamberNumbers.Where(x => x.ChamberIds.Contains(advanceSearchVM.ChamberName.Value)).ToList();
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
                    FilteredAssignedLawyerToCourt = await gridSearchExtension.Filter(GetAssignedLawyerToCourt, new Query()
                {
                    Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US" ?   
                    $@"i => ( i.LawyerFullNameEn != null && i.LawyerFullNameEn.ToString().ToLower().Contains(@0)) || 
                    (i.ChamberNumber != null && i.ChamberNumber.ToString().ToLower().Contains(@1)) ||
                    (i.CourtNameEn != null && i.CourtNameEn.ToString().ToLower().Contains(@2)) ||
                    (i.ChamberNameEn != null && i.ChamberNameEn.ToString().ToLower().Contains(@3))" 
                    :
                    $@"i => ( i.LawyerFullNameAr != null && i.LawyerFullNameAr.ToString().ToLower().Contains(@0))
                    || (i.ChamberNumber != null && i.ChamberNumber.ToString().ToLower().Contains(@1))
                    (i.CourtNameAr != null && i.CourtNameAr.ToString().ToLower().Contains(@2)) ||
                    (i.ChamberNameAr != null && i.ChamberNameAr.ToString().ToLower().Contains(@3))",
                    FilterParameters = new object[] { search, search, search, search }
                });
                if (!string.IsNullOrEmpty(ColumnName))
                    FilteredAssignedLawyerToCourt = await gridSearchExtension.Sort(FilteredAssignedLawyerToCourt, ColumnName, SortOrder);
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

        #region GRID Buttons
        
        //<History Author = 'Ijaz' Date='2022-11-29' Version="1.0" Branch="master"> Redirect to View Detail page</History>
        protected async Task Delete(AssignLawyerToCourtVM args)
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Delete_Assign_Lawyer_To_Court"), translationState.Translate("delete"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                var response = await lookupService.DeleteAssignLawyerToCourt(args);
                if (response.IsSuccessStatusCode)
                {
                   
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Delete_Assign_Lawyer_To_Court_Success"),
                        Style = "position:fixed !important;left: 0; margin: auto;"
                    });
                    grid.Reset();
                    await grid.Reload();
                    StateHasChanged();
                }
            }
            //navigationState.ReturnUrl = "merge-requests";
            //navigationManager.NavigateTo("merge-request-detail/" + args.Id);
        }
        //<History Author = 'Hassan Abbas' Date='2022-03-18' Version="1.0" Branch="master"> Redirect to Add book wizard</History>
        protected async Task ButtonAddClick(MouseEventArgs args)
        {
            navigationManager.NavigateTo("/AssignLawyerToCourt");
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
    }
}
