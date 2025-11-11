using Blazored.LocalStorage;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_WEB.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Services
{
    //<History Author = 'Umer Zaman' Date='2022-03-23' Version="1.0" Branch="master"> ErrorLog Service class for managing services</History>
    public partial class ErrorLogService
    {
        private readonly IConfiguration _config;
        private readonly NavigationManager navigationManager;
        private readonly ILocalStorageService browserStorage;
        private readonly TranslationState translationState;

        public ErrorLogService(IConfiguration configuration, NavigationManager _navigationManager, ILocalStorageService _browserStorage, TranslationState _translationState)
        {
            _config = configuration;
            navigationManager = _navigationManager;
            browserStorage = _browserStorage;
            translationState = _translationState;
        }


        #region Error and Process Log List through Procedure
        partial void OnErrorLogRead(ref IQueryable<ErrorLogVM> items);

        public async Task<ApiCallResponse> GetErrorLogByProc()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/AuditLog/GetErrorLogThroughProc");
                // add authorization header
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                // send request
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(translationState.Translate("Contact_Administrator"));
                }
                else if (response.IsSuccessStatusCode)
                {
                    var responselist = response.Content.ReadFromJsonAsync<IEnumerable<ErrorLogVM>>();
                    var queryableX = (await responselist).AsQueryable();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = queryableX };
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
        partial void OnProcessLogReadProc(ref IQueryable<ProcessLogVM> items);

        public async Task<ApiCallResponse> GetProcessLogByProc()
        {
            try
            {

                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/AuditLog/GetProcessLogThroughProc");
                // add authorization header
               request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                // send request
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(translationState.Translate("Contact_Administrator"));
                }
                else if (response.IsSuccessStatusCode)
                {
                    var responselist = response.Content.ReadFromJsonAsync<IEnumerable<ProcessLogVM>>();
                    var queryableX = (await responselist).AsQueryable();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = queryableX };
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
        #endregion
        public async Task<ApiCallResponse> GetErrorLogAdvanceSearch(ErrorLogAdvanceSearchVM searchErrorLog)
        {
            try
            {

                var response = await GetErrorLogAdvanceSearchs(searchErrorLog);
                if (response.IsSuccessStatusCode)
                {
                    var data = (IEnumerable<ErrorLogVM>)response.ResultData;
                    var items = data.AsQueryable();
                    OnErrorLogAdvanceSearchRead(ref items);
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = items };
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


        private async Task<ApiCallResponse> GetErrorLogAdvanceSearchs(ErrorLogAdvanceSearchVM searchErrorLog)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/AuditLog/GetErrorLogAdvanceSearch");
                var postBody = searchErrorLog;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                // add authorization header
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                // send request
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(translationState.Translate("Contact_Administrator"));
                }
                else if (response.IsSuccessStatusCode)
                {
                    var responselist = response.Content.ReadFromJsonAsync<IEnumerable<ErrorLogVM>>();
                    var queryableX = (await responselist).AsQueryable();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = queryableX };
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
        partial void OnErrorLogAdvanceSearchRead(ref IQueryable<ErrorLogVM> items);
        public async Task<ApiCallResponse> GetProcessLogAdvanceSearch(ProcessLogAdvanceSearchVM processLogAdvanceSearchVM)
        {
            try
            {
                var response = await GetProcessLogAdvanceSearchs(processLogAdvanceSearchVM);
                if(response.IsSuccessStatusCode)
                {
                    var data = (IEnumerable<ProcessLogVM>)response.ResultData;
                    var items = data.AsQueryable();
                    OnProcessLogRead(ref items);
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = items };
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


        private async Task<ApiCallResponse> GetProcessLogAdvanceSearchs(ProcessLogAdvanceSearchVM processLogAdvanceSearchVM)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/AuditLog/GetProcessLogAdvanceSearch");
                var postBody = processLogAdvanceSearchVM;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                // add authorization header
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                // send request
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(translationState.Translate("Contact_Administrator"));
                }
                else if (response.IsSuccessStatusCode)
                {
                    var responselist = response.Content.ReadFromJsonAsync<IEnumerable<ProcessLogVM>>();
                    var queryableX = (await responselist).AsQueryable();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = queryableX };
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


        partial void OnProcessLogRead(ref IQueryable<ProcessLogVM> items);


        #region Get Error Log By linq query 
        public async Task<IQueryable<ErrorLog>> GetErrorLog(Query query = null)
        {
            var response = await GetErrorLog();
            if (response.IsSuccessStatusCode)
            {
                var data = (IEnumerable<ErrorLog>)response.ResultData;
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
                OnErrorLogRead(ref items);
                return await Task.FromResult(items);
            }
            else
            {
                return null;
            }
        }
        partial void OnErrorLogRead(ref IQueryable<ErrorLog> items);

        private async Task<ApiCallResponse> GetErrorLog()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/AuditLog/GetErrorLog");
                // add authorization header
                //  request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                // send request
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(translationState.Translate("Contact_Administrator"));
                }
                else if (response.IsSuccessStatusCode)
                {
                    var responselist = response.Content.ReadFromJsonAsync<IEnumerable<ErrorLog>>();
                    var queryableX = (await responselist).AsQueryable();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = queryableX };
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
        protected ErrorLog UniqueErrorLog = new();
        partial void OnErrorLogGet(ErrorLog item);
        public async Task<ErrorLog> GetErrorLogDetailById(Guid? ErrorLogId)
        {
            try
            {

                UniqueErrorLog = await GetUniqueErrorLog(ErrorLogId);
                OnErrorLogGet(UniqueErrorLog);
                return await Task.FromResult(UniqueErrorLog);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<ErrorLog> GetUniqueErrorLog(Guid? ErrorLogId)
        {
            try
            {
                UniqueErrorLog = await new HttpClient().GetFromJsonAsync<ErrorLog>(_config.GetValue<string>("api_url") + "/AuditLog/GetErrorLogDetailById?ErrorLogId=" + ErrorLogId);
                return UniqueErrorLog;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion
        #region Import/Export
        public async Task ExportErrorLogToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/fatwadb/ErrorLogs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/fatwadb/ErrorLogs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }
        public async Task ExportErrorLogToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/fatwadb/ErrorLogs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/fatwadb/ErrorLogs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }
        #endregion

        #region Create Error Log
        public async Task<ApiCallResponse> CreateErrorLog(ErrorLog errorLog)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/AuditLog/CreateErrorLog");
                var postBody = errorLog;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
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
    }
}
