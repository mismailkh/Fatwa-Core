using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models.BugReporting;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.BugReportingVMs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_DOMAIN.Enums.BugReporting.BugReportingEnum;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
namespace FATWA_ADMIN.Pages.BugReporting
{
    public partial class TagTicketBug : ComponentBase
    {
        #region Parameters
        [Parameter]
        public Guid TicketId { get; set; }
        [Parameter]
        public Guid BugId { get; set; }
        #endregion

        #region Variables
        protected IEnumerable<ReportedBugListVM> detailReportedBug { get; set; }
        public ReportedBugListVM selectedBug { get; set; } = new ReportedBugListVM();
        public bool allowRowSelectOnRowClick = true;
        public List<BugIssueType> issueTypes = new List<BugIssueType>();
        public TicketAssignmentVM ticketAssignment = new TicketAssignmentVM();
        protected IEnumerable<BugApplication> Applications { get; set; }
        protected IEnumerable<BugModule> Modules { get; set; }
        protected IEnumerable<BugStatus> BugStatuses { get; set; }
        protected IEnumerable<Priority> Priorities { get; set; }
        protected IEnumerable<BugSeverity> Severities { get; set; }
        protected IEnumerable<BugIssueType> IssueTypes { get; set; }
        public string reasonValidationMessage { get; set; } = "";
        public class AssignmentOptionEnumTemp
        {
            public int OptionEnumValue { get; set; }
            public string OptionEnumName { get; set; }
        }
        public List<AssignmentOptionEnumTemp> assignmentOptions { get; set; } = new List<AssignmentOptionEnumTemp>();
        protected List<UserDataVM> users { get; set; } = new List<UserDataVM>();
        protected List<UserGroupVM> Groups { get; set; } = new List<UserGroupVM>();
        protected List<BugModuleTypeAssignment> assignedModule { get; set; } = new List<BugModuleTypeAssignment>();
        public string PageSummaryFormat { get; set; }

        #endregion

        #region OnInitialized
        protected override async Task OnInitializedAsync()
        {
            await Load();
            PageSummaryFormat = $"{translationState.Translate("Page")} {"{0}"} {translationState.Translate("of")} {"{1}"} ({"{2}"} {translationState.Translate("items")})";
        }
        #endregion
        #region Load 
        protected async Task Load()
        {
            try
            {
                var response = await bugReportingService.GetBugListForTagging();
                if (response.IsSuccessStatusCode)
                {
                    detailReportedBug = (IEnumerable<ReportedBugListVM>)response.ResultData;
                    await InvokeAsync(StateHasChanged);
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                await PopulateAllApplications();
                await PopulatePriorityList();
                await PopulateSeverity();
                await PopulateIssuesTypes();
                await PopulateAssignmentOptions();
                await LoadGroups();
                await PopulateUserList();
                if (BugId != Guid.Empty)
                {
                    ticketAssignment.BugId = BugId;
                    OnChangeBug(BugId);
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

        #region Dropdowns
        protected async Task PopulateAssignmentOptions()
        {
            try
            {
                try
                {
                    foreach (AssingmentOptionEnums item in Enum.GetValues(typeof(AssingmentOptionEnums)))
                    {
                        if (item == AssingmentOptionEnums.User)
                        {
                            assignmentOptions.Add(new AssignmentOptionEnumTemp { OptionEnumName = translationState.Translate("Assign_To_User"), OptionEnumValue = (int)item });
                        }
                        if (item == AssingmentOptionEnums.Group)
                        {
                            assignmentOptions.Add(new AssignmentOptionEnumTemp { OptionEnumName = translationState.Translate("Assign_To_Group"), OptionEnumValue = (int)item });
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

        protected async Task PopulateAllApplications()
        {
            try
            {
                var response = await lookupService.GetAllApplications();
                if (response.IsSuccessStatusCode)
                {
                    Applications = (List<BugApplication>)response.ResultData;
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
        protected async Task PopulatePriorityList()
        {
            try
            {
                var response = await lookupService.GetCasePriorities();
                if (response.IsSuccessStatusCode)
                {
                    Priorities = (List<Priority>)response.ResultData;
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
        protected async Task PopulateModules(int Id)
        {
            try
            {
                var response = await lookupService.GetModulesByApplicationId(Id);
                if (response.IsSuccessStatusCode)
                {
                    Modules = (List<BugModule>)response.ResultData;
                    if (assignedModule.Count() > 0)
                    {
                        Modules = Modules.Where(module => assignedModule.Select(assigned => assigned.ModuleId).Contains(module.Id)).ToList();
                        if (Modules.Count() == 0)
                        {
                            Modules = (List<BugModule>)response.ResultData;
                        }
                    }
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
            StateHasChanged();
        }
        protected async Task PopulateSeverity()
        {
            try
            {
                var response = await lookupService.GetSeverity();
                if (response.IsSuccessStatusCode)
                {
                    Severities = (List<BugSeverity>)response.ResultData;
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
            StateHasChanged();
        }
        protected async Task PopulateIssuesTypes()
        {
            try
            {
                var response = await lookupService.GetIssuesTypes();
                if (response.IsSuccessStatusCode)
                {
                    IssueTypes = (List<BugIssueType>)response.ResultData;
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
            StateHasChanged();
        }
        #endregion

        #region Change Bug 
        public async void OnChangeBug(object value)
        {
            try
            {
                ticketAssignment.BugId = Guid.Parse(value.ToString());
                if (ticketAssignment.BugId != Guid.Empty)
                {
                    selectedBug = detailReportedBug.Where(x => x.Id == ticketAssignment.BugId).FirstOrDefault();
                    ticketAssignment.IssueTypeId = selectedBug.TypeId;
                    ticketAssignment.ApplicationId = selectedBug.ApplicationId;
                    await PopulateModules(Convert.ToInt32(selectedBug.ApplicationId));
                    ticketAssignment.ModuleId = selectedBug.ModuleId;
                    await PopulateAssignedModulesByTypeId(Convert.ToInt32(selectedBug.TypeId));
                    await GetUsersByTypeId(Convert.ToInt32(selectedBug.TypeId));
                    await GetGroupsByTypeId(Convert.ToInt32(selectedBug.TypeId));
                    StateHasChanged();
                }
                else
                {
                    ticketAssignment = new TicketAssignmentVM();
                    assignedModule = new List<BugModuleTypeAssignment>();
                    Modules = new List<BugModule>();
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

        #region On Change Application
        protected async Task OnChangeApplication(object arg)
        {
            try
            {
                ticketAssignment.ModuleId = 0;
                if (arg != null)
                {
                    await PopulateModules((int)arg);
                }
                else
                {
                    Modules = new List<BugModule>();
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

        #region On Change Issue Type
        protected async Task OnChangeIssueType(object arg)
        {
            try
            {
                if (arg != null)
                {
                    await PopulateAssignedModulesByTypeId((int)arg);
                    await GetGroupsByTypeId((int)arg);
                    await GetUsersByTypeId((int)arg);
                }
                else
                {
                    assignedModule = new List<BugModuleTypeAssignment>();
                    ticketAssignment.PriorityId = 0;
                    ticketAssignment.SeverityId = 0;
                }
                ticketAssignment.ApplicationId = 0;
                ticketAssignment.ModuleId = 0;
                ticketAssignment.UserId = string.Empty;
                ticketAssignment.GroupId = Guid.Empty;
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

        #region Populate Assigned Modules By Type Id
        protected async Task PopulateAssignedModulesByTypeId(int IssueTypeId)
        {
            try
            {
                var response = await bugReportingService.GetAssignTypeModulesById(IssueTypeId);
                if (response.IsSuccessStatusCode)
                {
                    assignedModule = (List<BugModuleTypeAssignment>)response.ResultData;
                    if (assignedModule.Count() > 0)
                    {
                        ticketAssignment.PriorityId = assignedModule.FirstOrDefault().PriorityId > 0 ? assignedModule.Select(x => x.PriorityId).FirstOrDefault() : 0;
                        ticketAssignment.SeverityId = assignedModule.FirstOrDefault().SeverityId > 0 ? assignedModule.Select(x => x.SeverityId).FirstOrDefault() : 0;
                    }

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

        #region Get Gropus By Issue Type Id
        protected async Task GetGroupsByTypeId(int IssueTypeId)
        {
            try
            {


                var response = await bugReportingService.GetGroupsByBugTypeId(IssueTypeId);
                if (response.IsSuccessStatusCode)
                {
                    var typeAssignment = (List<BugUserTypeAssignment>)response.ResultData;
                    if (typeAssignment.Count() > 0)
                    {
                        await LoadGroups();
                        Groups = Groups.Where(x => typeAssignment.Select(t => t.GroupId).Distinct().ToList().Contains(x.GroupId)).ToList();
                    }
                    else
                    {
                        await LoadGroups();
                    }
                    StateHasChanged();
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

        #region Load Groups
        protected async Task LoadGroups()
        {
            try
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

        #region Get Users By Type Id
        protected async Task GetUsersByTypeId(int IssueTypeId)
        {
            try
            {
                var response = await lookupService.GetUsersByBugTypeId(IssueTypeId);
                if (response.IsSuccessStatusCode)
                {
                    var assignedTypeUser = (List<UserDataVM>)response.ResultData;
                    if (assignedTypeUser.Count() > 0)
                    {
                        users = assignedTypeUser;
                    }
                    else
                    {
                        await PopulateUserList();
                    }
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

        #region Populate Users By Role And Sector
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
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion

        #region On Change Assignment Option
        protected async Task OnChangeOption()
        {
            ticketAssignment.UserId = string.Empty;
            ticketAssignment.GroupId = Guid.Empty;
        }
        #endregion

        #region Submit 
        protected async Task FormSubmit()
        {
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
                         translationState.Translate("Sure_Submit"),
                         translationState.Translate("Confirm"),
                         new ConfirmOptions()
                         {
                             OkButtonText = @translationState.Translate("Yes"),
                             CancelButtonText = @translationState.Translate("No")
                         });

                if (dialogResponse != null)
                {
                    if ((bool)dialogResponse)
                    {
                        spinnerService.Show();
                        ticketAssignment.TicketId = TicketId;
                        var response = await bugReportingService.TicketTaggingAndAssignment(ticketAssignment);
                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Ticket_Tag_Assigned_Successfully"),
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
                spinnerService.Hide();
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
            bool? dialogResponse = await dialogService.Confirm(
                   translationState.Translate("Sure_Cancel"),
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
                    dialogService.Close();
                }
            }
        }
        #endregion

    }
}
