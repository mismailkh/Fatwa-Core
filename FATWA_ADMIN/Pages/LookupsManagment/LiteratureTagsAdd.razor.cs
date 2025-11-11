using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.Lms;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_GENERAL.Helper.Response;


namespace FATWA_ADMIN.Pages.LookupsManagment
{

    public partial class LiteratureTagsAdd : ComponentBase
	{


		[Parameter(CaptureUnmatchedValues = true)]
		public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

		public void Reload()
		{
			InvokeAsync(StateHasChanged);
		}

		public void OnPropertyChanged(PropertyChangedEventArgs args)
		{
		}

		[Parameter]
		public dynamic Id { get; set; }

		LiteratureTag _literatureTag;
		protected LiteratureTag literatureTag
		{
			get
			{
				return _literatureTag;
			}
			set
			{
				if (!object.Equals(_literatureTag, value))
				{
					var args = new PropertyChangedEventArgs() { Name = "literatureTag", NewValue = value, OldValue = _literatureTag };
					_literatureTag = value;
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
			if (Id == null)
			{
				spinnerService.Show();
				literatureTag = new LiteratureTag() { };
				spinnerService.Hide();
			}
			else
			{
				spinnerService.Show();
				response = await lookupService.GetLmsLiteratureTagsById(Id);
				if (response.IsSuccessStatusCode)
				{
					literatureTag = (LiteratureTag)response.ResultData;
				}
				else
				{
					await invalidRequestHandlerService.ReturnBadRequestNotification(response);
				}
				spinnerService.Hide();
			}

		}

		protected async Task SaveChanges(LiteratureTag args)
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

					if (Id == null)
					{
						var fatwaDbCreatelegalPrincipleTagResult = await lookupService.SaveLiteratureTags(literatureTag);
						if (fatwaDbCreatelegalPrincipleTagResult.IsSuccessStatusCode)
						{
							notificationService.Notify(new NotificationMessage()
							{
								Severity = NotificationSeverity.Success,
								Detail = translationState.Translate("Literature_Tags_Added_Successfully"),
								Style = "position: fixed !important; left: 0; margin: auto; "
							});
						}
						else
						{
							await invalidRequestHandlerService.ReturnBadRequestNotification(fatwaDbCreatelegalPrincipleTagResult, "Tag_Already_Added");
						}
					}
					else
					{
						var fatwaDbUpdateLmsLiteratureTypeResult = await lookupService.UpdateLiteratureTags(literatureTag);
						if (fatwaDbUpdateLmsLiteratureTypeResult.IsSuccessStatusCode)
						{
							notificationService.Notify(new NotificationMessage()
							{
								Severity = NotificationSeverity.Success,
								Detail = translationState.Translate("Literature_Tags_Updated_Successfully"),
								Style = "position: fixed !important; left: 0; margin: auto; "
							});
						}
						else
						{
							await invalidRequestHandlerService.ReturnBadRequestNotification(response, "Tag_Already_Added");
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
					Detail = Id == null ? translationState.Translate("Could_not_create_a_new_Literature_Tags") : translationState.Translate("Literature_Tags_could_not_be_updated"),
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
