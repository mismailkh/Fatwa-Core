using Blazored.LocalStorage;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using DMS_WEB.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore;
using Radzen;
using System.Linq.Dynamic.Core;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;

namespace DMS_WEB.Services
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

        private async Task<IQueryable<ErrorLogVM>> GetErrorLogByProc()
        {
            try
            {

                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("fatwa_api_url") + "/AuditLog/GetErrorLogThroughProc");
                // add authorization header
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                // send request
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(translationState.Translate("Contact_Administrator"));
                }
                var responselist = response.Content.ReadFromJsonAsync<IEnumerable<ErrorLogVM>>();
                var queryableX = (await responselist).AsQueryable();
                return queryableX;

            }
            catch (Exception)
            {

                throw;
            }
        }
        partial void OnProcessLogReadProc(ref IQueryable<ProcessLogVM> items);

        private async Task<IQueryable<ProcessLogVM>> GetProcessLogByProc()
        {
            try
            {

                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("fatwa_api_url") + "/AuditLog/GetProcessLogThroughProc");
                // add authorization header
               request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                // send request
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(translationState.Translate("Contact_Administrator"));
                }
                var responselist = response.Content.ReadFromJsonAsync<IEnumerable<ProcessLogVM>>();
                var queryableX = (await responselist).AsQueryable();
                return queryableX;

            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        public async Task<IQueryable<ErrorLogVM>> GetErrorLogAdvanceSearch(ErrorLogAdvanceSearchVM searchErrorLog)
        {
            try
            {

                var items = await GetErrorLogAdvanceSearchs(searchErrorLog);

                OnErrorLogAdvanceSearchRead(ref items);

                return await Task.FromResult(items);

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }


        private async Task<IQueryable<ErrorLogVM>> GetErrorLogAdvanceSearchs(ErrorLogAdvanceSearchVM searchErrorLog)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("fatwa_api_url") + "/AuditLog/GetErrorLogAdvanceSearch");
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
                var responselist = response.Content.ReadFromJsonAsync<IEnumerable<ErrorLogVM>>();
                var queryableX = (await responselist).AsQueryable();
                return queryableX;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        partial void OnErrorLogAdvanceSearchRead(ref IQueryable<ErrorLogVM> items);
        public async Task<IQueryable<ProcessLogVM>> GetProcessLogAdvanceSearch(ProcessLogAdvanceSearchVM processLogAdvanceSearchVM)
        {
            try
            {

                var items = await GetProcessLogAdvanceSearchs(processLogAdvanceSearchVM);

                OnProcessLogRead(ref items);

                return await Task.FromResult(items);

            }
            catch (Exception ex) { throw new Exception(ex.Message); }
        }


        private async Task<IQueryable<ProcessLogVM>> GetProcessLogAdvanceSearchs(ProcessLogAdvanceSearchVM processLogAdvanceSearchVM)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("fatwa_api_url") + "/AuditLog/GetProcessLogAdvanceSearch");
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
                var responselist = response.Content.ReadFromJsonAsync<IEnumerable<ProcessLogVM>>();
                var queryableX = (await responselist).AsQueryable();
                return queryableX;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        partial void OnProcessLogRead(ref IQueryable<ProcessLogVM> items);


        #region Get Error Log By linq query 
        partial void OnErrorLogRead(ref IQueryable<ErrorLog> items);

        private async Task<IQueryable<ErrorLog>> GetErrorLog()
        {
            try
            {

                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("fatwa_api_url") + "/AuditLog/GetErrorLog");
                // add authorization header
                //  request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                // send request
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(translationState.Translate("Contact_Administrator"));
                }
                var responselist = response.Content.ReadFromJsonAsync<IEnumerable<ErrorLog>>();
                var queryableX = (await responselist).AsQueryable();
                return queryableX;

            }
            catch (Exception)
            {

                throw;
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
                UniqueErrorLog = await new HttpClient().GetFromJsonAsync<ErrorLog>(_config.GetValue<string>("fatwa_api_url") + "/AuditLog/GetErrorLogDetailById?ErrorLogId=" + ErrorLogId);
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
    }
}
