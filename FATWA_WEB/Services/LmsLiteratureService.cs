using Blazored.LocalStorage;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Lms;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.Lms;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_GENERAL.Helper;
using FATWA_WEB.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Radzen;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Encodings.Web;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Services
{
    public partial class LmsLiteratureService
    {
        private readonly IConfiguration _config;
        private readonly NavigationManager navigationManager;
        private readonly ILocalStorageService browserStorage;
        private readonly TranslationState translationState;

        public LmsLiteratureService(IConfiguration configuration, NavigationManager _navigationManager, ILocalStorageService _browserStorage, TranslationState _translationState)
        {
            _config = configuration;
            navigationManager = _navigationManager;
            browserStorage = _browserStorage;
            translationState = _translationState;

        }

        #region Get Literature Methods
        //<History Author = 'Nadia Gull' Date='2022-10-23' Version="1.0" Branch="master"> Call API for getting new Literature List</History>
        public async Task<ApiCallResponse> GetLmsLiteratures(LiteratureAdvancedSearchVM advanceSearchVM, Query query = null)
        {
            try
            {
                var response = await GetLmsLiteratures(advanceSearchVM);
                if (response.IsSuccessStatusCode)
                {
                    var data = (IEnumerable<LiteratureDetailVM>)response.ResultData;
                    var items = data.AsQueryable();
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
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = items };
                }
                else
                {
                    return response;
                }
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };

            }
        }
        //<History Author = 'Nadia Gull' Date='2022-10-23' Version="1.0" Branch="master"> Call API for getting new Literature List</History>
        public async Task<ApiCallResponse> GetLmsLiteratures(LiteratureAdvancedSearchVM advanceSearchVM)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/LmsLiteratures/GetLmsLiteraturesAdvanceSearch");
                var postBody = advanceSearchVM;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<IEnumerable<LiteratureDetailVM>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else
                {
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = false };
                }
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }

        #region Get LMS Litrature Details By Id
        //<History Author = 'Nabeel ur Rehman' Date='2022-08-29' Version="1.0" Branch="master"> get litrature Detail By Id </History>

        protected LiteratureAllDetailsVM LiteratureAllDetailsVM;
        public async Task<ApiCallResponse> GetLMSLiteratureDetailById(int LiteratureId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratures/GetLMSLiteratureDetailById?LiteratureId=" + LiteratureId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<LiteratureAllDetailsVM>();
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
        #region Get LMS Litrature Details By Id
        //<History Author = 'Nabeel ur Rehman' Date='2022-08-29' Version="1.0" Branch="master"> get litrature Detail By Id </History>

        protected LiteratureAllAuthorsVM LiteratureAuthorsVM;
        public async Task<ApiCallResponse> GetLMSLiteratureAuthorsById(int LiteratureId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratures/GetLMSLiteratureAuthorslById?LiteratureId=" + LiteratureId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<LiteratureAllAuthorsVM>>();
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
        #region Get LMS Litrature Details By Id

        public async Task<ApiCallResponse> GetBorrowDetailById(int LiteratureId)
        {
            try
            {
                var loggedInUser = await browserStorage.GetItemAsync<UserDetailVM>("UserDetail");
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratures/GetBorrowDetailById?LiteratureId=" + LiteratureId + "&UserId=" + loggedInUser.UserId + "&RoleName=" + loggedInUser.RoleName);

                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<BorrowDetailVM>>();
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
        //<History Author = 'Hassan Abbas' Date='2022-08-30' Version="1.0" Branch="master"> Call API for getting new Literature Number</History>
        public async Task<int> GetNewLmsLiteratureNumber()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratures/GetNewLmsLiteratureNumber");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<int>();
                else
                    return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }


        #endregion

        #region Create
        public async Task<ApiCallResponse> CreateLmsLiterature(LmsLiterature lmsLiterature)
        {
            try
            {
                var result = await SubmitLmsLiterature(lmsLiterature);
                return result;
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }


        }
        protected async Task<ApiCallResponse> SubmitLmsLiterature(LmsLiterature item)
        {
            item.Author_FullName_Ar = item.Author_FirstName_Ar + " " + item.Author_SecondName_Ar + " " + item.Author_ThirdName_Ar;
            item.Author_FullName_En = item.Author_FirstName_En + " " + item.Author_SecondName_En + " " + item.Author_ThirdName_En;
            item.CreatedBy = await browserStorage.GetItemAsync<string>("User");
            item.CreatedDate = DateTime.Now;
            item.IsDeleted = false;
            var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/LmsLiteratures/CreateLmsLiterature");
            var postBody = item;
            request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<LmsLiterature>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }
        }
        #endregion

        #region Import/Export

        public async Task ExportLmsLiteraturesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/FatwaDb/lmsliteratures/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/FatwaDb/lmsliteratures/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportLmsLiteraturesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/FatwaDb/lmsliteratures/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/FatwaDb/lmsliteratures/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        #endregion

        #region Update
        protected LmsLiterature UniqueLiterature = new();
        public async Task<LmsLiterature> GetLmsLiteratureById(int id)
        {
            UniqueLiterature = await GetUniqueLmsLiteratures(id);
            OnLmsLiteratureGet(UniqueLiterature);
            return await Task.FromResult(UniqueLiterature);
        }

        partial void OnLmsLiteratureGet(LmsLiterature item);
        protected LmsLiterature task = new();

        private async Task<LmsLiterature> GetUniqueLmsLiteratures(int Id)
        {
            try
            {
                // send request
                UniqueLiterature = await new HttpClient().GetFromJsonAsync<LmsLiterature>(_config.GetValue<string>("api_url") + "/LmsLiteratures/GetLmsLiteratureById?id=" + Id);

                return UniqueLiterature;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }
        public async Task<LmsLiterature> GetLmsLiteratureTagLiteratureById(int id)
        {
            UniqueLiterature = await GetUniqueLmsLiteratTagsByLiteratureId(id);
            OnLmsLiteratureTagGet(UniqueLiterature);
            return await Task.FromResult(UniqueLiterature);
        }
        partial void OnLmsLiteratureTagGet(LmsLiterature item);
        protected LmsLiterature Tagtask = new();
        private async Task<LmsLiterature> GetUniqueLmsLiteratTagsByLiteratureId(int Id)
        {
            try
            {
                // send request
                UniqueLiterature = await new HttpClient().GetFromJsonAsync<LmsLiterature>(_config.GetValue<string>("api_url") + "/LmsLiteratures/GetLmsLiteratureTagById?id=" + Id);

                return UniqueLiterature;
            }
            catch (Exception ex)
            {

                throw ex;
            }


        }
        //<History Author = 'Hassan Abbas' Date='2022-04-01' Version="1.0" Branch="master">Send updated literature to API</History>
        public async Task<ApiCallResponse> UpdateLmsLiterature(LmsLiterature item)
        {
            item.Author_FullName_Ar = item.Author_FirstName_Ar + " " + item.Author_SecondName_Ar + " " + item.Author_ThirdName_Ar;
            item.Author_FullName_En = item.Author_FirstName_En + " " + item.Author_SecondName_En + " " + item.Author_ThirdName_En;
            item.ModifiedBy = await browserStorage.GetItemAsync<string>("User");
            item.ModifiedDate = DateTime.Now;
            if (item.DeletedLiteratureBarcodes.Count() != 0)
            {
                foreach (var itemDetails in item.DeletedLiteratureBarcodes)
                {
                    itemDetails.DeletedBy = await browserStorage.GetItemAsync<string>("User");
                    itemDetails.DeletedDate = DateTime.Now;
                    itemDetails.IsDeleted = true;
                    itemDetails.Active = false;
                }
            }
            var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/LmsLiteratures/UpdateLmsLiterature");
            var postBody = item;
            request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<LmsLiterature>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }
        }


        #endregion

        #region Delete

        public async Task<ApiCallResponse> DeleteLiterature(IList<LiteratureDetailVM> selectedLiteratures)
        {
            foreach (LiteratureDetailVM item in selectedLiteratures)
            {
                item.DeletedBy = await browserStorage.GetItemAsync<string>("User");
            }
            var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/LmsLiteratures/SoftDeleteLiterature");
            var postBody = selectedLiteratures.ToList();

            //var obj = System.Text.Json.JsonSerializer.Serialize(postBody);
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


        public async Task<LmsLiterature> DeleteLmsLiterature(int id)
        {
            UniqueLiterature = await GetUniqueLmsLiteratures(id);

            if (UniqueLiterature == null)
            {
                throw new Exception(translationState.Translate("Item_Unavailable"));
            }

            UniqueLiterature.DeletedBy = await browserStorage.GetItemAsync<string>("User");
            UniqueLiterature.DeletedDate = DateTime.Now;
            UniqueLiterature.IsDeleted = true;
            var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/LmsLiteratures/SoftDeleteLiterature");
            var postBody = UniqueLiterature;
            request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                //sucessfullyadded
            }

            return UniqueLiterature;
        }
        partial void OnLmsLiteratureDeleted(LmsLiterature item);
        partial void OnAfterLmsLiteratureDeleted(LmsLiterature item);

        #endregion

        #region Barcode

        //<History Author = 'Hassan Abbas' Date='2022-03-22' Version="1.0" Branch="master"> get Barcode from the API for the New Literature</History>
        public async Task<string> GetLiteratureBarcode()
        {
            try
            {
                var barcode = await GenerateLiteratureBarcode();
                return barcode;
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-03-22' Version="1.0" Branch="master"> Call API for getting the Barcode</History>
        private async Task<string> GenerateLiteratureBarcode()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratures/GenerateLiteratureBarcode");
            // add authorization header
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            // send request
            var response = await new HttpClient().SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                //JObject json = JObject.Parse(content);
                //obj["dialog"]["prompt"]
                dynamic obj = JsonConvert.DeserializeObject<dynamic>(content);
                return obj.barCodeNumber;
            }
            else
            {
                return string.Empty;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-08-31' Version="1.0" Branch="master"> Call API for getting the list of Barcodes</History>
        public async Task<ApiCallResponse> GenerateListofLiteratureBarcode(int copyCount)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratures/GenerateListofLiteratureBarcode?copyCount=" + copyCount);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request);
            var content = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {
                var barcodes = await response.Content.ReadFromJsonAsync<List<LmsLiteratureBarcode>>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = barcodes };
            }
            else
            {
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
            }
        }

        #endregion

        #region Remote Data for Literature Author Drop Down
        private static List<LmsLiteratureAuthor> AllAuthorData { get; set; }

        //<History Author = 'Hassan Abbas' Date='2022-03-22' Version="1.0" Branch="master">Filter Item based on value</History>
        public async Task<LmsLiteratureAuthor> GetAuthorItemFromValue(int selectedValue)
        {
            ////await Task.Delay(400);
            if (AllAuthorData != null)
            {
                return await Task.FromResult(AllAuthorData.FirstOrDefault(x => selectedValue == x.AuthorId));
            }
            else
            {
                return await Task.FromResult<LmsLiteratureAuthor>(null);
            }
        }
        public async Task<LmsLiteratureAuthor> GetLmsLiteratureAuthorById(int id)
        {
            try
            {
                // send request
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratures/GetLmsLiteratureAuthorById?authorId=" + id);
                // add authorization header
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                // send request
                var response = await new HttpClient().SendAsync(request2);
                return await response.Content.ReadFromJsonAsync<LmsLiteratureAuthor>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<int> GetLmsLiteratureCountByAuthorId(int id)
        {
            try
            {
                // send request
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratures/GetLmsLiteratureCountByAuthorId?authorId=" + id);
                // add authorization header
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                // send request
                var response = await new HttpClient().SendAsync(request2);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(translationState.Translate("Contact_Administrator"));
                }
                return await response.Content.ReadFromJsonAsync<int>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Literature Tags

        //<History Author = 'Hassan Abbas' Date='2022-08-24' Version="1.0" Branch="master">Tags List from API</History>
        public async Task<ApiCallResponse> GetAllActiveLiteratureTags()
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratures/GetAllActiveLiteratureTags");
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);
            if (response.IsSuccessStatusCode)
            {
                var tags = await response.Content.ReadFromJsonAsync<List<LiteratureTag>>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = tags };
            }
            else
            {
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
            }
        }
        #endregion

        #region Get Author Items
        public async Task<ApiCallResponse> GetAuthorItems()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratures/GetLmsLiteraturesAuthorBySearchTerm");
                // add authorization header
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                // send request
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<LmsLiteratureAuthor>>();
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
                return new ApiCallResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccessStatusCode = false,
                    ResultData = new BadRequestResponse
                    {
                        Message = ex.Message,
                        InnerException = ex.InnerException?.Message
                    }
                };
            }
        }
        #endregion

        #region CHeck RFid Vakue exists
        public async Task<ApiCallResponse> CheckRFIDValueExists(string barCodeNumber, string RFIDValue)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratures/CheckRFIDValueExists?barCodeNumber=" + barCodeNumber + "&rfIdValue=" + RFIDValue);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<bool>();
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
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        #endregion

        #region Get Stock Taking List
        public async Task<ApiCallResponse> GetStockTakingList(StockTakingAdvancedSearchVM advancedSearchVM)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratures/GetStockTakingList");
                var postBody = advancedSearchVM;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<LmsStockTakingListVM>>();
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

                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        #endregion

        #region Get Stock Taking Detail
        public async Task<ApiCallResponse> GetStockTakingDetailById(Guid StockTakingId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratures/GetStockTakingDetailById?Id=" + StockTakingId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<LmsStockTakingDetailVM>();
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

                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        #endregion

        #region Get StockTaking Statuses
        public async Task<ApiCallResponse> GetStockTakingStatuses()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratures/GetStockTakingStatuses");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<LmsStockTakingStatus>>();
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
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        #endregion

        #region Get Total number of Books
        public async Task<ApiCallResponse> GetTotalNoOfBooks()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratures/GetTotalNoOfBooks");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<int>();
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
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        #endregion

        #region Get Lms Book StockTaking Report List
        public async Task<ApiCallResponse> GetLmsBookStockTakingReportList(Guid? StockTakingId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratures/GetLmsBookStockTakingReportList?StockTakingId=" + StockTakingId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<LmsStockTakingBooksReportListVm>>();
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
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        #endregion

        #region Submit StockTaking Report
        public async Task<ApiCallResponse> SubmitStockTakingReport(SaveStockTakingVm saveStockTakingVm)
        {
            try
            {
                if (saveStockTakingVm.IsEdit == false)
                {
                    saveStockTakingVm.stockTaking.CreatedBy = await browserStorage.GetItemAsync<string>("User");
                    saveStockTakingVm.stockTaking.CreatedDate = DateTime.Now;
                }
                else
                {
                    saveStockTakingVm.stockTaking.ModifiedBy = await browserStorage.GetItemAsync<string>("User");
                    saveStockTakingVm.stockTaking.ModifiedDate = DateTime.Now;
                }
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/LmsLiteratures/SubmitStockTakingReport");
                var postBody = saveStockTakingVm;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return new ApiCallResponse { IsSuccessStatusCode = response.IsSuccessStatusCode, StatusCode = response.StatusCode };
                }
                else
                {
                    var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                    return new ApiCallResponse { IsSuccessStatusCode = response.IsSuccessStatusCode, StatusCode = response.StatusCode, ResultData = content };
                }
            }
            catch (Exception ex)
            {
                return new ApiCallResponse
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    IsSuccessStatusCode = false,
                    ResultData = new BadRequestResponse
                    {
                        Message = ex.Message,
                        InnerException = ex.InnerException?.Message
                    }
                };
            }
        }
        #endregion

        #region Get Auto Generated Report Number 
        public async Task<ApiCallResponse> GetAutoGeneratedReportNumber()
        {
            try
            {


                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratures/GetAutoGeneratedReportNumber");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<LmsStockTaking>();
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
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        #endregion

        #region Get StockTaking By Id
        public async Task<ApiCallResponse> GetLmsStockTakingById(Guid StockTakingId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratures/GetLmsStockTakingById?StockTakingId=" + StockTakingId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<LmsStockTaking>();
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
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        #endregion

        #region Approve StockTaking Report
        public async Task<ApiCallResponse> ApproveStockTakingReport(Guid StockTakingId)
        {
            try
            {
                var userDetail = await browserStorage.GetItemAsync<UserDetailVM>("UserDetail");
                string ApprovedBy = userDetail.Email;
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratures/ApproveStockTakingReport?Id=" + StockTakingId + "&ApprovedBy=" + ApprovedBy );
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<bool>();
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

                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        #endregion

        #region Get Performers By StockTakingId
        public async Task<ApiCallResponse> GetPerformersByStockTakingId(Guid StockTakingId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratures/GetPerformersByStockTakingId?StockTakingId=" + StockTakingId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<StockTakingPerformerVm>>();
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
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        #endregion

        #region Check If Any InProgress StockTaking
        public async Task<ApiCallResponse> CheckIfAnyInProgressStockTaking()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratures/CheckIfAnyInProgressStockTaking");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<bool>();
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
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        #endregion

        #region Get All Viewable Literature
        public async Task<ApiCallResponse> GetLmsViewableLiteratures(LiteratureAdvancedSearchVM advanceSearchVM)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/LmsLiteratures/GetLmsViewableLiteratures");
                var postBody = advanceSearchVM;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<IEnumerable<LmsViewableLiteratureVM>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else
                {
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = false };
                }
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }

        #endregion

        #region Delete StockTaking Report
        public async Task<ApiCallResponse> DeleteLmsStockTaking(LmsStockTakingListVM item)
        {
            try
            {
                var userDetail = await browserStorage.GetItemAsync<UserDetailVM>("UserDetail");
                item.DeletedBy = userDetail.Email;
                item.DeletedDate = DateTime.Now;
                item.IsDeleted = true;
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/LmsLiteratures/DeleteLmsStockTaking");
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(item), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
                }
                else
                {
                    var content = response.Content.ReadFromJsonAsync<BadRequestResponse>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        #endregion

        #region Get Lms StockTaking History By Id
        public async Task<ApiCallResponse> GetLmsStockTakingHistoryById(Guid StockTakingId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratures/GetLmsStockTakingHistoryById?StockTakingId=" + StockTakingId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer" , await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<LmsStockTakingHistoryVm>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else
                {
                    var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
            }
            catch(Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        #endregion

    }
}
