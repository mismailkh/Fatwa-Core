using FATWA_DOMAIN.Models;
using static FATWA_GENERAL.Helper.Response;
using System.Net.Http.Headers;
using Blazored.LocalStorage;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using System.Net;

namespace FATWA_WEB.Services
{
    public class GroupService
    {
        private readonly IConfiguration _config;
        private readonly ILocalStorageService browserStorage;
        public GroupService(IConfiguration config, ILocalStorageService _browserStorage)
        {
            _config = config;
            browserStorage = _browserStorage;
        }

		//<History Author = 'Nadia Gull' Date='2023-05-3' Version="1.0" Branch="master">Get Group Claims</History>
		public async Task<ApiCallResponse> GetGroupClaims(string userId)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Group/GetGroupClaims");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                request2.Content = new FormUrlEncodedContent(new[]{
                        new KeyValuePair<string, string>("userId", userId.ToString()),
                    });
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var claims = await response.Content.ReadFromJsonAsync<List<ClaimSucessResponse>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = claims };
                }
                else
                {
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
                }
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }

        public async Task<ApiCallResponse> GetGroupAccessTypes()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Group/GetGroupAccessTypes");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var groupAccessType = await response.Content.ReadFromJsonAsync<List<GroupTypeWebSystemVM>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = groupAccessType };
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
        public async Task<ApiCallResponse> GetGroups()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Group/GetGroups");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var groupAccessType = await response.Content.ReadFromJsonAsync<List<Group>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = groupAccessType };
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
    }
}
