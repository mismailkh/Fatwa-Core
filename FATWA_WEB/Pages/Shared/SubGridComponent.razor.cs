using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_GENERAL.Helper;
using Microsoft.AspNetCore.Components;
using Radzen;
using Syncfusion.Blazor.PdfViewerServer;
using System.Collections.ObjectModel;
using System.Security.Cryptography;
using System.Text;
using Telerik.Blazor.Components;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.Shared
{
    public partial class SubGridComponent : ComponentBase
    {
        #region Parameter
        [Parameter]
        public ObservableCollection<TempAttachementVM> MainTempFiles { get; set; }
        [Parameter]
        public List<TempAttachementVM> TempFiles { get; set; }
        [Parameter]
        public Guid? ReferenceGuid { get; set; }
        [Parameter]
        public int? SubModuleId { get; set; }
        #endregion

        #region variables
        public TelerikPdfViewer PdfViewerRef { get; set; }
        protected List<AttachmentType> AttachmentTypes { get; set; } = new List<AttachmentType>();
        public byte[] FileData { get; set; }
        public bool DisplayDocumentViewer { get; set; }
        public List<TempAttachementVM> ChildTempFiles { get; set; } = new List<TempAttachementVM>();
        public CommunicationListVM communicationDetail = new CommunicationListVM();
        protected string RedirectURL { get; set; }
        public SfPdfViewerServer pdfViewer;
        public string DocumentPath { get; set; } = string.Empty;

        #endregion

        #region Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateAttachmentTypes();
            spinnerService.Hide();
        }
        #endregion

        #region Dropdown Data and Change Events
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
        #endregion

        protected async Task ViewAttachement(TempAttachementVM theUpdatedItem)
        {
            try
            {
                var physicalPath = string.Empty;
                DisplayDocumentViewer = false;
                StateHasChanged();
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
                    FileData = await DocumentEncryptionService.GetDecryptedDocumentBytes(physicalPath, theUpdatedItem.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
                    string base64String = Convert.ToBase64String(FileData);
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

        protected async Task ExpandRow(TempAttachementVM tempfile)
        {
            ChildTempFiles = MainTempFiles.Where(x => x.PreCommunicationId == tempfile.CommunicationGuid).ToList();
        }

        public void RowRender(RowRenderEventArgs<TempAttachementVM> args)
        {
            if (args.Data.ChildCount != 0)
            {
                if (args.Data.AttachmentTypeId == (int)AttachmentTypeEnum.CmsLegalNotification || args.Data.AttachmentTypeId == (int)AttachmentTypeEnum.ComsLegalNotification
                    || args.Data.AttachmentTypeId == (int)AttachmentTypeEnum.LegalNotificationResponse)
                {

                }
                else
                {
                    args.Attributes.Add("class", "no-withdraw-linked");
                }
            }
            else
            {
                args.Attributes.Add("class", "no-withdraw-linked");
            }
        }
        #region Communication Detail
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
    }
}
