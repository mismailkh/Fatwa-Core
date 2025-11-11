using FATWA_DOMAIN.Enums;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.DigitalSignature;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_GENERAL.Helper;
using FATWA_WEB.Pages.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using System.Diagnostics;
using static FATWA_DOMAIN.Enums.DmsEnums;

namespace FATWA_WEB.Pages.DS
{
    public partial class DigitalSignature : ComponentBase
    {
        #region Parameter
        [Parameter]
        public int DocumentId { get; set; }
        [Parameter]
        public int AttachmentTypeId { get; set; }
        [Parameter]
        public int StatusId { get; set; }
        [Parameter]
        public bool? HideLocalSigning { get; set; } = false;
        #endregion

        #region Variables
        protected ExternalSigningRequest externalSigningRequest = new ExternalSigningRequest();
        protected AttachmentType attachmentType = new AttachmentType();
        protected DsSigningRequestTaskLogVM task { get; set; }
        protected List<DsSigningRequestTaskLogVM> taskList = new List<DsSigningRequestTaskLogVM>();
        protected bool isSigningAllow;
        #endregion

        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await Task.Delay(100);
            await GetAllTasksForSignature();
            if (StatusId == (int)SigningTaskStatusEnum.SendForSigning)
            {
                task = taskList.OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            }
            await GetAttachmentTypeById();
            spinnerService.Hide();
        }

        protected async Task GetAllTasksForSignature()
        {
            var response = await fileUploadService.GetAllTasksForSignature(DocumentId);
            if (response.IsSuccessStatusCode)
            {
                taskList = (List<DsSigningRequestTaskLogVM>)response.ResultData;
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        protected async Task GetAttachmentTypeById()
        {
            var response = await lookupService.GetDocumentTypeById(AttachmentTypeId);
            if (response.IsSuccessStatusCode)
            {
                attachmentType = (AttachmentType)response.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
        }

        #region LocalSigning
        public async Task LocalSigning()
        {
            if(!loginState.UserDetail.HasSignatureImage)
            {
                await dialogService.OpenAsync<AlertDialog>((translationState.Translate("alert")),
                 new Dictionary<string, object>()
                 {
                     { "Title","signature_image_notfound" }
                 },
                 new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = false });

            } else if (systemSettingState.SigningMethods.Where(x => x.MethodId == (int)SigningMethodEnum.LocalSigning && x.IsActive == false).Any()) {
                await dialogService.OpenAsync<AlertDialog>((translationState.Translate("alert")),
                 new Dictionary<string, object>()
                 {
                     { "Title","temporarily_unavailable" }
                 },
                 new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = false });

            } else {
                DsSigningResponseVM dsSigningResponse = new DsSigningResponseVM();
                string password = _config.GetValue<string>("DocumentEncryptionKey");
                var profName = "DPS_Image";
                var userId = loginState.UserDetail.UserId; //use the same AD user Id that used in Enrollment
                var sessionToken = EncryptionDecryption.EncryptText(userId + "_" + DateTime.Now, password);
                var uiLang = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? "EN" : "AR";//either use ‘EN’ or ‘AR’
                try
                {
                    var result = await JsInterop.InvokeAsync<int>("LocalSigning", DocumentId, userId, profName, sessionToken, uiLang);
                    // Handle the success result here
                    await JSRuntime.InvokeVoidAsync("console.log", $"Local signing succeeded with result: {result}");
                    NotifyMessage("Successfull_Signing", NotificationSeverity.Success);
                    dsSigningResponse.RequestStatus = SigningRequestStatusEnum.Approved.GetDisplayName();
                    dsSigningResponse.SigningMethodId = (int)SigningMethodEnum.LocalSigning;
                    dialogService.Close(dsSigningResponse);
                }    
                catch (Exception ex) 
                { // Handle the error here
                    await JSRuntime.InvokeVoidAsync("console.log", $"Local signing failed: {ex.Message}");
                    NotifyMessage("Failed_Signing", NotificationSeverity.Error);
                    dsSigningResponse.RequestStatus = SigningRequestStatusEnum.Failed.GetDisplayName();
                    dsSigningResponse.SigningMethodId = (int)SigningMethodEnum.LocalSigning;
                    dialogService.Close(dsSigningResponse);
                }
            }
        }
        #endregion

        #region RemoteSigning
        public async Task RemoteSigning()
        {
            if (!loginState.UserDetail.HasSignatureImage)
            {
                await dialogService.OpenAsync<AlertDialog>((translationState.Translate("alert")),
                 new Dictionary<string, object>()
                 {
                     { "Title","signature_image_notfound" }
                 },
                 new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = false });

            } else if (systemSettingState.SigningMethods.Where(x => x.MethodId == (int)SigningMethodEnum.RemoteSigning && x.IsActive == false).Any()) {
                await dialogService.OpenAsync<AlertDialog>((translationState.Translate("alert")),
                 new Dictionary<string, object>()
                 {
                     { "Title","temporarily_unavailable" }
                 },
                 new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = false });

            } else {
                var dialogResult = await dialogService.OpenAsync<DigitalsignatureMetaData>(
                translationState.Translate("Digital_Signature") + translationState.Translate(SigningMethodEnum.RemoteSigning.GetDisplayName()),
                new Dictionary<string, object>()
                {

                },
                new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
                if (dialogResult != null)
                {
                    spinnerService.SigningShow();

                    string requestStatus = "";
                    DsSigningResponseVM dsSigningResponse = new DsSigningResponseVM();
                    externalSigningRequest.DocumentId = DocumentId;
                    externalSigningRequest.UserId = loginState.UserDetail.UserId;
                    externalSigningRequest.DataTitle = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? attachmentType.Type_En : attachmentType.Type_Ar;
                    externalSigningRequest.DataDescription = dialogResult.Description; //translationState.Translate("Data_Description");
                    externalSigningRequest.SelectedReason = dialogResult.Reason; //translationState.Translate("Selected_Reason");
                    externalSigningRequest.CreatedBy = loginState.Username;
                    var res = await fileUploadService.RemoteSigningRequest(externalSigningRequest);
                    if (res.IsSuccessStatusCode)
                    {
                        var requestId = (string)res.ResultData;
                        Stopwatch s = new Stopwatch();
                        s.Start();
                        while (s.Elapsed < TimeSpan.FromSeconds(600))
                        {
                            var authResponse = await fileUploadService.GetDSPSigningRequestStatus(requestId);
                            if (authResponse.IsSuccessStatusCode)
                            {
                                requestStatus = (string)authResponse.ResultData;
                                if (requestStatus == SigningRequestStatusEnum.Approved.GetDisplayName())
                                {
                                    NotifyMessage("Successfull_Signing", NotificationSeverity.Success);
                                    break;
                                }
                                if (requestStatus == SigningRequestStatusEnum.Declined.GetDisplayName())
                                {
                                    NotifyMessage("Declined_Signing", NotificationSeverity.Info);
                                    break;
                                }
                                if (requestStatus == SigningRequestStatusEnum.Failed.GetDisplayName())
                                {
                                    NotifyMessage("Failed_Signing", NotificationSeverity.Error);
                                    dialogService.Close(SigningTaskStatusEnum.Failed);
                                    break;
                                }
                            }
                        }
                        s.Stop();
                        if (requestStatus == SigningRequestStatusEnum.Initiated.GetDisplayName())
                        {
                            await fileUploadService.UpdateDSPRequestLog(requestId, SigningRequestStatusEnum.TimeOut.GetDisplayName());
                            NotifyMessage("TimeOut_Signing", NotificationSeverity.Warning);
                        }
                    }
                    else
                    {
                        spinnerService.Hide();
                        await invalidRequestHandlerService.ReturnBadRequestNotification(res);
                    }
                    dsSigningResponse.RequestStatus = requestStatus;
                    dsSigningResponse.SigningMethodId = (int)SigningMethodEnum.RemoteSigning;
                    dialogService.Close(dsSigningResponse);
                }
            }
        }
        #endregion

        #region ExternalSigning
        public async Task ExternalSigning()
        {
            if (!loginState.UserDetail.HasSignatureImage)
            {
                await dialogService.OpenAsync<AlertDialog>((translationState.Translate("alert")),
                 new Dictionary<string, object>()
                 {
                     { "Title","signature_image_notfound" }
                 },
                 new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = false });

            } else if (systemSettingState.SigningMethods.Where(x => x.MethodId == (int) SigningMethodEnum.ExternalSigning && x.IsActive == false).Any()) {
                await dialogService.OpenAsync<AlertDialog>((translationState.Translate("alert")),
                 new Dictionary<string, object>()
                 {
                     { "Title","temporarily_unavailable" }
                 },
                 new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = false });

            } else {
                var dialogResult = await dialogService.OpenAsync<DigitalsignatureMetaData>(
                translationState.Translate("Digital_Signature") + translationState.Translate(SigningMethodEnum.ExternalSigning.GetDisplayName()),
                new Dictionary<string, object>()
                {

                },
                new DialogOptions() { Width = "30% !important", CloseDialogOnOverlayClick = true });
                if (dialogResult != null)
                {
                    spinnerService.SigningShow();
                    string requestStatus = "";
                    DsSigningResponseVM dsSigningResponse = new DsSigningResponseVM();
                    externalSigningRequest.DocumentId = DocumentId;
                    externalSigningRequest.UserId = loginState.UserDetail.UserId;
                    externalSigningRequest.DataTitle = Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? attachmentType.Type_En : attachmentType.Type_Ar;
                    externalSigningRequest.DataDescription = dialogResult.Description;  //translationState.Translate("Data_Description");
                    externalSigningRequest.SelectedReason = dialogResult.Reason; //translationState.Translate("Selected_Reason");
                    externalSigningRequest.CreatedBy = loginState.Username;
                    var res = await fileUploadService.ExternalSigningRequest(externalSigningRequest);
                    if (res.IsSuccessStatusCode)
                    {
                        var requestId = (string)res.ResultData;
                        await JSRuntime.InvokeVoidAsync("console.log", "RequestId= " + requestId);
                        Console.WriteLine("RequestId = " + requestId);

                        Stopwatch s = new Stopwatch();
                        s.Start();
                        while (s.Elapsed < TimeSpan.FromSeconds(600))
                        {
                            var authResponse = await fileUploadService.GetDSPSigningRequestStatus(requestId);
                            await JSRuntime.InvokeVoidAsync("console.log", "requestStatus = " + authResponse);
                            if (authResponse.IsSuccessStatusCode)
                            {
                                await JSRuntime.InvokeVoidAsync("console.log", "requestStatusWithinSuccess = " + requestStatus); 
                                await JSRuntime.InvokeVoidAsync("console.log", "TimeSpanWithinSuccess  = " + s.Elapsed);
                                requestStatus = (string)authResponse.ResultData;
                                if (requestStatus == SigningRequestStatusEnum.Approved.GetDisplayName())
                                {
                                    NotifyMessage("Successfull_Signing", NotificationSeverity.Success);
                                    break;
                                }
                                if (requestStatus == SigningRequestStatusEnum.Declined.GetDisplayName())
                                {
                                    NotifyMessage("Declined_Signing", NotificationSeverity.Info);
                                    break;
                                }
                                if (requestStatus == SigningRequestStatusEnum.Failed.GetDisplayName())
                                {
                                    NotifyMessage("Failed_Signing", NotificationSeverity.Error);
                                    break;
                                }
                            }
                        }
                        s.Stop();
                        await JSRuntime.InvokeVoidAsync("console.log", "RequestId = " + requestId);
                        Console.WriteLine("RequestId= " + requestId);
                        await JSRuntime.InvokeVoidAsync("console.log", "TimeSpan = " + s.Elapsed);
                        Console.WriteLine("TimeSpan outside while= " + s.Elapsed);
                        if (requestStatus == SigningRequestStatusEnum.Initiated.GetDisplayName())
                        {
                            await fileUploadService.UpdateDSPRequestLog(requestId, SigningRequestStatusEnum.TimeOut.GetDisplayName());
                            NotifyMessage("TimeOut_Signing", NotificationSeverity.Warning);
                        }
                    }
                    else
                    {
                        spinnerService.Hide();
                        NotifyMessage("Failed_Signing", NotificationSeverity.Error);
                    }
                    dsSigningResponse.RequestStatus = requestStatus;
                    dsSigningResponse.SigningMethodId = (int)SigningMethodEnum.ExternalSigning;
                    dialogService.Close(dsSigningResponse);
                }
            }
        }
        #endregion

        public void NotifyMessage(string messages, NotificationSeverity notificationSeverity)
        {
            spinnerService.Hide();
            notificationService.Notify(new NotificationMessage()
            {
                Severity = notificationSeverity,
                Detail = translationState.Translate(messages),
                Style = "position: fixed !important; left: 0; margin: auto; "
            });
        }

    }
}
