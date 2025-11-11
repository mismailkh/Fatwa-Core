using FATWA_DOMAIN.Models.TaskModels;
using Microsoft.AspNetCore.Components;
using Radzen;

namespace FATWA_WEB.Pages.Tasks
{
    public partial class RejectAndReason : ComponentBase
	{
		#region Parameter

		[Parameter]
		public dynamic ReferenceId { get; set; }

		#endregion

		#region Variables

		protected RejectReason rejectReason = new RejectReason();

		#endregion

		#region Functions

		protected async Task Form0Submit()
		{
			try
			{
				rejectReason.RejectionId = Guid.NewGuid();
				rejectReason.ReferenceId = ReferenceId;
				var response = await taskService.RejectReason(rejectReason);
				if (response.IsSuccessStatusCode)
				{
					var result = (bool)response.ResultData;
					dialogService.Close(result);

					//if (result == true)
					//               {
					//	notificationService.Notify(new NotificationMessage()
					//	{
					//		Severity = NotificationSeverity.Success,
					//		Detail = translationState.Translate("Rejected_successfully"),
					//		Style = "position: fixed !important; left: 0; margin: auto; "
					//	});
					//	StateHasChanged();
					//	dialogService.Close(result);
					//}

					//               notificationService.Notify(new NotificationMessage()
					//                   {
					//                       Severity = NotificationSeverity.Warning,
					//                       Detail = translationState.Translate("Its_Already_Rejected"),
					//                       Style = "position: fixed !important; left: 0; margin: auto; "
					//                   });
					//               StateHasChanged();
					//               dialogService.Close(result); 
				}
				else
				{
					await invalidRequestHandlerService.ReturnBadRequestNotification(response);
				}
			}
			catch (Exception)
			{
				notificationService.Notify(new NotificationMessage()
				{
					Severity = NotificationSeverity.Error,
					Detail = translationState.Translate("Unable_to_Reject"),
					Style = "position: fixed !important; left: 0; margin: auto; "
				});
			}
		}

		#endregion
	
	}
}
