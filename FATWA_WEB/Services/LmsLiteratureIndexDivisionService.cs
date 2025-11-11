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
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Services
{
    public partial class LmsLiteratureIndexDivisionService
    {
        public LmsLiteratureIndexDivisionService(ILocalStorageService _browserStorage, IConfiguration config, NavigationManager _navigationManager, TranslationState _translationState)
        {
            browserStorage = _browserStorage;
            _config = config;
            navigationManager = _navigationManager;
            translationState = _translationState;
        }

        #region Variables declaration
        private readonly ILocalStorageService browserStorage;
        private readonly TranslationState translationState;
        private IEnumerable<LmsLiteratureIndexDivisionAisle> LmsLiteratureIndexesId;

        private IEnumerable<LmsLiteratureIndexDivisionAisle> registeredDivisionNumber { get; set; }
        private IEnumerable<LmsLiterature> indexIdAssociatedLiteratureResult { get; set; }
        public string? ResultDetails { get; set; }

        private readonly IConfiguration _config;
        private readonly NavigationManager navigationManager;
        #endregion

        #region Get all & ById
        public async Task<IQueryable<LmsLiteratureIndexDivisionAisle>> GetLmsLiteratureIndexDivisions(Query query = null)
        {
            try
            {
                var items = await GetLmsLiteratureIndexDivisions();

                if (query != null)
                {
                    if (!string.IsNullOrEmpty(query.Expand))
                    {
                        var propertiesToExpand = query.Expand.Split(',');
                        foreach (var p in propertiesToExpand)
                        {
                            items = items.Include(p);
                        }
                    }

                    if (!string.IsNullOrEmpty(query.Filter))
                    {
                        if (query.FilterParameters != null)
                        {
                            items = items.Where(query.Filter, query.FilterParameters);
                        }
                        else
                        {
                            items = items.Where(query.Filter);
                        }
                    }

                    if (!string.IsNullOrEmpty(query.OrderBy))
                    {
                        items = items.OrderBy(query.OrderBy);
                    }

                    if (query.Skip.HasValue)
                    {
                        items = items.Skip(query.Skip.Value);
                    }

                    if (query.Top.HasValue)
                    {
                        items = items.Take(query.Top.Value);
                    }
                }

                OnLmsLiteratureIndexDivisionRead(ref items);

                return await Task.FromResult(items);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        partial void OnLmsLiteratureIndexDivisionRead(ref IQueryable<LmsLiteratureIndexDivisionAisle> items);
        private async Task<IQueryable<LmsLiteratureIndexDivisionAisle>> GetLmsLiteratureIndexDivisions()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratureIndexDivisionAisle/GetLmsLiteratureIndexDivisions");
                // add authorization header
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                // send request
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(translationState.Translate("Contact_Administrator"));
                }
                var responselist = response.Content.ReadFromJsonAsync<IEnumerable<LmsLiteratureIndexDivisionAisle>>();
                var queryableX = (await responselist).AsQueryable();
                return queryableX;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<LmsLiteratureIndexDivisionAisle> GetLiteratureIndexDivisionDetail(int divisionAisleId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratureIndexDivisionAisle/GetLiteratureIndexDivisionDetail?divisionAisleId=" + divisionAisleId);
                // add authorization header
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                // send request
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(translationState.Translate("Contact_Administrator"));
                }
                var responselist = response.Content.ReadFromJsonAsync<LmsLiteratureIndexDivisionAisle>();
                var queryableX = await responselist;
                return queryableX;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected LmsLiteratureIndexDivisionAisle UniqueLiteratureIndex = new();

        protected LmsLiteratureIndexDivisionAisle task = new();

        private async Task<LmsLiteratureIndexDivisionAisle> GetUniqueLmsLiteratureIndexDivision(int divisionAisleId)
        {
            try
            {
                // send request
                UniqueLiteratureIndex = await new HttpClient().GetFromJsonAsync<LmsLiteratureIndexDivisionAisle>(_config.GetValue<string>("api_url") + "/LmsLiteratureIndexDivisionAisle/GetLiteratureIndexDivisionDetail?divisionAisleId=" + divisionAisleId);

                return UniqueLiteratureIndex;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }


        }
        public async Task<IEnumerable<LmsLiterature>> GetLmsLiteratureDivisionDetailByUsingDivisionId(int divisionAisleId)
        {
            try
            {
                // send request
                indexIdAssociatedLiteratureResult = await new HttpClient().GetFromJsonAsync<IEnumerable<LmsLiterature>>(_config.GetValue<string>("api_url") + "/LmsLiteratureIndexDivisionAisle/GetLmsLiteratureDivisionDetailByUsingDivisionId?divisionAisleId=" + divisionAisleId);

                return indexIdAssociatedLiteratureResult;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Create Division
        public async Task<ApiCallResponse> CreateLmsLiteratureIndexDivision(LmsLiteratureIndexDivisionAisle lmsLiteratureIndexDivision)
        {
            try
            {
               
                try
                {

                   var response = await  SubmitLmsLiteratureIndexDivision(lmsLiteratureIndexDivision);
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
       
        protected async Task<ApiCallResponse> SubmitLmsLiteratureIndexDivision(LmsLiteratureIndexDivisionAisle item)
        {
            try
            {

                item.CreatedBy = await browserStorage.GetItemAsync<string>("User"); ;
                item.CreatedDate = DateTime.Now;
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/LmsLiteratureIndexDivisionAisle/CreateLmsLiteratureIndexDivision");
                var postBody = item;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<LmsLiteratureIndexDivisionAisle>();
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

        public async Task ExportLmsLiteratureIndexDivisionsToExcel(int indexId, Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/fatwadb/lmsliteratureindexdivisions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}'&indexId=" + indexId + ")") : $"export/fatwadb/lmsliteratureindexdivisions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}'&indexId=" + indexId + ")", true);
        }

        public async Task ExportLmsLiteratureIndexDivisionsToCSV(int indexId, Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/fatwadb/lmsliteratureindexdivisions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}'&indexId=" + indexId + ")") : $"export/fatwadb/lmsliteratureindexdivisions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}'&indexId=" + indexId + ")", true);
        }

        #endregion

        #region Update
        public async Task<LmsLiteratureIndexDivisionAisle> UpdateLmsLiteratureIndexDivision(int divisionAisleId, LmsLiteratureIndexDivisionAisle lmsLiteratureIndexDivision)
        {
            try
            {
                lmsLiteratureIndexDivision.ModifiedBy = await browserStorage.GetItemAsync<string>("User");
                lmsLiteratureIndexDivision.ModifiedDate = DateTime.Now;

                OnLmsLiteratureIndexDivisionUpdated(lmsLiteratureIndexDivision);
                UniqueLiteratureIndex = await GetUniqueLmsLiteratureIndexDivision(divisionAisleId);
                if (UniqueLiteratureIndex == null)
                {
                    throw new Exception(translationState.Translate("Item_Unavailable"));
                }

                await UpdateLmsLiteratureIndexDivision(lmsLiteratureIndexDivision);
                OnAfterLmsLiteratureIndexDivisionUpdated(lmsLiteratureIndexDivision);

                return lmsLiteratureIndexDivision;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        partial void OnAfterLmsLiteratureIndexDivisionUpdated(LmsLiteratureIndexDivisionAisle item);
        partial void OnLmsLiteratureIndexDivisionUpdated(LmsLiteratureIndexDivisionAisle item);
        protected async Task UpdateLmsLiteratureIndexDivision(LmsLiteratureIndexDivisionAisle item)
        {
            try
            {
                item.ModifiedBy = await browserStorage.GetItemAsync<string>("User");
                item.ModifiedDate = DateTime.Now;

                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/LmsLiteratureIndexDivisionAisle/UpdateLmsLiteratureIndexDivision");
                var postBody = item;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(translationState.Translate("Contact_Administrator"));
                }
                if (response.IsSuccessStatusCode)
                {
                    //sucessfullyadded
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region Delete
        public async Task<ApiCallResponse> DeleteLmsLiteratureIndexDivision(int divisionAisleId)
        {
            try
            {
                UniqueLiteratureIndex = await GetUniqueLmsLiteratureIndexDivision(divisionAisleId);

                if (UniqueLiteratureIndex == null)
                {
                    throw new Exception(translationState.Translate("Item_Unavailable"));
                }


                var request = new HttpRequestMessage(HttpMethod.Delete, _config.GetValue<string>("api_url") + "/LmsLiteratureIndexDivisionAisle/" + divisionAisleId);
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
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        

        #endregion

        #region All_Index's_Division's_&_Aisle's_Number
        public async Task<IEnumerable<LmsLiteratureIndexDivisionAisle>> GetLmsLiteratureDivisionDetailsByUsingIndexId(int indexId)
        {
            try
            {
                // send request
                var LmsLiteratureDivisionDetails = await new HttpClient().GetFromJsonAsync<IEnumerable<LmsLiteratureIndexDivisionAisle>>(_config.GetValue<string>("api_url") + "/LmsLiteratureIndexDivisionAisle/GetLmsLiteratureDivisionDetailsByUsingIndexId?indexId=" + indexId);

                return LmsLiteratureDivisionDetails;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<IEnumerable<LmsLiteratureIndexDivisionAisle>> GetLmsLiteratureDivisionDetailsByUsingIndexIdForViewPage(int indexId)
        {
            try
            {
                // send request
                var LmsLiteratureDivisionDetails = await new HttpClient().GetFromJsonAsync<IEnumerable<LmsLiteratureIndexDivisionAisle>>(_config.GetValue<string>("api_url") + "/LmsLiteratureIndexDivisionAisle/GetLmsLiteratureDivisionDetailsByUsingIndexIdForViewPage?indexId=" + indexId);

                return LmsLiteratureDivisionDetails;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<IEnumerable<LmsLiteratureIndexDivisionAisle>> GetDivisionDetailsByUsingIndexAndDivisionId(int divisionAisleId, int indexId)
        {
            try
            {
                // send request
                var LmsLiteratureDivisionDetails = await new HttpClient().GetFromJsonAsync<IEnumerable<LmsLiteratureIndexDivisionAisle>>(_config.GetValue<string>("api_url") + "/LmsLiteratureIndexDivisionAisle/GetDivisionDetailsByUsingIndexAndDivisionId?indexId=" + indexId + "&divisionAisleId=" + divisionAisleId);

                return LmsLiteratureDivisionDetails;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<LmsLiteratureIndexDivisionAisle>> GetLmsLiteratureAisleNumberDetailsByUsingIndexAndDivisionNumber(int indexNumber, int divisionNumber)
        {
            try
            {
                // send request
                registeredDivisionNumber = await new HttpClient().GetFromJsonAsync<IEnumerable<LmsLiteratureIndexDivisionAisle>>(_config.GetValue<string>("api_url") + "/LmsLiteratureIndexDivisionAisle/GetLmsLiteratureAisleNumberDetailsByUsingIndexAndDivisionNumber?indexNumber=" + indexNumber + "&divisionNumber=" + divisionNumber);

                return registeredDivisionNumber;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        #endregion
    }
}
