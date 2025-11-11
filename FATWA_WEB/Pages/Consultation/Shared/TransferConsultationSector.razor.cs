using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.ViewModel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_GENERAL.Helper.Response;

namespace FATWA_WEB.Pages.Consultation.Shared
{
    public partial class TransferConsultationSector : ComponentBase
    {
        #region Parameters

        [Parameter]
        public Guid ReferenceId { get; set; }

        [Parameter]
        public dynamic TransferConsultationType { get; set; }

        [Parameter]
        public bool IsAssignment { get; set; }
        [Parameter]
        public dynamic SectorTypeId { get; set; }
        [Parameter]
        public dynamic RequestTypeId { get; set; }

        [Parameter]
        public bool IsConfidential { get; set; }
        #endregion

        #region Variables

        protected List<OperatingSectorType> SectorTypes { get; set; } = new List<OperatingSectorType>();
        public ConsultationRequest consultationRequest { get; set; } = new ConsultationRequest();
        public CmsApprovalTracking approvalTracking { get; set; } = new CmsApprovalTracking { StatusId = (int)ApprovalStatusEnum.Pending, CreatedDate = DateTime.Now, Id = Guid.NewGuid() };
        protected string typeValidationMsgSector = "";
        protected string typeValidationMsgReason = "";

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
            var activeWorkflow = await GetActiveTransferWorkflow();
            if (activeWorkflow != null)
            {
                var response = await workflowService.GetWorkflowSectorTransferOptions((int)activeWorkflow.WorkflowTriggerId, (int)loginState.UserDetail.SectorTypeId);
                if (response.IsSuccessStatusCode)
                {
                    SectorTypes = (List<OperatingSectorType>)response.ResultData;
                    if (!SectorTypes.Any())
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
        }

        protected IEnumerable<OperatingSectorType> getExcludedConsultationTransferSectorType = new List<OperatingSectorType>();

        private void ChangeApprovalTypeForAddingSector(IEnumerable<OperatingSectorType> getOperatingStatusType)
        {
            try
            {
                List<OperatingSectorType> approvalTypes = new List<OperatingSectorType>();
                foreach (var item in getOperatingStatusType)
                {
                    if (item.Id == (int)OperatingSectorTypeEnum.Contracts)
                    {
                        if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                        {
                            item.Name_En = translationState.Translate("Contracts");
                            approvalTypes.Add(item);
                        }
                        else
                        {
                            item.Name_Ar = translationState.Translate("Contracts");
                            approvalTypes.Add(item);
                        }
                    }
                    if (item.Id == (int)OperatingSectorTypeEnum.InternationalArbitration)
                    {
                        if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                        {
                            item.Name_En = translationState.Translate("International_Arbitration");
                            approvalTypes.Add(item);
                        }
                        else
                        {
                            item.Name_Ar = translationState.Translate("International_Arbitration");
                            approvalTypes.Add(item);
                        }
                    }
                    if (item.Id == (int)OperatingSectorTypeEnum.AdministrativeComplaints)
                    {
                        if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                        {
                            item.Name_En = translationState.Translate("Administrative_Complaints");
                            approvalTypes.Add(item);
                        }
                        else
                        {
                            item.Name_Ar = translationState.Translate("Administrative_Complaints");
                            approvalTypes.Add(item);
                        }
                    }
                    if (item.Id == (int)OperatingSectorTypeEnum.LegalAdvice)
                    {
                        if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                        {
                            item.Name_En = translationState.Translate("Legal_Advice");
                            approvalTypes.Add(item);
                        }
                        else
                        {
                            item.Name_Ar = translationState.Translate("Legal_Advice");
                            approvalTypes.Add(item);
                        }
                    }
                    if (item.Id == (int)OperatingSectorTypeEnum.Legislations)
                    {
                        if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                        {
                            item.Name_En = translationState.Translate("Legislations");
                            approvalTypes.Add(item);
                        }
                        else
                        {
                            item.Name_Ar = translationState.Translate("Legislations");
                            approvalTypes.Add(item);
                        }
                    }
                    if (item.Id == (int)OperatingSectorTypeEnum.PrivateOperationalSector)
                    {
                        if (Thread.CurrentThread.CurrentUICulture.Name == "en-US")
                        {
                            item.Name_En = translationState.Translate("Private_Operational_Sector");
                            approvalTypes.Add(item);
                        }
                        else
                        {
                            item.Name_Ar = translationState.Translate("Private_Operational_Sector");
                            approvalTypes.Add(item);
                        }
                    }



                }
                getExcludedConsultationTransferSectorType = approvalTypes;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        #endregion

        #region Functions

        public async Task FormSubmit()
        {
            if (approvalTracking.SectorTo > 0 && approvalTracking.Remarks != null)
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
                    ApiCallResponse response = null;
                    approvalTracking.SectorFrom = (int)loginState.UserDetail.SectorTypeId;
                    approvalTracking.ReferenceId = ReferenceId;
                    approvalTracking.CreatedBy = loginState.Username;
                    approvalTracking.ProcessTypeId = (bool)IsAssignment ? (int)ApprovalProcessTypeEnum.FileAssignment : (int)ApprovalProcessTypeEnum.Transfer;
                    approvalTracking.TransferCaseType = TransferConsultationType;
                    approvalTracking.SectorTypeId = loginState.UserDetail.SectorTypeId;
                    approvalTracking.AssignedBy = loginState.UserDetail.UserId;
                    approvalTracking.ComsSectorTypeId = Convert.ToInt32(SectorTypeId);
                    approvalTracking.IsConfidential = IsConfidential;
                    var activeWorkflow = await GetActiveTransferWorkflow();
                    if (activeWorkflow != null)
                    {
                        response = await comsSharedService.AddTransferComsSectorTask(approvalTracking, TransferConsultationType, Convert.ToInt32(SectorTypeId));

                        if (response.IsSuccessStatusCode)
                        {
                            await workflowService.AssignWorkflowActivity(activeWorkflow, approvalTracking, (int)WorkflowModuleEnum.COMSConsultationManagement, activeWorkflow.ModuleTriggerId, null);
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
            }
            else
            {
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
            if (IsConfidential && loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.PrivateOperationalSector && TransferConsultationType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest)
            {
                triggerId = (int)WorkflowModuleTriggerEnum.TransferConfidentialConsultationRequestPrivateOffice;

                if ((int)RequestTypeId == (int)RequestTypeEnum.LegalAdvice)
                {
                    approvalTracking.SubModuleId = (int)WorkflowSubModuleEnum.LegalAdvice;
                }
                if ((int)RequestTypeId == (int)RequestTypeEnum.Legislations)
                {
                    approvalTracking.SubModuleId = (int)WorkflowSubModuleEnum.Legislations;
                }
                if ((int)RequestTypeId == (int)RequestTypeEnum.AdministrativeComplaints)
                {
                    approvalTracking.SubModuleId = (int)WorkflowSubModuleEnum.AdministrativeComplaints;
                }
                if ((int)RequestTypeId == (int)RequestTypeEnum.Contracts)
                {
                    approvalTracking.SubModuleId = (int)WorkflowSubModuleEnum.Contracts;
                }
                if ((int)RequestTypeId == (int)RequestTypeEnum.InternationalArbitration)
                {
                    approvalTracking.SubModuleId = (int)WorkflowSubModuleEnum.InternationalArbitration;
                }
            }
            else
            {
                if (IsConfidential)
                {
                    triggerId = TransferConsultationType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest ? (int)WorkflowModuleTriggerEnum.TransferConfidentialConsultationRequest : (int)WorkflowModuleTriggerEnum.TransferConfidentialConsultationFile;
                }
                else
                {
                    triggerId = TransferConsultationType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest ? (int)WorkflowModuleTriggerEnum.TransferConsultationRequest : (int)WorkflowModuleTriggerEnum.TransferConsultationFile;
                }
                if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.LegalAdvice)
                {
                    approvalTracking.SubModuleId = (int)WorkflowSubModuleEnum.LegalAdvice;
                }
                if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.Legislations)
                {
                    approvalTracking.SubModuleId = (int)WorkflowSubModuleEnum.Legislations;
                }
                if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.AdministrativeComplaints)
                {
                    approvalTracking.SubModuleId = (int)WorkflowSubModuleEnum.AdministrativeComplaints;
                }
                if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.Contracts)
                {
                    approvalTracking.SubModuleId = (int)WorkflowSubModuleEnum.Contracts;
                }
                if (loginState.UserDetail.SectorTypeId == (int)OperatingSectorTypeEnum.InternationalArbitration)
                {
                    approvalTracking.SubModuleId = (int)WorkflowSubModuleEnum.InternationalArbitration;
                }
            }

            var response = await workflowService.GetActiveWorkflows((int)WorkflowModuleEnum.COMSConsultationManagement, triggerId, null, approvalTracking.SubModuleId);
            if (response.IsSuccessStatusCode)
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
