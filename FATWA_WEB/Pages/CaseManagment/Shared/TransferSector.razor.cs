using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Newtonsoft.Json;
using Radzen;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.CaseManagment.Shared
{
    public partial class TransferSector : ComponentBase
    {
        #region Parameters

        [Parameter]
        public Guid ReferenceId { get; set; }

        [Parameter]
        public dynamic TransferCaseType { get; set; }

        [Parameter]
        public bool IsAssignment { get; set; }
        [Parameter]
        public bool IsConfidential { get; set; }
        [Parameter]
        public int RequestTypeId { get; set; }
        [Parameter]
        public List<int> RejectedTransferIds { get; set; }
        [Parameter]
        public int? SenderSector { get; set; }
        [Parameter]
        public bool? IsViewOnly { get; set; } = false;
        #endregion
        
        #region Variables

        protected List<OperatingSectorType> SectorTypes { get; set; } = new List<OperatingSectorType>();
        public CaseRequest caseRequest { get; set; } = new CaseRequest();
        public CmsApprovalTracking approvalTracking { get; set; } = new CmsApprovalTracking { StatusId = (int)ApprovalStatusEnum.Pending, CreatedDate = DateTime.Now, Id = Guid.NewGuid() };
        protected string typeValidationMsgSector = "";
        protected string typeValidationMsgReason = "";
        public RegisteredCaseFileVM selectedFile { get; set; } = new RegisteredCaseFileVM();
        public CaseRequestDetailVM caseRequestdetail { get; set; } = new CaseRequestDetailVM();
        #endregion

        #region On Load

        protected override async Task OnInitializedAsync()
        {
            if (IsAssignment == true)
            {
                await PopulateSectorTypesForAssign();
            }
            else
            {
                await PopulateSectorTypes();
            }
        }
        #endregion

        #region Dropdownlist and On Change Events

        protected async Task PopulateSectorTypesForAssign()
        {


            var response = await lookupService.GetOperatingSectorTypes();
            if (response.IsSuccessStatusCode)
            {
                SectorTypes = (List<OperatingSectorType>)response.ResultData;
                CalculateSectorTypesForAssignment();

            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }
        protected async Task PopulateSectorTypes()
        {
            var activeWorkflow = await GetActiveTransferWorkflow();
            if (activeWorkflow != null)
            {
                var response = await workflowService.GetWorkflowSectorTransferOptions((int)activeWorkflow.WorkflowTriggerId, (int)loginState.UserDetail.SectorTypeId);
                if (response.IsSuccessStatusCode)
                {
                    SectorTypes = (List<OperatingSectorType>)response.ResultData;
                    if(!SectorTypes.Any())
                    {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Error,
                            Detail = translationState.Translate("No_Transfer_Options_Defined"),
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                    }
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
            }
            StateHasChanged();
        }
        public async Task PopulateCaseRequestGrid(Guid RequestId)
        {
            var caseRequestResponse = await caseRequestService.GetCaseRequestDetailById(RequestId);
            if (caseRequestResponse.IsSuccessStatusCode)
            {
                caseRequestdetail = JsonConvert.DeserializeObject<CaseRequestDetailVM>(caseRequestResponse.ResultData.ToString());
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(caseRequestResponse);
            }
        }
        private void CalculateSectorTypesForTransfer()
        {
            var copyOfSectorTypes = new List<OperatingSectorType>(SectorTypes);
            if (RequestTypeId == (int)RequestTypeEnum.Administrative)
            {
                SectorTypes = SectorTypes.Where(x =>
                    (x.Id == (int)OperatingSectorTypeEnum.AdministrativeUnderFilingCases
                    || x.Id == (int)OperatingSectorTypeEnum.AdministrativeAppealCases
                    || x.Id == (int)OperatingSectorTypeEnum.AdministrativeSupremeCases
                    || x.Id == (int)OperatingSectorTypeEnum.CivilCommercialUnderFilingCases
                    || x.Id == (int)OperatingSectorTypeEnum.Execution)).ToList();
            }
            else if (RequestTypeId == (int)RequestTypeEnum.CivilCommercial)
            {
                SectorTypes = SectorTypes.Where(x =>
                    (x.Id == (int)OperatingSectorTypeEnum.CivilCommercialUnderFilingCases
                    || x.Id == (int)OperatingSectorTypeEnum.CivilCommercialAppealCases
                    || x.Id == (int)OperatingSectorTypeEnum.CivilCommercialSupremeCases
                    || x.Id == (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases
                    || x.Id == (int)OperatingSectorTypeEnum.AdministrativeUnderFilingCases)
                    || x.Id == (int)OperatingSectorTypeEnum.Execution).ToList();
            }


            if (IsConfidential)
            {
                SectorTypes.Add(copyOfSectorTypes.Where(x => x.Id == (int)OperatingSectorTypeEnum.PrivateOperationalSector).FirstOrDefault());
            }
            if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.PrivateOperationalSector)
            {
                SectorTypes.Add(copyOfSectorTypes.Where(x => x.Id == (int)OperatingSectorTypeEnum.PublicOperationalSector).FirstOrDefault());
                if (RequestTypeId == (int)RequestTypeEnum.Administrative)
                {
                    SectorTypes.Remove(copyOfSectorTypes.Where(x => x.Id == (int)OperatingSectorTypeEnum.CivilCommercialUnderFilingCases).FirstOrDefault());
                }
                else if (RequestTypeId == (int)RequestTypeEnum.CivilCommercial)
                {
                    SectorTypes.Remove(copyOfSectorTypes.Where(x => x.Id == (int)OperatingSectorTypeEnum.AdministrativeUnderFilingCases).FirstOrDefault());
                }
                SectorTypes.Remove(copyOfSectorTypes.Where(x => x.Id == (int)OperatingSectorTypeEnum.Execution).FirstOrDefault());
            }
            else if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeGeneralSupervisor
                || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialGeneralSupervisor)
            {
                SectorTypes = copyOfSectorTypes.Where(x =>
                            x.Id == (int)OperatingSectorTypeEnum.PublicOperationalSector).ToList();

            }
            //if(TransferCaseType = )
            //if(loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.PublicOperationalSector)
            //{
            //    SectorTypes.Remove(copyOfSectorTypes.Where(x => x.Id == ).FirstOrDefault());
            //}
            if (RejectedTransferIds.Any())
            {
                if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.PublicOperationalSector
                    || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeGeneralSupervisor
                    || loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.CivilCommercialGeneralSupervisor)
                {
                    SectorTypes.AddRange(copyOfSectorTypes.Where(x => RejectedTransferIds.Any(y => y == x.Id)).ToList().Except(SectorTypes));
                }
                else
                {
                    SectorTypes = SectorTypes.Where(x => !RejectedTransferIds.Any(y => y == x.Id)).ToList();
                }
                if (loginState.UserDetail.SectorTypeId != (int)OperatingSectorTypeEnum.PublicOperationalSector
                    && loginState.UserDetail.SectorTypeId != (int)OperatingSectorTypeEnum.FatwaPresidentOffice
                    && loginState.UserDetail.SectorTypeId != (int)OperatingSectorTypeEnum.PrivateOperationalSector)
                    if (RequestTypeId == (int)RequestTypeEnum.Administrative)
                        SectorTypes.Add(copyOfSectorTypes.Where(x => x.Id == (int)OperatingSectorTypeEnum.AdministrativeGeneralSupervisor).FirstOrDefault());
                    else if (RequestTypeId == (int)RequestTypeEnum.CivilCommercial)
                        SectorTypes.Add(copyOfSectorTypes.Where(x => x.Id == (int)OperatingSectorTypeEnum.CivilCommercialGeneralSupervisor).FirstOrDefault());
            }
            if (SenderSector != null && loginState.UserDetail.SectorTypeId != (int)OperatingSectorTypeEnum.PrivateOperationalSector)
                SectorTypes.Remove(copyOfSectorTypes.Where(x => x.Id == SenderSector).FirstOrDefault());
        }

        private void CalculateSectorTypesForAssignment()
        {
            switch (loginState.UserDetail.SectorTypeId)
            {
                case (int)OperatingSectorTypeEnum.AdministrativeUnderFilingCases:
                    SectorTypes = SectorTypes.Where(x =>
                        (x.Id == (int)OperatingSectorTypeEnum.AdministrativeRegionalCases)).ToList();
                    break;
                case (int)OperatingSectorTypeEnum.AdministrativeRegionalCases:
                    SectorTypes = SectorTypes.Where(x =>
                        (x.Id == (int)OperatingSectorTypeEnum.AdministrativeUnderFilingCases
                        || x.Id == (int)OperatingSectorTypeEnum.AdministrativeAppealCases
                        || x.Id == (int)OperatingSectorTypeEnum.AdministrativeSupremeCases)).ToList();
                    break;
                case (int)OperatingSectorTypeEnum.AdministrativeAppealCases:
                    SectorTypes = SectorTypes.Where(x =>
                        (x.Id == (int)OperatingSectorTypeEnum.AdministrativeRegionalCases
                        || x.Id == (int)OperatingSectorTypeEnum.AdministrativeSupremeCases)).ToList();
                    break;
                case (int)OperatingSectorTypeEnum.AdministrativeSupremeCases:
                    SectorTypes = SectorTypes.Where(x =>
                        (x.Id == (int)OperatingSectorTypeEnum.AdministrativeRegionalCases
                        || x.Id == (int)OperatingSectorTypeEnum.AdministrativeAppealCases)).ToList();
                    break;

                case (int)OperatingSectorTypeEnum.CivilCommercialUnderFilingCases:
                    SectorTypes = SectorTypes.Where(x =>
                        (x.Id == (int)OperatingSectorTypeEnum.CivilCommercialRegionalCases)).ToList();
                    break;
                case (int)OperatingSectorTypeEnum.CivilCommercialRegionalCases:
                    SectorTypes = SectorTypes.Where(x =>
                        (x.Id == (int)OperatingSectorTypeEnum.CivilCommercialUnderFilingCases
                        || x.Id == (int)OperatingSectorTypeEnum.CivilCommercialAppealCases
                        || x.Id == (int)OperatingSectorTypeEnum.CivilCommercialSupremeCases)).ToList();
                    break;
                case (int)OperatingSectorTypeEnum.CivilCommercialAppealCases:
                    SectorTypes = SectorTypes.Where(x =>
                        (x.Id == (int)OperatingSectorTypeEnum.CivilCommercialRegionalCases
                        || x.Id == (int)OperatingSectorTypeEnum.CivilCommercialSupremeCases)).ToList();
                    break;
                case (int)OperatingSectorTypeEnum.CivilCommercialSupremeCases:
                    SectorTypes = SectorTypes.Where(x =>
                        (x.Id == (int)OperatingSectorTypeEnum.CivilCommercialRegionalCases
                        || x.Id == (int)OperatingSectorTypeEnum.CivilCommercialAppealCases)).ToList();
                    break;
            }
        }
        #endregion


        #region Functions

        public async Task FormSubmit()
        {
            if (approvalTracking.SectorTo > 0 && approvalTracking.Remarks != null && (IsViewOnly != true || selectedFile.FileId != Guid.Empty))
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
                    if (IsViewOnly == true && selectedFile.FileId != Guid.Empty)
                    {
                        await PopulateCaseRequestGrid(selectedFile.RequestId);
                        ReferenceId = selectedFile.FileId;
                        IsConfidential = caseRequestdetail.IsConfidential;
                        RequestTypeId = (int)caseRequestdetail.RequestTypeId;
                    }
                    approvalTracking.SectorFrom = (int)loginState.UserDetail.SectorTypeId;
                    approvalTracking.ReferenceId = ReferenceId;
                    approvalTracking.CreatedBy = loginState.Username;
                    approvalTracking.ProcessTypeId = (bool)IsAssignment ? (int)ApprovalProcessTypeEnum.FileAssignment : (int)ApprovalProcessTypeEnum.Transfer;
                    approvalTracking.TransferCaseType = TransferCaseType;
                    approvalTracking.SectorTypeId = loginState.UserDetail.SectorTypeId;
                    approvalTracking.AssignedBy = loginState.UserDetail.UserId;
                    approvalTracking.IsConfidential = IsConfidential;
                    if (IsAssignment == false)
                    {
                        var activeWorkflow = await GetActiveTransferWorkflow();
                        if (activeWorkflow != null)
                        {
                            ApiCallResponse response = await cmsSharedService.AddTransferSectorTask(approvalTracking, TransferCaseType);

                            if (response.IsSuccessStatusCode)
                            {
                                await workflowService.AssignWorkflowActivity(activeWorkflow, approvalTracking, (int)WorkflowModuleEnum.CaseManagement, activeWorkflow.ModuleTriggerId, null);
                                await Task.Delay(1500);
                                notificationService.Notify(new NotificationMessage()
                                {
                                    Severity = NotificationSeverity.Success,
                                    Detail = translationState.Translate("Transfer_Request_Initiated"),
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
                    }
                    else
                    {
                        ApiCallResponse Assignresponse = await cmsSharedService.AddAssignSectorTask(approvalTracking, TransferCaseType);
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
            }
            else
            {
                if (selectedFile.FileId == Guid.Empty)
                {
                    notificationService.Notify(new NotificationMessage()
                    {
                        Severity = NotificationSeverity.Error,
                        Detail = translationState.Translate("No_File_Selected_To_Transfer"),
                        Style = "position: fixed !important; left: 0; margin: auto; "
                    });
                }
                typeValidationMsgSector = approvalTracking.SectorTo > 0 ? "" : @translationState.Translate("Required_Field");
                typeValidationMsgReason = approvalTracking.Remarks != null ? "" : @translationState.Translate("Required_Field_Reason");
            }
        }


        protected void ButtonCancelClick(MouseEventArgs args)
        {
            dialogService.Close(null);
        }

        #endregion

        #region Check Active Workflow
        protected async Task<WorkflowVM> GetActiveTransferWorkflow()
        {
            WorkflowVM activeWorkflow = null;
            int triggerId = 0;
            if (IsConfidential && loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.PrivateOperationalSector && TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest)
            {
                triggerId = (int)WorkflowModuleTriggerEnum.TransferConfidentialCaseRequestPrivateOffice;
                if ((int)RequestTypeId == (int)RequestTypeEnum.Administrative)
                {
                    approvalTracking.SubModuleId = (int)WorkflowSubModuleEnum.Administrative;
                }
                else if ((int)RequestTypeId == (int)RequestTypeEnum.CivilCommercial)
                {
                    approvalTracking.SubModuleId = (int)WorkflowSubModuleEnum.CivilCommercial;
                }
            }
            else
            {
                if (IsConfidential)
                {
                    triggerId = TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? (int)WorkflowModuleTriggerEnum.TransferConfidentialCaseRequest : (int)WorkflowModuleTriggerEnum.TransferConfidentialCaseFile;
                }
                else
                {
                    triggerId = TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? (int)WorkflowModuleTriggerEnum.TransferCaseRequest : (int)WorkflowModuleTriggerEnum.TransferCaseFile;
                }
                if (loginState.UserDetail.SectorTypeId >= (int)OperatingSectorTypeEnum.AdministrativeUnderFilingCases && loginState.UserDetail.SectorTypeId <= (int)OperatingSectorTypeEnum.AdministrativeSupremeCases)
                {
                    approvalTracking.SubModuleId = (int)WorkflowSubModuleEnum.Administrative;
                }
                else if (loginState.UserDetail.SectorTypeId >= (int)OperatingSectorTypeEnum.CivilCommercialUnderFilingCases && loginState.UserDetail.SectorTypeId <= (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases)
                {
                    approvalTracking.SubModuleId = (int)WorkflowSubModuleEnum.CivilCommercial;
                }
            }
            var response = await workflowService.GetActiveWorkflows((int)WorkflowModuleEnum.CaseManagement, triggerId, null, approvalTracking.SubModuleId);
            if(response.IsSuccessStatusCode)
            {
                var activeworkflowlist = (List<WorkflowVM>)response.ResultData;
                if (activeworkflowlist?.Count() > 0)
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
            else
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("No_Active_Workflow"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
            return activeWorkflow;
        }
        #endregion

    }

}
