using FATWA_DOMAIN.Enums;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_DOMAIN.Models.WorkflowModels;
using FATWA_WEB.Pages.DS;
using HtmlAgilityPack;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Radzen;
using Radzen.Blazor;
using SelectPdf;
using Syncfusion.Blazor.PdfViewerServer;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System.Net;
using System.Reflection.Metadata;
using System.Text;
using Telerik.Blazor.Components;
using Telerik.Blazor.Components.Editor;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.DmsEnums;
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
    //<History Author = 'Hassan Abbas' Date='2022-10-30' Version="1.0" Branch="master">Detail Case Draft Document</History>
    public partial class DetailCaseDraftDocument : ComponentBase
    {
        #region Parameter

        [Parameter]
        public string DraftId { get; set; }
        [Parameter]
        public dynamic? VersionId { get; set; }

        [Parameter]
        public string? TaskId { get; set; }

        #endregion

        #region Variables
        public int? sectorTypeId { get; set; }
        protected TaskDetailVM taskDetailVM = new TaskDetailVM();
        protected CmsDraftedDocumentDetailVM draftedTemplate = new CmsDraftedDocumentDetailVM { Id = Guid.NewGuid() };
        protected List<AttachmentType> AttachmentTypes { get; set; } = new List<AttachmentType>();
        protected List<CaseTemplate> CaseTemplates { get; set; } = new List<CaseTemplate>();
        protected List<CasePartyLinkVM> CasePartyDefendants { get; set; } = new List<CasePartyLinkVM>();
        protected List<CasePartyLinkVM> CasePartyPlaintiffs { get; set; } = new List<CasePartyLinkVM>();
        public List<CmsDraftedDocumentReasonVM> DraftDocumentReasons { get; set; } = new List<CmsDraftedDocumentReasonVM>();
        public List<CmsDraftedDocumentOpioninVM> DraftExpertOpinions { get; set; } = new List<CmsDraftedDocumentOpioninVM>();
        protected CmsCaseFileDetailVM caseFile { get; set; }
        protected CaseRequestDetailVM caseRequest { get; set; }
        protected CmsRegisteredCaseDetailVM registeredCase { get; set; } = new CmsRegisteredCaseDetailVM();
        protected SendCommunicationVM sendCommunication { get; set; } = new SendCommunicationVM();
        protected ConsultationFileDetailVM consultationFile { get; set; }
        protected List<WorkflowConditionsOptionVM> conditionOptions { get; set; } = new List<WorkflowConditionsOptionVM>();
        protected AttachmentType attachmentTypeDetail { get; set; } = new AttachmentType();
        protected List<CaseTemplate> HeaderFooterTemplates { get; set; }
        public List<CmsDraftedDocumentVM> DraftedTemplateVersions { get; set; } = new List<CmsDraftedDocumentVM>();
        protected RadzenDataGrid<CasePartyLinkVM> PartiesGrid;
        protected List<CasePartyLinkVM> CasePartyLinks;
        protected DateTime tempDatetime { get; set; } = DateTime.Now;
        protected string TemplateName { get; set; }
        protected string TemplateContent { get; set; }
        protected string Reason { get; set; }
        protected int NextApprovalWorkflowActivity { get; set; }
        protected string Opinion { get; set; }
        protected string reasonValidationMsg = "";
        protected string opinionValidationMsg = "";
        protected CaseTemplate Template { get; set; }
        protected RadzenSteps? steps;
        protected int selectedIndex = 0;
        public bool isVisible { get; set; }
        public bool isFooterHeaderVisible { get; set; } = true;
        public bool isReasonEntered { get; set; }
        public bool showReasonField { get; set; }
        public bool showOpinionField { get; set; }
        public TelerikPdfViewer PdfViewerRef { get; set; }
        public byte[] FileData { get; set; }
        public MarkupString html { get; set; }
        public string Plaintiffs { get; set; }
        public string Defendants { get; set; }
        public string Value { get; set; }
        public string ActivityNameKey { get; set; }
        protected bool ShowDocumentViewer { get; set; }
        public string DocumentPath { get; set; } = string.Empty;

        public List<IEditorTool> Tools { get; set; } =
            new List<IEditorTool>()
            {
            new EditorButtonGroup(new Bold(), new Italic(), new Underline()),
            new EditorButtonGroup(new AlignLeft(), new AlignCenter(), new AlignRight()),
            new UnorderedList(),
            //new EditorButtonGroup(new CreateLink(), new Unlink(), new InsertImage()),
            new InsertTable(),
            new EditorButtonGroup(new AddRowBefore(), new AddRowAfter(), new MergeCells(), new SplitCell()),
            new Format(),
            new FontSize(),
            new FontFamily()
            };
        public CmsDraftedTemplate draftDocument { get; set; } = new CmsDraftedTemplate();
        protected List<CmsCaseAssigneeVM> fileLawyers { get; set; } = new List<CmsCaseAssigneeVM>();
        public decimal highestVersionNumber { get; set; }
        public bool NoRecordFound = false;
        protected List<ManagerTaskReminderVM> managerTaskReminderData { get; set; } = new List<ManagerTaskReminderVM>();

        #endregion

        #region Component Load
        protected override async Task OnInitializedAsync()
        {
            try
            {
                spinnerService.Show();
                sectorTypeId = loginState.UserDetail.SectorTypeId;
                await PopulateHeaderFooter();
                await PopulateCaseDraftData();

                spinnerService.Hide();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Lookup events

        //<History Author = 'Hassan Abbas' Date='2024-01-01' Version="1.0" Branch="master">Populate Header Footer</History>
        protected async Task PopulateHeaderFooter()
        {
            var response = await cmsCaseTemplateService.GetHeaderFooter();
            if (response.IsSuccessStatusCode)
            {
                HeaderFooterTemplates = (List<CaseTemplate>)response.ResultData;
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Populate Subtypes data</History>
        protected async Task PopulateCaseDraftData()
        {
            bool lang_En = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? true : false;
            ApiCallResponse response;
            if (TaskId == null)
                response = await cmsCaseTemplateService.GetDraftDocDetailWithSectionAndParameters(Guid.Parse(DraftId), Guid.Parse(VersionId));
            else
                response = await cmsCaseTemplateService.GetDraftDocDetailWithSectionAndParameters(Guid.Parse(DraftId), null);
            if (response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
            {
                draftedTemplate = (CmsDraftedDocumentDetailVM)response.ResultData;
                if (draftedTemplate != null)
                {
                    await PopulateDraftVersionGrid();
                    await PopulateDrafDocumentTemplateDetailById();
                    await PreviewDraft();
                    if (TaskId != null)
                    {
                        await PopulateTaskDetails();
                        await GetManagerTaskReminderData();
                    }
                    await PopulateReasonsGrid();
                    if (draftedTemplate.AttachmentTypeId == (int)AttachmentTypeEnum.PresentationNotes)
                    {
                        await PopulateOpinionGrid();

                    }
                    if (draftedTemplate.ModuleId == (int)WorkflowModuleEnum.CaseManagement)
                    {
                        await PopulateCaseFileDetails();
                    }
                    else if (draftedTemplate.ModuleId == (int)WorkflowModuleEnum.COMSConsultationManagement)
                    {
                        await PopulateConsultationFileDetails();
                    };
                    await PopulateAttachmentTypeDetailById();
                    await GetCurrentWorkflowActivityInfo();
                    await GetNextWorkflowActivityInfo();
                    await PopulateLawyersFromCaseFile();
                    await PopulateCasePartyGrid();
                }
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                NoRecordFound = true;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        //<History Author = 'Hassan Abbas' Date='2023-03-10' Version="1.0" Branch="master">Populate Task Detail to be Approved or Rejected</History>
        protected async Task PopulateTaskDetails()
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
        //<History Author = 'Hassan Abbas' Date='2023-03-16' Version="1.0" Branch="master"> Populate Draft Grid</History>
        public async Task PopulateReasonsGrid()
        {
            var response = await cmsCaseTemplateService.GetDraftDocumentReasonsByReferenceId(Guid.Parse(VersionId));
            if (response.IsSuccessStatusCode)
            {
                DraftDocumentReasons = (List<CmsDraftedDocumentReasonVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        public async Task PopulateOpinionGrid()
        {
            var response = await cmsCaseTemplateService.GetDraftDocumentOpinionByReferenceId(Guid.Parse(VersionId));
            if (response.IsSuccessStatusCode)
            {
                DraftExpertOpinions = (List<CmsDraftedDocumentOpioninVM>)response.ResultData;

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        public async Task PopulateAttachmentTypeDetailById()
        {
            var response = await lookupService.GetDocumentTypeById((int)draftedTemplate.AttachmentTypeId);
            if (response.IsSuccessStatusCode)
            {
                attachmentTypeDetail = (AttachmentType)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        //<History Author = 'Hassan Abbas' Date='2023-03-16' Version="1.0" Branch="master"> Populate Case File Grid</History>
        public async Task PopulateCaseFileDetails()
        {
            if (draftedTemplate?.FileId is not null)
            {
                var response = await cmsCaseFileService.GetCaseFileDetailByIdVM(draftedTemplate.ReferenceId);
                if (response.IsSuccessStatusCode)
                {
                    caseFile = (CmsCaseFileDetailVM)response.ResultData;
                    await PopulateCaseRequestDetails(caseFile.RequestId);
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            else if (draftedTemplate?.CaseId is not null)
            {

                var result = await cmsRegisteredCaseService.GetRegisteredCaseDetailByIdVM(draftedTemplate.ReferenceId);

                if (result.IsSuccessStatusCode)
                {
                    registeredCase = (CmsRegisteredCaseDetailVM)result.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(result);
                }
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Populate Request Grid</History>
        protected async Task PopulateCaseRequestDetails(Guid requestId)
        {
            var caseRequestResponse = await caseRequestService.GetCaseRequestDetailById(requestId);
            if (caseRequestResponse.IsSuccessStatusCode)
            {
                caseRequest = JsonConvert.DeserializeObject<CaseRequestDetailVM>(caseRequestResponse.ResultData.ToString());
                //caseRequest = (CaseRequestDetailVM)caseRequestResponse.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(caseRequestResponse);
            }
        }
        public async Task PopulateConsultationFileDetails()
        {
            var result = await consultationFileService.GetConsultationFileDetailById((Guid)draftedTemplate.ConsultationFiletId);

            if (result.IsSuccessStatusCode)
            {
                consultationFile = (ConsultationFileDetailVM)result.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }
        }
        public async Task GetCurrentWorkflowActivityInfo()
        {
            var result = await workflowService.GetInstanceCurrentActivity(draftedTemplate.Id);
            if (result != null)
            {
                draftedTemplate.WorkflowActivityId = result.WorkflowActivityId;
                if (draftedTemplate.WorkflowActivityId > 0)
                {
                    var activity = await workflowService.GetWorkflowActivityBySequenceNumber((int)result.WorkflowId, (int)result.SequenceNumber);
                    ActivityNameKey = activity.AKey;
                }
            }
        }
        protected async Task PopulateLawyersFromCaseFile()
        {
            ApiCallResponse partyResponse;
            partyResponse = await cmsCaseFileService.GetCaseAssigeeList(draftedTemplate.ReferenceId);
            if (partyResponse.IsSuccessStatusCode)
            {
                fileLawyers = (List<CmsCaseAssigneeVM>)partyResponse.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(partyResponse);
            }
            StateHasChanged();
        }
        protected async Task PopulateCasePartyGrid()
        {
            var partyResponse = await caseRequestService.GetCMSCasePartyDetailById(draftedTemplate.ReferenceId);
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
        protected async Task GetNextWorkflowActivityInfo()
        {
            var response = await workflowService.GetNextWorrkflowActivity(draftedTemplate.Id);
            if (response.IsSuccessStatusCode)
            {
                NextApprovalWorkflowActivity = (int)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

        #region Get Case Draft List By Reference Id
        public async Task PopulateDraftVersionGrid()
        {
            var response = await cmsCaseTemplateService.GetCaseDraftListByReferenceId(draftedTemplate.ReferenceId);
            if (response.IsSuccessStatusCode)
            {
                var draftedDocuments = (List<CmsDraftedDocumentVM>)response.ResultData;
                DraftedTemplateVersions = draftedDocuments.Where(x => x.Id == draftedTemplate.Id).ToList();
                highestVersionNumber = DraftedTemplateVersions.Max(x => x.VersionNumber);
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }


        }
        #endregion
        public async Task PopulateDrafDocumentTemplateDetailById()
        {
            var response = await cmsCaseTemplateService.GetDraftedTemplateDetailById(draftedTemplate.Id, (Guid)draftedTemplate.VersionId);
            if (response.IsSuccessStatusCode)
            {
                draftDocument = (CmsDraftedTemplate)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }


        }

        #region Preview Draft

        //<History Author = 'Hassan Abbas' Date='2022-10-30' Version="1.0" Branch="master">Preview Draft</History>
        protected async Task PreviewDraft()
        {
            try
            {
                if (draftedTemplate?.TemplateId == (int)CaseTemplateEnum.BlankTemplate)
                {
                    TemplateContent = "";
                    foreach (var sec in draftedTemplate.TemplateSections?.OrderBy(s => s.SequenceNumber))
                    {
                        foreach (var parm in sec.SectionParameters)
                        {
                            TemplateContent += parm.Value + "</br></br>";
                        }
                    }
                }
                else
                {
                    TemplateContent = draftedTemplate?.Content;
                    foreach (var sec in draftedTemplate.TemplateSections)
                    {
                        foreach (var parm in sec.SectionParameters)
                        {
                            if (parm.PKey == CaseTemplateParamsEnum.CmsTempPlaintiffName.ToString())
                            {
                                TemplateContent = TemplateContent.Replace('#' + parm.PKey + '#', Plaintiffs);
                            }
                            else if (parm.PKey == CaseTemplateParamsEnum.CmsTempDefendantName.ToString())
                            {
                                TemplateContent = TemplateContent.Replace('#' + parm.PKey + '#', Defendants);
                            }
                            else
                            {
                                TemplateContent = TemplateContent.Replace('#' + parm.PKey + '#', parm.Value);
                            }
                        }
                    }
                }
                await Task.Run(() => PopulatePdfFromHtml());
                isVisible = true;
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        #endregion

        #region Document Previewer

        //<History Author = 'Hassan Abbas' Date='2022-09-28' Version="1.0" Branch="master">Open advance search popup </History>
        protected async Task ToggleAdvanceSearch()
        {
            isVisible = !isVisible;
        }


        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Populate Case Template Parameters</History>
        protected async Task PopulatePdfFromHtml()
        {
            try
            {
                HtmlToPdf converter = new HtmlToPdf();
                MemoryStream stream = new MemoryStream();

                // set converter options
                if (draftedTemplate.AttachmentTypeId != (int)AttachmentTypeEnum.ContractReview)
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
                    if (isFooterHeaderVisible)
                    {
                        converter.Header.Add(headerHtml);
                        converter.Footer.Add(footerHtml);
                    }

                }
                converter.Options.PdfPageSize = PdfPageSize.A4;
                converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                converter.Options.WebPageWidth = 1024;
                converter.Options.WebPageHeight = 1024;
                converter.Options.MarginRight = 30;
                converter.Options.MarginLeft = 30;
                converter.Options.EmbedFonts = true;
                TemplateContent = string.Concat($"<style>@font-face {{font-family: 'Sultan';src: url('{Directory.GetCurrentDirectory().Replace("\\", "/") + "/wwwroot/fonts/Sultan/arfonts-sultan-normal.ttf"}') format('truetype');font-weight: normal;font-style: normal;font-display: swap;}} @font-face {{font-family: 'Sultan Medium';src: url('{Directory.GetCurrentDirectory().Replace("\\", "/") + "/wwwroot/fonts/Sultan/arfonts-sultan-mudaim.ttf"}') format('truetype');font-weight: normal;font-style: normal;font-display: swap;}}</style>", TemplateContent);
                // create a new pdf document converting an url
                SelectPdf.PdfDocument pdfDocument = converter.ConvertHtmlString(TemplateContent);
                pdfDocument.Save(stream);
                pdfDocument.Close();
                stream.Close();
                FileData = stream.ToArray();
                string base64String = Convert.ToBase64String(FileData);
                DocumentPath = "data:application/pdf;base64," + base64String;
                //StateHasChanged();

            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        #endregion
        #region Dwonload HtmlContent As Word Document

        //<History Author = 'Ijaz Ahmad' Date='2024-09-24' Version="1.0" Branch="master">Download Template as Word Document</History>
        public async Task DownloadAsWord()
        {
            try
            {
                string xhtmlContent = ConvertToXhtml(TemplateContent);

                using (MemoryStream inputStream = new MemoryStream(Encoding.UTF8.GetBytes(xhtmlContent)))
                {
                    using (WordDocument document = new WordDocument(inputStream, FormatType.Html))
                    {
                        IWSection section = document.LastSection;
                        section.PageSetup.PageSize = PageSize.A4;
                        section.PageSetup.Orientation = PageOrientation.Portrait;
                        section.PageSetup.Margins.Top = 180;
                        section.PageSetup.Margins.Bottom = 50;
                        section.PageSetup.Margins.Left = 20;
                        section.PageSetup.Margins.Right = 20;
                        // Header
                        Header(section);

                        // Footer
                        Footer(section);

                        using (MemoryStream outputFileStream = new MemoryStream())
                        {
                            document.Save(outputFileStream, FormatType.Docx);
                            outputFileStream.Position = 0;

                            await JsInterop.InvokeVoidAsync("downloadHtmlAsWord", "document.docx", outputFileStream.ToArray());
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        private string ConvertToXhtml(string htmlContent)
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.OptionWriteEmptyNodes = true;

            htmlDoc.LoadHtml(htmlContent);

            foreach (var node in htmlDoc.DocumentNode.SelectNodes("//text()"))
            {
                node.InnerHtml = node.InnerHtml.Replace("&#x62A;", "?");
            }

            using (StringWriter writer = new StringWriter())
            {
                htmlDoc.Save(writer);

                return writer.ToString();
            }
        }
        private void Header(IWSection section)
        {
            HeaderFooter header = section.HeadersFooters.Header;
            IWParagraph headerParagraph = header.AddParagraph();
            string imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "fatwaPortfolioHeader.PNG");
            using (FileStream headerImageStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
            {
                WPicture headerPicture = (WPicture)headerParagraph.AppendPicture(headerImageStream);
                headerPicture.Width = 500;
                headerPicture.Height = 100;
                headerParagraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
            }

        }
        private void Footer(IWSection section)
        {
            HeaderFooter footer = section.HeadersFooters.Footer;
            IWParagraph footerParagraph = footer.AddParagraph();
            string footerImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "fatwaPortfolioFooter.PNG");
            using (FileStream footerImageStream = new FileStream(footerImagePath, FileMode.Open, FileAccess.Read))
            {
                WPicture footerPicture = (WPicture)footerParagraph.AppendPicture(footerImageStream);
                footerPicture.Width = 500;
                footerPicture.Height = 50;
                footerParagraph.ParagraphFormat.HorizontalAlignment = HorizontalAlignment.Center;
            }
        }
        #endregion
        #region Redirect Function
        //<History Author = 'Hassan Abbas' Date='2022-02-03' Version="1.0" Branch="master"> Redirect back to previous page from browserbrowser history</History>
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }

        //<History Author = 'Hassan Abbas' Date='2022-02-03' Version="1.0" Branch="master"> Redirect back to previous page from browser history</History>
        protected async Task EditDraft()
        {
            if (applicationState.LockedVersions.Where(x => x.Item1 == draftedTemplate.VersionId && x.Item2 == loginState.Username).Any())
            {
                var lockedVersiondetail = applicationState.LockedVersions.Where(x => x.Item1 == draftedTemplate.VersionId).FirstOrDefault();
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Already_Editing_Draft"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                return;
            }
            else if (applicationState.LockedVersions.Where(x => x.Item1 == draftedTemplate.VersionId).Any())
            {
                var lockedVersiondetail = applicationState.LockedVersions.Where(x => x.Item1 == draftedTemplate.VersionId).FirstOrDefault();
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Version_Locked") + " " + (Thread.CurrentThread.CurrentCulture.Name == "en-US" ? lockedVersiondetail.Item3 : lockedVersiondetail.Item4),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                return;
            }
            else
            {
                applicationState.LockedVersions.Add(new Tuple<Guid, string, string, string>((Guid)draftedTemplate.VersionId, loginState.Username, loginState.UserDetail.FullNameEn, loginState.UserDetail.FullNameAr));
            }
            if (TaskId != null)
            {
                navigationManager.NavigateTo("/create-filedraft/" + draftedTemplate.ReferenceId.ToString() + "/" + draftedTemplate.AttachmentTypeId + "/" + draftedTemplate.TemplateId + "/" + DraftId + "/" + draftedTemplate.VersionId + "/" + TaskId + "/" + draftedTemplate.ModuleId);
            }
            else
            {
                navigationManager.NavigateTo("/create-filedraft/" + draftedTemplate.ReferenceId.ToString() + "/" + draftedTemplate.AttachmentTypeId + "/" + draftedTemplate.TemplateId + "/" + DraftId + "/" + draftedTemplate.VersionId + "/" + draftedTemplate.ModuleId);
            }
        }

        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }

        protected void ButtonViewInformation()
        {
            string url = "";
            if (draftedTemplate?.RequestId is not null)
                url = $"/caserequest-view/{draftedTemplate?.RequestId}";
            if (draftedTemplate?.FileId is not null)
                url = $"/casefile-view/{draftedTemplate?.FileId}";
            if (draftedTemplate?.CaseId is not null)
                url = $"/case-view/{draftedTemplate?.CaseId}";
            if (draftedTemplate?.ConsultationRequestId is not null)
                url = $"/consultationrequest-detail/{draftedTemplate?.ConsultationRequestId}/{sectorTypeId}";
            if (draftedTemplate?.ConsultationFiletId is not null)
                url = $"/consultationfile-view/{draftedTemplate?.ConsultationFiletId}/{sectorTypeId}";
            navigationManager.NavigateTo(url);
        }

        //<History Author = 'Hassan Abbas' Date='2022-01-06' Version="1.0" Branch="master"> Redirect to View Detail page</History>
        protected async Task DetailDraft(CmsDraftedDocumentVM args)
        {
            navigationManager.NavigateTo("draftdocument-detail/" + args.Id + "/" + args.VersionId, true);
        }
        protected async Task ViewItemVersionHistory(CmsDraftedDocumentVM args)
        {
            var dialogResult = await dialogService.OpenAsync<ListDraftTemplateVersionLogs>
                (
                translationState.Translate("Draft_Version_History"),
                new Dictionary<string, object>()
                {
                    { "versionId", args.VersionId.ToString() },
                },
                new DialogOptions() { Width = "60% !important", CloseDialogOnOverlayClick = true, ShowClose = true });
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
        protected async Task ViewCaseFileDetails(Guid? FileId)
        {
            navigationManager.NavigateTo("/casefile-view/" + FileId);
        }
        #endregion

        #region Approve/Reject Document

        //<History Author = 'Hassan Abbas' Date='2024-09-04' Version="1.0" Branch="master">Sign and Approve Document through Workflow</History>
        protected async Task SignDocument()
        {
            try
            {
                bool? dialogResponse = false;
                if (attachmentTypeDetail.IsOpinion == true)
                {
                    if (!showOpinionField)
                        showOpinionField = true;
                    showReasonField = false;
                    if (String.IsNullOrEmpty(Opinion))
                    {
                        opinionValidationMsg = @translationState.Translate("Required_Field");
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Required_Field_Opinion"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        return;
                    }
                }
                spinnerService.Show();
                var response1 = await cmsCaseTemplateService.GetDraftedTemplateDetailById(draftedTemplate.Id, (Guid)draftedTemplate.VersionId);
                if (response1.IsSuccessStatusCode)
                {
                    var document = (CmsDraftedTemplate)response1.ResultData;
                    await PopulatePdfFromHtml(document.DraftedTemplateVersion.Content, document);
                    //Save Draft Template To Document
                    document.Token = await BrowserStorage.GetItemAsync<string>("Token");
                    var docResponse = await fileUploadService.SaveDraftTemplateToDocument(document);
                    if (docResponse != null)
                    {
                        var documentId = (int)docResponse.ResultData;

                        spinnerService.Hide();
                        var dialogResult = await dialogService.OpenAsync<DigitalSignature>(
                         translationState.Translate("Digital_Signature"),
                         new Dictionary<string, object>() {
                            { "DocumentId", documentId },
                            { "AttachmentTypeId", draftedTemplate.AttachmentTypeId },
                            { "StatusId", SigningTaskStatusEnum.UnSigned },
                            { "HideLocalSigning", true },
                         },
                         new DialogOptions() { Width = "32% !important", CloseDialogOnOverlayClick = true });
                        if (dialogResult != null)
                        {
                            string requestStatus = (string)dialogResult.RequestStatus;
                            if (requestStatus == SigningRequestStatusEnum.Approved.GetDisplayName())
                            {
                                await ApproveDocument(true, false);
                            }
                            else
                            {
                                var deleteDocResponse = await fileUploadService.RemoveDocument(documentId.ToString(), false);
                                if (!deleteDocResponse.IsSuccessStatusCode)
                                {
                                    notificationService.Notify(new NotificationMessage()
                                    {
                                        Severity = NotificationSeverity.Error,
                                        Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                                        Style = "position: fixed !important; left: 0; margin: auto; "
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                spinnerService.Hide();
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-10-30' Version="1.0" Branch="master">Approve Document through Workflow</History>
        protected async Task ApproveDocument(bool isDraftSigned, bool isDraftToDocumentConversion)
        {
            try
            {
                bool? dialogResponse = false;
                if (!isDraftSigned)
                {
                    if (attachmentTypeDetail.IsOpinion == true)
                    {
                        if (!showOpinionField)
                            showOpinionField = true;
                        showReasonField = false;
                        if (String.IsNullOrEmpty(Opinion))
                        {
                            opinionValidationMsg = @translationState.Translate("Required_Field");
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Required_Field_Opinion"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            return;
                        }
                    }
                }
                if (!isDraftSigned)
                {
                    dialogResponse = await dialogService.Confirm(
                      translationState.Translate("Approve_Confirm"),
                      translationState.Translate("Confirm"),
                      new ConfirmOptions()
                      {
                          OkButtonText = translationState.Translate("OK"),
                          CancelButtonText = translationState.Translate("Cancel")
                      });
                }

                if (dialogResponse == true || isDraftSigned)
                {
                    spinnerService.Show();
                    VersionId = draftedTemplate.VersionId;
                    var response = await cmsCaseTemplateService.GetDraftedTemplateDetailById(draftedTemplate.Id, VersionId);
                    if (response.IsSuccessStatusCode)
                    {
                        var document = (CmsDraftedTemplate)response.ResultData;
                        document.userName = await BrowserStorage.GetItemAsync<string>("User");
                        document.DraftedTemplateVersion.ReviewerRoleId = "";
                        document.DraftedTemplateVersion.ReviewerUserId = "";
                        document.FileData = FileData;
                        document.UploadFrom = document.ModuleId == (int)WorkflowModuleEnum.CaseManagement ? "CaseManagement" : "COMSConsultationManagement";
                        document.Project = "FATWA_WEB";
                        document.ModifiedBy = loginState.Username;
                        document.Opinion = Opinion;
                        document.DraftedTemplateVersion.StatusId = (int)DraftVersionStatusEnum.Approve;
                        document.DraftedTemplateVersion.ModifiedBy = loginState.Username;
                        document.DraftedTemplateVersion.ModifiedDate = DateTime.Now;
                        document.DraftedTemplateVersion.DraftActionId = (int)DraftActionIdEnum.Approved;
                        document.UserId = Guid.Parse(loginState.UserDetail.UserId);
                        document.IsDraftToDocumentConversion = isDraftToDocumentConversion;
                        document.IsDraftSigned = isDraftSigned;
                        if (TaskId != null)
                        {
                            taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
                            taskDetailVM.VersionId = VersionId;
                            taskDetailVM.SectorId = loginState.UserDetail.SectorTypeId;
                            if (!string.IsNullOrEmpty(draftedTemplate.ReviewerRoleId))
                            {
                                taskDetailVM.IsMultipleTaskUpdateForSameEntity = true;
                            }
                            var taskResponse = await taskService.DecisionTask(taskDetailVM);
                            if (!taskResponse.IsSuccessStatusCode)
                            {
                                await invalidRequestHandlerService.ReturnBadRequestNotification(taskResponse);
                            }
                        }
                        var respone = await workflowService.GetWorkflowConditionOptions(document.Id, document.DraftedTemplateVersion.StatusId);
                        if (respone.IsSuccessStatusCode)
                        {
                            conditionOptions = (List<WorkflowConditionsOptionVM>)respone.ResultData;
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(respone);
                        }
                        if (conditionOptions.Count > 0)
                        {
                            dialogService.Close(null);
                            spinnerService.Hide();
                            var result = await dialogService.OpenAsync<SelectConditionOptionPopup>(translationState.Translate("Select_Option"),
                            new Dictionary<string, object>()
                                {
                                    { "Options", conditionOptions},
                                    { "isActivity", false}
                                },
                                new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
                            if (result != null)
                            {
                                var SelectedOptionId = (int)result;
                                var selecetdConditionOption = conditionOptions.Where(x => x.ModuleOptionId == SelectedOptionId).FirstOrDefault();
                                await workflowService.ProcessWorkflowOptionActivites(selecetdConditionOption, document, document.ModuleId, document.ModuleId == (int)WorkflowModuleEnum.CaseManagement ? (int)WorkflowModuleTriggerEnum.UserSubmitsCaseDraft : (int)WorkflowModuleTriggerEnum.UserSubmitsConsultationDraft);
                            }
                            else
                            {
                                spinnerService.Hide();
                                return;
                            }
                        }
                        else
                        {
                            await workflowService.ProcessWorkflowActvivities(document, document.ModuleId, document.ModuleId == (int)WorkflowModuleEnum.CaseManagement ? (int)WorkflowModuleTriggerEnum.UserSubmitsCaseDraft : (int)WorkflowModuleTriggerEnum.UserSubmitsConsultationDraft);
                        }
                        if (isDraftSigned)
                            await fileUploadService.GetLatestVersionAndUpdateDocumentVersion((Guid)draftedTemplate.VersionId);
                        spinnerService.Hide();
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Changes_saved_successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        dialogService.Close(null);
                        await RedirectBack();
                    }
                    else
                    {
                        spinnerService.Hide();
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                }
            }
            catch (Exception ex)
            {
                spinnerService.Hide();
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-10-30' Version="1.0" Branch="master">Reject Document through Workflow</History>
        protected async Task RejectDocument()
        {
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
                      translationState.Translate("Reject_Confirm"),
                      translationState.Translate("Confirm"),
                      new ConfirmOptions()
                      {
                          OkButtonText = translationState.Translate("OK"),
                          CancelButtonText = translationState.Translate("Cancel")
                      });

                if (dialogResponse == true)
                {
                    spinnerService.Show();
                    VersionId = draftedTemplate.VersionId;
                    var response = await cmsCaseTemplateService.GetDraftedTemplateDetailById(draftedTemplate.Id, VersionId);

                    if (response.IsSuccessStatusCode)
                    {
                        var document = (CmsDraftedTemplate)response.ResultData;
                        document.userName = await BrowserStorage.GetItemAsync<string>("User");
                        document.DraftedTemplateVersion.ReviewerRoleId = "";
                        document.DraftedTemplateVersion.ReviewerUserId = "";
                        document.DraftedTemplateVersion.StatusId = (int)DraftVersionStatusEnum.Reject;
                        document.Reason = Reason;
                        //document.DraftedTemplateVersion.VersionId = Guid.NewGuid();
                        //document.DraftedTemplateVersion.CreatedBy = loginState.Username;
                        //document.DraftedTemplateVersion.CreatedDate = DateTime.Now;
                        document.DraftedTemplateVersion.ModifiedBy = loginState.Username;
                        document.DraftedTemplateVersion.ModifiedDate = DateTime.Now;
                        document.DraftedTemplateVersion.DraftActionId = (int)DraftActionIdEnum.Rejected;
                        document.UserId = Guid.Parse(loginState.UserDetail.UserId);
                        document.Opinion = Opinion;
                        if (TaskId != null)
                        {
                            taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
                            taskDetailVM.VersionId = VersionId;
                            taskDetailVM.SectorId = loginState.UserDetail.SectorTypeId;
                            if (!string.IsNullOrEmpty(draftedTemplate.ReviewerRoleId))
                            {
                                taskDetailVM.IsMultipleTaskUpdateForSameEntity = true;
                            }
                            var taskResponse = await taskService.DecisionTask(taskDetailVM);
                            if (!taskResponse.IsSuccessStatusCode)
                            {
                                await invalidRequestHandlerService.ReturnBadRequestNotification(taskResponse);
                            }
                        }
                        var respone = await workflowService.GetWorkflowConditionOptions(document.Id, document.DraftedTemplateVersion.StatusId);
                        if (respone.IsSuccessStatusCode)
                        {
                            conditionOptions = (List<WorkflowConditionsOptionVM>)respone.ResultData;
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(respone);
                        }
                        if (conditionOptions.Count > 0)
                        {
                            dialogService.Close(null);
                            spinnerService.Hide();
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
                                var selecetedConditionOption = conditionOptions.Where(x => x.ModuleOptionId == SelectedOptionId).FirstOrDefault();
                                await workflowService.ProcessWorkflowOptionActivites(selecetedConditionOption, document, document.ModuleId, document.ModuleId == (int)WorkflowModuleEnum.CaseManagement ? (int)WorkflowModuleTriggerEnum.UserSubmitsCaseDraft : (int)WorkflowModuleTriggerEnum.UserSubmitsConsultationDraft);
                            }
                            else
                            {
                                spinnerService.Hide();
                                return;
                            }
                        }
                        else
                        {
                            await workflowService.ProcessWorkflowActvivities(document, document.ModuleId, document.ModuleId == (int)WorkflowModuleEnum.CaseManagement ? (int)WorkflowModuleTriggerEnum.UserSubmitsCaseDraft : (int)WorkflowModuleTriggerEnum.UserSubmitsConsultationDraft);

                        }
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Changes_saved_successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        spinnerService.Hide();
                        dialogService.Close(null);
                        await RedirectBack();
                    }
                    else
                    {
                        spinnerService.Hide();
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                }
            }
            catch (Exception ex)
            {
                spinnerService.Hide();
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion

        #region Remarks/Reason for Rejection
        protected async Task RemarksForRejection()
        {
            try
            {
                if (!showReasonField)
                    showReasonField = true;
                showOpinionField = false;
                if (String.IsNullOrEmpty(Reason))
                {
                    reasonValidationMsg = @translationState.Translate("Required_Field");
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected async Task CancelRejectReason()
        {
            try
            {
                showReasonField = false;
                isReasonEntered = false;
                showOpinionField = false;
                Reason = string.Empty;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region Cancel Draft Document
        protected async Task CancelDraft()
        {
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
                      translationState.Translate("Cancel_Draft_Confirm"),
                      translationState.Translate("Confirm"),
                      new ConfirmOptions()
                      {
                          OkButtonText = translationState.Translate("OK"),
                          CancelButtonText = translationState.Translate("Cancel")
                      });
                if (dialogResponse == true)
                {
                    spinnerService.Show();
                    var response = await cmsCaseTemplateService.SoftDeleteDraftDocumentById(draftedTemplate.Id);
                    if (response.IsSuccessStatusCode)
                    {
                        if (TaskId != null)
                        {
                            taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
                            taskDetailVM.VersionId = Guid.Parse(VersionId);
                            taskDetailVM.SectorId = loginState.UserDetail.SectorTypeId;
                            if (!string.IsNullOrEmpty(draftedTemplate.ReviewerRoleId))
                            {
                                taskDetailVM.IsMultipleTaskUpdateForSameEntity = true;
                            }
                            var taskResponse = await taskService.DecisionTask(taskDetailVM);
                            if (!taskResponse.IsSuccessStatusCode)
                            {
                                await invalidRequestHandlerService.ReturnBadRequestNotification(taskResponse);
                            }
                        }
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Changes_saved_successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        spinnerService.Hide();
                        dialogService.Close();
                        await RedirectBack();
                    }
                    else
                    {
                        spinnerService.Hide();
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        private void ChangeOnInput(ChangeEventArgs e)
        {
            Reason = e.Value?.ToString();
            isReasonEntered = !string.IsNullOrWhiteSpace(Reason);
        }
        #endregion

        public async Task PopulatePdfFromHtml(string TemplateContent, CmsDraftedTemplate draft)
        {
            try
            {
                await PopulateHeaderFooter();
                HtmlToPdf converter = new HtmlToPdf();
                MemoryStream stream = new MemoryStream();

                // set converter options
                if (draft.AttachmentTypeId != (int)AttachmentTypeEnum.ContractReview)
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
                converter.Options.EmbedFonts = true;
                TemplateContent = string.Concat($"<style>@font-face {{font-family: 'Sultan';src: url('{Directory.GetCurrentDirectory().Replace("\\", "/") + "/wwwroot/fonts/Sultan/arfonts-sultan-normal.ttf"}') format('truetype');font-weight: normal;font-style: normal;font-display: swap;}} @font-face {{font-family: 'Sultan Medium';src: url('{Directory.GetCurrentDirectory().Replace("\\", "/") + "/wwwroot/fonts/Sultan/arfonts-sultan-mudaim.ttf"}') format('truetype');font-weight: normal;font-style: normal;font-display: swap;}}</style>", TemplateContent);
                SelectPdf.PdfDocument pdfDocument = converter.ConvertHtmlString(TemplateContent);
                pdfDocument.Save(stream);
                pdfDocument.Close();
                stream.Close();
                FileData = stream.ToArray();
                string base64String = Convert.ToBase64String(FileData);
                DocumentPath = "data:application/pdf;base64," + base64String;
                draft.FileData = FileData;
            }
            catch (Exception ex)
            {
                throw new Exception(translationState.Translate("Something_Went_Wrong"));
            }
        }

        public async Task NotifyMessage(string messages, int documentId)
        {
            await fileUploadService.DeleteUploadedDocument(documentId);
            spinnerService.Hide();
            notificationService.Notify(new NotificationMessage()
            {
                Severity = NotificationSeverity.Success,
                Detail = translationState.Translate(messages),
                Style = "position: fixed !important; left: 0; margin: auto; "
            });
        }
        #region Get Manager Task Reminder Data
        protected async Task GetManagerTaskReminderData()
        {
            try
            {
                var response = await lookupService.GetManagerTaskReminderData(Guid.Parse(TaskId));
                if (response.IsSuccessStatusCode)
                {
                    managerTaskReminderData = (List<ManagerTaskReminderVM>)response.ResultData;
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
    }
}
