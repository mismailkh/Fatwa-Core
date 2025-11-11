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
using System.Net;

namespace FATWA_WEB.Services
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
        public async Task<ApiCallResponse> GetNotifNotificationDetails(NotificationListAdvanceSearchVM advanceSearchVM)
        {
            try
            {
                var user = await _browserStorage.GetItemAsync<UserDetailVM>("UserDetail");
                advanceSearchVM.UserId = user.UserId;
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Notification/GetNotifNotificationDetails");
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(advanceSearchVM), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
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
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }

        protected NotificationVM UniqueNotification = new();
        public async Task<ApiCallResponse> DeleteNotification(NotificationVM item)
        {
            try
            {
                item.DeletedBy = await _browserStorage.GetItemAsync<string>("User");

                var request = new HttpRequestMessage(HttpMethod.Delete, _config.GetValue<string>("api_url") + "/Notification/DeleteNotification");
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
            catch (Exception ex)
            {

                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }

        }

        #endregion

        #region Bell Notification

        public async Task<ApiCallResponse> GetBellNotifications(int NotificationStatusId)
        {
            try
            {
                var user = await _browserStorage.GetItemAsync<UserDetailVM>("UserDetail");
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Notification/GetBellNotifications?userId=" + user.UserId + "&NotificationStatusId=" + NotificationStatusId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                return await response.Content.ReadFromJsonAsync<ApiCallResponse>();
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        public async Task<ApiCallResponse> MarkNotificationAsRead(List<Guid> notificationIds)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Notification/MarkNotificationAsRead");
                var postBody = notificationIds;
                request.Content = new StringContent(JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    //var content = await response.Content.ReadFromJsonAsync<bool>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = "" };
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

        public async Task<ApiCallResponse> MarkNotificationAsRead(Guid notificationId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Notification/MarkNotificationAsRead?notificationId=" + notificationId);
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
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }

        #endregion

        #region Notifcation Detail View

        public async Task<ApiCallResponse> GetNotificationDetailView(Guid NotificationId)
        {
            try
            {
                var user = await _browserStorage.GetItemAsync<UserDetailVM>("UserDetail");

                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Notification/GetNotificationDetailView?NotificationId=" + NotificationId + "&user=" + user.UserId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
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
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }

        #endregion

        #region Notifcation get Id

        public async Task<ApiCallResponse> GetNotificationByNotificationId(Guid NotificationId)
        {
            try
            {
                var user = await _browserStorage.GetItemAsync<UserDetailVM>("UserDetail");

                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Notification/GetNotificationByNotificationId?NotificationId=" + NotificationId + "&user=" + user.UserId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<BellNotificationVM>();
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
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }

        #endregion

        #region Functions



        #endregion

        #region Delete all Notifications
        public async Task<ApiCallResponse> DeleteAllNotification(List<Guid> notificationIds)
        {
            try
            {
                var user = await _browserStorage.GetItemAsync<UserDetailVM>("UserDetail");
                var request = new HttpRequestMessage(HttpMethod.Delete, _config.GetValue<string>("api_url") + "/Notification/DeleteAllNotification?userId=" + user.UserId);
                var postBody = notificationIds;
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
            catch (Exception ex)
            {

                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }

        }
        #endregion

        #region Send Notification 
        public async Task<ApiCallResponse> SendNotification(List<Notification> sendNotifications)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Notification/SendNotificationList");
                var postBody = sendNotifications;

                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
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
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        #endregion
    }
}
