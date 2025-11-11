using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.MeetModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.Meet;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System.Collections.ObjectModel;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.MeetingEnums;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.Meet
{
    public partial class MeetingRequestDecision : ComponentBase
    {
        #region Parameter
        [Parameter]
        public dynamic CommunicationId { get; set; }
        [Parameter]
        public dynamic IsRequestDecision { get; set; } 
        [Parameter]
        public dynamic ReferenceId { get; set; }
        [Parameter]
        public dynamic SubModuleId { get; set; }
        [Parameter]
        public dynamic TaskId { get; set; }

        #endregion

        #region Variables
        protected RadzenDropDown<string> ddlLegislationAttendees;
        public bool AllApproved = false;
        int? atttendeeLegislationSerialNo = 0;
        protected User LegislationAttendee = new User();
        //DDL
        protected RadzenDataGrid<MeetingAttendeeVM>? GeattendeeGrid;
        protected bool isOnline = false;
        int atttendeeGeSerialNo = 0;
        protected RadzenDataGrid<FatwaAttendeeVM> LegislationAttendeeGrid = new RadzenDataGrid<FatwaAttendeeVM>();
        public bool isRequestForMeetingDecision { get { return Convert.ToBoolean(IsRequestDecision); } set { IsRequestDecision = value; } }
        List<string> validFiles { get; set; } = new List<string>() { ".pdf" };
        protected List<CommunicationAttendeeVM> getCommunicationAttendees { get; set; } = new List<CommunicationAttendeeVM>();
        protected List<MeetingAttendeeVM> GetGeAttendees = new List<MeetingAttendeeVM>();
        protected List<FatwaAttendeeVM> GetLegislationAttendees = new List<FatwaAttendeeVM>();
        public IList<MeetingAttendeeVM> selectedGEAttandees { get; set; }
        public List<MeetingAttendeeStatus> attendeeStatus { get; set; } = new List<MeetingAttendeeStatus>();
        protected List<FatwaAttendeeVM> LegislationAttendees = new List<FatwaAttendeeVM>();
        SendCommunicationVM sendCommunication = new SendCommunicationVM()
        {
            Communication = new Communication(),
            CommunicationMeeting = new CommunicationMeeting(),
            CommunicationAttendee = new List<CommunicationAttendeeVM>(),
            CommunicationTargetLink = new CommunicationTargetLink(),
            LinkTarget = new List<LinkTarget>(),
            DeletedGeAttendeeIds = new List<Guid>(),
        };
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
            await PopulateLegislationAttendees();
            await PopulateMeetingAttendeeStatus();
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
                        var communication = (CommunicationVM)response.ResultData;
                        if (communication is not null)
                        {
                            if(communication.CommunicationMeeting.IsReplyForMeetingRequest)
                            {
                                var responseMeeting = await communicationService.GetMeetingDetailByUsingCommunicationId(Guid.Parse(CommunicationId));
                                if (responseMeeting.IsSuccessStatusCode)
                                {
                                    saveMeeting.Meeting = (Meeting)responseMeeting.ResultData;
                                    saveMeeting.Meeting.SectorTypeId = loginState.UserDetail.SectorTypeId;
                                    saveMeeting.Meeting.SentBy = communication.Communication.CreatedBy;
                                    saveMeeting.Meeting.ReceivedBy = loginState.Username;
                                    saveMeeting.CommunicationId = Guid.Parse(CommunicationId);
                                }
                            }
                            else
                            {
                                saveMeeting.CommunicationId = Guid.Parse(CommunicationId);
                                saveMeeting.Meeting.CommunicationId = Guid.Parse(CommunicationId);
                                saveMeeting.Meeting.Subject = communication.CommunicationMeeting.Subject;
                                saveMeeting.Meeting.Description = communication.CommunicationMeeting.Description;
                                saveMeeting.Meeting.Agenda = communication.CommunicationMeeting.Agenda;
                                saveMeeting.Meeting.Date = communication.CommunicationMeeting.Date;
                                saveMeeting.Meeting.StartTime = communication.CommunicationMeeting.StartTime;
                                saveMeeting.Meeting.EndTime = communication.CommunicationMeeting.EndTime;
                                saveMeeting.Meeting.IsOnline = communication.CommunicationMeeting.IsOnline;
                                saveMeeting.Meeting.MeetingLink = communication.CommunicationMeeting.MeetingLink;
                                saveMeeting.Meeting.RequirePassword = communication.CommunicationMeeting.RequirePassword;
                                saveMeeting.Meeting.Location = communication.CommunicationMeeting.Location;
                                saveMeeting.Meeting.MeetingPassword = communication.CommunicationMeeting.MeetingPassword;
                                saveMeeting.Meeting.MeetingStatusEn = communication.CommunicationMeeting.MeetingStatusEn;
                                saveMeeting.Meeting.MeetingStatusAr = communication.CommunicationMeeting.MeetingStatusAr;
                                saveMeeting.Meeting.MeetingTypeId = (int)MeetingTypeEnum.External;
                                saveMeeting.Meeting.MeetingStatusId = (int)MeetingStatusEnum.OnHold;
                                saveMeeting.Meeting.SubModulId = Convert.ToInt32(SubModuleId);
                                saveMeeting.Meeting.ReferenceGuid = Guid.Parse(ReferenceId);
                                saveMeeting.Meeting.CreatedBy = communication.Communication.CreatedBy;
                                saveMeeting.Meeting.CreatedDate = communication.Communication.CreatedDate;
                                saveMeeting.Meeting.IsDeleted = communication.Communication.IsDeleted;
                                saveMeeting.Meeting.SectorTypeId = loginState.UserDetail.SectorTypeId;
                                saveMeeting.Meeting.SentBy = communication.Communication.CreatedBy;
                                saveMeeting.Meeting.ReceivedBy = loginState.Username;
                                saveMeeting.Meeting.Note = communication.CommunicationMeeting.Note;
                                saveMeeting.Meeting.IsReplyForMeetingRequest = isRequestForMeetingDecision;
                            }
                            if (communication.MeetingAttendee.Any())
                            {
                                GetGeAttendees = communication.MeetingAttendee;
                                atttendeeGeSerialNo = communication.MeetingAttendee.Count();
                            }
                            if (communication.FatwaAttendee.Any())
                            {
                                GetLegislationAttendees = communication.FatwaAttendee.Where(x => x.Id != null).ToList();
                            }
                            if (TaskId != null)
                            {
                                saveMeeting.TaskId = Guid.Parse(TaskId);
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
        protected async Task PopulateLegislationAttendees(List<string> attendeeIds = null)
        {
            var response = await lookupService.GetAttendeeUser();
            if (response.IsSuccessStatusCode)
            {
                attendeeIds = attendeeIds ?? new List<string>();
                var user = await BrowserStorage.GetItemAsync<UserDetailVM>("UserDetail");
                LegislationAttendees = (List<FatwaAttendeeVM>)response.ResultData;
                LegislationAttendees = LegislationAttendees.Where(x => x.SectorTypeId == user.SectorTypeId ||
                                           x.Email == "hospublicoperational@fatwa.com" ||
                                           x.Email == "fatwapresidentoffice@fatwa.com")
                                           .Where(x => !attendeeIds.Contains(x.Id))
                                           .OrderBy(x => x.FirstNameEnglish).ToList();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task PopulateMeetingAttendeeStatus()
        {
            var response = await lookupService.GetAttendeeMeetingStatus();
            if (response.IsSuccessStatusCode)
            {
                attendeeStatus = (List<MeetingAttendeeStatus>)response.ResultData;

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

        #region Location 
        void OnChange(bool value)
        {
            if (value)
            {
                isOnline = true;
            }
            else
            {
                isOnline = false;
            }
            Console.WriteLine($"{value}");
        }
        protected bool isPassReq = false;
        void IsPasswordRequired(bool value)
        {
            isPassReq = value;
        }
        #endregion

        #region Add and Delete Fatwa Attendees
        protected async Task AddLegislationAttendee()
        {
            if (LegislationAttendee.Id != null)
            {
                var newAttendee = LegislationAttendees.FirstOrDefault(x => x.Id == LegislationAttendee.Id);
                if (newAttendee is not null)
                {
                    newAttendee.AttendeeStatusId = (int)MeetingAttendeeStatusEnum.New;
                    newAttendee.AttendeeStatusEn = attendeeStatus.Where(x => x.Id == (int)MeetingAttendeeStatusEnum.New).Select(y => y.NameEn).FirstOrDefault();
                    newAttendee.AttendeeStatusAr = attendeeStatus.Where(x => x.Id == (int)MeetingAttendeeStatusEnum.New).Select(y => y.NameAr).FirstOrDefault();
                    newAttendee.SerialNo = ++atttendeeLegislationSerialNo;
                    GetLegislationAttendees.Add(newAttendee);
                    AllApproved = GetLegislationAttendees.ToList().All(x => x.AttendeeStatusId != (int)MeetingAttendeeStatusEnum.NotResponded && x.AttendeeStatusId != (int)MeetingAttendeeStatusEnum.New && x.AttendeeStatusId != (int)MeetingAttendeeStatusEnum.Pending);
                    await PopulateLegislationAttendees(new List<string> { newAttendee.Id });

                    // remove selected attendee from attendee dropdown
                    var resultAttendee = LegislationAttendees.Where(x => x.Id == newAttendee.Id).FirstOrDefault();
                    if (resultAttendee != null)
                    {
                        LegislationAttendees.Remove(resultAttendee);
                    }
                }
                LegislationAttendeeGrid.Reset();
                ddlLegislationAttendees.Reset();
                await LegislationAttendeeGrid.Reload();
            }
            Reload();
        }

        protected async Task DeleteLegislationAttendee(FatwaAttendeeVM attendee)
        {
            saveMeeting.DeletedLegislationAttendeeIds.Add(Guid.Parse(attendee.Id));
            atttendeeLegislationSerialNo = atttendeeLegislationSerialNo--;
            GetLegislationAttendees.Remove(attendee);
            await PopulateLegislationAttendees();
            LegislationAttendeeGrid.Reset();
            AllApproved = GetLegislationAttendees.ToList().All(x => x.AttendeeStatusId != (int)MeetingAttendeeStatusEnum.NotResponded && x.AttendeeStatusId != (int)MeetingAttendeeStatusEnum.New && x.AttendeeStatusId != (int)MeetingAttendeeStatusEnum.Pending);
            await LegislationAttendeeGrid.Reload();
        }

        #endregion

        #region Action Button
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
        protected async Task SaveAndSendToAttendees()
        {
            try
            {
                saveMeeting.iSSaveAndSendToAttendees = true;
                await FormSubmit(saveMeeting);
            }
            catch (Exception)
            {

                throw;
            }
        }
        protected async Task SubmitApprovalRequest()
        {
            try
            {
                saveMeeting.Meeting.IsApproved = true;
                await FormSubmit(saveMeeting);
            }
            catch (Exception)
            {

                throw;
            }
        }
		protected async Task SubmitRejectRequest()
		{
			try
			{
				saveMeeting.Meeting.IsApproved = true;
				await FormRejectSubmit(saveMeeting);
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

        protected async Task FormSubmit(SaveMeetingVM saveMeeting)
        {
            try
            {
                ObservableCollection<TempAttachementVM> attachments = await fileUploadService.GetTempAttachements(Guid.Parse(ReferenceId)); 
                if ( attachments.Where(x => x.CommunicationGuid == Guid.Parse(CommunicationId) && x.AttachmentTypeId == (int)AttachmentTypeEnum.ReplytoMeetingRequest).Select(y=> y.FileName).FirstOrDefault() != null)
                {
                    bool? dialogResponse = false;
                    string dialogMessage = string.Empty;
                    dialogMessage = translationState.Translate("Sure_Submit");
                    if (isRequestForMeetingDecision) // for approval scenario
                    {
                        if (GetLegislationAttendees.Count == 0)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Please_Select_Atleast_One_Attendee"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            return;
                        }
                        else
                        {
                            if (saveMeeting.LegislationAttendee.Count() != 0)
                            {
                                saveMeeting.LegislationAttendee = new List<FatwaAttendeeVM>();
                            }
                            saveMeeting.LegislationAttendee = GetLegislationAttendees;
                        }
                        if (!saveMeeting.Meeting.IsReplyForMeetingRequest)
                        {
                            saveMeeting.GeAttendee = GetGeAttendees;
                        }
                    }
                    dialogResponse = await dialogService.Confirm(
                            dialogMessage,
                            translationState.Translate("Confirm"),
                            new ConfirmOptions()
                            {
                                OkButtonText = translationState.Translate("OK"),
                                CancelButtonText = translationState.Translate("Cancel")
                            });

                    if (dialogResponse == true)
                    {
                        ApiCallResponse response = null;
                        if (isRequestForMeetingDecision)
                        {
                            var userIdToCheck = loginState.UserDetail.UserId;

                            if (saveMeeting.LegislationAttendee.Any(attendee => attendee.Id == userIdToCheck))
                            {
                                var attendeeToUpdate = saveMeeting.LegislationAttendee.First(attendee => attendee.Id == userIdToCheck);
                                attendeeToUpdate.AttendeeStatusId = (int)MeetingAttendeeStatusEnum.Accept;
                            }
                        }

                        response = await meetingService.TakeRequestForMeetingDecisionFromFatwa(saveMeeting);
                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Decicion_Save_Success"),
                                Style = "position: fixed !important; left: 0; margin: auto;"
                            });
                            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
                        }
                    } 
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("RequiredDocument"), 
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

		protected async Task FormRejectSubmit(SaveMeetingVM saveMeeting)
		{
			try
			{
                ObservableCollection<TempAttachementVM> tempAttachments = await fileUploadService.GetTempAttachements(Guid.Parse(ReferenceId));
                if (tempAttachments.Where(x => x.CommunicationGuid == Guid.Parse(CommunicationId) && x.AttachmentTypeId == (int)AttachmentTypeEnum.ReplytoMeetingRequest).Select(y => y.FileName).FirstOrDefault() != null)
                {
                    bool? dialogResponse = false;
                    string dialogMessage = string.Empty;
                    dialogMessage = translationState.Translate("Sure_Submit");
                    dialogResponse = await dialogService.Confirm(
                            dialogMessage,
                            translationState.Translate("Confirm"),
                            new ConfirmOptions()
                            {
                                OkButtonText = translationState.Translate("OK"),
                                CancelButtonText = translationState.Translate("Cancel")
                            });

                    if (dialogResponse == true)
                    {
                        ApiCallResponse response = null;

                        response = await meetingService.TakeRequestForMeetingDecisionFromFatwa(saveMeeting);
                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Decicion_Save_Success"),
                                Style = "position: fixed !important; left: 0; margin: auto;"
                            });
                            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
                        }
                    }
                }
                else
                { 
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("RequiredDocument"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
                }
			}
			catch (Exception)
			{

				throw;
			}
		}
		#endregion

		#region Redirect Function
		private async Task PreviousPageUrl()
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
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion

	}
}

