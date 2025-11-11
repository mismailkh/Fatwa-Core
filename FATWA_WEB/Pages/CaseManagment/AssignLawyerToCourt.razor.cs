using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Extensions;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.CaseManagment
{
    public partial class AssignLawyerToCourt : ComponentBase
    {
        #region parameter
        [Parameter]
        public Guid Id { get; set; }
        [Parameter]
        public dynamic assignlawyertocourtId { get; set; }
        protected bool isEdit = false;
        #endregion

        #region variable declaration
        public int CourtTypeId { get; set; }
        public IList<UserVM> users { get; set; } = new List<UserVM>();
        public IList<Court> courts { get; set; } = new List<Court>();
        public IList<Chamber> chambers { get; set; } = new List<Chamber>();
        public IList<ChamberNumber> chamberNumbers { get; set; } = new List<ChamberNumber>();
        public List<ChamberNumber> allChamberNumbers { get; set; } = new List<ChamberNumber>();
        public List<Chamber> allChambers { get; set; } = new List<Chamber>();
        public CmsAssignLawyerToCourt assignLawyerToCourt { get; set; } = new CmsAssignLawyerToCourt();
        public IEnumerable<int> selectedcourts { get; set; } = new List<int>();
        public IEnumerable<int> selectedchambers { get; set; } = new List<int>();
        public OperatingSectorTypeEnum selectedSector { get; set; }
        protected List<object> operatingSectorsList { get; set; } = new List<object>();
        public List<Chamber> allChambersSelectedbyCourtId { get; set; } = new List<Chamber>();
        public List<ChamberNumber> allChamberNumberSelectedbyChamberId { get; set; } = new List<ChamberNumber>();
        #endregion
        public class OperatingSectorTypeEnumtemp
        {
            public OperatingSectorTypeEnum OperatingSectorTypeEnumValue { get; set; }
            public string OperatingSectorTypeEnumName { get; set; }
        }
        #region On Initialized
        protected override async Task OnInitializedAsync()
        {
            if (assignlawyertocourtId != null)
            {
                var response = await lookupService.GetAssignLawyertoCourtById(Guid.Parse(assignlawyertocourtId));
                if (response.IsSuccessStatusCode)
                {
                    assignLawyerToCourt = (CmsAssignLawyerToCourt)response.ResultData;


                }
                isEdit = true;
            }
            if (loginState.UserRoles.Any(x => SystemRoles.AdminRoles.Contains(x.RoleId)))
            {
                await PopulateSectors();
            }
            else
            {
                await PopulateLawyers();
                await PopulateCourts();
            }
            await PopulateChambers();
            await PopulateChamberNumbers();
        }
        #endregion

        #region Remote Dropdown Data  

        protected async Task PopulateLawyers()
        {
            if (selectedSector != null && selectedSector > 0)
            {
                loginState.UserDetail.SectorTypeId = (int)selectedSector;
                var userresponse = await lookupService.GetUsersBySectorForCourtAssignment(loginState.UserDetail.SectorTypeId);
                if (userresponse.IsSuccessStatusCode)
                {
                    users = (IList<UserVM>)userresponse.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(userresponse);
                }
                await PopulateCourts();
            }
            else
            {
                if (loginState.UserRoles.Any(x => SystemRoles.AdminRoles.Contains(x.RoleId)) && selectedSector == 0)
                {
                    users = new List<UserVM>();
                    courts = new List<Court>();
                    chambers = new List<Chamber>();
                    chamberNumbers = new List<ChamberNumber>();
                }
                else
                {
                    var userresponses = await lookupService.GetUsersBySectorForCourtAssignment(loginState.UserDetail.SectorTypeId);
                    if (userresponses.IsSuccessStatusCode)
                    {
                        users = (IList<UserVM>)userresponses.ResultData;
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(userresponses);
                    }
                }
            }
            StateHasChanged();
        }
        protected async Task PopulateCourts()
        {
            if (selectedSector != null && selectedSector > 0)
            {
                CourtTypeId = CaseConsultationExtension.GetCourtTypeIdBasedOnSectorId((int)selectedSector);
            }
            else if(loginState?.UserDetail?.SectorTypeId != null)
            {
                CourtTypeId = CaseConsultationExtension.GetCourtTypeIdBasedOnSectorId((int)loginState.UserDetail.SectorTypeId);
            }
            var response = await lookupService.GetCourt();
            if (response.IsSuccessStatusCode)
            {
                courts = (List<Court>)response.ResultData;
                courts = courts.Where(c => c.TypeId == CourtTypeId).ToList();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        protected async Task PopulateChambers()
        {
            var response = await lookupService.GetChamber();
            if (response.IsSuccessStatusCode)
            {
                //chambers = (List<Chamber>)response.ResultData;
                allChambers = (List<Chamber>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        public async Task PopulateChamberNumbers()
        {
            var response = await lookupService.GetChamberNumbersByChamberId(0);
            if (response.IsSuccessStatusCode)
            {
                //chamberNumbers = (List<ChamberNumber>)response.ResultData;
                allChamberNumbers = (List<ChamberNumber>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        public async Task PopulateSectors()
        {
            try
            {
                foreach (OperatingSectorTypeEnum item in Enum.GetValues(typeof(OperatingSectorTypeEnum)))
                {
                    int sectorTypeId = (int)item;
                    if (sectorTypeId >= (int)OperatingSectorTypeEnum.AdministrativeRegionalCases && sectorTypeId <= (int)OperatingSectorTypeEnum.CivilCommercialSupremeCases
                        && sectorTypeId != (int)OperatingSectorTypeEnum.CivilCommercialUnderFilingCases)
                    {
                        operatingSectorsList.Add(new OperatingSectorTypeEnumtemp { OperatingSectorTypeEnumName = translationState.Translate(item.ToString()), OperatingSectorTypeEnumValue = item });
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Dropdown Change Events 
        protected async void OnChangeCourt()
        {
            if (selectedcourts != null)
            {
                var courtIds = selectedcourts.ToList();
                if (courtIds.Count == 0)
                {
                    selectedchambers = new List<int>();
                    allChambersSelectedbyCourtId.Clear();
                    return;
                }
            }
            Dictionary<int, IList<Chamber>> courtChambers = new Dictionary<int, IList<Chamber>>();
            if (selectedcourts != null)
            {
                foreach (var courtId in selectedcourts.ToList())
                {
                    var response = await lookupService.GetChamberByCourtId(courtId);
                    if (response.IsSuccessStatusCode)
                    {
                        chambers = (List<Chamber>)response.ResultData;
                        courtChambers.Add(courtId, chambers);
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                }
            }
            List<Chamber> mergedChambers = courtChambers.Values.SelectMany(x => x).Distinct().ToList();
            allChambersSelectedbyCourtId.Clear();
            allChambersSelectedbyCourtId.AddRange(mergedChambers);
            //assign selected court
            assignLawyerToCourt.SelectedCourts = selectedcourts;
        }


        protected async void OnChangeChamber()
        {
            if (selectedchambers != null)
            {
                var chamberIds = selectedchambers.ToList();
                if (chamberIds.Count == 0)
                {
                    allChamberNumberSelectedbyChamberId.Clear();
                    return;
                }
            }
            Dictionary<int, IList<ChamberNumber>> chamberChamberNumbers = new Dictionary<int, IList<ChamberNumber>>();
            if (selectedchambers != null)
            {
                foreach (var chamberId in selectedchambers.ToList())
                {
                    var response = await lookupService.GetChamberNumbersByChamberId(chamberId);
                    if (response.IsSuccessStatusCode)
                    {
                        IList<ChamberNumber> chamberNumbers = (IList<ChamberNumber>)response.ResultData;
                        if (chamberChamberNumbers.ContainsKey(chamberId))
                        {
                            chamberChamberNumbers[chamberId] = chamberNumbers;
                        }
                        else
                        {
                            chamberChamberNumbers.Add(chamberId, chamberNumbers);
                        }
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                }
            }
            List<ChamberNumber> mergedChamberNumbers = chamberChamberNumbers.Values.SelectMany(x => x).Distinct().ToList();
            allChamberNumberSelectedbyChamberId.Clear();
            allChamberNumberSelectedbyChamberId.AddRange(mergedChamberNumbers);
            assignLawyerToCourt.SelectedChamber = selectedchambers;

        }

        #endregion

        #region BACK BUTTON

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

        #region Submit button click
        protected async Task Form0Submit(CmsAssignLawyerToCourt args)
        {
            try
            {
                if (!assignLawyerToCourt.SelectedUsers.Any())
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Required_Lawyer"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
                }
                if (!assignLawyerToCourt.SelectedChamberNumbers.Any())
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Required_Chamber_Number"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
                }
                bool? dialogResponse = await dialogService.Confirm(
                   translationState.Translate("Sure_Submit"),
                   translationState.Translate("Confirm"),
                   new ConfirmOptions()
                   {
                       OkButtonText = @translationState.Translate("Yes"),
                       CancelButtonText = @translationState.Translate("No")
                   });
                if (dialogResponse == true)
                {
                        ApiCallResponse response = null;
                        if (isEdit == false)
                        {

                            assignLawyerToCourt.Id = Id;
                            response = await lookupService.SubmitLawyerToCourt(assignLawyerToCourt);
                        }
                        else
                        {
                            spinnerService.Show();
                            response = await lookupService.EditAssignLawyertoCourt(assignLawyerToCourt);

                            navigationManager.NavigateTo("/assigned-lawyertocourt-list");
                        }
                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Lawyer_Assigned"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            navigationManager.NavigateTo("/assigned-lawyertocourt-list");
                        }
                        else
                        {
                            {
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Error,
                                    Detail = translationState.Translate("Something_Went_Wrong"),
                                    //Summary = translationState.Translate("Error"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                            }
                        }
                }
            }
            catch (Exception)
            {

            }
        }
        #endregion

    }
}
