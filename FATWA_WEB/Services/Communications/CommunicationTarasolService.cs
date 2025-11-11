using Blazored.LocalStorage;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using static FATWA_GENERAL.Helper.Response;
using System.Net.Http.Headers;
using System.Net;
using System.Text;
using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;

namespace FATWA_WEB.Services.Communications
{
    public class CommunicationTarasolService
    {

        private readonly IConfiguration _config;
        private readonly ILocalStorageService _browserStorage;
        private static readonly HttpClient httpClient = new HttpClient();
        public CommunicationTarasolService(IConfiguration configuration, ILocalStorageService browserStorage)
        {
            _config = configuration;
            _browserStorage = browserStorage;
        }
        #region Send

        public async Task<ApiCallResponse> SendTarassolCommunication(SendCommunicationVM item)
        {
            try
            {
                //Communication
                item.Communication.CreatedBy = await _browserStorage.GetItemAsync<string>("User");
                item.Communication.CreatedDate = DateTime.Now;
                item.Communication.IsDeleted = false;

                //CommunicationTargetLink
                item.CommunicationTargetLink.CreatedBy = await _browserStorage.GetItemAsync<string>("User");
                item.CommunicationTargetLink.CreatedDate = DateTime.Now;
                item.CommunicationTargetLink.IsDeleted = false;

                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/CommunicationTarasolRPA/SendTarassolCommunication");
                var postBody = item;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await httpClient.SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<SendCommunicationVM>();

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

                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message } };
            }
        }
        public async Task<ApiCallResponse> SaveTarassolCommunication(CommunicationTarassolSendVM item)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("g2G_Tarassol_api_Url") + "/CommunicationTarasolRPA/SaveTarassolCommunication");
                var postBody = item;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await httpClient.SendAsync(request);
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

                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException.Message } };
            }
        }
        #endregion
        public async Task<ApiCallResponse> GetGovernmentEntitiyById(int? Id)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/CommunicationTarasolRPA/GetGovernmentEntitiyById?Id=" + Id);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var govtEntities = await response.Content.ReadFromJsonAsync<GovernmentEntity>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = govtEntities };
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
        public async Task<ApiCallResponse> GetGEDepartmentById(int? Id)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/CommunicationTarasolRPA/GetGEDepartmentById?Id=" + Id);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var govtEntities = await response.Content.ReadFromJsonAsync<GEDepartments>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = govtEntities };
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

        public async Task<ApiCallResponse> GetRPAFaultyMessages()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/CommunicationTarasolRPA/GetRPAFaultyMessages");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var payloadWithDocuments = await response.Content.ReadFromJsonAsync<TarasolRPAPayloadWithDocuments>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = payloadWithDocuments };
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