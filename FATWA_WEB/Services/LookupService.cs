using Blazored.LocalStorage;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.Contact;
using FATWA_DOMAIN.Models.Dms;
using FATWA_DOMAIN.Models.Lms;
using FATWA_DOMAIN.Models.MeetModels;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.OrganizingCommittee;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.ContactManagmentVMs;
using FATWA_DOMAIN.Models.ViewModel.Meet;
using FATWA_DOMAIN.Models.WorkflowModels;
using FATWA_WEB.Data;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using static FATWA_GENERAL.Helper.Response;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.BugReporting;
using Group = FATWA_DOMAIN.Models.AdminModels.UserManagement.Group;

namespace FATWA_WEB.Services
{
    //<History Author = 'Hassan Abbas' Date='2022-09-06' Version="1.0" Branch="master"> Service to handle lookup requests</History>
    public class LookupService
    {
        private readonly IConfiguration _config;
        private readonly ILocalStorageService browserStorage;
        private readonly TranslationState translationState;

        public LookupService(IConfiguration config, ILocalStorageService _browserStorage, TranslationState _translationState)
        {
            _config = config;
            browserStorage = _browserStorage;
            translationState = _translationState;
        }


        #region Dropdown Data Events

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Request types</History>
        public async Task<ApiCallResponse> GetRequestTypes()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetRequestTypes");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var sectorTypes = await response.Content.ReadFromJsonAsync<List<RequestType>>();
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

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Operating Sector types</History>
        public async Task<ApiCallResponse> GetOperatingSectorTypes()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetOperatingSectorTypes");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var sectorTypes = await response.Content.ReadFromJsonAsync<List<OperatingSectorType>>();
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

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Sector Subtypes</History>
        public async Task<ApiCallResponse> GetAllRequestSubtypes()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetAllRequestSubtypes");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var subtypes = await response.Content.ReadFromJsonAsync<List<Subtype>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = subtypes };
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

        public async Task<ApiCallResponse> GetPreCourtTypes()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetPreCourtTypes");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var courtTypes = await response.Content.ReadFromJsonAsync<List<PreCourtType>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = courtTypes };
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
        public async Task<ApiCallResponse> GetCourtTypesByRequestType(int requestTypeId)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetCourtTypesByRequestType?requestTypeId=" + requestTypeId);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var courtTypes = await response.Content.ReadFromJsonAsync<List<CourtType>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = courtTypes };
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

        //<History Author = 'Hassan Abbas' Date='2022-04-23' Version="1.0" Branch="master">Remote Type Data for Add Attachment Drop Down</History>
        public async Task<ApiCallResponse> GetAttachmentTypes(int? ModuleId, bool ShowHidden = false)
        {
            try
            {
                var userDetail = await browserStorage.GetItemAsync<UserDetailVM>("UserDetail");
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("dms_api_url") + "/FileUpload/GetAttachmentTypes?ModuleId=" + ModuleId + "&SectorTypeId=" + userDetail.SectorTypeId + "&ShowHidden=" + ShowHidden);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var attachTypes = await response.Content.ReadFromJsonAsync<List<AttachmentType>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = attachTypes };
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

        //<History Author = 'Hassan Abbas' Date='2022-04-23' Version="1.0" Branch="master">Remote Type Data for Add Attachment Drop Down</History>
        public async Task<ApiCallResponse> GetCaseTemplates(int AttachmentTypeId)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetCaseTemplates?attachmentTypeId=" + AttachmentTypeId);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var templates = await response.Content.ReadFromJsonAsync<List<CaseTemplate>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = templates };
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

        //<History Author = 'Hassan Abbas' Date='2022-12-05' Version="1.0" Branch="master">Get Hearing Statuses</History>
        public async Task<ApiCallResponse> GetCaseHearingStatuses()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetCaseHearingStatuses");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var templates = await response.Content.ReadFromJsonAsync<List<HearingStatus>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = templates };
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

        //<History Author = 'Hassan Abbas' Date='2022-12-05' Version="1.0" Branch="master">Get Hearing Statuses</History>
        public async Task<ApiCallResponse> GetCaseJudgementTypes()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetCaseJudgementTypes");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var templates = await response.Content.ReadFromJsonAsync<List<JudgementType>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = templates };
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

        //<History Author = 'Hassan Abbas' Date='2023-11-28' Version="1.0" Branch="master">Get Judgement Categories</History>
        public async Task<ApiCallResponse> GetCaseJudgementCategories()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetCaseJudgementCategories");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var templates = await response.Content.ReadFromJsonAsync<List<JudgementCategory>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = templates };
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

        //<History Author = 'Hassan Abbas' Date='2023-11-28' Version="1.0" Branch="master">Get Judgement Statuses</History>
        public async Task<ApiCallResponse> GetCaseJudgementStatuses()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetCaseJudgementStatuses");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var templates = await response.Content.ReadFromJsonAsync<List<JudgementStatus>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = templates };
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

        //<History Author = 'Hassan Abbas' Date='2023-11-28' Version="1.0" Branch="master">Get Execution File Levels</History>
        public async Task<ApiCallResponse> GetExecutionFileLevels()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetExecutionFileLevels");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var templates = await response.Content.ReadFromJsonAsync<List<ExecutionFileLevel>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = templates };
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

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Case Priorities</History>
        public async Task<ApiCallResponse> GetCasePriorities()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetCasePriorities");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var priorities = await response.Content.ReadFromJsonAsync<List<Priority>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = priorities };
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

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Case Request Statuses</History>
        public async Task<ApiCallResponse> GetCaseRequestStatuses()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetCaseRequestStatuses");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var statuses = await response.Content.ReadFromJsonAsync<List<CaseRequestStatus>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = statuses };
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

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Govt entities</History>
        public async Task<ApiCallResponse> GetGovernmentEntities()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetGovernmentEntities?Culture=" + Thread.CurrentThread.CurrentUICulture.Name);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var govtEntities = await response.Content.ReadFromJsonAsync<List<GovernmentEntity>>();
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

        //<History Author = 'Hassan Abbas' Date='2022-02-27' Version="1.0" Branch="master">Get Ge Representatives</History>
        public async Task<ApiCallResponse> GetGeRepresentatives(int? govtEntitiyId)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetGeRepresentatives?govtEntitiyId=" + govtEntitiyId);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var geRepresentatives = await response.Content.ReadFromJsonAsync<List<GovernmentEntityRepresentative>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = geRepresentatives };
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

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Govt entities</History>
        public async Task<ApiCallResponse> GetDepartments()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetDepartments");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var departments = await response.Content.ReadFromJsonAsync<List<Department>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = departments };
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

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Case Request Statuses</History>
        public async Task<ApiCallResponse> GetCaseFileStatuses()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetCaseFileStatuses");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var statuses = await response.Content.ReadFromJsonAsync<List<CaseFileStatus>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = statuses };
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

        //<History Author = 'Zain Ul Islam' Date='2022-11-10' Version="1.0" Branch="master">Meeting Types</History>
        public async Task<ApiCallResponse> GetFileNumbers()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetFileNumbers");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<CaseFile>>();
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

        //<History Author = 'Nadia Gull' Date='2023-01-27' Version="1.0" Branch="master">Get Reference Number</History>
        public async Task<ApiCallResponse> GetReferenceNumber(Guid ReferenceId, int SubModulId)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetReferenceNumber?ReferenceId=" + ReferenceId + "&SubModulId=" + SubModulId);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
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
        public async Task<ApiCallResponse> GetConsultationReferenceNumber(Guid ReferenceId)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetConsultationReferenceNumber?ReferenceId=" + ReferenceId);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
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
        public async Task<ApiCallResponse> GetConsultationFileNumber()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetConsultationFileNumber");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<ConsultationFile>>();
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
        public async Task<ApiCallResponse> GetResponseTypes()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetResponseTypes");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var responses = await response.Content.ReadFromJsonAsync<List<ResponseType>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = responses };
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
        public async Task<ApiCallResponse> GetFrequency()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetFrequency");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var responses = await response.Content.ReadFromJsonAsync<List<Frequency>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = responses };
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

        //<History Author = 'Zain Ul Islam' Date='2022-11-10' Version="1.0" Branch="master">Meeting Types</History>
        public async Task<ApiCallResponse> GetMeetingTypes()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetMeetingTypes");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var meetingTypes = await response.Content.ReadFromJsonAsync<List<MeetingType>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = meetingTypes };
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

        #region Get users
        //<History Author = 'Muhammad Zaeem' Date='2022-11-1' Version="1.0" Branch="master">Get Users(Lawyer list) </History>

        public async Task<ApiCallResponse> GetUsers()
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Account/GetUsersList");
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);

            if (response.IsSuccessStatusCode)
            {
                var users = await response.Content.ReadFromJsonAsync<List<UserVM>>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = users };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }

        }

        #endregion

        #region Get users by sector
        //<History Author = 'Hassan Abbas' Date='2022-12-21' Version="1.0" Branch="master">Get Users by Sector </History>

        public async Task<ApiCallResponse> GetUsersBySector(int? sectorTypeId)
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LookUps/GetUsersBySector?sectorTypeId=" + sectorTypeId);
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);

            if (response.IsSuccessStatusCode)
            {
                var users = await response.Content.ReadFromJsonAsync<List<UserVM>>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = users };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }

        }

        #endregion

        #region Get users by sector for Court Assignment

        //<History Author = 'Hassan Abbas' Date='2024-02-07' Version="1.0" Branch="master">Get Users by Sector For Court Assignment </History>
        public async Task<ApiCallResponse> GetUsersBySectorForCourtAssignment(int? sectorTypeId)
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LookUps/GetUsersBySectorForCourtAssignment?sectorTypeId=" + sectorTypeId);
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);

            if (response.IsSuccessStatusCode)
            {
                var users = await response.Content.ReadFromJsonAsync<List<UserVM>>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = users };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }

        }

        #endregion

        #region GetLawyersBySector

        //<History Author = 'Zain Ul Islam' Date='2023-01-16' Version="1.0" Branch="master">Get Lawyers List By Sector</History>
        //<History Author = 'Hassan Abbas' Date='2023-03-07' Version="1.0" Branch="master">Modified to return only those who have role lawyer</History>    
        public async Task<ApiCallResponse> GetLawyersBySector(int? sectorTypeId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LookUps/GetLawyersBySector?sectorTypeId=" + sectorTypeId);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var users = await response.Content.ReadFromJsonAsync<List<LawyerVM>>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = users };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }

        }

        #endregion

        #region Get Lawyers by Sector And Chamber

        //<History Author = 'Hassan Abbas' Date='2023-12-04' Version="1.0" Branch="master">List of Lawyers based on Sector And Chamber</History>    
        public async Task<ApiCallResponse> GetLawyersBySectorAndChamber(int? sectorTypeId, int chamberNumberId = 0)
        {
            var userDetail = await browserStorage.GetItemAsync<UserDetailVM>("UserDetail");
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LookUps/GetLawyersBySectorAndChamber?sectorTypeId=" + sectorTypeId + "&UserId=" + userDetail.UserId + "&chamberNumberId=" + chamberNumberId);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var users = await response.Content.ReadFromJsonAsync<List<LawyerVM>>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = users };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }

        }

        #endregion

        #region Get Supervisors By Sector

        //<History Author = 'Hassan Abbas' Date='2023-03-07' Version="1.0" Branch="master">Get Supervisors List By Sector</History>    
        public async Task<ApiCallResponse> GetSupervisorsBySector(int? sectorTypeId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LookUps/GetSupervisorsBySector?sectorTypeId=" + sectorTypeId);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var users = await response.Content.ReadFromJsonAsync<List<LawyerVM>>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = users };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }

        }

        #endregion

        #region get court Type
        //<History Author = 'Muhammad Zaeem' Date='2022-11-11' Version="1.0" Branch="master">Get court types</History>
        public async Task<ApiCallResponse> GetCourtType()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetCourtType");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var types = await response.Content.ReadFromJsonAsync<List<CourtType>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = types };
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

        #region get court
        //<History Author = 'Muhammad Zaeem' Date='2022-11-11' Version="1.0" Branch="master">Get court</History>
        public async Task<ApiCallResponse> GetCourt()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetCourt");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var types = await response.Content.ReadFromJsonAsync<List<Court>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = types };
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
        #region Get Court By UserID
        public async Task<ApiCallResponse> GetCourtByUserId(string UserId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetCourtById?UserId=" + UserId);
                var apiKey = _config.GetValue<string>("FatwaApiKey");
                request.Headers.Add("FatwaApiKey", apiKey);
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var types = await response.Content.ReadFromJsonAsync<List<Court>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = types };
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

        #region get chamber
        //<History Author = 'Muhammad Zaeem' Date='2022-11-11' Version="1.0" Branch="master">Get Chamber</History>
        public async Task<ApiCallResponse> GetChamber()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetChamber");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var types = await response.Content.ReadFromJsonAsync<List<Chamber>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = types };
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

        #region Get Chamber Number
        public async Task<ApiCallResponse> GetChamberNumbersByChamberId(int chamberId)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetChamberNumbersByChamberId?chamberId=" + chamberId);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var chamberNumbers = await response.Content.ReadFromJsonAsync<List<ChamberNumber>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = chamberNumbers };
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

        public async Task<ApiCallResponse> GetChamberNumber()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetChamberNumber");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var chamberNumbers = await response.Content.ReadFromJsonAsync<List<ChamberNumber>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = chamberNumbers };
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

        #region SubmitLawyerToCourtAssignment
        public async Task<ApiCallResponse> SubmitLawyerToCourt(CmsAssignLawyerToCourt item)
        {
            try
            {
                item.CreatedBy = await browserStorage.GetItemAsync<string>("User");
                item.CreatedDate = DateTime.Now;
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Lookups/SubmitLawyerToCourt");
                var postBody = item;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<CmsAssignLawyerToCourt>();
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

        #region GET TASK TYPE
        public async Task<ApiCallResponse> GetTaskType()
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LookUps/GetTaskType");
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);

            if (response.IsSuccessStatusCode)
            {
                var types = await response.Content.ReadFromJsonAsync<List<TaskType>>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = types };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }

        }
        #endregion

        #region GET TASK SUB TYPE
        public async Task<ApiCallResponse> GetTaskSubType()
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetTaskSubType");
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);

            if (response.IsSuccessStatusCode)
            {
                var subTypes = await response.Content.ReadFromJsonAsync<List<TaskSubType>>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = subTypes };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }

        }
        #endregion

        #region GET File Number

        public async Task<ApiCallResponse> GetFileNumber()
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LookUps/GetFileNumbers");
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);

            if (response.IsSuccessStatusCode)
            {
                var fileNumbers = await response.Content.ReadFromJsonAsync<List<CaseFile>>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = fileNumbers };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }

        }

        #endregion

        #region GET Request Number

        public async Task<ApiCallResponse> GetRequestNumber()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LookUps/GetRequestNumber");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var fileNumbers = await response.Content.ReadFromJsonAsync<List<CaseRequest>>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = fileNumbers };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }

        }

        #endregion

        #region Get Task Statuses

        //<History Author = 'Zain Ul Islam' Date='2022-12-1' Version="1.0" Branch="master">Get Modules List</History>

        public async Task<ApiCallResponse> GetTaskStatuses()
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Account/GetTaskStatuses");
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);

            if (response.IsSuccessStatusCode)
            {
                var users = await response.Content.ReadFromJsonAsync<List<UserTaskStatus>>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = users };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }

        }

        #endregion

        #region Get Roles

        //<History Author = 'Zain Ul Islam' Date='2022-12-13' Version="1.0" Branch="master">Get Roles</History>

        public async Task<ApiCallResponse> GetRoles()
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LookUps/GetRoles");
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);

            if (response.IsSuccessStatusCode)
            {
                var users = await response.Content.ReadFromJsonAsync<List<Role>>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = users };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }

        }

        #endregion

        #region Get Court Visit Type 

        //<History Author = 'Nabeel ur Rehaman' Date='2022-12-13' Version="1.0" Branch="master">Get Roles</History>

        public async Task<ApiCallResponse> GetCourtVisitTypes()
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetCourtVisitTypes");
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);

            if (response.IsSuccessStatusCode)
            {
                var users = await response.Content.ReadFromJsonAsync<List<CmsCaseVisitType>>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = users };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }

        }

        #endregion


        //<History Author = ijaz Ahmad' Date='2023-01-19' Version="1.0" Branch="master">Get Assinged Lawyer To Court</History>
        public async Task<ApiCallResponse> GetAssingedLawyerToCourt(AdvanceSearchVMAssignLawyerToCourt advanceSearchVM)
        {
            try
            {
                var User = await browserStorage.GetItemAsync<UserDetailVM>("UserDetail");
                advanceSearchVM.SectorTypeId = User.SectorTypeId;
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetAssignLawyerToCourt");
                var postBody = advanceSearchVM;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var templates = await response.Content.ReadFromJsonAsync<List<AssignLawyerToCourtVM>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = templates };
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


        public async Task<ApiCallResponse> GetUserIdByUserEmail(string email)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetUserIdByUserEmail?email=" + email);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var userId = await response.Content.ReadAsStringAsync();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = userId };
                }
                else
                {
                    var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<ApiCallResponse> GetAssignLawyertoCourtById(Guid Id)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetAssignLawyertoCourtById?Id=" + Id);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<CmsAssignLawyerToCourt>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else
                {
                    var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<ApiCallResponse> EditAssignLawyertoCourt(CmsAssignLawyerToCourt item)
        {
            try
            {
                item.ModifiedBy = await browserStorage.GetItemAsync<string>("User");
                item.ModifiedDate = DateTime.Now;
                item.IsDeleted = false;

                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Lookups/EditAssignLawyertoCourt");
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
                throw new Exception(ex.Message);
            }
        }
        #region Delete Assign Lawyer to Court
        public async Task<ApiCallResponse> DeleteAssignLawyerToCourt(AssignLawyerToCourtVM item)
        {
            try
            {
                item.ModifiedBy = await browserStorage.GetItemAsync<string>("User");
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Lookups/DeleteAssignLawyerToCourt");
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
                throw new Exception(ex.Message);
            }

        }
        #endregion

        #region GET Supervisor By Lawyer
        //<History Author = 'Hassan Abbas' Date='2023-03-01' Version="1.0" Branch="master">Get Supervisor By LaywerId</History>
        public async Task<ApiCallResponse> GetSupervisorByLawyerId(string lawyerId)
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LookUps/GetSupervisorByLawyerId?lawyerId=" + lawyerId);
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);

            if (response.IsSuccessStatusCode)
            {
                var supervisorId = await response.Content.ReadAsStringAsync();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = supervisorId };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }

        }

        #endregion

        #region Get consultation Party type
        public async Task<ApiCallResponse> GetConsultationPartyTypes()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetConsultationPartyTypes");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<ConsultationPartyType>>();
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

        #region GET Consultation Request Number

        public async Task<ApiCallResponse> GetConsultationRequestNumber()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LookUps/GetConsultationRequestNumber");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var fileNumbers = await response.Content.ReadFromJsonAsync<List<ConsultationRequest>>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = fileNumbers };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }

        }

        #endregion

        #region Contact Managment
        public async Task<ApiCallResponse> GetContactJobRole()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetRoles");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var statuses = await response.Content.ReadFromJsonAsync<List<Role>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = statuses };
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


        public async Task<ApiCallResponse> GetMeetingStatus()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetMeetingStatus");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var meetingTypes = await response.Content.ReadFromJsonAsync<List<MeetingStatus>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = meetingTypes };
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
        public async Task<ApiCallResponse> GetContactType()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetContactType");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var statuses = await response.Content.ReadFromJsonAsync<List<CntContactType>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = statuses };
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
        public async Task<ApiCallResponse> GetReferenceNumberBySubmoduleId(int SubModuleId, int sectorId)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetReferenceNumberBySubmoduleId?SubModuleId=" + SubModuleId + "&sectorId=" + sectorId);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var statuses = await response.Content.ReadFromJsonAsync<List<ReferenceNumberVM>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = statuses };
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

        public async Task<ApiCallResponse> GetModules()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetModules");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var statuses = await response.Content.ReadFromJsonAsync<List<Module>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = statuses };
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

        public async Task<ApiCallResponse> GetEvents()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetEvents");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var statuses = await response.Content.ReadFromJsonAsync<List<NotificationEvent>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = statuses };
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
        public async Task<ApiCallResponse> GetSubmodule()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetSubmodule");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var statuses = await response.Content.ReadFromJsonAsync<List<SubModule>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = statuses };
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

        #region Get Document Classifications

        //<History Author = 'Hassan Abbas' Date='2023-06-15' Version="1.0" Branch="master">Get Document Classifications</History>

        public async Task<ApiCallResponse> GetDocumentClassifications()
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("dms_api_url") + "/FileUpload/GetDocumentClassifications");
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);

            if (response.IsSuccessStatusCode)
            {
                var users = await response.Content.ReadFromJsonAsync<List<DmsDocumentClassification>>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = users };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }

        }

        #endregion

        #region Get User by Department
        public async Task<ApiCallResponse> GetUsersByDepartment(int DepartmentId)
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/LookUps/GetUsersByDepartment?DepartmentId=" + DepartmentId);
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);

            if (response.IsSuccessStatusCode)
            {
                var users = await response.Content.ReadFromJsonAsync<List<UserVM>>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = users };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }

        }
        #endregion

        #region Company List
        public async Task<ApiCallResponse> GetCompanyList()
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetCompanyList");
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);

            if (response.IsSuccessStatusCode)
            {
                var users = await response.Content.ReadFromJsonAsync<List<Company>>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = users };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }

        }
        #endregion

        #region City List
        public async Task<ApiCallResponse> GetCityList()
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetCityList");
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);

            if (response.IsSuccessStatusCode)
            {
                var users = await response.Content.ReadFromJsonAsync<List<City>>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = users };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }

        }
        #endregion

        #region Add New Company
        public async Task<ApiCallResponse> AddNewCompany(Company args)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Lookups/AddNewCompany");
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
                    var content = await response.Content.ReadFromJsonAsync<bool>();
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

        #region Company List
        public async Task<ApiCallResponse> GetDesignationList()
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Users/GetDesignations");
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);

            if (response.IsSuccessStatusCode)
            {
                var users = await response.Content.ReadFromJsonAsync<List<Designation>>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = users };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }

        }
        #endregion

        #region Add New Designation
        public async Task<ApiCallResponse> AddNewDesignation(Designation args)
        {
            try
            {
                args.CreatedDate = DateTime.Now;
                args.CreatedBy = await browserStorage.GetItemAsync<string>("User");

                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Lookups/AddNewDesignation");
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
                    var content = await response.Content.ReadFromJsonAsync<bool>();
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

        #region Get Contact Details For File
        public async Task<ApiCallResponse> GetContactDetailsForFile(Guid fileId)
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetContactDetailsForFile?fileId=" + fileId);
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadFromJsonAsync<List<ContactFileLinkVM>>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }

        }
        #endregion

        #region Remove selected contact from file
        public async Task<ApiCallResponse> RemoveContact(ContactFileLinkVM args)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Lookups/RemoveContact");
                var postBody = args;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<bool>();
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

        #region Add Selected Contact to file
        public async Task<ApiCallResponse> AddSelectedContactToFile(IList<ContactListVM> selectedContact, Guid? fileId, int? fileModule)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Lookups/AddSelectedContactToFile?fileId=" + fileId + "&fileModule=" + fileModule + "&CurrentUser=" + await browserStorage.GetItemAsync<string>("User"));
                var postBody = selectedContact;
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
                    var content = await response.Content.ReadFromJsonAsync<bool>();
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

        #region Get Attachment Type Detail
        public async Task<ApiCallResponse> GetAttachmentTypeDetailById(int? AttachmentTypeId)
        {
            try
            {
                var userDetail = await browserStorage.GetItemAsync<UserDetailVM>("UserDetail");
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("dms_api_url") + "/FileUpload/GetAttachmentTypeDetailById?AttachmentTypeId=" + AttachmentTypeId);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var attachTypes = await response.Content.ReadFromJsonAsync<AttachmentType>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = attachTypes };
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

        public async Task<ApiCallResponse> GetAttendeeMeetingStatus()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetAttendeeMeetingStatus");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var statuses = await response.Content.ReadFromJsonAsync<List<MeetingAttendeeStatus>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = statuses };
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
        public async Task<ApiCallResponse> GetAttendeeUser()
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Account/GetAttendeeUser");
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);

            if (response.IsSuccessStatusCode)
            {
                var users = await response.Content.ReadFromJsonAsync<List<FatwaAttendeeVM>>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = users };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }

        }

        #region Get Chamber Detail By Id
        public async Task<ApiCallResponse> GetChamberById(int Id)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetChamberById?Id=" + Id);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var users = await response.Content.ReadFromJsonAsync<Chamber>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = users };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }

        }
        #endregion

        #region Get Chamber Detail By court Id
        public async Task<ApiCallResponse> GetChamberByCourtId(int courtId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetChamberByCourtId?courtId=" + courtId);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var users = await response.Content.ReadFromJsonAsync<List<Chamber>>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = users };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }

        }
        #endregion

        #region Weekdays Settings
        public async Task<ApiCallResponse> GetWeekdaysSettings()
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetWeekdaysSettings");
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);

            if (response.IsSuccessStatusCode)
            {
                var weekdaysSettings = await response.Content.ReadFromJsonAsync<List<WeekdaysSetting>>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = weekdaysSettings };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }

        }
        #endregion

        #region Get Status
        public async Task<ApiCallResponse> GetStatus()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("fatwa_oss_api_url") + "/OrganizingCommittee/GetStatus");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var status = await response.Content.ReadFromJsonAsync<List<CommitteeStatus>>();
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
        #endregion

        #region Get Committee Roles
        public async Task<ApiCallResponse> GetCommitteeRoles()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("fatwa_oss_api_url") + "/OrganizingCommittee/GetCommitteeRoles");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<IEnumerable<CommitteeRoles>>();
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



        public async Task<ApiCallResponse> GetDocumentTypeById(int AttachmentTypeId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("dms_api_url") + "/FileUpload/GetDocumentTypeById?AttachmentTypeId=" + AttachmentTypeId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var users = await response.Content.ReadFromJsonAsync<AttachmentType>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = users };
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
        public async Task<ApiCallResponse> GetGovernmentEntityRepresentatives(GovtEntityIdsPayload govtEntityIds)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Lookups/GetGovernmentEntityRepresentatives");
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(govtEntityIds), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    var users = await response.Content.ReadFromJsonAsync<GovtEntityRepresentativeNamesResponseVM>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = users };
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
        #region Get Stock Taking Status
        public async Task<ApiCallResponse> GetStockTakingStatus()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetStockTakingStatus");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<LmsStockTakingStatus>>();
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
        #region Get Sector Types By Department Id
        public async Task<ApiCallResponse> GetOperatingSectorsByDepartmentId(int DepartmentId)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetOperatingSectorsByDepartmentId?DepartmentId=" + DepartmentId);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<OperatingSectorType>>();
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

        #region Get Consultation Interrnational Arbitration Types List
        //<History Author = 'Muhammad Zaeem' Date='2024-10-10' Version="1.0" Branch="master">Get Consultation International Arbitration Types</History>
        public async Task<ApiCallResponse> GetConsultationInternationalArbitrationTypes()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetConsultationInternationalArbitrationTypes");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<ComsInternationalArbitrationType>>();
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

        #region Get Consultation Legislation File Types
        public async Task<ApiCallResponse> GetConsultationLegislationFileTypes()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetConsultationLegislationFileTypes");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var statuses = await response.Content.ReadFromJsonAsync<List<ConsultationLegislationFileType>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = statuses };
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
        #region Get Manager Task Reminder Data
        public async Task<ApiCallResponse> GetManagerTaskReminderData(Guid TaskId)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Lookups/GetManagerTaskReminderData?TaskId=" + TaskId);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var statuses = await response.Content.ReadFromJsonAsync<List<ManagerTaskReminderVM>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = statuses };
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

        #region Bug Reporting
        public async Task<ApiCallResponse> GetAllApplications()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/BugReporting/GetAllApplications");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<BugApplication>>();
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

        public async Task<ApiCallResponse> GetBugModules()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/BugReporting/GetBugModules");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var modules = await response.Content.ReadFromJsonAsync<List<BugModule>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = modules };
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

        public async Task<ApiCallResponse> GetBugStatuses()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/BugReporting/GetBugStatuses");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var issueTypes = await response.Content.ReadFromJsonAsync<List<BugStatus>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = issueTypes };
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

        public async Task<ApiCallResponse> GetSeverity()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/BugReporting/GetSeverityLevel");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var severities = await response.Content.ReadFromJsonAsync<List<BugSeverity>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = severities };
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
        public async Task<ApiCallResponse> GetIssuesTypes()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/BugReporting/GetIssueTypes");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var issueTypes = await response.Content.ReadFromJsonAsync<List<BugIssueType>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = issueTypes };
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

        #region Get User Data By Bug Type Id
        public async Task<ApiCallResponse> GetUsersByBugTypeId(int TypeId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Users/GetUsersByBugTypeId?TypeId=" + TypeId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<IEnumerable<UserDataVM>>();
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

        #region Get Group 
        public async Task<ApiCallResponse> GetGroups()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Group/GetGroups");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var groupAccessType = await response.Content.ReadFromJsonAsync<List<Group>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = groupAccessType };
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

        #region Get Modules By Application Id
        public async Task<ApiCallResponse> GetModulesByApplicationId(int ApplicationId)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/BugReporting/GetModulesByApplicationId?ApplicationId=" + ApplicationId);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var modules = await response.Content.ReadFromJsonAsync<List<BugModule>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = modules };
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
