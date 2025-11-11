using Blazored.LocalStorage;
using static FATWA_GENERAL.Helper.Response;
using System.Net.Http.Headers;
using FATWA_DOMAIN.Models.Notifications;
using Radzen;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using System.Text;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Models.ViewModel;
using System.Text.Json;
using Query = Radzen.Query;

namespace DMS_WEB.Services
{
    public class NotificationDetailService
    {

        private readonly IConfiguration _config;
        private readonly ILocalStorageService _browserStorage;
        public NotificationDetailService(IConfiguration configuration, ILocalStorageService browserStorage)
        {
            _config = configuration;
            _browserStorage = browserStorage;
        }

        #region Get Notificaton List
        // partial void OnNotificationDetailViewRead(ref IQueryable<NotifNotificationVM> items);
        public async Task<ApiCallResponse> GetNotifNotificationDetails()
        {
            try
            {
                var user = await _browserStorage.GetItemAsync<UserDetailVM>("UserDetail");

                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("fatwa_api_url") + "/Notification/GetNotifNotificationDetails?userId=" + user.UserId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    var content = new NotificationVM();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<NotificationVM>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else
                {
                    var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected NotificationVM UniqueNotification = new();
        public async Task<ApiCallResponse> DeleteNotification(NotificationVM item)
        {
            item.DeletedBy = await _browserStorage.GetItemAsync<string>("User");

            var request = new HttpRequestMessage(HttpMethod.Delete, _config.GetValue<string>("fatwa_api_url") + "/Notification/DeleteNotification");
            var postBody = item;
            request.Content = new StringContent(JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }
        }

        public async Task<NotificationVM> GetNotificationById(Guid NotificationId)
        {
            try
            {
                UniqueNotification = await new HttpClient().GetFromJsonAsync<NotificationVM>(_config.GetValue<string>("fatwa_api_url") + "/Notification/GetNotificationById?NotificationId=" + NotificationId);

                return UniqueNotification;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #endregion

        #region Bell Notification

        public async Task<ApiCallResponse> GetBellNotifications()
        {
            try
            {
                var user = await _browserStorage.GetItemAsync<UserDetailVM>("UserDetail");

                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("fatwa_api_url") + "/Notification/GetBellNotifications?userId=" + user.UserId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    var content = new BellNotificationVM();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<BellNotificationVM>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else
                {
                    var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<ApiCallResponse> CheckNotification(CheckNotificationVM notification)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("fatwa_api_url") + "/Notification/CheckNotification");
                var postBody = notification;
                request.Content = new StringContent(JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    var content = false;
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<bool>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else
                {
                    var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Notifcation Detail View

        public async Task<ApiCallResponse> GetNotificationDetailView(Guid NotificationId)
        {
            try
            {
                var user = await _browserStorage.GetItemAsync<UserDetailVM>("UserDetail");

                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("fatwa_api_url") + "/Notification/GetNotificationDetailView?NotificationId=" + NotificationId + "&user=" + user.UserId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    var content = new BellNotificationVM();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<NotificationDetailVM>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else
                {
                    var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Functions

        public void GetNotificationList(IQueryable<NotificationVM> notifications)
        {
            try
            {
                foreach (NotificationVM notification in notifications)
                {
                    string entity = notification.NotificationLink.Split('/')[1].Split('-')[0].ToUpper();
                    string entityId = notification.NotificationLink.Split('/')[2];
                    if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                    {
                        //For Notification With Some Number in their Entity  
                        if (notification.SubjectEn.Contains("#entityId#"))
                            notification.SubjectEn = (notification.SubjectEn).Replace("#entityId#", entityId);
                        else
                            //For Notification With Name as their Entity   
                            notification.SubjectEn = (notification.SubjectEn).Replace("#entity#", entity);

                    }
                    else
                    {
                        //For Notification With Some Number in their Entity   
                        if (notification.SubjectAr.Contains("#entityId#"))
                            notification.SubjectAr = (notification.SubjectAr).Replace("#entityId#", entityId);
                        else
                            //For Notification With Name as their Entity   
                            notification.SubjectAr = (notification.SubjectAr).Replace("#entity#", entity);
                    }
                }
            }
            catch (Exception)
            { 
                throw;
            }
            
        }

        #endregion

    }
}
