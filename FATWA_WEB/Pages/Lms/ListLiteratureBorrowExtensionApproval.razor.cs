using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.LiteratureEnum;

namespace FATWA_WEB.Pages.Lms
{
    public partial class ListLiteratureBorrowExtensionApproval : ComponentBase
    {
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        { }

        
        #region Variables

        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        protected RadzenDataGrid<BorrowDetailVM> grid;
        protected List<LiteratureBorrowApprovalType> LiteratureBorrowApprovalTypeDetails { get; set; } = new List<LiteratureBorrowApprovalType>();

        public int count { get; set; }

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
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<BorrowDetailVM> _getLiteratureBorrowExtensionApprovals;
        IEnumerable<BorrowDetailVM> FilteredGetLiteratureBorrowExtensionApprovals { get; set; } = new List<BorrowDetailVM>();
        protected IEnumerable<BorrowDetailVM> getLiteratureBorrowExtensionApprovals
        {
            get
            {
                return _getLiteratureBorrowExtensionApprovals;
            }
            set
            {
                if (!object.Equals(_getLiteratureBorrowExtensionApprovals, value))
                {
                    var args = new PropertyChangedEventArgs()
                    {
                        Name = "getLiteratureBorrowExtensionApprovals",
                        NewValue = value,
                        OldValue = _getLiteratureBorrowExtensionApprovals
                    };
                    _getLiteratureBorrowExtensionApprovals = value;
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
            translationState.TranslateGridFilterLabels(grid);

            spinnerService.Hide();
        }

        protected async Task Load()
        {
            await GetBorrowApprovalStatusDetails();
            var result = await lmsLiteratureBorrowDetailService.GetLmsLiteratureBorrowExtensionApprovals();
            if (result.IsSuccessStatusCode)
            {
                getLiteratureBorrowExtensionApprovals = (IEnumerable<BorrowDetailVM>)result.ResultData;
                FilteredGetLiteratureBorrowExtensionApprovals = (IEnumerable<BorrowDetailVM>)result.ResultData;
                count = getLiteratureBorrowExtensionApprovals.Count();

                if (FilteredGetLiteratureBorrowExtensionApprovals.Count() != 0)
                {
                    foreach (var item in FilteredGetLiteratureBorrowExtensionApprovals)
                    {
                        if (item.ExtendDueDate < DateTime.Now && item.Extended && item.DecisionId == (int)BorrowApprovalStatus.PendingForExtensionApproval)
                        {
                            foreach (var itemType in LiteratureBorrowApprovalTypeDetails)
                            {
                                if (itemType.DecisionId == (int)BorrowApprovalStatus.ExtendingPeriodExpired)
                                {
                                    item.BorrowApprovalStatus = itemType.Name;
                                    item.BorrowApprovalStatusAr = itemType.Name_Ar;
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
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
                FilteredGetLiteratureBorrowExtensionApprovals = await gridSearchExtension.Filter(getLiteratureBorrowExtensionApprovals,new Query()
                {
                    Filter = $@"i => (i.UserName != null && i.UserName.ToString().ToLower().Contains(@0) ) || i.ISBN.ToString().ToLower()Contains(@1) ",
                    FilterParameters = new object[] { search, search }
                });
            }
            catch (Exception ex)
            {
                throw;
            }
        }
            #endregion

            #region Borrow Approve/Reject

            protected void DecisionExtension(BorrowDetailVM literatureBorrow)
        {
            try
            {
                navigationManager.NavigateTo("/lmsliteratureborrowdetail-decision/" + literatureBorrow.BorrowId);
            }
            catch  
            {
                throw;
            }
        }

		#endregion
		#region Approve Reject Grid Button 
		protected async Task DecisionLmsLiteratureBorrowDetail(dynamic data)
		{
			try
			{
				int id = (data.BorrowId);
				var result = await dialogService.OpenAsync<DecisionLiteratureBorrowExtension>(translationState.Translate("Approve/Reject"),
				new Dictionary<string, object>()
					{
						{ "BorrowId", id },
				    }
				,
					new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });

				StateHasChanged();
			

			}
			catch (Exception ex)
			{

			}
		}

		#endregion

		#region Export Functions

		protected async Task SplitbuttonClick(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await lmsLiteratureBorrowDetailService.ExportLiteratureBorrowApprovalsToCSV(new Query()
                {
                    Filter = $@"{(string.IsNullOrEmpty(grid.Query.Filter) ? "true" : grid.Query.Filter)}",
                    OrderBy = $"{grid.Query.OrderBy}",
                    Expand = "",
                    Select = " UserName, ISBN, IssueDate, DueDate, Extended, ExtendDueDate, ReturnDate"
                }, $"Lms Literature Borrow Approvals");

            }

            if (args == null || args.Value == "xlsx")
            {
                await lmsLiteratureBorrowDetailService.ExportLiteratureBorrowApprovalsToExcel(new Query()
                {
                    Filter = $@"{(string.IsNullOrEmpty(grid.Query.Filter) ? "true" : grid.Query.Filter)}",
                    OrderBy = $"{grid.Query.OrderBy}",
                    Expand = "",
                    Select = " UserName, ISBN, IssueDate, DueDate, Extended, ExtendDueDate, ReturnDate"
                }, $"Lms Literature Borrow Approvals");
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
