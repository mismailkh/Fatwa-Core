using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.Lms;
using FATWA_DOMAIN.Models.WorkflowModels;
using FATWA_GENERAL.Helper;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Syncfusion.Blazor.PdfViewer;
using Syncfusion.Blazor.PdfViewerServer;
using System.Collections.ObjectModel;
using Telerik.Blazor.Components;
using static FATWA_DOMAIN.Enums.LegalLegislationEnum;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Response;


namespace FATWA_WEB.Pages.Lds
{
    //<History Author = 'Hassan Iftikhar'  Version="1.0" Branch="master"> </History>
    public partial class LegislationDecision : ComponentBase
    {
        #region Parameter
        [Parameter]
        public dynamic LegislationId { get; set; }
        [Parameter]
        public dynamic FromPage { get; set; }
        #endregion
        //<History Author = 'Nadia Gull' Version="1.0" Branch="master"> </History>
        #region Varriables
        LegalLegislationDecisionVM legalLegislationDecisionVM;
        // protected LegalLegislation legalLegislation { get; set; }
        protected List<LegalLegislationFlowStatus> legalLegislationFlowStatus { get; set; } = new List<LegalLegislationFlowStatus>();
        protected List<LegalLegislationFlowStatus> excludedFlowStatus { get; set; } = new List<LegalLegislationFlowStatus>();
        protected bool isCommentRequired { get; set; } = false;
        protected string requiredField = "";
        //start code
        public MarkupString LegalNotesEn { get; set; }
        public MarkupString LegalExplanatoryNotesEn { get; set; }
        public MarkupString Articaltext { get; set; }
        public MarkupString Clausestext { get; set; }

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
        public int count { get; set; } = 0;
        public TelerikPdfViewer PdfViewerRef { get; set; }
        public byte[] FileData { get; set; }
        //public TempAttachement FileNameResult { get; set; }
        public TempAttachementVM FileNameResult { get; set; }
        public SfPdfViewerServer pdfViewer;
        public string DocumentPath { get; set; } = string.Empty;
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
        protected RadzenDataGrid<LegalLegislationSignature> grid = new RadzenDataGrid<LegalLegislationSignature>();
        protected RadzenDataGrid<LegalArticalSectionVM> gridSectionArtical = new RadzenDataGrid<LegalArticalSectionVM>();

        protected RadzenDataGrid<LegalClausesSectionVM> gridSectionClauses = new RadzenDataGrid<LegalClausesSectionVM>();

        #endregion
        //end code

        #region Initialze & Load
        protected override async Task OnInitializedAsync()
        {

            spinnerService.Show();
            await PopulateLegislationFlowStatus();
            //Start Code

            await LegalLegislationDetailByIdLoad();
            await GetLegislationPublicationSourceLoad();
            await GetLegislationNoteLegislationLoad();
            await LegislationExplanatoryNoteLegislationLoad();
            await LegislationSignaturesbyLegislationLoad();
            await LegalClasousSectionLoad();
            await LegalArticalSectionLoad();
            await LegalReferanceLoad();

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
                    FileData = await DocumentEncryptionService.GetDecryptedDocumentBytes(physicalPath, FileNameResult.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
                    string base64String = Convert.ToBase64String(FileData);
                    DocumentPath = "data:application/pdf;base64," + base64String;
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
                }
            }
            // End Code
            await Load();
            spinnerService.Hide();
        }

        protected async Task Load()
        {
            try
            {
                var getDecision = await legalLegislationService.GetLegislationDecision(Guid.Parse(LegislationId));
                if (getDecision.IsSuccessStatusCode)
                {
                    legalLegislationDecisionVM = (LegalLegislationDecisionVM)getDecision.ResultData;
                }
                else
                {
                    legalLegislationDecisionVM = null;
                }
                ChangeApprovalTypeForAddingDecision(legalLegislationFlowStatus);
                legalLegislationDecisionVM.FlowStatusId = 0;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        //<History Author = 'Nadia Gull' Version="1.0" Branch="master">Populate Legislation FlowStatus</History>
        #region Remote Dropdown Data and Dropdown Change Events
        protected async Task PopulateLegislationFlowStatus()
        {
            var response = await legalLegislationService.GetLegislationFlowStatusDetails();
            if (response.IsSuccessStatusCode)
            {
                legalLegislationFlowStatus = (List<LegalLegislationFlowStatus>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        private async void ChangeApprovalTypeForAddingDecision(IEnumerable<LegalLegislationFlowStatus> legalLegislationFlowStatus)
        {
            try
            {
                if (legalLegislationDecisionVM.FlowStatusId == (int)LegislationFlowStatusEnum.InReview)
                {
                    excludedFlowStatus = legalLegislationFlowStatus.Where(x => x.Id == (int)LegislationFlowStatusEnum.Approved || x.Id == (int)LegislationFlowStatusEnum.NeedToModify).ToList();
                    await ChangeFlowStatusName(excludedFlowStatus);
                }
                else if (legalLegislationDecisionVM.FlowStatusId == (int)LegislationFlowStatusEnum.Approved)
                {
                    excludedFlowStatus = legalLegislationFlowStatus.Where(x => x.Id == (int)LegislationFlowStatusEnum.Published || x.Id == (int)LegislationFlowStatusEnum.NeedToModify).ToList();
                    await ChangeFlowStatusName(excludedFlowStatus);
                }
                else if (legalLegislationDecisionVM.FlowStatusId == (int)LegislationFlowStatusEnum.Published)
                {
                    excludedFlowStatus = legalLegislationFlowStatus.Where(x => x.Id == (int)LegislationFlowStatusEnum.Unpublished).ToList();
                    await ChangeFlowStatusName(excludedFlowStatus);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task ChangeFlowStatusName(List<LegalLegislationFlowStatus> excludedFlowStatus)
        {
            try
            {
                List<LegalLegislationFlowStatus> flowStatus = new List<LegalLegislationFlowStatus>();
                foreach (var item in excludedFlowStatus)
                {
                    if (item.Id == (int)LegislationFlowStatusEnum.InReview)
                    {
                        if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                        {
                            item.Name_En = translationState.Translate("In Review");
                            flowStatus.Add(item);
                        }
                        else
                        {
                            item.Name_Ar = translationState.Translate("In Review");
                            flowStatus.Add(item);
                        }
                    }
                    if (item.Id == (int)LegislationFlowStatusEnum.Approved)
                    {
                        if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                        {
                            item.Name_En = translationState.Translate("Approve");
                            flowStatus.Add(item);
                        }
                        else
                        {
                            item.Name_Ar = translationState.Translate("Approve");
                            flowStatus.Add(item);
                        }
                    }
                    if (item.Id == (int)LegislationFlowStatusEnum.Rejected)
                    {
                        if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                        {
                            item.Name_En = translationState.Translate("Reject");
                            flowStatus.Add(item);
                        }
                        else
                        {
                            item.Name_Ar = translationState.Translate("Reject");
                            flowStatus.Add(item);
                        }
                    }
                    if (item.Id == (int)LegislationFlowStatusEnum.NeedToModify)
                    {
                        if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                        {
                            item.Name_En = translationState.Translate("Need to modify");
                            flowStatus.Add(item);
                        }
                        else
                        {
                            item.Name_Ar = translationState.Translate("Need_To_Modify");
                            flowStatus.Add(item);
                        }
                    }
                    if (item.Id == (int)LegislationFlowStatusEnum.SendAComment)
                    {
                        if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                        {
                            item.Name_En = translationState.Translate("Send a comment");
                            flowStatus.Add(item);
                        }
                        else
                        {
                            item.Name_Ar = translationState.Translate("Send_a_comment");
                            flowStatus.Add(item);
                        }
                    }
                    if (item.Id == (int)LegislationFlowStatusEnum.Published)
                    {
                        if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                        {
                            item.Name_En = translationState.Translate("Publish");
                            flowStatus.Add(item);
                        }
                        else
                        {
                            item.Name_Ar = translationState.Translate("Publish");
                            flowStatus.Add(item);
                        }
                    }
                    if (item.Id == (int)LegislationFlowStatusEnum.Unpublished)
                    {
                        if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                        {
                            item.Name_En = translationState.Translate("Unpublish");
                            flowStatus.Add(item);
                        }
                        else
                        {
                            item.Name_Ar = translationState.Translate("UnPublish");
                            flowStatus.Add(item);
                        }
                    }

                }
                excludedFlowStatus = flowStatus;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region  Functions for Detail View

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
                ApiCallResponse response = await legalLegislationService.GetLegalPublicationSourceDetailByLegislationId(Guid.Parse(LegislationId));
                if (response.IsSuccessStatusCode)
                {
                    getLegalPublicationSourceResult = (LegalPublicationSourceVM)response.ResultData;
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

        protected async Task LegalLegislationDetailByIdLoad()
        {
            try
            {
                var response = await legalLegislationService.GetLegalLegislationDetailById(Guid.Parse(LegislationId));
                if (response.IsSuccessStatusCode)
                {
                    legalLegislationVM = (LegalLegislationDetailVM)response.ResultData;

                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }

                await InvokeAsync(StateHasChanged);
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
                    await PopulateLegalLegislationData(legalLegislation);
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
        protected async Task PopulateLegalLegislationData(LegalLegislation legislation)
        {
            try
            {
                legalArticleGet = (List<LegalArticle>)legislation.LegalArticles;
                legalClausesGet = (List<LegalClause>)legislation.LegalClauses;
                legalSectionGet = (List<LegalSection>)legislation.LegalSections;
                legalExplanatoryNotesGet = (List<LegalExplanatoryNote>)legislation.LegalExplanatoryNotes;
                legalNotesGet = (List<LegalNote>)legislation.legalNotes;
                legalPublicationSourcesGet = (List<LegalPublicationSource>)legislation.LegalPublicationSources;
                legalLegislationSignatureGet = (List<LegalLegislationSignature>)legislation.LegalLegislationSignatures;
                legalLegislationTypes = (List<LegalLegislationType>)legislation.LegalLegislationTypes;
                if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                {
                    if (legislation.Legislation_Type.ToString() != null && legislation.Legislation_Number.ToString() != null && legislation.Legislation_Type.ToString() != null && legislation.IssueDate != null)
                    {
                        foreach (var item in legislation.LegalLegislationTypes)
                        {
                            LegalLegislationtype = @"<div class=""TitleCenter"" style=""background-color:#FFF"" > <h6 style=""text-align: center;""> <p>" + item.Name_En + @" " + legislation.Legislation_Number + @" " + legislation.IssueDate.Value.ToString("dd/MM/yyyy") + @"</p></h6>";

                        }
                    }

                    head = @"<div class=""ContainerPr"" style=""background-color:#FFF"" > <h1 style=""text-align: center;""><strong>" + legislation.LegislationTitle + @"</strong></h1><p>";
                    if (legislation.Introduction != null)
                    {
                        referances += @"<div class=""row"" Style=""word-break: break-word;"" ><div class=""col-md-12 introduction"">" + legislation.Introduction + @"</div></div>";

                    }
                    if (legalSectionGet != null)
                    {
                        foreach (var item in legalSectionGet)
                        {
                            sections += @"<div class=""row""><div class=""col-md-12""><div class=""LegislationSection"">
                    <div class=""LegislationNumber""> " + item.Section_Number + @"" + item.SectionTitle + @"
                    </br>";

                            item.LegalArticlesUnderSection = (List<LegalArticle>)item.LegalArticlesUnderSection;

                            foreach (var itemSectionArticals in item.LegalArticlesUnderSection)
                            {
                                sectionArticals += @"<b class=""SectionArticalstitle"">
                        " + itemSectionArticals.Article_Title + @"</b><br/>
                        " + itemSectionArticals.Article_Text + @"</div></div>";

                            }
                            foreach (var itemSectionClasus in item.LegalClauseUnderSection)
                            {
                                sectionArticals += @"<b class=""SectionArticalstitle"">
                        " + itemSectionClasus.Clause_Name + @"</b><br/>
                        " + itemSectionClasus.Clause_Content + @"</div></div>";


                            }
                        }
                    }
                    if (legalClausesGet != null)
                    {
                        foreach (var item in legalArticleGet)
                        {
                            articls += @"<div class=""row Atext""><div class=""col-md-12""><div class=""SectionArticalstitle"">" + item.Article_Title + @"</div>
                        " + item.Article_Text + @"</div></div>";


                        }
                    }
                    if (legalClausesGet != null)
                    {
                        foreach (var item in legalClausesGet)
                        {
                            clauses += @"<div class=""row Atext""><div class=""col-md-12""><div class=""SectionArticalstitle"">" + item.Clause_Name + @"</div>
                        " + item.Clause_Content + @"</div></div>";

                        }
                    }
                    if (legalLegislationSignatureGet != null)
                    {
                        foreach (var item in legalLegislationSignatureGet)
                        {
                            signatures += @"<div class=""row Signaturetext""><div class=""col-md-12"">
                        <p class=""legalSignature"">" + item.Full_Name + @" </p>
                        <p class=""legalSignatureTitle""> " + item.Job_Title + @" </p></div></div>";
                        }
                    }
                    if (legalExplanatoryNotesGet != null)
                    {
                        foreach (var item in legalExplanatoryNotesGet)
                        {
                            explanatoryNote += @"" + translationState.Translate("Explanatory_Note") + @" <p class=""legalSignature"">" + item.ExplanatoryNote_Body + @"</p> ";
                        }
                    }
                    if (legalNotesGet != null)
                    {
                        foreach (var item in legalNotesGet)
                        {
                            Note += @"" + translationState.Translate("Note") + @"<p class=""legalSignature"">" + item.Note_Text + @"</p> ";
                        }
                    }
                    if (legalPublicationSourcesGet != null)
                    {
                        foreach (var item in legalPublicationSourcesGet)
                        {
                            source += @"<div class=""row"" Style=""word-break: break-word;""><div class=""col-md-12""><div class=""col-md-6""><p>" + translationState.Translate("Legislation_Preview_Footer1") + @"
                    " + item.Issue_Number + @"</p>
                    <p>" + translationState.Translate("at") + @": " + item.PublicationDate.ToString("dd/MM/yyyy") + @"</p></div></div>
                    </div>";
                        }
                    }
                }
                else
                {
                    if (legislation.Legislation_Type.ToString() != null && legislation.Legislation_Number.ToString() != null && legislation.Legislation_Type.ToString() != null && legislation.IssueDate != null)
                    {
                        foreach (var item in legislation.LegalLegislationTypes)
                        {
                            LegalLegislationtype = @"<div class=""TitleCenter"" style=""background-color:#FFF"" > <h6 style=""text-align: center;""> <p>" + item.Name_Ar + @" " + legislation.Legislation_Number + @" " + legislation.IssueDate.Value.ToString("dd/MM/yyyy") + @"</p></h6>";
                        }
                    }
                    head = @"<div class=""Container"" style=""word-break: break-word; background-color:#FFF"" > <h1 style=""text-align: center;""><strong>" + legislation.LegislationTitle + @"</strong></h1><p>";
                    if (legislation.Introduction != null)
                    {
                        referances += @"<div class=""row"" Style=""word-break: break-word;""><div class=""col-md-12 introduction"">" + legislation.Introduction + @"</div></div>";

                    }
                    if (legalSectionGet != null)
                    {
                        foreach (var item in legalSectionGet)
                        {
                            sections += @"<div class=""row"" Style=""word-break: break-word;""><div class=""col-md-12""><div class=""LegislationSection"">
                    <div class=""LegislationNumber""> " + item.Section_Number + @"" + item.SectionTitle + @"
                    </br>";

                            item.LegalArticlesUnderSection = (List<LegalArticle>)item.LegalArticlesUnderSection;

                            foreach (var itemSectionArticals in item.LegalArticlesUnderSection)
                            {
                                sectionArticals += @"<b class=""SectionArticalstitle"">
                              " + itemSectionArticals.Article_Title + @"</b><br/>
                              " + itemSectionArticals.Article_Text + @"</div></div>";

                            }
                            foreach (var itemSectionClasus in item.LegalClauseUnderSection)
                            {
                                sectionArticals += @"<b class=""SectionArticalstitle"">
                        " + itemSectionClasus.Clause_Name + @"</b><br/>
                        " + itemSectionClasus.Clause_Content + @"</div></div>";


                            }
                        }
                    }
                    if (legalClausesGet != null)
                    {
                        foreach (var item in legalArticleGet)
                        {
                            articls += @"<div class=""row Atext""><div class=""col-md-12""><div class=""SectionArticalstitle"">" + item.Article_Title + @"</div>
                        " + item.Article_Text + @"</div></div>";
                        }
                    }
                    if (legalClausesGet != null)
                    {
                        foreach (var item in legalClausesGet)
                        {
                            clauses += @"<div class=""row Atext""><div class=""col-md-12""><div class=""SectionArticalstitle"">" + item.Clause_Name + @"</div>
                        " + item.Clause_Content + @"</div></div>";
                        }
                    }
                    if (legalLegislationSignatureGet != null)
                    {
                        foreach (var item in legalLegislationSignatureGet)
                        {
                            signatures += @"<div class=""row Signaturetext"" Style=""word-break: break-word;"" ><div class=""col-md-12"">
                        <p class=""legalSignature"">" + item.Full_Name + @" </p>
                        <p class=""legalSignatureTitle""> " + item.Job_Title + @" </p></div></div>";
                        }
                    }
                    if (legalExplanatoryNotesGet != null)
                    {
                        foreach (var item in legalExplanatoryNotesGet)
                        {
                            explanatoryNote += @" <p class=""legalSignature"">" + item.ExplanatoryNote_Body + @"</p> ";
                        }
                    }
                    if (legalNotesGet != null)
                    {
                        foreach (var item in legalNotesGet)
                        {
                            Note += @"" + translationState.Translate("Note") + @"<p class=""legalSignature"">" + item.Note_Text + @"</p> ";
                        }
                    }
                    if (legalPublicationSourcesGet != null)
                    {
                        foreach (var item in legalPublicationSourcesGet)
                        {
                            source += @"<div class=""row"" Style=""word-break: break-word;""><div class=""col-md-12""><div class=""col-md-6""><p>" + translationState.Translate("Legislation_Preview_Footer1") + @"
                    " + item.Issue_Number + @"</p>
                    <p>" + translationState.Translate("at") + @": " + item.PublicationDate.ToString("dd/MM/yyyy") + @"</p></div></div>
                    </div>";
                        }
                    }
                }
                await InvokeAsync(StateHasChanged);

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        #region Form Submit
        protected async Task FormSubmit(LegalLegislationDecisionVM args)
        {
            try
            {

                bool? dialogResponse = await dialogService.Confirm(
                   translationState.Translate("Are_you_sure_you_want_to_save_this_change"),
                   translationState.Translate("Confirm"),
                   new ConfirmOptions()
                   {
                       OkButtonText = @translationState.Translate("OK"),
                       CancelButtonText = @translationState.Translate("Cancel")
                   });
                if (dialogResponse == true)
                {
                    var responseModel = await legalLegislationService.GetLegalLegislationDetailsByUsingLegislationIdForEditForm(args.LegislationId);
                    if (responseModel.IsSuccessStatusCode)
                    {
                        legalLegislation = (LegalLegislation)responseModel.ResultData;
                    }
                    legalLegislation.Legislation_Flow_Status = (int)args.FlowStatusId;

                    legalLegislationDecisionVM.ModifiedBy = loginState.UserDetail.UserName;
                    var response = await legalLegislationService.UpdateLegalLegislationDecision(legalLegislationDecisionVM);
                    if (legalLegislationDecisionVM.FlowStatusId != (int)LegislationFlowStatusEnum.Unpublished)
                    {
                        legalLegislation.SenderEmail = loginState.UserDetail.UserName;
                        await workflowService.ProcessWorkflowActvivities(legalLegislation, (int)WorkflowModuleEnum.LDSDocument, (int)WorkflowModuleTriggerEnum.UserSubmitsDocument);

                    }

                    if (response.IsSuccessStatusCode)
                    {

                        if (FromPage == "1") // approve, need to modify
                        {
                            if (legalLegislationDecisionVM.FlowStatusId == (int)LegislationFlowStatusEnum.NeedToModify)
                            {
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Success,
                                    Detail = translationState.Translate("legislation_NeedToModify_Success_Message"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                            }
                            else if (legalLegislationDecisionVM.FlowStatusId == (int)LegislationFlowStatusEnum.Approved)
                            {
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Success,
                                    Detail = translationState.Translate("legislation_Approved_Success_Message"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                            }
                            navigationManager.NavigateTo("/legallegislation-review");
                        }
                        else if (FromPage == "2") // publish, need to modify
                        {
                            if (legalLegislationDecisionVM.FlowStatusId == (int)LegislationFlowStatusEnum.NeedToModify)
                            {
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Success,
                                    Detail = translationState.Translate("legislation_NeedToModify_Success_Message"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                            }
                            else if (legalLegislationDecisionVM.FlowStatusId == (int)LegislationFlowStatusEnum.Published)
                            {
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Success,
                                    Detail = translationState.Translate("legislation_Published_Success_Message"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                            }
                            navigationManager.NavigateTo("/legallegislation-approve");
                        }
                        else if (FromPage == "3") // unpublish
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("legislation_UnPublished_Success_Message"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            navigationManager.NavigateTo("/legallegislation-publish-unpublish");
                        }

                        StateHasChanged();
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    //if (legalLegislationDecisionVM.FlowStatusId == (int)LegislationFlowStatusEnum.Published || legalLegislationDecisionVM.FlowStatusId == (int)LegislationFlowStatusEnum.Unpublished || legalLegislationDecisionVM.FlowStatusId == (int)LegislationFlowStatusEnum.Approved)
                    //{
                    //    navigationManager.NavigateTo("/legallegislation-publish-unpublish");
                    //}
                    //else
                    //{
                    //    navigationManager.NavigateTo("/legallegislation-review");
                    //}


                }

                else
                {
                    await Load();
                }
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Decision_could_not_be_updated"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }

        }


        #endregion

        //<History Author = 'Nadia Gull' Version="1.0" Branch="master">Populate Legislation FlowStatus</History>
        #region Validation
        protected async void OnChangeAddComment()
        {
            if (legalLegislationDecisionVM.FlowStatusId == (int)LegislationFlowStatusEnum.SendAComment)
            {
                isCommentRequired = true;
            }
            else
            {
                isCommentRequired = false;
            }
            StateHasChanged();
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
        protected async Task ButtonCancelClick(MouseEventArgs args)
        {

            if (legalLegislationDecisionVM.FlowStatusId == (int)LegislationFlowStatusEnum.Published || legalLegislationDecisionVM.FlowStatusId == (int)LegislationFlowStatusEnum.Unpublished || legalLegislationDecisionVM.FlowStatusId == (int)LegislationFlowStatusEnum.Approved)
            {
                if (await dialogService.Confirm(translationState.Translate("Sure_Cancel"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    navigationManager.NavigateTo("/legallegislation-publish-unpublish");
                }
            }
            else
            {
                if (await dialogService.Confirm(translationState.Translate("Sure_Cancel"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    navigationManager.NavigateTo("/legallegislation-review");
                }
            }
        }


        #endregion

    }
}
