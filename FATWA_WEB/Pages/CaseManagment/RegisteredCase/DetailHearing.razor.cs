using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_GENERAL.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Syncfusion.Blazor.PdfViewerServer;
using Telerik.Blazor.Components;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;

namespace FATWA_WEB.Pages.CaseManagment.RegisteredCase
{
    //<History Author = 'Hassan Abbas' Date='2023-11-27' Version="1.0" Branch="master">Outcome Hearing page showing the Hearing details, atachments and portfolio if any</History> -->
    public partial class DetailHearing : ComponentBase
    {
        #region Parameter
        [Parameter]
        public dynamic HearingId { get; set; }
        [Parameter]
        public dynamic OutcomeId { get; set; }
        #endregion

        #region Varriable
        public HearingDetailVM hearingDetail { get; set; } = new HearingDetailVM();
        protected byte[] FileData { get; set; }
        public string DocumentPath { get; set; } = string.Empty;
        protected TelerikPdfViewer PdfViewerRef { get; set; }
        protected bool ShowDocumentViewer { get; set; }
        public OutcomeHearingDetailVM outcomeDetail { get; set; } = new OutcomeHearingDetailVM();
        protected List<JudgementVM> Judgements = new List<JudgementVM>();
        protected List<TransferHistoryVM> TransferHistoryVMs = new List<TransferHistoryVM>();
        protected RadzenDataGrid<CaseOutcomePartyLinkHistoryVM> PartiesGrid;
        protected List<CaseOutcomePartyLinkHistoryVM> CasePartyLinks;
        public SfPdfViewerServer pdfViewer; 
        #endregion

        #region ON Load Component
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateHearingDetails();
            if (OutcomeId != null)
            {
                await PopulateOutcomeDetails();
                await PopulateJudgements();
                await PopulateTransferHistoryDetail();
                await PopulatePartiesGrid();
            }
            spinnerService.Hide();
        }

        #endregion
        #region Dropdown and Grid Population Events
        protected async Task PopulateHearingDetails()
        {
            try
            {
                var response = await cmsRegisteredCaseService.GetHearingDetail(Guid.Parse(HearingId));
                if (response.IsSuccessStatusCode)
                {
                    hearingDetail = (HearingDetailVM)response.ResultData;
                    if (!String.IsNullOrWhiteSpace(hearingDetail.PortfolioStoragePath))
                    {
                        await LoadDocumentPortfolio();
                    }
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected async Task PopulateOutcomeDetails()
        {
            try
            {
                var response = await cmsRegisteredCaseService.GetOutcomeDetail(Guid.Parse(OutcomeId));
                if (response.IsSuccessStatusCode)
                {
                    outcomeDetail = (OutcomeHearingDetailVM)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected async Task PopulateJudgements()
        {
            try
            {
                var response = await cmsRegisteredCaseService.GetJudgementsByOutcome(Guid.Parse(OutcomeId));
                if (response.IsSuccessStatusCode)
                {
                    Judgements = (List<JudgementVM>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected async Task PopulateTransferHistoryDetail()
        {
            try
            {
                var response = await cmsRegisteredCaseService.GetTransferHistoryByOutcome(Guid.Parse(OutcomeId), Guid.Empty);
                if (response.IsSuccessStatusCode)
                {
                    TransferHistoryVMs = (List<TransferHistoryVM>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        protected async Task PopulatePartiesGrid()
        {
            var partyResponse = await cmsRegisteredCaseService.GetCMSCaseOutcomePartyHistoryDetailById(Guid.Parse(OutcomeId));
            if (partyResponse.IsSuccessStatusCode)
            {
                CasePartyLinks = (List<CaseOutcomePartyLinkHistoryVM>)partyResponse.ResultData;
                CasePartyLinks = CasePartyLinks?.Select(c => { c.CasePartyCategory = (CasePartyCategoryEnum)c.CategoryId; c.CasePartyType = (CasePartyTypeEnum)c.TypeId; c.CasePartyAction = (CaseOutcomePartyActionEnum)c.ActionId; return c; }).ToList();
                foreach (var casePartyLink in CasePartyLinks)
                {
                    casePartyLink.CasePartyActionName = translationState.Translate(casePartyLink.CasePartyAction.ToString());
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(partyResponse);
            }
        }
        public void PartyRowRender(RowRenderEventArgs<CaseOutcomePartyLinkHistoryVM> args)
        {
            try
            {
                if (args.Data.AttachmentCount <= 0)
                {
                    args.Attributes.Add("class", "no-party-attachment");
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        #region Load Authority Letter
        //<History Author = 'Hassan Abbas' Date='2023-07-05' Version="1.0" Branch="master"> Modified the function to Decrypt the attachment and then view, also added functionality to convert .msg file to .pdf for preview</History>
        protected async Task LoadDocumentPortfolio()
        {
            try
            {
                var physicalPath = string.Empty;
#if DEBUG
                {
                    physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + hearingDetail.PortfolioStoragePath).Replace(@"\\", @"\");

                }
#else
                {
                    physicalPath = Path.Combine(_config.GetValue<string>("dms_file_path") + hearingDetail.PortfolioStoragePath).Replace(@"\\", @"\");
                    physicalPath = physicalPath.Replace("\\wwwroot\\Attachments\\", "\\");
                            
                }
#endif
                if (!string.IsNullOrEmpty(physicalPath))
                {
                    FileData = await DocumentEncryptionService.GetDecryptedDocumentBytes(physicalPath, ".pdf", _config.GetValue<string>("DocumentEncryptionKey"));
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

        #region Redirect Function
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        private void ViewCaseDetails()
        {
            navigationManager.NavigateTo("/case-view/" + hearingDetail.CaseId);
        }
        protected async Task BtnBack(MouseEventArgs args)
        {
            await JSRuntime.InvokeVoidAsync("history.back");
        }
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        protected void DetailJudgement(JudgementVM args)
        {
            navigationManager.NavigateTo("/judgement-view/" + args.Id);
        }
        protected void DetailCaseParty(CaseOutcomePartyLinkHistoryVM args)
        {
            navigationManager.NavigateTo("/caseparty-view/" + args.Id);
        }
        #endregion

    }
}

