using Blazored.LocalStorage;
using FATWA_ADMIN.Data;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Radzen;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;
using static FATWA_GENERAL.Helper.Response;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace FATWA_ADMIN.Services.UserManagement
{
    //<History Author = 'Hassan Abbas' Date='2022-07-20' Version="1.0" Branch="master"> Service for maaging Roles and Claims</History>
    public partial class RoleService
    {
        private readonly IConfiguration _config;
        private readonly ILocalStorageService browserStorage;
        private readonly TranslationState translationState;
        public RoleService(IConfiguration config, ILocalStorageService _browserStorage, TranslationState _translationState)
        {
            _config = config;
            browserStorage = _browserStorage;
            translationState = _translationState;
        }

        #region Get Roles lists

        public async Task<IEnumerable<IdentityRole>> GetAllUserRoles()
        {
            try
            {
                return await new HttpClient().GetFromJsonAsync<IdentityRole[]>(_config.GetValue<string>("api_url") + "/Roles/UserRoleList");
            }
            catch (Exception ex)
            {
                throw new Exception("Record not found", ex);
            }

        }

        //<History Author = 'Hassan Abbas' Date='2022-07-21' Version="1.0" Branch="master">Get all roles</History>
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

        //<History Author = 'Hassan Abbas' Date='2022-07-21' Version="1.0" Branch="master">Get role detail by id</History>
        public async Task<ApiCallResponse> GetRoleById(string roleId)
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Roles/GetRoleById?roleId=" + roleId);
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<Role>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }
        }

        #endregion

        #region Create / Update Role
        //<History Author = 'Hassan Abbas' Date='2022-07-21' Version="1.0" Branch="master">Function for handling create and update role</History>
        public async Task<ApiCallResponse> SaveRole(Role item)
        {
            if (item.Id == null)
            {
                item.Id = Guid.NewGuid().ToString();
                item.CreatedBy = await browserStorage.GetItemAsync<string>("User");
                item.CreatedDate = DateTime.Now;
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Roles/CreateRole");
                var postBody = item;
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
            else
            {
                item.ModifiedBy = await browserStorage.GetItemAsync<string>("User");
                item.ModifiedDate = DateTime.Now;
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Roles/UpdateRole");
                var postBody = item;
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
        }

        #endregion

        #region Translations

        //<History Author = 'Hassan Abbas' Date='2022-07-06' Version="1.0" Branch="master">Remote Users Data for Add Document Drop Down</History>
        public async Task<ApiCallResponse> GetAllTranslations()
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

        #endregion

        #region Claims and Role Claims

        //<History Author = 'Hassan Abbas' Date='2022-04-23' Version="1.0" Branch="master">Get role claims list</History>
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
        partial void OnRoleClaimsRead(ref IQueryable<ClaimVM> items);

        //<History Author = 'Hassan Abbas' Date='2022-07-20' Version="1.0" Branch="master"> Call API for getting List of Claims</History>
        public async Task<IQueryable<ClaimVM>> GetAllClaims(string roleId)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Roles/GetAllClaims?roleId=" + roleId);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(translationState.Translate("Contact_Administrator"));
                }
                var responselist = response.Content.ReadFromJsonAsync<IEnumerable<ClaimVM>>();
                var queryableX = (await responselist).AsQueryable();
                return queryableX;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        #endregion

        #region Get Roles Details
        partial void OnRoleDetailsRead(ref IQueryable<Role> items);
        public async Task<IQueryable<Role>> GetRoleDetails()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Roles/GetRoleDetails");
                // add authorization header
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                // send request
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(translationState.Translate("Contact_Administrator"));
                }
                var responselist = response.Content.ReadFromJsonAsync<IEnumerable<Role>>();
                var queryableX = (await responselist).AsQueryable();
                return queryableX;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Role dropdown

        static List<Role> AllRoles { get; set; }

        //<History Author = 'Hassan Abbas' Date='2022-07-27' Version="1.0" Branch="master">Remote Roles Data for Add Document Drop Down</History>
        public async Task<DataEnvelope<Role>> GetRemoteRolesData(DataSourceRequest request)
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Roles/GetRoleDetails");
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);
            AllRoles = await response.Content.ReadFromJsonAsync<List<Role>>();
            var result = await AllRoles.ToDataSourceResultAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(translationState.Translate("Contact_Administrator"));
            }
            DataEnvelope<Role> dataToReturn = new DataEnvelope<Role>
            {
                Data = result.Data.Cast<Role>().ToList(),
                Total = result.Total
            };
            return await Task.FromResult(dataToReturn);
        }
        //<History Author = 'Hassan Abbas' Date='2022-07-27' Version="1.0" Branch="master">Filter Item based on value</History>
        public async Task<Role> GetRoleModelFromValue(string selectedValue)
        {
            if (AllRoles != null)
            {
                return await Task.FromResult(AllRoles.FirstOrDefault(x => selectedValue == x.Id));
            }
            else
            {
                return await Task.FromResult<Role>(null);
            }
        }

        #endregion

        #region Detele Role (Soft delete status change)
        //<History Author = 'Umer Zaman' Date='2022-08-01' Version="1.0" Branch="master">soft delete user role</History>
        public async Task<ApiCallResponse> DeleteRole(Role item)
        {
            item.DeletedBy = await browserStorage.GetItemAsync<string>("User");
            item.DeletedDate = DateTime.Now;
            item.IsDeleted = true;
            var request = new HttpRequestMessage(HttpMethod.Delete, _config.GetValue<string>("api_url") + "/Roles/DeleteRole");
            var postBody = item;
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
