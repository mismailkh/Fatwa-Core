using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Models.ViewModel;
using static FATWA_GENERAL.Helper.Response;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using Blazored.LocalStorage;
using System.Text.Json;

namespace FATWA_ADMIN.Services.Notifications
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
    }
}
