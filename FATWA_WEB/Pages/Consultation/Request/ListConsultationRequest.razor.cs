using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_WEB.Services;
using FATWA_WEB.Shared;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using Query = Radzen.Query;
namespace FATWA_WEB.Pages.Consultation.Request
{
    public partial class ListConsultationRequest : ComponentBase
    {

        #region Variables Declaration
        protected RadzenDataGrid<ConsultationRequestVM>? grid;
        public bool isVisible { get; set; }
        protected AdvanceSearchConsultationRequestVM advanceSearchCOMSVM { get; set; } = new AdvanceSearchConsultationRequestVM();

        //protected bool AdvancedSearchResultGrid;
        protected bool Keywords = false;

        protected List<CaseRequestStatus> Statuses { get; set; } = new List<CaseRequestStatus>();
        protected List<RequestType> RequestTypes { get; set; } = new List<RequestType>();
        public int selectedIndex { get; set; } = 0;
        public bool allowRowSelectOnRowClick = true;
        public IList<ComsWithDrawConsultationRequestVM> getWithDrawConsultationRequest { get; set; } = new List<ComsWithDrawConsultationRequestVM>();
        public ComsWithDrawConsultationRequestVM comsWithdrawRequest { get; set; } = new ComsWithDrawConsultationRequestVM();
        protected AdvanceSearchTaskVM advanceSearchTaskVM = new AdvanceSearchTaskVM();
        protected IEnumerable<TaskVM> getTaskList { get; set; }
        protected RadzenDataGrid<TaskVM> gridTask = new RadzenDataGrid<TaskVM>();
        protected int pendingRequestsCount { get; set; }
        protected string RedirectURL { get; set; }
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => grid.CurrentPage + 1;
        private int CurrentPageSize => grid.PageSize;

        #endregion

        #region Full property declaration

        IEnumerable<ConsultationRequestVM> getConsultationRequestDetail = new List<ConsultationRequestVM>();
        protected IEnumerable<ConsultationRequestVM> FilteredConsultationRequestDetail { get; set; }


        #endregion

        #region On Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            advanceSearchCOMSVM.SectorTypeId = loginState.UserDetail.SectorTypeId;
            advanceSearchCOMSVM.PageSize = systemSettingState.Grid_Pagination;
            await PopulateCaseRequestStatuses();
            await PopulateRequestTypes();
            await PopulateSectorTypes();
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
                if (string.IsNullOrEmpty(args.OrderBy) || CurrentPage != advanceSearchCOMSVM.PageNumber || CurrentPageSize != advanceSearchCOMSVM.PageSize || (Keywords && advanceSearchCOMSVM.isDataSorted))
                {
                    if (advanceSearchCOMSVM.isGridLoaded && advanceSearchCOMSVM.PageSize == CurrentPageSize && !advanceSearchCOMSVM.isPageSizeChangeOnFirstLastPage)
                    {
                        grid.CurrentPage = (int)advanceSearchCOMSVM.PageNumber - 1;
                        advanceSearchCOMSVM.isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(args);
                    spinnerService.Show();
                    advanceSearchCOMSVM.ShowUndefinedRequest = false;
                    advanceSearchCOMSVM.userDetail = await BrowserStorage.GetItemAsync<UserDetailVM>("UserDetail");

                    var response = await consultationRequestService.GetConsultationRequestList(advanceSearchCOMSVM);
                    if (response.IsSuccessStatusCode)
                    {
                        getConsultationRequestDetail = (IEnumerable<ConsultationRequestVM>)response.ResultData;
                        FilteredConsultationRequestDetail = (IEnumerable<ConsultationRequestVM>)response.ResultData;
                        if (!(string.IsNullOrEmpty(search))) await OnSearchInput();
                        if (!(string.IsNullOrEmpty(args.OrderBy)) && (string.IsNullOrEmpty(search)))
                        {
                            FilteredConsultationRequestDetail = await gridSearchExtension.Sort(FilteredConsultationRequestDetail, ColumnName, SortOrder);
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
            if (advanceSearchCOMSVM.PageSize != null && advanceSearchCOMSVM.PageSize != CurrentPageSize)
            {
                int oldPageCount = getConsultationRequestDetail.Any() ? (getConsultationRequestDetail.First().TotalCount) / ((int)advanceSearchCOMSVM.PageSize) : 1;
                int oldPageNumber = (int)advanceSearchCOMSVM.PageNumber - 1;
                advanceSearchCOMSVM.isGridLoaded = true;
                advanceSearchCOMSVM.PageNumber = CurrentPage;
                advanceSearchCOMSVM.PageSize = args.Top;
                int TotalPages = getConsultationRequestDetail.Any() ? (getConsultationRequestDetail.First().TotalCount) / (grid.PageSize) : 1;
                if (CurrentPage > TotalPages)
                {
                    advanceSearchCOMSVM.PageNumber = TotalPages + 1;
                    advanceSearchCOMSVM.PageSize = args.Top;
                    grid.CurrentPage = TotalPages;
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
        private async Task OnSort(DataGridColumnSortEventArgs<ConsultationRequestVM> args)
        {
            if (args.SortOrder != null)
            {
                FilteredConsultationRequestDetail = await gridSearchExtension.Sort(FilteredConsultationRequestDetail, args.Column.Property, (SortOrder)args.SortOrder);
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
        protected async Task OnSearchInput()
        {
            try
            {
                search = string.IsNullOrEmpty(search) ? "" : search.TrimStart().TrimEnd().ToLower();
                FilteredConsultationRequestDetail = await gridSearchExtension.Filter(getConsultationRequestDetail, new Query()
                {
                    Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US"
                    ? $@"i => (i.RequestNumber != null && i.RequestNumber.ToString().ToLower().Contains(@0)) 
                        || (i.RequestType_Name_En != null && i.RequestType_Name_En.ToString().ToLower().Contains(@1)) 
                        || (i.Subject != null && i.Subject.ToString().ToLower().Contains(@2)) 
                        || (i.Status_Name_En != null && i.Status_Name_En.ToString().ToLower().Contains(@3)) 
                        || (i.RequestDate.HasValue && i.RequestDate.Value.ToString(""dd/MM/yyyy"").Contains(@4))
                        || (i.SectorType_Name_En != null && i.SectorType_Name_En.ToString().ToLower().Contains(@5))"

                    : $@"i => (i.RequestNumber != null && i.RequestNumber.ToString().ToLower().Contains(@0)) 
                        || (i.RequestType_Name_Ar != null && i.RequestType_Name_Ar.ToString().ToLower().Contains(@1)) 
                        || (i.Subject != null && i.Subject.ToString().ToLower().Contains(@2)) 
                        || (i.Status_Name_Ar != null && i.Status_Name_Ar.ToString().ToLower().Contains(@3)) 
                        || (i.RequestDate.HasValue && i.RequestDate.Value.ToString(""dd/MM/yyyy"").Contains(@4))
                        || (i.SectorType_Name_Ar != null && i.SectorType_Name_Ar.ToString().ToLower().Contains(@5))",
                    FilterParameters = new object[] { search, search, search, search, search, search }
                });
                if (!string.IsNullOrEmpty(ColumnName))
                    FilteredConsultationRequestDetail = await gridSearchExtension.Sort(FilteredConsultationRequestDetail, ColumnName, SortOrder);
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
        //<History Author = 'Muhammad Zaeem' Date='2022-06-06' Version="1.0" Branch="master"> Redirect to View Detail page</History>
        protected async Task Grid0RowSelect(ConsultationRequestVM args)
        {
            //navigationManager.NavigateTo("consultationrequest-detail/" + args.ConsultationRequestId + "/" + args.SectorTypeId);
            if (args.RequestTypeId == (int)RequestTypeEnum.Contracts || args.RequestTypeId == (int)RequestTypeEnum.Legislations || args.RequestTypeId == (int)RequestTypeEnum.LegalAdvice ||
                args.RequestTypeId == (int)RequestTypeEnum.InternationalArbitration ||
                args.RequestTypeId == (int)RequestTypeEnum.AdministrativeComplaints)
            {
                navigationManager.NavigateTo("consultationrequest-detail/" + args.ConsultationRequestId + "/" + args.SectorTypeId);
            }
            else
            {
                navigationManager.NavigateTo("caserequest-view/" + args.ConsultationRequestId);
            }
        }
        protected async Task ViewDetail(ComsWithDrawConsultationRequestVM item)
        {
            RedirectURL = "/detail-withdraw-consultationrequest/" + item.WithdrawId + "/" + (int)CommunicationTypeEnum.WithdrawRequested;
            navigationManager.NavigateTo(RedirectURL);
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
                Keywords = advanceSearchCOMSVM.isDataSorted = true;
                return;
            }
            if (string.IsNullOrEmpty(advanceSearchCOMSVM.RequestNumber)
                && !advanceSearchCOMSVM.StatusId.HasValue
                && string.IsNullOrWhiteSpace(advanceSearchCOMSVM.Subject)
                && !advanceSearchCOMSVM.RequestFrom.HasValue && !advanceSearchCOMSVM.RequestTo.HasValue) { }
            else
            {
                Keywords = advanceSearchCOMSVM.isDataSorted = true;
                if (grid.CurrentPage > 0)
                    await grid.FirstPage();
                else
                {
                    advanceSearchCOMSVM.isGridLoaded = false;
                    await grid.Reload();
                }
            }
        }

        public async void ResetForm()
        {
            advanceSearchCOMSVM = new AdvanceSearchConsultationRequestVM { PageSize = grid.PageSize };
            Keywords = advanceSearchCOMSVM.isDataSorted = false;
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

        #endregion

        #region Remote Dropdown Data and Dropdown Change Events
        //<History Author = 'Muhammad Zaeem' Date='2022-09-30' Version="1.0" Branch="master">Populate Subtypes data</History>
        protected async Task PopulateRequestTypes()
        {
            var response = await lookupService.GetRequestTypes();
            if (response.IsSuccessStatusCode)
            {
                RequestTypes = (List<RequestType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
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

        #region Get WithDraw Consultaiton Requests
        protected async Task ExpandWithDrawConsultationRequest(ConsultationRequestVM consultationRequestVM)
        {
            var response = await consultationRequestService.GetWithDrawConsultationRequestByRequestId(consultationRequestVM.ConsultationRequestId);
            if (response.IsSuccessStatusCode)
            {
                getWithDrawConsultationRequest = (List<ComsWithDrawConsultationRequestVM>)response.ResultData;
                getWithDrawConsultationRequest = getWithDrawConsultationRequest.Where(x => x.WithdrawRequestedBy != null).ToList();
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        #endregion

        #region Submitt Withdraw Reject reason
        protected async Task FormReasonSubmit(WithdrawRequestDetailVM request)
        {
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
                              translationState.Translate("Sure_Reject_WithdrawRequest"),
                              translationState.Translate("Confirm"),
                               new ConfirmOptions()
                               {
                                   OkButtonText = @translationState.Translate("OK"),
                                   CancelButtonText = @translationState.Translate("Cancel")
                               });

                if (dialogResponse == true)
                {
                    var response = await consultationRequestService.UpdateWithDrawConsultationRequest(request, true);
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("WithdrawRequest_Rejected"),
                            Style = "position: fixed !important; left: 0; margin: auto;"
                        });
                        await Task.Delay(300);
                        dialogService.Close(null);
                        grid.Reset();
                        await grid.Reload();
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
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion

        #region RowRender On withdraw list
        public void RowRender(RowRenderEventArgs<ConsultationRequestVM> args)
        {
            if (args.Data.WithdrawCount <= 0)
            {
                args.Attributes.Add("class", "no-withdraw-linked");
            }
        }
        #endregion

    }
}
