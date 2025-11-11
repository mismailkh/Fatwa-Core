using FATWA_DOMAIN.Models;
using static FATWA_GENERAL.Helper.Response;
using System.Net.Http.Headers;
using Blazored.LocalStorage;

namespace DMS_WEB.Services
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
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("fatwa_api_url") + "/Group/GetGroupClaims");
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
    }
}
