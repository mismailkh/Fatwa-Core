using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.MeetModels;
using FATWA_DOMAIN.Models.ViewModel.Meet;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.MeetingEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;

namespace FATWA_WEB.Pages.Meet
{
    public partial class MeetingDecision : ComponentBase
    {
        #region Variables Declaration

        [Parameter]
        public dynamic meetingId { get; set; }
        [Parameter]
        public dynamic TaskId { get; set; }
        protected List<MeetingStatus> meetingDecisionStatus = new List<MeetingStatus>();
        protected MeetingDecisionVM meetingDecisionVM = new MeetingDecisionVM();
        protected string govtEntityId;
        protected bool IsDisabled;
        public class MeetingStatuses
        {
            public int MeetingStatusId { get; set; }
            public string NameEn { get; set; }
        }
        protected TaskDetailVM taskDetailVM { get; set; } = new TaskDetailVM();
        protected List<ManagerTaskReminderVM> managerTaskReminderData { get; set; } = new List<ManagerTaskReminderVM>();

        #endregion

        #region OnInitialized

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            spinnerService.Hide();
        }

        protected async Task Load()
        {
            await GetAttendeeStatusesByMeetingId();
            if (TaskId != null)
            {
                await PopulateTaskDetails();
                await GetManagerTaskReminderData();
            }

            try
            {
                var sectorId = (int)loginState.UserDetail.SectorTypeId;
                var getDecision = await meetingService.GetMeetingDecisionDetailById(Guid.Parse(meetingId), sectorId);
                if (getDecision.IsSuccessStatusCode)
                {
                    meetingDecisionVM = (MeetingDecisionVM)getDecision.ResultData;
                    await GetGovtEnityByReferencId(meetingDecisionVM.ReferenceGuid, meetingDecisionVM.SubModulId);
                    //meetingDecisionVM.MeetingStatusId = 0;
                    if ((loginState.UserRoles.Any(u => u.RoleId == SystemRoles.HOS)) || (loginState.UserRoles.Any(u => u.RoleId == SystemRoles.ComsHOS)))
                    {
                        meetingDecisionVM.HOSUser = true;
                    }
                    else
                    {
                        meetingDecisionVM.HOSUser = false;
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion

        #region Remote Dropdown Data and Dropdown Change Events
        protected async Task PopulateMeetingStatus()
        {
            var response = await lookupService.GetMeetingStatus();
            if (response.IsSuccessStatusCode)
            {
                var meetingStatus = (List<MeetingStatus>)response.ResultData;
                foreach (MeetingStatus item in meetingStatus)
                {
                    if (item.MeetingStatusId == (int)MeetingStatusEnum.ApprovedByHOS || item.MeetingStatusId == (int)MeetingStatusEnum.RejectedByHOS)
                    {
                        meetingDecisionStatus.Add(new MeetingStatus
                        {
                            MeetingStatusId = item.MeetingStatusId,
                            NameEn = item.NameEn,
                            NameAr = item.NameAr,
                        });
                    }
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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
        #region Get Govt Entity Id By ReferencId
        protected async Task GetGovtEnityByReferencId(Guid ReferenceId, int SubModuleId)
        {
            var response = await cmsSharedService.GetGovtEnityByReferencId(ReferenceId, SubModuleId);
            if (response.IsSuccessStatusCode)
            {
                govtEntityId = (string)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion
        #endregion

        #region Get Attendee Statuses
        // if initiator add more attendees after sending request to HOS from this function it will disabled the submit button
        protected async Task GetAttendeeStatusesByMeetingId()
        {
            var response = await meetingService.GetAttendeeStatusesByMeetingId(Guid.Parse(meetingId));
            if (response.IsSuccessStatusCode)
            {
                IsDisabled = response.ResultData;
            }
        }

        #endregion

        #region Submit Form

        protected async Task FormSubmit(MeetingDecisionVM args)
        {
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
                   translationState.Translate("Are_you_sure_you_want_to_save_this_change"),
                   translationState.Translate("Confirm"),
                   new ConfirmOptions()
                   {
                       OkButtonText = @translationState.Translate("OK"),
                       CancelButtonText = @translationState.Translate("Cancel")
                   });
                if (dialogResponse == true)
                {
                    spinnerService.Show();
                    if (meetingDecisionVM.MeetingTypeId == (int)MeetingTypeEnum.Internal && meetingDecisionVM.MeetingStatusId == (int)MeetingStatusEnum.ApprovedByHOS)
                    {
                        meetingDecisionVM.IsApproved = true;
                        meetingDecisionVM.MeetingStatusId = (int)MeetingStatusEnum.Scheduled;
                    }
                    else if (meetingDecisionVM.MeetingTypeId == (int)MeetingTypeEnum.Internal && (meetingDecisionVM.MeetingStatusId == (int)MeetingStatusEnum.RejectedByHOS) || (meetingDecisionVM.MeetingStatusId == (int)MeetingStatusEnum.RejectedByViceHos))
                    {
                        meetingDecisionVM.IsApproved = false;
                        meetingDecisionVM.MeetingStatusId = (int)MeetingStatusEnum.OnHold;
                    }
                    else if (meetingDecisionVM.MeetingTypeId == (int)MeetingTypeEnum.External && meetingDecisionVM.MeetingStatusId == (int)MeetingStatusEnum.ApprovedByHOS)
                    {
                        meetingDecisionVM.IsApproved = true;
                    }
                    else if (meetingDecisionVM.MeetingTypeId == (int)MeetingTypeEnum.External && (meetingDecisionVM.MeetingStatusId == (int)MeetingStatusEnum.RejectedByHOS) || (meetingDecisionVM.MeetingStatusId == (int)MeetingStatusEnum.RejectedByViceHos))
                    {
                        meetingDecisionVM.IsApproved = false;
                        meetingDecisionVM.MeetingStatusId = (int)MeetingStatusEnum.OnHold;
                    }
                    meetingDecisionVM.SectorTypeId = loginState.UserDetail.SectorTypeId;
                    //meetingDecisionVM.SentBy = loginState.Username;
                    meetingDecisionVM.MeetingId = Guid.Parse(meetingId);
                    meetingDecisionVM.GovtEntityId = Convert.ToInt32(govtEntityId);
                    meetingDecisionVM.LoggedInUser = Guid.Parse(loginState.UserDetail.UserId);
                    var response = await meetingService.UpdateMeetingDecision(meetingDecisionVM);
                    if (response.IsSuccessStatusCode)
                    {
                        if (TaskId != null)
                        {
                            if (taskDetailVM.Url.StartsWith("meeting-decision"))
                            {

                                taskDetailVM.Url = $"meeting-view/{meetingId}/{true}";

                            }
                            taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
                            taskDetailVM.Name = "Save_Meeting_Task";
                            var taskResponse = await taskService.DecisionTaskByStatusAndRefId(taskDetailVM);
                            if (!taskResponse.IsSuccessStatusCode)
                            {
                                await invalidRequestHandlerService.ReturnBadRequestNotification(taskResponse);
                            }
                        }

                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("SaveMeeting_Success"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        spinnerService.Hide();
                        await RedirectBack();
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                }

            }
            catch (Exception)
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

        #region Redirect Function

        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }

        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }

        protected void ButtonCancelClick()
        {
            navigationManager.NavigateTo("/meeting-list");
        }
        protected void Accept()
        {
            if ((bool)meetingDecisionVM.HOSUser)
            {
                meetingDecisionVM.MeetingStatusId = (int)MeetingStatusEnum.ApprovedByHOS;

            }
            else
            {
                if (!(bool)meetingDecisionVM.HosApprovalRequire)
                {
                    meetingDecisionVM.MeetingStatusId = (int)MeetingStatusEnum.ApprovedByViceHos;
                }
                else
                {
                    meetingDecisionVM.MeetingStatusId = (int)MeetingStatusEnum.ApprovedByHOS;
                }
            }
            FormSubmit(meetingDecisionVM);
        }
        protected void Reject()
        {
            meetingDecisionVM.MeetingStatusId = (int)MeetingStatusEnum.RejectedByViceHos;
            FormSubmit(meetingDecisionVM);
        }
        protected void ButtonDetail()
        {
            if (meetingDecisionVM.SubModulId == (int)SubModuleEnum.CaseRequest)
            {
                navigationManager.NavigateTo("/caserequest-view/" + meetingDecisionVM.ReferenceGuid);
            }
            if (meetingDecisionVM.SubModulId == (int)SubModuleEnum.CaseFile)
            {
                navigationManager.NavigateTo("/casefile-view/" + meetingDecisionVM.ReferenceGuid);
            }
            if (meetingDecisionVM.SubModulId == (int)SubModuleEnum.RegisteredCase)
            {
                navigationManager.NavigateTo("/case-view/" + meetingDecisionVM.ReferenceGuid);
            }
            if (meetingDecisionVM.SubModulId == (int)SubModuleEnum.ConsultationRequest)
            {
                meetingDecisionVM.SectorTypeId = loginState.UserDetail.SectorTypeId;
                navigationManager.NavigateTo("/consultationrequest-detail/" + meetingDecisionVM.ReferenceGuid + "/" + meetingDecisionVM.SectorTypeId);
            }
            if (meetingDecisionVM.SubModulId == (int)SubModuleEnum.ConsultationFile)
            {
                meetingDecisionVM.SectorTypeId = loginState.UserDetail.SectorTypeId;
                navigationManager.NavigateTo("/consultationfile-view/" + meetingDecisionVM.ReferenceGuid + "/" + meetingDecisionVM.SectorTypeId);
            }
        }

        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");

        }
        protected async Task EditMeeting()
        {
            navigationManager.NavigateTo("/meeting-add/" + meetingDecisionVM.MeetingId);

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
