using FATWA_DOMAIN.Models.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;


namespace FATWA_ADMIN.Pages.LookupsManagment
{
    public partial class GovernmentEntityBankDetailAdd : ComponentBase
    { 
        private Action<CmsBankGovernmentEntity> onSave;
        #region Variable  
        protected List<CmsBank> CmsBanks { get; set; }= new List<CmsBank>(); 
        CmsBankGovernmentEntity CmsBankGovernmentEntity = new CmsBankGovernmentEntity(); 
        #endregion
        protected override async Task OnInitializedAsync()
        {
            spinnerService.Show(); 
            await GetBankNames();
            spinnerService.Hide();
        }
        protected async Task GetBankNames()
        {

            var response = await lookupService.GetBankNames();
            if (response.IsSuccessStatusCode)
            {
                CmsBanks = (List<CmsBank>)response.ResultData;
                
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        private async Task SaveChangesCallback(Action<CmsBankGovernmentEntity> onSave)
        { 
            await SaveChanges(onSave);
        }

        protected async Task SaveChanges(Action<CmsBankGovernmentEntity> onSave)
        {
            try
            {
                if (await dialogService.Confirm(translationState.Translate("Sure_Submit"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {
                    var bankId = CmsBankGovernmentEntity.BankId;
                    var response = await lookupService.GetBankNameById(bankId);
                    if (response.IsSuccessStatusCode)
                    {
                        var bankName = (CmsBank)response.ResultData;
                        CmsBankGovernmentEntity.BankNameEn = bankName.Name_En;
                        CmsBankGovernmentEntity.BankNameAr = bankName.Name_Ar;
                    }
                    dialogService.Close(CmsBankGovernmentEntity);
                     
                }
            }
            catch (Exception ex)
            {
                spinnerService.Hide();
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
         
        protected async Task Button2Click(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
    }
}
