using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.TimeInterval;
using FATWA_DOMAIN.Models.ViewModel.TimeIntervalVMs;
using FATWA_DOMAIN.Models.WorkerService;
using Microsoft.AspNetCore.Components;
using Radzen;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.TimeInterval.TimeIntervalEnums;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_ADMIN.Pages.TimeInterval
{
    public partial class AddTimeIntervals : ComponentBase
    {
        #region Parameters

        //[Parameter(CaptureUnmatchedValues = true)]
        //public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }
        [Parameter]
        public dynamic Id { get; set; }
        [Parameter]
        public bool IsEdit { get; set; }
        [Parameter]
        public IEnumerable<TimeIntervalVM> TimeIntervals { get; set; }
        [Parameter]
        public TimeIntervalVM TimeIntervalVM { get; set; }

        #endregion

        #region Properties

        public List<CourtType> courtTypes { get; set; } = new List<CourtType>();
        public List<CmsComsReminderType> getCmsComsReminderTypedetails = null;
        public bool DisablReminderfirst { get; set; }
        public bool DisablReminder2nd { get; set; }
        public bool DisablReminderthird { get; set; }
        public bool DisablCommunicationResponseTypes { get; set; } = true;
        public bool DisablFinalJudgment { get; set; } = true;
        protected bool isEditMode = false;
        protected bool isEditModeCommunicationType = false;
        #endregion

        #region Variable
        public List<WSCommCommunicationTypes> CommunicationTypes = null;
        CmsComsReminder _CmsComsReminders;

        #endregion

        #region Full Property Declaration
        protected CmsComsReminder CmsComsReminders
        {
            get
            {
                return _CmsComsReminders;
            }
            set
            {
                if (!object.Equals(_CmsComsReminders, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "CmsChamberG2GLKP", NewValue = value, OldValue = _CmsComsReminders };
                    _CmsComsReminders = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        #endregion

        #region On Initialization
        protected override async Task OnInitializedAsync()
        {
            TimeIntervalVM.IntervalNameEn = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? TimeIntervalVM.IntervalNameEn : TimeIntervalVM.IntervalNameAr;
            TimeIntervalVM.CommunicationTypeEn = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? TimeIntervalVM.CommunicationTypeEn : TimeIntervalVM.CommunicationTypeAr;
            await Load();
        }
        #endregion

        #region Load
        ApiCallResponse response = new ApiCallResponse();
        protected async Task Load()
        {
            if (Id == null)
            {
                spinnerService.Show();
                CmsComsReminders = new CmsComsReminder() { };
                spinnerService.Hide();
            }
            else
            {
                spinnerService.Show();



                if (TimeIntervalVM.SecondReminderDuration == null)
                {
                    DisablReminder2nd = true;
                }
                if (TimeIntervalVM.ThirdReminderDuration == null)
                {
                    DisablReminderthird = true;
                }
                if (TimeIntervalVM.ReminderId == (int)CmsComsReminderTypeEnums.DefineThePeriodToRegionalORAppealORSupreme ||
                        TimeIntervalVM.ReminderId == (int)CmsComsReminderTypeEnums.DefineThePeriodToReplyOnTheOperationConsultant)
                {
                    DisablCommunicationResponseTypes = false;
                }
                else
                {
                    DisablCommunicationResponseTypes = true;

                }
                spinnerService.Hide();
            }

        }
        #endregion

        #region Functions
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {

        }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public void Notify(string TranslationKey)
        {
            notificationService.Notify(new NotificationMessage()
            {
                Severity = NotificationSeverity.Error,
                Detail = translationState.Translate(TranslationKey),
                Style = "position: fixed !important; left: 0; margin: auto; width:25% "
            });
        }
        #endregion

        #region Form Submit
        protected async Task SaveChanges()
        {
            try
            {
                if (!TimeIntervalVM.ExecutionTime.HasValue) return;

                if (TimeIntervalVM.SLAInterval <= TimeIntervalVM.FirstReminderDuration)
                {
                    Notify("Sla_Should_Be_Greater_then_first_duration");
                    TimeIntervalVM.FirstReminderDuration = null;
                    TimeIntervalVM.SecondReminderDuration = null;
                    TimeIntervalVM.ThirdReminderDuration = null;
                    return;
                }
                else if (TimeIntervalVM.FirstReminderDuration >= TimeIntervalVM.SLAInterval)
                {
                    Notify("Sla_Should_Be_Greater_then_first_duration");
                    TimeIntervalVM.FirstReminderDuration = null;
                    TimeIntervalVM.SecondReminderDuration = null;
                    TimeIntervalVM.ThirdReminderDuration = null;
                    return;

                }
                else if (TimeIntervalVM.FirstReminderDuration is null || TimeIntervalVM.FirstReminderDuration == 0)
                {
                    Notify("Fill_First_Reminder");
                    TimeIntervalVM.SecondReminderDuration = null;
                    return;
                }
                else if (TimeIntervalVM.SecondReminderDuration >= TimeIntervalVM.FirstReminderDuration)
                {
                    Notify("Second_Reminder_Duration_Should_be_Less_Than_SLA_Interval");
                    TimeIntervalVM.SecondReminderDuration = null;
                    return;
                }
                else if (TimeIntervalVM.SecondReminderDuration <= TimeIntervalVM.ThirdReminderDuration)
                {
                    Notify("Third_Reminder_Duration_Should_be_Less_Than_SLA_Interval");
                    TimeIntervalVM.ThirdReminderDuration = null;
                    return;
                }
                else if (!DisablReminder2nd && TimeIntervalVM.SecondReminderDuration is null || TimeIntervalVM.SecondReminderDuration == 0)
                {
                    Notify("Fill_Second_Reminder");
                    TimeIntervalVM.ThirdReminderDuration = null;
                    return;
                }
                else if (TimeIntervalVM.ThirdReminderDuration >= TimeIntervalVM.SecondReminderDuration)
                {
                    Notify("Third_Reminder_Duration_Should_be_Less_Than_SLA_Interval");
                    TimeIntervalVM.ThirdReminderDuration = null;
                    return;
                }

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
                        if (CmsComsReminders.CmsComsReminderTypeId == (int)CmsComsReminderTypeEnums.DefineThePeriodToRegionalORAppealORSupreme)
                        {
                            CmsComsReminders.DraftTemplateVersionStatusId = (int)CaseDraftDocumentStatusEnum.InReview;
                        }
                        var response = await timeIntervalService.SaveCmsComsReminder(CmsComsReminders);
                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Interval_Added_Successfully"),
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
                        CmsComsReminder data = new CmsComsReminder();
                        data.ID = TimeIntervalVM.ID;
                        data.FirstReminderDuration = TimeIntervalVM.FirstReminderDuration;
                        data.SecondReminderDuration = TimeIntervalVM.SecondReminderDuration;
                        data.ThirdReminderDuration = TimeIntervalVM.ThirdReminderDuration;
                        data.CmsComsReminderTypeId = TimeIntervalVM.ReminderId;
                        data.CommunicationTypeId = TimeIntervalVM.CommunicationTypeId;
                        data.IsNotification = TimeIntervalVM.IsNotification;
                        data.SLAInterval = TimeIntervalVM.SLAInterval;
                        data.ExecutionTime = TimeIntervalVM.ExecutionTime;
                        if (DisablReminder2nd)
                        {
                            DisablReminder2nd = true;
                            data.SecondReminderDuration = null;
                        }
                        if (DisablReminderthird)
                        {
                            data.ThirdReminderDuration = null;
                        }

                        var response = await timeIntervalService.UpdateCmsComsReminder(data);
                        if (response.IsSuccessStatusCode)
                        {

                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Interval_Updated_Successfully"),
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
                    Detail = Id == null ? translationState.Translate("Could_not_create_a_new_interval") : translationState.Translate("Something_went_wrong"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                spinnerService.Hide();
            }
        }
        #endregion

        #region SLA Interval Validation
        protected void OnIsDisableReminderSecondValueChanged(bool value)
        {
            if (DisablReminder2nd == false)
            {
                TimeIntervalVM.SecondReminderDuration = null;
                TimeIntervalVM.ThirdReminderDuration = null;
                DisablReminder2nd = true;
                DisablReminderthird = true;
            }
            else
            {
                DisablReminder2nd = false;
            }
        }
        protected void OnIsDisableReminderThirdValueChanged(bool value)
        {
            if (!DisablReminder2nd)
            {
                if (!DisablReminderthird) DisablReminderthird = true;
                else DisablReminderthird = false;
            }
            else
            {
                TimeIntervalVM.ThirdReminderDuration = null;
                DisablReminderthird = true;
            }
        }
        protected void OnCancel()
        {
            dialogService.Close(false);
        }
        protected void OnChangeSLAIntervalValue()
        {
            if (TimeIntervalVM.SLAInterval <= TimeIntervalVM.FirstReminderDuration)
            {
                Notify("Sla_Should_Be_Greater_then_first_duration");
                TimeIntervalVM.FirstReminderDuration = null;
                TimeIntervalVM.SecondReminderDuration = null;
                TimeIntervalVM.ThirdReminderDuration = null;
            }
        }
        protected void OnChangeFirstReminderValue()
        {
            if (TimeIntervalVM.SLAInterval is null)
            {
                Notify("Fill_SLA_INTEVAL_FIRST");
                TimeIntervalVM.FirstReminderDuration = null;
                TimeIntervalVM.SecondReminderDuration = null;
                TimeIntervalVM.ThirdReminderDuration = null;
            }
            if (TimeIntervalVM.FirstReminderDuration >= TimeIntervalVM.SLAInterval)
            {
                Notify("First_Reminder_Duration_Should_be_Less_Than_SLA_Interval");
                TimeIntervalVM.FirstReminderDuration = null;
                TimeIntervalVM.SecondReminderDuration = null;
                TimeIntervalVM.ThirdReminderDuration = null;
            }
            else if (TimeIntervalVM.SecondReminderDuration <= TimeIntervalVM.FirstReminderDuration || TimeIntervalVM.SecondReminderDuration >= TimeIntervalVM.FirstReminderDuration)
            {
                Notify("Second_Reminder_Duration_Should_be_Less_Than_SLA_Interval");
                TimeIntervalVM.SecondReminderDuration = null;
                TimeIntervalVM.ThirdReminderDuration = null;
            }

        }
        protected void OnChangeSecondReminderValue()
        {
            if (TimeIntervalVM.FirstReminderDuration is null || TimeIntervalVM.FirstReminderDuration == 0)
            {
                Notify("Fill_First_Reminder");
                TimeIntervalVM.SecondReminderDuration = null;
            }
            else if (TimeIntervalVM.SecondReminderDuration >= TimeIntervalVM.FirstReminderDuration)
            {
                Notify("Second_Reminder_Duration_Should_be_Less_Than_SLA_Interval");
                TimeIntervalVM.SecondReminderDuration = null;
            }
            else if (TimeIntervalVM.SecondReminderDuration <= TimeIntervalVM.ThirdReminderDuration)
            {
                Notify("Third_Reminder_Duration_Should_be_Less_Than_SLA_Interval");
                TimeIntervalVM.ThirdReminderDuration = null;
            }
        }
        protected void OnChangeThirdReminderValue()
        {
            if (TimeIntervalVM.SecondReminderDuration is null || TimeIntervalVM.SecondReminderDuration == 0)
            {
                Notify("Fill_Second_Reminder");
                TimeIntervalVM.ThirdReminderDuration = null;
            }
            else if (TimeIntervalVM.ThirdReminderDuration >= TimeIntervalVM.SecondReminderDuration)
            {
                Notify("Third_Reminder_Duration_Should_be_Less_Than_SLA_Interval");
                TimeIntervalVM.ThirdReminderDuration = null;
            }
        }
        #endregion

    }
}
