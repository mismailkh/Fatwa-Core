using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using System.Net;
using static FATWA_GENERAL.Helper.Response;

namespace DMS_WEB.Services
{

    //<History Author = 'Umer Zaman' Date='2022-08-09' Version="1.0" Branch="master"> Service to handle system configuration requests</History>

    public class SystemConfigurationService
    {
        #region Constructor
        public SystemConfigurationService(IConfiguration config)
        {
            _config = config;
        }
        #endregion

        #region variables declaration
        private readonly IConfiguration _config;

        protected IEnumerable<SystemConfiguration> systemConfigDetails;
        public string ResultDetails { get; private set; }
        #endregion

        #region Login Validations 
        public async Task<ApiCallResponse> GetUserDetailByUsingEmail(string email)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("fatwa_api_url") + "/SystemConfiguration/GetUserDetailByUsingEmail?email=" + email);
                //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    var content = new User();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<User>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else
                {
                    var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
            }
            catch(Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false };
            }
        }
        public async Task<ApiCallResponse> GetUserDetailByUsingEmailForPasswordCheck(string email)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("fatwa_api_url") + "/SystemConfiguration/GetUserDetailByUsingEmailForPasswordCheck?email=" + email);
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                var content = new User();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }
            else if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<User>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }
        }
        public async Task<ApiCallResponse> GetUserGroupDetailByUsingGroupId()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("fatwa_api_url") + "/SystemConfiguration/GetUserGroupDetailByUsingGroupId");
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                var content = new SystemConfiguration();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }
            else if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<SystemConfiguration>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }
        }
        #endregion

        #region User account lock
        public async Task<ApiCallResponse> LockUserAccount(string email)
        {
            // user.IsLocked = true;
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("fatwa_api_url") + "/SystemConfiguration/LockUserAccount?email=" + email);
            //var postBody = user;
            //request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
            // request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
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

        #region User account access fail count manage
        public async Task<ApiCallResponse> UserAccountAccessFailCount(string email, int wrongCount)
        {
            //user.AccessFailedCount = wrongCount;
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("fatwa_api_url") + "/SystemConfiguration/UserAccountAccessFailCount?email=" + email + "&wrongCount=" + wrongCount);
            //var postBody = user;
            //request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
            //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
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