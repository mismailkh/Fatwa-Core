using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;
using Radzen;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_ADMIN.Shared.Components
{
    public partial class BellNotification : ComponentBase
    {
        #region Variables
        private HubConnection? _hubConnectionFatwa;
        private HubConnection? _hubConnectionOSS;
        public IList<BellNotificationVM> getBellNotifNotifications { get; set; }
        public IEnumerable<BellNotificationVM> getReadBellNotifNotifications { get; set; }
        public int? notificationCount { get; set; }
        Dictionary<string, List<BellNotificationVM>> groupedNotifications;
        NotificationDetailVM _notificationDetailVM;
        protected NotificationDetailVM notificationDetailVM
        {
            get
            {
                return _notificationDetailVM;
            }
            set
            {
                if (!object.Equals(_notificationDetailVM, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "notificationDetailVM", NewValue = value, OldValue = _notificationDetailVM };
                    _notificationDetailVM = value;
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

        #endregion

        #region On Load

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            spinnerService.Hide();
            await InitiateNotificationHubConnection();
        }

        public async ValueTask DisposeAsync()
        {
            if (_hubConnectionFatwa != null)
            {
                await _hubConnectionFatwa.DisposeAsync();
            }
            if (_hubConnectionOSS != null)
            {
                await _hubConnectionOSS.DisposeAsync();
            }
        }
        public async Task InitiateNotificationHubConnection()
        {
            try
            {
                _hubConnectionFatwa = new HubConnectionBuilder()
                .WithUrl(_config.GetValue<string>("fatwa_notification_hub_url"),
                o => o.AccessTokenProvider = () => Task.FromResult<string>(loginState.Token))
                .Build();

                _hubConnectionFatwa.On<BellNotificationVM>("SendNotification", async notification =>
                {
                    getBellNotifNotifications.Insert(0, notification);
                    notificationCount = getBellNotifNotifications.Count();
                    getBellNotifNotifications = (from item in getBellNotifNotifications select item).Take(200).ToList();
                    groupedNotifications = getBellNotifNotifications
                        .GroupBy(notification => GetGroupingKey(notification.CreationDate))
                        .OrderByDescending(x => x.Key)
                        .ToDictionary(x => x.Key, x => x.ToList());
                    StateHasChanged();
                });
                await _hubConnectionFatwa.StartAsync();
            }
            catch (Exception ex)
            {

            }
            try
            {
                _hubConnectionOSS = new HubConnectionBuilder()
                .WithUrl(_config.GetValue<string>("oss_notification_hub_url"),
                o => o.AccessTokenProvider = () => Task.FromResult<string>(loginState.Token))
                .Build();

                _hubConnectionOSS.On<BellNotificationVM>("SendNotification", async notification =>
                {
                    getBellNotifNotifications.Insert(0, notification);
                    notificationCount = getBellNotifNotifications.Count();
                    getBellNotifNotifications = (from item in getBellNotifNotifications select item).Take(200).ToList();
                    groupedNotifications = getBellNotifNotifications
                        .GroupBy(notification => GetGroupingKey(notification.CreationDate))
                        .OrderByDescending(x => x.Key)
                        .ToDictionary(x => x.Key, x => x.ToList());
                    StateHasChanged();
                });
                await _hubConnectionOSS.StartAsync();
            }
            catch (Exception ex)
            {

            }
        }

        public async Task Load()
        {
            try
            {
                var result = await notificationDetailService.GetBellNotifications((int)NotificationStatusEnum.Unread);
                if (result.IsSuccessStatusCode)
                {
                    getBellNotifNotifications = JsonConvert.DeserializeObject<List<BellNotificationVM>>(result.ResultData.ToString());
                    notificationCount = getBellNotifNotifications.Count();
                    getBellNotifNotifications = (from item in getBellNotifNotifications select item).Take(200).ToList();
                    groupedNotifications = getBellNotifNotifications
                        .GroupBy(notification => GetGroupingKey(notification.CreationDate))
                        .OrderByDescending(x => x.Key) // Order By setting
                        .ToDictionary(x => x.Key, x => x.ToList()); // storin into my Dictionary
                }
                else
                {
                    //notificationCount = getBellNotifNotifications.Count();
                    //getBellNotifNotifications = new List<BellNotificationVM>();
                }

            }
            catch
            {
                throw;
            }
        }

        // Author : Attique Ur Rehman  Date : 18/ April/2024 << " Get Grouping/ Catogaries for Notifications according creation datetime, If you need further grouping Just calculate the datetime and return The Key "
        string GetGroupingKey(DateTime date)
        {
            var today = DateTime.Today;
            var yesterday = today.AddDays(-1);
            var thisMonth = new DateTime(today.Year, today.Month, 1);
            var lastMonth = thisMonth.AddMonths(-1);

            if (date.Date == today)
            {
                return "z_Today";
            }
            if (date.Date == yesterday)
            {
                return "Yesterday";
            }
            else if (date >= thisMonth)
            {
                return "This Month";
            }
            else if (date >= lastMonth)
            {
                return "Last Month";
            }
            else
            {
                return ("elderly");
                //return date.ToString("MMM yyyy");
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
            if (await dialogService.Confirm(translationState.Translate("Sure_Want_To_Mark_All_Read"), translationState.Translate("Read"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                try
                {
                    var notificationIds = new List<Guid>();
                    spinnerService.Show();
                    foreach (BellNotificationVM bellNotification in getBellNotifNotifications)
                    {
                        notificationIds.Add(bellNotification.NotificationId);
                    }

                    var result = await notificationDetailService.MarkNotificationAsRead(notificationIds);
                    if (result.IsSuccessStatusCode)
                    {
                        await Load();
                    }
                    spinnerService.Hide();
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        protected async Task RedirecttoNotification()
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

        protected async Task DetailNotification(BellNotificationVM notification)
        {
            try
            {
                ApiCallResponse response = await notificationDetailService.MarkNotificationAsRead(notification.NotificationId);
                if (response.IsSuccessStatusCode)
                {
                    await Load();
                    navigationManager.NavigateTo(notification.Url);
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
        protected async Task ReadNotification(BellNotificationVM notification, bool isRead)
        {
            try
            {
                ApiCallResponse response = await notificationDetailService.MarkNotificationAsRead(notification.NotificationId);
                if (response.IsSuccessStatusCode)
                {
                    await Load();
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
        #endregion
    }
}
