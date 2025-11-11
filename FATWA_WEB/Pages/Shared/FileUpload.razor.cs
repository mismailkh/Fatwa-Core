using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.DigitalSignature;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_GENERAL.Helper;
using FATWA_WEB.Extensions;
using FATWA_WEB.Pages.DS;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Radzen;
using Radzen.Blazor;
using Syncfusion.Blazor.PdfViewer;
using Syncfusion.Blazor.PdfViewerServer;
using System.Collections.ObjectModel;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Telerik.Blazor;
using Telerik.Blazor.Components;
using Telerik.Blazor.Components.Upload;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.Shared
{
    public partial class FileUpload : ComponentBase
    {
        #region Parameters
        [Parameter]
        public Guid? ReferenceGuid { get; set; }
        [Parameter]
        public Guid? CommunicationId { get; set; } = Guid.Empty;
        [Parameter]
        public bool? IsLiterature { get; set; } = false;
        [Parameter]
        public int? LiteratureId { get; set; }

        [Parameter]
        public bool? IsViewOnly { get; set; } = false;

        [Parameter]
        public bool? LoadAllAttachmentTypes { get; set; } = false;

        [Parameter]
        public List<int> DeletedAttachementIds { get; set; } = new List<int>();

        [Parameter]
        public List<string> FileTypes { get; set; }

        [Parameter]
        public int? MaxFileSize { get; set; }

        [Parameter]
        public bool Multiple { get; set; }

        [Parameter]
        public string? UploadFrom { get; set; }

        [Parameter]
        public int? ModuleId { get; set; }

        [Parameter]
        public int? SubModuleId { get; set; }

        [Parameter]
        public EventCallback<List<int>> DeletedAttachementIdsChanged { get; set; }

        [Parameter]
        public bool? IsCaseRequest { get; set; } = false;

        [Parameter]
        public int? MandatoryAttachmentTypeId { get; set; }
        [Parameter]
        public bool? IsUploadPopup { get; set; } = false;
        [Parameter]
        public bool? ViewOfficialLettersOnly { get; set; } = false;
        [Parameter]
        public bool? AutoSave { get; set; } = false;
        [Parameter]
        public bool? AutoDelete { get; set; } = false;
        [Parameter]
        public bool? IgnoreAttachmentTypeRequiredValidator { get; set; } = false;
        [Parameter]
        public bool? ShowDocsBasedOnType { get; set; } = false;
        [Parameter]
        public bool? ShowFileNameField { get; set; } = false;
        [Parameter]
        public string? CANNumber { get; set; }
        [Parameter]
        public bool? HideView { get; set; } = false;
        [Parameter]
        public int MeetingStatus { get; set; }
        [Parameter]
        public bool IsLLSReferenceDocuments { get; set; } = false;
        [Parameter]
        public bool IsOrganizingCommittee { get; set; } = false;
        [Parameter]
        public bool IsLLSOtherSourceDocuments { get; set; } = false;
        [Parameter]
        public bool? ShowCorrespondenceButton { get; set; } = false;
        [Parameter]
        public bool IsRequestTypeSelection { get; set; } = false;
        [Parameter]
        public int RequestTypeId { get; set; }
        [Parameter]
        public EventCallback<bool> OnAuthorityLetterChanged { get; set; }
        [Parameter]
        public int? SubTypeId { get; set; }
        [Parameter]
        public bool? IsPartyAttachment { get; set; } = false;
        [Parameter]
        public EventCallback<bool> OnLegalPrincipleOtherDocumentUpload { get; set; }
        #endregion

        #region Variables 
        public int AttachmentTypeId { get; set; }
        protected bool ShowOtherAttachmentType { get; set; }
        protected List<AttachmentType> AttachmentTypes { get; set; } = new List<AttachmentType>();
        protected List<AttachmentType> G2GAttachmentTypes { get; set; } = new List<AttachmentType>();
        public bool Enabled { get; set; } = true;
        private string SelectFileButton { get; set; } = string.Empty;
        public string SaveUrl => _config.GetValue<string>("dms_api_url") + "/FileUpload/Upload";
        public string RemoveUrl => _config.GetValue<string>("dms_api_url") + "/FileUpload/Remove";
        public ObservableCollection<TempAttachementVM> AdditionalTempFiles { get; set; } = new ObservableCollection<TempAttachementVM>();
        public ObservableCollection<TempAttachementVM> MandatoryTempFiles { get; set; } = new ObservableCollection<TempAttachementVM>();
        public ObservableCollection<TempAttachementVM> OfficialTempFiles { get; set; } = new ObservableCollection<TempAttachementVM>();
        public ObservableCollection<TempAttachementVM> TempFiles { get; set; } = new ObservableCollection<TempAttachementVM>();
        public ObservableCollection<TempAttachementVM> TempFiles2 { get; set; } = new ObservableCollection<TempAttachementVM>();
        public List<TempAttachementVM> ChildTempFiles { get; set; } = new List<TempAttachementVM>();

        public ObservableCollection<TempAttachementVM> ParentTempFiles { get; set; } = new ObservableCollection<TempAttachementVM>();
        Dictionary<string, bool> FilesValidationInfo { get; set; } = new Dictionary<string, bool>();
        protected TempAttachementVM TempAttachement { get; set; } = new TempAttachementVM();

        protected RadzenDataGrid<TempAttachementVM>? mandatoryAttachGrid;
        protected RadzenDataGrid<TempAttachementVM>? additionalAttachGrid;
        protected RadzenDataGrid<TempAttachementVM>? tempAttachmentGrid;
        protected SfPdfViewerServer? PdfViewerRef;
        public byte[] FileData { get; set; }
        public string DocumentPath { get; set; }
        public int? PreviewedDocumentId { get; set; }
        public int? PreviewedAttachementId { get; set; }
        List<ToolbarItem> ToolbarItems = new List<ToolbarItem>();
        public bool DisplayDocumentViewer { get; set; }
        protected bool ShowReferenceFields { get; set; }
        public IList<TempAttachementVM> selectedFiles;
        public bool allowRowSelectOnRowClick = true;
        protected RadzenDataGrid<TempAttachementVM> grid;
        public bool EnableMandatoryDocumentDeleteButton { get; set; } = false;
        public CommunicationListVM communicationDetail;
        protected string RedirectURL { get; set; }
        public bool IsSplitButtonsVisible = true;
        private bool IsSignaturePanelVisible = false;
        private bool RefreshDivContainer = true;
        private SignatureVerificationResponse signatureVerification = new SignatureVerificationResponse();
        public ApiCallResponse VerificationResponse = new ApiCallResponse();
        private bool IsDocumentSigned { get; set; }
        protected List<RequestType> RequestTypes { get; set; } = new List<RequestType>();

        public DateTime Minimum = new DateTime(1950, 1, 1);
        public int GovtEntityId { get; set; }
        protected List<GovernmentEntity> GovtEntities { get; set; } = new List<GovernmentEntity>();

        #endregion

        #region Component Load
        //<History Author = 'Hassan Abbas' Date='2022-05-24' Version="1.0" Branch="master"> Populate Grid Temp and Uplaoded Attachments</History>
        //<History Author = 'Hassan Abbas' Date='2022-9-24' Version="2.0" Branch="master"> Populate Attachment Types dropdown based on Module</History>
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await JsInterop.InitilizeSignaturePanel(DotNetObjectReference.Create(this));
            await Load();
            spinnerService.Hide();
        }

        protected async Task Load()
        {
            if (IsRequestTypeSelection)
            {
                await PopulateRequestTypes();
                await PopulateGovtEntities();
                await OnRequestTypeChange();
                ReferenceGuid = Guid.NewGuid();
            }
            else if ((bool)IsUploadPopup)
            {
                await PopulateAttachmentTypes();
                if (MandatoryAttachmentTypeId != null)
                {
                    TempAttachement.AttachmentTypeId = MandatoryAttachmentTypeId;
                    await OnTypeChange(MandatoryAttachmentTypeId);
                    if ((bool)ShowFileNameField && !String.IsNullOrEmpty(CANNumber))
                    {
                        string attachmentTypeName = System.Enum.GetName(typeof(AttachmentTypeEnum), MandatoryAttachmentTypeId);
                        TempAttachement.FileName = attachmentTypeName + "_" + CANNumber;
                    }
                }
            }
            else
            {
                await PopulateAttachmentTypes();
                //if (MeetingStatus == (int)MeetingStatusEnum.RequestedByGE || CommunicationId != Guid.Empty)
                //{
                //	ReferenceGuid = CommunicationId;

                // }

                //ReferenceGuid = (MeetingStatus == (int)MeetingStatusEnum.SaveAsDraft ||
                //                 MeetingStatus == (int)MeetingStatusEnum.ApprovedByHOS ||
                //                 MeetingStatus == (int)MeetingStatusEnum.Complete ||
                //                 MeetingStatus == (int)MeetingStatusEnum.Scheduled||
                //                 MeetingStatus == (int)MeetingStatusEnum.SaveAsDraft) &&
                //                 CommunicationId != null ? CommunicationId : ReferenceGuid;

                await PopulateAttachmentGrid();

                if ((bool)IsCaseRequest || ModuleId == (int)WorkflowModuleEnum.Meeting || ModuleId == (int)WorkflowModuleEnum.OrganizingCommittee)
                    await PopulateMandatoryAttachmentsGrid();

                if (MandatoryAttachmentTypeId != null)
                {
                    TempAttachement.AttachmentTypeId = MandatoryAttachmentTypeId;
                    await OnTypeChange(MandatoryAttachmentTypeId);
                }
            }
        }

        #endregion

        #region Grid Data Population Events

        //<History Author = 'Hassan Abbas' Date='2022-05-24' Version="1.0" Branch="master"> Populating Attachment Grid based on Module and Concatinate Temporary and Uploaded Documents</History>

        private async Task PopulateAttachmentGrid()
        {
            if ((bool)IsLiterature)
            {
                if (LiteratureId > 0)
                {
                    TempFiles = await fileUploadService.GetUploadedAttachements(true, LiteratureId, Guid.NewGuid());
                    if (ReferenceGuid != null)
                    {
                        TempFiles2 = await fileUploadService.GetTempAttachements(ReferenceGuid);
                    }
                }
                else if (LiteratureId == null && ReferenceGuid != null)
                {
                    TempFiles = await fileUploadService.GetTempAttachements(ReferenceGuid);
                }
            }
            else if (ModuleId == (int)WorkflowModuleEnum.CNTContactManagement || ModuleId == (int)WorkflowModuleEnum.OrganizingCommittee)
            {
                if (ReferenceGuid != Guid.Empty)
                {
                    TempFiles = await fileUploadService.GetUploadedAttachements(false, 0, ReferenceGuid);
                    TempFiles2 = await fileUploadService.GetTempAttachements(ReferenceGuid);
                }
            }
            else if (IsLLSReferenceDocuments)
            {
                TempFiles = await fileUploadService.GetLLSLegalPrincipleReferenceUploadedAttachements((Guid)ReferenceGuid);
            }
            else if (IsLLSOtherSourceDocuments)
            {
                TempFiles = await fileUploadService.GetLLSLegalPrincipleReferenceUploadedAttachements((Guid)ReferenceGuid);
            }
            else if (ReferenceGuid != null)
            {
                TempFiles = await fileUploadService.GetUploadedAttachements(false, 0, ReferenceGuid);
                TempFiles2 = await fileUploadService.GetTempAttachements(ReferenceGuid);
            }



            if (TempFiles is not null)
            {
                // Concatincate Temp and Uploaded Docs
                if (TempFiles2 is not null) TempFiles = new ObservableCollection<TempAttachementVM>(TempFiles?.Concat(TempFiles2).ToList());
                Regex regex = new Regex("_[0-9]+", RegexOptions.RightToLeft);
                TempFiles = new ObservableCollection<TempAttachementVM>(TempFiles?.Select(f => { if (f.FileName.Contains("Document_Portfolio")) return f; else f.FileName = regex.Replace(f.FileName, "", 1); return f; }).ToList());
                if (ModuleId == (int)WorkflowModuleEnum.Meeting)
                {
                    MandatoryTempFiles = new ObservableCollection<TempAttachementVM>(TempFiles.Where(x => x.CommunicationGuid == CommunicationId).ToList());
                }
                else if (ModuleId == (int)WorkflowModuleEnum.COMSConsultationManagement)
                {
                    MandatoryTempFiles = new ObservableCollection<TempAttachementVM>(TempFiles.Where(t => AttachmentTypes.Select(m => m.AttachmentTypeId).ToList().Contains((int)t.AttachmentTypeId)).ToList());

                    //relevent to consultation Request
                    if (dataCommunicationService.consultationRequest != null)
                    {
                        dataCommunicationService.consultationRequest.MandatoryTempFiles = MandatoryTempFiles;
                    }
                }
                else if (ModuleId == (int)WorkflowModuleEnum.OrganizingCommittee)
                {
                    MandatoryTempFiles = new ObservableCollection<TempAttachementVM>(TempFiles.Where(x => x.ReferenceGuid == ReferenceGuid.ToString()).ToList());
                }
                else
                {
                    ParentTempFiles = new ObservableCollection<TempAttachementVM>(TempFiles.Where(x => x.PreCommunicationId == Guid.Empty).ToList());
                }
                //|| (x.AttachmentTypeId == (int)AttachmentTypeEnum.LegalNotification || x.ReferenceNo != null)).ToList());

                if (ParentTempFiles.Count == 0)
                {
                    ParentTempFiles = TempFiles;
                }
                if (ParentTempFiles.Count > 0)
                {
                    var tasks = ParentTempFiles.Select(async x =>
                    {
                        bool isAllowed = await GetAttachmentTypeById((int)x.AttachmentTypeId);
                        x.allowedDesignation = isAllowed;
                        if (x.UploadedDocumentId != null)
                        {
                            bool isAlreadySigned = await GetIsAlreadySigned(loginState.UserDetail.CivilId, (int)x.UploadedDocumentId);
                            x.isAlreadySigned = isAlreadySigned;
                        }
                    });
                    await Task.WhenAll(tasks);
                }
            }
            //<History Author = 'ijaz  Ahmad' Date='2022-05-24' Version="1.0" Branch="master"> Populating Attachment Grid based on Module and Concatinate Temporary and Uploaded Documents</History>
            // Show Attachment List of specific Attachment Type
            //if ((bool)ShowDocsBasedOnType)
            //	TempFiles = new ObservableCollection<TempAttachementVM>(TempFiles.Where(t => t.AttachmentTypeId == MandatoryAttachmentTypeId));


            // Disable Upload if single upload
            if (!Multiple)
            {
                if (TempFiles != null && TempFiles.Count() > 0)
                    Enabled = false;
            }

            //relevent to Case Request
            if ((bool)IsCaseRequest)
            {
                if (TempFiles != null && TempFiles.Any())
                {
                    MandatoryTempFiles = new ObservableCollection<TempAttachementVM>(TempFiles.Where(t => AttachmentTypes.Where(m => m.IsMandatory == true).Select(m => m.AttachmentTypeId).ToList().Contains((int)t.AttachmentTypeId)).ToList());
                    AdditionalTempFiles = new ObservableCollection<TempAttachementVM>(TempFiles.Where(t => AttachmentTypes.Where(m => m.IsMandatory == false).Select(m => m.AttachmentTypeId).ToList().Contains((int)t.AttachmentTypeId)).ToList());
                    OfficialTempFiles = new ObservableCollection<TempAttachementVM>(TempFiles.Where(t => AttachmentTypes.Where(m => m.IsOfficialLetter == true).Select(m => m.AttachmentTypeId).ToList().Contains((int)t.AttachmentTypeId)).ToList());
                }
                if (dataCommunicationService.caseRequest != null)
                {
                    dataCommunicationService.caseRequest.MandatoryTempFiles = MandatoryTempFiles;
                    dataCommunicationService.caseRequest.AdditionalTempFiles = AdditionalTempFiles;
                }
                if (dataCommunicationService.consultationRequest != null)
                {
                    dataCommunicationService.consultationRequest.MandatoryTempFiles = MandatoryTempFiles;
                    dataCommunicationService.consultationRequest.AdditionalTempFiles = AdditionalTempFiles;
                }
                //           else if (ModuleId == (int)WorkflowModuleEnum.Meeting)
                //           {
                //               //relevent to consultation Request
                //               if (dataCommunicationService.sendCommunicationVM != null)
                //{
                //                   dataCommunicationService.sendCommunicationVM.MandatoryTempFiles = MandatoryTempFiles;
                //               }
                //           }
            }
            //else if (ModuleId == (int)WorkflowModuleEnum.Meeting)
            //{
            //    MandatoryTempFiles = new ObservableCollection<TempAttachementVM>(TempFiles.Where(t => t.CommunicationGuid == CommunicationId).ToList());

            //    //relevent to consultation Request
            //    if (dataCommunicationService.sendCommunicationVM != null)
            //    {
            //        dataCommunicationService.sendCommunicationVM.MandatoryTempFiles = MandatoryTempFiles;
            //    }
            //}
        }

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Populate Mandatory Attachments Grid without File uploaded</History>
        protected async Task PopulateMandatoryAttachmentsGrid()
        {
            IEnumerable<AttachmentType> mandatorytypes = new List<AttachmentType>();
            if (SubTypeId != null)
            {
                mandatorytypes = AttachmentTypes.Where(t => t.IsMandatory == true && t.SubTypeId == SubTypeId && t.AttachmentTypeId != (int)AttachmentTypeEnum.AuthorityLetter);
            }
            else
            {
                mandatorytypes = AttachmentTypes.Where(t => t.IsMandatory == true && t.AttachmentTypeId != (int)AttachmentTypeEnum.AuthorityLetter);
            }
            if (mandatorytypes.Any())
            {
                if (ModuleId == (int)WorkflowModuleEnum.Meeting && MandatoryAttachmentTypeId == (int)AttachmentTypeEnum.RequestForMeeting)
                {
                    mandatorytypes = mandatorytypes.Where(type => type.AttachmentTypeId == (int)AttachmentTypeEnum.RequestForMeeting).ToList();
                }
                else if (ModuleId == (int)WorkflowModuleEnum.Meeting && MandatoryAttachmentTypeId == (int)AttachmentTypeEnum.ReplytoMeetingRequest)
                {
                    mandatorytypes = mandatorytypes.Where(type => type.AttachmentTypeId == (int)AttachmentTypeEnum.ReplytoMeetingRequest).ToList();
                }
                else if (ModuleId == (int)WorkflowModuleEnum.Meeting && MandatoryAttachmentTypeId == (int)AttachmentTypeEnum.MOMAttachment)
                {
                    mandatorytypes = mandatorytypes.Where(type => type.AttachmentTypeId == (int)AttachmentTypeEnum.MOMAttachment).ToList();
                }
                else if (ModuleId == (int)WorkflowModuleEnum.OrganizingCommittee)
                {
                    mandatorytypes = mandatorytypes.Where(type => type.AttachmentTypeId == (int)AttachmentTypeEnum.FatwaCircular).ToList();
                }
                foreach (var type in mandatorytypes)
                {
                    if (!MandatoryTempFiles.Where(e => e.AttachmentTypeId == type.AttachmentTypeId).Any())
                    {
                        MandatoryTempFiles.Add(new TempAttachementVM { AttachmentTypeId = type.AttachmentTypeId });
                    }
                }
                if (mandatoryAttachGrid != null)
                {
                    await mandatoryAttachGrid?.Reload();
                }
            }
        }

        #endregion

        #region File Uploader Handlers

        //<History Author = 'Hassan Abbas' Date='2022-05-24' Version="1.0" Branch="master"> On File select check if file already uploaded</History>
        protected void OnSelectHandler(UploadSelectEventArgs e)
        {
            foreach (var item in e.Files)
            {
                if (TempFiles != null && TempFiles.Where(x => x.FileName == item.Name).Count() > 0)
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
            if (!Multiple && FilesValidationInfo.Keys.Count > 0)
            {
                SelectFileButton = "disabled-upload-button";
                StateHasChanged();
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
            e.RequestData.Add("_uploadFrom", UploadFrom);
            e.RequestData.Add("_project", "FATWA_WEB");

            RemoveFailedFilesFromList(e.Files);
            if (!Multiple && FilesValidationInfo.Keys.Count == 0)
            {
                SelectFileButton = string.Empty;
            }
            //UpdateValidationModel();
        }

        //<History Author = 'Hassan Abbas' Date='2022-05-24' Version="1.0" Branch="master"> Remove uploaded document from Database if temporary else bind value to model and delete in repository</History>
        protected async Task RemoveTempAttachement(TempAttachementVM theUpdatedItem, bool isMandatory = false)
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Remove_File"), translationState.Translate("Remove_File"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                var result = false;
                if (theUpdatedItem.UploadedDocumentId > 0)
                {
                    if ((bool)AutoDelete)
                    {
                        //Remove the Uploaded attachment from db & Folder
                        var docResponse = await fileUploadService.RemoveDocument(theUpdatedItem.UploadedDocumentId.ToString(), false);
                        if (docResponse.IsSuccessStatusCode)
                        {
                            await PopulateAttachmentGrid();
                        }
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                        return;
                    }
                    else
                    {
                        DeletedAttachementIds.Add((int)theUpdatedItem.UploadedDocumentId);
                        await DeletedAttachementIdsChanged.InvokeAsync(DeletedAttachementIds);
                    }
                    result = true;
                }
                else
                {
                    result = await fileUploadService.RemoveTempAttachement(theUpdatedItem.FileName, theUpdatedItem.UploadedBy, UploadFrom, "FATWA_WEB", theUpdatedItem.AttachmentTypeId);
                }
                if (result)
                {
                    if ((bool)IsCaseRequest)
                    {
                        if (isMandatory)
                        {
                            MandatoryTempFiles.Remove(theUpdatedItem);
                            MandatoryTempFiles.Add(new TempAttachementVM { AttachmentTypeId = theUpdatedItem.AttachmentTypeId });
                            await mandatoryAttachGrid?.Reload();
                        }
                        else
                        {
                            AdditionalTempFiles.Remove(theUpdatedItem);
                            await additionalAttachGrid?.Reload();
                        }

                        TempFiles.Remove(theUpdatedItem);
                        //relevent to Case Request
                        if (dataCommunicationService.caseRequest != null)
                        {
                            dataCommunicationService.caseRequest.MandatoryTempFiles = MandatoryTempFiles;
                            dataCommunicationService.caseRequest.AdditionalTempFiles = AdditionalTempFiles;
                        }
                        if (dataCommunicationService.consultationRequest != null)
                        {
                            dataCommunicationService.consultationRequest.MandatoryTempFiles = MandatoryTempFiles;
                            dataCommunicationService.consultationRequest.AdditionalTempFiles = AdditionalTempFiles;
                        }
                        else if (ModuleId == (int)WorkflowModuleEnum.Meeting)
                        {
                            dataCommunicationService.saveMeetingVM.MandatoryTempFiles = MandatoryTempFiles;
                            dataCommunicationService.saveMeetingVM.AdditionalTempFiles = AdditionalTempFiles;
                        }
                        else if (ModuleId == (int)WorkflowModuleEnum.OrganizingCommittee)
                        {
                            dataCommunicationService.saveMeetingVM.MandatoryTempFiles = MandatoryTempFiles;
                            dataCommunicationService.saveMeetingVM.AdditionalTempFiles = AdditionalTempFiles;
                        }
                    }
                    else
                    {
                        MandatoryTempFiles.Remove(theUpdatedItem);
                        ParentTempFiles.Remove(theUpdatedItem);
                        AdditionalTempFiles.Remove(theUpdatedItem);
                        TempFiles.Remove(theUpdatedItem);
                        if (!Multiple)
                        {
                            if (TempFiles.Count() > 0)
                                Enabled = false;

                            else
                                Enabled = true;
                        }
                        if (ModuleId == (int)WorkflowModuleEnum.Meeting && (theUpdatedItem.AttachmentTypeId == (int)AttachmentTypeEnum.RequestForMeeting || theUpdatedItem.AttachmentTypeId == (int)AttachmentTypeEnum.ReplytoMeetingRequest || theUpdatedItem.AttachmentTypeId == (int)AttachmentTypeEnum.MOMAttachment))
                        {
                            MandatoryTempFiles.Add(new TempAttachementVM { AttachmentTypeId = theUpdatedItem.AttachmentTypeId });
                            await mandatoryAttachGrid.Reload();
                        }
                        if (ModuleId == (int)WorkflowModuleEnum.OrganizingCommittee && theUpdatedItem.AttachmentTypeId == (int)AttachmentTypeEnum.FatwaCircular)
                        {
                            MandatoryTempFiles.Add(new TempAttachementVM { AttachmentTypeId = theUpdatedItem.AttachmentTypeId });
                            await mandatoryAttachGrid.Reload();
                        }
                    }
                    if (dataCommunicationService.consultationRequest != null)
                    {
                        dataCommunicationService.consultationRequest.MandatoryTempFiles = MandatoryTempFiles;
                        dataCommunicationService.consultationRequest.AdditionalTempFiles = AdditionalTempFiles;
                    }
                    if (tempAttachmentGrid != null) { tempAttachmentGrid.Reload(); }
                    if (mandatoryAttachGrid != null) { mandatoryAttachGrid.Reload(); }
                    if (additionalAttachGrid != null) { additionalAttachGrid.Reload(); }
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                        Summary = translationState.Translate("Error"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-05-24' Version="1.0" Branch="master"> Add necessary parameters like Uploading Source, username, Token etc etc</History>
        protected async Task OnUploadHandler(UploadEventArgs e)
        {
            if (TempAttachement.AttachmentTypeId > 0)
            {
                e.RequestHeaders.Add("Authorization", new AuthenticationHeaderValue("Bearer", loginState.Token));
                e.RequestData.Add("_pEntityIdentifierGuid", ReferenceGuid);
                e.RequestData.Add("_pCommunicationGuid", CommunicationId);
                e.RequestData.Add("_userName", loginState.Username);
                e.RequestData.Add("_typeId", TempAttachement.AttachmentTypeId);
                e.RequestData.Add("_uploadFrom", UploadFrom);
                e.RequestData.Add("_project", "FATWA_WEB");
                if (TempAttachement.OtherAttachmentType != null)
                    e.RequestData.Add("_otherAttachmentType", TempAttachement.OtherAttachmentType);
                if (TempAttachement.Description != null)
                    e.RequestData.Add("_description", TempAttachement.Description);
                if (TempAttachement.ReferenceNo != null)
                    e.RequestData.Add("_referenceNo", TempAttachement.ReferenceNo);
                if (TempAttachement.ReferenceDate != null)
                    e.RequestData.Add("_referenceDate", TempAttachement.ReferenceDate);
                if (TempAttachement.DocumentDate != null)
                    e.RequestData.Add("_documentDate", TempAttachement.DocumentDate);
                if (TempAttachement.FileName != null)
                    e.RequestData.Add("_FileTitle", TempAttachement.FileName);
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
            if (!(bool)IsUploadPopup)
            {
                var actionText = e.Operation == UploadOperationType.Upload ? "uploaded" : "removed";
                TempAttachementVM tempAttachement = new TempAttachementVM();
                tempAttachement.DocType = e.Files[0].Extension;
                tempAttachement.FileName = String.IsNullOrEmpty(TempAttachement.FileName) ? e.Files[0].Name : TempAttachement.FileName + e.Files[0].Extension;
                tempAttachement.UploadedBy = loginState.Username;
                tempAttachement.DocDateTime = DateTime.Now;
                tempAttachement.DocumentDate = DateTime.Now;
                var type = AttachmentTypes?.Where(t => t.AttachmentTypeId == TempAttachement.AttachmentTypeId).FirstOrDefault();
                tempAttachement.AttachmentTypeId = type?.AttachmentTypeId;
                tempAttachement.Type_En = type?.Type_En;
                tempAttachement.Type_Ar = type?.Type_Ar;
                tempAttachement.StoragePath = pathResponse?.StoragePath;
                tempAttachement.AttachementId = pathResponse?.AttachementId;
                if (TempFiles == null)
                {
                    TempFiles = new ObservableCollection<TempAttachementVM>();
                }
                TempFiles.Add(tempAttachement);
                if (tempAttachmentGrid != null)
                {
                    await tempAttachmentGrid?.Reload();
                }
                if (!Multiple)
                {
                    if (TempFiles.Count() > 0)
                        Enabled = false;
                }
            }
            else
            {
                TempAttachement.DocType = e.Files[0].Extension;
                TempAttachement.FileName = String.IsNullOrEmpty(TempAttachement.FileName) ? e.Files[0].Name : TempAttachement.FileName + e.Files[0].Extension;
                TempAttachement.UploadedBy = loginState.Username;
                TempAttachement.DocDateTime = DateTime.Now;
                TempAttachement.DocumentDate = DateTime.Now;
                TempAttachement.StoragePath = pathResponse?.StoragePath;
                TempAttachement.AttachementId = pathResponse?.AttachementId;
                if (!Multiple)
                {
                    if (TempFiles.Count() > 0)
                        Enabled = false;
                }
                if ((bool)AutoSave)
                {
                    var response = await fileUploadService.UploadTempAttachmentToUploadedDocument((Guid)ReferenceGuid);
                    if (response.IsSuccessStatusCode)
                    {
                        dialogService.Close(TempAttachement);
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                }
                else
                {
                    dialogService.Close(TempAttachement);
                }
                if (IsRequestTypeSelection)
                {
                    RedirectToRequestPage();
                }
                spinnerService.Hide();
            }
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
            spinnerService.Hide();
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

        #region View and Download Attachments

        //<History Author = 'Hassan Abbas' Date='2022-05-24' Version="1.0" Branch="master"> Download Attachment using stream</History>
        //<History Author = 'Hassan Abbas' Date='2023-07-05' Version="1.0" Branch="master"> Modified the function to Decrypt the attachment and then download</History>
        protected async Task DownloadAttachement(TempAttachementVM theUpdatedItem)
        {
            try
            {
                //Encryption/Descyption Key
                string password = _config.GetValue<string>("DocumentEncryptionKey");
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);
                RijndaelManaged RMCrypto = new RijndaelManaged();
                MemoryStream fsOut = new MemoryStream();
                int data;
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
#if DEBUG
                {
                    //var physicalPath = Path.Combine(Directory.GetCurrentDirectory() + theUpdatedItem.StoragePath).Replace(@"\\", @"\");
                    var storagePath = string.IsNullOrEmpty(theUpdatedItem.SignedDocStoragePath) ? theUpdatedItem.StoragePath : theUpdatedItem.SignedDocStoragePath;
                    var physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + storagePath).Replace(@"\\", @"\");
                    if (!String.IsNullOrEmpty(physicalPath))
                    {
                        FileStream fsCrypt = new FileStream(physicalPath, FileMode.Open);
                        CryptoStream cs = new CryptoStream(fsCrypt,
                            RMCrypto.CreateDecryptor(key, key),
                            CryptoStreamMode.Read);

                        while ((data = cs.ReadByte()) != -1)
                            fsOut.WriteByte((byte)data);

                        await blazorDownloadFileService.DownloadFile(theUpdatedItem.FileName, fsOut, "application/octet-stream");
                        fsOut.Close();
                        cs.Close();
                        fsCrypt.Close();
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
#else
				{
					var httpClient = new HttpClient();
                    var storagePath = string.IsNullOrEmpty(theUpdatedItem.SignedDocStoragePath) ? theUpdatedItem.StoragePath : theUpdatedItem.SignedDocStoragePath;
                    var RleasephysicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + storagePath).Replace(@"\\", @"\");
					RleasephysicalPath = RleasephysicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
					if (!String.IsNullOrEmpty(RleasephysicalPath))
					{
						var response = await httpClient.GetAsync(RleasephysicalPath);
						if (response.IsSuccessStatusCode)
						{
							var fsCrypt = await response.Content.ReadAsStreamAsync();
							CryptoStream cs = new CryptoStream(fsCrypt,
								RMCrypto.CreateDecryptor(key, key),
								CryptoStreamMode.Read);

							while ((data = cs.ReadByte()) != -1)
								fsOut.WriteByte((byte)data);

							await blazorDownloadFileService.DownloadFile(theUpdatedItem.FileName, fsOut, "application/octet-stream");
							fsOut.Close();
							cs.Close();
							fsCrypt.Close();
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
				}
#endif
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

        /*<History Author = 'Hassan Abbas' Date='2024-03-14' Version="1.0" Branch="master"> Append Custom Action into the Pdf Viewer Toolbar through Html</History>*/
        protected async Task DocumentLoaded(LoadEventArgs args)
        {
            await JSRuntime.InvokeVoidAsync("addRotateButtonToPdfToolbar");
            if (IsDocumentSigned)
                await JSRuntime.InvokeVoidAsync("addSingaturePanelBtnToPdfToolbar");
        }
        [JSInvokable]
        public async Task OpenSignaturePanel()
        {
            var dialogResult = await dialogService.OpenAsync<SignatureVerificationDetailsPopUp>(
             translationState.Translate("Signature_Verification_Results"),
             new Dictionary<string, object>() {
                    { "DocumentId", PreviewedDocumentId },
             },
             new DialogOptions() { Width = "61% !important", CloseDialogOnOverlayClick = true });
            if (dialogResult != null)
            {

            }
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
            StateHasChanged();
        }
        //<History Author = 'Hassan Abbas' Date='2024-03-21' Version="1.0" Branch="master"> Open Document in New Window</History>
        protected async Task OpenInNewWindow(MouseEventArgs args)
        {
            string url = string.Empty;
            if ((bool)IsLiterature && ReferenceGuid == null && PreviewedDocumentId > 0)
            {
                url = $"/preview-literature-document/{LiteratureId}/{PreviewedDocumentId}";
            }
            else
            {
                url = PreviewedDocumentId > 0 ? $"/preview-document/{ReferenceGuid}/{PreviewedDocumentId}" : $"/preview-attachement/{ReferenceGuid}/{PreviewedAttachementId}";
            }
            await JsInterop.InvokeVoidAsync("openNewWindow", url);
        }


        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> View Attachment either Pdf or Image (read in stream and convert to PDF document) in Pdf Viewer</History>
        //<History Author = 'Hassan Abbas' Date='2023-06-20' Version="1.0" Branch="master"> Modified the function to Decrypt the attachment and then view, also added functionality to convert .msg file to .pdf for preview</History>
        protected async Task ViewAttachement(TempAttachementVM theUpdatedItem)
        {
            try
            {
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
                PreviewedDocumentId = theUpdatedItem.UploadedDocumentId != null ? theUpdatedItem.UploadedDocumentId : 0;
                PreviewedAttachementId = theUpdatedItem.AttachementId != null ? theUpdatedItem.AttachementId : 0;
#if DEBUG
                {
                    // Show signed document if the document has been digitally signed; otherwise, show another document.

                    var storagePath = string.IsNullOrEmpty(theUpdatedItem.SignedDocStoragePath) ? theUpdatedItem.StoragePath : theUpdatedItem.SignedDocStoragePath;
                    IsDocumentSigned = !string.IsNullOrEmpty(theUpdatedItem.SignedDocStoragePath);
                    physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + storagePath).Replace(@"\\", @"\");
                }
#else
				{
                    // Show signed document if the document has been digitally signed; otherwise, show another document.
                    var storagePath = string.IsNullOrEmpty(theUpdatedItem.SignedDocStoragePath) ? theUpdatedItem.StoragePath : theUpdatedItem.SignedDocStoragePath;

                    physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + storagePath).Replace(@"\\", @"\");
					IsDocumentSigned = !string.IsNullOrEmpty(theUpdatedItem.SignedDocStoragePath);
					physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
				}
#endif
                if (!String.IsNullOrEmpty(physicalPath))
                {
                    string base64String = await DocumentEncryptionService.GetDecryptedDocumentBase64(physicalPath, theUpdatedItem.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
                    DocumentPath = "data:application/pdf;base64," + base64String;
                    DisplayDocumentViewer = true;
                    StateHasChanged();
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
        private Stream GetFileStream(string filePath)
        {
            MemoryStream ms = new MemoryStream();
            using (FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                file.CopyTo(ms);
            return ms;
        }

        #endregion

        #region Add Document Popup

        //<History Author = 'Hassan Abbas' Date='2022-11-15' Version="1.0" Branch="master"> Open popup for uploading document in the same component</History>
        protected async Task AddDocument(TempAttachementVM? tempAttachement)
        {
            if ((bool)ShowFileNameField && (tempAttachement != null ? tempAttachement.AttachmentTypeId : MandatoryAttachmentTypeId) == (int)AttachmentTypeEnum.ClaimStatement && String.IsNullOrEmpty(CANNumber))
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Fill_CANNumber"),
                    Summary = translationState.Translate("Error"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                return;
            }
            //if (ModuleId == (int)WorkflowModuleEnum.Meeting && MandatoryAttachmentTypeId != (int)AttachmentTypeEnum.MOMAttachment)
            //{
            //	ReferenceGuid = CommunicationId;
            //}
            if (tempAttachement != null)
            {
                TempAttachementVM document = new TempAttachementVM();
                if (ModuleId == (int)WorkflowModuleEnum.Meeting)
                {
                    var result = await dialogService.OpenAsync<FileUpload>(translationState.Translate("Upload_Document"),
                    new Dictionary<string, object>()
                    {
                        { "ReferenceGuid", ReferenceGuid },
                        { "CommunicationId", CommunicationId },
                        { "IsViewOnly", IsViewOnly },
                        { "IsUploadPopup", true },
                        { "DeletedAttachementIds", DeletedAttachementIds },
                        { "FileTypes", FileTypes },
                        { "MaxFileSize", MaxFileSize },
                        { "Multiple", Multiple },
                        { "UploadFrom", UploadFrom },
                        { "ModuleId", ModuleId },
                        { "DeletedAttachementIdsChanged", DeletedAttachementIdsChanged },
                        { "IsCaseRequest", IsCaseRequest },
                        { "MandatoryAttachmentTypeId", tempAttachement.AttachmentTypeId },
                    }
                    ,
                    new DialogOptions() { Width = "30% !important" }
                    );
                    document = (TempAttachementVM)result;
                }
                else if (ModuleId == (int)WorkflowModuleEnum.LPSPrinciple)
                {
                    var result = await dialogService.OpenAsync<FileUpload>(translationState.Translate("Upload_Document"),
                    new Dictionary<string, object>()
                    {
                        { "ReferenceGuid", ReferenceGuid },
                        { "CommunicationId", CommunicationId },
                        { "IsViewOnly", IsViewOnly },
                        { "IsUploadPopup", true },
                        { "DeletedAttachementIds", DeletedAttachementIds },
                        { "FileTypes", FileTypes },
                        { "MaxFileSize", MaxFileSize },
                        { "Multiple", Multiple },
                        { "UploadFrom", UploadFrom },
                        { "ModuleId", ModuleId },
                        { "DeletedAttachementIdsChanged", DeletedAttachementIdsChanged },
                        { "IsCaseRequest", IsCaseRequest },
                        { "MandatoryAttachmentTypeId", tempAttachement.AttachmentTypeId },
                    }
                    ,
                    new DialogOptions() { Width = "30% !important" }
                    );
                    document = (TempAttachementVM)result;
                }
                else
                {
                    var result = await dialogService.OpenAsync<FileUpload>(translationState.Translate("Upload_Documents"),
                    new Dictionary<string, object>()
                    {
                        { "ReferenceGuid", ReferenceGuid },
                        { "IsViewOnly", IsViewOnly },
                        { "IsUploadPopup", true },
                        { "DeletedAttachementIds", DeletedAttachementIds },
                        { "FileTypes", FileTypes },
                        { "MaxFileSize", MaxFileSize },
                        { "Multiple", Multiple },
                        { "UploadFrom", UploadFrom },
                        { "ModuleId", ModuleId },
                        { "DeletedAttachementIdsChanged", DeletedAttachementIdsChanged },
                        { "IsCaseRequest", IsCaseRequest },
                        { "ShowFileNameField", ShowFileNameField },
                        { "MandatoryAttachmentTypeId", tempAttachement.AttachmentTypeId },
                        { "CANNumber",  CANNumber},
                        { "AutoSave", AutoSave },
                    },
                    new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true }
                    );
                    document = (TempAttachementVM)result;
                }
                if (document != null)
                {
                    MandatoryTempFiles.Remove(tempAttachement);
                    MandatoryTempFiles.Add(document);
                    if (ModuleId == (int)WorkflowModuleEnum.CaseManagement && tempAttachement.AttachmentTypeId == (int)AttachmentTypeEnum.AuthorityLetter)
                    {
                        await OnAuthorityLetterChanged.InvokeAsync((bool)true);
                    }
                    if (ModuleId == (int)WorkflowModuleEnum.COMSConsultationManagement && tempAttachement.AttachmentTypeId == (int)AttachmentTypeEnum.OfficialLetter)
                    {
                        await OnAuthorityLetterChanged.InvokeAsync((bool)true);
                    }
                }
                await mandatoryAttachGrid?.Reload();
                if (dataCommunicationService.caseRequest != null)
                {
                    dataCommunicationService.caseRequest.MandatoryTempFiles = MandatoryTempFiles;
                }
                else if (dataCommunicationService.consultationRequest != null)
                {
                    dataCommunicationService.consultationRequest.MandatoryTempFiles = MandatoryTempFiles;
                }
                else if (ModuleId == (int)WorkflowModuleEnum.Meeting)
                {
                    dataCommunicationService.saveMeetingVM.MandatoryTempFiles = MandatoryTempFiles;
                }
                if (ModuleId == (int)WorkflowModuleEnum.OrganizingCommittee)
                {
                    dataCommunicationService.saveCommitteeVM.MandatoryTempFiles = MandatoryTempFiles;
                }
            }
            else
            {
                TempAttachementVM document = new TempAttachementVM();
                if (ModuleId == (int)WorkflowModuleEnum.Meeting)
                {
                    var result = await dialogService.OpenAsync<FileUpload>(translationState.Translate("Upload_Documents"),
                    new Dictionary<string, object>()
                    {
                        { "ReferenceGuid", ReferenceGuid },
                        { "CommunicationId", CommunicationId },
                        { "IsViewOnly", IsViewOnly },
                        { "IsUploadPopup", true },
                        { "DeletedAttachementIds", DeletedAttachementIds },
                        { "FileTypes", FileTypes },
                        { "MaxFileSize", MaxFileSize },
                        { "Multiple", Multiple },
                        { "UploadFrom", UploadFrom },
                        { "ModuleId", ModuleId },
                        { "DeletedAttachementIdsChanged", DeletedAttachementIdsChanged },
                        { "MandatoryAttachmentTypeId", (int)AttachmentTypeEnum.Others },
                        { "IsCaseRequest", IsCaseRequest },
                    },
                    new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true }
                );

                    document = (TempAttachementVM)result;
                }
                else if (ModuleId == (int)WorkflowModuleEnum.OrganizingCommittee)
                {
                    var result = await dialogService.OpenAsync<FileUpload>(translationState.Translate("Upload_Documents"),
                    new Dictionary<string, object>()
                    {
                        { "ReferenceGuid", ReferenceGuid },
                        { "IsViewOnly", IsViewOnly },
                        { "IsUploadPopup", true },
                        { "DeletedAttachementIds", DeletedAttachementIds },
                        { "FileTypes", FileTypes },
                        { "MaxFileSize", MaxFileSize },
                        { "Multiple", Multiple },
                        { "UploadFrom", UploadFrom },
                        { "ModuleId", ModuleId },
                        { "DeletedAttachementIdsChanged", DeletedAttachementIdsChanged },
                        { "IsOrganizingCommittee", IsOrganizingCommittee },
                    },
                    new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true }
                );

                    document = (TempAttachementVM)result;
                }
                else
                {
                    var result = await dialogService.OpenAsync<FileUpload>(translationState.Translate("Upload_Documents"),
                    new Dictionary<string, object>()
                    {
                        { "ReferenceGuid", ReferenceGuid },
                        { "IsViewOnly", IsViewOnly },
                        { "IsUploadPopup", true },
                        { "DeletedAttachementIds", DeletedAttachementIds },
                        { "FileTypes", FileTypes },
                        { "MaxFileSize", MaxFileSize },
                        { "Multiple", Multiple },
                        { "UploadFrom", UploadFrom },
                        { "ModuleId", ModuleId },
                        { "DeletedAttachementIdsChanged", DeletedAttachementIdsChanged },
                        { "MandatoryAttachmentTypeId", MandatoryAttachmentTypeId },
                        { "IsCaseRequest", IsCaseRequest },
                        { "ShowFileNameField", ShowFileNameField },
                        { "CANNumber",  CANNumber},
                        { "AutoSave", AutoSave },
                    },
                    new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true }
                );

                    document = (TempAttachementVM)result;
                }

                if (ModuleId == (int)WorkflowModuleEnum.Meeting)
                {
                    if (document != null)
                    {
                        MandatoryTempFiles.Add(document);
                    }
                    await mandatoryAttachGrid?.Reload();
                    //relevent to Consultation Request
                    if (dataCommunicationService.sendCommunicationVM != null)
                    {
                        dataCommunicationService.sendCommunicationVM.MandatoryTempFiles = MandatoryTempFiles;
                    }
                    else if (ModuleId == (int)WorkflowModuleEnum.Meeting && MandatoryAttachmentTypeId == (int)AttachmentTypeEnum.RequestForMeeting)
                    {
                        dataCommunicationService.saveMeetingVM.MandatoryTempFiles = MandatoryTempFiles;
                    }
                }
                else if (ModuleId == (int)WorkflowModuleEnum.OrganizingCommittee)
                {
                    if (document != null)
                    {
                        MandatoryTempFiles.Add(document);
                    }
                    //relevent to Consultation Request
                    if (ModuleId == (int)WorkflowModuleEnum.OrganizingCommittee && MandatoryAttachmentTypeId == (int)AttachmentTypeEnum.FatwaCircular)
                    {
                        dataCommunicationService.saveCommitteeVM.MandatoryTempFiles = MandatoryTempFiles;
                    }
                }
                else
                {
                    if (document != null)
                    {
                        if (ModuleId == (int)WorkflowModuleEnum.Meeting)
                        {
                            AdditionalTempFiles.Add(document);
                        }
                        else if ((bool)IsCaseRequest)
                            AdditionalTempFiles.Add(document);
                        else if (IsLLSOtherSourceDocuments && ModuleId == (int)WorkflowModuleEnum.LPSPrinciple)
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Document_Uploaded_Successfully"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            await OnLegalPrincipleOtherDocumentUpload.InvokeAsync((bool)true);
                            return;

                        }
                        else if (ModuleId == (int)WorkflowModuleEnum.OrganizingCommittee)
                        {
                            AdditionalTempFiles.Add(document);
                        }
                        else
                            TempFiles.Add(document);
                    }
                }

                additionalAttachGrid?.Reset();
                if (additionalAttachGrid != null)
                {
                    await additionalAttachGrid?.Reload();
                }
                if (ModuleId != (int)WorkflowModuleEnum.Meeting && ModuleId != (int)WorkflowModuleEnum.OrganizingCommittee)
                {
                    await PopulateAttachmentGrid();
                }

                //relevent to Case Request
                if (dataCommunicationService.caseRequest != null)
                {
                    dataCommunicationService.caseRequest.AdditionalTempFiles = AdditionalTempFiles;
                }
                else if (dataCommunicationService.consultationRequest != null)
                {
                    dataCommunicationService.consultationRequest.AdditionalTempFiles = AdditionalTempFiles;
                }
                else if (ModuleId == (int)WorkflowModuleEnum.Meeting)
                {
                    dataCommunicationService.saveMeetingVM.AdditionalTempFiles = AdditionalTempFiles;
                }
                else if (ModuleId == (int)WorkflowModuleEnum.OrganizingCommittee)
                {
                    dataCommunicationService.saveCommitteeVM.AdditionalTempFiles = AdditionalTempFiles;

                }
            }
            Reload();
        }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        #endregion

        #region Dropdown Data and Change Events

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Populate Subtypes data</History>
        protected async Task PopulateAttachmentTypes()
        {
            ApiCallResponse response;
            if ((bool)IsViewOnly || (bool)LoadAllAttachmentTypes)
                response = await lookupService.GetAttachmentTypes(0);
            else if (ModuleId == (int)WorkflowModuleEnum.CNTContactManagement)
                response = await fileUploadService.GetAllAttachmentTypes();
            else
                response = await lookupService.GetAttachmentTypes(ModuleId);
            if (response.IsSuccessStatusCode)
            {
                AttachmentTypes = (List<AttachmentType>)response.ResultData;
                G2GAttachmentTypes = AttachmentTypes?.Where(t => t.IsGePortalType).ToList();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        protected async Task OnTypeChange(object args)
        {
            try
            {
                ShowOtherAttachmentType = false;
                ShowReferenceFields = false;
                if (args == null)
                {
                    ShowOtherAttachmentType = false;
                    ShowReferenceFields = false;
                    return;
                }
                if ((AttachmentTypeEnum)args == AttachmentTypeEnum.Other || (ModuleId == (int)WorkflowModuleEnum.Meeting && (AttachmentTypeEnum)args == AttachmentTypeEnum.Others)
                    || (ModuleId == (int)WorkflowModuleEnum.LPSPrinciple && (AttachmentTypeEnum)args == AttachmentTypeEnum.OthersPrinciple
                    || ModuleId == (int)WorkflowModuleEnum.LeaveAndAttendance && (AttachmentTypeEnum)args == AttachmentTypeEnum.OtherLeaveAttendance))
                {
                    ShowOtherAttachmentType = true;
                }
                else
                {
                    ShowOtherAttachmentType = false;
                }

                if ((bool)AttachmentTypes.Where(t => t.AttachmentTypeId == (int)args).FirstOrDefault()?.IsOfficialLetter)
                {
                    ShowReferenceFields = true;
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Pupup Events

        protected async Task CloseDialog(MouseEventArgs args)
        {
            dialogService.Close(null);
        }

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master">Invoke Hidden File Uploader button on Form submit click to upload file and submit form at the same time</History>
        protected async Task Form0Submit(TempAttachementVM args)
        {
            try
            {
                spinnerService.Show();
                if (FilesValidationInfo.Where(f => f.Value == true).Any())
                {
                    await JsInterop.InvokeVoidAsync("UploadFile");
                }
                else
                {
                    spinnerService.Hide();
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("Please_Select_File"),
                        Summary = translationState.Translate("Error"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Expansion/Render
        protected async Task ExpandRow(TempAttachementVM tempfile)
        {
            if (tempfile.CommunicationGuid != Guid.Empty)
                ChildTempFiles = TempFiles.Where(x => x.PreCommunicationId == tempfile.CommunicationGuid).ToList();

        }
        public void RowRender(RowRenderEventArgs<TempAttachementVM> args)
        {
            if (args.Data.ChildCount == 0)
            {
                args.Attributes.Add("class", "no-withdraw-linked");
            }
        }

        #endregion

        #region Decription View

        #endregion

        #region Communication Details
        public async Task PopulateCommunicationDetail(Guid communicationId)
        {
            ApiCallResponse communicationResponse = new ApiCallResponse();
            if (SubModuleId == (int)SubModuleEnum.CaseRequest)
            {
                communicationResponse = await communicationService.GetCommunicationDetailByCaseRequestId((Guid)ReferenceGuid, communicationId);
            }
            else if (SubModuleId == (int)SubModuleEnum.CaseFile)
            {
                communicationResponse = await communicationService.GetCommunicationDetailByCaseFileId((Guid)ReferenceGuid, communicationId);
            }
            else if (SubModuleId == (int)SubModuleEnum.RegisteredCase)
            {
                communicationResponse = await communicationService.GetCommunicationDetailByCaseId((Guid)ReferenceGuid, communicationId);
            }
            else if (SubModuleId == (int)SubModuleEnum.ConsultationRequest)
            {
                communicationResponse = await communicationService.GetCommunicationDetailByConsultationRequestId((Guid)ReferenceGuid, communicationId);
            }
            else if (SubModuleId == (int)SubModuleEnum.ConsultationFile)
            {
                communicationResponse = await communicationService.GetCommunicationDetailByConsultationFileId((Guid)ReferenceGuid, communicationId);
            }
            if (communicationResponse.IsSuccessStatusCode)
            {
                communicationDetail = (CommunicationListVM)communicationResponse.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(communicationResponse);
            }
        }
        protected async Task ViewResponse(TempAttachementVM value)
        {
            try
            {
                await PopulateCommunicationDetail((Guid)value.CommunicationGuid);
                if (communicationDetail != null)
                {
                    if (communicationDetail.CommunicationTypeId == (int)CommunicationTypeEnum.RequestForMeeting)
                    {
                        navigationManager.NavigateTo("/meeting-view/" + communicationDetail.CommunicationId + "/" + communicationDetail.ReferenceId + "/" + communicationDetail.CommunicationTypeId + "/" + true + "/" + communicationDetail.SubModuleId);
                    }
                    else if (communicationDetail.CommunicationTypeId == (int)CommunicationTypeEnum.MeetingScheduled)
                    {
                        navigationManager.NavigateTo("/meeting-view/" + communicationDetail.CommunicationId + "/" + communicationDetail.CommunicationTypeId + "/" + true);
                    }
                    else if (communicationDetail.CommunicationTypeId == (int)CommunicationTypeEnum.WithdrawRequested)
                    {
                        RedirectURL = "/detail-withdraw-request/" + communicationDetail.ReferenceId + "/" + (int)CommunicationTypeEnum.WithdrawRequested;
                        navigationManager.NavigateTo(RedirectURL);
                    }
                    else
                    {
                        RedirectURL = "/correspondence-detail-view/" + communicationDetail.ReferenceId + "/" + communicationDetail.CommunicationId + "/" + communicationDetail.SubModuleId + "/" + communicationDetail.CommunicationTypeId;
                        navigationManager.NavigateTo(RedirectURL);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        protected async Task DigitalSignature(TempAttachementVM value)
        {
            IsSplitButtonsVisible = false;
            var dialogResult = await dialogService.OpenAsync<DigitalSignature>(
                 translationState.Translate("Digital_Signature"),
                 new Dictionary<string, object>() {
                    { "DocumentId", value.UploadedDocumentId },
                    { "AttachmentTypeId", value.AttachmentTypeId },
                    { "StatusId", value.StatusId },
                 },
                 new DialogOptions() { Width = "32% !important", CloseDialogOnOverlayClick = true });
            await PopulateAttachmentGrid();
            IsSplitButtonsVisible = true;
        }
        protected async Task SendForSigning(TempAttachementVM value)
        {
            IsSplitButtonsVisible = false;
            if (await dialogService.OpenAsync<SendForSigning>(
                 translationState.Translate("Send_For_Signing"),
                 new Dictionary<string, object>() {
                    { "Document", value },
                    { "SubModuleId", SubModuleId }
                 },
                 new DialogOptions() { Width = "35% !important", CloseDialogOnOverlayClick = true }) == true)
            {
                await PopulateAttachmentGrid();
            }
            IsSplitButtonsVisible = true;
        }
        protected async Task DSDocumentHistory(TempAttachementVM value)
        {
            IsSplitButtonsVisible = false;
            var dialogResult = await dialogService.OpenAsync<DigitalSignatureDocumentHistory>(
                 translationState.Translate("Signing_Status_History"),
                 new Dictionary<string, object>()
                 {
                 { "DocumentId", value.UploadedDocumentId }
                 },
                 new DialogOptions() { Width = "70% !important", CloseDialogOnOverlayClick = true });
            if (dialogResult != null)
            {

            }
            IsSplitButtonsVisible = true;

        }
        protected async Task<bool> GetAttachmentTypeById(int attachmentTypeId)
        {
            var response = await lookupService.GetDocumentTypeById(attachmentTypeId);
            if (response.IsSuccessStatusCode)
            {
                var attachmentType = (AttachmentType)response.ResultData;
                return attachmentType.DesignationIds.Contains(loginState.UserDetail.DesignationId);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                return false;
            }
        }

        protected async Task<bool> GetIsAlreadySigned(string civilId, int documentId)
        {
            var response = await fileUploadService.GetIsAlreadySigned(civilId, documentId);
            if (response.IsSuccessStatusCode)
            {
                var isAlreadySigned = Boolean.Parse(response.ResultData.ToString());
                return isAlreadySigned;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                return false;
            }
        }

        #region For New Case File

        #region PopulateRequestTypes
        protected async Task PopulateRequestTypes()
        {
            var response = await lookupService.GetRequestTypes();
            if (response.IsSuccessStatusCode)
            {
                RequestTypes = (List<RequestType>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        #endregion

        protected async Task PopulateGovtEntities()
        {
            var response = await lookupService.GetGovernmentEntities();
            if (response.IsSuccessStatusCode)
            {
                GovtEntities = (List<GovernmentEntity>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        protected async Task OnRequestTypeChange()
        {
            try
            {
                if (RequestTypeId > 0)
                {
                    if (RequestTypeId > (int)RequestTypeEnum.CivilCommercial)
                    {
                        ModuleId = (int)WorkflowModuleEnum.COMSConsultationManagement;
                        UploadFrom = "ConsultationManagement";
                        if (RequestTypeId == (int)RequestTypeEnum.Contracts)
                        {
                            MandatoryAttachmentTypeId = (int)AttachmentTypeEnum.OfficialLetter;
                            TempAttachement.AttachmentTypeId = (int)AttachmentTypeEnum.OfficialLetter;
                        }
                        else
                        {
                            MandatoryAttachmentTypeId = (int)AttachmentTypeEnum.OfficialLetter;
                            TempAttachement.AttachmentTypeId = (int)AttachmentTypeEnum.OfficialLetter;
                        }
                    }
                    else
                    {
                        ModuleId = (int)WorkflowModuleEnum.CaseManagement;
                        UploadFrom = "CaseManagement";
                        MandatoryAttachmentTypeId = (int)AttachmentTypeEnum.AuthorityLetter;
                        TempAttachement.AttachmentTypeId = (int)AttachmentTypeEnum.AuthorityLetter;
                    }
                    await PopulateAttachmentTypes();
                    await OnTypeChange(MandatoryAttachmentTypeId);
                }
                StateHasChanged();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        protected void RedirectToRequestPage()
        {
            spinnerService.Show();
            if (RequestTypeId > (int)RequestTypeEnum.CivilCommercial)
            {
                dataCommunicationService.consultationRequest = new ConsultationRequest { ConsultationRequestId = (Guid)ReferenceGuid, RequestTypeId = RequestTypeId, GovtEntityId = GovtEntityId };
                navigationManager.NavigateTo("/consultationrequest-add");
            }
            else
            {
                dataCommunicationService.caseRequest = new CaseRequest { RequestId = (Guid)ReferenceGuid, RequestTypeId = RequestTypeId, GovtEntityId = GovtEntityId };
                navigationManager.NavigateTo("/create-casefile");
            }
            spinnerService.Hide();
        }

        #endregion

        #region Format File Max Size
        protected string FormatFileMaxSize(int fileMaxSizeBytes)
        {
            var asdasf = $"{fileMaxSizeBytes / (1024.0 * 1024 * 1024):F2} GB";
            if (fileMaxSizeBytes < 1024)
            {
                return $"{fileMaxSizeBytes} Bytes";
            }
            if (fileMaxSizeBytes < 1024.0 * 1024)
            {
                return $"{fileMaxSizeBytes / 1024:F2} KB";
            }
            if (fileMaxSizeBytes < 1024.0 * 1024 * 1024)
            {
                return $"{fileMaxSizeBytes / (1024 * 1024):F2} MB";
            }
            return $"{fileMaxSizeBytes / (1024.0 * 1024 * 1024):F2} GB";
        }
        #endregion
    }
}
