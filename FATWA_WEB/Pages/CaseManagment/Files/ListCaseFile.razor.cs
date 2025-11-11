using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_WEB.Pages.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using System.Globalization;
using System.Text;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;

namespace FATWA_WEB.Pages.CaseManagment.Files
{
    public partial class ListCaseFile : ComponentBase
    {

        #region Variables Declaration
        protected RadzenDataGrid<CmsCaseFileVM>? grid = new RadzenDataGrid<CmsCaseFileVM>();
        protected RadzenDataGrid<CmsDraftedRequestListVM>? gridDraftedRequests = new RadzenDataGrid<CmsDraftedRequestListVM>();
        IEnumerable<CmsCaseFileVM> FilterefGetCasefiles { get; set; }
        protected IEnumerable<CmsCaseFileVM> getCasefiles = new List<CmsCaseFileVM>();
        IEnumerable<CmsDraftedRequestListVM> filteredDraftedRequestList { get; set; }
        protected IEnumerable<CmsDraftedRequestListVM> cmsDraftedRequestList = new List<CmsDraftedRequestListVM>();
        protected bool Keywords = false;
        public bool isVisible { get; set; }
        public bool isdraftedVisible { get; set; }
        protected AdvanceSearchCmsCaseFileVM advanceSearchVM = new AdvanceSearchCmsCaseFileVM();
        protected AdvanceSearchCmsCaseRequestVM advanceSearchdraftedRequestVM = new AdvanceSearchCmsCaseRequestVM();
        protected List<CaseFileStatus> Statuses { get; set; } = new List<CaseFileStatus>();
        public bool allowRowSelectOnRowClick = true;
        protected int pendingRequestsCount { get; set; }
        protected int pendingFilesCount { get; set; }
        public IList<CmsCaseFileVM> selectedFiles;
        protected List<GovernmentEntity> GovtEntities { get; set; } = new List<GovernmentEntity>();
        protected string search { get; set; }
        private int SelectedTabIndex = 0;
        private int CurrentPage => SelectedTabIndex == 0 ? grid.CurrentPage + 1 : gridDraftedRequests.CurrentPage + 1;
        private int CurrentPageSize => SelectedTabIndex == 0 ? grid.PageSize : gridDraftedRequests.PageSize;
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        #endregion

        #region On Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateCaseFileStatuses();
            await PopulateGovtEntities();
            advanceSearchVM.PageSize = systemSettingState.Grid_Pagination;
            advanceSearchdraftedRequestVM.PageSize = systemSettingState.Grid_Pagination;
            translationState.TranslateGridFilterLabels(grid);
            translationState.TranslateGridFilterLabels(gridDraftedRequests);
            spinnerService.Hide();
        }
        #endregion

        #region CMS Case Files

        #region On Load Case Files Data
        protected async Task OnLoadCaseFilesData(LoadDataArgs args)
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
                    spinnerService.Show();
                    SetPagingProperties(args, getCasefiles.Any() ? getCasefiles.First().TotalCount : 0, grid);
                    advanceSearchVM.SectorTypeId = loginState.UserDetail.SectorTypeId;
                    advanceSearchVM.UserId = loginState.UserDetail.UserId;
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

        #region On Sort Case Files Data
        private async Task OnSort(DataGridColumnSortEventArgs<CmsCaseFileVM> args)
        {
            if (args.SortOrder != null)
            {
                FilterefGetCasefiles = await gridSearchExtension.Sort(FilterefGetCasefiles, args.Column.Property, (SortOrder)args.SortOrder);
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
        protected async Task OnSearchInput()
        {
            try
            {
                search = string.IsNullOrEmpty(search) ? "" : search.TrimStart().TrimEnd().ToLower();

                FilterefGetCasefiles = await gridSearchExtension.Filter(getCasefiles, new Query()
                {
                    Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US"
                    ? $@"i => (i.FileNumber != null && i.FileNumber.ToString().ToLower().Contains(@0)) 
                            || (i.GovernmentEntityNameEn != null && i.GovernmentEntityNameEn.ToLower().Contains(@1)) 
                            || (i.Subject != null && i.Subject.ToString().ToLower().Contains(@2))
                            || (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@3))
                            || (i.StatusNameEn != null && i.StatusNameEn.ToString().ToLower().Contains(@4))
                            || (i.LastActionEn != null && i.LastActionEn.ToString().ToLower().Contains(@5))"

                    : $@"i => (i.FileNumber != null && i.FileNumber.ToString().ToLower().Contains(@0)) 
                            || (i.GovernmentEntityNameAr != null && i.GovernmentEntityNameAr.ToLower().Contains(@1)) 
                            || (i.Subject != null && i.Subject.ToString().ToLower().Contains(@2))
                            || (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@3))
                            || (i.StatusNameAr != null && i.StatusNameAr.ToString().ToLower().Contains(@4))
                            || (i.LastActionAr != null && i.LastActionAr.ToString().ToLower().Contains(@5))",

                    FilterParameters = new object[] { RemoveDiacritics(search), RemoveDiacritics(search), RemoveDiacritics(search), RemoveDiacritics(search), RemoveDiacritics(search), RemoveDiacritics(search) }
                });
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
        #endregion

        #endregion

        #region CMS Drafted Files

        #region On Load Drafted Requests Data
        protected async Task OnLoadDataDraftedRequest(LoadDataArgs args)
        {
            try
            {
                if (string.IsNullOrEmpty(args.OrderBy) || CurrentPage != advanceSearchdraftedRequestVM.PageNumber || CurrentPageSize != advanceSearchdraftedRequestVM.PageSize || (Keywords && advanceSearchdraftedRequestVM.isDataSorted))
                {
                    if (advanceSearchdraftedRequestVM.isGridLoaded && advanceSearchdraftedRequestVM.PageSize == CurrentPageSize && !advanceSearchdraftedRequestVM.isPageSizeChangeOnFirstLastPage)
                    {
                        grid.CurrentPage = (int)advanceSearchdraftedRequestVM.PageNumber - 1;
                        advanceSearchdraftedRequestVM.isGridLoaded = false;
                        return;
                    }
                    spinnerService.Show();
                    SetPagingProperties(args, cmsDraftedRequestList.Any() ? cmsDraftedRequestList.First().TotalCount : 0, gridDraftedRequests);
                    advanceSearchdraftedRequestVM.StatusId = (int)CaseRequestStatusEnum.Draft;
                    advanceSearchdraftedRequestVM.CreatedBy = loginState.Username;
                    var response = await cmsCaseFileService.GetDraftedCaseRequestList(advanceSearchdraftedRequestVM);
                    if (response.IsSuccessStatusCode)
                    {
                        cmsDraftedRequestList = (List<CmsDraftedRequestListVM>)response.ResultData;
                        filteredDraftedRequestList = (List<CmsDraftedRequestListVM>)response.ResultData;
                        if (!(string.IsNullOrEmpty(search))) await OnSearchInput();
                        if (!(string.IsNullOrEmpty(args.OrderBy)) && (string.IsNullOrEmpty(search)))
                        {
                            FilterefGetCasefiles = await gridSearchExtension.Sort(FilterefGetCasefiles, ColumnName, SortOrder);
                        }
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
            spinnerService.Hide();
        }
        #endregion
        #region Grid Pagination Calculation
        private void SetPagingProperties<T>(LoadDataArgs args, int ListTotalCount, RadzenDataGrid<T> grid)
        {
            if (SelectedTabIndex == 0)
            {
                if (advanceSearchVM.PageSize != null && advanceSearchVM.PageSize != CurrentPageSize)
                {
                    int oldPageCount = ListTotalCount > 0 ? (ListTotalCount) / ((int)advanceSearchVM.PageSize) : 1;
                    int oldPageNumber = (int)advanceSearchVM.PageNumber - 1;
                    advanceSearchVM.isGridLoaded = true;
                    advanceSearchVM.PageNumber = CurrentPage;
                    advanceSearchVM.PageSize = args.Top;
                    int TotalPages = ListTotalCount > 0 ? (ListTotalCount) / (grid.PageSize) : 1;
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
            else
            {
                if (advanceSearchdraftedRequestVM.PageSize != null && advanceSearchdraftedRequestVM.PageSize != CurrentPageSize)
                {
                    int oldPageCount = ListTotalCount > 0 ? (ListTotalCount) / ((int)advanceSearchdraftedRequestVM.PageSize) : 1;
                    int oldPageNumber = (int)advanceSearchdraftedRequestVM.PageNumber - 1;
                    advanceSearchdraftedRequestVM.isGridLoaded = true;
                    advanceSearchdraftedRequestVM.PageNumber = CurrentPage;
                    advanceSearchdraftedRequestVM.PageSize = args.Top;
                    int TotalPages = ListTotalCount > 0 ? (ListTotalCount) / (grid.PageSize) : 1;
                    if (CurrentPage > TotalPages)
                    {
                        advanceSearchdraftedRequestVM.PageNumber = TotalPages + 1;
                        advanceSearchdraftedRequestVM.PageSize = args.Top;
                        grid.CurrentPage = TotalPages;
                    }
                    if ((advanceSearchdraftedRequestVM.PageNumber == 1 || (advanceSearchdraftedRequestVM.PageNumber == TotalPages + 1 && oldPageCount == oldPageNumber)) && oldPageCount != 0)
                    {
                        advanceSearchdraftedRequestVM.isPageSizeChangeOnFirstLastPage = true;
                    }
                    else
                    {
                        advanceSearchdraftedRequestVM.isPageSizeChangeOnFirstLastPage = false;
                    }
                    return;
                }
                advanceSearchdraftedRequestVM.PageNumber = CurrentPage;
                advanceSearchdraftedRequestVM.PageSize = args.Top;
            }
        }
        #endregion

        #region On Sort Drafted Requests Data
        private async Task OnSortDraftedRequest(DataGridColumnSortEventArgs<CmsDraftedRequestListVM> args)
        {
            if (args.SortOrder != null)
            {
                filteredDraftedRequestList = await gridSearchExtension.Sort(filteredDraftedRequestList, args.Column.Property, (SortOrder)args.SortOrder);
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
        protected async Task OnSearchInputDraftedFiles()
        {
            try
            {
                search = string.IsNullOrEmpty(search) ? "" : search.TrimStart().TrimEnd().ToLower();

                filteredDraftedRequestList = await gridSearchExtension.Filter(cmsDraftedRequestList, new Query()
                {
                    Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US"
                ? $@"i => (i.RequestNumber != null && i.RequestNumber.ToString().ToLower().Contains(@0)) 
                            || (i.GovtEntityEn != null && i.GovtEntityEn.ToLower().Contains(@1)) 
                            || (i.Subject != null && i.Subject.ToString().ToLower().Contains(@2))
                            || (i.RequestDate.HasValue && i.RequestDate.Value.ToString(""dd/MM/yyyy"").Contains(@3))
                            || (i.Status_Name_En != null && i.Status_Name_En.ToString().ToLower().Contains(@4))
                            || (i.LastActionEn != null && i.LastActionEn.ToString().ToLower().Contains(@5))"

                : $@"i => (i.RequestNumber != null && i.RequestNumber.ToString().ToLower().Contains(@0)) 
                            || (i.GovtEntityAr != null && i.GovtEntityAr.ToLower().Contains(@1)) 
                            || (i.Subject != null && i.Subject.ToString().ToLower().Contains(@2))
                            || (i.RequestDate.HasValue && i.RequestDate.Value.ToString(""dd/MM/yyyy"").Contains(@3))
                            || (i.Status_Name_Ar != null && i.Status_Name_Ar.ToString().ToLower().Contains(@4))
                            || (i.LastActionAr != null && i.LastActionAr.ToString().ToLower().Contains(@5))",

                    FilterParameters = new object[] { RemoveDiacritics(search), RemoveDiacritics(search), RemoveDiacritics(search), RemoveDiacritics(search), RemoveDiacritics(search), RemoveDiacritics(search) }
                });
                if (!string.IsNullOrEmpty(ColumnName))
                    filteredDraftedRequestList = await gridSearchExtension.Sort(filteredDraftedRequestList, ColumnName, SortOrder);
            }
            catch
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

        #region Advance Searches

        #region CMS Case Files Advance Search
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
            if (string.IsNullOrEmpty(advanceSearchVM.FileNumber) && !advanceSearchVM.StatusId.HasValue
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
                StateHasChanged();
            }
        }
        public async void ResetForm()
        {
            advanceSearchVM = new AdvanceSearchCmsCaseFileVM { PageSize = gridDraftedRequests.PageSize };
            Keywords = advanceSearchdraftedRequestVM.isDataSorted = false;
            await Task.Delay(100);
            grid.Reset();
            await grid.Reload();
            StateHasChanged();
        }
        protected async Task ToggleAdvanceSearch()
        {
            isVisible = !isVisible;
            if (!isVisible)
            {
                ResetForm();

            }
        }
        #endregion

        #region Drafted Requests Advance Search
        protected async Task SubmitDraftedRequestAdvanceSearch()
        {
            if (advanceSearchdraftedRequestVM.RequestFrom > advanceSearchdraftedRequestVM.RequestTo)
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

            if (string.IsNullOrEmpty(advanceSearchdraftedRequestVM.RequestNumber) && !advanceSearchdraftedRequestVM.StatusId.HasValue
                && !advanceSearchdraftedRequestVM.RequestFrom.HasValue && !advanceSearchdraftedRequestVM.RequestTo.HasValue) { }
            else
            {
                Keywords = advanceSearchdraftedRequestVM.isDataSorted = true;
                if (gridDraftedRequests.CurrentPage > 0)
                    await gridDraftedRequests.FirstPage();
                else
                {
                    advanceSearchdraftedRequestVM.isGridLoaded = false;
                    await gridDraftedRequests.Reload();
                }
                StateHasChanged();
            }
        }
        public async void ResetDraftedListForm()
        {
            advanceSearchdraftedRequestVM = new AdvanceSearchCmsCaseRequestVM { PageSize = gridDraftedRequests.PageSize };
            Keywords = advanceSearchdraftedRequestVM.isDataSorted = false;
            await Task.Delay(100);
            gridDraftedRequests.Reset();
            await gridDraftedRequests.Reload();
            StateHasChanged();
        }
        //<History Author = 'Hassan Abbas' Date='2022-09-28' Version="1.0" Branch="master">Open advance search popup </History>
        protected async Task ToggleDraftedListAdvanceSearch()
        {
            isdraftedVisible = !isdraftedVisible;
            if (!isdraftedVisible)
            {
                ResetDraftedListForm();
            }
        }
        #endregion

        #endregion

        #region Remote Dropdown Data and Dropdown Change Events
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Populate Subtypes data</History>

        protected async Task PopulateCaseFileStatuses()
        {
            try
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
        protected async Task PopulateGovtEntities()
        {
            try
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
        #endregion

        #region GRID Buttons
        //<History Author = 'Nabeel ur Rehman' Date='2022-06-06' Version="1.0" Branch="master"> Redirect to View Detail page</History>
        protected async Task Grid0RowSelect(CmsCaseFileVM args)
        {

            navigationManager.NavigateTo("/casefile-view/" + args.FileId);
        }
        protected async Task CaseRequestDetail(CmsCaseFileVM arg)
        {
            navigationManager.NavigateTo("/caserequest-view/" + arg.RequestId);
        }
        protected async Task DraftedCaseRequestDetail(CmsDraftedRequestListVM arg)
        {
            navigationManager.NavigateTo("/caserequest-view/" + arg.RequestId);
        }
        protected async Task EditRequest(CmsDraftedRequestListVM arg)
        {
            navigationManager.NavigateTo("/create-casefile/" + arg.RequestId);
        }
        protected void CellRender(RowRenderEventArgs<CmsCaseFileVM> args)
        {
            if (args.Data.IsAssignedBack == true)
            {
                args.Attributes.Add("style", $"background-color: #e3d4c9;");
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Redirect to Case Detail page</History>
        //protected async Task DetailRegisteredCase(RegisterdRequestVM args)
        //{
        //    navigationManager.NavigateTo("detail-registeredcase/" + args.CaseId);
        //}
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

        #region AddCaseFile

        //<History Author = 'Hassan Abbas' Date='2022-02-28' Version="1.0" Branch="master"> Redirect to Add case request wizard</History>
        protected async Task CreateCaseFile(MouseEventArgs args)
        {
            var result = await dialogService.OpenAsync<FileUpload>(translationState.Translate("Select_Document"),
            new Dictionary<string, object>()
            {
                { "IsViewOnly", false },
                { "IsUploadPopup", true },
                { "FileTypes", new List<string>() { ".pdf" } },
                { "MaxFileSize", systemSettingState.File_Maximum_Size },
                { "Multiple", false },
                { "IsCaseRequest", true },
                { "IsRequestTypeSelection", true },
                { "RequestTypeId", loginState.UserDetail.SectorTypeId <= (int)OperatingSectorTypeEnum.AdministrativeSupremeCases ? (int)RequestTypeEnum.Administrative : (int)RequestTypeEnum.CivilCommercial  }
            }
            ,
            new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
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
                            { "IsUnderFile", true },
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
                        grid.Reset();
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
                    Detail = translationState.Translate("Literature_Delete_Failed"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion

        #region Remove Diacraite
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

        #region On Tab Change
        protected async Task OnTabChange(int index)
        {
            if (index == SelectedTabIndex) { return; }
            search = ColumnName = string.Empty;
            SelectedTabIndex = index;
            await Task.Delay(100);
            isVisible = isdraftedVisible = Keywords = false;
            if (SelectedTabIndex == 0)
            {
                advanceSearchVM = new AdvanceSearchCmsCaseFileVM { PageSize = grid.PageSize };
                grid.Reset();
                await grid.Reload();
            }
            else
            {
                if (gridDraftedRequests != null)
                {
                    advanceSearchdraftedRequestVM = new AdvanceSearchCmsCaseRequestVM { PageSize = gridDraftedRequests.PageSize };
                    gridDraftedRequests.Reset();
                    await gridDraftedRequests.Reload();

                }
            }
        }

        #endregion

        #region Get Draft Case Request List
        protected async Task GetDraftedCaeRequestList()
        {
            try
            {
                advanceSearchdraftedRequestVM.StatusId = (int)CaseRequestStatusEnum.Draft;
                advanceSearchdraftedRequestVM.CreatedBy = loginState.Username;
                var response = await cmsCaseFileService.GetDraftedCaseRequestList(advanceSearchdraftedRequestVM);
                if (response.IsSuccessStatusCode)
                {
                    cmsDraftedRequestList = (List<CmsDraftedRequestListVM>)response.ResultData;
                    filteredDraftedRequestList = (List<CmsDraftedRequestListVM>)response.ResultData;
                    await InvokeAsync(StateHasChanged);


                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                await gridDraftedRequests.Reload();
                StateHasChanged();
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
