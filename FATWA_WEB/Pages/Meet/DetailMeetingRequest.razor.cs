using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.MeetModels;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.Meet;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.MeetingEnums;

namespace FATWA_WEB.Pages.Meet
{
    public partial class DetailMeetingRequest : ComponentBase
    {
        #region Parameter
        [Parameter]
        public dynamic CommunicationId { get; set; }
        [Parameter]
        public dynamic ReferenceId { get; set; }
        [Parameter]
        public dynamic SubModuleId { get; set; }
        [Parameter]
        public dynamic ReceivedBy { get; set; }
        [Parameter]
        public dynamic TaskId { get; set; }

        #endregion

        #region Variables
        SaveMeetingVM saveMeeting = new SaveMeetingVM()
        {
            Meeting = new Meeting()
            {
                Date = DateTime.Today
            },

            GeAttendee = new List<MeetingAttendeeVM>(),
            DeletedGeAttendeeIds = new List<Guid>(),
            LegislationAttendee = new List<FatwaAttendeeVM>(),
            DeletedLegislationAttendeeIds = new List<Guid>(),
            MeetingMom = new MeetingMom()
        };
        protected RadzenDataGrid<FatwaAttendeeVM> LegislationAttendeeGrid = new RadzenDataGrid<FatwaAttendeeVM>();
        protected TaskDetailVM taskDetailVM { get; set; } = new TaskDetailVM();
        protected List<MeetingAttendeeVM> GetGeAttendees = new List<MeetingAttendeeVM>();
        protected List<FatwaAttendeeVM> GetLegislationAttendees = new List<FatwaAttendeeVM>();
        public bool AllApproved = false;
        int? atttendeeLegislationSerialNo = 0;
        protected User LegislationAttendee = new User();
        protected RadzenDataGrid<MeetingAttendeeVM>? GeattendeeGrid { get; set; } = new RadzenDataGrid<MeetingAttendeeVM>();
        protected bool isOnline = false;
        int atttendeeGeSerialNo = 0;
        public CommunicationVM communicationVM { get; set; } = new CommunicationVM()
        {
            Communication = new Communication(),
            CommunicationMeeting = new CommunicationMeeting(),
            MeetingAttendee = new List<MeetingAttendeeVM>(),
            FatwaAttendee = new List<FatwaAttendeeVM>()
        };
        protected List<ManagerTaskReminderVM> managerTaskReminderData { get; set; } = new List<ManagerTaskReminderVM>();

        #endregion

        #region On Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            spinnerService.Hide();
        }

        protected async Task Load()
        {
            await PopulateMeetingDetail();
            if (TaskId != null)
            {
                await PopulateTaskDetails();
                await GetManagerTaskReminderData();
            }
        }

        #endregion

        #region Populate Function
        protected async Task PopulateMeetingDetail()
        {
            try
            {
                spinnerService.Show();
                if (CommunicationId != null)
                {
                    var response = await communicationService.GetCommunicationDetailCommunicationId(Guid.Parse(CommunicationId));
                    if (response.IsSuccessStatusCode)
                    {
                        communicationVM = (CommunicationVM)response.ResultData;
                        if (communicationVM is not null)
                        {
                            saveMeeting.CommunicationId = Guid.Parse(CommunicationId);
                            saveMeeting.Meeting.CommunicationId = Guid.Parse(CommunicationId);
                            saveMeeting.Meeting.Subject = communicationVM.CommunicationMeeting.Subject;
                            saveMeeting.Meeting.Description = communicationVM.CommunicationMeeting.Description;
                            saveMeeting.Meeting.Agenda = communicationVM.CommunicationMeeting.Agenda;
                            saveMeeting.Meeting.Date = communicationVM.CommunicationMeeting.Date;
                            saveMeeting.Meeting.StartTime = communicationVM.CommunicationMeeting.StartTime;
                            saveMeeting.Meeting.EndTime = communicationVM.CommunicationMeeting.EndTime;
                            saveMeeting.Meeting.IsOnline = communicationVM.CommunicationMeeting.IsOnline;
                            saveMeeting.Meeting.MeetingLink = communicationVM.CommunicationMeeting.MeetingLink;
                            saveMeeting.Meeting.RequirePassword = communicationVM.CommunicationMeeting.RequirePassword;
                            saveMeeting.Meeting.Location = communicationVM.CommunicationMeeting.Location;
                            saveMeeting.Meeting.MeetingPassword = communicationVM.CommunicationMeeting.MeetingPassword;
                            saveMeeting.Meeting.MeetingStatusEn = communicationVM.CommunicationMeeting.MeetingStatusEn;
                            saveMeeting.Meeting.MeetingStatusAr = communicationVM.CommunicationMeeting.MeetingStatusAr;
                            saveMeeting.Meeting.MeetingTypeId = (int)MeetingTypeEnum.External;
                            saveMeeting.Meeting.ReferenceGuid = Guid.Parse(ReferenceId);
                            saveMeeting.Meeting.CreatedBy = communicationVM.Communication.CreatedBy;
                            saveMeeting.Meeting.CreatedDate = communicationVM.Communication.CreatedDate;
                            saveMeeting.Meeting.IsDeleted = communicationVM.Communication.IsDeleted;
                            saveMeeting.Meeting.SectorTypeId = loginState.UserDetail.SectorTypeId;
                            saveMeeting.Meeting.SentBy = communicationVM.Communication.CreatedBy;
                            saveMeeting.Meeting.ReceivedBy = loginState.Username;
                            saveMeeting.Meeting.Note = communicationVM.CommunicationMeeting.Note;
                            saveMeeting.Meeting.IsReplyForMeetingRequest = communicationVM.CommunicationMeeting.IsReplyForMeetingRequest;

                            if (communicationVM.MeetingAttendee.Any())
                            {
                                GetGeAttendees = communicationVM.MeetingAttendee;
                                atttendeeGeSerialNo = communicationVM.MeetingAttendee.Count();
                            }
                            if (communicationVM.FatwaAttendee.Any())
                            {
                                GetLegislationAttendees = communicationVM.FatwaAttendee.Where(x => x.Id != null).ToList();
                            }
                        }
                        Reload();
                    }
                }
                spinnerService.Hide();
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected async Task PopulateTaskDetails()
        {
            var getTaskDetail = await taskService.GetTaskDetailById(Guid.Parse(TaskId));
            if (getTaskDetail.IsSuccessStatusCode)
            {
                taskDetailVM = (TaskDetailVM)getTaskDetail.ResultData;
            }
            else
            {
                taskDetailVM = new TaskDetailVM();
            }
        }
        #endregion

        #region Action Button
        public async Task ApproveRequest()
        {
            try
            {
                navigationManager.NavigateTo("/request-meeting-decision/" + CommunicationId + "/" + ReferenceId + "/" + SubModuleId + "/" + TaskId + "/" + true);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task RejectRequest()
        {
            try
            {
                navigationManager.NavigateTo("/request-meeting-decision/" + CommunicationId + "/" + ReferenceId + "/" + SubModuleId + "/" + TaskId + "/" + false);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task BackBtn()
        {
            try
            {
                await JSRuntime.InvokeVoidAsync("history.back");
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task Edit()
        {
            try
            {
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        protected async Task ReviewMeetingRequest()
        {
            try
            {
                navigationManager.NavigateTo("/request-meeting-decision/" + CommunicationId + "/" + ReferenceId + "/" + SubModuleId + "/" + TaskId + "/" + true);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Redirect Function
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion

        #region Get Manager Task Reminder Data
        protected async Task GetManagerTaskReminderData()
        {
            try
            {
                var response = await lookupService.GetManagerTaskReminderData(Guid.Parse(TaskId));
                if (response.IsSuccessStatusCode)
                {
                    managerTaskReminderData = (List<ManagerTaskReminderVM>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion
    }
}

