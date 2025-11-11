using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.ViewModel.OrganizingCommitteeVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.OrganizingCommittee.OrganizingCommitteeEnum;

namespace FATWA_WEB.Pages.OrganizingCommittee
{
    public partial class DetailCommittee:ComponentBase
    {
        #region Parameter
        [Parameter]
        public string CommitteeId { get; set; }
        [Parameter]
        public string TaskId { get; set; }
        #endregion

        #region Variables
        protected CommitteeDetailsVm Committee = new CommitteeDetailsVm();
        protected List<CommitteeMembersVMs> committeeMembers = new List<CommitteeMembersVMs>();
        protected RadzenGrid<CommitteeMembersVMs> committeeMembersGrid = new RadzenGrid<CommitteeMembersVMs>();
        protected CommitteeMembersVMs CommitteeMember { get; set; }
        protected List<CommitteeTaskVm> committeeTasks = new List<CommitteeTaskVm>();
        protected bool IsReadOnly = true;
        protected bool disable = false;
        protected bool Visible = false;
        protected bool IsVisible = false;
        public IEnumerable<RejectionReasonVm> rejectReason = new List<RejectionReasonVm>();
        public bool Update { get; set; } = true;
        protected bool Keywords = false;
        protected List<ManagerTaskReminderVM> managerTaskReminderData { get; set; } = new List<ManagerTaskReminderVM>();

        #endregion

        #region OnInitialized
        protected override async Task OnInitializedAsync()
        {
            await Load();
        }
        #endregion

        #region Load
        public async Task Load()
        {
            spinnerService.Show();
            await GetCommitteeDetail();
            await GetCommitteeMembers();
            await GetCommitteeTask();
            await GetRejection();
            if (TaskId != null)
            {
                await GetManagerTaskReminderData();
            }
            spinnerService.Hide();
        }
        #endregion

        #region Get Committee  Details
        protected async Task GetCommitteeDetail()
        {
            try
            {
                var response = await organizingCommitteeService.GetCommitteeDetail(Guid.Parse(CommitteeId));
                if (response.IsSuccessStatusCode)
                {
                    Committee = (CommitteeDetailsVm)response.ResultData;
                    if (Committee.StatusId == (int)CommitteeStatusEnum.Dissolved || Committee.StatusId == (int)CommitteeStatusEnum.Draft)
                    {
                        disable = true;
                    }
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Committee Members
        protected async Task GetCommitteeMembers()
        {
            try
            {
                var response = await organizingCommitteeService.GetCommitteeMembers(Committee.Id);
                if (response.IsSuccessStatusCode)
                {
                    committeeMembers = (List<CommitteeMembersVMs>)response.ResultData;
                    if (committeeMembers != null)
                    {
                        foreach (var member in committeeMembers)
                        {

                            if (member.MemberId == loginState.UserDetail.UserId)
                            {
                                if (!(bool)member.IsReadOnly)
                                {
                                    IsReadOnly = false;
                                }
                                if (member.CommitteeRoleId == (int)CommitteeRoleEnum.CommitteeHead || member.CommitteeRoleId == (int)CommitteeRoleEnum.Organizer || member.CommitteeRoleId == (int)CommitteeRoleEnum.ViceCommitteeHead)
                                {
                                    Visible = true;
                                }
                                else if (member.CommitteeRoleId == (int)CommitteeRoleEnum.Spectator)
                                {
                                    disable = true;
                                }

                            }
                            if (member.CommitteeRoleId == (int)CommitteeRoleEnum.CommitteeHead)
                            {
                                Committee.MemeberNameEn = member.MemeberNameEn;
                                Committee.MemeberNameAr = member.MemeberNameAr;
                            }
                        }
                    }
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Committee Task
        protected async Task GetCommitteeTask()
        {
            try
            {
                var response = await organizingCommitteeService.GetCommitteeTask(Committee.Id, Committee.StatusId);
                if (response.IsSuccessStatusCode)
                {
                    committeeTasks = (List<CommitteeTaskVm>)response.ResultData;
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Rejection Reason
        protected async Task GetRejection()
        {
            try
            {
                var response = await organizingCommitteeService.GetRejection(Guid.Parse(CommitteeId));
                if (response.IsSuccessStatusCode)
                {
                    rejectReason = (List<RejectionReasonVm>)response.ResultData;
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Add New Members
        protected async Task AddMembers(Guid Id)
        {
            try
            {
                await dialogService.OpenAsync<AddMember>(
                    translationState.Translate("Add_Committee_Members"),
                    new Dictionary<string, object>() { { "CommitteId", Id }, { "CommitteeNumber", Committee.CommitteeNumber } },
                    new DialogOptions() { Width = "45%", CloseDialogOnOverlayClick = true }
                );
                await Load();
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

        #region Assign New Task
        protected async Task AssignNewTask(Guid Id)
        {
            try
            {
                await dialogService.OpenAsync<AddMemberTask>(
                    translationState.Translate("Add_Member_Task"),
                    new Dictionary<string, object>() { { "CommitteeId", Id } },
                    new DialogOptions() { Width = "45%", CloseDialogOnOverlayClick = true }
                    );
                await Load();
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

        #region Edit Committee Task
        protected async Task EditCommitteeTask(Guid taskId)
        {
            try
            {
                await dialogService.OpenAsync<AddMemberTask>(
                    translationState.Translate("Add_Member_Task"),
                    new Dictionary<string, object>() { { "TaskId", taskId }, { "CommitteeId", Committee.Id }, { "Update", Update } },
                    new DialogOptions() { Width = "60%", CloseDialogOnOverlayClick = true }
                    );
                await Load();
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

        #region Remove Committee Members
        protected async Task RemoveMembers(CommitteeMembersVMs args)
        {
            try
            {
                if (args.CommitteeRoleId == (int)CommitteeRoleEnum.CommitteeHead || args.CommitteeRoleId == (int)CommitteeRoleEnum.Organizer)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate(args.CommitteeRoleId == (int)CommitteeRoleEnum.CommitteeHead ? "TeamLeader_NotDeleted" : "Organizer_NotDeleted"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    Keywords = true;
                    return;
                }
                if (await dialogService.Confirm(translationState.Translate("Sure_Delete"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    var response = await organizingCommitteeService.DeleteMembersById(args);
                    if (response.IsSuccessStatusCode)
                    {

                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Committee_Member_Deleted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        await Load();
                        StateHasChanged();
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    spinnerService.Hide();
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

        #region Remove Temp TAsk
        protected async Task RemoveTempTask(Guid taskId)
        {
            try
            {

                if (await dialogService.Confirm(translationState.Translate("Sure_Delete"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    var response = await organizingCommitteeService.DeleteTempTask(taskId);
                    if (response.IsSuccessStatusCode)
                    {

                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Committee_Task_Deleted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        await Load();
                        StateHasChanged();
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    spinnerService.Hide();
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

        #region Change Accesss To Read only
        protected async Task ChangeAccessToReadOnly()
        {
            try
            {
                await dialogService.OpenAsync<UpdateAccess>(
                    translationState.Translate("Change_Access_To_Read_Only"),
                    new Dictionary<string, object>() { { "CommitteId", Committee.Id } },
                    new DialogOptions() { Width = "40%", CloseDialogOnOverlayClick = true }
                );
                await Load();
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

        #region Dissolved Committee
        protected async Task DissolveCommittee()
        {
            try
            {
                await dialogService.OpenAsync<DissolvedCommittee>(
                    translationState.Translate("Dissolved_Committee"),
                    new Dictionary<string, object>() { { "CommitteId", Committee.Id } },
                    new DialogOptions() { Width = "40%", CloseDialogOnOverlayClick = true }
                );
                await Load();

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

        #region Meeting Request
        protected async Task MeetingRequest(MouseEventArgs args)
        {
            try
            {
                string ReferenceId = CommitteeId;
                int SubModuleId = (int)SubModuleEnum.OrganizingCommittee;
                string ReceivedBy = System.Net.WebUtility.UrlEncode(Committee.CreatedBy).Replace(".", "%999");
                if (TaskId == null)
                {
                    navigationManager.NavigateTo("/meeting-add/" + ReferenceId + "/" + SubModuleId + "/" + ReceivedBy);
                }
                else
                {
                    navigationManager.NavigateTo("/meeting-add/" + ReferenceId + "/" + SubModuleId + "/" + ReceivedBy + "/" + TaskId + "/" + true);

                }
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = ex.Message,
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion

        #region View Details
        protected async Task ViewDetails(Guid Id)
        {
            navigationManager.NavigateTo("/usertask-detail/" + Id);
        }
        #endregion

        #region Edit Members
        protected async Task EditCommitteeMembers(Guid? CommitteeId, string? Id)
        {
            try
            {
                await dialogService.OpenAsync<AddMember>(
                    translationState.Translate("Add_Committee_Members"),
                    new Dictionary<string, object>() { { "CommitteId", CommitteeId }, { "MemberId", Id } },
                    new DialogOptions() { Width = "60%", CloseDialogOnOverlayClick = true }
                );
                await Load();

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

        #region Button Click Event
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
