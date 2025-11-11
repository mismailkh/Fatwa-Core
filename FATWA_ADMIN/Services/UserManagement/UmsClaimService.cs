using Blazored.LocalStorage;
using FATWA_ADMIN.Data;
using FATWA_ADMIN.Pages.UserManagement.Translations;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using System.Linq.Dynamic.Core;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Telerik.DataSource.Extensions;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_ADMIN.Services.UserManagement
{

    //<History Author = 'Muhammad Zaeem' Date='2022-09-29' Version="1.0" Branch="master"> Services for the call of API response to UMS CLAIMS</History>
    public partial class UmsClaimService
    {
        private readonly IConfiguration _config;
        private readonly NavigationManager navigationManager;
        private readonly ILocalStorageService browserStorage;
        private readonly TranslationState translationState;
        public UmsClaimService(IConfiguration configuration, NavigationManager _navigationManager, ILocalStorageService _browserStorage, TranslationState _translationState)
        {
            _config = configuration;
            navigationManager = _navigationManager;
            browserStorage = _browserStorage;
            translationState = _translationState;

        }
        #region Get Claims lists

        partial void OnClaimsTypesRead(ref IQueryable<ClaimUms> items);
        public async Task<ApiCallResponse> GetClaimUms()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/UmsClaims/GetClaimUms");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    var content = new ClaimUms();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<ClaimUms>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else
                {
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Get Claims by Id
        public async Task<ApiCallResponse> GetClaimsById(int Id)
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/UmsClaims/GetClaimsById?id=" + Id);
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<ClaimUms>();
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

        public async Task<ApiCallResponse> SaveClaims(ClaimUms item)
        {
            
                if (item.Id == 0)
                {

                    var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/UmsClaims/CreateClaims");
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

                    var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/UmsClaims/UpdateClaims");
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

    

        #region Delete
        public async Task<ApiCallResponse> DeleteClaims(int id)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, _config.GetValue<string>("api_url") + "/UmsClaims/" + id);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception("تعذر حذف الأنواع المتصلة!");
                }

                return new ApiCallResponse
                {
                    StatusCode = response.StatusCode,
                    IsSuccessStatusCode = response.IsSuccessStatusCode,
                    ResultData = response.Content
                };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }


        #endregion

        #region  Soft Delete
        //public async Task<ApiCallResponse> DeleteClaims(ClaimUms item)
        //{
        //    item.IsDeleted = true;
        //    var request = new HttpRequestMessage(HttpMethod.Delete, _config.GetValue<string>("api_url") + "/UmsClaims/DeleteClaims");
        //    var postBody = item;
        //    request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
        //    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
        //    var response = await new HttpClient().SendAsync(request);
        //    if (response.IsSuccessStatusCode)
        //    {
        //        return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
        //    }
        //    else
        //    {
        //        var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
        //        return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
        //    }


        //}


        #endregion










    }
}

