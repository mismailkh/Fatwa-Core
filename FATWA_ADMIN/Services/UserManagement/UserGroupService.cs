using Blazored.LocalStorage;
using FATWA_ADMIN.Data;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Radzen;
using System.Data.Entity;
using System.Linq.Dynamic.Core;
using System.Net.Http.Headers;
using System.Text;
using Telerik.DataSource;
using Telerik.DataSource.Extensions;

namespace FATWA_ADMIN.Services.UserManagement
{
    public partial class UserGroupService
    {
        private readonly IConfiguration _config;
        private readonly ILocalStorageService browserStorage;
        private readonly TranslationState translationState;

        #region list 
        public async Task<IQueryable<UserGroupVM>> GetUserGroups(Query query = null)
        {
            var items = await GetUmsUserGroupList();

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

            OnUmsGroupRead(ref items);

            return await Task.FromResult(items);
        }
        #endregion
        partial void OnUmsGroupRead(ref IQueryable<UserGroupVM> items);

        private async Task<IQueryable<UserGroupVM>> GetUmsUserGroupList()
        {
            try
            {

                var request = new HttpRequestMessage(HttpMethod.Get, _config.GetValue<string>("api_url") + "/UmsGroups/GetUmsUserGroups");
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await browserStorage.GetItemAsync<string>("Token"));
                var response = await new HttpClient().SendAsync(request);
                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    throw new Exception(translationState.Translate("Contact_Administrator"));
                }
                var responselist = response.Content.ReadFromJsonAsync<IEnumerable<UserGroupVM>>();
                var queryableX = (await responselist).AsQueryable();
                return queryableX;

            }
            catch (Exception)
            {

                throw;
            }
        }

    }
}
