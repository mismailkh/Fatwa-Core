using FATWA_DOMAIN.Models.LLSLegalPrinciple;
using FATWA_DOMAIN.Models.ViewModel;
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
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.LLSLegalPrinciple
{
    public partial class LinkLLSLegalPrincipleContents : ComponentBase
    {
        #region Constructure
        public LinkLLSLegalPrincipleContents()
        {
            SourceDocument = new List<LLSLegalPrincipleDocumentVM>();
            LegalAdviceSourceDocument = new List<LLSLegalPrinciplLegalAdviceDocumentVM>();
            KuwaitAlYawmDocument = new List<LLSLegalPrincipleKuwaitAlYoumDocuments>();
            OtherDocument = new List<LLSLegalPrinciplOtherDocumentVM>();
            ToolbarSettings = new PdfViewerToolbarSettings()
            {
                ToolbarItems = new List<Syncfusion.Blazor.PdfViewer.ToolbarItem>()
                        {
                            Syncfusion.Blazor.PdfViewer.ToolbarItem.MagnificationTool,
                            Syncfusion.Blazor.PdfViewer.ToolbarItem.SelectionTool,
                            Syncfusion.Blazor.PdfViewer.ToolbarItem.UndoRedoTool,
                            Syncfusion.Blazor.PdfViewer.ToolbarItem.SearchOption,
                            Syncfusion.Blazor.PdfViewer.ToolbarItem.PanTool,
                            Syncfusion.Blazor.PdfViewer.ToolbarItem.AnnotationEditTool,
                        },
                AnnotationToolbarItems = new List<AnnotationToolbarItem>()
                        {
                            AnnotationToolbarItem.ShapeTool,
                            AnnotationToolbarItem.ColorEditTool
                        }
            };
            DocumentPath = string.Empty;
            DownloadFileName = string.Empty;
            FileNameResult = new LLSLegalPrincipleDocumentVM();
            password = string.Empty;
            gridRelation = new RadzenDataGrid<LLSLegalPrinciplesRelationVM>();
            activeworkflowlist = new List<WorkflowVM>();
            copySourceDoc = 0;
            IsMaskSourceJudgment = false;
            FilteredAppealJudgementDocuments = new List<LLSLegalPrincipleDocumentVM>();
            FilteredSupremeJudgementDocuments = new List<LLSLegalPrincipleDocumentVM>();
            FilteredLegalAdviceDocuments = new List<LLSLegalPrinciplLegalAdviceDocumentVM>();
            FilteredOthersJudgementDocuments = new List<LLSLegalPrinciplOtherDocumentVM>();
            FilteredKayDocumentList = new List<LLSLegalPrincipleKuwaitAlYoumDocuments>();
            SelectedGrid = 0;
            advanceSearch = new LLSLegalPrincipalDocumentSearchVM();
            ShowSourceDocumentGrid = false;
        }

        #endregion Constructure

        #region Parameter

        [Parameter]
        public dynamic JudmentDocumentId { get; set; }
        [Parameter]
        public dynamic selectedTabIndex { get; set; }

        #endregion Parameter

        #region Variables declaration

        protected RadzenDataGrid<LLSLegalPrincipleContent>? grid = new RadzenDataGrid<LLSLegalPrincipleContent>();
        protected List<LLSLegalPrincipleDocumentVM> SourceDocument { get; set; }
        protected List<LLSLegalPrinciplLegalAdviceDocumentVM> LegalAdviceSourceDocument { get; set; }
        protected List<LLSLegalPrincipleKuwaitAlYoumDocuments> KuwaitAlYawmDocument { get; set; }
        protected List<LLSLegalPrinciplOtherDocumentVM> OtherDocument { get; set; }
        protected SfPdfViewerServer pdfViewer;
        protected PdfViewerToolbarSettings ToolbarSettings;
        protected string DocumentPath { get; set; }
        protected string DownloadFileName { get; set; }
        protected dynamic FileNameResult { get; set; }

        //Encryption/Descyption Key
        protected string password;

        private System.Text.UnicodeEncoding UE;
        public byte[] key;
        public byte[] FileData { get; set; }

        private RijndaelManaged RMCrypto;
        private MemoryStream fsOut;
        protected int data = 0;
        protected DateTime Max = new DateTime(DateTime.Now.Date.Ticks);
        protected DateTime Min = new DateTime(1900, 1, 1);
        protected RadzenDataGrid<LLSLegalPrinciplesRelationVM> gridRelation { get; set; }
        public List<WorkflowVM> activeworkflowlist { get; set; }
        public int copySourceDoc { get; set; }
        protected bool IsMaskSourceJudgment { get; set; }
        public List<LLSLegalPrincipleDocumentVM> FilteredAppealJudgementDocuments { get; set; }
        public List<LLSLegalPrincipleDocumentVM> FilteredSupremeJudgementDocuments { get; set; }
        public List<LLSLegalPrinciplLegalAdviceDocumentVM> FilteredLegalAdviceDocuments { get; set; }
        public List<LLSLegalPrinciplOtherDocumentVM> FilteredOthersJudgementDocuments { get; set; }
        public IEnumerable<LLSLegalPrincipleKuwaitAlYoumDocuments> FilteredKayDocumentList { get; set; }
        List<LLSLegalPrincipleContentCategoriesVM> PrincipleCategories { get; set; } = new List<LLSLegalPrincipleContentCategoriesVM>();
        protected LLSLegalPrinciplesRelationVM PrincipleContentSearch { get; set; } = new LLSLegalPrinciplesRelationVM();
        protected List<LLSLegalPrinciplesRelationVM> PrincipleContents { get; set; } = new List<LLSLegalPrinciplesRelationVM>();
        protected IList<LLSLegalPrinciplesRelationVM> selectedPrincipleContents { get; set; } = new List<LLSLegalPrinciplesRelationVM>();
        protected RadzenDataGrid<LLSLegalPrinciplesRelationVM>? PrincipleContentGrid = new RadzenDataGrid<LLSLegalPrinciplesRelationVM>();
        protected bool allowRowSelectOnRowClick = false;
        private bool isAll = false;
        private int SelectedGrid;
        protected LLSLegalPrincipalDocumentSearchVM advanceSearch { get; set; }
        protected bool ShowSourceDocumentGrid { get; set; }

        #endregion Variables declaration

        #region Component load

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            JudmentDocumentId = int.Parse(JudmentDocumentId);
            await Load();
            await Task.Delay(1500);
            spinnerService.Hide();
        }

        #endregion Component load

        #region Principle Content Linked Documents Dialog

        protected async Task ViewLinkedDocments(Guid principleContentId)
        {
            var result = await dialogService.OpenAsync<PrincipleContentLinkedDocumentsDialog>(translationState.Translate("Principle_Content_Linked_Documents"),
                            new Dictionary<string, object>()
                            {
                                { "PrincipleContentId", principleContentId }
                            },
                            new DialogOptions() { Width = "80% !important", CloseDialogOnOverlayClick = false });
        }

        #endregion Principle Content Linked Documents Dialog

        #region Load

        private async Task Load()
        {
            if (dataCommunicationService.selectedLegalPrincipleDocumentTab == null)
            {
                dataCommunicationService.selectedLegalPrincipleDocumentTab = Convert.ToInt32(selectedTabIndex);
            }
            switch (dataCommunicationService.selectedLegalPrincipleDocumentTab)
            {
                case (int)JudgementsTabsEnums.AppealJudgements:
                    SelectedGrid = (int)JudgementsTabsEnums.AppealJudgements;
                    if (dataCommunicationService.selectedJudmentDocumentsList.StoragePath == null)
                    {
                        await GetLLSLegalPrincipleSourceDocuments();
                    }
                    if (FilteredAppealJudgementDocuments.Any())
                    {
                        dataCommunicationService.selectedJudmentDocumentsList = FilteredAppealJudgementDocuments.Where(x => x.UploadedDocumentId == Convert.ToInt32(JudmentDocumentId)).FirstOrDefault();
                    }
                    SourceDocument.Add(dataCommunicationService.selectedJudmentDocumentsList);
                    await GetSourceJudementDocumentLoad(SourceDocument);
                    ShowSourceDocumentGrid = true;
                    break;

                case (int)JudgementsTabsEnums.SupremeJudgements:
                    SelectedGrid = (int)JudgementsTabsEnums.SupremeJudgements;
                    if (dataCommunicationService.selectedJudmentDocumentsList.StoragePath == null)
                    {
                        await GetLLSLegalPrincipleSourceDocuments();
                    }
                    if (FilteredSupremeJudgementDocuments.Any())
                    {
                        dataCommunicationService.selectedJudmentDocumentsList = FilteredSupremeJudgementDocuments.Where(x => x.UploadedDocumentId == Convert.ToInt32(JudmentDocumentId)).FirstOrDefault();
                    }
                    SourceDocument.Add(dataCommunicationService.selectedJudmentDocumentsList);
                    await GetSourceJudementDocumentLoad(SourceDocument);
                    ShowSourceDocumentGrid = true;
                    break;

                case (int)JudgementsTabsEnums.LegalAdvice:
                    SelectedGrid = (int)JudgementsTabsEnums.LegalAdvice;
                    if (dataCommunicationService.selectedLegalAdviceDocument.StoragePath == null)
                    {
                        await GetLLSLegalPrincipleLegalAdviceSourceDocuments();
                    }
                    if (FilteredLegalAdviceDocuments.Any())
                    {
                        dataCommunicationService.selectedLegalAdviceDocument = FilteredLegalAdviceDocuments.Where(x => x.UploadedDocumentId == Convert.ToInt32(JudmentDocumentId)).FirstOrDefault();
                    }
                    LegalAdviceSourceDocument.Add(dataCommunicationService.selectedLegalAdviceDocument);
                    await GetSourceJudementDocumentLoad(LegalAdviceSourceDocument);
                    ShowSourceDocumentGrid = true;
                    break;

                case (int)JudgementsTabsEnums.KuwaitAlYawm:
                    SelectedGrid = (int)JudgementsTabsEnums.KuwaitAlYawm;
                    if (dataCommunicationService.selectedKuwaitAlYawmDocument.StoragePath == null)
                    {
                        await PopulateKayDocumentsGrid();
                    }
                    if (FilteredKayDocumentList.Any())
                    {
                        dataCommunicationService.selectedKuwaitAlYawmDocument = FilteredKayDocumentList.Where(x => x.UploadedDocumentId == Convert.ToInt32(JudmentDocumentId)).FirstOrDefault();
                    }
                    KuwaitAlYawmDocument.Add(dataCommunicationService.selectedKuwaitAlYawmDocument);
                    await GetSourceJudementDocumentLoad(KuwaitAlYawmDocument);
                    ShowSourceDocumentGrid = true;
                    break;

                case (int)JudgementsTabsEnums.Others:
                    SelectedGrid = (int)JudgementsTabsEnums.Others;
                    if (dataCommunicationService.selectedOtherDocument.StoragePath == null)
                    {
                        await GetLLSLegalPrincipleOtherSourceDocuments();
                    }
                    if (FilteredOthersJudgementDocuments.Any())
                    {
                        dataCommunicationService.selectedOtherDocument = FilteredOthersJudgementDocuments.Where(x => x.UploadedDocumentId == Convert.ToInt32(JudmentDocumentId)).FirstOrDefault();
                    }
                    OtherDocument.Add(dataCommunicationService.selectedOtherDocument);
                    await GetSourceJudementDocumentLoad(OtherDocument);
                    ShowSourceDocumentGrid = true;
                    break;

                default:
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Judgment_Document_Not_Loaded"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    //navigationManager.NavigateTo("llslegalprincipledocuments-list");
                    return;
            }
        }

        #endregion Load

        #region Get Judgments Functions
        private async Task GetLLSLegalPrincipleSourceDocuments()
        {
            var response = await lLSLegalPrincipleService.GetLLSLegalPrincipleSourceDocuments(advanceSearch);
            if (response.IsSuccessStatusCode)
            {
                var resultLLSLegalPrincipleDocuments = (List<LLSLegalPrincipleDocumentVM>)response.ResultData;
                if (resultLLSLegalPrincipleDocuments.Any())
                {
                    FilteredAppealJudgementDocuments = resultLLSLegalPrincipleDocuments.Where(x => x.CourtTypeId == (int)CourtTypeEnum.Appeal).OrderByDescending(x => x.JudgementDate).ToList();
                    FilteredSupremeJudgementDocuments = resultLLSLegalPrincipleDocuments.Where(x => x.CourtTypeId == (int)CourtTypeEnum.Supreme).OrderByDescending(x => x.JudgementDate).ToList();
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        private async Task GetLLSLegalPrincipleLegalAdviceSourceDocuments()
        {
            var response = await lLSLegalPrincipleService.GetLLSLegalPrincipleLegalAdviceSourceDocuments(advanceSearch);
            if (response.IsSuccessStatusCode)
            {
                FilteredLegalAdviceDocuments = (List<LLSLegalPrinciplLegalAdviceDocumentVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        private async Task GetLLSLegalPrincipleOtherSourceDocuments()
        {
            var response = await lLSLegalPrincipleService.GetLLSLegalPrincipleOtherSourceDocuments(advanceSearch);
            if (response.IsSuccessStatusCode)
            {
                FilteredOthersJudgementDocuments = (List<LLSLegalPrinciplOtherDocumentVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

        #region Kay Document List & Search/Advance Search

        protected async Task PopulateKayDocumentsGrid()
        {
            try
            {
                var response = await lLSLegalPrincipleService.GetKayDocumentsListForLLSLegalPrinciple(advanceSearch);
                if (response.IsSuccessStatusCode)
                {
                    FilteredKayDocumentList = (response.ResultData) as IEnumerable<LLSLegalPrincipleKuwaitAlYoumDocuments>;
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

        #region Link Contents

        protected async Task LinkPrincipleContents()
        {
            if (selectedPrincipleContents.Any(x => x.PageNumber == null || x.PageNumber == 0))
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Page_Number_is_required"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                return;
            }
            bool? dialogResponse = await dialogService.Confirm(
            translationState.Translate("Sure_Link_Principle_Content"),
            translationState.Translate("Link_Principle_Contents"),
            new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("Yes"),
                CancelButtonText = translationState.Translate("No")
            });

            if (dialogResponse == true)
            {
                if (pdfViewer.IsDocumentEdited)
                {
                    await CheckPdfViewerIsDocumentEdited();
                }
                else
                {
                    ApiCallResponse responseCopy = await lLSLegalPrincipleService.CheckCopyDocumentExists(JudmentDocumentId);
                    if (responseCopy.IsSuccessStatusCode)
                    {
                        copySourceDoc = (int)responseCopy.ResultData;
                    }
                }

                if (copySourceDoc == 0)
                {
                    dynamic judgementDocVM = null;
                    if (dataCommunicationService.selectedLegalPrincipleDocumentTab == null)
                    {
                        dataCommunicationService.selectedLegalPrincipleDocumentTab = Convert.ToInt32(selectedTabIndex);
                    }
                    switch (dataCommunicationService.selectedLegalPrincipleDocumentTab)
                    {
                        case (int)JudgementsTabsEnums.AppealJudgements:
                            judgementDocVM = dataCommunicationService.selectedJudmentDocumentsList;
                            break;

                        case (int)JudgementsTabsEnums.SupremeJudgements:
                            judgementDocVM = dataCommunicationService.selectedJudmentDocumentsList;
                            break;

                        case (int)JudgementsTabsEnums.LegalAdvice:
                            judgementDocVM = dataCommunicationService.selectedLegalAdviceDocument;
                            break;

                        case (int)JudgementsTabsEnums.KuwaitAlYawm:
                            judgementDocVM = dataCommunicationService.selectedKuwaitAlYawmDocument;
                            break;

                        case (int)JudgementsTabsEnums.Others:
                            judgementDocVM = dataCommunicationService.selectedOtherDocument;
                            break;

                        default:
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Judgment_Document_Not_Loaded"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            //navigationManager.NavigateTo("llslegalprincipledocuments-list");
                            return;
                    }

                    if (judgementDocVM is not null)
                    {
                        TempAttachementVM Obj = new TempAttachementVM()
                        {
                            MaskedFileData = await pdfViewer.GetDocumentAsync(),
                            IsMaskedAttachment = IsMaskSourceJudgment ? true : false,
                            UploadFrom = "LLSLegalPrincipleSystem",
                            Guid = selectedPrincipleContents.Select(x => x.PrincipleContentId).FirstOrDefault(),
                            DocType = judgementDocVM.DocType,
                            AttachmentTypeId = judgementDocVM.AttachmentTypeId,
                            Description = judgementDocVM.Description
                        };

                        if (dataCommunicationService.selectedLegalPrincipleDocumentTab == (int)JudgementsTabsEnums.KuwaitAlYawm)
                        {
                            Obj.DocumentDate = judgementDocVM.CreatedDate;
                            Obj.FileNameWithoutTimeStamp = judgementDocVM.FileTitle;
                        }
                        else if (dataCommunicationService.selectedLegalPrincipleDocumentTab == (int)JudgementsTabsEnums.Others)
                        {
                            Obj.OtherAttachmentType = judgementDocVM.OtherAttachmentType;
                            Obj.DocumentDate = judgementDocVM.DocumentDate;
                            Obj.FileNameWithoutTimeStamp = judgementDocVM.FileName;
                        }
                        else
                        {
                            Obj.FileNameWithoutTimeStamp = judgementDocVM.FileName;
                        }

                        var result = await fileUploadService.SaveMaskedDocumentInOriginalDocumentFolderForTemparory(Obj);
                        if (result.IsSuccessStatusCode)
                        {
                            var uploadDocument = (TempAttachementVM)result.ResultData;
                            copySourceDoc = (int)uploadDocument.AttachementId;
                        }
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
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
                            Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        return;
                    }
                }

                List<LLSLegalPrincipleContentSourceDocumentReference> linkContents = new List<LLSLegalPrincipleContentSourceDocumentReference>();
                foreach (var item in selectedPrincipleContents)
                {
                    linkContents.Add(new LLSLegalPrincipleContentSourceDocumentReference
                    {
                        PrincipleContentId = item.PrincipleContentId,
                        PageNumber = (int)item.PageNumber,
                        OriginalSourceDocId = Convert.ToInt32(JudmentDocumentId),
                        CopySourceDocId = copySourceDoc,
                        IsMaskedJudgment = IsMaskSourceJudgment ? true : false
                    });
                }
                var response = await lLSLegalPrincipleService.LinkLegalPrincipleContents(linkContents);
                if (response.IsSuccessStatusCode && (bool)response.ResultData)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Link_Existing_Principle_Contents_Successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    await Task.Delay(1000);
                    navigationManager.NavigateTo("/llslegalprincipledocuments-list");
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    return;
                }
            }
            else
            {
                dialogService.Close();
            }
        }

        protected async Task Cancel()
        {
            bool? dialogResponse = await dialogService.Confirm(
            translationState.Translate("Sure_Cancel"),
            translationState.Translate("Cancel"),
            new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("No")
            });
            if (dialogResponse == true)
            {
                navigationManager.NavigateTo("/llslegalprincipledocuments-list");
            }
            else
            {
                dialogService.Close();
            }
        }

        #endregion Link Contents

        #region Load source judgment document

        private async Task GetSourceJudementDocumentLoad(IEnumerable<dynamic> JudmentsDocument)
        {
            try
            {
                FileNameResult = JudmentsDocument.FirstOrDefault();
                if (FileNameResult != null && FileNameResult.StoragePath != null)
                {
                    var physicalPath = string.Empty;
#if DEBUG
                    {
                        if (dataCommunicationService.selectedLegalPrincipleDocumentTab == (int)JudgementsTabsEnums.KuwaitAlYawm)
                            physicalPath = Path.Combine(FileNameResult.StoragePath).Replace(@"\\", @"\");
                        else
                            physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + FileNameResult.StoragePath).Replace(@"\\", @"\");


                    }
#else
                {
                    if (dataCommunicationService.selectedLegalPrincipleDocumentTab == (int)JudgementsTabsEnums.KuwaitAlYawm)
                    {
                        physicalPath = Path.Combine(_config.GetValue<string>("kay_file_path") + FileNameResult.StoragePath).Replace(@"\\", @"\");
                        physicalPath = physicalPath.Replace(_config.GetValue<string>("KayPublicationsPath"), "");
                    }
                    else
                    {
                        physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + FileNameResult.StoragePath).Replace(@"\\", @"\");
                        physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
                    }
                }
#endif
                    if (!string.IsNullOrEmpty(physicalPath))
                    {


                        string base64String = await DocumentEncryptionService.GetDecryptedDocumentBase64(physicalPath, FileNameResult.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
                        DocumentPath = "data:application/pdf;base64," + base64String;
                        DownloadFileName = FileNameResult.UploadedDocumentId.ToString();
                        await Task.Delay(200);
                        await pdfViewer.LoadAsync(DocumentPath);
                        StateHasChanged();


                    }
                    else
                    {
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

        #endregion Load source judgment document

        #region Document grid check document masking

        private async Task CheckPdfViewerIsDocumentEdited()
        {
            try
            {
                if (pdfViewer.IsDocumentEdited)
                {
                    int uploadedDocumentId = Convert.ToInt32(pdfViewer.DownloadFileName);
                    dynamic viewFileDetail;

                    int indexSelected = SelectedGrid != 0 ? SelectedGrid : Convert.ToInt32(selectedTabIndex);

                    viewFileDetail = SetViewAttachmentList(SelectedGrid, uploadedDocumentId);

                    if (viewFileDetail != null)
                    {
                        string[] result = null;
                        string fileName = null;
                        if (SelectedGrid == (int)JudgementsTabsEnums.KuwaitAlYawm)
                        {
                            result = viewFileDetail.FileTitle.Split("_", StringSplitOptions.None);
                        }
                        else
                        {
                            result = viewFileDetail.FileName.Split("_", StringSplitOptions.None);
                        }
                        for (int i = 0; i < result.Length; i++)
                        {
                            if (i == 0)
                            {
                                fileName = result[i];
                            }
                        }

                        TempAttachementVM Obj = new TempAttachementVM()
                        {
                            MaskedFileData = await pdfViewer.GetDocumentAsync(),
                            IsMaskedAttachment = true,
                            UploadFrom = "LLSLegalPrincipleSystem",
                            FileNameWithoutTimeStamp = fileName,
                            Guid = selectedPrincipleContents.Select(x => x.PrincipleContentId).FirstOrDefault(),
                            DocType = viewFileDetail.DocType,
                            AttachmentTypeId = viewFileDetail.AttachmentTypeId,
                            Description = viewFileDetail.Description
                        };

                        var response = await fileUploadService.SaveMaskedDocumentInOriginalDocumentFolderForTemparory(Obj);
                        if (response.IsSuccessStatusCode)
                        {
                            var resultVM = (TempAttachementVM)response.ResultData;
                            copySourceDoc = (int)resultVM.AttachementId;
                            IsMaskSourceJudgment = true;
                        }
                        if (dataCommunicationService.selectedLegalPrincipleDocumentTab == (int)JudgementsTabsEnums.AppealJudgements
                                || dataCommunicationService.selectedLegalPrincipleDocumentTab == (int)JudgementsTabsEnums.SupremeJudgements)
                        {
                            int indexNo = SourceDocument.FindIndex(x => x.UploadedDocumentId == uploadedDocumentId);
                            if (indexNo > -1)
                            {
                                SourceDocument[indexNo].CopySourceDocId = copySourceDoc;
                                SourceDocument[indexNo].IsMaskedJudgment = true;
                            }
                        }
                        else if (dataCommunicationService.selectedLegalPrincipleDocumentTab == (int)JudgementsTabsEnums.LegalAdvice)
                        {
                            int indexNo = LegalAdviceSourceDocument.FindIndex(x => x.UploadedDocumentId == uploadedDocumentId);
                            if (indexNo > -1)
                            {
                                LegalAdviceSourceDocument[indexNo].CopySourceDocId = copySourceDoc;
                                LegalAdviceSourceDocument[indexNo].IsMaskedJudgment = true;
                            }
                        }
                        else if (dataCommunicationService.selectedLegalPrincipleDocumentTab == (int)JudgementsTabsEnums.KuwaitAlYawm)
                        {
                            int indexNo = KuwaitAlYawmDocument.FindIndex(x => x.UploadedDocumentId == uploadedDocumentId);
                            if (indexNo > -1)
                            {
                                KuwaitAlYawmDocument[indexNo].CopySourceDocId = copySourceDoc;
                                KuwaitAlYawmDocument[indexNo].IsMaskedJudgment = true;
                            }
                        }
                        else if (dataCommunicationService.selectedLegalPrincipleDocumentTab == (int)JudgementsTabsEnums.Others)
                        {
                            int indexNo = OtherDocument.FindIndex(x => x.UploadedDocumentId == uploadedDocumentId);
                            if (indexNo > -1)
                            {
                                OtherDocument[indexNo].CopySourceDocId = copySourceDoc;
                                OtherDocument[indexNo].IsMaskedJudgment = true;
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

        private dynamic SetViewAttachmentList(int selectedGrid, int uploadedDocumentId)
        {
            switch (selectedGrid)
            {
                case (int)JudgementsTabsEnums.AppealJudgements:
                    return SourceDocument.Where(x => x.UploadedDocumentId == uploadedDocumentId && x.CourtTypeId == (int)CourtTypeEnum.Appeal).FirstOrDefault();

                case (int)JudgementsTabsEnums.SupremeJudgements:
                    return SourceDocument.Where(x => x.UploadedDocumentId == uploadedDocumentId && x.CourtTypeId == (int)CourtTypeEnum.Supreme).FirstOrDefault();

                case (int)JudgementsTabsEnums.LegalAdvice:
                    return LegalAdviceSourceDocument.Where(x => x.UploadedDocumentId == uploadedDocumentId).FirstOrDefault();

                case (int)JudgementsTabsEnums.KuwaitAlYawm:
                    return KuwaitAlYawmDocument.Where(x => x.UploadedDocumentId == uploadedDocumentId).FirstOrDefault();

                case (int)JudgementsTabsEnums.Others:
                    return OtherDocument.Where(x => x.UploadedDocumentId == uploadedDocumentId).FirstOrDefault();

                default:
                    return null;
            }
        }

        #endregion Document grid check document masking

        #region Get Populate functions

        private async Task PopulateMethods()
        {
            await GetLLSLegalPrincipleCategories();
            await GetLLSLegalPrincipleContent(PrincipleContentSearch);
        }

        #endregion Get Populate functions

        #region Functions

        private async Task GetLLSLegalPrincipleCategories()
        {
            var response = await lLSLegalPrincipleService.GetLLSLegalPrincipleContentCategories();
            if (response.IsSuccessStatusCode)
            {
                PrincipleCategories = (List<LLSLegalPrincipleContentCategoriesVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        private async Task GetLLSLegalPrincipleContent(LLSLegalPrinciplesRelationVM item)
        {
            PrincipleContentSearch.FromPage = 1;
            var response = await lLSLegalPrincipleService.AdvanceSearchPrincipleRelation(PrincipleContentSearch);
            if (response.IsSuccessStatusCode)
            {
                PrincipleContents = (List<LLSLegalPrinciplesRelationVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        #endregion Functions

        //on change check box
        private void OnChange(LLSLegalPrinciplesRelationVM data)
        {
            if (data.IsChecked)
            {
                selectedPrincipleContents.Add(data);
            }
            else
            {
                selectedPrincipleContents.Remove(data);
                data.PageNumber = null;
            }
        }

        //On Change Header Check Box
        private void OnChangeHeader()
        {
            if (isAll)
            {
                PrincipleContents.ForEach(x => x.IsChecked = true);
                selectedPrincipleContents = PrincipleContents;
            }
            else
            {
                PrincipleContents.ForEach(x => x.IsChecked = false);
                selectedPrincipleContents = new List<LLSLegalPrinciplesRelationVM>();
            }
        }

        #region Button Click

        protected async Task OnClear()
        {
            spinnerService.Show();
            PrincipleContentSearch.PrincipleContent = null;
            selectedPrincipleContents = new List<LLSLegalPrinciplesRelationVM>();
            await PopulateMethods();
            StateHasChanged();
            spinnerService.Hide();
        }

        protected async Task OnSearchPrincipleContent()
        {
            spinnerService.Show();
            PrincipleContentSearch.SourceDocumentId = JudmentDocumentId;
            await PopulateMethods();
            selectedPrincipleContents = new List<LLSLegalPrinciplesRelationVM>();
            PrincipleContents.ForEach(x => x.IsChecked = false);
            StateHasChanged();
            spinnerService.Hide();
        }

        #endregion Button Click

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
        #endregion Redirect Function

        protected async Task DocumentLoaded(LoadEventArgs args)
        {
            await JSRuntime.InvokeVoidAsync("addRotateButtonToPdfToolbar");
        }
    }
}
