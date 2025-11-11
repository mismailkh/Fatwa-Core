using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Blazored.LocalStorage;
using FATWA_DOMAIN.Interfaces.Communication;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.MeetModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.Meet;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_GENERAL.Helper.Response;
namespace Fatwa_WEB.Services.Communications
{
    public class CommunicationService
    {
        private readonly IConfiguration _config;
        private readonly ILocalStorageService _browserStorage;
        private static readonly HttpClient httpClient = new HttpClient();
        public CommunicationService(IConfiguration configuration, ILocalStorageService browserStorage)
        {
            _config = configuration;
            _browserStorage = browserStorage;
        }


        #region Send

        public async Task<ApiCallResponse> SendCommunication(SendCommunicationVM item)
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

                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Communication/SendCommunication");
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

                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }

        }

        #endregion

        #region Get

        #region Get Send Response Detail by Id 
        public async Task<ApiCallResponse> CommunicationSendResponseDetailbyId(string communicationId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Communication/CommunicationSendResponseDetailbyId?communicationId=" + communicationId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<CommunicationSendResponseVM>();
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

        #region Get Correspondence History By Communication Id
        public async Task<ApiCallResponse> GetCorrespondenceHistoryByCommunicationId(Guid communicationId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Communication/GetCorrespondenceHistoryByCommunicationId?CommunicationId=" + communicationId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<CorrespondenceHistoryVM>>();
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

        #region Get Meeting Id By Communication id
        public async Task<ApiCallResponse> GetMeetingIdCommunitationbyId(string CommunicationId, int CommunicationTypeId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Communication/GetMeetingIdCommunitationbyId?CommunicationId=" + CommunicationId + "&CommunicationTypeId=" + CommunicationTypeId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<CommunicationMeetingDetailVM>();
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

        #region Get Send Message Detail by Id 
        public async Task<ApiCallResponse> CommunicationSendMessageDetailbyId(string communicationId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Communication/CommunicationSendMessageDetailbyId?communicationId=" + communicationId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<CommunicationSendMessageVM>();
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

        #region Get Meeting Detail By Using Communication Id
        public async Task<ApiCallResponse> GetMeetingDetailByUsingCommunicationId(Guid communicationId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Communication/GetMeetingDetailByUsingCommunicationId?communicationId=" + communicationId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<Meeting>();
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

        #region Get Communication Detail Communication Id
        public async Task<ApiCallResponse> GetCommunicationDetailCommunicationId(Guid communicationId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Communication/GetCommunicationDetailCommunicationId?communicationId=" + communicationId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<CommunicationVM>();
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

        #endregion

        #region Get Communication List 
        public async Task<ApiCallResponse> GetCommunicationListByCaseRequestId(Guid caseRequestId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Communication/GetCommunicationListByCaseRequestId?caseRequestId=" + caseRequestId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<CommunicationListVM>>();
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
        #region Get Communication Detail By Case Request ID, Case File ID & Case Id
        public async Task<ApiCallResponse> GetCommunicationDetailByCaseRequestId(Guid caseRequestId, Guid communicationId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Communication/GetCommunicationDetailByCaseRequestId?caseRequestId=" + caseRequestId + "&communicationId=" + communicationId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<CommunicationListVM>();
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
        public async Task<ApiCallResponse> GetCommunicationDetailByCaseFileId(Guid fileId, Guid communicationId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Communication/GetCommunicationDetailByCaseFileId?fileId=" + fileId + "&correspondenceTypeId=0" + "&communicationId=" + communicationId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<CommunicationListVM>();
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
        public async Task<ApiCallResponse> GetCommunicationDetailByCaseId(Guid caseId, Guid communicationId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Communication/GetCommunicationDetailByCaseId?caseId=" + caseId + "&CorrespondenceTypeId=0" + "&communicationId=" + communicationId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<CommunicationListVM>();
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

        #region Get Communication Detail By Consultation File ID & Consultation Request ID
        public async Task<ApiCallResponse> GetCommunicationDetailByConsultationFileId(Guid fileId, Guid communicationId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Communication/GetCommunicationDetailByConsultationFileId?fileId=" + fileId + "&correspondenceTypeId=0" + "&communicationId=" + communicationId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<CommunicationListVM>();
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
        public async Task<ApiCallResponse> GetCommunicationDetailByConsultationRequestId(Guid consultationRequestId, Guid communicationId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Communication/GetCommunicationDetailByConsultationRequestId?consultationRequestId=" + consultationRequestId + "&communicationId=" + communicationId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<CommunicationListVM>();
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
        public async Task<ApiCallResponse> GetCommunicationListByCaseFileId(Guid fileId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Communication/GetCommunicationListByCaseFileId?fileId=" + fileId + "&correspondenceTypeId=0");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<CommunicationListVM>>();
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
        
        public async Task<ApiCallResponse> GetCommunicationDetailByCommunicationId(Guid fileId, Guid communicationId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Communication/GetCommunicationDetailByCommunicationId?fileId=" + fileId + "&correspondenceTypeId=0" + "&communicationId=" + communicationId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<CommunicationListVM>();
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
        public async Task<ApiCallResponse> GetCommunicationListByCaseFileId(Guid fileId, int CorrespondenceTypeId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Communication/GetCommunicationListByCaseFileId?fileId=" + fileId + "&correspondenceTypeId=" + CorrespondenceTypeId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<CommunicationListVM>>();
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
        public async Task<ApiCallResponse> GetConslutationFileCommunication(Guid fileId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Communication/GetConslutationFileCommunication?fileId=" + fileId + "&CorrespondenceTypeId=0");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<CommunicationListVM>>();
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
        public async Task<ApiCallResponse> GetConslutationFileCommunication(Guid fileId, int CorrespondenceTypeId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Communication/GetConslutationFileCommunication?fileId=" + fileId + "&correspondenceTypeId=" + CorrespondenceTypeId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<CommunicationListVM>>();
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
        public async Task<ApiCallResponse> GetCommunicationListByCaseId(Guid caseId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Communication/GetCommunicationListByCaseId?caseId=" + caseId + "&CorrespondenceTypeId=0");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<CommunicationListVM>>();
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
        
        public async Task<ApiCallResponse> GetCommunicationListByCaseId(Guid caseId, int CorrespondenceTypeId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Communication/GetCommunicationListByCaseId?caseId=" + caseId + "&CorrespondenceTypeId=" + CorrespondenceTypeId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<CommunicationListVM>>();
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
        //<History Author = 'Hassan Abbas' Date='2022-02-01' Version="1.0" Branch="master"> Inbox Outbox List</History>
        public async Task<ApiCallResponse> GetInboxOutboxList(int correspondenceType, string userName,int PageSize, int PageNumber)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Communication/GetInboxOutboxList?correspondenceType=" + correspondenceType + "&userName=" + userName + "&PageSize=" + PageSize + "&PageNumber=" + PageNumber);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                return await response.Content.ReadFromJsonAsync<ApiCallResponse>();
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-02-10' Version="1.0" Branch="master"> Inbox Outbox List</History>
        public async Task<ApiCallResponse> GetInboxOutboxRequestNeedMoreDetail(Guid CommunicationId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Communication/GetInboxOutboxRequestNeedMoreDetail?CommunicationId=" + CommunicationId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<CmsCaseRequestResponseVM>();
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
                throw new Exception(ex.Message);
            }
        }
        #endregion

        public async Task<ApiCallResponse> GetCommunicationListByConsultationRequestId(Guid consultationRequestId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Communication/GetCommunicationListByConsultationRequestId?consultationRequestId=" + consultationRequestId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<CommunicationListVM>>();
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
        //<History Author = 'Hassan Iftikhar'  Version="1.0" Branch="master"> </History>
        public async Task<ApiCallResponse> SaveCommunicationResponse(CommunicationResponseMoreInfoVM communicationRequestMore)
        {
            try
            {
                //communication
                communicationRequestMore.Communication.CreatedBy = await _browserStorage.GetItemAsync<string>("User");
                communicationRequestMore.Communication.CreatedDate = DateTime.Now;
                communicationRequestMore.Communication.IsDeleted = false;

                //communicationResponse
                communicationRequestMore.CommunicationResponse.CreatedBy = await _browserStorage.GetItemAsync<string>("User");
                communicationRequestMore.CommunicationResponse.CreatedDate = DateTime.Now;
                communicationRequestMore.CommunicationResponse.IsDeleted = false;

                //CommunicationTargetLink
                communicationRequestMore.CommunicationTargetLink.CreatedBy = await _browserStorage.GetItemAsync<string>("User");
                communicationRequestMore.CommunicationTargetLink.CreatedDate = DateTime.Now;
                communicationRequestMore.CommunicationTargetLink.IsDeleted = false;
                communicationRequestMore.CommunicationTargetLink.CommunicationId = communicationRequestMore.Communication.CommunicationId;

                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Communication/SaveCommResponse");
                var postBody = communicationRequestMore;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
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
        public async Task<ApiCallResponse> CommunicationDetailbyComIdAndComType(Guid ReferenceId, Guid CommunicationId, int SubModuleId, int CommunicationTypeId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Communication/CommunicationDetailbyComIdAndComType?ReferenceId=" + ReferenceId + "&CommunicationId=" + CommunicationId + "&SubModuleId=" + SubModuleId + "&CommunicationTypeId=" + CommunicationTypeId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<CommunicationDetailVM>();
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
        public async Task<ApiCallResponse> StopExecutionRejectionReason(StopExecutionRejectionReason stopExecutionRejectionReason)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Communication/StopExecutionRejectionReason");
                var postBody = stopExecutionRejectionReason;
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
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
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiCallResponse> GetAnnouncementsListByCaseId(Guid caseId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Communication/GetGetAnnouncementsListByCaseId?caseId=" + caseId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<CmsAnnouncementVM>>();
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



        public async Task<ApiCallResponse> ForwardCorrespondenceToLawyer(CommunicationHistory communicationHistory)
        {
            try
            {
                communicationHistory.CreatedBy = await _browserStorage.GetItemAsync<string>("User");
                communicationHistory.CreatedDate = DateTime.Now;
               var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Communication/ForwardCorrespondenceToLawyer");
                var postBody = communicationHistory;
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
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
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiCallResponse> ForwardCorrespondenceToSector(CommunicationHistory communicationHistory)
        {
            try
            {
                communicationHistory.CreatedBy = await _browserStorage.GetItemAsync<string>("User");
                communicationHistory.CreatedDate = DateTime.Now;
               var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Communication/ForwardCorrespondenceToSector");
                var postBody = communicationHistory;
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
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
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiCallResponse> AssignBackToHos(CommunicationHistory communicationHistory)
        {
            try
            {
                communicationHistory.CreatedBy = await _browserStorage.GetItemAsync<string>("User");
                communicationHistory.CreatedDate = DateTime.Now;
               var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Communication/AssignBackToHos");
                var postBody = communicationHistory;
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
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
                throw new Exception(ex.Message);
            }
        }
        public async Task<ApiCallResponse> SendBackToSender(CommunicationHistory communicationHistory)
        {
            try
            {
                communicationHistory.CreatedBy = await _browserStorage.GetItemAsync<string>("User");
                communicationHistory.CreatedDate = DateTime.Now;
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Communication/SendBackToSender");
                var postBody = communicationHistory;
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
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
                throw new Exception(ex.Message);
            }
        }


    }
}
