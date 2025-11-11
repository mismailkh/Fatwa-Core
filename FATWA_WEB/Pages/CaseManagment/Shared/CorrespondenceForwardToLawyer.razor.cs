using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.CaseManagment.Shared
{
    public partial class CorrespondenceForwardToLawyer : ComponentBase
    {
        [Parameter]
        public dynamic CommunicationId { get; set; }
        public CommunicationHistory? communicationHistory { get; set; } = new();

        public string? CanNumber { get; set; }
        public string? CaseNumber { get; set; }
        public List<UserBasicDetailVM> basicDetail { get; set; } = new();
        public IEnumerable<string>? sentTo { get; set; }
        protected RadzenDropDown<IEnumerable<string>> ddlLawyer;
        public bool IsSearch = false;
        string validateMessage = "";
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            await  GetAllLawyersBySectorId();
            spinnerService.Hide();
        }
            public async void SubmitSearch()
        {
            var res = await cmsCaseFileService.GetLawyersByCaseAndCanNumber(CaseNumber, CanNumber);
            if (res.IsSuccessStatusCode)
            {
                basicDetail = (List<UserBasicDetailVM>)res.ResultData;
                ddlLawyer.Reset();

                IsSearch = true;
                if(IsSearch && basicDetail.Count()==0)
                {

                }
            }
        }

        public async Task GetAllLawyersBySectorId()
        {
            CaseNumber = "";
            CanNumber = "";
            var sectorId = (int)loginState.UserDetail.SectorTypeId;
            var res = await cmsCaseFileService.GetAllLawyersBySectorId(sectorId);
            if (res.IsSuccessStatusCode)
            {
                basicDetail = (List<UserBasicDetailVM>)res.ResultData;
                IsSearch = false;
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
                args.RecieversId = sentTo.ToList();
                args.SentBy = Guid.Parse(loginState.UserDetail.UserId);
                args.ReferenceId = Guid.Parse(CommunicationId);
                var response = await communicationService.ForwardCorrespondenceToLawyer(args);
                
                    if (response.IsSuccessStatusCode)
                    {
                        //lmsliteratureParentIndex = (LmsLiteratureParentIndex)response.ResultData;
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Correspondence_Forward_To_Lawyer_Done"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        StateHasChanged();
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                navigationManager.NavigateTo("/inboxOutbox-list");




            }


            catch (Exception ex) 
            {
                throw ex;
            }
        }
    }
}
