using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_DOMAIN.Models.ViewModel.Shared;
using FATWA_WEB.Shared;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;

namespace FATWA_WEB.Pages.Shared
{
    public partial class RequestList : ComponentBase
    {
        #region Variables Declaration
        protected RadzenDataGrid<RequestListVM>? requestGrid = new RadzenDataGrid<RequestListVM>();
        IEnumerable<RequestListVM> getRequestDetail { get; set; } = new List<RequestListVM>();
        protected IEnumerable<RequestListVM> FilteredRequestDetail { get; set; }
        protected AdvanceSearchConsultationRequestVM advanceSearchCOMSVM { get; set; } = new AdvanceSearchConsultationRequestVM();
        public bool isVisible { get; set; }
        protected List<CaseRequestStatus> Statuses { get; set; } = new List<CaseRequestStatus>();
        protected bool Keywords = false;
        protected string search { get; set; }
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => requestGrid.CurrentPage + 1;
        private int CurrentPageSize => requestGrid.PageSize;

        #endregion

        #region On Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateSectorTypes();
            await PopulateCaseRequestStatuses();
            translationState.TranslateGridFilterLabels(requestGrid);
            spinnerService.Hide();
        }
        #endregion

        #region On Load Grid Data 
        protected async Task OnLoadData(LoadDataArgs args)
        {
            try
            {
                if (string.IsNullOrEmpty(args.OrderBy) || CurrentPage != advanceSearchCOMSVM.PageNumber || CurrentPageSize != advanceSearchCOMSVM.PageSize || (Keywords && advanceSearchCOMSVM.isDataSorted))
                {
                    if (advanceSearchCOMSVM.isGridLoaded && advanceSearchCOMSVM.PageSize == CurrentPageSize && !advanceSearchCOMSVM.isPageSizeChangeOnFirstLastPage)
                    {
                        requestGrid.CurrentPage = (int)advanceSearchCOMSVM.PageNumber - 1;
                        advanceSearchCOMSVM.isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(args);
                    spinnerService.Show();
                    advanceSearchCOMSVM.ShowUndefinedRequest = false;
                    advanceSearchCOMSVM.userDetail = await BrowserStorage.GetItemAsync<UserDetailVM>("UserDetail");

                    var response = await cmsSharedService.GetCaseConsultationRequestList(advanceSearchCOMSVM);
                    spinnerService.Hide();
                    if (response.IsSuccessStatusCode)
                    {
                        getRequestDetail = (IEnumerable<RequestListVM>)response.ResultData;
                        FilteredRequestDetail = (IEnumerable<RequestListVM>)response.ResultData;
                        if (!(string.IsNullOrEmpty(search))) await OnSearchInput();
                        if (!(string.IsNullOrEmpty(args.OrderBy)) && (string.IsNullOrEmpty(search)))
                        {
                            FilteredRequestDetail = await gridSearchExtension.Sort(FilteredRequestDetail, ColumnName, SortOrder);
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
            if (advanceSearchCOMSVM.PageSize != null && advanceSearchCOMSVM.PageSize != CurrentPageSize)
            {
                int oldPageCount = getRequestDetail.Any() ? (getRequestDetail.First().TotalCount) / ((int)advanceSearchCOMSVM.PageSize) : 1;
                int oldPageNumber = (int)advanceSearchCOMSVM.PageNumber - 1;
                advanceSearchCOMSVM.isGridLoaded = true;
                advanceSearchCOMSVM.PageNumber = CurrentPage;
                advanceSearchCOMSVM.PageSize = args.Top;
                int TotalPages = getRequestDetail.Any() ? (getRequestDetail.First().TotalCount) / (requestGrid.PageSize) : 1;
                if (CurrentPage > TotalPages)
                {
                    advanceSearchCOMSVM.PageNumber = TotalPages + 1;
                    advanceSearchCOMSVM.PageSize = args.Top;
                    requestGrid.CurrentPage = TotalPages;
                }
                if ((advanceSearchCOMSVM.PageNumber == 1 || (advanceSearchCOMSVM.PageNumber == TotalPages + 1 && oldPageCount == oldPageNumber)) && oldPageCount != 0)
                {
                    advanceSearchCOMSVM.isPageSizeChangeOnFirstLastPage = true;
                }
                else
                {
                    advanceSearchCOMSVM.isPageSizeChangeOnFirstLastPage = false;
                }
                return;
            }
            advanceSearchCOMSVM.PageNumber = CurrentPage;
            advanceSearchCOMSVM.PageSize = args.Top;
        }
        #endregion

        #region On Sort Grid Data
        private async Task OnSort(DataGridColumnSortEventArgs<RequestListVM> args)
        {
            if (args.SortOrder != null)
            {
                FilteredRequestDetail = await gridSearchExtension.Sort(FilteredRequestDetail, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
                advanceSearchCOMSVM.isDataSorted = false;
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

                FilteredRequestDetail = await gridSearchExtension.Filter(getRequestDetail, new Query()
                {
                    Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US"
                    ? $@"i => (i.RequestNumber != null && i.RequestNumber.ToString().ToLower().Contains(@0)) 
                            || (i.Subject != null && i.Subject.ToString().ToLower().Contains(@1)) 
                            || (i.Status_Name_En != null && i.Status_Name_En.Replace(""  "","" "").ToString().ToLower().Contains(@2)) 
                            || (i.RequestDate.HasValue && i.RequestDate.Value.ToString(""dd/MM/yyyy"").Contains(@3))"

                    : $@"i => (i.RequestNumber != null && i.RequestNumber.ToString().ToLower().Contains(@0)) 
                            || (i.Subject != null && i.Subject.ToString().ToLower().Contains(@1)) 
                            || (i.Status_Name_Ar != null && i.Status_Name_Ar.Replace(""  "","" "").ToString().ToLower().Contains(@2)) 
                            || (i.RequestDate.HasValue && i.RequestDate.Value.ToString(""dd/MM/yyyy"").Contains(@3))",
                    FilterParameters = new object[] { search, search, search, search }
                });
                if (!string.IsNullOrEmpty(ColumnName))
                    FilteredRequestDetail = await gridSearchExtension.Sort(FilteredRequestDetail, ColumnName, SortOrder);
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
        protected async Task Grid0RowSelect(RequestListVM args)
        {
            if (args.RequestTypeId == (int)RequestTypeEnum.Contracts || args.RequestTypeId == (int)RequestTypeEnum.Legislations || args.RequestTypeId == (int)RequestTypeEnum.LegalAdvice ||
                args.RequestTypeId == (int)RequestTypeEnum.InternationalArbitration ||
                args.RequestTypeId == (int)RequestTypeEnum.AdministrativeComplaints)
            {
                navigationManager.NavigateTo("consultationrequest-detail/" + args.RequestId + "/" + args.SectorTypeId);
            }
            else
            {
                navigationManager.NavigateTo("caserequest-view/" + args.RequestId);
            }
        }
        #endregion

        #region Remote Dropdown Data and Dropdown Change Events

        protected async Task PopulateCaseRequestStatuses()
        {
            var response = await lookupService.GetCaseRequestStatuses();
            if (response.IsSuccessStatusCode)
            {
                Statuses = (List<CaseRequestStatus>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task PopulateSectorTypes()
        {
            advanceSearchCOMSVM.SectorTypeId = loginState.UserDetail.SectorTypeId;
        }

        #endregion

        #region Advance Search
        protected async Task SubmitAdvanceSearch()
        {
            if (advanceSearchCOMSVM.RequestFrom > advanceSearchCOMSVM.RequestTo)
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
            if (advanceSearchCOMSVM.RequestNumber == string.Empty && advanceSearchCOMSVM.StatusId == 0
                && string.IsNullOrWhiteSpace(advanceSearchCOMSVM.Subject) && !advanceSearchCOMSVM.RequestFrom.HasValue
                && !advanceSearchCOMSVM.RequestTo.HasValue && advanceSearchCOMSVM.RequestTypeId == 0
                && advanceSearchCOMSVM.SectorTypeId == 0 && string.IsNullOrWhiteSpace(advanceSearchCOMSVM.Subject)) { }
            else
            {
                Keywords = advanceSearchCOMSVM.isDataSorted = true;
                if (requestGrid.CurrentPage > 0)
                    await requestGrid.FirstPage();
                else
                {
                    advanceSearchCOMSVM.isGridLoaded = false;
                    await requestGrid.Reload();
                }
                StateHasChanged();
            }
        }

        public async void ResetForm()
        {
            advanceSearchCOMSVM = new AdvanceSearchConsultationRequestVM { PageSize = requestGrid.PageSize };
            Keywords = advanceSearchCOMSVM.isDataSorted = false;
            await PopulateSectorTypes();
            requestGrid.Reset();
            await requestGrid.Reload();
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
