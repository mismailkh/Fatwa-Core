using Microsoft.AspNetCore.Components;
using Radzen;

namespace FATWA_WEB.Pages.HRMS.Employee
{
    public partial class ForgotPasswordDialog : ComponentBase
    {


        public async Task CancelChanges()
        {
           dialogService.Close();
        }
    }
}
