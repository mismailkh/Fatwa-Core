using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_GENERAL.Helper;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Syncfusion.Blazor.PdfViewerServer;
using System.Collections.ObjectModel;
using Telerik.Blazor.Components;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;

namespace FATWA_WEB.Pages.Consultation.Shared
{
    public partial class ViewConsultationDetailMoreInformation : ComponentBase
    {
        #region Parameter

        [Parameter]
        public dynamic ReferenceId { get; set; }

        [Parameter]
        public dynamic IsConsultationRequest { get; set; }

        [Parameter]
        public dynamic ByActivity { get; set; }

        [Parameter]
        public dynamic CommunicationId { get; set; }

        #endregion

        #region Variables

        public bool IsRequest { get { return Convert.ToBoolean(IsConsultationRequest); } set { IsConsultationRequest = value; } }
        public int Activity { get { return Convert.ToInt32(ByActivity); } set { ByActivity = value; } }
        protected bool ShowDocumentViewer { get; set; }
        public ObservableCollection<TempAttachementVM> OfficialAttachments { get; set; } = new ObservableCollection<TempAttachementVM>();
        protected byte[] FileData { get; set; }
        protected string DocumentPath { get; set; }
        protected TelerikPdfViewer PdfViewerRef { get; set; }
        protected List<AttachmentType> AttachmentTypes { get; set; } = new List<AttachmentType>();
        protected bool isRecordFound = true;
        public SfPdfViewerServer pdfViewer; 
        #endregion

        #region Send Response Detail View Grid Load Properties Load

        CommunicationSendResponseVM _communicationSendResponseVM;
        protected CommunicationSendResponseVM communicationSendResponseVM
        {
            get
            {
                return _communicationSendResponseVM;
            }
            set
            {
                if (!object.Equals(_communicationSendResponseVM, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "CommunicationSendResponseVM", NewValue = value, OldValue = _communicationSendResponseVM };
                    _communicationSendResponseVM = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        #endregion

        #region Model full property Instance

        public MarkupString CaseRequestUrl { get; set; }
        public string CaseRequestUrlS { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {

        }
        #endregion

        #region Grid Search

        string _search;
        protected string search
        {
            get
            {
                return _search;
            }
            set
            {
                if (!object.Equals(_search, value))
                {
                    var args = new FATWA_WEB.Services.PropertyChangedEventArgs() { Name = "search", NewValue = value, OldValue = _search };
                    _search = value;

                    Reload();
                }
            }
        }

        #endregion

        #region Request Response Need More Detail View  Load Properties Load

        ComsConsultationRequestResponseVM _consultationRequestResponseVM;
        protected ComsConsultationRequestResponseVM consultationRequestResponseVM
        {
            get
            {
                return _consultationRequestResponseVM;
            }
            set
            {
                if (!object.Equals(_consultationRequestResponseVM, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "legalLegislation", NewValue = value, OldValue = _consultationRequestResponseVM };
                    _consultationRequestResponseVM = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }


        #endregion

        #region Request Response Need More Detail View  Load Properties Load

        CommunicationSendMessageVM _communicationSendMessageVM;
        protected CommunicationSendMessageVM communicationSendMessageVM
        {
            get
            {
                return _communicationSendMessageVM;
            }
            set
            {
                if (!object.Equals(_communicationSendMessageVM, value))
                {
                    var args = new PropertyChangedEventArgs() { Name = "CommunicationSendMessage", NewValue = value, OldValue = _communicationSendMessageVM };
                    _communicationSendMessageVM = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }


        #endregion

        #region Component Load

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();

            await PopulateAttachmentTypes();

            if (!IsRequest)
            {
                if (Activity == (int)CommunicationTypeEnum.SendResponse)
                {

                    CaseRequestUrlS = @"/consultationfile-list/";
                    await CommunicationSendResponseDetailbyId();

                }
                if (Activity == (int)CommunicationTypeEnum.RequestMoreInfo)
                {

                    CaseRequestUrlS = @"/consultationfile-list/";
                    await FileRequestNeedMoreDetailLoad();

                }
                if (Activity == (int)CommunicationTypeEnum.SendMessage)
                {
                    CaseRequestUrlS = @"/consultationfile-list/";
                    await CommunicationSendMessageDetailbyId();
                }
            }
            else
            {
                if (Activity == (int)CommunicationTypeEnum.SendResponse)
                {

                    CaseRequestUrlS = @"/consultationrequest-list";
                    await CommunicationSendResponseDetailbyId();

                }
                if (Activity == (int)CommunicationTypeEnum.RequestMoreInfo)
                {
                    CaseRequestUrlS = @"/consultationrequest-list";
                    await ConsultationRequestResponse();

                }
                if (Activity == (int)CommunicationTypeEnum.SendMessage)
                {
                    CaseRequestUrlS = @"/consultationrequest-list";
                    await CommunicationSendMessageDetailbyId();
                }

            }

            await LoadAuthorityLetter();
            spinnerService.Hide();
        }

        protected async Task PopulateAttachmentTypes()
        {
            var response = await lookupService.GetAttachmentTypes(0);
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

        //<History Author = 'Hassan Abbas' Date='2023-07-05' Version="1.0" Branch="master"> Modified the function to Decrypt the attachment and then view, also added functionality to convert .msg file to .pdf for preview</History>
        protected async Task LoadAuthorityLetter()
        {
            try
            {
                if (isRecordFound)
                {
                    OfficialAttachments = await fileUploadService.GetUploadedAttachements(false, 0, Guid.Parse(ReferenceId));
                    var attachment = OfficialAttachments?.Where(t => t.AttachmentTypeId == (int)AttachmentTypeEnum.ComsLegalNotification).FirstOrDefault() ?? OfficialAttachments?.FirstOrDefault();
                    if (attachment != null)
                    {
                        var physicalPath = string.Empty;
#if DEBUG
                        {

                            physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + attachment.StoragePath).Replace(@"\\", @"\");
                        }
#else
{

                            physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + attachment.StoragePath).Replace(@"\\", @"\");
                            physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
}
#endif
                        if (!string.IsNullOrEmpty(physicalPath))
                        {
                            FileData = await DocumentEncryptionService.GetDecryptedDocumentBytes(physicalPath, attachment.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
                            string base64String = Convert.ToBase64String(FileData);
                            DocumentPath = "data:application/pdf;base64," + base64String;
                            ShowDocumentViewer = true;
                            StateHasChanged();
                        }
                        else
                        {
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Error,
                                Detail = translationState.Translate("Authority_Letter_Not_Loaded"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                        }
                    }
                }
            }
            catch
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        protected async Task ConsultationRequestResponse()
        {
            try
            {
                var response = await consultationRequestService.GetConsultationRequestResponseById(Guid.Parse(ReferenceId));
                if (response.IsSuccessStatusCode)
                {
                    consultationRequestResponseVM = (ComsConsultationRequestResponseVM)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected async Task FileRequestNeedMoreDetailLoad()
        {
            try
            {
                // legislationsVM.LegislationId = Guid.Parse(LegislationId);
                var response = await consultationRequestService.GetConsultationFileResponseById(Guid.Parse(ReferenceId));
                if (response.IsSuccessStatusCode)
                {
                    consultationRequestResponseVM = (ComsConsultationRequestResponseVM)response.ResultData;
                }
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    isRecordFound = false;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected async Task CommunicationSendMessageDetailbyId()
        {
            try
            {
                // legislationsVM.LegislationId = Guid.Parse(LegislationId);
                var response = await communicationService.CommunicationSendMessageDetailbyId(ReferenceId);
                if (response.IsSuccessStatusCode)
                {
                    communicationSendMessageVM = (CommunicationSendMessageVM)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected async Task CommunicationSendResponseDetailbyId()
        {
            try
            {
                // legislationsVM.LegislationId = Guid.Parse(LegislationId);
                var response = await communicationService.CommunicationSendResponseDetailbyId(ReferenceId);
                if (response.IsSuccessStatusCode)
                {
                    communicationSendResponseVM = (CommunicationSendResponseVM)response.ResultData;
                }
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion

        #region FILE PREVIEW

        //<History Author = 'Hassan Abbas' Date='2023-07-05' Version="1.0" Branch="master"> Modified the function to Decrypt the attachment and then view, also added functionality to convert .msg file to .pdf for preview</History>
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
                    FileData = await DocumentEncryptionService.GetDecryptedDocumentBytes(physicalPath, theUpdatedItem.DocType, _config.GetValue<string>("DocumentEncryptionKey"));
                    string base64String = Convert.ToBase64String(FileData);
                    DocumentPath = "data:application/pdf;base64," + base64String;
                    ShowDocumentViewer = true;
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

        #region Redirect Functions

        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }

        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }

        protected async Task BtnBack(MouseEventArgs args)
        {
            await JSRuntime.InvokeVoidAsync("history.back");
        }

        protected void BtnView(MouseEventArgs args)
        {
            navigationManager.NavigateTo($"/Request-For-More-Information/{ReferenceId}/{IsConsultationRequest}");
        }
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion

    }
}
