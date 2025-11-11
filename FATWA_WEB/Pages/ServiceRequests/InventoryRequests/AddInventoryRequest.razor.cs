using FATWA_DOMAIN.Models.InventoryManagement;
using FATWA_DOMAIN.Models.ServiceRequestModels;
using FATWA_DOMAIN.Models.ViewModel.InventoryManagementVMs;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.Inventory.InventoryEnum;
using static FATWA_DOMAIN.Enums.ServiceRequests.ServiceRequestEnums;
namespace FATWA_WEB.Pages.ServiceRequests.InventoryRequests
{
    public partial class AddInventoryRequest : ComponentBase
    {
        public AddInventoryRequest()
        {
            RequestItem = new ServiceRequest();
            invServiceRequests = new ServiceRequest();
        }

        #region Parameters
        [Parameter]
        public dynamic? ServiceRequestId { get; set; }
        [Parameter]
        public dynamic? RequestTypeId { get; set; }
        [Parameter]
        public dynamic? SectorTypeId { get; set; }
        #endregion

        #region Model Full Property
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
        private ServiceRequest invServiceRequests;

        public ServiceRequest InvServiceRequests
        {
            get { return invServiceRequests; }
            set
            {
                if (!object.Equals(invServiceRequests, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "InvServiceRequests", NewValue = value, OldValue = invServiceRequests };
                    invServiceRequests = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        #endregion

        #region Variable Declaration
        public RadzenDataGrid<InvItemsVM> grid { get; set; } = new RadzenDataGrid<InvItemsVM>();
        public List<ServiceRequestStatus> InvServiceRequestStatuses { get; set; } = new List<ServiceRequestStatus>();
        public List<InvItemCategory> Categories { get; set; } = new List<InvItemCategory>();
        public ServiceRequest RequestItem { get; set; }
        public ServiceRequestStoreVM? ServiceRequestStoreVM { get; set; } = new ServiceRequestStoreVM();
        public List<InvItems> Items { get; set; } = new List<InvItems>();
        public List<InvItems> FilterItems { get; set; } = new List<InvItems>();
        public List<InvItemsVM> AddedItems { get; set; } = new List<InvItemsVM>();

        public int categoryId = 0;
        public string RequestorName = "";
        public int storeType = 0;
        #endregion

        #region Component OnLoad
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();

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
                await Load();
                await PopulateDropdowns();
                spinnerService.Hide();
            }
        }

        protected async Task Load()
        {
            InvServiceRequests = new ServiceRequest()
            {
                ServiceRequestId = Guid.NewGuid(),
                CreatedDate = DateTime.Now
            };
            RequestItem.RequestQuantity = 1;
            RequestorName = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? loginState.UserDetail.FullNameEn : loginState.UserDetail.FullNameAr;
        }
        #endregion

        #region Populate Functions
        protected async Task PopulateDropdowns()
        {
            try
            {
                await GetLatestServiceRequestNumber();
                await GetServiceRequestStatus();
                await GetItemCategory();
                await GetItems();
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }

        }
        private async Task GetLatestServiceRequestNumber()
        {
            try
            {
                var response = await serviceRequestSharedService.GetLatestServiceRequestNumber(Convert.ToInt32(RequestTypeId));
                if (response.IsSuccessStatusCode)
                {
                    InvServiceRequests.ServiceRequestNumber = (string)response.ResultData;
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
        private async Task GetItemCategory()
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
        #endregion

        #region Add Item To List
        protected void AddItemToSelectedList()
        {
            if (InvServiceRequests.RequestQuantity == 0)
            {
                var item = Items.Where(x => x.ItemId == RequestItem.ItemId).FirstOrDefault();
                var category = Categories.Where(x => x.ItemCategoryId == item.ItemCategoryId).FirstOrDefault();

                AddedItems.Add(new InvItemsVM
                {
                    TotalQuantity = RequestItem.RequestQuantity,
                    ItemId = item.ItemId,
                    ItemNameEn = item.ItemNameEn,
                    ItemNameAr = item.ItemNameAr,
                    ItemCategoryId = category.ItemCategoryId,
                    CategoryNameEn = category.NameEn,
                    CategoryNameAr = category.NameAr,
                });
                InvServiceRequests.RequestItems.Add(new InvServiceRequestItem
                {
                    ServiceRequestItemId = Guid.NewGuid(),
                    ServiceRequestId = InvServiceRequests.ServiceRequestId,
                    ServiceRequestItemStatusId = (int)RequestItemStatusEnum.Submitted,
                    ItemId = item.ItemId,
                    Quantity = RequestItem.RequestQuantity,
                    PendingQuantity = RequestItem.RequestQuantity,
                    IssuedQuantity = 0
                });

                RequestItem = new ServiceRequest()
                {
                    RequestQuantity = 1
                };
                categoryId = 0;
                grid.Reload();


            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Info,
                    Detail = translationState.Translate("Enter Quantity"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }

        }
        protected async Task DeleteSelectedItem(InvItemsVM item)
        {
            AddedItems.Remove(item);
            InvServiceRequests.RequestItems.Remove(RequestItem.RequestItems.Where(x => x.ItemId == item.ItemId).FirstOrDefault());
            await grid.Reload();
            StateHasChanged();
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

        #region Form Submit
        protected async Task Form0Submit()
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Are_you_sure_you_want_to_create_service_request"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    invServiceRequests.ServiceRequestTypeId = (Convert.ToInt32(RequestTypeId));
                    invServiceRequests.ServiceRequestStatusId = (int)ServiceRequestStatusEnum.ApprovedbyHOS;
                    invServiceRequests.CreatedBy = loginState.UserDetail.UserName;
                    invServiceRequests.StoreId = ServiceRequestStoreVM.StoreId;
                    invServiceRequests.ReceiverId = ServiceRequestStoreVM.StoreInchargeId;

                    var response = await invInventoryService.SubmitServiceRequest(invServiceRequests);
                    AddedItems.Clear();
                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Request_Submitted_Successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        StateHasChanged();
                        navigationManager.NavigateTo("list-of-store");
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
    }
}
