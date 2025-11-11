using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using FATWA_WEB.Services;
using FATWA_DOMAIN.Models;
using FATWA_WEB.Data;

namespace FATWA_WEB.Pages.Lms
{
    public partial class LmsLiteratureDetailsDialogCardPage : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public dynamic LiteratureId { get; set; }

        #region Service Injections
         
          
         
          
         
        protected LmsLiteratureService fatwaAPI { get; set; }
         
          

        #endregion

        protected LmsLiterature lmsLiterature { get; set; }

        public LmsLiteratureDetailsDialogCardPage()
        {
        }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }


        protected override async Task OnInitializedAsync()
        {
            await Load();
        }
        protected async Task Load()
        {
            spinnerService.Show();
            var getLmsLiteratureResult = await fatwaAPI.GetLmsLiteratureById(LiteratureId);
            lmsLiterature = getLmsLiteratureResult;
            spinnerService.Hide();
        }
    }
}
