using Blazored.LocalStorage;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_WEB.Data;
using System.Net;
using System.Net.Http.Headers;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Services
{
    public partial class DashboardService
    {
        private readonly IConfiguration _config;
        private readonly TranslationState _translationState;
        private readonly ILocalStorageService _browserStorage;


        public DashboardService(IConfiguration configuration, TranslationState translationState, ILocalStorageService browserStorage)
        {
            _config = configuration;
            _translationState = translationState;
            _browserStorage = browserStorage; 
        }

        public async Task<ApiCallResponse> GetDashboardDetails()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Dashboard/GetDashboardDetails");
                // add authorization header
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                // send request
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(_translationState.Translate("Contact_Administrator"));
                }
                else if (response.IsSuccessStatusCode)
                {
                    var responselist = response.Content.ReadFromJsonAsync<DashboardVM>();
                    var queryableX = await responselist;
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
    }
}
