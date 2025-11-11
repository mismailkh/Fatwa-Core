using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_GENERAL.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Telerik.Blazor.Components;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
namespace FATWA_WEB.Pages.CaseManagment.Shared
{
    public partial class DetailCaseParty : ComponentBase
    {

        #region Parameter
        [Parameter]
        public dynamic CasePartyId { get; set; }
        #endregion

        #region Varriable
        public CasePartyLinkVM CasePartyDetail { get; set; } = new CasePartyLinkVM();
        protected List<CasePartyLinkVM> CasePartyLinks;
        protected byte[] FileData { get; set; }
        protected string DocumentPath { get; set; }
        protected TelerikPdfViewer PdfViewerRef { get; set; }
        protected bool ShowDocumentViewer { get; set; }
        protected bool ShowIndividualSection { get; set; }
        protected bool ShowCompanySection { get; set; }
        protected bool ShowGovtEntitySection { get; set; }
        #endregion

        #region ON Load Component
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateCasePartyDetails();
            spinnerService.Hide();
        }

        protected async Task PopulateCasePartyDetails()
        {
            var partyResponse = await caseRequestService.GetCasePartyDetailById(Guid.Parse(CasePartyId));
            if (partyResponse.IsSuccessStatusCode)
            {
                CasePartyDetail = (CasePartyLinkVM)partyResponse.ResultData;
                if (CasePartyDetail.TypeId == (int)CasePartyTypeEnum.Individual)
                {
                    ShowIndividualSection = true;
                    ShowCompanySection = false;
                    ShowGovtEntitySection = false;
                }
                else if ((int)CasePartyDetail.TypeId == (int)CasePartyTypeEnum.Company)
                {
                    ShowIndividualSection = false;
                    ShowCompanySection = true;
                    ShowGovtEntitySection = false;
                }
                else if (CasePartyDetail.TypeId == (int)CasePartyTypeEnum.GovernmentEntity)
                {
                    ShowIndividualSection = false;
                    ShowCompanySection = false;
                    ShowGovtEntitySection = true;
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(partyResponse);
            }
        }
        #endregion

        #region Load Authority Letter

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

        #region Redirect Function
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
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion
    }
}
