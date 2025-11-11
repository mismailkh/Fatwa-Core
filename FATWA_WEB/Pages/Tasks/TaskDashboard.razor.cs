using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.Tasks
{
    // HISTORY HASSAN IFTIKHAR
    public partial class TaskDashboard : ComponentBase
	{ 
		#region Variables

		public TaskDashboardVM dashboardVM = new TaskDashboardVM();
		protected RadzenHtmlEditor editor;


		#endregion

		#region Load

		protected override async Task OnInitializedAsync()
		{ 
            spinnerService.Show();

			var result = await taskService.GetTaskDashBoard();
			if (result.IsSuccessStatusCode)
			{
				dashboardVM = (TaskDashboardVM)result.ResultData;
			}
			else
			{
				await invalidRequestHandlerService.ReturnBadRequestNotification(result);
			}

			spinnerService.Hide();
		}

		#endregion

		#region Functions

		async Task OnChangeFormatBlock(string value)
		{
			await editor.ExecuteCommandAsync("formatBlock", value);
		}

		public async Task SaveToDoList()
		{
			try
			{
				if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
				{
					OkButtonText = translationState.Translate("OK"),
					CancelButtonText = translationState.Translate("Cancel")
				}) == true)
				{
					var response = await taskService.SaveToDoList(dashboardVM);
					if (response.IsSuccessStatusCode)
					{
						notificationService.Notify(new NotificationMessage()
						{
							Severity = NotificationSeverity.Success,
							Detail = translationState.Translate("ToDo_List_Saved"),
							Style = "position: fixed !important; left: 0; margin: auto;"
						});
					}
					else
					{
						await invalidRequestHandlerService.ReturnBadRequestNotification(response);
					}
				}
			}
			catch (Exception)
			{
				notificationService.Notify(new NotificationMessage()
				{
					Severity = NotificationSeverity.Error,
					Detail = translationState.Translate("Something_Went_Wrong"),
					Style = "position: fixed !important; left: 0; margin: auto; "
				});
			}
		}

		public async Task ButtonCancelClick()
		{
			try
			{
				if (await dialogService.Confirm(translationState.Translate("Sure_Cancel_Unsaved_Changes"), translationState.Translate("Confirm"), new ConfirmOptions()
				{
					OkButtonText = translationState.Translate("OK"),
					CancelButtonText = translationState.Translate("Cancel")
				}) == true)
				{
					await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
				}
			}
			catch (Exception)
			{
				notificationService.Notify(new NotificationMessage()
				{
					Severity = NotificationSeverity.Error,
					Detail = translationState.Translate("Something_Went_Wrong"),
					Style = "position: fixed !important; left: 0; margin: auto; "
				});
			}
		}

		private void GoBackDashboard()
		{
			navigationManager.NavigateTo("/dashboard");
		}
		private void GoBackHomeScreen()
		{
			navigationManager.NavigateTo("/index");
		}
		#endregion

		#region Redirect Function
		//<History Author = 'Hassan Abbas' Date='2022-02-03' Version="1.0" Branch="master"> Redirect back to previous page from browser history</History>
		protected async Task RedirectBack()
		{
			await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
		}
		#endregion
	}
}
