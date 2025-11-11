using Blazored.LocalStorage;
using FATWA_DOMAIN.Models;
using FATWA_ADMIN.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_ADMIN.Services
{
    //<History Author = 'Hassan Abbas' Date='2022-03-16' Version="1.0" Branch="master"> Lms Literature Classification Service for communicating with API Literature Classifications Controller</History>
    public partial class ProcessLogService
    {
        private readonly IConfiguration _config;
        private readonly NavigationManager navigationManager;
        private readonly ILocalStorageService browserStorage;
        private readonly TranslationState translationState;

        public ProcessLogService(IConfiguration configuration, NavigationManager _navigationManager, ILocalStorageService _browserStorage, TranslationState _translationState)
        {
            _config = configuration;
            navigationManager = _navigationManager;
            browserStorage = _browserStorage;
            translationState = _translationState;
        }
        protected ProcessLog UniqueProcessLog = new();
        partial void OnProcessLogGet(ProcessLog item);
        protected ProcessLog task = new();
        public async Task<ProcessLog> GetProcessLogDetailById(Guid? ProcessLogId)
        {
            try
            {

                UniqueProcessLog = await GetUniqueProcessLog(ProcessLogId);
                OnProcessLogGet(UniqueProcessLog);
                return await Task.FromResult(UniqueProcessLog);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task<ProcessLog> GetUniqueProcessLog(Guid? ProcessLogId)
        {
            try
            {
                UniqueProcessLog = await new HttpClient().GetFromJsonAsync<ProcessLog>(_config.GetValue<string>("api_url") + "/AuditLog/GetProcessLogDetailById?ProcessLogId=" + ProcessLogId);
                return UniqueProcessLog;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-03-16' Version="1.0" Branch="master"> Get List of Literature Classifications</History>
        public async Task<IQueryable<ProcessLog>> GetProcessLog(Query query = null)
        {
            var items = await GetProcessLog();

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

            OnProcessLogRead(ref items);

            return await Task.FromResult(items);
        }
        partial void OnProcessLogRead(ref IQueryable<ProcessLog> items);

        //<History Author = 'Hassan Abbas' Date='2022-03-16' Version="1.0" Branch="master"> Call API for getting List of Literature Classifications</History>
        private async Task<IQueryable<ProcessLog>> GetProcessLog()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/AuditLog/GetProcessLog");
            // add authorization header
            // request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            // send request
            var response = await new HttpClient().SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(translationState.Translate("Contact_Administrator"));
            }
            var responselist = response.Content.ReadFromJsonAsync<IEnumerable<ProcessLog>>();
            var queryableX = (await responselist).AsQueryable();
            return queryableX;
        }

        #region Import/Export

        public async Task ExportProcessLogToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/fatwadb/processlogs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/fatwadb/processlogs/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportProcessLogToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/fatwadb/processlogs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/fatwadb/processlogs/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }
        #endregion

        #region Create Process Log
        public async Task<ApiCallResponse> CreateProcessLog(ProcessLog processLog)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/AuditLog/CreateProcessLog");
                var postBody = processLog;
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
