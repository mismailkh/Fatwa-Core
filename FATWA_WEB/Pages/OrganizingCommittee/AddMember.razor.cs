using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.OrganizingCommittee;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.GeneralVMs;
using FATWA_DOMAIN.Models.ViewModel.OrganizingCommitteeVMs;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using static FATWA_DOMAIN.Enums.OrganizingCommittee.OrganizingCommitteeEnum;

namespace FATWA_WEB.Pages.OrganizingCommittee
{
    public partial class AddMember:ComponentBase
    {
        #region Inject
        [Inject]
        public ProcessLogService processLogService { get; set; }
        [Inject]
        public ErrorLogService errorLogService { get; set; }
        #endregion

        #region parameters
        [Parameter]
        public Guid CommitteId { get; set; }
        [Parameter]
        public string MemberId { get; set; }
        [Parameter]
        public string CommitteeNumber { get; set; }
        #endregion

        #region Variables

        public IEnumerable<CommitteeUserDataVM> usersData { get; set; } = new List<CommitteeUserDataVM>();
        public CommitteeMembers committeeMembersVm { get; set; } = new CommitteeMembers();
        public string userValidationMessage { get; set; } = " ";
        public string roleValidationMessage { get; set; } = " ";
        public IEnumerable<CommitteeRoles> committeeRoles { get; set; } = new List<CommitteeRoles>();
        public IEnumerable<CommitteeUserDataVM> newUsers { get; set; } = new List<CommitteeUserDataVM>();
        protected List<CommitteeMembersVMs> committeeMembers = new List<CommitteeMembersVMs>();
        protected CommitteeMembersVMs committeeMember { get; set; } = new CommitteeMembersVMs();
        public List<OperatingSectorType> operatingSectorTypes { get; set; } = new List<OperatingSectorType>();
        public int SectorTypeId { get; set; }
        public bool update = false;
        #endregion

        #region OnInitialized
        protected override async Task OnInitializedAsync()
        {
            await GetCommitteeMembers();
            await PopulateSectorTypesByDepartmentId();
            await PopulateCommitteeRoles();
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
                    if (MemberId != null)
                    {
                        var committeeMember = committeeMembers.Where(x => x.MemberId == MemberId).FirstOrDefault();
                        committeeMembersVm.MemberId = committeeMember.MemberId;
                        committeeMembersVm.CommitteeRoleId = committeeMember.CommitteeRoleId;
                        update = true;

                    }

                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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

        #region Populate Data  
        protected async Task PopulateSectorTypesByDepartmentId()
        {
            try
            {
                var response = await lookupService.GetOperatingSectorsByDepartmentId(loginState.UserDetail.DepartmentId);
                if (response.IsSuccessStatusCode)
                {
                    operatingSectorTypes = (List<OperatingSectorType>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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
        protected async Task PopulateCommitteeRoles()
        {
            try
            {
                var response = await lookupService.GetCommitteeRoles();
                if (response.IsSuccessStatusCode)
                {
                    committeeRoles = (IEnumerable<CommitteeRoles>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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

        #region Add Members
        protected async Task AddMembers()
        {
            try
            {
                if (committeeMembers.Exists(x => x.CommitteeRoleId == (int)CommitteeRoleEnum.CommitteeHead) && committeeMembersVm.CommitteeRoleId == (int)CommitteeRoleEnum.CommitteeHead)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Team_Leader_Already_Exist"),
                        Style = "position: fixed !important; left: 0; margin: auto; ",
                        Duration = 1000
                    });
                }
                else
                {
                    if (string.IsNullOrEmpty(committeeMembersVm.MemberId) || committeeMembersVm.CommitteeRoleId == 0)
                    {
                        userValidationMessage = string.IsNullOrEmpty(committeeMembersVm.MemberId) ? translationState.Translate("Required_Field") : "";
                        roleValidationMessage = committeeMembersVm.CommitteeRoleId == 0 ? translationState.Translate("Required_Field") : "";
                    }
                    else
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
                            committeeMembersVm.CommitteeId = CommitteId;
                            committeeMembersVm.IsReadOnly = false;
                            committeeMembersVm.CommitteeNumber = CommitteeNumber;
                            committeeMembersVm.IsUpdate = update;
                            var response = await organizingCommitteeService.AddCommitteeMembers(committeeMembersVm);
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
                                    Detail = translationState.Translate(update ? "Member_Successfully_Updated" : "Member_Successfully_Added"),
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

                        }


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

        #region RedirectBack
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion

        #region On Change Sector Type
        protected async Task OnChangeSectorType()
        {
            try
            {
                if (SectorTypeId > 0)
                {
                    await GetUsersBySectorTypeId(SectorTypeId);
                }
                else
                {
                    newUsers = new List<CommitteeUserDataVM>();
                    committeeMembersVm.MemberId = null;
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

        #region Get Users By SectorTypeId
        protected async Task GetUsersBySectorTypeId(int SectorTypeId)
        {
            try
            {
                var response = await userService.GetUsersBySectorTypeId(SectorTypeId);
                if (response.IsSuccessStatusCode)
                {
                    usersData = (IEnumerable<CommitteeUserDataVM>)response.ResultData;
                    newUsers = new List<CommitteeUserDataVM>();
                    foreach (var member in usersData)
                    {
                        newUsers = usersData.Where(user => !committeeMembers.Any(member => member.MemberId == user.UserId)).ToList();
                    }
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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

        #region Reload Page
        protected async Task ReloadPage()
        {
            await JsInterop.InvokeVoidAsync("refreshPage");
        }
        #endregion

        #region Cancel
        protected async Task CloseDialog(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
        #endregion
    }
}
