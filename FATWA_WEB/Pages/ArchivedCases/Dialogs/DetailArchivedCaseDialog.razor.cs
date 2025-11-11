using FATWA_DOMAIN.Models.ViewModel.ArchivedCases;
using FATWA_GENERAL.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Syncfusion.Blazor.PdfViewer;

namespace FATWA_WEB.Pages.ArchivedCases.Dialogs
{
    //< History Author = 'Ammaar Naveed' Date = '2024-12-12' Version = "1.0" Branch = "master">Added dialog component for archived case detail view</History>

    public partial class DetailArchivedCaseDialog : ComponentBase
    {
        #region Parameters
        [Parameter]
        public dynamic CaseId { get; set; }
        #endregion

        #region Variables Declaration
        ArchivedCaseDetailVM ArchivedCaseDetail = new ArchivedCaseDetailVM();
        bool RefreshGrid = false;
        List<ToolbarItem> ToolbarItems = new List<ToolbarItem>();
        public bool DisplayDocumentViewer { get; set; }
        public Guid? DocumentId { get; set; }
        public string DocumentPath { get; set; }
        private bool IsDocumentSigned { get; set; }

        #endregion

        #region Grid Variables
        protected RadzenDataGrid<ArchivedCasePartiesVM> ArchivedCasePartiesGrid = new RadzenDataGrid<ArchivedCasePartiesVM>();
        protected RadzenDataGrid<ArchivedCaseDocumentsVM> ArchivedCaseDocumentsGrid = new RadzenDataGrid<ArchivedCaseDocumentsVM>();
        #endregion

        #region On Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            translationState.TranslateGridFilterLabels(ArchivedCasePartiesGrid);
            await GetArchivedCaseDetailByCaseId();
            spinnerService.Hide();
        }
        #endregion

        #region Get Archived Case Detail
        private async Task GetArchivedCaseDetailByCaseId()
        {
            var response = await archivedCasesService.GetArchiveCaseDetailByCaseId(CaseId);
            if (response.IsSuccessStatusCode)
            {
                ArchivedCaseDetail = (ArchivedCaseDetailVM)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

        #region View Case Document
        protected async Task ViewAttachement(ArchivedCaseDocumentsVM theUpdatedItem)
        {
            try
            {
                spinnerService.Show();
                var physicalPath = string.Empty;
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
                DisplayDocumentViewer = false;
                StateHasChanged();
                DocumentId = theUpdatedItem.DocumentId;
#if DEBUG
                {
                    physicalPath = Path.Combine(theUpdatedItem.StoragePath).Replace(@"\\", @"\");
                }
#else
				{
                    physicalPath = Path.Combine(_config.GetValue<string>("archiving_document_virtual_path") + theUpdatedItem.StoragePath).Replace(@"\\", @"\");
                    physicalPath = physicalPath.Replace(_config.GetValue<string>("archiving_document_path"), "");
				}
#endif
                if (!String.IsNullOrEmpty(physicalPath))
                {
                    string base64String = await DocumentEncryptionService.GetDecryptedDocumentBase64(physicalPath, theUpdatedItem.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
                    DocumentPath = "data:application/pdf;base64," + base64String;
                    DisplayDocumentViewer = true;
                    spinnerService.Hide();
                }
                else
                {
                    spinnerService.Hide();
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("File_Not_Found"),
                        Summary = translationState.Translate("Error"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
            }
            catch (Exception)
            {
                spinnerService.Hide();
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Summary = translationState.Translate("Error"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        #region View Document Helper Methods
        protected async Task OpenInNewWindow(MouseEventArgs args)
        {
            JsInterop.InvokeVoidAsync("openNewWindow", "/preview-archived-document/" + DocumentId);
        }
        protected async Task DocumentLoaded(LoadEventArgs args)
        {
            await JSRuntime.InvokeVoidAsync("addRotateButtonToPdfToolbar");
            if (IsDocumentSigned)
                await JSRuntime.InvokeVoidAsync("addSingaturePanelBtnToPdfToolbar");
        }
        #endregion

        #endregion

    }
}
