using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using FATWA_WEB.Services;
using Microsoft.EntityFrameworkCore;
using FATWA_DOMAIN.Models;
using FATWA_WEB.Pages.Lms;
using FATWA_WEB.Data;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_WEB.Extensions;
using System.ComponentModel.DataAnnotations;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.Lms
{
	public class EnumSearchViewByReturn
	{
		public ReturnDetailVM.SearchViewBy EnumValue { get; set; }
		public string EnumName { get; set; }
	}
	//<History Author = 'Nabeel ur Rehman' Date='2022-03-16' Version="1.0" Branch="master">Literature Return book Component</History>
	public partial class ListLiteratureReturn : ComponentBase
    {
        #region Variables Declaration

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        protected DateTime? SearchBoxValue { get; set; }

        protected string SearchBoxTextValue { get; set; } = null;
        protected ReturnDetailVM.SearchViewBy SerachTypeId { get; set; } = ReturnDetailVM.SearchViewBy.None;

        protected RadzenDataGrid<ReturnDetailVM> grid0;
        protected int count { get; set; }

        protected DateTime Min = new DateTime(1950, 1, 1);
        protected DateTime Max = new DateTime(DateTime.Now.Date.Ticks);
        public bool isVisible { get; set; }
		protected UserDetailVM userDetails { get; set; } = new UserDetailVM();
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
                    var args = new FATWA_WEB.Services.PropertyChangedEventArgs() { Name = "search", NewValue = value, OldValue = _search };
                    _search = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        IEnumerable<ReturnDetailVM.SearchViewBy> colorSchemes = Enum.GetValues(typeof(ReturnDetailVM.SearchViewBy)).Cast<ReturnDetailVM.SearchViewBy>();
        ReturnDetailVM.SearchViewBy colorScheme = ReturnDetailVM.SearchViewBy.OverdueDate;


        protected List<object> Options = new List<object>();
        IEnumerable<ReturnDetailVM> _getLmsLiteratureReturnDetailsResult;
        IEnumerable<ReturnDetailVM> FilteredGetLmsLiteratureReturnDetailsResult { get; set; } = new List<ReturnDetailVM>();
        protected IEnumerable<ReturnDetailVM> getLmsLiteratureReturnDetailsResult
        {
            get
            {
                return _getLmsLiteratureReturnDetailsResult;
            }
            set
            {
                if (!object.Equals(_getLmsLiteratureReturnDetailsResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getLmsLiteratureReturnDetailsResult", NewValue = value, OldValue = _getLmsLiteratureReturnDetailsResult };
					_getLmsLiteratureReturnDetailsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected ReturnDetailVM _lmsliteratureborrowdetailsingle;
        protected ReturnDetailVM lmsliteratureborrowdetail
        {
            get
            {
                return _lmsliteratureborrowdetailsingle;
            }
            set
            {
                if (!object.Equals(_lmsliteratureborrowdetailsingle, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "lmsliteratureborrowdetail", NewValue = value, OldValue = _lmsliteratureborrowdetailsingle };
                    _lmsliteratureborrowdetailsingle = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected bool Keywords = false;

        #endregion

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(FATWA_WEB.Services.PropertyChangedEventArgs args)
        {
        }

        #region On Component Load

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
			userDetails = await BrowserStorage.GetItemAsync<UserDetailVM>("UserDetail");
			await Load();
            translationState.TranslateGridFilterLabels(grid0);

            spinnerService.Hide();

            foreach (ReturnDetailVM.SearchViewBy item in Enum.GetValues(typeof(ReturnDetailVM.SearchViewBy)))
            {
                Options.Add(new EnumSearchViewByReturn
                {
                    EnumName = translationState.Translate(item.ToString()),
                    EnumValue = item
                }
                );
            }
        }
        protected virtual async Task Load()
        {
            var result = await lmsLiteratureBorrowDetailService.GetLmsLiteratureReturnDetails();
            if (result.IsSuccessStatusCode)
            {
                getLmsLiteratureReturnDetailsResult = (IEnumerable<ReturnDetailVM>)result.ResultData;
                FilteredGetLmsLiteratureReturnDetailsResult = (IEnumerable<ReturnDetailVM>)result.ResultData;
                count = getLmsLiteratureReturnDetailsResult.Count();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
        }
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
                FilteredGetLmsLiteratureReturnDetailsResult = await gridSearchExtension.Filter(getLmsLiteratureReturnDetailsResult,
                    new Query()
                    {
                        Filter = $@"i => (i.UserName != null && i.UserName.ToLower().Contains(@0)) || (i.LiteratureName != null && i.LiteratureName.ToLower().Contains(@1) ) || i.ISBN.ToString().ToLower().Contains(@2)||(i.BarCodeNumber != null && i.BarCodeNumber.ToLower().Contains(@0))",
                        FilterParameters = new object[] { search, search, search }
                    });
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
            #endregion

            #region Export File

            protected async Task Splitbutton0Click(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await lmsLiteratureBorrowDetailService.ExportLmsLiteratureBorrowDetailsToCSV(new Query()
                {
                    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter) ? "true" : grid0.Query.Filter)}",
                    OrderBy = $"{grid0.Query.OrderBy}",
                    Expand = "",
                    Select = "LiteratureName, ISBN, UserName, IssueDate, ReturnDate, DueDate, ExtendDueDate"
                },
                $"Lms Literature Borrow List");

            }

            if (args == null || args.Value == "xlsx")
            {
                await lmsLiteratureBorrowDetailService.ExportLmsLiteratureBorrowDetailsToExcel(new Query()
                {
                    Filter = $@"{(string.IsNullOrEmpty(grid0.Query.Filter) ? "true" : grid0.Query.Filter)}",
                    OrderBy = $"{grid0.Query.OrderBy}",
                    Expand = "",
                    Select = "LiteratureName, ISBN, UserName, IssueDate, ReturnDate, DueDate, ExtendDueDate"
                },
                $"Lms Literature Borrow List");
            }
        }

        #endregion

        #region Functions

        protected async Task SearchAsync()
        {
            //DateTime SearchValue = SearchBoxValue;
            //DateTime searchTextValue = SearchBoxTextValue;
            spinnerService.Show();

            Keywords = true;
            var enumtype = SerachTypeId;
            ReturnDetailVM.SearchViewBy SearchEnum = enumtype;
            
            var response = await lmsLiteratureBorrowDetailService.GetLmsLiteratureReturnDetails(
                new Query()
                {
                    Filter = $@"i => i.UserName.Contains(@0)|| i.LiteratureName.Contains(@1) || i.ISBN.Contains(@2)",
                    FilterParameters = new object[] { search, search, search }
                });
            if (response.IsSuccessStatusCode)
            {
                getLmsLiteratureReturnDetailsResult = (IEnumerable<ReturnDetailVM>)response.ResultData;

                switch (SearchEnum)
                {
                    case ReturnDetailVM.SearchViewBy.ReturnDate:
                        {
                            getLmsLiteratureReturnDetailsResult = getLmsLiteratureReturnDetailsResult.Where(x => x.ReturnDate <= SearchBoxValue).ToList().OrderByDescending(x => x.CreatedDate);
                            break;
                        }
                    case ReturnDetailVM.SearchViewBy.OverdueDate:
                        {
                            getLmsLiteratureReturnDetailsResult = getLmsLiteratureReturnDetailsResult.Where(x => x.DueDate.Date <= SearchBoxValue).ToList().OrderByDescending(x => x.CreatedDate);
                            break;
                        }
                    case ReturnDetailVM.SearchViewBy.BorrowDate:
                        {
                            getLmsLiteratureReturnDetailsResult = getLmsLiteratureReturnDetailsResult.Where(x => x.CreatedDate.Date <= SearchBoxValue).ToList().OrderByDescending(x => x.CreatedDate);
                            break;
                        }
                    case ReturnDetailVM.SearchViewBy.ExtendDate:
                        {
                            getLmsLiteratureReturnDetailsResult = getLmsLiteratureReturnDetailsResult.Where(x => x.ExtendDueDate <= SearchBoxValue).ToList().OrderByDescending(x => x.CreatedDate);
                            break;
                        }
                    case ReturnDetailVM.SearchViewBy.BookName:
                        {
                            if (SearchBoxTextValue != null)
                                getLmsLiteratureReturnDetailsResult = getLmsLiteratureReturnDetailsResult.Where(x => x.LiteratureName.ToLower().Contains(SearchBoxTextValue.ToLower())).ToList().OrderByDescending(x => x.CreatedDate);
                            break;
                        }
                    case ReturnDetailVM.SearchViewBy.UserName:
                        {

                            if (SearchBoxTextValue != null)
                            {
                                getLmsLiteratureReturnDetailsResult = getLmsLiteratureReturnDetailsResult.Where(x => x.UserName.ToLower().Contains(SearchBoxTextValue.ToLower()) && x.ReturnDate == null).ToList().OrderByDescending(x => x.CreatedDate);
                            }
                            break;
                        }
                    case ReturnDetailVM.SearchViewBy.IssueDate:
                        {

                            if (SearchBoxValue != null)
                            {
                                getLmsLiteratureReturnDetailsResult = getLmsLiteratureReturnDetailsResult.Where(x => x.IssueDate <= SearchBoxValue).ToList().OrderByDescending(x => x.CreatedDate);
                            }
                            break;
                        }
                }
                grid0.Reset();
                await grid0.Reload();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            spinnerService.Hide();
        }

        public async void ResetForm()
        {
            SerachTypeId = ReturnDetailVM.SearchViewBy.None;
            SearchBoxValue = null;
            SearchBoxTextValue = null;
            Keywords = false;

            await Load();
            grid0.Reset();
            StateHasChanged();
        }

        protected async Task ButtonAddClick(MouseEventArgs args)
        {
            try
            {
                navigationManager.NavigateTo("/lmsliteratureborrowdetail-add");
                await Task.Delay(400);
                await Load();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected async Task EditLiteratureBorrow(ReturnDetailVM data)
        {
            try
            {
                navigationManager.NavigateTo("/lmsliteratureborrowreturn-edit/" + data.BorrowId +"/"+ true);

                await Task.Delay(100);
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
		protected async Task DecisionLmsLiteratureBorrowDetail(MouseEventArgs args, dynamic data)
		{
			try
			{
				int id = (data.BorrowId);
				var result = await dialogService.OpenAsync<DecisionLiteratureBorrowDetail>(translationState.Translate("Approve/Reject"),
				new Dictionary<string, object>()
				{
					{ "BorrowId", id }
				}
				,
				new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });

				//RefreshFileUploadGrid = false;
				StateHasChanged();
				//RefreshFileUploadGrid = true;
				await Load();

			}
			catch (Exception ex)
			{

			}
		}
		protected async Task DeleteLiteratureBorrow(ReturnDetailVM data)
        { 
            if (await dialogService.Confirm(translationState.Translate("Sure_Delete_The_Record"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                //var response = await lmsLiteratureBorrowDetailService.DeleteLmsLiteratureBorrowDetail(data);
                //if (response.IsSuccessStatusCode)
                //{
                //    notificationService.Notify(new NotificationMessage()
                //    {
                //        Severity = NotificationSeverity.Success,
                //        Detail = translationState.Translate("Deleted_Successfully"),
                //        Style = "position: fixed !important; left: 0; margin: auto;"
                //    });
                //    await Load();
                //    StateHasChanged();
                //}
                //else
                //{
                //    if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                //    {
                //        notificationService.Notify(new NotificationMessage()
                //        {
                //            Severity = NotificationSeverity.Error,
                //            Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                //            Style = "position: fixed !important; left: 0; margin: auto; "
                //        });
                //    }
                //}
            } 
        }

		protected async Task ReturnLiteratureBorrow(ReturnDetailVM data)
		{
			if (await dialogService.Confirm(translationState.Translate("Sure_Returned_The_Record"), translationState.Translate("Confirm"), new ConfirmOptions()
			{
				OkButtonText = translationState.Translate("OK"),
				CancelButtonText = translationState.Translate("Cancel")
			}) == true)
			{
				//var response = await lmsLiteratureBorrowDetailService.DeleteLmsLiteratureBorrowDetail(data);
				//if (response.IsSuccessStatusCode)
				//{
				//	notificationService.Notify(new NotificationMessage()
				//	{
				//		Severity = NotificationSeverity.Success,
				//		Detail = translationState.Translate("Deleted_Successfully"),
				//		Style = "position: fixed !important; left: 0; margin: auto;"
				//	});
				//	await Load();
				//	StateHasChanged();
				//}
				//else
				//{
				//	if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
				//	{
				//		notificationService.Notify(new NotificationMessage()
				//		{
				//			Severity = NotificationSeverity.Error,
				//			Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
				//			Style = "position: fixed !important; left: 0; margin: auto; "
				//		});
				//	}
				//}
			}
		}
		#endregion

		#region Advance Search
		//<History Author = 'Hassan Abbas' Date='2022-09-09' Version="1.0" Branch="master">Open Advance search Popup </History>
		protected async Task ToggleAdvanceSearch()
        {
            isVisible = !isVisible;
            if (!isVisible)
            {
                ResetForm();
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
        #region Badrequest Notification

        private async Task ReturnBadRequestNotification(ApiCallResponse response)
        {
            try
            {
                if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Token_Expired"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    await Task.Delay(5000);
                    await BrowserStorage.RemoveItemAsync("User");
                    await BrowserStorage.RemoveItemAsync("Token");
                    await BrowserStorage.RemoveItemAsync("RefreshToken");
                    await BrowserStorage.RemoveItemAsync("UserDetail");
                    await BrowserStorage.RemoveItemAsync("SecurityStamp");
                    loginState.IsLoggedIn = false;
                    loginState.IsStateChecked = true;
                }
                else
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
        #endregion
    }
}
