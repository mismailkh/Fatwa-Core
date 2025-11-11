using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_WEB.Pages.HRMS.Employee;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using static FATWA_GENERAL.Helper.Response;
using FATWA_DOMAIN.Models.ViewModel;

namespace FATWA_WEB.Pages.CaseManagment.Users
{
    public partial class ListSectorUsers : ComponentBase
    {
        #region Variables Declaration
        private List<SectorUsersVM> UsersList { get; set; }
        private List<SectorUsersVM> FilteredUsersList { get; set; }
        protected RadzenDataGrid<SectorUsersVM>? UsersGrid = new RadzenDataGrid<SectorUsersVM>();
        protected string search;
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        private int CurrentPage => UsersGrid.CurrentPage + 1;
        private int CurrentPageSize => UsersGrid.PageSize;
        private int? PageSize { get; set; }
        private int? PageNumber { get; set; } = 1;
        private bool isPageSizeChangeOnFirstLastPage { get; set; }
        public bool isGridLoaded { get; set; }
        private Timer debouncer;
        private const int debouncerDelay = 500;

        #endregion

        #region On Component Load 
        protected override async Task OnInitializedAsync()
        {
            PageSize = systemSettingState.Grid_Pagination;
            translationState.TranslateGridFilterLabels(UsersGrid);
        }
        #endregion

        #region On Load Grid Data
        /*<History Author='Ammaar Naveed' Date='2025-02-02' Version="1.0" Branch="master">Fetch sector users list based on pagination</History>*/
        protected async Task OnLoadData(LoadDataArgs dataArgs)
        {
            if (string.IsNullOrEmpty(dataArgs.OrderBy) || CurrentPage != PageNumber || CurrentPageSize != PageSize)
            {
                try
                {
                    if (isGridLoaded && PageSize == CurrentPageSize && !isPageSizeChangeOnFirstLastPage)
                    {
                        UsersGrid.CurrentPage = (int)PageNumber - 1;
                        isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(dataArgs);
                    string RoleId = loginState.UserDetail.RoleId;
                    int? SectorTypeId = loginState.UserDetail.SectorTypeId;
                    spinnerService.Show();
                    var response = await cmsSharedService.GetSectorUsersList(RoleId, SectorTypeId, PageNumber, PageSize, loginState.UserDetail.UserId);

                    if (response.IsSuccessStatusCode)
                    {
                        UsersList = (List<SectorUsersVM>)response.ResultData;
                        UsersList = UsersList.Where(user => user.UserId != loginState.UserDetail.UserId).ToList();
                        FilteredUsersList = UsersList;
                        if (!(string.IsNullOrEmpty(search))) await OnSearchInput(search);
                        if (!(string.IsNullOrEmpty(dataArgs.OrderBy)))
                        {
                            FilteredUsersList = await gridSearchExtension.Sort(FilteredUsersList, ColumnName, SortOrder);
                        }
                        await InvokeAsync(StateHasChanged);
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    spinnerService.Hide();

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
        }
        private void SetPagingProperties(LoadDataArgs args)
        {
            if (PageSize != CurrentPageSize)
            {
                int oldPageCount = UsersList.Any() ? (UsersList.First().TotalCount) / ((int)PageSize) : 1;
                int oldPageNumber = (int)PageNumber - 1;
                isGridLoaded = true;
                PageNumber = CurrentPage;
                PageSize = args.Top;
                int TotalPages = UsersList.Any() ? (UsersList.First().TotalCount) / (UsersGrid.PageSize) : 1;
                if (CurrentPage > TotalPages)
                {
                    PageNumber = TotalPages + 1;
                    PageSize = args.Top;
                    UsersGrid.CurrentPage = TotalPages;
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
        /*<History Author='Ammaar Naveed' Date='2025-02-02' Version="1.0" Branch="master">Sort grid data</History>*/
        private async Task OnSortData(DataGridColumnSortEventArgs<SectorUsersVM> args)
        {
            if (args.SortOrder != null)
            {
                FilteredUsersList = await gridSearchExtension.Sort(FilteredUsersList, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
            }
            else
            {
                ColumnName = string.Empty;
            }
        }
        #endregion

        #region Delegate User Dialog
        //<History Author = 'Ammaar Naveed' Date='2024-05-16' Version="1.0" Branch="master">Delegate User Dialog (Leave/Absent Users)</History>
        protected async Task DelegateUserDialog(MouseEventArgs args, int? SectorTypeId, string RoleId, string SelectedUserId, int? EmployeeStatusId, int? EmployeeTypeId, int? DesignationId)
        {
            try
            {
                await dialogService.OpenAsync<DelegateUserDialog>
                  (
                      translationState.Translate("Delegate_An_Employee"),
                      new Dictionary<string, object>()
                           {
                             {"SectorTypeId",SectorTypeId },
                             {"RoleId",RoleId },
                             {"SelectedUserId",SelectedUserId },
                             {"Username",loginState.Username },
                             {"EmployeeTypeId",EmployeeTypeId },
                             {"EmployeeStatusId",EmployeeStatusId },
                             {"DesignationId",DesignationId }
                           },
                      new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true }
                  );
                UsersGrid.Reset();
                await UsersGrid.Reload();
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
        #endregion

        #region View Employee Leave Delegation History
        private async Task ViewEmployeeDelegationHistory(MouseEventArgs args, string UserId)
        {
            try
            {
                await dialogService.OpenAsync<EmployeeDelegationHistoryDialog>
                    (
                    translationState.Translate("Employee_Delegation_History"),
                    new Dictionary<string, object>()
                           {
                             {"UserId",UserId },
                           },
                      new DialogOptions() { Width = "55% !important", CloseDialogOnOverlayClick = true, CloseDialogOnEsc = true }

                    );

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
        #endregion

        #region Grid Search
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
                        FilteredUsersList = await gridSearchExtension.Filter(UsersList, new Query()
                        {
                            Filter = $@"i => (i.FullName_En != null && i.FullName_En.ToLower().Contains(@0)) || 
                        (i.RoleName_En != null && i.RoleName_En.ToLower().Contains(@1)) || 
                        (i.DepartmentName_En != null && i.DepartmentName_En.ToLower().Contains(@2)) ||
                        (i.EmployeeStatusEn != null && i.EmployeeStatusEn.ToLower().Contains(@3))",
                            FilterParameters = new object[] { search, search, search, search }
                        });
                    }
                    else
                    {
                        FilteredUsersList = await gridSearchExtension.Filter(UsersList, new Query()
                        {
                            Filter = $@"i => (i.FullName_Ar != null && i.FullName_Ar.ToLower().Contains(@0)) || 
                        (i.RoleName_Ar != null && i.RoleName_Ar.ToLower().Contains(@1)) || 
                        (i.DepartmentName_Ar != null && i.DepartmentName_Ar.ToLower().Contains(@2)) || 
                        (i.EmployeeStatusAr != null && i.EmployeeStatusAr.ToLower().Contains(@3))",
                            FilterParameters = new object[] { search, search, search, search }
                        });
                    }
                    if (!string.IsNullOrEmpty(ColumnName))
                        FilteredUsersList = await gridSearchExtension.Sort(FilteredUsersList, ColumnName, SortOrder);
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

        #region Badrequest Notification
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
