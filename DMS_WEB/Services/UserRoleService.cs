using Microsoft.AspNetCore.Identity;

namespace DMS_WEB.Services
{
    public class UserRoleService
    {
        private readonly HttpClient _httpClient;
        public UserRoleService(HttpClient httpClient)
        {
            this._httpClient = httpClient;
        }

        public async Task<IEnumerable<IdentityRole>> GetAllUsers()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<IdentityRole[]>("api/Roles/UserRoleList");
            }
            catch (Exception ex)
            {
                throw new Exception("Record not found", ex);
            }

        }
    }
}
