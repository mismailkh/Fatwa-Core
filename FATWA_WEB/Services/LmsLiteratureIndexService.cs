using Blazored.LocalStorage;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.ViewModel;
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
    public partial class LmsLiteratureIndexService
    {
        public LmsLiteratureIndexService(ILocalStorageService _browserStorage, IConfiguration config, NavigationManager _navigationManager, TranslationState _translationState)
        {
            browserStorage = _browserStorage;
            _config = config;
            navigationManager = _navigationManager;
            translationState = _translationState;
        }
        #region variables declaration

        private IEnumerable<LmsLiteratureIndex> LmsLiteratureIndexesId;
        private IEnumerable<LmsLiterature> LmsLiteratureResult;

        private readonly IConfiguration _config;
        private readonly NavigationManager navigationManager;
        private readonly ILocalStorageService browserStorage;
        private readonly TranslationState translationState;
        private LmsLiteratureIndex _literaturebyindexId;

        public string? ResultDetails { get; set; }
        #endregion

        public LmsLiteratureIndexService(IConfiguration config, NavigationManager _navigationManager, ILocalStorageService _browserStorage, TranslationState _translationState)
        {
            _config = config;
            navigationManager = _navigationManager;
            browserStorage = _browserStorage;
            translationState = _translationState;

        }
        #region Get, ById
        public async Task<IQueryable<LmsLiteratureIndex>> GetLmsLiteratureIndexs(Query query = null)
        {
            try
            {
                var items = await GetLmsLiteratureIndexs();

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

                OnLmsLiteratureIndexsRead(ref items);

                return await Task.FromResult(items);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        partial void OnLmsLiteratureIndexsRead(ref IQueryable<LmsLiteratureIndex> items);
        private async Task<IQueryable<LmsLiteratureIndex>> GetLmsLiteratureIndexs()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratureIndex/GetLmsLiteratureIndexs");
                // add authorization header
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                // send request
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(translationState.Translate("Contact_Administrator"));
                }
                var responselist = response.Content.ReadFromJsonAsync<IEnumerable<LmsLiteratureIndex>>();
                var queryableX = (await responselist).AsQueryable();
                return queryableX;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiCallResponse> GetLmsLiteratureIndexDetilsByUsingParentIdAndNumber(int parentIndexId, string parentIndexNumber)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratureIndex/GetLmsLiteratureIndexDetilsByUsingParentIdAndNumber?parentIndexId=" + parentIndexId + "&parentIndexNumber=" + parentIndexNumber);
                // add authorization header
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                // send request
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    var content = new List<LmsLiteratureIndex>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<LmsLiteratureIndex>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else
                {
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Create
        public async Task<ApiCallResponse> CreateLmsLiteratureIndex(LmsLiteratureIndex lmsLiteratureIndex)
        {
            try
            {
                //OnLmsLiteratureIndexCreated(lmsLiteratureIndex);
                try
                {

                   var response = await SubmitLmsLiteratureIndex(lmsLiteratureIndex);
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
                //OnAfterLmsLiteratureIndexCreated(lmsLiteratureIndex);

                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //partial void OnLmsLiteratureIndexCreated(LmsLiteratureIndex item);
        //partial void OnAfterLmsLiteratureIndexCreated(LmsLiteratureIndex item);
        //<History Author = 'Aqeel Altaf' Date='2022-03-16' Version="1.0" Branch="master"> Fill data from translationState</History>
        protected async Task<ApiCallResponse> SubmitLmsLiteratureIndex(LmsLiteratureIndex item)
        {
            try
            {
                item.IndexCreationDate = DateTime.Now;
                item.CreatedBy = await browserStorage.GetItemAsync<string>("User");
                item.CreatedDate = DateTime.Now;
                item.IsDeleted = false;

                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/LmsLiteratureIndex/CreateLmsLiteratureIndex");
                var postBody = item;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<LmsLiteratureIndex>();
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
        public async Task ExportLmsLiteratureIndexsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/fatwadb/lmsliteratureIndexs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/fatwadb/lmsliteratureIndexs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportLmsLiteratureIndexsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/fatwadb/lmsliteratureIndexs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/fatwadb/lmsliteratureIndexs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }
        #endregion

        #region Update
        protected LmsLiteratureIndex UniqueLiteratureIndex = new();
        public async Task<LmsLiteratureIndex> GetLmsLiteratureIndexById(int indexId)
        {
            try
            {
                UniqueLiteratureIndex = await GetUniqueLmsLiteratureIndex(indexId);
                OnLmsLiteratureIndexGet(UniqueLiteratureIndex);
                return await Task.FromResult(UniqueLiteratureIndex);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<LmsLiteratureIndex> GetLiteratureIndexDetailByUsingIndexId(int indexNo)
        {
            try
            {
                UniqueLiteratureIndex = await GetUniqueLmsLiteratureIndexByIndexNo(indexNo);
                return await Task.FromResult(UniqueLiteratureIndex);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        partial void OnLmsLiteratureIndexGet(LmsLiteratureIndex item);
        protected LmsLiteratureIndex task = new();

        private async Task<LmsLiteratureIndex> GetUniqueLmsLiteratureIndex(int indexId)
        {
            try
            {
                UniqueLiteratureIndex = await new HttpClient().GetFromJsonAsync<LmsLiteratureIndex>(_config.GetValue<string>("api_url") + "/LmsLiteratureIndex/GetLmsLiteratureIndexById?indexId=" + indexId);
                return UniqueLiteratureIndex;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        private async Task<LmsLiteratureIndex> GetUniqueLmsLiteratureIndexByIndexNo(int indexNo)
        {
            try
            {
                UniqueLiteratureIndex = await new HttpClient().GetFromJsonAsync<LmsLiteratureIndex>(_config.GetValue<string>("api_url") + "/LmsLiteratureIndex/GetLiteratureIndexDetailByUsingIndexId?indexId=" + indexNo);
                return UniqueLiteratureIndex;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }



        public async Task<ApiCallResponse> UpdateLmsLiteratureIndex(int indexId, LmsLiteratureIndex lmsLiteratureIndex)
        {
            try
            {
                //OnLmsLiteratureIndexUpdated(lmsLiteratureIndex);
                UniqueLiteratureIndex = await GetUniqueLmsLiteratureIndex(indexId);
                if (UniqueLiteratureIndex == null)
                {
                    throw new Exception(translationState.Translate("Item_Unavailable"));
                }

               var response = await UpdateLiteratureIndex(lmsLiteratureIndex);
                if (response.IsSuccessStatusCode)
                {
                    return response;
                }
                else
                {
                    return response;
                }
                //OnAfterLmsLiteratureIndexUpdated(lmsLiteratureIndex);

                
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        //partial void OnAfterLmsLiteratureIndexUpdated(LmsLiteratureIndex item);
        //partial void OnLmsLiteratureIndexUpdated(LmsLiteratureIndex item);
        //<History Author = 'Aqeel Altaf' Date='2022-03-16' Version="1.0" Branch="master">Call Fill data from translationState</History>
        protected async Task<ApiCallResponse> UpdateLiteratureIndex(LmsLiteratureIndex item)
        {
            try
            {
                item.ModifiedBy = await browserStorage.GetItemAsync<string>("User");
                item.ModifiedDate = DateTime.Now;

                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/LmsLiteratureIndex/UpdateLmsLiteratureIndex");
                var postBody = item;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<LmsLiteratureIndex>();
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
        public async Task<LmsLiteratureIndex> DeleteLmsLiteratureIndex(int indexId)
        {
            try
            {
                UniqueLiteratureIndex = await GetUniqueLmsLiteratureIndex(indexId);

                if (UniqueLiteratureIndex == null)
                {
                    throw new Exception(translationState.Translate("Item_Unavailable"));
                }

                OnLmsLiteratureDeleted(UniqueLiteratureIndex);
                UniqueLiteratureIndex.DeletedBy = await browserStorage.GetItemAsync<string>("User");
                UniqueLiteratureIndex.DeletedDate = DateTime.Now;
                UniqueLiteratureIndex.IsDeleted = true;
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/LmsLiteratureIndex/SoftDeleteLiteratureIndex");
                var postBody = UniqueLiteratureIndex;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    ResultDetails = translationState.Translate("Literature_Index_Delete_Success");
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception("تعذر حذف الفهارس المتصلة!");
                }

                OnAfterLmsLiteratureDeleted(UniqueLiteratureIndex);

                return UniqueLiteratureIndex;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        partial void OnLmsLiteratureDeleted(LmsLiteratureIndex item);
        partial void OnAfterLmsLiteratureDeleted(LmsLiteratureIndex item);

        #endregion

        #region Get_All_Related_IndexesId
        public async Task<IEnumerable<LmsLiteratureIndex>> GetLmsLiteratureIndexesIdByIndexNumber(string indexNumber)
        {
            try
            {
                // send request
                LmsLiteratureIndexesId = await new HttpClient().GetFromJsonAsync<IEnumerable<LmsLiteratureIndex>>(_config.GetValue<string>("api_url") + "/LmsLiteratureIndex/GetLmsLiteratureIndexesIdByIndexNumber?indexNumber=" + indexNumber);

                return LmsLiteratureIndexesId;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<IEnumerable<LmsLiteratureIndex>> GetLmsLiteratureIndexDetailByUsingNameAndIndexNumber(LmsLiteratureIndex args)
        {
            try
            {
                // send request
                LmsLiteratureIndexesId = await new HttpClient().GetFromJsonAsync<IEnumerable<LmsLiteratureIndex>>(_config.GetValue<string>("api_url") + "/LmsLiteratureIndex/GetLmsLiteratureIndexDetailByUsingNameAndIndexNumber?indexNumber=" + args.IndexNumber + "&name_En=" + args.Name_En + "&name_Ar=" + args.Name_Ar);

                return LmsLiteratureIndexesId;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiCallResponse> GetLiteratureIndexByIndexIdAndNumber(int indexId, string indexNumber) 
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratureIndex/GetLiteratureIndexByIndexIdAndNumber?indexId=" + indexId + "&indexNumber=" + indexNumber);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                var content = new LmsLiteratureParentIndexVM();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }
            else if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<LmsLiteratureParentIndexVM>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }
            else
            {
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
            }
        }
        
        #endregion

        #region Check Lms Literature IndexId Associated With Literatures
        public async Task<IEnumerable<LmsLiterature>> CheckLmsLiteratureIndexIdAssociatedWithLiteratures(int indexId)
        {
            try
            {
                // send request
                LmsLiteratureResult = await new HttpClient().GetFromJsonAsync<IEnumerable<LmsLiterature>>(_config.GetValue<string>("api_url") + "/LmsLiteratureIndex/CheckLmsLiteratureIndexIdAssociatedWithLiteratures?indexId=" + indexId);

                return LmsLiteratureResult;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Remote Data for Add Literature Drop Down
        private List<LmsLiteratureIndex> AllData { get; set; } = new List<LmsLiteratureIndex>();
        private static List<LmsLiteratureIndexDivisionAisle> AllDivisionsData { get; set; }
        private static List<LmsLiteratureIndexDivisionAisle> AllAislesData { get; set; }
        public async Task<IQueryable<LmsLiteratureIndex>> GetLiteratureIndexDetails()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratureIndex/GetLmsLiteratureIndexs");
            // add authorization header
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            // send request
            var response = await new HttpClient().SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(translationState.Translate("Contact_Administrator"));
            }
            //AllData = await response.Content.ReadFromJsonAsync<List<LmsLiteratureIndex>>();

            //var result = await AllData.ToDataSourceResultAsync(request);
            //DataEnvelope<LmsLiteratureIndex> dataToReturn = new DataEnvelope<LmsLiteratureIndex>
            //{
            //    Data = result.Data.Cast<LmsLiteratureIndex>().ToList(),
            //    Total = result.Total
            //};

            //return await Task.FromResult(dataToReturn);
            var responselist = response.Content.ReadFromJsonAsync<IEnumerable<LmsLiteratureIndex>>();
            AllData = (List<LmsLiteratureIndex>)await responselist;
            var queryableX = (await responselist).AsQueryable();
            return queryableX;
        }
        //<History Author = 'Hassan Abbas' Date='2022-03-22' Version="1.0" Branch="master">Filter Item based on value</History>
        public async Task<LmsLiteratureIndex> GetLiteratureIndexByValue(int selectedValue)
        {
            if (AllData != null)
            {
                return await Task.FromResult(AllData.FirstOrDefault(x => selectedValue == x.IndexId));
            }
            else
            {
                return await Task.FromResult<LmsLiteratureIndex>(null);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-04-11' Version="1.0" Branch="master">Remote Division Data for Add Literature Drop Down</History>
        public async Task<DataEnvelope<LmsLiteratureIndexDivisionAisle>> GetLiteratureIndexDivisionsDetails(DataSourceRequest request, int indexId)
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratureIndexDivisionAisle/GetLmsLiteratureDivisionDetailsByUsingIndexId?indexId=" + indexId);
            // add authorization header
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            // send request
            var response = await new HttpClient().SendAsync(request2);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(translationState.Translate("Contact_Administrator"));
            }
            AllDivisionsData = await response.Content.ReadFromJsonAsync<List<LmsLiteratureIndexDivisionAisle>>();

            var result = await AllDivisionsData.ToDataSourceResultAsync(request);
            DataEnvelope<LmsLiteratureIndexDivisionAisle> dataToReturn = new DataEnvelope<LmsLiteratureIndexDivisionAisle>
            {
                Data = result.Data.Cast<LmsLiteratureIndexDivisionAisle>().ToList(),
                Total = result.Total
            };

            return await Task.FromResult(dataToReturn);
        }
        //<History Author = 'Hassan Abbas' Date='2022-04-11' Version="1.0" Branch="master">Filter Item based on value</History>
        public async Task<LmsLiteratureIndexDivisionAisle> GetIndexDivisionItemFromValue(string selectedValue, int selectedIndexId)
        {
            if (AllDivisionsData != null)
            {
                return await Task.FromResult(AllDivisionsData.FirstOrDefault(x => selectedValue == x.DivisionNumber && selectedIndexId == x.IndexId));
            }
            else
            {
                return await Task.FromResult<LmsLiteratureIndexDivisionAisle>(null);
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-04-11' Version="1.0" Branch="master">Remote Aisles Data for Add Literature Drop Down</History>
        public async Task<DataEnvelope<LmsLiteratureIndexDivisionAisle>> GetIndexDivisionAisleItems(DataSourceRequest request, int indexId, int divisionNumber)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratureIndexDivisionAisle/GetLmsLiteratureAisleNumberDetailsByUsingIndexAndDivisionNumber?indexId=" + indexId + "&divisionNumber=" + divisionNumber);
                // add authorization header
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));

                // send request

                var response = await new HttpClient().SendAsync(request2);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(translationState.Translate("Contact_Administrator"));
                }
                AllAislesData = await response.Content.ReadFromJsonAsync<List<LmsLiteratureIndexDivisionAisle>>();

                var result = await AllAislesData.ToDataSourceResultAsync(request);
                DataEnvelope<LmsLiteratureIndexDivisionAisle> dataToReturn = new DataEnvelope<LmsLiteratureIndexDivisionAisle>
                {
                    Data = result.Data.Cast<LmsLiteratureIndexDivisionAisle>().ToList(),
                    Total = result.Total
                };

                return await Task.FromResult(dataToReturn);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-04-11' Version="1.0" Branch="master">Filter Item based on value</History>
        //<History Author = 'Umer Zaman' Date='2022-07-18' Version="1.0" Branch="master">Modified parameters in method</History>
        public async Task<LmsLiteratureIndexDivisionAisle> GetIndexDivisionAisleItemFromValue(string selectedValue, string SelectedDivisionNumber, int SelectedIndexId)
        {
            if (AllAislesData != null)
            {
                return await Task.FromResult(AllAislesData.FirstOrDefault(x => selectedValue == x.AisleNumber && SelectedDivisionNumber == x.DivisionNumber && SelectedIndexId == x.IndexId));
            }
            else
            {
                return await Task.FromResult<LmsLiteratureIndexDivisionAisle>(null);
            }
        }
        #endregion
    }

}
