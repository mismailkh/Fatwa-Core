using FATWA_DOMAIN.Models.ViewModel.MojStatistics;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.MojStatistics
{
    //<History Author = 'Hassan Abbas' Date='2024-02-13' Version="1.0" Branch="master"> Migrated from Statistics Dashboard Project</History>
    public partial class ListMojStatistics : ComponentBase
    {
        #region Variables
        protected RadzenDataGrid<MojStatsDasboardVM>? grid;
        protected RadzenDataGrid<MojStatsRegisteredDasboardVM>? gridnewregisteredcase;
        public bool isVisible { get; set; }
        public bool isGovFavor { get; set; }
        protected MojStatsAdvanceSearchStatisticesVM advanceSearchVM = new MojStatsAdvanceSearchStatisticesVM();

        public List<MojStatsDasboardVM>? mojstaticesDasboardVM { get; set; } = new List<MojStatsDasboardVM>();
        protected List<MojStatsGovernmentEntityVM> GovtEntities { get; set; } = new List<MojStatsGovernmentEntityVM>();

        protected MojStatsProcessLogsVM rPAStatisticsProcessLogsVMs { get; set; }
        protected MojStatsAdvanceSearchStatisticesVM args;
        //protected bool AdvancedSearchResultGrid;
        protected bool Keywords = false;
        public int selectedIndex { get; set; } = 0;
        public string Username;
        public bool isAdmin = false;
        public bool IsAuthorized { get; set; }
        public bool IsError { get; set; } = false;
        public string[] UserGroupNames => _config.GetSection("UserGroupNames").Get<string[]>();
        public string[] AdminGroupNames => _config.GetSection("AdminGroupNames").Get<string[]>();
        MojStatsDasboardVM selectedRow;
        #endregion

        #region Full property declaration

        IEnumerable<MojStatsDasboardVM> _getStatisticsDashboardDetail;
        IEnumerable<MojStatsDasboardVM> FilteredGetStatisticsDashboardList{ get; set; } = new List<MojStatsDasboardVM>();
        protected IEnumerable<MojStatsDasboardVM> getStatisticsDashboardDetail
        {
            get
            {
                return _getStatisticsDashboardDetail;
            }
            set
            {
                if (!object.Equals(_getStatisticsDashboardDetail, value))
                {
                    var args = new FATWA_WEB.Services.PropertyChangedEventArgs() { Name = "getStatisticsDashboardDetail", NewValue = value, OldValue = _getStatisticsDashboardDetail };
                    _getStatisticsDashboardDetail = value;

                    Reload();
                }
            }
        }
        //New RegisteredCase
        IEnumerable<MojStatsRegisteredDasboardVM> _getNewRegisteredStatisticsDashboardDetail;
        IEnumerable<MojStatsRegisteredDasboardVM> FilteredNewRegisteredStatisticsDashboardList { get; set; } = new List<MojStatsRegisteredDasboardVM>();
        protected IEnumerable<MojStatsRegisteredDasboardVM> getNewRegisteredStatisticsDashboardDetail
        {
            get
            {
                return _getNewRegisteredStatisticsDashboardDetail;
            }
            set
            {
                if (!object.Equals(_getStatisticsDashboardDetail, value))
                {
                    var args = new FATWA_WEB.Services.PropertyChangedEventArgs() { Name = "getNewRegisteredStatisticsDashboardDetail", NewValue = value, OldValue = _getStatisticsDashboardDetail };
                    _getNewRegisteredStatisticsDashboardDetail = value;

                    Reload();
                }
            }
        }
        #endregion
        #region On initialized
        public void Reload()
        {
            //InvokeAsync(StateHasChanged);
        }
        protected override async Task OnInitializedAsync()
        {
            try
            {

                spinnerService.Show();
                IsAuthorized = true;
                await Load();
                await PopulateGovtEntities();
                await GetstatisticprocesslogsDate();
                translationState.TranslateGridFilterLabels(grid);
                spinnerService.Hide();

            }
            catch (Exception ex)
            {
                spinnerService.Hide();
                throw;
            }

        }



        #endregion
        #region on tab change
        public async void OnTabChange(int index)
        {
            search = "";
            selectedIndex = index;
            if (index == 1)
            {
                await LoadNewRegistredCase();
            }
            else
            {
                await Load();
            }
            Reload();
        }
        #endregion
        
        #region Load 
        protected async Task Load()
        {
            try
            {
                
                var response = await mojStatisticsDashboardService.GetMOJStatisticDashboardList(advanceSearchVM);
                if (response.IsSuccessStatusCode)
                {
                    getStatisticsDashboardDetail = (IEnumerable<MojStatsDasboardVM>)response.ResultData;
                    FilteredGetStatisticsDashboardList = (IEnumerable<MojStatsDasboardVM>)response.ResultData;
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
        protected async Task LoadNewRegistredCase()
        {
            try
            {
                
                var response = await mojStatisticsDashboardService.GetMOJStatisticsNewRegisteredCase(advanceSearchVM);
                if (response.IsSuccessStatusCode)
                {
                    getNewRegisteredStatisticsDashboardDetail = (IEnumerable<MojStatsRegisteredDasboardVM>)response.ResultData;
                    FilteredNewRegisteredStatisticsDashboardList = (IEnumerable<MojStatsRegisteredDasboardVM>)response.ResultData;
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
        #endregion
        #region search component
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

                    Reload();
                }
            }
        }
        protected async Task OnSearchInput()
        {
            try
            {
                if (string.IsNullOrEmpty(search))
                    search = "";
                else
                    search = search.ToLower();
                if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
                {

                    FilteredGetStatisticsDashboardList = await gridSearchExtension.Filter(getStatisticsDashboardDetail, new Query()
                    {
                        Filter = $@"i => (i.Name_En != null && i.Name_En.ToString().ToLower().Contains(@0))",
                        FilterParameters = new object[] { search, search }
                    });
                }
                else
                {
                    FilteredGetStatisticsDashboardList = await gridSearchExtension.Filter(getStatisticsDashboardDetail, new Query()
                    {
                        Filter = $@"i => (i.Name_Ar != null && i.Name_Ar.ToString().ToLower().Contains(@0))",
                        FilterParameters = new object[] { search, search }
                    });

                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected async Task OnSearchInputNewRegistredCase()
        {
            try
            {
                if (string.IsNullOrEmpty(search))
                    search = "";
                else
                    search = search.ToLower();
                if (Thread.CurrentThread.CurrentCulture.Name == "en-US")
                {

                    FilteredNewRegisteredStatisticsDashboardList = await gridSearchExtension.Filter(getNewRegisteredStatisticsDashboardDetail, new Query()
                    {
                        Filter = $@"i => (i.Name_En != null && i.Name_En.ToString().ToLower().Contains(@0))",
                        FilterParameters = new object[] { search, search }
                    });
                }
                else
                {
                    FilteredNewRegisteredStatisticsDashboardList = await gridSearchExtension.Filter(getNewRegisteredStatisticsDashboardDetail, new Query()
                    {
                        Filter = $@"i => (i.Name_Ar != null && i.Name_Ar.ToString().ToLower().Contains(@0))",
                        FilterParameters = new object[] { search, search }
                    });

                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Advance Search

        protected async Task SubmitAdvanceSearch()
        {
            if (advanceSearchVM.CreatedFrom > advanceSearchVM.CreatedTo)
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
            if (!advanceSearchVM.id.HasValue
                 && !advanceSearchVM.CreatedFrom.HasValue && !advanceSearchVM.CreatedTo.HasValue)
            {

            }
            else
            {
                Keywords = true;
                MojStatsAdvanceSearchStatisticesVM? advComsObj = new MojStatsAdvanceSearchStatisticesVM();
                advComsObj.id = advanceSearchVM.id;
                advComsObj.EntityName = advanceSearchVM.EntityName;
                advComsObj.EntityId = advanceSearchVM.EntityId;

                advComsObj.CreatedFrom = advanceSearchVM.CreatedFrom;
                advComsObj.CreatedTo = advanceSearchVM.CreatedTo;
                //rPAStatisticsProcessLogsVMs.CreatedFrom =(DateTime) advanceSearchVM.CreatedFrom;
                //rPAStatisticsProcessLogsVMs.CreatedTo =(DateTime) advanceSearchVM.CreatedTo;
                if (selectedIndex == 1)
                {
                    await LoadNewRegistredCase();
                    StateHasChanged();
                }
                else
                {
                    await Load();
                    StateHasChanged();
                }

            }
        }

        public async void ResetForm()
        {
            advanceSearchVM = new MojStatsAdvanceSearchStatisticesVM();
            if (selectedIndex == 1)
            {
                await LoadNewRegistredCase();
                await GetstatisticprocesslogsDate();
                Keywords = false;
                StateHasChanged();
            }
            else
            {
                await GetstatisticprocesslogsDate();
                await Load();
                Keywords = false;
                StateHasChanged();
            }
        }

        //<History Author = 'Ijaz Ahmad' Date='2022-09-28' Version="1.0" Branch="master">Open advance search popup </History>
        protected async Task ToggleAdvanceSearch()
        {
            isVisible = !isVisible;
            if (!isVisible)
            {
                ResetForm();
            }
        }

        #endregion
        #region Remote Dropdown Data and Dropdown Change Events
        //<History Author = 'Ijaz Ahmad' Date='2022-09-30' Version="1.0" Branch="master">Populate Govt Entitie </History>
        protected async Task PopulateGovtEntities()
        {
            var response = await mojStatisticsDashboardService.GetGovernmentEntities();
            if (response.IsSuccessStatusCode)
            {
                GovtEntities = (List<MojStatsGovernmentEntityVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

        }
        //<History Author = 'Ijaz Ahmad' Date='2022-09-30' Version="1.0" Branch="master">Populate Subtypes data</History>
        protected async Task GetstatisticprocesslogsDate()
        {
            try
            {
                var response = await mojStatisticsDashboardService.Getsatisticprocesslogs();
                if (response.IsSuccessStatusCode)
                {
                    rPAStatisticsProcessLogsVMs = (MojStatsProcessLogsVM)response.ResultData;

                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception)
            {

                throw;
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
        protected async Task OnRowSelect(MojStatsDasboardVM row)
        {
            try
            {
                navigationManager.NavigateTo("/can-view/" + row.EntityId + "/" + 2);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion
    }
}
