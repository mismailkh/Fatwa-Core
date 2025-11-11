using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FATWA_WEB.Pages.Consultation.ConsultationFiles
{
    public partial class ConsultationFileHistoryDetail : ComponentBase

    {
        #region Parameters

        [Parameter]
        public dynamic HistoryId { get; set; }

        #endregion

        #region Variables

        protected ConsultationFileHistoryVM consultationFileHistory { get; set; }
        public string consultationFileViewPath = "";
		public string consultationFilePathUrl = "";

		#endregion

		#region Load

		protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();

            await Load();

            spinnerService.Hide();
        }

        protected async Task Load()
        {
            try
            {
                var result = await cmsCaseFileService.GetCaseFileHistoryDetailByHistoryId(Guid.Parse(HistoryId));
                if (result.IsSuccessStatusCode)
                {
                    consultationFileHistory = (ConsultationFileHistoryVM)result.ResultData;
                    if (consultationFileHistory is not null)
						consultationFileViewPath = $"/consultationfile-view/{consultationFileHistory.FileId}";
                    consultationFilePathUrl = $"/consultationfile-list/{consultationFileHistory.RequestTypeId}";



                }

				else
                {
                    consultationFileHistory = new ConsultationFileHistoryVM();
                }
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        #endregion

        #region Redirect Function

        protected void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }

        protected void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }

		protected async Task RedirectBack()
		{
			await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
		}
		#endregion
	}
}
