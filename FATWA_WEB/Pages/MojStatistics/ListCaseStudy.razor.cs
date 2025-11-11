using FATWA_DOMAIN.Models.ViewModel.MojStatistics;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.MojStatistics
{
    //<History Author = 'Hassan Abbas' Date='2024-02-13' Version="1.0" Branch="master"> Migrated from Statistics Case Study Project</History>
    public partial class ListCaseStudy : ComponentBase
    {
        #region Variables
        protected RadzenDataGrid<MojStatsCaseStudyVM>? grid;
        protected RadzenDataGrid<MojStatsRegisteredDasboardVM>? regnewgrid;
        public bool isVisible { get; set; }
        public bool isGovFavor { get; set; }
        protected MojStatsAdvanceSearchStatisticesVM advanceSearchVM = new MojStatsAdvanceSearchStatisticesVM();

        public List<MojStatsCaseStudyVM>? mojstaticesDasboardVM { get; set; } = new List<MojStatsCaseStudyVM>();
        protected List<MojStatsGovernmentEntityVM> GovtEntities { get; set; } = new List<MojStatsGovernmentEntityVM>();

        protected MojStatsProcessLogsVM rPAStatisticsProcessLogsVMs { get; set; }
        protected MojStatsAdvanceSearchStatisticesVM args;
        protected bool Keywords = false;
        #endregion

        #region Full property declaration
        
        IEnumerable<MojStatsCaseStudyVM> _getMojStatisticsCaseStudyRequestDetails;
        IEnumerable<MojStatsCaseStudyVM> FilteredCaseStudyStatisticsDashboardList { get; set; } = new List<MojStatsCaseStudyVM>();
        protected IEnumerable<MojStatsCaseStudyVM> getMojStatisticsCaseStudyRequestDetails
        {
            get
            {
                return _getMojStatisticsCaseStudyRequestDetails;
            }
            set
            {
                if (!object.Equals(_getMojStatisticsCaseStudyRequestDetails, value))
                {
                    var args = new FATWA_WEB.Services.PropertyChangedEventArgs() { Name = "getMojStatisticsCaseStudyRequestDetails", NewValue = value, OldValue = _getMojStatisticsCaseStudyRequestDetails };
                    _getMojStatisticsCaseStudyRequestDetails = value;

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
     
        #region Load 
        protected async Task Load()
        {
            try
            {
                
                var response = await mojStatisticsService.GetMOJCaseStudyRequest(advanceSearchVM);
                if (response.IsSuccessStatusCode)
                {
                    getMojStatisticsCaseStudyRequestDetails = (IEnumerable<MojStatsCaseStudyVM>)response.ResultData;
                    FilteredCaseStudyStatisticsDashboardList = (IEnumerable<MojStatsCaseStudyVM>)response.ResultData;
                    await InvokeAsync(StateHasChanged);
                    spinnerService.Hide();

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

                    FilteredCaseStudyStatisticsDashboardList = await gridSearchExtension.Filter(getMojStatisticsCaseStudyRequestDetails, new Query()
                    {
                        Filter = $@"i => (i.EntityName != null && i.EntityName.ToString().ToLower().Contains(@0)) || (i.CaseAutomatedNumber!=null && i.CaseAutomatedNumber.ToString().Contains(@1))",
                        FilterParameters = new object[] { search, search }
                    });
                }
                else
                {
                    FilteredCaseStudyStatisticsDashboardList = await gridSearchExtension.Filter(getMojStatisticsCaseStudyRequestDetails, new Query()
                    {
                        Filter = $@"i => (i.EntityName != null && i.EntityName.ToString().ToLower().Contains(@0))||(i.CaseAutomatedNumber!=null && i.CaseAutomatedNumber.ToString().Contains(@1))",
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
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                Keywords = true;
                return;
            }
            if (!advanceSearchVM.id.HasValue
                 && !advanceSearchVM.CreatedFrom.HasValue && !advanceSearchVM.CreatedTo.HasValue )
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
                     await Load();
                    StateHasChanged();
                
  
            }
        }

        public async void ResetForm()
        {
            advanceSearchVM = new MojStatsAdvanceSearchStatisticesVM();         
                await GetstatisticprocesslogsDate();
                await Load();
                 Keywords = false;
                StateHasChanged();
           
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
            var response = await mojStatisticsService.GetGovernmentEntities();
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
                var response = await mojStatisticsService.Getsatisticprocesslogs();
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
        protected async Task ViewDetails(MojStatsCaseStudyVM args)
        {
            navigationManager.NavigateTo("/case-study-view/" + args.CaseAutomatedNumber);
        }
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
