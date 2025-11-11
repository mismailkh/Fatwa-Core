using Blazored.LocalStorage;
using FATWA_ADMIN.Data;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using System.Net.Http.Headers;
using System.Text;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_ADMIN.Services.UserManagement
{
    //<History Author = 'Umer Zaman' Date='2022-07-26' Version="1.0" Branch="master"> Service to handle transfer user requests</History>
    public partial class TransferUserService
    {
        #region Constructor
        public TransferUserService(IConfiguration config, ILocalStorageService _browserStorage, TranslationState _translationState)
        {
            _config = config;
            browserStorage = _browserStorage;
            translationState = _translationState;
        }
        #endregion

        #region variables declaration
        private readonly IConfiguration _config;
        private readonly ILocalStorageService browserStorage;
        private readonly TranslationState translationState;

        protected IEnumerable<UserTransferVM> userTransferVM;
        public string ResultDetails { get; private set; }
        #endregion

        #region Get All Department List
        public async Task<List<Department>> GetAllDepartmentList()
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/TransferUsers/GetAllDepartmentList"); ;
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(translationState.Translate("Contact_Administrator"));
            }
            var responselist = response.Content.ReadFromJsonAsync<List<Department>>();
            var queryableX = await responselist;
            return queryableX;
        }
        #endregion

        #region Get Transfer User By Id
        public async Task<List<UserTransferVM>> GetUmsUserTransfer(string userId)
        {
            var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Users/GetTransferUsers?UserId=" + userId);
            request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
            var response = await new HttpClient().SendAsync(request2);
            if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
            {
                throw new Exception(translationState.Translate("Contact_Administrator"));
            }
            var responselist = response.Content.ReadFromJsonAsync<List<UserTransferVM>>();
            var queryableX = await responselist;
            return queryableX;
        }
        public async Task<IEnumerable<UserTransferVM>> GetTransferUserById(string UserId)
        {

            try
            {

                userTransferVM = await new HttpClient().GetFromJsonAsync<IEnumerable<UserTransferVM>>(_config.GetValue<string>("api_url") + "/Users/GetTransferUsers?UserId=" + UserId);
                return userTransferVM;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        //var request2 = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/Users/GetTransferUsers?UserId=" + UserId);
        //request2.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
        //var response = await new HttpClient().SendAsync(request2);
        //if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
        //{
        //    throw new Exception(translationState.Translate("Contact_Administrator"));
        //}
        //if (response.IsSuccessStatusCode)
        //{
        //    //      return response;
        //}
        //else
        //{
        //    //        return response;
        //}
        //}
        //catch (Exception ex)
        //{
        //    throw;
        //}

        #endregion


        #region Save Transfer
        public async Task<ApiCallResponse> SaveTransferUser(TransferUser item)
        {
            item.CreatedBy = await browserStorage.GetItemAsync<string>("User");
            item.CreatedDate = DateTime.Now;
            var request = new HttpRequestMessage(HttpMethod.Post, _config.GetValue<string>("api_url") + "/TransferUsers/SaveTransferUser");
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
        #endregion
    }
}
