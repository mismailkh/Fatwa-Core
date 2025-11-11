using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_DOMAIN.Enums.LiteratureEnum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_ADMIN.Pages.LookupsManagment
{
    public partial class LiteratureDeweyNumberPatternAdd : ComponentBase
	{

		#region Grid Function
		public void Reload()
		{
			InvokeAsync(StateHasChanged);
		}

		public void OnPropertyChanged(PropertyChangedEventArgs args)
		{
		}
		#endregion

		#region Paramter

		[Parameter]
		public dynamic Id { get; set; }
        #endregion

        #region Variable 
        public class literatureDeweyNumberPatternEnumTemp
        {
            public int LiteratureDeweyNumberPatternTypeEnumValue { get; set; }
            public string LiteratureDeweyNumberPatternTypeEnumName { get; set; }
        }
        LiteratureDeweyNumberPattern _LiteratureDeweyNumberPatterns;
        protected LiteratureDeweyNumberPattern LiteratureDeweyNumberPatterns
        {
            get
            {
                return _LiteratureDeweyNumberPatterns;
            }
            set
            {
                if (!object.Equals(_LiteratureDeweyNumberPatterns, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "LiteratureDeweyNumberPatterns", NewValue = value, OldValue = _LiteratureDeweyNumberPatterns };
                    _LiteratureDeweyNumberPatterns = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        ApiCallResponse response = new ApiCallResponse();
        protected List<literatureDeweyNumberPatternEnumTemp> LiteratureDeweyNumberPatternType { get; set; } = new List<literatureDeweyNumberPatternEnumTemp>();
		protected List<int> SequnceOrderList { get; set; } = new List<int>();
		#endregion

		#region On Load
		protected override async Task OnInitializedAsync()
		{
			foreach (literatureDeweyNumberPatternEnum item in Enum.GetValues(typeof(literatureDeweyNumberPatternEnum)))
			{
				LiteratureDeweyNumberPatternType.Add(new literatureDeweyNumberPatternEnumTemp { LiteratureDeweyNumberPatternTypeEnumName = translationState.Translate(item.ToString()), LiteratureDeweyNumberPatternTypeEnumValue = (int)item });
			}
			foreach (int value in Enum.GetValues(typeof(OrderSequenceNumber)))
			{
				SequnceOrderList.Add(value);
			}
			await Load();
		}

		protected async Task Load()
		{
			if (Id == null)
			{
				spinnerService.Show();
				LiteratureDeweyNumberPatterns = new LiteratureDeweyNumberPattern() { };
				spinnerService.Hide();
			}
			else
			{
				spinnerService.Show();
				response = await lookupService.GetLiteratureDeweyNumberPatternById(Id);
				if (response.IsSuccessStatusCode)
				{
					LiteratureDeweyNumberPatterns = (LiteratureDeweyNumberPattern)response.ResultData;
				}
				else
				{
					await invalidRequestHandlerService.ReturnBadRequestNotification(response);
				}
				spinnerService.Hide();
			}

		}
		#endregion

		#region Form Submit
		protected async Task SaveChanges(LiteratureDeweyNumberPattern args)
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
						if (LiteratureDeweyNumberPatterns.CheracterSeriesOrder != LiteratureDeweyNumberPatterns.DigitSequnceOrder)
						{

							LiteratureDeweyNumberPatterns.SequenceResult = string.Join("", LiteratureDeweyNumberPatterns.SeriesNumber, LiteratureDeweyNumberPatterns.DigitSequenceNumber);
							LiteratureDeweyNumberPatterns.SequenceFormatResult = string.Join("/-/", LiteratureDeweyNumberPatterns.SeriesNumber, LiteratureDeweyNumberPatterns.DigitSequenceNumber);
							var fatwaDbCreatelegalPrincipleTagResult = await lookupService.SaveLiteratureDeweyNumberPattern(LiteratureDeweyNumberPatterns);
							if (fatwaDbCreatelegalPrincipleTagResult.IsSuccessStatusCode)
							{
								notificationService.Notify(new NotificationMessage()
								{
									Severity = NotificationSeverity.Success,
									Detail = translationState.Translate("Literature_Dewey_Number_Pattern_Added_Successfully"),
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
							notificationService.Notify(new NotificationMessage()
							{
								Severity = NotificationSeverity.Error,
								Detail = translationState.Translate("cheracter_Sequence_Order_Not_Equal_To_Series_Sequence_Order"),
								Style = "position: fixed !important; left: 0; margin: auto; "
							});
							dialogService.Close(true);
						}
					}
					else
					{
						if (LiteratureDeweyNumberPatterns.CheracterSeriesOrder != LiteratureDeweyNumberPatterns.DigitSequnceOrder)
						{

							LiteratureDeweyNumberPatterns.SequenceResult = string.Join("", LiteratureDeweyNumberPatterns.SeriesNumber, LiteratureDeweyNumberPatterns.DigitSequenceNumber);
							LiteratureDeweyNumberPatterns.SequenceFormatResult = string.Join("/-/", LiteratureDeweyNumberPatterns.SeriesNumber, LiteratureDeweyNumberPatterns.DigitSequenceNumber);
							var fatwaDbUpdateLmsLiteratureTypeResult = await lookupService.UpdateLiteratureDeweyNumberPattern(LiteratureDeweyNumberPatterns);
							if (fatwaDbUpdateLmsLiteratureTypeResult.IsSuccessStatusCode)
							{
								notificationService.Notify(new NotificationMessage()
								{
									Severity = NotificationSeverity.Success,
									Detail = translationState.Translate("Literature_Dewey_Number_Pattern_Updated_Successfully"),
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
							notificationService.Notify(new NotificationMessage()
							{
								Severity = NotificationSeverity.Error,
								Detail = translationState.Translate("cheracter_Sequence_Order_N ot_Equal_To_Series_Sequence_Order"),
								Style = "position: fixed !important; left: 0; margin: auto; "
							});
							dialogService.Close(true);
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
					Detail = Id == null ? translationState.Translate("Could_not_create_a_new_Literature_Dewey_Number_Pattern") : translationState.Translate("Literature_Dewey_Number_Pattern_could_not_be_updated"),
					Style = "position: fixed !important; left: 0; margin: auto; "
				});
				spinnerService.Hide();
			}
		}

		protected void OnInput(ChangeEventArgs e)
		{
			LiteratureDeweyNumberPatterns.DigitSequenceNumber = e.Value?.ToString();
			// Remove any non-zero digits from the input
			//string zerosOnly = new string('0', input.Length);

			// Update the binding value with the restricted input
			// LiteratureDeweyNumberPatterns.DigitSequenceNumber = zerosOnly;
		}
		protected void OnInputSeriesSequenceNumber(ChangeEventArgs e)
		{
			LiteratureDeweyNumberPatterns.SeriesSequenceNumber = e.Value?.ToString();

			// Remove any non-zero digits from the input
			//string zerosOnly = new string('0', input.Length);

			// Update the binding value with the restricted input
			//LiteratureDeweyNumberPatterns.SeriesSequenceNumber = zerosOnly;
		}

		protected async Task Button2Click(MouseEventArgs args)
		{
			dialogService.Close(false);
		} 
		#endregion
	}
}
