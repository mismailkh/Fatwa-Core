using Blazored.LocalStorage;
using FATWA_DOMAIN.Models.Employee;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace FATWA_ADMIN.Services.General
{
    public class EmployeeServices
    {
        private readonly IConfiguration config;
        private readonly ILocalStorageService browserStorage;

        public string ResultDetails { get; private set; }


        public EmployeeServices(IConfiguration _config, ILocalStorageService _browserStorage)
        {
            config = _config;
            browserStorage = _browserStorage;

        }
        public async Task<bool> CreateEmp(Employee employee)
        {
            bool isSaved = false;
            
            

                var request = new HttpRequestMessage(HttpMethod.Post, config.GetValue<string>("api_url") + "/Employee/CreateEmployee");
                var postBody = employee;
                request.Content = new StringContent(JsonSerializer.Serialize(postBody), Encoding.UTF8, "application/json");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    isSaved = false;

                    
                }
                if (response.IsSuccessStatusCode)
                {
                    isSaved = true;
                }
                return isSaved;
            
           
        }
    
    
    }
}
