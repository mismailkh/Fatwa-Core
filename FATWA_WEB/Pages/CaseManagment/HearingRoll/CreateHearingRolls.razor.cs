using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.MojRollsVM;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_GENERAL.Helper.Enum;

namespace FATWA_WEB.Pages.CaseManagment.HearingRoll
{
    public partial class CreateHearingRolls:ComponentBase
    {
        #region Variable Declaration
        public int CourtTypeId { get; set; }
        public bool IsView { get; set; } = true;
        public DateTime? SessionDate;
        DateTime? MOJRollsSessionDates;
        public int RollsId;
        public int ChamberId;
        public int ChamberTypeCodeId;
        public int CourtId;
        public IEnumerable<MOJRollsChamberVM> Chambers;
        public IEnumerable<MOJRollsChamberVM> RMSChambersDdl;
        public IList<Court> courts { get; set; } = new List<Court>();
        public List<MOJRollsChamberTypeCode> ChamberNumbers { get; set; }
        public IList<MOJRollsChamberTypeCode> allChamberNumbers { get; set; } = new List<MOJRollsChamberTypeCode>();

        public IEnumerable<MOJRollsChamberVM> allChambers;
        // public List<Chamber> allChambers { get; set; } = new List<Chamber>();
        public IEnumerable<int> selectedcourts { get; set; } = new List<int>();
        public IEnumerable<int> selectedchambers { get; set; } = new List<int>();
        public IList<MOJRollsChamberVM> chambers { get; set; } = new List<MOJRollsChamberVM>();
        public MOJRollsRequest addDMOJRollsRequest { get; set; } = new MOJRollsRequest();

        public IEnumerable<MOJRollsVM> MOJRollsddl;
        public int MOJRollsId { get; set; }
        protected RadzenDataGrid<CmsRegisteredCase> grid;
        IEnumerable<CmsRegisteredCase> GetCmsRegisteredCaseVM;
            public IList<CmsRegisteredCase> selectedRegisteredcase;
            public CmsMojRPAHearing mojRPAHearing { get; set; } = new CmsMojRPAHearing();

        public bool allowRowSelectOnRowClick = true;

        IEnumerable<CmsRegisteredCase> _getRegisteredCaseVMs;
        int chamberid;
        #endregion
        string _search;

        protected IEnumerable<CmsRegisteredCase> getRegisteredCaseVMs
        {
            get
            {
                return _getRegisteredCaseVMs;
            }
            set
            {
                if (!Equals(_getRegisteredCaseVMs, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getRegisteredCasefile", NewValue = value, OldValue = _getRegisteredCaseVMs };
                    _getRegisteredCaseVMs = value;

                    Reload();
                }

            }
        }
        protected string search
        {
            get
            {
                return _search;
            }
            set
            {
                if (!Equals(_search, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "search", NewValue = value, OldValue = _search };
                    _search = value;

                    Reload();
                }
            }
        }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        #region OnInitialized
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateCourts();
            spinnerService.Hide();
        }
        #endregion

        #region Submit Button
        protected async Task Form0Submit(MOJRollsRequest mOJRollsRequest)
        {
            try
            {
                if (selectedRegisteredcase != null && selectedRegisteredcase.Any())
                {
                    bool? dialogResponse = await dialogService.Confirm(
                        translationState.Translate("Sure_Submit"),
                        translationState.Translate("Confirm"),
                        new ConfirmOptions()
                        {
                            OkButtonText = @translationState.Translate("Yes"),
                            CancelButtonText = @translationState.Translate("No")
                        });

                    if (dialogResponse == true) // User confirmed
                    {
                        spinnerService.Show();

                        // Set properties of mOJRollsRequest
                        mOJRollsRequest.IsAssigned = false;
                        mOJRollsRequest.RequestStatus_LookUp = 2; // Completed status
                        mOJRollsRequest.RollId_LookUp = MOJRollsId;
                        mOJRollsRequest.RequestedBy = loginState.UserDetail.ActiveDirectoryUserName ;
                        mOJRollsRequest.IsFatwaManual = true;

                        // Add selected cases to mojRPAHearing
                        foreach (var item in selectedRegisteredcase)
                        {
                            mojRPAHearing.Cases.Add(new CanAndCaseNumber
                            {
                                CANNumber = item.CANNumber,
                                CaseNumber = item.CaseNumber
                            });
                        }

                        // Set HearingDate from SessionDate
                        mojRPAHearing.HearingDate = (DateTime)mOJRollsRequest.SessionDate;
                        
                        // Create MOJRollsRequest and add hearing
                        var response = await mojRollsService.CreateRMSRequests(mOJRollsRequest);

                        if (response.IsSuccessStatusCode)
                        {
                            var addHearingResponse = await mojRollsService.AddHearingRolls(mojRPAHearing);

                            if (addHearingResponse.IsSuccessStatusCode)
                            {
                                spinnerService.Hide();
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Success,
                                    Detail = translationState.Translate("Request_Success_Message"),
                                    Style = "position: fixed !important; left: 0; right: 0; margin: auto; text-align: center;",
                                });

                                navigationManager.NavigateTo("/upcominghearings-rolls-list");
                                Reload();
                            }
                            else
                            {
                                spinnerService.Hide();
                                await invalidRequestHandlerService.ReturnBadRequestNotification(addHearingResponse);
                            }
                        }
                        else
                        {
                            spinnerService.Hide();
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }
                    }
                }
                else
                {
                    // Notify user to select at least one case
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Please_Select_AtLeast_One_Case"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
            }
            catch (Exception ex)
            {
                spinnerService.Hide();
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        #endregion

        #region Back Buttons
        protected async Task ButtonCancel(MouseEventArgs args)
        {
            await JSRuntime.InvokeVoidAsync("history.back");
        }
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion
        #region Populate DropDowns
        protected async Task PopulateCourts()
        {
            var response = await lookupService.GetCourtByUserId(loginState.UserDetail.UserId);
            if (response.IsSuccessStatusCode)
            {
                courts = (List<Court>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task PopulateChambersLookuUp()
        {
            var response = await mojRollsService.GetRmsChambersLookuUp(loginState.UserDetail.UserId);
            if (response.IsSuccessStatusCode)
            {
                Chambers = (List<MOJRollsChamberVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task PopulateMojRolleChamberNumber()
        {

            var response = await mojRollsService.GetMojRolleChamberNumberByUserId(loginState.UserDetail.UserId);
            if (response.IsSuccessStatusCode)
            {
                ChamberNumbers = (List<MOJRollsChamberTypeCode>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        
        #endregion
        #region OnChange 
        protected async void OnChangeCourt(object arg)
        {

            await PopulateChambersLookuUp();
            int courtId = (int)arg;
            var selectedCourt = courts.FirstOrDefault(x => x.Id == courtId);
            if (selectedCourt != null)
            {
                RMSChambersDdl = Chambers.Where(x => x.CourtId == courtId).ToList();
                if (RMSChambersDdl != null && RMSChambersDdl.Any())
                {
                    Chambers = RMSChambersDdl.ToList();

                }
                else
                {
                    await PopulateChambersLookuUp();
                }
                if (selectedCourt.TypeId == (int)CourtTypeEnum.Regional)
                {
                    MOJRollsId = (int)MOJRollsEnum.Regional;
                }
                else if (selectedCourt.TypeId == (int)CourtTypeEnum.Appeal)
                {
                    MOJRollsId = (int)MOJRollsEnum.Appeal;
                }
                else if (selectedCourt.TypeId == (int)CourtTypeEnum.Supreme)
                {
                    MOJRollsId = (int)MOJRollsEnum.Supreme;
                }

                StateHasChanged();
            }

        }
        protected async void OnChangeChamber(object arg)
        {
            await PopulateMojRolleChamberNumber();
            ChamberNumbers = ChamberNumbers.Where(x => x.ChamberId == (int)arg)
            .Select(x => new MOJRollsChamberTypeCode
            {
                Id = x.Id,
                Name = x.Name,
            })
            .ToList();
            StateHasChanged();

        }
        protected async void OnChangeChamberNumber(object arg)
        {
            if (arg != null)
            {
                await PopulateRegisteredCasesByChamberNumberId((int)arg);
                StateHasChanged();
            }
            else
            {
                getRegisteredCaseVMs = new List<CmsRegisteredCase>();
                GetCmsRegisteredCaseVM = new List<CmsRegisteredCase>();
            }
        }
        public async Task PopulateRegisteredCasesByChamberNumberId(int chamberNumberId)
        {
            var response = await cmsRegisteredCaseService.GetRegisteredCasesByChamberNumberId(chamberNumberId);
            if (response.IsSuccessStatusCode)
            {
                getRegisteredCaseVMs = (List<CmsRegisteredCase>)response.ResultData;
                GetCmsRegisteredCaseVM = (IEnumerable<CmsRegisteredCase>)response.ResultData;
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

        }
        #endregion
        #region Search Grid
        protected async Task OnSearchInput()
        {
            try
            {
                if (string.IsNullOrEmpty(search))
                {
                    search = "";
                }
                else
                    search = search.ToLower();
                if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
                {
                    GetCmsRegisteredCaseVM = await gridSearchExtension.Filter(getRegisteredCaseVMs, new Query()
                    {
                        Filter = $@"i => ( i.CANNumber != null && i.CANNumber.ToString().ToLower().Contains(@0) ) || ( i.CaseNumber != null && i.CaseNumber.ToLower().Contains(@1) ) ",
                        FilterParameters = new object[] { search, search }
                    });
                }
                else
                {
                    GetCmsRegisteredCaseVM = await gridSearchExtension.Filter(getRegisteredCaseVMs, new Query()
                    {
                        Filter = $@"i => ( i.CANNumber != null && i.CANNumber.ToString().ToLower().Contains(@0) ) || ( i.CaseNumber != null && i.CaseNumber.ToLower().Contains(@1) ) ",
                        FilterParameters = new object[] { search, search }
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
     
    }
}
