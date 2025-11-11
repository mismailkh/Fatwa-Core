using Blazored.LocalStorage;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using System.Net.Http.Headers;
using System.Net;
using static FATWA_GENERAL.Helper.Response;
using System.Text;

namespace DMS_WEB.Services
{
    //<History Author = 'Umer Zaman' Date='2022-09-12' Version="1.0" Branch="master"> Service to handle system setting requests</History>
    public class SystemSettingService
    {
        private readonly IConfiguration _config;
        private readonly ILocalStorageService browserStorage;
        public SystemSettingService(IConfiguration config, ILocalStorageService _browserStorage)
        {
            _config = config;
            browserStorage = _browserStorage;
        }

        public async Task<ApiCallResponse> GetSystemSetting()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("fatwa_api_url") + "/SystemSetting/GetSystemSetting");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<SystemSetting>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else
                {
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
                }
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = ex.Message };
            }
        }

        #region Update system setting
        public async Task<ApiCallResponse> UpdateSystemSetting(SystemSetting systemsetting)
        {
            systemsetting.CreatedBy = await browserStorage.GetItemAsync<string>("User");
            systemsetting.CreatedDate = DateTime.Now;
            systemsetting.ModifiedBy = await browserStorage.GetItemAsync<string>("User");
            systemsetting.ModifiedDate = DateTime.Now;
            var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("fatwa_api_url") + "/SystemSetting/UpdateSystemSetting");
            var postBody = systemsetting;
            request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
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
        #endregion
    }
}
