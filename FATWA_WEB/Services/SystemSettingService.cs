using Blazored.LocalStorage;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using System.Net.Http.Headers;
using System.Net;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Services
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
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/SystemSetting/GetSystemSetting");
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
    }
}
