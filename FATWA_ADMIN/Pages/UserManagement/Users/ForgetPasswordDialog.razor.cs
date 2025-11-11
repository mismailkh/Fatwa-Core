using Microsoft.AspNetCore.Components;

namespace FATWA_ADMIN.Pages.UserManagement.Users
{
    public partial class ForgetPasswordDialog : ComponentBase
    {
        public async Task CancelChanges()
        {
            dialogService.Close();

        }
    }
}
