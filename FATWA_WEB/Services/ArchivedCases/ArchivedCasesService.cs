using Blazored.LocalStorage;
using DocumentFormat.OpenXml.Office2013.Excel;
using FATWA_DOMAIN.Models.ArchivedCasesModels;
using System.Collections.Generic;
using System;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using static FATWA_GENERAL.Helper.Response;
using FATWA_DOMAIN.Models.ViewModel.ArchivedCases;
using FATWA_DOMAIN.Models;

namespace FATWA_WEB.Services.ArchivedCases
{
    public class ArchivedCasesService
    {
        #region Variables
        private readonly IConfiguration _config;
        private readonly ILocalStorageService _browserStorage;
        #endregion

        #region Constructor
        public ArchivedCasesService(IConfiguration configuration, ILocalStorageService browserStorage)
        {
            _config = configuration;
            _browserStorage = browserStorage;
        }
        #endregion
        #region Get Archived Cases 
        public async Task<ApiCallResponse> GetArchivedCaseList(ArchivedCaseAdvanceSearchVM archivedCaseAdvanceSearchVM)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/ArchivedCases/GetArchivedCaseList");
                var postBody2 = archivedCaseAdvanceSearchVM;
                request2.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody2), Encoding.UTF8, "application/json");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var archivedcaseList = await response.Content.ReadFromJsonAsync<IEnumerable<ArchivedCaseListVM>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = archivedcaseList };
                }
                else if (response.StatusCode == HttpStatusCode.NotFound)
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
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        #endregion

        #region Get Archived Case Detail
        public async Task<ApiCallResponse> GetArchiveCaseDetailByCaseId(Guid CaseId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/ArchivedCases/GetArchivedCaseDetailByCaseId?caseId="+ CaseId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var archivedcaseDetail = await response.Content.ReadFromJsonAsync<ArchivedCaseDetailVM>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = archivedcaseDetail };
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

        #region Get Archived Case Documents
        public async Task<ApiCallResponse> GetArchivedCaseDocumentDetailsById(Guid? documentId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/ArchivedCases/GetArchivedCaseDocumentDetailsById?documentId=" + documentId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var archivedcaseDocumentDetail = await response.Content.ReadFromJsonAsync<ArchivedCaseDocuments>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = archivedcaseDocumentDetail };
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
    }
}
