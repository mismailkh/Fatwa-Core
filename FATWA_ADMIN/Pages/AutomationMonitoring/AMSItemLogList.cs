using FATWA_ADMIN.Services.General;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.AutomationMonitoring;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace FATWA_ADMIN.Pages.AutomationMonitoring
{
    public partial class AMSItemLogList : ComponentBase
    {
        #region Varriables
        [Parameter]
        public dynamic ItemId { get; set; }
        protected bool Keywords = false;
        public bool isVisible { get; set; }
        protected RadzenDataGrid<AMSItemLogVM>? ItemLogsGrid;
        public AMSItemLogVM ItemLogVM { get; set; }
        protected AdvanceSearchProcessVM advanceSearchVM = new AdvanceSearchProcessVM();
        protected bool isLoading { get; set; }
        public int count { get; set; }
        IEnumerable<AMSItemLogVM> _Itemloglist;
        IEnumerable<AMSItemLogVM> FilteredItemLog { get; set; } = new List<AMSItemLogVM>();
        protected IEnumerable<AMSItemLogVM> Itemloglist
        {
            get
            {
                return _Itemloglist;
            }
            set
            {
                if (!object.Equals(_Itemloglist, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "FilteredCaseFiles", NewValue = value, OldValue = _Itemloglist };
                    _Itemloglist = value;

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
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
        #endregion

        #region On Initialized
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();

            await Load();
            spinnerService.Hide();
        }
        public async Task Load()
        {
            isLoading = true;
            var response = await automationmonitoringService.GetItemLogsList(Convert.ToInt32(ItemId));
            if (response.IsSuccessStatusCode)
            {
                _Itemloglist = (IEnumerable<AMSItemLogVM>)response.ResultData;
                FilteredItemLog = (IEnumerable<AMSItemLogVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            isLoading = false;

            StateHasChanged();
        }
        #endregion

        #region Redirect Functions
        public void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        public void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }

        #endregion
    }
}