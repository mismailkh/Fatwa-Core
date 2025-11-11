using FATWA_DOMAIN.Models.Notifications.ViewModel;
using DMS_WEB.Pages.Notifications;
using Microsoft.AspNetCore.Components;
using Radzen;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;

namespace DMS_WEB.Shared
{
    public partial class BellNotification : ComponentBase
    {
        #region Variables

        public IEnumerable<BellNotificationVM> getBellNotifNotifications { get; set; }
        public int? notificationCount { get; set; }

        #endregion

        #region On Load

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();

            await Load();

            spinnerService.Hide();
        }

        public async Task Load()
        {
            try
            {
                var result = await notificationDetailService.GetBellNotifications();
                if (result.IsSuccessStatusCode)
                {
                    getBellNotifNotifications = (List<BellNotificationVM>)result.ResultData;
                    notificationCount = getBellNotifNotifications.Count();

                    getBellNotifNotifications = (from item in getBellNotifNotifications
                                                 select item).Take(10);

                    CreateNotificationMessage();
                }
                else
                {
                    notificationCount = getBellNotifNotifications.Count();

                    getBellNotifNotifications = new List<BellNotificationVM>();
                }

            }
            catch
            {
                throw;
            }
        }

        protected void CreateNotificationMessage()
        {
            foreach (var bellNotification in getBellNotifNotifications)
            {
                string entity = bellNotification.Url.Split('/')[1].Split('-')[0];
                entity = translationState.Translate(entity);

                string entityId = bellNotification.Url.Split('/')[2];

                //For Notification With Some Number in their Entity
                if (translationState.Translate(bellNotification.NotificationMessageEn).Contains("#entityId#"))
                    bellNotification.NotificationMessageEn = translationState.Translate(bellNotification.NotificationMessageEn).Replace("#entityId#", entityId);
                //For Notification With Name as their Entity 
                else
                    bellNotification.NotificationMessageEn = translationState.Translate(bellNotification.NotificationMessageEn).Replace("#entity#", entity);

                int maxLength = 55;
                if (bellNotification.NotificationMessageEn.Length > maxLength)
                {
                    var tmp = bellNotification.NotificationMessageEn.Substring(0, maxLength);
                    if (tmp.LastIndexOf(' ') > 0)
                        bellNotification.BellNotificationMessageEn = tmp.Substring(0, tmp.LastIndexOf(' ')) + " ...";
                }
                else 
                    bellNotification.BellNotificationMessageEn = bellNotification.NotificationMessageEn; 
            }
        }

        #endregion

        #region Functions

        protected void ButtonViewAll()
        {
            try
            {
                navigationManager.NavigateTo("/notifications");
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected async Task ButtonReadAllAsync()
        {
            try
            {
                CheckNotificationVM notification = new CheckNotificationVM()
                {
                    notificationStatus = (int)NotificationStatusEnum.Read,
                    NotificationReadDate = DateTime.Now,
                    notificationIds = new List<Guid>()
                };

                foreach (BellNotificationVM bellNotification in getBellNotifNotifications)
                {
                    notification.notificationIds.Add(bellNotification.NotificationId);
                }

                var result = await notificationDetailService.CheckNotification(notification);
                if (result.IsSuccessStatusCode)
                {
                    await Load();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected async Task DetailNotification(Guid notificationId)
        {
            try
            {
                CheckNotificationVM notification = new CheckNotificationVM()
                {
                    notificationStatus = (int)NotificationStatusEnum.Read,
                    NotificationReadDate = DateTime.Now,
                    notificationIds = new List<Guid>()
                    {
                        notificationId
                    }
                };

                ApiCallResponse response = await notificationDetailService.CheckNotification(notification);
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
                                translationState.Translate("Back_To_Notifications"),
                                new Dictionary<string, object>() { { "NotficationDetailId", notificationId } },
                                new DialogOptions() { Width = "900px", CloseDialogOnOverlayClick = true });

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

        #endregion
    }
}
