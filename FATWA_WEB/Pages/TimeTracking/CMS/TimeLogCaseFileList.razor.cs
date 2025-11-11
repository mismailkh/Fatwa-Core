using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_WEB.Pages.CaseManagment.Files;
using FATWA_WEB.Pages.CaseManagment.RegisteredCase;
using FATWA_WEB.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.TimeTracking.CMS
{
    public partial class TimeLogCaseFileList : ComponentBase
    {
        #region Variables Declaration
        protected RadzenDataGrid<RegisteredCaseFileVM> grid = new RadzenDataGrid<RegisteredCaseFileVM>();
        protected IEnumerable<RegisteredCaseFileVM> FilteredGetRegisteredCasefile;
        protected IEnumerable<RegisteredCaseFileVM> getRegisteredCasefile { get; set; } = new List<RegisteredCaseFileVM>();
        protected RadzenDataGrid<CmsCaseFileVM> gridlinkedFiles;
        protected bool Keywords = false;
        public int selectedIndex { get; set; } = 0;
        public int selectedSubIndex { get; set; } = 0;
        public bool isVisible { get; set; }
        protected AdvanceSearchCmsCaseFileVM advanceSearchVM = new AdvanceSearchCmsCaseFileVM();
        protected AdvanceSearchCmsCaseFileVM args;
        protected List<CaseFileStatus> Statuses { get; set; } = new List<CaseFileStatus>();
        protected List<CmsRegisteredCaseVM> RegisteredCases { get; set; } = new List<CmsRegisteredCaseVM>();
        public bool allowRowSelectOnRowClick = true;
        public IList<RegisteredCaseFileVM> selectedFiles;
        public IList<CmsRegisteredCaseVM> selectedCases;
        protected int pendingRequestsCount { get; set; }
        protected int pendingFilesCount { get; set; }
        protected CmsRegisteredCaseVM caseregistered { get; set; }
        protected IEnumerable<CmsCaseFileVM> linkedFiles;
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => grid.CurrentPage + 1;
        private int CurrentPageSize => grid.PageSize;

        protected string search { get; set; }
        #endregion

        #region On Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateSectorTypes();
            await PopulateCaseFileStatuses();
            await PopulateGovtEntities();
            if (grid != null)
                translationState.TranslateGridFilterLabels(grid);
            spinnerService.Hide();
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

                    advanceSearchVM.SectorTypeId = loginState.UserDetail.SectorTypeId;
                    advanceSearchVM.UserId = loginState.UserDetail.UserId;
                    var response = await cmsCaseFileService.GetRegisteredCaseFile(advanceSearchVM);
                    spinnerService.Hide();
                    if (response.IsSuccessStatusCode)
                    {
                        getRegisteredCasefile = (IEnumerable<RegisteredCaseFileVM>)response.ResultData;
                        FilteredGetRegisteredCasefile = (IEnumerable<RegisteredCaseFileVM>)response.ResultData;
                        if (!(string.IsNullOrEmpty(search))) await OnSearchInput();
                        if (!(string.IsNullOrEmpty(args.OrderBy)) && (string.IsNullOrEmpty(search)))
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

        #region On Sort Grid Data
        private async Task OnSort(DataGridColumnSortEventArgs<RegisteredCaseFileVM> args)
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

        #region Grid Search
        protected async Task OnSearchInput()
        {
            try
            {
                search = string.IsNullOrEmpty(search) ? "" : search.TrimStart().TrimEnd().ToLower();

                FilteredGetRegisteredCasefile = await gridSearchExtension.Filter(getRegisteredCasefile, new Query()
                {
                    Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US"
                    ? $@"i => (i.FileNumber != null && i.FileNumber.ToString().ToLower().Contains(@0)) 
                            || (i.GovernmentEntityNameEn != null && i.GovernmentEntityNameEn.ToLower().Contains(@1))
                            || (i.StatusNameEn != null && i.StatusNameEn.Replace(""  "","" "").ToLower().Contains(@2))
                            || (i.LastActionEn != null && i.LastActionEn.ToLower().Contains(@3))"

                    : $@"i => (i.FileNumber != null && i.FileNumber.ToString().ToLower().Contains(@0)) 
                            || (i.GovernmentEntityNameAr != null && i.GovernmentEntityNameAr.ToLower().Contains(@1))
                            || (i.StatusNameAr != null && i.StatusNameAr.Replace(""  "","" "").ToLower().Contains(@2))
                            || (i.LastActionAr != null && i.LastActionAr.ToLower().Contains(@3))",
                    FilterParameters = new object[] { search, search, search, search }
                });
                if (!string.IsNullOrEmpty(ColumnName))
                    FilteredGetRegisteredCasefile = await gridSearchExtension.Sort(FilteredGetRegisteredCasefile, ColumnName, SortOrder);
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

        #region Remote Dropdown Data and Dropdown Change Events
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Populate Subtypes data</History>
        protected async Task PopulateSectorTypes()
        {
            advanceSearchVM.SectorTypeId = loginState.UserDetail.SectorTypeId;
            advanceSearchVM.UserId = loginState.UserDetail.UserId;
        }
        //<History Author = 'Danish' Date='2022-11-29' Version="1.0" Branch="master"> Populate Subtypes data </History>
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
        #endregion

        #region Advance Search
        //<History Author = 'Danish' Date='2022-11-29' Version="1.0" Branch="master"> Advance Search </History>
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
                && !advanceSearchVM.StatusId.HasValue && !advanceSearchVM.GovEntityId.HasValue) { }
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
        #region Populate Government Entities
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
                invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

        #endregion

        #region GRID Buttons
        //<History Author = 'Danish' Date='2022-11-29' Version="1.0" Branch="master"> Redirect </History>
        protected async Task Grid0RowSelect(RegisteredCaseFileVM args)
        {
            navigationManager.NavigateTo("/casefile-view/" + args.FileId);
        }

        //<History Author = 'Danish' Date='2022-10-29' Version="1.0" Branch="master"> Redirect to Case Detail page</History>
        protected async Task DetailRegisteredCase(CmsRegisteredCaseVM args)
        {
            navigationManager.NavigateTo("/case-view/" + args.CaseId);
        }
        protected async Task DetailJudgmentDecision(CmsJugdmentDecisionVM args)
        {
            navigationManager.NavigateTo("/judgmentdecision-detail/" + args.Id);
        }

        public void RowRender(RowRenderEventArgs<CmsRegisteredCaseVM> args)
        {
            if (args.Data.SubcaseCount <= 0)
            {
                args.Attributes.Add("class", "no-subcase");
            }
        }
        protected void CellRender(RowRenderEventArgs<RegisteredCaseFileVM> args)
        {
            if (args.Data.IsAssignedBack == true)
            {
                args.Attributes.Add("style", $"background-color: #e3d4c9;");
            }
        }

        #endregion

        #region Redirect Function
        //<History Author = 'Danish' Date='2022-11-29' Version="1.0" Branch="master"> Redirect Function </History>

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
        //<History Author = 'Danish' Date='2022-11-29' Version="1.0" Branch="master"> Load Cases for a File </History>

        //protected async Task ExpandCases(RegisteredCaseFileVM file)
        //{
        //    var response = await cmsCaseFileService.GetAllRegisteredCasesByFileId(file.FileId);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        file.RegisteredCases = (List<CmsRegisteredCaseFileDetailVM>)response.ResultData;
        //    }
        //    else
        //    {
        //        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
        //    }

        //    await PopulateLinkedFiles(file.FileId);
        //    if (gridRegionalCases != null)
        //        translationState.TranslateGridFilterLabels(gridRegionalCases);
        //    if (gridAppealCases != null)
        //        translationState.TranslateGridFilterLabels(gridAppealCases);
        //    if (gridSupremeCases != null)
        //        translationState.TranslateGridFilterLabels(gridSupremeCases);
        //    if (gridlinkedFiles != null)
        //        translationState.TranslateGridFilterLabels(gridlinkedFiles);
        //}
        #endregion

        #region Load SubCases for a Case
        //<History Author = 'Nabeel ur Rehman' Date='2022-11-29' Version="1.0" Branch="master"> Load  SubCases for a File </History>

        protected async Task ExpandSubCases(CmsRegisteredCaseVM subcase)
        {
            var response = await cmsRegisteredCaseService.GetSubcasesByCase(subcase.CaseId);
            if (response.IsSuccessStatusCode)
            {
                subcase.RegisteredCases = (List<CmsRegisteredCaseVM>)response.ResultData;
            }
            var responseJudgment = await cmsRegisteredCaseService.GetJudgmentDecision(subcase.CaseId);
            if (responseJudgment.IsSuccessStatusCode)
            {
                subcase.jugdmentDecisionVMs = (List<CmsJugdmentDecisionVM>)responseJudgment.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

        }
        #endregion

        #region Merge Cases

        //<History Author = 'Hassan Abbas' Date='2022-09-28' Version="1.0" Branch="master">Merge Cases </History>
        protected async Task MergeCases(MouseEventArgs args)
        {
            try
            {
                if (selectedFiles != null && selectedFiles.Any())
                {
                    await dialogService.OpenAsync<SelectCasesToMergePopup>(translationState.Translate("Select_Cases_Merge"),
                        new Dictionary<string, object>()
                        {
                            { "Files", selectedFiles },
                        },
                        new DialogOptions() { Width = "80% !important", CloseDialogOnOverlayClick = true }
                    );
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
        }
        #endregion

        #region Link Case Files

        //<History Author = 'Hassan Abbas' Date='2022-09-28' Version="1.0" Branch="master">Link Case Files </History>
        protected async Task LinkFiles(MouseEventArgs args)
        {
            try
            {
                if (selectedFiles != null && selectedFiles.Any())
                {
                    var result = await dialogService.OpenAsync<SelectPrimaryFileForLinkingPopup>(translationState.Translate("Select_Primary_File"),
                        new Dictionary<string, object>()
                        {
                            { "Files", selectedFiles },
                            { "IsUnderFile", false },
                        },
                        new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true }
                    );

                    if (result != null)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Files_Linked_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        await grid.Reload();
                        selectedFiles = null;
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
        }

        public async Task PopulateLinkedFiles(Guid fileId)
        {
            var response = await cmsCaseFileService.GetLinkedFilesByPrimaryFileId(fileId);
            if (response.IsSuccessStatusCode)
            {
                linkedFiles = (List<CmsCaseFileVM>)response.ResultData;

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

        #region Link CANs

        //<History Author = 'Hassan Abbas' Date='2022-09-28' Version="1.0" Branch="master">Link CANs </History>
        protected async Task LinkCANs(MouseEventArgs args)
        {
            try
            {
                if (selectedFiles != null && selectedFiles.Any())
                {
                    var result = await dialogService.OpenAsync<SelectPrimaryCANForLinkingPopup>(translationState.Translate("Select_Primary_CAN"),
                        new Dictionary<string, object>()
                        {
                            { "Files", selectedFiles },
                        },
                        new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true }
                    );

                    if (result != null)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("CANs_Linked_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        await grid.Reload();
                        selectedFiles = null;
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
        }
        #endregion
    }
}
