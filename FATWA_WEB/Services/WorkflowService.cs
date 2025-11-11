using Blazored.LocalStorage;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.Dms;
using FATWA_DOMAIN.Models.LLSLegalPrinciple;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_DOMAIN.Models.WorkflowModels;
using FATWA_WEB.Data;
using FATWA_WEB.Pages.Lds;
using MsgKit.Enums;
using Radzen;
using System.Data.Entity;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;
using static FATWA_DOMAIN.Enums.DmsEnums;
using static FATWA_DOMAIN.Enums.WorkflowEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Services
{
    //<History Author = 'Hassan Abbas' Date='2022-04-30' Version="1.0" Branch="master"> Service for communication between Razor Components and API</History>
    public partial class WorkflowService
    {
        #region Variables
        private readonly IConfiguration _config;
        private readonly ILocalStorageService browserStorage;
        private readonly TranslationState translationState;
        private readonly FileUploadService fileUploadService;
        #endregion

        #region Constructor
        public WorkflowService(IConfiguration configuration, ILocalStorageService _browserStorage, TranslationState _translationState, FileUploadService _fileUploadService)
        {
            _config = configuration;
            browserStorage = _browserStorage;
            translationState = _translationState;
            fileUploadService = _fileUploadService;
        }
        #endregion

        #region Get Workflows
        //<History Author = 'Hassan Abbas' Date='2022-04-30' Version="1.0" Branch="master"> Call API for getting List of Workflows</History>
        public async Task<ApiCallResponse> GetWorkflows(WorkflowAdvanceSearchVM advanceSearchVM)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Workflow/GetWorkflows");
                var postBody = advanceSearchVM;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<IEnumerable<WorkflowListVM>>();
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

        public async Task<ApiCallResponse> GetWorkflowsCount()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetWorkflowsCount");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<WorkflowCountVM>();
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

        public async Task<ApiCallResponse> GetWorkflowsInstanceCount(int Workflowid)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + $"/Workflow/GetWorkflowsInstanceCount?Workflowid=" + Workflowid);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<WorkflowInstanceCountVM>();
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

        //<History Author = 'Hassan Abbas' Date='2022-04-30' Version="1.0" Branch="master"> Call API for getting List of Workflows</History>
        private async Task<IQueryable<WorkflowVM>> GetWorkflows()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetWorkflows");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(translationState.Translate("Contact_Administrator"));
            }
            var responselist = response.Content.ReadFromJsonAsync<IEnumerable<WorkflowVM>>();
            var queryableX = (await responselist).AsQueryable();
            return queryableX;
        }

        // <History Author = 'Hassan Abbas' Date='2022-04-30' Version="1.0" Branch="master"> Call API for getting List of Workflows</History>
        public async Task<ApiCallResponse> GetActiveWorkflows(int moduleId, int moduleTriggerId, int? attachmentTypeId, int? submoduleId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetActiveWorkflows");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                request.Content = new FormUrlEncodedContent(new[]{
                          new KeyValuePair<string, string>("moduleId", moduleId.ToString()),
                          new KeyValuePair<string, string>("moduleTriggerId", moduleTriggerId.ToString()),
                          new KeyValuePair<string, string>("attachmentTypeId", attachmentTypeId.ToString()),
                          new KeyValuePair<string, string>("submoduleId", submoduleId.ToString()),
                      });
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<IEnumerable<WorkflowVM>>();
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

        //<History Author = 'Hassan Abbas' Date='2022-04-30' Version="1.0" Branch="master"> Call API for getting workflow by Id</History>
        public async Task<ApiCallResponse> GetWorkflowDetailById(int workflowId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetWorkflowDetailById");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                request.Content = new FormUrlEncodedContent(new[]{
                        new KeyValuePair<string, string>("workflowId", workflowId.ToString()),
                    });
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<Workflow>();
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

        //<History Author = 'Hassan Abbas' Date='2022-04-30' Version="1.0" Branch="master"> Call API for getting workflow Trigger by Workflow Id</History>
        public async Task<WorkflowTrigger> GetWorkflowTriggerByWorkflowId(int workflowId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetWorkflowTriggerByWorkflowId");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            request.Content = new FormUrlEncodedContent(new[]{
                        new KeyValuePair<string, string>("workflowId", workflowId.ToString()),
                    });
            var response = await new HttpClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<WorkflowTrigger>();
            else
                return null;
        }

        #endregion

        #region Virtual Dropdown Data Filling Events

        static List<Module> WorkflowModule { get; set; }
        static List<ModuleTrigger> AllTriggers { get; set; }
        static List<WorkflowSubModule> SubModuleTriggers { get; set; }

        //<History Author = 'Hassan Abbas' Date='2022-04-23' Version="1.0" Branch="master">Remote Module Data for Add Document Drop Down</History>
        public async Task<DataEnvelope<Module>> GetModuleItems(DataSourceRequest request)
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetWorkflowModules");
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(translationState.Translate("Contact_Administrator"));
            }
            WorkflowModule = await response.Content.ReadFromJsonAsync<List<Module>>();
            var result = await WorkflowModule.ToDataSourceResultAsync(request);
            DataEnvelope<Module> dataToReturn = new DataEnvelope<Module>
            {
                Data = result.Data.Cast<Module>().ToList(),
                Total = result.Total
            };
            return await Task.FromResult(dataToReturn);
        }
        //<History Author = 'Hassan Abbas' Date='2022-04-23' Version="1.0" Branch="master">Filter Item based on value</History>
        public async Task<Module> GetModuleItemFromValue(int selectedValue)
        {
            if (WorkflowModule != null)
            {
                return await Task.FromResult(WorkflowModule.FirstOrDefault(x => selectedValue == x.ModuleId));
            }
            else
            {
                return await Task.FromResult<Module>(null);
            }
        }

        public async Task<ApiCallResponse> GetTriggerItemsData(int submoduleId)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetModuleTriggers?submoduleId=" + submoduleId);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    AllTriggers = await response.Content.ReadFromJsonAsync<List<ModuleTrigger>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = AllTriggers };
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
        public async Task<ApiCallResponse> GetSubModuleItems(int moduleId)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetSubModuleTriggers?moduleId=" + moduleId);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    SubModuleTriggers = await response.Content.ReadFromJsonAsync<List<WorkflowSubModule>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = SubModuleTriggers };
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

        //<History Author = 'Hassan Abbas' Date='2022-04-23' Version="1.0" Branch="master">Get Workflow Trigger by Id</History>
        public async Task<ModuleTriggerVM> GetModuleTriggerById(int triggerId)
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetModuleTriggerById?triggerId=" + triggerId);
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(translationState.Translate("Contact_Administrator"));
            }
            return await response.Content.ReadFromJsonAsync<ModuleTriggerVM>();
        }
        public async Task<List<AttachmentTypeListVM>> GetAttachementTypesById(int workflowId)
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetAttachementTypesById?workflowId=" + workflowId);
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(translationState.Translate("Contact_Administrator"));
            }
            return await response.Content.ReadFromJsonAsync<List<AttachmentTypeListVM>>();
        }


        //<History Author = 'Hassan Abbas' Date='2022-04-23' Version="1.0" Branch="master">Get Module conditions by API</History>
        public async Task<List<ModuleCondition>> GetModuleConditions(int triggerId)
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetModuleConditions?triggerId=" + triggerId);
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<List<ModuleCondition>>();
            else
                return new List<ModuleCondition>();
        }

        //<History Author = 'Hassan Abbas' Date='2022-04-23' Version="1.0" Branch="master">Remote Catalog Data for Add Document Drop Down</History>
        public async Task<List<ModuleActivity>> GetModuleActvities(int triggerId)
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetModuleActvities?triggerId=" + triggerId);
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<List<ModuleActivity>>();
            else
                return new List<ModuleActivity>();
        }


        //<History Author = 'Hassan Abbas' Date='2022-04-23' Version="1.0" Branch="master">Remote Catalog Data for Add Document Drop Down</History>
        public async Task<List<ModuleActivity>> GetModuleActvitiesByCategory(int triggerId, int categoryId)
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetModuleActvitiesByCategory?triggerId=" + triggerId + "categoryId");
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            request2.Content = new FormUrlEncodedContent(new[]{
                        new KeyValuePair<string, string>("triggerId", triggerId.ToString()),
                        new KeyValuePair<string, string>("categoryId", categoryId.ToString())
                    });
            var response = await new HttpClient().SendAsync(request2);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<List<ModuleActivity>>();
            else
                return new List<ModuleActivity>();
        }

        //<History Author = 'Hassan Abbas' Date='2022-04-23' Version="1.0" Branch="master">Remote Catalog Data for Add Document Drop Down</History>
        public async Task<List<Parameter>> GetModuleActivityParameters(int activityId)
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetModuleActivityParameters");
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            request2.Content = new FormUrlEncodedContent(new[]{
                        new KeyValuePair<string, string>("activityId", activityId.ToString()),
                    });
            var response = await new HttpClient().SendAsync(request2);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<List<Parameter>>();
            else
                return new List<Parameter>();
        }

        //<History Author = 'Hassan Abbas' Date='2022-04-23' Version="1.0" Branch="master">Remote Catalog Data for Add Document Drop Down</History>
        public async Task<List<Parameter>> GetSlaActionParameters(int actionId)
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetSlaActionParameters");
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            request2.Content = new FormUrlEncodedContent(new[]{
                        new KeyValuePair<string, string>("actionId", actionId.ToString()),
                    });
            var response = await new HttpClient().SendAsync(request2);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<List<Parameter>>();
            else
                return new List<Parameter>();
        }

        //<History Author = 'Hassan Abbas' Date='2022-04-23' Version="1.0" Branch="master">Remote Catalog Data for Add Document Drop Down</History>
        public async Task<ApiCallResponse> GetWorkflowStatuses()
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetWorkflowStatuses");
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<WorkflowStatus>>();
                    return new ApiCallResponse { StatusCode=response.StatusCode, IsSuccessStatusCode=response.IsSuccessStatusCode, ResultData=content};
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
        public async Task<IEnumerable<Module>> GetWorkflowModules()
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetWorkflowStatuses");
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<List<Module>>();
            else
                return new List<Module>();
        }

        public async Task<ApiCallResponse> GetModuleOptionsByTriggerId(int triggerId)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetModuleOptionsByTriggerId?triggerId=" + triggerId);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<ModuleConditionOptions>>();
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

        #region Create Workflow

        //<History Author = 'Hassan Abbas' Date='2022-04-23' Version="1.0" Branch="master">Submit workflow to API</History>
        public async Task<ApiCallResponse> CreateWorkflow(Workflow item)
        {
            try
            {
                item.CreatedBy = await browserStorage.GetItemAsync<string>("User");
                item.CreatedDate = DateTime.Now;
                item.IsDeleted = false;
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Workflow/CreateWorkflow");
                var postBody = item;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return new ApiCallResponse { StatusCode=response.StatusCode, IsSuccessStatusCode=response.IsSuccessStatusCode };
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

        #region Workflow Activities

        //<History Author = 'Hassan Abbas' Date='2022-04-30' Version="1.0" Branch="master"> Call API for getting List of Workflow Activities</History>
        public async Task<ApiCallResponse> GetWorkflowActivities(int workflowId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetWorkflowActivities");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                request.Content = new FormUrlEncodedContent(new[]{
                        new KeyValuePair<string, string>("workflowId", workflowId.ToString()),
                    });
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<IEnumerable<WorkflowActivityVM>>();
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

        //<History Author = 'Hassan Abbas' Date='2023-10-29' Version="1.0" Branch="master"> Get workflow Trigger Conditions</History>
        public async Task<IEnumerable<WorkflowTriggerConditionsVM>> GetWorkflowTriggerConditions(int workflowTriggerId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetWorkflowTriggerConditions?workflowTriggerId=" + workflowTriggerId);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<IEnumerable<WorkflowTriggerConditionsVM>>();
            else
                return null;
        }

        //<History Author = 'Hassan Abbas' Date='2022-04-30' Version="1.0" Branch="master"> Call API for getting List of Workflows</History>
        public async Task<List<WorkflowActivity>> GetWorkflowActivitiesByWorkflowId(int workflowId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetWorkflowActivitiesByWorkflowId");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            request.Content = new FormUrlEncodedContent(new[]{
                        new KeyValuePair<string, string>("workflowId", workflowId.ToString()),
                    });
            var response = await new HttpClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<List<WorkflowActivity>>();
            else
                return null;
        }

        //<History Author = 'Hassan Abbas' Date='2022-04-30' Version="1.0" Branch="master"> Call API for getting List of Workflows</History>
        public async Task<WorkflowActivity> GetWorkflowActivityById(int workflowActivityId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetWorkflowActivityById");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            request.Content = new FormUrlEncodedContent(new[]{
                        new KeyValuePair<string, string>("workflowActivityId", workflowActivityId.ToString()),
                    });
            var response = await new HttpClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<WorkflowActivity>();
            else
                return null;
        }

        //<History Author = 'Hassan Abbas' Date='2022-04-30' Version="1.0" Branch="master"> Call API for getting List of Workflows</History>
        public async Task<WorkflowActivityVM> GetWorkflowActivityBySequenceNumber(int workflowId, int sequenceNumber)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetWorkflowActivityBySequenceNumber");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            request.Content = new FormUrlEncodedContent(new[]{
                        new KeyValuePair<string, string>("workflowId", workflowId.ToString()),
                        new KeyValuePair<string, string>("sequenceNumber", sequenceNumber.ToString()),
                    });
            var response = await new HttpClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<WorkflowActivityVM>();
            else
                return null;
        }

        //<History Author = 'Hassan Abbas' Date='2022-04-30' Version="1.0" Branch="master"> Call API for getting List of Workflows</History>
        public async Task<List<WorkflowConditionsVM>> GetWorkflowConditions(int workflowActivityId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetWorkflowConditions");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            request.Content = new FormUrlEncodedContent(new[]{
                        new KeyValuePair<string, string>("workflowActivityId", workflowActivityId.ToString()),
                    });
            var response = await new HttpClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<List<WorkflowConditionsVM>>();
            else
                return null;
        }

        //<History Author = 'Hassan Abbas' Date='2022-04-30' Version="1.0" Branch="master"> Call API for getting List of Workflows</History>
        public async Task<List<WorkflowCondition>> GetWorkflowConditionsForUpdate(int workflowActivityId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetWorkflowConditions");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            request.Content = new FormUrlEncodedContent(new[]{
                        new KeyValuePair<string, string>("workflowActivityId", workflowActivityId.ToString()),
                    });
            var response = await new HttpClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<List<WorkflowCondition>>();
            else
                return null;
        }
        public async Task<List<WorkflowOption>> GetWorkflowOptionsForUpdate(int workflowActivityId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetWorkflowOptions");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            request.Content = new FormUrlEncodedContent(new[]{
                        new KeyValuePair<string, string>("workflowActivityId", workflowActivityId.ToString()),
                    });
            var response = await new HttpClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<List<WorkflowOption>>();
            else
                return null;
        }
        public async Task<List<WorkflowConditionsOptionsListVM>> GetWorkflowConditionsOptionsList(int workflowConditionId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetWorkflowConditionOptionsList");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            request.Content = new FormUrlEncodedContent(new[]{
                        new KeyValuePair<string, string>("workflowConditionId", workflowConditionId.ToString()),
                    });
            var response = await new HttpClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<List<WorkflowConditionsOptionsListVM>>();
            else
                return null;
        }

        //<History Author = 'Hassan Abbas' Date='2022-04-30' Version="1.0" Branch="master"> Call API for getting List of Workflows</History>
        public async Task<IEnumerable<WorkflowActivityParametersVM>> GetWorkflowActivityParameters(int workflowActivityId, dynamic entity, int? TriggerId)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Workflow/GetWorkflowActivityParameters?workflowActivityId=" + workflowActivityId + "&TriggerId=" + TriggerId);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var postBody = entity;
            request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
            var response = await new HttpClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<IEnumerable<WorkflowActivityParametersVM>>();
            else
                return null;
        }


        //<History Author = 'Hassan Abbas' Date='2022-04-30' Version="1.0" Branch="master"> Call API for getting List of Workflows</History>
        public async Task<List<Parameter>> GetWorkflowActivityParametersForUpdate(int workflowActivityId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetWorkflowActivityParametersForUpdate?workflowActivityId=" + workflowActivityId);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<List<Parameter>>();
            else
                return null;
        }

        public async Task<bool> UpdateWorkflowInstanceStatus(Guid referenceId, int statusId)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Workflow/UpdateWorkflowInstanceStatus");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            request.Content = new FormUrlEncodedContent(new[]{
                        new KeyValuePair<string, string>("referenceId", referenceId.ToString()),
                        new KeyValuePair<string, string>("statusId", statusId.ToString()),
                    });
            var response = await new HttpClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }


        //<History Author = 'Hassan Abbas' Date='2022-04-30' Version="1.0" Branch="master"> Call API for getting List of Workflows</History>
        public async Task<WorkflowActivity> GetInstanceCurrentActivity(Guid referenceId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetInstanceCurrentActivity");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            request.Content = new FormUrlEncodedContent(new[]{
                        new KeyValuePair<string, string>("referenceId", referenceId.ToString()),
                    });
            var response = await new HttpClient().SendAsync(request);
            try
            {
                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadFromJsonAsync<WorkflowActivity>();
                else
                    return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-04-30' Version="1.0" Branch="master"> Call API for getting List of Workflows</History>
        public async Task<IEnumerable<WorkflowActivityVM>> GetToDoWorkflowActivities(int workflowId, int sequenceNumber)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetToDoWorkflowActivities");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            request.Content = new FormUrlEncodedContent(new[]{
                        new KeyValuePair<string, string>("workflowId", workflowId.ToString()),
                        new KeyValuePair<string, string>("sequenceNumber", sequenceNumber.ToString()),
                    });
            var response = await new HttpClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<IEnumerable<WorkflowActivityVM>>();
            else
                return null;
        }

        #endregion

        #region Lds Document

        //<History Author = 'Hassan Abbas' Date='2022-04-30' Version="1.0" Branch="master"> Submit Document Instance for update</History>
        public async Task<bool> UpdateDocumentInstance(LegalLegislation document)
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Workflow/UpdateDocumentInstance");
                var postBody = document;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", document.Token);
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                return true;
                else
                return false;
        }

        #endregion

        #region Lps Principle

        //<History Author = 'Hassan Abbas' Date='2022-04-30' Version="1.0" Branch="master"> Submit Principle Instance for update</History>
        public async Task<bool> UpdatePrincipleInstance(LLSLegalPrincipleSystem principle)
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Workflow/UpdatePrincipleInstance");
                var postBody = principle;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", principle.Token);
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                return true;
                else
                return false;
        }

        #endregion

        #region Cms Case Draft

        //<History Author = 'Hassan Abbas' Date='2022-04-30' Version="1.0" Branch="master"> Submit Draft Instance for update</History>
        public async Task<bool> UpdateCaseDraftInstance(CmsDraftedTemplate draft)
        {
            draft.ModifiedDate = DateTime.Now;
            var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Workflow/UpdateCaseDraftInstance");
            var postBody = draft;
            request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", draft.Token);
            var response = await new HttpClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        #endregion

        #region Coms Case Draft

        //<History Author = 'Hassan Abbas' Date='2022-04-30' Version="1.0" Branch="master"> Submit Draft Instance for update</History>
        public async Task<bool> UpdateConsultationDraftInstance(ComsDraftedTemplate draft)
        {
            draft.ModifiedDate = DateTime.Now;
            var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Workflow/UpdateConsultationDraftInstance");
            var postBody = draft;
            request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", draft.Token);
            var response = await new HttpClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        #endregion

        #region DMS Review Document
        public async Task<bool> UpdateDMSDocumentInstance(DmsAddedDocument document)
        {
            document.ModifiedDate = DateTime.Now;
            var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Workflow/UpdateDMSDocumentInstance");
            var postBody = document;
            request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", document.Token);
            var response = await new HttpClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }
        #endregion

        #region Get Workflow Instances
        partial void OnWorkflowInstanceDocumentRead(ref IQueryable<WorkflowInstanceDocumentVM> items);

        //<History Author = 'Hassan Abbas' Date='2022-04-30' Version="1.0" Branch="master"> Call API for getting List of Workflows</History>
        public async Task<ApiCallResponse> GetWorkflowInstanceDocuments(int PageSize, int PageNumber)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetWorkflowInstanceDocuments?PageSize=" + PageSize + "&PageNumber=" + PageNumber);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<IEnumerable<WorkflowInstanceDocumentVM>>();
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

        #region Update Workflow Status

        //<History Author = 'Hassan Abbas' Date='2022-04-30' Version="1.0" Branch="master"> Update workflow Status</History>
        public async Task<bool> UpdateWorkflowStatus(int workflowId, int statusId)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Workflow/UpdateWorkflowStatus");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            request.Content = new FormUrlEncodedContent(new[]{
                        new KeyValuePair<string, string>("workflowId", workflowId.ToString()),
                        new KeyValuePair<string, string>("statusId", statusId.ToString()),
                    });
            var response = await new HttpClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }
        public async Task<ApiCallResponse> GetWorkflowforSuspend(int workflowId, int statusId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetWorkflowforSuspend?workflowId=" + workflowId + "&statusId=" + statusId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<Workflow>();
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

        #region Assign Workflow Activity

        //<History Author = 'Hassan Abbas' Date='2022-09-07' Version="1.0" Branch="master">Assign Workflow Activity to a Submitted Entity</History>
        public async Task AssignWorkflowActivity(WorkflowVM activeWorkflow, dynamic entity, int module, int? TriggerId, List<UserRole>? userRoles)
        {
            try
            {
                dynamic Entity = "";
                if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsDocument)
                {
                    Entity = (LegalLegislation)entity;
                }
                else if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsPrinciple)
                {
                    Entity = (LLSLegalPrincipleSystem)entity;
                }
                else if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsCaseDraft || TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsConsultationDraft)
                {
                    Entity = (CmsDraftedTemplate)entity;
                }
                else if (TriggerId >= (int)WorkflowModuleTriggerEnum.TransferCaseRequest && TriggerId <= (int)WorkflowModuleTriggerEnum.TransferConfidentialConsultationRequestPrivateOffice)
                {
                    Entity = (CmsApprovalTracking)entity;
                }
                else if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsDMSDocument)
                {
                    Entity = (DmsAddedDocument)entity;
                }
                Entity.Token = await browserStorage.GetItemAsync<string>("Token");
                var userDetail = await browserStorage.GetItemAsync<UserDetailVM>("UserDetail");

                var workflowTriggerConditions = await GetWorkflowTriggerConditions((int)activeWorkflow.WorkflowTriggerId);
                if (workflowTriggerConditions.Any())
                {
                    foreach (var condition in workflowTriggerConditions.OrderByDescending(x => x.MKey == WorkflowConditionEnum.CmsSubmittedByViceHOSAndViceHOSApprovalEnough.ToString()))
                    {
                        bool viceHosResponsibilityCheck = false;
                        if (String.Equals(condition.MKey, WorkflowConditionEnum.CmsSubmittedByViceHOSAndViceHOSApprovalEnough.ToString(), StringComparison.OrdinalIgnoreCase))
                        {
                            if (userRoles.Any(r => r.RoleId == condition.ValueToCompare) && userDetail.SectorOnlyViceHOSApprovalEnough)
                            {
                                viceHosResponsibilityCheck = true;
                            }
                            else
                            {
                                continue;
                            }
                        }
                        if (userRoles.Any(r => r.RoleId == condition.ValueToCompare) || viceHosResponsibilityCheck)
                        {
                            if (condition.TrueCaseFlowControlId == WorkflowControl.JumptoStep)
                            {
                                var trueCaseActivity = await GetWorkflowActivityBySequenceNumber((int)activeWorkflow.WorkflowId, (int)condition.TrueCaseActivityNo);
                                Type type = typeof(WorkflowImplementationService);
                                System.Reflection.MethodInfo methodInfo = type.GetMethod(trueCaseActivity.Method);
                                if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsDMSDocument)
                                {
                                    Entity.IsEndofFlow = (bool)trueCaseActivity.IsEndofFlow;
                                }
                                var activityParams = await GetWorkflowActivityParameters((int)trueCaseActivity.WorkflowActivityId, entity, TriggerId);
                                int arraySize = activityParams.Count + 1;
                                object[] methodParams = new object[arraySize];
                                methodParams[0] = Entity;
                                for (int i = 1; i <= activityParams.Count; i++)
                                {
                                    methodParams[i] = activityParams.ToArray()[i - 1];
                                }
                                var response3 = (Task<WorkflowActivityResponse>)methodInfo.Invoke(methodInfo, methodParams);
                                if (response3.Result.Success)
                                {
                                }
                                else
                                {
                                    if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsDocument)
                                    {
                                        await UpdateWorkflowInstanceStatus(Entity.LegislationId, (int)WorkflowInstanceStatusEnum.Failed);
                                    }
                                    else if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsPrinciple)
                                    {
                                        await UpdateWorkflowInstanceStatus(Entity.PrincipleId, (int)WorkflowInstanceStatusEnum.Failed);
                                    }
                                    else if (TriggerId > (int)WorkflowModuleTriggerEnum.UserSubmitsPrinciple)
                                    {
                                        await UpdateWorkflowInstanceStatus(Entity.Id, (int)WorkflowInstanceStatusEnum.Failed);
                                    }
                                }
                                break;
                            }
                            else if (condition.TrueCaseFlowControlId == WorkflowControl.EndofFlow)
                            {
                                Entity.WorkflowInstanceStatusId = WorkflowInstanceStatusEnum.Success;
                                if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsDocument)
                                {
                                    await UpdateDocumentInstance(Entity);
                                }
                                else if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsPrinciple)
                                {
                                    await UpdatePrincipleInstance(Entity);
                                }
                                else if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsCaseDraft || TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsConsultationDraft)
                                {
                                    await UpdateCaseDraftInstance(Entity);
                                }
                                else if (TriggerId == (int)WorkflowModuleTriggerEnum.TransferCaseRequest || TriggerId == (int)WorkflowModuleTriggerEnum.TransferCaseFile)
                                {
                                    await UpdateApprovalTrackingInstance(Entity);

                                }
                                else if (TriggerId == (int)WorkflowModuleTriggerEnum.TransferConsultationRequest || TriggerId == (int)WorkflowModuleTriggerEnum.TransferConsultationFile)
                                {
                                    await UpdateApprovalTrackingConsultationInstance(Entity);
                                }
                                else if (TriggerId == (int)WorkflowModuleTriggerEnum.SendCopyCaseRequest || TriggerId == (int)WorkflowModuleTriggerEnum.SendCopyCaseFile)
                                {
                                    await UpdateCopyTrackingInstance(Entity);
                                }
                                else if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsDMSDocument)
                                {
                                    await UpdateDMSDocumentInstance(Entity);
                                }
                                break;
                            }
                        }
                    }
                }
                else
                {
                    IEnumerable<WorkflowActivityVM> workflowActivities;
                    var response1 = await GetWorkflowActivities((int)activeWorkflow.WorkflowId);
                    if(response1.IsSuccessStatusCode)
                    {
                        workflowActivities = (IEnumerable<WorkflowActivityVM>)response1.ResultData;
                        if (workflowActivities?.Count() > 0)
                        {
                            foreach (var activity in workflowActivities.OrderBy(wa => wa.SequenceNumber))
                            {
                                if (activity.CategoryId == (int)WorkflowActivityCategory.GeneralControls)
                                {
                                    Type type = typeof(WorkflowImplementationService);
                                    System.Reflection.MethodInfo methodInfo = type.GetMethod(activity.Method);

                                    var activityParams = await GetWorkflowActivityParameters((int)activity.WorkflowActivityId, entity, TriggerId);
                                    int arraySize = activityParams.Count;
                                    object[] methodParams = new object[arraySize];
                                    for (int i = 0; i < arraySize; i++)
                                    {
                                        methodParams[i] = activityParams.ToArray()[i];
                                    }
                                    var response = (Task<WorkflowActivityResponse>)methodInfo.Invoke(activity.Method, methodParams);
                                    if (response.Result.Success)
                                    {
                                        var nextActivities = await GetToDoWorkflowActivities((int)activity.WorkflowId, (int)activity.SequenceNumber);
                                        if (nextActivities?.Count() <= 0)
                                        {
                                            if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsDocument)
                                            {
                                                await UpdateWorkflowInstanceStatus(Entity.LegislationId, (int)WorkflowInstanceStatusEnum.Success);
                                            }
                                            else if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsPrinciple)
                                            {
                                                await UpdateWorkflowInstanceStatus(Entity.PrincipleId, (int)WorkflowInstanceStatusEnum.Success);
                                            }
                                            else if (TriggerId > (int)WorkflowModuleTriggerEnum.UserSubmitsPrinciple)
                                            {
                                                await UpdateWorkflowInstanceStatus(Entity.Id, (int)WorkflowInstanceStatusEnum.Success);
                                            }

                                        }
                                    }
                                    else
                                    {
                                        if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsDocument)
                                        {
                                            await UpdateWorkflowInstanceStatus(Entity.LegislationId, (int)WorkflowInstanceStatusEnum.Failed);
                                        }
                                        else if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsPrinciple)
                                        {
                                            await UpdateWorkflowInstanceStatus(Entity.PrincipleId, (int)WorkflowInstanceStatusEnum.Failed);
                                        }
                                        else if (TriggerId > (int)WorkflowModuleTriggerEnum.UserSubmitsPrinciple)
                                        {
                                            await UpdateWorkflowInstanceStatus(Entity.Id, (int)WorkflowInstanceStatusEnum.Failed);
                                        }
                                        break;
                                    }
                                }
                                else if (activity.CategoryId == (int)WorkflowActivityCategory.Tasks)
                                {
                                    Type type = typeof(WorkflowImplementationService);
                                    System.Reflection.MethodInfo methodInfo = type.GetMethod(activity.Method);
                                    var activityParams = await GetWorkflowActivityParameters((int)activity.WorkflowActivityId, entity, TriggerId);
                                    int arraySize = activityParams.Count + 1;
                                    object[] methodParams = new object[arraySize];
                                    methodParams[0] = Entity;
                                    for (int i = 1; i <= activityParams.Count; i++)
                                    {
                                        methodParams[i] = activityParams.ToArray()[i - 1];
                                    }
                                    var response = (Task<WorkflowActivityResponse>)methodInfo.Invoke(methodInfo, methodParams);
                                    if (response.Result.Success)
                                    {
                                    }
                                    else
                                    {
                                        if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsDocument)
                                        {
                                            await UpdateWorkflowInstanceStatus(Entity.LegislationId, (int)WorkflowInstanceStatusEnum.Failed);
                                        }
                                        else if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsPrinciple)
                                        {
                                            await UpdateWorkflowInstanceStatus(Entity.PrincipleId, (int)WorkflowInstanceStatusEnum.Failed);
                                        }
                                        else if (TriggerId > (int)WorkflowModuleTriggerEnum.UserSubmitsPrinciple)
                                        {
                                            await UpdateWorkflowInstanceStatus(Entity.Id, (int)WorkflowInstanceStatusEnum.Failed);
                                        }
                                        break;
                                    }
                                    break;
                                }
                            }
                        }
                    }
                  
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Activities Execution for Entities

        //<History Author = 'Hassan Abbas' Date='2022-09-07' Version="1.0" Branch="master">Process Upcoming Workflow Activities for the given entity</History>
        public async Task ProcessWorkflowActvivities(dynamic entity, int module, int? TriggerId)
        {
            Guid ReferenceId = Guid.NewGuid();

            if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsDocument)
            {
                var document = (LegalLegislation)entity;
                ReferenceId = document.LegislationId;
            }
            else if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsPrinciple)
            {
                var principle = (LLSLegalPrincipleSystem)entity;
                ReferenceId = principle.PrincipleId;
            }
            else if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsCaseDraft || TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsConsultationDraft)
            {
                var template = (CmsDraftedTemplate)entity;
                ReferenceId = template.Id;
            }
            else if (TriggerId >= (int)WorkflowModuleTriggerEnum.TransferCaseRequest && TriggerId <= (int)WorkflowModuleTriggerEnum.TransferConfidentialConsultationRequestPrivateOffice)
            {
                var cmsApproval = (CmsApprovalTracking)entity;
                ReferenceId = cmsApproval.Id;
            }
            else if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsDMSDocument)
            {
                var template = (DmsAddedDocument)entity;
                ReferenceId = template.Id;
            }
            var currentActivity = await GetInstanceCurrentActivity(ReferenceId);

            if (currentActivity?.BranchId == WorkflowBranch.ConditionalBranch)
            {
                await ProcessWorkflowConditionalBranch(currentActivity, entity, module, TriggerId);
            }
            else
            {
                await ProcessWorkflowContinuousBranch(currentActivity, entity, module, TriggerId);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-09-07' Version="1.0" Branch="master">Process Branch based on defined conditions for the given activity</History>
        protected async Task ProcessWorkflowConditionalBranch(WorkflowActivity currentActivity, dynamic entity, int module, int? TriggerId)
        {
            var activityConditions = await GetWorkflowConditions((int)currentActivity.WorkflowActivityId);
            dynamic Entity = "";
            int StatusId = 0;
            if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsDocument)
            {
                Entity = (LegalLegislation)entity;
                StatusId = Entity.Legislation_Flow_Status;
            }
            else if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsPrinciple)
            {
                Entity = (LLSLegalPrincipleSystem)entity;
                StatusId = Entity.FlowStatus;
            }
            else if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsCaseDraft || TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsConsultationDraft)
            {
                Entity = (CmsDraftedTemplate)entity;
                StatusId = Entity.DraftedTemplateVersion.StatusId;
            }
            else if (TriggerId >= (int)WorkflowModuleTriggerEnum.TransferCaseRequest && TriggerId <= (int)WorkflowModuleTriggerEnum.TransferConfidentialConsultationRequestPrivateOffice)
            {
                Entity = (CmsApprovalTracking)entity;
                StatusId = Entity.StatusId;
            }
            else if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsDMSDocument)
            {
                Entity = (DmsAddedDocument)entity;
                StatusId = Entity.DocumentVersion.StatusId;
            }
            Entity.Token = await browserStorage.GetItemAsync<string>("Token");
            var userDetail = await browserStorage.GetItemAsync<UserDetailVM>("UserDetail");

            foreach (var condition in activityConditions.OrderByDescending(x => x.MKey == WorkflowConditionEnum.CmsDraftDocumentStatusApproveAndViceHOSApprovalEnough.ToString()))
            {
                bool viceHosResponsibilityCheck = false;
                if(String.Equals(condition.MKey, WorkflowConditionEnum.CmsDraftDocumentStatusApproveAndViceHOSApprovalEnough.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    if (StatusId == Convert.ToInt32(condition.ValueToCompare) && userDetail.SectorOnlyViceHOSApprovalEnough)
                    {
                        viceHosResponsibilityCheck = true;
                    }
                    else
                    {
                        continue;
                    }
                }
                if (StatusId == Convert.ToInt32(condition.ValueToCompare) || viceHosResponsibilityCheck)
                {
                    if (condition.TrueCaseFlowControlId == WorkflowControl.JumptoStep)
                    {
                        var trueCaseActivity = await GetWorkflowActivityBySequenceNumber((int)currentActivity.WorkflowId, (int)condition.TrueCaseActivityNo);
                        Type type = typeof(WorkflowImplementationService);
                        System.Reflection.MethodInfo methodInfo = type.GetMethod(trueCaseActivity.Method);
                        if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsDMSDocument)
                        {
                            Entity.IsEndofFlow = (bool)trueCaseActivity.IsEndofFlow;
                        }
                        var activityParams = await GetWorkflowActivityParameters((int)trueCaseActivity.WorkflowActivityId, entity, TriggerId);
                        int arraySize = activityParams.Count + 1;
                        object[] methodParams = new object[arraySize];
                        methodParams[0] = Entity;
                        for (int i = 1; i <= activityParams.Count; i++)
                        {
                            methodParams[i] = activityParams.ToArray()[i - 1];
                        }
                        var response3 = (Task<WorkflowActivityResponse>)methodInfo.Invoke(methodInfo, methodParams);
                        if (response3.Result.Success)
                        {
                        }
                        else
                        {
                            if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsDocument)
                            {
                                await UpdateWorkflowInstanceStatus(Entity.LegislationId, (int)WorkflowInstanceStatusEnum.Failed);
                            }
                            else if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsPrinciple)
                            {
                                await UpdateWorkflowInstanceStatus(Entity.PrincipleId, (int)WorkflowInstanceStatusEnum.Failed);
                            }
                            else if (TriggerId > (int)WorkflowModuleTriggerEnum.UserSubmitsPrinciple)
                            {
                                await UpdateWorkflowInstanceStatus(Entity.Id, (int)WorkflowInstanceStatusEnum.Failed);
                            }
                        }
                        break;
                    }
                    else if (condition.TrueCaseFlowControlId == WorkflowControl.EndofFlow)
                    {
                        Entity.WorkflowInstanceStatusId = WorkflowInstanceStatusEnum.Success;
                        if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsDocument)
                        {
                            await UpdateDocumentInstance(Entity);
                        }
                        else if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsPrinciple)
                        {
                            await UpdatePrincipleInstance(Entity);
                        }
                        else if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsCaseDraft || TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsConsultationDraft)
                        {
                            Entity.IsLawyerTask = condition.IsLawyerTask;
                            await UpdateCaseDraftInstance(Entity);
                        }
                        else if (TriggerId == (int)WorkflowModuleTriggerEnum.TransferCaseRequest || TriggerId == (int)WorkflowModuleTriggerEnum.TransferCaseFile)
                        {
                            await UpdateApprovalTrackingInstance(Entity);

                        }
                        else if (TriggerId == (int)WorkflowModuleTriggerEnum.TransferConsultationRequest || TriggerId == (int)WorkflowModuleTriggerEnum.TransferConsultationFile)
                        {
                            await UpdateApprovalTrackingConsultationInstance(Entity);

                        }
                        else if (TriggerId == (int)WorkflowModuleTriggerEnum.SendCopyCaseRequest || TriggerId == (int)WorkflowModuleTriggerEnum.SendCopyCaseFile)
                        {
                            await UpdateCopyTrackingInstance(Entity);

                        }
                        else if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsDMSDocument)
                        {
                            await UpdateDMSDocumentInstance(Entity);
                        }
                        break;
                    }
                }
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-09-07' Version="1.0" Branch="master">Process Branch based on defined conditions for the given activity</History>
        protected async Task ProcessWorkflowContinuousBranch(WorkflowActivity currentActivity, dynamic entity, int module, int? TriggerId)
        {
            dynamic Entity = "";
            if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsDocument)
            {
                Entity = (LegalLegislation)entity;
                await UpdateDocumentInstance(Entity);
            }
            else if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsPrinciple)
            {
                Entity = (LLSLegalPrincipleSystem)entity;
                await UpdatePrincipleInstance(Entity);
            }
            else if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsCaseDraft || TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsConsultationDraft)
            {
                Entity = (CmsDraftedTemplate)entity;
                await UpdateCaseDraftInstance(Entity);
            }
            else if (TriggerId == (int)WorkflowModuleTriggerEnum.TransferCaseRequest || TriggerId == (int)WorkflowModuleTriggerEnum.TransferCaseFile)
            {
                Entity = (CmsApprovalTracking)entity;
                await UpdateApprovalTrackingInstance(Entity);
            }
            else if (TriggerId == (int)WorkflowModuleTriggerEnum.TransferConsultationRequest || TriggerId == (int)WorkflowModuleTriggerEnum.TransferConsultationFile)
            {
                Entity = (CmsApprovalTracking)entity;
                await UpdateApprovalTrackingConsultationInstance(Entity);

            }
            else if (TriggerId == (int)WorkflowModuleTriggerEnum.SendCopyCaseRequest || TriggerId == (int)WorkflowModuleTriggerEnum.SendCopyCaseFile)
            {
                Entity = (CmsApprovalTracking)entity;
                await UpdateCopyTrackingInstance(Entity);

            }
            else if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsDMSDocument)
            {
                Entity = (DmsAddedDocument)entity;
                await UpdateDMSDocumentInstance(Entity);
            }
            Entity.Token = await browserStorage.GetItemAsync<string>("Token");
            //next workflow activities
            var workflowActivities = await GetToDoWorkflowActivities((int)currentActivity.WorkflowId, (int)currentActivity.SequenceNumber);
            if (workflowActivities?.Count() > 0)
            {
                foreach (var activity in workflowActivities.OrderBy(wa => wa.SequenceNumber))
                {
                    if (activity.CategoryId == (int)WorkflowActivityCategory.GeneralControls)
                    {
                        Type type = typeof(WorkflowImplementationService);
                        System.Reflection.MethodInfo methodInfo = type.GetMethod(activity.Method);

                        var activityParams = await GetWorkflowActivityParameters((int)activity.WorkflowActivityId, entity, TriggerId);
                        int arraySize = activityParams.Count;
                        object[] methodParams = new object[arraySize];
                        for (int i = 0; i < arraySize; i++)
                        {
                            methodParams[i] = activityParams.ToArray()[i];
                        }
                        var response2 = (Task<WorkflowActivityResponse>)methodInfo.Invoke(activity.Method, methodParams);
                        if (response2.Result.Success)
                        {
                            var nextActivities = await GetToDoWorkflowActivities((int)activity.WorkflowId, (int)activity.SequenceNumber);
                            if (nextActivities?.Count() <= 0)
                            {
                                if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsDocument)
                                {
                                    await UpdateWorkflowInstanceStatus(Entity.LegislationId, (int)WorkflowInstanceStatusEnum.Success);
                                }
                                else if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsPrinciple)
                                {
                                    await UpdateWorkflowInstanceStatus(Entity.PrincipleId, (int)WorkflowInstanceStatusEnum.Success);
                                }
                                else if (TriggerId > (int)WorkflowModuleTriggerEnum.UserSubmitsPrinciple)
                                {
                                    await UpdateWorkflowInstanceStatus(Entity.Id, (int)WorkflowInstanceStatusEnum.Success);
                                }

                            }
                        }
                        else
                        {
                            if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsDocument)
                            {
                                await UpdateWorkflowInstanceStatus(Entity.LegislationId, (int)WorkflowInstanceStatusEnum.Failed);
                            }
                            else if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsPrinciple)
                            {
                                await UpdateWorkflowInstanceStatus(Entity.PrincipleId, (int)WorkflowInstanceStatusEnum.Failed);
                            }
                            else if (TriggerId > (int)WorkflowModuleTriggerEnum.UserSubmitsPrinciple)
                            {
                                await UpdateWorkflowInstanceStatus(Entity.Id, (int)WorkflowInstanceStatusEnum.Failed);
                            }
                            break;
                        }
                    }
                    else if (activity.CategoryId == (int)WorkflowActivityCategory.Tasks)
                    {
                        Type type = typeof(WorkflowImplementationService);
                        System.Reflection.MethodInfo methodInfo = type.GetMethod(activity.Method);

                        var activityParams = await GetWorkflowActivityParameters((int)activity.WorkflowActivityId, entity, TriggerId);
                        int arraySize = activityParams.Count + 1;
                        object[] methodParams = new object[arraySize];
                        methodParams[0] = Entity;
                        for (int i = 1; i <= activityParams.Count; i++)
                        {
                            methodParams[i] = activityParams.ToArray()[i - 1];
                        }
                        var response3 = (Task<WorkflowActivityResponse>)methodInfo.Invoke(methodInfo, methodParams);
                        if (response3.Result.Success)
                        {
                        }
                        else
                        {
                            if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsDocument)
                            {
                                await UpdateWorkflowInstanceStatus(Entity.LegislationId, (int)WorkflowInstanceStatusEnum.Failed);
                            }
                            else if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsPrinciple)
                            {
                                await UpdateWorkflowInstanceStatus(Entity.PrincipleId, (int)WorkflowInstanceStatusEnum.Failed);
                            }
                            else if (TriggerId > (int)WorkflowModuleTriggerEnum.UserSubmitsPrinciple)
                            {
                                await UpdateWorkflowInstanceStatus(Entity.Id, (int)WorkflowInstanceStatusEnum.Failed);
                            }
                        }
                        break;
                    }
                }
            }
            else
            {
                if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsDocument)
                {
                    await UpdateWorkflowInstanceStatus(Entity.LegislationId, (int)WorkflowInstanceStatusEnum.Success);
                }
                else if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsPrinciple)
                {
                    await UpdateWorkflowInstanceStatus(Entity.PrincipleId, (int)WorkflowInstanceStatusEnum.Success);
                }
                else if (TriggerId > (int)WorkflowModuleTriggerEnum.UserSubmitsPrinciple)
                {
                    await UpdateWorkflowInstanceStatus(Entity.Id, (int)WorkflowInstanceStatusEnum.Success);
                }
            }
        }

        #endregion

        #region Process Workflow Condition Option
        //<History Author = 'Muhamamd Zaeem' Date='2023-01-11' Version="1.0" Branch="master"> Get Workflow Condition Options</History>
        public async Task<ApiCallResponse> GetWorkflowConditionOptions(Guid ReferneceId, int StatusId)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetWorkflowConditionOptions?ReferneceId=" + ReferneceId + "&StatusId=" + StatusId);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<WorkflowConditionsOptionVM>>();
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
        //<History Author = 'Muhamamd Zaeem' Date='2023-01-11' Version="1.0" Branch="master"> Process Workflow Option Activites</History>

        public async Task ProcessWorkflowOptionActivites(dynamic selectedOption, dynamic entity, int module, int triggerId, bool IsTriggerConditionOption = false)
        {
            dynamic Entity = "";
            dynamic SelectedOption = "";
            if (triggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsCaseDraft || triggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsConsultationDraft)
            {
                Entity = (CmsDraftedTemplate)entity;
                if (IsTriggerConditionOption)
                    SelectedOption = (WorkflowTriggerConditionOptionsVM)selectedOption;
                else
                    SelectedOption = (WorkflowConditionsOptionVM)selectedOption;
            }
            else if (triggerId >= (int)WorkflowModuleTriggerEnum.TransferCaseRequest && triggerId <= (int)WorkflowModuleTriggerEnum.TransferConfidentialConsultationRequestPrivateOffice)
            {
                Entity = (CmsApprovalTracking)entity;
                if (IsTriggerConditionOption)
                    SelectedOption = (WorkflowTriggerConditionOptionsVM)selectedOption;
                else
                    SelectedOption = (WorkflowActivityOptionVM)selectedOption;
            }
            Entity.Token = await browserStorage.GetItemAsync<string>("Token");

            if (SelectedOption.TrueCaseFlowControlId == WorkflowControl.JumptoStep)
            {
                var trueCaseActivity = await GetWorkflowActivityBySequenceNumber((int)SelectedOption.WorkflowId, (int)SelectedOption.TrueCaseActivityNo);
                Type type = typeof(WorkflowImplementationService);
                System.Reflection.MethodInfo methodInfo = type.GetMethod(trueCaseActivity.Method);
                var activityParams = await GetWorkflowActivityParameters((int)trueCaseActivity.WorkflowActivityId, entity, triggerId);
                int arraySize = activityParams.Count + 1;
                object[] methodParams = new object[arraySize];
                methodParams[0] = Entity;
                for (int i = 1; i <= activityParams.Count; i++)
                {
                    methodParams[i] = activityParams.ToArray()[i - 1];
                }
                var response3 = (Task<WorkflowActivityResponse>)methodInfo.Invoke(methodInfo, methodParams);
                if (response3.Result.Success)
                {
                }
                else
                {
                    if (triggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsDocument)
                    {
                        await UpdateWorkflowInstanceStatus(Entity.LegislationId, (int)WorkflowInstanceStatusEnum.Failed);
                    }
                    else if (triggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsPrinciple)
                    {
                        await UpdateWorkflowInstanceStatus(Entity.PrincipleId, (int)WorkflowInstanceStatusEnum.Failed);
                    }
                    else if (triggerId > (int)WorkflowModuleTriggerEnum.UserSubmitsPrinciple)
                    {
                        await UpdateWorkflowInstanceStatus(Entity.Id, (int)WorkflowInstanceStatusEnum.Failed);
                    }
                }
            }
            else if (SelectedOption.TrueCaseFlowControlId == WorkflowControl.EndofFlow)
            {
                Entity.WorkflowInstanceStatusId = WorkflowInstanceStatusEnum.Success;
                if (triggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsDocument)
                {
                    await UpdateDocumentInstance(Entity);
                }
                else if (triggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsPrinciple)
                {
                    await UpdatePrincipleInstance(Entity);
                }
                else if (triggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsCaseDraft || triggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsConsultationDraft)
                {
                    await UpdateCaseDraftInstance(Entity);
                }
                else if (triggerId == (int)WorkflowModuleTriggerEnum.TransferCaseRequest || triggerId == (int)WorkflowModuleTriggerEnum.TransferCaseFile)
                {
                    await UpdateApprovalTrackingInstance(Entity);
                }
                else if (triggerId == (int)WorkflowModuleTriggerEnum.TransferConsultationRequest || triggerId == (int)WorkflowModuleTriggerEnum.TransferConsultationFile)
                {
                    await UpdateApprovalTrackingConsultationInstance(Entity);
                }
                else if (triggerId == (int)WorkflowModuleTriggerEnum.SendCopyCaseRequest || triggerId == (int)WorkflowModuleTriggerEnum.SendCopyCaseFile)
                {
                    await UpdateCopyTrackingInstance(Entity);
                }
            }
        }

        #endregion

        #region Cms Update Transfer Instance

        //<History Author = 'Muhamamd Zaeem' Date='2023-01-11' Version="1.0" Branch="master"> Cms Update trasnfer tracking Instance</History>
        public async Task<bool> UpdateApprovalTrackingInstance(CmsApprovalTracking cmsApprovalTracking)
        {
            cmsApprovalTracking.ModifiedDate = DateTime.Now;
            var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Workflow/UpdateApprovalTrackingInstance");
            var postBody = cmsApprovalTracking;
            request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", cmsApprovalTracking.Token);
            var response = await new HttpClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        #endregion

        #region Cms Update Copy  Instance

        //<History Author = 'Muhamamd Zaeem' Date='2023-01-11' Version="1.0" Branch="master"> Cms Update Copy tracking Instance</History>
        public async Task<bool> UpdateCopyTrackingInstance(CmsApprovalTracking cmsApprovalTracking)
        {
            cmsApprovalTracking.ModifiedDate = DateTime.Now;
            var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Workflow/UpdateCopyTrackingInstance");
            var postBody = cmsApprovalTracking;
            request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", cmsApprovalTracking.Token);
            var response = await new HttpClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }
        //<History Author = 'Muhamamd Zaeem' Date='2023-01-11' Version="1.0" Branch="master"> Cms Update approved Copy tracking Instance</History>

        public async Task<bool> UpdateCopyApprovedTrackingInstance(CmsApprovalTracking cmsApprovalTracking)
        {
            cmsApprovalTracking.ModifiedDate = DateTime.Now;
            var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Workflow/UpdateCopyApprovedTrackingInstance");
            var postBody = cmsApprovalTracking;
            request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", cmsApprovalTracking.Token);
            var response = await new HttpClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
                return true;
            else
                return false;
        }

        #endregion

        #region Consultation Update Transfer Instance

        //<History Author = 'Muhamamd Zaeem' Date='2023-01-11' Version="1.0" Branch="master"> Consultation Update Transfer Instance</History>
        public async Task<bool> UpdateApprovalTrackingConsultationInstance(CmsApprovalTracking cmsApprovalTracking)
        {
            try
            {
                cmsApprovalTracking.ModifiedDate = DateTime.Now;
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Workflow/UpdateApprovalTrackingConsultationInstance");
                var postBody = cmsApprovalTracking;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", cmsApprovalTracking.Token);
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        #endregion

        #region Get Workflow Activity Options
        //<History Author = 'Muhamamd Zaeem' Date='2023-01-12' Version="1.0" Branch="master">Get Workflow Activity Options by activity id </History>

        public async Task<ApiCallResponse> GetWorkflowActivityOptionsByActivityId(int ActivityId)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetWorkflowActivityOptions?ActivityId=" + ActivityId);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<WorkflowActivityOptionVM>>();
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

        #region Get Current Instance By Reference Id 
        //<History Author = 'Muhamamd Zaeem' Date='2023-01-12' Version="1.0" Branch="master"> Get Current Instance By Reference Id </History>

        public async Task<ApiCallResponse> GetCurrentInstanceByReferneceId(Guid referenceId)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetCurrentInstanceByReferneceId?referenceId=" + referenceId);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<WorkflowInstance>();
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

        #region Get Workflow Sector Transfer Options
        //<History Author = 'Muhamamd Zaeem' Date='2023-01-12' Version="1.0" Branch="master"> Get Workflow Sector Transfer Options</History>
        public async Task<ApiCallResponse> GetWorkflowSectorTransferOptions(int workflowTriggerId, int sectorTypeId)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetWorkflowSectorTransferOptions?workflowTriggerId=" + workflowTriggerId + "&sectorTypeId=" + sectorTypeId);
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

        #region Get Workflow Trigger Conditions by trigger id
        //<History Author = 'Muhamamd Zaeem' Date='2023-01-12' Version="1.0" Branch="master"> Get Workflow Trigger Conditions by trigger id</History>
        public async Task<ApiCallResponse> GetWorkflowTriggerConditionsByTriggerId(int TriggerId)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetWorkflowTriggerConditionsByTriggerId?TriggerId=" + TriggerId);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<WorkflowTriggerCondition>>();
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

        #region Get workflow trigger sector  options
        //<History Author = 'Muhamamd Zaeem' Date='2023-01-12' Version="1.0" Branch="master"> Get workflow trigger sector  options</History>

        public async Task<ApiCallResponse> GetWorkflowTriggerSectorOptions(int TriggerId)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetWorkflowTriggerSectorOptions?TriggerId=" + TriggerId);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<WorkflowTriggerSectorOptions>>();
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

        #region Get workflow trigger sector transfer options
        //<History Author = 'Muhamamd Zaeem' Date='2023-01-12' Version="1.0" Branch="master"> Get workflow trigger sector transfer options</History>

        public async Task<ApiCallResponse> GetWorkflowTriggerSectorTransferOptions(int TriggerOptionId)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetWorkflowTriggerSectorTransferOptions?TriggerOptionId=" + TriggerOptionId);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<int>>();
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

        #region Get activty Slas by activity Id
        //<History Author = 'Muhamamd Zaeem' Date='2023-01-12' Version="1.0" Branch="master"> Get activty Slas by activity Id</History>
        public async Task<ApiCallResponse> GetActivtySlAsByActivityId(int WorkflowActivityId)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetActivtySlAsByActivityId?WorkflowActivityId=" + WorkflowActivityId);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<SLA>>();
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

        #region Get Activity SLAs action paramater
        //<History Author = 'Muhamamd Zaeem' Date='2023-01-12' Version="1.0" Branch="master"> Get Activty SLAs Action Parameter By SLAId</History>

        public async Task<ApiCallResponse> GetActivtySLAsActionParameterBySLAId(int WorkflowSLAId)
        {
            try
            {
                var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetActivtySLAsActionParameterBySLAId?WorkflowSLAId=" + WorkflowSLAId);
                request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request2);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<SLAActionParameters>>();
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

        #region Get Next Worrkflow Activity
        public async Task<ApiCallResponse> GetNextWorrkflowActivity(Guid draftId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetNextWorrkflowActivity?draftId=" + draftId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<int>();
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
        #region Workflow Trigger Condition Options
        public async Task<IEnumerable<WorkflowTriggerConditionOptionsVM>> GetWorkflowTriggerConditionsOptions(int TriggerConditionId, Guid ReferenceId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Workflow/GetWorkflowTriggerConditionsOptions?TriggerConditionId=" + TriggerConditionId + "&ReferenceId=" + ReferenceId);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request);
            if (response.IsSuccessStatusCode)
                return await response.Content.ReadFromJsonAsync<IEnumerable<WorkflowTriggerConditionOptionsVM>>();
            else
                return null;
        }
        #endregion
    }
}
