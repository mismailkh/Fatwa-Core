using Blazored.LocalStorage;
using DMS_WEB.Data;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using Microsoft.AspNetCore.Identity;
using System.Net.Http.Headers;
using static FATWA_GENERAL.Helper.Response;

namespace DMS_WEB.Services
{
    public class UserService
    {
        private readonly IConfiguration _config;
        private readonly ILocalStorageService browserStorage;
        private readonly TranslationState translationState;

        public UserService(IConfiguration config, ILocalStorageService _browserStorage, TranslationState _translationState)
        {
            _config = config;
            browserStorage = _browserStorage;
            translationState = _translationState;
        }

        public async Task<IEnumerable<IdentityUser>> GetAllUsers()
        {
            try
            {
                return await new HttpClient().GetFromJsonAsync<IdentityUser[]>(_config.GetValue<string>("fatwa_api_url") + "/Account/UserList");
                //  return await httpClient.GetFromJsonAsync<IdentityUser[]>("api/Account/UserList"); //Okay
            }
            catch (Exception ex)
            {
                throw new Exception("Record not found", ex);
            }

        }

        public async Task<ApiCallResponse> GetUserRoles(string userName)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("fatwa_api_url") + "/Account/GetUserRolesByUserName?userName=" + userName);
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

        public async Task<IEnumerable<User>?> UserListBySearchTerm(string filter)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("fatwa_api_url") + "/Account/UserListBySearchTerm?searchTerm=" + filter);
                // add authorization header
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                // send request
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(translationState.Translate("Contact_Administrator"));
                }
                return await response.Content.ReadFromJsonAsync<IEnumerable<User>>();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        //<History Author = 'Nadia Gull' Date='2022-11-3' Version="1.0" Branch="master"> returns the list of User Borrow Literatures</History>
        public async Task<List<UserBorrowLiteratureVM>> UserBorrowLiteratures(string userId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("fatwa_api_url") + "/Account/UserBorrowLiteratures?userId=" + userId);
                // add authorization header
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                // send request
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(translationState.Translate("Contact_Administrator"));
                }
                return await response.Content.ReadFromJsonAsync<List<UserBorrowLiteratureVM>>();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<User> UserListByUserId(string userId)
        {
            try
            {

                return await new HttpClient().GetFromJsonAsync<User>(_config.GetValue<string>("fatwa_api_url") + "/Account/UserListByUserId?userId=" + userId);
            }
            catch (Exception ex)
            {
                throw new Exception("Record not found", ex);
            }
        }
        //<History Author = 'Aqeel Altaf' Date='2022-08-09' Version="1.0" Branch="master">Get user detail by id</History>
        public async Task<ApiCallResponse> GetSecurityStampByEmail(string emailId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("fatwa_api_url") + "/Account/GetSecurityStampByEmail?emailId=" + emailId);
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
        public async Task<ApiCallResponse> GetUserById(string userId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("fatwa_api_url") + "/Account/UserListByUserId?userId=" + userId);
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadFromJsonAsync<User>();
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
                throw new Exception("Record not found", ex);
            }
        }

        //#region User DDL

        //static List<User> userData { get; set; } 
        //public async Task<DataEnvelope<User>> GetUserData(DataSourceRequest dataSourceRequest)
        //{
        //    try
        //    {
        //        var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("fatwa_api_url") + "/Account/GetUserData");
        //        // add authorization header
        //        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
        //        // send request
        //        var response = await new HttpClient().SendAsync(request);
        //        userData = await response.Content.ReadFromJsonAsync<List<User>>();
        //        var result = await userData.ToDataSourceResultAsync(dataSourceRequest);
        //        if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        //        {
        //            throw new Exception(translationState.Translate("Contact_Administrator"));
        //        }
        //        DataEnvelope<User> dataToReturn = new DataEnvelope<User>
        //        {
        //            Data = result.Data.Cast<User>().ToList(),
        //            Total = result.Total
        //        };
        //        return await Task.FromResult(dataToReturn);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception(ex.Message);
        //    }
        //}

        //public async Task<User> GetUserValue(string selectedValue)
        //{
        //    if (userData != null)
        //    {
        //        return await Task.FromResult(userData.FirstOrDefault(x => selectedValue == x.Id));
        //    }
        //    else
        //    {
        //        return await Task.FromResult<User>(null);
        //    }
        //}

        //#endregion

        public async Task Register(Object user)
        {
            try
            {
                await new HttpClient().PostAsJsonAsync<Object>(_config.GetValue<string>("fatwa_api_url") + "/Account/Register", user);
                //  await httpClient.PostAsJsonAsync<Object>("api/Account/Register", user); //Okay
            }
            catch (Exception ex)
            {
                throw new Exception("Error in register user", ex);
            }
        }

		#region Record User Logout Activity In Database
		public async Task<ApiCallResponse> RecordUserLogoutActivity(string username, string userId)
		{
			try
			{
				var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("fatwa_api_url") + "/Users/RecordUserLogoutActivity?username=" + username + "&userId=" + userId);

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
	}
}
