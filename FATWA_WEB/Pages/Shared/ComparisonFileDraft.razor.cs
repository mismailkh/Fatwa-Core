using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using SelectPdf;
using System.Net;
using Telerik.Blazor.Components;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;

namespace FATWA_WEB.Pages.Shared
{
    public partial class ComparisonFileDraft : ComponentBase
    {
        #region Parameter

        [Parameter]
        public dynamic? DraftId { get; set; }
        [Parameter]
        public dynamic? VersionId1 { get; set; }
        [Parameter]
        public dynamic? VersionId2 { get; set; }
        #endregion

        #region Variables
        public TelerikPdfViewer PdfViewerRef { get; set; }
        protected List<CaseTemplate> HeaderFooterTemplates { get; set; }

        public class DraftedDocumentComparisonVM
        {
            public byte[] File { get; set; }
            public decimal VersionNumber { get; set; }
            public string CreatedBy { get; set; }
        }

        public DraftedDocumentComparisonVM comparisonVM1 = new DraftedDocumentComparisonVM();
        public DraftedDocumentComparisonVM comparisonVM2 = new DraftedDocumentComparisonVM();
        #endregion

        #region Component Load

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Component Load</History>
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulateHeaderFooter();
            comparisonVM1 = await PopulateCaseDraftData(Guid.Parse(DraftId),Guid.Parse(VersionId1));
            comparisonVM2 = await PopulateCaseDraftData(Guid.Parse(DraftId), Guid.Parse(VersionId2));
            spinnerService.Hide();
        }

        #endregion

        //<History Author = 'Hassan Abbas' Date='2024-01-01' Version="1.0" Branch="master">Populate Header Footer</History>
        protected async Task PopulateHeaderFooter()
        {
            var response = await cmsCaseTemplateService.GetHeaderFooter();
            if (response.IsSuccessStatusCode)
            {
                HeaderFooterTemplates = (List<CaseTemplate>)response.ResultData;
            }
        }
        protected async Task<DraftedDocumentComparisonVM> PopulateCaseDraftData(Guid DraftId, Guid VersionId)
        {
            var response = await cmsCaseTemplateService.GetDraftDocDetailWithSectionAndParameters(DraftId,VersionId);
            if (response.IsSuccessStatusCode && response.StatusCode == HttpStatusCode.OK)
            {
                var draftedTemplate = (CmsDraftedDocumentDetailVM)response.ResultData;
                var comparison = new DraftedDocumentComparisonVM
                {
                    File = await PopulatePdfFromHtml(draftedTemplate.Content),
                    VersionNumber = draftedTemplate.VersionNumber,
                    CreatedBy = draftedTemplate.CreatedBy
                };
                return comparison;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                return null;
            }
            StateHasChanged();
        }

        //<History Author = 'Hassan Abbas' Date='2024-01-01' Version="1.0" Branch="master">Modified the function to add header footers on each page</History>
        protected async Task<byte[]> PopulatePdfFromHtml(string TemplateContent)
        {
            try
            {
                HtmlToPdf converter = new HtmlToPdf();
                MemoryStream stream = new MemoryStream();

                // set converter options
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
                converter.Options.MarginRight = 30;
                converter.Options.MarginLeft = 30;

                string headerHtmlContent = HeaderFooterTemplates.Where(x => x.Id == (Thread.CurrentThread.CurrentUICulture.Name == "en-US" ? (int)CaseTemplateEnum.HeaderEn : (int)CaseTemplateEnum.HeaderAr)).Select(x => x.Content).FirstOrDefault();
                string footerHtmlContent = HeaderFooterTemplates.Where(x => x.Id == (int)CaseTemplateEnum.Footer).Select(x => x.Content).FirstOrDefault();
                PdfHtmlSection headerHtml = new PdfHtmlSection(headerHtmlContent, "");
                PdfHtmlSection footerHtml = new PdfHtmlSection(footerHtmlContent, "");
                headerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                footerHtml.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
                converter.Header.Add(headerHtml);
                converter.Footer.Add(footerHtml);
				converter.Options.EmbedFonts = true; 
				TemplateContent = string.Concat($"<style>@font-face {{font-family: 'Sultan';src: url('{Directory.GetCurrentDirectory().Replace("\\", "/") + "/wwwroot/fonts/Sultan/arfonts-sultan-normal.ttf"}') format('truetype');font-weight: normal;font-style: normal;font-display: swap;}} @font-face {{font-family: 'Sultan Medium';src: url('{Directory.GetCurrentDirectory().Replace("\\", "/") + "/wwwroot/fonts/Sultan/arfonts-sultan-mudaim.ttf"}') format('truetype');font-weight: normal;font-style: normal;font-display: swap;}}</style>", TemplateContent);
				// create a new pdf document converting an url
				SelectPdf.PdfDocument pdfDocument = converter.ConvertHtmlString(TemplateContent);
                pdfDocument.Save(stream);
                pdfDocument.Close();
                stream.Close();
                return stream.ToArray();
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                return null;
            }
        }
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
    }
}
