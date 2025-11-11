using Microsoft.AspNetCore.Components;

namespace FATWA_ADMIN.Pages.UserManagement.Groups
{
    public partial class PermissionsDialog : ComponentBase
    {
        public async Task CancelChanges()
        {
            dialogService.Close();

        }
    }
}
