using FATWA_DOMAIN.Models.LLSLegalPrinciple;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.LLSLegalPrinciple;
using FATWA_GENERAL.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.PdfViewer;
using Syncfusion.Blazor.PdfViewerServer;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using Telerik.Blazor.Components;
using Telerik.DataSource.Extensions;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.LegalPrinciple.LegalPrincipleEnum;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.LLSLegalPrinciple
{
    //<History Author = 'Umer Zaman' Date = '2024-04-18' Version = "1.0" Branch = "master">Create and Edit new legal principle</History>

    public partial class AddLLSLegalPrinciple : ComponentBase
    {
        #region Constructor
        public AddLLSLegalPrinciple()
        {
            lLSLegalPrinciple = new LLSLegalPrincipleSystem();
            lLSLegalPrincipleCategoryDetails = new List<LLSLegalPrincipleCategoriesVM>();
            SeclectedJudmentsDocumentList = new List<LLSLegalPrincipleDocumentVM>();
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
            //FileNameResult = new LLSLegalPrincipleDocumentVM();
            password = string.Empty;
            FileGrid = new RadzenDataGrid<LLSLegalPrincipleDocumentVM>();
            gridRelation = new RadzenDataGrid<LLSLegalPrinciplesContentVM>();
            activeworkflowlist = new List<WorkflowVM>();
            lLSLegalPrincipleContent = new LLSLegalPrincipleContent();
            lLSLegalPrincipleContentSourceDocumentReference = new LLSLegalPrincipleContentSourceDocumentReference();
            lLSLegalPrinciplesContentVM = new LLSLegalPrinciplesContentVM();
            lLSLegalPrinciplesContentVMs = new List<LLSLegalPrinciplesContentVM>();
            copySourceDoc = 0;
            IsMaskSourceJudgment = false;
            principleContentId = Guid.Empty;
            //LinkedDocuments = new List<LLSLegalPrinciplContentLinkedDocumentVM>();
            EditFileGrid = new RadzenDataGrid<LLSLegalPrinciplContentLinkedDocumentVM>();
            AppealSupremeFileGrid = new RadzenDataGrid<LLSLegalPrincipleAppealSupremenContentLinkedDocVM>();
            LegalAdviceSourceDocument = new List<LLSLegalPrinciplLegalAdviceDocumentVM>();
            KuwaitAlYawmDocument = new List<LLSLegalPrincipleKuwaitAlYoumDocuments>();

            LinkedDocuments = new LLSLegalPrincipleLinkedDocVM();
            AppealSupremeLinkedDocuments = new List<LLSLegalPrincipleAppealSupremenContentLinkedDocVM>();
            LegalAdviceLinkedDocuments = new List<LLSLegalPrincipleLegalAdviceContentLinkedDocVM>();
            KuwaitAlYoumLinkedDocuments = new List<LLSLegalPrincipleKuwaitAlYoumContentLinkedDocVM>();
            OthersLinkedDocuments = new List<LLSLegalPrincipleOthersContentLinkedDocVM>();
            OtherDocument = new List<LLSLegalPrinciplOtherDocumentVM>();
            SelectedGrid = -1;
            isNextDisabled = false;
            isPreviousDisabled = true;
            AppealLinkedDocCount = 0;
            SupremeLinkedDocCount = 0;
            LegalAdviceLinkedDocCount = 0;
            KuwaitAlYoumLinkedDocCount = 0;
            OthersLinkedDocCount = 0;
            LegalAdviceFileGrid = new RadzenDataGrid<LLSLegalPrincipleLegalAdviceContentLinkedDocVM>();
            KuwaitAlYoumFileGrid = new RadzenDataGrid<LLSLegalPrincipleKuwaitAlYoumContentLinkedDocVM>();
            OthersFileGrid = new RadzenDataGrid<LLSLegalPrincipleOthersContentLinkedDocVM>();
            FileGridLegal = new RadzenDataGrid<LLSLegalPrinciplLegalAdviceDocumentVM>();
            FileGridKuwait = new RadzenDataGrid<LLSLegalPrincipleKuwaitAlYoumDocuments>();
            FileGridOthers = new RadzenDataGrid<LLSLegalPrinciplOtherDocumentVM>();
            pdfViewer = new SfPdfViewerServer();
            advanceSearch = new LLSLegalPrincipalDocumentSearchVM();
            FilteredAppealJudgementDocuments = new List<LLSLegalPrincipleDocumentVM>();
            FilteredSupremeJudgementDocuments = new List<LLSLegalPrincipleDocumentVM>();
            FilteredLegalAdviceDocuments = new List<LLSLegalPrinciplLegalAdviceDocumentVM>();
            FilteredOthersJudgementDocuments = new List<LLSLegalPrinciplOtherDocumentVM>();
            FilteredKayDocumentList = new List<LLSLegalPrincipleKuwaitAlYoumDocuments>();
            PdfViewerDocumentVisible = false;
        }
        #endregion

        #region Parameter
        [Parameter]
        public dynamic PrincipleContentId { get; set; }
        [Parameter]
        public dynamic JudmentDocumentId { get; set; }
        [Parameter]
        public dynamic selectedTabIndex { get; set; }
        #endregion

        #region Variables declaration
        public List<LLSLegalPrincipleCategoriesVM> lLSLegalPrincipleCategoryDetails { get; set; }
        public List<LLSLegalPrincipleDocumentVM> SeclectedJudmentsDocumentList { get; set; }
        public List<LLSLegalPrinciplLegalAdviceDocumentVM> LegalAdviceSourceDocument { get; set; }
        public List<LLSLegalPrincipleKuwaitAlYoumDocuments> KuwaitAlYawmDocument { get; set; }
        public List<LLSLegalPrinciplOtherDocumentVM> OtherDocument { get; set; }
        public SfPdfViewerServer pdfViewer { get; set; }
        public PdfViewerToolbarSettings ToolbarSettings;
        public string DocumentPath { get; set; }
        public string DownloadFileName { get; set; }
        public dynamic FileNameResult { get; set; }
        //Encryption/Descyption Key
        public string password;
        System.Text.UnicodeEncoding UE;
        public byte[] key;
        RijndaelManaged RMCrypto;
        MemoryStream fsOut;
        public int data = 0;
        public byte[] MaskedFileData { get; set; }
        public byte[] FileData { get; set; }
        public RadzenDataGrid<LLSLegalPrincipleDocumentVM> FileGrid { get; set; }
        public RadzenDataGrid<LLSLegalPrinciplLegalAdviceDocumentVM> FileGridLegal { get; set; }
        public RadzenDataGrid<LLSLegalPrincipleKuwaitAlYoumDocuments> FileGridKuwait { get; set; }
        public RadzenDataGrid<LLSLegalPrinciplOtherDocumentVM> FileGridOthers { get; set; }
        public DateTime Max = new DateTime(DateTime.Now.Date.Ticks);
        public DateTime Min = new DateTime(1900, 1, 1);
        protected RadzenDataGrid<LLSLegalPrinciplesContentVM> gridRelation { get; set; }
        public List<WorkflowVM> activeworkflowlist { get; set; }
        public bool SavePrincipleResponseResult { get; set; }
        public string[] selectedCategories { get; set; } = Array.Empty<string>();
        protected bool ShowSourceDocumentGrid { get; set; }
        public int copySourceDoc { get; set; }
        public bool IsMaskSourceJudgment { get; set; }
        public Guid principleContentId { get; set; }
        //public List<LLSLegalPrinciplContentLinkedDocumentVM> LinkedDocuments { get; set; }
        public LLSLegalPrincipleLinkedDocVM LinkedDocuments { get; set; }

        public List<LLSLegalPrincipleAppealSupremenContentLinkedDocVM> AppealSupremeLinkedDocuments { get; set; }
        public List<LLSLegalPrincipleLegalAdviceContentLinkedDocVM> LegalAdviceLinkedDocuments { get; set; }
        public List<LLSLegalPrincipleKuwaitAlYoumContentLinkedDocVM> KuwaitAlYoumLinkedDocuments { get; set; }
        public List<LLSLegalPrincipleOthersContentLinkedDocVM> OthersLinkedDocuments { get; set; }
        private int SelectedGrid;
        private bool isNextDisabled;
        private bool isPreviousDisabled;
        private int AppealLinkedDocCount;
        private int SupremeLinkedDocCount;
        private int LegalAdviceLinkedDocCount;
        private int KuwaitAlYoumLinkedDocCount;
        private int OthersLinkedDocCount;
        public RadzenDataGrid<LLSLegalPrinciplContentLinkedDocumentVM> EditFileGrid { get; set; }
        public RadzenDataGrid<LLSLegalPrincipleAppealSupremenContentLinkedDocVM> AppealSupremeFileGrid { get; set; }
        public RadzenDataGrid<LLSLegalPrincipleLegalAdviceContentLinkedDocVM> LegalAdviceFileGrid { get; set; }
        public RadzenDataGrid<LLSLegalPrincipleKuwaitAlYoumContentLinkedDocVM> KuwaitAlYoumFileGrid { get; set; }
        public RadzenDataGrid<LLSLegalPrincipleOthersContentLinkedDocVM> OthersFileGrid { get; set; }
        protected LLSLegalPrincipalDocumentSearchVM advanceSearch { get; set; }
        public List<LLSLegalPrincipleDocumentVM> FilteredAppealJudgementDocuments { get; set; }
        public List<LLSLegalPrincipleDocumentVM> FilteredSupremeJudgementDocuments { get; set; }
        public List<LLSLegalPrinciplLegalAdviceDocumentVM> FilteredLegalAdviceDocuments { get; set; }
        public List<LLSLegalPrinciplOtherDocumentVM> FilteredOthersJudgementDocuments { get; set; }
        public IEnumerable<LLSLegalPrincipleKuwaitAlYoumDocuments> FilteredKayDocumentList { get; set; }
        #endregion

        #region Wizard variables
        public bool ShowWizard { get; set; } = true;
        public int Value { get; set; } = 0;
        protected string filePath1 = "\\images\\lmsLiteratureDetail-1.png";
        protected string filePathHover1 = "\\images\\lmsLiteratureDetail-1.png";

        protected string filePath3 = "\\images\\lmsLiteratureDetail-3.png";
        protected string filePathHover3 = "\\images\\lmsLiteratureDetail-3.png";

        protected string basePath1 = string.Empty;
        protected ValidationClass validations { get; set; } = new ValidationClass();
        public TelerikForm BasicSectionForm { get; set; }
        protected bool isBasicStep = false;
        public TelerikForm Notes_Form { get; set; }
        SfTreeView<LLSLegalPrincipleCategoriesVM> tree { get; set; }
        public string[] LegalPrincipleCategoryList { get; set; } = new string[] { };
        public string[] selectedNodes = new string[] { };
        string selectedId;

        SfContextMenu<MenuItem> menu;
        public string[] expandedNodes = new string[] { };
        bool isEdit = false;
        int index = 0;
        // Datasource for menu items
        public List<MenuItem> MenuItems { get; set; }
        //public List<MenuItem> MenuItems = new List<MenuItem>
        //{
        //	new MenuItem { Text = "Add_SubCategory" },
        //	new MenuItem { Text = "Edit_Label" },
        //	new MenuItem { Text = "Remove_Node" },
        //};
        public bool PdfViewerDocumentVisible;
        #endregion

        #region Validations class
        protected class ValidationClass
        {
            public string PageNumber { get; set; } = string.Empty;
            public string StartDate { get; set; } = string.Empty;
            public string LegalTemplateChoose { get; set; } = string.Empty;
            public string PrincipleContent { get; set; } = string.Empty;

        }
        #endregion

        #region Model full property Instance
        public LLSLegalPrincipleSystem lLSLegalPrinciple { get; set; }
        public LLSLegalPrincipleContent lLSLegalPrincipleContent { get; set; }
        public LLSLegalPrincipleContentSourceDocumentReference lLSLegalPrincipleContentSourceDocumentReference { get; set; }
        public LLSLegalPrinciplesContentVM lLSLegalPrinciplesContentVM { get; set; }
        public List<LLSLegalPrinciplesContentVM> lLSLegalPrinciplesContentVMs { get; set; }
        #endregion

        #region Component load
        protected override async Task OnInitializedAsync()
        {
            try
            {
                spinnerService.Show();
                await PopulateMethods();
                MenuItems = new List<MenuItem>
            {
                new MenuItem { Text = translationState.Translate("Add_SubCategory") },
                new MenuItem { Text = translationState.Translate("Edit_Label") },
                new MenuItem { Text = translationState.Translate("Remove_Node") },
            };
                await Load();
                spinnerService.Hide();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task Load()
        {
            try
            {
                if (PrincipleContentId != null) // edit principle content form scenario
                {
                    principleContentId = Guid.Parse(PrincipleContentId);
                    await GetLLSLegalPrincipleDetailsByUsingPrincipleContentId(principleContentId);
                    await LoadAttachment();
                    lLSLegalPrinciplesContentVM.StartDate = null;
                    lLSLegalPrinciplesContentVM.EndDate = null;
                    lLSLegalPrinciplesContentVM.PageNumber = 0;
                    lLSLegalPrinciplesContentVM.PrincipleContent = string.Empty;

                }
                else // add principle with contents form scenario
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
                            SeclectedJudmentsDocumentList.Add(dataCommunicationService.selectedJudmentDocumentsList);
                            await GetSourceJudementDocumentLoad(SeclectedJudmentsDocumentList);
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
                            SeclectedJudmentsDocumentList.Add(dataCommunicationService.selectedJudmentDocumentsList);
                            await GetSourceJudementDocumentLoad(SeclectedJudmentsDocumentList);
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
                            SelectedGrid = -1;
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Judgment_Document_Not_Loaded"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            await Task.Delay(200);
                            return;
                    }

                    lLSLegalPrinciple.PrincipleId = Guid.NewGuid();
                    lLSLegalPrinciple.FlowStatus = (int)PrincipleFlowStatusEnum.InReview;
                    //lLSLegalPrinciple.PrincipleNumber = 1;
                    lLSLegalPrinciple.OriginalSourceDocumentId = Convert.ToInt32(JudmentDocumentId);
                    lLSLegalPrinciplesContentVM.StartDate = DateTime.Now;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task GetLLSLegalPrincipleDetailsByUsingPrincipleContentId(Guid principleContentId)
        {
            try
            {
                var response = await lLSLegalPrincipleService.GetLLSLegalPrincipleDetailsByUsingPrincipleContentId(principleContentId);
                if (response.IsSuccessStatusCode)
                {
                    lLSLegalPrinciple = (LLSLegalPrincipleSystem)response.ResultData;

                    // fill existing selected category list
                    List<string> ObjCategoryId = new List<string>();
                    foreach (var item in lLSLegalPrinciple.lLSLegalPrincipleCategoryList)
                    {
                        ObjCategoryId.Add(item.CategoryId.ToString());
                    }
                    LegalPrincipleCategoryList = ObjCategoryId.ToArray();

                    // get all linked source documents details
                    var responseSource = await lLSLegalPrincipleService.GetLLSLegalPrincipleContentLinkedDocuments(principleContentId);
                    if (responseSource.IsSuccessStatusCode)
                    {
                        LinkedDocuments = (LLSLegalPrincipleLinkedDocVM)responseSource.ResultData;

                        if (LinkedDocuments.AppealSupremenLinkedDocuments.Count() != 0)
                        {
                            if (SelectedGrid == -1)
                            {
                                SelectedGrid = (int)JudgementsTabsEnums.AppealJudgements;
                            }
                            AppealSupremeLinkedDocuments = LinkedDocuments.AppealSupremenLinkedDocuments;
                            AppealLinkedDocCount = AppealSupremeLinkedDocuments.Where(x => x.CourtTypeId == (int)CourtTypeEnum.Appeal).ToList().Count;
                            SupremeLinkedDocCount = AppealSupremeLinkedDocuments.Where(x => x.CourtTypeId == (int)CourtTypeEnum.Supreme).ToList().Count;
                        }
                        if (LinkedDocuments.LegalAdviceLinkedDocuments.Count() != 0)
                        {
                            if (SelectedGrid == -1)
                            {
                                SelectedGrid = (int)JudgementsTabsEnums.LegalAdvice;
                            }
                            LegalAdviceLinkedDocuments = LinkedDocuments.LegalAdviceLinkedDocuments;
                            LegalAdviceLinkedDocCount = LegalAdviceLinkedDocuments.Count;
                        }
                        if (LinkedDocuments.KuwaitAlYoumLinkedDocuments.Count() != 0)
                        {
                            if (SelectedGrid == -1)
                            {
                                SelectedGrid = (int)JudgementsTabsEnums.KuwaitAlYawm;
                            }
                            KuwaitAlYoumLinkedDocuments = LinkedDocuments.KuwaitAlYoumLinkedDocuments;
                            KuwaitAlYoumLinkedDocCount = KuwaitAlYoumLinkedDocuments.Count;
                        }
                        if (LinkedDocuments.OthersLinkedDocuments.Count() != 0)
                        {
                            if (SelectedGrid == -1)
                            {
                                SelectedGrid = (int)JudgementsTabsEnums.Others;
                            }
                            OthersLinkedDocuments = LinkedDocuments.OthersLinkedDocuments;
                            OthersLinkedDocCount = OthersLinkedDocuments.Count;
                        }
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    // after getting source documentdetails fill principle content list.
                    var viewfileResult = SetViewAttachmentList(SelectedGrid, 0, 0);
                    LLSLegalPrinciplesContentVM ObjFill = new LLSLegalPrinciplesContentVM()
                    {
                        PrincipleContentId = principleContentId,
                        PrincipleId = lLSLegalPrinciple.PrincipleId,
                        PrincipleContent = lLSLegalPrinciple.lLSLegalPrinciplesContentList.Select(x => x.PrincipleContent).FirstOrDefault(),
                        StartDate = lLSLegalPrinciple.lLSLegalPrinciplesContentList.Select(x => x.StartDate).FirstOrDefault(),
                        EndDate = lLSLegalPrinciple.lLSLegalPrinciplesContentList.Select(x => x.EndDate).FirstOrDefault(),
                        PageNumber = viewfileResult != null ? viewfileResult.PageNumber : 0,
                        ReferenceId = viewfileResult != null ? viewfileResult.ReferenceId : 0
                    };
                    lLSLegalPrinciplesContentVMs.Add(ObjFill);
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception)
            {
                throw new NotImplementedException();
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
        #endregion

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

        #region Load source judgment document
        private async Task GetSourceJudementDocumentLoad(IEnumerable<dynamic> seclectedJudmentsDocumentList)
        {
            try
            {
                FileNameResult = seclectedJudmentsDocumentList.FirstOrDefault();
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

                        // Construct the physical path of the file on the server
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
        #endregion

        #region Get Populate functions
        private async Task PopulateMethods()
        {
            try
            {
                await GetLLSLegaPrincipleCategories();
                if (lLSLegalPrincipleCategoryDetails.Count > 0)
                    index = lLSLegalPrincipleCategoryDetails.Max(x => x.CategoryId) + 1;
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region Document grid view action also check document masking
        protected async Task OnGridViewClick(dynamic args)
        {
            try
            {

                string physicalPath = string.Empty;

                if (PrincipleContentId == null)
                {
                    await CheckPdfViewerIsDocumentEdited();
                }
                else
                {
                    if (lLSLegalPrinciplesContentVMs.Select(x => x.PageNumber).FirstOrDefault() != null)
                    {
                        await CheckPdfViewerIsDocumentEdited();
                    }
                }
                //var viewfile = LinkedDocuments.Where(x => x.CaseNumber == args.CaseNumber && x.UploadedDocumentId == args.UploadedDocumentId).FirstOrDefault();

                var viewfile = SetViewAttachmentList(SelectedGrid, args.ReferenceId, 0);

                //var viewfile = args.Guid != null ? FileExplanatoryNoteTemp.Where(x => x.Guid == args.Guid && x.FileName == args.FileName).FirstOrDefault() : FileExplanatoryNoteTemp.Where(x => x.ReferenceGuid == args.ReferenceGuid && x.FileName == args.FileName).FirstOrDefault();
                if (viewfile != null)
                {
                    foreach (var item in lLSLegalPrinciplesContentVMs)
                    {
                        item.PageNumber = viewfile.PageNumber;
                        item.ReferenceId = viewfile.ReferenceId != 0 ? viewfile.ReferenceId : 0;
                    }
                    await gridRelation.Reload();
#if DEBUG
                    {
                        if (SelectedGrid == (int)JudgementsTabsEnums.KuwaitAlYawm)
                            if (PrincipleContentId != null)
                            {
                                physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + viewfile.StoragePath).Replace(@"\\", @"\");
                            }
                            else
                            {
                                physicalPath = Path.Combine(viewfile.StoragePath).Replace(@"\\", @"\");
                            }
                        else
                            physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + viewfile.StoragePath).Replace(@"\\", @"\");

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
                    string base64String = await DocumentEncryptionService.GetDecryptedDocumentBase64(physicalPath, viewfile.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
                    if (!string.IsNullOrEmpty(base64String))
                    {
                        DocumentPath = "data:application/pdf;base64," + base64String;
                        DownloadFileName = viewfile.UploadedDocumentId.ToString();
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
            catch (Exception)
            {
                throw new NotImplementedException();

            }
        }

        private async Task CheckPdfViewerIsDocumentEdited()
        {
            try
            {
                if (pdfViewer.IsDocumentEdited)
                {
                    int uploadedDocumentId = Convert.ToInt32(pdfViewer.DownloadFileName);
                    dynamic viewFileDetail;
                    //if (PrincipleContentId == null) // add form
                    //{
                    int indexSelected = SelectedGrid != 0 ? SelectedGrid : Convert.ToInt32(selectedTabIndex);

                    viewFileDetail = SetViewAttachmentList(SelectedGrid, 0, uploadedDocumentId);
                    //}
                    //else // edit form
                    //{
                    //	viewFileDetail = SeclectedJudmentsDocumentList.Where(x => x.UploadedDocumentId == uploadedDocumentId).FirstOrDefault();
                    //}
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
                            Guid = lLSLegalPrinciplesContentVMs.Select(x => x.PrincipleContentId).FirstOrDefault(),
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

                            if (PrincipleContentId != null) // edit form
                            {
                                await SetNewlyAddedCopySourceNumber(SelectedGrid, (int)viewFileDetail.ReferenceId, copySourceDoc, IsMaskSourceJudgment);
                            }
                            else // add form
                            {
                                if (dataCommunicationService.selectedLegalPrincipleDocumentTab == (int)JudgementsTabsEnums.AppealJudgements
                                || dataCommunicationService.selectedLegalPrincipleDocumentTab == (int)JudgementsTabsEnums.SupremeJudgements)
                                {
                                    int indexNo = SeclectedJudmentsDocumentList.FindIndex(x => x.UploadedDocumentId == uploadedDocumentId);
                                    if (indexNo > -1)
                                    {
                                        SeclectedJudmentsDocumentList[indexNo].CopySourceDocId = copySourceDoc;
                                        SeclectedJudmentsDocumentList[indexNo].IsMaskedJudgment = true;
                                        await FileGrid.Reload();
                                    }
                                }
                                else if (dataCommunicationService.selectedLegalPrincipleDocumentTab == (int)JudgementsTabsEnums.LegalAdvice)
                                {
                                    int indexNo = LegalAdviceSourceDocument.FindIndex(x => x.UploadedDocumentId == uploadedDocumentId);
                                    if (indexNo > -1)
                                    {
                                        LegalAdviceSourceDocument[indexNo].CopySourceDocId = copySourceDoc;
                                        LegalAdviceSourceDocument[indexNo].IsMaskedJudgment = true;
                                        await FileGridLegal.Reload();
                                    }
                                }
                                else if (dataCommunicationService.selectedLegalPrincipleDocumentTab == (int)JudgementsTabsEnums.KuwaitAlYawm)
                                {
                                    int indexNo = KuwaitAlYawmDocument.FindIndex(x => x.UploadedDocumentId == uploadedDocumentId);
                                    if (indexNo > -1)
                                    {
                                        KuwaitAlYawmDocument[indexNo].CopySourceDocId = copySourceDoc;
                                        KuwaitAlYawmDocument[indexNo].IsMaskedJudgment = true;
                                        await FileGridKuwait.Reload();
                                    }
                                }
                                else if (dataCommunicationService.selectedLegalPrincipleDocumentTab == (int)JudgementsTabsEnums.Others)
                                {
                                    int indexNo = OtherDocument.FindIndex(x => x.UploadedDocumentId == uploadedDocumentId);
                                    if (indexNo > -1)
                                    {
                                        OtherDocument[indexNo].CopySourceDocId = copySourceDoc;
                                        OtherDocument[indexNo].IsMaskedJudgment = true;
                                        await FileGridOthers.Reload();
                                    }
                                }
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

        private dynamic SetViewAttachmentList(int selectedGrid, int referenceId, int uploadedDocumentId)
        {
            if (PrincipleContentId != null)
            {
                if (referenceId == 0 && uploadedDocumentId == 0)
                {
                    switch (selectedGrid)
                    {
                        case (int)JudgementsTabsEnums.AppealJudgements:
                            if (AppealSupremeLinkedDocuments.Any())
                            {
                                return AppealSupremeLinkedDocuments.FirstOrDefault();
                            }
                            else
                            {
                                return new List<LLSLegalPrincipleAppealSupremenContentLinkedDocVM>();
                            }
                        case (int)JudgementsTabsEnums.SupremeJudgements:
                            if (AppealSupremeLinkedDocuments.Any())
                            {
                                return AppealSupremeLinkedDocuments.FirstOrDefault();
                            }
                            else
                            {
                                return new List<LLSLegalPrincipleAppealSupremenContentLinkedDocVM>();
                            }
                        case (int)JudgementsTabsEnums.LegalAdvice:
                            if (LegalAdviceLinkedDocuments.Any())
                            {
                                return LegalAdviceLinkedDocuments.FirstOrDefault();
                            }
                            else
                            {
                                return new List<LLSLegalPrincipleLegalAdviceContentLinkedDocVM>();
                            }
                        case (int)JudgementsTabsEnums.KuwaitAlYawm:
                            if (KuwaitAlYoumLinkedDocuments.Any())
                            {
                                return KuwaitAlYoumLinkedDocuments.FirstOrDefault();
                            }
                            else
                            {
                                return new List<LLSLegalPrincipleKuwaitAlYoumContentLinkedDocVM>();
                            }
                        case (int)JudgementsTabsEnums.Others:
                            if (OthersLinkedDocuments.Any())
                            {
                                return OthersLinkedDocuments.FirstOrDefault();
                            }
                            else
                            {
                                return new List<LLSLegalPrincipleOthersContentLinkedDocVM>();
                            }
                        default:
                            return null;
                    }
                }
                else
                {
                    switch (selectedGrid)
                    {
                        case (int)JudgementsTabsEnums.AppealJudgements:
                            return AppealSupremeLinkedDocuments.Where(x => (referenceId != 0 ? x.ReferenceId == referenceId : x.UploadedDocumentId == uploadedDocumentId) && x.CourtTypeId == (int)CourtTypeEnum.Appeal).FirstOrDefault();

                        case (int)JudgementsTabsEnums.SupremeJudgements:
                            return AppealSupremeLinkedDocuments.Where(x => (referenceId != 0 ? x.ReferenceId == referenceId : x.UploadedDocumentId == uploadedDocumentId) && x.CourtTypeId == (int)CourtTypeEnum.Supreme).FirstOrDefault();

                        case (int)JudgementsTabsEnums.LegalAdvice:
                            return LegalAdviceLinkedDocuments.Where(x => (referenceId != 0 ? x.ReferenceId == referenceId : x.UploadedDocumentId == uploadedDocumentId)).FirstOrDefault();

                        case (int)JudgementsTabsEnums.KuwaitAlYawm:
                            return KuwaitAlYoumLinkedDocuments.Where(x => (referenceId != 0 ? x.ReferenceId == referenceId : x.UploadedDocumentId == uploadedDocumentId)).FirstOrDefault();

                        case (int)JudgementsTabsEnums.Others:
                            return OthersLinkedDocuments.Where(x => (referenceId != 0 ? x.ReferenceId == referenceId : x.UploadedDocumentId == uploadedDocumentId)).FirstOrDefault();

                        default:
                            return null;
                    }
                }
            }
            else
            {
                if (referenceId == 0 && uploadedDocumentId == 0)
                {
                    switch (selectedGrid)
                    {
                        case (int)JudgementsTabsEnums.AppealJudgements:
                            if (SeclectedJudmentsDocumentList.Any())
                            {
                                return SeclectedJudmentsDocumentList.FirstOrDefault();
                            }
                            else
                            {
                                return new List<LLSLegalPrincipleAppealSupremenContentLinkedDocVM>();
                            }
                        case (int)JudgementsTabsEnums.SupremeJudgements:
                            if (SeclectedJudmentsDocumentList.Any())
                            {
                                return SeclectedJudmentsDocumentList.FirstOrDefault();
                            }
                            else
                            {
                                return new List<LLSLegalPrincipleAppealSupremenContentLinkedDocVM>();
                            }
                        case (int)JudgementsTabsEnums.LegalAdvice:
                            if (LegalAdviceSourceDocument.Any())
                            {
                                return LegalAdviceSourceDocument.FirstOrDefault();
                            }
                            else
                            {
                                return new List<LLSLegalPrincipleLegalAdviceContentLinkedDocVM>();
                            }
                        case (int)JudgementsTabsEnums.KuwaitAlYawm:
                            if (KuwaitAlYawmDocument.Any())
                            {
                                return KuwaitAlYawmDocument.FirstOrDefault();
                            }
                            else
                            {
                                return new List<LLSLegalPrincipleKuwaitAlYoumContentLinkedDocVM>();
                            }
                        case (int)JudgementsTabsEnums.Others:
                            if (OtherDocument.Any())
                            {
                                return OtherDocument.FirstOrDefault();
                            }
                            else
                            {
                                return new List<LLSLegalPrincipleOthersContentLinkedDocVM>();
                            }
                        default:
                            return null;
                    }
                }
                else
                {
                    switch (selectedGrid)
                    {
                        case (int)JudgementsTabsEnums.AppealJudgements:
                            return SeclectedJudmentsDocumentList.Where(x => x.UploadedDocumentId == uploadedDocumentId && x.CourtTypeId == (int)CourtTypeEnum.Appeal).FirstOrDefault();

                        case (int)JudgementsTabsEnums.SupremeJudgements:
                            return SeclectedJudmentsDocumentList.Where(x => x.UploadedDocumentId == uploadedDocumentId && x.CourtTypeId == (int)CourtTypeEnum.Supreme).FirstOrDefault();

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
            }
        }
        private async Task SetViewAttachmentListPageNumber(int SelectedGrid, int referenceId)
        {
            switch (SelectedGrid)
            {
                case (int)JudgementsTabsEnums.AppealJudgements:
                    var resultAppeal = AppealSupremeLinkedDocuments.Where(x => x.ReferenceId == referenceId && x.CourtTypeId == (int)CourtTypeEnum.Appeal).FirstOrDefault();
                    if (resultAppeal != null)
                    {
                        var index = AppealSupremeLinkedDocuments.FindIndex(x => x.ReferenceId == resultAppeal.ReferenceId && x.CourtTypeId == (int)CourtTypeEnum.Appeal);
                        if (index > -1)
                        {
                            resultAppeal.PageNumber = 0;
                            AppealSupremeLinkedDocuments[index] = resultAppeal;
                            await AppealSupremeFileGrid.Reload();
                        }
                    }
                    break;
                case (int)JudgementsTabsEnums.SupremeJudgements:
                    var resultSupreme = AppealSupremeLinkedDocuments.Where(x => x.ReferenceId == referenceId && x.CourtTypeId == (int)CourtTypeEnum.Supreme).FirstOrDefault();
                    if (resultSupreme != null)
                    {
                        var index = AppealSupremeLinkedDocuments.FindIndex(x => x.ReferenceId == resultSupreme.ReferenceId && x.CourtTypeId == (int)CourtTypeEnum.Supreme);
                        if (index > -1)
                        {
                            resultSupreme.PageNumber = 0;
                            AppealSupremeLinkedDocuments[index] = resultSupreme;
                            await AppealSupremeFileGrid.Reload();
                        }
                    }
                    break;
                case (int)JudgementsTabsEnums.LegalAdvice:
                    var resultLegalAdvice = LegalAdviceLinkedDocuments.Where(x => x.ReferenceId == referenceId).FirstOrDefault();
                    if (resultLegalAdvice != null)
                    {
                        var index = LegalAdviceLinkedDocuments.FindIndex(x => x.ReferenceId == resultLegalAdvice.ReferenceId);
                        if (index > -1)
                        {
                            resultLegalAdvice.PageNumber = 0;
                            LegalAdviceLinkedDocuments[index] = resultLegalAdvice;
                            await LegalAdviceFileGrid.Reload();
                        }
                    }
                    break;

                case (int)JudgementsTabsEnums.KuwaitAlYawm:
                    var resultKuwaitAlYoum = KuwaitAlYoumLinkedDocuments.Where(x => x.ReferenceId == referenceId).FirstOrDefault();
                    if (resultKuwaitAlYoum != null)
                    {
                        var index = KuwaitAlYoumLinkedDocuments.FindIndex(x => x.ReferenceId == resultKuwaitAlYoum.ReferenceId);
                        if (index > -1)
                        {
                            resultKuwaitAlYoum.PageNumber = 0;
                            KuwaitAlYoumLinkedDocuments[index] = resultKuwaitAlYoum;
                            await KuwaitAlYoumFileGrid.Reload();
                        }
                    }
                    break;

                case (int)JudgementsTabsEnums.Others:
                    var resultOthers = OthersLinkedDocuments.Where(x => x.ReferenceId == referenceId).FirstOrDefault();
                    if (resultOthers != null)
                    {
                        var index = OthersLinkedDocuments.FindIndex(x => x.ReferenceId == resultOthers.ReferenceId);
                        if (index > -1)
                        {
                            resultOthers.PageNumber = 0;
                            OthersLinkedDocuments[index] = resultOthers;
                            await OthersFileGrid.Reload();
                        }
                    }
                    break;

                default:
                    break;
            }
        }

        private async Task SetNewlyAddedCopySourceNumber(int SelectedGrid, int referenceId, int copySourceDoc, bool isMaskSourceJudgment)
        {
            switch (SelectedGrid)
            {
                case (int)JudgementsTabsEnums.AppealJudgements:
                    var resultAppeal = AppealSupremeLinkedDocuments.Where(x => x.ReferenceId == referenceId && x.CourtTypeId == (int)CourtTypeEnum.Appeal).FirstOrDefault();
                    if (resultAppeal != null)
                    {
                        var index = AppealSupremeLinkedDocuments.FindIndex(x => x.ReferenceId == resultAppeal.ReferenceId && x.CourtTypeId == (int)CourtTypeEnum.Appeal);
                        if (index > -1)
                        {
                            resultAppeal.CopySourceDocId = copySourceDoc;
                            resultAppeal.IsMaskedJudgment = isMaskSourceJudgment;
                            AppealSupremeLinkedDocuments[index] = resultAppeal;
                            await AppealSupremeFileGrid.Reload();
                        }
                    }
                    break;
                case (int)JudgementsTabsEnums.SupremeJudgements:
                    var resultSupreme = AppealSupremeLinkedDocuments.Where(x => x.ReferenceId == referenceId && x.CourtTypeId == (int)CourtTypeEnum.Supreme).FirstOrDefault();
                    if (resultSupreme != null)
                    {
                        var index = AppealSupremeLinkedDocuments.FindIndex(x => x.ReferenceId == resultSupreme.ReferenceId && x.CourtTypeId == (int)CourtTypeEnum.Supreme);
                        if (index > -1)
                        {
                            resultSupreme.CopySourceDocId = copySourceDoc;
                            resultSupreme.IsMaskedJudgment = isMaskSourceJudgment;
                            AppealSupremeLinkedDocuments[index] = resultSupreme;
                            await AppealSupremeFileGrid.Reload();
                        }
                    }
                    break;
                case (int)JudgementsTabsEnums.LegalAdvice:
                    var resultLegalAdvice = LegalAdviceLinkedDocuments.Where(x => x.ReferenceId == referenceId).FirstOrDefault();
                    if (resultLegalAdvice != null)
                    {
                        var index = LegalAdviceLinkedDocuments.FindIndex(x => x.ReferenceId == resultLegalAdvice.ReferenceId);
                        if (index > -1)
                        {
                            resultLegalAdvice.CopySourceDocId = copySourceDoc;
                            resultLegalAdvice.IsMaskedJudgment = isMaskSourceJudgment;
                            LegalAdviceLinkedDocuments[index] = resultLegalAdvice;
                            await LegalAdviceFileGrid.Reload();
                        }
                    }
                    break;

                case (int)JudgementsTabsEnums.KuwaitAlYawm:
                    var resultKuwaitAlYoum = KuwaitAlYoumLinkedDocuments.Where(x => x.ReferenceId == referenceId).FirstOrDefault();
                    if (resultKuwaitAlYoum != null)
                    {
                        var index = KuwaitAlYoumLinkedDocuments.FindIndex(x => x.ReferenceId == resultKuwaitAlYoum.ReferenceId);
                        if (index > -1)
                        {
                            resultKuwaitAlYoum.CopySourceDocId = copySourceDoc;
                            resultKuwaitAlYoum.IsMaskedJudgment = isMaskSourceJudgment;
                            KuwaitAlYoumLinkedDocuments[index] = resultKuwaitAlYoum;
                            await KuwaitAlYoumFileGrid.Reload();
                        }
                    }
                    break;

                case (int)JudgementsTabsEnums.Others:
                    var resultOthers = OthersLinkedDocuments.Where(x => x.ReferenceId == referenceId).FirstOrDefault();
                    if (resultOthers != null)
                    {
                        var index = OthersLinkedDocuments.FindIndex(x => x.ReferenceId == resultOthers.ReferenceId);
                        if (index > -1)
                        {
                            resultOthers.CopySourceDocId = copySourceDoc;
                            resultOthers.IsMaskedJudgment = isMaskSourceJudgment;
                            OthersLinkedDocuments[index] = resultOthers;
                            await OthersFileGrid.Reload();
                        }
                    }
                    break;

                default:
                    break;
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

        #region Content grid actions
        protected async Task EditGridContent(LLSLegalPrinciplesContentVM args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_You_Want_to_Update_Record"), translationState.Translate("Update")) == true)
                {
                    if (args != null)
                    {
                        lLSLegalPrinciplesContentVM.PrincipleContentId = args.PrincipleContentId;
                        lLSLegalPrinciplesContentVM.PrincipleId = args.PrincipleId;
                        lLSLegalPrinciplesContentVM.StartDate = (DateTime)args.StartDate;
                        lLSLegalPrinciplesContentVM.EndDate = args.EndDate;
                        lLSLegalPrinciplesContentVM.PageNumber = (int)args.PageNumber;
                        lLSLegalPrinciplesContentVM.PrincipleContent = args.PrincipleContent;
                        lLSLegalPrinciplesContentVM.ReferenceId = args.ReferenceId;
                    }
                }
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        protected async Task DeleteGridContent(LLSLegalPrinciplesContentVM args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Are_you_Sure_You_Want_to_Remove_Linked_Source_Record"), translationState.Translate("Delete")) == true)
                {
                    if (args != null)
                    {
                        if (PrincipleContentId == null) // add form
                        {
                            var result = lLSLegalPrinciplesContentVMs.Where(x => x.PrincipleContentId == args.PrincipleContentId).FirstOrDefault();
                            if (result != null)
                            {
                                lLSLegalPrinciplesContentVMs.Remove(result);
                                await gridRelation.Reload();
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Success,
                                    Detail = translationState.Translate("Deleted_Successfully"),
                                    Style = "position: fixed !important; left: 0; margin: auto;"
                                });
                            }
                        }
                        else // editing principle content form
                        {
                            var result = lLSLegalPrinciplesContentVMs.Where(x => x.PrincipleContentId == args.PrincipleContentId).FirstOrDefault();
                            if (result != null)
                            {
                                //// if principle content linked with multiple source documents, then only remove delete document from the grid and also atatched new document page number into principle content VM.
                                //if (LinkedDocuments.Count() > 1)
                                //{
                                //	if (result.ReferenceId != 0)
                                //	{
                                //		// first add delete source document reference id into list.
                                //		lLSLegalPrinciple.SourceDocumentDeletedReferenceId.Add((int)result.ReferenceId);
                                //		// remove that source document from the source document list.
                                //		var sourceDocumentResult = LinkedDocuments.Where(x => x.ReferenceId == result.ReferenceId).FirstOrDefault();
                                //		if (sourceDocumentResult != null)
                                //		{
                                //			LinkedDocuments.Remove(sourceDocumentResult);
                                //		}
                                //		await Task.Delay(200);
                                //		// now add top source document reference id and page number into principle content VM and also view top source document into PDF viewer.
                                //		await OnGridViewClick(LinkedDocuments.FirstOrDefault());
                                //	}
                                //}
                                //// if only one source document linked with principle content, then delete both principle content and source document from grid's.
                                //else if (LinkedDocuments.Count() == 1)
                                //{
                                //	lLSLegalPrinciple.SourceDocumentDeletedReferenceId.Add((int)result.ReferenceId);
                                //	lLSLegalPrinciplesContentVMs.Remove(result);
                                //	var sourceDocumentResult = LinkedDocuments.Where(x => x.ReferenceId == result.ReferenceId).FirstOrDefault();
                                //	if (sourceDocumentResult != null)
                                //	{
                                //		LinkedDocuments.Remove(sourceDocumentResult);
                                //	}

                                //}

                                // Remove page number from principle content list.
                                var index = lLSLegalPrinciplesContentVMs.FindIndex(x => x.PrincipleContentId == result.PrincipleContentId);
                                if (index > -1)
                                {
                                    result.PageNumber = 0;
                                    lLSLegalPrinciplesContentVMs[index] = result;
                                }
                                //add delete source document reference id into list.
                                lLSLegalPrinciple.SourceDocumentDeletedReferenceId.Add((int)result.ReferenceId);

                                // get concerned source document records from grid.
                                var viewExistingFileRecord = SetViewAttachmentList(SelectedGrid, (int)result.ReferenceId, 0);
                                if (viewExistingFileRecord != null)
                                {
                                    // remove page number from the source document list.
                                    await SetViewAttachmentListPageNumber(SelectedGrid, (int)result.ReferenceId);
                                }
                                if (SelectedGrid == (int)JudgementsTabsEnums.AppealJudgements || SelectedGrid == (int)JudgementsTabsEnums.SupremeJudgements)
                                {
                                    await AppealSupremeFileGrid.Reload();
                                }
                                else if (SelectedGrid == (int)JudgementsTabsEnums.LegalAdvice)
                                {
                                    await LegalAdviceFileGrid.Reload();
                                }
                                else if (SelectedGrid == (int)JudgementsTabsEnums.KuwaitAlYawm)
                                {
                                    await KuwaitAlYoumFileGrid.Reload();
                                }
                                else if (SelectedGrid == (int)JudgementsTabsEnums.Others)
                                {
                                    await OthersFileGrid.Reload();
                                }

                                await gridRelation.Reload();
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Success,
                                    Detail = translationState.Translate("Deleted_Successfully"),
                                    Style = "position: fixed !important; left: 0; margin: auto;"
                                });
                            }
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

        #region Wizard Meta Data step
        //<History Author = 'Umer Zaman' Date='2024-04-30' Version="1.0" Branch="master"> Basic information Next button</History>
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
            bool valid = ValidateBasicInfoOnChange();
            if (valid)
            {
                if (PrincipleContentId != null) // edit form
                {
                    await CheckPdfViewerIsDocumentEdited();
                }
                Value += 1;
            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Please_Add_Atleast_One_Principle_Content"),
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
        protected async void AddPrincipleContentIntoGrid()
        {
            if (lLSLegalPrinciplesContentVM.PageNumber == null || string.IsNullOrWhiteSpace(lLSLegalPrinciplesContentVM.PrincipleContent))
            {
                if (lLSLegalPrinciplesContentVM.PageNumber == null)
                {
                    validations.PageNumber = "k-invalid";
                }
                else
                {
                    validations.PageNumber = "k-valid";
                }
                if (string.IsNullOrWhiteSpace(lLSLegalPrinciplesContentVM.PrincipleContent))
                {
                    validations.PrincipleContent = "k-invalid";
                }
                else
                {
                    validations.PrincipleContent = "k-valid";
                }
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Fill_Required_Fields"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                return;
            }
            if (lLSLegalPrinciplesContentVM.PageNumber == null)
            {
                validations.PageNumber = "k-invalid";
            }
            else
            {
                validations.PageNumber = "k-valid";
            }
            if (string.IsNullOrWhiteSpace(lLSLegalPrinciplesContentVM.PrincipleContent))
            {
                validations.PrincipleContent = "k-invalid";
            }
            else
            {
                validations.PrincipleContent = "k-valid";
            }
            if (lLSLegalPrinciplesContentVM.PrincipleContentId != Guid.Empty)
            {
                if (lLSLegalPrinciplesContentVMs.Any())
                {
                    var resultExisting = lLSLegalPrinciplesContentVMs.Where(x => x.PrincipleContentId == lLSLegalPrinciplesContentVM.PrincipleContentId).FirstOrDefault();
                    if (resultExisting != null)
                    {
                        var index = lLSLegalPrinciplesContentVMs.FindIndex(x => x.PrincipleContentId == lLSLegalPrinciplesContentVM.PrincipleContentId);
                        if (index > -1)
                        {
                            resultExisting.StartDate = lLSLegalPrinciplesContentVM.StartDate;
                            resultExisting.EndDate = lLSLegalPrinciplesContentVM.EndDate;
                            resultExisting.PrincipleContent = ConvertHtmlConvertToPlainText(lLSLegalPrinciplesContentVM.PrincipleContent);
                            resultExisting.PageNumber = lLSLegalPrinciplesContentVM.PageNumber;
                            resultExisting.ReferenceId = lLSLegalPrinciplesContentVM.ReferenceId;
                            lLSLegalPrinciplesContentVMs[index] = resultExisting;
                        }
                        // check if reference id exist in SourceDocumentDeletedReferenceId list then delete it, because user have added new pagenumber against that reference id.
                        if (lLSLegalPrinciple.SourceDocumentDeletedReferenceId.Any() && lLSLegalPrinciplesContentVM.ReferenceId != 0)
                        {
                            var resultReferenceId = lLSLegalPrinciple.SourceDocumentDeletedReferenceId.Where(x => x == lLSLegalPrinciplesContentVM.ReferenceId).FirstOrDefault();
                            if (resultReferenceId != 0)
                            {
                                lLSLegalPrinciple.SourceDocumentDeletedReferenceId.Remove((int)lLSLegalPrinciplesContentVM.ReferenceId);
                            }
                        }
                    }
                }
            }
            else
            {
                lLSLegalPrinciplesContentVM.PrincipleContentId = Guid.NewGuid();
                lLSLegalPrinciplesContentVM.PrincipleId = lLSLegalPrinciple.PrincipleId;

                LLSLegalPrinciplesContentVM ObjFill = new LLSLegalPrinciplesContentVM()
                {
                    PrincipleContentId = lLSLegalPrinciplesContentVM.PrincipleContentId,
                    PrincipleId = lLSLegalPrinciplesContentVM.PrincipleId,
                    StartDate = lLSLegalPrinciplesContentVM.StartDate,
                    EndDate = lLSLegalPrinciplesContentVM.EndDate,
                    PrincipleContent = ConvertHtmlConvertToPlainText(lLSLegalPrinciplesContentVM.PrincipleContent),
                    PageNumber = lLSLegalPrinciplesContentVM.PageNumber,
                    ReferenceId = 0
                };
                lLSLegalPrinciplesContentVMs.Add(ObjFill);
            }
            if (PrincipleContentId == null)
            {
                lLSLegalPrinciplesContentVM = new LLSLegalPrinciplesContentVM();
                lLSLegalPrinciplesContentVM.PrincipleContent = string.Empty;
                lLSLegalPrinciplesContentVM.StartDate = DateTime.Now;
            }
            else // editing form
            {
                // update page number in document grid
                //string[] strlist = pdfViewer.DownloadFileName.Split("=", StringSplitOptions.None);
                //string caseNumber = null;
                //int uploadedDocumentId = 0;
                //for (int i = 0; i < strlist.Length; i++)
                //{
                //	if (i == 0)
                //	{
                //		caseNumber = strlist[i];
                //	}
                //	else if (i == 1)
                //	{
                //		uploadedDocumentId = Convert.ToInt32(strlist[i]);
                //	}
                //}
                int uploadedDocumentId = Convert.ToInt32(pdfViewer.DownloadFileName);
                var viewFileRecord = SetViewAttachmentList(SelectedGrid, 0, uploadedDocumentId);
                if (viewFileRecord.UploadedDocumentId == uploadedDocumentId)
                {
                    viewFileRecord.PageNumber = lLSLegalPrinciplesContentVM.PageNumber;
                }

                lLSLegalPrinciplesContentVM = new LLSLegalPrinciplesContentVM();
                lLSLegalPrinciplesContentVM.PrincipleContent = string.Empty;
                lLSLegalPrinciplesContentVM.StartDate = null;
            }
            await gridRelation.Reload();
        }
        private string ConvertHtmlConvertToPlainText(string principleContent)
        {
            // Replace </div><div> sequences with a space
            string withoutbothDivs = Regex.Replace(principleContent, @"</div><div>", " ");
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

        #region Validate template setup
        protected bool ValidateBasicInfoOnChange()
        {
            bool templatesetupDetailsValid = true;
            if (lLSLegalPrinciplesContentVMs.Count() == 0)
            {
                validations.LegalTemplateChoose = "k-invalid";
                templatesetupDetailsValid = false;
            }
            else
            {
                validations.LegalTemplateChoose = "k-valid";
            }
            return templatesetupDetailsValid;
        }

        #endregion

        #region Form submit

        protected async Task FormSubmit()
        {
            if (!lLSLegalPrinciplesContentVMs.Any())
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Please_Add_Atleast_One_Principle_Content"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                return;
            }
            if (lLSLegalPrinciplesContentVMs.Any())
            {
                if (PrincipleContentId != null)
                {
                    // update existing principle content record

                    //lLSLegalPrinciple.lLSLegalPrinciplesContentList = new List<LLSLegalPrincipleContent>();
                    foreach (var itemExist in lLSLegalPrinciplesContentVMs)
                    {
                        var indexExisting = lLSLegalPrinciple.lLSLegalPrinciplesContentList.FindIndex(x => x.PrincipleContentId == itemExist.PrincipleContentId);
                        if (indexExisting > -1)
                        {
                            lLSLegalPrinciple.lLSLegalPrinciplesContentList[indexExisting].PrincipleContent = itemExist.PrincipleContent;
                            lLSLegalPrinciple.lLSLegalPrinciplesContentList[indexExisting].StartDate = (DateTime)itemExist.StartDate;
                            lLSLegalPrinciple.lLSLegalPrinciplesContentList[indexExisting].EndDate = itemExist.EndDate;
                        }
                    }
                }
                else
                {
                    foreach (var item in lLSLegalPrinciplesContentVMs)
                    {
                        LLSLegalPrincipleContent ObjFill = new LLSLegalPrincipleContent()
                        {
                            PrincipleContentId = item.PrincipleContentId,
                            PrincipleId = item.PrincipleId,
                            PrincipleContent = item.PrincipleContent,
                            StartDate = (DateTime)item.StartDate,
                            EndDate = item.EndDate
                        };
                        lLSLegalPrinciple.lLSLegalPrinciplesContentList.Add(ObjFill);
                    }
                }
            }

            if (!LegalPrincipleCategoryList.Any())
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Atleast_One_Category"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                return;
            }
            if (PrincipleContentId != null)
            {
                lLSLegalPrinciple.lLSLegalPrincipleCategoryList = new List<LLSLegalPrincipleContentCategory>();
            }
            foreach (var item in LegalPrincipleCategoryList)
            {
                foreach (var itemContent in lLSLegalPrinciplesContentVMs)
                {
                    LLSLegalPrincipleContentCategory ObjFill = new LLSLegalPrincipleContentCategory()
                    {
                        PrincipleContentId = itemContent.PrincipleContentId,
                        CategoryId = Convert.ToInt32(item)
                    };
                    lLSLegalPrinciple.lLSLegalPrincipleCategoryList.Add(ObjFill);
                }
            }

            if (await dialogService.Confirm(translationState.Translate("Principle_Submit_Popup_Confirm_Message"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                if (PrincipleContentId == null)
                {
                    if (pdfViewer.IsDocumentEdited)
                    {
                        await CheckPdfViewerIsDocumentEdited();
                    }
                    else
                    {
                        ApiCallResponse responseCopy = await lLSLegalPrincipleService.CheckCopyDocumentExists(lLSLegalPrinciple.OriginalSourceDocumentId);
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
                                //await Task.Delay(200);
                                //await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
                                return;
                        }
                        if (judgementDocVM is not null)
                        {
                            TempAttachementVM Obj = new TempAttachementVM()
                            {
                                MaskedFileData = await pdfViewer.GetDocumentAsync(),
                                IsMaskedAttachment = IsMaskSourceJudgment ? true : false,
                                UploadFrom = "LLSLegalPrincipleSystem",
                                Guid = lLSLegalPrinciplesContentVMs.Select(x => x.PrincipleContentId).FirstOrDefault(),
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
                                Obj.FileNameWithoutTimeStamp = judgementDocVM.FileName;
                                Obj.OtherAttachmentType = judgementDocVM.OtherAttachmentType;
                                Obj.DocumentDate = judgementDocVM.DocumentDate;
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
                }
                else
                {
                    if (lLSLegalPrinciplesContentVMs.Select(x => x.PageNumber).FirstOrDefault() != null)
                    {
                        if (pdfViewer.IsDocumentEdited)
                        {
                            await CheckPdfViewerIsDocumentEdited();
                        }
                        else
                        {
                            ApiCallResponse responseCopy = await lLSLegalPrincipleService.CheckCopyDocumentExists(lLSLegalPrinciple.OriginalSourceDocumentId);
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
                                    //await Task.Delay(200);
                                    //await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
                                    return;
                            }
                            if (judgementDocVM is not null)
                            {
                                TempAttachementVM Obj = new TempAttachementVM()
                                {
                                    MaskedFileData = await pdfViewer.GetDocumentAsync(),
                                    IsMaskedAttachment = IsMaskSourceJudgment ? true : false,
                                    UploadFrom = "LLSLegalPrincipleSystem",
                                    Guid = lLSLegalPrinciplesContentVMs.Select(x => x.PrincipleContentId).FirstOrDefault(),
                                    DocType = judgementDocVM.DocType,
                                    AttachmentTypeId = judgementDocVM.AttachmentTypeId,
                                    Description = judgementDocVM.Description
                                };
                                if (dataCommunicationService.selectedLegalPrincipleDocumentTab == (int)JudgementsTabsEnums.KuwaitAlYawm)
                                {
                                    Obj.FileNameWithoutTimeStamp = judgementDocVM.FileTitle;
                                }
                                else if (dataCommunicationService.selectedLegalPrincipleDocumentTab == (int)JudgementsTabsEnums.Others)
                                {
                                    Obj.FileNameWithoutTimeStamp = judgementDocVM.FileName;
                                    Obj.OtherAttachmentType = judgementDocVM.OtherAttachmentType;
                                    Obj.DocumentDate = judgementDocVM.DocumentDate;
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
                    }
                }

                if (PrincipleContentId == null) // add form
                {
                    foreach (var item in lLSLegalPrinciplesContentVMs)
                    {
                        lLSLegalPrinciple.linkContents.Add(new LLSLegalPrincipleContentSourceDocumentReference
                        {
                            PrincipleContentId = item.PrincipleContentId,
                            PageNumber = (int)item.PageNumber,
                            OriginalSourceDocId = Convert.ToInt32(JudmentDocumentId),
                            CopySourceDocId = copySourceDoc,
                            IsMaskedJudgment = IsMaskSourceJudgment ? true : false
                        });
                    }
                }
                else // editing form
                {
                    // update concerned judgement record

                    //lLSLegalPrinciple.linkContents = new List<LLSLegalPrincipleContentSourceDocumentReference>();
                    lLSLegalPrinciple.FlowStatus = (int)PrincipleFlowStatusEnum.InReview;
                    // maybe principle content is linked with one or multiple source documents and user also changed or remove page number against one or multiple source documents.
                    if (AppealSupremeLinkedDocuments.Any())
                    {
                        foreach (var itemAppeal in AppealSupremeLinkedDocuments)
                        {
                            foreach (var item in lLSLegalPrinciplesContentVMs)
                            {
                                var AppealIndex = lLSLegalPrinciple.linkContents.FindIndex(x => x.ReferenceId == item.ReferenceId);
                                if (AppealIndex > -1)
                                {
                                    lLSLegalPrinciple.linkContents[AppealIndex].PageNumber = (int)itemAppeal.PageNumber;
                                    lLSLegalPrinciple.linkContents[AppealIndex].OriginalSourceDocId = (int)itemAppeal.OriginalSourceDocId;
                                    lLSLegalPrinciple.linkContents[AppealIndex].CopySourceDocId = (int)itemAppeal.CopySourceDocId;
                                    lLSLegalPrinciple.linkContents[AppealIndex].IsMaskedJudgment = itemAppeal.IsMaskedJudgment;
                                }
                                //lLSLegalPrinciple.linkContents.Add(new LLSLegalPrincipleContentSourceDocumentReference
                                //{
                                //	PrincipleContentId = item.PrincipleContentId,
                                //	PageNumber = (int)itemAppeal.PageNumber,
                                //	OriginalSourceDocId = (int)itemAppeal.OriginalSourceDocId,
                                //	CopySourceDocId = (int)itemAppeal.CopySourceDocId,
                                //	IsMaskedJudgment = itemAppeal.IsMaskedJudgment
                                //});
                            }
                        }
                    }
                    if (LegalAdviceLinkedDocuments.Any())
                    {
                        foreach (var itemLegalAdvice in LegalAdviceLinkedDocuments)
                        {
                            foreach (var item in lLSLegalPrinciplesContentVMs)
                            {
                                var LegalIndex = lLSLegalPrinciple.linkContents.FindIndex(x => x.ReferenceId == item.ReferenceId);
                                if (LegalIndex > -1)
                                {
                                    lLSLegalPrinciple.linkContents[LegalIndex].PageNumber = (int)itemLegalAdvice.PageNumber;
                                    lLSLegalPrinciple.linkContents[LegalIndex].OriginalSourceDocId = (int)itemLegalAdvice.OriginalSourceDocId;
                                    lLSLegalPrinciple.linkContents[LegalIndex].CopySourceDocId = (int)itemLegalAdvice.CopySourceDocId;
                                    lLSLegalPrinciple.linkContents[LegalIndex].IsMaskedJudgment = itemLegalAdvice.IsMaskedJudgment;
                                }
                                //lLSLegalPrinciple.linkContents.Add(new LLSLegalPrincipleContentSourceDocumentReference
                                //{
                                //	PrincipleContentId = item.PrincipleContentId,
                                //	PageNumber = (int)itemLegalAdvice.PageNumber,
                                //	OriginalSourceDocId = (int)itemLegalAdvice.OriginalSourceDocId,
                                //	CopySourceDocId = (int)itemLegalAdvice.CopySourceDocId,
                                //	IsMaskedJudgment = itemLegalAdvice.IsMaskedJudgment
                                //});
                            }
                        }
                    }
                    if (KuwaitAlYoumLinkedDocuments.Any())
                    {
                        foreach (var itemKuwaitAlYoum in KuwaitAlYoumLinkedDocuments)
                        {
                            foreach (var item in lLSLegalPrinciplesContentVMs)
                            {
                                var KuwaitIndex = lLSLegalPrinciple.linkContents.FindIndex(x => x.ReferenceId == item.ReferenceId);
                                if (KuwaitIndex > -1)
                                {
                                    lLSLegalPrinciple.linkContents[KuwaitIndex].PageNumber = (int)itemKuwaitAlYoum.PageNumber;
                                    lLSLegalPrinciple.linkContents[KuwaitIndex].OriginalSourceDocId = (int)itemKuwaitAlYoum.OriginalSourceDocId;
                                    lLSLegalPrinciple.linkContents[KuwaitIndex].CopySourceDocId = (int)itemKuwaitAlYoum.CopySourceDocId;
                                    lLSLegalPrinciple.linkContents[KuwaitIndex].IsMaskedJudgment = itemKuwaitAlYoum.IsMaskedJudgment;
                                }
                                //lLSLegalPrinciple.linkContents.Add(new LLSLegalPrincipleContentSourceDocumentReference
                                //{
                                //	PrincipleContentId = item.PrincipleContentId,
                                //	PageNumber = (int)itemKuwaitAlYoum.PageNumber,
                                //	OriginalSourceDocId = (int)itemKuwaitAlYoum.OriginalSourceDocId,
                                //	CopySourceDocId = (int)itemKuwaitAlYoum.CopySourceDocId,
                                //	IsMaskedJudgment = itemKuwaitAlYoum.IsMaskedJudgment
                                //});
                            }
                        }
                    }
                    if (OthersLinkedDocuments.Any())
                    {
                        foreach (var itemOthers in OthersLinkedDocuments)
                        {
                            foreach (var item in lLSLegalPrinciplesContentVMs)
                            {
                                var OtherIndex = lLSLegalPrinciple.linkContents.FindIndex(x => x.ReferenceId == item.ReferenceId);
                                if (OtherIndex > -1)
                                {
                                    lLSLegalPrinciple.linkContents[OtherIndex].PageNumber = (int)itemOthers.PageNumber;
                                    lLSLegalPrinciple.linkContents[OtherIndex].OriginalSourceDocId = (int)itemOthers.OriginalSourceDocId;
                                    lLSLegalPrinciple.linkContents[OtherIndex].CopySourceDocId = (int)itemOthers.CopySourceDocId;
                                    lLSLegalPrinciple.linkContents[OtherIndex].IsMaskedJudgment = itemOthers.IsMaskedJudgment;
                                }
                                //lLSLegalPrinciple.linkContents.Add(new LLSLegalPrincipleContentSourceDocumentReference
                                //{
                                //	PrincipleContentId = item.PrincipleContentId,
                                //	PageNumber = (int)itemOthers.PageNumber,
                                //	OriginalSourceDocId = (int)itemOthers.OriginalSourceDocId,
                                //	CopySourceDocId = (int)itemOthers.CopySourceDocId,
                                //	IsMaskedJudgment = itemOthers.IsMaskedJudgment
                                //});
                            }
                        }
                    }
                }
                if (lLSLegalPrinciple.FlowStatus == (int)PrincipleFlowStatusEnum.InReview)
                {
                    await PopulateSubmitPrincipleWithWorkflow();
                }
                else
                {
                    await PopulateSubmitPrincipleWithoutWorkflow();
                }
            }
        }
        #endregion

        #region Submit Principle (with Workflow)
        protected async Task PopulateSubmitPrincipleWithWorkflow()
        {
            try
            {
                var response = await workflowService.GetActiveWorkflows((int)WorkflowModuleEnum.LPSPrinciple, (int)WorkflowModuleTriggerEnum.UserSubmitsPrinciple, null, (int)WorkflowSubModuleEnum.LegalPrinciples);
                if (response.IsSuccessStatusCode)
                {
                    activeworkflowlist = (List<WorkflowVM>)response.ResultData;
                    if (activeworkflowlist?.Count() > 0)
                    {
                        if (PrincipleContentId != null)
                        {
                            var responsePrinciple = await lLSLegalPrincipleService.UpdateLLSLegalPrinciple(lLSLegalPrinciple);
                            if (responsePrinciple.IsSuccessStatusCode)
                            {
                                SavePrincipleResponseResult = (bool)responsePrinciple.ResultData;
                            }
                        }
                        else
                        {
                            var responsePrinciple = await lLSLegalPrincipleService.SaveLLSLegalPrinciple(lLSLegalPrinciple);
                            if (responsePrinciple.IsSuccessStatusCode)
                            {
                                SavePrincipleResponseResult = (bool)responsePrinciple.ResultData;
                            }
                        }
                    }
                    else
                    {
                        SavePrincipleResponseResult = false;
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
                    return;
                }

                if (SavePrincipleResponseResult)
                {
                    lLSLegalPrinciple.SenderEmail = loginState.UserDetail.UserName;
                    await workflowService.AssignWorkflowActivity(activeworkflowlist.FirstOrDefault(), lLSLegalPrinciple, (int)WorkflowModuleEnum.LPSPrinciple, (int)WorkflowModuleTriggerEnum.UserSubmitsPrinciple, null);
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Principle_Success_Message"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    await Task.Delay(200);
                    await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Populate Submit Principle (Without Workflow)

        protected async Task PopulateSubmitPrincipleWithoutWorkflow()
        {
            try
            {
                if (PrincipleContentId != null)
                {
                    var resultPrinciple = await lLSLegalPrincipleService.UpdateLLSLegalPrinciple(lLSLegalPrinciple);
                    if (resultPrinciple.IsSuccessStatusCode)
                    {
                        SavePrincipleResponseResult = (bool)resultPrinciple.ResultData;
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Principle_Success_Message"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        await Task.Delay(200);
                        await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
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
                    var resultPrinciple = await lLSLegalPrincipleService.SaveLLSLegalPrinciple(lLSLegalPrinciple);
                    if (resultPrinciple.IsSuccessStatusCode)
                    {
                        SavePrincipleResponseResult = (bool)resultPrinciple.ResultData;
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Principle_Success_Message"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        await Task.Delay(200);
                        await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Functions
        //principle content base on CategoryId
        private async Task PopulatePrincipleContents()
        {
            var dialogResult = await dialogService.OpenAsync<ListLLSLegalPrincipleContentDialog>(
                translationState.Translate("Principe_Contents"),
                new Dictionary<string, object>() {
                    { "CategoryId", this.selectedId },
                },
                new DialogOptions() { Width = "60% !important", CloseDialogOnOverlayClick = true });
            dialogService.Close();
            return;
        }
        #endregion

        #region Tree View Context Menu

        //open context menu on button click
        private async Task OpenContextMenu(MouseEventArgs e)
        {
            await menu.OpenAsync(e.ClientX, e.ClientY);
        }

        //trigger before opening the menu items
        public async Task BeforeContextOpen(BeforeOpenCloseMenuEventArgs<MenuItem> args)
        {
            var treeNode = tree.GetNode(selectedId);
            var node = lLSLegalPrincipleCategoryDetails.Where(x => x.CategoryId == int.Parse(treeNode.Id)).FirstOrDefault();
            if (node.PrincipleContentCount > 0)
            {
                //MenuItems.Where(x => x.Text == "Edit_Label" && x.Text == "Remove_Node").Select(x => { x.Disabled = true; return x; });

                foreach (var item in MenuItems)
                {
                    if (item.Text == translationState.Translate("Edit_Label") || item.Text == translationState.Translate("Remove_Node"))
                        item.Disabled = true;
                }
            }
            else
            {
                foreach (var item in MenuItems)
                {
                    if (item.Text == translationState.Translate("Edit_Label") || item.Text == translationState.Translate("Remove_Node"))
                        item.Disabled = false;
                }
            }
        }
        #endregion

        #region Method Triggered After Context Menu Selection

        // Triggers when context menu is selected
        public async Task MenuSelect(MenuEventArgs<MenuItem> args)
        {
            string selectedText;
            selectedText = args.Item.Text;
            if (selectedText == translationState.Translate("Edit_Label"))
            {
                isEdit = true;
                await this.RenameNodes();

            }
            else if (selectedText == translationState.Translate("Remove_Node"))
            {
                this.RemoveNodes();
            }
            else if (selectedText == translationState.Translate("Add_SubCategory"))
            {
                isEdit = false;
                await this.AddNodes();
            }
        }
        #endregion

        #region TreeView Events Calls
        // Triggers when TreeView Node is selected
        public void OnSelect(NodeSelectEventArgs args)
        {
            this.selectedId = args.NodeData.Id;
        }

        // Triggers when TreeView node is clicked
        public void nodeClicked(NodeClickEventArgs args)
        {
            selectedId = args.NodeData.Id;
        }
        //Triggered after the node being edited
        public async Task NodeEdited(NodeEditEventArgs args)
        {
            if (!isEdit)
            {
                lLSLegalPrincipleCategoryDetails
                   .Where(x => x.CategoryId == index)
                   .Select(x => { x.Name = args.NewText; return x; }).ToList();
            }
            else
            {
                lLSLegalPrincipleCategoryDetails
                    .Where(x => x.CategoryId == Convert.ToInt32(selectedId))
                    .Select(x => { x.Name = args.NewText; return x; }).ToList();
            }
            if (string.IsNullOrWhiteSpace(args.NewText))
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Node_Name_Cannot_be_Empty"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                if (!isEdit)
                    lLSLegalPrincipleCategoryDetails.Remove(lLSLegalPrincipleCategoryDetails.FirstOrDefault(x => x.CategoryId == index));
                await PopulateMethods();
                return;
            }
            else
            {
                string Message = isEdit ? translationState.Translate("Sure_Rename_Category") : translationState.Translate("Sure_Add_Category");
                if (await dialogService.Confirm(Message, translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    if (!isEdit)
                    {
                        selectedId = args.NodeData.Id;
                        selectedNodes = new string[] { args.NodeData.Id };
                        LLSLegalPrincipleCategory llsCategory = new LLSLegalPrincipleCategory
                        {
                            ParentId = Convert.ToInt32(args.NodeData.ParentID),
                            Name = args.NewText,
                        };
                        await AddCategory(llsCategory);
                    }
                    else
                    {
                        var updateNode = lLSLegalPrincipleCategoryDetails.Where(x => x.CategoryId == Convert.ToInt32(args.NodeData.Id)).FirstOrDefault();
                        LLSLegalPrincipleCategory updateCategory = new LLSLegalPrincipleCategory
                        {
                            CategoryId = updateNode.CategoryId,
                            Name = args.NewText,
                        };
                        await UpdateCategory(updateCategory);
                    }
                    await PopulateMethods();
                    spinnerService.Hide();

                }
                else
                {
                    spinnerService.Show();
                    args.Cancel = true;
                    await PopulateMethods();
                    spinnerService.Hide();
                    return;
                }
            }
        }


        #endregion

        #region Add Legal Principle Category

        protected async Task AddCategory(LLSLegalPrincipleCategory args)
        {
            try
            {
                spinnerService.Show();
                args.IsActive = true;
                var result = await lLSLegalPrincipleService.SaveLegalPrincipleCategory(args);
                if (result.IsSuccessStatusCode && (bool)result.ResultData)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Category_Save_Success_Message"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    await PopulateMethods();
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("something_went_wrong"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
                spinnerService.Hide();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Update Legal Principle Category

        protected async Task UpdateCategory(LLSLegalPrincipleCategory args)
        {
            spinnerService.Show();
            args.IsActive = true;
            var result = await lLSLegalPrincipleService.UpdateLegalPrincipleCategory(args);
            if (result.IsSuccessStatusCode && (bool)result.ResultData)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Success,
                    Detail = translationState.Translate("Category_Updated_Successfully"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                await PopulateMethods();
            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("something_went_wrong"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
            spinnerService.Hide();
        }
        #endregion

        #region Delete Legal Principle Category
        //soft delete
        protected async Task DeleteCategory(LLSLegalPrincipleCategory args)
        {
            spinnerService.Show();
            args.IsActive = true;
            var result = await lLSLegalPrincipleService.DeleteLegalPrincipleCategory(args);
            if (result.IsSuccessStatusCode && (bool)result.ResultData)
            {
                await PopulateMethods();
                StateHasChanged();
                await Task.Delay(500);
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Success,
                    Detail = translationState.Translate("Category_Deleted_Successfully"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("something_went_wrong"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
            spinnerService.Hide();
        }
        #endregion

        #region Get Principle Categories
        private async Task GetLLSLegaPrincipleCategories()
        {
            var response = await lLSLegalPrincipleService.GetLLSLegaPrincipleCategories();
            if (response.IsSuccessStatusCode)
            {
                lLSLegalPrincipleCategoryDetails = (List<LLSLegalPrincipleCategoriesVM>)response.ResultData;
                if (lLSLegalPrincipleCategoryDetails.Count > 0)
                {
                    foreach (var item in lLSLegalPrincipleCategoryDetails)
                    {
                        bool IsExpanded = lLSLegalPrincipleCategoryDetails.Any(x => x.ParentId == item.CategoryId);
                        item.Expanded = IsExpanded;
                        item.HasSubChildren = IsExpanded;
                    }
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

        #region Context Select Methods 

        // To add a new node
        async Task AddNodes()
        {
            // Expand the selected nodes
            expandedNodes = new string[] { this.selectedId };

            int NodeId = index;
            lLSLegalPrincipleCategoryDetails.Add(new LLSLegalPrincipleCategoriesVM
            {
                CategoryId = NodeId,
                Name = "New Category",
                ParentId = Convert.ToInt32(this.selectedId)
            });

            lLSLegalPrincipleCategoryDetails.FirstOrDefault(x => x.CategoryId == int.Parse(selectedId)).Expanded = true;
            lLSLegalPrincipleCategoryDetails.FirstOrDefault(x => x.CategoryId == int.Parse(selectedId)).HasSubChildren = true;

            await Task.Delay(100);
            StateHasChanged();

            // Edit the added node.
            await this.tree.BeginEditAsync(NodeId.ToString());
        }

        // To delete a tree node
        async void RemoveNodes()
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Remove_Category"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                var NodeToRemove = tree.GetNode(selectedId);
                LLSLegalPrincipleCategory removeNode = new LLSLegalPrincipleCategory()
                {
                    CategoryId = int.Parse(NodeToRemove.Id),
                    IsDeleted = true,
                };
                await DeleteCategory(removeNode);
            }
            else
                return;
        }

        // To edit a tree node
        async Task RenameNodes()
        {
            await this.tree.BeginEditAsync(this.selectedId);
        }
        #endregion

        #region Wizard Finish
        protected void OnWizardFinish()
        {
            ShowWizard = false;
        }
        #endregion

        #region Cancel button click
        protected async Task CancelLegalPrincipleForm()
        {
            if (await dialogService.Confirm(translationState.Translate("Principle_Cancel"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                var response = await legalLegislationService.DeleteAttachmentFromTempTable(lLSLegalPrinciple.PrincipleId);
                if (response.IsSuccessStatusCode)
                {
                }
                await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
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

        protected async Task DocumentLoaded(LoadEventArgs args)
        {
            if (args.PageData != null)
            {
                await JSRuntime.InvokeVoidAsync("addRotateButtonToPdfToolbar");
            }
        }
    }
}
