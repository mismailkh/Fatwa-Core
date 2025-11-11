using Blazored.LocalStorage;
using FATWA_ADMIN.Data;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.EntityFrameworkCore;
using Radzen;
using System.Linq.Dynamic.Core;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;
using static FATWA_GENERAL.Helper.Response;
using static Org.BouncyCastle.Math.EC.ECCurve;
using Group = FATWA_DOMAIN.Models.AdminModels.UserManagement.Group;

namespace FATWA_ADMIN.Services.UserManagement
{
    public partial class UserService
    {
        private readonly IConfiguration config;
        private readonly ILocalStorageService browserStorage;
        private readonly TranslationState translationState;
        public string ResultDetails { get; private set; }


        public UserService(IConfiguration _config, ILocalStorageService _browserStorage, TranslationState _translationState)
        {
            config = _config;
            browserStorage = _browserStorage;
            translationState = _translationState;
        }

        //<History Author = 'Aqeel Altaf' Date='2022-08-09' Version="1.0" Branch="master">Get user detail by id</History>
        public async Task<ApiCallResponse> GetSecurityStampByEmail(string emailId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, config.GetValue<string>("api_url") + "/Account/GetSecurityStampByEmail?emailId=" + emailId);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request);
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

        #region User List
        partial void OnUserDetailsRead(ref IQueryable<UserVM> items);
        public async Task<IQueryable<UserVM>> GetUserDetails()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, config.GetValue<string>("api_url") + "/Account/GetUsersList");
                // add authorization header
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                // send request
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(translationState.Translate("Contact_Administrator"));
                }
                var responselist = response.Content.ReadFromJsonAsync<IEnumerable<UserVM>>();
                var queryableX = (await responselist).AsQueryable();
                return queryableX;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Record User Logout Activity In Database
        public async Task<ApiCallResponse> RecordUserLogoutActivity(string username, string userId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, config.GetValue<string>("api_url") + "/Users/RecordUserLogoutActivity?username=" + username + "&userId=" + userId);

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

        #region FATWA Employees List
        public async Task<ApiCallResponse> GetEmployeesListForAdmin(int EmployeeTypeId, int? SectorTypeId, int? DesignationId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, config.GetValue<string>("api_url") + "/Users/GetEmployeesListForAdmin?EmployeeTypeId="+EmployeeTypeId+"&SectorTypeId="+SectorTypeId+"&DesignationId="+DesignationId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<IEnumerable<EmployeesListVM>>();
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
        public async Task<ApiCallResponse> GetEmployeesListForUserGroup(UserListAdvanceSearchVM advanceSearchVM)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, config.GetValue<string>("api_url") + "/Users/GetEmployeesListForUserGroup");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                request.Content = new StringContent(System.Text.Json.JsonSerializer.Serialize(advanceSearchVM), Encoding.UTF8, "application/json");
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<IEnumerable<EmployeesListDropdownVM>>();
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

        #region Assign Role To Employee
        public async Task<ApiCallResponse> SaveEmployeeRole(UserRoleAssignmentVM userRoleAssignmentVM)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, config.GetValue<string>("api_url") + "/Users/SaveEmployeeRole");
                var postBody = userRoleAssignmentVM;
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

        #region Assign Claims To Users
        public async Task<ApiCallResponse> SaveUserClaims(UserClaimsVM userClaimsVM)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, config.GetValue<string>("api_url") + "/Users/SaveUserClaims");
                var postBody = userClaimsVM;
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
        public async Task<ApiCallResponse> AllowBulkDigitalSign(EmployeeVMForDropDown data)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, config.GetValue<string>("api_url") + "/Users/AllowBulkDigitalSign");
                var postBody = data;
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

        #region Get Employee Details
        public async Task<ApiCallResponse> GetEmployeeDetailById(Guid Id)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, config.GetValue<string>("api_url") + "/Users/GetEmployeeDetailById?Id=" + Id);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var emploee = await response.Content.ReadFromJsonAsync<AddEmployeeVM>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = emploee };
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

        #region Get System Roles
        public async Task<ApiCallResponse> GetRoles()
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, config.GetValue<string>("api_url") + "/Roles/GetRoleData");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode };
                }
                else if (response.IsSuccessStatusCode)
                {
                    var roles = await response.Content.ReadFromJsonAsync<List<Role>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = roles };
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


        //< History Author = "Ammaar Naveed" Date = "03/07/2024" Version = "1.0" Branch = "master" >Get lawyer manager by sector type</ History >
        public async Task<ApiCallResponse> GetManagersList(int? SectorTypeId, int DesignationId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, config.GetValue<string>("api_url") + "/Users/GetManagersList?SectorTypeId=" + SectorTypeId + "&DesignationId=" + DesignationId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<IEnumerable<ManagersListVM>>();
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

        #region Update Default Correspondence Receiver Status
        //<History Author='Ammaar Naveed' Date='30-07-2024'>Update default correspondence receiver status.</History>// 
        public async Task<ApiCallResponse> UpdateDefaultReceiverStatus(bool isDefaultCorrespondenceReceiver, string userId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Post, config.GetValue<string>("api_url") + "/Users/UpdateDefaultReceiverStatus?isDefaultCorrespondenceReceiver=" + isDefaultCorrespondenceReceiver + "&userId=" + userId);

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

        #region Get Designations List
        public async Task<ApiCallResponse> GetDesignationsList()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, config.GetValue<string>("api_url") + "/Users/GetDesignations");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var designations = await response.Content.ReadFromJsonAsync<List<Designation>>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = designations };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }

        }
        #endregion

        #region Get Employees By Designation Id
        //< History Author = "Ammaar Naveed" Date = "08/08/2024" Version = "1.0" Branch = "master" >Get employees by designation Id</ History>
        public async Task<ApiCallResponse> GetEmployeesByDesignationId(int? DesignationId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, config.GetValue<string>("api_url") + "/Users/GetEmployeesByDesignationId?DesignationId=" + DesignationId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var designations = await response.Content.ReadFromJsonAsync<IEnumerable<UserClaimsVM>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = designations };
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

        #region Get UMS Claims List
        //< History Author = "Ammaar Naveed" Date = "08/08/2024" Version = "1.0" Branch = "master" >Get UMS claims by module Id</ History>
        public async Task<ApiCallResponse> GetUmsClaimsByModuleId(int moduleId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, config.GetValue<string>("api_url") + "/Users/GetUmsClaimsByModuleId?moduleId=" + moduleId);
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                var claims = await response.Content.ReadFromJsonAsync<List<UserClaimsVM>>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = claims };
            }
            else
            {
                var content = await response.Content.ReadFromJsonAsync<BadRequestResponse>();
                return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = content };
            }

        }
        #endregion

        #region Get Sector Types List
        public async Task<ApiCallResponse> GetSectorTypes()
        {
            try
            {
                var operatingSectorsRequest = new HttpRequestMessage(HttpMethod.Get, config.GetValue<string>("api_url") + "/Users/GetEmployeeSectortype");
                operatingSectorsRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(operatingSectorsRequest);
                if (response.IsSuccessStatusCode)
                {
                    var operatingSectorTypes = await response.Content.ReadFromJsonAsync<List<OperatingSectorType>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = operatingSectorTypes };
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

        #region Get Employee Types
        //<History Author = 'Ammaar Naveed' Date='2024-10-16' Version="1.0" Branch="master">Get employee types.</History>
        public async Task<ApiCallResponse> GetEmployeeTypes()
        {
            try
            {
                var employeeTypesRequest = new HttpRequestMessage(HttpMethod.Get, config.GetValue<string>("api_url") + "/Users/GetEmployeeType");
                employeeTypesRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(employeeTypesRequest);
                if (response.IsSuccessStatusCode)
                {
                    var employeeTypes = await response.Content.ReadFromJsonAsync<List<EmployeeType>>();
                    return new ApiCallResponse { StatusCode = response.StatusCode, IsSuccessStatusCode = response.IsSuccessStatusCode, ResultData = employeeTypes };
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

        public async Task<ApiCallResponse> GetUserRoles(string userName)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, config.GetValue<string>("api_url") + "/Account/GetUserRolesByUserName?userName=" + userName);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<List<UserRole>>();
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
        #region Get Users List For Mention
        public async Task<ApiCallResponse> GetUsersListForMention(Guid TicketId, string LoggedInUserEmail)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, config.GetValue<string>("api_url") + "/Users/GetUsersListForMention?TicketId=" + TicketId + "&LoggedInUserEmail=" + LoggedInUserEmail);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<IEnumerable<UserListMentionVM>>();
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
                return new ApiCallResponse { StatusCode = HttpStatusCode.BadRequest, IsSuccessStatusCode = false, ResultData = new BadRequestResponse { InnerException = ex.InnerException.Message, Message = ex.Message } };
            }
        }
        #endregion
    }
}
