using FATWA_DOMAIN.Models.ServiceRequestModels;
using FATWA_DOMAIN.Models.ViewModel.InventoryManagementVMs;
using FATWA_DOMAIN.Models.ViewModel.ServiceRequestVMs;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.ServiceRequests.ServiceRequestEnums;

namespace FATWA_WEB.Pages.ServiceRequests
{
    public partial class ListServiceRequests : ComponentBase
    {
		#region Varriable
		protected RadzenDataGrid<ServiceRequestVM>? grid = new RadzenDataGrid<ServiceRequestVM>();
		protected ServiceRequestAdvanceSearchVM advanceSearchVM { get; set; } = new ServiceRequestAdvanceSearchVM();
		protected List<ServiceRequestStatus> ServiceRequestStatus { get; set; } = new List<ServiceRequestStatus>();


		protected bool Keywords = false;
		public bool isVisible { get; set; }

		#endregion

		#region Full property declaration

		IEnumerable<ServiceRequestVM> ServiceRequestList;
		IEnumerable<ServiceRequestVM> _FilteredServiceRequestList;
		protected IEnumerable<ServiceRequestVM> FilteredServiceRequestList
		{
			get
			{
				return _FilteredServiceRequestList;
			}
			set
			{
				if (!object.Equals(_FilteredServiceRequestList, value))
				{
					var args = new PropertyChangedEventArgs() { Name = "FilteredServiceRequestList", NewValue = value, OldValue = _FilteredServiceRequestList };
					_FilteredServiceRequestList = value;

					Reload();
				}
			}

		}
		string _search;
		protected string search
		{
			get
			{
				return _search;
			}
			set
			{
				if (!object.Equals(_search, value))
				{
					var args = new PropertyChangedEventArgs() { Name = "search", NewValue = value, OldValue = _search };
					_search = value;
					Reload();
				}
			}
		}
		#endregion

		#region On Initialize
		public void Reload()
		{
			InvokeAsync(StateHasChanged);
		}

		protected override async Task OnInitializedAsync()
		{
			spinnerService.Show();
			await PopulateServiceRequestStatus();
			await Load();
			spinnerService.Hide();
		}

		protected async Task Load()
		{
			try
			{
				var response = await serviceRequestSharedService.GetServiceRequestList(advanceSearchVM);
				if (response.IsSuccessStatusCode)
				{
					ServiceRequestList = (List<ServiceRequestVM>?)response.ResultData;
					FilteredServiceRequestList = (IEnumerable<ServiceRequestVM>?)response.ResultData;
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
		//protected async Task OnSearchInput()
		//{
		//	try
		//	{
		//		if (string.IsNullOrEmpty(search))
		//			search = "";
		//		else
		//			search = search;
		//		FilteredServiceRequestList = await gridSearchExtension.Filter(ServiceRequestList, new Query()
		//		{
		//			Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US" ? $@"i => (i.ServiceRequestNumber != null && i.ServiceRequestNumber.ToString().Contains(@0)) || (i.RequestorNameEn != null && i.RequestorNameEn.ToString().ToLower().Contains(@1)) || (i.SectorNameEn != null && i.SectorNameEn.ToString().ToLower().Contains(@2)) || (i.RequestStatusEn != null && i.RequestStatusEn.ToString().ToLower().Contains(@3)) " :
		//			$@"i => (i.ServiceRequestNumber != null && i.ServiceRequestNumber.ToString().Contains(@0)) || (i.RequestorNameAr != null && i.RequestorNameAr.ToString().ToLower().Contains(@1)) || (i.SectorNameAr != null && i.SectorNameAr.ToString().ToLower().Contains(@2)) || (i.RequestStatusAr != null && i.RequestStatusAr.ToString().ToLower().Contains(@3)) ",
		//			FilterParameters = new object[] { search.ToLower(), search.ToLower(), search.ToLower(), search.ToLower() }
		//		});
		//	}
		//	catch (Exception ex)
		//	{
		//		notificationService.Notify(new NotificationMessage()
		//		{
		//			Severity = NotificationSeverity.Error,
		//			Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
		//			Style = "position: fixed !important; left: 0; margin: auto; "
		//		});
		//	}
		//}

        protected async Task OnSearchInput()
       {
            try
            {
                if (string.IsNullOrEmpty(search))
                {
                    search = "";
                }
                else
                    search = search.ToLower();
                if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
                {
                    FilteredServiceRequestList = await gridSearchExtension.Filter(ServiceRequestList, new Radzen.Query()
                    {
                        Filter = $@"i => (i.ServiceRequestNumber != null && i.ServiceRequestNumber.ToString().Contains(@0)) || 
										 (i.RequestorNameEn != null && i.RequestorNameEn.ToString().ToLower().Contains(@1)) ||
										 (i.SectorFromEn != null && i.SectorFromEn.ToString().ToLower().Contains(@2)) ||
										 (i.SectorToEn != null && i.SectorToEn.ToString().ToLower().Contains(@3))",
                        FilterParameters = new object[] { search, search, search, search }

                    });
                }
                else
                {
                    FilteredServiceRequestList = await gridSearchExtension.Filter(ServiceRequestList, new Radzen.Query()
                    {
                        Filter = $@"i => (i.ServiceRequestNumber != null && i.ServiceRequestNumber.ToString().Contains(@0)) || 
										 (i.RequestorNameAr != null && i.RequestorNameAr.ToString().ToLower().Contains(@1)) ||
										 (i.SectorFromAr != null && i.SectorFromAr.ToString().ToLower().Contains(@2)) ||
										 (i.SectorToAr != null && i.SectorToAr.ToString().ToLower().Contains(@3))",
                        FilterParameters = new object[] { search, search, search, search }

                    });
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Drop Downs 

        protected async Task PopulateServiceRequestStatus()
		{

			var response = await serviceRequestSharedService.GetServiceRequestStatus();
			if (response.IsSuccessStatusCode)
			{
				ServiceRequestStatus = (List<ServiceRequestStatus>)response.ResultData;
			}
			else
			{
				await invalidRequestHandlerService.ReturnBadRequestNotification(response);
			}
			StateHasChanged();

		}
		#endregion

		#region Add Service Request
		protected async Task AddServiceRequest(MouseEventArgs args)
		{
			try
			{
				var dialogResult = await dialogService.OpenAsync<SelectRequestTypePopup>
				  (
					  translationState.Translate("Add_Service_Request"),
					  null,
					  new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true }
				  );

				if (dialogResult != null)
					await NavigateToForm((int)dialogResult);
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

		private async Task NavigateToForm(int dialogResult)
		{
			int sectorId = 0;
			switch (dialogResult)
			{
				case (int)ServiceRequestTypeEnum.RequestToIssueAnyITItem:
					sectorId = (int)OperatingSectorTypeEnum.InformationTechnology;
					navigationManager.NavigateTo("/add-inventory-request/" + dialogResult + "/" + sectorId);
					break;

				case (int)ServiceRequestTypeEnum.RequestToReturnAnyITItem:
					sectorId = (int)OperatingSectorTypeEnum.InformationTechnology;
					navigationManager.NavigateTo("/return-inventory-request/" + dialogResult + "/" + sectorId);
					//navigationManager.NavigateTo("/add-inventory-request/" + dialogResult + "/" + sectorId);
					break;

				case (int)ServiceRequestTypeEnum.RequestToIssueAnyGSItem:
					sectorId = (int)OperatingSectorTypeEnum.GeneralServices;
					navigationManager.NavigateTo("/add-inventory-request/" + dialogResult + "/" + sectorId);
					break;

				case (int)ServiceRequestTypeEnum.RequestToReturnAnyGSItem:
                    sectorId = (int)OperatingSectorTypeEnum.GeneralServices;
                    navigationManager.NavigateTo("/return-inventory-request/" + dialogResult + "/" + sectorId);
                   // navigationManager.NavigateTo("/add-inventory-request/" + dialogResult + "/" + sectorId);
					break;

				case (int)ServiceRequestTypeEnum.SubmitComplaintRequest:
					sectorId = (int)OperatingSectorTypeEnum.MaintenanceANDEngineeringAffairDepartment;
					navigationManager.NavigateTo("/add-complaint-request/" + dialogResult + "/" + sectorId);
					break;
				
				case (int)ServiceRequestTypeEnum.SubmitaLeaveRequest:
					sectorId = (int)OperatingSectorTypeEnum.LeaveAndDutyDepartment;
					navigationManager.NavigateTo("/add-leave-request/" + dialogResult + "/" + sectorId);
					break;
				
				case (int)ServiceRequestTypeEnum.RequestToReduceWorkingHours:
					sectorId = (int)OperatingSectorTypeEnum.LeaveAndDutyDepartment;
					navigationManager.NavigateTo("/add-reduceworkinghours-request/" + dialogResult + "/" + sectorId);
					break;
				
				case (int)ServiceRequestTypeEnum.RequestForFingerprintExemption:
					sectorId = (int)OperatingSectorTypeEnum.LeaveAndDutyDepartment;
					navigationManager.NavigateTo("/add-fingerprintexemption-request/" + dialogResult + "/" + sectorId);
					break;
				
				case (int)ServiceRequestTypeEnum.SubmitaRequestforPermission:
					sectorId = (int)OperatingSectorTypeEnum.LeaveAndDutyDepartment;
					navigationManager.NavigateTo("/add-permission-request/" + dialogResult + "/" + sectorId);
					break;
				
				case (int)ServiceRequestTypeEnum.RequestforAppointmentwithMedicalCouncil:
					sectorId = (int)OperatingSectorTypeEnum.LeaveAndDutyDepartment;
					navigationManager.NavigateTo("/add-appointment-medicalcouncil-request/" + dialogResult + "/" + sectorId);
					break;

                //case (int)ServiceRequestTypeEnum.RequestToReturnAnyGSItem:
                //    sectorId = (int)OperatingSectorTypeEnum.GeneralServices;
                //    navigationManager.NavigateTo("/return-inventory-request/" + dialogResult + "/" + sectorId);
                //    break;
            }
		}

		#region Redirect Function
		private void GoBackDashboard()
		{
			navigationManager.NavigateTo("/dashboard");
		}
		private void GoBackHomeScreen()
		{
			navigationManager.NavigateTo("/index");
		}
		protected async Task ServiceRequestDetailView(ServiceRequestVM serviceRequestDetail)
		{

            switch (serviceRequestDetail.RequestTypeId)
            {
                case (int)ServiceRequestTypeEnum.RequestToIssueAnyITItem:
                    navigationManager.NavigateTo("detail-servicerequest/" + serviceRequestDetail.ServiceRequestId);
                    break;

                case (int)ServiceRequestTypeEnum.RequestToIssueAnyGSItem:
                    navigationManager.NavigateTo("detail-servicerequest/" + serviceRequestDetail.ServiceRequestId);
                    break;

                case (int)ServiceRequestTypeEnum.RequestToReturnAnyITItem:
                    navigationManager.NavigateTo("detail-return-servicerequest/" + serviceRequestDetail.ServiceRequestId);
                    break;

                case (int)ServiceRequestTypeEnum.RequestToReturnAnyGSItem:
                    navigationManager.NavigateTo("detail-return-servicerequest/" + serviceRequestDetail.ServiceRequestId);
                    break;

                case (int)ServiceRequestTypeEnum.SubmitComplaintRequest:
                    navigationManager.NavigateTo("detail-complaint-request/" + serviceRequestDetail.ServiceRequestId);
                    break;

                case (int)ServiceRequestTypeEnum.SubmitaLeaveRequest:
                    navigationManager.NavigateTo("leave-request-detail/" + serviceRequestDetail.RequestTypeId + "/" + serviceRequestDetail.ServiceRequestId);
                    break;

                case (int)ServiceRequestTypeEnum.RequestToReduceWorkingHours:
                    navigationManager.NavigateTo("reduceworkinghours-request-detail/" + serviceRequestDetail.RequestTypeId + "/" + serviceRequestDetail.ServiceRequestId);
                    break;

                case (int)ServiceRequestTypeEnum.RequestForFingerprintExemption:
                    navigationManager.NavigateTo("fingerprintexemption-request-detail/" + serviceRequestDetail.RequestTypeId + "/" + serviceRequestDetail.ServiceRequestId);
                    break;

                case (int)ServiceRequestTypeEnum.SubmitaRequestforPermission:
                    navigationManager.NavigateTo("permission-request-detail/" + serviceRequestDetail.RequestTypeId + "/" + serviceRequestDetail.ServiceRequestId);
                    break;

                case (int)ServiceRequestTypeEnum.RequestforAppointmentwithMedicalCouncil:
                    navigationManager.NavigateTo("appointment-medicalcouncil-request-detail/" + serviceRequestDetail.RequestTypeId+"/" + serviceRequestDetail.ServiceRequestId);
                    break;
            }
        }

		protected async Task EditServiceRequest(ServiceRequestVM serviceRequestDetail)
		{
			switch (serviceRequestDetail.RequestTypeId)
			{
				case (int)ServiceRequestTypeEnum.SubmitComplaintRequest:
					navigationManager.NavigateTo("edit-complaint-request/" + serviceRequestDetail.ServiceRequestId);
					break;

                case (int)ServiceRequestTypeEnum.SubmitaLeaveRequest:
                    navigationManager.NavigateTo("edit-leave-request/" + serviceRequestDetail.RequestTypeId + "/" + serviceRequestDetail.ServiceRequestId);
                    break;

                case (int)ServiceRequestTypeEnum.RequestToReduceWorkingHours:
                    navigationManager.NavigateTo("edit-reduceworkinghours-request/" + serviceRequestDetail.RequestTypeId + "/" + serviceRequestDetail.ServiceRequestId);
                    break;

                case (int)ServiceRequestTypeEnum.RequestForFingerprintExemption:
                    navigationManager.NavigateTo("edit-fingerprintexemption-request/" + serviceRequestDetail.RequestTypeId + "/" + serviceRequestDetail.ServiceRequestId);
                    break;

                case (int)ServiceRequestTypeEnum.SubmitaRequestforPermission:
                    navigationManager.NavigateTo("edit-permission-request/" + serviceRequestDetail.RequestTypeId + "/" + serviceRequestDetail.ServiceRequestId);
                    break;

                case (int)ServiceRequestTypeEnum.RequestforAppointmentwithMedicalCouncil:
                    navigationManager.NavigateTo("edit-appointment-medicalcouncil-request/" + serviceRequestDetail.RequestTypeId + "/" + serviceRequestDetail.ServiceRequestId);
                    break;
            }
		}

		#endregion

		#region Advance Search

		protected async Task SubmitAdvanceSearch()
		{
			if (advanceSearchVM.RequestedDateFrom > advanceSearchVM.RequestedDateTo)
			{
				notificationService.Notify(new NotificationMessage()
				{
					Severity = NotificationSeverity.Error,
					Detail = translationState.Translate("FromDate_NotGreater_ToDate"),
					//Summary = $"???!",
					Style = "position: fixed !important; left: 0; margin: auto; "
				});
				Keywords = true;
				return;
			}
			if (!advanceSearchVM.RequestStatusId.HasValue && !advanceSearchVM.RequestedDateFrom.HasValue && !advanceSearchVM.RequestedDateTo.HasValue)
			{

			}
			else
			{
				Keywords = true;

				await Load();
				await grid.Reload();
			}
		}

		public async void ResetForm()
		{

			advanceSearchVM = new ServiceRequestAdvanceSearchVM();
			await Load();
			StateHasChanged();
			Keywords = false;
		}

		protected async Task ToggleAdvanceSearch()
		{
			isVisible = !isVisible;
			if (!isVisible)
			{
				ResetForm();
			}
		}

		#endregion

	}
}
