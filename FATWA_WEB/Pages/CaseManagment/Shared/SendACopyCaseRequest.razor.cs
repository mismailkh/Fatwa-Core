using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.CaseManagment.Shared
{
    //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master">Create a new copy of the Case Request and Send it to another Sector </History>
    public partial class SendACopyCaseRequest : ComponentBase
    {
        #region Parameters

        [Parameter]
        public Guid ReferenceId { get; set; }
        [Parameter]
        public dynamic SendACopyType { get; set; }

        #endregion

        #region Variables

        protected List<OperatingSectorType> SectorTypes { get; set; } = new List<OperatingSectorType>();
        public CmsApprovalTracking approvalTracking { get; set; } = new CmsApprovalTracking { Id = Guid.NewGuid(), StatusId = (int)ApprovalStatusEnum.Pending, CreatedDate = DateTime.Now };
        public CaseRequest caseRequest { get; set; } = new CaseRequest();
        protected string typeValidationMsgSector = "";
        protected string typeValidationMsgReason = "";

        #endregion

        #region Component Load

        protected override async Task OnInitializedAsync()
        {
            await PopulateSectorTypes();
        }

        #endregion

        #region Populate Dropdown Events

        //<History Author = 'Hassan Abbas' Date='2022-10-21' Version="1.0" Branch="master"> Populate Sector Types </History>
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
                if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeUnderFilingCases)
                {
                    SectorTypes = getOperatingStatusType.Where(x => (x.Id == (int)OperatingSectorTypeEnum.CivilCommercialUnderFilingCases)).ToList();
                }
                else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialUnderFilingCases)
                {
                    SectorTypes = getOperatingStatusType.Where(x => (x.Id == (int)OperatingSectorTypeEnum.AdministrativeUnderFilingCases)).ToList();
                }
                if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialSupremeCases)
                {
                    SectorTypes = getOperatingStatusType.Where(x => (x.Id == (int)OperatingSectorTypeEnum.AdministrativeSupremeCases)).ToList();
                }
                else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeSupremeCases)
                {
                    SectorTypes = getOperatingStatusType.Where(x => (x.Id == (int)OperatingSectorTypeEnum.CivilCommercialSupremeCases)).ToList();
                }
                if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeAppealCases)
                {
                    SectorTypes = getOperatingStatusType.Where(x => (x.Id == (int)OperatingSectorTypeEnum.CivilCommercialAppealCases)).ToList();
                }
                else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialAppealCases)
                {
                    SectorTypes = getOperatingStatusType.Where(x => (x.Id == (int)OperatingSectorTypeEnum.AdministrativeAppealCases)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Button Events
        public async void FormSubmit()
        {
            if (approvalTracking.SectorTo > 0 && !String.IsNullOrEmpty(approvalTracking.Remarks))
            {
                if (await dialogService.Confirm(translationState.Translate("Sure_Send_Copy"), translationState.Translate("Confirm"), new ConfirmOptions()
                {
                    OkButtonText = translationState.Translate("OK"),
                    CancelButtonText = translationState.Translate("Cancel")
                }) == true)
                {

                    ApiCallResponse response = null;
                    approvalTracking.SectorFrom = (int)loginState.UserDetail.SectorTypeId;
                    approvalTracking.ReferenceId = ReferenceId;
                    approvalTracking.CreatedBy = loginState.Username;
                    approvalTracking.ProcessTypeId = (int)ApprovalProcessTypeEnum.SendaCopy;
                    approvalTracking.TransferCaseType = SendACopyType;
                    approvalTracking.SectorTypeId = loginState.UserDetail.SectorTypeId;
                    approvalTracking.AssignedBy = loginState.UserDetail.UserId;
                    approvalTracking.UserName = loginState.Username;
                    spinnerService.Show();
                    var activeWorkflow = await GetActiveCopyWorkflow();
                    if (activeWorkflow != null)
                    {
                        response = await cmsSharedService.AddSendACopyTask(approvalTracking, SendACopyType);

                        if (response.IsSuccessStatusCode)
                        {
                            await workflowService.AssignWorkflowActivity(activeWorkflow, approvalTracking, (int)WorkflowModuleEnum.CaseManagement, activeWorkflow.ModuleTriggerId, null);
                            await Task.Delay(1500);
                            notificationService.Notify(new NotificationMessage()
                            {
                                Severity = NotificationSeverity.Success,
                                Detail = translationState.Translate("Copy_Sent"),
                                Style = "position: fixed !important; left: 0; margin: auto; "
                            });
                            dialogService.Close();
                            dialogService.Close(1);
                        }
                        else
                        {
                            await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                        }
                    }
                    spinnerService.Hide();
                }
            }
            else
            {
                typeValidationMsgSector = approvalTracking.SectorTo > 0 ? "" : @translationState.Translate("Required_Field");
                typeValidationMsgReason = !String.IsNullOrEmpty(approvalTracking.Remarks) ? "" : @translationState.Translate("Required_Field");
            }



        }
        protected void ButtonCancelClick(MouseEventArgs args)
        {
            dialogService.Close(null);
        }

        #endregion

        #region Check Active Workflow
        protected async Task<WorkflowVM> GetActiveCopyWorkflow()
        {
            
            WorkflowVM activeWorkflow = null;   
            int triggerId = 0;
            int submoduleId = 0;
            triggerId = SendACopyType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? (int)WorkflowModuleTriggerEnum.SendCopyCaseRequest : (int)WorkflowModuleTriggerEnum.SendCopyCaseFile;
            if (loginState.UserDetail.SectorTypeId >= (int)OperatingSectorTypeEnum.AdministrativeUnderFilingCases && loginState.UserDetail.SectorTypeId <= (int)OperatingSectorTypeEnum.AdministrativeSupremeCases)
            {
                submoduleId = (int)RequestTypeEnum.Administrative;
            }
            else if (loginState.UserDetail.SectorTypeId >= (int)OperatingSectorTypeEnum.CivilCommercialUnderFilingCases && loginState.UserDetail.SectorTypeId <= (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases)
            {
                submoduleId = (int)RequestTypeEnum.CivilCommercial;
            }
            var response = await workflowService.GetActiveWorkflows((int)WorkflowModuleEnum.CaseManagement, triggerId, null, submoduleId);
            if (response.IsSuccessStatusCode)
            {
                var activeworkflowlist = (List<WorkflowVM>)response.ResultData;
                if(activeworkflowlist.Count > 0)
                {
                    activeWorkflow = activeworkflowlist.FirstOrDefault();
                }
                else
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("No_Active_Workflow"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
                

            }
            return activeWorkflow;
        }
        #endregion
    }
}

