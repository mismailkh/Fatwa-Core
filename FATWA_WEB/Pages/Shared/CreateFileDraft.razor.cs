using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CMSCOMSRequestNumberVMs;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_DOMAIN.Models.ViewModel.Shared;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_DOMAIN.Models.WorkflowModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Radzen;
using Radzen.Blazor;
using SelectPdf;
using Syncfusion.Blazor.PdfViewerServer;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;
using Telerik.Blazor.Components;
using Telerik.Blazor.Components.Editor;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Response;
using Bold = Telerik.Blazor.Components.Editor.Bold;
using FontFamily = Telerik.Blazor.Components.Editor.FontFamily;
using FontSize = Telerik.Blazor.Components.Editor.FontSize;
using Format = Telerik.Blazor.Components.Editor.Format;
using Italic = Telerik.Blazor.Components.Editor.Italic;
using Underline = Telerik.Blazor.Components.Editor.Underline;

namespace FATWA_WEB.Pages.Shared
{
    //< History Author = 'Hassan Abbas' Date = '2022-11-01' Version = "1.0" Branch = "master" >Create File Draft</History>
    public partial class CreateFileDraft : ComponentBase, IDisposable
    {
        #region Parameter

        [Parameter]
        public string ReferenceId { get; set; }
        [Parameter]
        public dynamic TypeId { get; set; }
        [Parameter]
        public dynamic? TemplateId { get; set; }
        [Parameter]
        public string? DraftId { get; set; }
        [Parameter]
        public string? VersionId { get; set; }
        [Parameter]
        public string? TaskId { get; set; }
        [Parameter]
        public dynamic ModuleId { get; set; } 
		#endregion

		#region Variables
		public int DocumentTemplateId { get { return Convert.ToInt32(TemplateId); } set { TemplateId = value; } }
        public int DocumentTypeId { get { return Convert.ToInt32(TypeId); } set { TypeId = value; } }
        public int DocumentModuleId { get { return Convert.ToInt32(ModuleId); } set { ModuleId = value; } }
        protected TaskDetailVM taskDetailVM { get; set; }

        protected CmsDraftedTemplate draftedTemplate = new CmsDraftedTemplate
        {
            Id = Guid.NewGuid(),
            DraftedTemplateVersion = new CmsDraftedTemplateVersions()
        };

        protected CmsDraftedTemplateVersions DraftedTemplateVersion = new CmsDraftedTemplateVersions();
        protected RadzenHtmlEditor editor;
        protected List<AttachmentType> AttachmentTypes { get; set; } = new List<AttachmentType>();
        protected Dictionary<int, TelerikEditor> telerikEditors = new Dictionary<int, TelerikEditor>();
        protected TelerikEditor telerikEditor = new TelerikEditor();
        protected DateTime tempDatetime { get; set; } = DateTime.Now;
        protected string TemplateContent { get; set; }
        protected CaseTemplate Template { get; set; }
        protected CmsDraftedDocumentParentEntityDetailVM draftEntityDetail { get; set; }
        protected GovtEntityRepresentativeNamesResponseVM geRepresentatives { get; set; }
        protected List<CaseTemplate> HeaderFooterTemplates { get; set; }
        protected RadzenSteps? steps;
        protected int selectedIndex = 0;
        public bool isVisible { get; set; }
        public bool IsDraftSubmit { get; set; }
        public bool IsContentAligned { get; set; }
        public bool busyPreviewBtn { get; set; }
        public TelerikPdfViewer PdfViewerRef { get; set; }
        public byte[] FileData { get; set; }
        public MarkupString html { get; set; }
        public string Plaintiffs { get; set; }
        public string Defendants { get; set; }
        public string Lawyers { get; set; }
        public string Value { get; set; }
        public bool CreateAnother { get; set; } = false;
        public bool ShowEditor { get; set; } = false;
        public bool IsEditorInitialized { get; set; }
        public List<IEditorTool> Tools { get; set; } = new List<IEditorTool>()
        {
            new EditorButtonGroup(new Bold(), new Italic(), new Underline()),
            new EditorButtonGroup(new AlignLeft(), new AlignCenter(), new AlignRight()),
            new UnorderedList(),
            new InsertTable(),
            new EditorButtonGroup(new AddRowBefore(), new AddRowAfter(), new MergeCells(), new SplitCell()),
            new Format(),
            new FontSize(),
            new FontFamily()
        };
        public string TransKeyHeader = string.Empty;
        int SectorTypeIdCheck = 0;
        public ConsultationFileDetailVM consultationFileDetailVM { get; set; } = new ConsultationFileDetailVM();
        protected List<WorkflowConditionsOptionVM> conditionOptions { get; set; } = new List<WorkflowConditionsOptionVM>();
        public SfPdfViewerServer pdfViewer;
        public string DocumentPath { get; set; } = string.Empty;
        #endregion

        #region Component Load

        //< History Author = 'Hassan Abbas' Date = '2022-11-02' Version = "1.0" Branch = "master" >Populate related data on Component Load</History>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                spinnerService.Show();
                if (DocumentModuleId == (int)WorkflowModuleEnum.COMSConsultationManagement)
                {
                    SectorTypeIdCheck = (int)loginState.UserDetail.SectorTypeId;
                    PopulateTransationKey();
                }
                if (DraftId == null)
                {
                    await PopulateAttachmentTypes();
                    await PopulateDraftNameNumberVersion();
                    await PopulateTemplateSections();
                    await PopulateGovtEntityRepresentatives();
                    await PopulatePartiesFromCaseFile();
                    await PopulateLawyersFromCaseFile();
                    await PopulateParentEntityDetails();
                    await PopulateTaskDetails();
                    await PopulateHeaderFooter();
                    if (Convert.ToInt32(TypeId) == (int)AttachmentTypeEnum.ContractReview)
                    {
                        await PopulateConsultationContractTemplateDetailByUsingFileId(Guid.Parse(ReferenceId));
                    }
                    else
                    {
                        await PopulateTemplateContent();
                    }
                }
                else
                {
                    await PopulateAttachmentTypes();
                    await PopulateDraftNameNumberVersion();
                    await PopulateTemplateContent(VersionId);
                    await PopulatePartiesFromCaseFile();
                    await PopulateLawyersFromCaseFile();
                    await PopulateTaskDetails();
                    await PopulateParentEntityDetails();
                    await PopulateHeaderFooter();
                }
                spinnerService.Hide();
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

        //< History Author = 'Hassan Abbas' Date = '2024-04-04' Version = "1.0" Branch = "master" >Unlock the Version when finished Editing/Closed Tab/Navigated Away etc</History>
        void IDisposable.Dispose()
        {
            if (navigationManager.Uri.Contains("create-filedraft") && VersionId != null)
            {

            }
            else
            {
                if (VersionId != null && applicationState.LockedVersions.Where(x => x.Item1 == Guid.Parse(VersionId)).Any())
                {
                    applicationState.LockedVersions.Remove(applicationState.LockedVersions.Where(x => x.Item1 == Guid.Parse(VersionId)).FirstOrDefault());
                }
            }
        }
        #endregion

        #region Lookup events

        protected async Task PopulateDraftNameNumberVersion()
        {
            ApiCallResponse response;
            if (DraftId == null)
            {
                response = await cmsCaseTemplateService.GetDraftNumberVersionNumber(null, null);
            }
            else
            {
                response = await cmsCaseTemplateService.GetDraftNumberVersionNumber(Guid.Parse(DraftId), Guid.Parse(VersionId));
            }
            if (response.IsSuccessStatusCode)
            {
                draftedTemplate = (CmsDraftedTemplate)response.ResultData;


                if (DraftId == null)
                {
                    draftedTemplate.Name = AttachmentTypes.Where(t => t.AttachmentTypeId == DocumentTypeId).FirstOrDefault().Type_En + "_" + draftedTemplate.DraftNumber.ToString();
                    draftedTemplate.AttachmentTypeId = DocumentTypeId;
                    draftedTemplate.TemplateId = DocumentTemplateId;
                    draftedTemplate.ReferenceId = Guid.Parse(ReferenceId);
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        //<History Author = 'Hassan Abbas' Date='2022-11-14' Version="1.0" Branch="master">Populate Subtypes data</History>
        protected async Task PopulateAttachmentTypes()
        {
            var response = await lookupService.GetAttachmentTypes(0); // for all attachment types
            if (response.IsSuccessStatusCode)
            {
                AttachmentTypes = (List<AttachmentType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        //<History Author = 'Hassan Abbas' Date='2022-11-12' Version="1.0" Branch="master">Populate Case Template Sections and Parameters</History>
        protected async Task PopulateTemplateSections()
        {
            if (DocumentTemplateId > 2)
            {
                var secResponse = await cmsCaseTemplateService.GetTemplateSections(DocumentTemplateId);
                if (secResponse.IsSuccessStatusCode)
                {
                    draftedTemplate.TemplateSections = (List<CaseTemplateSectionsVM>)secResponse.ResultData;
                    foreach (var section in draftedTemplate.TemplateSections)
                    {
                        var parResponse = await cmsCaseTemplateService.GetTemplateSectionParameters(section.Id);
                        if (parResponse.IsSuccessStatusCode)
                        {
                            section.SectionParameters = (List<CaseTemplateParametersVM>)parResponse.ResultData;
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(parResponse);
                        }
                    }
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(secResponse);
                }
            }
            StateHasChanged();
        }

        //<History Author = 'Hassan Abbas' Date='2022-11-10' Version="1.0" Branch="master">Populate Case Template Parameters</History>
        protected async Task PopulateTemplateContent()
        {
            if (DocumentTemplateId > 0)
            {
                var response = await cmsCaseTemplateService.GetCaseTemplateContent(DocumentTemplateId, null, null);
                if (response.IsSuccessStatusCode)
                {
                    Template = (CaseTemplate)response.ResultData;
                    Template.Content = Template.Content.Replace("[----", "<span class=\"temp-read-only-content\" contenteditable=\"false\" style=\"display: inline;\">");
                    Template.Content = Template.Content.Replace("----]", "</span>");
                    await ReplaceTemplateLabelsWithValues();
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            StateHasChanged();
        }
        protected async Task PopulateTemplateContent(string? VersionId)
        {
            var response = await cmsCaseTemplateService.GetCaseTemplateContent(DocumentTemplateId, Guid.Parse(DraftId), Guid.Parse(VersionId));
            if (response.IsSuccessStatusCode)
            {
                Template = (CaseTemplate)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }

            StateHasChanged();
        }

        //<History Author = 'Hassan Abbas' Date='2022-11-11' Version="1.0" Branch="master">Populate Case Template Parameters</History>
        protected async Task PopulatePartiesFromCaseFile()
        {
            ApiCallResponse partyResponse;
            if (DraftId == null)
            {
                partyResponse = await caseRequestService.GetCMSCasePartyDetailById(Guid.Parse(ReferenceId));
            }
            else
            {
                partyResponse = await caseRequestService.GetCMSCasePartyDetailById(draftedTemplate.ReferenceId);
            }
            if (partyResponse.IsSuccessStatusCode)
            {
                List<CasePartyLinkVM> CasePartyLinks = (List<CasePartyLinkVM>)partyResponse.ResultData;
                List<CasePartyLinkVM> CasePartyPlaintiffs = CasePartyLinks?.Where(p => p.CategoryId == (int)CasePartyCategoryEnum.Plaintiff).ToList();
                List<CasePartyLinkVM> CasePartyDefendants = CasePartyLinks?.Where(p => p.CategoryId == (int)CasePartyCategoryEnum.Defendant).ToList();
                if ((bool)CasePartyPlaintiffs?.Any())
                {
                    var length = CasePartyPlaintiffs.Count();
                    for (int i = 0; i < length; i++)
                    {
                        var seperator = i + 1 == length ? "" : ", ";
                        Plaintiffs += (CasePartyPlaintiffs[i].TypeId == (int)CasePartyTypeEnum.GovernmentEntity ? Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? CasePartyPlaintiffs[i].RepresentativeEn : CasePartyPlaintiffs[i].RepresentativeAr : CasePartyPlaintiffs[i].Name) + seperator;
                    }
                }
                if ((bool)CasePartyDefendants?.Any())
                {
                    var length = CasePartyDefendants.Count();
                    for (int i = 0; i < length; i++)
                    {
                        var seperator = i + 1 == length ? "" : ", ";
                        Defendants += (CasePartyDefendants[i].TypeId == (int)CasePartyTypeEnum.GovernmentEntity ? Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? CasePartyDefendants[i].RepresentativeEn : CasePartyDefendants[i].RepresentativeAr : CasePartyDefendants[i].Name) + seperator;
                    }
                }

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(partyResponse);
            }
            StateHasChanged();
        }

        //<History Author = 'Hassan Abbas' Date='2022-11-10' Version="1.0" Branch="master">Populate Lawyers from Under filing</History>
        protected async Task PopulateLawyersFromCaseFile()
        {
            ApiCallResponse partyResponse;

            if (DraftId == null)
            {
                partyResponse = await cmsCaseFileService.GetCaseAssigeeList(Guid.Parse(ReferenceId));
            }
            else
            {
                partyResponse = await cmsCaseFileService.GetCaseAssigeeList(draftedTemplate.ReferenceId);
            }
            if (partyResponse.IsSuccessStatusCode)
            {
                var fileLawyers = (List<CmsCaseAssigneeVM>)partyResponse.ResultData;
                if ((bool)fileLawyers?.Any())
                {
                    var length = fileLawyers.Count();
                    for (int i = 0; i < length; i++)
                    {
                        var seperator = i + 1 == length ? "" : ", ";
                        Lawyers += fileLawyers[i].LawyerNameAr + seperator;
                    }
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(partyResponse);
            }
            StateHasChanged();
        }

        protected async Task OnChangeTab(int index)
        {
        }

        //<History Author = 'Hassan Abbas' Date='2023-03-10' Version="1.0" Branch="master">Populate Task Detail to be Approved or Rejected</History>
        protected async Task PopulateTaskDetails()
        {
            if (TaskId != null)
            {
                var getTaskDetail = await taskService.GetTaskDetailById(Guid.Parse(TaskId));
                if (getTaskDetail.IsSuccessStatusCode)
                {
                    taskDetailVM = (TaskDetailVM)getTaskDetail.ResultData;
                }
                else
                {
                    taskDetailVM = new TaskDetailVM();
                }
            }
        }

        //<History Author = 'Hassan Abbas' Date='2023-03-10' Version="1.0" Branch="master">Populate Task Detail to be Approved or Rejected</History>
        protected async Task PopulateParentEntityDetails()
        {
            if (ReferenceId != null)
            {
                var response = await cmsCaseTemplateService.GetDraftParentEntityDetails(Guid.Parse(ReferenceId));
                if (response.IsSuccessStatusCode)
                {
                    draftEntityDetail = (CmsDraftedDocumentParentEntityDetailVM)response.ResultData;
                }
            }
        }

        protected async Task PopulateHeaderFooter()
        {
            var response = await cmsCaseTemplateService.GetHeaderFooter();
            if (response.IsSuccessStatusCode)
            {
                HeaderFooterTemplates = (List<CaseTemplate>)response.ResultData;
            }
        }

        private async Task PopulateConsultationContractTemplateDetailByUsingFileId(Guid guid)
        {
            try
            {
                var response = await consultationFileService.GetConsultationFileDetailById(guid);
                if (response.IsSuccessStatusCode)
                {
                    consultationFileDetailVM = (ConsultationFileDetailVM)response.ResultData;
                    draftedTemplate.TemplateId = (int)consultationFileDetailVM.TemplateId;
                }
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-11-14' Version="1.0" Branch="master">Populate Subtypes data</History>
        protected async Task PopulateGovtEntityRepresentatives()
        {
            if(dataCommunicationService.draftEntityData.DraftEntityType == (int)DraftEntityTypeEnum.RequestNeedMoreInfo || dataCommunicationService.draftEntityData.DraftEntityType == (int)DraftEntityTypeEnum.FileNeedMoreInfo || dataCommunicationService.draftEntityData.DraftEntityType == (int)DraftEntityTypeEnum.CaseNeedMoreInfo)
            {
                SendCommunicationVM sendCommunication = JsonConvert.DeserializeObject<SendCommunicationVM>(dataCommunicationService.draftEntityData.Payload);
                var geList = sendCommunication.CommunicationResponse.EntityIds.Concat(sendCommunication.CommunicationResponse.PartyEntityIds).ToList();
                var response = await lookupService.GetGovernmentEntityRepresentatives(new GovtEntityIdsPayload { EntityIds = geList });
                if (response.IsSuccessStatusCode)
                {
                    geRepresentatives = (GovtEntityRepresentativeNamesResponseVM)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            StateHasChanged();
        }
        #endregion

        #region Preview Draft
        //< History Author = 'Hassan Abbas' Date = '2022-11-18' Version = "1.0" Branch = "master" >Preview Draft</History>
        protected async Task PreviewDraft()
        {
            try
            {
                busyPreviewBtn = true;
                StateHasChanged();
                if (DraftId == null && Convert.ToInt32(TypeId) == (int)AttachmentTypeEnum.ContractReview)
                {
                   
                        TemplateContent = consultationFileDetailVM.RequestTemplateContent.Replace("text-decoration-line:", "text-decoration:");
                    
                }
                else if (DocumentTemplateId == (int)CaseTemplateEnum.NoTemplate)
                {
                    TemplateContent = Template.Content.Replace("text-decoration-line:", "text-decoration:");
                    if (Thread.CurrentThread.CurrentUICulture.Name == "ar-KW")
                    {
                        TemplateContent = "<div style=\"text-align: right;\">" + Template.Content.Replace("text-decoration-line:", "text-decoration:") + "</div>";
                    }
                }
                else
                {
                    TemplateContent = Template.Content.Replace("text-decoration-line:", "text-decoration:");
					//TemplateContent = "<div style=\"font-family:'Sultan'; src: url('../fonts/Sultan/arfonts-sultan-normal.ttf') format('opentype');\">" + Template.Content.Replace("text-decoration-line:", "text-decoration:") + "</div>";

				}

				string pattern = @"<([^>]+)>|&nbsp;|\s+";
                string cleanString = TemplateContent;
                cleanString = Regex.Replace(cleanString, pattern, string.Empty);

                if (String.IsNullOrWhiteSpace(cleanString))
                {
                    busyPreviewBtn = false;
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Fill_Draft_Content"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
                }
                isVisible = true;
                await Task.Run(() => PopulatePdfFromHtml());
                busyPreviewBtn = false;
            }
            catch (Exception)
            {
                busyPreviewBtn = false;
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion

        #region Replace Template Labels with Values

        //<History Author = 'Hassan Abbas' Date='2023-08-30' Version="1.0" Branch="master">Replace Tempalte Labels with Values </History>
        protected async Task ReplaceTemplateLabelsWithValues()
        {
            try
            {
                int submoduleId = 0;
                CmsRegisteredCaseDetailVM registeredCaseDetail = new CmsRegisteredCaseDetailVM();
                CmsCaseFileDetailVM caseFileDetail = new CmsCaseFileDetailVM();
                CaseRequestDetailVM caseRequestDetail = new CaseRequestDetailVM();
                if (draftEntityDetail != null && draftEntityDetail.SubmoduleId > 0)
                {
                    if (draftEntityDetail.SubmoduleId == (int)SubModuleEnum.RegisteredCase)
                    {
                        registeredCaseDetail = JsonConvert.DeserializeObject<CmsRegisteredCaseDetailVM>(draftEntityDetail.Payload.ToString());
                        submoduleId = draftEntityDetail.SubmoduleId;
                    }
                    else if (draftEntityDetail.SubmoduleId == (int)SubModuleEnum.CaseFile)
                    {
                        caseFileDetail = JsonConvert.DeserializeObject<CmsCaseFileDetailVM>(draftEntityDetail.Payload.ToString());
                        if (caseFileDetail != null)
                        {
                            caseRequestDetail = caseFileDetail.CaseRequest.FirstOrDefault();
                            submoduleId = draftEntityDetail.SubmoduleId;
                        }
                    }
                }
                foreach (var sec in draftedTemplate.TemplateSections)
                {
                    foreach (var parm in sec.SectionParameters)
                    {
                        if (submoduleId == (int)SubModuleEnum.RegisteredCase)
                        {
                            if (parm.PKey == CaseTemplateParamsEnum.CmsTempCaseNumber.ToString())
                            {
                                Template.Content = Template.Content.Replace('#' + parm.PKey + '#', "<span class=\"temp-label-key\" contenteditable=\"false\">" + registeredCaseDetail?.CaseNumber + "</span>");
                                parm.Value = registeredCaseDetail?.CaseNumber;
                            }
                            if (parm.PKey == CaseTemplateParamsEnum.CmsTempChamberName.ToString())
                            {
                                Template.Content = Template.Content.Replace('#' + parm.PKey + '#', "<span class=\"temp-label-key\" contenteditable=\"false\">" + registeredCaseDetail?.ChamberNameAr + "</span>");
                                parm.Value = registeredCaseDetail?.ChamberNameAr;
                            }
                            if (parm.PKey == CaseTemplateParamsEnum.CmsTempCourtName.ToString())
                            {
                                Template.Content = Template.Content.Replace('#' + parm.PKey + '#', "<span class=\"temp-label-key\" contenteditable=\"false\">" + registeredCaseDetail?.CourtNameAr + "</span>");
                                parm.Value = registeredCaseDetail?.CourtNameAr;
                            }
                            if (parm.PKey == CaseTemplateParamsEnum.CmsTempChamberNumber.ToString())
                            {
                                Template.Content = Template.Content.Replace('#' + parm.PKey + '#', "<span class=\"temp-label-key\" contenteditable=\"false\">" + registeredCaseDetail?.ChamberNumber + "</span>");
                                parm.Value = registeredCaseDetail?.ChamberNumber;
                            }
                            if (parm.PKey == CaseTemplateParamsEnum.CmsTempHearingDate.ToString())
                            {
                                Template.Content = Template.Content.Replace('#' + parm.PKey + '#', "<span class=\"temp-label-key\" contenteditable=\"false\">" + DateTime.Now.ToString("dd/MM/yyyy") + "</span>");
                                parm.Value = DateTime.Now.ToString("dd/MM/yyyy");
                            }
                        }
                        if (parm.PKey == CaseTemplateParamsEnum.CmsTempOutboxNumber.ToString())
                        {
                            string outboxNumber = string.Empty;
                            CmsDraftedTemplate items = new CmsDraftedTemplate();
                            items.CreatedBy = await BrowserStorage.GetItemAsync<string>("User");

                            var response = await cmsCaseTemplateService.GenerateNumberPattern(0,(int)CmsComsNumPatternTypeEnum.OutboxNumber);
                            if (response.IsSuccessStatusCode)
                            {
                                var result = (NumberPatternResult)response.ResultData;
                                outboxNumber = result.GenerateRequestNumber;
                            }
                            Template.Content = Template.Content.Replace('#' + parm.PKey + '#', "<span class=\"temp-label-key\" contenteditable=\"false\">" + outboxNumber + "</span>");
                            parm.Value = outboxNumber;
                        }
                        if (parm.PKey == CaseTemplateParamsEnum.CmsTempRepresentativeName.ToString())
                        {
                            Template.Content = Template.Content.Replace('#' + parm.PKey + '#', "<span class=\"temp-label-key\" contenteditable=\"false\">" + geRepresentatives?.RepresentativeNameAr + "</span>");
                            parm.Value = geRepresentatives?.RepresentativeNameAr;
                        }
                        if (parm.PKey == CaseTemplateParamsEnum.CmsTempGovernmentEntityName.ToString())
                        {
                            Template.Content = Template.Content.Replace('#' + parm.PKey + '#', "<span class=\"temp-label-key\" contenteditable=\"false\">" + geRepresentatives?.GovtEntityNameAr + "</span>");
                            parm.Value = geRepresentatives?.GovtEntityNameAr;
                        }
                        if (parm.PKey == CaseTemplateParamsEnum.CmsTempOutboxDate.ToString())
                        {
                            Template.Content = Template.Content.Replace('#' + parm.PKey + '#', "<span class=\"temp-label-key\" contenteditable=\"false\">" + DateTime.Now.ToString("dd/MM/yyyy") + "</span>");
                            parm.Value = DateTime.Now.ToString("dd/MM/yyyy");
                        }
                        if (parm.PKey == CaseTemplateParamsEnum.CmsTempPlaintiffName.ToString())
                        {
                            Template.Content = Template.Content.Replace('#' + parm.PKey + '#', "<span class=\"temp-label-key\" contenteditable=\"false\">" + Plaintiffs + "</span>");
                            parm.Value = Plaintiffs;
                        }
                        else if (parm.PKey == CaseTemplateParamsEnum.CmsTempDefendantName.ToString())
                        {
                            Template.Content = Template.Content.Replace('#' + parm.PKey + '#', "<span class=\"temp-label-key\" contenteditable=\"false\">" + Defendants + "</span>");
                            parm.Value = Defendants;
                        }
                        else if (parm.PKey == CaseTemplateParamsEnum.CmsTempLawywerName.ToString())
                        {
                            Template.Content = Template.Content.Replace('#' + parm.PKey + '#', "<span class=\"temp-label-key\" contenteditable=\"false\">" + Lawyers + "</span>");
                            parm.Value = Lawyers;
                        }
                        else
                        {
                            Template.Content = Template.Content.Replace('#' + parm.PKey + '#', "<span class=\"temp-label-key\" contenteditable=\"false\">" + parm.Value + "</span>");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Document Previewer

        //<History Author = 'Hassan Abbas' Date='2022-11-18' Version="1.0" Branch="master">Open advance search popup </History>
        protected async Task TogglePreviewWindow(bool? value)
        {
            if (value != null)
                isVisible = (bool)value;
            else
                isVisible = !isVisible;
        }


        //<History Author = 'Hassan Abbas' Date='2022-11-18' Version="1.0" Branch="master">Populate Pdf from Html</History>
        //<History Author = 'Hassan Abbas' Date='2024-01-01' Version="1.0" Branch="master">Modified the function to add header footers on each page</History>
        protected void PopulatePdfFromHtml()
        {
            try
            {
                HtmlToPdf converter = new HtmlToPdf();
                MemoryStream stream = new MemoryStream();

                // set converter options
                if (Convert.ToInt32(TypeId) != (int)AttachmentTypeEnum.ContractReview)
                {
                    converter.Options.DisplayHeader = true;
                    converter.Options.DisplayFooter = true;
                    converter.Header.DisplayOnFirstPage = true;
                    converter.Header.DisplayOnOddPages = true;
                    converter.Header.DisplayOnEvenPages = true;
                    converter.Header.Height = 100;
                    converter.Footer.Height = 50;
                    converter.Footer.DisplayOnFirstPage = true;
                    converter.Footer.DisplayOnOddPages = true;
                    converter.Footer.DisplayOnEvenPages = true;
                    string headerHtmlContent = HeaderFooterTemplates.Where(x => x.Id == (Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? (int)CaseTemplateEnum.HeaderEn : (int)CaseTemplateEnum.HeaderAr)).Select(x => x.Content).FirstOrDefault();
                    string footerHtmlContent = HeaderFooterTemplates.Where(x => x.Id == (int)CaseTemplateEnum.Footer).Select(x => x.Content).FirstOrDefault();
                    PdfHtmlSection headerHtml = new PdfHtmlSection(headerHtmlContent, "");
                    PdfHtmlSection footerHtml = new PdfHtmlSection(footerHtmlContent, "");
                    headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                    footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                    converter.Header.Add(headerHtml);
                    converter.Footer.Add(footerHtml);
                }
                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                converter.Options.WebPageWidth = 1024;
                converter.Options.WebPageHeight = 1024;
                converter.Options.MarginRight = 30;
                converter.Options.MarginLeft = 30;
                converter.Options.MarginRight = 30;
                converter.Options.MarginLeft = 30;
                converter.Options.EmbedFonts = true;
                TemplateContent = string.Concat($"<style>@font-face {{font-family: 'Sultan';src: url('{Directory.GetCurrentDirectory().Replace("\\", "/") + "/wwwroot/fonts/Sultan/arfonts-sultan-normal.ttf"}') format('truetype');font-weight: normal;font-style: normal;font-display: swap;}} @font-face {{font-family: 'Sultan Medium';src: url('{Directory.GetCurrentDirectory().Replace("\\", "/") + "/wwwroot/fonts/Sultan/arfonts-sultan-mudaim.ttf"}') format('truetype');font-weight: normal;font-style: normal;font-display: swap;}}</style>", TemplateContent);
                PdfDocument pdfDocument = converter.ConvertHtmlString(TemplateContent);  
                pdfDocument.Save(stream);
                pdfDocument.Close();
                stream.Close();
                FileData = stream.ToArray();
                string base64String = Convert.ToBase64String(FileData);
                DocumentPath = "data:application/pdf;base64," + base64String;
                //StateHasChanged();
            }
            catch (Exception)
            {
                busyPreviewBtn = false;
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
         
        #endregion

        #region Submit Draft
        //<History Author = 'Hassan Abbas' Date='2022-11-25' Version="1.0" Branch="master">Submit Drafted Document</History>
        protected async Task SubmitDraft(MouseEventArgs args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Sure_Submit_Draft"), translationState.Translate("Submit_Draft"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    ApiCallResponse response = new ApiCallResponse();
                    draftedTemplate.userName = await BrowserStorage.GetItemAsync<string>("User");
                    draftedTemplate.subModuleId = await GetSubmoduleId();
                    draftedTemplate.UserId = Guid.Parse(loginState.UserDetail.UserId);
                    var response1 = await workflowService.GetActiveWorkflows(DocumentModuleId,
                       DocumentModuleId == (int)WorkflowModuleEnum.CaseManagement ?
                       (int)WorkflowModuleTriggerEnum.UserSubmitsCaseDraft :
                       (int)WorkflowModuleTriggerEnum.UserSubmitsConsultationDraft, DocumentTypeId, draftedTemplate.subModuleId);
                    if (response1.IsSuccessStatusCode)
                    {
                        var activeworkflowlist = (List<WorkflowVM>)response1.ResultData;
                        if (activeworkflowlist?.Count() > 0)
                        {
                            spinnerService.Show();

                            if (DraftId == null && Convert.ToInt32(TypeId) == (int)AttachmentTypeEnum.ContractReview)
                            {
                                draftedTemplate.DraftedTemplateVersion.Content = consultationFileDetailVM.RequestTemplateContent.Replace("text-decoration-line: underline", "text-decoration: underline");
                            }
                            else
                            {
                                draftedTemplate.DraftedTemplateVersion.Content = Template.Content.Replace("text-decoration-line: underline", "text-decoration: underline");
                            }
                            if (DraftId == null)
                            {
                                draftedTemplate.DraftedTemplateVersion.StatusId = (int)DraftVersionStatusEnum.InReview;
                                draftedTemplate.Payload = dataCommunicationService.draftEntityData.Payload;
                                draftedTemplate.DraftEntityType = dataCommunicationService.draftEntityData.DraftEntityType;
                                draftedTemplate.SectorTypeId = (int)loginState.UserDetail.SectorTypeId;
                                draftedTemplate.FileData = FileData;
                                draftedTemplate.ModuleId = DocumentModuleId;
                                draftedTemplate.DraftedTemplateVersion.DraftActionId = (int)DraftActionIdEnum.Submitted;
                                response = await cmsCaseTemplateService.CreateCaseDraftDocument(draftedTemplate);
                            }
                            else
                            {
                                if (draftedTemplate.DraftedTemplateVersion.StatusId == (int)DraftVersionStatusEnum.Draft)
                                {
                                    if (draftedTemplate.CreatedBy == loginState.Username)
                                    {
                                        IsDraftSubmit = true;
                                        draftedTemplate.DraftedTemplateVersion.StatusId = (int)DraftVersionStatusEnum.InReview;
                                    }
                                    else
                                    {
                                        if (loginState.UserRoles.Any(u => u.RoleId != SystemRoles.Lawyer) && loginState.UserRoles.Any(u => u.RoleId != SystemRoles.ComsLawyer))
                                        {
                                            draftedTemplate.DraftedTemplateVersion.StatusId = (int)DraftVersionStatusEnum.Approve;
                                        }
                                        else
                                        {
                                            IsDraftSubmit = true;
                                            draftedTemplate.DraftedTemplateVersion.StatusId = (int)DraftVersionStatusEnum.InReview;
                                        }
                                    }
                                    draftedTemplate.DraftedTemplateVersion.DraftActionId = (int)DraftActionIdEnum.Submitted;
                                    response = await cmsCaseTemplateService.UpdateCaseDraftDocument(draftedTemplate);
                                }
                                else
                                {
                                    if (draftedTemplate.CreatedBy == loginState.Username)
                                    {
                                        draftedTemplate.DraftedTemplateVersion.StatusId = (int)DraftVersionStatusEnum.InReview;
                                    }
                                    else
                                    {
                                        draftedTemplate.DraftedTemplateVersion.StatusId = (int)DraftVersionStatusEnum.Approve;
                                    }
                                    if (!string.IsNullOrEmpty(draftedTemplate.DraftedTemplateVersion.ReviewerRoleId))
                                    {
                                        taskDetailVM.IsMultipleTaskUpdateForSameEntity = true;
                                    }
                                    draftedTemplate.DraftedTemplateVersion.VersionId = Guid.NewGuid();
                                    draftedTemplate.DraftedTemplateVersion.ReviewerRoleId = "";
                                    draftedTemplate.DraftedTemplateVersion.ReviewerUserId = "";
                                    draftedTemplate.FileData = FileData;
                                    draftedTemplate.UploadFrom = DocumentModuleId == (int)WorkflowModuleEnum.CaseManagement ? "CaseManagement" : "COMSConsultationManagement";
                                    draftedTemplate.Project = "FATWA_WEB";
                                    draftedTemplate.DraftedTemplateVersion.VersionNumber = Decimal.Add(draftedTemplate.DraftedTemplateVersion.VersionNumber, .1m);
                                    draftedTemplate.DraftedTemplateVersion.CreatedBy = loginState.Username;
                                    draftedTemplate.DraftedTemplateVersion.CreatedDate = DateTime.Now;
                                    draftedTemplate.DraftedTemplateVersion.ModifiedBy = null;
                                    draftedTemplate.DraftedTemplateVersion.ModifiedDate = null;
                                    draftedTemplate.DraftedTemplateVersion.DraftActionId = (int)DraftActionIdEnum.EditedAndSubmitted;
                                    response = await cmsCaseTemplateService.CreateDraftDocumentVersion(draftedTemplate);
                                }
                            }
                            if (response.IsSuccessStatusCode)
                            {
                                draftedTemplate = (CmsDraftedTemplate)response.ResultData;
                                isVisible = false;
                                StateHasChanged();

                                var respone = await workflowService.GetWorkflowConditionOptions(draftedTemplate.Id, draftedTemplate.DraftedTemplateVersion.StatusId);
                                if (respone.IsSuccessStatusCode)
                                {
                                    conditionOptions = (List<WorkflowConditionsOptionVM>)respone.ResultData;
                                }
                                else
                                {
                                    await invalidRequestHandlerService.ReturnBadRequestNotification(respone);
                                }

                                if (DraftId == null || IsDraftSubmit)
                                {
                                    var workflowTriggerConditions = await workflowService.GetWorkflowTriggerConditions((int)activeworkflowlist.FirstOrDefault().WorkflowTriggerId);
                                    if (workflowTriggerConditions.Any())
                                    {
                                        foreach (var condition in workflowTriggerConditions)
                                        {
                                            if (loginState.UserRoles.Any(r => r.RoleId == condition.ValueToCompare))
                                            {
                                                var triggerConditionsOptions = await workflowService.GetWorkflowTriggerConditionsOptions((int)condition.TriggerConditionId, draftedTemplate.Id);
                                                if (triggerConditionsOptions.Any())
                                                {
                                                    var result = await dialogService.OpenAsync<SelectTriggerConditionOptionPopup>(translationState.Translate("Select_Option"),
                                                    new Dictionary<string, object>()
                                                        {
                                                            { "Options", triggerConditionsOptions},
                                                            { "isActivity", false}
                                                        },
                                                    new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
                                                    if (result != null)
                                                    {
                                                        var SelectedOptionId = (int)result;
                                                        var selectedConditionOption = triggerConditionsOptions.Where(x => x.ModuleOptionId == SelectedOptionId).FirstOrDefault();
                                                        await workflowService.ProcessWorkflowOptionActivites(selectedConditionOption, draftedTemplate, draftedTemplate.ModuleId, draftedTemplate.ModuleId == (int)WorkflowModuleEnum.CaseManagement ? (int)WorkflowModuleTriggerEnum.UserSubmitsCaseDraft : (int)WorkflowModuleTriggerEnum.UserSubmitsConsultationDraft, true);
                                                    }
                                                    else
                                                    {
                                                        dialogService.Close(null);
                                                        spinnerService.Hide();
                                                        await RedirectToDetail();
                                                        return;
                                                    }
                                                    break;
                                                }
                                                else
                                                {
                                                    await workflowService.AssignWorkflowActivity(activeworkflowlist.FirstOrDefault(), draftedTemplate, DocumentModuleId, DocumentModuleId == (int)WorkflowModuleEnum.CaseManagement ? (int)WorkflowModuleTriggerEnum.UserSubmitsCaseDraft : (int)WorkflowModuleTriggerEnum.UserSubmitsConsultationDraft, loginState.UserRoles);
                                                    await Task.Delay(1500);
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        await workflowService.AssignWorkflowActivity(activeworkflowlist.FirstOrDefault(), draftedTemplate, DocumentModuleId, DocumentModuleId == (int)WorkflowModuleEnum.CaseManagement ? (int)WorkflowModuleTriggerEnum.UserSubmitsCaseDraft : (int)WorkflowModuleTriggerEnum.UserSubmitsConsultationDraft, loginState.UserRoles);
                                        await Task.Delay(1500);
                                    }
                                }

                                else if (conditionOptions.Count > 0)
                                {
                                    var result = await dialogService.OpenAsync<SelectConditionOptionPopup>(translationState.Translate("Select_Option"),
                                    new Dictionary<string, object>()
                                        {
                                    { "Options", conditionOptions},
                                    { "isActivity", false}
                                        },
                                        new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
                                    var SelectedOptionId = (int)result;
                                    if (SelectedOptionId > 0)
                                    {
                                        var selecetdConditionOption = conditionOptions.Where(x => x.ModuleOptionId == SelectedOptionId).FirstOrDefault();
                                        await workflowService.ProcessWorkflowOptionActivites(selecetdConditionOption, draftedTemplate, draftedTemplate.ModuleId, draftedTemplate.ModuleId == (int)WorkflowModuleEnum.CaseManagement ? (int)WorkflowModuleTriggerEnum.UserSubmitsCaseDraft : (int)WorkflowModuleTriggerEnum.UserSubmitsConsultationDraft);
                                    }
                                    else
                                        return;
                                }
                                else
                                {
                                    await workflowService.ProcessWorkflowActvivities(draftedTemplate, DocumentModuleId, DocumentModuleId == (int)WorkflowModuleEnum.CaseManagement ?
                                    (int)WorkflowModuleTriggerEnum.UserSubmitsCaseDraft :
                                    (int)WorkflowModuleTriggerEnum.UserSubmitsConsultationDraft);
                                }
                                if (TaskId != null)
                                {
                                    taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
                                    if (VersionId != null)
                                    {
                                        taskDetailVM.VersionId = Guid.Parse(VersionId);
                                    }
                                    taskDetailVM.SectorId = loginState.UserDetail.SectorTypeId;
                                    var taskResponse = await taskService.DecisionTask(taskDetailVM);
                                    if (!taskResponse.IsSuccessStatusCode)
                                    {
                                        await invalidRequestHandlerService.ReturnBadRequestNotification(taskResponse);
                                    }
                                    navigationManager.NavigateTo("/usertask-list", true);
                                }

                                if (CreateAnother)
                                {
                                    spinnerService.Hide();
                                    notificationService.Notify(new NotificationMessage()
                                    {
                                        Severity = NotificationSeverity.Success,
                                        Detail = translationState.Translate("Case_Draft_Created_Successfully"),
                                        Style = "position: fixed !important; left: 0; margin: auto; "
                                    });
                                    await Task.Delay(1500);
                                    if (DocumentTypeId == (int)AttachmentTypeEnum.CmsLegalNotification)
                                    {
                                        navigationManager.NavigateTo("/Request-For-More-Information/" + ReferenceId + "/" + false);

                                    }
                                    else if (DocumentTypeId == (int)AttachmentTypeEnum.ComsLegalNotification)
                                    {
                                        navigationManager.NavigateTo("/Coms-Request-For-More-Information/" + ReferenceId + "/" + false);

                                    }

                             

                                     if (dataCommunicationService?.draftEntityData?.Payload != null && DocumentModuleId == (int)WorkflowModuleEnum.COMSConsultationManagement)
                                    {
                                        navigationManager.NavigateTo("/Coms-Request-For-More-Information/" + ReferenceId + "/" + false);
                                    }
                                    else
                                    {
                                        //Reload();o
                                        navigationManager.NavigateTo("/create-filedraft/" +ReferenceId + "/" + DocumentTypeId + "/" + DocumentTemplateId + "/" + DocumentModuleId,true );

                                    }
                                }
                                else
                                {
                                    spinnerService.Hide();
                                    notificationService.Notify(new NotificationMessage()
                                    {
                                        Severity = NotificationSeverity.Success,
                                        Detail = translationState.Translate("Case_Draft_Created_Successfully"),
                                        Style = "position: fixed !important; left: 0; margin: auto; "
                                    });
                                    await Task.Delay(1500);
                                    await RedirectToDetail();
                                }
                            }
                            else
                            {
                                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                            }
                        }
                        else
                        {
                            isVisible = false;
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("No_Active_Workflow"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        protected async Task<int> GetSubmoduleId()
        {
            try
            {
                draftedTemplate.subModuleId = 0;
                if (loginState.UserDetail.SectorTypeId >= (int)OperatingSectorTypeEnum.AdministrativeUnderFilingCases && loginState.UserDetail.SectorTypeId <= (int)OperatingSectorTypeEnum.AdministrativeSupremeCases)
                {
                    draftedTemplate.subModuleId = (int)RequestTypeEnum.Administrative;
                }
                else if (loginState.UserDetail.SectorTypeId >= (int)OperatingSectorTypeEnum.CivilCommercialUnderFilingCases && loginState.UserDetail.SectorTypeId <= (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases)
                {
                    draftedTemplate.subModuleId = (int)RequestTypeEnum.CivilCommercial;
                }
                else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.LegalAdvice)
                {
                    draftedTemplate.subModuleId = (int)WorkflowSubModuleEnum.LegalAdvice;
                }
                else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.Legislations)
                {
                    draftedTemplate.subModuleId = (int)WorkflowSubModuleEnum.Legislations;
                }
                else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeComplaints)
                {
                    draftedTemplate.subModuleId = (int)WorkflowSubModuleEnum.AdministrativeComplaints;
                }
                else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.Contracts)
                {
                    draftedTemplate.subModuleId = (int)WorkflowSubModuleEnum.Contracts;
                }
                else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.InternationalArbitration)
                {
                    draftedTemplate.subModuleId = (int)WorkflowSubModuleEnum.InternationalArbitration;
                }
                return draftedTemplate.subModuleId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Save Draft As Draft
        //<History Author = 'Hassan Abbas' Date='2022-11-25' Version="1.0" Branch="master">Submit Drafted Document</History>
        protected async Task SaveDraftAsDraft(MouseEventArgs args)
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Save_Draft"), translationState.Translate("Save_Draft"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                ApiCallResponse response = new ApiCallResponse();
                draftedTemplate.userName = await BrowserStorage.GetItemAsync<string>("User");
                draftedTemplate.UserId = Guid.Parse(loginState.UserDetail.UserId);
                spinnerService.Show();
                if (DraftId == null && Convert.ToInt32(TypeId) == (int)AttachmentTypeEnum.ContractReview)
                {
                    draftedTemplate.DraftedTemplateVersion.Content = consultationFileDetailVM.RequestTemplateContent.Replace("text-decoration-line: underline", "text-decoration: underline");
                }
                else
                {
                    draftedTemplate.DraftedTemplateVersion.Content = Template.Content.Replace("text-decoration-line: underline", "text-decoration: underline");
                }

                if (DraftId == null)
                {
                    draftedTemplate.DraftedTemplateVersion.StatusId = (int)DraftVersionStatusEnum.Draft;
                    draftedTemplate.Payload = dataCommunicationService.draftEntityData.Payload;
                    draftedTemplate.DraftEntityType = dataCommunicationService.draftEntityData.DraftEntityType;
                    draftedTemplate.SectorTypeId = (int)loginState.UserDetail.SectorTypeId;
                    draftedTemplate.ModuleId = DocumentModuleId;
                    draftedTemplate.DraftedTemplateVersion.DraftActionId = (int)DraftActionIdEnum.CreatedAndDraft;
                    response = await cmsCaseTemplateService.CreateCaseDraftDocument(draftedTemplate);
                }
                else
                {
                    if (draftedTemplate.DraftedTemplateVersion.CreatedBy == loginState.Username)
                    {
                        if (draftedTemplate.DraftedTemplateVersion.StatusId == (int)DraftVersionStatusEnum.Reject)
                        {
                            draftedTemplate.DraftedTemplateVersion.CreatedBy = loginState.Username;
                            draftedTemplate.DraftedTemplateVersion.VersionId = Guid.NewGuid();
                            draftedTemplate.DraftedTemplateVersion.DraftedTemplateId = draftedTemplate.Id;
                            draftedTemplate.DraftedTemplateVersion.VersionNumber = Decimal.Add(draftedTemplate.DraftedTemplateVersion.VersionNumber, .1m);
                            draftedTemplate.DraftedTemplateVersion.StatusId = (int)DraftVersionStatusEnum.Draft;
                            draftedTemplate.DraftedTemplateVersion.CreatedDate = DateTime.Now;
                            draftedTemplate.DraftedTemplateVersion.ModifiedBy = null;
                            draftedTemplate.DraftedTemplateVersion.ModifiedDate = null;
                            draftedTemplate.DraftedTemplateVersion.DraftActionId = (int)DraftActionIdEnum.EditedAndDraft;
                            response = await cmsCaseTemplateService.CreateDraftDocumentVersion(draftedTemplate);
                        }
                        else
                        {
                            draftedTemplate.DraftedTemplateVersion.CreatedBy = loginState.Username;
                            draftedTemplate.DraftedTemplateVersion.CreatedDate = DateTime.Now;
                            draftedTemplate.DraftedTemplateVersion.DraftActionId = (int)DraftActionIdEnum.EditedAndDraft;
                            response = await cmsCaseTemplateService.UpdateCaseDraftDocument(draftedTemplate);
                        }
                    }
                    else
                    {
                        if (loginState.UserRoles.Any(u => u.RoleId != SystemRoles.Lawyer) && loginState.UserRoles.Any(u => u.RoleId != SystemRoles.ComsLawyer))
                        {
                            draftedTemplate.DraftedTemplateVersion.CreatedBy = loginState.Username;
                            draftedTemplate.DraftedTemplateVersion.VersionId = Guid.NewGuid();
                            draftedTemplate.DraftedTemplateVersion.DraftedTemplateId = draftedTemplate.Id;
                            draftedTemplate.DraftedTemplateVersion.VersionNumber = Decimal.Add(draftedTemplate.DraftedTemplateVersion.VersionNumber, 0.1M);
                            draftedTemplate.DraftedTemplateVersion.StatusId = (int)DraftVersionStatusEnum.Draft;
                            draftedTemplate.DraftedTemplateVersion.CreatedDate = DateTime.Now;
                            draftedTemplate.DraftedTemplateVersion.Content = draftedTemplate.DraftedTemplateVersion.Content;
                            draftedTemplate.DraftedTemplateVersion.DraftActionId = (int)DraftActionIdEnum.EditedAndDraft;
                            draftedTemplate.DraftedTemplateVersion.ModifiedBy = null;
                            draftedTemplate.DraftedTemplateVersion.ModifiedDate = null;
                            await cmsCaseTemplateService.CreateDraftDocumentVersion(draftedTemplate);

                        }
                        else
                        {
                            draftedTemplate.DraftedTemplateVersion.DraftActionId = (int)DraftActionIdEnum.EditedAndDraft;
                            await cmsCaseTemplateService.UpdateCaseDraftDocument(draftedTemplate);
                        }
                    }
                }
                spinnerService.Hide();
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Success,
                    Detail = translationState.Translate("Case_Draft_Created_Successfully"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                await Task.Delay(1500);
                await RedirectToDetail();

            }
        }

        #endregion

        #region Redirect Function 
        //<History Author = 'Hassan Abbas' Date='2022-11-25' Version="1.0" Branch="master">Redirect to respective page</History>
        protected async Task RedirectToDetail()
        {
            if ((draftedTemplate.DraftEntityType == (int)DraftEntityTypeEnum.RequestNeedMoreInfo || draftedTemplate.DraftEntityType == (int)DraftEntityTypeEnum.FileNeedMoreInfo || draftedTemplate.DraftEntityType == (int)DraftEntityTypeEnum.CaseNeedMoreInfo) && DraftId == null)
            {
                await JsInterop.InvokeVoidAsync("RedirectToSecondLastPage");
            }
            else
            {
                await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
            }
        }
        private async Task RedirectToTaskList()
        {
            navigationManager.NavigateTo("/usertask-list");
        }

        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion

        #region Task Status update
        //protected async Task TaskStatusUpdate()
        //{
        //    if (fileTask != null)
        //    {
        //        fileTask.TaskStatusId = (int)TaskStatusEnum.Done;
        //        var taskResponse = await taskService.DecisionTask(fileTask);
        //        if (!taskResponse.IsSuccessStatusCode)
        //        {
        //            await invalidRequestHandlerService.ReturnBadRequestNotification(taskResponse);
        //        }
        //    }
        //}
        #endregion

        #region Component After Render
        //<History Author = 'Hassan Abbas' Date='2023-09-16' Version="1.0" Branch="master">Restrict Html Editing On Read Only Content</History>
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (!IsEditorInitialized)
            {
                if (editor != null)
                {
                    await JsInterop.InvokeVoidAsync("initializeEditor");
                    IsEditorInitialized = true;
                }
            }
        }
        #endregion

        #region breadcrumb
        protected void PopulateTransationKey()
        {
            if (SectorTypeIdCheck == (int)OperatingSectorTypeEnum.Contracts)
            {
                TransKeyHeader = "Contracts_File";


            }
            else if (SectorTypeIdCheck == (int)OperatingSectorTypeEnum.LegalAdvice)
            {
                TransKeyHeader = "Legal_Advice_File";


            }
            else if (SectorTypeIdCheck == (int)OperatingSectorTypeEnum.InternationalArbitration)
            {
                TransKeyHeader = "International_Arbitration_File";

            }
            else if (SectorTypeIdCheck == (int)OperatingSectorTypeEnum.AdministrativeComplaints)
            {
                TransKeyHeader = "Subject";

            }
            else if (SectorTypeIdCheck == (int)OperatingSectorTypeEnum.Legislations)
            {
                TransKeyHeader = "List_Legislations_File";


            }
        }

        #endregion
    }
}
