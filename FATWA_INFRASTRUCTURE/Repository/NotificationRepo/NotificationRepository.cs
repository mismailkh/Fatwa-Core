using FATWA_DOMAIN.Common.Service;
using FATWA_DOMAIN.Enums;
using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Interfaces.Notification;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.Email;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_INFRASTRUCTURE.Database;
using FATWA_INFRASTRUCTURE.Resources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using MimeKit.Text;
using System.CodeDom;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_GENERAL.Helper.Enum;
using FATWA_GENERAL.Helpers;
using Microsoft.AspNetCore.SignalR;

namespace FATWA_INFRASTRUCTURE.Repository.NotificationRepo
{
    public class NotificationRepository : INotification
    {
        private readonly DatabaseContext _dbContext;
        private List<NotificationVM> _Notif_Notification;
        private List<BellNotificationVM> _BellNotification;
        private NotificationDetailVM _NotificationDetail;
        private IEnumerable<NotificationEventListVM> _NotificationEvent;
        private IEnumerable<NotificationTemplateListVM> _NotificationTemplateList;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly CustomResourceManager _resourceManager;
        string storedProc;
        private IEmailService _emailService;
        private readonly IHubContext<NotificationsHub, INotificationClient> _notificationSignalRClient;
        public NotificationRepository(DatabaseContext dbContext, IServiceScopeFactory serviceScopeFactory, IEmailService emailService, IHubContext<NotificationsHub, INotificationClient> notificationSignalRClient)
        {
            _dbContext = dbContext;
            _serviceScopeFactory = serviceScopeFactory;
            _emailService = emailService;
            _resourceManager = new CustomResourceManager("FATWA_INFRASTRUCTURE.Resources.NotificationResources", typeof(NotificationRepository).Assembly);
            _notificationSignalRClient = notificationSignalRClient;
        }

        #region GET NOTIFICATION

        public async Task<List<NotificationVM>> GetNotifNotificationDetails(NotificationListAdvanceSearchVM advanceSearchVM)
        {
            try
            {
                if (_Notif_Notification == null)
                {
                    string fromDate = advanceSearchVM.CreatedFrom != null ? Convert.ToDateTime(advanceSearchVM.CreatedFrom).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    string toDate = advanceSearchVM.CreatedTo != null ? Convert.ToDateTime(advanceSearchVM.CreatedTo).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    string storedProc = $"exec pNotificationList @UserId='{advanceSearchVM.UserId}',@ModuleId='{advanceSearchVM.ModuleId}',@EventId='{advanceSearchVM.EventId}',@FromDate='{fromDate}', @ToDate='{toDate}'" +
                        $" ,@IsLatest = '{advanceSearchVM.IsLatest}',@PageNumber ='{advanceSearchVM.PageNumber}',@PageSize ='{advanceSearchVM.PageSize}'";
                    _Notif_Notification = await _dbContext.NotifNotificationVMs.FromSqlRaw(storedProc).ToListAsync();
                }
                return _Notif_Notification;
            }
            catch
            {
                throw;
            }


        }

        public async Task<List<BellNotificationVM>> GetBellNotifications(string userId, int NotificationStatusId, int channelId)
        {
            try
            {
                if (_BellNotification == null)
                {
                    storedProc = "exec pBellNotifications @UserId = '" + userId + "' , @notificationStatusId ='" + NotificationStatusId + "' ";
                    _BellNotification = await _dbContext.BellNotificationVMs.FromSqlRaw(storedProc).ToListAsync();
                }
                return _BellNotification;
            }
            catch
            {
                throw;
            }
        }

        public async Task<NotificationDetailVM> GetNotificationDetailView(Guid NotificationId, string user)
        {
            try
            {
                if (_NotificationDetail == null)
                {
                    storedProc = $"exec pNotificationdetailView @NotificationId = '{NotificationId}', @UserId = '{user}' ";
                    var result = await _dbContext.NotificationDetailVMs.FromSqlRaw(storedProc).ToListAsync();
                    if (result != null)
                        _NotificationDetail = result.FirstOrDefault();
                }
                return _NotificationDetail;
            }
            catch
            {
                throw;
            }


        }

        public async Task<Notification> GetNotificationById(Guid notificationId)
        {
            try
            {
                Notification? notificationDetail = await _dbContext.NotifNotifications.FindAsync(notificationId);
                if (notificationDetail != null)
                {
                    return notificationDetail;
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch
            {
                throw;
            }
        }

        //public async Task<bool> CheckNotification(CheckNotificationVM notification)
        //{
        //    bool isSaved;
        //    try
        //    {
        //        List<NotificationUser> notifications = new List<NotificationUser>();

        //        foreach (Guid notificationId in notification.notificationIds)
        //        {
        //            var result = await _dbContext.NotificationUsers.Where(x => x.NotificationId == notificationId).FirstOrDefaultAsync();
        //            if (result != null)
        //            {
        //                result.NotificationStatusId = notification.notificationStatus;
        //                result.ReadDate = notification.NotificationReadDate;

        //                notifications.Add(result);
        //            }
        //        }

        //        _dbContext.UpdateRange(notifications);
        //        await _dbContext.SaveChangesAsync();

        //        isSaved = true;
        //    }
        //    catch
        //    {
        //        isSaved = false;

        //    }
        //    return isSaved;
        //}


        public async Task MarkNotificationAsRead(List<Guid> notificationIds)
        {
            foreach (var notificationId in notificationIds)
            {
                await MarkNotificationAsRead(notificationId);
            }
        }

        public async Task<bool> MarkNotificationAsRead(Guid notificationId)
        {
            bool isSaved = false;
            try
            {

                var result = await _dbContext.NotifNotifications.Where(x => x.NotificationId == notificationId).FirstOrDefaultAsync();
                if (result != null)
                {
                    result.NotificationStatusId = (int)NotificationStatusEnum.Read;
                    result.ReadDate = DateTime.Now;
                    _dbContext.Update(result);
                    await _dbContext.SaveChangesAsync();
                    isSaved = true;
                }
            }
            catch
            {
                isSaved = false;

            }
            return isSaved;
        }

        #endregion

        #region DELETE NOTIFICATION

        public async Task DeleteNotification(NotificationVM item)
        {
            try
            {
                Notification? notification = await _dbContext.NotifNotifications.FindAsync(item.NotificationId);
                if (notification != null)
                {
                    notification.DeletedBy = item.DeletedBy;
                    notification.DeletedDate = DateTime.Now;
                    notification.IsDeleted = true;

                    _dbContext.Entry(notification).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch
            {
                _dbContext.Entry(_dbContext).State = EntityState.Unchanged;
                throw;
            }
        }

        public async Task<bool> DeleteNotificationByEntityAndId(string entityName, string entityId, string deletedBy, int deleteAt, DateTime createdDate)
        {
            bool isSaved = false;
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                using (_dbContext)
                {
                    var notifications = await _dbContext.NotifNotifications.Where(x => x.CreatedDate == createdDate && x.IsDeleted == false).ToListAsync();
                    if (notifications.Count() != 0)
                    {
                        foreach (var item in notifications)
                        {
                            item.DeletedBy = deletedBy;
                            item.DeletedDate = DateTime.Now;
                            item.IsDeleted = true;
                            item.NotificationStatusId = (int)NotificationStatusEnum.Read;
                            _dbContext.Entry(item).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                        }
                        isSaved = true;
                    }
                    else
                    {
                        isSaved = true;
                    }
                }
            }
            catch
            {
                isSaved = false;
            }
            return isSaved;
        }


        #endregion

        #region SEND NOTIFICATION

        public async Task<bool> SendNotification(Notification notification, int eventId, string action, string entityName, string entityId, NotificationParameter notificationParameter)
        {
            bool isSaved = false;

            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            using (_DbContext)
            {
                using (var transaction = _DbContext.Database.BeginTransaction())
                {
                    try
                    {
                        if (await _DbContext.NotificationEvents.Where(x => x.EventId == eventId && x.IsActive).AnyAsync())
                        {
                            var loggedUser = await _DbContext.Users.Where(x => x.UserName == notification.CreatedBy).FirstOrDefaultAsync();
                            if (loggedUser != null)
                            {
                                notification.SenderId = loggedUser.Id;
                                notificationParameter.SenderName = await _DbContext.UserPersonalInformation.Where(x => x.UserId == notification.SenderId).Select(x => x.FirstName_En + " " + x.LastName_En + "/" + x.FirstName_Ar + " " + x.LastName_Ar).FirstOrDefaultAsync();
                            }
                            notificationParameter.ReceiverName = await _DbContext.UserPersonalInformation.Where(x => x.UserId == notification.ReceiverId).Select(x => x.FirstName_En + " " + x.LastName_En + "/" + x.FirstName_Ar + " " + x.LastName_Ar).FirstOrDefaultAsync();
                            notificationParameter.CreatedDate = notification.CreatedDate;

                            // NotificationTemplates
                            var notificationTemplates = await _DbContext.NotificationTemplates.Where(x => x.EventId == eventId && x.isActive).ToListAsync();
                            notification.NotificationURL = await CreateNotificationURL(action, entityName, entityId);
                            foreach (var notificationTemplate in notificationTemplates)
                            {
                                notification.NotificationTemplateId = notificationTemplate.TemplateId;
                                (notification.NotificationMessageEn, notification.NotificationMessageAr) = await CreateNotificationMessage(notificationTemplate, notificationParameter, _DbContext);
                                switch (notificationTemplate.ChannelId)
                                {
                                    case (int)NotificationChannelEnum.Email:
                                        isSaved = await SendEmailNotification(notification, notificationTemplate.SubjectAr, _DbContext);
                                        break;
                                    case (int)NotificationChannelEnum.Mobile:
                                        isSaved = await SendSmsNotification(notification);
                                        break;
                                    default:
                                        isSaved = await SendBrowserNotification(notification, _DbContext);
                                        break;
                                }
                            }
                            if (eventId == (int)NotificationEventEnum.NewRequest || eventId == (int)NotificationEventEnum.AssignToLawyer || eventId == (int)NotificationEventEnum.AttendeeDecisionForMeeting)
                            {
                                var response = await SendMultipleDevicePushNotificationFCM(notification.ReceiverId, notification);
                            }
                            transaction.Commit();
                            await SendNotificationViaSignalR(notification);
                            return true;
                        }
                    }
                    catch
                    {
                        isSaved = false;
                        transaction.Rollback();
                    }
                    return isSaved;
                }
            }
        }
        public async Task<string> CreateNotificationURL(string action, string entityName, string entityId)
        {
            try
            {
                var Url = $"/{entityName.ToLower()}{"-"}{action.ToLower()}/{entityId}";
                //if (action.StartsWith("-transfer-review") && (entityName.StartsWith("ConsultationRequest")))
                //{
                //    if (action != "list" && action != "delete")
                //    {
                //        string sectorTypeId = action.Substring(action.IndexOf("_") + 1);
                //        Url = $"consultationrequest-transfer-review/{sectorTypeId}/{entityId}";
                //    }
                //    else
                //    {
                //        Url = $"{entityName.ToLower()}-{action}";
                //    }
                //}
                //else if (action.StartsWith("transfer-review_") && (entityName.StartsWith("ConsultationFile")))
                //{
                //    if (action != "list" && action != "delete")
                //    {
                //        string sectorTypeId = action.Substring(action.IndexOf("_") + 1);

                //        Url = $"consultationfile-transfer-review/{sectorTypeId}/{entityId}";
                //    }
                //}
                //else if (action.StartsWith("consultationrequest-detail"))
                //{
                //    if (action != "list" && action != "delete")
                //    {
                //        string sectorTypeId = action.Substring(action.IndexOf("_") + 1);
                //        Url = $"consultationrequest-detail/{entityId}/{sectorTypeId}";
                //    }

                //}
                if (entityName == "moj-registration")
                {
                    Url = $"/{entityName.ToLower()}{"-"}{action.ToLower()}/";
                }
                if (Url.StartsWith("/meeting-view/"))
                {
                    Url = $"{Url}/{true}";
                }
                if (Url.StartsWith("/meeting-list/"))
                {
                    Url = $"/meeting-list";
                }
                if (Url.StartsWith("/lmsliteratureparentindex-list/"))
                {
                    Url = $"/lmsliteratureparentindex-list";
                }

                return Url;
            }
            catch
            {
                return null;
            }
        }

        public async Task<(string, string)> CreateNotificationMessage(NotificationTemplate notificationTemplate, NotificationParameter entity, DatabaseContext _dbContext)
        {
            try
            {
                var placeholders = await _dbContext.NotificationEventPlaceholders.Where(x => x.EventId == notificationTemplate.EventId || x.EventId == null).ToListAsync();
                var bodyEN = FillPlaceHolders(placeholders, notificationTemplate.BodyEn, entity, "en");
                var bodyAR = FillPlaceHolders(placeholders, notificationTemplate.BodyAr, entity, "ar-KW");
                return (bodyEN, bodyAR);
            }
            catch
            {
                return (null, null);
            }
        }

        private string FillPlaceHolders(List<NotificationEventPlaceholders> placeholders, string message, NotificationParameter entity, string lang)
        {

            if (message == null)
            {
                return _resourceManager.GetString("DefaultMessage", lang);
            }
            foreach (var item in placeholders)
            {
                switch (item.PlaceHolderName)
                {
                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.Entity.GetDisplayName() && entity.Entity != null:
                        message = message.Replace(placeHolder, _resourceManager.GetString(entity.Entity, lang));
                        break;

                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.RequestNumber.GetDisplayName():
                        message = message.Replace(placeHolder, entity.RequestNumber);
                        break;

                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.FileNumber.GetDisplayName():
                        message = message.Replace(placeHolder, entity.FileNumber);
                        break;

                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.CaseNumber.GetDisplayName():
                        message = message.Replace(placeHolder, entity.CaseNumber);
                        break;

                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.PrimaryCaseNumber.GetDisplayName():
                        message = message.Replace(placeHolder, entity.PrimaryCaseNumber);
                        break;

                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.SenderName.GetDisplayName():
                        var senderName = entity.SenderName != null ? (lang == "en" ? entity.SenderName.Split("/")[0] : entity.SenderName.Split("/")[1]) : "";
                        message = message.Replace(placeHolder, senderName);
                        break;

                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.ReceiverName.GetDisplayName():
                        var receiverName = entity.ReceiverName != null ? (lang == "en" ? entity.ReceiverName.Split("/")[0] : entity.ReceiverName.Split("/")[1]) : "";
                        message = message.Replace(placeHolder, receiverName);
                        break;

                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.CreatedDate.GetDisplayName():
                        var date = lang == "en" ? entity.CreatedDate.ToString("dd/MM/yyyy hh:mm tt") : entity.CreatedDate.ToString("yyyy/MM/dd hh:mm tt");
                        date = lang == "en" ? date : date.Replace("PM", "م").Replace("AM", "ص");
                        message = message.Replace(placeHolder, date);
                        break;

                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.DocumentName.GetDisplayName():
                        message = message.Replace(placeHolder, entity.DocumentName);
                        break;

                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.ReferenceNumber.GetDisplayName():
                        message = message.Replace(placeHolder, entity.ReferenceNumber);
                        break;

                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.LegislationNumber.GetDisplayName():
                        message = message.Replace(placeHolder, entity.LegislationNumber);
                        break;

                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.PrincipleNumber.GetDisplayName():
                        message = message.Replace(placeHolder, entity.PrincipleNumber);
                        break;

                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.Name.GetDisplayName():
                        message = message.Replace(placeHolder, entity.Name);
                        break;

                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.SectorFrom.GetDisplayName():
                        var sectorFrom = lang == "en" ? entity.SectorFrom.Split("/")[0] : entity.SectorFrom.Split("/")[1];
                        message = message.Replace(placeHolder, sectorFrom);
                        break;

                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.SectorTo.GetDisplayName():
                        var sectorTo = lang == "en" ? entity.SectorTo.Split("/")[0] : entity.SectorTo.Split("/")[1];
                        message = message.Replace(placeHolder, sectorTo);
                        break;

                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.RequestType.GetDisplayName():
                        var requestType = lang == "en" ? entity.RequestType.Split("/")[0] : entity.RequestType.Split("/")[1];
                        message = message.Replace(placeHolder, requestType);
                        break;

                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.Type.GetDisplayName():
                        var type = lang == "en" ? entity.Type.Split("/")[0] : entity.Type.Split("/")[1];
                        message = message.Replace(placeHolder, type);
                        break;

                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.Status.GetDisplayName():
                        var status = lang == "en" ? entity.Status.Split("/")[0] : entity.Status.Split("/")[1];
                        message = message.Replace(placeHolder, status);
                        break;

                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.CANNumber.GetDisplayName():
                        message = message.Replace(placeHolder, entity.CANNumber);
                        break;

                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.Duration.GetDisplayName():
                        message = message.Replace(placeHolder, entity.Duration);
                        break;

                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.CorrespondenceNumber.GetDisplayName():
                        message = message.Replace(placeHolder, entity.CorrespodenceNumber);
                        break;

                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.DraftNumber.GetDisplayName():
                        message = message.Replace(placeHolder, entity.DraftNumber);
                        break;

                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.DraftName.GetDisplayName():
                        message = message.Replace(placeHolder, entity.DraftName);
                        break;

                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.GEName.GetDisplayName():
                        var GEName = lang == "en" ? entity.GEName.Split("/")[0] : entity.GEName.Split("/")[1];
                        message = message.Replace(placeHolder, GEName);
                        break;

                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.ServiceRequestNumber.GetDisplayName():
                        message = message.Replace(placeHolder, entity.ServiceRequestNumber);
                        break;

                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.StartDate.GetDisplayName():
                        message = message.Replace(placeHolder, entity.StartDate.ToString());
                        break;

                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.EndDate.GetDisplayName():
                        message = message.Replace(placeHolder, entity.EndDate.ToString());
                        break;

                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.StartTime.GetDisplayName():
                        message = message.Replace(placeHolder, entity.EndDate.ToString());
                        break;

                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.EndTime.GetDisplayName():
                        message = message.Replace(placeHolder, entity.EndDate.ToString());
                        break;

                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.PermissionDate.GetDisplayName():
                        message = message.Replace(placeHolder, entity.PermissionDate.ToString());
                        break;

                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.ReviewerName.GetDisplayName():
                        var reviewerName = entity.ReviewerName != null ? (lang == "en" ? entity.ReviewerName.Split("/")[0] : entity.ReviewerName.Split("/")[1]) : "";
                        message = message.Replace(placeHolder, reviewerName);
                        break;

                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.StoreIncharge.GetDisplayName():
                        message = message.Replace(placeHolder, entity.Name);
                        break;
                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.AssigneNameEn.GetDisplayName():
                        message = message.Replace(placeHolder, entity.AssigneeNameEn);
                        break;
                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.AssigneNameAr.GetDisplayName():
                        message = message.Replace(placeHolder, entity.AssigneeNameAr);
                        break;
                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.AssigorNameEn.GetDisplayName():
                        message = message.Replace(placeHolder, entity.AssignorNameEn);
                        break;
                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.AssigorNameAr.GetDisplayName():
                        message = message.Replace(placeHolder, entity.AssignorNameAr);
                        break;
                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.SubjectEn.GetDisplayName():
                        message = message.Replace(placeHolder, entity.SubjectEn);
                        break;
                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.SubjectAr.GetDisplayName():
                        message = message.Replace(placeHolder, entity.SubjectAr);
                        break;
                    case var placeHolder when placeHolder == NotificationPlaceholderEnum.EmployeeName.GetDisplayName():
                        var employeeName = entity.EmployeeName != null ? (lang == "en" ? entity.EmployeeName.Split("/")[0] : entity.EmployeeName.Split("/")[1]) : "";
                        message = message.Replace(placeHolder, employeeName);
                        break;

                    default: break;
                }
            }
            return message;
        }

        public async Task<bool> SendEmailNotification(Notification notification, string Subject, DatabaseContext _dbContext)
        {
            try
            {
                // Email send code here
                var emailConfiguration = await _dbContext.EmailConfigurations.Where(u => u.ApplicationId == (int)CommunicationSourceEnum.FATWA).AsNoTracking().FirstOrDefaultAsync();
                emailConfiguration.ToEmail = await _dbContext.Users.Where(x => x.Id == notification.ReceiverId).Select(x => x.Email).FirstOrDefaultAsync();
                emailConfiguration.FromEmail = emailConfiguration.SmtpUser;
                emailConfiguration.EmailBody = notification.NotificationMessageAr.ToString();
                emailConfiguration.BodyType = (int)EmailBodyTypeEnum.Html;
                emailConfiguration.EmailSubject = Subject;
                _emailService.Send(emailConfiguration);
                return true;
            }
            catch { return false; }
        }

        public async Task<bool> SendSmsNotification(Notification notification)
        {
            // Sms send code here
            return true;
        }

        public async Task<bool> SendBrowserNotification(Notification notification, DatabaseContext _dbContext)
        {
            try
            {
                notification.NotificationStatusId = (int)NotificationStatusEnum.Unread;
                await _dbContext.NotifNotifications.AddAsync(notification);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch { return false; }
        }

        #endregion

        public async Task<IEnumerable<NotificationEventListVM>> GetEventList()
        {
            try
            {
                if (_NotificationEvent == null)
                {
                    storedProc = $"exec pNotificationEvent";
                    var result = await _dbContext.NotificationEventListVMs.FromSqlRaw(storedProc).ToListAsync();
                    if (result != null)
                        _NotificationEvent = (IEnumerable<NotificationEventListVM>)result;
                }
                return _NotificationEvent;
            }
            catch
            {
                throw;
            }


        }
        //<History Author = 'Hassan Iftikhar' Date='2023-03-14' Version="1.0" Branch="master"> </History>

        public async Task<List<NotificationEvent>> GetEvent()
        {
            try
            {
                return await _dbContext.NotificationEvents.OrderBy(u => u.EventId).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        //<History Author = 'Hassan Iftikhar' Date='2023-03-14' Version="1.0" Branch="master"> </History>

        public async Task<List<NotificationChannel>> GetChannel()
        {
            try
            {
                return await _dbContext.NotificationChannels.OrderBy(u => u.ChannelId).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        //<History Author = 'Hassan Iftikhar' Date='2023-03-14' Version="1.0" Branch="master"> </History>

        public async Task<IEnumerable<NotificationEventPlaceholders>> GetPlaceHoldersByEventId(int EventId)
        {
            try
            {
                return await _dbContext.NotificationEventPlaceholders.Where(u => (u.EventId == EventId) || (u.EventId == null)).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<bool> CreateNotificationEventTemplate(NotificationTemplate item)
        {
            try
            {
                item.CreatedDate = DateTime.Now;
                _dbContext.NotificationTemplates.Add(item);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }

        }
        //<History Author = 'Hassan Iftikhar' Date='2023-03-14' Version="1.0" Branch="master"> </History>

        public async Task<NotificationTemplate> GetNotificationEventTemplateById(Guid Id)
        {
            try
            {

                return await _dbContext.NotificationTemplates.Where(u => u.TemplateId == Id).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateNotificationEventTemplate(NotificationTemplate Template)
        {
            try
            {
                Template.ModifiedDate = DateTime.Now;
                _dbContext.NotificationTemplates.Update(Template);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }
        public async Task<IEnumerable<NotificationTemplateListVM>> GetTemplateListByEventId(int EventId)
        {
            try
            {
                if (_NotificationEvent == null)
                {
                    storedProc = $"exec pNotificationTemplateByEventId @eventId = '" + EventId + "'";
                    var result = await _dbContext.NotificationTemplateListVMs.FromSqlRaw(storedProc).ToListAsync();
                    if (result != null)
                        _NotificationTemplateList = (IEnumerable<NotificationTemplateListVM>)result;
                }
                return _NotificationTemplateList;
            }
            catch
            {
                throw;
            }
        }

        #region Delete Government Entity 
        public async Task<bool> DeleteEventTemplate(NotificationTemplateListVM template)
        {
            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var task = await _dbContext.NotificationTemplates.Where(x => x.TemplateId == template.TemplateId).FirstOrDefaultAsync();
                        if (task != null)
                        {
                            task.isActive = template.isActive;
                            _dbContext.NotificationTemplates.Update(task);
                            _dbContext.Entry(task).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();
                            return true;
                        }
                        return false;
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        return false;
                        throw new Exception(ex.Message);
                    }
                }
            }

        }
        #endregion

        #region DELETE All NOTIFICATION

        public async Task DeleteAllNotification(List<Guid> notificationIds, String userId)
        {
            try
            {

                var user = await _dbContext.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();

                foreach (var notificationId in notificationIds)
                {
                    var result = await _dbContext.NotifNotifications.Where(x => x.NotificationId == notificationId && x.ReceiverId == userId).FirstOrDefaultAsync();
                    if (result != null)
                    {
                        result.IsDeleted = true;
                        result.DeletedBy = user.UserName;
                        result.DeletedDate = DateTime.Now;
                        _dbContext.Update(result);
                        await _dbContext.SaveChangesAsync();
                    }
                }



            }
            catch
            {
                _dbContext.Entry(_dbContext).State = EntityState.Unchanged;
                throw;
            }
        }
        #endregion

        public async Task<bool> UpdateEventStatus(NotificationEventListVM Event)
        {
            try
            {
                var task = await _dbContext.NotificationEvents.Where(x => x.EventId == Event.EventId).FirstOrDefaultAsync();
                if (task != null)
                {
                    task.IsActive = Event.IsActive;
                    _dbContext.Entry(task).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        #region Get Event By Event Id
        public async Task<NotificationEvent> GetNotificationEventByEventId(int EventId)
        {
            try
            {
                return await _dbContext.NotificationEvents.Where(u => u.EventId == EventId).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Edit Notification Event
        public async Task EditNotificationEvent(NotificationEvent Event)
        {
            try
            {
                Event.ModifiedDate = DateTime.Now;
                Event.NameEn = Event.NameEn;
                Event.NameAr = Event.NameAr;
                Event.DescriptionEn = Event.DescriptionEn;
                Event.DescriptionAr = Event.DescriptionAr;
                Event.ReceiverTypeId = Event.ReceiverTypeId;
                //if (Event.ReceiverTypeRefId != Guid.Empty)
                //{
                //        Event.ReceiverTypeRefId = Event.ReceiverTypeRefId;
                //}
                //else
                //{
                //    Event.ReceiverTypeRefId = null;
                //}
                _dbContext.NotificationEvents.Update(Event);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Get Receiver Type
        public async Task<List<NotificationReceiverType>> GetReceiverType()
        {
            try
            {
                return await _dbContext.NotificationReceiverTypes.OrderBy(u => u.ReceiverTypeId).ToListAsync();

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion
        #region FCM Push Notification for Mobile App
        public async Task<string> SendMultipleDevicePushNotificationFCM(string userId, FATWA_DOMAIN.Models.Notifications.Notification notification)
        {
            try
            {
                string responsemessage = string.Empty;
                var registrationTokens = new List<string>();
                List<UmsUserDeviceToken> umsUserDeviceTokens = await GetDeviceRegisteredFCMToken(userId);
                if (umsUserDeviceTokens.Any())
                {
                    foreach (var item in umsUserDeviceTokens)
                    {
                        registrationTokens.Add(item.DeviceToken);
                    }
                    var multicastMessage = new FirebaseAdmin.Messaging.MulticastMessage()
                    {
                        Tokens = registrationTokens,
                        Notification = new FirebaseAdmin.Messaging.Notification
                        {
                            Title = notification != null ? notification.EntityName : "test fatwa",
                            Body = notification != null ? notification.NotificationMessageEn : "test message fatwa app"
                        },
                        Data = new Dictionary<string, string>()
                        {
                             { "title", notification != null ? notification.EntityName : "test fatwa" },
                             { "body", notification != null ? notification.NotificationMessageEn : "test message fatwa app" },
                        },
                    };
                    var multiresponse = await FirebaseAdmin.Messaging.FirebaseMessaging.DefaultInstance.SendEachForMulticastAsync(multicastMessage);
                    if (multiresponse.FailureCount > 0)
                    {
                        foreach (var failure in multiresponse.Responses)
                        {
                            if (!failure.IsSuccess)
                            {
                                responsemessage = "Failed to send notification to:" + failure.Exception.Message;
                            }
                        }
                    }
                    else
                    {
                        responsemessage = "FCM_Notification_Sent";
                    }
                }
                return responsemessage;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public async Task<string> SendPushNotificationFCM(string userId, FATWA_DOMAIN.Models.Notifications.Notification notification)
        {
            try
            {
                string responsemessage = string.Empty;
                List<UmsUserDeviceToken> umsUserDeviceTokens = await GetDeviceRegisteredFCMToken(userId);
                if (umsUserDeviceTokens.Any())
                {
                    foreach (var token in umsUserDeviceTokens)
                    {
                        //var condition = "'stock-GOOG' in topics || 'industry-tech' in topics";
                        var message = new FirebaseAdmin.Messaging.Message()
                        {
                            Notification = new FirebaseAdmin.Messaging.Notification
                            {
                                Title = notification != null ? notification.EntityName : "test fatwa",
                                Body = notification != null ? notification.NotificationMessageEn : "test message fatwa app"
                            },
                            //Data = new Dictionary<string, string>()
                            //{
                            //    ["CustomData"] = "Custom Data"
                            //},
                            Token = token.DeviceToken
                            //Condition = condition
                        };
                        var messaging = FirebaseAdmin.Messaging.FirebaseMessaging.DefaultInstance;
                        var response = await messaging.SendAsync(message);
                        if (!string.IsNullOrEmpty(response))
                        {
                            responsemessage = "Message sent successfully!" + response;
                        }
                        else
                        {
                            responsemessage = "Error_Ocurred";
                        }
                    }
                }
                return responsemessage;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        public async Task<List<UmsUserDeviceToken>> GetDeviceRegisteredFCMToken(string userId)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                List<UmsUserDeviceToken> umsUserDeviceToken = new List<UmsUserDeviceToken>();
                if (!string.IsNullOrEmpty(userId))
                {
                    umsUserDeviceToken = await _DbContext.UmsUserDeviceToken.Where(x => x.UserId == userId).ToListAsync();
                }
                return umsUserDeviceToken;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        public async Task<bool> SendNotificationList(List<Notification> notifications)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using var transaction = _DbContext.Database.BeginTransaction();

            try
            {
                foreach (var notification in notifications)
                {
                    bool isSaved = false;

                    if (await _DbContext.NotificationEvents.Where(x => x.EventId == notification.EventId && x.IsActive).AnyAsync())
                    {
                        var loggedUser = await _DbContext.Users.FirstOrDefaultAsync(x => x.UserName == notification.CreatedBy);
                        if (loggedUser != null)
                        {
                            notification.SenderId = loggedUser.Id;
                            notification.NotificationParameter.SenderName = await _DbContext.UserPersonalInformation
                                .Where(x => x.UserId == notification.SenderId)
                                .Select(x => x.FirstName_En + " " + x.LastName_En + "/" + x.FirstName_Ar + " " + x.LastName_Ar)
                                .FirstOrDefaultAsync();
                        }

                        notification.NotificationParameter.ReceiverName = await _DbContext.UserPersonalInformation
                            .Where(x => x.UserId == notification.ReceiverId)
                            .Select(x => x.FirstName_En + " " + x.LastName_En + "/" + x.FirstName_Ar + " " + x.LastName_Ar)
                            .FirstOrDefaultAsync();

                        notification.NotificationParameter.CreatedDate = notification.CreatedDate;

                        var notificationTemplates = await _DbContext.NotificationTemplates
                            .Where(x => x.EventId == notification.EventId && x.isActive)
                            .ToListAsync();

                        notification.NotificationURL = await CreateNotificationURL(notification.Action, notification.EntityName, notification.EntityId);

                        foreach (var notificationTemplate in notificationTemplates)
                        {
                            notification.NotificationTemplateId = notificationTemplate.TemplateId;
                            (notification.NotificationMessageEn, notification.NotificationMessageAr) = await CreateNotificationMessage(notificationTemplate, notification.NotificationParameter, _DbContext);

                            switch (notificationTemplate.ChannelId)
                            {
                                case (int)NotificationChannelEnum.Email:
                                    isSaved = await SendEmailNotification(notification, notificationTemplate.SubjectAr, _DbContext);
                                    break;
                                case (int)NotificationChannelEnum.Mobile:
                                    isSaved = await SendSmsNotification(notification);
                                    break;
                                default:
                                    isSaved = await SendBrowserNotification(notification, _DbContext);
                                    break;
                            }
                        }

                        if (notification.EventId == (int)NotificationEventEnum.NewRequest || notification.EventId == (int)NotificationEventEnum.AssignToLawyer)
                        {
                            var response = await SendMultipleDevicePushNotificationFCM(notification.ReceiverId, notification);
                        }
                    }

                    if (!isSaved)
                    {
                        transaction.Rollback();
                        return false;
                    }
                }

                transaction.Commit();
                return true;
            }
            catch
            {
                transaction.Rollback();
                return false;
            }
        }

        public async Task<bool> UpdateNotificationUrl(Guid Id, string Url)
        {
            try
            {
                var notification = await _dbContext.NotifNotifications.Where(x => x.NotificationId == Id).FirstOrDefaultAsync();
                if(notification != null)
                {
                    notification.NotificationURL = Url;
                    _dbContext.Entry(notification).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2024-12-29' Version="1.0" Branch="master"> Send Notification to connected SignalR clients </History>
        private async Task SendNotificationViaSignalR(Notification notification)
        {
            try
            {
                BellNotificationVM notificationMessage = new BellNotificationVM
                {
                    NotificationId = notification.NotificationId,
                    NotificationMessageEn = notification.NotificationMessageEn,
                    NotificationMessageAr = notification.NotificationMessageEn,
                    Url = notification.NotificationURL,
                    CreationDate = notification.CreatedDate,
                };
                await _notificationSignalRClient.Clients.User(notification.ReceiverId).SendNotification(notificationMessage);
            }
            catch(Exception ex)
            {

            }
        }
    }
}
