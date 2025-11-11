using Blazored.LocalStorage;
using FATWA_ADMIN.Data;
using System.Net;
using System.Net.Http.Headers;
using Telerik.DataSource.Extensions;
using static FATWA_GENERAL.Helper.Response;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using System.Data.Entity;
using System.Linq.Dynamic.Core;
using Radzen;
using System.Text;
using FATWA_DOMAIN.Models.TimeInterval;
using FATWA_DOMAIN.Models.WorkerService;
using FATWA_DOMAIN.Models.ViewModel.TimeIntervalVMs;
using FATWA_DOMAIN.Models.ViewModel.WorkerServiceVMs;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.WorkflowModels;
using FATWA_ADMIN.Pages.TimeInterval;

namespace FATWA_ADMIN.Services.TimeInterval
{
    public class TimeIntervalService
    {
        #region Variables
        private readonly IConfiguration _config;
        private readonly ILocalStorageService browserStorage;
        private readonly TranslationState translationState;

        #endregion

        #region Constructor
        public TimeIntervalService(IConfiguration config, ILocalStorageService _browserStorage, TranslationState _translationState)
        {
            _config = config;
            browserStorage = _browserStorage;
            translationState = _translationState;
        }
        #endregion

        #region  GetTimeInttervals
        public async Task<ApiCallResponse> GetTimeIntervals()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/TimeInterval/GetTimeIntervals");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var list = await response.Content.ReadFromJsonAsync<List<TimeIntervalVM>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = list };
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
        #region Get cms coms Reminder By Id
        public async Task<ApiCallResponse> GetReminderIntervalById(int Id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/TimeInterval/GetReminderIntervalById?Id=" + Id);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var users = await response.Content.ReadFromJsonAsync<CmsComsReminder>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = users };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }

        }

        public async Task<ApiCallResponse> GetTimeIntervalHistoryList()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/TimeInterval/GetTimeIntervalHistoryList");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var list = await response.Content.ReadFromJsonAsync<List<TimeIntervalHistoryVM>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = list };
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

        public async Task<ApiCallResponse> UpdateIntervalStatus(bool isActive, int id)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/TimeInterval/UpdateIntervalStatus");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                request.Content = new FormUrlEncodedContent(new[]{
                        new KeyValuePair<string, string>("isActive", isActive.ToString()),
                        new KeyValuePair<string, string>("id", id.ToString()),
                    });
                var response = await new HttpClient().SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = true, ResultData = null };
                }
                else
                {
                    var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = false, ResultData = content };
                }
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }

        }

        #endregion



        #region Save TimeIntervals
        public async Task<ApiCallResponse> SaveCmsComsReminder(CmsComsReminder cmsComsReminder)
        {
            try
            {
                cmsComsReminder.CreatedBy = await browserStorage.GetItemAsync<string>("User");
                cmsComsReminder.CreatedDate = DateTime.Now;
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/TimeInterval/SaveCmsComsReminder");
                var postBody = cmsComsReminder;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    var content = false;
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<CmsComsReminder>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else
                {
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Update TimeIntervals
        public async Task<ApiCallResponse> UpdateCmsComsReminder(CmsComsReminder args)
        {
            try
            {
                args.CreatedBy = await browserStorage.GetItemAsync<string>("User");
                args.CreatedDate = DateTime.Now;
                args.WorkerServiceAppSettingPath = _config["WSAppSettingPath"];
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/TimeInterval/UpdateCmsComsReminder");
                var postBody = args;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                {
                    var content = false;
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<CmsComsReminder>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else
                {
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion
        #region Populate Reminder Types
        public async Task<ApiCallResponse> GetCmsComsReminderType()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/TimeInterval/GetCmsComsReminderType");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var sectorTypes = await response.Content.ReadFromJsonAsync<List<CmsComsReminderType>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = sectorTypes };
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

        #region Get Worker Service Execution Detail 
        public async Task<ApiCallResponse> GetWorkerServiceExecutionDetail(WSExecutionAdvanceSearchVM wSExecutionAdvanceSearchVM)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/TimeInterval/GetWorkerServiceExecutionDetail");
                var postBody = wSExecutionAdvanceSearchVM;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var wsExecutionDetail = await response.Content.ReadFromJsonAsync<List<WSExecutionDetailVM>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = wsExecutionDetail };
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

        public async Task<IEnumerable<WSExecutionStatus>> GetWSExecutionStatuses()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/TimeInterval/GetWSExecutionStatuses");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<List<WSExecutionStatus>>();
            else
                return new List<WSExecutionStatus>();
        }

        public async Task<List<WSWorkerServices>> GetWorkerServices()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/TimeInterval/GetWorkerServices");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<List<WSWorkerServices>>();
            else
                return new List<WSWorkerServices>();
        }
        #endregion

        #region Public Holidays
        public async Task<ApiCallResponse> AddPublicHolidays(PublicHoliday publicHoliday)
        {
            try
            {
                publicHoliday.CreatedBy = await browserStorage.GetItemAsync<string>("User");
                publicHoliday.CreatedDate = DateTime.Now;
                publicHoliday.IsActive = true;
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/TimeInterval/AddPublicHoliday");
                var postBody = publicHoliday;
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
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        public async Task<ApiCallResponse> UpdatePublicHoliday(PublicHoliday publicHoliday)
        {
            try
            {
                publicHoliday.ModifiedBy = await browserStorage.GetItemAsync<string>("User");
                publicHoliday.ModifiedDate = DateTime.Now;
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/TimeInterval/UpdatePublicHoliday");
                var postBody = publicHoliday;
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
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        public async Task<ApiCallResponse> DeletePublicHoliday(PublicHolidaysVM publicHoliday)
        {
            try
            {
                publicHoliday.DeletedBy = await browserStorage.GetItemAsync<string>("User");
                publicHoliday.DeletedDate = DateTime.Now;
                publicHoliday.IsDeleted = true;
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/TimeInterval/DeletePublicHoliday");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var postBody = publicHoliday;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
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
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }

        public async Task<ApiCallResponse> GetPublicHolidays()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/TimeInterval/GetPublicHolidays");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var publicHolidays = await response.Content.ReadFromJsonAsync<List<PublicHolidaysVM>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = publicHolidays };
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
        public async Task<ApiCallResponse> GetPublicHolidayById(int? Id)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/TimeInterval/GetPublicHolidayById?Id=" + Id);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var publicHoliday = await response.Content.ReadFromJsonAsync<PublicHoliday>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = publicHoliday };
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
