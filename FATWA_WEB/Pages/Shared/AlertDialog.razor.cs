using Microsoft.AspNetCore.Components;

namespace FATWA_WEB.Pages.Shared
{
    public partial class AlertDialog : ComponentBase
    {
        [Parameter]
        public string Title { get; set; }

        public async Task OK()
        {
            dialogService.Close();

        }
    }
}
