using FATWA_DOMAIN.Models.ServiceRequestModels;
using static FATWA_GENERAL.Helper.Response;
using static Org.BouncyCastle.Math.EC.ECCurve;
using System.Net.Http.Headers;
using System.Net;
using Blazored.LocalStorage;

namespace FATWA_ADMIN.Services.ServiceRequest
{
    public class ServiceRequestService
    {
        #region variables declaration
        private readonly IConfiguration _config;
        private readonly ILocalStorageService _browserStorage;

        #endregion

        #region Ctor

        public ServiceRequestService(IConfiguration config, ILocalStorageService browserStorage)
        {
            _config = config;
            _browserStorage = browserStorage;
        }

        #endregion

        public async Task<ApiCallResponse> GetServiceRequestTypes()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("fatwa_oss_api_url") + "/ServiceRequest/GetServiceRequestTypes");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var status = await response.Content.ReadFromJsonAsync<List<ServiceRequestType>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = status };
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
    }
}
