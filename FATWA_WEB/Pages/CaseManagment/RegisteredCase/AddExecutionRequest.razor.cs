using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_GENERAL.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Syncfusion.Blazor.PdfViewerServer;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Telerik.Blazor.Components;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    public partial class AddExecutionRequest : ComponentBase
    {

        #region Parameters
        [Parameter]
        public string CaseId { get; set; }
        #endregion

        #region Variables
        public Guid? FileId { get; set; }
        public Guid FileIdForViewDetail { get; set; }

        public Guid referenceId { get; set; }
        protected CmsRegisteredCaseDetailVM registeredCase { get; set; } = new CmsRegisteredCaseDetailVM();
        protected MojExecutionRequest executionRequest { get; set; } = new MojExecutionRequest { Id = Guid.NewGuid() };
        protected bool RefreshFileUploadGrid { get; set; } = true;
        protected bool IsRespectiveSector { get; set; }
        protected List<AttachmentType> AttachmentTypes { get; set; }
        protected RadzenDataGrid<TempAttachementVM> gridAttachments { get; set; }
        protected ObservableCollection<TempAttachementVM> attachments { get; set; }
        public bool allowRowSelectOnRowClick = true;
        public byte[] FileDataViewer { get; set; }
        public bool DisplayDocumentViewer { get; set; }
        public TelerikPdfViewer PdfViewerRef { get; set; }
        public SfPdfViewerServer pdfViewer;
        public string DocumentPath { get; set; } = string.Empty;
        #endregion 

        #region Component Load

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Component Load</History>
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();

            var result = await cmsRegisteredCaseService.GetRegisteredCaseDetailByIdVM(Guid.Parse(CaseId));

            if (result.IsSuccessStatusCode)
            {
                registeredCase = (CmsRegisteredCaseDetailVM)result.ResultData;

                if (registeredCase.IsDissolved == null)
                    registeredCase.IsDissolved = false;
                await PopulateAttachmentTypes();
                await PopulateAttachementsGrid();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(result);
            }

            spinnerService.Hide();
        }

        #endregion

        #region Grid Events


        //<History Author = 'Hassan Abbas' Date='2023-03-20' Version="1.0" Branch="master"> Populate Attachments Grid</History>
        protected async Task PopulateAttachementsGrid()
        {
            try
            {
                attachments = await fileUploadService.GetUploadedAttachements(false, 0, Guid.Parse(CaseId));
                if (attachments != null && attachments.Any())
                {
                    Regex regex = new Regex("_[0-9]+", RegexOptions.RightToLeft);
                    attachments = new ObservableCollection<TempAttachementVM>(attachments?.Select(f => { if (f.FileName.Contains("Document_Portfolio")) return f; else f.FileName = regex.Replace(f.FileName, "", 1); return f; }).ToList());
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
        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Populate Subtypes data</History>
        protected async Task PopulateAttachmentTypes()
        {
            var response = await lookupService.GetAttachmentTypes((int)WorkflowModuleEnum.CaseManagement);
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
        #endregion

        #region Redirect Function
        //<History Author = 'Hassan Abbas' Date='2022-02-03' Version="1.0" Branch="master"> Redirect back to previous page from browser history</History>
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }

        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        #endregion

        #region Submit 
        protected async Task SubmitExecutionRequest()
        {
            try
            {
                if (!executionRequest.SelectedDocuments.Any())
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Select_Documents_For_Execution"),
                        Style = "position: fixed !important; left: 0; margin: auto;"
                    });
                    return;
                }
                if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    spinnerService.Show();
                    executionRequest.CaseId = Guid.Parse(CaseId);
                    executionRequest.SectorTypeId = (int)loginState.UserDetail.SectorTypeId;
                    ApiCallResponse response = await cmsRegisteredCaseService.AddExecutionRequest(executionRequest);

                    if (response.IsSuccessStatusCode)
                    {
                        await CopySelectedAttachmentsToDestination();
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Execution_Request_Created"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        await RedirectBack();
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    spinnerService.Hide();
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

        protected async Task CopySelectedAttachmentsToDestination()
        {
            try
            {
                CopySelectedAttachmentsVM copyAttachments = new CopySelectedAttachmentsVM
                {
                    SelectedDocuments = executionRequest.SelectedDocuments.ToList(),
                    DestinationId = executionRequest.Id,
                    CreatedBy = loginState.Username
                };
                var docResponse = await fileUploadService.CopySelectedAttachmentsToDestination(copyAttachments);
                if (!docResponse.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Attachment_Save_Failed"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
                }
            }
            catch (Exception)
            {
                return;
                throw;
            }
        }

        #endregion

        #region Document Events

        //<History Author = 'Hassan Abbas' Date='2023-03-20' Version="1.0" Branch="master"> View Attachment either Pdf or Image (read in stream and convert to PDF document) in Pdf Viewer</History>
        protected async Task ViewAttachement(TempAttachementVM theUpdatedItem)
        {
            try
            {
                var physicalPath = string.Empty;
#if DEBUG
                {
                    physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + theUpdatedItem.StoragePath).Replace(@"\\", @"\");

                }
#else
                {
                    physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + theUpdatedItem.StoragePath).Replace(@"\\", @"\");
                    physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
                }
#endif
                if (!String.IsNullOrEmpty(physicalPath))
                {
                    FileDataViewer = await DocumentEncryptionService.GetDecryptedDocumentBytes(physicalPath, theUpdatedItem.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
                    string base64String = Convert.ToBase64String(FileDataViewer);
                    DocumentPath = "data:application/pdf;base64," + base64String;
                    DisplayDocumentViewer = true;
                    StateHasChanged();
                    await Task.Delay(500);
                    await JsInterop.InvokeVoidAsync("ScrollPortfolioGridToBottom");
                }
                else
                {

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

                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Summary = translationState.Translate("Error"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }

        }

        #endregion
    }
}
