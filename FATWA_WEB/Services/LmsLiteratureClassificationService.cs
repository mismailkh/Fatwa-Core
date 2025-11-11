using Blazored.LocalStorage;
using FATWA_DOMAIN.Models;
using FATWA_WEB.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Services
{
    //<History Author = 'Hassan Abbas' Date='2022-03-16' Version="1.0" Branch="master"> Lms Literature Classification Service for communicating with API Literature Classifications Controller</History>
    //<History Author = 'Zain Ul Islam' Date='2022-08-01' Version="1.0" Branch="master"> Lms Literature Classification Service Generic Solution Reverted</History>

    public partial class LmsLiteratureClassificationService
    {
        private readonly IConfiguration _config;
        private readonly NavigationManager navigationManager;
        private readonly ILocalStorageService browserStorage;
        private readonly TranslationState translationState;

        public LmsLiteratureClassificationService(IConfiguration configuration,
            NavigationManager _navigationManager,
            ILocalStorageService _browserStorage,
            TranslationState _translationState)
        {
            _config = configuration;
            navigationManager = _navigationManager;
            browserStorage = _browserStorage;
            translationState = _translationState;
        }


        //<History Author = 'Nadia Gull' Date='2022-10-23' Version="1.0" Branch="master"> Add ApiCallResponse </History>
        public async Task<ApiCallResponse> GetLiteratureClassifications()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratureClassifications/GetLmsLiteratureClassifications");
                // add authorization header
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                // send request
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<LmsLiteratureClassification>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
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

        #region Create

        //<History Author = 'Hassan Abbas' Date='2022-03-16' Version="1.0" Branch="master"> Creating Literature Classifications</History>
        public async Task<IQueryable<LmsLiteratureClassification>> LoadData(LoadDataArgs args)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratureClassifications/GetLmsLiteratureClassifications");
            request.RequestUri.GetODataUri(filter: args.Filter, top: args.Top, skip: args.Skip, orderby: args.OrderBy, count: true);
            // add authorization header
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            // send request
            var response = await new HttpClient().SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(translationState.Translate("Contact_Administrator"));
            }
            var responselist = response.Content.ReadFromJsonAsync<IEnumerable<LmsLiteratureClassification>>();
            var queryableX = (await responselist).AsQueryable();
            return queryableX;
        }




        //<History Author = 'Hassan Abbas' Date='2022-03-16' Version="1.0" Branch="master"> Call API for creating Literature Classifications</History>
        public async Task<ApiCallResponse> SubmitLmsLiteratureClassification(LmsLiteratureClassification item)
        {
            try
            {
                item.CreatedBy = await browserStorage.GetItemAsync<string>("User");
                item.CreatedDate = DateTime.Now;
                item.IsDeleted = false;
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/LmsLiteratureClassifications/CreateLmsLiteratureClassification");
                var postBody = item;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<LmsLiteratureClassification>();
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

        #region Update
        protected LmsLiteratureClassification UniqueLiteratureClassification = new();
        protected LmsLiteratureClassification task = new();

        //<History Author = 'Hassan Abbas' Date='2022-03-16' Version="1.0" Branch="master"> Call API for getting Literature Classification by Id</History>
        public async Task<ApiCallResponse> GetUniqueLmsLiteratureClassifications(int Id)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratureClassifications/GetLiteratureClassificationDetailById?id=" + Id);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<LmsLiteratureClassification>();
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

        
        //<History Author = 'Hassan Abbas' Date='2022-03-16' Version="1.0" Branch="master">Call API for Updating Literature Classification</History>
        public async Task<ApiCallResponse> UpdateLiteratureClassification(LmsLiteratureClassification item)
        {
            try
            {

                item.ModifiedBy = await browserStorage.GetItemAsync<string>("User");
                item.ModifiedDate = DateTime.Now;
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/LmsLiteratureClassifications/UpdateLmsLiteratureClassification");
                var postBody = item;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<LmsLiteratureClassification>();
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

        #region Delete
        //<History Author = 'Hassan Abbas' Date='2022-03-16' Version="1.0" Branch="master">Deleing Literature Classification</History>

        public async Task<ApiCallResponse> DeleteLmsLiteratureClassification(int id)
        {
            try
            {
                
                var request = new HttpRequestMessage(HttpMethod.Delete, _config.GetValue<string>("api_url") + "/LmsLiteratureClassifications/" + id);
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

        #region Import/Export

        public void ExportLmsLiteratureClassificationsToCSV(Query? query = null, string? fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/fatwadb/lmsliteratureclassifications/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/fatwadb/lmsliteratureclassifications/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }
        public void ExportLmsLiteratureClassificationsToExcel(Query? query = null, string? fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/fatwadb/lmsliteratureclassifications/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/fatwadb/lmsliteratureclassifications/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        #endregion

        #region Remote Data for Add Literature Drop Down
        private static List<LmsLiteratureClassification> AllData { get; set; }
        //<History Author = 'Hassan Abbas' Date='2022-03-22' Version="1.0" Branch="master">Remote Classifications Data for Add Literature Drop Down</History>
        public async Task<DataEnvelope<LmsLiteratureClassification>> GetClassificationItems(DataSourceRequest request)
        {
            //if (AllData == null)
            //{
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LmsLiteratureClassifications/GetLmsLiteratureClassifications");
            // add authorization header
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            // send request
            var response = await new HttpClient().SendAsync(request2);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(translationState.Translate("Contact_Administrator"));
            }
            AllData = await response.Content.ReadFromJsonAsync<List<LmsLiteratureClassification>>();

            //}


            var result = await AllData.ToDataSourceResultAsync(request);
            DataEnvelope<LmsLiteratureClassification> dataToReturn = new DataEnvelope<LmsLiteratureClassification>
            {
                Data = result.Data.Cast<LmsLiteratureClassification>().ToList(),
                Total = result.Total
            };

            return await Task.FromResult(dataToReturn);
        }
        //<History Author = 'Hassan Abbas' Date='2022-03-22' Version="1.0" Branch="master">Filter Item based on value</History>
        public async Task<LmsLiteratureClassification> GetItemFromValue(int selectedValue)
        {
            ////await Task.Delay(400);
            if (AllData != null)
            {
                return await Task.FromResult(AllData.FirstOrDefault(x => selectedValue == x.ClassificationId));
            }
            else
            {
                return await Task.FromResult<LmsLiteratureClassification>(null);
            }
        }
        #endregion
    }
}
