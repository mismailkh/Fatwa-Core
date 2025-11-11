using BlazorDownloadFile;
using FATWA_DOMAIN.Models.DigitalSignature;
using Microsoft.AspNetCore.Components;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.DS
{
    public partial class SignatureVerificationDetailsPopUp : ComponentBase
    {
        #region Paramter
        [Parameter]
        public int? DocumentId { get; set; }
        #endregion

        #region Variables
        private SignatureVerificationResponse signatureVerification = new SignatureVerificationResponse();
        public ApiCallResponse VerificationResponse = new ApiCallResponse();
        string FailedResponse = string.Empty;
        #endregion

        #region OnInitialized
        protected override async Task OnInitializedAsync()
        {
            FailedResponse = translationState.Translate("Loading") + "...";
            spinnerService.Show();

            VerificationResponse = await fileUploadService.GetSignatureVerification((int)DocumentId);
            if (VerificationResponse.IsSuccessStatusCode)
            {
                signatureVerification = (SignatureVerificationResponse)VerificationResponse.ResultData;
            }
            else
            {
                FailedResponse = translationState.Translate("Something_went_wrong_Please_try_again");
            }
            spinnerService.Hide();
        }
        #endregion
        #region Funtions
        private async Task DownloadCertificate(string certificate)
        {
            byte[] certificateBytes = Convert.FromBase64String(certificate);
            var fileName = "certificate.crt";
            await blazorDownloadFileService.DownloadFile(fileName, certificateBytes, "application/x-x509-ca-cert");
        }
        public async Task CancelChanges()
        {
            dialogService.Close();
        }
        #endregion
    }
}
