using DocumentFormat.OpenXml.Drawing.Diagrams;
using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_GENERAL.Helper;
using FATWA_WEB.Pages.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Syncfusion.Blazor.PdfViewer;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Response;
using LinkTarget = FATWA_DOMAIN.Models.CommonModels.LinkTarget;

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    public partial class RequestCaseForMoreInformation : ComponentBase
    {
        #region parameter

        [Parameter]
        public string ReferenceId { get; set; }
        [Parameter]
        public string PreCommunicationId { get; set; }
        [Parameter]
        public dynamic TaskId { get; set; }
        #endregion

        #region Variables

        protected int Priority_Id;
        protected int GE_Id;
        protected string typeValidationMsg = "";
        protected string reasonValidationMsg = "";
        protected string dateValidationMsg = "";
        protected List<Priority> Priorities { get; set; } = new List<Priority>();
        protected List<ResponseType> ResponseType { get; set; }
        public DateTime ResponseDate = DateTime.Now;
        public DateTime NowTime = DateTime.Now;

        protected List<AttachmentType> AttachmentTypes { get; set; } = new List<AttachmentType>();
        protected CmsRegisteredCaseDetailVM registeredCaseDetail { get; set; } = new CmsRegisteredCaseDetailVM();
        protected List<GovernmentEntity> GovernmentEntities { get; set; } = new List<GovernmentEntity>();
        protected List<Frequency> frequency { get; set; } = new List<Frequency>();
        protected RequestMoreInfoVM requestMoreInfoVM { get; set; } = new RequestMoreInfoVM();
        protected Validations validations { get; set; } = new Validations();
        public string caseRequestUrl { get; set; }
        protected List<TempAttachementVM> uploadedAttachment { get; set; } = new List<TempAttachementVM>();
        protected RadzenDataGrid<TempAttachementVM>? upload = new RadzenDataGrid<TempAttachementVM>();
        protected bool isUploaded { get; set; } = true;
        public byte[] FileData { get; set; }
        public string DocumentPath { get; set; }
        public int? PreviewedDocumentId { get; set; }
        public int? PreviewedAttachementId { get; set; }
        List<ToolbarItem> ToolbarItems = new List<ToolbarItem>();
        public bool DisplayDocumentViewer { get; set; }

        protected SendCommunicationVM sendCommunication = new SendCommunicationVM()
        {
            Communication = new Communication(),
            CommunicationResponse = new CommunicationResponse(),
            CommunicationTargetLink = new CommunicationTargetLink(),
            LinkTarget = new List<LinkTarget>(),
        };

        LinkTarget linkTarget { get; set; }
        protected List<GovernmentEntity> PartiesGovernmentEntities { get; set; } = new List<GovernmentEntity>();
        protected List<CasePartyLinkVM> CasePartyLinks = new List<CasePartyLinkVM>();
        protected RadzenDataGrid<TempAttachementVM> gridAttachments { get; set; }
        protected ObservableCollection<TempAttachementVM> attachments { get; set; }
        public bool allowRowSelectOnRowClick = true;
        #endregion

        #region LOAD

        //<History Author = 'Hassan Iftikhar'  Version="1.0" Branch="master"> </History>
        protected override async Task OnInitializedAsync()
        {
            sendCommunication.Communication.CommunicationId = Guid.NewGuid();
            caseRequestUrl = "/case-view/" + ReferenceId + "";
            var result = await cmsRegisteredCaseService.GetRegisteredCaseDetailByIdVM(Guid.Parse(ReferenceId));

            if (result.IsSuccessStatusCode)
            {
                registeredCaseDetail = (CmsRegisteredCaseDetailVM)result.ResultData;
                //await PopulateRegisteredCaseDetail();
                await PopulatePriorities();
                await PopulateResponseTypes();
                await PopulateGovernmentEntities();
                await PopulateFrequency();
                await PopulateAttachmentTypes();
                await PopulateCasePartyGrid();
                await PopulateAttachementsGrid();
            }

            else
            {
                caseRequestUrl = "/case-view/" + ReferenceId + "";
                await PopulateResponseTypes();
            }
        }
        //<History Author = 'Hassan Iftikhar'  Version="1.0" Branch="master"> </History>


        #endregion

        #region Validation
        protected class Validations
        {
            public string reason { get; set; } = string.Empty;
            public string types { get; set; } = string.Empty;
            public string dueDate { get; set; } = string.Empty;

        }
        protected bool ValidateBasicDetailsOnChange()
        {
            bool basicDetailsValid = true;
            if (sendCommunication.CommunicationResponse.ResponseTypeId == null)
            {
                typeValidationMsg = @translationState.Translate("Required_Field");
                validations.types = "k-invalid";
                basicDetailsValid = false;
            }
            else
            {
                validations.types = "k-valid";
            }
            //if (string.IsNullOrWhiteSpace(sendCommunication.CommunicationResponse.Reason))
            //{

            //    reasonValidationMsg = @translationState.Translate("Required_Field");
            //    validations.reason = "k-invalid";
            //    basicDetailsValid = false;
            //}
            //else
            //{
            //    validations.reason = "k-valid";
            //}

            if (basicDetailsValid != true)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Fill_Required_Fields"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });


            }
            return basicDetailsValid;
        }
        #endregion

        #region Button Click
        //<History Author = 'Hassan Iftikhar'  Version="1.0" Branch="master"> </History>
        public async Task ButtonSaveClick()
        {
            typeValidationMsg = "";
            reasonValidationMsg = "";


            bool res = ValidateBasicDetailsOnChange();
            if (res != false)
            {
                await FillCommunicationModel();

                var result = await dialogService.OpenAsync<SelectDraftTemplatePopup>(translationState.Translate("Add_Draft"),
                new Dictionary<string, object>()
                {
                    { "ReferenceId", ReferenceId },
                    { "ModuleId", (int)WorkflowModuleEnum.CaseManagement },
                    { "DraftEntityType",   (int)DraftEntityTypeEnum.CaseNeedMoreInfo},
                    { "Payload", Newtonsoft.Json.JsonConvert.SerializeObject(sendCommunication) },
                    { "Document_Type", (int)AttachmentTypeEnum.CmsLegalNotification },
                    { "ResponseTypeId", (int)sendCommunication.CommunicationResponse.ResponseTypeId },
                    { "DocumentTypes", await identifyDocumentTypes.GetDocumentTypes(sendCommunication,(int)DraftEntityTypeEnum.CaseNeedMoreInfo, loginState.UserDetail.SectorTypeId != null ? (int)loginState.UserDetail.SectorTypeId : 0)},
                    { "TaskId" , TaskId != null ?  TaskId : null }
                }
                ,
                new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
            }
        }

        protected async Task ButtonBackClick(MouseEventArgs args)
        {
            navigationManager.NavigateTo("/case-view/" + ReferenceId);
        }


        #endregion

        #region Remote Dropdown Data and Dropdown Change Events
        protected async Task PopulateCasePartyGrid()
        {
            var partyResponse = await caseRequestService.GetCMSCasePartyDetailById(Guid.Parse(ReferenceId));
            if (partyResponse.IsSuccessStatusCode)
            {
                CasePartyLinks = (List<CasePartyLinkVM>)partyResponse.ResultData;
                CasePartyLinks = CasePartyLinks?.Where(x => x.TypeId == (int)CasePartyTypeEnum.GovernmentEntity).ToList();
                if (CasePartyLinks?.Count > 0)
                {
                    var entityIds = CasePartyLinks?.Select(link => link.EntityId).ToList();
                    PartiesGovernmentEntities = GovernmentEntities?
                        .Where(entity => entityIds?.Contains(entity.EntityId) == true)
                        .ToList();
                    GovernmentEntities = GovernmentEntities?
                        .Where(entity => entityIds?.Contains(entity.EntityId) == false)
                        .ToList();
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(partyResponse);
            }
        }
        protected async Task PopulatePriorities()
        {
            var response = await lookupService.GetCasePriorities();
            if (response.IsSuccessStatusCode)
            {
                Priorities = (List<Priority>)response.ResultData;

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        protected async Task PopulateGovernmentEntities()
        {
            var response = await lookupService.GetGovernmentEntities();
            if (response.IsSuccessStatusCode)
            {
                GovernmentEntities = (List<GovernmentEntity>)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        protected async Task PopulateResponseTypes()
        {
            var response = await lookupService.GetResponseTypes();
            if (response.IsSuccessStatusCode)
            {
                ResponseType = (List<ResponseType>)response.ResultData;
                ResponseType = ResponseType.Where(x =>
                                x.Id == (int)ResponseTypeEnum.CaseRegistered
                             || x.Id == (int)ResponseTypeEnum.CaseFileExecution
                             || x.Id == (int)ResponseTypeEnum.InitialJudgement
                             || x.Id == (int)ResponseTypeEnum.FinalJudgement
                             || x.Id == (int)ResponseTypeEnum.GeneralUpdate
                             || x.Id == (int)ResponseTypeEnum.InterrogationJudgement
                             || x.Id == (int)ResponseTypeEnum.RequestForMoreInformation
                             || x.Id == (int)ResponseTypeEnum.IncomingReport).ToList();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task PopulateFrequency()
        {
            var response = await lookupService.GetFrequency();
            if (response.IsSuccessStatusCode)
            {
                frequency = (List<Frequency>)response.ResultData;


            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        protected async Task PopulateAttachmentTypes()
        {
            ApiCallResponse response;
            response = await lookupService.GetAttachmentTypes(0);

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
        protected async Task PopulateAttachementsGrid()
        {
            try
            {
                if (ReferenceId != null)
                {
                    attachments = await fileUploadService.GetUploadedAttachements(false, 0, Guid.Parse(ReferenceId));
                    if (attachments != null && attachments.Any())
                    {
                        Regex regex = new Regex("_[0-9]+", RegexOptions.RightToLeft);
                        attachments = new ObservableCollection<TempAttachementVM>(attachments?.Select(f => { if (f.FileName.Contains("Document_Portfolio")) return f; else f.FileName = regex.Replace(f.FileName, "", 1); return f; }).OrderByDescending(item => item.AttachmentTypeId == (int)AttachmentTypeEnum.ClaimStatement).ThenBy(item => item.AttachmentTypeId).ToList());
                        sendCommunication.SelectedDocuments = attachments.Where(x => x.AttachmentTypeId == (int)AttachmentTypeEnum.ClaimStatement).ToList();
                    }
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

        #region Redirect Buttons
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

        #region Fill Communication Model

        protected async Task FillCommunicationModel()
        {
            await CalculateCommunicationType();
            sendCommunication.Communication.SectorTypeId = (int)loginState.UserDetail.SectorTypeId;
            sendCommunication.Communication.SourceId = (int)CommunicationSourceEnum.FATWA;
            sendCommunication.Communication.SentBy = loginState.Username;
            sendCommunication.Communication.ReceivedBy = registeredCaseDetail.CaseRequstCreatedBy;
            sendCommunication.Communication.GovtEntityId = registeredCaseDetail.GovtEntityId;
            sendCommunication.Communication.CorrespondenceTypeId = (int)CommunicationCorrespondenceTypeEnum.Outbox;
            sendCommunication.Communication.CreatedBy = loginState.Username;
            sendCommunication.Communication.CreatedDate = DateTime.Now;
            sendCommunication.Communication.IsDeleted = false;
            sendCommunication.Communication.OutboxShortNum = 0;
            sendCommunication.Communication.PreCommunicationId = PreCommunicationId != null ? Guid.Parse(PreCommunicationId) : Guid.Empty;
            //if (loginState.UserRoles.Any(u => u.RoleId == SystemRoles.Lawyer))
            //{
            //    sendCommunication.Communication.ColorId = (int)CommunicationColorEnum.Yellow;
            //}
            //else if (loginState.UserRoles.Any(u => u.RoleId == SystemRoles.HOS))
            //{
            //    sendCommunication.Communication.ColorId = (int)CommunicationColorEnum.Green;
            //}

            //Communication Response 
            sendCommunication.CommunicationResponse.CommunicationResponseId = Guid.NewGuid();
            sendCommunication.CommunicationResponse.CommunicationId = sendCommunication.Communication.CommunicationId;
            sendCommunication.CommunicationResponse.RequestDate = registeredCaseDetail.CreatedDate;
            sendCommunication.CommunicationResponse.ResponseDate = NowTime;
            sendCommunication.CommunicationResponse.CreatedBy = loginState.Username;
            sendCommunication.CommunicationResponse.CreatedDate = DateTime.Now;
            sendCommunication.CommunicationResponse.IsDeleted = false;


            //CommunicationTargetLink
            sendCommunication.CommunicationTargetLink.CreatedBy = loginState.Username;
            sendCommunication.CommunicationTargetLink.CreatedDate = DateTime.Now;
            sendCommunication.CommunicationTargetLink.IsDeleted = false;
            sendCommunication.CommunicationTargetLink.CommunicationId = sendCommunication.Communication.CommunicationId;
            PopulateLinkTargets();
        }

        protected async Task CalculateCommunicationType()
        {
            if (sendCommunication.CommunicationResponse.ResponseTypeId == (int)ResponseTypeEnum.CaseRegistered)
            {
                sendCommunication.Communication.Title = "Legal_Notification_Case_Registered";
                sendCommunication.Communication.Description = "Legal_Notification_Case_Registered";
                sendCommunication.Communication.CommunicationTypeId = (int)CommunicationTypeEnum.CaseRegistered;
            }
            else if (sendCommunication.CommunicationResponse.ResponseTypeId == (int)ResponseTypeEnum.CaseFileExecution)
            {
                sendCommunication.Communication.Title = "Legal_Notification_Case_File_Execution";
                sendCommunication.Communication.Description = "Legal_Notification_Case_File_Execution";
                sendCommunication.Communication.CommunicationTypeId = (int)CommunicationTypeEnum.CaseFileExecution;
            }
            else if (sendCommunication.CommunicationResponse.ResponseTypeId == (int)ResponseTypeEnum.InitialJudgement)
            {
                sendCommunication.Communication.Title = "Legal_Notification_Initial_Judgement";
                sendCommunication.Communication.Description = "Legal_Notification_Initial_Judgement";
                sendCommunication.Communication.CommunicationTypeId = (int)CommunicationTypeEnum.InitialJudgement;
            }
            else if (sendCommunication.CommunicationResponse.ResponseTypeId == (int)ResponseTypeEnum.GeneralUpdate)
            {
                sendCommunication.Communication.Title = "Legal_Notification_General_Update";
                sendCommunication.Communication.Description = "Legal_Notification_General_Update";
                sendCommunication.Communication.CommunicationTypeId = (int)CommunicationTypeEnum.GeneralUpdate;
            }
            else if (sendCommunication.CommunicationResponse.ResponseTypeId == (int)ResponseTypeEnum.InterrogationJudgement)
            {
                sendCommunication.Communication.Title = "Legal_Notification_Interrogation_Judgement";
                sendCommunication.Communication.Description = "Legal_Notification_Interrogation_Judgement";
                sendCommunication.Communication.CommunicationTypeId = (int)CommunicationTypeEnum.InterrogationJudgement;
            }
            else if (sendCommunication.CommunicationResponse.ResponseTypeId == (int)ResponseTypeEnum.IncomingReport)
            {
                sendCommunication.Communication.Title = "Legal_Notification_Incoming_Report";
                sendCommunication.Communication.Description = "Legal_Notification_Incoming_Report";
                sendCommunication.Communication.CommunicationTypeId = (int)CommunicationTypeEnum.IncomingReport;
            }
            else if (sendCommunication.CommunicationResponse.ResponseTypeId == (int)ResponseTypeEnum.RequestForMoreInformation)
            {
                sendCommunication.Communication.Title = "Legal_Notification_Registered_Case_Need_More_Details";
                sendCommunication.Communication.Description = "Legal_Notification_Registered_Case_Need_More_Details";
                sendCommunication.Communication.CommunicationTypeId = (int)CommunicationTypeEnum.RequestMoreInfo;
            }
            else if (sendCommunication.CommunicationResponse.ResponseTypeId == (int)ResponseTypeEnum.FinalJudgement)
            {
                sendCommunication.Communication.Title = "Legal_Notification_Final_Judgement";
                sendCommunication.Communication.Description = "Legal_Notification_Final_Judgement";
                sendCommunication.Communication.CommunicationTypeId = (int)CommunicationTypeEnum.FinalJudgement;
            }
            else if (sendCommunication.CommunicationResponse.ResponseTypeId == (int)ResponseTypeEnum.RequestForMoreInformationReminder)
            {
                sendCommunication.Communication.Title = "Legal_Notification_Request_For_More_Information_Reminder";
                sendCommunication.Communication.Description = "Legal_Notification_Request_For_More_Information_Reminder";
                sendCommunication.Communication.CommunicationTypeId = (int)CommunicationTypeEnum.RequestForMoreInformationReminder;
            }
            else if (sendCommunication.CommunicationResponse.ResponseTypeId == (int)ResponseTypeEnum.SaveAndCloseFile)
            {
                sendCommunication.Communication.Title = "Legal_Notification_Save_And_Close_File";
                sendCommunication.Communication.Description = "Legal_Notification_Save_And_Close_File";
                sendCommunication.Communication.CommunicationTypeId = (int)CommunicationTypeEnum.SaveAndCloseFile;
            }
            else if (sendCommunication.CommunicationResponse.ResponseTypeId == (int)ResponseTypeEnum.FinalDocument)
            {
                sendCommunication.Communication.Title = "Legal_Notification_Final_Document";
                sendCommunication.Communication.Description = "Legal_Notification_Final_Document";
                sendCommunication.Communication.CommunicationTypeId = (int)CommunicationTypeEnum.FinalDocument;
            }
        }

        protected void PopulateLinkTargets()
        {
            linkTarget = new LinkTarget()
            {
                LinkTargetId = new Guid(),
                ReferenceId = sendCommunication.Communication.CommunicationId,
                IsPrimary = false,
                LinkTargetTypeId = (int)LinkTargetTypeEnum.Communication
            };
            sendCommunication.LinkTarget.Add(linkTarget);

            linkTarget = new LinkTarget()
            {
                LinkTargetId = new Guid(),
                ReferenceId = sendCommunication.CommunicationResponse.CommunicationResponseId,
                IsPrimary = false,
                LinkTargetTypeId = (int)LinkTargetTypeEnum.Communication
            };
            sendCommunication.LinkTarget.Add(linkTarget);

            linkTarget = new LinkTarget()
            {
                LinkTargetId = new Guid(),
                ReferenceId = Guid.Parse(ReferenceId),
                IsPrimary = true,
                LinkTargetTypeId = (int)LinkTargetTypeEnum.RegisteredCase,
            };


            sendCommunication.LinkTarget.Add(linkTarget);

        }
        #endregion

        #region File Upload
        protected async Task AddDocument()
        {
            var result = await dialogService.OpenAsync<FileUpload>(translationState.Translate("Upload_Document"),
                new Dictionary<string, object>()
                {
                        { "ReferenceGuid", Guid.Empty },
                        { "CommunicationId", sendCommunication.Communication.CommunicationId },
                        { "IsViewOnly", false },
                        { "IsUploadPopup", true },
                        { "FileTypes", new List<string>() { ".pdf" } },
                        { "MaxFileSize", systemSettingState.File_Maximum_Size },
                        { "Multiple", true },
                        { "UploadFrom", "CaseManagement" },
                        { "ModuleId", (int)WorkflowModuleEnum.CaseManagement },
                        { "IsCaseRequest", false },

                }
                ,
                new DialogOptions() { Width = "30% !important" });
            uploadedAttachment = uploadedAttachment ?? new List<TempAttachementVM>();
            if (result != null)
                uploadedAttachment.Add((TempAttachementVM)result);
            if (uploadedAttachment.Count != 0)
            {
                isUploaded = true;
            }
            else
            {
                isUploaded = false;
            }
            upload.Reset();
            StateHasChanged();
        }
        protected async Task RemoveDocument(TempAttachementVM file)
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Remove_File"), translationState.Translate("Remove_File"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                var result = false;
                if (file == null)
                    return;

                result = await fileUploadService.RemoveTempAttachement(file?.FileName, file?.UploadedBy, "CaseManagement", "G2G_WEB", file?.AttachmentTypeId);
                if (result)
                {
                    uploadedAttachment = uploadedAttachment ?? new List<TempAttachementVM>();
                    var fileToRemove = uploadedAttachment.Find(f => f.FileName == file.FileName);
                    if (fileToRemove != null)
                    {
                        uploadedAttachment.Remove(fileToRemove);
                        upload.Reset();
                    }

                    if (uploadedAttachment.Count == 0)
                    {
                        isUploaded = false;
                    }
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
        #endregion

        #region View and Download Attachment


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
                    var physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + theUpdatedItem.StoragePath).Replace(@"\\", @"\");
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
					var RleasephysicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + theUpdatedItem.StoragePath).Replace(@"\\", @"\");
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
        }

        //<History Author = 'Hassan Abbas' Date='2024-03-21' Version="1.0" Branch="master"> Open Document in New Window</History>
        protected async Task OpenInNewWindow(MouseEventArgs args)
        {
            var url = PreviewedDocumentId > 0 ? $"/preview-document/{ReferenceId}/{PreviewedDocumentId}" : $"/preview-attachement/{ReferenceId}/{PreviewedAttachementId}";
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

        #endregion
    }
}
