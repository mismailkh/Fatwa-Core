using Blazored.LocalStorage;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_WEB.Data;
using static FATWA_GENERAL.Helper.Response;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using Radzen;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using FATWA_DOMAIN.Models.ViewModel.TimeTrackingVM;
using FATWA_DOMAIN.Models.CaseManagment;
using Microsoft.Extensions.Azure;

namespace FATWA_WEB.Services.TimeTracking
{
    public partial class TimeTrackingService
    {
        #region Variables
        private readonly IConfiguration _config;
        private readonly ILocalStorageService browserStorage;
        private readonly TranslationState translationState;
        #endregion

        #region Constructor
        public TimeTrackingService(IConfiguration config, ILocalStorageService _browserStorage, TranslationState _translationState)
        {
            _config = config;
            browserStorage = _browserStorage;
            translationState = _translationState;
        }
        #endregion

        #region Time Tracking List

        public async Task<ApiCallResponse> GetTimeTracking(TimeTrackingAdvanceSearchVM advanceSearchVM, Query query = null)
        {
            try
            {
                List<TimeTrackingVM> timeTrackingVMs = new List<TimeTrackingVM>();
                var response = await GetTimeTracking(advanceSearchVM);
                if (response.IsSuccessStatusCode)
                {

                    var data = (IEnumerable<TimeTrackingVM>)response.ResultData;
                    foreach (var item in data)
                    {
                        item.ActivityName = translationState.Translate(item.ActivityName);
                        timeTrackingVMs.Add(item);
                    }
                    data = timeTrackingVMs;
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
                    OnTimeTrackingRead(ref items);
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
        partial void OnTimeTrackingRead(ref IQueryable<TimeTrackingVM> items);

        public async Task<ApiCallResponse> GetTimeTrackingList(TimeTrackingAdvanceSearchVM advanceSearchVM)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/TimeTracking/GetTimeTracking");
                var postBody = advanceSearchVM;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                { 
                    var content = await response.Content.ReadFromJsonAsync<IEnumerable<TimeTrackingVM>>();
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
