using System;
using System.Collections.Generic;
using System.Linq;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;

namespace FATWA_WEB.Pages.CaseManagment.Files
{
	public partial class HistoryDetailCaseFile : ComponentBase
	{
		#region Parameters

		[Parameter]
		public dynamic HistoryId { get; set; }

		#endregion

		#region Variables

		protected CmsCaseFileStatusHistoryVM cmsCaseFileHistoryVM { get; set; }
		public string caseFileViewPath = "";

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
					cmsCaseFileHistoryVM = (CmsCaseFileStatusHistoryVM)result.ResultData;
					if (cmsCaseFileHistoryVM is not null)
						caseFileViewPath = $"/casefile-view/{cmsCaseFileHistoryVM.FileId}";
				}
				else
				{
					cmsCaseFileHistoryVM = new CmsCaseFileStatusHistoryVM();
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
