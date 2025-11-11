using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_WEB.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.CaseManagment.Shared
{
    public partial class CreateCaseFileTransferRequest : ComponentBase
    {
        #region Parameters
        [Parameter]
        public int SectorTypeId { get; set; }
        #endregion
        #region Variables
        protected IEnumerable<OperatingSectorType> SectorTypesList { get; set; } = new List<OperatingSectorType>();
        public CmsCaseFileTranferRequest cmsCaseFileTranferRequest { get; set; } = new CmsCaseFileTranferRequest();
        public string typeValidationMsgSector;
        #endregion
        #region On Load
        protected override async Task OnInitializedAsync()
        {
            await PopulateSectorTypes();
        }
        #endregion
        #region Dropdownlist and On Change Events
        protected async Task PopulateSectorTypes()
        {
            var response = await userService.GetEmployeeSectortype();
            if (response.IsSuccessStatusCode)
            {
                SectorTypesList = (IEnumerable<OperatingSectorType>)response.ResultData;
                SectorTypesList = SectorTypesList.Where(x => x.Id >= (int)OperatingSectorTypeEnum.AdministrativeUnderFilingCases && x.Id <= (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases);
            }
            StateHasChanged();
        }
        #endregion
        #region Functions
        public async Task FormSubmit()
        {
            if (cmsCaseFileTranferRequest.SectorTo > 0 && cmsCaseFileTranferRequest.Description != null)
            {
                bool? dialogResponse = await dialogService.Confirm(
                     translationState.Translate("Sure_Submit"),
                     translationState.Translate("Confirm"),
                     new ConfirmOptions()
                     {
                         OkButtonText = @translationState.Translate("OK"),
                         CancelButtonText = @translationState.Translate("Cancel")
                     });

                if (dialogResponse == true)
                {
                    spinnerService.Show();
                    cmsCaseFileTranferRequest.Id = Guid.NewGuid();
                    cmsCaseFileTranferRequest.StatusId = (int)CaseFileTransferRequestStatusEnum.Submitted;
                    cmsCaseFileTranferRequest.SectorFrom = (int)loginState.UserDetail.SectorTypeId;
                    cmsCaseFileTranferRequest.CreatedBy = loginState.Username;
                    cmsCaseFileTranferRequest.CreatedDate = DateTime.Now;
                    ApiCallResponse Assignresponse = await cmsSharedService.AddCaseFileTransferRequest(cmsCaseFileTranferRequest);
                    if (Assignresponse.IsSuccessStatusCode)
                    {
                        await Task.Delay(1500);
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Assign_Request_Initiated"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        dialogService.Close();
                        dialogService.Close(1);
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(Assignresponse);
                    }

                }
                spinnerService.Hide();
            }
            else
            {
                typeValidationMsgSector = cmsCaseFileTranferRequest.SectorTo > 0 ? "" : @translationState.Translate("Required_Field");
                typeValidationMsgSector = cmsCaseFileTranferRequest.SectorTo > 0 ? "" : @translationState.Translate("Required_Field");
            }
        }
        protected void ButtonCancelClick(MouseEventArgs args)
        {
            dialogService.Close(null);
        }
        #endregion
    }
}
