using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Telerik.Blazor.Components;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;

namespace FATWA_WEB.Pages.CaseManagment.MOJ
{
    //< History Author = 'Hassan Abbas' Date = '2022-11-21' Version = "1.0" Branch = "master" >Register Case to MOJ</History>
    public partial class RegisterToMOJ : ComponentBase
    {

        [Parameter]
        public string FileId { get; set; }
        [Parameter]
        public string MojRegistrationRequestId { get; set; }

        #region Variables

        protected RadzenDataGrid<CasePartyLinkVM> PartiesGrid;
        protected TelerikPdfViewer pdfViewer = new TelerikPdfViewer();
        protected List<CasePartyLinkVM> CasePartyLinks { get; set; }
        protected bool RefreshFileUploadGrid { get; set; } = true;

        #endregion

        #region Component Load

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Component Load</History>
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();

            await PopulateCasePartyGrid();

            spinnerService.Hide();
        }

        #endregion


        #region Populate Grids

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Populate Case Parties</History>
        protected async Task PopulateCasePartyGrid()
        {
            var partyResponse = await caseRequestService.GetCMSCasePartyDetailById(Guid.Parse(FileId));
            if (partyResponse.IsSuccessStatusCode)
            {
                CasePartyLinks = (List<CasePartyLinkVM>)partyResponse.ResultData;
                CasePartyLinks = CasePartyLinks?.Select(c => { c.CasePartyCategory = (CasePartyCategoryEnum)c.CategoryId; c.CasePartyType = (CasePartyTypeEnum)c.TypeId; return c; }).ToList();
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(partyResponse);
            }
        }

        //<History Author = 'Hassan Abbas' Date='2023-03-11' Version="1.0" Branch="master">Check if Attachments</History>
        public void PartyRowRender(RowRenderEventArgs<CasePartyLinkVM> args)
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

        #region  Actions

        //<History Author = 'Hassan Abbas' Date='2022-11-21' Version="1.0" Branch="master"> Upload Attachments recieved from MOJ and create Case</History>
        protected void UploadAttachments(MouseEventArgs args)
        {
            navigationState.ReturnUrl = "moj-registration-requests";
            navigationManager.NavigateTo("/create-registered-case/" + FileId + "/" + MojRegistrationRequestId);
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
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion

        #region view response
        protected async Task ViewResponse()
        {
            navigationManager.NavigateTo("/communication-detail/" + FileId);
        }
        #endregion
        #region Grid Button
        //<History Author = 'Ijaz Ahmad' Date='2024-02-07' Version="1.0" Branch="master"> Detail  Case Party Info</History>
        protected void DetailCaseParty(CasePartyLinkVM args)
        {
            navigationManager.NavigateTo("/caseparty-view/" + args.Id);
        }
        #endregion
    }
}
