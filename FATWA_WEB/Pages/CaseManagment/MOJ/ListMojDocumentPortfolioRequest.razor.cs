using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.CaseManagment.MOJ
{
    //<History Author = 'Hassan Abbas' Date='2023-03-23' Version="1.0" Branch="master"> List of Moj Document Portfolio Requests</History>
    public partial class ListMojDocumentPortfolioRequest : ComponentBase
    {

        #region Variables
        protected RadzenDataGrid<MojDocumentPortfolioRequestVM>? grid = new RadzenDataGrid<MojDocumentPortfolioRequestVM>();
        protected bool Keywords = false;
        public int selectedIndex { get; set; } = 0;
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => grid.CurrentPage + 1;
        private int CurrentPageSize => grid.PageSize;
        private int PageNumber { get; set; } = 1;
        private int? PageSize { get; set; }
        private bool isGridLoaded { get; set; }
        private bool isPageSizeChangeOnFirstLastPage { get; set; }
        IEnumerable<MojDocumentPortfolioRequestVM> _mojPortfolioRequests;
        IEnumerable<MojDocumentPortfolioRequestVM> FileterdMojPortfolioRequests { get; set; }
        protected IEnumerable<MojDocumentPortfolioRequestVM> mojPortfolioRequests = new List<MojDocumentPortfolioRequestVM>();
        private Timer debouncer;
        private const int debouncerDelay = 500;

        #endregion

        #region On Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            PageSize = systemSettingState.Grid_Pagination;
            translationState.TranslateGridFilterLabels(grid);
            spinnerService.Hide();
        }

        protected string search { get; set; }
       
        #endregion

        #region On Load Grid Data
        protected async Task OnLoadData(LoadDataArgs dataArgs)
        {
            try
            {
                if (string.IsNullOrEmpty(dataArgs.OrderBy) || CurrentPage != PageNumber || CurrentPageSize != PageSize)
                {
                    if (isGridLoaded && PageSize == CurrentPageSize && !isPageSizeChangeOnFirstLastPage)
                    {
                        grid.CurrentPage = (int)PageNumber - 1;
                        isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(dataArgs);
                    spinnerService.Show();
                    var response = await cmsCaseFileService.GetMojDocumentPortfolioRequests(loginState.UserDetail.SectorTypeId, PageNumber, PageSize);
                    if (response.IsSuccessStatusCode)
                    {
                        FileterdMojPortfolioRequests = mojPortfolioRequests = (IEnumerable<MojDocumentPortfolioRequestVM>)response.ResultData;
                        if (!(string.IsNullOrEmpty(search))) await OnSearchInput(search);
                        if (!string.IsNullOrEmpty(dataArgs.OrderBy) && string.IsNullOrEmpty(search))
                        {
                            FileterdMojPortfolioRequests = await gridSearchExtension.Sort(FileterdMojPortfolioRequests, ColumnName, SortOrder);
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

        private void SetPagingProperties(LoadDataArgs args)
        {
            if (PageSize != CurrentPageSize)
            {
                int oldPageCount = mojPortfolioRequests.Any() ? (mojPortfolioRequests.First().TotalCount) / ((int)PageSize) : 1;
                int oldPageNumber = PageNumber - 1;
                isGridLoaded = true;
                PageNumber = CurrentPage;
                PageSize = args.Top;
                int TotalPages = mojPortfolioRequests.Any() ? (mojPortfolioRequests.First().TotalCount) / (grid.PageSize) : 1;
                if (CurrentPage > TotalPages)
                {
                    PageNumber = TotalPages + 1;
                    PageSize = args.Top;
                    grid.CurrentPage = TotalPages;
                }
                if ((PageNumber == 1 || (PageNumber == TotalPages + 1 && oldPageCount == oldPageNumber)) && oldPageCount != 0)
                {
                    isPageSizeChangeOnFirstLastPage = true;
                }
                else
                {
                    isPageSizeChangeOnFirstLastPage = false;
                }
                return;
            }
            PageNumber = CurrentPage;
            PageSize = args.Top;
        }
        #endregion

        #region On Sort Grid Data
        private async Task OnSortData(DataGridColumnSortEventArgs<MojDocumentPortfolioRequestVM> args)
        {
            if (args.SortOrder != null)
            {
                FileterdMojPortfolioRequests = await gridSearchExtension.Sort(FileterdMojPortfolioRequests, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
            }
            else
            {
                ColumnName = string.Empty;
            }
        }
        #endregion


        #region On Search Input
        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FileterdMojPortfolioRequests = await gridSearchExtension.Filter(mojPortfolioRequests, new Query()
                {
                    Filter = $@"i => ( i.CaseNumber != null && i.CaseNumber.ToString().ToLower().Contains(@0))||
                    (i.RequiredDocuments != null && i.RequiredDocuments.ToString().ToLower().Contains(@1)) ||
                    (i.HearingDate != null && i.HearingDate.ToString(""dd/MM/yyyy"").Contains(@2)) ||
                    (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@3))",
                    FilterParameters = new object[] { search, search, search, search }
                });
                if (!string.IsNullOrEmpty(ColumnName))
                    FileterdMojPortfolioRequests = await gridSearchExtension.Sort(FileterdMojPortfolioRequests, ColumnName, SortOrder);
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
        //<History Author = 'Hassan Abbas' Date='2022-11-29' Version="1.0" Branch="master"> Redirect to View Detail page</History>
        protected async Task Detail(MojDocumentPortfolioRequestVM args)
        {
            navigationManager.NavigateTo("detail-portfolio-request/" + args.Id + "/" + args.CaseNumber);
        }

        #endregion

        #region Badrequest Notification
        //History Author = 'Hassan Abbas' Date='2022-07-22' Version="1.0" Branch="master"> Handle bad request and display error messages in whole class</History>
        protected async Task ReturnBadRequestNotification(ApiCallResponse response)
        {
            try
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Token_Expired"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    await Task.Delay(5000);
                    await BrowserStorage.RemoveItemAsync("User");
                    await BrowserStorage.RemoveItemAsync("Token");
                    await BrowserStorage.RemoveItemAsync("RefreshToken");
                    await BrowserStorage.RemoveItemAsync("UserDetail");
                    await BrowserStorage.RemoveItemAsync("SecurityStamp");
                    loginState.IsLoggedIn = false;
                    loginState.IsStateChecked = true;
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
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
