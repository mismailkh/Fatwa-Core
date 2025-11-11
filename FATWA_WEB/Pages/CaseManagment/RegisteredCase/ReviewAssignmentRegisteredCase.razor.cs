using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.CommunicationEnums;

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    //< History Author = 'Hassan Abbas' Date = '2022-12-29' Version = "1.0" Branch = "master" >Review Case Assignment Task</History>
    public partial class ReviewAssignmentRegisteredCase : ComponentBase
    {

        #region Parameters
        [Parameter]
        public string CaseId { get; set; }
        #endregion

        #region Variables
        public Guid? FileId { get; set; }
        public Guid referenceId { get; set; }
        protected CmsRegisteredCaseDetailVM registeredCase { get; set; }
        protected CmsCaseFileDetailVM caseFile { get; set; }

        protected RadzenDataGrid<CmsRegisteredCaseStatusHistoryVM> HistoryGrid;
        protected RadzenDataGrid<CmsCaseAssigneesHistoryVM> LawyersGrid;
        protected RadzenDataGrid<CasePartyLinkVM> PartiesGrid;
        protected RadzenDataGrid<HearingVM> HearingGrid;
        protected List<CasePartyLinkVM> CasePartyLinks;
        protected List<CmsRegisteredCaseVM> Subcases;
        protected List<CmsRegisteredCaseVM> MergedCases;
        protected List<HearingVM> Hearings;
        protected List<OutcomeHearingVM> OutcomeHearings;
        protected List<JudgementVM> Judgements = new List<JudgementVM>();
        public IEnumerable<CmsRegisteredCaseStatusHistoryVM> caseStatusHistory { get; set; } = new List<CmsRegisteredCaseStatusHistoryVM>();
        public List<CmsCaseAssigneeVM> caseFileAssignees { get; set; } = new List<CmsCaseAssigneeVM>();
        public IEnumerable<CmsCaseAssigneesHistoryVM> caseFileAssigneesHistory { get; set; } = new List<CmsCaseAssigneesHistoryVM>();
        protected RadzenDataGrid<CommunicationListVM>? CommunicationGrid;
        public IEnumerable<CommunicationListVM> communicationListVm = new List<CommunicationListVM>();
        protected bool RefreshFileUploadGrid { get; set; } = true;

        protected RadzenDataGrid<CmsDraftedDocumentVM> DraftCaseRequestGrid;
		protected string RedirectURL { get; set; }


        #endregion

        #region Draft Request Grid
        IEnumerable<CmsDraftedDocumentVM> _getCmsDraftDocumentDetail;
        protected IEnumerable<CmsDraftedDocumentVM> getCmsDraftDocumentDetail
        {
            get
            {
                return _getCmsDraftDocumentDetail;
            }
            set
            {
                if (!object.Equals(_getCmsDraftDocumentDetail, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getCmsDraftDocumentDetail", NewValue = value, OldValue = _getCmsDraftDocumentDetail };
                    _getCmsDraftDocumentDetail = value;

                    // Reload();
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
                    var args = new FATWA_WEB.Services.PropertyChangedEventArgs() { Name = "search", NewValue = value, OldValue = _search };
                    _search = value;

                    //Reload();
                }
            }
        }
        #endregion

        #region Component Load

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Component Load</History>
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();

            var result = await cmsRegisteredCaseService.GetRegisteredCaseDetailByIdVM(Guid.Parse(CaseId));

            if (result.IsSuccessStatusCode)
            {
                registeredCase = (CmsRegisteredCaseDetailVM)result.ResultData;
                await PopulateCaseFileGrid();
                await PopulateRegisteredCasePartyGrid();
                await PopulateRegisteredCaseStatusHistory();
                await PopulateLawyerAssignmentHistory();
                await PopulateCaseAssignees();
                await PopulateCommunicationList(CaseId);
                await PopulateHearingsGrid();
                await PopulateOutcomesGrid();
                await PopulateJudgementsGrid();
                await PopulateSubcasesGrid();
                await PopulateMergedCasesGrid();
                await GetCaseDraftListByReferenceId();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }

            spinnerService.Hide();
        }

        #endregion

        #region Populate Grids

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Populate Case Parties</History>
        protected async Task PopulateRegisteredCasePartyGrid()
        {
            var partyResponse = await caseRequestService.GetCMSCasePartyDetailById(Guid.Parse(CaseId));
            if (partyResponse.IsSuccessStatusCode)
            {
                CasePartyLinks = (List<CasePartyLinkVM>)partyResponse.ResultData;
                CasePartyLinks = CasePartyLinks?.Select(c => { c.CasePartyCategory = (CasePartyCategoryEnum)c.CategoryId; c.CasePartyType = (CasePartyTypeEnum)c.TypeId; return c; }).ToList();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(partyResponse);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Populate Hearings of a Case</History>
        protected async Task PopulateHearingsGrid()
        {
            var response = await cmsRegisteredCaseService.GetHearingsByCase(Guid.Parse(CaseId));
            if (response.IsSuccessStatusCode)
            {
                Hearings = (List<HearingVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Populate Outcoems of Hearing Hearing</History>
        protected async Task PopulateOutcomesGrid()
        {
            var response = await cmsRegisteredCaseService.GetOutcomesHearingByCase(Guid.Parse(CaseId));
            if (response.IsSuccessStatusCode)
            {
                OutcomeHearings = (List<OutcomeHearingVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Populate Judgements Hearing</History>
        protected async Task PopulateJudgementsGrid()
        {
            var response = await cmsRegisteredCaseService.GetJudgementsByCase(Guid.Parse(CaseId));
            if (response.IsSuccessStatusCode)
            {
                Judgements = (List<JudgementVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Populate Subcases Grid</History>
        protected async Task PopulateSubcasesGrid()
        {
            var response = await cmsRegisteredCaseService.GetSubcasesByCase(Guid.Parse(CaseId));
            if (response.IsSuccessStatusCode)
            {
                Subcases = (List<CmsRegisteredCaseVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Populate Merged Cases Grid</History>
        protected async Task PopulateMergedCasesGrid()
        {
            var response = await cmsRegisteredCaseService.GetMergedCasesByCase(Guid.Parse(CaseId));
            if (response.IsSuccessStatusCode)
            {
                MergedCases = (List<CmsRegisteredCaseVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        public async Task PopulateCommunicationList(string CaseId)
        {
            var CommunicationResponse = await communicationService.GetCommunicationListByCaseId(Guid.Parse(CaseId));
            if (CommunicationResponse.IsSuccessStatusCode)
            {
                communicationListVm = (List<CommunicationListVM>)CommunicationResponse.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(CommunicationResponse);
            }

        }

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Populate Request Grid</History>
        protected async Task PopulateRegisteredCaseStatusHistory()
        {
            var response = await cmsRegisteredCaseService.GetRegisteredCaseStatusHistory(Guid.Parse(CaseId));
            if (response.IsSuccessStatusCode)
            {
                caseStatusHistory = (List<CmsRegisteredCaseStatusHistoryVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Populate Request Grid</History>
        protected async Task PopulateLawyerAssignmentHistory()
        {
            var response = await cmsCaseFileService.GetCaseAssigmentHistory(Guid.Parse(CaseId));
            if (response.IsSuccessStatusCode)
            {
                caseFileAssigneesHistory = (List<CmsCaseAssigneesHistoryVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Populate Request Grid</History>
        protected async Task PopulateCaseAssignees()
        {
            var response = await cmsCaseFileService.GetCaseAssigeeList(Guid.Parse(CaseId));
            if (response.IsSuccessStatusCode)
            {
                caseFileAssignees = (List<CmsCaseAssigneeVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Populate Request Grid</History>
        protected async Task PopulateCaseFileGrid()
        {
            var caseRequestResponse = await cmsCaseFileService.GetCaseFileDetailByIdVM((Guid)registeredCase.FileId);
            if (caseRequestResponse.IsSuccessStatusCode)
            {
                caseFile = (CmsCaseFileDetailVM)caseRequestResponse.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(caseRequestResponse);
            }
        }

        #endregion

        #region  Buttons

        //<History Author = 'Hassan Abbas' Date='2022-10-29' Version="1.0" Branch="master"> Redirect to Case Detail page</History>
        protected async Task DetailCase(CmsRegisteredCaseVM args)
        {
            navigationManager.NavigateTo("/case-view/" + args.CaseId);
        }

        #endregion

        #region Redirect Function
        //<History Author = 'Hassan Abbas' Date='2022-11-25' Version="1.0" Branch="master">Redirect to respective page</History>
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

		#region view response
		protected async Task ViewResponse(CommunicationListVM item)
		{
            if (item.CommunicationTypeId == (int)CommunicationTypeEnum.RequestForMeeting)
            {
                navigationManager.NavigateTo("/meeting-view/" + item.CommunicationId + "/" + item.ReferenceId + "/" + item.CommunicationTypeId + "/" + true + "/" + item.SubModuleId);
            }
            else if (item.CommunicationTypeId == (int)CommunicationTypeEnum.MeetingScheduled)
            {
                navigationManager.NavigateTo("/meeting-view/" + item.CommunicationId + "/" + item.CommunicationTypeId + "/" + true);
            }
            else if (item.CommunicationTypeId == (int)CommunicationTypeEnum.CaseRequest)
            {
                //IsStatusCaseRequest = false;
            }
            else
            {
                RedirectURL = "/request-need-more-detail/" + item.ReferenceId + "/" + item.CommunicationId + "/" + item.SubModuleId + "/" + item.CommunicationTypeId;
                navigationManager.NavigateTo(RedirectURL);
            }
        }
		#endregion

		#region Get Case Draft List By Reference Id
		public async Task GetCaseDraftListByReferenceId()
        {
            if (string.IsNullOrEmpty(search))
            {
                search = "";
            }
            else
                search = search.ToLower();
            referenceId = Guid.Parse(CaseId);
            var response = await cmsCaseTemplateService.GetCaseDraftListByReferenceId(referenceId, new Query()
            {
                Filter = $@"i => (i.DraftNumber != null && i.DraftNumber.ToString().Contains(@0)) || (i.Name != null && i.Name.ToString().ToLower().Contains(@1))",
                FilterParameters = new object[] { search, search }
            }
            );
            if (response.IsSuccessStatusCode)
            {
                getCmsDraftDocumentDetail = (IEnumerable<CmsDraftedDocumentVM>)response.ResultData;
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }


		}
		//<History Author = 'Hassan Abbas' Date='2023-03-11' Version="1.0" Branch="master">Check if Attachments</History>
		public void PartyRowRender(RowRenderEventArgs<CasePartyLinkVM> args)
		{
			try
			{
				if (args.Data.AttachmentCount <= 0)
				{
					args.Attributes.Add("class", "no-party-attachment");
				}
			}
			catch (Exception ex)
			{
			}
		}
		#endregion

		#region Expand list Scheduliing Court Visit        
		//<History Author = 'Danish' Date='2022-11-29' Version="1.0" Branch="master">Expand list Scheduliing Court Visit</History>        
		protected async Task ExpandCourtVisit(HearingVM hearing)
        {
            var response = await cmsRegisteredCaseService.GetSchedulCourtVisitByHearingId(hearing.Id);
            if (response.IsSuccessStatusCode)
            {
                hearing.ScheduleCourtVisit = (List<SchedulingCourtVisitVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion
        #region Grid Button
        //<History Author = 'Ijaz Ahmad' Date='2024-02-07' Version="1.0" Branch="master"> Detail  Case Party Info</History>
        protected void DetailCaseParty(CasePartyLinkVM args)
        {
            navigationManager.NavigateTo("/caseparty-view/" + args.Id);
        }
        #endregion
    }
}
