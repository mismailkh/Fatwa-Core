using DocumentFormat.OpenXml.Spreadsheet;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.InventoryManagement;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.ServiceRequestModels;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_DOMAIN.Models.ViewModel.GeneralVMs;
using FATWA_DOMAIN.Models.ViewModel.InventoryManagementVMs;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using System.Net;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.Inventory.InventoryEnum;
using static FATWA_DOMAIN.Enums.ServiceRequests.ServiceRequestEnums;

namespace FATWA_WEB.Pages.ServiceRequests.InventoryRequests
{
    public partial class ReturnInventoryRequest : ComponentBase
    {
        #region Parameters

        [Parameter]
        public dynamic? SectorTypeId { get; set; }
        [Parameter]
        public dynamic? RequestTypeId { get; set; }
        [Parameter]
        public dynamic? ServiceRequestId { get; set; }
        //[Parameter]
        //public string? RequestTitle { get; set; }

        [Inject]
        public ProcessLogService processLogService { get; set; }
        [Inject]
        public ErrorLogService errorLogService { get; set; }
        #endregion

        #region Variable Declaration
        protected int storeType = 0;
        protected int categoryId = 0;
        protected string itemCode = "";
        protected int requestTypeId = 0;
        protected string managerId = null;
        protected string requestTitle = "";
        protected string requestorName = "";
        protected string requestStatus = "";
        protected string loginUserSector = "";
        protected string firstApprovalUserName = "";


        List<string> ValidFiles { get; set; } = new List<string>() { ".pdf" };
        public List<int> DeletedAttachementIds { get; set; } = new List<int>();


        public List<InvItems> Items { get; set; } = new List<InvItems>();
        public List<InvItems> FilterItems { get; set; } = new List<InvItems>();
        public List<InvItemCategory> Categories { get; set; } = new List<InvItemCategory>();
        protected InvServiceReqReturnItem InvSrReturnItem { get; set; } = new InvServiceReqReturnItem();
        public ServiceRequestStoreVM? ServiceRequestStoreVM { get; set; } = new ServiceRequestStoreVM();
        public List<ServiceRequestStatus> InvServiceRequestStatuses { get; set; } = new List<ServiceRequestStatus>();

        #endregion

        #region Constructor
        public ReturnInventoryRequest()
        {
            InvSrReturnItem.ServiceRequest.ServiceRequestId = Guid.NewGuid();
            InvSrReturnItem.ServiceRequestId = InvSrReturnItem.ServiceRequest.ServiceRequestId;
        }
        #endregion

        #region Initialize
        protected override async Task OnInitializedAsync()
        {
            requestTypeId = Convert.ToInt32(RequestTypeId);
            loginUserSector = loginState.UserDetail.SectorTypeId.ToString();
            await Load();
        }
        #endregion

        #region Functions
        protected async Task Load()
        {
            spinnerService.Show();
            if (ServiceRequestId == null)
            {
                requestorName = loginState.Username;
                InvSrReturnItem.ServiceRequest.CreatedDate = DateTime.Now;
                // following two lines are for testing purpose
                InvSrReturnItem.ServiceRequestItemId = Guid.NewGuid();
            }
            storeType = Convert.ToInt32(SectorTypeId) == (int)OperatingSectorTypeEnum.GeneralServices ? (int)StoreTypeEnum.GSFloorStore : (int)StoreTypeEnum.ITFloorStore;
            await GetServiceRequestStore();
            if (ServiceRequestStoreVM.StoreId == default(Guid) || ServiceRequestStoreVM.StoreInchargeId == default(Guid))
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("No_Store_Associated_Contact_Administrator"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                navigationManager.NavigateTo("servicerequest-list");
                return;
            }
            else
            {
                await GetManagerByuserId();
                await PopulateDropdowns();
                await SetRequestInitialValues();
            }
            spinnerService.Hide();
        }
        #endregion


        #region Populate Functions

        private async Task SetRequestInitialValues()
        {
            switch (requestTypeId)
            {
                case (int)ServiceRequestTypeEnum.RequestToReturnAnyGSItem:
                    requestStatus = ServiceRequestStatusEnum.Submitted.ToString();
                    requestTitle = "Request To Return Any GS Item";
                    InvSrReturnItem.ServiceRequest.ServiceRequestTypeId = (int)ServiceRequestTypeEnum.RequestToReturnAnyGSItem;
                    //  PageHeading = ServiceRequestId is null ? translationState.Translate("Add_Leave_Request") : translationState.Translate("Edit_Leave_Request");
                    break;

                case (int)ServiceRequestTypeEnum.RequestToReturnAnyITItem:
                    requestStatus = ServiceRequestStatusEnum.Submitted.ToString();
                    requestTitle = "Request To Return Any IT Item";
                    break;
            }
        }
        private async Task GetManagerByuserId()
        {
            //var response = await userService.GetManagersList(loginState.UserDetail.SectorTypeId, loginState.UserDetail.DesignationId);
            var response = await userService.GetManagerByuserId(loginState.UserDetail.UserId);

            if (response.IsSuccessStatusCode || response.StatusCode == HttpStatusCode.BadRequest)
            {
                managerId = (string)response.ResultData;
                await GetUserDetailByUserId();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        protected async Task GetUserDetailByUserId()
        {
            var response = await userService.GetUserById(managerId);
            if (response.IsSuccessStatusCode)
            {
                var userInfo = (UserPersonalInformationVM)response.ResultData;
                firstApprovalUserName = userInfo.UserName;
            }

        }
        protected async Task PopulateDropdowns()
        {
            await GetLatestServiceRequestNumber();
            await GetServiceRequestStatus();
            await GetItemCategory();
            await GetItems();
        }

        protected async Task GetLatestServiceRequestNumber()
        {
            try
            {
                var response = await serviceRequestSharedService.GetLatestServiceRequestNumber(Convert.ToInt32(RequestTypeId));
                if (response.IsSuccessStatusCode)
                {
                    InvSrReturnItem.ServiceRequest.ServiceRequestNumber = (string)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        protected async Task GetItemCategory()
        {
            try
            {
                var response = await invInventoryService.GetItemCategory(Convert.ToInt32(SectorTypeId));
                if (response.IsSuccessStatusCode)
                {
                    Categories = (List<InvItemCategory>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }
        protected async Task GetItems()
        {
            var response = await invInventoryService.GetItems();
            if (response.IsSuccessStatusCode && response.ResultData != null)
            {
                Items = (List<InvItems>)response.ResultData;
                FilterItems = Items;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        private async Task GetServiceRequestStore()
        {
            try
            {
                var response = await invInventoryService.GetServiceRequestStore(storeType, loginState.UserDetail.FloorId, loginState.UserDetail.BuildingId);
                if (response.IsSuccessStatusCode)
                {
                    ServiceRequestStoreVM = (ServiceRequestStoreVM)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        private async Task GetServiceRequestStatus()
        {
            try
            {
                var response = await serviceRequestSharedService.GetServiceRequestStatus();
                if (response.IsSuccessStatusCode)
                {
                    InvServiceRequestStatuses = (List<ServiceRequestStatus>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        protected async Task SaveTempAttachementToUploadedDocument()
        {
            try
            {
                var docResponse = await fileUploadService.SaveTempAttachementToUploadedDocument(new FileUploadVM()
                {
                    RequestIds = new List<Guid> { InvSrReturnItem.ServiceRequestItemId },
                    CreatedBy = loginState.Username,
                    FilePath = _config.GetValue<string>("dms_file_path"),
                    DeletedAttachementIds = null
                });

                if (!docResponse.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Attachment_Save_Failed"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
                }
            }
            catch (Exception)
            {
                return;
                throw;
            }
        }
        #endregion

        #region Category dropdown selected value on change
        protected void OnChangeCategory(int categoryId)
        {
            spinnerService.Show();
            FilterItems = Items.Where(x => x.ItemCategoryId == categoryId).ToList();
            spinnerService.Hide();
        }
        #endregion

        #region Item dropdown selected value on change
        protected void OnSelectItem(Guid itemId)
        {
            spinnerService.Show();
            itemCode = Items.Where(x => x.ItemId == itemId).Select(x => x.ItemCode).FirstOrDefault();
            spinnerService.Hide();
        }
        #endregion



        #region Form Submit
        protected async Task Form0Submit()
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Sure_Approve"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    //invServiceRequests.ServiceRequestTypeId = (Convert.ToInt32(RequestTypeId));
                    InvSrReturnItem.ServiceRequest.ServiceRequestStatusId = (int)ServiceRequestStatusEnum.Submitted;
                    InvSrReturnItem.ServiceRequest.CreatedBy = loginState.UserDetail.UserName;
                    InvSrReturnItem.ServiceRequest.StoreId = ServiceRequestStoreVM.StoreId;
                    InvSrReturnItem.ServiceRequest.ReceiverId = ServiceRequestStoreVM.StoreInchargeId;
                    await FillTaskVm();
                    var response = await invInventoryService.AddReturnInventoryServiceRequest(InvSrReturnItem);
                    if (response.IsSuccessStatusCode)
                    {
                       await SaveTempAttachementToUploadedDocument();

                        var apiResponse = (ApiReturnTaskNotifAuditLogVM)response.ResultData;

                        if (apiResponse.addedTaskList.Any())
                        {
                            await SaveMemberTasks(apiResponse.addedTaskList);
                            //await AddSystemGeneratedTask();
                        }
                        if (apiResponse.sendNotifications.Any())
                        {
                           await CreateSystemNotification(apiResponse.sendNotifications);
                        }
                        if (apiResponse.processLog != null)
                        {
                            await CreateProcessLog(apiResponse.processLog);
                        }

                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Request_Submitted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        StateHasChanged();
                        navigationManager.NavigateTo("servicerequest-list");
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    spinnerService.Hide();

                    navigationManager.NavigateTo("servicerequest-list");
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
        protected async Task CancelForm()
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Cancel"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                navigationManager.NavigateTo("servicerequest-list");
            }
        }
       
        
        
        #endregion

        #region Redirection Function
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

        #region Fill System Generated Task Model
        protected async Task FillTaskVm()
        {
            ServiceReqTaskNotificationDto invTask = new ServiceReqTaskNotificationDto()
            {
                AssignToId = managerId,
                AssignById = loginState.UserDetail.UserId,
                TaskName = translationState.Translate("Return_Inventory_Request"),
                Url = $"detail-return-servicerequest/{InvSrReturnItem.ServiceRequest.ServiceRequestId}/"
            };
            InvSrReturnItem.InvReturnReqDto = invTask;
        }
        #endregion

        protected async Task SaveMemberTasks(List<SaveTaskVM> requestTasks)
        {
            try
            {
                List<SaveTaskVM> tasks = new List<SaveTaskVM>();
                foreach (var task in requestTasks)
                {
                    task.Task.SubModuleId = null;
                    task.Task.SystemGenTypeId = null;
                    tasks.Add(task);
                }
                var response = await taskService.AddTaskList(tasks);
                if (!response.IsSuccessStatusCode)
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

        protected async Task CreateProcessLog(ProcessLog processLog)
        {
            try
            {
                var response = await processLogService.CreateProcessLog(processLog);
                if(!response.IsSuccessStatusCode)
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

        protected async Task CreateErrorLog(ErrorLog errorLog)
        {
            try
            {
                var response = await errorLogService.CreateErrorLog(errorLog);
                if (!response.IsSuccessStatusCode)
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
        protected async Task CreateSystemNotification(List<Notification> notifications)
        {
            try
            {
                var notificationResponse = await notificationDetailService.SendNotification(notifications);
                if (!notificationResponse.IsSuccessStatusCode)
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(notificationResponse);
                }
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
    }
}
