using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using FATWA_WEB.Services;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using System.ComponentModel.DataAnnotations;
using static FATWA_DOMAIN.Enums.LiteratureEnum;
using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Enums;

namespace FATWA_WEB.Pages.Lms
{
    public class EnumSearchViewBy
    {
        public BorrowDetailVM.SearchViewBy EnumValue { get; set; }
        public string EnumName { get; set; }
    }
    //<History Author = 'Hassan Abbas' Date='2022-03-16' Version="1.0" Branch="master">Literature Classification Component</History>
    public partial class ListLiteratureBorrow : ComponentBase
    {
        #region Variables Declaration

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy/MM/dd}")]
        protected DateTime? SearchBoxValue { get; set; }

        protected string SearchBoxTextValue { get; set; } = null;
        protected BorrowDetailVM.SearchViewBy SerachTypeId { get; set; } = BorrowDetailVM.SearchViewBy.None;

        protected RadzenDataGrid<BorrowDetailVM> grid0;
        protected int count { get; set; }

        protected DateTime Min = new DateTime(1950, 1, 1);
        protected DateTime Max = new DateTime(DateTime.Now.Date.Ticks);
        public bool isVisible { get; set; }

        protected List<LiteratureBorrowApprovalType> LiteratureBorrowApprovalTypeDetails { get; set; } = new List<LiteratureBorrowApprovalType>();

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
        IEnumerable<BorrowDetailVM.SearchViewBy> colorSchemes = Enum.GetValues(typeof(BorrowDetailVM.SearchViewBy)).Cast<BorrowDetailVM.SearchViewBy>();
        BorrowDetailVM.SearchViewBy colorScheme = BorrowDetailVM.SearchViewBy.OverdueDate;


        protected List<object> Options = new List<object>();
        IEnumerable<BorrowDetailVM> _getLmsLiteratureBorrowDetailsResult;
        IEnumerable<BorrowDetailVM> FilteredGetLmsLiteratureBorrowDetailsResult { get; set; } = new List<BorrowDetailVM>();
        protected IEnumerable<BorrowDetailVM> getLmsLiteratureBorrowDetailsResult
        {
            get
            {
                return _getLmsLiteratureBorrowDetailsResult;
            }
            set
            {
                if (!object.Equals(_getLmsLiteratureBorrowDetailsResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getLmsLiteratureBorrowDetailsResult", NewValue = value, OldValue = _getLmsLiteratureBorrowDetailsResult };
                    _getLmsLiteratureBorrowDetailsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected BorrowDetailVM _lmsliteratureborrowdetailsingle;
        protected BorrowDetailVM lmsliteratureborrowdetail
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
			await Load();
            translationState.TranslateGridFilterLabels(grid0);

            spinnerService.Hide();

            foreach (BorrowDetailVM.SearchViewBy item in Enum.GetValues(typeof(BorrowDetailVM.SearchViewBy)))
            {
                Options.Add(new EnumSearchViewBy
                {
                    EnumName = translationState.Translate(item.ToString()),
                    EnumValue = item
                }
                );
            }
        }
        protected virtual async Task Load()
        {
            await GetBorrowApprovalStatusDetails();
            var result = await lmsLiteratureBorrowDetailService.GetLmsLiteratureBorrowDetails();
            if (result.IsSuccessStatusCode)
            {
				getLmsLiteratureBorrowDetailsResult = (IEnumerable<BorrowDetailVM>)result.ResultData;
				FilteredGetLmsLiteratureBorrowDetailsResult = (IEnumerable<BorrowDetailVM>)result.ResultData;

                if (FilteredGetLmsLiteratureBorrowDetailsResult.Count() != 0)
                {
                    foreach (var item in FilteredGetLmsLiteratureBorrowDetailsResult)
                    {
                        if (item.DueDate < DateTime.Now && !item.Extended && item.DecisionId == (int)BorrowApprovalStatus.Approved && item.ApplyReturnDate == null && item.ReturnDate == null)
                        {
                            foreach (var itemType in LiteratureBorrowApprovalTypeDetails)
                            {
                                if (itemType.DecisionId == (int)BorrowApprovalStatus.BorrowingPeriodExpired)
                                {
                                    item.BorrowApprovalStatus = itemType.Name;
                                    item.BorrowApprovalStatusAr = itemType.Name_Ar;
                                }
                            }    
                        }
                    }
                }
                count = getLmsLiteratureBorrowDetailsResult.Count();
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
        protected async Task GetBorrowApprovalStatusDetails()
        {
            var response = await lmsLiteratureBorrowDetailService.GetBorrowApprovalStatusDetails();
            if (response.IsSuccessStatusCode)
            {
                LiteratureBorrowApprovalTypeDetails = (List<LiteratureBorrowApprovalType>)response.ResultData;
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
				FilteredGetLmsLiteratureBorrowDetailsResult = await gridSearchExtension.Filter(getLmsLiteratureBorrowDetailsResult,
					new Query()
					{
						Filter = $@"i => (i.UserName != null && i.UserName.ToString().ToLower().Contains(@0)) || (i.LiteratureName != null && i.LiteratureName.ToString().ToLower().Contains(@1)) || (i.ISBN != null && i.ISBN.ToString().ToLower().Contains(@2)) || (i.BarCodeNumber != null && i.BarCodeNumber.ToString().ToLower().Contains(@3))",
						FilterParameters = new object[] { search, search, search, search }
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
            BorrowDetailVM.SearchViewBy SearchEnum = enumtype;
            var response = await lmsLiteratureBorrowDetailService.GetLmsLiteratureBorrowDetails();
            if (response.IsSuccessStatusCode)
            {
                getLmsLiteratureBorrowDetailsResult = (IEnumerable<BorrowDetailVM>)response.ResultData;
            }
            switch (SearchEnum)
            {
                case BorrowDetailVM.SearchViewBy.ReturnDate:
                    {
                        if (SearchBoxValue != null)
                        {
							getLmsLiteratureBorrowDetailsResult = getLmsLiteratureBorrowDetailsResult.Where(x => x.ReturnDate.HasValue && x.ReturnDate.Value.Date <= SearchBoxValue.Value.Date).OrderByDescending(x => x.CreatedDate).ToList();
						}
                        else
                        {
							getLmsLiteratureBorrowDetailsResult = getLmsLiteratureBorrowDetailsResult.OrderByDescending(x => x.CreatedDate).ToList();
						}
						break;
                    }
                case BorrowDetailVM.SearchViewBy.OverdueDate:
                    {
						if (SearchBoxValue != null)
						{
						    getLmsLiteratureBorrowDetailsResult = getLmsLiteratureBorrowDetailsResult.Where(x => x.DueDate.Date <= SearchBoxValue.Value.Date).OrderByDescending(x => x.CreatedDate).ToList();
						}
						else
						{
							getLmsLiteratureBorrowDetailsResult = getLmsLiteratureBorrowDetailsResult.OrderByDescending(x => x.CreatedDate).ToList();
						}
						break;
                    }
                case BorrowDetailVM.SearchViewBy.BorrowDate:
                    {
						if (SearchBoxValue != null)
						{
							getLmsLiteratureBorrowDetailsResult = getLmsLiteratureBorrowDetailsResult.Where(x => x.CreatedDate.Date <= SearchBoxValue.Value.Date).OrderByDescending(x => x.CreatedDate).ToList();
						}
						else
						{
							getLmsLiteratureBorrowDetailsResult = getLmsLiteratureBorrowDetailsResult.OrderByDescending(x => x.CreatedDate).ToList();
						}
						break;
                    }
                case BorrowDetailVM.SearchViewBy.ExtendDate:
                    {
						if (SearchBoxValue != null)
						{
						    getLmsLiteratureBorrowDetailsResult = getLmsLiteratureBorrowDetailsResult.Where(x => x.ExtendDueDate.HasValue && x.ExtendDueDate.Value.Date <= SearchBoxValue.Value.Date).OrderByDescending(x => x.CreatedDate).ToList();
						}
						else
						{
							getLmsLiteratureBorrowDetailsResult = getLmsLiteratureBorrowDetailsResult.OrderByDescending(x => x.CreatedDate).ToList();
						}
                        break;
                    }
                case BorrowDetailVM.SearchViewBy.BookName:
                    {
                        if (SearchBoxTextValue != null)
                        {
							getLmsLiteratureBorrowDetailsResult = getLmsLiteratureBorrowDetailsResult.Where(x => x.LiteratureName.ToLower().Contains(SearchBoxTextValue.ToLower())).OrderByDescending(x => x.CreatedDate).ToList();
						}
						else
						{
							getLmsLiteratureBorrowDetailsResult = getLmsLiteratureBorrowDetailsResult.OrderByDescending(x => x.CreatedDate).ToList();
						}
						break;
                    }
                case BorrowDetailVM.SearchViewBy.UserName:
                    {

                        if (SearchBoxTextValue != null)
                        {
                            getLmsLiteratureBorrowDetailsResult = getLmsLiteratureBorrowDetailsResult.Where(x => x.UserName.ToLower().Contains(SearchBoxTextValue.ToLower()) && x.ReturnDate == null).OrderByDescending(x => x.CreatedDate).ToList();
                        }
						else
						{
							getLmsLiteratureBorrowDetailsResult = getLmsLiteratureBorrowDetailsResult.OrderByDescending(x => x.CreatedDate).ToList();
						}
						break;
                    }
                case BorrowDetailVM.SearchViewBy.IssueDate:
                    {
                        if (SearchBoxValue != null)
                        {
							getLmsLiteratureBorrowDetailsResult = getLmsLiteratureBorrowDetailsResult.Where(x => x.IssueDate.Date <= SearchBoxValue.Value.Date).OrderByDescending(x => x.CreatedDate).ToList();
						}
                        else
                        {
                            getLmsLiteratureBorrowDetailsResult = getLmsLiteratureBorrowDetailsResult.OrderByDescending(x => x.CreatedDate).ToList();
                        }
                        break;
                    }
            }
            grid0.Reset();
            await grid0.Reload();

            spinnerService.Hide();
        }

        public async void ResetForm()
        {
            SerachTypeId = BorrowDetailVM.SearchViewBy.None;
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
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected async Task EditLiteratureBorrow(BorrowDetailVM data)
        {
            try
            {
                navigationManager.NavigateTo("/lmsliteratureborrowdetail-edit/" + data.BorrowId);

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

        protected async Task DeleteLiteratureBorrow(BorrowDetailVM data)
        { 
            if (await dialogService.Confirm(translationState.Translate("Sure_Delete_The_Record"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {


                var response = await lmsLiteratureBorrowDetailService.DeleteLmsLiteratureBorrowDetail(data);
                if (response.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Deleted_Successfully"),
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

		protected async Task ReturnLiteratureBorrow(BorrowDetailVM data)
		{
            data.LoggedInUser = loginState.UserDetail.UserName;

            if (await dialogService.Confirm(translationState.Translate("Sure_Returned_The_Record"), translationState.Translate("Confirm"), new ConfirmOptions()
			{
				OkButtonText = translationState.Translate("OK"),
				CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
				data.ApplyReturnDate = DateTime.Now;
                data.DecisionId = (int)BorrowReturnApprovalStatus.PendingForReturnBookApproval;
                if (loginState.UserRoles.Any(u => u.RoleId == SystemRoles.LMSAdmin)) // role id getting to check the admin
                {
                    data.RoleId = SystemRoles.LMSAdmin;
                }
                else if (loginState.UserRoles.Any(u => u.RoleId == SystemRoles.FatwaAdmin)) // role id getting to check the admin
                {
                    data.RoleId = SystemRoles.FatwaAdmin;
                }
                else // if there is no admin role except LMSAdmin and LMSAdmin then role id will be null
                     // and LmsLiteraturesController in (CreateLmsLiterature method) using there this role Id.
                {
                    data.RoleId = null;
                }
                var response = await lmsLiteratureBorrowDetailService.UpdateLmsLiteratureReturnDetail(data);
				if (response.IsSuccessStatusCode)
				{
					notificationService.Notify(new NotificationMessage()
					{
						Severity = NotificationSeverity.Success,
						Detail = translationState.Translate("Return_Success"),
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

		#region Edit Borrow Extension
		protected async Task EditLmsLiteratureBorrowDetail(BorrowDetailVM data)
		{
			try
			{
				if (await dialogService.Confirm(translationState.Translate("Sure_Extend_The_Record"), translationState.Translate("Confirm"), new ConfirmOptions()
				{
					OkButtonText = translationState.Translate("OK"),
					CancelButtonText = translationState.Translate("Cancel")
				}) == true)
				{
                    data.ExtensionApprovalStatus = (int)BorrowApprovalStatus.PendingForExtensionApproval;
					data.Extended = true;
					data.ExtendDueDate = DateTime.Now.AddDays(7);
					var response = await lmsLiteratureBorrowDetailService.UpdateLmsLiteratureReturnDetail(data);
					if (response.IsSuccessStatusCode)
					{
						notificationService.Notify(new NotificationMessage()
						{
							Severity = NotificationSeverity.Success,
							Detail = translationState.Translate("Extended_Success"),
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
				//navigationManager.NavigateTo("/lmsliteratureborrowdetail-edit/" + data.BorrowId);
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
	}
}
