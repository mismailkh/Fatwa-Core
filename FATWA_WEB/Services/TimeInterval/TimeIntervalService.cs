using Blazored.LocalStorage;
using FATWA_DOMAIN.Models.TimeInterval;
using FATWA_DOMAIN.Models.ViewModel.TimeIntervalVMs;
using FATWA_DOMAIN.Models.ViewModel.WorkerServiceVMs;
using FATWA_DOMAIN.Models.WorkerService;
using FATWA_WEB.Data;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Services.TimeInterval
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


        #region Public Holidays
     
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
        #endregion
    }
}
