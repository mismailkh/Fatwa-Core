using Blazored.LocalStorage;
using FATWA_ADMIN.Data;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using Microsoft.AspNetCore.Identity;
using System.Net.Http.Headers;
using Radzen;
using System.Linq.Dynamic.Core;
using System.Data.Entity;

namespace FATWA_ADMIN.Services.UserManagement
{
    public partial class WebSystemsService
    {
        private readonly IConfiguration _config;
        private readonly ILocalStorageService browserStorage;
        private readonly TranslationState translationState;

        public WebSystemsService(IConfiguration config, ILocalStorageService _browserStorage, TranslationState _translationState)
        {
            _config = config;
            browserStorage = _browserStorage;
            translationState = _translationState;
        }

        public async Task<IEnumerable<IdentityRole>> GetAllUserRoles()
        {
            try
            {
                return await new HttpClient().GetFromJsonAsync<IdentityRole[]>(_config.GetValue<string>("api_url") + "/WebSystems/UserRoleList");
            }
            catch (Exception ex)
            {
                throw new Exception("Record not found", ex);
            }

        }
    }
}
