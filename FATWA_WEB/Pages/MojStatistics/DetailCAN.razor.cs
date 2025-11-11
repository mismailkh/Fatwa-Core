using FATWA_DOMAIN.Models.ViewModel.MojStatistics;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System.Reflection.Metadata;
using Telerik.Blazor.Components;
namespace FATWA_WEB.Pages.MojStatistics
{
    public partial class DetailCAN : ComponentBase
    {
        #region parameter
        [Parameter]
        public dynamic EntityId { get; set; }
        [Parameter]
        public string CaseSide { get; set; }
        public string? EntityName { get; set; }
        #endregion
        #region Variable
        public bool allowRowSelectOnRowClick = true;
        public string CaseautomatedNumber { get; set; }
        public string CaseType { get; set; }
        public string Year { get; set; }
        protected MojStatsCaseAutomatedNumberVM registerdRequestViewResponseVM { get; set; }
        public IEnumerable<MojStatsViewJudgementsVM> judgementsDetailsList { get; set; } = new List<MojStatsViewJudgementsVM>();
        public IEnumerable<MojStatsJudgementsYearVM> judgementsYearVMs { get; set; } = new List<MojStatsJudgementsYearVM>();
        public IEnumerable<MojStatsPartiesDetailsVM> partiesDetails { get; set; } = new List<MojStatsPartiesDetailsVM>();
        public IEnumerable<MojStatsFilesDetailsVM> filesDetails { get; set; } = new List<MojStatsFilesDetailsVM>();
        public IEnumerable<MojStatsCaseDetailsVM> caseDetails { get; set; } = new List<MojStatsCaseDetailsVM>();
        public IEnumerable<MojStatsHearingVM> hearingDetails { get; set; } = new List<MojStatsHearingVM>();
        public IEnumerable<MojStatsExecutionVM> executionVMs { get; set; } = new List<MojStatsExecutionVM>();
        public IEnumerable<MojStatsAnnouncementVM> announcementVMs { get; set; } = new List<MojStatsAnnouncementVM>();
        public IEnumerable<MojStatsExecutionFinancialVM> executionFinancialslist { get; set; } = new List<MojStatsExecutionFinancialVM>();
        public IEnumerable<MojStatsExecutionProcedureVM> executionprocedurelist { get; set; } = new List<MojStatsExecutionProcedureVM>();
        public MarkupString JudgementsStatements { get; set; }
        private int expandedRowIndex = -1;
        public TelerikPdfViewer PdfViewerRef { get; set; }
        public byte[] FileData { get; set; }
        public bool ShowDocumentViewer { get; set; }
        public string DocumentPath { get; set; }
        public bool DisplayDocumentViewer { get; set; }
        #endregion
        #region AutoComplete

        protected List<MojStatsGovernmentEntityVM> GovtEntities { get; set; } = new List<MojStatsGovernmentEntityVM>();
        protected RadzenDataGrid<MojStatsViewJudgementsVM>? JudgmentsGrid;
        protected RadzenDataGrid<MojStatsCaseAutomatedNumberVM>? grid0;
        protected RadzenDataGrid<MojStatsJudgementsYearVM>? JudgementsyearGrid;
        protected RadzenDataGrid<MojStatsPartiesDetailsVM>? PartiesGrid;
        protected RadzenDataGrid<MojStatsFilesDetailsVM>? RelatedFilesGrid;
        protected RadzenDataGrid<MojStatsCaseDetailsVM>? CaseDetailsGrid;
        protected RadzenDataGrid<MojStatsHearingVM>? HearingDetailsGrid;
        protected RadzenDataGrid<MojStatsExecutionVM>? ExecutionDetailsGrid;
        protected RadzenDataGrid<MojStatsAnnouncementVM>? AnnouncementDetailsGrid;
        protected RadzenDataGrid<MojStatsExecutionFinancialVM>? ExecutionFinancialGrid;
        protected RadzenDataGrid<MojStatsExecutionProcedureVM>? ExecutionProceduresGrid;
        IEnumerable<MojStatsJudgementsYearVM> FilteredGetJudgementsYearList { get; set; } = new List<MojStatsJudgementsYearVM>();
        protected MojStatsProcessLogsVM rPAStatisticsProcessLogsVMs { get; set; }
        #endregion  
        #region Load
        IEnumerable<MojStatsCaseAutomatedNumberVM> _getCaseAutomatedNumberVM;
        protected IEnumerable<MojStatsCaseAutomatedNumberVM> getCaseAutomatedNumberVM
        {
            get
            {
                return _getCaseAutomatedNumberVM;
            }
            set
            {
                if (!object.Equals(_getCaseAutomatedNumberVM, value))
                {
                    var args = new FATWA_WEB.Services.PropertyChangedEventArgs() { Name = "getCaseAutomatedNumberVM", NewValue = value, OldValue = _getCaseAutomatedNumberVM };
                    _getCaseAutomatedNumberVM = value;
                    Reload();
                }
            }
        }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateJudgementsYeaerlist(Convert.ToInt32(EntityId), Convert.ToInt32(CaseSide));
            await PopulateGovtEntities();
            await GetstatisticprocesslogsDate();
            spinnerService.Hide();
        }
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
        #endregion

        protected async Task Load()
        {
            try
            {


                spinnerService.Show();

                if (string.IsNullOrEmpty(search))
                {
                    search = "";
                }

                var response = await mojStatisticsDashboardService.GetAutomateNumberbyEntityId(Convert.ToInt32(EntityId), Convert.ToInt32(CaseSide), Year, new Query()
                {
                    Filter = $@"i => i.CaseAutmatedNumber.Contains(@0)",
                    FilterParameters = new object[] { search }
                });
                getCaseAutomatedNumberVM = (IEnumerable<MojStatsCaseAutomatedNumberVM>)response.ResultData;
                await InvokeAsync(StateHasChanged);
                spinnerService.Hide();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        protected async Task ViewJudgments(MojStatsCaseAutomatedNumberVM args)
        {
            CaseautomatedNumber = args.CaseAutmatedNumber;
            await PopulateJudgementslist(args.CaseAutmatedNumber);
            await PopulatePartieslist(args.CaseAutmatedNumber);
            await PopulateFileslist(args.CaseAutmatedNumber);
            await PopulateCaseDetailslist(args.CaseAutmatedNumber);
            await PopulateHearingDetailslist(args.CaseAutmatedNumber);
            await PopulateExecutionDetailslist(args.CaseAutmatedNumber);
            await PopulateAccouncementDetailslist(args.CaseAutmatedNumber);


        }

        //<History Author = 'Ijaz Ahmad' Date='2023-7-21' Version="1.0" Branch="master"> Populate Request Grid</History>
        protected async Task PopulateJudgementslist(string CaseAutmatedNumber)
        {

            try
            {
                var response = await mojStatisticsDashboardService.GetJudgementsDetails(CaseautomatedNumber);
                if (response.IsSuccessStatusCode)
                {
                    judgementsDetailsList = (List<MojStatsViewJudgementsVM>)response.ResultData;

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
        //<History Author = 'Ijaz Ahmad' Date='2023-7-21' Version="1.0" Branch="master"> Populate Request Grid</History>
        protected async Task PopulatePartieslist(string CaseAutmatedNumber)
        {

            try
            {
                var response = await mojStatisticsDashboardService.GetPartiesDetails(CaseautomatedNumber);
                if (response.IsSuccessStatusCode)
                {
                    partiesDetails = (List<MojStatsPartiesDetailsVM>)response.ResultData;

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
        //<History Author = 'Ijaz Ahmad' Date='2023-7-21' Version="1.0" Branch="master"> Populate Files list</History>
        protected async Task PopulateFileslist(string CaseAutmatedNumber)
        {

            try
            {
                var response = await mojStatisticsDashboardService.GetFilesDetails(CaseautomatedNumber);
                if (response.IsSuccessStatusCode)
                {
                    filesDetails = (List<MojStatsFilesDetailsVM>)response.ResultData;

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
        //<History Author = 'Ijaz Ahmad' Date='2023-7-21' Version="1.0" Branch="master"> Case Details list</History>
        protected async Task PopulateCaseDetailslist(string CaseAutmatedNumber)
        {

            try
            {
                var response = await mojStatisticsDashboardService.GetCaseDetails(CaseautomatedNumber);
                if (response.IsSuccessStatusCode)
                {
                    caseDetails = (List<MojStatsCaseDetailsVM>)response.ResultData;

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
        //<History Author = 'Ijaz Ahmad' Date='2023-09-17' Version="1.0" Branch="master"> Hearing Details list</History>
        protected async Task PopulateHearingDetailslist(string CaseAutmatedNumber)
        {

            try
            {
                var response = await mojStatisticsDashboardService.GetHearingDetails(CaseautomatedNumber);
                if (response.IsSuccessStatusCode)
                {
                    hearingDetails = (List<MojStatsHearingVM>)response.ResultData;

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
        //<History Author = 'Ijaz Ahmad' Date='2023-09-17' Version="1.0" Branch="master"> Execution Details list</History>
        protected async Task PopulateExecutionDetailslist(string CaseAutmatedNumber)
        {

            try
            {
                var response = await mojStatisticsDashboardService.GetExecutionDetails(CaseautomatedNumber);
                if (response.IsSuccessStatusCode)
                {
                    executionVMs = (List<MojStatsExecutionVM>)response.ResultData;

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
        //<History Author = 'Ijaz Ahmad' Date='2023-09-17' Version="1.0" Branch="master"> Accouncement Details list</History>
        protected async Task PopulateAccouncementDetailslist(string CaseAutmatedNumber)
        {

            try
            {
                var response = await mojStatisticsDashboardService.GetAccouncementsDetails(CaseautomatedNumber);
                if (response.IsSuccessStatusCode)
                {
                    announcementVMs = (List<MojStatsAnnouncementVM>)response.ResultData;

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
        //<History Author = 'Ijaz Ahmad' Date='2022-09-30' Version="1.0" Branch="master">Populate process logs date</History>
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

        protected async Task PopulateGovtEntities()
        {
            var response = await mojStatisticsDashboardService.GetGovernmentEntities();
            if (response.IsSuccessStatusCode)
            {
                GovtEntities = (List<MojStatsGovernmentEntityVM>)response.ResultData;
            }

            var governmentEntity = GovtEntities.FirstOrDefault(entity => entity.Id == Convert.ToInt32(EntityId));

            if (governmentEntity != null)
            {
                EntityName = governmentEntity.EntityName;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

        }
        protected async Task ExpandCases(MojStatsViewJudgementsVM viewJudgements)
        {
            allowRowSelectOnRowClick = true;

            if (viewJudgements.JudgementStatement != null)
            {
                JudgementsStatements = (MarkupString)"";
            }

            var response = await mojStatisticsDashboardService.GetJudgementsDetails(CaseautomatedNumber);
            if (response.IsSuccessStatusCode)
            {
                viewJudgements.JudgementsDetails = (List<MojStatsViewJudgementsVM>)response.ResultData;
                var comments = judgementsDetailsList.FirstOrDefault(entity => entity.CaseNumber == viewJudgements.CaseNumber);


                JudgementsStatements = new MarkupString(string.Join("<br>", comments.JudgementStatement));

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        protected async Task ExpandExecution(MojStatsExecutionVM executionexpend)
        {
            try
            {
                await PopulateExecutionProcedureslist(executionexpend.ExecutionId);
                var response = await mojStatisticsDashboardService.GetExecutionFinancialDetails(executionexpend.ExecutionId);
                if (response.IsSuccessStatusCode)
                {
                    executionFinancialslist = (List<MojStatsExecutionFinancialVM>)response.ResultData;

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
        protected async Task ExpandeCaseCanNumber(MojStatsJudgementsYearVM CaseAutomatedNumberDetails)
        {

            try
            {
                Year = CaseAutomatedNumberDetails.JudgementYear;
                spinnerService.Show();

                if (string.IsNullOrEmpty(search))
                {
                    search = "";
                }

                var response = await mojStatisticsDashboardService.GetAutomateNumberbyEntityId(Convert.ToInt32(EntityId), Convert.ToInt32(CaseSide), CaseAutomatedNumberDetails.JudgementYear, new Query()
                {
                    Filter = $@"i => i.CaseAutmatedNumber.Contains(@0)",
                    FilterParameters = new object[] { search }
                });
                getCaseAutomatedNumberVM = (IEnumerable<MojStatsCaseAutomatedNumberVM>)response.ResultData;
                await InvokeAsync(StateHasChanged);
                spinnerService.Hide();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        protected async Task PopulateJudgementsYeaerlist(int EntityId, int CasesideId)
        {

            try
            {
                var response = await mojStatisticsDashboardService.GetJudgementsYaers(EntityId, CasesideId);
                if (response.IsSuccessStatusCode)
                {
                    judgementsYearVMs = (List<MojStatsJudgementsYearVM>)response.ResultData;
                    FilteredGetJudgementsYearList = (List<MojStatsJudgementsYearVM>)response.ResultData;
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

                    FilteredGetJudgementsYearList = await gridSearchExtension.Filter(judgementsYearVMs, new Query()
                    {
                        Filter = $@"i => (i.JudgementYear != null && i.JudgementYear.ToString().ToLower().Contains(@0))",
                        FilterParameters = new object[] { search }
                    });
                }
                else
                {
                    FilteredGetJudgementsYearList = await gridSearchExtension.Filter(judgementsYearVMs, new Query()
                    {
                        Filter = $@"i => (i.JudgementYear != null && i.JudgementYear.ToString().ToLower().Contains(@0))",
                        FilterParameters = new object[] { search }
                    });

                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected async Task PopulateExecutionProcedureslist(int ExecutionId)
        {
            try
            {
                var response = await mojStatisticsDashboardService.GetExecutionProceduresDetails(ExecutionId);
                if (response.IsSuccessStatusCode)
                {
                    executionprocedurelist = (List<MojStatsExecutionProcedureVM>)response.ResultData;

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
        #region View File
        //<History Author = 'ijaz Ahmad' Date='2023-05-23' Version="1.0" Branch="master"> File view</History>
        protected async Task ViewFiles(MojStatsFilesDetailsVM args)
        {
            try
            {


                var dialogResult = await dialogService.OpenAsync<PDFView>(

                "",
                 new Dictionary<string, object>() { { "FilesId", args.FilesId } },
               new DialogOptions() { Width = "95% !important", CloseDialogOnOverlayClick = true, });
                await Task.Delay(200);
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
        #region Back to Home Screen
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
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
