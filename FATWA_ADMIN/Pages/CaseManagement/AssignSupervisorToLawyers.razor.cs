using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.UserEnum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_ADMIN.Pages.CaseManagement
{
    //< History Author = 'Hassan Abbas' Date = '2023-03-01' Version = "1.0" Branch = "master" Assign Supervisor To Lawyers</History>

    public partial class AssignSupervisorToLawyers : ComponentBase
    {
        #region Variables

        protected List<OperatingSectorType> SectorTypes { get; set; } = new List<OperatingSectorType>();
        protected CmsLawyerSupervisorVM lawyerSupervisor { get; set; } = new CmsLawyerSupervisorVM();
        public int SectorId { get; set; }
        protected IEnumerable<ListLawyerSupervisorAssignmentVM> assignSupervisorLawyers { get; set; }
        protected IEnumerable<ListLawyerSupervisorAssignmentVM> filteredSupervisorsAndManagers { get; set; }
        protected IEnumerable<LawyerVM> supervisors { get; set; }
        protected List<ManagersListVM> ManagersList { get; set; }
        protected RadzenDataGrid<ListLawyerSupervisorAssignmentVM> supervisorsAndManagersGrid = new RadzenDataGrid<ListLawyerSupervisorAssignmentVM>();
        protected bool allowRowSelectOnRowClick = true;
        protected string search;
        private Timer debouncer;
        private const int debouncerDelay = 500;
        #endregion

        #region On Load

        protected override async Task OnInitializedAsync()
        {
            await PopulateSectorTypes();
        }
        #endregion

        #region Dropdownlist and On Change Events

        protected async Task PopulateSectorTypes()
        {

            var response = await lookupService.GetOperatingSectorTypes();
            if (response.IsSuccessStatusCode)
            {
                SectorTypes = (List<OperatingSectorType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        protected async Task PopulateAssignLawyerSupervisorList()
        {
            var userresponse = await lookupService.GetLawyerSupervisorAssignmentListBySector(SectorId);
            if (userresponse.IsSuccessStatusCode)
            {
                assignSupervisorLawyers = (IEnumerable<ListLawyerSupervisorAssignmentVM>)userresponse.ResultData;
                filteredSupervisorsAndManagers = (IEnumerable<ListLawyerSupervisorAssignmentVM>)userresponse.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(userresponse);
            }

        }
        protected async Task PopulateSupervisorlist()
        {
            var userresponse = await lookupService.GetSupervisorsBySector(SectorId);
            if (userresponse.IsSuccessStatusCode)
            {
                supervisors = (IEnumerable<LawyerVM>)userresponse.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(userresponse);
            }

        }
        protected async Task populateManagersList()
        {
            var response = await userService.GetManagersList(SectorId, (int)DesignationEnum.Lawyer);
            if (response.IsSuccessStatusCode)
            {
                ManagersList = ((List<ManagersListVM>)response.ResultData);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();

        }


        protected async Task OnSectorChange(object args)
        {
            try
            {
                if (args != null)
                {
                    await PopulateAssignLawyerSupervisorList();
                    await PopulateSupervisorlist();
                    await populateManagersList();
                }
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Summary = translationState.Translate("Error"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion

        #region Grid Search
        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
                    {
                        filteredSupervisorsAndManagers = await gridSearchExtension.Filter(assignSupervisorLawyers, new Query()
                        {
                            Filter = $@"i => (i.SupervisorNameEn != null && i.SupervisorNameEn.ToLower().Contains(@0)) || (i.ManagerNameEn != null && i.ManagerNameEn.ToLower().Contains(@1)) || (i.UserNameEn != null && i.UserNameEn.ToLower().Contains(@2))",
                            FilterParameters = new object[] { search, search, search }
                        });
                    }
                    else
                    {
                        filteredSupervisorsAndManagers = await gridSearchExtension.Filter(assignSupervisorLawyers, new Query()
                        {
                            Filter = $@"i => (i.SupervisorNameAr != null && i.SupervisorNameAr.ToLower().Contains(@0)) || (i.ManagerNameAr != null && i.ManagerNameAr.ToLower().Contains(@1)) || (i.UserNameAr != null && i.UserNameAr.ToLower().Contains(@2))",
                            FilterParameters = new object[] { search, search, search, search, search, search }
                        });
                    }
                    await InvokeAsync(StateHasChanged);
                }, null, debouncerDelay, Timeout.Infinite);
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

        #region Functions

        public async Task FormSubmit()
        {
            //if (approvalTracking.SectorTo > 0 && approvalTracking.Remarks != null)
            //{
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
                ApiCallResponse response = null;

                response = await lookupService.AssignSupervisorAndManagerToLawyers(lawyerSupervisor);

                if (response.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Changes_saved_successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    dialogService.Close();
                    dialogService.Close(1);
                    SectorId = 0;
                    lawyerSupervisor = new CmsLawyerSupervisorVM();
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            else
            {
                dialogService.Close();
                dialogService.Close(1);
            }
            //}
            //else
            //{
            //    typeValidationMsgSector = approvalTracking.SectorTo > 0 ? "" : @translationState.Translate("Required_Field");
            //    typeValidationMsgReason = approvalTracking.Remarks != null ? "" : @translationState.Translate("Required_Field_Reason");
            //}
        }


        protected void ButtonCancelClick(MouseEventArgs args)
        {
            navigationManager.NavigateTo("index");
        }

        #region Redirect Functions
        public void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        public void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion

        #endregion

    }
}
