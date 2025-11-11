//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Components.Web;
//using Radzen;
//using FATWA_WEB.Services;
//using FATWA_DOMAIN.Models;
//using FATWA_WEB.Data;
//using Syncfusion.Blazor.PdfViewerServer;
//using Syncfusion.Blazor.PdfViewer;
//using static FATWA_DOMAIN.Enums.WorkflowEnums;
//using System.Reflection;
//using static FATWA_GENERAL.Helper.Response;
//using static FATWA_GENERAL.Helper.Enum;
//using FATWA_DOMAIN.Models.WorkflowModels;
//using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;

//namespace FATWA_WEB.Pages
//{
//    public partial class DocumentApproveRejectComponent : ComponentBase
//    {
//        [Parameter(CaptureUnmatchedValues = true)]
//        public dynamic DocumentId { get; set; }

//        #region Service Injection

//         
//         

//         
//         

//         
//         

//         
//         

//         
//         

//         
//          

//         
//          

//         
//          

//         
//         

//        #endregion

//        #region Variable Declaration

//        protected LdsDocument legalDocument { get; set; }

//        public string DocumentPath { get; set; }

//        public SfPdfViewerServer pdfViewer;

//        public PdfViewerToolbarSettings ToolbarSettings = new PdfViewerToolbarSettings()
//        {
//            ToolbarItems = new List<ToolbarItem>()
//            {
//                ToolbarItem.MagnificationTool,
//                ToolbarItem.SelectionTool,
//                ToolbarItem.SearchOption,
//                ToolbarItem.CommentTool,
//                ToolbarItem.PrintOption,
//                ToolbarItem.MagnificationTool,
//                ToolbarItem.DownloadOption,
//                ToolbarItem.PrintOption,
//                ToolbarItem.PageNavigationTool,
//                ToolbarItem.PanTool,
//                ToolbarItem.SelectionTool,
//                ToolbarItem.UndoRedoTool
//            }
//        };

//        #endregion
//        public void Reload()
//        {
//            InvokeAsync(StateHasChanged);
//        }

//        public void OnPropertyChanged(PropertyChangedEventArgs args)
//        {
//        }

//        LdsDocument _getLdsDocumentViewResult;
//        protected LdsDocument getLdsDocumentViewResult
//        {
//            get
//            {
//                return _getLdsDocumentViewResult;
//            }
//            set
//            {
//                if (!object.Equals(_getLdsDocumentViewResult, value))
//                {
//                    var args = new PropertyChangedEventArgs() { Name = "getLdsDocumentViewResult", NewValue = value, OldValue = _getLdsDocumentViewResult };
//                    _getLdsDocumentViewResult = value;
//                    OnPropertyChanged(args);
//                    Reload();
//                }
//            }
//        }

//        #region On Load

//        public IEnumerable<LdsDocumentApprovalType> getDocumentApprovalType;

//        protected override async Task OnInitializedAsync()
//        {
//            getDocumentApprovalType = await ldsDocumentService.getDocumentApprovalType();
//            await Load();
//        }

//        protected async Task Load()
//        {
//            try
//            {
//                Guid guid = Guid.Parse(DocumentId);
//                legalDocument = await ldsDocumentService.GetDocumentById(guid);
//                getLdsDocumentViewResult = legalDocument;
                
//                await using var memoryStream = await wordToPdfSyncFusionService.WordToPDF(legalDocument.Content);
//                byte[] byteArray = memoryStream.ToArray();
//                string base64String = Convert.ToBase64String(byteArray);
//                DocumentPath = "data:application/pdf;base64," + base64String;
//                //Change this after demo
//                if (getLdsDocumentViewResult.Status == (int)FATWA_GENERAL.Helper.Enum.DocumentApprovalStatus.Draft || getLdsDocumentViewResult.Status == (int)FATWA_GENERAL.Helper.Enum.DocumentApprovalStatus.InReview)
//                    getLdsDocumentViewResult.Status = 0;
//                await InvokeAsync(StateHasChanged);
//            }
//            catch (Exception)
//            {
//                notificationService.Notify(new NotificationMessage()
//                {
//                    Severity = NotificationSeverity.Error,
//                    Detail = translationState.Translate("Error_Load_Approval_Detail"),
//                    Summary = translationState.Translate("Error"),
//                    Style = "position: fixed !important; left: 0; margin: auto; "
//                });
//            }
//        }

//        #endregion

//        public async Task SavePdfContent()
//        {
//            legalDocument.ReadOnlyContent = null;
//            byte[] data = await pdfViewer.GetDocument();
//            legalDocument.ReadOnlyContent = Convert.ToBase64String(data);
//        }

//        protected async Task FormReasonSubmit(LdsDocument args)
//        {
//            try
//            {
//                await workflowService.ProcessWorkflowActvivities(legalDocument, WorkflowModuleEnum.LDSDocument);

//                notificationService.Notify(new NotificationMessage()
//                {
//                    Severity = NotificationSeverity.Success,
//                    Detail = translationState.Translate("Changes_saved_successfully"),
//                    Style = "position: fixed !important; left: 0; margin: auto; "
//                });
//            }
//            catch (Exception ex)
//            {
//                notificationService.Notify(new NotificationMessage()
//                {
//                    Severity = NotificationSeverity.Error,
//                    Detail = translationState.Translate("Something_Went_Wrong"),
//                    Style = "position: fixed !important; left: 0; margin: auto; "
//                });
//            }

//            DialogService.Close(null);
//            DialogService.Close(null);
//            navigationManager.NavigateTo("/lds-documents-approval");
//        }

//        protected async Task ButtonCloseDialog(MouseEventArgs args)
//        {
//            navigationManager.NavigateTo("/lds-documents-approval");
//            //DialogService.Close(null);
//            //await Load();
//        }

//    }
//}
