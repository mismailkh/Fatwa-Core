using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.CommunicationModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_GENERAL.Helper.Response;


namespace FATWA_ADMIN.Pages.LookupsManagment
{

    public partial class CommunicationTypeAdd : ComponentBase
	{
		public void Reload()
		{
			InvokeAsync(StateHasChanged);
		}

		public void OnPropertyChanged(PropertyChangedEventArgs args)
		{
		}

		[Parameter]
		public dynamic CommunicationTypeId { get; set; }

		CommunicationType _communicationtypeadd;
		protected CommunicationType communicationtypeadd
		{
			get
			{
				return _communicationtypeadd;
			}
			set
			{
				if (!object.Equals(_communicationtypeadd, value))
				{
					var args = new PropertyChangedEventArgs() { Name = "communicationtypeadd", NewValue = value, OldValue = _communicationtypeadd };
					_communicationtypeadd = value;
					OnPropertyChanged(args);
					Reload();
				}
			}
		}

		protected override async Task OnInitializedAsync()
		{
			await Load();
		}
		ApiCallResponse response = new ApiCallResponse();
		protected async Task Load()
		{
			if (CommunicationTypeId == null)
			{
				spinnerService.Show();
				communicationtypeadd = new CommunicationType() { };
				spinnerService.Hide();
			}
			else
			{
				spinnerService.Show();
				response = await lookupService.GetCommunicationTypeById(CommunicationTypeId);
				if (response.IsSuccessStatusCode)
				{
					communicationtypeadd = (CommunicationType)response.ResultData;
				}
				else
				{
					await invalidRequestHandlerService.ReturnBadRequestNotification(response);
				}

				spinnerService.Hide();
			}

		}

		protected async Task SaveChanges(CommunicationType args)
		{
			try
			{
				bool? dialogResponse = await dialogService.Confirm(
									translationState.Translate("Sure_Submit"),
									translationState.Translate("Confirm"),
									new ConfirmOptions()
									{
										OkButtonText = translationState.Translate("OK"),
										CancelButtonText = translationState.Translate("Cancel")
									});
				if (dialogResponse == true)
				{
					spinnerService.Show();
					if (CommunicationTypeId== null)
					{
						var fatwaDbCreatelegalPrincipleTagResult = await lookupService.SaveCommunicationType(communicationtypeadd);
						if (fatwaDbCreatelegalPrincipleTagResult.IsSuccessStatusCode)
						{
							notificationService.Notify(new NotificationMessage()
							{
								Severity = NotificationSeverity.Success,
								Detail = translationState.Translate("Communication_type_Added_Successfully"),
								Style = "position: fixed !important; left: 0; margin: auto; "
							});
						}
						else
						{
							await invalidRequestHandlerService.ReturnBadRequestNotification(response);
						}
					}
					else
					{
						var fatwaDbUpdateLmsLiteratureTypeResult = await lookupService.UpdateCommunicationType(communicationtypeadd);
						if (fatwaDbUpdateLmsLiteratureTypeResult.IsSuccessStatusCode)
						{
							notificationService.Notify(new NotificationMessage()
							{
								Severity = NotificationSeverity.Success,
								Detail = translationState.Translate("Communication_Type_Updated_Successfully"),
								Style = "position: fixed !important; left: 0; margin: auto; "
							});
						}
						else
						{
							await invalidRequestHandlerService.ReturnBadRequestNotification(response);
						}
					}

					dialogService.Close(true);
					StateHasChanged();
					spinnerService.Hide();
				}
			}
			catch (Exception)
			{
				notificationService.Notify(new NotificationMessage()
				{
					Severity = NotificationSeverity.Error,
					Detail = CommunicationTypeId == null ? translationState.Translate("Could_not_create_a_new_Communication") : translationState.Translate("Communication_type_could_not_be_updated"),
					Style = "position: fixed !important; left: 0; margin: auto; "
				});
				spinnerService.Hide();
			}
		}
		protected async Task Button2Click(MouseEventArgs args)
		{
			dialogService.Close(false);
		}
	}
}
