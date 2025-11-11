using Blazored.LocalStorage;
using FATWA_DOMAIN.Models;
using DMS_WEB.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using System.Linq.Dynamic.Core;
using System.Net.Http.Headers;
using System.Text.Encodings.Web;

namespace DMS_WEB.Services
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
                UniqueProcessLog = await new HttpClient().GetFromJsonAsync<ProcessLog>(_config.GetValue<string>("fatwa_api_url") + "/AuditLog/GetProcessLogDetailById?ProcessLogId=" + ProcessLogId);
                return UniqueProcessLog;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-03-16' Version="1.0" Branch="master"> Get List of Literature Classifications</History>
        partial void OnProcessLogRead(ref IQueryable<ProcessLog> items);

        //<History Author = 'Hassan Abbas' Date='2022-03-16' Version="1.0" Branch="master"> Call API for getting List of Literature Classifications</History>
        private async Task<IQueryable<ProcessLog>> GetProcessLog()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("fatwa_api_url") + "/AuditLog/GetProcessLog");
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
    }
}
