using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;

namespace FATWA_DOMAIN.Interfaces.Notification
{
    public interface INotification
    {
        Task<List<NotificationVM>> GetNotifNotificationDetails(NotificationListAdvanceSearchVM advanceSearchVM);
        Task<List<BellNotificationVM>> GetBellNotifications(string userId, int NotificationStatusId, int channelId);
        Task<FATWA_DOMAIN.Models.Notifications.Notification> GetNotificationById(Guid NotificationId);
        Task<NotificationDetailVM> GetNotificationDetailView(Guid NotificationId, string user);
        Task DeleteNotification(NotificationVM item);
        Task<bool> SendNotification(FATWA_DOMAIN.Models.Notifications.Notification notification, int eventId, string action, string entityName, string entityId, NotificationParameter notificationParameter);
        Task MarkNotificationAsRead(List<Guid> notificationIds);
        Task<bool> MarkNotificationAsRead(Guid notificationId);
        Task<bool> DeleteNotificationByEntityAndId(string entityName, string entityId, string deletedBy, int deleteAt, DateTime createdDate);
        Task<IEnumerable<NotificationEventListVM>> GetEventList();
        Task<List<NotificationEvent>> GetEvent();
        Task<List<NotificationChannel>> GetChannel();
        Task<IEnumerable<NotificationEventPlaceholders>> GetPlaceHoldersByEventId(int EventId);
        Task<bool> CreateNotificationEventTemplate(NotificationTemplate item);
        Task<NotificationTemplate> GetNotificationEventTemplateById(Guid Id);
        Task<bool> UpdateNotificationEventTemplate(NotificationTemplate template);
        Task<IEnumerable<NotificationTemplateListVM>> GetTemplateListByEventId(int eventId);
        Task<bool> DeleteEventTemplate(NotificationTemplateListVM template);
        //Task<BellNotificationVM> GetNotificationByNotificationId(Guid NotificationId , string user); 
        Task DeleteAllNotification(List<Guid> notificationIds, String userId);
        Task<bool> UpdateEventStatus(NotificationEventListVM Event);
        Task<NotificationEvent> GetNotificationEventByEventId(int EventId);
        Task EditNotificationEvent(NotificationEvent template);
        Task<List<NotificationReceiverType>> GetReceiverType();
        Task<string> SendMultipleDevicePushNotificationFCM(string userId, FATWA_DOMAIN.Models.Notifications.Notification notification);
        Task<string> SendPushNotificationFCM(string userId, FATWA_DOMAIN.Models.Notifications.Notification notification);
        Task<bool> SendNotificationList(List<FATWA_DOMAIN.Models.Notifications.Notification> notifications);
        Task<bool> UpdateNotificationUrl(Guid Id, string Url);

    }
}
