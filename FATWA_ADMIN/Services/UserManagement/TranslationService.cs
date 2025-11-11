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

    //<History Author = 'Nabeel ur Rehman' Date='2022-07-22' Version="1.0" Branch="master"> Service to handle group requests</History>
    public partial class TranslationService
    {
        private readonly IConfiguration _config;
        private readonly ILocalStorageService browserStorage;
        public TranslationService(IConfiguration configuration, ILocalStorageService _browserStorage)
        {
            _config = configuration;
            browserStorage = _browserStorage;

        }
        #region  Get Translationts
        public async Task<ApiCallResponse> GetTranslation()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Translation/GetTranslation");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);

                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    var content = new Translation();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<Translation>>();
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
        public async Task<ApiCallResponse> GetTranslationById(int TranslationId)
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Translation/GetTranslationById?id=" + TranslationId);
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<Translation>();
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

        public async Task<ApiCallResponse> UpdateTranslation(Translation item)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Translation/UpdateTranslation");
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

        #region Delete
        public async Task<ApiCallResponse> DeleteTranslation(int id)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Delete, _config.GetValue<string>("api_url") + "/Translation/" + id);
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



    }
}

