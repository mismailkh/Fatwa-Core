using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_DOMAIN.Enums.LiteratureEnum;
using static FATWA_GENERAL.Helper.Enum;

namespace FATWA_WEB.Pages.Lms
{


    public partial class DecisionLiteratureBorrowExtension : ComponentBase
	{
		

		public void Reload()
		{
			InvokeAsync(StateHasChanged);
		}

		public void OnPropertyChanged(PropertyChangedEventArgs args)
		{
		}

		#region Service Injections



		#endregion

		#region Variables

		[Parameter]
		public dynamic BorrowId { get; set; }
		public UserPersonalInformationVM PersonalInformationVM { get; set; }
		public IEnumerable<LiteratureBorrowApprovalType> getLiteratureBorrowApprovalType;

		protected IEnumerable<LiteratureBorrowApprovalType> getExcludedLiteratureBorrowApprovalType;
		

		LmsLiteratureBorrowDetail _literatureBorrowApproveReject;
		protected LmsLiteratureBorrowDetail literatureBorrowApproveReject
		{
			get
			{
				return _literatureBorrowApproveReject;
			}
			set
			{
				if (!object.Equals(_literatureBorrowApproveReject, value))
				{
					var args = new PropertyChangedEventArgs() { Name = "literatureBorrowApproveReject", NewValue = value, OldValue = _literatureBorrowApproveReject };
					_literatureBorrowApproveReject = value;
					OnPropertyChanged(args);
					Reload();
				}
			}
		}

		#endregion

		#region On Load

		protected override async Task OnInitializedAsync()
		{
			spinnerService.Show();

			await Load();

			spinnerService.Hide();

		}

		protected async Task Load()
		{
			try
			{
				getLiteratureBorrowApprovalType = await lmsLiteratureBorrowDetailService.getLiteratureBorrowApprovalTypes();
				ChangeApprovalTypeForAddingDecision(getLiteratureBorrowApprovalType);

				var borrowDetail = await lmsLiteratureBorrowDetailService.GetUniqueLmsLiteratureBorrowDetails(BorrowId);
                if (borrowDetail.IsSuccessStatusCode)
                {
                    literatureBorrowApproveReject = (LmsLiteratureBorrowDetail)borrowDetail.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(borrowDetail);
                }
				LmsLiterature literatureDetail = await lmsLiteratureService.GetLmsLiteratureById(literatureBorrowApproveReject.LiteratureId);
				literatureBorrowApproveReject.ISBN = literatureDetail.ISBN;
				literatureBorrowApproveReject.LiteratureName = literatureDetail.Name;

				var userDetail = await userService.UserListByUserId(literatureBorrowApproveReject.UserId);
				if(userDetail.IsSuccessStatusCode)
				{
					PersonalInformationVM = (UserPersonalInformationVM)userDetail.ResultData;
                    literatureBorrowApproveReject.UserName = PersonalInformationVM.UserName;
                    literatureBorrowApproveReject.PhoneNumber = PersonalInformationVM.PhoneNumber;
                    literatureBorrowApproveReject.EligibleCount = PersonalInformationVM.EligibleCount;
                }
				
				OnApprovalTypeChange(literatureBorrowApproveReject.ExtensionApprovalStatus, null);

				await InvokeAsync(StateHasChanged);

			}
			catch (Exception)
			{
				notificationService.Notify(new NotificationMessage()
				{
					Severity = NotificationSeverity.Error,
					Detail = translationState.Translate("Literature_Extension_Load_Error"),
					Style = "position: fixed !important; left: 0; margin: auto; "
				});
			}
		}

		#endregion

		#region Borrow Approve/Reject

		protected async Task FormSubmit(LmsLiteratureBorrowDetail args)
		{
			try
			{
				string dialogMsg = null;
				string notificationMsg = null;

				if (args.ExtensionApprovalStatus == (int)BorrowExtensionApprovalStatus.Approve)
				{
					dialogMsg = translationState.Translate("Approve_Literature_Extension");
					notificationMsg = translationState.Translate("Approve_Literature_Extension_Success");
				}
				else if (args.ExtensionApprovalStatus == (int)BorrowExtensionApprovalStatus.Reject)
				{
					dialogMsg = translationState.Translate("Reject_Literature_Extension");
					notificationMsg = translationState.Translate("Reject_Literature_Extension_Success");
				}

				bool? dialogResponse = await dialogService.Confirm(
				   dialogMsg,
				   translationState.Translate("Confirm"),
				   new ConfirmOptions()
				   {
					   OkButtonText = translationState.Translate("OK"),
					   CancelButtonText = translationState.Translate("Cancel")
				   });


				if (dialogResponse == true)
				{
					var response = await lmsLiteratureBorrowDetailService.UpdateLiteratureBorrowApprovalStatus((BorrowId), literatureBorrowApproveReject);
					if (response.IsSuccessStatusCode)
					{
						literatureBorrowApproveReject = (LmsLiteratureBorrowDetail)response.ResultData;
						notificationService.Notify(new NotificationMessage()
						{
							Severity = NotificationSeverity.Success,
							Detail = notificationMsg,
							Style = "position: fixed !important; left: 0; margin: auto; "
						});
						StateHasChanged();
					}
					else
					{
						await invalidRequestHandlerService.ReturnBadRequestNotification(response);
					}
					if (response != null)
					{

						navigationManager.NavigateTo("/lmsliteratureborrowdetail-extension-approval-list");

					}
				}
				else
				{
					dialogResponse = await dialogService.Confirm(
						translationState.Translate("Sure_Cancel"),
						translationState.Translate("Confirm_Cancel"),
						new ConfirmOptions()
						{
							CloseDialogOnOverlayClick = true,
							OkButtonText = translationState.Translate("OK"),
							CancelButtonText = translationState.Translate("Cancel")
						});

					if (dialogResponse == true)
					{
						await Load();
					}
				}

			}
			catch (Exception)
			{
				notificationService.Notify(new NotificationMessage()
				{
					Severity = NotificationSeverity.Error,
					Detail = translationState.Translate("Literature_Extension_Error"),
					Style = "position: fixed !important; left: 0; margin: auto; "
				});
			}
		}

		protected void Button2Click(MouseEventArgs args)
		{
			try
			{
				navigationManager.NavigateTo("/lmsliteratureborrowdetail-extension-approval-list");
			}
			catch (Exception)
			{
				throw;
			}
		}

		#endregion

		#region Functions

		public string showComment = "";

		protected void OnApprovalTypeChange(object value, string msg)
		{
			if (value != null)
				showComment = "";
			else
				showComment = "none";
			if (msg != null)
				literatureBorrowApproveReject.Comment = "";
			InvokeAsync(StateHasChanged);
		}

		private void ChangeApprovalTypeForAddingDecision(IEnumerable<LiteratureBorrowApprovalType> getLiteratureBorrowApprovalType)
		{
			try
			{
				List<LiteratureBorrowApprovalType> approvalTypes = new List<LiteratureBorrowApprovalType>();
				foreach (var documentApprovalTypeItem in getLiteratureBorrowApprovalType)
				{
					if (documentApprovalTypeItem.DecisionId == (int)BorrowApprovalStatus.Approved)
					{
						if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")

						{
							documentApprovalTypeItem.Name = translationState.Translate("Approve");
							approvalTypes.Add(documentApprovalTypeItem);
						}
						else
						{
							documentApprovalTypeItem.Name_Ar = translationState.Translate("Approve");
							approvalTypes.Add(documentApprovalTypeItem);
						}

					}
					if (documentApprovalTypeItem.DecisionId == (int)BorrowApprovalStatus.Rejected)
					{
						if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")

						{
							documentApprovalTypeItem.Name = translationState.Translate("Reject");
							approvalTypes.Add(documentApprovalTypeItem);
						}
						else
						{
							documentApprovalTypeItem.Name_Ar = translationState.Translate("Reject");
							approvalTypes.Add(documentApprovalTypeItem);
						}
					}
				}
				getExcludedLiteratureBorrowApprovalType = approvalTypes;
			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
			}
		}


		#endregion

		#region Redirect Function
		private void GoBackDashboard()
		{
			navigationManager.NavigateTo("/dashboard");
		}
		private void GoBackHomeScreen()
		{
			navigationManager.NavigateTo("/index");
		}
		#endregion
	}

}
