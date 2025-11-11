using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel.GeneralVMs;
using FATWA_DOMAIN.Models.ViewModel.OrganizingCommitteeVMs;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.OrganizingCommittee.OrganizingCommitteeEnum;

namespace FATWA_WEB.Pages.OrganizingCommittee
{
    public partial class UpdateAccess:ComponentBase
    {
        #region Inject
        [Inject]
        public ProcessLogService processLogService { get; set; }
        [Inject]
        public ErrorLogService errorLogService { get; set; }
        #endregion

        #region Parameters
        [Parameter]
        public Guid CommitteId { get; set; }

        #endregion

        #region Variables
        protected RadzenDataGrid<CommitteeMembersVMs>? memberGrid { get; set; }
        protected List<CommitteeMembersVMs> committeeMembers = new List<CommitteeMembersVMs>();
        protected List<CommitteeMembersVMs> Filtermembers = new List<CommitteeMembersVMs>();
        public List<CommitteeMembersVMs> SelectedMemberList = new List<CommitteeMembersVMs>();
        public List<CommitteeMembersVMs> UnSelectedMemberList = new List<CommitteeMembersVMs>();
        public UpdateMemberAccessVm updateMemberAccess = new UpdateMemberAccessVm();
        public bool allowRowSelectOnRowClick = true;

        #endregion


        #region OnInitialized
        protected override async Task OnInitializedAsync()
        {
            await GetCommitteeMembers();
        }
        #endregion

        #region On Select List
        protected async Task OnSelectList(CommitteeMembersVMs membersVMs, bool isChecked)
        {
            if (membersVMs != null)
            {
                if (isChecked)
                {
                    if (!SelectedMemberList.Contains(membersVMs))
                    {
                        membersVMs.IsReadOnly = true;
                        SelectedMemberList.Add(membersVMs);
                        updateMemberAccess.checkedMembers.Add(membersVMs);
                    }
                }
                else
                {
                    if (SelectedMemberList.Contains(membersVMs))
                    {
                        SelectedMemberList.Remove(membersVMs);
                        membersVMs.IsReadOnly = false;
                        if (updateMemberAccess.checkedMembers.Contains(membersVMs))
                        {
                            updateMemberAccess.checkedMembers.Remove(membersVMs);
                        }
                        updateMemberAccess.UncheckedMembers.Add(membersVMs);

                    }
                }
            }
            else
            {
                if (isChecked)
                {
                    SelectedMemberList.Clear();
                    SelectedMemberList.AddRange(Filtermembers);
                    foreach (var member in Filtermembers)
                    {
                        member.IsReadOnly = true;
                    }
                    updateMemberAccess.checkedMembers.AddRange(SelectedMemberList);
                    updateMemberAccess.UncheckedMembers = new List<CommitteeMembersVMs>();

                }
                else
                {
                    foreach (var member in SelectedMemberList)
                    {
                        member.IsReadOnly = false;
                    }
                    updateMemberAccess.UncheckedMembers.AddRange(SelectedMemberList);
                    updateMemberAccess.checkedMembers = new List<CommitteeMembersVMs>();

                    SelectedMemberList.Clear();
                }
            }
            await InvokeAsync(StateHasChanged);
        }
        #endregion

        #region Get Committee Members
        protected async Task GetCommitteeMembers()
        {
            try
            {
                var response = await organizingCommitteeService.GetCommitteeMembers(CommitteId);
                if (response.IsSuccessStatusCode)
                {
                    committeeMembers = (List<CommitteeMembersVMs>)response.ResultData;
                    Filtermembers = committeeMembers.Where(x => x.CommitteeRoleId == (int)CommitteeRoleEnum.TeamMember || x.CommitteeRoleId == (int)CommitteeRoleEnum.ViceCommitteeHead).ToList();

                    if (SelectedMemberList == null)
                    {
                        SelectedMemberList = new List<CommitteeMembersVMs>();
                    }
                    SelectedMemberList.Clear();

                    if (Filtermembers != null)
                    {
                        foreach (var member in Filtermembers)
                        {
                            if ((bool)member.IsReadOnly)
                            {
                                SelectedMemberList.Add(member);
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

        #region Update Members
        protected async Task Update()
        {
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
                    translationState.Translate("Sure_Submit"),
                    translationState.Translate("Submit"),
                    new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    });
                if (dialogResponse != null)
                {
                    if ((bool)dialogResponse)
                    {
                        spinnerService.Show();
                        updateMemberAccess.FilteredMembers = Filtermembers;

                        var response = await organizingCommitteeService.UpdateMembersAccess(updateMemberAccess);
                        if (response.IsSuccessStatusCode)
                        {
                            var apiResponse = (ApiReturnTaskNotifAuditLogVM)response.ResultData;
                            if (apiResponse.sendNotifications.Count > 0)
                            {
                                await notificationDetailService.SendNotification(apiResponse.sendNotifications);
                            }
                            if (apiResponse.processLog != null)
                            {
                                await processLogService.CreateProcessLog(apiResponse.processLog);
                            }
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Members_Access_Update_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            dialogService.Close();
                            await ReloadPage();
                        }
                        else
                        {
                            var errorLog = (ErrorLog)response.ResultData;
                            if (errorLog != null)
                            {
                                await errorLogService.CreateErrorLog(errorLog);
                            }
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }
                        spinnerService.Hide();
                    }
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

        #region Cancel
        protected async Task CloseDialog(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
        #endregion

        #region Reload Page
        protected async Task ReloadPage()
        {
            await JsInterop.InvokeVoidAsync("refreshPage");
        }
        #endregion
    }
}
