using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Models.ViewModel;
using DMS_WEB.Data;
using DMS_WEB.Services;
using DMS_WEB.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;
using DMS_WEB.Extensions;

namespace DMS_WEB.Pages.Notifications
{
    public partial class NotificationListComponent : ComponentBase
    {

        #region Service Injections

        [Inject]
        protected NotificationDetailService notificationServices { get; set; }

        [Inject]
        protected SpinnerService spinnerService { get; set; }

        [Inject]
        protected DialogService dialogService { get; set; }

        [Inject]
        protected NotificationService notificationService { get; set; }

        [Inject]
        protected TranslationState translationState { get; set; }

        [Inject]
        protected LoginState loginState { get; set; }

        [Inject]
        protected NavigationManager navigationManager { get; set; }

        [Inject]
        protected SystemSettingState systemSettingState { get; set; }
        [Inject]
        protected RadzenGridSearchExtension gridSearchExtension { get; set; }
        //[Inject]
        //protected NavigationManager navigationManager { get; set; }

        #endregion

        #region Variables Declarations

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
                    var args = new PropertyChangedEventArgs() { Name = "search", NewValue = value, OldValue = _search };
                    _search = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
        protected RadzenDataGrid<NotificationVM> grid = new RadzenDataGrid<NotificationVM>();

        protected IEnumerable<NotificationVM> getNotificationList { get; set; }
        protected IEnumerable<NotificationVM> FilteredGetNotificationList { get; set; }
        protected IEnumerable<NotificationVM> currentNotifications { get; set; }
        protected IEnumerable<NotificationVM> oldNotifications { get; set; }

        protected List<string> notifStatus = new List<string>() { "Current", "Old" };

        #endregion

        #region On Load

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            translationState.TranslateGridFilterLabels(grid);

            spinnerService.Hide();
        }

        protected async Task Load()
        {
            var response = await notificationServices.GetNotifNotificationDetails();
            if (response.IsSuccessStatusCode)
            {
                var notification = (List<NotificationVM>)response.ResultData;
                IQueryable<NotificationVM> notifications = notification.AsQueryable();
                notificationServices.GetNotificationList(notifications);
                var oldNotifDate = DateTime.Now.Date.AddDays(-30);
                oldNotifications = notifications.Where(x => x.CreatedDate < oldNotifDate);
                currentNotifications = notifications.Where(x => x.CreatedDate >= oldNotifDate);
                await OnTabChange(selectedTabIndex);
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
        protected async Task OnSearchInput()
        {
            try
            {
                if (string.IsNullOrEmpty(search))
                {
                    search = "";
                }
                else
                {
                    search = search.ToLower();
                }
                string Filter;
                if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                {
                    Filter = $@"i => (i.SubjectEn != null && i.SubjectEn.ToLower().Contains(@0)) || (i.EventNameEn != null && i.EventNameEn.ToLower().Contains(@1))|| (i.CatNameEn != null && i.CatNameEn.ToLower().Contains(@2)) || (i.ModuleNameEn != null && i.ModuleNameEn.ToLower().Contains(@3))";
                }
                else
                {
                    Filter = $@"i => (i.SubjectAr != null && i.SubjectAr.ToLower().Contains(@0)) || (i.EventNameAr != null && i.EventNameAr.ToLower().Contains(@1))|| (i.CatNameAr != null && i.CatNameAr.ToLower().Contains(@2)) || (i.ModuleNameAr != null && i.ModuleNameAr.ToLower().Contains(@3))";
                }
                FilteredGetNotificationList = await gridSearchExtension.Filter(getNotificationList,new Query()
                {
                    Filter = Filter,
                    FilterParameters = new object[] { search, search, search, search }
                });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Functions

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
                await Load();
                StateHasChanged();
            }
        }

        protected async Task DetailNotification(NotificationVM item)
        {
            try
            {
                CheckNotificationVM notification = new CheckNotificationVM()
                {
                    notificationStatus = (int)NotificationStatusEnum.Read,
                    NotificationReadDate = DateTime.Now,
                    notificationIds = new List<Guid>()
                    {
                        item.NotificationId
                    }
                };

                ApiCallResponse response = await notificationServices.CheckNotification(notification);
                if (response.ResultData == null)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Contact_Administrator"),
                        Style = "position: fixed !important; left: 0; margin: auto;"
                    });
                }
                else
                {
                    var dialogResult = await dialogService.OpenAsync<NotificationViewDetail>(
                                translationState.Translate("Notification"),
                                new Dictionary<string, object>() { { "NotficationDetailId", item.NotificationId } },
                                new DialogOptions() 
                                { 
                                    Width = "1000px !important", 
                                    CloseDialogOnOverlayClick = true,  
                                });
					var currentUrl = navigationManager.Uri;
					if (currentUrl.Contains("/notifications"))
					{
						navigationManager.NavigateTo("/notifications", forceLoad: true);
					}

					await Task.Delay(400);
					await Load();

				}
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

        protected void RowCellRender(RowRenderEventArgs<NotificationVM> notification)
        {
            if (notification.Data.NotificationStatus != (int)NotificationStatusEnum.Read)
            {
                notification.Attributes.Add("style", $"background-color: #e3d4c9;");
            }
        }

        #region TabChange

        public int count { get; set; } = 0;
        protected int selectedTabIndex { get; set; } = 0;
        protected async Task OnTabChange(int index)
        {
            await Task.Delay(100);
            grid.Reset();
            getNotificationList = null;
            FilteredGetNotificationList = null;
            if (index == 0)
            {
                getNotificationList = currentNotifications;
                FilteredGetNotificationList = currentNotifications;
            }
            else
            {
                getNotificationList = oldNotifications;
                FilteredGetNotificationList = oldNotifications;
            }
            count = getNotificationList.Count();

            Reload();
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
    }
}
