using FATWA_DOMAIN.Models.ViewModel.MojStatistics;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using System.Reflection.Metadata;
using Telerik.Blazor.Components;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.MojStatistics
{
    //<History Author = 'Hassan Abbas' Date='2024-02-13' Version="1.0" Branch="master"> Migrated from Statistics Case Study Project</History>
    public partial class DetailCaseStudy : ComponentBase
    {
        #region parameter
        [Parameter]
        public dynamic EntityId { get; set; }
        [Parameter]
        public string CaseSide { get; set; }
        public string? EntityName { get; set; }
        [Parameter]
        public string CaseautomatedNumber { get; set; }
        #endregion

        #region Variable
        RadzenDataGrid<MojStatsCasePartiesJudgementAmountVM> partiesjudgmentAmountGrid;
        IList<MojStatsCasePartiesJudgementAmountVM> CasePartiesJudgementAmountVMs;
        List<MojStatsCasePartiesJudgementAmountVM> originalData = new List<MojStatsCasePartiesJudgementAmountVM>();
        MojStatsCasePartiesJudgementAmountVM PartiesToInsert;
        MojStatsCasePartiesJudgementAmountVM PartiesToUpdate;
        List<MojStatsCasePartiesJudgementAmountVM> editedRows = new List<MojStatsCasePartiesJudgementAmountVM>();
        public bool allowRowSelectOnRowClick = true;
        public bool IsView { get; set; } = true;
        public bool IsddlView { get; set; } = true;
        public string Year { get; set; }
        public string? JudgementAmount { get; set; }
        bool isEditMode = false;
        bool isFormSubmitted = false;
        public int? CANId { get; set; }    
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
        bool IsFormValid = false;
        MojStatsCanJudgementStatusVM canJudgement { get; set; } = new MojStatsCanJudgementStatusVM();
        MojStatsCasePartyJudgementAmountVM CasePartyJudgementAmount { get; set; } = new MojStatsCasePartyJudgementAmountVM();
        #endregion

        #region AutoComplete

        protected List<MojStatsGovernmentEntityVM> GovtEntities { get; set; } = new List<MojStatsGovernmentEntityVM>();
        protected List<MojStatsExecutionFileLevelLookupVM> executionfilelevellookups { get; set; } = new List<MojStatsExecutionFileLevelLookupVM>();
        protected List<MojStatsJudgementStatusLookupVM> judgementStatuses { get; set; } = new List<MojStatsJudgementStatusLookupVM>();
        protected List<MojStatsCaseRaisedLookupVM> raisedLookups { get; set; } = new List<MojStatsCaseRaisedLookupVM>();
        List<MojStatsCasePartiesJudgementAmountVM> partiesJudgementAmountList = new List<MojStatsCasePartiesJudgementAmountVM>();
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
            await PopulateJudgementslist(CaseautomatedNumber);
            await PopulatJudgementsByCanId();
            await PopulateCaseStudyPartyAmountlist(CaseautomatedNumber);
            await PopulatePartieslist(CaseautomatedNumber);
            await PopulateFileslist(CaseautomatedNumber);
            await PopulateCaseDetailslist(CaseautomatedNumber);
            await PopulatJudgementsByCanId();
            await PopulateHearingDetailslist(CaseautomatedNumber);
            await PopulateExecutionDetailslist(CaseautomatedNumber);
            await PopulateAccouncementDetailslist(CaseautomatedNumber);
            await GetstatisticprocesslogsDate();
            await PopulateCaseRaised();
            await PopulateJudgementStatus();
            await PopulatExecutionFileLevelStatus();
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

                var response = await mojStatisticsService.GetAutomateNumberbyEntityId(Convert.ToInt32(EntityId), Convert.ToInt32(CaseSide), Year, new Query()
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
                var response = await mojStatisticsService.GetJudgementsDetails(CaseautomatedNumber);
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
                var response = await mojStatisticsService.GetPartiesDetails(CaseautomatedNumber);
                if (response.IsSuccessStatusCode)
                {
                     partiesDetails = (List<MojStatsPartiesDetailsVM>)response.ResultData;
                    if (CasePartiesJudgementAmountVMs.Count()==0)
                    {
                        foreach (var partiesDetail in partiesDetails)
                        {
                            MojStatsCasePartiesJudgementAmountVM judgementAmountVM = new MojStatsCasePartiesJudgementAmountVM
                            {

                                PartyId = (int)partiesDetail.PartyId,
                                PartyName = partiesDetail.PartyName,
                                PartyType = partiesDetail.PartyType
                            };
                            partiesJudgementAmountList.Add(judgementAmountVM);
                            CasePartiesJudgementAmountVMs = (partiesJudgementAmountList);
                        }
                    }
                    
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
                var response = await mojStatisticsService.GetFilesDetails(CaseautomatedNumber);
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
                var response = await mojStatisticsService.GetCaseDetails(CaseautomatedNumber);
                if (response.IsSuccessStatusCode)
                {
                    caseDetails = (List<MojStatsCaseDetailsVM>)response.ResultData;        
                    var caseDetailsList = (IEnumerable<MojStatsCaseDetailsVM>)response.ResultData; 
                        var firstCaseDetail = caseDetailsList.FirstOrDefault();
                        if (firstCaseDetail != null)
                        {
                            
                            canJudgement.CANid = firstCaseDetail.CANid;
                            CANId = firstCaseDetail.CANid;
                        }                                     
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
                var response = await mojStatisticsService.GetHearingDetails(CaseautomatedNumber);
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
                var response = await mojStatisticsService.GetExecutionDetails(CaseautomatedNumber);
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
        public async Task PopulateAccouncementDetailslist(string CaseAutmatedNumber)
        {

            try
            {
                var response = await mojStatisticsService.GetAccouncementsDetails(CaseautomatedNumber);
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

        protected async Task PopulateGovtEntities()
        {
            var response = await mojStatisticsService.GetGovernmentEntities();
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

            var response = await mojStatisticsService.GetJudgementsDetails(CaseautomatedNumber);
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
                var response = await mojStatisticsService.GetExecutionFinancialDetails(executionexpend.ExecutionId);
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

                var response = await mojStatisticsService.GetAutomateNumberbyEntityId(Convert.ToInt32(EntityId), Convert.ToInt32(CaseSide), CaseAutomatedNumberDetails.JudgementYear, new Query()
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
                var response = await mojStatisticsService.GetJudgementsYaers(EntityId, CasesideId);
                if (response.IsSuccessStatusCode)
                {
                    judgementsYearVMs = (List<MojStatsJudgementsYearVM>)response.ResultData;

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
        protected async Task PopulateExecutionProcedureslist(int ExecutionId)
        {
            try
            {
                var response = await mojStatisticsService.GetExecutionProceduresDetails(ExecutionId);
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
        protected async Task PopulateCaseRaised()
        {
            var response = await mojStatisticsService.GetCaseRaised();
            if (response.IsSuccessStatusCode)
            {
                raisedLookups = (List<MojStatsCaseRaisedLookupVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

        }
        protected async Task PopulateJudgementStatus()
        {
            var response = await mojStatisticsService.GetJudgementsStatus();
            if (response.IsSuccessStatusCode)
            {
                judgementStatuses = (List<MojStatsJudgementStatusLookupVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

        }
        protected async Task PopulatExecutionFileLevelStatus()
        {
            var response = await mojStatisticsService.GetExecutionFileLevelStatus();
            if (response.IsSuccessStatusCode)
            {
                executionfilelevellookups = (List<MojStatsExecutionFileLevelLookupVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

        }
        protected async Task PopulatJudgementsByCanId()
        {
            var response = await mojStatisticsService.GetCanJudgementsDetails(CANId);
            if (response.IsSuccessStatusCode)
            {
                var reslut = (MojStatsCanJudgementStatusVM)response.ResultData;
                canJudgement.JudgementStatusId = reslut.JudgementStatusId;
                canJudgement.JudgementAmount = reslut.JudgementAmount;
                canJudgement.JudgementAmountCollected = reslut.JudgementAmountCollected;
                canJudgement.OpenExecution = reslut.OpenExecution;
                canJudgement.ExecutionFilelevelId = reslut.ExecutionFilelevelId;
                canJudgement.Remarks = reslut.Remarks;
                canJudgement.Id = reslut.Id;
                canJudgement.CANid = reslut.CANid;
                canJudgement.RaisedById = reslut.RaisedById;
                IsView = false;//Enable the Save button
            }


        }

        //<History Author = 'Ijaz Ahmad' Date='2023-7-21' Version="1.0" Branch="master"> Populate Case Study Amount Details</History>
        protected async Task PopulateCaseStudyPartyAmountlist(string CaseAutmatedNumber)
        {

            try
            {
                var response = await mojStatisticsService.GetPartiesCaseStudyAmountDetails(CaseAutmatedNumber);
                if (response.IsSuccessStatusCode)
                {
                    CasePartiesJudgementAmountVMs = (List<MojStatsCasePartiesJudgementAmountVM>)response.ResultData;
                    decimal totalPaidAmount = CasePartiesJudgementAmountVMs.Sum(o => o.AmountPaid) ?? 0;
                    JudgementAmount = totalPaidAmount.ToString();
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

        #region save Execution File level
        protected async Task FormSubmit(MojStatsCanJudgementStatusVM canJudgementStatus)
        {
            try
            {
                ApiCallResponse response;
                canJudgementStatus.CANid = canJudgement.CANid;
                canJudgementStatus.JudgementAmount = JudgementAmount;
                response = await mojStatisticsService.SaveCanJudgementStatus(canJudgementStatus);

                if (response.IsSuccessStatusCode)
                {
                    var judgementsDetails = (MojStatsCanJudgementStatusVM)response.ResultData;
                    canJudgement.Id = judgementsDetails.Id;
                    isFormSubmitted = true;

                    foreach (var editedRow in editedRows)
                    {
                        MojStatsCasePartyJudgementAmountVM casePartyJudgementAmount = new MojStatsCasePartyJudgementAmountVM
                        {
                            PartyId = (int)editedRow.PartyId,
                            AmountPaid = editedRow.AmountPaid,
                            AmountReceived = editedRow.AmountReceived,
                            LawyerExpense = editedRow.LawyerExpense,
                            CANJudgementStatusId = canJudgement.Id,
                            CANid = CaseautomatedNumber,
                            IsDeleted = false,
                        };

                        
                        response = await mojStatisticsService.SaveCasePartiesJudgementAmount(casePartyJudgementAmount);

                        if (!response.IsSuccessStatusCode)
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                            return; 
                        }
                    }

                    
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = canJudgementStatus.Id == null ? translationState.Translate("CanJudgementStatus_Submitted_Successfully") : translationState.Translate("Case_Updated_Successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });

                    navigationManager.NavigateTo("/case-study-view/" + CaseautomatedNumber);
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

        #region On Change
        public void OnRaised(object value)
        {
            ValidateForm();
        }
        public void OnJudgementsStatus(object value)
        {
            ValidateForm();
        }
        public void OnExecution(object value)
        {
            if ((bool)value==false)
            {
                IsddlView = true;
            }
            else
            {
                IsddlView= false;
            }
            //ValidateForm();
        }
        public void ExecutionFileleve(object value)
        {
            ValidateForm();
        }

        public void OnRemarksinput(ChangeEventArgs args)
        {
            ValidateForm();
        }
        public void OnRemarks(string args)
        {
            ValidateForm();
        }
        public void OnAmount(ChangeEventArgs args)
        {
            ValidateForm();
        }
        public void OnCollectedAmount(ChangeEventArgs args)
        {
            ValidateForm();
        }

        public void ValidateForm()
        {
            if (canJudgement.OpenExecution != false)
            {
                //IsddlView = false;
                if (canJudgement.JudgementStatusId > 0 && canJudgement.RaisedById > 0 && !string.IsNullOrEmpty(canJudgement.Remarks))
                {

                    IsView = false;
                }
                else
                {

                    IsView = true;
                }
            }

            else
            {
                if (canJudgement.JudgementStatusId > 0 && canJudgement.RaisedById > 0 && !string.IsNullOrEmpty(canJudgement.Remarks))
                {
                    IsddlView = false;
                    IsView = false;
                }
                else
                {
                    //IsddlView = true;
                    IsView = true;
                }
            }
            StateHasChanged();
        }

        #endregion

        void Reset()
        {
            PartiesToInsert = null;
            PartiesToUpdate = null;
        }
        async Task EditRow(MojStatsCasePartiesJudgementAmountVM party)
        {
           
            
            PartiesToUpdate = party;
            await partiesjudgmentAmountGrid.EditRow(party);
            StoreOriginalData();
            isEditMode = true;
        }

        void OnUpdateRow(MojStatsCasePartiesJudgementAmountVM party)
        {
        }
        public async Task SaveRow(MojStatsCasePartiesJudgementAmountVM CasePartiesJudgementAmount)
        {
            
                var existingIndex = editedRows.FindIndex(row => row.Id == CasePartiesJudgementAmount.Id);

                if (existingIndex >= 0)
                {
                   
                    editedRows.RemoveAt(existingIndex);
                }
                editedRows.Add(CasePartiesJudgementAmount);
                await partiesjudgmentAmountGrid.UpdateRow(CasePartiesJudgementAmount);
           
        }
        private TaskCompletionSource<bool> cancelEditTaskCompletionSource;

        private void CancelEdit(MojStatsCasePartiesJudgementAmountVM casePartiesJudgement)
        {
            if (isEditMode)
            {
                RevertChanges();
            }
            else
            {
              
            }
        }
        async Task DeleteRow(MojStatsCasePartiesJudgementAmountVM JudgementAmount)
        {
            Reset();

            if (CasePartiesJudgementAmountVMs.Contains(JudgementAmount))
            {
                ApiCallResponse response;
                response = await mojStatisticsService.DeleteRow(JudgementAmount.Id);
                if (response.IsSuccessStatusCode)
                {
                    await partiesjudgmentAmountGrid.Reload();
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Row_Delete_Successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });

                    CasePartiesJudgementAmountVMs.Remove(JudgementAmount);
                    await partiesjudgmentAmountGrid.Reload();
                    await partiesjudgmentAmountGrid.UpdateRow(JudgementAmount);
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }

               
            }
            else
            {
                partiesjudgmentAmountGrid.CancelEditRow(JudgementAmount);
                await partiesjudgmentAmountGrid.Reload();
            }
        }

        async Task InsertRow()
        {
            PartiesToInsert = new MojStatsCasePartiesJudgementAmountVM();
            await partiesjudgmentAmountGrid.InsertRow(PartiesToInsert);
        }

       async void OnCreateRow(MojStatsCasePartiesJudgementAmountVM PartiesJudgementAmount)
        {
            try
            {
                MojStatsCasePartyJudgementAmountVM casePartyJudgementAmount = new MojStatsCasePartyJudgementAmountVM
                {
                    PartyId = (int)PartiesJudgementAmount.PartyId,
                    AmountPaid = PartiesJudgementAmount.AmountPaid,
                    AmountReceived = PartiesJudgementAmount.AmountReceived,
                    LawyerExpense = PartiesJudgementAmount.LawyerExpense,
                    CANJudgementStatusId = canJudgement.Id // Can Judgement Status Id
                };
                ApiCallResponse response;
                response = await mojStatisticsService.SaveCasePartiesJudgementAmount(casePartyJudgementAmount);
                if (response.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Submitted_Successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    PartiesToInsert = null;
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
       
        private bool AreTotalsValid()
        {
            decimal totalPaidAmount = CasePartiesJudgementAmountVMs.Sum(o => o.AmountPaid) ?? 0;
            JudgementAmount = totalPaidAmount.ToString();
            decimal totalReceivedAmount = CasePartiesJudgementAmountVMs.Sum(o => o.AmountReceived) ?? 0;
            return totalPaidAmount == totalReceivedAmount;
        }
        private void StoreOriginalData()
        {
            originalData = DeepCopy(CasePartiesJudgementAmountVMs);
        }

        private List<MojStatsCasePartiesJudgementAmountVM> DeepCopy(IEnumerable<MojStatsCasePartiesJudgementAmountVM> source)
        {
            return source.Select(item => new MojStatsCasePartiesJudgementAmountVM
            {
              
                PartyId = item.PartyId,
                PartyName = item.PartyName,
                PartyType = item.PartyType,
               LawyerExpense = item.LawyerExpense,
               AmountPaid = item.AmountPaid,
               AmountReceived = item.AmountReceived,
               
            }).ToList();
        }
        private void RevertChanges()
        {
            CasePartiesJudgementAmountVMs = DeepCopy(originalData);
            isEditMode = false;
        }

        private void ValidateInput(MojStatsCasePartiesJudgementAmountVM item, string propertyName)
        {
            if (item != null)
            {
                var propertyInfo = typeof(MojStatsCasePartiesJudgementAmountVM).GetProperty(propertyName);

                if (propertyInfo != null)
                {
                    var currentValue = (decimal?)propertyInfo.GetValue(item);

                    
                    if (currentValue.HasValue && currentValue.Value < 0)
                    {
                        propertyInfo.SetValue(item, 0); 
                    }
                }
            }
        }
    }
}
