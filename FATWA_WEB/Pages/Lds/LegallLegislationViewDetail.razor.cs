using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_GENERAL.Helper;
using FATWA_WEB.Extensions;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Syncfusion.Blazor.PdfViewer;
using Syncfusion.Blazor.PdfViewerServer;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Telerik.Blazor.Components;

namespace FATWA_WEB.Pages.Lds
{
    public partial class LegallLegislationViewDetail : ComponentBase
    {

        #region Parameter

        [Parameter]
        public dynamic LegislationId { get; set; }
        [Parameter]
        public dynamic RedirectFromPage { get; set; }
        [Parameter]
        public dynamic modelName { get; set; }
        public dynamic RedirectToPage { get; set; }
        public string RedirectFromTranslation { get; set; }

        #endregion

        #region list

        protected List<LegalLegislationReference> legalLegislationReferencesGet;
        protected List<LegalPublicationSource> legalPublicationSourcesGet;
        protected List<LegalExplanatoryNote> legalExplanatoryNotesGet;
        protected List<LegalNote> legalNotesGet;
        protected List<LegalArticle> legalArticleGet;
        protected List<LegalClause> legalClausesGet;
        protected List<LegalSection> legalSectionGet;
        protected List<LegalArticle> legalSectionArticalGet;
        protected List<LegalSectionArticalVM> legalSectionArticalGetVM;
        protected List<LegalLegislationSignature> legalLegislationSignatureGet;
        protected List<LegalLegislationType> legalLegislationTypes;
        protected List<LegalLegislationArticleEffectHistory> legalLegislationArticleEffectHistorys { get; set; }
        protected RadzenDataGrid<LegalLegislationSignature> grid = new RadzenDataGrid<LegalLegislationSignature>();
        protected RadzenDataGrid<LegalArticalSectionVM> gridSectionArtical = new RadzenDataGrid<LegalArticalSectionVM>();
        protected RadzenDataGrid<LegalClausesSectionVM> gridSectionClauses = new RadzenDataGrid<LegalClausesSectionVM>();
        protected RadzenDataGrid<LegalLegislationCommentVM> gridComments = new RadzenDataGrid<LegalLegislationCommentVM>();
        protected LegalLegislation legalLegislationGet { get; set; }
        LegalLegislationsVM legislationsVM = new LegalLegislationsVM();
        public int count { get; set; } = 0;
        public TelerikPdfViewer PdfViewerRef { get; set; }
        public byte[] FileData { get; set; }
        //public TempAttachement FileNameResult { get; set; }
        public TempAttachementVM FileNameResult { get; set; }
        public SfPdfViewerServer pdfViewer;
        public PdfViewerToolbarSettings ToolbarSettings = new PdfViewerToolbarSettings()
        {
            ToolbarItems = new List<Syncfusion.Blazor.PdfViewer.ToolbarItem>()
            {
                Syncfusion.Blazor.PdfViewer.ToolbarItem.MagnificationTool,
             //   Syncfusion.Blazor.PdfViewer.ToolbarItem.SelectionTool,
                Syncfusion.Blazor.PdfViewer.ToolbarItem.SearchOption,
                Syncfusion.Blazor.PdfViewer.ToolbarItem.CommentTool,
                Syncfusion.Blazor.PdfViewer.ToolbarItem.PrintOption,
                Syncfusion.Blazor.PdfViewer.ToolbarItem.MagnificationTool,
                Syncfusion.Blazor.PdfViewer.ToolbarItem.DownloadOption,
                Syncfusion.Blazor.PdfViewer.ToolbarItem.PrintOption,
                Syncfusion.Blazor.PdfViewer.ToolbarItem.PageNavigationTool,
                Syncfusion.Blazor.PdfViewer.ToolbarItem.PanTool,
              //  Syncfusion.Blazor.PdfViewer.ToolbarItem.SelectionTool,
               // Syncfusion.Blazor.PdfViewer.ToolbarItem.UndoRedoTool 
            }
        };
        public bool VisibleSourceDocument { get; set; } = false;

        #endregion

        #region Model full property Instance
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }
        LegalLegislationDetailVM _legalLegislationVM;
        protected LegalLegislationDetailVM legalLegislationVM
        {
            get
            {
                return _legalLegislationVM;
            }
            set
            {
                if (!object.Equals(_legalLegislationVM, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "LegalLegislationDetailVM", NewValue = value, OldValue = _legalLegislationVM };
                    _legalLegislationVM = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        LegalSectionArticalVM _legalSection;
        protected LegalSectionArticalVM legalSection
        {
            get
            {
                return _legalSection;
            }
            set
            {
                if (!object.Equals(_legalSection, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "LegalSectionArticalVM", NewValue = value, OldValue = _legalSection };
                    _legalSection = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        #endregion

        #region Grid Search
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

        #region Legal Legislation Detail View Grid Load Properties Load
        LegalPublicationSourceVM _getLegalPublicationSourceResult;
        protected LegalPublicationSourceVM getLegalPublicationSourceResult
        {
            get
            {
                return _getLegalPublicationSourceResult;
            }
            set
            {
                if (!object.Equals(_getLegalPublicationSourceResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getLegalPublicationSourceResult", NewValue = value, OldValue = _getLegalPublicationSourceResult };
                    _getLegalPublicationSourceResult = value;
                    Reload();
                }
            }
        }
        LegalLegislation _legalLegislation;
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
        IEnumerable<LegalArticalSectionVM> _getLegalArticalSectionResultVM;
        protected IEnumerable<LegalArticalSectionVM> getLegalArticalSectionResultVM
        {
            get
            {
                return _getLegalArticalSectionResultVM;
            }
            set
            {
                if (!object.Equals(_getLegalArticalSectionResultVM, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getLegalArticalSectionResultVM", NewValue = value, OldValue = _getLegalArticalSectionResultVM };
                    _getLegalArticalSectionResultVM = value;
                    Reload();
                }
            }
        }
        IEnumerable<LegalClausesSectionVM> _getLegalClausesSectionResultVM;
        protected IEnumerable<LegalClausesSectionVM> getLegalClausesSectionResultVM
        {
            get
            {
                return _getLegalClausesSectionResultVM;
            }
            set
            {
                if (!object.Equals(_getLegalArticalSectionResultVM, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getLegalClausesSectionResultVM", NewValue = value, OldValue = _getLegalClausesSectionResultVM };
                    _getLegalClausesSectionResultVM = value;
                    Reload();
                }
            }
        }

        IEnumerable<LegalLegislationSignature> _getLegalLegislationSignatureResult;
        protected IEnumerable<LegalLegislationSignature> getLegalLegislationSignatureResult
        {
            get
            {
                return _getLegalLegislationSignatureResult;
            }
            set
            {
                if (!object.Equals(_getLegalLegislationSignatureResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getLegalLegislationSignatureResult", NewValue = value, OldValue = _getLegalLegislationSignatureResult };
                    _getLegalLegislationSignatureResult = value;
                    Reload();
                }
            }
        }
        LegalExplanatoryNote _getLegalExplanatoryNoteResult;
        protected LegalExplanatoryNote getLegalExplanatoryNoteResult
        {
            get
            {
                return _getLegalExplanatoryNoteResult;
            }
            set
            {
                if (!object.Equals(_getLegalExplanatoryNoteResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getLegalExplanatoryNoteResult", NewValue = value, OldValue = _getLegalExplanatoryNoteResult };
                    _getLegalExplanatoryNoteResult = value;
                    Reload();
                }
            }
        }
        IEnumerable<LegalLegislationReference> _getLegalLegislationReferenceResult;
        protected IEnumerable<LegalLegislationReference> getLegalLegislationReferenceResult
        {
            get
            {
                return _getLegalLegislationReferenceResult;
            }
            set
            {
                if (!object.Equals(_getLegalLegislationReferenceResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getLegalLegislationReferenceResult", NewValue = value, OldValue = _getLegalLegislationReferenceResult };
                    _getLegalLegislationReferenceResult = value;
                    Reload();
                }
            }
        }
        LegalNote _getLegalNoteResult;
        protected LegalNote getLegalNoteResult
        {
            get
            {
                return _getLegalNoteResult;
            }
            set
            {
                if (!object.Equals(_getLegalNoteResult, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getLegalNoteResult", NewValue = value, OldValue = _getLegalNoteResult };
                    _getLegalNoteResult = value;
                    Reload();
                }
            }
        }
        IEnumerable<LegislationVM> _getLegislationResultVM;
        protected IEnumerable<LegislationVM> getLegislationResultVM
        {
            get
            {
                return _getLegislationResultVM;
            }
            set
            {
                if (!object.Equals(_getLegislationResultVM, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "getLegislationResultVM", NewValue = value, OldValue = _getLegislationResultVM };
                    _getLegislationResultVM = value;
                    Reload();
                }
            }
        }
        #endregion

        #region Variables 
        public string referances { get; set; }
        public string LegalLegislationtype { get; set; }
        public string head { get; set; }
        public string sections { get; set; }
        public string sectionArticals { get; set; }
        public string sectionClauses { get; set; }
        public string clauses { get; set; }
        public string articls { get; set; }
        public string signatures { get; set; }
        public string source { get; set; }
        public string explanatoryNote { get; set; }
        public string Note { get; set; }
        public string val1 { get; set; }
        public MarkupString documentPriview { get; set; }
        public MarkupString legalNotesEn { get; set; }
        public MarkupString legalNotesAr { get; set; }
        public MarkupString legalExplanatoryNotesAr { get; set; }
        public MarkupString legalExplanatoryNotesEn { get; set; }
        public MarkupString introduction { get; set; }
        public string ArticleEffectNoteHistory { get; set; }
        public MarkupString ArticleEffectNoteHistoryList { get; set; }
        public List<LegalLegislationCommentVM> LegalLegislationCommentsDetails { get; set; } = new List<LegalLegislationCommentVM>();
        public string DocumentPath { get; set; }
        List<ToolbarItem> ToolbarItems = new List<ToolbarItem>();

        #endregion

        #region Component Load
        public string Sections { get; set; }
        public string SectionArticals { get; set; }
        public string SectionClauses { get; set; }
        public string Clauses { get; set; }
        public string Articls { get; set; }
        public string Signatures { get; set; }
        public string Source { get; set; }
        public string ExplanatoryNote { get; set; }
        public MarkupString NewTest { get; set; }


        public MarkupString LegalNotesEn { get; set; }
        public MarkupString LegalExplanatoryNotesEn { get; set; }
        public MarkupString Articaltext { get; set; }
        public MarkupString Clausestext { get; set; }
        public DateTime IssueDate { get; set; }

        //<History Author = 'Hassan Abbas' Date='2023-07-05' Version="1.0" Branch="master"> Modified the function to Decrypt the attachment and then view, also added functionality to convert .msg file to .pdf for preview</History>
        protected override async Task OnInitializedAsync()
        {
            // legalLegislationVM.IssueDate = IssueDate.Date;

            if (RedirectFromPage == "legallegislationpublishunpublish")
            {
                RedirectToPage = "/legallegislation-publish-unpublish";
                RedirectFromTranslation = @translationState.Translate("Legal_Legislation_Publish_and_UnPublish");
            }
            else if (RedirectFromPage == "legallegislationreview")
            {
                RedirectToPage = "/legallegislation-review";
                RedirectFromTranslation = @translationState.Translate("Legal_Legislation_Review");
            }
            else if (RedirectFromPage == "Deletelegalligislation")
            {
                RedirectToPage = "/legallegislation-delete";
                RedirectFromTranslation = @translationState.Translate("Legal_Legislation_Delete");
            }
            else
            {
                RedirectToPage = "/legallegislation-list";
                RedirectFromTranslation = @translationState.Translate("Legal_Legislation_Heading");
            }
            spinnerService.Show();
            await LegalLegislationDetailByIdLoad();
            await GetLegislationPublicationSourceLoad();
            await GetLegislationNoteLegislationLoad();
            await LegislationExplanatoryNoteLegislationLoad();
            await LegislationSignaturesbyLegislationLoad();
            await LegalClasousSectionLoad();
            await LegalArticalSectionLoad();
            await LegalReferanceLoad();
            await GetLegalLegislationCommentsDetailByUsingId();

            await LegalLegislationDetailPriviewByIdLoad();

            if (getLegalNoteResult != null)
            {
                LegalNotesEn = new MarkupString(getLegalNoteResult.Note_Text);
            }
            if (getLegalExplanatoryNoteResult != null)
            {
                LegalExplanatoryNotesEn = new MarkupString(getLegalExplanatoryNoteResult.ExplanatoryNote_Body);

            }
            if (legalLegislation.Introduction != null)
            {
                introduction = new MarkupString(legalLegislation.Introduction);
            }

            documentPriview = new MarkupString(string.Format("{0} {1} {2} {3} {4} {5} {6} {7} {8} {9} {10} {11} {12}", LegalLegislationtype, head, referances, sections, sectionArticals, sectionClauses, articls, clauses, signatures, source, explanatoryNote, Note, ArticleEffectNoteHistory));
            var response = await legalLegislationService.GetAttachmentDetailForGridByUsingLegislationId(legalLegislation.LegislationId);
            if (response.IsSuccessStatusCode)
            {
                ObservableCollection<TempAttachementVM> res = new ObservableCollection<TempAttachementVM>();
                res = (ObservableCollection<TempAttachementVM>)response.ResultData;
                FileNameResult = res.FirstOrDefault();
                //FileNameResult = (TempAttachement)response.ResultData;
            }
            if (FileNameResult != null && FileNameResult.StoragePath != null)
            {
                ToolbarItems = new List<ToolbarItem>()
                {
                    ToolbarItem.PageNavigationTool,
                    ToolbarItem.MagnificationTool,
                    ToolbarItem.SelectionTool,
                    ToolbarItem.PanTool,
                    ToolbarItem.SearchOption,
                    ToolbarItem.PrintOption,
                    ToolbarItem.DownloadOption
                };
                var physicalPath = string.Empty;
#if DEBUG
                {
                   physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + FileNameResult.StoragePath).Replace(@"\\", @"\");
                    
                }
#else
                {
                    physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + FileNameResult.StoragePath).Replace(@"\\", @"\");
                    physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
                }
#endif
                if (!string.IsNullOrEmpty(physicalPath))
                {
                    string base64String = await DocumentEncryptionService.GetDecryptedDocumentBase64(physicalPath, FileNameResult.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
                    DocumentPath = "data:application/pdf;base64," + base64String;
                    VisibleSourceDocument = true;
                    StateHasChanged();
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Authority_Letter_Not_Loaded"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    VisibleSourceDocument = false;
                }
            }
            spinnerService.Hide();
            await JsInterop.InitilizePrincipleDetailReference(DotNetObjectReference.Create(this));
        }

        protected async Task LegalReferanceLoad()
        {
            try
            {
                var response = await legalLegislationService.GetLegislationReferencebyLegislationId(Guid.Parse(LegislationId));

                getLegalLegislationReferenceResult = (List<LegalLegislationReference>)response.ResultData;

                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected async Task LegalArticalSectionLoad()
        {
            try
            {

                var response = await legalLegislationService.GetLegalArticalSectionByLegislationId(Guid.Parse(LegislationId));

                getLegalArticalSectionResultVM = (List<LegalArticalSectionVM>)response.ResultData;
                foreach (var artical in getLegalArticalSectionResultVM)
                {
                    Articaltext = (MarkupString)artical.Article_Text;
                }
                var NumOfArtical = getLegalArticalSectionResultVM.Count();

                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected async Task LegalClasousSectionLoad()
        {
            try
            {

                var response = await legalLegislationService.GetLegalClausesSectionByLegislationId(Guid.Parse(LegislationId));

                getLegalClausesSectionResultVM = (List<LegalClausesSectionVM>)response.ResultData;
                foreach (var clasus in getLegalClausesSectionResultVM)
                {
                    Clausestext = (MarkupString)clasus.Clause_Content;
                }
                var NumOfClasuses = getLegalClausesSectionResultVM.Count();

                await InvokeAsync(StateHasChanged);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected async Task LegislationSignaturesbyLegislationLoad()
        {
            try
            {
                var response = await legalLegislationService.GetLegislationSignaturesbyLegislationId(Guid.Parse(LegislationId));

                getLegalLegislationSignatureResult = (List<LegalLegislationSignature>)response.ResultData;

                await InvokeAsync(StateHasChanged);



            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected async Task LegislationExplanatoryNoteLegislationLoad()
        {
            try
            {
                var response = await legalLegislationService.GetLegislationExplanatoryNoteLegislationId(Guid.Parse(LegislationId));

                if (response.IsSuccessStatusCode)
                {
                    getLegalExplanatoryNoteResult = (LegalExplanatoryNote)response.ResultData;
                }
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected async Task GetLegislationNoteLegislationLoad()
        {
            try
            {
                var response = await legalLegislationService.GetLegislationNoteLegislationId(Guid.Parse(LegislationId));

                getLegalNoteResult = (LegalNote)response.ResultData;
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected async Task GetLegislationPublicationSourceLoad()
        {
            try
            {
                var response = await legalLegislationService.GetLegalPublicationSourceDetailByLegislationId(Guid.Parse(LegislationId));

                getLegalPublicationSourceResult = (LegalPublicationSourceVM)response.ResultData;
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected async Task LegalLegislationDetailByIdLoad()
        {
            try
            {
                var response = await legalLegislationService.GetLegalLegislationDetailById(Guid.Parse(LegislationId));
                if (response.IsSuccessStatusCode)
                {
                    legalLegislationVM = (LegalLegislationDetailVM)response.ResultData;
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

        protected async Task LegalLegislationDetailPriviewByIdLoad()
        {
            try
            {
                legalLegislation = new LegalLegislation();
                legalLegislation.LegislationId = Guid.Parse(LegislationId);
                var response = await legalLegislationService.GetLegislationPriviewById(legalLegislation.LegislationId);
                if (response.IsSuccessStatusCode)
                {
                    legalLegislation = (LegalLegislation)response.ResultData;
                    legalArticleGet = (List<LegalArticle>)legalLegislation.LegalArticles;
                    legalClausesGet = (List<LegalClause>)legalLegislation.LegalClauses;
                    legalSectionGet = (List<LegalSection>)legalLegislation.LegalSections;
                    legalExplanatoryNotesGet = (List<LegalExplanatoryNote>)legalLegislation.LegalExplanatoryNotes;
                    legalNotesGet = (List<LegalNote>)legalLegislation.legalNotes;
                    legalPublicationSourcesGet = (List<LegalPublicationSource>)legalLegislation.LegalPublicationSources;
                    legalLegislationSignatureGet = (List<LegalLegislationSignature>)legalLegislation.LegalLegislationSignatures;
                    legalLegislationTypes = (List<LegalLegislationType>)legalLegislation.LegalLegislationTypes;
                    legalLegislationArticleEffectHistorys = (List<LegalLegislationArticleEffectHistory>)legalLegislation.LegalLegislationArticleEffectHistorys;

                    if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                    {
                        if (legalLegislation.Legislation_Type.ToString() != null && legalLegislation.Legislation_Number.ToString() != null && legalLegislation.Legislation_Type.ToString() != null && legalLegislation.IssueDate != null)
                        {
                            foreach (var item in legalLegislation.LegalLegislationTypes)
                            {
                                LegalLegislationtype = @"<div class=""TitleCenter"" style=""background-color:#FFF"" > <h6 style=""text-align: center;""> <p>" + item.Name_En + @" " + legalLegislation.Legislation_Number + @" " + legalLegislation.IssueDate.Value.ToString("dd/MM/yyyy") + @"</p></h6>";

                            }
                        }

                        head = @"<div class=""Container"" style=""background-color:#FFF"" > 
                              <h1 style=""text-align: center;""><strong>" + legalLegislation.LegislationTitle +
                              @"</strong></h1><p>";
                        if (legalLegislation.Introduction != null)
                        {
                            if (Regex.IsMatch(legalLegislation.Introduction, @"^[0-9A-Za-z]"))
                            {
                                referances += @"<div class=""row text"" Style=""word-break: break-word;""><div class=""col-md-12""><p Class=""introduction"">" + legalLegislation.Introduction + @"</div></div></p>";
                            }
                            else
                            {
                                referances += @"<div class=""row preview-text"" Style=""word-break: break-word;""><div class=""col-md-12""><p class=""introduction"">" + legalLegislation.Introduction + @"</div></div></p>";
                            }

                        }
                        if (legalSectionGet.Count() != 0)
                        {
                            foreach (var item in legalSectionGet)
                            {
                                if (item.SectionTitle != null)
                                {
                                    if (Regex.IsMatch(item.SectionTitle, @"^[0-9A-Za-z]"))
                                    {
                                        sections += @"<div class=""row text""><div class=""col-md-12""><div class=""LegislationSection"">
                                    <div class=""LegislationNumber""> " + @"" + item.SectionTitle + @"
                                    </br>";
                                    }
                                    else
                                    {
                                        sections += @"<div class=""row preview-text""><div class=""col-md-12""><div class=""LegislationSectionAr"">
                                    <div class=""LegislationNumber""> " + @"" + item.SectionTitle + @"
                                    </br>";
                                    }

                                }

                                item.LegalArticlesUnderSection = (List<LegalArticle>)item.LegalArticlesUnderSection;

                                foreach (var itemSectionArticals in item.LegalArticlesUnderSection)
                                {
                                    if (Regex.IsMatch(itemSectionArticals.Article_Title, @"^[0-9A-Za-z]"))
                                    {
                                        sectionArticals += @"<b class=""SectionArticalstitle"">
                                    " + itemSectionArticals.Article_Title + @"</b><br/>
                                    " + itemSectionArticals.Article_Text + @"</div></div>";
                                    }
                                    else
                                    {
                                        sectionArticals += @"<b class=""SectionArticalstitleAr"">
                                    " + itemSectionArticals.Article_Title + @"</b><br/>
                                    " + itemSectionArticals.Article_Text + @"</div></div>";
                                    }
                                }
                                foreach (var itemSectionClasus in item.LegalClauseUnderSection)
                                {
                                    if (Regex.IsMatch(itemSectionClasus.Clause_Name, @"^[0-9A-Za-z]") && Regex.IsMatch(itemSectionClasus.Clause_Content, @"^[0-9A-Za-z]"))
                                    {
                                        sectionArticals += @"<b class=""SectionArticalstitle"">
                                    " + itemSectionClasus.Clause_Name + @"</b><br/>
                                    " + itemSectionClasus.Clause_Content + @"</div ></div>";
                                    }
                                    else
                                    {
                                        sectionArticals += @"<b class=""SectionArticalstitleAr"">
                                    " + itemSectionClasus.Clause_Name + @"</b><br/>
                                    " + itemSectionClasus.Clause_Content + @"</div ></div>";
                                    }
                                }
                            }
                        }
                        if (legalArticleGet.Count() != 0)
                        {
                            foreach (var item in legalArticleGet)
                            {
                                if (Regex.IsMatch(item.Article_Title, @"^[0-9A-Za-z]"))
                                {
                                    articls += @"<div class=""row Atext"">
                                             <div class=""col-md-12"">
                                                  <div class=""SectionArticalstitle"">" + item.Article_Title + @"</div>
                                                    <div class=""SectionArticalstext"">" + item.Article_Text + @" </div>
                                             </div>
                                          </div>";
                                }
                                else
                                {
                                    articls += @"<div class=""row preview-text"">
                                             <div class=""col-md-12"">
                                                  <div class=""SectionArticalstitle"">" + item.Article_Title + @"</div>
                                                    <div class=""SectionArticalstext"">" + item.Article_Text + @" </div>
                                             </div>
                                          </div>";
                                }
                            }
                        }
                        if (legalClausesGet.Count() != 0)
                        {
                            foreach (var item in legalClausesGet)
                            {
                                if (Regex.IsMatch(item.Clause_Name, @"^[0-9A-Za-z]") && Regex.IsMatch(item.Clause_Content, @"^[0-9A-Za-z]"))
                                {
                                    clauses += @"<div class=""row Atext""><div class=""col-md-12""><div class=""SectionArticalstitle"">" + item.Clause_Name + @"</div>
                                " + item.Clause_Content + @"</div></div>";
                                }
                                else
                                {
                                    clauses += @"<div class=""row preview-text""><div class=""col-md-12""><div class=""SectionArticalstitle"">" + item.Clause_Name + @"</div>
                                " + item.Clause_Content + @"</div></div>";
                                }


                            }
                        }
                        if (legalLegislationSignatureGet.Count() != 0)
                        {
                            foreach (var item in legalLegislationSignatureGet)
                            {
                                if (Regex.IsMatch(item.Full_Name, @"^[0-9A-Za-z]") && Regex.IsMatch(item.Job_Title, @"^[0-9A-Za-z]"))
                                {
                                    signatures += @"<div class=""row Signaturetext""><div class=""col-md-12"">
                                <p class=""legalSignature"">" + item.Full_Name + @" </p>
                                <p class=""legalSignatureTitle""> " + item.Job_Title + @" </p></div></div>";
                                }
                                else
                                {
                                    signatures += @"<div class=""row SignaturetextAr""><div class=""col-md-12"">
                                <p class=""legalSignature"">" + item.Full_Name + @" </p>
                                <p class=""legalSignatureTitle""> " + item.Job_Title + @" </p></div></div>";
                                }

                            }
                        }
                        if (legalExplanatoryNotesGet.Count() != 0)
                        {

                            foreach (var item in legalExplanatoryNotesGet)
                            {
                                if (item.ExplanatoryNote_Body != null)
                                {
                                    if (Regex.IsMatch(item.ExplanatoryNote_Body, @"^[0-9A-Za-z]"))
                                    {
                                        explanatoryNote += @"" + translationState.Translate("Explanatory_Note") + @" <p class=""legalSignature text"">" + item.ExplanatoryNote_Body + @"</p> ";
                                        //explanatoryNote += @"<div class=""row text"" Style=""word-break: break-word;""><div class=""col-md-12""><p class=""introduction"">" + translationState.Translate("Explanatory_Note") + @" <p class=""legalSignature"">" + item.ExplanatoryNote_Body + @"</div></div></p>";
                                    }
                                    else
                                    {
                                        explanatoryNote += @"" + translationState.Translate("Explanatory_Note") + @" <p class=""legalSignature preview-text"">" + item.ExplanatoryNote_Body + @"</p> ";
                                        //explanatoryNote += @"<div class=""row preview-text"" Style=""word-break: break-word;""><div class=""col-md-12""><p class=""introduction"">" + translationState.Translate("Explanatory_Note") + @" <p class=""legalSignature"">" + item.ExplanatoryNote_Body + @"</div></div></p>";
                                    }
                                }

                            }
                        }
                        if (legalNotesGet.Count() != 0)
                        {
                            foreach (var item in legalNotesGet)
                            {
                                if (item.Note_Text != null)
                                {
                                    if (Regex.IsMatch(item.Note_Text, @"^[0-9A-Za-z]"))
                                    {
                                        Note += @"" + translationState.Translate("Note") + @"<p class=""legalSignature text"">" + item.Note_Text + @"</p> ";
                                        //Note += @"<div class=""row text"" Style=""word-break: break-word;""><div class=""col-md-12""><p class=""introduction"">" + translationState.Translate("Note") + @" <p class=""legalSignature"">" + item.Note_Text + @"</div></div></p>";
                                    }
                                    else
                                    {
                                        Note += @"" + translationState.Translate("Note") + @"<p class=""legalSignature preview-text"">" + item.Note_Text + @"</p> ";
                                        //Note += @"<div class=""row preview-text"" Style=""word-break: break-word;""><div class=""col-md-12""><p class=""introduction"">" + translationState.Translate("Note") + @" <p class=""legalSignature"">" + item.Note_Text + @"</div></div></p>";
                                    }
                                }

                            }
                        }
                        if (legalPublicationSourcesGet.Count() != 0)
                        {
                            foreach (var item in legalPublicationSourcesGet)
                            {
                                source += @"<div class=""row"" Style=""word-break: break-word;""><div class=""col-md-12""><div class=""col-md-6 signatureFooter""><p>" + translationState.Translate("Legislation_Preview_Footer1") + @"
                                " + item.Issue_Number + @"</p>
                                <p>" + translationState.Translate("at") + @": " + item.PublicationDate.ToString("dd/MM/yyyy") + @"</p></div></div>
                                </div>";
                            }
                        }
                    }
                    else
                    {
                        if (legalLegislation.Legislation_Type.ToString() != null && legalLegislation.Legislation_Number.ToString() != null && legalLegislation.Legislation_Type.ToString() != null && legalLegislation.IssueDate != null)
                        {
                            foreach (var item in legalLegislation.LegalLegislationTypes)
                            {
                                LegalLegislationtype = @"<div  style=""background-color:#FFF"" > <h6 style=""text-align: center;""> <p>" + item.Name_Ar + @" " + legalLegislation.Legislation_Number + @" " + legalLegislation.IssueDate.Value.ToString("dd/MM/yyyy") + @"</p></h6>";
                            }
                        }
                        head = @"<div style=""word-break: break-word; background-color:#FFF"" > 
                              <h1 style=""text-align: center;"">
                              <strong>" + legalLegislation.LegislationTitle +
                              @"</strong></h1><p>";
                        if (legalLegislation.Introduction != null)
                        {
                            if (Regex.IsMatch(legalLegislation?.Introduction, @"^[0-9A-Za-z]"))
                            {
                                referances += @"<div class=""row textEn"" Style=""word-break: break-word;""><div class=""col-md-12""><p Class=""introduction"">" + legalLegislation.Introduction + @"</div></div></p>";
                            }
                            else
                            {
                                referances += @"<div class=""row textAr"" Style=""word-break: break-word;""><div class=""col-md-12""><p Class=""introduction"">" + legalLegislation.Introduction + @"</div></div></p>";
                            }

                        }
                        if (legalSectionGet.Count() != 0)
                        {
                            foreach (var item in legalSectionGet)
                            {
                                if (item.SectionTitle != null)
                                {
                                    if (Regex.IsMatch(item.SectionTitle, @"^[0-9A-Za-z]"))
                                    {
                                        sections += @"<div class=""row textEn"" Style=""word-break: break-word;""><div class=""col-md-12""><div class=""LegislationSection"">
                                                <div class=""LegislationNumber""> " + @"" + item.SectionTitle + @"
                                                </br>";
                                    }
                                    else
                                    {
                                        sections += @"<div class=""row textAr"" Style=""word-break: break-word;""><div class=""col-md-12""><div class=""LegislationSection"">
                                                <div class=""LegislationNumber""> " + @"" + item.SectionTitle + @"
                                                </br>";
                                    }
                                }

                                item.LegalArticlesUnderSection = (List<LegalArticle>)item.LegalArticlesUnderSection;

                                foreach (var itemSectionArticals in item.LegalArticlesUnderSection)
                                {
                                    if (Regex.IsMatch(itemSectionArticals.Article_Title, @"^[0-9A-Za-z]"))
                                    {
                                        sectionArticals += @"<b class=""SectionArticalstitle "">
                                    " + itemSectionArticals.Article_Title + @"</b><br/>
                                    " + itemSectionArticals.Article_Text + @"</div></div>";
                                    }
                                    else
                                    {
                                        sectionArticals += @"<b class=""SectionArticalstitle "">
                                    " + itemSectionArticals.Article_Title + @"</b><br/>
                                    " + itemSectionArticals.Article_Text + @"</div></div>";
                                    }


                                }
                                foreach (var itemSectionClasus in item.LegalClauseUnderSection)
                                {
                                    if (Regex.IsMatch(itemSectionClasus.Clause_Name, @"^[0-9A-Za-z]") && Regex.IsMatch(itemSectionClasus.Clause_Content, @"^[0-9A-Za-z]"))
                                    {
                                        sectionArticals += @"<b class=""SectionArticalstitle"">
                                                            " + itemSectionClasus.Clause_Name + @"</b><br/>
                                                            " + itemSectionClasus.Clause_Content + @"</div></div>";
                                    }
                                    else
                                    {
                                        sectionArticals += @"<b class=""SectionArticalstitle"">
                                                            " + itemSectionClasus.Clause_Name + @"</b><br/>
                                                            " + itemSectionClasus.Clause_Content + @"</div></div>";
                                    }
                                }
                            }
                        }
                        if (legalArticleGet.Count() != 0)
                        {
                            foreach (var item in legalArticleGet)
                            {
                                if (Regex.IsMatch(item.Article_Title, @"^[0-9A-Za-z]"))
                                {
                                    articls += @"<div class=""row Atext textEn"">
                                             <div class=""col-md-12"">
                                                  <div class=""SectionArticalstitle textEn"">" + item.Article_Title + @"</div>
                                                    <div class=""SectionArticalstext textEn"">" + item.Article_Text + @" </div>
                                             </div>
                                          </div>";
                                }
                                else
                                {
                                    articls += @"<div class=""row Atext textAr"">
                                             <div class=""col-md-12"">
                                                  <div class=""SectionArticalstitle textAr"">" + item.Article_Title + @"</div>
                                                    <div class=""SectionArticalstext textAr"">" + item.Article_Text + @" </div>
                                             </div>
                                          </div>";
                                }

                            }
                        }
                        if (legalClausesGet.Count() != 0)
                        {
                            foreach (var item in legalClausesGet)
                            {
                                if (Regex.IsMatch(item.Clause_Name, @"^[0-9A-Za-z]") && Regex.IsMatch(item.Clause_Content, @"^[0-9A-Za-z]"))
                                {
                                    clauses += @"<div class=""row Atext textEn""><div class=""col-md-12""><div class=""SectionArticalstitle textEn"">" + item.Clause_Name + @"</div>
                                          " + item.Clause_Content + @"</div></div>";
                                }
                                else
                                {
                                    clauses += @"<div class=""row Atext textAr""><div class=""col-md-12""><div class=""SectionArticalstitle textAr"">" + item.Clause_Name + @"</div>
                                          " + item.Clause_Content + @"</div></div>";
                                }
                            }
                        }
                        if (legalLegislationSignatureGet.Count() != 0)
                        {
                            foreach (var item in legalLegislationSignatureGet)
                            {
                                if (Regex.IsMatch(item.Full_Name, @"^[0-9A-Za-z]") && Regex.IsMatch(item.Job_Title, @"^[0-9A-Za-z]"))
                                {
                                    signatures += @"<div class=""row SignaturetextEn textEn""><div class=""col-md-12"">
                                            <p class=""legalSignature textEn"">" + item.Full_Name + @" </p>
                                            <p class=""legalSignatureTitle textEn""> " + item.Job_Title + @" </p></div></div>";
                                }

                                else
                                {
                                    signatures += @"<div class=""row Signaturetext textAr""><div class=""col-md-12"">
                                            <p class=""legalSignature textAr"">" + item.Full_Name + @" </p>
                                            <p class=""legalSignatureTitle textAr""> " + item.Job_Title + @" </p></div></div>";
                                }

                            }
                        }
                        if (legalExplanatoryNotesGet.Count() != 0)
                        {
                            foreach (var item in legalExplanatoryNotesGet)
                            {
                                if (item.ExplanatoryNote_Body != null)
                                {
                                    if (Regex.IsMatch(item?.ExplanatoryNote_Body, @"^[0-9A-Za-z]"))
                                    {
                                        explanatoryNote += @"" + translationState.Translate("Explanatory_Note") + @" <p class=""legalSignature textEn"">" + item.ExplanatoryNote_Body + @"</p> ";

                                    }
                                    else
                                    {
                                        explanatoryNote += @"" + translationState.Translate("Explanatory_Note") + @" <p class=""legalSignature textAr"">" + item.ExplanatoryNote_Body + @"</p> ";

                                    }
                                }
                            }
                        }
                        if (legalNotesGet.Count() != 0)
                        {
                            foreach (var item in legalNotesGet)
                            {
                                if (item.Note_Text != null)
                                {
                                    if (Regex.IsMatch(item.Note_Text, @"^[0-9A-Za-z]"))
                                    {
                                        Note += @"" + translationState.Translate("Note") + @"<p class=""legalSignature textEn"">" + item.Note_Text + @"</p> ";
                                    }
                                    else
                                    {
                                        Note += @"" + translationState.Translate("Note") + @"<p class=""legalSignature textAr"">" + item.Note_Text + @"</p> ";
                                    }
                                }
                            }
                        }
                        if (legalPublicationSourcesGet.Count() != 0)
                        {
                            foreach (var item in legalPublicationSourcesGet)
                            {
                                source += @"<div class=""row"" Style=""word-break: break-word;""><div class=""col-md-12""><div class=""col-md-6 signatureFooter""><p>" + translationState.Translate("Legislation_Preview_Footer1") + @"
                                " + item.Issue_Number + @"</p>
                                <p>" + translationState.Translate("at") + @": " + item.PublicationDate.ToString("dd/MM/yyyy") + @"</p></div></div>
                                </div>";
                            }
                        }
                    }
                    if (legalLegislationArticleEffectHistorys.Count() != 0)
                    {
                        foreach (var item in legalLegislationArticleEffectHistorys)
                        {
                            ArticleEffectNoteHistory += @"<p class=""legalSignature"">" + item.Note + @"</p> ";
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
                var result = await dialogService.OpenAsync<LegallLegislationViewDetail>(translationState.Translate("Legal_legislation_Details"),
                               new Dictionary<string, object>()
                               {
                                   { "LegislationId", id.ToString() },
                                   { "modelName", modelName }
                               },
                               new DialogOptions() { Width = "80% !important", CloseDialogOnOverlayClick = true });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get legislation comments
        protected async Task GetLegalLegislationCommentsDetailByUsingId()
        {
            try
            {
                var response = await legalLegislationService.GetLegalLegislationCommentsDetailByUsingId(Guid.Parse(LegislationId));
                if (response.IsSuccessStatusCode)
                {
                    LegalLegislationCommentsDetails = (List<LegalLegislationCommentVM>)response.ResultData;
                }
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region  Document Loaded
        protected async Task DocumentLoaded(LoadEventArgs args)
        {
            await JSRuntime.InvokeVoidAsync("addRotateButtonToPdfToolbar");
        }
        #endregion
    }
}
