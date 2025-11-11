using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_ADMIN.Pages.LookupsManagment
{

    public partial class RequestSubTypeDetailAdd : ComponentBase
	{

		[Inject]
		protected IJSRuntime JSRuntime { get; set; }


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

		#region Variable
		public List<RequestType> RequestType { get; set; } = new List<RequestType>();
		SubTypeVM subTypeVM = new SubTypeVM();
		string ExistingNameEn = "";
		string ExistingNameAr = "";
		#endregion

		Subtype _CmsChamberG2GLKP;
		protected Subtype CmsChamberG2GLKP
		{
			get
			{
				return _CmsChamberG2GLKP;
			}
			set
			{
				if (!object.Equals(_CmsChamberG2GLKP, value))
				{
					var args = new PropertyChangedEventArgs() { Name = "CmsChamberG2GLKP", NewValue = value, OldValue = _CmsChamberG2GLKP };
					_CmsChamberG2GLKP = value;
					OnPropertyChanged(args);
					Reload();
				}
			}
		}

		protected override async Task OnInitializedAsync()
		{
			await GetRequestSubType();
			await Load();
		}
		#region populate Request types 
		protected async Task GetRequestSubType()
		{

			var response = await lookupService.GetRequestTypes();
			if (response.IsSuccessStatusCode)
			{
				RequestType = (List<RequestType>)response.ResultData;
			}
			else
			{
				await invalidRequestHandlerService.ReturnBadRequestNotification(response);
			}

			StateHasChanged();

		}
		#endregion

		ApiCallResponse response = new ApiCallResponse();
		protected async Task Load()
		{
			if (Id == null)
			{
				CmsChamberG2GLKP = new Subtype() { };
			}
			else
			{
				spinnerService.Show();
				response = await lookupService.GetSubTypeById(Id);
				if (response.IsSuccessStatusCode)
				{
					CmsChamberG2GLKP = (Subtype)response.ResultData;
				}
				else
				{
					await invalidRequestHandlerService.ReturnBadRequestNotification(response);
				}
				spinnerService.Hide();
			}

		}

		protected async Task SaveChanges(Subtype args)
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
						var G2GDbCreateChamberResult = await lookupService.SaveSubType(CmsChamberG2GLKP);
						if (G2GDbCreateChamberResult.IsSuccessStatusCode)
						{
							notificationService.Notify(new NotificationMessage()
							{
								Severity = NotificationSeverity.Success,
								Detail = translationState.Translate("Sub_Type_Added_Successfully"),
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
						var checkExistanceofNameEnAndNameAr = await ExistsSubTypeNames(args);
						if (checkExistanceofNameEnAndNameAr)
						{
							spinnerService.Hide();
							return;
						}

						var G2GDbUpdateLmsLiteratureTypeResult = await lookupService.UpdateSubType(CmsChamberG2GLKP);
						if (G2GDbUpdateLmsLiteratureTypeResult.IsSuccessStatusCode)
						{
							notificationService.Notify(new NotificationMessage()
							{
								Severity = NotificationSeverity.Success,
								Detail = translationState.Translate("Sub_Type_Updated_Successfully"),
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
					Detail = Id == null ? translationState.Translate("Could_not_create_a_new_Sub_Type") : translationState.Translate("Sub_Type_could_not_be_updated"),
					Style = "position: fixed !important; left: 0; margin: auto; "
				});
				spinnerService.Hide();
			}
		}

		#region  Check Existanse of Name EN and Name AR for SubTypes
		bool isNameEnExist = false;
		bool isNameArExist = false;
		protected async Task<bool> ExistsSubTypeNames(Subtype subtypes)
		{
			if (ExistingNameEn != subTypeVM.Name_En && ExistingNameAr != subTypeVM.Name_Ar)
			{
				isNameEnExist = await lookupService.CheckNameEnExists(subtypes.Name_En, subtypes.RequestTypeId , subtypes.Id);
				isNameArExist = await lookupService.CheckNameArExists(subtypes.Name_Ar, subtypes.RequestTypeId, subtypes.Id);

				string errorMessage = string.Empty;

				if (isNameEnExist && isNameArExist)
				{
					errorMessage = translationState.Translate("Both_English_And_Arabic_Names_Already_Exist");
				}
				else if (isNameEnExist)
				{
					errorMessage = translationState.Translate("English_Name_Already_Exist");
				}
				else if (isNameArExist)
				{
					errorMessage = translationState.Translate("Arabic_Name_Already_Exist");
				}

				if (!string.IsNullOrEmpty(errorMessage))
				{
					notificationService.Notify(new NotificationMessage()
					{
						Severity = NotificationSeverity.Error,
						Detail = errorMessage,
						Style = "position: fixed !important; left: 0; margin: auto; "
					});

					return true;
				}
			}

			return false;
		}
		#endregion

		protected async Task Button2Click(MouseEventArgs args)
		{
			dialogService.Close(false);
		}
	}
}
