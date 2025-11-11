using FATWA_DOMAIN.Models.BugReporting;
using FATWA_DOMAIN.Models.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_DOMAIN.Enums.BugReporting.BugReportingEnum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_ADMIN.Pages.BugReporting
{
    public partial class BugTypeModuleAssignment : ComponentBase
    {
        #region Parameter
        [Parameter]
        public int TypeId { get; set; }
        [Parameter]
        public int Assignment { get; set; }
        #endregion

        #region Variables
        protected List<BugIssueType> issueTypes { get; set; } = new List<BugIssueType>();
        public BugModuleTypeAssignment bugModuleType { get; set; } = new BugModuleTypeAssignment();
        protected List<BugApplication> Applications { get; set; } = new List<BugApplication>();
        protected List<BugModule> Modules { get; set; } = new List<BugModule>();
        protected List<BugModuleTypeAssignment> assignedModule { get; set; } = new List<BugModuleTypeAssignment>();
        protected List<Priority> priorities { get; set; } = new List<Priority>();
        protected List<BugSeverity> severities { get; set; } = new List<BugSeverity>();
        public string moduleValidationMessage { get; set; } = "";
        public string applicationValidationMessage { get; set; } = "";
        #endregion

        #region OnInitialized
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            bugModuleType.BugTypeId = TypeId;
            await PopulateDropDowns();
            spinnerService.Hide();
        }
        #endregion

        #region Populate Dropdowns
        protected async Task PopulateDropDowns()
        {
            await PopulateIssueTypes();
            await PopulateApplications();
            await PopulatePriorityList();
            await PopulateSeverity();
            await PopulateAssignedModuleByTypeId();
        }
        #endregion

        #region Populate Data
        protected async Task PopulateIssueTypes()
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
        protected async Task PopulateApplications()
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
                throw new Exception(ex.Message);
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
                    if (Assignment == (int)IssueTypeAssignmentEnum.Assign)
                    {
                        if (assignedModule.Count() > 0)
                        {
                            bugModuleType.SelectedModule = Modules.Where(module => assignedModule.Select(assigned => assigned.ModuleId).Contains(module.Id)).Select(x => x.Id).ToList();
                        }
                    }
                    else
                    {
                        Modules = Modules.Where(module => assignedModule.Select(assigned => assigned.ModuleId).Contains(module.Id)).ToList();
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
            StateHasChanged();
        }
        protected async Task PopulateAssignedModuleByTypeId()
        {
            try
            {
                var response = await bugReportingService.GetAssignTypeModulesById(TypeId);
                if (response.IsSuccessStatusCode)
                {
                    assignedModule = (List<BugModuleTypeAssignment>)response.ResultData;
                    bugModuleType.PriorityId = assignedModule.Select(x => x.PriorityId).FirstOrDefault();
                    bugModuleType.SeverityId = assignedModule.Select(x => x.SeverityId).FirstOrDefault();
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
        protected async Task PopulatePriorityList()
        {
            try
            {
                var response = await lookupService.GetCasePriorities();
                if (response.IsSuccessStatusCode)
                {
                    priorities = (List<Priority>)response.ResultData;
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
        protected async Task PopulateSeverity()
        {
            try
            {
                var response = await lookupService.GetSeverity();
                if (response.IsSuccessStatusCode)
                {
                    severities = (List<BugSeverity>)response.ResultData;
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
        #endregion

        #region On Change Application
        protected async Task OnChangeApplication(object arg)
        {
            bugModuleType.SelectedModule = new List<int>();
            if (arg != null)
            {
                await PopulateModules((int)arg);
            }
            if (bugModuleType.SelectedModule.Count() == 0)
            {
                bugModuleType.SelectedModule = null;
            }
        }
        protected void OnChangeModuleDropdown()
        {
            if (bugModuleType.SelectedModule != null)
            {
                var res = bugModuleType.SelectedModule.ToList().Count() == 0 ? bugModuleType.SelectedModule = null : bugModuleType.SelectedModule;
            }
        }
        #endregion

        #region Form Submit
        protected async Task FormSubmit(BugModuleTypeAssignment bugModuleTypes)
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
                        ApiCallResponse response = new ApiCallResponse();
                        bugModuleTypes.CreatedBy = loginState.Username;
                        bugModuleTypes.CreatedDate = DateTime.Now;
                        bugModuleTypes.IsDeleted = false;
                        if (Assignment == (int)IssueTypeAssignmentEnum.Assign)
                        {
                            response = await bugReportingService.AssigningTypesModule(bugModuleTypes);
                        }
                        else
                        {
                            response = await bugReportingService.UnAssigningTypesModule(bugModuleTypes);
                        }
                        if (response.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate(Assignment == (int)IssueTypeAssignmentEnum.Assign ? "Assign_TypeModule_Added_Successfully" : "UnAssign_TypeModule_Added_Successfully"),
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
    }
}
