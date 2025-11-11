using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.Dms;
using FATWA_DOMAIN.Models.LLSLegalPrinciple;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.WorkflowModels;

namespace FATWA_DOMAIN.Interfaces
{
    public interface IWorkflow
    {
        Task<WorkflowCountVM> GetWorkflowsCount();
        Task<WorkflowInstanceCountVM> GetWorkflowsInstanceCount(int workflowId);
        Task<List<WorkflowListVM>> GetWorkflows(WorkflowAdvanceSearchVM advanceSearchVM);
        Task<Workflow> GetWorkflowDetailById(int workflowId);
        Task<List<OperatingSectorType>> GetWorkflowSectorTransferOptions(int workflowTriggerId, int sectorTypeId);
        Task<WorkflowTrigger> GetWorkflowTriggerByWorkflowId(int workflowId);
        Task<List<WorkflowVM>> GetActiveWorkflows(int moduleTriggerId,int? attachmentTypeId, int? submoduleId);
        Task<List<Module>> GetWorkflowModules();
        Task<List<ModuleTrigger>> GetModuleTriggers(int submoduleId);
        Task<List<WorkflowSubModule>> GetSubModuleTriggers(int moduleId);
        Task<ModuleTriggerVM> GetModuleTriggerById(int triggerId);
        Task<List<AttachmentTypeListVM>> GetAttachementTypesById(int workflowId);
        Task<List<ModuleCondition>> GetModuleConditions(int triggerId);
        Task<List<ModuleActivity>> GetModuleActvities(int triggerId);
        Task<WorkflowActivity> GetWorkflowActivityById(int workflowActivityId);
        Task<WorkflowActivityVM> GetWorkflowActivityBySequenceNumber(int workflowId, int sequenceNumber);
        Task<List<WorkflowConditionsVM>> GetWorkflowConditions(int workflowActivityId);
        Task<List<WorkflowOptionsVM>> GetWorkflowOption(int workflowActivityId);
        Task<List<WorkflowConditionsOptionsListVM>> GetWorkflowConditionOptionList(int workflowConditionId);
        Task<List<ModuleActivity>> GetModuleActvitiesByCategory(int triggerId, int categoryId);
        Task<List<ParameterVM>> GetModuleActivityParameters(int activityId);
        Task<List<ParameterVM>> GetSlaActionParameters(int actionId);
        Task<List<WorkflowStatus>> GetWorkflowStatuses();
        Task<List<WorkflowActivityVM>> GetWorkflowActivities(int workflowId);
        Task<List<WorkflowTriggerConditionsVM>> GetWorkflowTriggerConditions(int workflowTriggerId);
        Task<List<WorkflowActivity>> GetWorkflowActivitiesByWorkflowId(int workflowId);
        Task<List<WorkflowActivityVM>> GetToDoWorkflowActivities(int workflowId, int sequenceNumber);
        Task<List<WorkflowActivityParametersVM>> GetWorkflowActivityParameters(int workflowActivityId, int? TriggerId, dynamic entity);
        Task CreateWorkflow(Workflow workflow);
        Task UpdateWorkflowInstanceStatus(Guid referenceId, int statusId);
        Task<WorkflowInstance> GetCurrentInstanceByReferneceId(Guid referenceId);
        Task<List<WorkflowInstanceDocumentVM>> GetWorkflowInstanceDocuments(int PageSize, int PageNumber);
        Task<List<WorkflowConditionsOptionVM>> GetWorkflowConditionOptions(Guid ReferneceId, int StatusId);
        Task<List<WorkflowActivityOptionVM>> GetWorkflowActivityOptions(int ActivityId);
        Task<WorkflowActivity> GetInstanceCurrentActivity(Guid referenceId);
        Task UpdateDocumentInstance(LegalLegislation document);
        Task UpdatePrincipleInstance(LLSLegalPrincipleSystem principle);
        Task<string> UpdateCaseDraftInstance(CmsDraftedTemplate draft, string userName); 
        Task<CmsCaseFileStatusHistory> UpdateApprovalTrackingInstance(CmsApprovalTracking approvalTracking); 
        Task UpdateApprovalTrackingConsultationInstance(CmsApprovalTracking approvalTracking); 
        Task<string> UpdateConsultationDraftInstance(ComsDraftedTemplate draft, string username);
        Task<string> UpdateDMSDocumentInstance(DmsAddedDocument document);
        Task<List<string>> GetUsersByRoleIdandSectorId(string RoleId, int sectorTypeId);
        Task UpdateWorkflowStatus(int workflowId, int statusId);
        Task<Workflow> GetActiveWorkflowforSuspend(int workflowId, int statusId);
        Task<List<ModuleConditionOptions>> GetModuleOptionsByTriggerId(int triggerId);
        Task UpdateCopyTrackingInstance(CmsApprovalTracking approvalTracking);
        Task<dynamic> UpdateCopyApprovedTrackingInstance(CmsApprovalTracking approvalTracking);
        Task<List<WorkflowTriggerCondition>> GetWorkflowTriggerConditionsByTriggerId(int TriggerId);
        Task<List<WorkflowTriggerSectorOptions>> GetWorkflowTriggerSectorOptions(int TriggerId);

        Task<List<int>> GetWorkflowTriggerSectorTransferOptions(int TriggerOptionId);
         Task<List<SLA>> GetActivtySlAsByActivityId(int WorkflowActivityId);
        Task<List<SLAActionParameters>> GetActivtySLAsActionParameterBySLAId(int WorkflowSLAId);
        Task<List<User>> GetViceHOSBySectorId(int sectorTypeId);
        Task<OperatingSectorType> GetViceHosResponsibleDetailBySectorId(int sectorTypeId);
        Task<User> GetViceHOSByUserId(string userId);
        Task<int> GetNextWorrkflowActivity(Guid draftId);
        Task<List<WorkflowTriggerConditionOptionsVM>> GetWorkflowTriggerConditionsOptions(int TriggerConditionId, Guid ReferenceId);

    }
}
