using FATWA_DOMAIN.Models.ViewModel;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.Lms
{
    public partial class ListLiteratureBorrowApproval : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

      

        protected RadzenDataGrid<BorrowDetailVM>? grid;

        #region On Load 

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
                    var args = new FATWA_WEB.Services.PropertyChangedEventArgs() { Name = "search", NewValue = value, OldValue = _search };
                    _search = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<BorrowDetailVM> _literatureBorrowApprovalList;
        IEnumerable<BorrowDetailVM> FilteredliteratureBorrowApprovalList { get; set; } = new List<BorrowDetailVM>();
        protected IEnumerable<BorrowDetailVM> literatureBorrowApprovalList
        {
            get
            {
                return _literatureBorrowApprovalList;
            }
            set
            {
                if (!object.Equals(_literatureBorrowApprovalList, value))
                {
                    var args = new PropertyChangedEventArgs()
                    {
                        Name = "literatureBorrowApprovalList",
                        NewValue = value,
                        OldValue = _literatureBorrowApprovalList
                    };

                    _literatureBorrowApprovalList = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();

            await Load();
            translationState.TranslateGridFilterLabels(grid);

            spinnerService.Hide();
        }

        protected async Task Load()
        { 
            var result = await lmsLiteratureBorrowDetailService.GetLiteratureBorrowApprovals();
            if (result.IsSuccessStatusCode)
            {
                literatureBorrowApprovalList = (IEnumerable<BorrowDetailVM>)result.ResultData;
                FilteredliteratureBorrowApprovalList = (IEnumerable<BorrowDetailVM>)result.ResultData;
                count = literatureBorrowApprovalList.Count();
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
				FilteredliteratureBorrowApprovalList = await gridSearchExtension.Filter(literatureBorrowApprovalList,new Query()
				{
					Filter = $@"i => ( i.UserName != null && i.UserName.ToLower().Contains(@0) ) || (i.LiteratureName != null && i.LiteratureName.ToLower().Contains(@1) ) || (i.ISBN != null && i.ISBN.ToString().ToLower().Contains(@2)||( i.BarCodeNumber != null && i.BarCodeNumber.ToLower().Contains(@0)))",
					FilterParameters = new object[] { search, search, search }
				});
			}
            catch (Exception ex)
            {
                throw ex;
            }
        }
			#endregion

			protected async Task EditLmsLiteratureBorrowDetail(MouseEventArgs args, dynamic data)
        {
            try
            {
                navigationManager.NavigateTo("/lmsliteratureborrowdetail-edit/" + data.BorrowId); 
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
						{ "BorrowId", id },
					
						
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
