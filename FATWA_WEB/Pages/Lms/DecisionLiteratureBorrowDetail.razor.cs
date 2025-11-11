using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Enums;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using System.Text.RegularExpressions;
using static FATWA_DOMAIN.Enums.LiteratureEnum;
namespace FATWA_WEB.Pages.Lms
{
    public partial class DecisionLiteratureBorrowDetail : ComponentBase
	{
		public void Reload()
		{
			InvokeAsync(StateHasChanged);
		}

		public void OnPropertyChanged(PropertyChangedEventArgs args)
		{
		}
		protected bool isExtended = false;
		protected bool isExtendedDue = true;
		protected bool isAllowToReturnDisable = false;
		protected bool isAllowedToReturnDisableFromUI = false;
		protected DateTime? SelectedReturnDate = DateTime.MinValue;
		public int SaveBorrowApprovalStatus { get; set; } = 0;

		[Parameter]
		public dynamic BorrowId { get; set; }

		LmsLiteratureBorrowDetail _lmsLiteratureBorrowDetail;

		protected RadzenDataGrid<UserBorrowLiteratureVM> userBorrowLiteratureGrid;

		List<UserBorrowLiteratureVM> userBorrowLiteratureDetails = new List<UserBorrowLiteratureVM>();

		protected LmsLiteratureBorrowDetail lmsliteratureborrowdetail
		{
			get
			{
				return _lmsLiteratureBorrowDetail;
			}
			set
			{
				if (!object.Equals(_lmsLiteratureBorrowDetail, value))
				{
					var args = new PropertyChangedEventArgs() { Name = "lmsliteratureborrowdetail", NewValue = value, OldValue = _lmsLiteratureBorrowDetail };
					_lmsLiteratureBorrowDetail = value;
					OnPropertyChanged(args);
					Reload();
				}
			}

		}


		IEnumerable<LmsLiterature> _getLmsLiteratureDetailsForLiteratureIdResult;
		protected IEnumerable<LmsLiterature> getLmsLiteratureDetailsForLiteratureIdResult
		{
			get
			{
				return _getLmsLiteratureDetailsForLiteratureIdResult;
			}
			set
			{
				if (!object.Equals(_getLmsLiteratureDetailsForLiteratureIdResult, value))
				{
					var args = new PropertyChangedEventArgs() { Name = "getLmsLiteratureDetailsForLiteratureIdResult", NewValue = value, OldValue = _getLmsLiteratureDetailsForLiteratureIdResult };
					_getLmsLiteratureDetailsForLiteratureIdResult = value;
					OnPropertyChanged(args);
					Reload();
				}
			}

		}

		public IEnumerable<UserVM> getUserDetails = null;

		public async void OnLoadUserData(LoadDataArgs args)
		{
			string str = args.Filter;
			bool hasNumSpecialChars = rgx.IsMatch(str);

			if (!hasNumSpecialChars)
			{
                var getUserDetail = await userService.UserListBySearchTerm(str);
                if (getUserDetail.IsSuccessStatusCode)
                {
                    getUserDetails = (IEnumerable<UserVM>)getUserDetail.ResultData;
                }
            }
			await InvokeAsync(StateHasChanged);
			//old
			//  getUserDetails = await userService.UserListBySearchTerm(args.Filter);

			await InvokeAsync(StateHasChanged);
		}


		public void OnUserChange(object value, string name)
		{
			if (getUserDetails != null)
			{
				var obj = getUserDetails.FirstOrDefault();
				lmsliteratureborrowdetail.UserId = obj.Id;
				InvokeAsync(StateHasChanged);
			}
		}

		public IEnumerable<LiteratureDetailVM> getBookDetails = null;
		Regex rgx = new Regex("[^a-zA-Z?-?]");

		string appCulture = Thread.CurrentThread.CurrentUICulture.Name;
		public async void OnLoadData(LoadDataArgs args)
		{
			string str = args.Filter;
			bool hasNumSpecialChars = rgx.IsMatch(str);

			if (!hasNumSpecialChars)
			{
                var response = await lmsLiteratureBorrowDetailService.GetLmsLiteratureBySearchTerm(args.Filter, appCulture);
				if (response.IsSuccessStatusCode)
				{
                    getBookDetails = (IEnumerable<LiteratureDetailVM>)response.ResultData;
                }
				else
				{
					await invalidRequestHandlerService.ReturnBadRequestNotification(response);
				}
            }
				

			await InvokeAsync(StateHasChanged);
		}

		public void OnChange(bool? isExtendedvalue, string name)
		{
			if (isExtendedvalue == true)
			{
				isExtendedDue = false;
				isAllowedToReturnDisableFromUI = true;
				SelectedReturnDate = lmsliteratureborrowdetail.ReturnDate;
				lmsliteratureborrowdetail.ReturnDate = null;
				lmsliteratureborrowdetail.ExtendDueDate = lmsliteratureborrowdetail.DueDate.AddDays(7);

			}
			else
			{
				isExtendedDue = true;
				lmsliteratureborrowdetail.ReturnDate = SelectedReturnDate;
				SelectedReturnDate = null;
				lmsliteratureborrowdetail.ExtendDueDate = null;
				isAllowedToReturnDisableFromUI = false;
			}

		}

		public class BorrowApprovalStatusEnum
		{
			public int DecisionId { get; set; }
			public string Name { get; set; }
		}

		protected List<BorrowApprovalStatusEnum> BorrowApprovalStatusOptions { get; set; } = new List<BorrowApprovalStatusEnum>();
		protected IEnumerable<BorrowApprovalStatusEnum> getExcludedLiteratureBorrowApprovalType;

		protected override async Task OnInitializedAsync()
		{
			foreach (BorrowApprovalStatus item in Enum.GetValues(typeof(BorrowApprovalStatus)))
			{
				BorrowApprovalStatusOptions.Add(new BorrowApprovalStatusEnum
				{
					Name = translationState.Translate(item.GetDisplayName()),
					DecisionId = (int)item
				});
			}

			ChangeApprovalTypeForAddingDecision(BorrowApprovalStatusOptions);


			await Load();
		}
		private void ChangeApprovalTypeForAddingDecision(IEnumerable<BorrowApprovalStatusEnum> BorrowApprovalStatusOptions)
		{
			try
			{
				List<BorrowApprovalStatusEnum> approvalTypes = new List<BorrowApprovalStatusEnum>();
				foreach (var item in BorrowApprovalStatusOptions)
				{
					if (item.DecisionId == (int)BorrowApprovalStatus.Approved)
					{
						item.Name = translationState.Translate("Approve");
						approvalTypes.Add(item);
					}
					if (item.DecisionId == (int)BorrowApprovalStatus.Rejected)
					{
						item.Name = translationState.Translate("Reject");
						approvalTypes.Add(item);
					}
				}
				getExcludedLiteratureBorrowApprovalType = approvalTypes;
			}
			catch (Exception ex)
			{

				throw ex;
			}
		}

		public bool isPendingOrApprovedStatus = false;
		protected async Task Load()
		{
			try
			{
				spinnerService.Show();

				var borrowDetail = await lmsLiteratureBorrowDetailService.GetUniqueLmsLiteratureBorrowDetails(BorrowId);
                if (borrowDetail.IsSuccessStatusCode)
                {
                    lmsliteratureborrowdetail = (LmsLiteratureBorrowDetail)borrowDetail.ResultData;
                }
                else
                {
					await invalidRequestHandlerService.ReturnBadRequestNotification(borrowDetail);
                }
				SaveBorrowApprovalStatus = lmsliteratureborrowdetail.BorrowApprovalStatus;
				if (lmsliteratureborrowdetail.BorrowApprovalStatus == (int)BorrowApprovalStatus.PendingForApproval || lmsliteratureborrowdetail.BorrowApprovalStatus == (int)BorrowApprovalStatus.Approved || lmsliteratureborrowdetail.BorrowReturnApprovalStatus == (int)BorrowReturnApprovalStatus.Rejected || lmsliteratureborrowdetail.BorrowReturnApprovalStatus == (int)BorrowReturnApprovalStatus.PendingForReturnBookApproval)
					isPendingOrApprovedStatus = true;

				var literatureDetail = await lmsLiteratureService.GetLmsLiteratureById(lmsliteratureborrowdetail.LiteratureId);
				lmsliteratureborrowdetail.ISBN = literatureDetail.ISBN;
				lmsliteratureborrowdetail.LiteratureName = literatureDetail.Name;


                UserPersonalInformationVM? userInfo = null;
				var response = await userService.GetUserById(lmsliteratureborrowdetail.UserId);
				if (response.IsSuccessStatusCode)
				{
					userInfo = (UserPersonalInformationVM)response.ResultData;
					if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
					{

						lmsliteratureborrowdetail.UserName = userInfo.FirstName_En + userInfo.LastName_En;
					}
					else
					{
						lmsliteratureborrowdetail.UserName = userInfo.FirstName_Ar + userInfo.LastName_Ar;

					}
					lmsliteratureborrowdetail.PhoneNumber = userInfo.PhoneNumber == null ? "---" : userInfo.PhoneNumber;
					lmsliteratureborrowdetail.EligibleCount = userInfo.EligibleCount;
				}

				var CurrentDate = DateTime.Now;
				if (CurrentDate > lmsliteratureborrowdetail.DueDate)
					isExtended = false;
				else
					isExtended = true;
				if (userInfo.EligibleCount == 0)
					isAllowToReturnDisable = true;
				else
					isAllowToReturnDisable = false;
				if (lmsliteratureborrowdetail.ExtendDueDate != null && lmsliteratureborrowdetail.ReturnDate != null)
				{
					isExtended = true;
				}
				//<History Author = 'Nadia Gull' Date='2022-11-8' Version="1.0" Branch="master"> call sevice method that returns the list of User Borrow Literatures</History>
				var BorrowLiteratureDetails = await userService.UserBorrowLiteratures(lmsliteratureborrowdetail.UserId);
				if(BorrowLiteratureDetails.IsSuccessStatusCode)
				{
					userBorrowLiteratureDetails = (List<UserBorrowLiteratureVM>)BorrowLiteratureDetails.ResultData;

                }

				await InvokeAsync(StateHasChanged);

				spinnerService.Hide();
			}
			catch (Exception ex)
			{
				notificationService.Notify(new NotificationMessage()
				{
					Severity = NotificationSeverity.Error,
					Detail = translationState.Translate("Error_loading_borrower_details"),
					Style = "position: fixed !important; left: 0; margin: auto; "
				});
				throw new Exception(ex.Message);
			}
		}

		public void DateRender(DateRenderEventArgs args)
		{
			var issueDate = lmsliteratureborrowdetail.IssueDate;
			args.Disabled = args.Date.Date < issueDate.Date;
		}

		public void ExtendedDueDateRender(DateRenderEventArgs args)
		{
			var dueDate = lmsliteratureborrowdetail.DueDate;
			IEnumerable<DateTime> dates = new DateTime[]
			{
				dueDate.AddDays(1).Date,
				dueDate.AddDays(2).Date,
				dueDate.AddDays(3).Date,
				dueDate.AddDays(4).Date,
				dueDate.AddDays(5).Date,
				dueDate.AddDays(6).Date,
				dueDate.AddDays(7).Date
			};
			args.Disabled = !dates.Contains(args.Date);
		}

		public void OnChangeReturnDate()
		{
			if (lmsliteratureborrowdetail.ReturnDate != null)
			{
				isExtended = true;
			}
		}

		void OnChangExtended(bool? isExtendedvalue, string name)
		{
			if (isExtendedvalue == true)
			{
				isExtendedDue = true;
			}
			else
			{
				isExtendedDue = false;
			}
		}
		protected async Task Button2Click(MouseEventArgs args)
		{
			dialogService.Close(null);
		}

		protected async Task FormSubmit(LmsLiteratureBorrowDetail args)
		{
			try
			{
				if (lmsliteratureborrowdetail.BorrowApprovalStatus == 0)
				{
					notificationService.Notify(new NotificationMessage()
					{
						Severity = NotificationSeverity.Warning,
						Detail = translationState.Translate("Please_Select_Literature_Decision"),
						Style = "position: fixed !important; left: 0; margin: auto; "
					});
					return;
				}
				else
				{
					bool? dialogResponse = await dialogService.Confirm(
					   translationState.Translate("Are_you_sure_you_want_to_save_this_change"),
					   translationState.Translate("Confirm"),
					   new ConfirmOptions()
					   {
						   OkButtonText = @translationState.Translate("OK"),
						   CancelButtonText = @translationState.Translate("Cancel")
					   });
					LmsLiteratureBorrowDetail lmsLiteratureBorrowDetail;
					if (dialogResponse == true)
					{
						// if library admin handle return request then run this scenario 
						if (lmsliteratureborrowdetail.BorrowReturnApprovalStatus != (int)BorrowReturnApprovalStatus.Default)
						{
							if (lmsliteratureborrowdetail.BorrowApprovalStatus == (int)BorrowApprovalStatus.Approved)
							{
								lmsliteratureborrowdetail.BorrowReturnApprovalStatus = (int)BorrowReturnApprovalStatus.Returned;
							}
							else if (lmsliteratureborrowdetail.BorrowApprovalStatus == (int)BorrowApprovalStatus.Rejected)
							{
								lmsliteratureborrowdetail.BorrowReturnApprovalStatus = (int)BorrowReturnApprovalStatus.Rejected;
								lmsliteratureborrowdetail.BorrowApprovalStatus = SaveBorrowApprovalStatus;
								SaveBorrowApprovalStatus = 0;
							}
						}
						if (lmsliteratureborrowdetail.BorrowApprovalStatus == (int)BorrowApprovalStatus.Approved)
						{
							lmsliteratureborrowdetail.ApprovalDate = DateTime.Now;
							lmsliteratureborrowdetail.DueDate = DateTime.Now.AddDays((int)lmsliteratureborrowdetail.BorrowReturnDayDuration);
                            //lmsliteratureborrowdetail.DueDate = DateTime.Now.AddDays(14);
						}
                        if (loginState.UserRoles.Any(u => u.RoleId == SystemRoles.LMSAdmin)) // role id getting to check the admin
                        {
                            lmsliteratureborrowdetail.RoleId = SystemRoles.LMSAdmin;
                        }
                        else if (loginState.UserRoles.Any(u => u.RoleId == SystemRoles.FatwaAdmin)) // role id getting to check the admin
                        {
                            lmsliteratureborrowdetail.RoleId = SystemRoles.FatwaAdmin;
                        }
                        else // if there is no admin role except LMSAdmin and LMSAdmin then role id will be null
                             // and LmsLiteraturesController in (CreateLmsLiterature method) using there this role Id.
                        {
                            lmsliteratureborrowdetail.RoleId = null;
                        }
                        var response = await lmsLiteratureBorrowDetailService.UpdateLmsLiteratureBorrowDetail((BorrowId), lmsliteratureborrowdetail);
						if (response.IsSuccessStatusCode)
						{
							lmsLiteratureBorrowDetail = (LmsLiteratureBorrowDetail)response.ResultData;
							if (lmsLiteratureBorrowDetail != null)
							{
								notificationService.Notify(new NotificationMessage()
								{
									Severity = NotificationSeverity.Success,
									Detail = translationState.Translate("Borrower_updated_successfully"),
									Style = "position: fixed !important; left: 0; margin: auto; "
								});
							}
							StateHasChanged();
						}
						else
						{
							await invalidRequestHandlerService.ReturnBadRequestNotification(response);
						}


						//dialogService.Close(lmsliteratureborrowdetail);
						navigationManager.NavigateTo("/lmsliteratureborrowdetail-approval-list");
						await Load();

					}
					#region cancel Borrow Detail scenario
					else
					{
						dialogResponse = await dialogService.Confirm(
							@translationState.Translate("Sure_Cancel"),
							@translationState.Translate("Confirm_Cancel"),
							new ConfirmOptions()
							{
								CloseDialogOnOverlayClick = true,
								OkButtonText = @translationState.Translate("OK"),
								CancelButtonText = @translationState.Translate("Cancel")
							});
						if (dialogResponse == true)
							await Load();
					}
						#endregion
				}
			}
			catch (Exception)
			{
				notificationService.Notify(new NotificationMessage()
				{
					Severity = NotificationSeverity.Error,
					Detail = translationState.Translate("Borrower_could_not_be_updated"),
					Style = "position: fixed !important; left: 0; margin: auto; "
				});
			}
		}

		protected void ButtonCancelClick()
		{
			try
			{
				navigationManager.NavigateTo("/lmsliteratureborrowdetail-list");

			}
			catch (Exception ex)
			{
				throw new Exception(ex.Message);
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
		#endregion
	}



}
