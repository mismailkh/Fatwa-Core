using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using Syncfusion.Blazor.PdfViewer;
using Syncfusion.Blazor.PdfViewerServer;
using System.Collections.ObjectModel;
using System.Net;
using System.Text;
using Telerik.Blazor;
using Telerik.Blazor.Components;
using static FATWA_DOMAIN.Enums.LegalLegislationEnum;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Response;
using System.Security.Cryptography;
using PdfSharp.Drawing.Layout;
using PdfSharp.Pdf;
using MsgReader.Outlook;
using PdfSharp.Drawing;
using Microsoft.JSInterop;
using FATWA_WEB.Extensions;
using System.Web;
using System.Text.RegularExpressions;
using FATWA_GENERAL.Helper;
using static Org.BouncyCastle.Math.EC.ECCurve;
using DocumentFormat.OpenXml;

namespace FATWA_WEB.Pages.Lds
{
    public partial class AddLegalLegislation : ComponentBase
    {
        #region Constructor
        public AddLegalLegislation()
        {
            //FileNameResult = new TempAttachement();
            FileNameResult = new TempAttachementVM();
            ToolbarSettings = new PdfViewerToolbarSettings()
            {
                ToolbarItems = new List<ToolbarItem>()
                {
                    ToolbarItem.MagnificationTool,
                    ToolbarItem.SelectionTool,
                    ToolbarItem.UndoRedoTool,
                    ToolbarItem.SearchOption,
                    ToolbarItem.PanTool,
                    ToolbarItem.AnnotationEditTool,
                },
                AnnotationToolbarItems = new List<AnnotationToolbarItem>()
                {
                    AnnotationToolbarItem.ShapeTool,
                    AnnotationToolbarItem.ColorEditTool
                }
            };
            GetLegislationTypeDetails = new List<LegalLegislationType>();
            GetLegislationStatusDetails = new List<LegalLegislationStatus>();
            GetLegislationFlowStatusDetails = new List<LegalLegislationFlowStatus>();
            LegislationTagDetails = new List<LegalLegislationTag>();
            SelectedTagIds = new List<int>();
            legalPublicationSourceCS = new LegalPublicationSource() { PublicationDate = DateTime.Now, PublicationDate_Hijri = DateTime.Now };
            GetLegislationPublicationSourceNameDetails = new List<LegalPublicationSourceName>();
            legalLegislationSignature = new LegalLegislationSignature();
            SignatureDetailForGrid = new List<LegalLegislationSignature>();
            ShowSignatureGrid = false;
            RelationLinkResult = new LegalLegislationReference();
            legalArticlesList = new List<LegalArticle>();
            legalClausesList = new List<LegalClause>();
            explanatoryNote = new LegalExplanatoryNote();
            legalNote = new LegalNote();
            LegislationPreview = string.Empty;
            RelatedArticleDetails = new LegalArticle();
            GetLegalTemplateDetails = new List<LegalTemplate>();
            GetLegalTemplateSettingDetails = new List<LegalTemplateSetting>();
            legalTemplates = new LegalTemplate() { };
            legalTemplateSettings = new LegalTemplateSetting();
            TemplateCheckboxSelectedValues = new List<int>();
            ShowTemplateNameAndTypeField = false;
            OldTemplateCheckBoxSelectedValues = new List<int>();
            legalArticles = new LegalArticle();
            legalClauses = new LegalClause();
            ArticleStatusDetails = new List<LegalArticleStatus>();
            ShowExplanatoryNoteInArticle = false;
            //FileExplanatoryNoteTemp = new List<TempAttachement>();
            FileExplanatoryNoteTemp = new List<TempAttachementVM>();
            ExplanatoryNoteFileAdded = false;
            IntroWithRelationNumber = 0;
            SectionListDetails = new List<LegalSection>();
            ArticleNumberDifferenceCount = 0;
            ClauseNumberDifferenceCount = 0;
            LegislationFlowStatusCheck = false;
            //FileGrid = new RadzenDataGrid<TempAttachement>();
            FileGrid = new RadzenDataGrid<TempAttachementVM>();
            ShowTemplateAssociatedWithLegislationMessage = false;
            TotalLegislationCount = 0;
            resultFiles = new ObservableCollection<TempAttachementVM>();
            legalLegislationLegalTemplates = new List<LegalLegislationLegalTemplate>();
        }
        #endregion

        #region Parameter
        [Parameter]
        public dynamic LegislationId { get; set; }
        [Parameter]
        public dynamic editLegislationCheck { get; set; }
        #endregion

        #region Variable declaration
        //public TempAttachement FileNameResult { get; set; }
        public string PageStart { get; set; }
        public string PageEnd { get; set; }
        public TempAttachementVM FileNameResult { get; set; }
        public SfPdfViewerServer pdfViewer;
        public PdfViewerToolbarSettings ToolbarSettings;
        public string DocumentPath { get; set; } = string.Empty;
        public string DownloadFileName { get; set; } = string.Empty;
        public List<LegalLegislationType> GetLegislationTypeDetails { get; set; }
        public List<LegalLegislationStatus> GetLegislationStatusDetails { get; set; }
        public List<LegalLegislationFlowStatus> GetLegislationFlowStatusDetails { get; set; }
        public List<LegalLegislationTag> LegislationTagDetails { get; set; }
        protected List<int> SelectedTagIds { get; set; }
        public LegalPublicationSource legalPublicationSourceCS { get; set; }
        public List<LegalPublicationSourceName> GetLegislationPublicationSourceNameDetails { get; set; }
        public LegalLegislationSignature legalLegislationSignature { get; set; } = new LegalLegislationSignature();
        public List<LegalLegislationSignature> SignatureDetailForGrid { get; set; }
        public bool ShowSignatureGrid;
        public RadzenDataGrid<LegalLegislationSignature>? SignatureGridRef = new RadzenDataGrid<LegalLegislationSignature>();
        public LegalLegislationReference RelationLinkResult { get; set; }
        protected List<LegalArticle> legalArticlesList { get; set; }
        protected TelerikGrid<LegalArticle> gridSectionArticle { get; set; }
        protected TelerikGrid<LegalClause> gridSectionClause { get; set; }
        protected LegalArticle legalArticles { get; set; }
        protected LegalClause legalClauses { get; set; }
        protected List<LegalClause> legalClausesList { get; set; }
        public LegalExplanatoryNote explanatoryNote { get; set; }
        public LegalNote legalNote { get; set; }
        public string LegislationPreview { get; set; }
        public LegalArticle RelatedArticleDetails { get; set; }
        private bool resultSection;
        public List<LegalTemplate> GetLegalTemplateDetails { get; set; }
        public List<LegalTemplateSetting> GetLegalTemplateSettingDetails { get; set; }
        public LegalTemplate legalTemplates { get; set; }
        public LegalTemplateSetting legalTemplateSettings { get; set; }
        public string templateHeadingCheck { get; set; } = string.Empty;
        public IEnumerable<int> TemplateCheckboxSelectedValues { get; set; }
        public bool ShowTemplateNameAndTypeField { get; set; }
        public byte[] FileData { get; set; }
        public TelerikPdfViewer PdfViewerRef { get; set; }
        public string? previousHeading { get; set; } = string.Empty;
        public int OldArticleCount { get; set; } = 0;
        public int OldClauseCount { get; set; } = 0;
        public int ArticleCounter { get; set; } = 0;
        public int ClauseCounter { get; set; } = 0;
        public IEnumerable<int> OldTemplateCheckBoxSelectedValues { get; set; }
        public bool EditLegislationTrue { get; set; }
        public List<LegalArticleStatus> ArticleStatusDetails { get; set; }
        public int valueRadioButtonIntroduction { get; set; } = 0;
        public int valueRadioButtonLegislationBody { get; set; } = 0;
        public bool ShowPublicationStartEndPageCompareMessage { get; set; } = false;
        public bool ShowExplanatoryNoteInArticle { get; set; }
        //public List<TempAttachement> FileExplanatoryNoteTemp { get; set; }
        public List<TempAttachementVM> FileExplanatoryNoteTemp { get; set; }
        public bool ExplanatoryNoteFileAdded { get; set; }
        //public RadzenDataGrid<TempAttachement> FileGrid { get; set; }
        public RadzenDataGrid<TempAttachementVM> FileGrid { get; set; }
        public int IntroWithRelationNumber { get; set; }
        public List<LegalSection> SectionListDetails { get; set; }
        public int ArticleNumberDifferenceCount { get; set; }
        public int ClauseNumberDifferenceCount { get; set; }
        public bool LegislationFlowStatusCheck { get; set; }
        public bool SaveLegislationResponseResult { get; set; }
        public bool UpdateLegislationResponseResult { get; set; }
        public bool ShowTemplateAssociatedWithLegislationMessage { get; set; }
        public int TotalLegislationCount { get; set; }
        //Encryption/Descyption Key
        public string password = string.Empty;
        UnicodeEncoding UE;
        public byte[] key;
        RijndaelManaged RMCrypto;
        MemoryStream fsOut;
        public int data = 0;
        public byte[] MaskedFileData { get; set; }
        ObservableCollection<TempAttachementVM> resultFiles;
        public IEnumerable<WorkflowVM> activeworkflowlist { get; set; }
        public List<LegalLegislationLegalTemplate> legalLegislationLegalTemplates { get; set; }
        #endregion

        #region Validations class
        protected class ValidationClass
        {
            public string LegislationType { get; set; } = string.Empty;
            public string LegislationNumber { get; set; } = string.Empty;
            public string IssueDate { get; set; } = string.Empty;
            public string IssueDate_Hijri { get; set; } = string.Empty;
            public string TitleEn { get; set; } = string.Empty;
            public string LegislationStatus { get; set; } = string.Empty;
            public string StartDate { get; set; } = string.Empty;
            public string CancelDate { get; set; } = string.Empty;
            public string LegalTemplateChoose { get; set; } = string.Empty;
            public string LegalTemplateName { get; set; } = string.Empty;
            public string TemplateValues { get; set; } = string.Empty;
            public string ArticleNumber { get; set; } = string.Empty;
            public string ClauseNumber { get; set; } = string.Empty;
            public string PublicationName { get; set; } = string.Empty;
            public string PublicationIssueNumber { get; set; } = string.Empty;
            public string PublicationDate { get; set; } = string.Empty;
            public string PublicationDateHijri { get; set; } = string.Empty;
            public string PublicationPageStart { get; set; } = string.Empty;
            public string PublicationPageEnd { get; set; } = string.Empty;
            public string SignatureFullName { get; set; } = string.Empty;
            public string SignatureJobTitle { get; set; } = string.Empty;

        }
        #endregion

        #region Wizard variables
        public bool ShowWizard { get; set; } = true;
        public int Value { get; set; } = 0;
        protected string filePath1 = "\\images\\lmsLiteratureDetail-1.png";
        protected string filePathHover1 = "\\images\\lmsLiteratureDetail-1.png";

        protected string filePath2 = "\\images\\lmsLiteratureDetail-2.png";
        protected string filePathHover2 = "\\images\\lmsLiteratureDetail-2.png";

        protected string filePath3 = "\\images\\lmsLiteratureDetail-3.png";
        protected string filePathHover3 = "\\images\\lmsLiteratureDetail-3.png";

        protected string filePath4 = "\\images\\lmsLiteratureDetail-4.png";
        protected string filePathHover4 = "\\images\\lmsLiteratureDetail-4.png";

        protected string filePath5 = "\\images\\lmsLiteratureDetail-5.png";
        protected string filePathHover5 = "\\images\\lmsLiteratureDetail-5.png";

        protected string basePath1 = string.Empty;
        protected ValidationClass validations { get; set; } = new ValidationClass();
        public TelerikForm BasicSectionForm { get; set; }
        public DateTime Max = new DateTime(DateTime.Now.Date.Ticks);
        public DateTime Min = new DateTime(1900, 1, 1);
        protected bool isBasicStep = false;
        public TelerikForm Intro_Article_Section_Ref_Form { get; set; }
        public TelerikForm Notes_Form { get; set; }
        public TelerikForm Preview_Form { get; set; }
        public TelerikForm TemplateSetupForm { get; set; }

        #endregion

        #region Model full property Instance
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
        private LegalLegislation _legalLegislation;

        protected LegalLegislation legalLegislation
        {
            get
            {
                return _legalLegislation;
            }
            set
            {
                if (!object.Equals(_legalLegislation, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "legalLegislation", NewValue = value, OldValue = _legalLegislation };
                    _legalLegislation = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }
        #endregion

        #region Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();

            await Load();
            await JsInterop.InitilizePrincipleDetailReference(DotNetObjectReference.Create(this));
            spinnerService.Hide();
        }

        //<History Author = 'Hassan Abbas' Date='2023-07-05' Version="1.0" Branch="master"> Modified the function to Decrypt the attachment and then view, also added functionality to convert .msg file to .pdf for preview</History>
        protected async Task Load()
        {
            try
            {
                var resultArticleStatus = await legalLegislationService.GetLegalArticleStatusList();
                if (resultArticleStatus.IsSuccessStatusCode)
                {
                    ArticleStatusDetails = (List<LegalArticleStatus>)resultArticleStatus.ResultData;
                }
                var resultParent = await legalLegislationService.GetLegalSectionParentList();
                if (resultParent.IsSuccessStatusCode)
                {
                    SectionListDetails = (List<LegalSection>)resultParent.ResultData;
                }
                EditLegislationTrue = Convert.ToBoolean(editLegislationCheck);
                if (EditLegislationTrue)
                {
                    var responseLegislation = await legalLegislationService.GetLegalLegislationDetailsByUsingLegislationIdForEditForm(Guid.Parse(LegislationId));
                    if (responseLegislation != null)
                    {
                        legalLegislation = (LegalLegislation)responseLegislation.ResultData;
                        if (legalLegislation.Legislation_Number != string.Empty)
                        {
                            legalLegislation.EditCaseLegislationNumber = legalLegislation.Legislation_Number;
                        }
                        legalTemplates = legalLegislation.LegalTemplates;
                        await CheckLegalTemplateAssociatedWithLegislationCount(legalTemplates.TemplateId);
                        TemplateCheckboxSelectedValues = legalLegislation.LegalTemplates.SelectedCheckBoxValues;
                        var convertFromIEnumerableToList = TemplateCheckboxSelectedValues.ToList();
                        int indexIntro = convertFromIEnumerableToList.FindIndex(x => x == (int)LegalTemplateSettingEnum.Introduction_with_relation || x == (int)LegalTemplateSettingEnum.Introduction_without_relation);
                        if (indexIntro != -1)
                        {
                            valueRadioButtonIntroduction = convertFromIEnumerableToList[indexIntro];
                        }
                        int indexArticleClause = convertFromIEnumerableToList.FindIndex(x => x == (int)LegalTemplateSettingEnum.Articles_with_sections || x == (int)LegalTemplateSettingEnum.Articles_without_sections || x == (int)LegalTemplateSettingEnum.Clauses_with_sections || x == (int)LegalTemplateSettingEnum.Clauses_without_sections);
                        if (indexArticleClause != -1)
                        {
                            valueRadioButtonLegislationBody = convertFromIEnumerableToList[indexArticleClause];
                        }
                        if (legalLegislation.LegalLegislationTags.Count() != 0)
                        {
                            SelectedTagIds = legalLegislation.LegalLegislationTags;
                        }
                        if (legalLegislation.LegalPublicationSources.Count() != 0)
                        {
                            foreach (var item in legalLegislation.LegalPublicationSources)
                            {
                                if (item.PublicationNameId != 0)
                                {
                                    legalPublicationSourceCS = item;
                                }
                            }
                        }
                        else
                        {
                            legalPublicationSourceCS = new LegalPublicationSource() { PublicationDate = DateTime.Now, PublicationDate_Hijri = DateTime.Now };
                        }
                        if (legalLegislation.LegalLegislationSignatures.Count() != 0)
                        {
                            SignatureDetailForGrid = legalLegislation.LegalLegislationSignatures;
                            if (SignatureDetailForGrid.Count() != 0)
                            {
                                ShowSignatureGrid = true;
                            }
                        }
                        if (legalLegislation.LegalArticles.Count() != 0)
                        {
                            legalArticlesList = legalLegislation.LegalArticles;
                            foreach (var item in legalArticlesList)
                            {
                                var articleStatusDetail = ArticleStatusDetails.Where(x => x.Id == item.Article_Status).FirstOrDefault();
                                if (articleStatusDetail != null)
                                {
                                    item.Article_Status_Name_En = articleStatusDetail.Name_En;
                                    item.Article_Status_Name_Ar = articleStatusDetail.Name_Ar;
                                }
                                if (item.SectionId != Guid.Empty) // get section title for grid
                                {
                                    if (SectionListDetails.Count() != 0)
                                    {
                                        var resultSection = SectionListDetails.Where(x => x.SectionId == item.SectionId).FirstOrDefault();
                                        if (resultSection != null)
                                        {
                                            item.ShowSectionTitle = resultSection.SectionTitle;
                                        }
                                    }
                                }
                            }
                            legalLegislation.NumberofArticle = legalArticlesList.Count();
                            OldArticleCount = (int)legalLegislation.NumberofArticle;
                        }
                        if (legalLegislation.LegalClauses.Count() != 0)
                        {
                            legalClausesList = legalLegislation.LegalClauses;
                            foreach (var item in legalClausesList)
                            {
                                var clauseStatusDetail = ArticleStatusDetails.Where(x => x.Id == item.Clause_Status).FirstOrDefault();
                                if (clauseStatusDetail != null)
                                {
                                    item.Clause_Status_Name_En = clauseStatusDetail.Name_En;
                                    item.Clause_Status_Name_Ar = clauseStatusDetail.Name_Ar;
                                }
                            }
                            legalLegislation.NumberofClause = legalClausesList.Count();
                            OldClauseCount = (int)legalLegislation.NumberofClause;
                        }
                        if (legalLegislation.LegalExplanatoryNotes.Count() != 0)
                        {
                            foreach (var item in legalLegislation.LegalExplanatoryNotes)
                            {
                                if (item.LegislationId != Guid.Empty)
                                {
                                    explanatoryNote = item;

                                }
                            }
                        }
                        if (legalLegislation.legalNotes.Count() != 0)
                        {
                            foreach (var item in legalLegislation.legalNotes)
                            {
                                if (item.ParentId != Guid.Empty)
                                {
                                    legalNote = item;
                                }
                            }
                        }
                    }
                    PageStart = legalPublicationSourceCS.Page_Start.ToString();
                    PageEnd = legalPublicationSourceCS.Page_End.ToString();
                }
                else
                {
                    legalLegislation = new LegalLegislation()
                    {
                        LegislationId = Guid.Parse(LegislationId),
                        IssueDate = DateTime.Now.Date,
                        IssueDate_Hijri = DateTime.Now.Date,
                        StartDate = DateTime.Now.Date,
                        Legislation_Status = (int)LegislationStatus.Active
                    };
                    await GetAllTemplateSettingDetails();
                }
                var response = await legalLegislationService.GetAttachmentDetailForGridByUsingLegislationId(legalLegislation.LegislationId);
                if (response.IsSuccessStatusCode)
                {
                    resultFiles = (ObservableCollection<TempAttachementVM>)response.ResultData;
                    if (resultFiles != null)
                    {
                        FileNameResult = resultFiles.FirstOrDefault();
                        int indexnumber = 0;
                        var physicalPath = string.Empty;
                        foreach (var item in resultFiles)
                        {
                            FileExplanatoryNoteTemp.Insert(indexnumber, item);
                            indexnumber++;
                        }
                        ExplanatoryNoteFileAdded = true;
                        if (FileNameResult != null && FileNameResult.StoragePath != null)
                        {
#if DEBUG
                            {
                                physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + FileNameResult.StoragePath).Replace(@"\\", @"\");
                            }
#else
                            {
                                // Construct the physical path of the file on the server
                                physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + FileNameResult.StoragePath).Replace(@"\\", @"\");
                                // Remove the wwwroot/Attachments part of the path to get the actual file path
                                physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
                            }
#endif
                            if (!string.IsNullOrEmpty(physicalPath))
                            {
                                string base64String = await DocumentEncryptionService.GetDecryptedDocumentBase64(physicalPath, FileNameResult.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
                                DocumentPath = "data:application/pdf;base64," + base64String;
                                var referenceGuidId = FileNameResult.Guid != null ? FileNameResult.Guid.ToString() : FileNameResult.ReferenceGuid;
                                DownloadFileName = FileNameResult.FileName + "=" + FileNameResult.Description + "=" + referenceGuidId;
                            }
                            else
                            {
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Error,
                                    Detail = translationState.Translate("Authority_Letter_Not_Loaded"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                            }
                        }
                    }
                    else
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Authority_Letter_Not_Loaded"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                }

                var resultTemplate = await legalLegislationService.GetLegalTemplateDetails();
                if (resultTemplate.IsSuccessStatusCode)
                {
                    GetLegalTemplateDetails = (List<LegalTemplate>)resultTemplate.ResultData;

                    if (GetLegalTemplateDetails.Count() >= 1)
                    {
                        int index = GetLegalTemplateDetails.FindIndex(s => s.Template_Name.Contains("New Template") && s.IsDefault);
                        if (index != -1)
                        {
                            var temp = GetLegalTemplateDetails[0];
                            GetLegalTemplateDetails[0] = GetLegalTemplateDetails[index];
                            GetLegalTemplateDetails[index] = temp;
                        }
                        List<LegalTemplate>? TranslateTemplateName = new List<LegalTemplate>();

                        foreach (var item in GetLegalTemplateDetails)
                        {
                            if (item.Template_Name.Contains("New Template") && item.IsDefault)
                            {
                                if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                                {
                                    item.Template_Name = translationState.Translate("New_Template");
                                }
                                else
                                {
                                    item.Template_Name = translationState.Translate("New_Template");
                                }
                            }
                            TranslateTemplateName.Add(item);
                        }
                        GetLegalTemplateDetails = TranslateTemplateName;
                    }
                }
                var resultTemplateSetting = await legalLegislationService.GetLegalTemplateSettingDetails();
                if (resultTemplateSetting.IsSuccessStatusCode)
                {
                    GetLegalTemplateSettingDetails = (List<LegalTemplateSetting>)resultTemplateSetting.ResultData;
                }
                var resultType = await legalLegislationService.GetLegislationTypeDetails();
                if (resultType.IsSuccessStatusCode)
                {
                    GetLegislationTypeDetails = (List<LegalLegislationType>)resultType.ResultData;
                }
                var resultStatus = await legalLegislationService.GetLegislationStatusDetails();
                if (resultStatus.IsSuccessStatusCode)
                {
                    GetLegislationStatusDetails = (List<LegalLegislationStatus>)resultStatus.ResultData;
                }
                var resultFlowStatus = await legalLegislationService.GetLegislationFlowStatusDetails();
                if (resultFlowStatus.IsSuccessStatusCode)
                {
                    GetLegislationFlowStatusDetails = (List<LegalLegislationFlowStatus>)resultFlowStatus.ResultData;
                }
                var resultTag = await legalLegislationService.GetLegislationTagDetails();
                if (resultTag.IsSuccessStatusCode)
                {
                    LegislationTagDetails = (List<LegalLegislationTag>)resultTag.ResultData;
                }
                var resultSourceName = await legalLegislationService.GetPublicationSourceNameDetails();
                if (resultSourceName.IsSuccessStatusCode)
                {
                    GetLegislationPublicationSourceNameDetails = (List<LegalPublicationSourceName>)resultSourceName.ResultData;
                }
                if (explanatoryNote.ExplanatoryNoteId != Guid.Empty || explanatoryNote.LegislationId != Guid.Empty)
                {
                    var responseExplanatory1 = await legalLegislationService.GetExplanatoryNoteAttachmentFromTempTableByUsingId(explanatoryNote.ExplanatoryNoteId);
                    if (responseExplanatory1.IsSuccessStatusCode)
                    {
                        FileExplanatoryNoteTemp = (List<TempAttachementVM>)responseExplanatory1.ResultData;

                        // at the top of attachments list add legislation file.
                        if (FileNameResult != null && FileNameResult.StoragePath != null)
                        {
                            FileExplanatoryNoteTemp.Insert(0, FileNameResult);
                        }
                        ExplanatoryNoteFileAdded = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task GetAllTemplateSettingDetails()
        {
            try
            {
                var responseSetting = await legalLegislationService.GetAllTemplateSettingDetails();
                if (responseSetting.IsSuccessStatusCode)
                {
                    legalLegislationLegalTemplates = (List<LegalLegislationLegalTemplate>)responseSetting.ResultData;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task CheckLegalTemplateAssociatedWithLegislationCount(Guid templateId)
        {
            try
            {
                var responseCount = await legalLegislationService.CountAssociatedLegislationInTemplateByUsingTemplateId(templateId);
                if (responseCount.IsSuccessStatusCode)
                {
                    TotalLegislationCount = (int)responseCount.ResultData;
                    if (TotalLegislationCount != 0)
                    {
                        ShowTemplateAssociatedWithLegislationMessage = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Check and get template selected value details (template dropdown on change)
        protected async Task CheckAndGetTemplateSelectedValueDetails()
        {
            if (legalTemplates.TemplateId != Guid.Empty)
            {
                var result = GetLegalTemplateDetails.Where(x => x.TemplateId == legalTemplates.TemplateId).FirstOrDefault();
                if (result != null)
                {
                    if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                    {
                        if (result.Template_Name.Contains("New Template") && result.IsDefault)
                        {
                            legalTemplates.Template_Name = string.Empty;
                            legalTemplates.Legislation_Type = 0;
                            TemplateCheckboxSelectedValues = new List<int>();
                            ShowTemplateNameAndTypeField = true;
                            valueRadioButtonIntroduction = 0;
                            valueRadioButtonLegislationBody = 0;
                        }
                        else
                        {
                            //var response = await legalLegislationService.GetRegisteredTemplateDetailsByUsingSelectedTemplateId(legalTemplates.TemplateId); // get details from db and bind all checkboxes
                            //if (response.IsSuccessStatusCode)
                            //{
                            //legalTemplates = (LegalTemplate)response.ResultData;
                            //TemplateCheckboxSelectedValues = legalTemplates.SelectedCheckBoxValues;
                            if (legalLegislationLegalTemplates.Count() != 0)
                            {
                                TemplateCheckboxSelectedValues = legalLegislationLegalTemplates.Where(x => x.TemplateId == legalTemplates.TemplateId).Select(z => z.TemplateSettingId).Distinct().ToList();
                                var convertFromIEnumerableToList = TemplateCheckboxSelectedValues.ToList();
                                int indexIntro = convertFromIEnumerableToList.FindIndex(x => x == (int)LegalTemplateSettingEnum.Introduction_with_relation || x == (int)LegalTemplateSettingEnum.Introduction_without_relation);
                                if (indexIntro != -1)
                                {
                                    valueRadioButtonIntroduction = convertFromIEnumerableToList[indexIntro];
                                }
                                int indexArticleClause = convertFromIEnumerableToList.FindIndex(x => x == (int)LegalTemplateSettingEnum.Articles_with_sections || x == (int)LegalTemplateSettingEnum.Articles_without_sections || x == (int)LegalTemplateSettingEnum.Clauses_with_sections || x == (int)LegalTemplateSettingEnum.Clauses_without_sections);
                                if (indexArticleClause != -1)
                                {
                                    valueRadioButtonLegislationBody = convertFromIEnumerableToList[indexArticleClause];
                                }
                                legalTemplates.Template_Name = result.Template_Name;
                                legalTemplates.Legislation_Type = result.Legislation_Type;

                            }
                            //}
                            ShowTemplateNameAndTypeField = false;
                        }
                    }
                    else
                    {
                        if (result.Template_Name.Contains("قالب جديد") && result.IsDefault)
                        {
                            legalTemplates.Template_Name = string.Empty;
                            legalTemplates.Legislation_Type = 0;
                            TemplateCheckboxSelectedValues = new List<int>();
                            ShowTemplateNameAndTypeField = true;
                            valueRadioButtonIntroduction = 0;
                            valueRadioButtonLegislationBody = 0;
                        }
                        else
                        {
                            //var response = await legalLegislationService.GetRegisteredTemplateDetailsByUsingSelectedTemplateId(legalTemplates.TemplateId); // get details from db and bind all checkboxes
                            //if (response.IsSuccessStatusCode)
                            //{
                            //    legalTemplates = (LegalTemplate)response.ResultData;
                            if (legalLegislationLegalTemplates.Count() != 0)
                            {
                                TemplateCheckboxSelectedValues = legalLegislationLegalTemplates.Where(x => x.TemplateId == legalTemplates.TemplateId).Select(z => z.TemplateSettingId).Distinct().ToList();
                                //TemplateCheckboxSelectedValues = legalTemplates.SelectedCheckBoxValues;
                                var convertFromIEnumerableToList = TemplateCheckboxSelectedValues.ToList();
                                int indexIntro = convertFromIEnumerableToList.FindIndex(x => x == (int)LegalTemplateSettingEnum.Introduction_with_relation || x == (int)LegalTemplateSettingEnum.Introduction_without_relation);
                                if (indexIntro != -1)
                                {
                                    valueRadioButtonIntroduction = convertFromIEnumerableToList[indexIntro];
                                }
                                int indexArticleClause = convertFromIEnumerableToList.FindIndex(x => x == (int)LegalTemplateSettingEnum.Articles_with_sections || x == (int)LegalTemplateSettingEnum.Articles_without_sections || x == (int)LegalTemplateSettingEnum.Clauses_with_sections || x == (int)LegalTemplateSettingEnum.Clauses_without_sections);
                                if (indexArticleClause != -1)
                                {
                                    valueRadioButtonLegislationBody = convertFromIEnumerableToList[indexArticleClause];
                                }
                                legalTemplates.Template_Name = result.Template_Name;
                                legalTemplates.Legislation_Type = result.Legislation_Type;
                            }
                            //}
                            ShowTemplateNameAndTypeField = false;
                        }
                    }

                }
                await CheckLegalTemplateAssociatedWithLegislationCount(legalTemplates.TemplateId);
            }
            else
            {
                legalTemplates.Template_Name = string.Empty;
                legalTemplates.Legislation_Type = 0;
                TemplateCheckboxSelectedValues = new List<int>();
                ShowTemplateNameAndTypeField = false;
                valueRadioButtonIntroduction = 0;
                valueRadioButtonLegislationBody = 0;
            }
        }
        #endregion

        #region template setting checkbox (radio button) onChange

        protected async Task OnChangeIntroductionRadioButtonSetting(LegalTemplateSetting legalTemplateSetting, int value)
        {
            if (value != 0)
            {
                var resultCheck = TemplateCheckboxSelectedValues.ToList();
                int withRelation = resultCheck.FindIndex(x => x == (int)LegalTemplateSettingEnum.Introduction_with_relation);
                if (withRelation != -1)
                {
                    resultCheck[withRelation] = value;
                }
                int withOutRelation = resultCheck.FindIndex(x => x == (int)LegalTemplateSettingEnum.Introduction_without_relation);
                if (withOutRelation != -1)
                {
                    resultCheck[withOutRelation] = value;
                }
                var res = resultCheck.Where(x => x == value).FirstOrDefault();
                if (res == 0)
                {
                    resultCheck.Add(value);
                }
                TemplateCheckboxSelectedValues = resultCheck;
            }
        }
        protected async Task OnChangeArticleClauseRadioButtonSetting(LegalTemplateSetting legalTemplateSetting, int value)
        {
            if (value != 0)
            {
                var resultCheck = TemplateCheckboxSelectedValues.ToList();
                int withRelation = resultCheck.FindIndex(x => x == (int)LegalTemplateSettingEnum.Articles_with_sections);
                if (withRelation != -1)
                {
                    resultCheck[withRelation] = value;
                }
                int withOutRelation = resultCheck.FindIndex(x => x == (int)LegalTemplateSettingEnum.Articles_without_sections);
                if (withOutRelation != -1)
                {
                    resultCheck[withOutRelation] = value;
                }
                int withClauseRelation = resultCheck.FindIndex(x => x == (int)LegalTemplateSettingEnum.Clauses_with_sections);
                if (withClauseRelation != -1)
                {
                    resultCheck[withClauseRelation] = value;
                }
                int withOutClauseRelation = resultCheck.FindIndex(x => x == (int)LegalTemplateSettingEnum.Clauses_without_sections);
                if (withOutClauseRelation != -1)
                {
                    resultCheck[withOutClauseRelation] = value;
                }
                var res = resultCheck.Where(x => x == value).FirstOrDefault();
                if (res == 0)
                {
                    resultCheck.Add(value);
                }
                TemplateCheckboxSelectedValues = resultCheck;
            }
        }
        #endregion

        #region Validate template setup
        protected bool ValidateTemplateSetupOnChange()
        {
            bool templatesetupDetailsValid = true;
            if (legalTemplates.TemplateId == Guid.Empty)
            {
                validations.LegalTemplateChoose = "k-invalid";
                templatesetupDetailsValid = false;
            }
            else
            {
                validations.LegalTemplateChoose = "k-valid";
            }
            if (legalTemplates.TemplateId != Guid.Empty)
            {
                var result = GetLegalTemplateDetails.Where(x => x.TemplateId == legalTemplates.TemplateId).FirstOrDefault();
                if (result != null)
                {
                    if (result.Template_Name.Contains(translationState.Translate("New_Template")) && result.IsDefault)
                    {
                        if (string.IsNullOrWhiteSpace(legalTemplates.Template_Name))
                        {
                            validations.LegalTemplateName = "k-invalid";
                            templatesetupDetailsValid = false;
                        }
                        else
                        {
                            validations.LegalTemplateName = "k-valid";
                        }
                        if (legalTemplates.Legislation_Type == 0)
                        {
                            validations.LegislationType = "k-invalid";
                            templatesetupDetailsValid = false;
                        }
                        else
                        {
                            validations.LegislationType = "k-valid";
                        }
                        if (TemplateCheckboxSelectedValues.Count() == 0)
                        {
                            validations.TemplateValues = "k-invalid";
                            templatesetupDetailsValid = false;
                        }
                        else
                        {
                            validations.TemplateValues = "k-valid";
                            //var LegislationBodySelected = TemplateCheckboxSelectedValues
                            //                                .Where(item =>
                            //                                    item == (int)LegalTemplateSettingEnum.Introduction_with_relation ||
                            //                                    item == (int)LegalTemplateSettingEnum.Introduction_without_relation ||
                            //                                    (item >= (int)LegalTemplateSettingEnum.Articles_with_sections &&
                            //                                    item <= (int)LegalTemplateSettingEnum.Clauses_without_sections) ||
                            //                                    item == (int)LegalTemplateSettingEnum.Explanatory_Note ||
                            //                                    item == (int)LegalTemplateSettingEnum.Note)
                            //                                .ToList();

                            //var LegislationBodySelected = TemplateCheckboxSelectedValues
                            //                                .Where(item =>

                            //                                    item >= (int)LegalTemplateSettingEnum.Introduction_with_relation &&
                            //                                    item <= (int)LegalTemplateSettingEnum.Note && item != (int)LegalTemplateSettingEnum.Publication_details)
                            //                                .ToList();


                            //var LegislationBodySelected = TemplateCheckboxSelectedValues.Where(item => item == (int)LegalTemplateSettingEnum.Introduction_with_relation || item == (int)LegalTemplateSettingEnum.Introduction_without_relation || item >= (int)LegalTemplateSettingEnum.Articles_with_sections && item <= (int)LegalTemplateSettingEnum.Clauses_without_sections || item == (int)LegalTemplateSettingEnum.Explanatory_Note || item == (int)LegalTemplateSettingEnum.Note).ToList();
                            var LegislationBodySelected = TemplateCheckboxSelectedValues.Where(item => item >= (int)LegalTemplateSettingEnum.Articles_with_sections && item <= (int)LegalTemplateSettingEnum.Clauses_without_sections).ToList();

                            if (LegislationBodySelected.Count() != 0)
                            {
                                validations.TemplateValues = "k-valid";
                            }
                            else
                            {
                                validations.TemplateValues = "k-invalid";
                                templatesetupDetailsValid = false;
                            }
                            int counter = 0;
                            foreach (var item in TemplateCheckboxSelectedValues)
                            {
                                if (item == (int)LegalTemplateSettingEnum.Introduction_with_relation)
                                {
                                    counter++;
                                }
                                else if (item == (int)LegalTemplateSettingEnum.Introduction_without_relation)
                                {
                                    counter++;
                                }
                                else if (item == (int)LegalTemplateSettingEnum.Explanatory_Note)
                                {
                                    counter++;
                                }
                                else if (item == (int)LegalTemplateSettingEnum.Note)
                                {
                                    counter++;
                                }
                            }

                            if (counter == 3)
                            {
                                validations.TemplateValues = "k-valid";
                            }
                            else
                            {
                                counter = 0;
                                validations.TemplateValues = "k-invalid";
                                templatesetupDetailsValid = false;
                            }
                        }
                    }
                }
            }
            return templatesetupDetailsValid;
        }

        #endregion

        #region Template edit icon click
        protected async Task EditTemplateIconClick(int currentStepIndex)
        {
            var args = new WizardStepChangeEventArgs()
            {
                IsCancelled = false,
                TargetIndex = currentStepIndex - currentStepIndex
            };
            Value = 0;
            if (!args.IsCancelled)
            {
                Value = currentStepIndex - currentStepIndex;
            }
        }
        #endregion

        #region Wizard Template step
        //<History Author = 'Umer Zaman' Date='2022-10-13' Version="1.0" Branch="master"> Basic Section Next button</History>
        protected async Task OnTemplateSetupStepChangeCustom(int currentStepIndex)
        {
            var args = new WizardStepChangeEventArgs()
            {
                IsCancelled = false,
                TargetIndex = currentStepIndex + 1
            };

            await OnTemplateSetupStepChange(args);

            if (!args.IsCancelled)
            {
                Value = currentStepIndex + 1;
            }
        }

        protected async Task OnTemplateSetupStepChange(WizardStepChangeEventArgs args)
        {
            isBasicStep = true;
            bool valid = ValidateTemplateSetupOnChange();
            if (valid)
            {
                List<int> SettingCheckBoxes = new List<int>();
                foreach (var item in GetLegalTemplateSettingDetails)
                {
                    SettingCheckBoxes.Add(item.TemplateSettingId);
                }
                var remainingList = SettingCheckBoxes.Where(x => !TemplateCheckboxSelectedValues.Contains(x));
                RemoveTemplateChangedCheckboxValue(remainingList);

                TemplateCheckboxSelectedValues = BubbleSortTemplateList(TemplateCheckboxSelectedValues);
                await Task.Delay(1000);

                if (legalArticlesList.Count() != 0)
                {
                    legalLegislation.NumberofArticle = legalArticlesList.Count();
                }
                if (legalClausesList.Count() != 0)
                {
                    legalLegislation.NumberofClause = legalClausesList.Count();
                }
                var result = GetLegalTemplateDetails.Where(x => x.TemplateId == legalTemplates.TemplateId).FirstOrDefault();
                if (result != null)
                {
                    LegalTemplate? Obj = new LegalTemplate();
                    if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                    {
                        if (result.Template_Name.Contains("New Template") && result.IsDefault)
                        {
                            Obj.TemplateId = Guid.NewGuid();
                            Obj.Template_Name = legalTemplates.Template_Name;
                            Obj.Legislation_Type = legalTemplates.Legislation_Type;
                            Obj.IsDefault = false;
                            legalLegislation.LegalTemplates = Obj;
                        }
                        else
                        {
                            Obj.TemplateId = legalTemplates.TemplateId;
                            Obj.Template_Name = legalTemplates.Template_Name;
                            Obj.Legislation_Type = legalTemplates.Legislation_Type;
                            legalLegislation.LegalTemplates = Obj;
                        }
                    }
                    else
                    {
                        if (result.Template_Name.Contains("قالب جديد") && result.IsDefault)
                        {
                            Obj.TemplateId = Guid.NewGuid();
                            Obj.Template_Name = legalTemplates.Template_Name;
                            Obj.Legislation_Type = legalTemplates.Legislation_Type;
                            Obj.IsDefault = false;
                            legalLegislation.LegalTemplates = Obj;
                        }
                        else
                        {
                            Obj.TemplateId = legalTemplates.TemplateId;
                            Obj.Template_Name = legalTemplates.Template_Name;
                            Obj.Legislation_Type = legalTemplates.Legislation_Type;
                            legalLegislation.LegalTemplates = Obj;
                        }
                    }

                }
                List<int> ObjValues = new List<int>();
                foreach (var item in TemplateCheckboxSelectedValues)
                {
                    ObjValues.Add(item);
                }
                legalLegislation.LegalTemplates.SelectedCheckBoxValues = ObjValues;

                Value += 1;

                var isFormValid = TemplateSetupForm.IsValid();
                if (!isFormValid)
                {
                    args.IsCancelled = true;
                }
                legalLegislation.Legislation_Type = legalTemplates.Legislation_Type;
            }
            else
            {
                if (TemplateCheckboxSelectedValues.Count() != 0)
                {
                    var resultLegislationBody = TemplateCheckboxSelectedValues.Where(item => item >= (int)LegalTemplateSettingEnum.Articles_with_sections && item <= (int)LegalTemplateSettingEnum.Clauses_without_sections).ToList();
                    if (resultLegislationBody.Count() == 0)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Legislation_Body_Option_Error_Message"),
                            //Summary = $"" + translationState.Translate("Error"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                    else
                    {
                        int counter = 0;
                        foreach (var item in TemplateCheckboxSelectedValues)
                        {
                            if (item == (int)LegalTemplateSettingEnum.Introduction_with_relation)
                            {
                                counter++;
                            }
                            else if (item == (int)LegalTemplateSettingEnum.Introduction_without_relation)
                            {
                                counter++;
                            }
                            else if (item == (int)LegalTemplateSettingEnum.Explanatory_Note)
                            {
                                counter++;
                            }
                            else if (item == (int)LegalTemplateSettingEnum.Note)
                            {
                                counter++;
                            }
                        }
                        if (counter != 3)
                        {
                            counter = 0;
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Mandatory_Template_Option_Error_Message"),
                                //Summary = $"" + translationState.Translate("Error"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                    }
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Fill_Required_Fields"),
                        //Summary = $"" + translationState.Translate("Error"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }

                args.IsCancelled = true;
            }
        }

        private IEnumerable<int> BubbleSortTemplateList(IEnumerable<int> arr)
        {
            var num = arr.ToArray();
            for (int i = 0; i < num.Length; i++)
            {
                for (int j = num.Length - 1; j > i; j--)
                {
                    if (num[j] < num[j - 1])
                    {
                        var temp = num[i];
                        num[i] = num[j];
                        num[j] = temp;
                    }
                }
            }
            return num;
        }

        private void RemoveTemplateChangedCheckboxValue(IEnumerable<int> newCheckboxValues)
        {
            if (newCheckboxValues.Count() != 0)
            {
                foreach (var item in newCheckboxValues)
                {
                    if (item == (int)LegalTemplateSettingEnum.Legislation_Number)
                    {
                        legalLegislation.Legislation_Number = string.Empty;
                    }
                    if (item == (int)LegalTemplateSettingEnum.Legislation_Issue_Date)
                    {
                        legalLegislation.IssueDate = null;
                        legalLegislation.IssueDate_Hijri = null;
                    }
                    if (item == (int)LegalTemplateSettingEnum.Legislation_Start_Date)
                    {
                        legalLegislation.StartDate = null;
                    }
                    if (item == (int)LegalTemplateSettingEnum.Legislation_Subject)
                    {
                        legalLegislation.LegislationTitle = string.Empty;
                    }
                    if (item == (int)LegalTemplateSettingEnum.Introduction_without_relation)
                    {
                        IntroWithRelationNumber = TemplateCheckboxSelectedValues.Where(x => x == (int)LegalTemplateSettingEnum.Introduction_with_relation).FirstOrDefault();

                        if (IntroWithRelationNumber == 0)
                        {
                            legalLegislation.LegalLegislationReferences = new List<LegalLegislationReference>();
                            legalLegislation.Introduction = string.Empty;
                        }
                    }
                    if (item == (int)LegalTemplateSettingEnum.Publication_details)
                    {
                        legalLegislation.LegalPublicationSources = new List<LegalPublicationSource>();
                        legalPublicationSourceCS = new LegalPublicationSource() { PublicationDate = DateTime.Now, PublicationDate_Hijri = DateTime.Now };
                    }
                    if (item == (int)LegalTemplateSettingEnum.Articles_with_sections)
                    {
                        var resultArticleWithoutSection = newCheckboxValues.Where(x => x == (int)LegalTemplateSettingEnum.Articles_without_sections).FirstOrDefault();
                        if (resultArticleWithoutSection != 0)
                        {
                            legalArticlesList = new List<LegalArticle>();
                            legalLegislation.LegalArticles = new List<LegalArticle>();
                        }
                    }
                    if (item == (int)LegalTemplateSettingEnum.Articles_without_sections)
                    {
                        var resultArticleWithSection = newCheckboxValues.Where(x => x == (int)LegalTemplateSettingEnum.Articles_with_sections).FirstOrDefault();
                        if (resultArticleWithSection != 0)
                        {
                            legalArticlesList = new List<LegalArticle>();
                            legalLegislation.LegalArticles = new List<LegalArticle>();
                        }
                    }
                    if (item == (int)LegalTemplateSettingEnum.Clauses_with_sections)
                    {
                        var resultClauseWithoutSection = newCheckboxValues.Where(x => x == (int)LegalTemplateSettingEnum.Clauses_without_sections).FirstOrDefault();
                        if (resultClauseWithoutSection != 0)
                        {
                            legalClausesList = new List<LegalClause>();
                            legalLegislation.LegalClauses = new List<LegalClause>();
                        }
                    }
                    if (item == (int)LegalTemplateSettingEnum.Clauses_without_sections)
                    {
                        var resultClauseWithSection = newCheckboxValues.Where(x => x == (int)LegalTemplateSettingEnum.Clauses_with_sections).FirstOrDefault();
                        if (resultClauseWithSection != 0)
                        {
                            legalClausesList = new List<LegalClause>();
                            legalLegislation.LegalClauses = new List<LegalClause>();
                        }
                    }
                    if (item == (int)LegalTemplateSettingEnum.Explanatory_Note)
                    {
                        ExplanatoryNoteFileAdded = false;
                        ShowExplanatoryNoteInArticle = false;
                        explanatoryNote = new LegalExplanatoryNote();
                        legalLegislation.LegalExplanatoryNotes = new List<LegalExplanatoryNote>();
                    }
                    if (item == (int)LegalTemplateSettingEnum.Note)
                    {
                        legalNote = new LegalNote();
                        legalLegislation.legalNotes = new List<LegalNote>();
                    }
                }
            }
        }
        #endregion

        #region Add LegalLegislation Tags
        protected async Task AddTagsButtonClick(MouseEventArgs args)
        {
            try
            {
                var dialogResult = await dialogService.OpenAsync<AddLegalLegislationTags>(
                translationState.Translate("Add_Legislation_Tags"),
                null,
                new DialogOptions() { CloseDialogOnOverlayClick = true });
                await Task.Delay(100);
                var resultTags = (LegalLegislationTag)dialogResult;
                if (resultTags != null)
                {
                    var resultTag = await legalLegislationService.GetLegislationTagDetails();
                    if (resultTag.IsSuccessStatusCode)
                    {
                        LegislationTagDetails = (List<LegalLegislationTag>)resultTag.ResultData;
                    }
                    SelectedTagIds.Add(resultTags.TagId);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Add signature button click
        protected async Task AddSignatureDetailToGrid()
        {
            if (!string.IsNullOrWhiteSpace(legalLegislationSignature.Full_Name) && !string.IsNullOrWhiteSpace(legalLegislationSignature.Job_Title))
            {
                ShowSignatureGrid = true;
                SignatureDetailForGrid.Add(new LegalLegislationSignature()
                {
                    LegislationId = legalLegislation.LegislationId,
                    Full_Name = legalLegislationSignature.Full_Name,
                    Job_Title = legalLegislationSignature.Job_Title
                });
            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Fill_Required_Fields"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                if (SignatureDetailForGrid.Count() == 0)
                {
                    ShowSignatureGrid = false;
                }
            }
            legalLegislationSignature = new LegalLegislationSignature();
            SignatureGridRef.Reset();
        }
        #endregion

        #region Signature grid operations
        protected async Task UpdateSignatureHandler(LegalLegislationSignature args) //step 3.  grid row record click for update
        {
            var resultObj = args;
            if (resultObj != null)
            {
                legalLegislationSignature.Full_Name = resultObj.Full_Name;
                legalLegislationSignature.Job_Title = resultObj.Job_Title;
                SignatureDetailForGrid.Remove(resultObj);
                await Task.Delay(1000);
                await SignatureGridRef.Reload();
            }
        }
        protected async Task DeleteSignatureHandler(LegalLegislationSignature args)
        {
            var DeleteObj = args;
            bool? dialogResponse = await dialogService.Confirm(
                translationState.Translate("Edit_Grid_Article_Confirm_Message"),
                translationState.Translate("Confirm"),
                new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                });
            if (dialogResponse == true)
            {
                // remove from grid list
                SignatureDetailForGrid.Remove(DeleteObj);
                await Task.Delay(200);
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Success,
                    Detail = translationState.Translate("Legal_Signature_Delete_Success_Message"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                if (SignatureDetailForGrid.Count() == 0)
                {
                    ShowSignatureGrid = false;
                }
                await Task.Delay(1000);
                await SignatureGridRef.Reload();
            }
        }
        #endregion

        #region Validate Basic Details
        protected void ValidateBasicDetailsOnChange()
        {
            if (isBasicStep == true)
            {
                ValidateBasicDetails();
            }
        }
        protected bool ValidateBasicDetails()
        {
            bool basicDetailsValid = true;
            if (legalLegislation.Legislation_Type == 0)
            {
                validations.LegislationType = "k-invalid";
                basicDetailsValid = false;
            }
            else
            {
                validations.LegislationType = "k-valid";
            }
            if (TemplateCheckboxSelectedValues.Count() != 0)
            {
                foreach (var item in TemplateCheckboxSelectedValues)
                {
                    if (item == (int)LegalTemplateSettingEnum.Legislation_Number)
                    {
                        if (string.IsNullOrWhiteSpace(legalLegislation.Legislation_Number))
                        {
                            validations.LegislationNumber = "k-invalid";
                            basicDetailsValid = false;
                        }
                        else
                        {
                            validations.LegislationNumber = "k-valid";
                        }
                    }
                    if (item == (int)LegalTemplateSettingEnum.Legislation_Issue_Date)
                    {
                        if (string.IsNullOrWhiteSpace(legalLegislation.IssueDate.ToString()))
                        {
                            validations.IssueDate = "k-invalid";
                            basicDetailsValid = false;
                        }
                        else
                        {
                            validations.IssueDate = "k-valid";
                        }
                        if (string.IsNullOrWhiteSpace(legalLegislation.IssueDate_Hijri.ToString()))
                        {
                            validations.IssueDate_Hijri = "k-invalid";
                            basicDetailsValid = false;
                        }
                        else
                        {
                            validations.IssueDate_Hijri = "k-valid";
                        }
                    }
                    if (item == (int)LegalTemplateSettingEnum.Legislation_Start_Date)
                    {
                        if (string.IsNullOrWhiteSpace(legalLegislation.StartDate.ToString()))
                        {
                            validations.StartDate = "k-invalid";
                            basicDetailsValid = false;
                        }
                        else
                        {
                            validations.StartDate = "k-valid";
                        }
                    }
                    if (item == (int)LegalTemplateSettingEnum.Legislation_Subject)
                    {
                        if (string.IsNullOrWhiteSpace(legalLegislation.LegislationTitle))
                        {
                            validations.TitleEn = "k-invalid";
                            basicDetailsValid = false;
                        }
                        else
                        {
                            validations.TitleEn = "k-valid";
                        }
                    }
                    if (item == (int)LegalTemplateSettingEnum.Articles_with_sections || item == (int)LegalTemplateSettingEnum.Articles_without_sections)
                    {
                        if (legalLegislation.NumberofArticle == 0)
                        {
                            validations.ArticleNumber = "k-invalid";
                            basicDetailsValid = false;
                        }
                        else
                        {
                            validations.ArticleNumber = "k-valid";
                        }
                    }
                    if (item == (int)LegalTemplateSettingEnum.Clauses_with_sections || item == (int)LegalTemplateSettingEnum.Clauses_without_sections)
                    {
                        if (legalLegislation.NumberofClause == 0)
                        {
                            validations.ClauseNumber = "k-invalid";
                            basicDetailsValid = false;
                        }
                        else
                        {
                            validations.ClauseNumber = "k-valid";
                        }
                    }
                    if (item == (int)LegalTemplateSettingEnum.Publication_details)
                    {
                        if (legalPublicationSourceCS.PublicationNameId == 0)
                        {
                            validations.PublicationName = "k-invalid";
                            basicDetailsValid = false;
                        }
                        else
                        {
                            validations.PublicationName = "k-valid";
                        }
                        if (legalPublicationSourceCS.Issue_Number == 0)
                        {
                            validations.PublicationIssueNumber = "k-invalid";
                            basicDetailsValid = false;
                        }
                        else
                        {
                            validations.PublicationIssueNumber = "k-valid";
                        }
                        if (string.IsNullOrWhiteSpace(legalPublicationSourceCS.PublicationDate.ToString()))
                        {
                            validations.PublicationDate = "k-invalid";
                            basicDetailsValid = false;
                        }
                        else
                        {
                            validations.PublicationDate = "k-valid";
                        }
                        if (string.IsNullOrWhiteSpace(legalPublicationSourceCS.PublicationDate_Hijri.ToString()))
                        {
                            validations.PublicationDateHijri = "k-invalid";
                            basicDetailsValid = false;
                        }
                        else
                        {
                            validations.PublicationDateHijri = "k-valid";
                        }
                        if (legalPublicationSourceCS.Page_Start == 0)
                        {
                            validations.PublicationPageStart = "k-invalid";
                            basicDetailsValid = false;
                        }
                        else
                        {
                            validations.PublicationPageStart = "k-valid";
                        }
                        if (legalPublicationSourceCS.Page_End == 0)
                        {
                            validations.PublicationPageEnd = "k-invalid";
                            basicDetailsValid = false;
                        }
                        else
                        {
                            validations.PublicationPageEnd = "k-valid";
                        }
                        if (legalPublicationSourceCS.Page_Start != 0 && legalPublicationSourceCS.Page_End != 0)
                        {
                            if (legalPublicationSourceCS.Page_Start > legalPublicationSourceCS.Page_End)
                            {
                                ShowPublicationStartEndPageCompareMessage = true;
                                validations.PublicationPageStart = "k-invalid";
                                validations.PublicationPageEnd = "k-invalid";
                                basicDetailsValid = false;
                            }
                            else
                            {
                                validations.PublicationPageStart = "k-valid";
                                validations.PublicationPageEnd = "k-valid";
                                ShowPublicationStartEndPageCompareMessage = false;
                            }
                        }
                    }
                }
            }
            if (legalLegislation.Legislation_Status == 0)
            {
                validations.LegislationStatus = "k-invalid";
                basicDetailsValid = false;
            }
            else
            {
                validations.LegislationStatus = "k-valid";
            }
            if (legalLegislation.Legislation_Status == (int)LegislationStatus.Expired)
            {
                if (string.IsNullOrWhiteSpace(legalLegislation.CanceledDate.ToString()))
                {
                    validations.CancelDate = "k-invalid";
                    basicDetailsValid = false;
                }
                else
                {
                    validations.CancelDate = "k-valid";
                }
            }
            if (SignatureDetailForGrid.Count() == 0)
            {
                validations.SignatureFullName = "k-invalid";
                validations.SignatureJobTitle = "k-invalid";
                basicDetailsValid = false;
            }
            else
            {
                validations.SignatureFullName = "k-valid";
                validations.SignatureJobTitle = "k-valid";
            }
            return basicDetailsValid;
        }

        #endregion

        #region Wizard Basic Details onChange Click
        //<History Author = 'Umer Zaman' Date='2022-10-13' Version="1.0" Branch="master"> Basic Section Next button</History>
        protected async Task OnBasicDetailsStepChangeCustom(int currentStepIndex)
        {
            var args = new WizardStepChangeEventArgs()
            {
                IsCancelled = false,
                TargetIndex = currentStepIndex + 1
            };

            await OnBasicDetailsStepChange(args);

            if (!args.IsCancelled)
            {
                Value = currentStepIndex + 1;
            }
        }

        protected async Task OnBasicDetailsStepChange(WizardStepChangeEventArgs args)
        {
            isBasicStep = true;
            bool valid = ValidateBasicDetails();
            if (valid)
            {
                if (EditLegislationTrue)
                {
                    var resultLegislationNumber = legalLegislation.LegalTemplates.SelectedCheckBoxValues.Where(x => x == (int)LegalTemplateSettingEnum.Legislation_Number).FirstOrDefault();
                    if (resultLegislationNumber != 0)
                    {
                        if (legalLegislation.EditCaseLegislationNumber != legalLegislation.Legislation_Number)
                        {
                            var resultLegNumber = await legalLegislationService.CheckLegislationNumberDuplication(legalLegislation.Legislation_Type, legalLegislation.Legislation_Number);
                            if (resultLegNumber.IsSuccessStatusCode)
                            {
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Error,
                                    Detail = translationState.Translate("Legislation_Number_Duplication_Message"),
                                    //Summary = $"" + translationState.Translate("Error"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                                args.IsCancelled = true;
                            }
                            else
                            {
                                var res = await CheckNumberofArticleAndClauseChangeOrNot();
                                if (res == true)
                                {
                                    Value += 1;
                                }
                                else
                                {
                                    args.IsCancelled = true;
                                }
                            }
                        }
                        else
                        {
                            var res = await CheckNumberofArticleAndClauseChangeOrNot();
                            if (res == true)
                            {
                                Value += 1;
                            }
                            else
                            {
                                args.IsCancelled = true;
                            }
                        }
                    }
                    else
                    {
                        var res = await CheckNumberofArticleAndClauseChangeOrNot();
                        if (res == true)
                        {
                            Value += 1;
                        }
                        else
                        {
                            args.IsCancelled = true;
                        }
                    }
                }
                else
                {
                    var resultLegislationNumber = legalLegislation.LegalTemplates.SelectedCheckBoxValues.Where(x => x == (int)LegalTemplateSettingEnum.Legislation_Number).FirstOrDefault();
                    if (resultLegislationNumber != 0)
                    {
                        var resultLegNumber = await legalLegislationService.CheckLegislationNumberDuplication(legalLegislation.Legislation_Type, legalLegislation.Legislation_Number);
                        if (resultLegNumber.IsSuccessStatusCode)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Legislation_Number_Duplication_Message"),
                                //Summary = $"" + translationState.Translate("Error"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            args.IsCancelled = true;
                        }
                        else
                        {
                            var res = await CheckNumberofArticleAndClauseChangeOrNot();
                            if (res == true)
                            {
                                Value += 1;
                            }
                            else
                            {
                                args.IsCancelled = true;
                            }
                        }
                    }
                    else
                    {
                        var res = await CheckNumberofArticleAndClauseChangeOrNot();
                        if (res == true)
                        {
                            Value += 1;
                        }
                        else
                        {
                            args.IsCancelled = true;
                        }
                    }
                }
            }
            else
            {
                if (ShowPublicationStartEndPageCompareMessage)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Publication_Page_Start_End_Check"),
                        //Summary = $"" + translationState.Translate("Error"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Fill_Required_Fields"),
                        //Summary = $"" + translationState.Translate("Error"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
                args.IsCancelled = true;
            }
            var isFormValid = BasicSectionForm.IsValid();
            if (!isFormValid)
            {
                args.IsCancelled = true;
            }
        }

        protected async Task<bool> CheckNumberofArticleAndClauseChangeOrNot()
        {
            try
            {
                if (TemplateCheckboxSelectedValues.Count() != 0)
                {
                    foreach (var item in TemplateCheckboxSelectedValues)
                    {
                        if (item == (int)LegalTemplateSettingEnum.Articles_with_sections || item == (int)LegalTemplateSettingEnum.Articles_without_sections)
                        {
                            if (OldArticleCount != 0)
                            {
                                if (legalLegislation.NumberofArticle != OldArticleCount)
                                {
                                    // first show the confirmation message popup to user that number of articles changed may effects the article list in grid

                                    if (await dialogService.Confirm(translationState.Translate("Number_Of_Article_Change_Confirm_Message"), translationState.Translate("Confirm"), new ConfirmOptions()
                                    {
                                        OkButtonText = translationState.Translate("Yes"),
                                        CancelButtonText = translationState.Translate("No")
                                    }) == true)
                                    {
                                        if (legalLegislation.NumberofArticle < OldArticleCount)
                                        {
                                            ArticleNumberDifferenceCount = (int)(OldArticleCount - legalLegislation.NumberofArticle);
                                            if (legalArticlesList.Count() != 0)
                                            {
                                                foreach (var itemArticle in legalArticlesList.ToList())
                                                {
                                                    if (itemArticle.Start_Date == null)
                                                    {
                                                        legalArticlesList.Remove(itemArticle);
                                                        ArticleNumberDifferenceCount--;
                                                    }
                                                    if (ArticleNumberDifferenceCount == 0)
                                                    {
                                                        break;
                                                    }
                                                }
                                                if (ArticleNumberDifferenceCount != 0)
                                                {
                                                    for (int i = legalArticlesList.ToList().Count() - 1; i >= 0; i--)
                                                    {
                                                        legalArticlesList.RemoveAt(i);
                                                        ArticleNumberDifferenceCount--;
                                                        if (ArticleNumberDifferenceCount == 0)
                                                        {
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            OldArticleCount = (int)legalLegislation.NumberofArticle;
                                        }
                                        else if (legalLegislation.NumberofArticle > OldArticleCount)
                                        {
                                            var differentCount = legalLegislation.NumberofArticle - OldArticleCount;
                                            for (int i = 0; i < differentCount; i++)
                                            {
                                                LegalArticle ObjArticle = new LegalArticle();
                                                ObjArticle.ArticleId = Guid.NewGuid();
                                                ObjArticle.LegislationId = legalLegislation.LegislationId;
                                                ObjArticle.Article_Status = (int)LegalArticleStatusEnum.Active;
                                                var articleStatusDetail = ArticleStatusDetails.Where(x => x.Id == ObjArticle.Article_Status).FirstOrDefault();
                                                if (articleStatusDetail != null)
                                                {
                                                    ObjArticle.Article_Status_Name_En = articleStatusDetail.Name_En;
                                                    ObjArticle.Article_Status_Name_Ar = articleStatusDetail.Name_Ar;
                                                }
                                                //ObjArticle.Article_Title = translationState.Translate("Article") + " " + i;
                                                legalArticlesList.Add(ObjArticle);
                                            }
                                            OldArticleCount = (int)legalLegislation.NumberofArticle;
                                        }
                                        return true;
                                    } // confirmation popup claosed
                                    else
                                    {
                                        return false;
                                    }
                                }
                            }
                            if (legalLegislation.NumberofArticle != 0)
                            {
                                OldArticleCount = (int)legalLegislation.NumberofArticle;
                                if (legalArticlesList.Count() == 0)
                                {
                                    for (int i = 1; i <= legalLegislation.NumberofArticle; i++)
                                    {
                                        LegalArticle ObjArticle = new LegalArticle();
                                        ObjArticle.ArticleId = Guid.NewGuid();
                                        ObjArticle.LegislationId = legalLegislation.LegislationId;
                                        ObjArticle.Article_Status = (int)LegalArticleStatusEnum.Active;
                                        var articleStatusDetail = ArticleStatusDetails.Where(x => x.Id == ObjArticle.Article_Status).FirstOrDefault();
                                        if (articleStatusDetail != null)
                                        {
                                            ObjArticle.Article_Status_Name_En = articleStatusDetail.Name_En;
                                            ObjArticle.Article_Status_Name_Ar = articleStatusDetail.Name_Ar;
                                        }
                                        //ObjArticle.Article_Title = translationState.Translate("Article") + " " + i;
                                        legalArticlesList.Add(ObjArticle);
                                    }
                                }
                                return true;
                            }
                        }
                        if (item == (int)LegalTemplateSettingEnum.Clauses_with_sections || item == (int)LegalTemplateSettingEnum.Clauses_without_sections)
                        {
                            if (OldClauseCount != 0)
                            {
                                if (legalLegislation.NumberofClause != OldClauseCount)
                                {
                                    // first show the confirmation message popup to user that number of articles changed may effects the article list in grid

                                    if (await dialogService.Confirm(translationState.Translate("Number_Of_Article_Change_Confirm_Message"), translationState.Translate("Confirm"), new ConfirmOptions()
                                    {
                                        OkButtonText = translationState.Translate("OK"),
                                        CancelButtonText = translationState.Translate("Cancel")
                                    }) == true)
                                    {
                                        if (legalLegislation.NumberofClause < OldClauseCount)
                                        {
                                            ClauseNumberDifferenceCount = (int)(OldClauseCount - legalLegislation.NumberofClause);
                                            if (legalClausesList.Count() != 0)
                                            {
                                                foreach (var itemClause in legalClausesList.ToList())
                                                {
                                                    if (itemClause.Start_Date == null)
                                                    {
                                                        legalClausesList.Remove(itemClause);
                                                        ClauseNumberDifferenceCount--;
                                                    }
                                                    if (ClauseNumberDifferenceCount == 0)
                                                    {
                                                        break;
                                                    }
                                                }
                                                if (ClauseNumberDifferenceCount != 0)
                                                {
                                                    for (int i = legalClausesList.ToList().Count() - 1; i >= 0; i--)
                                                    {
                                                        legalClausesList.RemoveAt(i);
                                                        ClauseNumberDifferenceCount--;
                                                        if (ClauseNumberDifferenceCount == 0)
                                                        {
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            OldClauseCount = (int)legalLegislation.NumberofClause;
                                        }
                                        else if (legalLegislation.NumberofClause > OldClauseCount)
                                        {
                                            var differentCount = legalLegislation.NumberofClause - OldClauseCount;
                                            for (int i = 0; i < differentCount; i++)
                                            {
                                                LegalClause ObjClause = new LegalClause();
                                                ObjClause.ClauseId = Guid.NewGuid();
                                                ObjClause.LegislationId = legalLegislation.LegislationId;
                                                ObjClause.Clause_Status = (int)LegalArticleStatusEnum.Active;
                                                var articleStatusDetail = ArticleStatusDetails.Where(x => x.Id == ObjClause.Clause_Status).FirstOrDefault();
                                                if (articleStatusDetail != null)
                                                {
                                                    ObjClause.Clause_Status_Name_En = articleStatusDetail.Name_En;
                                                    ObjClause.Clause_Status_Name_Ar = articleStatusDetail.Name_Ar;
                                                }
                                                //ObjClause.Clause_Name = translationState.Translate("Clause") + " " + i;
                                                legalClausesList.Add(ObjClause);
                                            }
                                            OldClauseCount = (int)legalLegislation.NumberofClause;
                                        }
                                        return true;
                                    } // confirmation popup claosed
                                    else
                                    {
                                        return false;
                                    }
                                }
                            }
                            if (legalLegislation.NumberofClause != 0)
                            {
                                OldClauseCount = (int)legalLegislation.NumberofClause;
                                if (legalClausesList.Count() == 0)
                                {
                                    for (int j = 1; j <= legalLegislation.NumberofClause; j++)
                                    {
                                        LegalClause ObjClause = new LegalClause();
                                        ObjClause.ClauseId = Guid.NewGuid();
                                        ObjClause.LegislationId = legalLegislation.LegislationId;
                                        ObjClause.Clause_Status = (int)LegalArticleStatusEnum.Active;
                                        var articleStatusDetail = ArticleStatusDetails.Where(x => x.Id == ObjClause.Clause_Status).FirstOrDefault();
                                        if (articleStatusDetail != null)
                                        {
                                            ObjClause.Clause_Status_Name_En = articleStatusDetail.Name_En;
                                            ObjClause.Clause_Status_Name_Ar = articleStatusDetail.Name_Ar;
                                        }
                                        //ObjClause.Clause_Name = translationState.Translate("Clause") + " " + j;
                                        legalClausesList.Add(ObjClause);
                                    }
                                }
                                return true;
                            }
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Grid Add Article and Clause click
        protected async Task AddGridArticleButtonClick(GridCommandEventArgs args, bool check)
        {
            try
            {
                if (legalLegislation.LegalTemplates.SelectedCheckBoxValues.Count() != 0)
                {
                    foreach (var item in legalLegislation.LegalTemplates.SelectedCheckBoxValues)
                    {
                        if (item == (int)LegalTemplateSettingEnum.Explanatory_Note)
                        {
                            ShowExplanatoryNoteInArticle = true;
                        }
                    }
                }
                legalArticles = (LegalArticle)args.Item;
                var result = await dialogService.OpenAsync<AddLegislationArticle>(translationState.Translate("Add_Article"),
                    new Dictionary<string, object>()
                {
                   {"ArticleWithSectionCheck", check },
                   { "AddArticleFromGrid", legalArticles },
                   { "NewLegislationDetailForArticleEffect", legalLegislation}
                });
                var ArticleGridResult = (LegalArticle)result;
                if (ArticleGridResult != null)
                {

                    ArticleGridResult.Article_Text = ConvertHtmlConvertToPlainText(ArticleGridResult.Article_Text);
                    // add remaining values
                    var articleStatusDetail = ArticleStatusDetails.Where(x => x.Id == ArticleGridResult.Article_Status).FirstOrDefault();
                    if (articleStatusDetail != null)
                    {
                        ArticleGridResult.Article_Status_Name_En = articleStatusDetail.Name_En;
                        ArticleGridResult.Article_Status_Name_Ar = articleStatusDetail.Name_Ar;
                    }
                    var resAllArticleDetail = await legalLegislationService.GetLatestArticleDetailByUsingLegislationId(ArticleGridResult.LegislationId); // get those article model detail whose nextarticleid field value become empty.
                    if (resAllArticleDetail.IsSuccessStatusCode)
                    {
                        RelatedArticleDetails = (LegalArticle)resAllArticleDetail.ResultData; // get latest legislation articles record for NextArticleId variable
                    }
                    else if (resAllArticleDetail.StatusCode == HttpStatusCode.NoContent)
                    {
                        RelatedArticleDetails = new LegalArticle();
                    }
                    if (RelatedArticleDetails != null) // if record found
                    {
                        ArticleGridResult.ExistingArticleId = RelatedArticleDetails.ArticleId;
                        ArticleGridResult.NextArticleId = Guid.Empty;
                    }
                    else
                    {
                        ArticleGridResult.ExistingArticleId = Guid.Empty;
                        ArticleGridResult.NextArticleId = Guid.Empty;
                    }
                    int index = legalArticlesList.FindIndex(s => s.ArticleId == ArticleGridResult.ArticleId);
                    if (index != -1)
                    {
                        legalArticlesList[index] = ArticleGridResult;
                    }
                    gridSectionArticle.Rebind();
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        protected async Task DeleteGridArticleButtonClick(GridCommandEventArgs args)
        {
            try
            {
                legalArticles = (LegalArticle)args.Item;
                if (await dialogService.Confirm(translationState.Translate("Edit_Grid_Article_Confirm_Message"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    legalArticlesList.Remove(legalArticles);
                    gridSectionArticle.Rebind();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void OnInStockArticleDrop(GridRowDropEventArgs<LegalArticle> args)
        {
            foreach (var item in args.Items)
            {
                legalArticlesList.Remove(item);
            }

            InsertArticleItem(args);
        }
        private void InsertArticleItem(GridRowDropEventArgs<LegalArticle> args)
        {
            var destinationIndex = 0;

            if (args.DestinationItem != null)
            {
                destinationIndex = legalArticlesList.IndexOf(args.DestinationItem);
                if (args.DropPosition == GridRowDropPosition.After)
                {
                    destinationIndex += 1;
                }
            }

            if (destinationIndex < 0)
            {
                destinationIndex = 0;
            }

            legalArticlesList.InsertRange(destinationIndex, args.Items);
        }
        public void ArticleRowRender(GridRowRenderEventArgs args)
        {
            try
            {
                var item = args.Item as LegalArticle;
                if (item.Article_Name == null || item.Article_Title == null)
                {
                    args.Class = "empty-article";
                }
            }
            catch (Exception ex)
            {
            }
        }
        protected async Task AddGridClauseButtonClick(GridCommandEventArgs args, bool checkSection)
        {
            try
            {
                legalClauses = (LegalClause)args.Item;
                var result = await dialogService.OpenAsync<AddLegislationClause>(translationState.Translate("Add_Clause"),
                    new Dictionary<string, object>()
                {
                   { "ClauseWithSectionCheck", checkSection },
                   { "AddClauseFromGrid", legalClauses }
                });
                var ClauseGridResult = (LegalClause)result;
                if (ClauseGridResult != null)
                {
                    // add remaining values
                    var articleStatusDetail = ArticleStatusDetails.Where(x => x.Id == ClauseGridResult.Clause_Status).FirstOrDefault();
                    if (articleStatusDetail != null)
                    {
                        ClauseGridResult.Clause_Status_Name_En = articleStatusDetail.Name_En;
                        ClauseGridResult.Clause_Status_Name_Ar = articleStatusDetail.Name_Ar;
                    }
                    //now update Grid list
                    int index = legalClausesList.FindIndex(s => s.ClauseId == ClauseGridResult.ClauseId);
                    if (index != -1)
                    {
                        legalClausesList[index] = ClauseGridResult;
                    }
                    gridSectionClause.Rebind();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected async Task DeleteGridClauseButtonClick(GridCommandEventArgs args)
        {
            try
            {
                legalClauses = (LegalClause)args.Item;
                if (await dialogService.Confirm(translationState.Translate("Edit_Grid_Article_Confirm_Message"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    legalClausesList.Remove(legalClauses);
                    gridSectionClause.Rebind();
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void OnInStockClauseDrop(GridRowDropEventArgs<LegalClause> args)
        {
            foreach (var item in args.Items)
            {
                legalClausesList.Remove(item);
            }

            InsertClauseItem(args);
        }
        private void InsertClauseItem(GridRowDropEventArgs<LegalClause> args)
        {
            var destinationIndex = 0;

            if (args.DestinationItem != null)
            {
                destinationIndex = legalClausesList.IndexOf(args.DestinationItem);
                if (args.DropPosition == GridRowDropPosition.After)
                {
                    destinationIndex += 1;
                }
            }

            if (destinationIndex < 0)
            {
                destinationIndex = 0;
            }

            legalClausesList.InsertRange(destinationIndex, args.Items);
        }
        public void ClauseRowRender(GridRowRenderEventArgs args)
        {
            try
            {
                var item = args.Item as LegalClause;
                if (item.Clause_Name == null || item.Clause_Content == null)
                {
                    args.Class = "empty-article";
                }
            }
            catch (Exception ex)
            {
            }
        }
        private string ConvertHtmlConvertToPlainText(string articleText)
        {
            // Replace </div><div> sequences with a space
            string withoutbothDivs = Regex.Replace(articleText, @"</div><div>", " ");
            // Replace <div> sequences with a space
            string withoutDivs = Regex.Replace(withoutbothDivs, @"<div>", " ");
            // Replace <br> tags with spaces
            string withoutBr = Regex.Replace(withoutDivs, @"<br\s*/?>", " ");
            // Replace all html tags
            string withoutTags = Regex.Replace(withoutBr, @"<[^>]+>", "");
            // Replace HTML entities like &nbsp; with spaces
            string withoutEntities = Regex.Replace(withoutTags, @"&\S+?;", " ");
            // Remove extra spaces between strings
            string withoutExtraSpaces = Regex.Replace(withoutEntities, @"\s+", " ");
            // Trim leading and trailing spaces
            string trimmedString = withoutExtraSpaces.Trim();
            return trimmedString;

        }
        #endregion

        #region Wizard Add Relation, Article, Clause, Section & Relation in Note part
        protected async Task AddRelation()
        {
            try
            {
                var result = await dialogService.OpenAsync<AddLegislationRelation>(translationState.Translate("Add_Relation"),
                new Dictionary<string, object>()
                {
                   { "ArticleEffectCheckInRelationPage", false },
                   { "NewLegislationDetailForArticleEffect", legalLegislation}
                },
                new DialogOptions() { Width = "80% !important", CloseDialogOnOverlayClick = true });
                RelationLinkResult = (LegalLegislationReference)result;
                if (RelationLinkResult != null)
                {
                    RelationLinkResult.Reference_Parent_Id = legalLegislation.LegislationId;
                    legalLegislation.LegalLegislationReferences.Add(RelationLinkResult);
                    if (RelationLinkResult.Legislation_Link != string.Empty)
                    {
                        string linkText = RelationLinkResult.Legislation_Link.Replace(RelationLinkResult.Legislation_Link, "<a href='javascript:void(0)' id='" + RelationLinkResult.Legislation_Link_Id + "' class='relation-attachment-popup' name='Legislation'>" + RelationLinkResult.Legislation_Link + "</a>");
                        RelationLinkResult.Legislation_Link = linkText;
                        legalLegislation.Introduction = legalLegislation.Introduction + " " + RelationLinkResult.Legislation_Link;
                    }
                    if (RelationLinkResult.CheckNewLegislation == true)
                    {
                        legalLegislation.NewLegislationAddedId.Add(RelationLinkResult.Legislation_Link_Id);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected async Task AddRelationInNote()
        {
            try
            {
                var result = await dialogService.OpenAsync<AddLegislationRelation>(translationState.Translate("Add_Relation"),
                new Dictionary<string, object>()
                {
                   { "ArticleEffectCheckInRelationPage", false },
                   { "NewLegislationDetailForArticleEffect", legalLegislation}
                },
                new DialogOptions() { Width = "80% !important", CloseDialogOnOverlayClick = true });
                RelationLinkResult = (LegalLegislationReference)result;
                if (RelationLinkResult != null)
                {
                    if (RelationLinkResult.Legislation_Link != string.Empty)
                    {
                        string linkText = RelationLinkResult.Legislation_Link.Replace(RelationLinkResult.Legislation_Link, "<a href='javascript:void(0)' id='" + RelationLinkResult.Legislation_Link_Id + "' class='relation-attachment-popup' name='Legislation'>" + RelationLinkResult.Legislation_Link + "</a>");
                        RelationLinkResult.Legislation_Link = linkText;
                        legalNote.Note_Text = legalNote.Note_Text + " " + RelationLinkResult.Legislation_Link;
                    }
                    if (RelationLinkResult.CheckNewLegislation == true)
                    {
                        legalLegislation.NewLegislationAddedId.Add(RelationLinkResult.Legislation_Link_Id);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected async Task AddSection()
        {
            try
            {
                var result = await dialogService.OpenAsync<AddLegislationSection>(@translationState.Translate("Add_Section"));
                var sectionResult = (LegalSection)result;
                if (sectionResult != null)
                {
                    sectionResult.LegislationId = legalLegislation.LegislationId;
                    if (sectionResult.Section_Parent_Id != Guid.Empty)
                    {
                        List<LegalSection>? resultSectionListDetails = new List<LegalSection>();
                        var resultParent = await legalLegislationService.GetLegalSectionParentList();
                        if (resultParent.IsSuccessStatusCode)
                        {
                            resultSectionListDetails = (List<LegalSection>)resultParent.ResultData;
                        }
                        if (resultSectionListDetails.Count() != 0)
                        {
                            var resultModel = resultSectionListDetails.Where(x => x.SectionId == sectionResult.Section_Parent_Id).FirstOrDefault();
                            sectionResult.HasChildren = true;
                            sectionResult.ParentId = resultModel.Section_Number;
                            sectionResult.SectionParentTitle = resultModel.SectionTitle;
                            var responseResult = await legalLegislationService.UpdateSelectedSectionAsParentHasChildColumn(resultModel.SectionId);
                            if (responseResult.IsSuccessStatusCode)
                            {

                            }
                        }

                    }
                    else
                    {
                        sectionResult.HasChildren = false;
                        sectionResult.ParentId = 0;
                    }
                    // add this section in table because if user again add section then this section must available in parent section list.
                    var response = await legalLegislationService.AddLegalSection(sectionResult);
                    if (response.IsSuccessStatusCode)
                    {
                        resultSection = (bool)response.ResultData;
                        legalLegislation.LegalSections.Add(sectionResult);
                    }
                    if (resultSection)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Legislation_Section_Success_Message"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                    else
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Legislation_Section_UnSuccess_Message"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected async Task AddArticle(bool args)
        {
            try
            {
                if (legalLegislation.LegalTemplates.SelectedCheckBoxValues.Count() != 0)
                {
                    foreach (var item in legalLegislation.LegalTemplates.SelectedCheckBoxValues)
                    {
                        if (item == (int)LegalTemplateSettingEnum.Explanatory_Note)
                        {
                            ShowExplanatoryNoteInArticle = true;
                        }
                    }
                }
                var result = await dialogService.OpenAsync<AddLegislationArticle>(translationState.Translate("Add_Article"),
                    new Dictionary<string, object>()
                {
                    {"ArticleWithSectionCheck", args },
                    { "ShowExplanatoryNoteInArticle", ShowExplanatoryNoteInArticle},
                    { "NewLegislationDetailForArticleEffect", legalLegislation}
                },
                new DialogOptions() { Width = "100% !important", CloseDialogOnOverlayClick = true });
                var ArticleLinkResult = (LegalArticle)result;
                if (ArticleLinkResult != null)
                {
                    // add remaining values
                    var articleStatusDetail = ArticleStatusDetails.Where(x => x.Id == ArticleLinkResult.Article_Status).FirstOrDefault();
                    if (articleStatusDetail != null)
                    {
                        ArticleLinkResult.Article_Status_Name_En = articleStatusDetail.Name_En;
                        ArticleLinkResult.Article_Status_Name_Ar = articleStatusDetail.Name_Ar;
                    }
                    ArticleLinkResult.LegislationId = legalLegislation.LegislationId;
                    var resAllArticleDetail = await legalLegislationService.GetLatestArticleDetailByUsingLegislationId(legalLegislation.LegislationId); // get those article model detail whose nextarticleid field value become empty.
                    if (resAllArticleDetail.IsSuccessStatusCode)
                    {
                        RelatedArticleDetails = (LegalArticle)resAllArticleDetail.ResultData; // get latest legislation articles record for NextArticleId variable
                    }
                    else if (resAllArticleDetail.StatusCode == HttpStatusCode.NoContent)
                    {
                        RelatedArticleDetails = new LegalArticle();
                    }
                    if (RelatedArticleDetails != null) // if record found
                    {
                        ArticleLinkResult.ExistingArticleId = RelatedArticleDetails.ArticleId;
                        ArticleLinkResult.NextArticleId = Guid.Empty;
                    }
                    else
                    {
                        ArticleLinkResult.ExistingArticleId = Guid.Empty;
                        ArticleLinkResult.NextArticleId = Guid.Empty;
                    }
                    ArticleLinkResult.Article_Text = ConvertHtmlConvertToPlainText(ArticleLinkResult.Article_Text);
                    //now add section id & title in article list and show this list in grid
                    legalArticlesList.Add(ArticleLinkResult);
                    gridSectionArticle.Rebind();
                }
            }
            catch (Exception ex)
            {

            }
        }

        protected async Task AddClause(bool args)
        {
            try
            {
                var result = await dialogService.OpenAsync<AddLegislationClause>(translationState.Translate("Add_Clause"),
                    new Dictionary<string, object>()
                {
                    {"ClauseWithSectionCheck", args }
                },
                new DialogOptions() { Width = "100% !important", CloseDialogOnOverlayClick = true });
                var ClauseLinkResult = (LegalClause)result;
                if (ClauseLinkResult != null)
                {
                    // add remaining values
                    var articleStatusDetail = ArticleStatusDetails.Where(x => x.Id == ClauseLinkResult.Clause_Status).FirstOrDefault();
                    if (articleStatusDetail != null)
                    {
                        ClauseLinkResult.Clause_Status_Name_En = articleStatusDetail.Name_En;
                        ClauseLinkResult.Clause_Status_Name_Ar = articleStatusDetail.Name_Ar;
                    }
                    ClauseLinkResult.LegislationId = legalLegislation.LegislationId;
                    ClauseLinkResult.Clause_Content = ConvertHtmlConvertToPlainText(ClauseLinkResult.Clause_Content);
                    legalClausesList.Add(ClauseLinkResult);
                    gridSectionClause.Rebind();
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        #region Section / Article Step On Change Click
        protected async Task OnSectionArticlePreviousStepCustom(int currentStepIndex)
        {
            var args = new WizardStepChangeEventArgs()
            {
                IsCancelled = false,
                TargetIndex = currentStepIndex - 1
            };
            OldArticleCount = legalArticlesList.Count();
            legalLegislation.NumberofArticle = legalArticlesList.Count();
            OldClauseCount = legalClausesList.Count();
            legalLegislation.NumberofClause = legalClausesList.Count();
            Value -= 1;
            if (!args.IsCancelled)
            {
                Value = currentStepIndex - 1;
            }
        }
        protected async Task OnSectionArticleStepChangeCustom(int currentStepIndex)
        {
            var args = new WizardStepChangeEventArgs()
            {
                IsCancelled = false,
                TargetIndex = currentStepIndex + 1
            };

            await OnSectionArticleDetailsStepChange(args);

            if (!args.IsCancelled)
            {
                Value = currentStepIndex + 1;
            }
        }
        protected async Task OnSectionArticleDetailsStepChange(WizardStepChangeEventArgs args)
        {
            isBasicStep = true;
            bool valid = ValidateBasicDetails();
            if (valid)
            {
                // first check that if article / clause list have empty record.
                if (TemplateCheckboxSelectedValues.Count() != 0)
                {
                    foreach (var item in TemplateCheckboxSelectedValues)
                    {
                        if (item == (int)LegalTemplateSettingEnum.Articles_with_sections || item == (int)LegalTemplateSettingEnum.Articles_without_sections)
                        {
                            if (legalArticlesList.Count() != 0)
                            {
                                foreach (var itemArticle in legalArticlesList)
                                {
                                    itemArticle.ArticleOrder = DateTime.Now;
                                    await Task.Delay(1000);
                                    if (itemArticle.Start_Date == null)
                                    {
                                        ArticleCounter++;
                                    }
                                }
                            }
                            if (ArticleCounter == 0)
                            {
                                OldArticleCount = legalArticlesList.Count();
                                legalLegislation.NumberofArticle = legalArticlesList.Count();
                                Value += 1;
                            }
                            else
                            {
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Error,
                                    Detail = translationState.Translate("Article_Grid_Fill"),
                                    //Summary = $"" + translationState.Translate("Error"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                                args.IsCancelled = true;
                                ArticleCounter = 0;
                            }
                        }
                        if (item == (int)LegalTemplateSettingEnum.Clauses_with_sections || item == (int)LegalTemplateSettingEnum.Clauses_without_sections)
                        {
                            if (legalClausesList.Count() != 0)
                            {
                                foreach (var itemClause in legalClausesList)
                                {
                                    itemClause.ClauseOrder = DateTime.Now;
                                    await Task.Delay(100);
                                    if (itemClause.Start_Date == null)
                                    {
                                        ClauseCounter++;
                                    }
                                }
                            }
                            if (ClauseCounter == 0)
                            {
                                OldClauseCount = legalClausesList.Count();
                                legalLegislation.NumberofClause = legalClausesList.Count();
                                Value += 1;
                            }
                            else
                            {
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Error,
                                    Detail = translationState.Translate("Clause_Grid_Fill"),
                                    //Summary = $"" + translationState.Translate("Error"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                                args.IsCancelled = true;
                                ClauseCounter = 0;
                            }
                        }
                    }
                }
            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Fill_Required_Fields"),
                    //Summary = $"" + translationState.Translate("Error"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                args.IsCancelled = true;
            }
            var isFormValid = BasicSectionForm.IsValid();
            if (!isFormValid)
            {
                args.IsCancelled = true;
            }
        }
        #endregion

        #region Upload Explanatory Document Button Click and Grid View button click
        protected async Task UploadExplanatoryDocumentButtonClick()
        {
            try
            {
                if (!EditLegislationTrue) // if create legislation form
                {
                    if (explanatoryNote.ExplanatoryNoteId == Guid.Empty)
                    {
                        explanatoryNote = new LegalExplanatoryNote() { ExplanatoryNoteId = Guid.NewGuid() };
                    }
                }
                var dialogResult = await dialogService.OpenAsync<AddLegalLegislationExplanatoryNoteAttachment>(
                    translationState.Translate("Add_Explanatory_Note_Attachment"),
                    new Dictionary<string, object>()
                    {
                       {"ExplanatoryNoteGuidId", explanatoryNote.ExplanatoryNoteId }
                    },
                    new DialogOptions() { CloseDialogOnOverlayClick = true });
                await Task.Delay(200);
                if (dialogResult != null)
                {
                    var resultExplanatoryNote = (Guid)dialogResult;
                    if (resultExplanatoryNote != Guid.Empty)
                    {
                        explanatoryNote.ExplanatoryNoteId = resultExplanatoryNote;
                        var responseExplanatory = await legalLegislationService.GetExplanatoryNoteAttachmentFromTempTableByUsingId(explanatoryNote.ExplanatoryNoteId);
                        //var responseExplanatory = await legalLegislationService.GetAttachmentDetailForGridByUsingLegislationId(explanatoryNote.ExplanatoryNoteId);
                        if (responseExplanatory.IsSuccessStatusCode)
                        {
                            if (!EditLegislationTrue) // if create legislation form
                            {
                                FileExplanatoryNoteTemp = new List<TempAttachementVM>();
                            }
                            foreach (var item in (List<TempAttachementVM>)responseExplanatory.ResultData)
                            {
                                FileExplanatoryNoteTemp.Add(item);
                            }
                            // at the top of attachments list add legislation file.
                            if (FileNameResult != null && FileNameResult.StoragePath != null)
                            {
                                if (FileExplanatoryNoteTemp.Where(x => x.FileName == FileNameResult.FileName).FirstOrDefault() == null)
                                {
#if DEBUG
                                    {
                                        var physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + FileNameResult.StoragePath).Replace(@"\\", @"\");

                                        if (File.Exists(physicalPath))
                                        {
                                            FileExplanatoryNoteTemp.Insert(0, FileNameResult);
                                        }
                                        else
                                        {
                                            notificationService.Notify(new NotificationMessage()
                                            {
                                                Severity = NotificationSeverity.Error,
                                                Detail = translationState.Translate("Authority_Letter_Not_Loaded"),
                                                Style = "position: fixed !important; left: 0; margin: auto; "
                                            });
                                        }
                                    }
#else
                                    {
                                        //var physicalPath = Path.Combine(Directory.GetCurrentDirectory() + attachment.StoragePath).Replace(@"\\", @"\");
                                        // Construct the physical path of the file on the server
                                        var physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + FileNameResult.StoragePath).Replace(@"\\", @"\");
                                        // Remove the wwwroot/Attachments part of the path to get the actual file path
                                        physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
                                        // Create a new HttpClient instance to download the file
                                        using var httpClient = new HttpClient();
                                        var httpresponse = await httpClient.GetAsync(physicalPath);
                                        // Check if the file was downloaded successfully
                                        if (httpresponse.IsSuccessStatusCode)
                                        {
                                            FileExplanatoryNoteTemp.Insert(0, FileNameResult);
                                        }
                                        else
                                        {
                                            notificationService.Notify(new NotificationMessage()
                                            {
                                                Severity = NotificationSeverity.Error,
                                                Detail = translationState.Translate("Authority_Letter_Not_Loaded"),
                                                Style = "position: fixed !important; left: 0; margin: auto; "
                                            });
                                        }
                                    }
#endif
                                }
                                // FileExplanatoryNoteTemp.Add(FileNameResult);
                            }
                            ExplanatoryNoteFileAdded = true;
                            FileGrid.Reset();
                        }
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Something_Went_Wrong"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                    }
                    else
                    {
                        explanatoryNote.ExplanatoryNoteId = Guid.NewGuid();
                    }
                }
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
        protected async Task OnGridViewClick(TempAttachementVM args)
        {
            await CheckPdfViewerIsDocumentEdited();
            var viewfile = args.Guid != null ? FileExplanatoryNoteTemp.Where(x => x.Guid == args.Guid && x.FileName == args.FileName).FirstOrDefault() : FileExplanatoryNoteTemp.Where(x => x.ReferenceGuid == args.ReferenceGuid && x.FileName == args.FileName).FirstOrDefault();
            if (viewfile != null)
            {
                string physicalPath;
#if DEBUG
                {
                    physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + viewfile.StoragePath).Replace(@"\\", @"\");

                }
#else
                {
                    physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + viewfile.StoragePath).Replace(@"\\", @"\");
                    physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
                }
#endif
                string base64String = await DocumentEncryptionService.GetDecryptedDocumentBase64(physicalPath, viewfile.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
                if (!string.IsNullOrEmpty(base64String))
                {
                    DocumentPath = "data:application/pdf;base64," + base64String;
                    var referenceGuidId = viewfile.Guid != null ? viewfile.Guid.ToString() : viewfile.ReferenceGuid;
                    DownloadFileName = viewfile.FileName + "=" + viewfile.Description + "=" + referenceGuidId;
                    await Task.Delay(200);
                    await pdfViewer.LoadAsync(DocumentPath);
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Legislation_Attachment_File_Not_Loaded"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
            }
        }

        private async Task CheckPdfViewerIsDocumentEdited()
        {
            try
            {
                if (pdfViewer.IsDocumentEdited)
                {
                    //var record = pdfViewer.DownloadFileName;
                    string[] strlist = pdfViewer.DownloadFileName.Split("=", StringSplitOptions.None);
                    string filename = null;
                    string description = null;
                    string guidReference = null;
                    for (int i = 0; i < strlist.Length; i++)
                    {
                        if (i == 0)
                        {
                            filename = strlist[i];
                        }
                        else if (i == 1)
                        {
                            description = strlist[i];
                        }
                        else if (i == 2)
                        {
                            guidReference = strlist[i];
                        }
                    }
                    var viewFileDetail = FileExplanatoryNoteTemp.Where(x => (x.Guid != null ? x.Guid == new Guid(guidReference) : x.ReferenceGuid == guidReference) && x.FileName == filename).FirstOrDefault();
                    if (viewFileDetail != null)
                    {
                        // update masked file with actual file
                        viewFileDetail.MaskedFileData = await pdfViewer.GetDocumentAsync();
                        viewFileDetail.IsMaskedAttachment = true;
                        viewFileDetail.UploadFrom = description;
                        if (viewFileDetail.FileNameWithoutTimeStamp == null)
                        {
                            viewFileDetail.FileNameWithoutTimeStamp = viewFileDetail.FileTitle;
                        }
                        var response = await fileUploadService.SaveMaskedDocumentInOriginalDocumentFolderForTemparory(viewFileDetail);
                        if (response.IsSuccessStatusCode)
                        {
                            var resultVM = (TempAttachementVM)response.ResultData;
                            int indexNo = FileExplanatoryNoteTemp.FindIndex(x => x.FileName == filename);
                            if (indexNo != -1)
                            {
                                FileExplanatoryNoteTemp.Insert(indexNo, resultVM);
                                legalLegislation.SelectedSourceDocumentForDelete.Add(viewFileDetail);
                                FileExplanatoryNoteTemp.Remove(viewFileDetail);
                                legalLegislation.MaskedDocumentAttachmentIdList.Add(resultVM.AttachementId);
                                var referenceGuidId = resultVM.Guid != null ? resultVM.Guid.ToString() : resultVM.ReferenceGuid;
                                DownloadFileName = resultVM.FileName + "=" + resultVM.Description + "=" + referenceGuidId;
                                await FileGrid.Reload();
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        protected async Task OnGridDeleteClick(TempAttachementVM args)
        {
            var viewfile = args.Guid != null ? FileExplanatoryNoteTemp.Where(x => x.Guid == args.Guid && x.FileName == args.FileName).FirstOrDefault() : FileExplanatoryNoteTemp.Where(x => x.ReferenceGuid == args.ReferenceGuid && x.FileName == args.FileName).FirstOrDefault();
            string Referenceid = args.Guid == null ? args.ReferenceGuid : args.Guid.ToString();
            var physicalPath = string.Empty;
            if (viewfile != null)
            {
#if DEBUG
                {
                    physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + viewfile.StoragePath).Replace(@"\\", @"\");

                }
#else
                {
                    // Construct the physical path of the file on the server
                     physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + viewfile.StoragePath).Replace(@"\\", @"\");
                    // Remove the wwwroot/Attachments part of the path to get the actual file path
                    physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
                }
#endif
                if (!string.IsNullOrEmpty(physicalPath))
                {
                    if (EditLegislationTrue)
                    {
                        legalLegislation.SelectedSourceDocumentForDelete.Add(viewfile);
                        var nextResult = args.Guid != null ? FileExplanatoryNoteTemp.SkipWhile(x => x.Guid == args.Guid && x.FileName == args.FileName).Skip(1).FirstOrDefault() : FileExplanatoryNoteTemp.SkipWhile(x => x.ReferenceGuid == args.ReferenceGuid && x.FileName == args.FileName).Skip(1).FirstOrDefault();
                        FileExplanatoryNoteTemp.Remove(viewfile);
                        if (nextResult != null)
                        {
                            // next index data file show in pdf viewer
                            var physicalPathNext = Path.Combine(_config.GetValue<string>("dms_file_path") + nextResult.StoragePath).Replace(@"\\", @"\");

                            if (string.IsNullOrEmpty(physicalPathNext))
                            {
                                string base64String = await DocumentEncryptionService.GetDecryptedDocumentBase64(physicalPath, FileNameResult.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
                                DocumentPath = "data:application/pdf;base64," + base64String;
                                var referenceGuidId = nextResult.Guid != null ? nextResult.Guid.ToString() : nextResult.ReferenceGuid;
                                DownloadFileName = nextResult.FileName + "=" + nextResult.Description + "=" + referenceGuidId;
                                await Task.Delay(200);
                                await pdfViewer.LoadAsync(DocumentPath);

                            }
                        }
                        await FileGrid.Reload();
                    }
                    else // in add form
                    {
                        legalLegislation.SelectedSourceDocumentForDelete.Add(viewfile);
                        var nextResult = args.Guid != null ? FileExplanatoryNoteTemp.SkipWhile(x => x.Guid == args.Guid && x.FileName == args.FileName).Skip(1).FirstOrDefault() : FileExplanatoryNoteTemp.SkipWhile(x => x.ReferenceGuid == args.ReferenceGuid && x.FileName == args.FileName).Skip(1).FirstOrDefault();
                        FileExplanatoryNoteTemp.Remove(viewfile);
                        if (nextResult != null)
                        {
                            // next index data file show in pdf viewer
                            var physicalPathNext = Path.Combine(_config.GetValue<string>("dms_file_path") + nextResult.StoragePath).Replace(@"\\", @"\");

                            if (string.IsNullOrEmpty(physicalPathNext))
                            {
                                string base64String = await DocumentEncryptionService.GetDecryptedDocumentBase64(physicalPath, FileNameResult.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
                                DocumentPath = "data:application/pdf;base64," + base64String;
                                var referenceGuidId = nextResult.Guid != null ? nextResult.Guid.ToString() : nextResult.ReferenceGuid;
                                DownloadFileName = nextResult.FileName + "=" + nextResult.Description + "=" + referenceGuidId;
                                await Task.Delay(200);
                                await pdfViewer.LoadAsync(DocumentPath);
                            }
                        }
                        await FileGrid.Reload();
                    }
                    StateHasChanged();
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Legislation_Attachment_File_Not_Loaded"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }

            }
        }
        #endregion

        #region Wizard legislation notes section
        protected async Task OnNotesDetailsStepChangeCustom(int currentStepIndex)
        {
            var args = new WizardStepChangeEventArgs()
            {
                IsCancelled = false,
                TargetIndex = currentStepIndex + 1
            };

            await OnNotesDetailsStepChange(args);

            if (!args.IsCancelled)
            {
                Value = currentStepIndex + 1;
            }
        }
        protected async Task OnNotesDetailsStepChange(WizardStepChangeEventArgs args)
        {
            isBasicStep = true;
            Value += 1;
            var legislationModel = await LegislationAllCollectionsBind();
            NewTest = new MarkupString();
            await LegislationPreviewDesign(legislationModel);
            NewTest = new MarkupString(string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11}", LegalLegislationtype, Head, Referances, Sections, SectionArticals, SectionClasus, Articls, Clasus, Signatures, Source, ExplanatoryNote, Note));
            var isFormValid = BasicSectionForm.IsValid();
            if (!isFormValid)
            {
                args.IsCancelled = true;
            }
        }
        protected async Task<bool> CheckExplanatoryNoteAttachment()
        {
            if (TemplateCheckboxSelectedValues.Count() != 0)
            {
                foreach (var item in TemplateCheckboxSelectedValues)
                {
                    if (item == (int)LegalTemplateSettingEnum.Explanatory_Note)
                    {
                        if (FileExplanatoryNoteTemp.Count() != 0 && explanatoryNote.ExplanatoryNote_Body != null && string.IsNullOrEmpty(HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(explanatoryNote.ExplanatoryNote_Body)).Trim()))
                        {
                            if (await dialogService.Confirm(translationState.Translate("Explanatory_Note_Attachment_Error_Message"), translationState.Translate("Confirm"), new ConfirmOptions()
                            {
                                OkButtonText = translationState.Translate("OK"),
                                CancelButtonText = translationState.Translate("Cancel")
                            }) == true)
                            {
                                FileExplanatoryNoteTemp = new List<TempAttachementVM>();
                                ExplanatoryNoteFileAdded = false;
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return false;
        }
        #endregion

        #region Legislation all collections bind
        protected async Task<LegalLegislation> LegislationAllCollectionsBind()
        {
            if (SelectedTagIds.Count() != 0)
            {
                legalLegislation.LegalLegislationTags = SelectedTagIds;
            }
            if (SignatureDetailForGrid.Count() != 0)
            {
                legalLegislation.LegalLegislationSignatures = SignatureDetailForGrid;
            }
            if (TemplateCheckboxSelectedValues.Count() != 0)
            {
                //TemplateCheckboxSelectedValues = BubbleSortTemplateList(TemplateCheckboxSelectedValues);
                //await Task.Delay(1000);
                //List<int> ObjValues = new List<int>();
                //foreach (var item in TemplateCheckboxSelectedValues)
                //{
                //    ObjValues.Add(item);
                //}
                //legalLegislation.LegalTemplates.SelectedCheckBoxValues = ObjValues;
                foreach (var item in TemplateCheckboxSelectedValues)
                {
                    if (item == (int)LegalTemplateSettingEnum.Publication_details)
                    {
                        if (legalPublicationSourceCS != null && legalPublicationSourceCS.PublicationNameId != 0)
                        {
                            legalPublicationSourceCS.LegislationId = legalLegislation.LegislationId;
                            if (legalLegislation.LegalPublicationSources.Count() == 0)
                            {
                                legalLegislation.LegalPublicationSources.Add(legalPublicationSourceCS);
                            }
                            else
                            {
                                legalLegislation.LegalPublicationSources = new List<LegalPublicationSource>();
                                legalLegislation.LegalPublicationSources.Add(legalPublicationSourceCS);
                            }
                        }
                        else
                        {
                            legalLegislation.LegalPublicationSources = new List<LegalPublicationSource>();
                        }
                    }
                    if (item == (int)LegalTemplateSettingEnum.Articles_with_sections || item == (int)LegalTemplateSettingEnum.Articles_without_sections)
                    {
                        if (legalArticlesList.Count() != 0)
                        {
                            List<Guid>? ObjGuid = new List<Guid>();
                            foreach (var itemList in legalArticlesList)
                            {

                                ObjGuid.Add(itemList.ArticleId);
                            }
                            var num11 = ObjGuid.ToArray();
                            int j = 1;
                            foreach (var itemzz in legalArticlesList)
                            {
                                if (itemzz != legalArticlesList.Last())
                                {
                                    for (int i = j; i < num11.Length;)
                                    {
                                        itemzz.NextArticleId = num11[i];
                                        j++;
                                        break;
                                    }
                                }
                            }
                            legalLegislation.LegalArticles = legalArticlesList;
                        }
                    }
                    if (item == (int)LegalTemplateSettingEnum.Clauses_with_sections || item == (int)LegalTemplateSettingEnum.Clauses_without_sections)
                    {
                        if (legalClausesList.Count() != 0)
                        {
                            legalLegislation.LegalClauses = legalClausesList;
                        }
                    }
                    if (item == (int)LegalTemplateSettingEnum.Explanatory_Note)
                    {
                        if ((FileExplanatoryNoteTemp.Count() != 0)
                            || (FileExplanatoryNoteTemp.Count() != 0 && string.IsNullOrEmpty(explanatoryNote.ExplanatoryNote_Body))
                            || (FileExplanatoryNoteTemp.Count() != 0 && !string.IsNullOrEmpty(explanatoryNote.ExplanatoryNote_Body)))
                        {
                            explanatoryNote.LegislationId = legalLegislation.LegislationId;
                            if (legalLegislation.LegalExplanatoryNotes.Count() == 0)
                            {
                                legalLegislation.LegalExplanatoryNotes.Add(explanatoryNote);
                            }
                            else
                            {
                                legalLegislation.LegalExplanatoryNotes = new List<LegalExplanatoryNote>();
                                legalLegislation.LegalExplanatoryNotes.Add(explanatoryNote);
                            }
                        }
                        else
                        {
                            if (explanatoryNote.ExplanatoryNoteId == Guid.Empty)
                            {
                                explanatoryNote.ExplanatoryNoteId = Guid.NewGuid();
                            }
                            explanatoryNote.LegislationId = legalLegislation.LegislationId;
                            if (legalLegislation.LegalExplanatoryNotes.Count() == 0)
                            {
                                legalLegislation.LegalExplanatoryNotes.Add(explanatoryNote);
                            }
                            else
                            {
                                legalLegislation.LegalExplanatoryNotes = new List<LegalExplanatoryNote>();
                                legalLegislation.LegalExplanatoryNotes.Add(explanatoryNote);
                            }
                        }
                    }
                    if (item == (int)LegalTemplateSettingEnum.Note)
                    {
                        if (!string.IsNullOrEmpty(legalNote.Note_Text))
                        {
                            legalNote.ParentId = legalLegislation.LegislationId;
                            legalNote.Note_Location = legalLegislation.LegislationTitle;
                            legalNote.Note_Date = DateTime.Now;
                            if (legalLegislation.legalNotes.Count() == 0)
                            {
                                legalLegislation.legalNotes.Add(legalNote);
                            }
                            else
                            {
                                legalLegislation.legalNotes = new List<LegalNote>();
                                legalLegislation.legalNotes.Add(legalNote);
                            }
                        }
                    }
                }
            }
            return legalLegislation;
        }
        #endregion

        #region Save Legal Legislation Button Click as a (Partially Completed) flow status
        protected async Task<bool> SaveLegislationButtonClickAsPartiallyCompleted()
        {
            try
            {
                bool valid = ValidateBasicDetails();
                if (valid)
                {
                    if (await dialogService.Confirm(translationState.Translate("Legislation_Save_Popup_Confirm_Message"), translationState.Translate("Confirm"), new ConfirmOptions()
                    {
                        OkButtonText = translationState.Translate("OK"),
                        CancelButtonText = translationState.Translate("Cancel")
                    }) == true)
                    {

                        if (TemplateCheckboxSelectedValues.Count() != 0)
                        {
                            foreach (var item in TemplateCheckboxSelectedValues)
                            {
                                if (item == (int)LegalTemplateSettingEnum.Legislation_Number)
                                {
                                    if (EditLegislationTrue)
                                    {
                                        if (legalLegislation.EditCaseLegislationNumber != legalLegislation.Legislation_Number)
                                        {
                                            var resultLegNumber = await legalLegislationService.CheckLegislationNumberDuplication(legalLegislation.Legislation_Type, legalLegislation.Legislation_Number);
                                            if (resultLegNumber.IsSuccessStatusCode)
                                            {
                                                notificationService.Notify(new NotificationMessage()
                                                {
                                                    Severity = NotificationSeverity.Error,
                                                    Detail = translationState.Translate("Legislation_Number_Duplication_Message"),
                                                    //Summary = $"" + translationState.Translate("Error"),
                                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                                });
                                                return false;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        var resultLegNumber = await legalLegislationService.CheckLegislationNumberDuplication(legalLegislation.Legislation_Type, legalLegislation.Legislation_Number);
                                        if (resultLegNumber.IsSuccessStatusCode)
                                        {
                                            notificationService.Notify(new NotificationMessage()
                                            {
                                                Severity = NotificationSeverity.Error,
                                                Detail = translationState.Translate("Legislation_Number_Duplication_Message"),
                                                //Summary = $"" + translationState.Translate("Error"),
                                                Style = "position: fixed !important; left: 0; margin: auto; "
                                            });
                                            return false;
                                        }
                                    }

                                }
                                if (item == (int)LegalTemplateSettingEnum.Introduction_with_relation || item == (int)LegalTemplateSettingEnum.Introduction_without_relation)
                                {
                                    if (legalLegislation.Introduction != null && !string.IsNullOrEmpty(HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(legalLegislation.Introduction)).Trim()))
                                    {

                                    }
                                    else
                                    {
                                        LegislationFlowStatusCheck = true;
                                    }
                                }
                                if (item == (int)LegalTemplateSettingEnum.Legislation_Subject)
                                {
                                    if (string.IsNullOrEmpty(legalLegislation.LegislationTitle))
                                    {
                                        LegislationFlowStatusCheck = true;
                                    }
                                }
                                if (item == (int)LegalTemplateSettingEnum.Articles_with_sections || item == (int)LegalTemplateSettingEnum.Articles_without_sections)
                                {
                                    if (legalLegislation.LegalArticles.Count() == 0)
                                    {
                                        LegislationFlowStatusCheck = true;
                                    }
                                }
                                if (item == (int)LegalTemplateSettingEnum.Clauses_with_sections || item == (int)LegalTemplateSettingEnum.Clauses_without_sections)
                                {
                                    if (legalLegislation.LegalClauses.Count() == 0)
                                    {
                                        LegislationFlowStatusCheck = true;
                                    }
                                }
                                if (item == (int)LegalTemplateSettingEnum.Explanatory_Note)
                                {
                                    if (legalLegislation.LegalExplanatoryNotes.Count() == 0)
                                    {
                                        LegislationFlowStatusCheck = true;
                                    }
                                    else
                                    {
                                        foreach (var itemExplanatory in legalLegislation.LegalExplanatoryNotes)
                                        {
                                            if (itemExplanatory.ExplanatoryNote_Body != null && !string.IsNullOrEmpty(HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(itemExplanatory.ExplanatoryNote_Body)).Trim()))
                                            {

                                            }
                                            else
                                            {
                                                LegislationFlowStatusCheck = true;
                                            }
                                        }
                                    }
                                }
                                if (item == (int)LegalTemplateSettingEnum.Note)
                                {
                                    if (legalLegislation.legalNotes.Count() == 0)
                                    {
                                        LegislationFlowStatusCheck = true;
                                    }
                                }
                            }
                        }

                        var resultLegalLegislation = await LegislationAllCollectionsBind();

                        if (LegislationFlowStatusCheck)
                        {
                            resultLegalLegislation.Legislation_Flow_Status = (int)LegislationFlowStatusEnum.PartiallyCompleted;
                        }
                        else
                        {
                            resultLegalLegislation.Legislation_Flow_Status = (int)LegislationFlowStatusEnum.New;
                        }
                        var resultLegislation = new ApiCallResponse();
                        if (EditLegislationTrue)
                        {
                            resultLegislation = await legalLegislationService.UpdateLegalLegislation(resultLegalLegislation);

                        }
                        else
                        {
                            resultLegislation = await legalLegislationService.SaveLegalLegislation(resultLegalLegislation);
                        }

                        if (resultLegislation.IsSuccessStatusCode)
                        {
                            // If the HTTP response from the server indicates success, proceed with saving attachments to a document.
                            var returnResult = (bool)resultLegislation.ResultData;
                            var resultAttachment = await LegislationAttachmentSaveFromTempAttachementToUploadedDocument(resultLegalLegislation);
                            if (resultAttachment)
                            {
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Success,
                                    Detail = translationState.Translate("legislation_Success_Message"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                            }
                        }
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Something_Went_Wrong"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                        await Task.Delay(200);
                        navigationManager.NavigateTo("/legallegislation-list");
                        return true;
                    }
                }
                else
                {
                    if (ShowPublicationStartEndPageCompareMessage)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Publication_Page_Start_End_Check"),
                            //Summary = $"" + translationState.Translate("Error"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                    else
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Fill_Required_Fields"),
                            //Summary = $"" + translationState.Translate("Error"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                }
                return false;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        private async Task<bool> LegislationAttachmentSaveFromTempAttachementToUploadedDocument(LegalLegislation resultLegislationObject)
        {
            try
            {
                await CheckPdfViewerIsDocumentEdited();
                resultLegislationObject.AttachedDocumentList = FileExplanatoryNoteTemp;
                // Save Temp Attachement To Uploaded Documents
                var docResponse = await fileUploadService.LegislationAttachmentSaveFromTempAttachementToUploadedDocument(resultLegislationObject);
                // If the file upload service returns an error, show a notification message
                if (!docResponse.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Attachment_Save_Failed"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    await Task.Delay(200);
                    navigationManager.NavigateTo("/legallegislation-list");
                    return false;
                }
                return true;
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }
        #endregion

        #region Save Legal Legislation Button Click as a (New) flow status
        protected async Task SaveLegalLegislationButtonClick()
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Legislation_Save_Popup_Confirm_Message"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    var resultLegalLegislation = await LegislationAllCollectionsBind();
                    if (TemplateCheckboxSelectedValues.Count() != 0)
                    {
                        foreach (var item in TemplateCheckboxSelectedValues)
                        {
                            if (item == (int)LegalTemplateSettingEnum.Introduction_with_relation || item == (int)LegalTemplateSettingEnum.Introduction_without_relation)
                            {
                                if (resultLegalLegislation.Introduction != null && !string.IsNullOrEmpty(HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(resultLegalLegislation.Introduction)).Trim()))
                                {

                                }
                                else
                                {
                                    LegislationFlowStatusCheck = true;
                                }
                            }
                            if (item == (int)LegalTemplateSettingEnum.Articles_with_sections || item == (int)LegalTemplateSettingEnum.Articles_without_sections)
                            {
                                if (legalLegislation.LegalArticles.Count() == 0)
                                {
                                    LegislationFlowStatusCheck = true;
                                }
                            }
                            if (item == (int)LegalTemplateSettingEnum.Clauses_with_sections || item == (int)LegalTemplateSettingEnum.Clauses_without_sections)
                            {
                                if (legalLegislation.LegalClauses.Count() == 0)
                                {
                                    LegislationFlowStatusCheck = true;
                                }
                            }
                            if (item == (int)LegalTemplateSettingEnum.Explanatory_Note)
                            {
                                if (legalLegislation.LegalExplanatoryNotes.Count() == 0)
                                {
                                    LegislationFlowStatusCheck = true;
                                }
                                else
                                {
                                    foreach (var itemExplanatory in legalLegislation.LegalExplanatoryNotes)
                                    {
                                        if (itemExplanatory.ExplanatoryNote_Body != null && !string.IsNullOrEmpty(HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(itemExplanatory.ExplanatoryNote_Body)).Trim()))
                                        {

                                        }
                                        else
                                        {
                                            LegislationFlowStatusCheck = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (LegislationFlowStatusCheck)
                    {
                        resultLegalLegislation.Legislation_Flow_Status = (int)LegislationFlowStatusEnum.PartiallyCompleted;
                    }
                    else
                    {
                        resultLegalLegislation.Legislation_Flow_Status = (int)LegislationFlowStatusEnum.New;
                    }
                    var resultLegislation = await legalLegislationService.SaveLegalLegislation(resultLegalLegislation);
                    if (resultLegislation.IsSuccessStatusCode)
                    {
                        // If the HTTP response from the server indicates success, proceed with saving attachments to a document.
                        var returnResult = (bool)resultLegislation.ResultData;
                        var resultAttachment = await LegislationAttachmentSaveFromTempAttachementToUploadedDocument(resultLegalLegislation);
                        if (resultAttachment)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("legislation_Success_Message"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            await Task.Delay(200);
                            navigationManager.NavigateTo("/legallegislation-list");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Submit Legal Legislation Button Click as a (In Review) flow status
        protected async Task SubmitLegalLegislationButtonClick()
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Legislation_Submit_Popup_Confirm_Message"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    var resultLegalLegislation = await LegislationAllCollectionsBind();
                    if (TemplateCheckboxSelectedValues.Count() != 0)
                    {
                        foreach (var item in TemplateCheckboxSelectedValues)
                        {
                            if (item == (int)LegalTemplateSettingEnum.Introduction_with_relation || item == (int)LegalTemplateSettingEnum.Introduction_without_relation)
                            {
                                if (resultLegalLegislation.Introduction != null && !string.IsNullOrEmpty(HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(resultLegalLegislation.Introduction)).Trim()))
                                {

                                }
                                else
                                {
                                    LegislationFlowStatusCheck = true;
                                }
                            }
                            if (item == (int)LegalTemplateSettingEnum.Articles_with_sections || item == (int)LegalTemplateSettingEnum.Articles_without_sections)
                            {
                                if (legalLegislation.LegalArticles.Count() == 0)
                                {
                                    LegislationFlowStatusCheck = true;
                                }
                            }
                            if (item == (int)LegalTemplateSettingEnum.Clauses_with_sections || item == (int)LegalTemplateSettingEnum.Clauses_without_sections)
                            {
                                if (legalLegislation.LegalClauses.Count() == 0)
                                {
                                    LegislationFlowStatusCheck = true;
                                }
                            }
                            if (item == (int)LegalTemplateSettingEnum.Explanatory_Note)
                            {
                                if (legalLegislation.LegalExplanatoryNotes.Count() == 0)
                                {
                                    LegislationFlowStatusCheck = true;
                                }
                                else
                                {
                                    foreach (var itemExplanatory in legalLegislation.LegalExplanatoryNotes)
                                    {
                                        if (itemExplanatory.ExplanatoryNote_Body != null && !string.IsNullOrEmpty(HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(itemExplanatory.ExplanatoryNote_Body)).Trim()))
                                        {

                                        }
                                        else
                                        {
                                            LegislationFlowStatusCheck = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (LegislationFlowStatusCheck)
                    {
                        resultLegalLegislation.Legislation_Flow_Status = (int)LegislationFlowStatusEnum.PartiallyCompleted;
                    }
                    else
                    {
                        resultLegalLegislation.Legislation_Flow_Status = (int)LegislationFlowStatusEnum.InReview;
                    }
                    if (legalLegislation.Legislation_Flow_Status == (int)LegislationFlowStatusEnum.InReview)
                    {
                        var response = await workflowService.GetActiveWorkflows((int)WorkflowModuleEnum.LDSDocument, (int)WorkflowModuleTriggerEnum.UserSubmitsDocument, null, (int)WorkflowSubModuleEnum.LegalLegislations);
                        if (response.IsSuccessStatusCode)
                        {
                            activeworkflowlist = (IEnumerable<WorkflowVM>)response.ResultData;
                            if (activeworkflowlist?.Count() > 0)
                            {
                                var resultLegislation = await legalLegislationService.SaveLegalLegislation(resultLegalLegislation);
                                if (resultLegislation.IsSuccessStatusCode)
                                {
                                    // If the HTTP response from the server indicates success, proceed with saving attachments to a document.
                                    SaveLegislationResponseResult = (bool)resultLegislation.ResultData;
                                    // Get a list of IDs for the explanatory notes and the legislation itself.
                                    var resultAttachment = await LegislationAttachmentSaveFromTempAttachementToUploadedDocument(resultLegalLegislation);
                                }
                            }
                            else
                            {
                                SaveLegislationResponseResult = false;
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Error,
                                    Detail = translationState.Translate("No_Active_Workflow"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                                return;
                            }
                        }
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("No_Active_Workflow"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }

                        if (SaveLegislationResponseResult)
                        {
                            legalLegislation.SenderEmail = loginState.UserDetail.UserName;
                            await workflowService.AssignWorkflowActivity(activeworkflowlist.FirstOrDefault(), legalLegislation, (int)WorkflowModuleEnum.LDSDocument, (int)WorkflowModuleTriggerEnum.UserSubmitsDocument, null);
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("legislation_Success_Message"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            await Task.Delay(200);
                            navigationManager.NavigateTo("/legallegislation-list");
                        }
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Something_Went_Wrong"),
                                //Summary = translationState.Translate("Error"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });

                        }

                    }

                    else
                    {
                        var resultLegislation = await legalLegislationService.SaveLegalLegislation(resultLegalLegislation);
                        if (resultLegislation.IsSuccessStatusCode)
                        {
                            SaveLegislationResponseResult = (bool)resultLegislation.ResultData;

                            var resultAttachment = await LegislationAttachmentSaveFromTempAttachementToUploadedDocument(resultLegalLegislation);
                            if (resultAttachment)
                            {
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Success,
                                    Detail = translationState.Translate("legislation_Success_Message"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                                await Task.Delay(200);
                                navigationManager.NavigateTo("/legallegislation-list");
                            }
                        }
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Something_Went_Wrong"),
                                //Summary = translationState.Translate("Error"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });

                        }

                    }

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Update Legal Legislation Button Click as a (In Review) flow status
        protected async Task UpdateLegalLegislationButtonClick(bool args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Legislation_Update_Popup_Confirm_Message"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    var resultLegalLegislation = await LegislationAllCollectionsBind();
                    if (TemplateCheckboxSelectedValues.Count() != 0)
                    {
                        foreach (var item in TemplateCheckboxSelectedValues)
                        {
                            if (item == (int)LegalTemplateSettingEnum.Introduction_with_relation || item == (int)LegalTemplateSettingEnum.Introduction_without_relation)
                            {
                                if (resultLegalLegislation.Introduction != null && !string.IsNullOrEmpty(HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(resultLegalLegislation.Introduction)).Trim()))
                                {

                                }
                                else
                                {
                                    LegislationFlowStatusCheck = true;
                                }
                            }
                            if (item == (int)LegalTemplateSettingEnum.Articles_with_sections || item == (int)LegalTemplateSettingEnum.Articles_without_sections)
                            {
                                if (legalLegislation.LegalArticles.Count() == 0)
                                {
                                    LegislationFlowStatusCheck = true;
                                }
                            }
                            if (item == (int)LegalTemplateSettingEnum.Clauses_with_sections || item == (int)LegalTemplateSettingEnum.Clauses_without_sections)
                            {
                                if (legalLegislation.LegalClauses.Count() == 0)
                                {
                                    LegislationFlowStatusCheck = true;
                                }
                            }
                            if (item == (int)LegalTemplateSettingEnum.Explanatory_Note)
                            {
                                if (legalLegislation.LegalExplanatoryNotes.Count() == 0)
                                {
                                    LegislationFlowStatusCheck = true;
                                }
                                else
                                {
                                    foreach (var itemExplanatory in legalLegislation.LegalExplanatoryNotes)
                                    {
                                        if (itemExplanatory.ExplanatoryNote_Body != null && !string.IsNullOrEmpty(HttpUtility.HtmlDecode(HttpUtility.HtmlDecode(itemExplanatory.ExplanatoryNote_Body)).Trim()))
                                        {

                                        }
                                        else
                                        {
                                            LegislationFlowStatusCheck = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    if (LegislationFlowStatusCheck)
                    {
                        resultLegalLegislation.Legislation_Flow_Status = (int)LegislationFlowStatusEnum.PartiallyCompleted;
                    }
                    else
                    {
                        if (args == true) // if submit button click
                        {
                            resultLegalLegislation.Legislation_Flow_Status = (int)LegislationFlowStatusEnum.InReview;
                        }
                        else // if save button click
                        {
                            resultLegalLegislation.Legislation_Flow_Status = (int)LegislationFlowStatusEnum.New;
                        }
                    }
                    if (legalLegislation.Legislation_Flow_Status == (int)LegislationFlowStatusEnum.InReview)
                    {
                        var response = await workflowService.GetActiveWorkflows((int)WorkflowModuleEnum.LDSDocument, (int)WorkflowModuleTriggerEnum.UserSubmitsDocument, null, (int)WorkflowSubModuleEnum.LegalLegislations);
                        if (response.IsSuccessStatusCode)
                        {
                            activeworkflowlist = (List<WorkflowVM>)response.ResultData;
                            if (activeworkflowlist?.Count() > 0)
                            {
                                var resultLegislation = await legalLegislationService.UpdateLegalLegislation(resultLegalLegislation);
                                if (resultLegislation.IsSuccessStatusCode)
                                {
                                    UpdateLegislationResponseResult = (bool)resultLegislation.ResultData;
                                    var resultAttachment = await LegislationAttachmentSaveFromTempAttachementToUploadedDocument(resultLegalLegislation);
                                }
                            }
                            else
                            {
                                UpdateLegislationResponseResult = false;
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Error,
                                    Detail = "No Active workflows detected. Please contact administrator.",
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                                return;
                            }
                        }
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("No_Active_Workflow"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                        if (UpdateLegislationResponseResult)
                        {
                            legalLegislation.SenderEmail = loginState.UserDetail.UserName;
                            await workflowService.AssignWorkflowActivity(activeworkflowlist.FirstOrDefault(), legalLegislation, (int)WorkflowModuleEnum.LDSDocument, (int)WorkflowModuleTriggerEnum.UserSubmitsDocument, null);

                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("legislation_Success_Message"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            await Task.Delay(200);
                            navigationManager.NavigateTo("/legallegislation-list");
                        }
                    }

                    else
                    {
                        var resultLegislation = await legalLegislationService.UpdateLegalLegislation(resultLegalLegislation);
                        if (resultLegislation.IsSuccessStatusCode)
                        {
                            UpdateLegislationResponseResult = (bool)resultLegislation.ResultData;
                            var resultAttachment = await LegislationAttachmentSaveFromTempAttachementToUploadedDocument(resultLegalLegislation);
                            if (resultAttachment)
                            {
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Success,
                                    Detail = translationState.Translate("legislation_Success_Message"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                                await Task.Delay(200);
                                navigationManager.NavigateTo("/legallegislation-list");
                            }
                        }
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Something_Went_Wrong"),
                                //Summary = translationState.Translate("Error"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Wizard Finish
        protected void OnWizardFinish()
        {
            ShowWizard = false;
        }
        #endregion

        #region Variables preview
        public string Referances { get; set; }
        public string Head { get; set; }
        public string LegalLegislationtype { get; set; }
        public string Sections { get; set; }
        public string SectionArticals { get; set; }
        public string SectionClasus { get; set; }
        public string Articls { get; set; }
        public string Clasus { get; set; }
        public string Signatures { get; set; }
        public string Source { get; set; }
        public string ExplanatoryNote { get; set; }
        public string Note { get; set; }
        public MarkupString NewTest { get; set; } = new MarkupString();
        private string BodyVales;
        #endregion

        #region Legislation Preview
        private async Task LegislationPreviewDesign(LegalLegislation legislationModel)
        {
            Head = string.Empty;
            LegalLegislationtype = string.Empty;
            Referances = string.Empty;
            Sections = string.Empty;
            SectionArticals = string.Empty;
            SectionClasus = string.Empty;
            Articls = string.Empty;
            Clasus = string.Empty;
            Signatures = string.Empty;
            ExplanatoryNote = string.Empty;
            Note = string.Empty;
            Source = string.Empty;


            var legislationtype = GetLegislationTypeDetails.Where(x => x.Id == legalLegislation.Legislation_Type).FirstOrDefault();



            if (legalLegislation != null)
            {
                if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                {
                    if (legalLegislation.Legislation_Type.ToString() != null && legalLegislation.Legislation_Number.ToString() != null && legalLegislation.Legislation_Type.ToString() != null && legalLegislation.IssueDate != null)
                    {
                        LegalLegislationtype = @"<div class=""TitleCenter"" style=""background-color:#FFF"" > <h6 style=""text-align: center;""> <p>" + legislationtype.Name_En + @" " + legalLegislation.Legislation_Number + @" " + legalLegislation.IssueDate.Value.ToString("dd/MM/yyyy") + @"</p></h6>";
                    }
                }
                else
                {
                    if (legalLegislation.Legislation_Type.ToString() != null && legalLegislation.Legislation_Number.ToString() != null && legalLegislation.Legislation_Type.ToString() != null && legalLegislation.IssueDate != null)
                    {
                        LegalLegislationtype = @"<div class=""TitleCenter"" style=""background-color:#FFF"" > <h6 style=""text-align: center;""> <p>" + legislationtype.Name_Ar + @" " + legalLegislation.Legislation_Number + @" " + legalLegislation.IssueDate.Value.ToString("dd/MM/yyyy") + @"</p></h6>";
                    }
                }
                if (Head.ToString() != null)
                {
                    Head = @"<div class=""Container"" style=""background-color:#FFF"" > <h1 style=""text-align: center;""><strong>" + legalLegislation.LegislationTitle + @"</strong></h1><p>";
                }
                if (legalLegislation.Introduction.ToString() != null)
                {
                    Referances += @"<div class=""row text""><div class=""col-md-12""><p Class=""introduction"">" + legalLegislation.Introduction + @"</div></div></p>";
                }
                if (legislationModel.LegalSections.Count() > 0)
                {
                    foreach (var item in legislationModel.LegalSections)
                    {
                        Sections += @"<div class=""row text""><div class=""col-md-12""><div class=""LegislationSection""> 
                    <div class=""LegislationNumber""> " + @"" + item.SectionTitle + @" 
                    </br>";
                        item.LegalArticlesUnderSection = (List<LegalArticle>)item.LegalArticlesUnderSection;
                        foreach (var itemSectionArticals in item.LegalArticlesUnderSection)
                        {
                            SectionArticals += @"<b class=""SectionArticalstitle"">
                            " + itemSectionArticals.Article_Title + @"</b><br/>
                            " + itemSectionArticals.Article_Text + @"</div></div>";
                        }
                        foreach (var itemSectionClasus in item.LegalClauseUnderSection)
                        {
                            SectionClasus += @"<b class=""SectionArticalstitle"">
                            " + itemSectionClasus.Clause_Name + @"</b><br/>
                            " + itemSectionClasus.Clause_Content + @"</div></div>";
                        }
                    }
                }
                if (legislationModel.LegalArticles.Count() > 0)
                {
                    foreach (var item in legislationModel.LegalArticles)
                    {
                        Articls += @"<div class=""row Atext""><div class=""col-md-12""><div class=""SectionArticalstitle"">" + item.Article_Title + @"</div>
                      " + item.Article_Text + @"</div></div>";
                    }
                }
                if (legislationModel.LegalClauses.Count() > 0)
                {
                    foreach (var item in legislationModel.LegalClauses)
                    {
                        Clasus += @" <div class=""row ""><div class=""col-md-12""><div class=""SectionArticalstitle"">" + item.Clause_Name + @"</div>
                      " + item.Clause_Content + @"</div></div>";
                    }
                }


                foreach (var item in legislationModel.LegalLegislationSignatures)
                {
                    Signatures += @"<div class=""row Signaturetext""><div class=""col-md-12"">
                        <p class=""legalSignature"">" + item.Full_Name + @" </p>
                        <p class=""legalSignatureTitle""> " + item.Job_Title + @" </p></div></div>";


                }
                if (legislationModel.LegalExplanatoryNotes.Count() > 0)
                {
                    foreach (var item in legislationModel.LegalExplanatoryNotes)
                    {
                        ExplanatoryNote += @"" + translationState.Translate("Explanatory_Note") + @" <p class=""legalSignature"">" + item.ExplanatoryNote_Body + @"</p> ";
                    }
                }
                if (legislationModel.legalNotes.Count() > 0)
                {
                    foreach (var item in legislationModel.legalNotes)
                    {
                        Note += @"" + translationState.Translate("Legislation_Notes") + @"<p class=""legalSignature"">" + item.Note_Text + @"</p> ";
                    }
                }
                if (legislationModel.LegalPublicationSources.Count() > 0 && legalPublicationSourceCS.PublicationNameId != 0)
                {
                    foreach (var item in legislationModel.LegalPublicationSources)
                    {
                        Source += @"<div class=""row"" Style=""word-break: break-word;""><div class=""col-md-12""><div class=""col-md-6 signatureFooter""><p>" + translationState.Translate("Legislation_Preview_Footer1") + @"
                                    " + item.Issue_Number + @"</p>
                                    <p>" + translationState.Translate("at") + @": " + item.PublicationDate.ToString("dd/MM/yyyy") + @"</p></div></div> 
                                    </div>";
                    }
                }
            }
        }
        #endregion

        #region Cancel wizard button click
        protected async Task CancelLegislationForm()
        {
            if (await dialogService.Confirm(translationState.Translate("Legislation_Cancel"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                var response = await legalLegislationService.DeleteAttachmentFromTempTable(legalLegislation.LegislationId);
                if (response.IsSuccessStatusCode)
                {
                    navigationManager.NavigateTo("legallegislation-list");
                }
                navigationManager.NavigateTo("legallegislation-list");
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
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion

        #region Relation document detail popup open
        [JSInvokable]
        public async Task LegislationRelationDetailByUsingReferenceId(Guid id, string modelName)
        {
            try
            {
                StateHasChanged();
                var result = await dialogService.OpenAsync<LegallLegislationViewDetail>(translationState.Translate("Legal_legislation_Details"),
                               new Dictionary<string, object>()
                               {
                                   { "LegislationId", id.ToString() },
                                   { "modelName", modelName }
                               },
                               new DialogOptions() { Width = "100% !important", CloseDialogOnOverlayClick = true });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Function String to int
        protected async Task ParseStringToIntPageStart(string value)
        {
            legalPublicationSourceCS.Page_Start = string.IsNullOrEmpty(value) ? 0 : Int32.Parse(value);
        }
        protected async Task ParseStringToIntPageEnd(string value)
        {
            legalPublicationSourceCS.Page_End = string.IsNullOrEmpty(value) ? 0 : Int32.Parse(value);
        }
        protected async Task DocumentLoaded(LoadEventArgs args)
        {
            await JSRuntime.InvokeVoidAsync("addRotateButtonToPdfToolbar");
        }
        #endregion

    }
}

