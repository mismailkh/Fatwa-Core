using Blazored.LocalStorage;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.ServiceRequestModels.ComplaintRequestModels;
using Microsoft.AspNetCore.Identity;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Services
{
    public class RoleService
    {
        private readonly IConfiguration _config;
        private readonly ILocalStorageService browserStorage;
        public RoleService(IConfiguration config, ILocalStorageService _browserStorage)
        {
            _config = config;
            browserStorage = _browserStorage;
        }

        public async Task<ApiCallResponse> GetAllUserRoles()
        {
            try
            {
                var request=new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url")+ "/Roles/UserRoleList");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response=await new HttpClient().SendAsync(request);
                if(response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<IEnumerable<IdentityRole>>();
                   return  new ApiCallResponse { StatusCode = response.StatusCode , IsSuccessStatusCode=response.IsSuccessStatusCode, ResultData=content };
                }
                else
                {
                    var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
            }
            catch (Exception ex)
            {

                return new ApiCallResponse { StatusCode=HttpStatusCode.BadRequest, IsSuccessStatusCode=false, ResultData=new BadRequestResponse { Message=ex.Message, InnerException=ex.InnerException.Message } };
            }       

        }

        //<History Author = 'Hassan Abbas' Date='2022-04-23' Version="1.0" Branch="master">Remote Users Data for Add Document Drop Down</History>
        public async Task<List<IdentityRole>> GetAllRoles()
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Account/GetRoleList");
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<List<IdentityRole>>();
            else
                return new List<IdentityRole>();
        }

        //<History Author = 'Hassan Abbas' Date='2022-04-23' Version="1.0" Branch="master">Remote Users Data for Add Document Drop Down</History>
        public async Task<ApiCallResponse> GetRoleCLaims(string userId)
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Roles/GetRoleClaims");
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

        //<History Author = 'Hassan Abbas' Date='2022-07-06' Version="1.0" Branch="master">Remote Users Data for Add Document Drop Down</History>
        public async Task<ApiCallResponse> GetAllTranslations()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Account/GetAllTranslations");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var translations = await response.Content.ReadFromJsonAsync<List<TranslationSucessResponse>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = translations };
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

        public async Task<ApiCallResponse> GetHOSBySectorId(int sectorId)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Roles/GetHOSBySectorId?sectorTypeId=" + sectorId);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var translations = await response.Content.ReadFromJsonAsync<User>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = translations };
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
    }
}
