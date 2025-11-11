using FATWA_DOMAIN.Models.Notifications.ViewModel;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace FATWA_GENERAL.Helpers
{
    //<History Author = 'Hassan Abbas' Date='2024-04-24' Version="1.0" Branch="master"> SignalR Notification Hub for sending Notifications to connected users</History>
    public class CustomUserIdProvider : IUserIdProvider
    {
        public string GetUserId(HubConnectionContext connection)
        {
            return connection.User?.FindFirst("Id")?.Value;
        }
    }

    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class NotificationsHub : Hub<INotificationClient>
    {
        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }
    }

    public interface INotificationClient
    {
        Task SendNotification(BellNotificationVM notification);
    }
}
