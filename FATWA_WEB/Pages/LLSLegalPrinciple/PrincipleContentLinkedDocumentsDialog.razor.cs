using FATWA_DOMAIN.Models.ViewModel.LLSLegalPrinciple;
using FATWA_DOMAIN.Models.WorkflowModels;
using FATWA_GENERAL.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Syncfusion.Blazor.PdfViewer;
using Syncfusion.Blazor.PdfViewerServer;
using System.Security.Cryptography;
using Telerik.DataSource.Extensions;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.LegalPrinciple.LegalPrincipleEnum;

namespace FATWA_WEB.Pages.LLSLegalPrinciple
{
    public partial class PrincipleContentLinkedDocumentsDialog : ComponentBase
    {
        #region Parameter

        [Parameter]
        public dynamic PrincipleContentId { get; set; }
        [Parameter]
        public int LLSDocumentPageNumber { get; set; }
        [Parameter]
        public EventCallback<int> LLSDocumentPageNumberChanged { get; set; }

        #endregion

        #region Constructor
        public PrincipleContentLinkedDocumentsDialog()
        {
            ToolbarSettings = new PdfViewerToolbarSettings()
            {
                ToolbarItems = new List<Syncfusion.Blazor.PdfViewer.ToolbarItem>()
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
            DocumentPath = string.Empty;
            DownloadFileName = string.Empty;
            password = string.Empty;
            AppealSupremeFileGrid = new RadzenDataGrid<LLSLegalPrincipleAppealSupremenContentLinkedDocVM>();
            AppealSupremeLinkedDocuments = new List<LLSLegalPrincipleAppealSupremenContentLinkedDocVM>();
            LegalAdviceLinkedDocuments = new List<LLSLegalPrincipleLegalAdviceContentLinkedDocVM>();
            KuwaitAlYoumLinkedDocuments = new List<LLSLegalPrincipleKuwaitAlYoumContentLinkedDocVM>();
            OthersLinkedDocuments = new List<LLSLegalPrincipleOthersContentLinkedDocVM>();
        }

        #endregion

        #region Variables Declarations

        private List<ToolbarItem> ToolbarItems = new List<ToolbarItem>();

        public SfPdfViewerServer pdfViewer = new SfPdfViewerServer();
        public PdfViewerToolbarSettings ToolbarSettings;
        public string DocumentPath { get; set; }
        public string DownloadFileName { get; set; }
        public LLSLegalPrinciplContentLinkedDocumentVM FileNameResult { get; set; }

        //Encryption/Descyption Key
        public string password;
        private System.Text.UnicodeEncoding UE;
        public byte[] key;
        private RijndaelManaged RMCrypto;
        private MemoryStream fsOut;
        public int data = 0;

        public byte[] FileData { get; set; }
        public RadzenDataGrid<LLSLegalPrincipleAppealSupremenContentLinkedDocVM> AppealSupremeFileGrid { get; set; }
        public DateTime Max = new DateTime(DateTime.Now.Date.Ticks);
        public DateTime Min = new DateTime(1900, 1, 1);

        //public List<LLSLegalPrinciplContentLinkedDocumentVM> LinkedDocuments { get; set; }
        public LLSLegalPrincipleLinkedDocVM LinkedDocuments { get; set; }

        public List<LLSLegalPrincipleAppealSupremenContentLinkedDocVM> AppealSupremeLinkedDocuments { get; set; }
        public List<LLSLegalPrincipleLegalAdviceContentLinkedDocVM> LegalAdviceLinkedDocuments { get; set; }
        public List<LLSLegalPrincipleKuwaitAlYoumContentLinkedDocVM> KuwaitAlYoumLinkedDocuments { get; set; }
        public List<LLSLegalPrincipleOthersContentLinkedDocVM> OthersLinkedDocuments { get; set; }
        private int SelectedGrid;
        private bool isNextDisabled;
        private bool isPreviousDisabled = true;
        private int AppealLinkedDocCount;
        private int SupremeLinkedDocCount;
        private int LegalAdviceLinkedDocCount;
        private int KuwaitAlYoumLinkedDocCount;
        private int OthersLinkedDocCount;

        #endregion

        #region OnInitialize

        protected override async Task OnInitializedAsync()
        {
            if (PrincipleContentId is not null)
                PrincipleContentId = Guid.Parse(PrincipleContentId.ToString());
            spinnerService.Show();
            await Load();
            spinnerService.Hide();
        }

        #endregion OnInitialize

        #region Load

        private async Task Load()
        {
            SelectedGrid = (int)JudgementsTabsEnums.AppealJudgements;
            await GetLLSLegalPrincipleContentLinkedDocuments();
            await LoadAttachment();
        }

        #endregion

        #region Functions

        private async Task GetLLSLegalPrincipleContentLinkedDocuments()
        {
            var response = await lLSLegalPrincipleService.GetLLSLegalPrincipleContentLinkedDocuments(PrincipleContentId);
            if (response.IsSuccessStatusCode)
            {
                LinkedDocuments = (LLSLegalPrincipleLinkedDocVM)response.ResultData;
                if (LinkedDocuments.AppealSupremenLinkedDocuments.Count() != 0)
                {
                    AppealSupremeLinkedDocuments = LinkedDocuments.AppealSupremenLinkedDocuments;
                    AppealLinkedDocCount = AppealSupremeLinkedDocuments.Where(x => x.CourtTypeId == (int)CourtTypeEnum.Appeal).ToList().Count;
                    SupremeLinkedDocCount = AppealSupremeLinkedDocuments.Where(x => x.CourtTypeId == (int)CourtTypeEnum.Supreme).ToList().Count;
                }
                if (LinkedDocuments.LegalAdviceLinkedDocuments.Count() != 0)
                {
                    LegalAdviceLinkedDocuments = LinkedDocuments.LegalAdviceLinkedDocuments;
                    LegalAdviceLinkedDocCount = LegalAdviceLinkedDocuments.Count;
                }
                if (LinkedDocuments.KuwaitAlYoumLinkedDocuments.Count() != 0)
                {
                    KuwaitAlYoumLinkedDocuments = LinkedDocuments.KuwaitAlYoumLinkedDocuments;
                    KuwaitAlYoumLinkedDocCount = KuwaitAlYoumLinkedDocuments.Count;
                }
                if (LinkedDocuments.OthersLinkedDocuments.Count() != 0)
                {
                    OthersLinkedDocuments = LinkedDocuments.OthersLinkedDocuments;
                    OthersLinkedDocCount = OthersLinkedDocuments.Count;
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        private async Task LoadAttachment()
        {
            if (AppealLinkedDocCount > 0)
            {
                if (SupremeLinkedDocCount > 0 || LegalAdviceLinkedDocCount > 0 || KuwaitAlYoumLinkedDocCount > 0 || OthersLinkedDocCount > 0)
                    isNextDisabled = false;
                else
                    isNextDisabled = true;

                await OnGridViewClick(AppealSupremeLinkedDocuments.FirstOrDefault());
            }
            else if (SupremeLinkedDocCount > 0)
            {
                SelectedGrid = (int)JudgementsTabsEnums.SupremeJudgements;
                if (LegalAdviceLinkedDocCount > 0 || KuwaitAlYoumLinkedDocCount > 0 || OthersLinkedDocCount > 0)
                    isNextDisabled = false;
                else
                    isNextDisabled = true;
                await OnGridViewClick(AppealSupremeLinkedDocuments.FirstOrDefault());
            }
            else if (LegalAdviceLinkedDocuments.Count > 0)
            {
                SelectedGrid = (int)JudgementsTabsEnums.LegalAdvice;
                if (KuwaitAlYoumLinkedDocCount > 0 || OthersLinkedDocCount > 0)
                    isNextDisabled = false;
                else
                    isNextDisabled = true;
                await OnGridViewClick(LegalAdviceLinkedDocuments.FirstOrDefault());
            }
            else if (KuwaitAlYoumLinkedDocCount > 0)
            {
                SelectedGrid = (int)JudgementsTabsEnums.KuwaitAlYawm;
                if (OthersLinkedDocCount > 0)
                    isNextDisabled = false;
                else
                    isNextDisabled = true;
                await OnGridViewClick(KuwaitAlYoumLinkedDocuments.FirstOrDefault());
            }
            else if (OthersLinkedDocCount > 0)
            {
                SelectedGrid = (int)JudgementsTabsEnums.Others;
                isNextDisabled = true;
                await OnGridViewClick(OthersLinkedDocuments.FirstOrDefault());
            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("No_Linked_Document_Found"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                dialogService.Close(null);
            }
        }

        private dynamic SetViewAttachmentList(int SelectedGrid, int referenceId)
        {
            switch (SelectedGrid)
            {
                case (int)JudgementsTabsEnums.AppealJudgements:
                    return AppealSupremeLinkedDocuments.Where(x => x.ReferenceId == referenceId && x.CourtTypeId == (int)CourtTypeEnum.Appeal).FirstOrDefault();

                case (int)JudgementsTabsEnums.SupremeJudgements:
                    return AppealSupremeLinkedDocuments.Where(x => x.ReferenceId == referenceId && x.CourtTypeId == (int)CourtTypeEnum.Supreme).FirstOrDefault();

                case (int)JudgementsTabsEnums.LegalAdvice:
                    return LegalAdviceLinkedDocuments.Where(x => x.ReferenceId == referenceId).FirstOrDefault();

                case (int)JudgementsTabsEnums.KuwaitAlYawm:
                    return KuwaitAlYoumLinkedDocuments.Where(x => x.ReferenceId == referenceId).FirstOrDefault();

                case (int)JudgementsTabsEnums.Others:
                    return OthersLinkedDocuments.Where(x => x.ReferenceId == referenceId).FirstOrDefault();

                default:
                    return null;
            }
        }

        #endregion 

        #region On Button Click
        async void OnClickNext()
        {
            switch (SelectedGrid)
            {
                case (int)JudgementsTabsEnums.AppealJudgements:
                    if (SupremeLinkedDocCount > 0)
                    {
                        SelectedGrid = (int)JudgementsTabsEnums.SupremeJudgements;
                        if (LegalAdviceLinkedDocCount > 0 || KuwaitAlYoumLinkedDocCount > 0 || OthersLinkedDocCount > 0)
                            isNextDisabled = false;
                        else
                            isNextDisabled = true;
                        isPreviousDisabled = false;
                        await OnGridViewClick(AppealSupremeLinkedDocuments.FirstOrDefault());
                    }
                    else if (LegalAdviceLinkedDocCount > 0)
                    {
                        SelectedGrid = (int)JudgementsTabsEnums.LegalAdvice;
                        isPreviousDisabled = false;
                        if (KuwaitAlYoumLinkedDocCount > 0 || OthersLinkedDocCount > 0)
                            isNextDisabled = false;
                        else
                            isNextDisabled = true;
                        await OnGridViewClick(LegalAdviceLinkedDocuments.FirstOrDefault());
                    }
                    else if (KuwaitAlYoumLinkedDocCount > 0)
                    {
                        SelectedGrid = (int)JudgementsTabsEnums.KuwaitAlYawm;
                        isPreviousDisabled = false;
                        if (OthersLinkedDocCount > 0)
                            isNextDisabled = false;
                        else
                            isNextDisabled = true; await OnGridViewClick(KuwaitAlYoumLinkedDocuments.FirstOrDefault());
                    }
                    else if (OthersLinkedDocCount > 0)
                    {
                        SelectedGrid = (int)JudgementsTabsEnums.Others;
                        isPreviousDisabled = false;
                        isNextDisabled = true;
                        await OnGridViewClick(OthersLinkedDocuments.FirstOrDefault());
                    }
                    else
                        return;
                    break;

                case (int)JudgementsTabsEnums.SupremeJudgements:

                    if (LegalAdviceLinkedDocCount > 0)
                    {
                        SelectedGrid = (int)JudgementsTabsEnums.LegalAdvice;
                        isPreviousDisabled = false;

                        if (KuwaitAlYoumLinkedDocCount > 0 || OthersLinkedDocCount > 0)
                            isNextDisabled = false;
                        else
                            isNextDisabled = true;

                        await OnGridViewClick(LegalAdviceLinkedDocuments.FirstOrDefault());
                    }
                    else if (KuwaitAlYoumLinkedDocCount > 0)
                    {
                        SelectedGrid = (int)JudgementsTabsEnums.KuwaitAlYawm;
                        isPreviousDisabled = false;
                        if (OthersLinkedDocCount == 0)
                            isNextDisabled = true;
                        else
                            isNextDisabled = false;
                        await OnGridViewClick(KuwaitAlYoumLinkedDocuments.FirstOrDefault());
                    }
                    else if (OthersLinkedDocCount > 0)
                    {
                        SelectedGrid = (int)JudgementsTabsEnums.Others;
                        isPreviousDisabled = false;
                        isNextDisabled = true;
                        await OnGridViewClick(OthersLinkedDocuments.FirstOrDefault());
                    }
                    else
                        return;
                    break;

                case (int)JudgementsTabsEnums.LegalAdvice:

                    if (KuwaitAlYoumLinkedDocCount > 0)
                    {
                        SelectedGrid = (int)JudgementsTabsEnums.KuwaitAlYawm;
                        isPreviousDisabled = false;
                        if (OthersLinkedDocCount == 0)
                            isNextDisabled = true;
                        else
                            isNextDisabled = false;
                        await OnGridViewClick(KuwaitAlYoumLinkedDocuments.FirstOrDefault());
                    }
                    else if (OthersLinkedDocCount > 0)
                    {
                        SelectedGrid = (int)JudgementsTabsEnums.Others;
                        isPreviousDisabled = false;
                        isNextDisabled = true;
                        await OnGridViewClick(OthersLinkedDocuments.FirstOrDefault());
                    }
                    else
                        return;
                    break;

                case (int)JudgementsTabsEnums.KuwaitAlYawm:

                    if (OthersLinkedDocCount > 0)
                    {
                        SelectedGrid = (int)JudgementsTabsEnums.Others;
                        isPreviousDisabled = false;
                        isNextDisabled = true;
                        await OnGridViewClick(OthersLinkedDocuments.FirstOrDefault());
                    }
                    else
                        return;
                    break;

            }
        }

        async void OnClickPrevious()
        {
            switch (SelectedGrid)
            {
                case (int)JudgementsTabsEnums.SupremeJudgements:
                    if (AppealLinkedDocCount > 0)
                    {
                        isPreviousDisabled = true;
                        isNextDisabled = false;
                        SelectedGrid = (int)JudgementsTabsEnums.AppealJudgements;
                        await OnGridViewClick(AppealSupremeLinkedDocuments.FirstOrDefault());
                    }
                    else
                        return;

                    break;

                case (int)JudgementsTabsEnums.LegalAdvice:
                    if (SupremeLinkedDocCount > 0)
                    {
                        if (AppealLinkedDocCount > 0)
                            isPreviousDisabled = false;
                        else
                            isPreviousDisabled = true;
                        isNextDisabled = false;
                        SelectedGrid = (int)JudgementsTabsEnums.SupremeJudgements;
                        await OnGridViewClick(AppealSupremeLinkedDocuments.FirstOrDefault());
                    }

                    else if (AppealLinkedDocCount > 0)
                    {
                        isPreviousDisabled = true;
                        isNextDisabled = false;
                        SelectedGrid = (int)JudgementsTabsEnums.AppealJudgements;
                        await OnGridViewClick(AppealSupremeLinkedDocuments.FirstOrDefault());
                    }
                    else
                        return;
                    break;

                case (int)JudgementsTabsEnums.KuwaitAlYawm:
                    if (LegalAdviceLinkedDocCount > 0)
                    {
                        if (SupremeLinkedDocCount > 0 || AppealLinkedDocCount > 0)
                            isPreviousDisabled = false;
                        else
                            isPreviousDisabled = true;
                        isNextDisabled = false;
                        SelectedGrid = (int)JudgementsTabsEnums.LegalAdvice;
                        await OnGridViewClick(LegalAdviceLinkedDocuments.FirstOrDefault());
                    }
                    else if (SupremeLinkedDocCount > 0)
                    {
                        if (AppealLinkedDocCount > 0)
                            isPreviousDisabled = false;
                        else
                            isPreviousDisabled = true;
                        isNextDisabled = false;
                        SelectedGrid = (int)JudgementsTabsEnums.SupremeJudgements;
                        await OnGridViewClick(AppealSupremeLinkedDocuments.FirstOrDefault());
                    }
                    else if (AppealLinkedDocCount > 0)
                    {
                        isPreviousDisabled = true;
                        isNextDisabled = false;
                        SelectedGrid = (int)JudgementsTabsEnums.AppealJudgements;
                        await OnGridViewClick(AppealSupremeLinkedDocuments.FirstOrDefault());
                    }
                    else
                        return;
                    break;

                case (int)JudgementsTabsEnums.Others:
                    if (KuwaitAlYoumLinkedDocCount > 0)
                    {
                        if (LegalAdviceLinkedDocCount > 0 || SupremeLinkedDocCount > 0 || AppealLinkedDocCount > 0)
                            isPreviousDisabled = false;
                        else
                            isPreviousDisabled = true;
                        isNextDisabled = false;
                        SelectedGrid = (int)JudgementsTabsEnums.KuwaitAlYawm;
                        await OnGridViewClick(KuwaitAlYoumLinkedDocuments.FirstOrDefault());
                    }

                    else if (LegalAdviceLinkedDocCount > 0)
                    {
                        if (SupremeLinkedDocCount > 0 || AppealLinkedDocCount > 0)
                            isPreviousDisabled = false;
                        else
                            isPreviousDisabled = true;
                        isNextDisabled = false;
                        SelectedGrid = (int)JudgementsTabsEnums.LegalAdvice;
                        await OnGridViewClick(LegalAdviceLinkedDocuments.FirstOrDefault());
                    }
                    else if (SupremeLinkedDocCount > 0)
                    {
                        if (AppealLinkedDocCount > 0)
                            isPreviousDisabled = false;
                        else
                            isPreviousDisabled = true;
                        isNextDisabled = false;
                        SelectedGrid = (int)JudgementsTabsEnums.SupremeJudgements;
                        await OnGridViewClick(AppealSupremeLinkedDocuments.FirstOrDefault());
                    }
                    else if (AppealLinkedDocCount > 0)
                    {
                        isPreviousDisabled = true;
                        isNextDisabled = false;
                        SelectedGrid = (int)JudgementsTabsEnums.AppealJudgements;
                        await OnGridViewClick(AppealSupremeLinkedDocuments.FirstOrDefault());
                    }
                    else
                        return;
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region Document grid view action also check document masking

        protected async Task OnGridViewClick(dynamic args)
        {
            try
            {
                var viewfile = SetViewAttachmentList(SelectedGrid, args.ReferenceId);
                if (viewfile != null)
                {
                    await LLSDocumentPageNumberChanged.InvokeAsync(viewfile.PageNumber);
                    string physicalPath = string.Empty;
#if DEBUG
                    {
                        physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + args.StoragePath).Replace(@"\\", @"\");

                    }
#else
                {
                    if (SelectedGrid == (int)JudgementsTabsEnums.KuwaitAlYawm)
                    {
                        if (PrincipleContentId != null)
						{
                            physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + viewfile.StoragePath).Replace(@"\\", @"\");
							physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
                        }
						else
						{
                            physicalPath = Path.Combine(_config.GetValue<string>("kay_file_path") + viewfile.StoragePath).Replace(@"\\", @"\");
							physicalPath = physicalPath.Replace(_config.GetValue<string>("KayPublicationsPath"), "");
                        }
                    }
                    else
                    {
                        physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + viewfile.StoragePath).Replace(@"\\", @"\");
                        physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
                    }
                }
#endif
                    if (!string.IsNullOrEmpty(physicalPath))
                    {
                        string base64String = await DocumentEncryptionService.GetDecryptedDocumentBase64(physicalPath, viewfile.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
                        DocumentPath = "data:application/pdf;base64," + base64String;
                        DownloadFileName = viewfile.UploadedDocumentId.ToString();
                        await Task.Delay(200);
                        await pdfViewer.LoadAsync(DocumentPath);
                        StateHasChanged();
                    }
                    else
                    {
                        FileData = null;
                        DownloadFileName = null;
                        DocumentPath = null;
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Judgment_Document_Not_Loaded"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                }
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        #endregion 

        protected async Task DocumentLoaded(LoadEventArgs args)
        {
            await JSRuntime.InvokeVoidAsync("addRotateButtonToPdfToolbar");
        }

    }
}
