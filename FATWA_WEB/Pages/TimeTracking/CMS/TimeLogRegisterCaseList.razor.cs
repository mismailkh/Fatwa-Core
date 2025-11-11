using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.TimeTrackingVM;
using FATWA_WEB.Services;
using FATWA_WEB.Shared;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using System.Globalization;
using System.Text;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_GENERAL.Helper.Enum;


namespace FATWA_WEB.Pages.TimeTracking.CMS
{
    public partial class TimeLogRegisterCaseList : ComponentBase
    {
        #region Variables Declaration
        protected RadzenDataGrid<CmsCaseFileVM>? grid;
        IEnumerable<CmsCaseFileVM> FilterefGetCasefiles { get; set; }
        protected IEnumerable<CmsCaseFileVM> getCasefiles { get; set; } = new List<CmsCaseFileVM>();
        protected bool Keywords = false;
        public bool isVisible { get; set; }
        public bool isVisibleTimeTracking { get; set; }
        protected AdvanceSearchCmsCaseFileVM advanceSearchVM = new AdvanceSearchCmsCaseFileVM();
        protected AdvanceSearchCmsCaseFileVM args;
        protected List<CaseFileStatus> Statuses { get; set; } = new List<CaseFileStatus>();
        public bool allowRowSelectOnRowClick = true;
        public IList<CmsCaseFileVM> selectedFiles;
        protected string search { get; set; }
        protected int pendingRequestsCount { get; set; }
        protected int pendingFilesCount { get; set; }
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => grid.CurrentPage + 1;
        private int CurrentPageSize => grid.PageSize;

        #endregion

        #region On Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateSectorTypes();
            await PopulateCaseFileStatuses();
            translationState.TranslateGridFilterLabels(grid);
            spinnerService.Hide();
        }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        #endregion

        #region On Load Grid Data 
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
                    var response = await cmsCaseFileService.GetCmsCaseFile(advanceSearchVM);
                 
                    if (response.IsSuccessStatusCode)
                    {
                        getCasefiles = (IEnumerable<CmsCaseFileVM>)response.ResultData;
                        FilterefGetCasefiles = (IEnumerable<CmsCaseFileVM>)response.ResultData;
                        if (!(string.IsNullOrEmpty(search))) await OnSearchInput();
                        if (!(string.IsNullOrEmpty(args.OrderBy)) && (string.IsNullOrEmpty(search)))
                        {
                            FilterefGetCasefiles = await gridSearchExtension.Sort(FilterefGetCasefiles, ColumnName, SortOrder);
                        }
                        await InvokeAsync(StateHasChanged);
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    await PopulateGovtEntities();
                    await PopulateTaskStatuses();
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
            spinnerService.Hide();
        }
        #endregion

        #region Grid Pagination Calculation
        private void SetPagingProperties(LoadDataArgs args)
        {
            if (advanceSearchVM.PageSize != null && advanceSearchVM.PageSize != CurrentPageSize)
            {
                int oldPageCount = getCasefiles.Any() ? (getCasefiles.First().TotalCount) / ((int)advanceSearchVM.PageSize) : 1;
                int oldPageNumber = (int)advanceSearchVM.PageNumber - 1;
                advanceSearchVM.isGridLoaded = true;
                advanceSearchVM.PageNumber = CurrentPage;
                advanceSearchVM.PageSize = args.Top;
                int TotalPages = getCasefiles.Any() ? (getCasefiles.First().TotalCount) / (grid.PageSize) : 1;
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
        private async Task OnSort(DataGridColumnSortEventArgs<CmsCaseFileVM> args)
        {
            if (args.SortOrder != null)
            {
                FilterefGetCasefiles = await gridSearchExtension.Sort(FilterefGetCasefiles, args.Column.Property, (SortOrder)args.SortOrder);
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

        #region Grid Search
        protected async Task OnSearchInput()
        {
            try
            {
                search = string.IsNullOrEmpty(search) ? "" : search.TrimStart().TrimEnd().ToLower();

                if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
                {
                    FilterefGetCasefiles = await gridSearchExtension.Filter(getCasefiles, new Query()
                    {
                        Filter = $@"i => (i.FileNumber != null && i.FileNumber.ToString().ToLower().Contains(@0)) 
                                    || (i.GovernmentEntityNameEn != null && i.GovernmentEntityNameEn.ToLower().Contains(@1)) 
                                    || (i.StatusNameEn != null && i.StatusNameEn.ToString().Replace(""  "","" "").ToLower().Contains(@2))
                                    || (i.LastActionEn != null && i.LastActionEn.ToString().ToLower().Contains(@3))",
                        FilterParameters = new object[] { RemoveDiacritics(search), RemoveDiacritics(search), RemoveDiacritics(search), search }
                    });
                }
                else
                {
                    FilterefGetCasefiles = await gridSearchExtension.Filter(getCasefiles, new Query()
                    {
                        Filter = $@"i => ( i.FileNumber != null && i.FileNumber.ToString().ToLower().Contains(@0)) 
                                    || (i.GovernmentEntityNameAr != null && i.GovernmentEntityNameAr.ToLower().Contains(@1)) 
                                    || (i.StatusNameAr != null && i.StatusNameAr.ToString().Replace(""  "","" "").ToLower().Contains(@2))
                                    || (i.LastActionAr != null && i.LastActionAr.ToString().ToLower().Contains(@3))",
                        FilterParameters = new object[] { search, search, search, search }
                    });
                }
                if (!string.IsNullOrEmpty(ColumnName))
                    FilterefGetCasefiles = await gridSearchExtension.Sort(FilterefGetCasefiles, ColumnName, SortOrder);
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
        protected string RemoveDiacritics(string text)
        {
            // Use CultureInfo.InvariantCulture for consistent comparison
            CultureInfo culture = CultureInfo.InvariantCulture;

            // Remove diacritic marks from the text
            return new string(
                text.Normalize(NormalizationForm.FormD)
                    .Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                    .ToArray()
            ).Normalize(NormalizationForm.FormC);
        }
        #endregion

        #region Remote Dropdown Data and Dropdown Change Events
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Populate Subtypes data</History>
        protected async Task PopulateSectorTypes()
        {
            advanceSearchVM.SectorTypeId = loginState.UserDetail.SectorTypeId;
            advanceSearchVM.UserId = loginState.UserDetail.UserId;
        }
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Populate Subtypes data</History>

        protected async Task PopulateCaseFileStatuses()
        {
            var response = await lookupService.GetCaseFileStatuses();
            if (response.IsSuccessStatusCode)
            {
                Statuses = (List<CaseFileStatus>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        protected void CellRender(RowRenderEventArgs<CmsCaseFileVM> args)
        {
            if (args.Data.IsAssignedBack == true)
            {
                args.Attributes.Add("style", $"background-color: #e3d4c9;");
            }
        }
        #endregion

        #region Advance Search
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
            if (string.IsNullOrEmpty(advanceSearchVM.FileNumber)
                && !advanceSearchVM.StatusId.HasValue && !advanceSearchVM.GovEntityId.HasValue
                && !advanceSearchVM.CreatedFrom.HasValue && !advanceSearchVM.CreatedTo.HasValue
                && !advanceSearchVM.ModifiedFrom.HasValue && !advanceSearchVM.ModifiedTo.HasValue) { }
            else
            {
                Keywords = advanceSearchVM.isDataSorted = true;
                if (grid.CurrentPage > 0)
                    await grid.FirstPage();
                else
                {
                    advanceSearchVM.isGridLoaded = false;
                    await grid.Reload();
                }
            }
        }
        public async void ResetForm()
        {
            advanceSearchVM = new AdvanceSearchCmsCaseFileVM { PageSize = grid.PageSize };
            Keywords = advanceSearchVM.isDataSorted = false;
            await PopulateSectorTypes();
            grid.Reset();
            await grid.Reload();
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

        protected List<GovernmentEntity> GovtEntities = new List<GovernmentEntity>();
        protected async Task PopulateGovtEntities()
        {
            var response = await lookupService.GetGovernmentEntities();
            if (response.IsSuccessStatusCode)
            {
                GovtEntities = (List<GovernmentEntity>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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

        #region TimeTracking
        public int count { get; set; } = 0;
        public string roleId;
        protected TimeTrackingAdvanceSearchVM advanceSearchTimeTrackingVM = new TimeTrackingAdvanceSearchVM();
        IEnumerable<TimeTrackingVM> FilterefGetTimeTracking { get; set; } = new List<TimeTrackingVM>();
        protected List<UserTaskStatus> TaskStatuses = new List<UserTaskStatus>();
        IEnumerable<TimeTrackingVM> _getTimeTrackingList;
        protected IEnumerable<TimeTrackingVM> getTimeTrackingList
        {
            get
            {
                return _getTimeTrackingList;
            }
            set
            {
                if (!Equals(_getTimeTrackingList, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getTimeTrackingList", NewValue = value, OldValue = _getTimeTrackingList };
                    _getTimeTrackingList = value;
                    Reload();
                }
            }
        }
        protected async Task ExpandCaseTimelog(Guid? ReferenceId)
        {
            advanceSearchTimeTrackingVM.SectortypeId = (int)loginState.UserDetail.SectorTypeId;
            roleId = loginState.UserDetail.RoleId;
            if (SystemRoles.HOS == roleId || SystemRoles.ComsHOS == roleId || loginState.UserRoles.Any(r => SystemRoles.ViceHOS.Contains(r.RoleId)))
            {
                advanceSearchTimeTrackingVM.UserId = "";
                advanceSearchTimeTrackingVM.UserName = loginState.UserDetail.UserName;
            }
            else
            {
                advanceSearchTimeTrackingVM.UserId = loginState.UserDetail.UserId;
            }

            advanceSearchTimeTrackingVM.ReferenceId = ReferenceId ?? Guid.Empty;
            if (loginState.UserRoles.Any(r => SystemRoles.CaseRoles.Contains(r.RoleId)) || loginState.UserRoles.Any(r => SystemRoles.ViceHOS.Contains(r.RoleId)))
            {
                advanceSearchTimeTrackingVM.ModuleId = (int)ModuleEnum.CaseManagement;
            }
            else if (loginState.UserRoles.Any(r => SystemRoles.ConsultationRoles.Contains(r.RoleId)))
            {
                advanceSearchTimeTrackingVM.ModuleId = (int)ModuleEnum.ConsultationManagement;
            }

            search = string.IsNullOrEmpty(search) ? "" : search.ToLower();
            List<TimeTrackingVM> timeTrackingVMs = new List<TimeTrackingVM>();
            var result = await timeTrackingService.GetTimeTrackingList(advanceSearchTimeTrackingVM);
            if (result.IsSuccessStatusCode)
            {
                getTimeTrackingList = (IEnumerable<TimeTrackingVM>)result.ResultData;
                foreach (var item in getTimeTrackingList)
                {
                    item.ActivityName = translationState.Translate(item.ActivityName);
                    timeTrackingVMs.Add(item);
                }
                getTimeTrackingList = timeTrackingVMs;
                FilterefGetTimeTracking = (IEnumerable<TimeTrackingVM>)result.ResultData;
                count = getTimeTrackingList.Count();
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
            //await gridTimeTracking.Reload();
            //StateHasChanged();
        }
        protected async Task PopulateTaskStatuses()
        {
            try
            {
                var response = await lookupService.GetTaskStatuses();
                if (response.IsSuccessStatusCode)
                {
                    TaskStatuses = (List<UserTaskStatus>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception)
            {
                throw;
            }

        }

        #endregion  
    }
}
