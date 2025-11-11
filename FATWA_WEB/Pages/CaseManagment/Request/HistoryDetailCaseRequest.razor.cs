using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;

namespace FATWA_WEB.Pages.CaseManagment.Request
{
	public partial class HistoryDetailCaseRequest : ComponentBase
	{
		#region Parameters

		[Parameter]
		public dynamic HistoryId { get; set; }

		#endregion

		#region Variables

		protected CmsCaseRequestHistoryVM cmsCaseRequestHistoryVM { get; set; }
		public string caseRequestViewPath = ""; 

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
				var result = await caseRequestService.GetCaseRequestHistoryDetailByHistoryId(Guid.Parse(HistoryId));
				if (result.IsSuccessStatusCode)
				{
					cmsCaseRequestHistoryVM = (CmsCaseRequestHistoryVM)result.ResultData;
					if (cmsCaseRequestHistoryVM is not null)
						caseRequestViewPath = $"/caserequest-view/{cmsCaseRequestHistoryVM.RequestId}";
				}
				else
				{
					cmsCaseRequestHistoryVM = new CmsCaseRequestHistoryVM();
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
