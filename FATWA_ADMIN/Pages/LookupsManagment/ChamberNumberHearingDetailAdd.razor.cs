using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_GENERAL.Helper.Response;
using Court = FATWA_DOMAIN.Models.CaseManagment.Court;

namespace FATWA_ADMIN.Pages.LookupsManagment
{
    public partial class ChamberNumberHearingDetailAdd : ComponentBase
	{
		#region Paramter

		[Parameter]
		public dynamic Id { get; set; }
        #endregion

        #region Variables 
        ApiCallResponse response = new ApiCallResponse();

        HearingDay _HearingDay;
        protected HearingDay HearingDay
        {
            get
            {
                return _HearingDay;
            }
            set
            {
                if (!object.Equals(_HearingDay, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "HearingDay", NewValue = value, OldValue = _HearingDay };
                    _HearingDay = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        ChamberNumberHearing _CmsChamberNumberHearing;
        protected ChamberNumberHearing CmsChamberNumberHearing
        {
            get
            {
                return _CmsChamberNumberHearing;
            }
            set
            {
                if (!object.Equals(_CmsChamberNumberHearing, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "CmsChamberNumberHearing", NewValue = value, OldValue = _CmsChamberNumberHearing };
                    _CmsChamberNumberHearing = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        public IList<Court> Courts { get; set; } = new List<Court>();
		public IList<Chamber> Chambers { get; set; } = new List<Chamber>();
		//public IEnumerable<int> SelectedCourts { get; set; } = new List<int>();   
		public List<Chamber> allChambersSelectedbyCourtId { get; set; } = new List<Chamber>();
		//public IEnumerable<int> SelectedChambers { get; set; } = new List<int>();  
		public List<ChamberNumber> allChamberNumberSelectedbyChamberId { get; set; } = new List<ChamberNumber>();
		public List<HearingDay> HearingDays { get; set; } = new List<HearingDay>();
		public List<ChamberNumberHearing> ChamberNumberHearings { get; set; } = new List<ChamberNumberHearing>();
		public bool Disable;
		public string moduleValidationMessageCourts { get; set; } = "";
		public string moduleValidationMessageChambers { get; set; } = "";
		public string moduleValidationMessageChamberNumbers { get; set; } = "";
		public string moduleValidationMessageHearingDays { get; set; } = "";
        public bool HearingDayValidation { get; set; }
        public bool JudgmentDayValidation { get; set; }

        #endregion

        #region On Load
        protected override async Task OnInitializedAsync()
		{
			await GetCourt();
			await GetHearingDays();
			await Load();
		}
        protected async Task Load()
        {
            spinnerService.Show();

            if (Id == null)
            {
                Disable = false;
                HearingDay = new HearingDay();
                CmsChamberNumberHearing = new ChamberNumberHearing();
            }
            else
            {
                Disable = true;

                // Fetch ChamberNumberHearing by Id
                response = await lookupService.GetChamberNumberHearingById(Id);
                var result = response.ResultData;

                if (result != null)
                {
                    var selectedresult = (ChamberNumberHearingDetailVM)result;

                    CmsChamberNumberHearing = new ChamberNumberHearing
                    {
                        Id = selectedresult.Id,
                        SelectedCourts = new List<int> { selectedresult.CourtId ?? 0 },
                        SelectedChambers = new List<int> { selectedresult.ChamberId ?? 0 },
                        SelectedChamberNumbers = new List<int> { selectedresult.ChamberNumberId ?? 0 },
                        SelectedHearingDayIds = new List<int> { selectedresult.HearingDaysId ?? 0 },
                        CreatedBy = selectedresult.CreatedBy,
                        JudgmentsRollDays = selectedresult.JudgmentsRollDays,
                        HearingsRollDays = selectedresult.HearingsRollDays
                    };

                    // Populate chambers directly using the Court ID from selectedresult
                    var chamberResponse = await lookupService.GetChamberByCourtId(selectedresult.CourtId ?? 0);
                    if (chamberResponse.IsSuccessStatusCode)
                    {
                        var chambers = (List<Chamber>)chamberResponse.ResultData;
                        if (chambers != null && chambers.Any())
                        {
                            allChambersSelectedbyCourtId.Clear();
                            allChambersSelectedbyCourtId.AddRange(chambers);
                        }
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(chamberResponse);
                    }

                    // Populate chamber numbers directly using the Chamber ID from selectedresult
                    var chamberNumbersResponse = await lookupService.GetChamberNumbersByChamberId(selectedresult.ChamberId ?? 0);
                    if (chamberNumbersResponse.IsSuccessStatusCode)
                    {
                        var chamberNumbers = (List<ChamberNumber>)chamberNumbersResponse.ResultData;
                        if (chamberNumbers != null && chamberNumbers.Any())
                        {
                            allChamberNumberSelectedbyChamberId.Clear();
                            allChamberNumberSelectedbyChamberId.AddRange(chamberNumbers);
                        }
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(chamberNumbersResponse);
                    }
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }

            spinnerService.Hide();
        }

        #endregion

        #region populate Court Name 
        protected async Task GetCourt()
		{

			var response = await lookupService.GetCourt();
			if (response.IsSuccessStatusCode)
			{
				Courts = (List<Court>)response.ResultData;
			}
			else
			{
				await invalidRequestHandlerService.ReturnBadRequestNotification(response);
			}

			StateHasChanged();

		}
		#endregion

		#region populate Hearing Days 
		protected async Task GetHearingDays()
		{

			var response = await lookupService.GetHearingDays();
			if (response.IsSuccessStatusCode)
			{
				HearingDays = (List<HearingDay>)response.ResultData;
			}
			else
			{
				await invalidRequestHandlerService.ReturnBadRequestNotification(response);
			}
			StateHasChanged();

		}
		#endregion

		#region Dropdown Change Events  
		protected async void OnChangeCourt()
		{
			spinnerService.Show();

			// Check if SelectedCourts is not null and has items
			if (CmsChamberNumberHearing.SelectedCourts != null && CmsChamberNumberHearing.SelectedCourts.Any())
			{
				var chmbers = new List<Chamber>();
				CmsChamberNumberHearing.SelectedChambers = new List<int>();
				CmsChamberNumberHearing.SelectedChamberNumbers = new List<int>();
				allChambersSelectedbyCourtId.Clear();
				allChamberNumberSelectedbyChamberId.Clear();

				foreach (var courtId in CmsChamberNumberHearing.SelectedCourts)
				{
					var response = await lookupService.GetChamberByCourtId(courtId);
					if (response.IsSuccessStatusCode)
					{
						var chambers = (List<Chamber>)response.ResultData;
						chmbers.AddRange(chambers);
					}
					else
					{
						await invalidRequestHandlerService.ReturnBadRequestNotification(response);
					}
				}
				allChambersSelectedbyCourtId.AddRange(chmbers);
			}
			else
			{
				CmsChamberNumberHearing.SelectedChambers = new List<int>();
				allChambersSelectedbyCourtId.Clear();
				CmsChamberNumberHearing.SelectedChamberNumbers = new List<int>();
				allChamberNumberSelectedbyChamberId.Clear();
			}

			spinnerService.Hide();
		}

		protected async void OnChangeChamber()
		{
			spinnerService.Show();

			// Check if SelectedChambers is not null and has items
			if (CmsChamberNumberHearing.SelectedChambers != null && CmsChamberNumberHearing.SelectedChambers.Any())
			{
				var chmber = new List<ChamberNumber>();
				allChamberNumberSelectedbyChamberId.Clear();
				CmsChamberNumberHearing.SelectedChamberNumbers = new List<int>();

				foreach (var chamberId in CmsChamberNumberHearing.SelectedChambers.ToList())
				{
					var response = await lookupService.GetChamberNumbersByChamberId(chamberId);
					if (response.IsSuccessStatusCode)
					{
						var chamberNumbers = (List<ChamberNumber>)response.ResultData;
						chmber.AddRange(chamberNumbers);
					}
					else
					{
						await invalidRequestHandlerService.ReturnBadRequestNotification(response);
					}
				}
				allChamberNumberSelectedbyChamberId.AddRange(chmber);
			}
			else
			{
				CmsChamberNumberHearing.SelectedChamberNumbers = new List<int>();
				allChamberNumberSelectedbyChamberId.Clear();
			}

			spinnerService.Hide();
		}

		#endregion

		#region Form Subit
		protected async Task SaveChanges(ChamberNumberHearing args)
		{
			bool isValid = true;
			string validationMessage = translationState.Translate("Required_Field");


			isValid = (args.SelectedCourts == null || args.SelectedCourts.Count() == 0)
				? (moduleValidationMessageCourts = validationMessage, false).Item2
				: (moduleValidationMessageCourts = string.Empty, isValid).Item2;

			isValid = (args.SelectedChambers == null || args.SelectedChambers.Count() == 0)
				? (moduleValidationMessageChambers = validationMessage, false).Item2
				: (moduleValidationMessageChambers = string.Empty, isValid).Item2;


			isValid = (args.SelectedChamberNumbers == null || args.SelectedChamberNumbers.Count() == 0)
				? (moduleValidationMessageChamberNumbers = validationMessage, false).Item2
				: (moduleValidationMessageChamberNumbers = string.Empty, isValid).Item2;


			isValid = (args.SelectedHearingDayIds == null || args.SelectedHearingDayIds.Count() == 0)
				? (moduleValidationMessageHearingDays = validationMessage, false).Item2
				: (moduleValidationMessageHearingDays = string.Empty, isValid).Item2;

			if (!isValid)
			{
				StateHasChanged();
				return;
			}
            if (string.IsNullOrEmpty(CmsChamberNumberHearing.HearingsRollDays.ToString()) || CmsChamberNumberHearing.HearingsRollDays == 0)
            {
                HearingDayValidation = true;
                return;
            }
            if (string.IsNullOrEmpty(CmsChamberNumberHearing.JudgmentsRollDays.ToString()) || CmsChamberNumberHearing.JudgmentsRollDays == 0)
            {
                JudgmentDayValidation = true;
                return;
            }
            else
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
							var fatwaDbCreateChamberNumberHearingResult = await lookupService.SaveChambernumberHearing(CmsChamberNumberHearing);
							if (fatwaDbCreateChamberNumberHearingResult.IsSuccessStatusCode)
							{
								notificationService.Notify(new NotificationMessage()
								{
									Severity = NotificationSeverity.Success,
									Detail = translationState.Translate("Chamber_Number_Hearing_Added_Successfully"),
									Style = "position: fixed !important; left: 0; margin: auto;width:30px; "
								});
							}
							else
							{
								await invalidRequestHandlerService.ReturnBadRequestNotification(response);
							}
						}
						else
						{
							var fatwaDbUpdateLmsLiteratureTypeResult = await lookupService.UpdateChamberNumberHearing(CmsChamberNumberHearing);
							if (fatwaDbUpdateLmsLiteratureTypeResult.IsSuccessStatusCode)
							{
								notificationService.Notify(new NotificationMessage()
								{
									Severity = NotificationSeverity.Success,
									Detail = translationState.Translate("Chamber_Number_Hearing_Updated_Successfully"),
									Style = "position: fixed !important; left: 0; margin: auto; width:25px;"
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
						Detail = Id == null ? translationState.Translate("Could_not_create_a_new_Chamber_Hearing_Days") : translationState.Translate("Chamber_Number_Hearing_could_not_be_updated"),
						Style = "position: fixed !important; left: 0; margin: auto; "
					});
					spinnerService.Hide();
				}
			}
		}

		protected async Task Button2Click(MouseEventArgs args)
		{
			dialogService.Close(false);
		}

        #endregion
        
		#region Funtions

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
        #endregion

        #region Check hearing roll day
        void CheckHearingRollDay()
        {
            if (string.IsNullOrEmpty(CmsChamberNumberHearing.HearingsRollDays.ToString()) || CmsChamberNumberHearing.HearingsRollDays == 0)
            {
                HearingDayValidation = true;
            }
            else
            {
                HearingDayValidation = false;
            }
        }
        #endregion
        #region Check Judgment day
        void CheckJudgmentDay()
        {
            if (string.IsNullOrEmpty(CmsChamberNumberHearing.JudgmentsRollDays.ToString()) || CmsChamberNumberHearing.JudgmentsRollDays == 0)
            {
                JudgmentDayValidation = true;
            }
            else
            {
                JudgmentDayValidation = false;
            }
        }
        #endregion
    }
}
