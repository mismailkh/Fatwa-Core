using Blazored.LocalStorage;
using FATWA_DOMAIN.Models;
using FATWA_WEB.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using System.Linq.Dynamic.Core;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Services
{
    public partial class LmsLiteratureTypeService
    {
        private readonly IConfiguration _config;
        private readonly NavigationManager navigationManager;
        private readonly ILocalStorageService browserStorage;
        private readonly TranslationState translationState;

        public LmsLiteratureTypeService(IConfiguration configuration, NavigationManager _navigationManager, ILocalStorageService _browserStorage, TranslationState _translationState)
        {
            _config = configuration;
            navigationManager = _navigationManager;
            browserStorage = _browserStorage;
            translationState = _translationState;

        }
        partial void OnLmsLiteratureTypesRead(ref IQueryable<LmsLiteratureType> items);
        public async Task<IQueryable<LmsLiteratureType>> GetLmsLiteratureTypes()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteraturetype/GetLmsLiteraturetypes");
                // add authorization header
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                // send request
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(translationState.Translate("Contact_Administrator"));
                }
                var responselist = response.Content.ReadFromJsonAsync<IEnumerable<LmsLiteratureType>>();
                var queryableX = (await responselist).AsQueryable();
                return queryableX;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region Create
        public async Task<ApiCallResponse> CreateLmsLiteratureType(LmsLiteratureType lmsLiteratureType)
        {
            try
            {
                try
                {

                    var response = await SubmitLmsLiteratureType(lmsLiteratureType);
                    if (response.IsSuccessStatusCode)
                    {
                        return response;
                    }
                    else
                    {
                        return response;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
               
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //<History Author = 'Aqeel Altaf' Date='2022-03-16' Version="1.0" Branch="master"> Fill data from loginstate</History>
        protected async Task<ApiCallResponse> SubmitLmsLiteratureType(LmsLiteratureType item)
        {
            try
            {
                item.CreatedBy = await browserStorage.GetItemAsync<string>("User");
                item.CreatedDate = DateTime.Now;
                item.IsDeleted = false;
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/LmsLiteratureType/CreateLmsLiteratureType");
                var postBody = item;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<LmsLiteratureType>();
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
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Import/Export
        public async Task ExportLmsLiteratureTypesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/fatwadb/lmsliteraturetypes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/fatwadb/lmsliteraturetypes/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportLmsLiteratureTypesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/fatwadb/lmsliteraturetypes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/fatwadb/lmsliteraturetypes/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }
        #endregion

        #region Update
        protected LmsLiteratureType UniqueLiteratureType = new();
        public async Task<LmsLiteratureType> GetLmsLiteratureTypeById(int id)
        {
            try
            {
                UniqueLiteratureType = await GetUniqueLmsLiteratureType(id);
                OnLmsLiteratureTypeGet(UniqueLiteratureType);
                return await Task.FromResult(UniqueLiteratureType);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        partial void OnLmsLiteratureTypeGet(LmsLiteratureType item);
        protected LmsLiteratureType task = new();

        private async Task<LmsLiteratureType> GetUniqueLmsLiteratureType(int Id)
        {
            try
            {


                //var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratures/GetLmsLiteratureById?id=" + Id);
                //request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                //UniqueLiteratureType = await new HttpClient().GetFromJsonAsync<LmsLiterature>(request.RequestUri);

                // send request
                UniqueLiteratureType = await new HttpClient().GetFromJsonAsync<LmsLiteratureType>(_config.GetValue<string>("api_url") + "/LmsLiteratureType/GetLmsLiteratureTypeById?id=" + Id);

                return UniqueLiteratureType;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }

        public async Task<ApiCallResponse> UpdateLmsLiteratureType(int id, LmsLiteratureType lmsLiteratureType)
        {
            try
            {
                UniqueLiteratureType = await GetUniqueLmsLiteratureType(id);
                if (UniqueLiteratureType == null)
                {
                    throw new Exception(translationState.Translate("Item_Unavailable"));
                }

                var response = await UpdateLiteratureType(lmsLiteratureType);
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }
                else
                {
                    return response;
                }


            
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        //<History Author = 'Aqeel Altaf' Date='2022-03-16' Version="1.0" Branch="master">Call Fill data from loginstate</History>
        protected async Task<ApiCallResponse> UpdateLiteratureType(LmsLiteratureType item)
        {
            try
            {

                item.ModifiedBy = await browserStorage.GetItemAsync<string>("User");
                item.ModifiedDate = DateTime.Now;
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/LmsLiteratureType/UpdateLmsLiteratureType");
                var postBody = item;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<LmsLiteratureType>();
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

                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Delete
        public async Task<ApiCallResponse> DeleteLmsLiteratureType(int id)
        {
            try
            {
                UniqueLiteratureType = await GetUniqueLmsLiteratureType(id);

                if (UniqueLiteratureType == null)
                {
                    throw new Exception(translationState.Translate("Item_Unavailable"));
                }

                OnLmsLiteratureDeleted(UniqueLiteratureType);
                var request = new HttpRequestMessage(HttpMethod.Delete, _config.GetValue<string>("api_url") + "/LmsLiteratureType/" + id);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<int>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception("تعذر حذف الأنواع المتصلة!");
                }
                OnAfterLmsLiteratureDeleted(UniqueLiteratureType);

                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = UniqueLiteratureType };
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        partial void OnLmsLiteratureDeleted(LmsLiteratureType item);
        partial void OnAfterLmsLiteratureDeleted(LmsLiteratureType item);

        #endregion

        #region Remote Data for Add Literature Drop Down
        private static List<LmsLiteratureType> AllData { get; set; }
        //<History Author = 'Hassan Abbas' Date='2022-03-22' Version="1.0" Branch="master">Remote Types Data for Add Literature Drop Down</History>
        public async Task<DataEnvelope<LmsLiteratureType>> GetTypesItems(DataSourceRequest request)
        {
            try
            {
                //if (AllData == null)
                //{
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteraturetype/GetLmsLiteraturetypes");
                // add authorization header
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                // send request
                var response = await new HttpClient().SendAsync(request2);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(translationState.Translate("Contact_Administrator"));
                }
                AllData = await response.Content.ReadFromJsonAsync<List<LmsLiteratureType>>();

                //}
                var result = await AllData.ToDataSourceResultAsync(request);
                DataEnvelope<LmsLiteratureType> dataToReturn = new DataEnvelope<LmsLiteratureType>
                {
                    Data = result.Data.Cast<LmsLiteratureType>().ToList(),
                    Total = result.Total
                };

                return await Task.FromResult(dataToReturn);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-03-22' Version="1.0" Branch="master">Filter Item based on value</History>
        public async Task<LmsLiteratureType> GetItemFromValue(int selectedValue)
        {
            ////await Task.Delay(400);
            if (AllData != null)
            {
                return await Task.FromResult(AllData.FirstOrDefault(x => selectedValue == x.TypeId));
            }
            else
            {
                return await Task.FromResult<LmsLiteratureType>(null);
            }
        }
        #endregion
    }

}
