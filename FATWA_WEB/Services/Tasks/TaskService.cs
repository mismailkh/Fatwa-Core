using System.Data.Entity;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using Blazored.LocalStorage;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Dms;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_WEB.Data;
using Radzen;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Services.Tasks
{
    public class TaskService
    {
        private readonly IConfiguration _config;
        private readonly ILocalStorageService _browserStorage;
        public TaskService(IConfiguration configuration, ILocalStorageService browserStorage)
        {
            _config = configuration;
            _browserStorage = browserStorage;
        }

        #region Get  
        public async Task<ApiCallResponse> GetTasksList(AdvanceSearchTaskVM advanceSearchVM)
        {
            try
            {
                var user = await _browserStorage.GetItemAsync<UserDetailVM>("UserDetail");
                advanceSearchVM.UserId = user.UserId;

                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Task/GetTasksList");
                var postBody = advanceSearchVM;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                return await response.Content.ReadFromJsonAsync<ApiCallResponse>();
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }

        public async Task<ApiCallResponse> GetTaskById(Guid taskId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Task/GetTaskById?taskId=" + taskId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<SaveTaskVM>();
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

        public async Task<ApiCallResponse> GetTaskDetailById(Guid taskId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Task/GetTaskDetailById?taskId=" + taskId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<TaskDetailVM>();
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

        public async Task<ApiCallResponse> GetTaskDetailByReferenceAndUserId(Guid referenceId, string userId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Task/GetTaskDetailByReferenceAndUserId?referenceId=" + referenceId + "&userId=" + userId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    var content = new TaskDetailVM();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<TaskDetailVM>(); ;
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
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

        public async Task<ApiCallResponse> GetTaskDashBoard()
        {
            // History HAssan Iftikhar
            try
            {
                var user = await _browserStorage.GetItemAsync<UserDetailVM>("UserDetail");
                var item = user.UserId;

                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Task/GetTaskDashBoard?item=" + item);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<TaskDashboardVM>();
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
        public async Task<ApiCallResponse> GetDraftList(AdvanceSearchDraftVM advanceSearchVM)
        {
            // History HAssan Iftikhar
            try
            {


                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Task/GetDraftList");
                var postBody = advanceSearchVM;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<DraftListVM>>();
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

        public async Task<ApiCallResponse> GetMaxTaskNumber()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Task/GetMaxTaskNumber");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
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

        public async Task<ApiCallResponse> GetTaskDetailByReferenceAndAssignedToId(Guid referenceId, string userId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Task/GetTaskDetailByReferenceAndAssignedToId?referenceId=" + referenceId + "&userId=" + userId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<UserTask>();
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

        #region Save

        public async Task<ApiCallResponse> AddTask(SaveTaskVM task)
        {
            try
            {
                task.Task.CreatedBy = await _browserStorage.GetItemAsync<string>("User");
                task.Task.CreatedDate = DateTime.Now;

                task.Task.IsSystemGenerated = false;
                task.Task.AssignedBy = task.Task.CreatedBy;
                task.Task.IsDeleted = false;

                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Task/AddTask");
                var postBody = task;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
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
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }

        public async Task<ApiCallResponse> EditTask(SaveTaskVM task)
        {
            try
            {
                task.Task.ModifiedBy = await _browserStorage.GetItemAsync<string>("User");
                task.Task.ModifiedDate = DateTime.Now;

                task.Task.AssignedBy = task.Task.ModifiedBy;
                task.Task.IsDeleted = false;

                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Task/EditTask");
                var postBody = task;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
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
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }

        public async Task<ApiCallResponse> SaveToDoList(TaskDashboardVM toDo)
        {
            // History HAssan Iftikhar
            try
            {
                toDo.User = await _browserStorage.GetItemAsync<string>("User");
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Task/SaveToDoList");
                var postBody = toDo;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
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

                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }

        public async Task<ApiCallResponse> DecisionTask(TaskDetailVM task)
        {
            task.ModifiedBy = await _browserStorage.GetItemAsync<string>("User");

            var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Task/DecisionTask");
            var postBody = task;
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
        public async Task<ApiCallResponse> DecisionTaskByStatusAndRefId(TaskDetailVM task)
        {
            task.ModifiedBy = await _browserStorage.GetItemAsync<string>("User");

            var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Task/DecisionTaskByStatusAndRefId");
            var postBody = task;
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

        public async Task<ApiCallResponse> SaveCaseAssignment(TaskDetailVM task)
        {
            task.ModifiedBy = await _browserStorage.GetItemAsync<string>("User");

            var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Task/SaveCaseAssignment");
            var postBody = task;
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
        public async Task<ApiCallResponse> SaveConsultationAssignment(TaskDetailVM task)
        {
            task.ModifiedBy = await _browserStorage.GetItemAsync<string>("User");

            var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Task/SaveConsultationAssignment");
            var postBody = task;
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


        public async Task<ApiCallResponse> NotifyTaskAssignedBy(TaskDetailVM task)
        {
            task.ModifiedBy = await _browserStorage.GetItemAsync<string>("User");

            var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Task/NotifyTaskAssignedBy");
            var postBody = task;
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
        #endregion

        #region Consultation Task Rejection
        public async Task<ApiCallResponse> ConsultationTaskRejection(TaskDetailVM task)
        {
            task.ModifiedBy = await _browserStorage.GetItemAsync<string>("User");

            var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Task/ConsultationTaskRejection");
            var postBody = task;
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
        #endregion

        #region Delete Draft

        // History HAssan Iftikhar
        public async Task<ApiCallResponse> SoftDeleteDraft(DraftListVM draftListVM)
        {
            try
            {
                draftListVM.DeletedBy = await _browserStorage.GetItemAsync<string>("User");

                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Task/SoftDeleteDraft");
                var postBody = draftListVM;
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

        #endregion

        #region Save Reason For Rejection

        public async Task<ApiCallResponse> RejectReason(RejectReason RejectReason)
        {
            try
            {
                RejectReason.CreatedBy = await _browserStorage.GetItemAsync<string>("User");

                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Task/RejectReason");
                var postBody = RejectReason;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
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
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }

        }

        #endregion

        #region Get Task Response Status

        public async Task<ApiCallResponse> GetTaskResponseStatus()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Task/GetTaskResponseStatus");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == HttpStatusCode.NoContent)
                {
                    var content = new List<TaskResponseStatus>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
                }
                else if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<TaskResponseStatus>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
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
        #endregion

        #region Task Response

        public async Task<ApiCallResponse> AddTaskResponseDecision(TaskResponseVM task)
        {
            task.TaskResponse.CreatedBy = await _browserStorage.GetItemAsync<string>("User");
            task.TaskResponse.CreatedDate = DateTime.Now;
            task.TaskResponse.IsDeleted = false;

            var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Task/AddTaskResponseDecision");
            var postBody = task;
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

        public async Task<ApiCallResponse> EditTaskResponseDecision(TaskResponseVM task)
        {
            task.TaskResponse.ModifiedBy = await _browserStorage.GetItemAsync<string>("User");
            task.TaskResponse.ModifiedDate = DateTime.Now;
            task.TaskResponse.IsDeleted = false;

            var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Task/EditTaskResponseDecision");
            var postBody = task;
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

        #endregion

        #region Get Task Actions By TaskId
        public async Task<ApiCallResponse> GetTaskActionsByTaskId(Guid taskId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Task/GetTaskActionsByTaskId?taskId=" + taskId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<TaskAction>>();
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

        #region Cms Tasks
        //<History Author = "Hassan Abbas" Date="2023-03-08" Version="1.0" Branch="master">List of Tasks related to Case Management filtered based on screen, task type and submodule</History>
        public async Task<ApiCallResponse> GetCmsTasksList(AdvanceSearchTaskVM advanceSearchVM)
        {
            try
            {
                var user = await _browserStorage.GetItemAsync<UserDetailVM>("UserDetail");
                advanceSearchVM.UserId = user.UserId;

                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Task/GetCmsTasksList");
                var postBody = advanceSearchVM;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<TaskVM>>();
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



        //<History Author = "Hassan Abbas" Date="2023-04-18" Version="1.0" Branch="master">List of Tasks related to Case Management</History>

        //<History Author = "Hassan Abbas" Date="2023-04-18" Version="1.0" Branch="master">List of Tasks related to Case Management</History>
        public async Task<ApiCallResponse> GetAllCmsTasks(AdvanceSearchTaskVM advanceSearchVM)
        {
            try
            {
                var user = await _browserStorage.GetItemAsync<UserDetailVM>("UserDetail");
                advanceSearchVM.UserId = user.UserId;
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Task/GetAllCmsTasks");
                var postBody = advanceSearchVM;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                return await response.Content.ReadFromJsonAsync<ApiCallResponse>();
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }

        //<History Author = "Hassan Abbas" Date="2023-03-08" Version="1.0" Branch="master">Count of Tasks related to Case Management filtered based on screen, task type and submodule</History>
        public async Task<ApiCallResponse> GetCountCmsTasksList(AdvanceSearchTaskVM advanceSearchVM)
        {
            try
            {
                var user = await _browserStorage.GetItemAsync<UserDetailVM>("UserDetail");
                advanceSearchVM.UserId = user.UserId;

                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Task/GetCountCmsTasksList");
                var postBody = advanceSearchVM;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
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

        #region Consultation Tasks

        //<History Author = "Hassan Abbas" Date="2023-03-08" Version="1.0" Branch="master">List of Tasks related to Case Management filtered based on screen, task type and submodule</History>
        public async Task<IQueryable<TaskVM>> GetComsTasksList(AdvanceSearchTaskVM advanceSearchVM, Query query = null)
        {
            try
            {
                var response = await GetComsTasksList(advanceSearchVM);
                if (response.IsSuccessStatusCode)
                {
                    var res = (List<TaskVM>)response.ResultData;
                    var items = res.AsQueryable();

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
                    return await Task.FromResult(items);
                }

                return null;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //<History Author = "Hassan Abbas" Date="2023-03-08" Version="1.0" Branch="master">List of Tasks related to Case Management filtered based on screen, task type and submodule</History>
        public async Task<ApiCallResponse> GetComsTasksList(AdvanceSearchTaskVM advanceSearchVM)
        {
            try
            {
                var user = await _browserStorage.GetItemAsync<UserDetailVM>("UserDetail");
                advanceSearchVM.UserId = user.UserId;

                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Task/GetComsTasksList");
                var postBody = advanceSearchVM;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<TaskVM>>();
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

        #region Get Count Consultation Tasks List 
        public async Task<ApiCallResponse> GetCountComsTasksList(AdvanceSearchTaskVM advanceSearchVM)
        {
            try
            {
                var user = await _browserStorage.GetItemAsync<UserDetailVM>("UserDetail");
                advanceSearchVM.UserId = user.UserId;

                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Task/GetCountComsTasksList");
                var postBody = advanceSearchVM;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
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

        #region Get All COnsultation TAsk List
        public async Task<ApiCallResponse> GetAllComsTasks(AdvanceSearchTaskVM advanceSearchVM)
        {
            try
            {
                var user = await _browserStorage.GetItemAsync<UserDetailVM>("UserDetail");
                advanceSearchVM.UserId = user.UserId;

                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Task/GetAllComsTasks");
                var postBody = advanceSearchVM;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                return await response.Content.ReadFromJsonAsync<ApiCallResponse>();
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        #endregion

        #region Get All Case/Consultation Task List
        public async Task<ApiCallResponse> GetAllCMSComsTasks(AdvanceSearchTaskVM advanceSearchVM)
        {
            try
            {
                var user = await _browserStorage.GetItemAsync<UserDetailVM>("UserDetail");
                advanceSearchVM.UserId = user.UserId;
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Task/GetAllCMSComsTasks");
                var postBody = advanceSearchVM;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                return await response.Content.ReadFromJsonAsync<ApiCallResponse>();
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        #endregion

        #region Add Task

        //<History Author = 'Hassan Abbas' Date='2023-06-21' Version="1.0" Branch="master">Add Document </History>
        public async Task<ApiCallResponse> AddSystemGeneratedTask(SaveTaskVM task, string action, string entityName, string entityId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Task/AddSystemGeneratedTask?action=" + action + "&entityName=" + entityName + "&entityId=" + entityId);
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(task), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return new ApiCallResponse { IsSuccessStatusCode = response.IsSuccessStatusCode, StatusCode = response.StatusCode };
                }
                else
                {
                    var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                    return new ApiCallResponse { IsSuccessStatusCode = response.IsSuccessStatusCode, StatusCode = response.StatusCode, ResultData = content };
                }
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }

        #endregion

        #region Task Update with service
        public async Task<ApiCallResponse> TaskUpdateWithService(Guid ReferenceId)
        {
            try
            {
                var UserId = await _browserStorage.GetItemAsync<string>("User");
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Task/TaskUpdateWithService?ReferenceId=" + ReferenceId + "&UserId=" + UserId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
                }
                else if (response.IsSuccessStatusCode)
                {
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
                }
                else
                {
                    var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
                }

            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        #endregion

        #region Complete All Pending Tasks
        public async Task<ApiCallResponse> CompleteAllPendingTasks(Guid RequestId)
        {
            var user = await _browserStorage.GetItemAsync<string>("User");
            var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Task/CompleteAllPendingTasks?RequestId=" + RequestId + "&User=" + user);
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
        #endregion

        #region Complete Assign Task
        public async Task<ApiCallResponse> CompleteAssignTask(Guid FileId, string token)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Task/CompleteAssignTask?FileId=" + FileId);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
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
        #endregion

        #region Get Task Detail By File Id
        public async Task<ApiCallResponse> GetTaskDetailByFileId(Guid FileId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Task/GetTaskDetailByFileId?taskId=" + FileId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<UserTask>();
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

        #region Approve Task By Referenece Id

        public async Task<ApiCallResponse> ApproveTaskByReferenceId(Guid ReferenceId, bool IsViceHos)
        {
            try
            {
                var ModifiedBy = await _browserStorage.GetItemAsync<string>("User");
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Task/ApproveTaskByReferenceId?MeetingId=" + ReferenceId + "&ModifiedBy=" + ModifiedBy + "&IsViceHos=" + IsViceHos);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
                }
                else
                {
                    var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
                }
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        #endregion
        #region Get Task Entity History By ReferenceId And Submodule

        public async Task<ApiCallResponse> GetTaskEntityHistoryByReferenceIdAndSubmodule(Guid referenceId, int submoduleId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Task/GetTaskEntityHistoryByReferenceIdAndSubmodule?referenceId=" + referenceId + "&submoduleId=" + submoduleId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<TaskEntityHistoryVM>>();
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

        public async Task<ApiCallResponse> AddOrUpdateUserTaskViewTime(string userId, Guid? referenceId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Task/AddOrUpdateUserTaskViewTime?userId=" + userId + "&referenceId=" + referenceId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
                }
                else
                {
                    var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
                }
            }
            catch (Exception ex)
            {
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        #region Add Task List
        public async Task<ApiCallResponse> AddTaskList(List<SaveTaskVM> task)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/Task/AddTaskList");
                var postBody = task;
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await _browserStorage.GetItemAsync<string>("Token"));
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
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { Message = ex.Message, InnerException = ex.InnerException?.Message } };
            }
        }
        #endregion

    }
}
