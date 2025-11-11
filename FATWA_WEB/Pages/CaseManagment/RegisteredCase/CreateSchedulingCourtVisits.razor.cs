using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_WEB.Pages.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.JSInterop;
using Radzen;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    public partial class CreateSchedulingCourtVisits : ComponentBase
    {

        #region Parameter
        [Parameter]
        public dynamic HearingId { get; set; }
        [Parameter]
        public string CaseId { get; set; }
        #endregion

        #region Variables
        public bool IsVisible { get; set; }
        protected SchedulingCourtVisits schedulingCourtVisit = new SchedulingCourtVisits()
        {
            DueDate = DateTime.Now,
            VisitDate = DateTime.Now,
            StartDate = DateTime.Now,
            EndDate = DateTime.Now,
            CreatedDate = DateTime.Now,
            IsDeleted = false
        };
        protected List<SchedulingCourtVisits> schedulingCourtVisits = new List<SchedulingCourtVisits>();
        public IEnumerable<LawyerVM> users { get; set; }
        protected List<SelectListItem> days = new List<SelectListItem>();
        protected List<CmsCaseVisitType> CaseVisitTypes = new List<CmsCaseVisitType>();
        protected string starttimeValidationMsg = "";
        protected string endtimeValidationMsg = "";
        #endregion

        #region OnInitialized
        protected override async Task OnInitializedAsync()
        {
            await PopulateUserdropdownlist();
            await PopulateDurations();
            await PopulateVisitType();
        }
        #endregion

        #region Cancel
        //< History Author = 'Danish' Date = '2022-12-13' Version = "1.0" Branch = "master" >Button Close Dialog</History>

        protected void ButtonBackDialog()
        {
            //navigationManager.NavigateTo(navigationState.ReturnUrl);
            navigationManager.NavigateTo("case-view/" + CaseId);
        }

        #endregion

        #region Draft A File
        //< History Author = 'Danish' Date = '2022-12-13' Version = "1.0" Branch = "master" >Button Draft A File Dialog</History>
        protected Task ButtonDraftAFileDialog(MouseEventArgs args)
        {
            return Task.CompletedTask;
        }
        #endregion

        #region OnChange Time
        //< History Author = 'Danish' Date = '2022-12-13' Version = "1.0" Branch = "master" > OnChangeTime</History>
        protected async void OnChangeTime()
        {
            try
            {
                if (schedulingCourtVisit.StartTime > schedulingCourtVisit.EndTime)
                {
                    DateTime endtime = schedulingCourtVisit.EndTime.AddMinutes(1);
                }
                if (schedulingCourtVisit.EndTime >= schedulingCourtVisit.StartTime)
                {
                    DateTime start = schedulingCourtVisit.StartTime;
                    DateTime end = schedulingCourtVisit.EndTime;
                    TimeSpan span = end.Subtract(start);
                    schedulingCourtVisit.Duration = string.Format("{0:00}:{1:00}", (int)span.TotalHours, span.Minutes);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region OnChange Clear
        //< History Author = 'Danish' Date '2022-12-13' Version = "1.0" Branch = "master" > OnChangeClear</History>
        protected async void OnChangeClear()
        {
            try
            {
                if (schedulingCourtVisit.IsReccuring)
                {
                    schedulingCourtVisit.StartDate = DateTime.Now;
                    schedulingCourtVisit.EndDate = DateTime.Now;
                    schedulingCourtVisit.StartTime = default(DateTime);
                    schedulingCourtVisit.EndTime = default(DateTime);
                    schedulingCourtVisit.Duration = " ";
                }
                else
                {
                    schedulingCourtVisit.VisitDate = DateTime.Now;
                    schedulingCourtVisit.StartTime = default(DateTime);
                    schedulingCourtVisit.EndTime = default(DateTime);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region Redirect Function
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

        #region Form Submit
        protected async Task FormSubmit(SchedulingCourtVisits item)
        {
            try
            {
                if (item.StartTime != default(DateTime) && item.EndTime!= default(DateTime))
                {
                    bool? dialogResponse = await dialogService.Confirm(
                          translationState.Translate("Sure_Submit"),
                          translationState.Translate("Confirm"),
                          new ConfirmOptions()
                          {
                              OkButtonText = @translationState.Translate("OK"),
                              CancelButtonText = @translationState.Translate("Cancel")
                          });
                    if (dialogResponse == true)
                    {
                        schedulingCourtVisit.CreatedBy = loginState.Username;
                        schedulingCourtVisit.HearingId = Guid.Parse(HearingId);


                        var result = await dialogService.OpenAsync<SelectDraftTemplatePopup>(translationState.Translate("Add_Draft"),
                        new Dictionary<string, object>()
                            {
                                { "ReferenceId", CaseId },
                                { "ModuleId", (int)WorkflowModuleEnum.CaseManagement },
                                { "DraftEntityType",  (int)DraftEntityTypeEnum.HearingSchedulingCourtVisit},
                                { "Payload", Newtonsoft.Json.JsonConvert.SerializeObject(schedulingCourtVisit) },
                                { "DocumentTypes", await identifyDocumentTypes.GetDocumentTypes(schedulingCourtVisit,(int)DraftEntityTypeEnum.HearingSchedulingCourtVisit, loginState.UserDetail.SectorTypeId != null ? (int)loginState.UserDetail.SectorTypeId : 0) }
                            }
                            ,
                            new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
                        //var response = await cmsRegisteredCaseService.CreateSchedulingCourtVists(schedulingCourtVisit);
                        //if (response.IsSuccessStatusCode)
                        //{
                        //    notificationService.Notify(new NotificationMessage()
                        //    {
                        //        Severity = NotificationSeverity.Success,
                        //        Detail = translationState.Translate("Saved"),
                        //        Style = "position: fixed !important; left: 0; margin: auto;"
                        //    });

                        //    //navigationManager.NavigateTo(navigationState.ReturnUrl);
                        //}
                        //else
                        //{
                        //    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        //}

                    }
                }
                else
                {
                    
                    starttimeValidationMsg = item.StartTime <= DateTime.Now ? translationState.Translate("Required_Field") : "";
                    endtimeValidationMsg = item.EndTime <= DateTime.Now ? translationState.Translate("Required_Field") : "";
                    
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
        #endregion

        #region Populate Dropdown
        protected async Task PopulateUserdropdownlist()
        {
            var userresponse = await lookupService.GetLawyersBySector(loginState.UserDetail.SectorTypeId);
            if (userresponse.IsSuccessStatusCode)
            {
                users = (IEnumerable<LawyerVM>)userresponse.ResultData;

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(userresponse);
            }

        }

        protected async Task PopulateDurations()
        {
            try
            {
                for (int i = 1; i <= DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month); i++)
                {
                    days.Add(new SelectListItem { Value = i.ToString(), Text = i.ToString() });
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected async Task PopulateVisitType()
        {
            try
            {
                var response = await lookupService.GetCourtVisitTypes();
                if (response.IsSuccessStatusCode)
                {
                    CaseVisitTypes = (List<CmsCaseVisitType>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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


