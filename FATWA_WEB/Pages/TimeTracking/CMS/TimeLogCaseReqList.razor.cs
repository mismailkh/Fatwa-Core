using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_WEB.Pages.CaseManagment.Request;
using FATWA_WEB.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.CommunicationEnums;

namespace FATWA_WEB.Pages.TimeTracking.CMS
{
    public partial class TimeLogCaseReqList : ComponentBase
    {
        #region Parameter 
        [Parameter]
        public Guid CommunicationId { get; set; }
        #endregion

        #region Variables Declaration
        protected RadzenDataGrid<CmsCaseRequestVM> grid = new RadzenDataGrid<CmsCaseRequestVM>();
        public bool isVisible { get; set; }
        protected AdvanceSearchCmsCaseRequestVM advanceSearchVM = new AdvanceSearchCmsCaseRequestVM();
        protected AdvanceSearchTaskVM advanceSearchTaskVM = new AdvanceSearchTaskVM();
        protected AdvanceSearchCmsCaseRequestVM args;
        protected bool Keywords = false;
        protected List<CaseRequestStatus> Statuses { get; set; } = new List<CaseRequestStatus>();
        protected List<GovernmentEntity> GovtEntities = new List<GovernmentEntity>();
        public bool allowRowSelectOnRowClick = true;
        public IList<CmsCaseRequestVM> selectedRequests;
        public IList<CmsWithDrawCaseRequestVM> getWithDrawCaseRequest { get; set; } = new List<CmsWithDrawCaseRequestVM>();
        protected RadzenDataGrid<TaskVM> gridTask = new RadzenDataGrid<TaskVM>();
        protected IEnumerable<CmsCaseRequestVM> linkedRequests;
        protected string RedirectURL { get; set; }
        IEnumerable<CmsCaseRequestVM> getCaseRequestDetail { get; set; } = new List<CmsCaseRequestVM>();
        IEnumerable<CmsCaseRequestVM> FilteredGetCaseRequestDetail { get; set; }
        protected string? search { get; set; }
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
            translationState.TranslateGridFilterLabels(grid);
            await PopulateCaseRequestStatuses();
            await PopulateGovtEntities();
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
                    advanceSearchVM.ShowUndefinedRequest = false;

                    var response = await caseRequestService.GetCMSCaseRequest(advanceSearchVM);

                    if (response.IsSuccessStatusCode)
                    {
                        getCaseRequestDetail = (IEnumerable<CmsCaseRequestVM>)response.ResultData;
                        FilteredGetCaseRequestDetail = (IEnumerable<CmsCaseRequestVM>)response.ResultData;
                        if (!(string.IsNullOrEmpty(search))) await OnSearchInput();
                        if (!(string.IsNullOrEmpty(args.OrderBy)) && (string.IsNullOrEmpty(search)))
                        {
                            FilteredGetCaseRequestDetail = await gridSearchExtension.Sort(FilteredGetCaseRequestDetail, ColumnName, SortOrder);
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
            spinnerService.Hide();
        }
        #endregion


        #region Grid Pagination Calculation
        private void SetPagingProperties(LoadDataArgs args)
        {
            if (advanceSearchVM.PageSize != null && advanceSearchVM.PageSize != CurrentPageSize)
            {
                int oldPageCount = getCaseRequestDetail.Any() ? (getCaseRequestDetail.First().TotalCount) / ((int)advanceSearchVM.PageSize) : 1;
                int oldPageNumber = (int)advanceSearchVM.PageNumber - 1;
                advanceSearchVM.isGridLoaded = true;
                advanceSearchVM.PageNumber = CurrentPage;
                advanceSearchVM.PageSize = args.Top;
                int TotalPages = getCaseRequestDetail.Any() ? (getCaseRequestDetail.First().TotalCount) / (grid.PageSize) : 1;
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
        private async Task OnSort(DataGridColumnSortEventArgs<CmsCaseRequestVM> args)
        {
            if (args.SortOrder != null)
            {
                FilteredGetCaseRequestDetail = await gridSearchExtension.Sort(FilteredGetCaseRequestDetail, args.Column.Property, (SortOrder)args.SortOrder);
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

                FilteredGetCaseRequestDetail = await gridSearchExtension.Filter(getCaseRequestDetail, new Query()
                {
                    Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US"
                    ? $@"i => (i.RequestNumber != null && i.RequestNumber.ToString().ToLower().Contains(@0)) 
                            || (i.GovermentEntity_Name_En != null && i.GovermentEntity_Name_En.ToLower().Contains(@1)) 
                            || (i.Status_Name_En != null && i.Status_Name_En.Replace(""  "","" "").ToLower().Contains(@2))"

                    : $@"i => (i.RequestNumber != null && i.RequestNumber.ToString().ToLower().Contains(@0)) 
                            || (i.Status_Name_Ar != null && i.Status_Name_Ar.Replace(""  "","" "").ToString().ToLower().Contains(@1)) 
                            || (i.GovermentEntity_Name_Ar != null && i.GovermentEntity_Name_Ar.ToString().ToLower().Contains(@2))",
                    FilterParameters = new object[] { search, search, search }
                });
                if (!string.IsNullOrEmpty(ColumnName))
                    FilteredGetCaseRequestDetail = await gridSearchExtension.Sort(FilteredGetCaseRequestDetail, ColumnName, SortOrder);
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
        protected async Task Grid0RowSelect(CmsCaseRequestVM args)
        {
            navigationManager.NavigateTo("/caserequest-view/" + args.RequestId);
        }

        protected async Task ViewDetail(CmsWithDrawCaseRequestVM item)
        {
            RedirectURL = "/detail-withdraw-request/" + item.WithdrawRequestId + "/" + (int)CommunicationTypeEnum.WithdrawRequested;
            navigationManager.NavigateTo(RedirectURL);
        }
        protected async Task ConvertedtoFileButton(CmsCaseRequestVM args)
        {
            navigationManager.NavigateTo("/casefile-view/" + args.FileId);
        }
        public void RowRender(RowRenderEventArgs<CmsCaseRequestVM> args)
        {
            if (args.Data.WithdrawCount <= 0 && args.Data.LinkedCount <= 0)
            {
                args.Attributes.Add("class", "no-withdraw-linked");
            }
        }

        protected void CellRender(RowRenderEventArgs<CmsCaseRequestVM> args)
        {
            if (args.Data.IsViewed == false)
            {
                args.Attributes.Add("style", $"background-color: #e3d4c9;");
            }
        }
        //protected async Task Grid0ShowCmsCaseRequest(CaseRequestDetailVM args)
        //{
        //    navigationManager.NavigateTo("/Add-Case-Request/" + args.RequestId);
        //}
        //protected async Task Grid0ShowCmsDocument(CaseRequestDetailVM args)
        //{
        //    navigationManager.NavigateTo("/Add-Case-Request/" + args.RequestId);
        //}
        #endregion

        #region Advance Search
        protected async Task SubmitAdvanceSearch()
        {
            if (advanceSearchVM.RequestFrom > advanceSearchVM.RequestTo)
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
            if (string.IsNullOrWhiteSpace(advanceSearchVM.RequestNumber)
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
            advanceSearchVM = new AdvanceSearchCmsCaseRequestVM { PageSize = grid.PageSize };
            await PopulateSectorTypes();
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

        #region Remote Dropdown Data and Dropdown Change Events
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Populate Subtypes data</History>
        protected async Task PopulateSectorTypes()
        {
            advanceSearchVM.SectorTypeId = loginState.UserDetail.SectorTypeId;
        }

        //<History Author = 'Hassan Abbas' Date='2023-03-10' Version="1.0" Branch="master">Populate Count data</History>

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

        //<History Author = 'Hassan Abbas' Date='2022-01-31' Version="1.0" Branch="master">Populate Linked Requests</History>
        protected async Task PopulateLinkedRequests(string RequestId)
        {
            var response = await caseRequestService.GetLinkedRequestsByPrimaryRequestId(RequestId);
            if (response.IsSuccessStatusCode)
            {
                linkedRequests = (List<CmsCaseRequestVM>)response.ResultData;
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

        #region Link Requests

        //<History Author = 'Hassan Abbas' Date='2022-09-28' Version="1.0" Branch="master">Link Requests </History>
        protected async Task LinkRequests(MouseEventArgs args)
        {
            try
            {
                if (selectedRequests != null && selectedRequests.Any())
                {
                    if (selectedRequests.Where(r => r.StatusId >= (int)CaseRequestStatusEnum.RegisteredInMOJ).Any())
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("One_Request_Already_Registered_Moj"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        return;
                    }
                    var result = await dialogService.OpenAsync<SelectPrimaryRequestForLinkingPopup>(translationState.Translate("Select_Primary_Request"),
                        new Dictionary<string, object>()
                        {
                            { "Requests", selectedRequests },
                        },
                        new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true }
                    );

                    if (result != null)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Requests_Linked_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        await grid.Reload();
                        selectedRequests = null;
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

        #region Get WithDraw And Linked Case Requests
        protected async Task ExpandWithDrawAndLinkedCaseRequest(CmsCaseRequestVM caseRequestVM)
        {
            await PopulateLinkedRequests(caseRequestVM.RequestId.ToString());
            var response = await caseRequestService.GetWithDrawCaseRequestByRequestId(caseRequestVM.RequestId);
            if (response.IsSuccessStatusCode)
            {
                getWithDrawCaseRequest = (List<CmsWithDrawCaseRequestVM>)response.ResultData;
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        #endregion 
    }
}
