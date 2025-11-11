using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Models.ViewModel.LiteratureAdvancedSearchVM;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.Lms
{
	public partial class ListLiteratures : ComponentBase
	{
		// <History Author = 'Nadia Gull' Date='2022-10-18' Version="1.0" Branch="master">ListLiteratures</History>

		#region Variables Declaration
		protected List<LmsLiteratureClassification> literatureClassifications { get; set; } = new List<LmsLiteratureClassification>();
		public bool isVisible { get; set; }
		protected LiteratureAdvancedSearchVM advancedSearchVM = new LiteratureAdvancedSearchVM();
		protected RadzenDataGrid<LiteratureDetailVM> grid;
		protected bool Keywords = false;
		protected List<object> AdvancedSearchOptions { get; set; } = new List<object>();
		protected bool CheckKeywords = false;
		protected UserDetailVM userDetails { get; set; } = new UserDetailVM();
		protected IEnumerable<LmsLiteratureIndex> LiteratureIndexeDetails { get; set; } = new List<LmsLiteratureIndex>();
		public IEnumerable<LmsLiteratureAuthor> LmsLiteratureAuthor { get; set; } = new List<LmsLiteratureAuthor>();
		protected string search { get; set; }
		public bool allowRowSelectOnRowClick = true;
		protected int count;
		public IList<LiteratureDetailVM> selectedLiteratures;
		protected IEnumerable<LiteratureDetailVM> getLmsLiteraturesResult { get; set; } = new List<LiteratureDetailVM>();

		public IEnumerable<LiteratureDetailVM> FilteredGetLmsLiteraturesResult { get; set; }
		public bool isStockTakingInPrgoress { get; set; }
		private string? ColumnName { get; set; }
		private SortOrder SortOrder { get; set; }
		private int CurrentPage => grid.CurrentPage + 1;
		private int CurrentPageSize => grid.PageSize;
		private Timer debouncer;
		private const int debouncerDelay = 500;

		#endregion

		#region Advance Search Decalations
		public class AdvancedSearchEnumTypes
		{
			public LiteratureAdvancedSearchVM.AdvancedSearchDropDownEnum advancedSearchEnumValue { get; set; }
			public string advancedSearchEnumName { get; set; }
		}
		#endregion

		#region On Component Load
		protected override async Task OnInitializedAsync()
		{
			userDetails = await BrowserStorage.GetItemAsync<UserDetailVM>("UserDetail");
			spinnerService.Show();
			translationState.TranslateGridFilterLabels(grid);
			await PopulateLiteratureClassification();
			await PopulateAdvancedSearchOptions();
			await CheckAnyInProgressStockTaking();
			spinnerService.Hide();
		}
		public void Reload()
		{
			InvokeAsync(StateHasChanged);
		}
		#endregion

		#region On Load Grid Data 
		protected async Task OnLoadData(LoadDataArgs dataArgs)
		{
			try
			{
				if (string.IsNullOrEmpty(dataArgs.OrderBy) || CurrentPage != advancedSearchVM.PageNumber || CurrentPageSize != advancedSearchVM.PageSize || (Keywords && advancedSearchVM.isDataSorted))
				{
					if (advancedSearchVM.isGridLoaded && advancedSearchVM.PageSize == CurrentPageSize && !advancedSearchVM.isPageSizeChangeOnFirstLastPage)
					{
						grid.CurrentPage = (int)advancedSearchVM.PageNumber - 1;
						advancedSearchVM.isGridLoaded = false;
						return;
					}
					SetPagingProperties(dataArgs);
					spinnerService.Show();
					var response = await lmsLiteratureService.GetLmsLiteratures(advancedSearchVM);
					if (response.IsSuccessStatusCode)
					{
						getLmsLiteraturesResult = (IEnumerable<LiteratureDetailVM>)response.ResultData;
						FilteredGetLmsLiteraturesResult = (IEnumerable<LiteratureDetailVM>)response.ResultData;
						if (!(string.IsNullOrEmpty(search))) await OnSearchInput(search);
						if (!(string.IsNullOrEmpty(dataArgs.OrderBy)) && (string.IsNullOrEmpty(search)))
						{
							FilteredGetLmsLiteraturesResult = await gridSearchExtension.Sort(FilteredGetLmsLiteraturesResult, ColumnName, SortOrder);
						}
						await InvokeAsync(StateHasChanged);
					}
					else
					{
						await invalidRequestHandlerService.ReturnBadRequestNotification(response);
					}
					spinnerService.Hide();
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

		#region On Sort Grid Data
		private async Task OnSortData(DataGridColumnSortEventArgs<LiteratureDetailVM> args)
		{
			if (args.SortOrder != null)
			{
				FilteredGetLmsLiteraturesResult = await gridSearchExtension.Sort(FilteredGetLmsLiteraturesResult, args.Column.Property, (SortOrder)args.SortOrder);
				ColumnName = args.Column.Property;
				SortOrder = (SortOrder)args.SortOrder;
				advancedSearchVM.isDataSorted = false;
			}
			else
			{
				ColumnName = string.Empty;
			}
		}
		#endregion

		#region Grid Pagination Calculation
		private void SetPagingProperties(LoadDataArgs args)
		{
			if (advancedSearchVM.PageSize != null && advancedSearchVM.PageSize != CurrentPageSize)
			{
				int oldPageCount = getLmsLiteraturesResult.Any() ? (getLmsLiteraturesResult.First().TotalCount) / ((int)advancedSearchVM.PageSize) : 1;
				int oldPageNumber = (int)advancedSearchVM.PageNumber - 1;
				advancedSearchVM.isGridLoaded = true;
				advancedSearchVM.PageNumber = CurrentPage;
				advancedSearchVM.PageSize = args.Top;
				int TotalPages = getLmsLiteraturesResult.Any() ? (getLmsLiteraturesResult.First().TotalCount) / (grid.PageSize) : 1;
				if (CurrentPage > TotalPages)
				{
					advancedSearchVM.PageNumber = TotalPages + 1;
					advancedSearchVM.PageSize = args.Top;
					grid.CurrentPage = TotalPages;
				}
				if ((advancedSearchVM.PageNumber == 1 || (advancedSearchVM.PageNumber == TotalPages + 1 && oldPageCount == oldPageNumber)) && oldPageCount != 0)
				{
					advancedSearchVM.isPageSizeChangeOnFirstLastPage = true;
				}
				else
				{
					advancedSearchVM.isPageSizeChangeOnFirstLastPage = false;
				}
				return;
			}
			advancedSearchVM.PageNumber = CurrentPage;
			advancedSearchVM.PageSize = args.Top;
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

					FilteredGetLmsLiteraturesResult = await gridSearchExtension.Filter(getLmsLiteraturesResult, new Query()
					{
						Filter = $@"i => (i.LiteratureName != null && i.LiteratureName.ToLower().Contains(@0)) 
                            || (i.ISBN != null && i.ISBN.ToLower().Contains(@1)) 
                            || (i.DeweyBookNumber != null && i.DeweyBookNumber.ToLower().Contains(@2)) 
                            || (i.CopyCount != null && i.CopyCount.ToString().ToLower().Contains(@3)) 
                            || (i.Subject != null && i.Subject.ToLower().Contains(@4)) 
                            || (i.BookStatus != null && i.BookStatus.ToLower().Contains(@5)) 
                            || (i.BookStatus_Ar != null && i.BookStatus_Ar.ToLower().Contains(@6)) 
                            || (i.IndexNumber != null && i.IndexNumber.ToLower().Contains(@7))",
						FilterParameters = new object[] { search, search, search, search, search, search, search, search }
					});
					if (!string.IsNullOrEmpty(ColumnName))
					{
						FilteredGetLmsLiteraturesResult = await gridSearchExtension.Sort(FilteredGetLmsLiteraturesResult, ColumnName, SortOrder);
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

		#region Check Stock Taking Status
		protected async Task CheckAnyInProgressStockTaking()
		{
			try
			{
				var response = await lmsLiteratureService.CheckIfAnyInProgressStockTaking();
				if (response.IsSuccessStatusCode)
				{
					isStockTakingInPrgoress = (bool)response.ResultData;
				}
				else
				{
					await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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

		#region GRID Buttons
		//<History Author = 'Hassan Abbas' Date='2022-03-18' Version="1.0" Branch="master"> Redirect to Add book wizard</History>
		protected async Task ButtonAddClick(MouseEventArgs args)
		{
			navigationManager.NavigateTo("/lmsliterature-add");
		}

		protected async Task Splitbutton0Click(RadzenSplitButtonItem args)
		{
			if (args?.Value == "csv")
			{
				await lmsLiteratureService.ExportLmsLiteraturesToCSV(new Query()
				{
					Filter = $@"{(string.IsNullOrEmpty(grid.Query.Filter) ? "true" : grid.Query.Filter)}",
					OrderBy = $"{grid.Query.OrderBy}",
					Expand = "",
					Select = " BookName, LiteratureType, ISBN, AuthorName, IndexNumber, PurchaseDate"
				}, $"Lms Literatures");
			}
			if (args == null || args.Value == "xlsx")
			{
				await lmsLiteratureService.ExportLmsLiteraturesToExcel(new Query()
				{
					Filter = $@"{(string.IsNullOrEmpty(grid.Query.Filter) ? "true" : grid.Query.Filter)}",
					OrderBy = $"{grid.Query.OrderBy}",
					Expand = "",
					Select = " BookName, LiteratureType, ISBN, AuthorName, IndexNumber, PurchaseDate"
				}, $"Lms Literatures");
			}
		}
		protected async Task EditLiterature(LiteratureDetailVM args)
		{
			navigationManager.NavigateTo("/lmsliterature-edit/" + args.LiteratureId);
		}
		//<History Author = 'Hassan Abbas' Date='2022-03-18' Version="1.0" Branch="master"> Redirect to Edit book wizard</History>
		protected async Task ViewLiteratureDetail(LiteratureDetailVM args)
		{
			navigationManager.NavigateTo("/lmsliterature-detail/" + args.LiteratureId);
		}
		protected async Task ButtonDeleteClick(MouseEventArgs args)
		{
			try
			{
				if (selectedLiteratures != null && selectedLiteratures.Any())
				{
					bool? dialogResponse = await dialogService.Confirm(
						translationState.Translate("Sure_Delete_The_Record?"),
						translationState.Translate("Confirm"),
						new ConfirmOptions()
						{
							OkButtonText = translationState.Translate("OK"),
							CancelButtonText = translationState.Translate("Cancel")
						});

					if (dialogResponse == true)
					{
						spinnerService.Show();
						var response = await lmsLiteratureService.DeleteLiterature(selectedLiteratures);
						if (response.IsSuccessStatusCode)
						{
							notificationService.Notify(new NotificationMessage()
							{
								Severity = NotificationSeverity.Success,
								Detail = translationState.Translate("Literature_Deleted_Successfully"),
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
							grid.Reset();
							await grid.Reload();
							spinnerService.Hide();
						}
					}
				}
				else
				{
					bool? dialogResponse = await dialogService.Confirm(
					   translationState.Translate("Select_Record_For_Delete"),
					   translationState.Translate("warning"),
					   new ConfirmOptions()
					   {
						   OkButtonText = translationState.Translate("OK"),
						   CancelButtonText = translationState.Translate("Cancel")
					   });
				}
			}
			catch (Exception)
			{
				notificationService.Notify(new NotificationMessage()
				{
					Severity = NotificationSeverity.Error,
					Detail = translationState.Translate("Literature_Delete_Failed"),
					Style = "position: fixed !important; left: 0; margin: auto; "
				});
			}
		}
		#endregion

		#region Remote Dropdown Data and Dropdown Change Events
		protected async Task PopulateLiteratureClassification()
		{
			var response = await lmsLiteratureClassificationService.GetLiteratureClassifications();
			if (response.IsSuccessStatusCode)
			{
				literatureClassifications = (List<LmsLiteratureClassification>)response.ResultData;
			}
			else
			{
				await invalidRequestHandlerService.ReturnBadRequestNotification(response);
			}
			StateHasChanged();
		}
		protected async Task PopulateAdvancedSearchOptions()
		{
			foreach (LiteratureAdvancedSearchVM.AdvancedSearchDropDownEnum item in Enum.GetValues(typeof(LiteratureAdvancedSearchVM.AdvancedSearchDropDownEnum)))
			{
				AdvancedSearchOptions.Add(new AdvancedSearchEnumTypes { advancedSearchEnumName = translationState.Translate(item.ToString()), advancedSearchEnumValue = item });
			}
			StateHasChanged();
		}
		#endregion

		#region Advance Search
		protected async Task SubmitAdvanceSearch()
		{
			if (advancedSearchVM.FromDate > advancedSearchVM.ToDate)
			{
				notificationService.Notify(new NotificationMessage()
				{
					Severity = NotificationSeverity.Error,
					Detail = translationState.Translate("FromDate_NotGreater_ToDate"),
					Style = "position: fixed !important; left: 0; margin: auto; "
				});
				return;
			}
			if (advancedSearchVM.EnumSearchValue != 0)
			{
				if (string.IsNullOrWhiteSpace(advancedSearchVM.KeywordsType)
					&& advancedSearchVM.GenericsIntergerKeyword == 0
					&& !advancedSearchVM.FromDate.HasValue
					&& !advancedSearchVM.ToDate.HasValue) { }
				else
				{
					ResetAndLoadGrid();
				}
			}
			else if (advancedSearchVM.FromDate.HasValue && advancedSearchVM.ToDate.HasValue)
			{
				ResetAndLoadGrid();
			}
		}
		//<History Author = 'Hassan Abbas' Date='2022-09-09' Version="1.0" Branch="master">Open Advance search Popup </History>
		protected async Task ToggleAdvanceSearch()
		{
			isVisible = !isVisible;
			if (!isVisible)
			{
				ResetForm();
			}
		}

		public async void ResetForm()
		{
			advancedSearchVM = new LiteratureAdvancedSearchVM { PageSize = grid.PageSize };
			Keywords = advancedSearchVM.isDataSorted = false;
			grid.Reset();
			await grid.Reload();
			StateHasChanged();
		}
		private async void ResetAndLoadGrid()
		{
			spinnerService.Show();
			Keywords = advancedSearchVM.isDataSorted = true;
			if (grid.CurrentPage > 0)
				await grid.FirstPage();

			else
			{
				advancedSearchVM.isGridLoaded = false;
				await grid.Reload();
			}
			spinnerService.Hide();
			StateHasChanged();
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

		protected async Task OnChangeSearchValue()
		{
			spinnerService.Show();
			advancedSearchVM.KeywordsType = null;
			advancedSearchVM.GenericsIntergerKeyword = 0;
			switch ((int)advancedSearchVM.EnumSearchValue)
			{
				case (int)AdvancedSearchDropDownEnum.Barcode:
					break;

				case (int)AdvancedSearchDropDownEnum.Author_Name:
					await GetAuthors();
					break;

				case (int)AdvancedSearchDropDownEnum.Book_Index:
					await GetLiteratureIndexDetails();
					break;
				default:
					break;
			}
			spinnerService.Hide();
			StateHasChanged();
		}

		private async Task GetLiteratureIndexDetails()
		{
			LiteratureIndexeDetails = await lmsLiteratureIndexService.GetLiteratureIndexDetails();
		}

		private async Task GetAuthors()
		{
			try
			{
				var response = await lmsLiteratureService.GetAuthorItems();
				if (response.IsSuccessStatusCode)
				{
					LmsLiteratureAuthor = (IEnumerable<LmsLiteratureAuthor>)response.ResultData;
				}
				else
				{
					await invalidRequestHandlerService.ReturnBadRequestNotification(response);
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
	}
}
