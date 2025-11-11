using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.CommunicationModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;

namespace FATWA_WEB.Pages.CaseManagment.Shared
{
    public partial class CorrespondenceForwardToSector : ComponentBase
    {
        [Parameter]
        public dynamic CommunicationId { get; set; }
        public CommunicationHistory? communicationHistory { get; set; } = new();

        public List<OperatingSectorType> Sectors { get; set; } = new();
        //protected RadzenDropDown<List<string>> ddlLawyer;
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await GetOperatingSectorTypes();
            spinnerService.Hide();
        }


        public async Task GetOperatingSectorTypes()
        {
            var sectorId = (int)loginState.UserDetail.SectorTypeId;
            var res = await lookupService.GetOperatingSectorTypes();
            if (res.IsSuccessStatusCode)
            {
                Sectors = (List<OperatingSectorType>)res.ResultData;
                Sectors =  Sectors.Where(x => x.Id != sectorId).ToList();
            }
        }

        protected async Task ButtonCancelClick(MouseEventArgs args)
        {
            dialogService.Close(null);
        }

        protected async Task FormSubmit(CommunicationHistory args)
        {
            try
            {
                args.SentBy = Guid.Parse(loginState.UserDetail.UserId);
                args.ReferenceId = Guid.Parse(CommunicationId);
                var response = await communicationService.ForwardCorrespondenceToSector(args);

                if (response.IsSuccessStatusCode)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Success,
                        Detail = translationState.Translate("Correspondence_Forward_To_Sector_Done"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                    StateHasChanged();
                    navigationManager.NavigateTo("/inboxOutbox-list");

                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                //dialogService.Close(lmsliteratureParentIndex);



            }


            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
