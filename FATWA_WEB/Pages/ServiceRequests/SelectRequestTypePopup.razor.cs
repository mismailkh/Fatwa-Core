using FATWA_DOMAIN.Models.ServiceRequestModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace FATWA_WEB.Pages.ServiceRequests
{
	public partial class SelectRequestTypePopup : ComponentBase
	{
		#region Constructor
		public SelectRequestTypePopup()
		{
			requestTypes = new RequestType();
		}
		#endregion

		#region Variable Declaration
		public RequestType requestTypes { get; set; }

		public class RequestType
		{
			public int RequestTypeId { get; set; }
		}
		public List<ServiceRequestType> ServiceRequestTypes { get; set; }

		#endregion

		#region Component OnLoad
		protected override async void OnInitialized()
		{
			spinnerService.Show();
			await Load();
			StateHasChanged();
			spinnerService.Hide();

		}
		public async Task Load()
		{
			await GetServiceRequestTypes();
		}
		#endregion

		#region Function
		public async Task GetServiceRequestTypes()
		{
			var response = await serviceRequestSharedService.GetServiceRequestTypes();
			if (response.IsSuccessStatusCode)
				ServiceRequestTypes = (List<ServiceRequestType>)response.ResultData;
			else
			{
				dialogService.Close(null);
				notificationService.Notify(new NotificationMessage()
				{
					Severity = NotificationSeverity.Error,
					Detail = translationState.Translate("Something_Went_Wrong"),
					Style = "position: fixed !important; left: 0; margin: auto; "
				});
			}
		}
		#endregion

		#region Form Submit
		public async Task FormSubmit()
		{
			dialogService.Close(requestTypes.RequestTypeId);

		}
		#endregion

		#region Redirection Function
		protected void ButtonCancelClick(MouseEventArgs args)
		{
			dialogService.Close(null);
		}
		#endregion
	}
}
