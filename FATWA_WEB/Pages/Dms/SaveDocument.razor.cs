using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Dms;
using FATWA_DOMAIN.Models.TaskModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_DOMAIN.Models.WorkflowModels;
using FATWA_GENERAL.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Radzen;
using Radzen.Blazor;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using Telerik.Blazor;
using Telerik.Blazor.Components;
using Telerik.Blazor.Components.Upload;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.DmsEnums;
using static FATWA_DOMAIN.Enums.TaskEnums;
using static FATWA_DOMAIN.Enums.UserEnum;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.Dms
{
    //<History Author = 'Hassan Abbas' Date='2023-06-13' Version="1.0" Branch="master"> Add Document Form as DMS Utility</History>
    public partial class SaveDocument : ComponentBase
    {
        #region Parameters
        [Parameter]
        public dynamic? DocumentId { get; set; }
        [Parameter]
        public dynamic? TaskId { get; set; }
        public Guid AddedDocumentId { get { return (DocumentId == null ? Guid.Empty : Guid.Parse(DocumentId)); } set { DocumentId = value; } }
        public Guid DocumentTaskId { get { return (TaskId == null ? Guid.Empty : Guid.Parse(TaskId)); } set { TaskId = value; } }
        #endregion

        #region Variables
        private Guid AttachmentReferenceId = Guid.NewGuid();
        public TelerikPdfViewer PdfViewerRef { get; set; }
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
        protected TaskDetailVM taskDetailVM { get; set; } = new TaskDetailVM();
        public bool Enabled { get; set; } = true;
        public bool Multiple { get; set; } = false;
        public bool ShowModuleDD { get; set; } = false;
        public int? MaxFileSize { get; set; }
        public int TemplateId { get; set; }
        protected CaseTemplate Template { get; set; } = new CaseTemplate();
        protected RadzenHtmlEditor editor = new RadzenHtmlEditor();
        public byte[] FileData { get; set; }
        #endregion

        #region Component Load

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateDocumentNumberAndVersion();
            await PopulateModules();
            await PopulateAttachmentTypes();
            await PopulateDocumentClassifications();
            await PopulateTemplateContent();
            await PopulateTaskDetails();
            spinnerService.Hide();
        }

        #endregion

        #region Dropdown Data and Change Events

        //<History Author = 'Hassan Abbas' Date='2023-06-07' Version="1.0" Branch="master">Populate Attachement Types</History>
        protected async Task PopulateAttachmentTypes()
        {
            if (AddedDocument.ModuleId > 0)
            {
                var response = await lookupService.GetAttachmentTypes(AddedDocument.ModuleId);
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
            else
            {
                AttachmentTypes = new List<AttachmentType>();
            }
        }

        //<History Author = 'Hassan Abbas' Date='2023-06-07' Version="1.0" Branch="master">Populate Attachement Types</History>
        protected async Task PopulateModules()
        {
            if (loginState.UserDetail.SectorTypeId != null && loginState.UserDetail.SectorTypeId > 0)
            {
                if ((loginState.UserDetail.SectorTypeId >= (int)OperatingSectorTypeEnum.AdministrativeUnderFilingCases && loginState.UserDetail.SectorTypeId <= (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases) || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.Execution)
                {
                    AddedDocument.ModuleId = (int)WorkflowModuleEnum.CaseManagement;
                }
                else if (loginState.UserDetail.SectorTypeId >= (int)OperatingSectorTypeEnum.LegalAdvice && loginState.UserDetail.SectorTypeId <= (int)OperatingSectorTypeEnum.PublicOperationalSector)
                {
                    AddedDocument.ModuleId = (int)WorkflowModuleEnum.COMSConsultationManagement;
                }
            }
            else
            {
                ShowModuleDD = true;
                var response = await lookupService.GetModules();
                if (response.IsSuccessStatusCode)
                {
                    Modules = (List<Module>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                StateHasChanged();
            }
        }

        //<History Author = 'Hassan Abbas' Date='2023-06-15' Version="1.0" Branch="master">Populate Attachement Types</History>
        protected async Task PopulateDocumentClassifications()
        {
            var response = await lookupService.GetDocumentClassifications();
            if (response.IsSuccessStatusCode)
            {
                DocumentClassifications = (List<DmsDocumentClassification>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }


        //<History Author = 'Hassan Abbas' Date='2022-11-10' Version="1.0" Branch="master">Populate Case Template Parameters</History>
        protected async Task PopulateTemplateContent()
        {
            if (TemplateId > 0)
            {
                var response = await cmsCaseTemplateService.GetCaseTemplateDetail(TemplateId);
                if (response.IsSuccessStatusCode)
                {
                    Template = (CaseTemplate)response.ResultData;
                    AddedDocument.DocumentVersion.Content = Template.Content;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            StateHasChanged();
        }
        //<History Author = 'Hassan Abbas' Date='2023-06-20' Version="1.0" Branch="master">Populate Document Number and Version</History>
        protected async Task PopulateDocumentNumberAndVersion()
        {
            var response = await fileUploadService.GetDocumentNumberAndVersion(AddedDocumentId);
            if (response.IsSuccessStatusCode)
            {
                AddedDocument = (DmsAddedDocument)response.ResultData;
                if (AddedDocumentId != Guid.Empty && AddedDocument.ClassificationId == (int)DocumentClassificationEnum.External)
                {
                    TempFiles.Add(new TempAttachementVM
                    {
                        FileName = AddedDocument.DocumentVersion.FileName,
                        DocType = AddedDocument.DocumentVersion.DocType,
                        DocDateTime = AddedDocument.DocumentVersion.CreatedDate,
                        DocumentDate = AddedDocument.DocumentVersion.CreatedDate,
                    });
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Populate Case Templates</History>
        protected async Task PopulateTemplates()
        {
            var response = await lookupService.GetCaseTemplates((int)AddedDocument.AttachmentTypeId);
            if (response.IsSuccessStatusCode)
            {
                CaseTemplates = (List<CaseTemplate>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        //<History Author = 'Hassan Abbas' Date='2023-03-10' Version="1.0" Branch="master">Populate Task Detail to be Approved or Rejected</History>
        protected async Task PopulateTaskDetails()
        {
            if (DocumentTaskId != Guid.Empty)
            {
                ApiCallResponse taskResponse = await taskService.GetTaskDetailById(DocumentTaskId);
                if (taskResponse.IsSuccessStatusCode)
                {
                    taskDetailVM = (TaskDetailVM)taskResponse.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(taskResponse);
                }
            }
        }
        //<History Author = 'Hassan Abbas' Date='2023-06-13' Version="1.0" Branch="master"> Populate Attachment Types on Module Change</History>
        protected async Task OnModuleChange()
        {
            await PopulateAttachmentTypes();
        }

        //<History Author = 'Hassan Abbas' Date='2023-06-13' Version="1.0" Branch="master"> Populate Templates on Classification Change</History>
        protected async Task OnClassificationChange()
        {
            if (AddedDocument.ClassificationId == (int)DocumentClassificationEnum.FreeEditor)
            {
                Template = new CaseTemplate();
                AddedDocument.DocumentVersion.Content = null;
            }
            else if (AddedDocument.ClassificationId == (int)DocumentClassificationEnum.PredefinedTemplate)
            {
                if (AddedDocument.AttachmentTypeId > 0)
                {
                    await PopulateTemplates();
                }
                else
                {
                    Template = new CaseTemplate();
                    AddedDocument.DocumentVersion.Content = null;
                }
            }
            else
            {
                CaseTemplates = new List<CaseTemplate>();
                TemplateId = 0;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2023-06-13' Version="1.0" Branch="master"> Populate Templates on Type Change</History>
        protected async Task OnTypeChange()
        {
            if (AddedDocument.AttachmentTypeId > 0 && AddedDocument.ClassificationId == (int)DocumentClassificationEnum.PredefinedTemplate)
            {
                AddedDocument.DocumentName = AttachmentTypes.Where(t => t.AttachmentTypeId == AddedDocument.AttachmentTypeId).FirstOrDefault()?.Type_En + "_" + AddedDocument.DocumentNumber;
                await PopulateTemplates();
            }
            else if (AddedDocument.AttachmentTypeId > 0)
            {
                AddedDocument.DocumentName = AttachmentTypes.Where(t => t.AttachmentTypeId == AddedDocument.AttachmentTypeId).FirstOrDefault()?.Type_En + "_" + AddedDocument.DocumentNumber;
            }
            else
            {
                CaseTemplates = new List<CaseTemplate>();
                TemplateId = 0;
                AddedDocument.DocumentName = "" + AddedDocument.DocumentNumber;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2023-06-13' Version="1.0" Branch="master"> Populate Templates on Type Change</History>
        protected async Task OnTemplateChange()
        {
            if (TemplateId > 0)
            {
                await PopulateTemplateContent();
            }
            else
            {
                Template = new CaseTemplate();
                AddedDocument.DocumentVersion.Content = null;
            }
        }

        #endregion

        #region File Uploader Handlers

        //<History Author = 'Hassan Abbas' Date='2022-05-24' Version="1.0" Branch="master"> On File select check if file already uploaded</History>
        protected void OnSelectHandler(UploadSelectEventArgs e)
        {
            foreach (var item in e.Files)
            {
                if (TempFiles.Where(x => x.FileName == item.Name).Count() > 0)
                {
                    e.IsCancelled = true;
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("File_Already_Uploaded"),
                        Summary = translationState.Translate("Error"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
                if (!FilesValidationInfo.Keys.Contains(item.Id))
                {
                    // nothing is assumed to be valid until the server returns an OK
                    FilesValidationInfo.Add(item.Id, IsSelectedFileValid(item));
                }
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-05-24' Version="1.0" Branch="master"> On File Cancel remove from List</History>
        protected void OnCancelHandler(UploadCancelEventArgs e)
        {
            RemoveFailedFilesFromList(e.Files);
        }

        //<History Author = 'Hassan Abbas' Date='2022-05-24' Version="1.0" Branch="master"> Remove file from list</History>
        protected void OnRemoveHandler(UploadEventArgs e)
        {
            e.RequestHeaders.Add("Authorization", new AuthenticationHeaderValue("Bearer", loginState.Token));
            e.RequestData.Add("_userName", loginState.Username);
            e.RequestData.Add("_uploadFrom", "DMS");
            e.RequestData.Add("_project", "FATWA_WEB");

            RemoveFailedFilesFromList(e.Files);
            //UpdateValidationModel();
        }

        //<History Author = 'Hassan Abbas' Date='2022-05-24' Version="1.0" Branch="master"> Remove uploaded document from Database if temporary else bind value to model and delete in repository</History>
        protected async Task RemoveTempAttachement()
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Remove_File"), translationState.Translate("Remove_File"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                TempFiles = new ObservableCollection<TempAttachementVM>();
                AttachmentReferenceId = Guid.NewGuid();
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-05-24' Version="1.0" Branch="master"> Add necessary parameters like Uploading Source, username, Token etc etc</History>
        protected async Task OnUploadHandler(UploadEventArgs e)
        {
            if (AddedDocument.AttachmentTypeId > 0)
            {
                e.RequestHeaders.Add("Authorization", new AuthenticationHeaderValue("Bearer", loginState.Token));
                e.RequestData.Add("_pEntityIdentifierGuid", AttachmentReferenceId);
                e.RequestData.Add("_userName", loginState.Username);
                e.RequestData.Add("_typeId", AddedDocument.AttachmentTypeId);
                e.RequestData.Add("_uploadFrom", "DMS");
                e.RequestData.Add("_project", "FATWA_WEB");
            }
            else
            {
                e.IsCancelled = true;
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Must_Attachment_Type"),
                    Summary = translationState.Translate("Error"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-05-24' Version="1.0" Branch="master"> Add uploaded document to grid list</History>
        //<History Author = 'Hassan Abbas' Date='2022-11-15' Version="1.0" Branch="master"> Auto save the document in UPLOADED_DOCUMENT table after success</History>
        protected async Task OnSuccessHandler(UploadSuccessEventArgs e)
        {
            var pathResponse = JsonConvert.DeserializeObject<FileUploadSuccessResponse>(e.Request.ResponseText);
            var actionText = e.Operation == UploadOperationType.Upload ? "uploaded" : "removed";
            TempAttachementVM tempAttachement = new TempAttachementVM();
            tempAttachement.DocType = e.Files[0].Extension;
            tempAttachement.FileName = e.Files[0].Name;
            tempAttachement.UploadedBy = loginState.Username;
            tempAttachement.DocDateTime = DateTime.Now;
            tempAttachement.DocumentDate = DateTime.Now;
            var type = AttachmentTypes?.Where(t => t.AttachmentTypeId == TempAttachement.AttachmentTypeId).FirstOrDefault();
            tempAttachement.Type_En = type?.Type_En;
            tempAttachement.Type_Ar = type?.Type_Ar;
            tempAttachement.StoragePath = pathResponse?.StoragePath;
            TempFiles.Add(tempAttachement);
            await ViewAttachement();
            //if (!Multiple)
            //{
            //	if (TempFiles.Count() > 0)
            //		Enabled = false;
            //}
        }

        protected async Task OnErrorHandler(Telerik.Blazor.Components.UploadErrorEventArgs e)
        {
            var actionText = e.Operation == UploadOperationType.Upload ? "uploaded" : "removed";
            notificationService.Notify(new NotificationMessage()
            {
                Severity = NotificationSeverity.Error,
                Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                Summary = translationState.Translate("Error"),
                Style = "position: fixed !important; left: 0; margin: auto; "
            });
        }
        protected void RemoveFailedFilesFromList(List<UploadFileInfo> files)
        {
            foreach (var file in files)
            {
                if (FilesValidationInfo.Keys.Contains(file.Id))
                {
                    FilesValidationInfo.Remove(file.Id);
                }
            }
        }

        protected bool IsSelectedFileValid(UploadFileInfo file)
        {
            return !(file.InvalidExtension || file.InvalidMaxFileSize || file.InvalidMinFileSize);
        }

        #endregion

        #region Document Previewer

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> View Attachment either Pdf or Image (read in stream and convert to PDF document) in Pdf Viewer</History>
        protected async Task ViewAttachement()
        {
            try
            {

                var physicalPath = string.Empty;
                if (TempFiles.Any())
                {
#if DEBUG
                    {
                        physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + TempFiles.FirstOrDefault().StoragePath).Replace(@"\\", @"\");

                    }
#else
                    {
                        physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + TempFiles.FirstOrDefault().StoragePath).Replace(@"\\", @"\");
                    	physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
                    }
#endif
                }
                if (!String.IsNullOrEmpty(physicalPath))
                {
                    FileData = await DocumentEncryptionService.GetDecryptedDocumentBytes(physicalPath, TempFiles.FirstOrDefault().DocType, _config.GetValue<string>("DocumentEncryptionKey"));
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

        #region Form Events

        //<History Author = 'Hassan Abbas' Date='2023-06-07' Version="1.0" Branch="master">Save Document As Draft</History>
        protected async Task SaveAsDraft()
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    ApiCallResponse response = new ApiCallResponse();
                    if (AddedDocument.AttachmentTypeId <= 0)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("Must_Attachment_Type"),
                            Summary = translationState.Translate("Error"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        return;
                    }
                    if (AddedDocument.ClassificationId > (int)DocumentClassificationEnum.External)
                    {
                        string pattern = @"<([^>]+)>|&nbsp;|\s+";
                        string cleanString = AddedDocument.DocumentVersion.Content == null ? "" : AddedDocument.DocumentVersion.Content;
                        cleanString = Regex.Replace(cleanString, pattern, string.Empty);

                        if (String.IsNullOrWhiteSpace(cleanString))
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Fill_Document_Content"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            return;
                        }
                    }
                    else
                    {
                        if (!TempFiles.Any())
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Please_Select_File"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            return;
                        }
                    }
                    spinnerService.Show();
                    #region new implementation
                    if (DocumentId == null)
                    {
                        AddedDocument.DocumentVersion.StatusId = (int)DocumentStatusEnum.Draft;
                        response = await fileUploadService.SaveAddedDocument(AddedDocument);
                    }
                    else
                    {
                        if (AddedDocument.DocumentVersion.CreatedBy == loginState.Username)
                        {
                            if (AddedDocument.DocumentVersion.StatusId == (int)DocumentStatusEnum.Rejected)
                            {
                                AddedDocument.DocumentVersion.VersionNo = AddedDocument.DocumentVersion.VersionNo + 0.01M;
                                AddedDocument.DocumentVersion.StatusId = (int)DocumentStatusEnum.Draft;
                                response = await fileUploadService.SaveAddedDocument(AddedDocument);
                            }
                            else
                            {
                                AddedDocument.DocumentVersion.CreatedBy = loginState.Username;
                                AddedDocument.DocumentVersion.CreatedDate = DateTime.Now;
                                response = await fileUploadService.UpdateDMSDocument(AddedDocument);
                            }
                        }
                        else
                        {
                            AddedDocument.DocumentVersion.AddedDocumentId = AddedDocument.Id;
                            AddedDocument.DocumentVersion.PreviousVersionId = AddedDocument.DocumentVersion.Id;
                            AddedDocument.DocumentVersion.Id = Guid.NewGuid();
                            AddedDocument.DocumentVersion.VersionNo = AddedDocument.DocumentVersion.VersionNo + 0.01M;
                            AddedDocument.DocumentVersion.StatusId = (int)DocumentStatusEnum.Draft;
                            AddedDocument.DocumentVersion.FileName = AddedDocument.DocumentVersion.FileName;
                            AddedDocument.DocumentVersion.StoragePath = AddedDocument.DocumentVersion.StoragePath;
                            AddedDocument.DocumentVersion.DocType = AddedDocument.DocumentVersion.DocType;
                            AddedDocument.DocumentVersion.Content = AddedDocument.DocumentVersion.Content;
                            //AddedDocument.DocumentVersion.ReviewerUserId = AddedDocument.DocumentVersion.ReviewerUserId;
                            //AddedDocument.DocumentVersion.ReviewerRoleId = AddedDocument.DocumentVersion.ReviewerRoleId;
                            AddedDocument.DocumentVersion.CreatedBy = loginState.Username;
                            AddedDocument.DocumentVersion.CreatedDate = DateTime.Now;
                            AddedDocument.DocumentVersion.ModifiedBy = loginState.Username;
                            AddedDocument.DocumentVersion.ModifiedDate = DateTime.Now;
                            response = await fileUploadService.CreateDMSDocumentVersion(AddedDocument);
                        }
                    }
                    #endregion
                    #region old implementation
                    //if (AddedDocument.DocumentVersion.Id != Guid.Empty && loginState.Username != AddedDocument.DocumentVersion.CreatedBy)
                    //{
                    //    if (AddedDocument.DocumentVersion == null)
                    //    {
                    //        AddedDocument.DocumentVersion = new DmsAddedDocumentVersion();
                    //    }
                    //    AddedDocument.DocumentVersion.AddedDocumentId = AddedDocument.Id;
                    //    AddedDocument.DocumentVersion.VersionNo = AddedDocument.DocumentVersion.VersionNo + 0.01M;
                    //    AddedDocument.DocumentVersion.StatusId = (int)DocumentStatusEnum.Draft;
                    //    AddedDocument.DocumentVersion.FileName = AddedDocument.DocumentVersion.FileName;
                    //    AddedDocument.DocumentVersion.StoragePath = AddedDocument.DocumentVersion.StoragePath;
                    //    AddedDocument.DocumentVersion.DocType = AddedDocument.DocumentVersion.DocType;
                    //    AddedDocument.DocumentVersion.Content = AddedDocument.DocumentVersion.Content;
                    //    AddedDocument.DocumentVersion.ReviewerUserId = AddedDocument.DocumentVersion.ReviewerUserId;
                    //    AddedDocument.DocumentVersion.ReviewerRoleId = AddedDocument.DocumentVersion.ReviewerRoleId;
                    //    AddedDocument.DocumentVersion.CreatedBy = loginState.Username;
                    //    AddedDocument.DocumentVersion.CreatedDate = DateTime.Now;
                    //}
                    //else
                    //{
                    //    if (AddedDocument.DocumentVersion.StatusId == (int)DocumentStatusEnum.Rejected)
                    //    {
                    //        if (AddedDocument.DocumentVersion == null)
                    //        {
                    //            AddedDocument.DocumentVersion = new DmsAddedDocumentVersion();
                    //        }
                    //        AddedDocument.DocumentVersion.AddedDocumentId = AddedDocument.Id;
                    //        AddedDocument.DocumentVersion.VersionNo = AddedDocument.DocumentVersion.VersionNo + 0.01M;
                    //        AddedDocument.DocumentVersion.StatusId = (int)DocumentStatusEnum.Draft;
                    //        AddedDocument.DocumentVersion.FileName = AddedDocument.DocumentVersion.FileName;
                    //        AddedDocument.DocumentVersion.StoragePath = AddedDocument.DocumentVersion.StoragePath;
                    //        AddedDocument.DocumentVersion.DocType = AddedDocument.DocumentVersion.DocType;
                    //        AddedDocument.DocumentVersion.Content = AddedDocument.DocumentVersion.Content;
                    //        AddedDocument.DocumentVersion.ReviewerUserId = AddedDocument.DocumentVersion.ReviewerUserId;
                    //        AddedDocument.DocumentVersion.ReviewerRoleId = AddedDocument.DocumentVersion.ReviewerRoleId;
                    //        AddedDocument.DocumentVersion.CreatedBy = loginState.Username;
                    //        AddedDocument.DocumentVersion.CreatedDate = DateTime.Now;
                    //    }
                    //    else
                    //    {
                    //        AddedDocument.DocumentVersion.StatusId = (int)DocumentStatusEnum.Draft;
                    //    }
                    //}
                    #endregion
                    if (response.IsSuccessStatusCode)
                    {
                        AddedDocument = (DmsAddedDocument)response.ResultData;
                        if (AddedDocument.ClassificationId == (int)DocumentClassificationEnum.External)
                        {
                            await fileUploadService.MoveAttachmentToAddedDocumentVersion(new MoveAttachmentAddedDocumentVM { AddedDocumentVersionId = AddedDocument.DocumentVersion.Id, ReferenceId = AttachmentReferenceId });
                        }
                        spinnerService.Hide();
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Changes_Saved_Successfully"),
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
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        //<History Author = 'Hassan Abbas' Date='2023-06-07' Version="1.0" Branch="master">Submit Document Form</History>
        protected async Task Form0Submit(DmsAddedDocument args)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    AddedDocument.SubModuleId = await GetSubmoduleId();
                    ApiCallResponse response = new ApiCallResponse();
                    var responses = await workflowService.GetActiveWorkflows(AddedDocument.ModuleId, (int)WorkflowModuleTriggerEnum.UserSubmitsDMSDocument, AddedDocument.AttachmentTypeId, AddedDocument.SubModuleId);
                    if (responses.IsSuccessStatusCode)
                    {
                        var activeworkflowlist = (List<WorkflowVM>)responses.ResultData;
                        if (activeworkflowlist != null && activeworkflowlist?.Count() > 0)
                        {
                            AddedDocument.SectorTypeId = (int)loginState.UserDetail.SectorTypeId;
                            AddedDocument.UserLoginState = loginState.Username;
                            if (AddedDocument.ClassificationId > (int)DocumentClassificationEnum.External)
                            {
                                string pattern = @"<([^>]+)>|&nbsp;|\s+";
                                string cleanString = AddedDocument.DocumentVersion.Content == null ? "" : AddedDocument.DocumentVersion.Content;
                                cleanString = Regex.Replace(cleanString, pattern, string.Empty);

                                if (String.IsNullOrWhiteSpace(cleanString))
                                {
                                    notificationService.Notify(new NotificationMessage()
                                    {
                                        Severity = NotificationSeverity.Error,
                                        Detail = translationState.Translate("Fill_Document_Content"),
                                        Style = "position: fixed !important; left: 0; margin: auto; "
                                    });
                                    return;
                                }
                            }
                            else
                            {
                                if (!TempFiles.Any())
                                {
                                    notificationService.Notify(new NotificationMessage()
                                    {
                                        Severity = NotificationSeverity.Error,
                                        Detail = translationState.Translate("Please_Select_File"),
                                        Style = "position: fixed !important; left: 0; margin: auto; "
                                    });
                                    return;
                                }
                            }
                            #region new implementation
                            if (DocumentId == null)
                            {
                                AddedDocument.DocumentVersion.StatusId = (int)DocumentStatusEnum.InReview;
                                response = await fileUploadService.SaveAddedDocument(AddedDocument);
                            }
                            else
                            {
                                if (AddedDocument.DocumentVersion.StatusId == (int)DocumentStatusEnum.Draft)
                                {
                                    if (AddedDocument.CreatedBy == loginState.Username)
                                    {
                                        AddedDocument.IsSubmit = true;
                                        AddedDocument.DocumentVersion.StatusId = (int)DocumentStatusEnum.InReview;
                                    }
                                    else
                                    {
                                        if (AddedDocument.CreatedBy != loginState.Username)
                                        {
                                            AddedDocument.DocumentVersion.StatusId = (int)DocumentStatusEnum.Approved;
                                        }
                                    }
                                    response = await fileUploadService.UpdateDMSDocument(AddedDocument);
                                }
                                else
                                {
                                    if (AddedDocument.CreatedBy == loginState.Username)
                                    {
                                        AddedDocument.DocumentVersion.StatusId = (int)DocumentStatusEnum.InReview;
                                    }
                                    else
                                    {
                                        if (AddedDocument.CreatedBy != loginState.Username)
                                        {
                                            AddedDocument.DocumentVersion.StatusId = (int)DocumentStatusEnum.Approved;
                                        }
                                    }
                                    AddedDocument.DocumentVersion.AddedDocumentId = AddedDocument.Id;
                                    AddedDocument.DocumentVersion.PreviousVersionId = AddedDocument.DocumentVersion.Id;
                                    AddedDocument.DocumentVersion.Id = Guid.NewGuid();
                                    AddedDocument.DocumentVersion.VersionNo = AddedDocument.DocumentVersion.VersionNo + 0.01M;
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
                                    AddedDocument.DocumentVersion.IsPreviousVersionApproved = false;
                                    response = await fileUploadService.CreateDMSDocumentVersion(AddedDocument);
                                }
                            }
                            #endregion
                            spinnerService.Show();
                            if (response.IsSuccessStatusCode)
                            {
                                AddedDocument = (DmsAddedDocument)response.ResultData;
                                if (DocumentId == null || AddedDocument.IsSubmit)//AddedDocument.DocumentVersion.Id != Guid.Empty && AddedDocument.UserLoginState != AddedDocument.CreatedBy
                                {
                                    await workflowService.AssignWorkflowActivity(activeworkflowlist.FirstOrDefault(), AddedDocument, (int)WorkflowModuleEnum.DMS, (int)WorkflowModuleTriggerEnum.UserSubmitsDMSDocument, null);
                                }
                                else
                                {
                                    await workflowService.ProcessWorkflowActvivities(AddedDocument, (int)WorkflowModuleEnum.DMS, (int)WorkflowModuleTriggerEnum.UserSubmitsDMSDocument);
                                }
                                await Task.Delay(1500);
                                if (AddedDocument.ClassificationId == (int)DocumentClassificationEnum.External)
                                {
                                    await fileUploadService.MoveAttachmentToAddedDocumentVersion(new MoveAttachmentAddedDocumentVM { AddedDocumentVersionId = AddedDocument.DocumentVersion.Id, ReferenceId = AttachmentReferenceId });
                                }
                                if (!String.IsNullOrEmpty(AddedDocument.DocumentVersion.ReviewerUserId))
                                {
                                    await ProcessTaskAndNotificationItems(AddedDocument);
                                }
                                if (taskDetailVM.TaskId != Guid.Empty)
                                {
                                    taskDetailVM.TaskStatusId = (int)TaskStatusEnum.Done;
                                    var taskResponse = await taskService.DecisionTask(taskDetailVM);
                                    if (!taskResponse.IsSuccessStatusCode)
                                    {
                                        await invalidRequestHandlerService.ReturnBadRequestNotification(taskResponse);
                                    }
                                }
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Success,
                                    Detail = translationState.Translate("Changes_Saved_Successfully"),
                                    Style = "position: fixed !important; left: 0; margin: auto; "
                                });
                                spinnerService.Hide();
                                await RedirectBack();
                            }
                            else
                            {
                                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                            }
                            spinnerService.Hide();
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

                }
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
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
        protected async Task FormInvalidSubmit()
        {
            try
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Fill_Required_Fields"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                return;
            }
            catch (Exception)
            {
                throw;
            }
        }

        protected async Task ProcessTaskAndNotificationItems(DmsAddedDocument document)
        {
            try
            {
                if (document.DocumentVersion.StatusId > (int)DocumentStatusEnum.Draft)
                {
                    if (document.DocumentVersion.StatusId == (int)DocumentStatusEnum.InReview)
                    {
                        var taskId = Guid.NewGuid();
                        ApiCallResponse taskResponse = await taskService.AddSystemGeneratedTask(new SaveTaskVM
                        {
                            Task = new UserTask
                            {
                                TaskId = taskId,
                                Name = "Document_Submitted_For_Review",
                                Date = DateTime.Now.Date,
                                AssignedBy = loginState.UserDetail.UserId,
                                AssignedTo = document.DocumentVersion.ReviewerUserId,
                                IsSystemGenerated = true,
                                TaskStatusId = (int)TaskStatusEnum.Pending,
                                ModuleId = (int)WorkflowModuleEnum.DMS,
                                SectorId = 0,
                                DepartmentId = (int)DepartmentEnum.Operational,
                                TypeId = (int)TaskTypeEnum.Assignment,
                                RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
                                CreatedBy = document.CreatedBy,
                                CreatedDate = DateTime.Now,
                                IsDeleted = false,
                                ReferenceId = document.DocumentVersion.Id,
                                SubModuleId = 0,
                                Url = "",
                                Description = ""
                            },
                            TaskActions = new List<TaskAction>()
                                {
                                    new TaskAction()
                                    {
                                        ActionName = "Document Assigned For Review",
                                        TaskId = taskId,
                                        CreatedBy = loginState.Username,
                                        CreatedDate = DateTime.Now,
                                    }
                                }
                        },
                        "view",
                        "document",
                        document.DocumentVersion.Id.ToString());
                    }

                    string NotificationKey = "";
                    if (document.DocumentVersion.StatusId == (int)DocumentStatusEnum.InReview)
                    {
                        NotificationKey = "Document_Submitted_For_Review";
                    }
                    else if (document.DocumentVersion.StatusId == (int)DocumentStatusEnum.Rejected)
                    {
                        NotificationKey = "Document_Rejected_After_Review";
                    }
                    else if (document.DocumentVersion.StatusId == (int)DocumentStatusEnum.Approved)
                    {
                        NotificationKey = "Document_Approved_After_Review";
                    }
                    //var notificationResult = await _iNotifications.SendNotification(new Notification
                    //{
                    //    NotificationId = Guid.NewGuid(),
                    //    DueDate = DateTime.Now.AddDays(5),
                    //    CreatedBy = document.DocumentVersion.CreatedBy,
                    //    CreatedDate = DateTime.Now,
                    //    IsDeleted = false,
                    //    ReceiverId = document.DocumentVersion.StatusId == (int)DocumentStatusEnum.InReview ? document.DocumentVersion.ReviewerUserId : document.CreatedBy,
                    //    ReceiverTypeId = (int)NotificationReceiverTypeEnum.User,
                    //    DocumentModuleId = (int)WorkflowModuleEnum.CaseManagement,
                    //    NotificationTypeId = (int)NotificationTypeEnum.Asynchronous,
                    //    NotificationCommunicationMethodId = (int)NotificationChannelEnum.Browser,
                    //    NotificationLinkId = Guid.NewGuid(),
                    //    NotificationEventId = (int)NotificationEventEnum.SubmitRequest,
                    //    NotificationCategoryId = (int)NotificationCategoryEnum.Important
                    //},
                    //NotificationKey,
                    //"view",
                    //"document",
                    //document.DocumentVersion.Id.ToString());


                    //if (document.DocumentVersion.StatusId == (int)DocumentStatusEnum.InReview)
                    //{
                    //    _auditLogs.CreateProcessLog(new ProcessLog
                    //    {
                    //        Process = "Submit Document For Review",
                    //        Task = "To submit the document for review",
                    //        Description = "Document Submitted for review",
                    //        ProcessLogEventId = (int)ProcessLogEnum.Processed,
                    //        Message = "Document Submitted for review",
                    //        IPDetails = Request.HttpContext.Connection.RemoteIpAddress.ToString(),
                    //        ApplicationID = (int)PortalEnum.FatwaPortal,
                    //        ModuleID = (int)WorkflowModuleEnum.LMSLiterature,
                    //        Token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "")
                    //    });
                    //}
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
        #endregion
    }
}
