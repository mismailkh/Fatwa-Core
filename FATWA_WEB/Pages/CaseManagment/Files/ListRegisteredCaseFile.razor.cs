using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Extensions;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.TimeTrackingVM;
using FATWA_WEB.Pages.CaseManagment.RegisteredCase;
using FATWA_WEB.Pages.CaseManagment.Shared;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_GENERAL.Helper.Enum;

namespace FATWA_WEB.Pages.CaseManagment.Files
{
    public partial class ListRegisteredCaseFile : ComponentBase
    {
        #region Parameters
        [Parameter]
        public bool IsViewOnly { get; set; } = false;
        [Parameter]
        public RegisteredCaseFileVM SelectedFile { get; set; }
        [Parameter]
        public EventCallback<RegisteredCaseFileVM> SelectedFileChanged { get; set; }
        private async Task OnSelectedFilesChanged(RegisteredCaseFileVM newSelectedFile)
        {
            if (IsViewOnly == true)
            {
                SelectedFile = newSelectedFile;
                await SelectedFileChanged.InvokeAsync(SelectedFile);
            }
        }
        #endregion

        #region Variables Declaration
        protected RadzenDataGrid<RegisteredCaseFileVM> grid = new RadzenDataGrid<RegisteredCaseFileVM>();
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
        protected List<CmsRegisteredCaseFileDetailVM> RegisteredCases { get; set; } = new List<CmsRegisteredCaseFileDetailVM>();
        protected List<GovernmentEntity> GovtEntities { get; set; } = new List<GovernmentEntity>();
        public IList<Court> Courts { get; set; } = new List<Court>();
        public IList<ChamberNumber> ChamberNumbers { get; set; } = new List<ChamberNumber>();
        public IList<Chamber> Chambers { get; set; } = new List<Chamber>();
        public bool allowRowSelectOnRowClick = true;
        public IList<RegisteredCaseFileVM> selectedFiles;
        public IList<CmsRegisteredCaseVM> selectedCases;
        protected int pendingRequestsCount { get; set; }
        protected int pendingFilesCount { get; set; }
        protected CmsRegisteredCaseVM caseregistered { get; set; }
        protected IEnumerable<CmsCaseFileVM> linkedFiles;
        public IList<LawyerVM> lawyers { get; set; } = new List<LawyerVM>();
        protected string search { get; set; }
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => grid.CurrentPage + 1;
        private int CurrentPageSize => grid.PageSize;
        #endregion

        #region On Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateCaseFileStatuses();
            await PopulateGovernmentEntities();
            await PopulateCourts();
            await PopulateLawyers();
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
                    if (response.IsSuccessStatusCode)
                    {
                        getRegisteredCasefile = (IEnumerable<RegisteredCaseFileVM>)response.ResultData;
                        FilteredGetRegisteredCasefile = (IEnumerable<RegisteredCaseFileVM>)response.ResultData;
                        if (!(string.IsNullOrEmpty(search))) await OnSearchInput();
                        if (!(string.IsNullOrEmpty(args.OrderBy)) && (string.IsNullOrEmpty(search)))
                        {
                            FilteredGetRegisteredCasefile = await gridSearchExtension.Sort(FilteredGetRegisteredCasefile, ColumnName, SortOrder);
                        }
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    StateHasChanged();
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

        #region Grid Search
        protected async Task OnSearchInput()
        {
            try
            {
                search = string.IsNullOrEmpty(search) ? "" : search.TrimStart().TrimEnd().ToLower();

                FilteredGetRegisteredCasefile = await gridSearchExtension.Filter(getRegisteredCasefile, new Query()
                {
                    Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US"
                    ? $@"i => ( i.FileNumber != null && i.FileNumber.ToString().ToLower().Contains(@0)) 
                        || ( i.PriorityNameEn != null && i.PriorityNameEn.ToLower().Contains(@1)) 
                        || ( i.GovernmentEntityNameEn != null && i.GovernmentEntityNameEn.ToLower().Contains(@2)) 
                        || (i.StatusNameEn !=null && i.StatusNameEn.ToLower().Contains(@3)) 
                        || (i.LastActionEn !=null && i.LastActionEn.ToLower().Contains(@4))
                        || (i.CreatedDate !=null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@5))"

                    : $@"i => ( i.FileNumber != null && i.FileNumber.ToString().ToLower().Contains(@0) ) 
                        || ( i.PriorityNameAr != null && i.PriorityNameAr.ToLower().Contains(@1) )
                        || ( i.GovernmentEntityNameAr != null && i.GovernmentEntityNameAr.ToLower().Contains(@2) )
                        ||(i.StatusNameEn!=null && i.StatusNameEn.ToLower().Contains(@3))
                        || (i.LastActionAr!=null && i.LastActionAr.ToLower().Contains(@4))
                        || (i.CreatedDate !=null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@5))",
                    FilterParameters = new object[] { search, search, search, search, search, search }
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

        #region Grid Events

        IEnumerable<RegisteredCaseFileVM> _getRegisteredCasefile;
        IEnumerable<RegisteredCaseFileVM> FilteredGetRegisteredCasefile;
        protected IEnumerable<RegisteredCaseFileVM> getRegisteredCasefile
        {
            get
            {
                return _getRegisteredCasefile;
            }
            set
            {
                if (!Equals(_getRegisteredCasefile, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getRegisteredCasefile", NewValue = value, OldValue = _getRegisteredCasefile };
                    _getRegisteredCasefile = value;

                    Reload();
                }

            }
        }

        IEnumerable<CmsRegisteredCaseVM> _getRegisteredCase;
        protected IEnumerable<CmsRegisteredCaseVM> getRegisteredCase
        {
            get
            {
                return _getRegisteredCase;
            }
            set
            {
                if (!Equals(_getRegisteredCasefile, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getRegisteredCase", NewValue = value, OldValue = _getRegisteredCase };
                    _getRegisteredCase = value;

                    Reload();
                }

            }
        }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public async void OnSubTabChange(int index)
        {
            search = "";
            selectedSubIndex = index;
            if (index == 0)
            {
            }

        }
        #endregion

        #region Remote Dropdown Data and Dropdown Change Events
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
        protected async Task PopulateGovernmentEntities()
        {
            var govtEntityResponse = await lookupService.GetGovernmentEntities();
            if (govtEntityResponse.IsSuccessStatusCode)
            {
                GovtEntities = (List<GovernmentEntity>)govtEntityResponse.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(govtEntityResponse);
            }
        }
        protected async Task PopulateCourts()
        {
            int courtTypeId = 0;
            if (loginState?.UserDetail?.SectorTypeId != null)
            {
                courtTypeId = CaseConsultationExtension.GetCourtTypeIdBasedOnSectorId((int)loginState.UserDetail.SectorTypeId);
            }
            var response = await lookupService.GetCourt();
            if (response.IsSuccessStatusCode)
            {
                Courts = (List<Court>)response.ResultData;
                Courts = Courts.Where(c => c.TypeId == courtTypeId).ToList();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task OnChangeCourt()
        {
            try
            {
                advanceSearchVM.ChamberId = null;
                advanceSearchVM.ChamberNumberId = null;
                var response = await lookupService.GetChamberByCourtId(advanceSearchVM.CourtId != null ? (int)advanceSearchVM.CourtId : 0);
                if (response.IsSuccessStatusCode)
                {
                    Chambers = (List<Chamber>)response.ResultData;
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


        protected async Task OnChangeChamber()
        {
            try
            {
                advanceSearchVM.ChamberNumberId = null;
                var response = await lookupService.GetChamberNumbersByChamberId(advanceSearchVM.ChamberId != null ? (int)advanceSearchVM.ChamberId : 0);
                if (response.IsSuccessStatusCode)
                {
                    ChamberNumbers = (List<ChamberNumber>)response.ResultData;
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
            if (string.IsNullOrEmpty(advanceSearchVM.FileNumber)
                && string.IsNullOrEmpty(advanceSearchVM.CANNumber)
                && string.IsNullOrEmpty(advanceSearchVM.CaseNumber)
                && !advanceSearchVM.StatusId.HasValue
                && !advanceSearchVM.CreatedFrom.HasValue && !advanceSearchVM.CreatedTo.HasValue
                && !advanceSearchVM.GovEntityId.HasValue && !advanceSearchVM.CourtId.HasValue
                && !advanceSearchVM.ChamberId.HasValue && !advanceSearchVM.ChamberNumberId.HasValue
                && string.IsNullOrEmpty(advanceSearchVM.LawyerId)
                && string.IsNullOrEmpty(advanceSearchVM.PlaintiffName)
                && string.IsNullOrEmpty(advanceSearchVM.DefendantName)
                && !advanceSearchVM.IsImpportant
                && !advanceSearchVM.isFinalJudgment)
            {

            }
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
            advanceSearchVM = new AdvanceSearchCmsCaseFileVM { PageSize = grid.PageSize };
            Keywords = advanceSearchVM.isDataSorted = false;
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

        #endregion

        #region GRID Buttons
        //<History Author = 'Danish' Date='2022-11-29' Version="1.0" Branch="master"> Redirect </History>
        protected async Task Grid0RowSelect(RegisteredCaseFileVM args)
        {
            navigationManager.NavigateTo("/casefile-view/" + args.FileId);
        }

        //<History Author = 'Danish' Date='2022-10-29' Version="1.0" Branch="master"> Redirect to Case Detail page</History>
        protected async Task DetailRegisteredCase(CmsRegisteredCaseFileDetailVM args)
        {
            navigationManager.NavigateTo("/case-view/" + args.CaseId);
        }
        protected async Task DetailJudgmentDecision(CmsJugdmentDecisionVM args)
        {
            navigationManager.NavigateTo("/judgmentdecision-detail/" + args.Id);
        }
        protected async Task LegalNotificationList(CmsRegisteredCaseFileDetailVM args)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<ListLegalNotification>(

                translationState.Translate("Legal_Notifications"),
                 new Dictionary<string, object>()
                 {
                     { "ReferenceId", args.CaseId  },
                     { "SubModuleId", (int)SubModuleEnum.RegisteredCase },
                 },
               new DialogOptions() { Width = "40 !important", CloseDialogOnOverlayClick = true, });
                await Task.Delay(200);
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        public void RowRender(RowRenderEventArgs<CmsRegisteredCaseFileDetailVM> args)
        {
            //if (args.Data.SubcaseCount <= 0)
            //{
            //    args.Attributes.Add("class", "no-subcase");
            //}
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

        protected async Task ExpandCases(RegisteredCaseFileVM file)
        {
            var response = await cmsCaseFileService.GetAllRegisteredCasesByFileId(file.FileId, advanceSearchVM.isFinalJudgment);
            if (response.IsSuccessStatusCode)
            {
                file.RegisteredCases = (List<CmsRegisteredCaseFileDetailVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

            await PopulateLinkedFiles(file.FileId);
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

        #region Load SubCases for a Case
        //<History Author = 'Nabeel ur Rehman' Date='2022-11-29' Version="1.0" Branch="master"> Load  SubCases for a File </History>

        protected async Task ExpandSubCases(CmsRegisteredCaseFileDetailVM subcase)
        {
            var response = await cmsRegisteredCaseService.GetSubcasesByCase(subcase.CaseId);
            if (response.IsSuccessStatusCode)
            {
                subcase.RegisteredCaseFileDetails = (List<CmsRegisteredCaseVM>)response.ResultData;
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

        #region TimeTracking
        public int countTimeTrackingGrid { get; set; } = 0;
        public string roleId;
        protected TimeTrackingAdvanceSearchVM advanceSearchTimeTrackingVM = new TimeTrackingAdvanceSearchVM();
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
        protected async Task ExpandCaseFileTimelog(Guid? ReferenceId)
        {
            advanceSearchTimeTrackingVM.SectortypeId = (int)loginState.UserDetail.SectorTypeId;
            if (SystemRoles.HOS == roleId || SystemRoles.ComsHOS == roleId)
            {
                advanceSearchTimeTrackingVM.UserId = "";
            }
            else
            {
                advanceSearchTimeTrackingVM.UserId = loginState.UserDetail.UserId;
            }

            advanceSearchTimeTrackingVM.ReferenceId = ReferenceId ?? Guid.Empty;
            if (loginState.UserRoles.Any(r => SystemRoles.CaseRoles.Contains(r.RoleId)))
            {
                advanceSearchTimeTrackingVM.ModuleId = (int)ModuleEnum.CaseManagement;
            }
            else if (loginState.UserRoles.Any(r => SystemRoles.ConsultationRoles.Contains(r.RoleId)))
            {
                advanceSearchTimeTrackingVM.ModuleId = (int)ModuleEnum.ConsultationManagement;
            }

            search = string.IsNullOrEmpty(search) ? "" : search.ToLower();

            var result = await timeTrackingService.GetTimeTracking(advanceSearchTimeTrackingVM, new Query()
            {
                Filter = $@"i => ( ( i.ActivityName != null && i.ActivityName.ToLower().Contains(@0)) ||  ( i.AssignedByEn != null && i.AssignedByEn.ToLower().Contains(@1)) || ( i.AssignedByAr != null && i.AssignedByAr.ToLower().Contains(@2)) || ( i.AssignedByDepartmentNameEn != null && i.AssignedByDepartmentNameEn.ToLower().Contains(@3)) || ( i.AssignedByDepartmentNameAr != null && i.AssignedByDepartmentNameAr.ToLower().Contains(@4)) || ( i.AssignedToDepartmentNameEn != null && i.AssignedToDepartmentNameEn.ToLower().Contains(@5)) || ( i.AssignedToDepartmentNameAr != null && i.AssignedToDepartmentNameAr.ToLower().Contains(@6)) || ( i.AssignedToEn != null && i.AssignedToEn.ToLower().Contains(@7)) || ( i.AssignedToAr != null && i.AssignedToAr.ToLower().Contains(@8)) || ( i.StatusEn != null && i.StatusEn.ToLower().Contains(@9)) || ( i.StatusAr != null && i.StatusAr.ToLower().Contains(@10)) )",
                FilterParameters = new object[] { search, search, search, search, search, search, search, search, search, search, search }
            });
            if (result.IsSuccessStatusCode)
            {
                getTimeTrackingList = (IEnumerable<TimeTrackingVM>)result.ResultData;
                countTimeTrackingGrid = getTimeTrackingList.Count();
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
        }
        #endregion
    }
}
