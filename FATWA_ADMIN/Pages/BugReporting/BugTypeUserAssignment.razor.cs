using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models.BugReporting;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using static FATWA_DOMAIN.Enums.BugReporting.BugReportingEnum;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_ADMIN.Pages.BugReporting
{
    public partial class BugTypeUserAssignment : ComponentBase
    {
        #region Paramter
        [Parameter]
        public int TypeId { get; set; }
        [Parameter]
        public int AssignmentType { get; set; }
        #endregion

        #region Variables
        protected List<BugIssueType> issueTypes { get; set; } = new List<BugIssueType>();
        protected List<UserDataVM> users { get; set; } = new List<UserDataVM>();
        protected BugUserTypeAssignment assignTypeUser { get; set; } = new BugUserTypeAssignment();
        protected List<BugUserTypeAssignment> assignedGroup { get; set; } = new List<BugUserTypeAssignment>();
        protected List<UserGroupVM> Groups { get; set; } = new List<UserGroupVM> { };
        protected bool DisabledUser = false;
        protected bool DisabledGroup = false;
        #endregion

        #region OnInitialized
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateDropDowns();
            assignTypeUser.BugTypeId = TypeId;
            spinnerService.Hide();
        }
        #endregion

        #region Populate DropDown Data
        protected async Task PopulateDropDowns()
        {
            await PopulateIssuesTypes();
            await PopulateUserList();
            await GetUsersByTypeId();
            await LoadGroups();
            await GetGroupsByTypeId();
        }
        #endregion

        #region Populate Data
        protected async Task PopulateIssuesTypes()
        {
            try
            {
                var response = await lookupService.GetIssuesTypes();
                if (response.IsSuccessStatusCode)
                {
                    issueTypes = (List<BugIssueType>)response.ResultData;
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
            StateHasChanged();
        }
        protected async Task PopulateUserList()
        {
            try
            {
                var response = await lookupService.GetUserByRoleIdAndSectorId(SystemRoles.ITSupport, (int)OperatingSectorTypeEnum.InformationTechnology);
                if (response.IsSuccessStatusCode)
                {
                    users = (List<UserDataVM>)response.ResultData;
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
        protected async Task LoadGroups()
        {
            var response = await lookupService.GetGroupsByRoleIdandSectorId(SystemRoles.ITSupport, (int)OperatingSectorTypeEnum.InformationTechnology);
            if (response.IsSuccessStatusCode)
            {
                Groups = (List<UserGroupVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        #endregion

        #region Get Users By Type Id
        protected async Task GetUsersByTypeId()
        {
            try
            {
                var response = await lookupService.GetUsersByBugTypeId(TypeId);
                if (response.IsSuccessStatusCode)
                {
                    if (AssignmentType == (int)IssueTypeAssignmentEnum.Assign)
                    {
                        var users = (List<UserDataVM>)response.ResultData;
                        assignTypeUser.selectedUserIds = users.Select(x => x.UserId).ToList();
                    }
                    else
                    {
                        users = (List<UserDataVM>)response.ResultData;
                    }
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

        #region Get User Group By Bug Type Id
        protected async Task GetGroupsByTypeId()
        {
            var response = await bugReportingService.GetGroupsByBugTypeId(TypeId);
            if (response.IsSuccessStatusCode)
            {

                assignedGroup = (List<BugUserTypeAssignment>)response.ResultData;
                if (AssignmentType == (int)IssueTypeAssignmentEnum.Assign)
                {
                    assignTypeUser.selectedGroupIds = assignedGroup.Select(x => x.GroupId).Cast<Guid>().ToList();
                }
                else
                {
                    Groups = Groups.Where(x => assignedGroup.Select(t => t.GroupId).Distinct().ToList().Contains(x.GroupId)).ToList();
                }
                StateHasChanged();
            }
        }
        #endregion

        #region Form Submit
        protected async Task FormSubmit(BugUserTypeAssignment assignBugTypeUser)
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
                        ApiCallResponse response = new ApiCallResponse();
                        spinnerService.Show();
                        assignBugTypeUser.CreatedBy = loginState.Username;
                        assignBugTypeUser.CreatedDate = DateTime.Now;
                        assignBugTypeUser.IsDeleted = false;
                        response = AssignmentType == (int)IssueTypeAssignmentEnum.Assign ? await bugReportingService.AssigningTypesToUser(assignBugTypeUser) : await bugReportingService.UnAssigningTypesToUser(assignBugTypeUser);
                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate(AssignmentType == (int)IssueTypeAssignmentEnum.Assign ? "Bug_Type_Assigned_User_Successfully" : "Bug_Type_Unassigned_User_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            dialogService.Close();
                        }
                        else
                        {
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

        #region Redirect and Dialog Events
        protected async void ButtonCancelClick(MouseEventArgs args)
        {
            dialogService.Close(false);
        }
        #endregion

        #region OnChange Group
        protected async Task OnChangeGroup()
        {
            DisabledUser = true;
        }
        #endregion
    }
}
