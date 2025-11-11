using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace FATWA_ADMIN.Pages.ServiceRequest
{
    public partial class ServiceRequestApprovalWorkflowList : ComponentBase
    {
        #region Varriables

        public int count { get; set; }
        protected bool isLoading { get; set; }
        public int? countTemplate { get; set; }
        protected RadzenDataGrid<ServiceRequestApprovalDetailVm>? ServiceReqGrid;
        private string search;
        private Timer debouncer;
        private const int debouncerDelay = 500;
        protected IEnumerable<ServiceRequestApprovalDetailVm> ServiceRequestWorkFlowList { get; set; } = new List<ServiceRequestApprovalDetailVm>();
        protected IEnumerable<ServiceRequestApprovalDetailVm> FilterServiceRequestWorkFlowList { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        #endregion

        #region On Initialized
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Load();
            spinnerService.Hide();
        }
        public async Task Load()
        {
            isLoading = true;
            var response = await lookupService.GetAllServiceRequestApprovalList();
            if (response.IsSuccessStatusCode)
            {
                FilterServiceRequestWorkFlowList = ServiceRequestWorkFlowList = (IEnumerable<ServiceRequestApprovalDetailVm>)response.ResultData;
                // FilteredEvent = (List<NotificationEventListVM>)EventList;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            isLoading = false;
            StateHasChanged();
        }
        #endregion
        #region GRid search 

        protected async Task OnSearchInput(string value)
        {
            try
            {
                debouncer?.Dispose();
                debouncer = new Timer(async (e) =>
                {
                    search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
                    FilterServiceRequestWorkFlowList = await gridSearchExtension.Filter(ServiceRequestWorkFlowList, new Query()
                    {
                        Filter = @"i => 
                          (i.DepartmentNameAr != null && i.DepartmentNameAr.ToLower().Contains(@0)) || 
                          (i.DepartmentNameEn != null && i.DepartmentNameEn.ToLower().Contains(@0)) || 
                          (i.SectorNameEn != null && i.SectorNameEn.ToLower().Contains(@0)) || 
                          (i.NoOfApprovals != null && i.NoOfApprovals.ToString().ToLower().Contains(@0)) || 
                          (i.RoleNameEn != null && i.RoleNameEn.ToLower().Contains(@0)) || 
                          (i.RoleNameAr != null && i.RoleNameAr.ToLower().Contains(@0)) || 
                          (i.ServiceRequestNameAr != null && i.ServiceRequestNameAr.ToLower().Contains(@0)) || 
                          (i.ServiceRequestNameEn != null && i.ServiceRequestNameEn.ToLower().Contains(@0)) || 
                          (i.SectorNameAr != null && i.SectorNameAr.ToLower().Contains(@0))",
                        FilterParameters = new object[] { search }
                    });  await InvokeAsync(StateHasChanged);
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


        #region Edit Service Request Approval Flow
        protected async Task EditServiceReqApproval(ServiceRequestApprovalDetailVm sr)
        {
            try
            {
                await dialogService.OpenAsync<ServiceRequestApprovalWorkflowForm>(
                     translationState.Translate("Edit_Service_Request_Approval_WorkFlow"), new Dictionary<string, object>() { { "Id", sr.Id } },
                    new DialogOptions()
                    {
                        Width = "50% !important",
                        CloseDialogOnOverlayClick = false,
                        CloseDialogOnEsc = false
                    });
                await Load();
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

        #region Detail Service Request Approval Flow
        protected async Task DetailServiceReqApproval(ServiceRequestApprovalDetailVm sr)
        {
            try
            {
                if (await dialogService.OpenAsync<ServiceRequestApprovalDeatil>(
                      translationState.Translate("Service_Request_Approval_Detail"),
                       new Dictionary<string, object>() { { "Id", sr.Id } },
                       new DialogOptions() { Width = "80% !important", CloseDialogOnOverlayClick = false, CloseDialogOnEsc = false }) == true)
                {
                    await Task.Delay(100);
                }
                StateHasChanged();
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

        #region Delete
        protected async Task UpdateTemplateStatus(bool value, NotificationTemplateListVM args)
        {

            if (await dialogService.Confirm(value ? translationState.Translate("Sure_Want_To_Activate_Template") : translationState.Translate("Sure_Want_To_Deactivate_Template"),
                translationState.Translate("Status")) == true)
            {
                args.isActive = value;
                var response = await notificationsService.DeleteEventTemplate(args);
                if (response.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Update_Successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto;"
                    });
                    await Load();
                    StateHasChanged();
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                }
            }

        }
        protected async Task UpdateEventStatus(bool value, NotificationEventListVM args)
        {
            if (await dialogService.Confirm(value ? translationState.Translate("Sure_Want_To_Activate_Event") : translationState.Translate("Sure_Want_To_Deactivate_Event"),
                translationState.Translate("Status")) == true)
            {
                args.IsActive = value;
                var response = await notificationsService.UpdateEventStatus(args);
                if (response.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Update_Successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto;"
                    });
                    await Load();
                    StateHasChanged();
                }
                else
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                }
            }

        }
        #endregion

        #region Add Service Request Approval Flow
        protected async Task Button1Click(MouseEventArgs args)
        {
            try
            {
                await dialogService.OpenAsync<ServiceRequestApprovalWorkflowForm>(
                     translationState.Translate("Add_Service_Request_Approval_WorkFlow"), null,
                    new DialogOptions()
                    {
                        Width = "80% !important",
                        CloseDialogOnOverlayClick = false,
                        CloseDialogOnEsc = false
                    });

                await Load();
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
    }
}
