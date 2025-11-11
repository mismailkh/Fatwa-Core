using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Telerik.Blazor.Components;

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    public partial class DetailExecution : ComponentBase
	{
		#region Parameter
		[Parameter]
		public dynamic ExecutionId { get; set; }
		#endregion

		#region Varriable
		public CmsJudgmentExecutionDetailVM executionDetail { get; set; } = new CmsJudgmentExecutionDetailVM();
        protected byte[] FileData { get; set; }
		protected string DocumentPath { get; set; }
		protected TelerikPdfViewer PdfViewerRef { get; set; }
		protected bool ShowDocumentViewer { get; set; }
		#endregion

		#region ON Load Component
		public void Reload()
		{
			InvokeAsync(StateHasChanged);
		}

		protected override async Task OnInitializedAsync()
		{
			spinnerService.Show();
			await PopulateExecutionDetails();
			spinnerService.Hide();
		}


		protected async Task PopulateExecutionDetails()
		{
			try
			{
				var response = await cmsRegisteredCaseService.GetExecutionDetail(Guid.Parse(ExecutionId));
				if (response.IsSuccessStatusCode)
				{
					executionDetail = (CmsJudgmentExecutionDetailVM)response.ResultData;

					if (string.IsNullOrEmpty(executionDetail.PayerName))
					{
						if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
							executionDetail.PayerName = executionDetail.GovtEntityPayer_En;
						else
							executionDetail.PayerName = executionDetail.GovtEntityPayer_Ar;
					}
					if (string.IsNullOrEmpty(executionDetail.ReceiverName))
					{
						if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
							executionDetail.ReceiverName = executionDetail.GovtEntityReceiver_En;
						else
							executionDetail.ReceiverName = executionDetail.GovtEntityReceiver_Ar;
					}

				}
				else
				{
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
				}
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}
		#endregion


		#region Redirect Function
		private void GoBackDashboard()
		{
			navigationManager.NavigateTo("/dashboard");
		}
		private void GoBackHomeScreen()
		{
			navigationManager.NavigateTo("/index");
		}
		protected async Task BtnBack(MouseEventArgs args)
		{
			await JSRuntime.InvokeVoidAsync("history.back");
		}
		protected async Task RedirectBack()
		{
			await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
		}
		#endregion

	}
}
