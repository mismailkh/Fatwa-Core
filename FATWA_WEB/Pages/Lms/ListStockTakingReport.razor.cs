using FATWA_DOMAIN.Models.Lms;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.Lms;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.Lms
{
	public partial class ListStockTakingReport : ComponentBase
	{
		#region Variables Declaration
		protected RadzenDataGrid<LmsStockTakingListVM> grid = new RadzenDataGrid<LmsStockTakingListVM>();
		public IEnumerable<LmsStockTakingListVM> getLmsStocktakingResult = new List<LmsStockTakingListVM>();
		public IEnumerable<LmsStockTakingListVM> FilteredGetLmsLiteraturesResult { get; set; }
		protected StockTakingAdvancedSearchVM advancedSearchVM = new StockTakingAdvancedSearchVM();
		protected IList<LmsStockTakingStatus> stockTakingStatuses = new List<LmsStockTakingStatus>();
		protected string search { get; set; }
		public bool isVisible { get; set; }
		protected bool Keywords = false;
		private string? ColumnName { get; set; }
		private SortOrder SortOrder { get; set; }
		private int CurrentPage => grid.CurrentPage + 1;
		private int CurrentPageSize => grid.PageSize;
		private Timer debouncer;
		private const int debouncerDelay = 500;
		#endregion

		#region On Component Load
		protected override async Task OnInitializedAsync()
		{
			spinnerService.Show();
			await PopulateStatus();
			translationState.TranslateGridFilterLabels(grid);
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
					var response = await lmsLiteratureService.GetStockTakingList(advancedSearchVM);
					if (response.IsSuccessStatusCode)
					{
						getLmsStocktakingResult = (List<LmsStockTakingListVM>)response.ResultData;
						FilteredGetLmsLiteraturesResult = (List<LmsStockTakingListVM>)response.ResultData;
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

		#region Grid Pagination Calculation
		private void SetPagingProperties(LoadDataArgs args)
		{
			if (advancedSearchVM.PageSize != null && advancedSearchVM.PageSize != CurrentPageSize)
			{
				int oldPageCount = getLmsStocktakingResult.Any() ? (getLmsStocktakingResult.First().TotalCount) / ((int)advancedSearchVM.PageSize) : 1;
				int oldPageNumber = (int)advancedSearchVM.PageNumber - 1;
				advancedSearchVM.isGridLoaded = true;
				advancedSearchVM.PageNumber = CurrentPage;
				advancedSearchVM.PageSize = args.Top;
				int TotalPages = getLmsStocktakingResult.Any() ? (getLmsStocktakingResult.First().TotalCount) / (grid.PageSize) : 1;
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


		#region On Sort Grid Data
		private async Task OnSortData(DataGridColumnSortEventArgs<LmsStockTakingListVM> args)
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

		#region Populate Status
		public async Task PopulateStatus()
		{
			try
			{
				var response = await lookupService.GetStockTakingStatus();
				if (response.IsSuccessStatusCode)
				{
					stockTakingStatuses = (List<LmsStockTakingStatus>)response.ResultData;
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

		#region Grid Search
		protected async Task OnSearchInput(string value)
		{
			try
			{
				debouncer?.Dispose();
				debouncer = new Timer(async (e) =>
				{
					search = string.IsNullOrEmpty(value) ? "" : value.TrimStart().TrimEnd().ToLower();
					FilteredGetLmsLiteraturesResult = await gridSearchExtension.Filter(getLmsStocktakingResult, new Query()
					{
						Filter = Thread.CurrentThread.CurrentCulture.Name == "en-US"
							  ? $@"i =>  (i.Name_En != null && i.Name_En.ToLower().Contains(@1)) || (i.MemeberNameEn != null && i.MemeberNameEn.ToLower().Contains(@2)) || (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@3)) || (i.StockTakingDate!=null && i.StockTakingDate.ToString(""dd/MM/yyyy"").Contains(@4)) || (i.ReportNumber!=null && i.ReportNumber.ToLower().Contains(@5)) || (i.TotalBooks!=null && i.TotalBooks.ToString().Contains(@6))"
							  : $@"i =>  (i.Name_Ar != null && i.Name_Ar.ToLower().Contains(@1)) || (i.MemeberNameAr != null && i.MemeberNameAr.ToLower().Contains(@2)) || (i.CreatedDate != null && i.CreatedDate.ToString(""dd/MM/yyyy"").Contains(@3)) || (i.StockTakingDate!=null && i.StockTakingDate.ToString(""dd/MM/yyyy"").Contains(@4)  || (i.ReportNumber!=null && i.ReportNumber.ToLower().Contains(@5)) || (i.TotalBooks!=null && i.TotalBooks.ToString().Contains(@6))",
						FilterParameters = new object[] { search, search, search, search, search, search, search }
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

		#region Add Button
		protected async Task ButtonAddClick(MouseEventArgs args)
		{
			navigationManager.NavigateTo("/stocktaking-add");
		}
		#endregion

		#region Toggle Advance Search
		protected async Task ToggleAdvanceSearch()
		{
			isVisible = !isVisible;
			if (!isVisible)
			{
				ResetForm();
			}
		}
		protected async Task SubmitAdvanceSearch()
		{
			if (advancedSearchVM.FromDate > advancedSearchVM.ToDate || advancedSearchVM.StockTakingFromDate > advancedSearchVM.StockTakingToDate)
			{
				notificationService.Notify(new NotificationMessage()
				{
					Severity = NotificationSeverity.Error,
					Detail = translationState.Translate("FromDate_NotGreater_ToDate"),
					Style = "position: fixed !important; left: 0; margin: auto; "
				});
				return;
			}
			else
			{
				if (!advancedSearchVM.StockTakingFromDate.HasValue
					&& !advancedSearchVM.StockTakingToDate.HasValue
					&& !advancedSearchVM.StatusId.HasValue
					&& !advancedSearchVM.FromDate.HasValue
					&& !advancedSearchVM.ToDate.HasValue) { }
				else
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
					StateHasChanged();
					spinnerService.Hide();
				}
			}
		}
		public async void ResetForm()
		{
			advancedSearchVM = new StockTakingAdvancedSearchVM { PageSize = grid.PageSize };
			Keywords = advancedSearchVM.isDataSorted = false;
			grid.Reset();
			await grid.Reload();
			StateHasChanged();
		}
		#endregion

		#region GRID Buttons
		protected async Task ViewStocktakingDetail(LmsStockTakingListVM args)
		{
			try
			{
				navigationManager.NavigateTo("/stocktaking-detail/" + args.Id);
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
		protected async Task EditStockTaking(LmsStockTakingListVM args)
		{
			try
			{
				navigationManager.NavigateTo("/stocktaking-add/" + args.Id);
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
		protected async Task GridDeleteButtonClick(LmsStockTakingListVM args)
		{
			try
			{
				bool? dialogResult = await dialogService.Confirm(
					translationState.Translate("Sure_Delete_The_Record"),
					translationState.Translate("Confirm"),
					new ConfirmOptions
					{
						OkButtonText = translationState.Translate("OK"),
						CancelButtonText = translationState.Translate("Cancel")
					});
				if (dialogResult == true)
				{
					var response = await lmsLiteratureService.DeleteLmsStockTaking(args);
					if (response.IsSuccessStatusCode)
					{
						notificationService.Notify(new NotificationMessage()
						{
							Severity = NotificationSeverity.Success,
							Detail = translationState.Translate("StockTaking_Report") + " " + args.ReportNumber + " " + translationState.Translate("Has_Been_Deleted_Successfully"),
							Style = "position: fixed !important; left: 0; margin: auto; "

						});
						grid.Reset();
						await grid.Reload();
					}
					else
					{
						await invalidRequestHandlerService.ReturnBadRequestNotification(response);
					}
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
		protected async Task ViewItemHistory(LmsStockTakingListVM args)
		{
			var dialogResult = await dialogService.OpenAsync<LmsStockTakingHistoryDialog>(translationState.Translate("StockTaking_History"),
			   new Dictionary<string, object>()
			   {
					 { "StockTakingId", args.Id }
			   },
			   new DialogOptions() { Width = "50% !important", CloseDialogOnOverlayClick = true, ShowClose = true }
		   );
		}
		#endregion

	}
}
