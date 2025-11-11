using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;

namespace FATWA_WEB.Pages.CaseManagment.Shared
{
    public partial class AssignDecisionToLawyer : ComponentBase

    {
        #region Parameter
        [Parameter]
        public Guid DecisionId { get; set; }

        #endregion

        #region Variable declaration

        //public string PrimaryLaywerId { get; set; } 
        protected IEnumerable<LawyerVM> lawyers { get; set; }
        protected bool IsVisible { get; set; }
        protected bool showadvancegrid { get; set; }
        protected RadzenDataGrid<LawyerVM> grid = new RadzenDataGrid<LawyerVM>();
        protected bool allowRowSelectOnRowClick = true;
        protected CmsCaseDecisionAssignee cmsCaseDecisionAssignee = new CmsCaseDecisionAssignee();
        public ConsultationAssignment dmsCopyAttachmentFromSourceToDestination { get; set; } = new ConsultationAssignment();
        //public CmsCaseRequestLawyerVM cmsCaseRequestLawyerVM = new CmsCaseRequestLawyerVM();
        public string LawyerId { get; set; } = string.Empty;
        #endregion

        #region  OnInitialized
        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show();
            cmsCaseDecisionAssignee = new CmsCaseDecisionAssignee() { Id = Guid.NewGuid() };
            await PopulateLawyersList();
            spinnerService.Hide();
        }
        protected async Task PopulateLawyersList()
        {
            var userresponse = await lookupService.GetLawyersBySector(loginState.UserDetail.SectorTypeId);
            if (userresponse.IsSuccessStatusCode)
            {
                lawyers = (IEnumerable<LawyerVM>)userresponse.ResultData;
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(userresponse);
            }
        }
        #endregion

        #region Change type
        LawyerVM? selectedLawyer = null;
        protected async Task OnTypeChange(object args)
        {
            try
            {
                selectedLawyer = lawyers.FirstOrDefault(x => x.Id == (string)args);
                if (args != null)
                {
                    IsVisible = false;
                    cmsCaseDecisionAssignee.SelectedUsers = new List<LawyerVM>();
                   
                   
                }
                StateHasChanged();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Submit button click
        protected async Task FormSubmit(CmsCaseDecisionAssignee args)
        {
            try
            {
                bool? dialogResponse = await dialogService.Confirm(
                   translationState.Translate("Sure_Assign_Request"),
                   translationState.Translate("Confirm"),
                   new ConfirmOptions()
                   {
                       OkButtonText = @translationState.Translate("OK"),
                       CancelButtonText = @translationState.Translate("Cancel")
                   });

                if (dialogResponse == true)
                {
                    spinnerService.Show();
                    cmsCaseDecisionAssignee.DecisionId = DecisionId;
                    if(LawyerId != string.Empty)
                    {
                        cmsCaseDecisionAssignee.UserId = Guid.Parse(LawyerId);
                    }
                    var response = await cmsSharedService.AssignDecisionRequestToLawyer(cmsCaseDecisionAssignee);

                    
                    if (response.IsSuccessStatusCode)
                    {
                       notificationService.Notify(new NotificationMessage()
                          {
                             Severity = NotificationSeverity.Success,
                             Detail = translationState.Translate("Request_Assigned"),
                             Style = "position: fixed !important; left: 0; margin: auto;"
                           });                           
                           dialogService.Close(cmsCaseDecisionAssignee);

                        spinnerService.Hide();
                        Reload();
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }

                }
            }
            catch
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong")
                });
            }
        }
        #endregion

        #region cancel button

        protected async Task ButtonCloseDialog(MouseEventArgs args)
        {
            dialogService.Close(null);
        }

        #endregion

    }
}
