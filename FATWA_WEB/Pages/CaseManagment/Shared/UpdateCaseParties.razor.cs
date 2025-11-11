using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;

namespace FATWA_WEB.Pages.CaseManagment.Shared
{
    //<History Author = 'Hassan Abbas' Date='2022-12-10' Version="1.0" Branch="master"> Update Case Parties</History>
    public partial class UpdateCaseParties : ComponentBase
    {
        #region Parameters

        [Parameter]
        public Guid ReferenceId { get; set; }

        #endregion

        #region Variables

        protected RadzenDataGrid<CasePartyLinkVM> PartiesGrid;
        protected List<CasePartyLinkVM> CasePartyLinks;
        public bool allowRowSelectOnRowClick = true;

        #endregion

        #region Component Load
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await PopulatePartiesGrid();
            spinnerService.Hide();
        }

        #endregion

        #region Grid Events

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Populate Case Parties</History>
        protected async Task PopulatePartiesGrid()
        {
            var partyResponse = await caseRequestService.GetCMSCasePartyDetailById(ReferenceId);
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

        #region Button Evetns

        //<History Author = 'Hassan Abbas' Date='2022-11-30' Version="1.0" Branch="master"> Close Dialog</History>
        protected async Task CloseDialog(MouseEventArgs args)
        {
            dialogService.Close(null);
        }

        #endregion

        #region Add Party, Delete Party

        //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Open Add Party dialog</History>
        protected async Task AddParty(int categoryId)
        {
            try
            {
                var result = await dialogService.OpenAsync<AddCaseParty>(translationState.Translate("Add_Case_Party"),
                    new Dictionary<string, object>()
                    {
                        { "CategoryId", categoryId },
                        { "ReferenceId", ReferenceId.ToString() },
                    },
                    new DialogOptions() { Width = "40% !important", CloseDialogOnOverlayClick = true }
                );
                var party = (CasePartyLinkVM)result;
                if (party != null)
                {
                    await PopulatePartiesGrid();
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {

            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-11-20' Version="1.0" Branch="master"> Delete Party</History>
        protected async Task DeleteParty(CasePartyLinkVM party)
        {
            if (await dialogService.Confirm(translationState.Translate("Sure_Delete"), translationState.Translate("Confirm"), new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("Cancel")
            }) == true)
            {
                spinnerService.Show();
                var response = await cmsCaseFileService.DeleteCaseParty(party);
                if (response.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Party_Deleted_Successfully"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    await PopulatePartiesGrid();
                    StateHasChanged();
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                spinnerService.Hide();
            }
        }

        #endregion

        #region Grid Button
        //<History Author = 'Ijaz Ahmad' Date='2024-02-07' Version="1.0" Branch="master">Detail  Case Party Info</History>
        protected void DetailCaseParty(CasePartyLinkVM args)
        {
            navigationManager.NavigateTo("/caseparty-view/" + args.Id);
        }
        #endregion
    }
}
