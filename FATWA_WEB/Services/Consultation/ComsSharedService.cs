using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using static FATWA_GENERAL.Helper.Response;
using System.Net.Http.Headers;
using System.Net;
using Blazored.LocalStorage;
using FATWA_WEB.Data;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.BaseModels;
using System.Text;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using Radzen;
using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.EntityFrameworkCore;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;

namespace FATWA_WEB.Services.Consultation
{
    //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Shared service for Case Management with API</History>
    public class ComsSharedService
    {
        #region variables declaration
        private readonly IConfiguration _config;
        private readonly ILocalStorageService browserStorage;
        private readonly TranslationState translationState;

        public string ResultDetails { get; private set; }

        #endregion

        #region Constructor
        public ComsSharedService(IConfiguration config, ILocalStorageService _browserStorage, TranslationState _translationState)
        {
            _config = config;
            browserStorage = _browserStorage;
            translationState = _translationState;
        }
        #endregion

        #region Transfer Sector

        //<History Author = 'Muhammad Zaeem' Date='2023-1-9' Version="1.0" Branch="master">Add Transfer Coms Sector Task</History>
        public async Task<ApiCallResponse> AddTransferComsSectorTask(CmsApprovalTracking Item, int TransferConsultationType, int SectorTypeId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/ComsShared/AddTransferComsSectorTask?TransferConsultationType=" + TransferConsultationType + "&SectorTypeId=" + SectorTypeId);
                var postBody = Item;
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



        #endregion

        #region SubmitAssignToLawyer
        public async Task<ApiCallResponse> AssignConsultationRequestToLawyer(ConsultationAssignment item, string userName)
        {
            try
            {
                item.CreatedBy = await browserStorage.GetItemAsync<string>("User");
                item.CreatedDate = DateTime.Now;
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/ComsShared/AssignConsultationToLawyer?userName=" + userName);
                var postBody = item;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    var content = new ConsultationAssignment();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<ConsultationAssignment>();
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

        #region Get Approval Tracking
        //<History Author = 'Muhammad Zaeem' Date='2022-1-18' Version="1.0" Branch="master">Get Approval Tracking Process</History>
        public async Task<ApiCallResponse> GetApprovalTrackingProcess(Guid referenceId, int sectorTypeId, int processTypeId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/ComsShared/GetApprovalTrackingProcess?referenceId=" + referenceId + "&sectorTypeId=" + sectorTypeId + "&processTypeId=" + processTypeId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<CmsApprovalTracking>();
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

        #region Add Send Copy Consultation Task
        public async Task<ApiCallResponse> AddSendACopyConsultationTask(ComsApprovalTracking Item, int TransferConsultationType, int SectorTypeId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/ComsShared/AddSendACopyConsultationTask?TransferConsultationType=" + TransferConsultationType + "&SectorTypeId=" + SectorTypeId);
                var postBody = Item;
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
        #endregion

        #region Approve/Reject Send Copy Consultation
        public async Task<ApiCallResponse> ApproveSendACopyConsultation(dynamic Item, int TransferConsultationType)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/ComsShared/ApproveSendACopyConsultation?TransferConsultationType=" + TransferConsultationType);
                var postBody = Item;
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


        public async Task<ApiCallResponse> RejectSendACopyConsultation(dynamic Item, int TransferConsultationType)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/ComsShared/RejectSendACopyConsultation?TransferConsultationType=" + TransferConsultationType);
                var postBody = Item;
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
        #endregion

        #region Approve Consultation File
        public async Task<ApiCallResponse> ApproveConsultationFile(ConsultationFileDetailVM item)
        {
            try
            {

                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/ComsShared/ApproveConsultationFile");
                var postBody = item;
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
        #endregion

        #region Get Consultation Assignment by Lawyer Id and File Id
        public async Task<ApiCallResponse> GetConsultationAssigmentByLawyerIdAndFileId(Guid FileId, string UserId)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/ComsShared/GetConsultationAssigmentByLawyerIdAndFileId?FileId=" + FileId + "&UserId=" + UserId);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<ConsultationAssignment>();
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

        #region Assign Consultation Back to Hos
        public async Task<ApiCallResponse> AssignConsultationBackToHos(CmsAssignCaseFileBackToHos Item)
        {
            try
            {
                Item.CreatedBy = await browserStorage.GetItemAsync<string>("User");
                Item.CreatedDate = DateTime.Now;
                Item.IsDeleted = false;
                var userDetail = await browserStorage.GetItemAsync<UserDetailVM>("UserDetail");
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/ComsShared/AssignConsultationBackToHos?SectorTypeId=" + userDetail.SectorTypeId);
                var postBody = Item;
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
        #endregion

        #region Get Send back to hos By reference Id
        public async Task<ApiCallResponse> GetSendBackToHosByReferenceId(Guid ReferenceId, string LawyerId)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/ComsShared/GetSendBackToHosByReferenceId?ReferenceId=" + ReferenceId + "&LawyerId=" + LawyerId);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<CmsAssignCaseFileBackToHos>();
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
