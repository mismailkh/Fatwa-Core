using FATWA_DOMAIN.Models.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.CaseManagment.Shared
{
    public partial class AssignBackToHosCaseFile : ComponentBase
    {
        #region Parameters

        [Parameter]
        public Guid ReferenceId { get; set; }
        [Parameter]
        public string TaskId { get; set; }
        #endregion

        #region Variables
        protected string typeValidationMsgSector = "";
        protected string typeValidationMsgReason = "";
        protected List<OperatingSectorType> SectorTypes { get; set; } = new List<OperatingSectorType>();
        public CmsAssignCaseFileBackToHos cmsAssignBackToHos { get; set; } =new CmsAssignCaseFileBackToHos();

        #endregion

        #region On Load

        protected override async Task OnInitializedAsync()
        {
           
           
        }
        #endregion

        #region Functions

        public async Task FormSubmit()
        {
            if (cmsAssignBackToHos.Remarks != null)
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
                    ApiCallResponse response = null;
                    cmsAssignBackToHos.SectorTypeId = (int)loginState.UserDetail.SectorTypeId;
                    cmsAssignBackToHos.ReferenceId = ReferenceId;
                    cmsAssignBackToHos.TaskUserId = loginState.UserDetail.UserId;
                    cmsAssignBackToHos.TaskId = Guid.Parse(TaskId);

                    response = await cmsSharedService.AssignBackToHos(cmsAssignBackToHos);

                    if (response.IsSuccessStatusCode)
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = translationState.Translate("Case_File_Assign_To_Hos"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        dialogService.Close();
                        dialogService.Close(1);
                    }
                    else
                    {
                        await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                    }
                    spinnerService.Hide();
                }
            }
            else
            {
                typeValidationMsgReason = cmsAssignBackToHos.Remarks != null ? "" : @translationState.Translate("Required_Field_Reason");
            }
        }

        #region Dropdownlist and On Change Events

        protected async Task PopulateSectorTypes()
        {

            var response = await lookupService.GetOperatingSectorTypes();
            if (response.IsSuccessStatusCode)
            {
                SectorTypes = (List<OperatingSectorType>)response.ResultData;
               
                    SegregateSectorTypeListByUsingSectorId(SectorTypes);
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        private void SegregateSectorTypeListByUsingSectorId(List<OperatingSectorType> getOperatingStatusType)
        {
            try
            {
                if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeUnderFilingCases ||
                    loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeRegionalCases ||
                    loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeAppealCases ||
                    loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeSupremeCases)
                {
                    SectorTypes = getOperatingStatusType.Where(x => (x.Id >= (int)OperatingSectorTypeEnum.AdministrativeUnderFilingCases &&
                                                                       x.Id <= (int)OperatingSectorTypeEnum.AdministrativeSupremeCases) ||
                                                                      x.Id == (int)OperatingSectorTypeEnum.Execution).ToList();

                }
                else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialUnderFilingCases ||
                        loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialRegionalCases ||
                        loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialAppealCases ||
                        loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialSupremeCases ||
                        loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases)
                {
                    SectorTypes = getOperatingStatusType.Where(x => (x.Id >= (int)OperatingSectorTypeEnum.CivilCommercialUnderFilingCases &&
                                                               x.Id <= (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases) ||
                                                               x.Id == (int)OperatingSectorTypeEnum.Execution).ToList();
                }
                else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.Execution)
                {
                    SectorTypes = getOperatingStatusType.Where(x => (x.Id >= (int)OperatingSectorTypeEnum.AdministrativeUnderFilingCases &&
                                                               x.Id <= (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion
        protected void ButtonCancelClick(MouseEventArgs args)
        {
            dialogService.Close(null);
        }

        #endregion

    }
}
