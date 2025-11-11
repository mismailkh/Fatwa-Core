using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Dms;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_DOMAIN.Models.WorkflowModels;
using FATWA_GENERAL.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using SelectPdf;
using Syncfusion.Blazor.PdfViewerServer;
using System.Collections.ObjectModel;
using Telerik.Blazor.Components;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.DmsEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;

namespace FATWA_WEB.Pages.Dms
{
    //<History Author = 'Hassan Abbas' Date='2023-06-23' Version="1.0" Branch="master"> Detail Document</History>
    public partial class DetailDocument : ComponentBase
    {
        #region Parameters
        [Parameter]
        public dynamic DocumentId { get; set; }
        [Parameter]
        public dynamic VersionId { get; set; }
        public Guid DocumentVersionId { get { return (VersionId == null ? Guid.Empty : Guid.Parse(VersionId)); } set { VersionId = value; } }
        #endregion

        #region Variables

        protected DmsAddedDocument AddedDocument { get; set; } = new DmsAddedDocument { DocumentVersion = new DmsAddedDocumentVersion() };
        protected List<AttachmentType> AttachmentTypes { get; set; } = new List<AttachmentType>();
        protected List<Module> Modules { get; set; } = new List<Module>();
        protected List<DmsDocumentClassification> DocumentClassifications { get; set; } = new List<DmsDocumentClassification>();
        protected List<CaseTemplate> CaseTemplates { get; set; } = new List<CaseTemplate>();
        public List<string> FileTypes { get; set; } = new List<string>() { ".pdf", ".jpg", ".png" };
        public string SaveUrl => _config.GetValue<string>("dms_api_url") + "/FileUpload/Upload";
        public string RemoveUrl => _config.GetValue<string>("dms_api_url") + "/FileUpload/Remove";
        public ObservableCollection<TempAttachementVM> TempFiles { get; set; } = new ObservableCollection<TempAttachementVM>();
        Dictionary<string, bool> FilesValidationInfo { get; set; } = new Dictionary<string, bool>();
        protected TempAttachementVM TempAttachement { get; set; } = new TempAttachementVM();
        public bool Enabled { get; set; } = true;
        public bool Multiple { get; set; } = false;
        public int? MaxFileSize { get; set; }
        public int TemplateId { get; set; }
        protected CaseTemplate Template { get; set; } = new CaseTemplate();
        protected RadzenHtmlEditor editor = new RadzenHtmlEditor();
        public bool isVisible { get; set; }
        public bool showReasonField { get; set; }
        public bool isReasonEntered { get; set; }
        public TelerikPdfViewer PdfViewerRef { get; set; }
        public byte[] FileData { get; set; }
        public MarkupString html { get; set; }
        protected string Reason { get; set; }
        protected string reasonValidationMsg = "";
        public List<DmsAddedDocumentReasonVM> AddedDocumentReasons { get; set; } = new List<DmsAddedDocumentReasonVM>();
        private static List<CaseTemplate> HeaderFooterTemplates { get; set; }
        public string ActivityNameKey { get; set; }
        protected bool ShowDocumentViewer { get; set; }
        public SfPdfViewerServer pdfViewer;
        public string DocumentPath { get; set; } = string.Empty;
        #endregion

        #region Component Load

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateHeaderFooter();
            await PopulateDocumentDetailsByVersionId();
            await PreviewDraft();
            await PopulateReasonsGrid();
            await GetCurrentWorkflowActivityInfo();
            spinnerService.Hide();
        }

        #endregion

        #region Dropdown Data and Change Events

        public async Task PopulateHeaderFooter()
        {
            var response = await cmsCaseTemplateService.GetHeaderFooter();
            if (response.IsSuccessStatusCode)
            {
                HeaderFooterTemplates = (List<CaseTemplate>)response.ResultData;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2023-06-07' Version="1.0" Branch="master">Populate Attachement Types</History>
        protected async Task PopulateDocumentDetailsByVersionId()
        {
            var response = await fileUploadService.GetDocumentDetailByVersionId(DocumentVersionId, Guid.Parse(DocumentId));
            if (response.IsSuccessStatusCode)
            {
                AddedDocument = (DmsAddedDocument)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        #endregion

        #region Preview Draft

        //<History Author = 'Hassan Abbas' Date='2022-10-30' Version="1.0" Branch="master">Preview Draft</History>
        protected async Task PreviewDraft()
        {
            try
            {
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


        //<History Author = 'Hassan Abbas' Date='2023-06-24' Version="1.0" Branch="master"> Populate Reasons Grid</History>
        public async Task PopulateReasonsGrid()
        {
            var response = await fileUploadService.GetAddedDocumentReasonsByReferenceId(DocumentVersionId);
            if (response.IsSuccessStatusCode)
            {
                AddedDocumentReasons = (List<DmsAddedDocumentReasonVM>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }
        #endregion

        #region Document Previewer

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> View Attachment either Pdf or Image (read in stream and convert to PDF document) in Pdf Viewer</History>
        protected async Task ViewAttachement()
        {
            try
            {
                var physicalPath = string.Empty; 
#if DEBUG
                {
                    physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + AddedDocument.DocumentVersion.StoragePath).Replace(@"\\", @"\");

                }
#else
                {
                    physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + AddedDocument.DocumentVersion.StoragePath).Replace(@"\\", @"\");
                    physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
                }
#endif
                if (!String.IsNullOrEmpty(physicalPath))
                {
                    FileData = await DocumentEncryptionService.GetDecryptedDocumentBytes(physicalPath, AddedDocument.DocumentVersion.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
                    string base64String = Convert.ToBase64String(FileData);
                    DocumentPath = "data:application/pdf;base64," + base64String;
                    ShowDocumentViewer = true;
                    //StateHasChanged();
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


        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Populate Case Template Parameters</History>
        protected async Task PopulatePdfFromHtml()
        {
            try
            {
                if (AddedDocument.ClassificationId == (int)DocumentClassificationEnum.External)
                {
                    await ViewAttachement();
                }
                else
                {
                    HtmlToPdf converter = new HtmlToPdf();
                    MemoryStream stream = new MemoryStream();

                    // set converter optionsr
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
                    converter.Options.PdfPageSize = PdfPageSize.A4;
                    converter.Options.PdfPageOrientation = PdfPageOrientation.Portrait;
                    converter.Options.WebPageWidth = 1024;
                    converter.Options.WebPageHeight = 1024;

                    // create a new pdf document converting an url
                    string headerHtmlContent = HeaderFooterTemplates.Where(x => x.Id == (Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? (int)CaseTemplateEnum.HeaderEn : (int)CaseTemplateEnum.HeaderAr)).Select(x => x.Content).FirstOrDefault();
                    string footerHtmlContent = HeaderFooterTemplates.Where(x => x.Id == (int)CaseTemplateEnum.Footer).Select(x => x.Content).FirstOrDefault();
                    PdfHtmlSection headerHtml = new PdfHtmlSection(headerHtmlContent, "");
                    PdfHtmlSection footerHtml = new PdfHtmlSection(footerHtmlContent, "");
                    headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                    footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                    converter.Header.Add(headerHtml);
                    converter.Footer.Add(footerHtml);
                    converter.Options.EmbedFonts = true;
                    AddedDocument.DocumentVersion.Content = string.Concat($"<style>@font-face {{font-family: 'Sultan';src: url('{Directory.GetCurrentDirectory().Replace("\\", "/") + "/wwwroot/fonts/Sultan/arfonts-sultan-normal.ttf"}') format('truetype');font-weight: normal;font-style: normal;font-display: swap;}} @font-face {{font-family: 'Sultan Medium';src: url('{Directory.GetCurrentDirectory().Replace("\\", "/") + "/wwwroot/fonts/Sultan/arfonts-sultan-mudaim.ttf"}') format('truetype');font-weight: normal;font-style: normal;font-display: swap;}}</style>", AddedDocument.DocumentVersion.Content);
                    SelectPdf.PdfDocument pdfDocument = converter.ConvertHtmlString(AddedDocument.DocumentVersion.Content);
                    pdfDocument.Save(stream);
                    pdfDocument.Close();
                    stream.Close();
                    FileData = stream.ToArray();
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

        #endregion

        #region Approve/Reject Document

        //<History Author = 'Hassan Abbas' Date='2022-10-30' Version="1.0" Branch="master">Approve Document through Workflow</History>
        protected async Task ApproveDocument()
        {
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
                      translationState.Translate("Approve_Confirm"),
                      translationState.Translate("Confirm"),
                      new ConfirmOptions()
                      {
                          OkButtonText = translationState.Translate("OK"),
                          CancelButtonText = translationState.Translate("Cancel")
                      });
                if (dialogResponse == true)
                {
                    spinnerService.Show();
                    var response = await cmsCaseTemplateService.GetDMSDocumentDetailById(AddedDocument.Id, AddedDocument.DocumentVersion.Id);
                    //ApiCallResponse response = await fileUploadService.SaveAddedDocument(AddedDocument);
                    if (response.IsSuccessStatusCode)
                    {
                        AddedDocument = (DmsAddedDocument)response.ResultData;
                        AddedDocument.SubModuleId = await GetSubmoduleId();
                        AddedDocument.SectorTypeId = (int)loginState.UserDetail.SectorTypeId;
                        AddedDocument.UserLoginState = loginState.Username;
                        if (AddedDocument.DocumentVersion.StatusId == (int)DocumentStatusEnum.InReview)
                        {
                            if (AddedDocument.DocumentVersion == null)
                            {
                                AddedDocument.DocumentVersion = new DmsAddedDocumentVersion();
                            }
                            AddedDocument.DocumentVersion.AddedDocumentId = AddedDocument.Id;
                            AddedDocument.DocumentVersion.VersionNo = AddedDocument.DocumentVersion.VersionNo;
                            AddedDocument.DocumentVersion.StatusId = (int)DocumentStatusEnum.Approved;
                            AddedDocument.DocumentVersion.FileName = AddedDocument.DocumentVersion.FileName;
                            AddedDocument.DocumentVersion.StoragePath = AddedDocument.DocumentVersion.StoragePath;
                            AddedDocument.DocumentVersion.DocType = AddedDocument.DocumentVersion.DocType;
                            AddedDocument.DocumentVersion.Content = AddedDocument.DocumentVersion.Content;
                            AddedDocument.DocumentVersion.ReviewerUserId = "";
                            AddedDocument.DocumentVersion.ReviewerRoleId = "";
                            AddedDocument.DocumentVersion.CreatedBy = loginState.Username;
                            AddedDocument.DocumentVersion.CreatedDate = DateTime.Now;
                            AddedDocument.DocumentVersion.ModifiedBy = loginState.Username;
                            AddedDocument.DocumentVersion.ModifiedDate = DateTime.Now;
                            AddedDocument.DocumentVersion.IsPreviousVersionApproved = true;
                        }
                        else
                        {
                            if (AddedDocument.DocumentVersion == null)
                            {
                                AddedDocument.DocumentVersion = new DmsAddedDocumentVersion();
                            }
                            AddedDocument.DocumentVersion.AddedDocumentId = AddedDocument.Id;
                            AddedDocument.DocumentVersion.VersionNo = AddedDocument.DocumentVersion.VersionNo;
                            AddedDocument.DocumentVersion.StatusId = (int)DocumentStatusEnum.Approved;
                            AddedDocument.DocumentVersion.FileName = AddedDocument.DocumentVersion.FileName;
                            AddedDocument.DocumentVersion.StoragePath = AddedDocument.DocumentVersion.StoragePath;
                            AddedDocument.DocumentVersion.DocType = AddedDocument.DocumentVersion.DocType;
                            AddedDocument.DocumentVersion.Content = AddedDocument.DocumentVersion.Content;
                            AddedDocument.DocumentVersion.ReviewerUserId = "";
                            AddedDocument.DocumentVersion.ReviewerRoleId = "";
                            AddedDocument.DocumentVersion.CreatedBy = loginState.Username;
                            AddedDocument.DocumentVersion.CreatedDate = DateTime.Now;
                            AddedDocument.DocumentVersion.ModifiedBy = loginState.Username;
                            AddedDocument.DocumentVersion.ModifiedDate = DateTime.Now;
                            AddedDocument.DocumentVersion.IsPreviousVersionApproved = true;
                        }
                        await workflowService.ProcessWorkflowActvivities(AddedDocument, (int)WorkflowModuleEnum.DMS, (int)WorkflowModuleTriggerEnum.UserSubmitsDMSDocument);
                        spinnerService.Hide();
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Changes_saved_successfully"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        await RedirectBack();
                    }
                    else
                    {
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

        //<History Author = 'Hassan Abbas' Date='2023-06-23' Version="1.0" Branch="master">Reject Document</History>
        protected async Task EditDocument()
        {
            navigationManager.NavigateTo("document-add/" + AddedDocument.Id);
        }

        //<History Author = 'Hassan Abbas' Date='2023-06-23' Version="1.0" Branch="master">Reject Document</History>
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
                    var response = await cmsCaseTemplateService.GetDMSDocumentDetailById(AddedDocument.Id, AddedDocument.DocumentVersion.Id);
                    //ApiCallResponse response = await fileUploadService.SaveAddedDocument(AddedDocument);
                    if (response.IsSuccessStatusCode)
                    {
                        AddedDocument = (DmsAddedDocument)response.ResultData;
                        AddedDocument.SubModuleId = await GetSubmoduleId();
                        AddedDocument.SectorTypeId = (int)loginState.UserDetail.SectorTypeId;
                        AddedDocument.UserLoginState = loginState.Username;
                        if (AddedDocument.DocumentVersion.StatusId == (int)DocumentStatusEnum.InReview)
                        {
                            if (AddedDocument.DocumentVersion == null)
                            {
                                AddedDocument.DocumentVersion = new DmsAddedDocumentVersion();
                            }
                            AddedDocument.DocumentVersion.StatusId = (int)DocumentStatusEnum.Rejected;
                            AddedDocument.DocumentVersion.Reason = Reason;
                            AddedDocument.DocumentVersion.ModifiedBy = loginState.Username;
                            AddedDocument.DocumentVersion.ModifiedDate = DateTime.Now;
                            AddedDocument.DocumentVersion.AddedDocumentId = AddedDocument.Id;
                            AddedDocument.DocumentVersion.VersionNo = AddedDocument.DocumentVersion.VersionNo;
                            AddedDocument.DocumentVersion.FileName = AddedDocument.DocumentVersion.FileName;
                            AddedDocument.DocumentVersion.StoragePath = AddedDocument.DocumentVersion.StoragePath;
                            AddedDocument.DocumentVersion.DocType = AddedDocument.DocumentVersion.DocType;
                            AddedDocument.DocumentVersion.Content = AddedDocument.DocumentVersion.Content;
                            AddedDocument.DocumentVersion.ReviewerUserId = "";
                            AddedDocument.DocumentVersion.ReviewerRoleId = "";
                            AddedDocument.DocumentVersion.CreatedBy = loginState.Username;
                            AddedDocument.DocumentVersion.CreatedDate = DateTime.Now;
                        }
                        else
                        {
                            if (AddedDocument.DocumentVersion == null)
                            {
                                AddedDocument.DocumentVersion = new DmsAddedDocumentVersion();
                            }
                            AddedDocument.DocumentVersion.StatusId = (int)DocumentStatusEnum.Rejected;
                            AddedDocument.DocumentVersion.Reason = Reason;
                            AddedDocument.DocumentVersion.ModifiedBy = loginState.Username;
                            AddedDocument.DocumentVersion.ModifiedDate = DateTime.Now;
                            AddedDocument.DocumentVersion.AddedDocumentId = AddedDocument.Id;
                            AddedDocument.DocumentVersion.VersionNo = AddedDocument.DocumentVersion.VersionNo;
                            AddedDocument.DocumentVersion.FileName = AddedDocument.DocumentVersion.FileName;
                            AddedDocument.DocumentVersion.StoragePath = AddedDocument.DocumentVersion.StoragePath;
                            AddedDocument.DocumentVersion.DocType = AddedDocument.DocumentVersion.DocType;
                            AddedDocument.DocumentVersion.Content = AddedDocument.DocumentVersion.Content;
                            AddedDocument.DocumentVersion.ReviewerUserId = "";
                            AddedDocument.DocumentVersion.ReviewerRoleId = "";
                            AddedDocument.DocumentVersion.CreatedBy = loginState.Username;
                            AddedDocument.DocumentVersion.CreatedDate = DateTime.Now;
                        }
                        await workflowService.ProcessWorkflowActvivities(AddedDocument, (int)WorkflowModuleEnum.DMS, (int)WorkflowModuleTriggerEnum.UserSubmitsDMSDocument);
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
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                }
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Style = "position: fixed !important; left: 0; margin: auto;"
                });
            }
        }
        protected async Task<int> GetSubmoduleId()
        {
            try
            {
                AddedDocument.SubModuleId = 0;
                if (loginState.UserDetail.SectorTypeId >= (int)OperatingSectorTypeEnum.AdministrativeUnderFilingCases && loginState.UserDetail.SectorTypeId <= (int)OperatingSectorTypeEnum.AdministrativeSupremeCases)
                {
                    AddedDocument.SubModuleId = (int)RequestTypeEnum.Administrative;
                }
                else if (loginState.UserDetail.SectorTypeId >= (int)OperatingSectorTypeEnum.CivilCommercialUnderFilingCases && loginState.UserDetail.SectorTypeId <= (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases)
                {
                    AddedDocument.SubModuleId = (int)RequestTypeEnum.CivilCommercial;
                }
                else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.LegalAdvice)
                {
                    AddedDocument.SubModuleId = (int)WorkflowSubModuleEnum.LegalAdvice;
                }
                else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.Legislations)
                {
                    AddedDocument.SubModuleId = (int)WorkflowSubModuleEnum.Legislations;
                }
                else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeComplaints)
                {
                    AddedDocument.SubModuleId = (int)WorkflowSubModuleEnum.AdministrativeComplaints;
                }
                else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.Contracts)
                {
                    AddedDocument.SubModuleId = (int)WorkflowSubModuleEnum.Contracts;
                }
                else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.InternationalArbitration)
                {
                    AddedDocument.SubModuleId = (int)WorkflowSubModuleEnum.InternationalArbitration;
                }
                else if (loginState.UserDetail.SectorTypeId == 0 && AddedDocument.ModuleId == (int)WorkflowModuleEnum.LDSDocument)
                {
                    AddedDocument.SubModuleId = (int)WorkflowSubModuleEnum.LegalLegislations;
                }
                else if (loginState.UserDetail.SectorTypeId == 0 && AddedDocument.ModuleId == (int)WorkflowModuleEnum.LPSPrinciple)
                {
                    AddedDocument.SubModuleId = (int)WorkflowSubModuleEnum.LegalPrinciples;
                }
                return AddedDocument.SubModuleId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2023-06-23' Version="1.0" Branch="master">Link Document with an Entity</History>
        protected async Task LinkDocument()
        {
            var result = await dialogService.OpenAsync<LinkDocument>(translationState.Translate("Link_Document"),
                new Dictionary<string, object>()
                {
                        { "DocumentVersionId", DocumentVersionId },
                },
                new DialogOptions() { Width = "80% !important", CloseDialogOnOverlayClick = true }
            );
        }

        #endregion
        #region Remarks/Reason for Rejection
        protected async Task RemarksForRejection()
        {
            try
            {
                if (!showReasonField)
                    showReasonField = true;
                if (String.IsNullOrEmpty(Reason))
                {
                    reasonValidationMsg = @translationState.Translate("Required_Field");
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Required_Field_Reason"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    return;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void ChangeOnInput(ChangeEventArgs e)
        {
            Reason = e.Value?.ToString();
            isReasonEntered = !string.IsNullOrWhiteSpace(Reason);
        }
        #endregion

        #region Form Events
        public async Task GetCurrentWorkflowActivityInfo()
        {
            var result = await workflowService.GetInstanceCurrentActivity(AddedDocument.Id);
            if (result != null)
            {
                AddedDocument.WorkflowActivityId = result.WorkflowActivityId;
                if (AddedDocument.WorkflowActivityId > 0)
                {
                    var activity = await workflowService.GetWorkflowActivityBySequenceNumber((int)result.WorkflowId, (int)result.SequenceNumber);
                    ActivityNameKey = activity.AKey;
                }
            }
        }
        #endregion

        #region Redirect Function

        //<History Author = 'Hassan Abbas' Date='2022-11-25' Version="1.0" Branch="master">Redirect to respective page</History>
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
    }
    #endregion
}

