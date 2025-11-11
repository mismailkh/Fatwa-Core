using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.WorkflowModels;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.Notifications
{
    public partial class NotificationList : ComponentBase
    {
        #region Variables Declarations

        protected string search { get; set; }

        protected RadzenDataGrid<NotificationVM> gridCurrent = new RadzenDataGrid<NotificationVM>();
        protected RadzenDataGrid<NotificationVM> gridOld = new RadzenDataGrid<NotificationVM>();
        protected IEnumerable<NotificationVM> getNotificationList { get; set; } = new List<NotificationVM>();
        protected IEnumerable<NotificationVM> FilteredNotificationList { get; set; }
        protected NotificationListAdvanceSearchVM advanceSearchVM { get; set; } = new NotificationListAdvanceSearchVM();
        protected static IEnumerable<Module> Modules = new List<Module>();
        protected static IEnumerable<NotificationEvent> Events = new List<NotificationEvent>();
        protected bool Keywords = false;
        public bool isVisible { get; set; }
        protected int selectedTabIndex { get; set; } = 0;
        private int CurrentPage { get; set; }
        private int CurrentPageSize { get; set; }
        private string? ColumnName { get; set; }
        private SortOrder SortOrder { get; set; }
        #endregion

        #region On Load

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await LoadModules();
            await LoadEvents();
            advanceSearchVM.PageSize = systemSettingState.Grid_Pagination;
            translationState.TranslateGridFilterLabels(gridOld);
            translationState.TranslateGridFilterLabels(gridCurrent);
            spinnerService.Hide();
        }
        #region On Load Grid Data
        private async Task OnLoadData(LoadDataArgs args)
        {
            try
            {
                if (selectedTabIndex == 0)
                {
                    CurrentPage = gridCurrent.CurrentPage + 1;
                    CurrentPageSize = gridCurrent.PageSize;
                }
                else
                {
                    CurrentPage = gridOld.CurrentPage + 1;
                    CurrentPageSize = gridOld.PageSize;
                }

                if (string.IsNullOrEmpty(args.OrderBy) || CurrentPage != advanceSearchVM.PageNumber || CurrentPageSize != advanceSearchVM.PageSize || (Keywords && advanceSearchVM.isDataSorted))
                {
                    if (advanceSearchVM.isGridLoaded && advanceSearchVM.PageSize == CurrentPageSize && !advanceSearchVM.isPageSizeChangeOnFirstLastPage)
                    {
                        if (selectedTabIndex == 0)
                            gridCurrent.CurrentPage = (int)advanceSearchVM.PageNumber - 1;
                        else
                            gridOld.CurrentPage = (int)advanceSearchVM.PageNumber - 1;
                        advanceSearchVM.isGridLoaded = false;
                        return;
                    }
                    SetPagingProperties(args);
                    if (selectedTabIndex == 0)
                        advanceSearchVM.IsLatest = true;
                    else
                        advanceSearchVM.IsLatest = false;
                    spinnerService.Show();
                    var response = await notificationServices.GetNotifNotificationDetails(advanceSearchVM);
                    if (response.IsSuccessStatusCode)
                    {
                        FilteredNotificationList = getNotificationList = (List<NotificationVM>)response.ResultData;
                        if (!(string.IsNullOrEmpty(search))) await OnSearchInput();
                        if (!string.IsNullOrEmpty(args.OrderBy) && string.IsNullOrEmpty(search))
                        {
                            FilteredNotificationList = await gridSearchExtension.Sort(FilteredNotificationList, ColumnName, SortOrder);
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
        private void SetPagingProperties(LoadDataArgs args)
        {
            if (advanceSearchVM.PageSize != null && advanceSearchVM.PageSize != CurrentPageSize)
            {
                int oldPageCount = getNotificationList.Any() ? (getNotificationList.First().TotalCount) / ((int)advanceSearchVM.PageSize) : 1;
                int oldPageNumber = (int)advanceSearchVM.PageNumber - 1;
                advanceSearchVM.isGridLoaded = true;
                advanceSearchVM.PageNumber = CurrentPage;
                advanceSearchVM.PageSize = args.Top;
                int TotalPages = getNotificationList.Any() ? (getNotificationList.First().TotalCount) / (selectedTabIndex == 0 ? gridCurrent.PageSize : gridOld.PageSize) : 1;
                if (CurrentPage > TotalPages)
                {
                    advanceSearchVM.PageNumber = TotalPages + 1;
                    advanceSearchVM.PageSize = args.Top;
                    if (selectedTabIndex == 0)
                        gridCurrent.CurrentPage = TotalPages;
                    else
                        gridOld.CurrentPage = TotalPages;
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


        private async Task OnSort(DataGridColumnSortEventArgs<NotificationVM> args)
        {
            if (args.SortOrder != null)
            {
                FilteredNotificationList = await gridSearchExtension.Sort(FilteredNotificationList, args.Column.Property, (SortOrder)args.SortOrder);
                ColumnName = args.Column.Property;
                SortOrder = (SortOrder)args.SortOrder;
                advanceSearchVM.isDataSorted = false;
            }
            else
            {
                ColumnName = string.Empty;
            }
        }
        protected async Task OnSearchInput()
        {
            try
            {
                search = string.IsNullOrEmpty(search) ? "" : search.TrimStart().TrimEnd().ToLower();
                FilteredNotificationList = await gridSearchExtension.Filter(getNotificationList, new Query()
                {
                    Filter = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ?
                           $@"i => (i.SubjectEn != null && i.SubjectEn.ToLower().Contains(@0))
                            || (i.EventNameEn != null && i.EventNameEn.ToLower().Contains(@0))
                            || (i.NotificationMessageEn != null && i.NotificationMessageEn.ToLower().Contains(@0))
                            || (i.CreatedDate.HasValue && i.CreatedDate.Value.ToString(""dd/MM/yyyy"").Contains(@0)) 
                            || (i.ModuleNameEn != null && i.ModuleNameEn.ToLower().Contains(@0))" :
                            $@"i => (i.SubjectAr != null && i.SubjectAr.ToLower().Contains(@0)) 
                            || (i.EventNameAr != null && i.EventNameAr.ToLower().Contains(@0)) 
                            || (i.NotificationMessageAr != null && i.NotificationMessageAr.ToLower().Contains(@0))
                            || (i.CreatedDate.HasValue && i.CreatedDate.Value.ToString(""dd/MM/yyyy"").Contains(@0)) 
                            || (i.ModuleNameAr != null && i.ModuleNameAr.ToLower().Contains(@0))",
                    FilterParameters = new object[] { search }
                });
                FilteredNotificationList = await gridSearchExtension.Sort(FilteredNotificationList, ColumnName, SortOrder);

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

        #region Functions

        protected async Task LoadModules()
        {
            var response = await lookupService.GetModules();
            if (response.IsSuccessStatusCode)
            {
                Modules = (List<Module>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        protected async Task LoadEvents()
        {
            var response = await lookupService.GetEvents();
            if (response.IsSuccessStatusCode)
            {
                Events = (List<NotificationEvent>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        public void GetNotificationList(IQueryable<NotificationVM> notifications)
        {
            try
            {
                foreach (NotificationVM notification in notifications)
                {
                    string[] linkSegments = notification.NotificationLink.Split('/');

                    if (linkSegments.Length >= 3)
                    {
                        string entity = linkSegments[1].Split('-')[0].ToUpper();
                        string entityId = linkSegments[2];
                        string transValue = translationState.Translate(entity);

                        if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                        {
                            //For Notification With Some Number in their Entity  
                            notification.SubjectEn = notification.SubjectEn.Replace(", #entityId#", "");
                            notification.SubjectEn = notification.SubjectEn.Replace("#entity#", "");
                        }
                        else
                        {
                            notification.SubjectAr = notification.SubjectAr.Replace("? #entityId#", "");
                            notification.SubjectAr = notification.SubjectAr.Replace("#entity#", "");
                        }
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

        protected async Task DeleteNotification(NotificationVM item)
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Delete_Notification"), translationState.Translate("delete"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                await notificationServices.DeleteNotification(item);
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Success,
                    Detail = translationState.Translate("Delete_Notification_Success"),
                    Style = "position: fixed !important; left: 0; margin: auto;"
                });
                if (selectedTabIndex == 0)
                {
                    await gridCurrent.Reload();
                }
                else
                {
                    await gridOld.Reload();
                }
                StateHasChanged();
            }
        }

        protected async Task DetailNotification(NotificationVM notification)
        {
            try
            {
                ApiCallResponse response = await notificationServices.MarkNotificationAsRead(notification.NotificationId);
                if (response.IsSuccessStatusCode)
                {
                    navigationManager.NavigateTo(notification.NotificationLink, true);
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Contact_Administrator"),
                        Style = "position: fixed !important; left: 0; margin: auto;"
                    });
                }
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto;"
                });
            }

        }

        protected void RowCellRender(RowRenderEventArgs<NotificationVM> notification)
        {
            if (notification.Data.NotificationStatus != (int)NotificationStatusEnum.Read)
            {
                notification.Attributes.Add("style", $"background-color: #e3d4c9;");
            }
        }

        #region TabChange

        protected async Task OnTabChange(int index)
        {
            if (index == selectedTabIndex) { return; }
            selectedTabIndex = index;
            search = ColumnName = string.Empty;
            isVisible = Keywords = false;
            advanceSearchVM = new NotificationListAdvanceSearchVM { PageSize = selectedTabIndex == 0 ? gridCurrent.PageSize : gridOld.PageSize };
            await Task.Delay(100);
            if (selectedTabIndex == 0)
            {
                gridCurrent.Reset();
                await gridCurrent.Reload();
            }
            else
            {
                gridOld.Reset();
                await gridOld.Reload();
            }
            StateHasChanged();
        }

        #endregion

        #endregion

        #region Redirect Function

        protected void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        protected void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion

        #region AdvanceSearch
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
                Keywords = advanceSearchVM.isDataSorted = true;
                return;
            }
            if (!advanceSearchVM.ModuleId.HasValue && !advanceSearchVM.EventId.HasValue && !advanceSearchVM.CreatedFrom.HasValue
                && !advanceSearchVM.CreatedTo.HasValue)
            {
            }
            else
            {
                Keywords = advanceSearchVM.isDataSorted = true;

                if (selectedTabIndex == 0)
                {
                    if (gridCurrent.CurrentPage > 0)
                    {
                        await gridCurrent.FirstPage();
                    }
                    else
                    {
                        advanceSearchVM.isGridLoaded = false;
                        await gridCurrent.Reload();
                    }
                }
                else
                {
                    if (gridOld.CurrentPage > 0)
                    {
                        await gridOld.FirstPage();
                    }
                    else
                    {
                        advanceSearchVM.isGridLoaded = false;
                        await gridOld.Reload();
                    }
                }
            }
        }

        protected async void ToggleAdvanceSearch()
        {
            isVisible = !isVisible;
            if (!isVisible)
            {
                ResetForm();
            }
        }
        public async void ResetForm()
        {
            advanceSearchVM = new NotificationListAdvanceSearchVM { PageSize = selectedTabIndex == 0 ? gridCurrent.PageSize : gridOld.PageSize };
            Keywords = advanceSearchVM.isDataSorted = false;
            await Task.Delay(100);
            if (selectedTabIndex == 0)
            {
                gridCurrent.Reset();
                await gridCurrent.Reload();
            }
            else
            {
                gridOld.Reset();
                await gridOld.Reload();
            }
            StateHasChanged();
        }
        #endregion
    }
}
