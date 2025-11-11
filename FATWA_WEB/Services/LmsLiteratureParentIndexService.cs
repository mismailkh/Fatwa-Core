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
using Telerik.DataSource.Extensions;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Services
{
    public partial class LmsLiteratureParentIndexService
    {
        public LmsLiteratureParentIndexService(ILocalStorageService _browserStorage, IConfiguration config, NavigationManager _navigationManager, TranslationState _translationState)
        {
            browserStorage = _browserStorage;
            _config = config;
            navigationManager = _navigationManager;
            translationState = _translationState;

        }
        #region variables declaration
        private LmsLiteratureParentIndex lmsLiteratureParentIndexSingleResult;
        private readonly IConfiguration _config;
        private readonly NavigationManager navigationManager;
        private readonly ILocalStorageService browserStorage;
        private readonly TranslationState translationState;
        public string? ResultDetails { get; set; }

        #endregion

        #region Get all & by Id
        partial void OnLmsLiteratureParentIndexsRead(ref IQueryable<LmsLiteratureParentIndex> items);
        public async Task<IQueryable<LmsLiteratureParentIndex>> GetLmsLiteratureParentIndexs()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratureParentIndex/GetLmsLiteratureParentIndexs");
                // add authorization header
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                // send request
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(translationState.Translate("Contact_Administrator"));
                }
                var responselist = response.Content.ReadFromJsonAsync<IEnumerable<LmsLiteratureParentIndex>>();
                var queryableX = (await responselist).AsQueryable();
                return queryableX;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<LmsLiteratureParentIndex> GetLiteratureParentIndexDetailById(int parentIndexId)
        {
            try
            {
                // send request
                lmsLiteratureParentIndexSingleResult = await new HttpClient().GetFromJsonAsync<LmsLiteratureParentIndex>(_config.GetValue<string>("api_url") + "/LmsLiteratureParentIndex/GetLiteratureParentIndexDetailById?parentIndexId=" + parentIndexId);
                return lmsLiteratureParentIndexSingleResult;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> CheckLiteratureParentIndexByUsingParentIndexNumber(LmsLiteratureParentIndex args)
        {
            try
            {
                // send request
                var result = await new HttpClient().GetFromJsonAsync<bool>(_config.GetValue<string>("api_url") + "/LmsLiteratureParentIndex/CheckLiteratureParentIndexByUsingParentIndexNumber?parentIndexNumber=" + args.ParentIndexNumber + "&name_En=" + args.Name_En + "&name_Ar=" + args.Name_Ar);
                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> CheckLiteratureParentIndexByUsingParentNumber(string parentIndexNumber)
        {
            try
            {
                // send request
                var result = await new HttpClient().GetFromJsonAsync<bool>(_config.GetValue<string>("api_url") + "/LmsLiteratureParentIndex/CheckLiteratureParentIndexByUsingParentNumber?parentIndexNumber=" + parentIndexNumber);
                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<LmsLiteratureParentIndex> GetLmsLiteratureParentIndexDetailByNumber(string parentIndexNumber)
        {
            try
            {
                // send request
                var result = await new HttpClient().GetFromJsonAsync<LmsLiteratureParentIndex>(_config.GetValue<string>("api_url") + "/LmsLiteratureParentIndex/GetLmsLiteratureParentIndexDetailByNumber?parentIndexNumber=" + parentIndexNumber);

                return result;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Create
        public async Task<ApiCallResponse> CreateLmsLiteratureParentIndex(LmsLiteratureParentIndex lmsLiteratureParentIndex)
        {
            try
            {
                
                try
                {
                   var response = await SubmitLmsLiteratureParentIndex(lmsLiteratureParentIndex);
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
        protected async Task<ApiCallResponse> SubmitLmsLiteratureParentIndex(LmsLiteratureParentIndex item)
        {
            try
            {
                item.CreatedBy = await browserStorage.GetItemAsync<string>("User");
                item.CreatedDate = DateTime.Now;
                item.IsDeleted = false;

                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/LmsLiteratureParentIndex/CreateLmsLiteratureParentIndex");
                var postBody = item;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<LmsLiteratureParentIndex>();
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
        public async Task ExportLmsLiteratureParentIndexsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/fatwadb/lmsliteratureParentIndexs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/fatwadb/lmsliteratureParentIndexs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportLmsLiteratureParentIndexsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/fatwadb/lmsliteratureParentIndexs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/fatwadb/lmsliteratureParentIndexs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }
        #endregion

        #region Update
        protected LmsLiteratureParentIndex UniqueLiteratureParentIndex = new();
       
        protected LmsLiteratureParentIndex task = new();

        private async Task<LmsLiteratureParentIndex> GetUniqueLmsLiteratureParentIndex(int parentIndexId)
        {
            try
            {
                UniqueLiteratureParentIndex = await new HttpClient().GetFromJsonAsync<LmsLiteratureParentIndex>(_config.GetValue<string>("api_url") + "/LmsLiteratureParentIndex/GetLiteratureParentIndexDetailById?parentIndexId=" + parentIndexId);
                return UniqueLiteratureParentIndex;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiCallResponse> UpdateLmsLiteratureParentIndex(int parentIndexId, LmsLiteratureParentIndex lmsLiteratureParentIndex)
        {
            try
            {
               
                UniqueLiteratureParentIndex = await GetUniqueLmsLiteratureParentIndex(parentIndexId);
                if (UniqueLiteratureParentIndex == null)
                {
                    throw new Exception(translationState.Translate("Item_Unavailable"));
                }

               var response = await UpdateLiteratureParentIndex(lmsLiteratureParentIndex);
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
       
        protected async Task<ApiCallResponse> UpdateLiteratureParentIndex(LmsLiteratureParentIndex item)
        {
            try
            {
                item.ModifiedBy = await browserStorage.GetItemAsync<string>("User");
                item.ModifiedDate = DateTime.Now;

                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/LmsLiteratureParentIndex/UpdateLmsLiteratureParentIndex");
                var postBody = item;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<LmsLiteratureParentIndex>();
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
        public async Task<LmsLiteratureParentIndex> DeleteLmsLiteratureParentIndex(int parentIndexId)
        {
            try
            {
                UniqueLiteratureParentIndex = await GetUniqueLmsLiteratureParentIndex(parentIndexId);

                if (UniqueLiteratureParentIndex == null)
                {
                    throw new Exception(translationState.Translate("Item_Unavailable"));
                }

                OnLmsLiteratureParentIndexDeleted(UniqueLiteratureParentIndex);
                UniqueLiteratureParentIndex.DeletedBy = await browserStorage.GetItemAsync<string>("User");
                UniqueLiteratureParentIndex.DeletedDate = DateTime.Now;
                UniqueLiteratureParentIndex.IsDeleted = true;
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/LmsLiteratureParentIndex/SoftDeleteLiteratureParentIndex");
                var postBody = UniqueLiteratureParentIndex;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ResultDetails = translationState.Translate("Literature_Parent_Index_Delete_Success");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(translationState.Translate("Literature_Parent_Index_Delete_Error"));
                }

                OnAfterLmsLiteratureParentIndexDeleted(UniqueLiteratureParentIndex);

                return UniqueLiteratureParentIndex;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        partial void OnLmsLiteratureParentIndexDeleted(LmsLiteratureParentIndex item);
        partial void OnAfterLmsLiteratureParentIndexDeleted(LmsLiteratureParentIndex item);

        #endregion
    }

}
