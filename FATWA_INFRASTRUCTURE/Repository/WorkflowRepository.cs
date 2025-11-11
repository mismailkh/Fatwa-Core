using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.Consultation;
using FATWA_DOMAIN.Models.Dms;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using FATWA_DOMAIN.Models.WorkflowModels;
using FATWA_INFRASTRUCTURE.Database;
using FATWA_INFRASTRUCTURE.Repository.CaseManagement;
using FATWA_INFRASTRUCTURE.Repository.CommonRepos;
using FATWA_INFRASTRUCTURE.Repository.Communications;
using FATWA_INFRASTRUCTURE.Repository.Consultation;
using FATWA_INFRASTRUCTURE.Repository.PatternNumber;
using FATWA_INFRASTRUCTURE.Repository.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.CommunicationEnums;
using static FATWA_DOMAIN.Enums.DmsEnums;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.LegalLegislationEnum;
using static FATWA_DOMAIN.Enums.WorkflowEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_DOMAIN.Enums.WorkflowParameterEnums;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Interfaces.Communication;
using FATWA_DOMAIN.Models.ViewModel.ConsultationVMs;
using FATWA_DOMAIN.Extensions;
using System.Reflection.Metadata;
using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models.LLSLegalPrinciple;
using static FATWA_DOMAIN.Enums.LegalPrinciple.LegalPrincipleEnum;
using System.Xml.Linq;
using FATWA_DOMAIN.Models.ViewModel.Lms;
using FATWA_DOMAIN.Interfaces.LLSLegalPrinciple;
using FATWA_DOMAIN.Models.ViewModel.CMSCOMSRequestNumberVMs;

namespace FATWA_INFRASTRUCTURE.Repository
{
    //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Repo for managing Workflow related operations</History>
    public class WorkflowRepository : IWorkflow
    {
        #region Variable Declaration
        private readonly DatabaseContext _dbContext;
        private readonly DmsDbContext _dbDmsContext;
        private List<WorkflowVM> _WorkflowVM;
        private List<WorkflowListVM> _WorkflowListVM;
        private WorkflowCountVM _WorkflowCountVM;
        private WorkflowInstanceCountVM _WorkflowInstanceCountVM;
        private readonly CommunicationRepository _communicationRepository;
        private readonly CMSCaseRequestRepository _CMSCaseRequestRepository;
        private readonly COMSConsultationRepository _COMSConsultationRequestRepository;
        private readonly CmsRegisteredCaseRepository _registeredCaseRepository;
        private readonly AccountRepository _accountRepository;
        private readonly TaskRepository _taskRepository;
        private readonly CmsSharedRepository _cmsSharedRepository;
        private readonly CmsCaseFileRepository _cmsCaseFileRepository;
        private readonly ComsSharedRepository _comsSharedRepository;
        private readonly IAccount _IAccount;

        public List<int> SectorToIds { get; set; } = new List<int>();
        SendCommunicationVM sendCommunication = new SendCommunicationVM();
        protected CaseRequestDetailVM caseRequest { get; set; } = new CaseRequestDetailVM();
        protected ViewConsultationVM viewConsultationVM { get; set; } = new ViewConsultationVM();
        protected ConsultationFileDetailVM consultationFileDetailVM { get; set; } = new ConsultationFileDetailVM();
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly CMSCOMSInboxOutboxPatternNumberRepository _cMSCOMSInboxOutboxPatternNumberRepository;
        public CommonRepository _commonRepo { get; }
        #endregion

        #region Constructor
        public WorkflowRepository(DatabaseContext dbContext, IServiceScopeFactory serviceScopeFactory, CommonRepository commonRepo, CMSCOMSInboxOutboxPatternNumberRepository CMSCOMSInboxOutboxPatternNumberRepository, DmsDbContext dbDmsContext, IAccount iAccount)
        {
            _dbContext = dbContext;
            _dbDmsContext = dbDmsContext;
            _serviceScopeFactory = serviceScopeFactory;
            using var scope = _serviceScopeFactory.CreateScope();
            _communicationRepository = scope.ServiceProvider.GetRequiredService<CommunicationRepository>();
            _CMSCaseRequestRepository = scope.ServiceProvider.GetService<CMSCaseRequestRepository>();
            _COMSConsultationRequestRepository = scope.ServiceProvider.GetService<COMSConsultationRepository>();
            _registeredCaseRepository = scope.ServiceProvider.GetRequiredService<CmsRegisteredCaseRepository>();
            _accountRepository = scope.ServiceProvider.GetRequiredService<AccountRepository>();
            _taskRepository = scope.ServiceProvider.GetRequiredService<TaskRepository>();
            _cmsSharedRepository = scope.ServiceProvider.GetRequiredService<CmsSharedRepository>();
            _cmsCaseFileRepository = scope.ServiceProvider.GetRequiredService<CmsCaseFileRepository>();
            _comsSharedRepository = scope.ServiceProvider.GetRequiredService<ComsSharedRepository>();
            _commonRepo = commonRepo;
            _cMSCOMSInboxOutboxPatternNumberRepository = CMSCOMSInboxOutboxPatternNumberRepository;
            _IAccount = iAccount;
        }
        #endregion

        #region Get Workflows

        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Get Workflows</History>
        public async Task<List<WorkflowListVM>> GetWorkflows(WorkflowAdvanceSearchVM advanceSearchVM)
        {
            try
            {
                if (_WorkflowListVM == null)
                {
                    string fromDate = advanceSearchVM.FromDate != null ? Convert.ToDateTime(advanceSearchVM.FromDate).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    string toDate = advanceSearchVM.ToDate != null ? Convert.ToDateTime(advanceSearchVM.ToDate).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    string StoredProc = $"exec pWorkflowListFiltered @StatusId='{advanceSearchVM.StatusId}', @FromDate='{fromDate}', @ToDate='{toDate}',@ModuleId='{advanceSearchVM.ModuleId}'" +
                        $",@PageNumber ='{advanceSearchVM.PageNumber}',@PageSize ='{advanceSearchVM.PageSize}'";
                    _WorkflowListVM = await _dbContext.WorkflowListVM.FromSqlRaw(StoredProc).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _WorkflowListVM;
        }

        public async Task<WorkflowCountVM> GetWorkflowsCount()
        {
            try
            {
                if (_WorkflowCountVM == null)
                {
                    string StoredProc = $"exec pWorkflowsCount";
                    var WorkflowCount = await _dbContext.WorkflowCountVM.FromSqlRaw(StoredProc).ToListAsync();
                    _WorkflowCountVM = WorkflowCount.FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return _WorkflowCountVM;
        }
        //<History Author = 'Muhammad Zaeem' Date='2022-06-02' Version="1.0" Branch="master"> Get Workflow count</History>
        public async Task<WorkflowInstanceCountVM> GetWorkflowsInstanceCount(int workflowId)
        {
            try
            {
                string StoredProc = $"exec pWorkflowsinstancecount @workflowId={workflowId}";
                var WorkflowInstanceCount = await _dbContext.WorkflowInstanceCountVM.FromSqlRaw(StoredProc).ToListAsync();
                _WorkflowInstanceCountVM = WorkflowInstanceCount.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return _WorkflowInstanceCountVM;
        }
        //<History Author = 'Muhammad Zaeem' Date='2022-06-02' Version="1.0" Branch="master"> Get Workflow detail by id</History>
        public async Task<Workflow> GetWorkflowDetailById(int workflowId)
        {
            try
            {
                var workflow = await _dbContext.Workflow.FindAsync(workflowId);
                if (workflow != null)
                {
                    workflow.WorkflowTrigger = await GetWorkflowTriggerByWorkflowId(workflowId);
                    workflow.ModuleTriggerVM = await GetModuleTriggerById(workflow.WorkflowTrigger.ModuleTriggerId);
                }
                return workflow;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //<History Author = 'Muhammad Zaeem' Date='2022-01-12' Version="1.0" Branch="master"> Get Workflow trigger by workflowId</History>
        public async Task<WorkflowTrigger> GetWorkflowTriggerByWorkflowId(int workflowId)
        {
            try
            {
                return _dbContext.WorkflowTrigger.Where(t => t.WorkflowId == workflowId).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<WorkflowVM>> GetActiveWorkflows(int moduleTriggerId, int? attachmentTypeId, int? submoduleId)
        {
            try
            {
                string StoredProc = $"exec pWorkflowSelAdvanceSearch @statusId = '{(int)WorkflowStatusEnum.Active}', @moduleTriggerId = '{moduleTriggerId}',@attachmenttypeId='{attachmentTypeId}',@submoduleId='{submoduleId}'";
                return await _dbContext.WorkflowVM.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //<History Author = 'Muhammad Zaeem' Date='2022-01-12' Version="1.0" Branch="master"> Get Workflow Modules by id</History>
        public async Task<List<Module>> GetWorkflowModules()
        {
            try
            {
                return await _dbContext.WorkflowModule.OrderByDescending(u => u.ModuleId).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<ModuleTrigger>> GetModuleTriggers(int submoduleId)
        {
            try
            {
                string StoredProc = $"exec pWorkflowSubModuleTriggerListFiltered @submoduleId = '{submoduleId}'";
                var triggers = await _dbContext.ModuleTrigger.FromSqlRaw(StoredProc).ToListAsync();
                return triggers;
                //return await _dbContext.ModuleTrigger.Where(t => t.ModuleId == moduleId&&t.WorkflowSubModuleId== submoduleId).OrderByDescending(u => u.ModuleTriggerId).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<WorkflowSubModule>> GetSubModuleTriggers(int moduleId)
        {
            try
            {
                return await _dbContext.SubModuleTrigger.Where(t => t.ModuleId == moduleId).OrderByDescending(u => u.ModuleId).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        //<History Author = 'Muhammad Zaeem' Date='2022-01-12' Version="1.0" Branch="master"> Get Workflow Trigger Conditions By TriggerId</History>

        public async Task<List<WorkflowTriggerCondition>> GetWorkflowTriggerConditionsByTriggerId(int TriggerId)
        {
            try
            {
                List<WorkflowTriggerCondition> triggerConditions = await _dbContext.WorkflowTriggerCondition.Where(t => t.WorkflowTriggerId == TriggerId).OrderByDescending(x => x.CreatedDate).ToListAsync();
                if (triggerConditions.Any())
                {
                    foreach (var condition in triggerConditions)
                    {
                        List<WorkflowTriggerConditionOption> triggerConditionOptions = await _dbContext.WorkflowTriggerConditionOption.Where(t => t.TriggerConditionId == condition.TriggerConditionId).OrderByDescending(x => x.CreatedDate).ToListAsync();
                        condition.workflowTriggerConditionOptions = triggerConditionOptions;
                    }
                }
                return triggerConditions;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<ModuleTriggerVM> GetModuleTriggerById(int triggerId)
        {
            try
            {
                string StoredProc = $"exec pWorkflowModuleTriggerListFiltered @triggerId = '{triggerId}'";
                var triggers = await _dbContext.ModuleTriggerVM.FromSqlRaw(StoredProc).ToListAsync();
                return triggers.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<AttachmentTypeListVM>> GetAttachementTypesById(int workflowId)
        {
            try
            {
                string StoredProc = $"exec pWorkflowAttachmentTypesByWorkflowId @workflowId = '{workflowId}'";
                var types = await _dbContext.AttachmentTypeVM.FromSqlRaw(StoredProc).ToListAsync();
                return types;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //<History Author = 'Muhammad Zaeem' Date='2022-01-12' Version="1.0" Branch="master"> Get Workflow Trigger Sector Options</History>
        public async Task<List<WorkflowTriggerSectorOptions>> GetWorkflowTriggerSectorOptions(int TriggerId)
        {
            try
            {
                var sectorOptions = await _dbContext.WorkflowTriggerSectorOptions.Where(x => x.TriggerId == TriggerId).ToListAsync();
                return sectorOptions;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //<History Author = 'Muhammad Zaeem' Date='2022-01-12' Version="1.0" Branch="master"> Get Workflow Trigger Sector Transfer Options</History>
        public async Task<List<int>> GetWorkflowTriggerSectorTransferOptions(int TriggerOptionId)
        {
            try
            {
                var sectorTransferOptions = await _dbContext.WorkflowTriggerSectorTransferOptions.Where(x => x.TriggerOptionId == TriggerOptionId).ToListAsync();
                foreach (var options in sectorTransferOptions)
                {
                    SectorToIds.Add(options.SectorToId);
                }
                return SectorToIds;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //<History Author = 'Muhammad Zaeem' Date='2022-01-12' Version="1.0" Branch="master"> Get Activty SlAs By ActivityId</History>
        public async Task<List<SLA>> GetActivtySlAsByActivityId(int WorkflowActivityId)
        {
            try
            {
                var workflowactivitySLAs = await _dbContext.SLA.Where(x => x.WorkflowActivityId == WorkflowActivityId).ToListAsync();
                return workflowactivitySLAs;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //<History Author = 'Muhammad Zaeem' Date='2022-01-12' Version="1.0" Branch="master"> Get Activty SLAs Action Parameter By SLAId</History>
        public async Task<List<SLAActionParameters>> GetActivtySLAsActionParameterBySLAId(int WorkflowSLAId)
        {
            try
            {
                var workflowSLAsParam = await _dbContext.SLAActionParameters.Where(x => x.WorkflowSLAId == WorkflowSLAId).ToListAsync();
                return workflowSLAsParam;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<ModuleCondition>> GetModuleConditions(int triggerId)
        {
            try
            {
                string StoredProc = $"exec pWorkflowSubModuleConditionListFiltered @triggerId='{triggerId}'";
                return await _dbContext.ModuleCondition.FromSqlRaw(StoredProc).ToListAsync();
                //return await _dbContext.ModuleCondition.Where(t => t.ModuleId == moduleId).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<ModuleActivity>> GetModuleActvities(int triggerId)
        {
            try
            {
                string StoredProc = $"exec pWorkflowSubModuleActivityListFiltered @triggerId = '{triggerId}'";
                return await _dbContext.ModuleActivity.FromSqlRaw(StoredProc).ToListAsync();
                //return await _dbContext.ModuleActivity.Where(t => t.ModuleId == moduleId).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<ModuleActivity>> GetModuleActvitiesByCategory(int triggerId, int categoryId)
        {
            try
            {
                string StoredProc = $"exec pWorkflowSubModuleActivityListFiltered @triggerId = '{triggerId}'";
                return await _dbContext.ModuleActivity.FromSqlRaw(StoredProc).ToListAsync();
                //return await _dbContext.ModuleActivity.Where(t => t.ModuleId == moduleId && t.CategoryId == categoryId).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<ParameterVM>> GetModuleActivityParameters(int activityId)
        {
            try
            {
                string StoredProc = $"exec pWorkflowModuleActivityParametersList @activityId = '{activityId}'";
                return await _dbContext.ParameterVM.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<WorkflowStatus>> GetWorkflowStatuses()
        {
            try
            {
                return await _dbContext.WorkflowStatus.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<ModuleConditionOptions>> GetModuleOptionsByTriggerId(int triggerId)
        {
            try
            {
                string StoredProc = $"exec pWorkflowConditionOptionsListFiltered @triggerId = '{triggerId}'";
                return await _dbContext.ConditionOptions.FromSqlRaw(StoredProc).ToListAsync();
                //return await _dbContext.ModuleActivity.Where(t => t.ModuleId == moduleId).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<OperatingSectorType>> GetWorkflowSectorTransferOptions(int workflowTriggerId, int sectorTypeId)
        {
            try
            {
                string StoredProc = $"exec pWorkflowSectorTransferOptionsList @workflowTriggerId = '{workflowTriggerId}', @sectorTypeId = '{sectorTypeId}'";
                return await _dbContext.OperatingSectorType.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Create Workflow

        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Create Workflow</History>
        public async Task CreateWorkflow(Workflow Workflow)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        Workflow.WorkflowId = 0;
                        _dbContext.Workflow.Add(Workflow);
                        await _dbContext.SaveChangesAsync();
                        var WorkflowId = Workflow.WorkflowId;
                        Workflow.WorkflowTrigger.WorkflowId = WorkflowId;
                        await InsertWorkflowTriggerDetails(Workflow, _dbContext);
                        await InsertWorkflowActivities(Workflow, _dbContext);
                        await InsertWorkflowAttachmentType(Workflow, _dbContext);
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        private async Task InsertWorkflowTriggerDetails(Workflow workflow, DatabaseContext dbContext)
        {
            try
            {
                workflow.WorkflowTrigger.WorkflowTriggerId = 0;
                await dbContext.WorkflowTrigger.AddAsync(workflow.WorkflowTrigger);
                await dbContext.SaveChangesAsync();
                foreach (var condition in workflow.WorkflowTrigger.WorkflowConditions)
                {
                    WorkflowTriggerCondition triggerCondition = new WorkflowTriggerCondition();
                    triggerCondition.WorkflowTriggerId = workflow.WorkflowTrigger.WorkflowTriggerId;
                    triggerCondition.ConditionId = condition.ModuleConditionId;
                    triggerCondition.TrueCaseActivityNo = condition.TrueCaseActivityNo;
                    triggerCondition.TrueCaseFlowControlId = condition.TrueCaseFlowControlId;
                    triggerCondition.FalseCaseActivityNo = condition.FalseCaseActivityNo;
                    triggerCondition.FalseCaseFlowControlId = condition.FalseCaseFlowControlId;
                    triggerCondition.CreatedBy = workflow.CreatedBy;
                    triggerCondition.CreatedDate = DateTime.Now;
                    triggerCondition.IsOption = condition.IsOption;
                    await dbContext.WorkflowTriggerCondition.AddAsync(triggerCondition);
                    await dbContext.SaveChangesAsync();
                    if (condition.workflowConditionOptions.Any())
                    {
                        foreach (var option in condition.workflowConditionOptions)
                        {
                            WorkflowTriggerConditionOption workflowTriggerConditionOption = new WorkflowTriggerConditionOption();
                            workflowTriggerConditionOption.WorkflowOptionId = 0;
                            workflowTriggerConditionOption.ModuleOptionId = option.ModuleOptionId;
                            workflowTriggerConditionOption.TriggerConditionId = triggerCondition.TriggerConditionId;
                            workflowTriggerConditionOption.TrueCaseFlowControlId = option.TrueCaseFlowControlId;
                            workflowTriggerConditionOption.TrueCaseActivityNo = option.TrueCaseActivityNo;
                            workflowTriggerConditionOption.CreatedBy = workflow.CreatedBy;
                            workflowTriggerConditionOption.CreatedDate = workflow.CreatedDate;
                            await dbContext.WorkflowTriggerConditionOption.AddAsync(workflowTriggerConditionOption);
                            await dbContext.SaveChangesAsync();
                        }
                    }
                }
                foreach (var option in workflow.WorkflowTrigger.WorkflowTriggerSectorOptions)
                {
                    WorkflowTriggerSectorOptions sectorOption = new WorkflowTriggerSectorOptions();
                    sectorOption.TriggerId = workflow.WorkflowTrigger.WorkflowTriggerId;
                    sectorOption.SectorFromId = option.SectorFrom.Id;
                    sectorOption.CreatedBy = workflow.CreatedBy;
                    sectorOption.CreatedDate = DateTime.Now;
                    await dbContext.WorkflowTriggerSectorOptions.AddAsync(sectorOption);
                    await dbContext.SaveChangesAsync();
                    foreach (var transferOption in option.SectorToIds)
                    {
                        WorkflowTriggerSectorTransferOptions sectorTransferOption = new WorkflowTriggerSectorTransferOptions();
                        sectorTransferOption.TriggerOptionId = sectorOption.TriggerOptionId;
                        sectorTransferOption.SectorToId = transferOption.Id;
                        sectorTransferOption.CreatedBy = workflow.CreatedBy;
                        sectorTransferOption.CreatedDate = DateTime.Now;
                        await dbContext.WorkflowTriggerSectorTransferOptions.AddAsync(sectorTransferOption);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task InsertWorkflowAttachmentType(Workflow workflow, DatabaseContext dbContext)
        {
            try
            {
                if (workflow.AttachmentTypeId != null && workflow.AttachmentTypeId.Count() > 0)
                {
                    foreach (var item in workflow.AttachmentTypeId)
                    {
                        WorkflowAttachmentType workflowAttachmentType = new WorkflowAttachmentType();
                        workflowAttachmentType.WorkflowId = workflow.WorkflowId;
                        workflowAttachmentType.AttachmentTypeId = item;
                        workflowAttachmentType.CreatedBy = workflow.CreatedBy;
                        workflowAttachmentType.CreatedDate = workflow.CreatedDate;
                        workflowAttachmentType.ModifiedDate = workflow.ModifiedDate;
                        workflowAttachmentType.ModifiedBy = workflow.ModifiedBy;
                        workflowAttachmentType.DeletedDate = workflow.DeletedDate;
                        workflowAttachmentType.DeletedBy = workflow.DeletedBy;
                        workflowAttachmentType.IsDeleted = workflow.IsDeleted;
                        workflowAttachmentType.IsActiveFlow = true;
                        await dbContext.WorkflowAttachmentType.AddAsync(workflowAttachmentType);
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task InsertWorkflowActivities(Workflow workflow, DatabaseContext dbContext)
        {
            try
            {
                foreach (var activity in workflow.WorkflowActivities)
                {
                    activity.WorkflowId = workflow.WorkflowId;
                    activity.WorkflowActivityId = 0;
                    await dbContext.WorkflowActivity.AddAsync(activity);
                    await dbContext.SaveChangesAsync();
                    foreach (var param in activity.Parameters)
                    {
                        await dbContext.WorkflowActivityParameters.AddAsync(new WorkflowActivityParameters { WorkflowActivityId = activity.WorkflowActivityId, ParameterId = param.ParameterId, Value = param.Value });
                        await dbContext.SaveChangesAsync();
                    }
                    foreach (var condition in activity.WorkflowConditions)
                    {
                        condition.WorkflowActivityId = activity.WorkflowActivityId;
                        condition.WorkflowConditionId = 0;
                        await dbContext.WorkflowCondition.AddAsync(condition);
                        await dbContext.SaveChangesAsync();
                        if (condition.workflowConditionOptions.Count() > 0)
                        {
                            foreach (var option in condition.workflowConditionOptions)
                            {
                                option.WorkflowConditionId = condition.WorkflowConditionId;
                                option.CreatedBy = workflow.CreatedBy;
                                option.CreatedDate = workflow.CreatedDate;
                                option.WorkflowOptionId = 0;
                                await dbContext.WorkflowConditionOptions.AddAsync(option);
                                await dbContext.SaveChangesAsync();

                            }
                        }
                    }
                    foreach (var activityOption in activity.WorkflowOptions)
                    {
                        activityOption.WorkflowOptionId = 0;
                        activityOption.WorkflowActivityId = activity.WorkflowActivityId;
                        activityOption.CreatedBy = workflow.CreatedBy;
                        activityOption.CreatedDate = workflow.CreatedDate;
                        await dbContext.WorkflowActivityOptions.AddAsync(activityOption);
                        await dbContext.SaveChangesAsync();

                    }
                    foreach (var sla in activity.SLAs)
                    {
                        sla.WorkflowActivityId = activity.WorkflowActivityId;
                        sla.WorkflowSLAId = 0;
                        await dbContext.SLA.AddAsync(sla);
                        await dbContext.SaveChangesAsync();
                        foreach (var par in sla.Parameters)
                        {
                            await dbContext.SLAActionParameters.AddAsync(new SLAActionParameters { WorkflowSLAId = sla.WorkflowSLAId, ParameterId = par.ParameterId, Value = par.Value });
                            await dbContext.SaveChangesAsync();
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region SLA
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Proc to get Action Parameters for SLA</History>
        public async Task<List<ParameterVM>> GetSlaActionParameters(int actionId)
        {
            try
            {
                string StoredProc = $"exec pWorkflowListSlaActionParameters @actionId = '{actionId}'";
                return await _dbContext.ParameterVM.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Workflow Activities

        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Proc to get Workflow Activities</History>
        public async Task<List<WorkflowActivityVM>> GetWorkflowActivities(int workflowId)
        {
            try
            {
                string StoredProc = $"exec pWorkflowActivitiesList @workflowId = '{workflowId}'";
                return await _dbContext.WorkflowActivityVM.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2023-10-29' Version="1.0" Branch="master"> Get workflow Trigger Conditions</History>
        public async Task<List<WorkflowTriggerConditionsVM>> GetWorkflowTriggerConditions(int workflowTriggerId)
        {
            try
            {
                string StoredProc = $"exec pWorkflowTriggerConditionsByTriggerId @workflowTriggerId = '{workflowTriggerId}'";
                return await _dbContext.WorkflowTriggerConditionsVM.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        public async Task<List<WorkflowActivity>> GetWorkflowActivitiesByWorkflowId(int workflowId)
        {
            try
            {
                return await _dbContext.WorkflowActivity.Where(w => w.WorkflowId == workflowId).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<WorkflowActivityVM>> GetToDoWorkflowActivities(int workflowId, int sequenceNumber)
        {
            try
            {
                string StoredProc = $"exec pWorkflowToDoActivitiesList @workflowId = '{workflowId}', @sequenceNumber = '{sequenceNumber}'";
                return await _dbContext.WorkflowActivityVM.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<WorkflowActivity> GetWorkflowActivityById(int workflowActivityId)
        {
            try
            {
                return _dbContext.WorkflowActivity.Where(w => w.WorkflowActivityId == workflowActivityId).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<WorkflowActivityVM> GetWorkflowActivityBySequenceNumber(int workflowId, int sequenceNumber)
        {
            try
            {
                string StoredProc = $"exec pWorkflowActivityBySequenceNo @workflowId = '{workflowId}', @sequenceNumber = '{sequenceNumber}'";
                var list = await _dbContext.WorkflowActivityVM.FromSqlRaw(StoredProc).ToListAsync();
                return list.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<WorkflowConditionsVM>> GetWorkflowConditions(int workflowActivityId)
        {
            try
            {
                string StoredProc = $"exec pWorkflowConditionsByActivityList @workflowActivityId = '{workflowActivityId}'";
                var list = await _dbContext.WorkflowConditionsVM.FromSqlRaw(StoredProc).ToListAsync();
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<WorkflowOptionsVM>> GetWorkflowOption(int workflowActivityId)
        {
            try
            {
                string StoredProc = $"exec pWorkflowOptionsByActivityList @workflowActivityId = '{workflowActivityId}'";
                var list = await _dbContext.WorkflowOptionsVM.FromSqlRaw(StoredProc).ToListAsync();
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<WorkflowConditionsOptionsListVM>> GetWorkflowConditionOptionList(int workflowConditionId)
        {
            try
            {
                string StoredProc = $"exec pWorkflowConditionsOptionsByConditionId @WorkflowConditionId = '{workflowConditionId}'";
                var list = await _dbContext.WorkflowConditionsOptionsListVM.FromSqlRaw(StoredProc).ToListAsync();
                return list;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<WorkflowActivityParametersVM>> GetWorkflowActivityParameters(int workflowActivityId, int? TriggerId, dynamic entity)
        {
            try
            {

                if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsCaseDraft || TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsConsultationDraft)
                {
                    return await GetDraftDocumentParameters(workflowActivityId, TriggerId, entity);
                }
                else if (TriggerId >= (int)WorkflowModuleTriggerEnum.TransferCaseRequest && TriggerId <= (int)WorkflowModuleTriggerEnum.TransferConfidentialConsultationFile || TriggerId == (int)WorkflowModuleTriggerEnum.TransferConfidentialCaseRequestPrivateOffice || TriggerId == (int)WorkflowModuleTriggerEnum.TransferConfidentialConsultationRequestPrivateOffice)
                {
                    return await GetTransferRequestParameters(workflowActivityId, TriggerId, entity);
                }
                else if (TriggerId == (int)WorkflowModuleTriggerEnum.SendCopyCaseRequest || TriggerId == (int)WorkflowModuleTriggerEnum.SendCopyCaseFile)
                {
                    return await GetCopyRequestParameters(workflowActivityId, TriggerId, entity);
                }
                else if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsDMSDocument)
                {
                    return await GetDMSInitiatorModificationParameters(workflowActivityId, TriggerId, entity);
                }
                else
                {
                    string StoredProc = $"exec pWorkflowActivityParametersList @workflowActivityId = '{workflowActivityId}'";
                    var list = await _dbContext.WorkflowActivityParametersVM.FromSqlRaw(StoredProc).ToListAsync();
                    return list;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<List<WorkflowActivityParametersVM>> GetDraftDocumentParameters(int workflowActivityId, int? TriggerId, dynamic entity)
        {
            try
            {
                CmsDraftedTemplate draftedTemplate = System.Text.Json.JsonSerializer.Deserialize<CmsDraftedTemplate>(entity);
                WorkflowActivity workflowActivity = await _dbContext.WorkflowActivity.FindAsync(workflowActivityId);
                ModuleActivity moduleActivity = await _dbContext.ModuleActivity.FindAsync(workflowActivity.ActivityId);
                User user = await _dbContext.Users.Where(u => u.UserName == draftedTemplate.CreatedBy).FirstOrDefaultAsync();
                string StoredProc = $"exec pWorkflowActivityParametersList @workflowActivityId = '{workflowActivityId}'";
                List<WorkflowActivityParametersVM> parameters = await _dbContext.WorkflowActivityParametersVM.FromSqlRaw(StoredProc).ToListAsync();
                if ((WorkflowActivityEnum)Enum.Parse(typeof(WorkflowActivityEnum), moduleActivity.AKey) == WorkflowActivityEnum.CmsReviewDraftDocument || (WorkflowActivityEnum)Enum.Parse(typeof(WorkflowActivityEnum), moduleActivity.AKey) == WorkflowActivityEnum.ComsReviewDraftDocument)
                {
                    foreach (var param in parameters)
                    {
                        if ((WorkflowParams)Enum.Parse(typeof(WorkflowParams), param.PKey) == WorkflowParams.CmsReviewDraftDocument_User)
                        {
                            if (String.IsNullOrEmpty(param.Value))
                            {
                                dynamic caseAssignment = null;
                                if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsCaseDraft)
                                {
                                    caseAssignment = await _dbContext.CaseFileAssignment.Where(a => a.ReferenceId == draftedTemplate.ReferenceId && a.LawyerId == user.Id).FirstOrDefaultAsync();
                                }
                                else if (TriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsConsultationDraft)
                                {
                                    caseAssignment = await _dbContext.ConsultationAssignments.Where(a => a.ReferenceId == draftedTemplate.ReferenceId && a.LawyerId == user.Id).FirstOrDefaultAsync();
                                }
                                param.Value = caseAssignment?.SupervisorId;
                            }
                            break;
                        }
                    }
                }
                else if ((WorkflowActivityEnum)Enum.Parse(typeof(WorkflowActivityEnum), moduleActivity.AKey) == WorkflowActivityEnum.CmsReviewDraftDocumentHOS || (WorkflowActivityEnum)Enum.Parse(typeof(WorkflowActivityEnum), moduleActivity.AKey) == WorkflowActivityEnum.ComsReviewDraftDocumentHOS)
                {
                    foreach (var param in parameters)
                    {
                        if ((WorkflowParams)Enum.Parse(typeof(WorkflowParams), param.PKey) == WorkflowParams.CmsReviewDraftDocumentHos_User)
                        {
                            if (String.IsNullOrEmpty(param.Value))
                            {
                                User hos = await GetHOSBySectorId(draftedTemplate.SectorTypeId);
                                param.Value = hos?.Id;
                            }
                            break;
                        }
                    }
                }
                else if ((WorkflowActivityEnum)Enum.Parse(typeof(WorkflowActivityEnum), moduleActivity.AKey) == WorkflowActivityEnum.CmsReviewDraftDocumentGS)
                {
                    foreach (var param in parameters)
                    {
                        if ((WorkflowParams)Enum.Parse(typeof(WorkflowParams), param.PKey) == WorkflowParams.CmsReviewDraftDocumentGS_User)
                        {
                            if (String.IsNullOrEmpty(param.Value))
                            {
                                if (draftedTemplate.SectorTypeId < (int)OperatingSectorTypeEnum.CivilCommercialUnderFilingCases)
                                {
                                    User hos = await GetHOSBySectorId((int)OperatingSectorTypeEnum.AdministrativeGeneralSupervisor);
                                    param.Value = hos?.Id;
                                }
                                else if ((draftedTemplate.SectorTypeId >= (int)OperatingSectorTypeEnum.CivilCommercialUnderFilingCases) &&
                                         (draftedTemplate.SectorTypeId <= (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases))
                                {
                                    User hos = await GetHOSBySectorId((int)OperatingSectorTypeEnum.CivilCommercialGeneralSupervisor);
                                    param.Value = hos?.Id;
                                }
                            }
                            break;
                        }
                    }
                }
                else if ((WorkflowActivityEnum)Enum.Parse(typeof(WorkflowActivityEnum), moduleActivity.AKey) == WorkflowActivityEnum.CmsReviewDraftDocumentPOO)
                {
                    foreach (var param in parameters)
                    {
                        if ((WorkflowParams)Enum.Parse(typeof(WorkflowParams), param.PKey) == WorkflowParams.CmsReviewDraftDocumentPOO_User)
                        {
                            if (String.IsNullOrEmpty(param.Value))
                            {
                                User hos = await GetHOSBySectorId((int)OperatingSectorTypeEnum.PublicOperationalSector);
                                param.Value = hos?.Id;
                            }
                            break;
                        }
                    }
                }
                else if ((WorkflowActivityEnum)Enum.Parse(typeof(WorkflowActivityEnum), moduleActivity.AKey) == WorkflowActivityEnum.CmsReviewDraftDocumentLawyer || (WorkflowActivityEnum)Enum.Parse(typeof(WorkflowActivityEnum), moduleActivity.AKey) == WorkflowActivityEnum.LawyerDraftModification)
                {
                    foreach (var param in parameters)
                    {
                        if ((WorkflowParams)Enum.Parse(typeof(WorkflowParams), param.PKey) == WorkflowParams.CmsReviewDraftDocumentLawyer_User || (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param.PKey) == WorkflowParams.LawModifyDraftDocument_User)
                        {
                            if (String.IsNullOrEmpty(param.Value))
                            {
                                User lawyer = _accountRepository.GetUserByUserEmail(draftedTemplate.CreatedBy);
                                param.Value = lawyer?.Id;
                            }
                            break;
                        }
                    }
                }
                else if ((WorkflowActivityEnum)Enum.Parse(typeof(WorkflowActivityEnum), moduleActivity.AKey) == WorkflowActivityEnum.CmsReviewDraftDocumentViceHOS)
                {
                    var viceHosResponsible = await GetViceHosResponsibleDetailBySectorId(draftedTemplate.SectorTypeId);
                    foreach (var param in parameters)
                    {
                        if (viceHosResponsible.IsViceHosResponsibleForAllLawyers == false)
                        {
                            if ((WorkflowParams)Enum.Parse(typeof(WorkflowParams), param.PKey) == WorkflowParams.CmsReviewDraftDocumentViceHos_User)
                            {
                                if (String.IsNullOrEmpty(param.Value))
                                {
                                    User lawyer = _accountRepository.GetUserByUserEmail(draftedTemplate.CreatedBy);
                                    if (lawyer != null)
                                    {
                                        User hos = await GetViceHOSByUserId(lawyer?.Id);
                                        param.Value = hos?.Id;
                                    }
                                }
                                break;
                            }
                        }
                        else
                        {
                            if ((WorkflowParams)Enum.Parse(typeof(WorkflowParams), param.PKey) == WorkflowParams.CmsReviewDraftDocumentViceHos_UserRole)
                            {
                                param.Value = SystemRoles.ViceHOS;
                                break;
                            }
                        }
                    }
                }
                return parameters;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task<List<WorkflowActivityParametersVM>> GetTransferRequestParameters(int workflowActivityId, int? TriggerId, dynamic entity)
        {
            try
            {
                CmsApprovalTracking cmsApprovalTracking = System.Text.Json.JsonSerializer.Deserialize<CmsApprovalTracking>(entity);
                WorkflowActivity workflowActivity = await _dbContext.WorkflowActivity.FindAsync(workflowActivityId);
                ModuleActivity moduleActivity = await _dbContext.ModuleActivity.FindAsync(workflowActivity.ActivityId);
                var transferHistory = await _dbContext.CmsTransferHistories.Where(x => x.ReferenceId == cmsApprovalTracking.ReferenceId && x.ApprovalTrackingId == cmsApprovalTracking.Id).OrderBy(c => c.CreatedDate).FirstOrDefaultAsync();
                string StoredProc = $"exec pWorkflowActivityParametersList @workflowActivityId = '{workflowActivityId}'";
                List<WorkflowActivityParametersVM> parameters = await _dbContext.WorkflowActivityParametersVM.FromSqlRaw(StoredProc).ToListAsync();
                if ((WorkflowActivityEnum)Enum.Parse(typeof(WorkflowActivityEnum), moduleActivity.AKey) == WorkflowActivityEnum.CmsTransferToSector)
                {
                    foreach (var param in parameters)
                    {
                        if ((WorkflowParams)Enum.Parse(typeof(WorkflowParams), param.PKey) == WorkflowParams.CmsTransferUser)
                        {
                            if (String.IsNullOrEmpty(param.Value))
                            {
                                User hos = await GetHOSBySectorId(cmsApprovalTracking.SectorTo);
                                param.Value = hos?.Id;
                            }
                            break;
                        }

                    }
                }
                else if ((WorkflowActivityEnum)Enum.Parse(typeof(WorkflowActivityEnum), moduleActivity.AKey) == WorkflowActivityEnum.CmsTransferToRecieverAndEndflow)
                {
                    foreach (var param in parameters)
                    {
                        if ((WorkflowParams)Enum.Parse(typeof(WorkflowParams), param.PKey) == WorkflowParams.CmsTransferToRecieverAndEndflowUser)
                        {
                            if (String.IsNullOrEmpty(param.Value))
                            {
                                User hos = await GetHOSBySectorId(transferHistory != null ? transferHistory.SectorTo : cmsApprovalTracking.SectorTo);
                                param.Value = hos?.Id;
                            }
                            break;
                        }

                    }
                }
                else if ((WorkflowActivityEnum)Enum.Parse(typeof(WorkflowActivityEnum), moduleActivity.AKey) == WorkflowActivityEnum.CmsTransferToIntiator || (WorkflowActivityEnum)Enum.Parse(typeof(WorkflowActivityEnum), moduleActivity.AKey) == WorkflowActivityEnum.CmsTransferToInitiatorAndEndflow)
                {
                    foreach (var param in parameters)
                    {
                        if ((WorkflowParams)Enum.Parse(typeof(WorkflowParams), param.PKey) == WorkflowParams.CmsTransferToInitiatorUser || (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param.PKey) == WorkflowParams.CmsTransferToInitiatorAndEndflowUser)
                        {
                            if (String.IsNullOrEmpty(param.Value))
                            {
                                User hos = await GetHOSBySectorId(transferHistory != null ? transferHistory.SectorFrom : cmsApprovalTracking.SectorFrom);
                                param.Value = hos?.Id;
                            }
                            break;
                        }
                    }
                }
                else if ((WorkflowActivityEnum)Enum.Parse(typeof(WorkflowActivityEnum), moduleActivity.AKey) == WorkflowActivityEnum.TransferToRespectiveSectorAndEndflow)
                {
                    foreach (var param in parameters)
                    {
                        if ((WorkflowParams)Enum.Parse(typeof(WorkflowParams), param.PKey) == WorkflowParams.CmsTransferToRespectiveSectorAndEndflowUser)
                        {
                            if (String.IsNullOrEmpty(param.Value))
                            {
                                int? sectorTypeId = 0;
                                sectorTypeId = await GetRespectiveSectorIdFromApporvalTracking(cmsApprovalTracking, _dbContext);
                                User hos = await GetHOSBySectorId((int)sectorTypeId);
                                param.Value = hos?.Id;
                            }
                            break;
                        }
                    }
                }
                else if ((WorkflowActivityEnum)Enum.Parse(typeof(WorkflowActivityEnum), moduleActivity.AKey) == WorkflowActivityEnum.CmsSendToGS)
                {
                    foreach (var param in parameters)
                    {
                        if ((WorkflowParams)Enum.Parse(typeof(WorkflowParams), param.PKey) == WorkflowParams.CmsSendToGSUser)
                        {
                            if (String.IsNullOrEmpty(param.Value))
                            {
                                int? requestTypeId = 0;
                                if (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest)
                                {
                                    requestTypeId = await _dbContext.CaseRequests.Where(x => x.RequestId == cmsApprovalTracking.ReferenceId).Select(x => x.RequestTypeId).FirstOrDefaultAsync();
                                }
                                else if (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseFile)
                                {
                                    var caseRequest = await _dbContext.CaseFiles.Where(x => x.FileId == cmsApprovalTracking.ReferenceId).FirstOrDefaultAsync();
                                    if (caseRequest != null)
                                        requestTypeId = await _dbContext.CaseRequests.Where(x => x.RequestId == caseRequest.RequestId).Select(x => x.RequestTypeId).FirstOrDefaultAsync();
                                }
                                if (requestTypeId == (int)RequestTypeEnum.Administrative)
                                {
                                    User hos = await GetHOSBySectorId((int)OperatingSectorTypeEnum.AdministrativeGeneralSupervisor);
                                    param.Value = hos?.Id;
                                }
                                else if (requestTypeId == (int)RequestTypeEnum.CivilCommercial)
                                {
                                    User hos = await GetHOSBySectorId((int)OperatingSectorTypeEnum.CivilCommercialGeneralSupervisor);
                                    param.Value = hos?.Id;
                                }
                            }
                            break;
                        }
                    }
                }
                else if ((WorkflowActivityEnum)Enum.Parse(typeof(WorkflowActivityEnum), moduleActivity.AKey) == WorkflowActivityEnum.CmsSendToPOO || (WorkflowActivityEnum)Enum.Parse(typeof(WorkflowActivityEnum), moduleActivity.AKey) == WorkflowActivityEnum.CmsTransferToPOAndEndFlow)
                {
                    foreach (var param in parameters)
                    {
                        if ((WorkflowParams)Enum.Parse(typeof(WorkflowParams), param.PKey) == WorkflowParams.CmsSendToPOOUser || (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param.PKey) == WorkflowParams.CmsTransferToPOAndEndFlowUser)
                        {
                            if (String.IsNullOrEmpty(param.Value))
                            {
                                User hos = await GetHOSBySectorId((int)OperatingSectorTypeEnum.PublicOperationalSector);
                                param.Value = hos?.Id;
                            }
                            break;
                        }
                    }
                }
                else if ((WorkflowActivityEnum)Enum.Parse(typeof(WorkflowActivityEnum), moduleActivity.AKey) == WorkflowActivityEnum.CmsSendToPOS)
                {
                    foreach (var param in parameters)
                    {
                        if ((WorkflowParams)Enum.Parse(typeof(WorkflowParams), param.PKey) == WorkflowParams.CmsSendToPOSUser)
                        {
                            if (String.IsNullOrEmpty(param.Value))
                            {
                                User hos = await GetHOSBySectorId((int)OperatingSectorTypeEnum.PrivateOperationalSector);
                                param.Value = hos?.Id;
                            }
                            break;
                        }
                    }
                }
                else if ((WorkflowActivityEnum)Enum.Parse(typeof(WorkflowActivityEnum), moduleActivity.AKey) == WorkflowActivityEnum.CmsTransferToPOButSendToFPForDecision)
                {
                    foreach (var param in parameters)
                    {
                        if ((WorkflowParams)Enum.Parse(typeof(WorkflowParams), param.PKey) == WorkflowParams.CmsSendToFPUser)
                        {
                            if (String.IsNullOrEmpty(param.Value))
                            {
                                User hos = await GetHOSBySectorId((int)OperatingSectorTypeEnum.FatwaPresidentOffice);
                                param.Value = hos?.Id;
                            }
                            break;
                        }
                    }
                }
                else if ((WorkflowActivityEnum)Enum.Parse(typeof(WorkflowActivityEnum), moduleActivity.AKey) == WorkflowActivityEnum.CmsApproveAndWork)
                {
                    foreach (var param in parameters)
                    {
                        if ((WorkflowParams)Enum.Parse(typeof(WorkflowParams), param.PKey) == WorkflowParams.CmsApproveTransferAndWorkUser)
                        {
                            if (String.IsNullOrEmpty(param.Value))
                            {
                                User hos = await GetHOSBySectorId((int)cmsApprovalTracking.SectorTypeId);
                                param.Value = hos?.Id;
                            }
                            break;
                        }
                    }
                }

                return parameters;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<List<WorkflowActivityParametersVM>> GetDMSInitiatorModificationParameters(int workflowActivityId, int? TriggerId, dynamic entity)
        {
            try
            {
                DmsAddedDocument dmsAddedDocument = System.Text.Json.JsonSerializer.Deserialize<DmsAddedDocument>(entity);
                WorkflowActivity workflowActivity = await _dbContext.WorkflowActivity.FindAsync(workflowActivityId);
                ModuleActivity moduleActivity = await _dbContext.ModuleActivity.FindAsync(workflowActivity.ActivityId);
                User user = await _dbContext.Users.Where(u => u.UserName == dmsAddedDocument.CreatedBy).FirstOrDefaultAsync();
                string StoredProc = $"exec pWorkflowActivityParametersList @workflowActivityId = '{workflowActivityId}'";
                List<WorkflowActivityParametersVM> parameters = await _dbContext.WorkflowActivityParametersVM.FromSqlRaw(StoredProc).ToListAsync();
                if ((WorkflowActivityEnum)Enum.Parse(typeof(WorkflowActivityEnum), moduleActivity.AKey) == WorkflowActivityEnum.InitiatorDocumentModification)
                {
                    foreach (var param in parameters)
                    {
                        if ((WorkflowParams)Enum.Parse(typeof(WorkflowParams), param.PKey) == WorkflowParams.InitiatorDocumentModification_User)
                        {
                            if (String.IsNullOrEmpty(param.Value))
                            {
                                User initiator = _accountRepository.GetUserByUserEmail(dmsAddedDocument.CreatedBy);
                                param.Value = initiator?.Id;
                            }
                            break;
                        }

                    }
                }
                return parameters;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<List<WorkflowActivityParametersVM>> GetCopyRequestParameters(int workflowActivityId, int? TriggerId, dynamic entity)
        {
            try
            {
                CmsApprovalTracking cmsApprovalTracking = System.Text.Json.JsonSerializer.Deserialize<CmsApprovalTracking>(entity);
                WorkflowActivity workflowActivity = await _dbContext.WorkflowActivity.FindAsync(workflowActivityId);
                ModuleActivity moduleActivity = await _dbContext.ModuleActivity.FindAsync(workflowActivity.ActivityId);
                var copyHistory = await _dbContext.CmsCopyHistories.Where(x => x.ReferenceId == cmsApprovalTracking.ReferenceId && x.ApprovalTrackingId == cmsApprovalTracking.Id).OrderBy(c => c.CreatedDate).FirstOrDefaultAsync();
                User user = await _dbContext.Users.Where(u => u.UserName == cmsApprovalTracking.CreatedBy).FirstOrDefaultAsync();
                string StoredProc = $"exec pWorkflowActivityParametersList @workflowActivityId = '{workflowActivityId}'";
                List<WorkflowActivityParametersVM> parameters = await _dbContext.WorkflowActivityParametersVM.FromSqlRaw(StoredProc).ToListAsync();
                if ((WorkflowActivityEnum)Enum.Parse(typeof(WorkflowActivityEnum), moduleActivity.AKey) == WorkflowActivityEnum.CmsSendCopyToSector)
                {
                    foreach (var param in parameters)
                    {
                        if ((WorkflowParams)Enum.Parse(typeof(WorkflowParams), param.PKey) == WorkflowParams.CmsSendCopyUser)
                        {
                            if (String.IsNullOrEmpty(param.Value))
                            {
                                User hos = await GetHOSBySectorId(cmsApprovalTracking.SectorTo);
                                param.Value = hos?.Id;
                            }
                            break;
                        }

                    }
                }
                else if ((WorkflowActivityEnum)Enum.Parse(typeof(WorkflowActivityEnum), moduleActivity.AKey) == WorkflowActivityEnum.CmsSendCopyToRecieverAndEndflow)
                {
                    foreach (var param in parameters)
                    {
                        if ((WorkflowParams)Enum.Parse(typeof(WorkflowParams), param.PKey) == WorkflowParams.CmsSendCopyToRecieverAndEndflowUser)
                        {
                            if (String.IsNullOrEmpty(param.Value))
                            {
                                User hos = await GetHOSBySectorId(copyHistory != null ? copyHistory.SectorTo : cmsApprovalTracking.SectorTo);
                                param.Value = hos?.Id;
                            }
                            break;
                        }

                    }
                }
                else if ((WorkflowActivityEnum)Enum.Parse(typeof(WorkflowActivityEnum), moduleActivity.AKey) == WorkflowActivityEnum.CmsSendCopyToInitiator || (WorkflowActivityEnum)Enum.Parse(typeof(WorkflowActivityEnum), moduleActivity.AKey) == WorkflowActivityEnum.CmsSendCopyToInitiatorAndEndflow)
                {
                    foreach (var param in parameters)
                    {
                        if ((WorkflowParams)Enum.Parse(typeof(WorkflowParams), param.PKey) == WorkflowParams.CmsSendCopyToInitiatorUser || (WorkflowParams)Enum.Parse(typeof(WorkflowParams), param.PKey) == WorkflowParams.CmsSendCopyToInitiatorAndEndflowUser)
                        {
                            if (String.IsNullOrEmpty(param.Value))
                            {

                                User hos = await GetHOSBySectorId(copyHistory != null ? copyHistory.SectorFrom : cmsApprovalTracking.SectorFrom);
                                param.Value = hos?.Id;


                            }
                            break;
                        }
                    }
                }
                else if ((WorkflowActivityEnum)Enum.Parse(typeof(WorkflowActivityEnum), moduleActivity.AKey) == WorkflowActivityEnum.CmsSendCopyToGS)
                {
                    foreach (var param in parameters)
                    {
                        if ((WorkflowParams)Enum.Parse(typeof(WorkflowParams), param.PKey) == WorkflowParams.CmsSendCopyToGSUser)
                        {
                            if (String.IsNullOrEmpty(param.Value))
                            {
                                if (cmsApprovalTracking.SectorTypeId < (int)OperatingSectorTypeEnum.CivilCommercialUnderFilingCases)
                                {
                                    User hos = await GetHOSBySectorId((int)OperatingSectorTypeEnum.AdministrativeGeneralSupervisor);
                                    param.Value = hos?.Id;
                                }
                                else if ((cmsApprovalTracking.SectorTypeId >= (int)OperatingSectorTypeEnum.CivilCommercialUnderFilingCases) &&
                                         (cmsApprovalTracking.SectorTypeId <= (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases))
                                {
                                    User hos = await GetHOSBySectorId((int)OperatingSectorTypeEnum.CivilCommercialGeneralSupervisor);
                                    param.Value = hos?.Id;
                                }
                            }
                            break;
                        }
                    }
                }
                else if ((WorkflowActivityEnum)Enum.Parse(typeof(WorkflowActivityEnum), moduleActivity.AKey) == WorkflowActivityEnum.CmsApproveCopyAndWork)
                {
                    foreach (var param in parameters)
                    {
                        if ((WorkflowParams)Enum.Parse(typeof(WorkflowParams), param.PKey) == WorkflowParams.CmsApproveCopyAndWorkUser)
                        {
                            if (String.IsNullOrEmpty(param.Value))
                            {
                                User hos = await GetHOSBySectorId((int)cmsApprovalTracking.SectorTypeId);
                                param.Value = hos?.Id;
                            }
                            break;
                        }
                    }
                }

                return parameters;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<List<WorkflowActivityParametersVM>> GetDraftDocumentParametersForConsultation(int workflowActivityId, int moduleId, dynamic entity)
        {
            try
            {
                ComsDraftedTemplate draftedTemplate = System.Text.Json.JsonSerializer.Deserialize<ComsDraftedTemplate>(entity);
                WorkflowActivity workflowActivity = await _dbContext.WorkflowActivity.FindAsync(workflowActivityId);
                ModuleActivity moduleActivity = await _dbContext.ModuleActivity.FindAsync(workflowActivity.ActivityId);
                User user = await _dbContext.Users.Where(u => u.UserName == draftedTemplate.CreatedBy).FirstOrDefaultAsync();
                string StoredProc = $"exec pWorkflowActivityParametersList @workflowActivityId = '{workflowActivityId}'";
                List<WorkflowActivityParametersVM> parameters = await _dbContext.WorkflowActivityParametersVM.FromSqlRaw(StoredProc).ToListAsync();
                if ((WorkflowActivityEnum)Enum.Parse(typeof(WorkflowActivityEnum), moduleActivity.AKey) == WorkflowActivityEnum.ComsReviewDraftDocument)
                {
                    foreach (var param in parameters)
                    {
                        if ((WorkflowParams)Enum.Parse(typeof(WorkflowParams), param.PKey) == WorkflowParams.ComsReviewDraftDocument_User)
                        {
                            if (String.IsNullOrEmpty(param.Value))
                            {
                                ConsultationAssignment caseAssignment = await _dbContext.ConsultationAssignments.Where(a => a.ReferenceId == draftedTemplate.ReferenceId && a.LawyerId == user.Id).FirstOrDefaultAsync();
                                param.Value = caseAssignment?.SupervisorId;
                            }
                            break;
                        }
                    }
                }
                else if ((WorkflowActivityEnum)Enum.Parse(typeof(WorkflowActivityEnum), moduleActivity.AKey) == WorkflowActivityEnum.ComsReviewDraftDocumentHOS)
                {
                    foreach (var param in parameters)
                    {
                        if ((WorkflowParams)Enum.Parse(typeof(WorkflowParams), param.PKey) == WorkflowParams.ComsReviewDraftDocumentHos_User)
                        {
                            if (String.IsNullOrEmpty(param.Value))
                            {
                                User hos = await GetHOSBySectorId(draftedTemplate.SectorTypeId);
                                param.Value = hos?.Id;
                            }
                            break;
                        }
                    }
                }
                return parameters;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<WorkflowConditionsOptionVM>> GetWorkflowConditionOptions(Guid ReferneceId, int StatusId)
        {
            try
            {
                string StoredProc = $"exec pWorkflowConditionsOptionsByReferenceId @ReferenceId = '{ReferneceId}' , @StatusId ='{StatusId}'";
                return await _dbContext.WorkflowConditionsOptionVMs.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<List<WorkflowActivityOptionVM>> GetWorkflowActivityOptions(int ActivityId)
        {
            try
            {
                string StoredProc = $"exec pWorkflowActivityOptionsByActivityId @ActivityId = '{ActivityId}'";
                return await _dbContext.WorkflowActivityOptionVMs.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region GetRequest TypeId From ApporvalTracking
        //<History Author = 'Hassan Abbas' Date='2023-12-19' Version="1.0" Branch="master"> Get Request Type Id form Approval Tracking</History>
        public async Task<int?> GetRespectiveSectorIdFromApporvalTracking(CmsApprovalTracking cmsApprovalTracking, DatabaseContext dbContext)
        {
            try
            {
                int? requestTypeId = 0;
                int? courtTypeId = 0;
                if (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest)
                {
                    var caseRequest = await dbContext.CaseRequests.Where(x => x.RequestId == cmsApprovalTracking.ReferenceId).FirstOrDefaultAsync();
                    requestTypeId = caseRequest?.RequestTypeId;
                    courtTypeId = caseRequest?.CourtTypeId;
                }
                else if (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseFile)
                {
                    var caseFile = await dbContext.CaseFiles.Where(x => x.FileId == cmsApprovalTracking.ReferenceId).FirstOrDefaultAsync();
                    if (caseFile != null)
                    {
                        var caseRequest = await dbContext.CaseRequests.Where(x => x.RequestId == caseFile.RequestId).FirstOrDefaultAsync();
                        requestTypeId = caseRequest?.RequestTypeId;
                        courtTypeId = caseRequest?.CourtTypeId;
                    }
                }
                else if (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest)
                {
                    requestTypeId = await dbContext.ConsultationRequests.Where(x => x.ConsultationRequestId == cmsApprovalTracking.ReferenceId).Select(x => x.RequestTypeId).FirstOrDefaultAsync();
                }
                else if (cmsApprovalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationFile)
                {
                    var caseRequest = await dbContext.ConsultationFiles.Where(x => x.FileId == cmsApprovalTracking.ReferenceId).FirstOrDefaultAsync();
                    if (caseRequest != null)
                        requestTypeId = await dbContext.ConsultationRequests.Where(x => x.ConsultationRequestId == caseRequest.RequestId).Select(x => x.RequestTypeId).FirstOrDefaultAsync();
                }
                return CaseConsultationExtension.GetSectorIdBasedOnRequestAndCourtId((int)requestTypeId, (int)courtTypeId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Workflow Instance

        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Proc to get Workflow Document Instances</History>
        public async Task<List<WorkflowInstanceDocumentVM>> GetWorkflowInstanceDocuments(int PageSize, int PageNumber)
        {
            try
            {
                string StoredProc = $"exec pWorkflowInstancesDocumentList @PageSize = '{PageSize}', @PageNumber = '{PageNumber}'";
                return await _dbContext.WorkflowInstanceDocumentVM.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<WorkflowActivity> GetInstanceCurrentActivity(Guid referenceId)
        {
            try
            {
                string StoredProc = $"exec pWorkflowInstanceCurrentActivity @referenceId = '{referenceId}'";
                var list = await _dbContext.WorkflowActivity.FromSqlRaw(StoredProc).ToListAsync();
                return list.FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateWorkflowInstanceStatus(Guid referenceId, int statusId)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            //update instance status
                            var isntance = _dbContext.WorkflowInstance.Where(i => i.ReferenceId == referenceId).FirstOrDefault();
                            isntance.StatusId = statusId;
                            _dbContext.Entry(isntance).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();

                            //get workflow from current instance
                            Workflow workflow = await _dbContext.Workflow.FindAsync(isntance.WorkflowId);
                            if (workflow.StatusId == (int)WorkflowStatusEnum.Suspended)
                            {
                                //if workflow is suspended check to make it inactive
                                var runningInstances = await _dbContext.WorkflowInstance.Where(i => i.WorkflowId == workflow.WorkflowId && i.StatusId == (int)WorkflowInstanceStatusEnum.InProgress).ToListAsync();

                                //if no instances then inactive workflow
                                if (runningInstances?.Count() <= 0)
                                {
                                    workflow.StatusId = (int)WorkflowStatusEnum.Inactive;
                                    _dbContext.Entry(workflow).State = EntityState.Modified;
                                    await _dbContext.SaveChangesAsync();
                                }
                            }

                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<WorkflowInstance> GetCurrentInstanceByReferneceId(Guid referenceId)
        {
            try
            {
                var result = await _dbContext.WorkflowInstance.Where(x => x.ReferenceId == referenceId).FirstOrDefaultAsync();
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return new WorkflowInstance();
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Lds Document
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Update Legal Document</History>
        public async Task UpdateDocumentInstance(LegalLegislation document)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            //document
                            if (document.Legislation_Flow_Status > (int)LegislationFlowStatusEnum.Rejected && document.Legislation_Flow_Status < (int)LegislationFlowStatusEnum.Published)
                            {
                                document.Legislation_Flow_Status = (int)LegislationFlowStatusEnum.NeedModification;
                            }
                            if (document.Legislation_Flow_Status == (int)LegislationFlowStatusEnum.Unpublished)
                            {
                                document.Legislation_Flow_Status = (int)LegislationFlowStatusEnum.Unpublished;
                            }
                            _dbContext.Entry(document).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();

                            //// For Notification
                            //document.NotificationParameter.Entity = new LegalLegislation().GetType().Name + "Document"; 
                            //document.NotificationParameter.Type = _dbContext.legalLegislationTypes.Where(x => x.Id == document.Legislation_Type).Select(x => x.Name_En + "/" + x.Name_Ar).FirstOrDefault();  
                            //document.NotificationParameter.Status = _dbContext.legalLegislationFlowStatuss.Where(x => x.Id == document.Legislation_Flow_Status).Select(x => x.Name_En + "/" + x.Name_Ar).FirstOrDefault();

                            //workflow instance
                            var isntance = _dbContext.WorkflowInstance.Where(i => i.ReferenceId == document.LegislationId).FirstOrDefault();

                            if (document.WorkflowActivityId.HasValue && document.WorkflowActivityId > 0)
                            {
                                isntance.WorkflowActivityId = (int)document.WorkflowActivityId;
                                isntance.IsSlaExecuted = false;
                                SLA sla = await _dbContext.SLA.Where(s => s.WorkflowActivityId == isntance.WorkflowActivityId).FirstOrDefaultAsync();
                                if (sla != null)
                                {
                                    isntance.ApplySla = true;
                                    isntance.SlaStartDate = DateTime.Now.Date.AddDays(1);
                                    isntance.SlaEndDate = isntance.SlaStartDate.AddDays(sla.Duration - 1);
                                }
                                else
                                {
                                    isntance.ApplySla = false;
                                    isntance.SlaStartDate = DateTime.Now.Date;
                                    isntance.SlaEndDate = DateTime.Now.Date;
                                }
                            }
                            if (document.WorkflowInstanceStatusId != null && (int)document.WorkflowInstanceStatusId > 0)
                                isntance.StatusId = (int)document.WorkflowInstanceStatusId;
                            _dbContext.Entry(isntance).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();

                            //get workflow from current instance
                            Workflow workflow = await _dbContext.Workflow.FindAsync(isntance.WorkflowId);
                            if (workflow.StatusId == (int)WorkflowStatusEnum.Suspended)
                            {
                                //if workflow is suspended check to make it inactive
                                var runningInstances = await _dbContext.WorkflowInstance.Where(i => i.WorkflowId == workflow.WorkflowId && i.StatusId == (int)WorkflowInstanceStatusEnum.InProgress).ToListAsync();

                                //if no instances then inactive workflow
                                if (runningInstances?.Count() <= 0)
                                {
                                    workflow.StatusId = (int)WorkflowStatusEnum.Inactive;
                                    _dbContext.Entry(workflow).State = EntityState.Modified;
                                    await _dbContext.SaveChangesAsync();
                                }
                            }
                            if (document.Legislation_Flow_Status != (int)LegislationFlowStatusEnum.New && document.Legislation_Flow_Status != (int)LegislationFlowStatusEnum.PartiallyCompleted && document.Legislation_Flow_Status != (int)LegislationFlowStatusEnum.InReview)
                                await InserCommentsByDocument(document, _dbContext);
                            //// For Notification
                            document.NotificationParameter.Type = _dbContext.legalLegislationTypes.Where(x => x.Id == document.Legislation_Type).Select(x => x.Name_En + "/" + x.Name_Ar).FirstOrDefault();
                            document.NotificationParameter.Entity = "Legal Legislation";
                            document.NotificationParameter.LegislationNumber = document.Legislation_Number;
                            //var result = await _dbContext.legalLegislations.FindAsync(document.LegislationId);
                            //document.ModifiedBy = result.ModifiedBy;
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task InserCommentsByDocument(LegalLegislation document, DatabaseContext _dbContext)
        {
            try
            {
                if (document.Legislation_Comment != null)
                {
                    LdsDocumentComment commentobj = new LdsDocumentComment();
                    if (document.Legislation_Flow_Status != (int)LegislationFlowStatusEnum.Published)
                    {
                        commentobj.CommentId = Guid.NewGuid();
                        commentobj.DocumentId = document.LegislationId;
                        ///  commentobj.Comment = document.Comment;
                        commentobj.Status = document.Legislation_Flow_Status.ToString();
                        commentobj.Reason = document.Legislation_Comment;
                        commentobj.CreatedDate = document.AddedDate;
                        commentobj.Createdby = document.AddedBy;
                        await _dbContext.LdsDocumentComment.AddAsync(commentobj);

                    }
                    await _dbContext.SaveChangesAsync();
                }
                else
                {

                }

                //LdsDocumentComment commentobj = new LdsDocumentComment();
                //    commentobj.DocumentId= document.DocumentId;
                //    commentobj.Comment = document.Comment;
                //    commentobj.Status = document.Status.ToString();
                //    commentobj.Reason = document.Reason;
                //    commentobj.CreatedDate = document.CreatedDate;
                //    commentobj.Createdby = document.IssuedBy;
                //    await _dbContext.LdsDocumentComment.AddAsync(commentobj);

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Lps Principle
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Update Legal Principle</History>
        public async Task UpdatePrincipleInstance(LLSLegalPrincipleSystem principle)
        {
            try
            {
                using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var existingPrinciple = await _dbContext.LLSLegalPrinciples
                                                                .Where(x => x.PrincipleId == principle.PrincipleId)
                                                                .FirstOrDefaultAsync();

                        if (existingPrinciple != null)
                        {
                            // Update only the necessary fields
                            if (principle.FlowStatus > (int)PrincipleFlowStatusEnum.Reject && principle.FlowStatus < (int)PrincipleFlowStatusEnum.Publish)
                            {
                                principle.FlowStatus = (int)PrincipleFlowStatusEnum.NeedModification;
                            }
                            else if (principle.FlowStatus == (int)PrincipleFlowStatusEnum.Unpublished)
                            {
                                principle.FlowStatus = (int)PrincipleFlowStatusEnum.Unpublished;
                            }
                            existingPrinciple.Principle_Comment = principle.Principle_Comment;
                            existingPrinciple.FlowStatus = principle.FlowStatus;
                            existingPrinciple.UserId = principle.UserId;
                            existingPrinciple.RoleId = principle.RoleId;
                            // Other property updates if necessary
                            _dbContext.Entry(existingPrinciple).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();

                            // Notification parameter
                            principle.NotificationParameter.Entity = new LLSLegalPrincipleSystem().GetType().Name + "Document";
                            principle.NotificationParameter.Type = await _dbContext.LegalPrincipleFlowStatuses
                                .Where(x => x.Id == existingPrinciple.FlowStatus)
                                .Select(x => x.Name_En + "/" + x.Name_Ar)
                                .FirstOrDefaultAsync();
                            principle.NotificationParameter.Status = principle.NotificationParameter.Type;
                            principle.NotificationParameter.PrincipleNumber = existingPrinciple.PrincipleNumber.ToString();

                            // Workflow instance
                            var instance = await _dbContext.WorkflowInstance
                                .Where(i => i.ReferenceId == principle.PrincipleId)
                                .FirstOrDefaultAsync();

                            if (instance != null && principle.WorkflowActivityId.HasValue && principle.WorkflowActivityId > 0)
                            {
                                instance.WorkflowActivityId = (int)principle.WorkflowActivityId;
                                instance.IsSlaExecuted = false;
                                var sla = await _dbContext.SLA
                                    .Where(s => s.WorkflowActivityId == instance.WorkflowActivityId)
                                    .FirstOrDefaultAsync();
                                if (sla != null)
                                {
                                    instance.ApplySla = true;
                                    instance.SlaStartDate = DateTime.Now.Date.AddDays(1);
                                    instance.SlaEndDate = instance.SlaStartDate.AddDays(sla.Duration - 1);
                                }
                                else
                                {
                                    instance.ApplySla = false;
                                    instance.SlaStartDate = DateTime.Now.Date;
                                    instance.SlaEndDate = DateTime.Now.Date;
                                }
                            }

                            if (principle.WorkflowInstanceStatusId.HasValue && principle.WorkflowInstanceStatusId > 0)
                            {
                                instance.StatusId = (int)principle.WorkflowInstanceStatusId;
                            }

                            _dbContext.Entry(instance).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();

                            // Workflow status update
                            var workflow = await _dbContext.Workflow
                                .FindAsync(instance.WorkflowId);
                            if (workflow.StatusId == (int)WorkflowStatusEnum.Suspended)
                            {
                                var runningInstances = await _dbContext.WorkflowInstance
                                    .Where(i => i.WorkflowId == workflow.WorkflowId && i.StatusId == (int)WorkflowInstanceStatusEnum.InProgress)
                                    .ToListAsync();

                                if (runningInstances?.Count <= 0)
                                {
                                    workflow.StatusId = (int)WorkflowStatusEnum.Inactive;
                                    _dbContext.Entry(workflow).State = EntityState.Modified;
                                    await _dbContext.SaveChangesAsync();
                                }
                            }

                            // Handle comments
                            if (!string.IsNullOrEmpty(principle.Principle_Comment))
                            {
                                var comment = new LpsPrincipleComment
                                {
                                    CommentId = Guid.NewGuid(),
                                    PrincipleId = principle.PrincipleId,
                                    Comment = principle.Principle_Comment,
                                    Status = principle.FlowStatus.ToString(),
                                    Reason = principle.Principle_Comment,
                                    CreatedDate = principle.CreatedDate,
                                    Createdby = principle.CreatedBy
                                };

                                await _dbContext.LpsPrincipleComments.AddAsync(comment);
                                await _dbContext.SaveChangesAsync();
                            }

                            await transaction.CommitAsync();
                        }
                        else
                        {
                            throw new InvalidOperationException();
                        }
                    }
                    catch (Exception)
                    {
                        await transaction.RollbackAsync();
                        throw;
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        #endregion

        #region Cms Case Draft
        //<History Author = 'Hassan Abbas' Date='2022-06-02' Version="1.0" Branch="master"> Update Case Draft</History>
        public async Task<string> UpdateCaseDraftInstance(CmsDraftedTemplate draft, string userName)
        {
            try
            {
                using (_dbContext)
                {

                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            if (draft.DraftedTemplateVersion.StatusId == (int)DraftVersionStatusEnum.Reject
                                 || draft.DraftedTemplateVersion.StatusId == (int)DraftVersionStatusEnum.Approve)
                            {
                                if (draft.DraftEntityType == (int)DraftEntityTypeEnum.RequestNeedMoreInfo || draft.DraftEntityType == (int)DraftEntityTypeEnum.FileNeedMoreInfo || draft.DraftEntityType == (int)DraftEntityTypeEnum.CaseNeedMoreInfo)
                                {
                                    int colorId = draft.DraftedTemplateVersion.StatusId == (int)DraftVersionStatusEnum.Approve ? (int)CommunicationColorEnum.LightGreen : (int)CommunicationColorEnum.Red;
                                    UpdateCommunicationColor(draft, colorId, _dbContext);
                                }
                            }
                            if (draft.DraftedTemplateVersion.StatusId == (int)DraftVersionStatusEnum.Published)
                            {
                                var previouseVersion = await _dbContext.CmsDraftedTemplateVersions.Where(x => x.VersionId == draft.DraftedTemplateVersion.OldVersionId).FirstOrDefaultAsync();
                                if (previouseVersion != null)
                                {
                                    previouseVersion.StatusId = draft.DraftedTemplateVersion.OldStatusId;
                                    previouseVersion.ModifiedBy = draft.DraftedTemplateVersion.CreatedBy;
                                    previouseVersion.ModifiedDate = DateTime.Now;
                                    _dbContext.CmsDraftedTemplateVersions.Update(previouseVersion);
                                    await _dbContext.SaveChangesAsync();

                                }
                                draft.DraftedTemplateVersion.ReviewerUserId = "";
                                draft.DraftedTemplateVersion.ReviewerRoleId = "";
                                _dbContext.CmsDraftedTemplateVersions.Add(draft.DraftedTemplateVersion);
                                await _dbContext.SaveChangesAsync();
                                await CreateDraftTemplateHistoryLogs(draft, (int)DraftActionIdEnum.Published, draft.DraftedTemplateVersion.DraftActionId);
                            }
                            else
                            {
                                _dbContext.CmsDraftedTemplateVersions.Update(draft.DraftedTemplateVersion);
                                await _dbContext.SaveChangesAsync();
                                await CreateDraftTemplateHistoryLogs(draft, draft.DraftedTemplateVersion.DraftActionId);
                            }
                            if (!string.IsNullOrEmpty(draft.Reason))
                            {
                                await UpdateDraftdteamplateReasonTable(draft, _dbContext);
                            }
                            if (!string.IsNullOrEmpty(draft.Opinion))
                            {
                                await UpdateDraftdtemplateOpinionTable(draft, _dbContext);
                            }

                            if (draft.DraftedTemplateVersion.StatusId == (int)DraftVersionStatusEnum.Published)
                            {
                                draft.Payload = await ProcessDraftEntity(draft, _dbContext);
                            }
                            //workflow instance
                            var isntance = _dbContext.WorkflowInstance.Where(i => i.ReferenceId == draft.Id).FirstOrDefault();

                            if (draft.WorkflowActivityId.HasValue && draft.WorkflowActivityId > 0)
                            {
                                isntance.WorkflowActivityId = (int)draft.WorkflowActivityId;
                                isntance.IsSlaExecuted = false;
                                SLA sla = await _dbContext.SLA.Where(s => s.WorkflowActivityId == isntance.WorkflowActivityId).FirstOrDefaultAsync();
                                if (sla != null)
                                {
                                    isntance.ApplySla = true;
                                    isntance.SlaStartDate = DateTime.Now.Date.AddDays(1);
                                    isntance.SlaEndDate = isntance.SlaStartDate.AddDays(sla.Duration - 1);
                                }
                                else
                                {
                                    isntance.ApplySla = false;
                                    isntance.SlaStartDate = DateTime.Now.Date;
                                    isntance.SlaEndDate = DateTime.Now.Date;
                                }
                            }
                            if (draft.WorkflowInstanceStatusId != null && (int)draft.WorkflowInstanceStatusId > 0)
                                isntance.StatusId = (int)draft.WorkflowInstanceStatusId;
                            _dbContext.Entry(isntance).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();

                            //get workflow from current instance
                            Workflow workflow = await _dbContext.Workflow.FindAsync(isntance.WorkflowId);
                            if (workflow.StatusId == (int)WorkflowStatusEnum.Suspended)
                            {
                                //if workflow is suspended check to make it inactive
                                var runningInstances = await _dbContext.WorkflowInstance.Where(i => i.WorkflowId == workflow.WorkflowId && i.StatusId == (int)WorkflowInstanceStatusEnum.InProgress).ToListAsync();

                                //if no instances then inactive workflow
                                if (runningInstances?.Count() <= 0)
                                {
                                    workflow.StatusId = (int)WorkflowStatusEnum.Inactive;
                                    _dbContext.Entry(workflow).State = EntityState.Modified;
                                    await _dbContext.SaveChangesAsync();
                                }
                            }
                            transaction.Commit();
                            // For Notification 
                            PrepareNotificationParameter(draft, _dbContext);
                            return draft.Payload;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        //<History Author = 'Umer Zaman' Date='2023-01-31' Version="1.0" Branch="master">Save drafted template reject reason</History>

        private async Task UpdateDraftdteamplateReasonTable(CmsDraftedTemplate draft, DatabaseContext dbContext)
        {
            try
            {
                CmsDraftedTemplateReason? Obj = new CmsDraftedTemplateReason();
                Obj.DraftedTemplateVersionId = draft.DraftedTemplateVersion.VersionId;
                Obj.VersionNumber = draft.DraftedTemplateVersion.VersionNumber;
                Obj.Reason = draft.Reason;
                Obj.StatusId = draft.DraftedTemplateVersion.StatusId;
                if (!string.IsNullOrWhiteSpace(draft.DraftedTemplateVersion.ModifiedBy))
                {
                    Obj.CreatedBy = draft.DraftedTemplateVersion.ModifiedBy;
                }
                else
                {
                    Obj.CreatedBy = draft.DraftedTemplateVersion.CreatedBy;
                }
                Obj.CreatedDate = DateTime.Now;
                Obj.IsDeleted = false;
                await dbContext.CmsDraftedTemplateReasons.AddAsync(Obj);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task UpdateDraftdtemplateOpinionTable(CmsDraftedTemplate draft, DatabaseContext dbContext)
        {
            try
            {
                DraftExpertOpinion? Obj = new DraftExpertOpinion();
                Obj.Opinion = draft.Opinion;
                Obj.DraftedTemplateVersionId = draft.DraftedTemplateVersion.VersionId;
                if (!string.IsNullOrWhiteSpace(draft.DraftedTemplateVersion.ModifiedBy))
                {
                    Obj.CreatedBy = draft.DraftedTemplateVersion.ModifiedBy;
                }
                else
                {
                    Obj.CreatedBy = draft.DraftedTemplateVersion.CreatedBy;
                }
                Obj.CreatedDate = DateTime.Now;
                await dbContext.DraftExpertOpinions.AddAsync(Obj);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }



        //<History Author = 'Hassan Abbas' Date='2022-12-21' Version="1.0" Branch="master"> Get HOS By Sector</History>
        public async Task<User> GetHOSBySectorId(int sectorTypeId)
        {
            try
            {
                string StoreProc = $"exec pGetHOSBSectorId @sectorTypeId = '{sectorTypeId}'";
                var users = await _dbContext.Users.FromSqlRaw(StoreProc).ToListAsync();
                return users.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Coms Consultation Draft
        //<History Author = 'Muhammad Zaeem' Date='2023-22-02' Version="1.0" Branch="master"> Update COnsultation Draft</History>
        public async Task<string> UpdateConsultationDraftInstance(ComsDraftedTemplate draft, string username)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            _dbContext.Entry(draft).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();

                            //Draft Approval By HOS
                            if (draft.StatusId == (int)CaseDraftDocumentStatusEnum.ApproveByHOS)
                            {
                                //await SaveComsDraftTemplateToDocument(draft, _dbContext);
                                draft.Payload = await ProcessComsDraftEntity(draft, _dbContext, username);
                            }

                            //workflow instance
                            var isntance = _dbContext.WorkflowInstance.Where(i => i.ReferenceId == draft.Id).FirstOrDefault();

                            if (draft.WorkflowActivityId.HasValue && draft.WorkflowActivityId > 0)
                            {
                                isntance.WorkflowActivityId = (int)draft.WorkflowActivityId;
                                isntance.IsSlaExecuted = false;
                                SLA sla = await _dbContext.SLA.Where(s => s.WorkflowActivityId == isntance.WorkflowActivityId).FirstOrDefaultAsync();
                                if (sla != null)
                                {
                                    isntance.ApplySla = true;
                                    isntance.SlaStartDate = DateTime.Now.Date.AddDays(1);
                                    isntance.SlaEndDate = isntance.SlaStartDate.AddDays(sla.Duration - 1);
                                }
                                else
                                {
                                    isntance.ApplySla = false;
                                    isntance.SlaStartDate = DateTime.Now.Date;
                                    isntance.SlaEndDate = DateTime.Now.Date;
                                }
                            }
                            if (draft.WorkflowInstanceStatusId != null && (int)draft.WorkflowInstanceStatusId > 0)
                                isntance.StatusId = (int)draft.WorkflowInstanceStatusId;
                            _dbContext.Entry(isntance).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();

                            //get workflow from current instance
                            Workflow workflow = await _dbContext.Workflow.FindAsync(isntance.WorkflowId);
                            if (workflow.StatusId == (int)WorkflowStatusEnum.Suspended)
                            {
                                //if workflow is suspended check to make it inactive
                                var runningInstances = await _dbContext.WorkflowInstance.Where(i => i.WorkflowId == workflow.WorkflowId && i.StatusId == (int)WorkflowInstanceStatusEnum.InProgress).ToListAsync();

                                //if no instances then inactive workflow
                                if (runningInstances?.Count() <= 0)
                                {
                                    workflow.StatusId = (int)WorkflowStatusEnum.Inactive;
                                    _dbContext.Entry(workflow).State = EntityState.Modified;
                                    await _dbContext.SaveChangesAsync();
                                }
                            }
                            if (!string.IsNullOrEmpty(draft.Reason))
                            {
                                await UpdateComsDraftdteamplateReasonTable(draft, _dbContext);
                            }
                            transaction.Commit();
                            return draft.Payload;
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        //<History Author = 'Muhammad Zaeem' Date='2023-01-31' Version="1.0" Branch="master">Save drafted template reject reason</History>

        private async Task UpdateComsDraftdteamplateReasonTable(ComsDraftedTemplate draft, DatabaseContext dbContext)
        {
            try
            {
                ComsDraftedTemplateReason? Obj = new ComsDraftedTemplateReason();
                Obj.DraftedTemplateId = draft.Id;
                Obj.VersionNumber = draft.VersionNumber;
                Obj.Reason = draft.Reason;
                Obj.StatusId = draft.StatusId;
                if (!string.IsNullOrWhiteSpace(draft.ModifiedBy.ToString()))
                {
                    Obj.CreatedBy = draft.ModifiedBy;
                }
                else
                {
                    Obj.CreatedBy = draft.CreatedBy;
                }
                Obj.CreatedDate = DateTime.Now;
                Obj.IsDeleted = false;
                await dbContext.ComsDraftedTemplateReasons.AddAsync(Obj);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region DMS Review Document
        public async Task<string> UpdateDMSDocumentInstance(DmsAddedDocument document)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            if (document.DocumentVersion.StatusId == (int)DocumentStatusEnum.Rejected)
                            {
                                var temp = _dbDmsContext.DmsAddedDocumentsVersion.Where(i => i.Id == document.DocumentVersion.Id).FirstOrDefault();
                                temp.StatusId = (int)DocumentStatusEnum.Rejected;
                                temp.ReviewerUserId = document.DocumentVersion.ReviewerUserId;
                                temp.ReviewerRoleId = document.DocumentVersion.ReviewerRoleId;
                                temp.CreatedBy = document.DocumentVersion.CreatedBy;
                                temp.CreatedDate = document.DocumentVersion.CreatedDate;
                                temp.ModifiedBy = document.DocumentVersion.ModifiedBy;
                                temp.ModifiedDate = document.DocumentVersion.ModifiedDate;
                                _dbDmsContext.Entry(temp).State = EntityState.Modified;
                                await _dbDmsContext.SaveChangesAsync();
                            }
                            if (document.DocumentVersion.StatusId == (int)DocumentStatusEnum.Approved)
                            {
                                var temp = _dbDmsContext.DmsAddedDocumentsVersion.Where(i => i.Id == document.DocumentVersion.Id).FirstOrDefault();
                                temp.StatusId = (int)DocumentStatusEnum.Approved;
                                temp.ReviewerUserId = document.DocumentVersion.ReviewerUserId;
                                temp.ReviewerRoleId = document.DocumentVersion.ReviewerRoleId;
                                temp.CreatedBy = document.DocumentVersion.CreatedBy;
                                temp.CreatedDate = document.DocumentVersion.CreatedDate;
                                temp.ModifiedBy = document.DocumentVersion.ModifiedBy;
                                temp.ModifiedDate = document.DocumentVersion.ModifiedDate;
                                _dbDmsContext.Entry(temp).State = EntityState.Modified;
                                await _dbDmsContext.SaveChangesAsync();
                            }
                            if (document.DocumentVersion.StatusId == (int)DocumentStatusEnum.Published)
                            {
                                var isntance_temp = _dbContext.WorkflowInstance.Where(i => i.ReferenceId == document.Id).FirstOrDefault();
                                #region For Previouse Version Hide from Reviewer Document List
                                var previousVersion = await _dbDmsContext.DmsAddedDocumentVersions.Where(x => x.AddedDocumentId == document.Id && x.Id == document.DocumentVersion.Id).FirstOrDefaultAsync();
                                if (previousVersion != null)
                                {
                                    previousVersion.ReviewerUserId = "";
                                    previousVersion.ReviewerRoleId = "";
                                    _dbDmsContext.Entry(previousVersion).State = EntityState.Modified;
                                    await _dbDmsContext.SaveChangesAsync();
                                }
                                #endregion
                                document.DocumentVersion.Id = Guid.NewGuid();
                                await _dbDmsContext.DmsAddedDocumentsVersion.AddAsync(document.DocumentVersion);
                                await _dbDmsContext.SaveChangesAsync();
                                if (document.DocumentVersion.Id != Guid.Empty && document.UserLoginState != document.CreatedBy)
                                {
                                    isntance_temp.ReferenceId = document.Id;
                                    _dbContext.Entry(isntance_temp).State = EntityState.Modified;
                                    await _dbContext.SaveChangesAsync();
                                }
                            }
                            if (document.DocumentVersion.Id != Guid.Empty && document.DocumentVersion.StatusId == (int)DocumentStatusEnum.InReview)
                            {
                                var temp = _dbDmsContext.DmsAddedDocumentsVersion.Where(i => i.Id == document.DocumentVersion.Id).FirstOrDefault();
                                temp.StatusId = (int)DocumentStatusEnum.InReview;
                                temp.ReviewerUserId = document.DocumentVersion.ReviewerUserId;
                                temp.ReviewerRoleId = document.DocumentVersion.ReviewerRoleId;
                                temp.CreatedBy = document.DocumentVersion.CreatedBy;
                                temp.CreatedDate = document.DocumentVersion.CreatedDate;
                                temp.ModifiedBy = document.DocumentVersion.ModifiedBy;
                                temp.ModifiedDate = document.DocumentVersion.ModifiedDate;
                                _dbDmsContext.Entry(temp).State = EntityState.Modified;
                                await _dbDmsContext.SaveChangesAsync();
                            }
                            //workflow instance
                            var isntance = _dbContext.WorkflowInstance.Where(i => i.ReferenceId == document.Id).FirstOrDefault();
                            if (document.WorkflowActivityId.HasValue && document.WorkflowActivityId > 0)
                            {
                                isntance.WorkflowActivityId = (int)document.WorkflowActivityId;
                                isntance.IsSlaExecuted = false;
                                SLA sla = await _dbContext.SLA.Where(s => s.WorkflowActivityId == isntance.WorkflowActivityId).FirstOrDefaultAsync();
                                if (sla != null)
                                {
                                    isntance.ApplySla = true;
                                    isntance.SlaStartDate = DateTime.Now.Date.AddDays(1);
                                    isntance.SlaEndDate = isntance.SlaStartDate.AddDays(sla.Duration - 1);
                                }
                                else
                                {
                                    isntance.ApplySla = false;
                                    isntance.SlaStartDate = DateTime.Now.Date;
                                    isntance.SlaEndDate = DateTime.Now.Date;
                                }
                            }
                            //if (document.IsEndofFlow)
                            //{
                            //    isntance.StatusId = (int)WorkflowInstanceStatusEnum.Success;
                            //}
                            if (document.WorkflowInstanceStatusId != null && (int)document.WorkflowInstanceStatusId > 0)
                                isntance.StatusId = (int)document.WorkflowInstanceStatusId;
                            _dbContext.Entry(isntance).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transaction.Commit();
                            if (!string.IsNullOrEmpty(document.DocumentVersion.Reason))
                            {
                                await UpdateDMSDocumenttemplateReasonTable(document, _dbDmsContext);
                            }
                            // For Notification
                            document.NotificationParameter.Type = await _dbDmsContext.AttachmentType.Where(x => x.AttachmentTypeId == document.AttachmentTypeId)
                                                                .Select(x => x.Type_En + "/" + x.Type_Ar)
                                                                .FirstOrDefaultAsync();
                            return document.Payload;
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task UpdateDMSDocumenttemplateReasonTable(DmsAddedDocument document, DmsDbContext dbContext)
        {
            try
            {
                DmsAddedDocumentReason? Obj = new DmsAddedDocumentReason();
                Obj.AddedDocumentVersionId = document.DocumentVersion.Id;
                Obj.Reason = document.DocumentVersion.Reason;
                if (!string.IsNullOrWhiteSpace(document.ModifiedBy))
                {
                    Obj.CreatedBy = document.DocumentVersion.ModifiedBy;
                }
                else
                {
                    Obj.CreatedBy = document.CreatedBy;
                }
                Obj.CreatedDate = DateTime.Now;
                Obj.IsDeleted = false;
                await dbContext.DmsAddedDocumentReasons.AddAsync(Obj);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<string>> GetUsersByRoleIdandSectorId(string RoleId, int sectorTypeId)
        {
            try
            {
                List<UserRole> userRoleList = new List<UserRole>();
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                string StoreProc = $"exec pGetUsersByRoleIdandSectorId @RoleId='{RoleId}',@sectorTypeId = '{sectorTypeId}'";
                userRoleList = await _DbContext.UserRoles.FromSqlRaw(StoreProc).ToListAsync();
                return userRoleList.Select(userRole => userRole.UserId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Update Workflow Status
        //<History Author = 'Muhammad Zaeem' Date='2023-03-11' Version="1.0" Branch="master"> Update Workflow Status</History>
        public async Task UpdateWorkflowStatus(int workflowId, int statusId)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            if (statusId == (int)WorkflowStatusEnum.Active)
                            {
                                Workflow workflow = await _dbContext.Workflow.FindAsync(workflowId);
                                WorkflowTrigger workflowTrigger = _dbContext.WorkflowTrigger.Where(w => w.WorkflowId == workflowId).FirstOrDefault();
                                List<WorkflowAttachmentType> workflowAttachmentTypes = _dbContext.WorkflowAttachmentType.Where(w => w.WorkflowId == workflowId).ToList();
                                ModuleTrigger moduleTrigger = await _dbContext.ModuleTrigger.FindAsync(workflowTrigger.ModuleTriggerId);
                                if (!workflowAttachmentTypes.Any())
                                {
                                    var activeWorkflow = GetActiveWorkflows(moduleTrigger.ModuleTriggerId, null, workflow.SubModuleId).Result.FirstOrDefault();
                                    if (activeWorkflow != null)
                                    {
                                        List<WorkflowInstance> workflowInstances = await _dbContext.WorkflowInstance.Where(i => i.WorkflowId == activeWorkflow.WorkflowId && i.StatusId == (int)WorkflowInstanceStatusEnum.InProgress).ToListAsync();
                                        if (workflowInstances?.Count() > 0)
                                        {
                                            Workflow tobeSuspendedWorkflow = await _dbContext.Workflow.FindAsync(activeWorkflow.WorkflowId);
                                            tobeSuspendedWorkflow.StatusId = (int)WorkflowStatusEnum.Suspended;
                                            _dbContext.Entry(tobeSuspendedWorkflow).State = EntityState.Modified;
                                            await _dbContext.SaveChangesAsync();
                                        }
                                        else
                                        {
                                            Workflow tobeSuspendedWorkflow = await _dbContext.Workflow.FindAsync(activeWorkflow.WorkflowId);
                                            tobeSuspendedWorkflow.StatusId = (int)WorkflowStatusEnum.Inactive;
                                            _dbContext.Entry(tobeSuspendedWorkflow).State = EntityState.Modified;
                                            await _dbContext.SaveChangesAsync();
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (var item in workflowAttachmentTypes)
                                    {
                                        var activeWorkflows = GetActiveWorkflows(moduleTrigger.ModuleTriggerId, item.AttachmentTypeId, workflow.SubModuleId).Result.FirstOrDefault();
                                        if (activeWorkflows != null)
                                        {
                                            var previousTypes = await _dbContext.WorkflowAttachmentType.Where(c => c.AttachmentTypeId == item.AttachmentTypeId && c.WorkflowId == activeWorkflows.WorkflowId).FirstOrDefaultAsync();
                                            if (previousTypes != null)
                                            {
                                                previousTypes.IsActiveFlow = false;
                                                await _dbContext.SaveChangesAsync();
                                            }
                                            bool inactiveAttachmentType = _dbContext.WorkflowAttachmentType.Where(i => i.WorkflowId == activeWorkflows.WorkflowId).ToList().All(x => x.IsActiveFlow == false);
                                            if (inactiveAttachmentType == true)
                                            {
                                                List<WorkflowInstance> workflowInstances = await _dbContext.WorkflowInstance.Where(i => i.WorkflowId == activeWorkflows.WorkflowId && i.StatusId == (int)WorkflowInstanceStatusEnum.InProgress).ToListAsync();
                                                if (workflowInstances?.Count() > 0)
                                                {
                                                    Workflow tobeSuspendedWorkflow = await _dbContext.Workflow.FindAsync(activeWorkflows.WorkflowId);
                                                    tobeSuspendedWorkflow.StatusId = (int)WorkflowStatusEnum.Suspended;
                                                    _dbContext.Entry(tobeSuspendedWorkflow).State = EntityState.Modified;
                                                    await _dbContext.SaveChangesAsync();
                                                }
                                                else
                                                {
                                                    Workflow tobeInactiveWorkflow = await _dbContext.Workflow.FindAsync(activeWorkflows.WorkflowId);
                                                    tobeInactiveWorkflow.StatusId = (int)WorkflowStatusEnum.Inactive;
                                                    _dbContext.Entry(tobeInactiveWorkflow).State = EntityState.Modified;
                                                    await _dbContext.SaveChangesAsync();
                                                }
                                            }

                                        }
                                        item.IsActiveFlow = true;
                                        _dbContext.Entry(item).State = EntityState.Modified;
                                        await _dbContext.SaveChangesAsync();
                                    }
                                }
                                workflow.StatusId = statusId;
                                _dbContext.Entry(workflow).State = EntityState.Modified;
                                await _dbContext.SaveChangesAsync();
                            }
                            else
                            {
                                var workflow = await _dbContext.Workflow.FindAsync(workflowId);
                                workflow.StatusId = statusId;
                                _dbContext.Entry(workflow).State = EntityState.Modified;
                                await _dbContext.SaveChangesAsync();
                            }
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Get active workflow for suspend
        public async Task<Workflow> GetActiveWorkflowforSuspend(int workflowId, int statusId)
        {
            try
            {
                Workflow workflowtobsuspended = new Workflow();
                if (statusId == (int)WorkflowStatusEnum.Active)
                {
                    Workflow workflow = await _dbContext.Workflow.FindAsync(workflowId);
                    WorkflowTrigger workflowTrigger = _dbContext.WorkflowTrigger.Where(w => w.WorkflowId == workflowId).FirstOrDefault();
                    List<WorkflowAttachmentType> workflowAttachmentTypes = _dbContext.WorkflowAttachmentType.Where(w => w.WorkflowId == workflowId).ToList();
                    ModuleTrigger moduleTrigger = await _dbContext.ModuleTrigger.FindAsync(workflowTrigger.ModuleTriggerId);
                    if (!workflowAttachmentTypes.Any())
                    {
                        var activeWorkflow = GetActiveWorkflows(moduleTrigger.ModuleTriggerId, null, workflow.SubModuleId).Result.FirstOrDefault();
                        if (activeWorkflow != null)
                        {
                            workflowtobsuspended = await _dbContext.Workflow.FindAsync(activeWorkflow.WorkflowId);
                        }
                    }
                    else
                    {
                        foreach (var item in workflowAttachmentTypes)
                        {
                            var activeWorkflows = GetActiveWorkflows(moduleTrigger.ModuleTriggerId, item.AttachmentTypeId, workflow.SubModuleId).Result.FirstOrDefault();
                            if (activeWorkflows != null)
                            {
                                workflowtobsuspended = await _dbContext.Workflow.FindAsync(activeWorkflows.WorkflowId);
                            }
                        }
                    }
                }
                return workflowtobsuspended;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Process Draft Entity
        private async Task<string> ProcessDraftEntity(CmsDraftedTemplate draft, DatabaseContext dbContext)
        {
            try
            {
                if (draft.DraftEntityType == (int)DraftEntityTypeEnum.RequestNeedMoreInfo || draft.DraftEntityType == (int)DraftEntityTypeEnum.FileNeedMoreInfo || draft.DraftEntityType == (int)DraftEntityTypeEnum.CaseNeedMoreInfo)
                {

                    List<SendCommunicationVM> Communication = JsonConvert.DeserializeObject<List<SendCommunicationVM>>(draft.Payload);
                    List<SendCommunicationVM> CopyCommunication = new List<SendCommunicationVM>();
                    string OutboxNo = "";
                    string OutboxFormat = "";
                    foreach (var communicationItem in Communication)
                    {
                        var sendCommunication = await UpdateCommunicationOutboxInfoReturnCommunicationDetail(communicationItem, dbContext, OutboxNo, OutboxFormat);
                        OutboxNo = sendCommunication.Communication.OutboxNumber;
                        OutboxFormat = sendCommunication.Communication.OutBoxNumberFormat;
                        // Update Case Request Status
                        UpdateEntityStatusVM updateEntity = new UpdateEntityStatusVM();
                        updateEntity.ReferenceId = draft.ReferenceId;
                        if (draft.SectorTypeId >= (int)OperatingSectorTypeEnum.LegalAdvice && draft.SectorTypeId <= (int)OperatingSectorTypeEnum.InternationalArbitration)
                        {
                            if (draft.DraftEntityType == (int)DraftEntityTypeEnum.RequestNeedMoreInfo)
                            {
                                updateEntity.SubModuleId = (int)SubModuleEnum.ConsultationRequest;
                                updateEntity.StatusId = (int)CaseRequestStatusEnum.PendingForGEResponse;
                            }
                            if (draft.DraftEntityType == (int)DraftEntityTypeEnum.FileNeedMoreInfo)
                            {
                                if (sendCommunication.CommunicationResponse.ResponseTypeId == (int)ResponseTypeEnum.FinalDocument)
                                {
                                    updateEntity.SubModuleId = (int)SubModuleEnum.ConsultationFile;
                                    updateEntity.StatusId = (int)CaseFileStatusEnum.FileIsClosed;
                                }
                                else
                                {
                                    updateEntity.SubModuleId = (int)SubModuleEnum.ConsultationFile;
                                    updateEntity.StatusId = (int)CaseFileStatusEnum.PendingForGEResponse;
                                }

                            }
                        }
                        else
                        {
                            if (draft.DraftEntityType == (int)DraftEntityTypeEnum.RequestNeedMoreInfo)
                            {
                                updateEntity.SubModuleId = (int)SubModuleEnum.CaseRequest;
                                updateEntity.StatusId = (int)CaseRequestStatusEnum.PendingForGEResponse;
                            }
                            if (draft.DraftEntityType == (int)DraftEntityTypeEnum.FileNeedMoreInfo)
                            {
                                updateEntity.SubModuleId = (int)SubModuleEnum.CaseFile;
                                updateEntity.StatusId = (int)CaseFileStatusEnum.PendingForGEResponse;
                            }
                            //for Registered Case Need More Info
                            if (draft.DraftEntityType == (int)DraftEntityTypeEnum.CaseNeedMoreInfo)
                            {
                                updateEntity.SubModuleId = (int)SubModuleEnum.RegisteredCase;
                                updateEntity.StatusId = (int)RegisteredCaseStatusEnum.PendingForGEResponse;
                            }
                        }
                        updateEntity.ModifiedBy = draft.ModifiedBy;
                        updateEntity.ModifiedDate = draft.ModifiedDate;
                        await _commonRepo.UpdateEntityStatus(updateEntity);
                        CopyCommunication.Add(sendCommunication);
                        draft.UpdateEntity = updateEntity;
                    }
                    draft.Payload = JsonConvert.SerializeObject(CopyCommunication);


                }
                if (draft.DraftEntityType == (int)DraftEntityTypeEnum.PostPoneHearing)
                {
                    PostponeHearing postponeHearing = JsonConvert.DeserializeObject<PostponeHearing>(draft.Payload);
                    await dbContext.PostponeHearings.AddAsync(postponeHearing);
                    Hearing hearing = await dbContext.Hearings.FindAsync(postponeHearing.HearingId);
                    hearing.StatusId = (int)HearingStatusEnum.HearingCancelled;
                    dbContext.Hearings.Update(hearing);
                    await dbContext.SaveChangesAsync();
                }

                if (draft.DraftEntityType == (int)DraftEntityTypeEnum.HearingSchedulingCourtVisit)
                {
                    Hearing hearingCourtVisit = JsonConvert.DeserializeObject<Hearing>(draft.Payload);
                    hearingCourtVisit.CreatedDate = draft.CreatedDate;
                    await dbContext.Hearings.AddAsync(hearingCourtVisit);
                    await dbContext.SaveChangesAsync();
                    if (hearingCourtVisit.SendPortfolioRequestMoj)
                    {
                        hearingCourtVisit.RequestForDocument.CreatedDate = DateTime.Now;
                        hearingCourtVisit.RequestForDocument.CreatedBy = hearingCourtVisit.CreatedBy;
                        await dbContext.MojRequestForDocument.AddAsync(hearingCourtVisit.RequestForDocument);
                        await dbContext.SaveChangesAsync();
                    }
                }
                if (draft.DraftEntityType == (int)DraftEntityTypeEnum.SubCase)
                {
                    CmsRegisteredCase cmsRegisteredCase = JsonConvert.DeserializeObject<CmsRegisteredCase>(draft.Payload);
                    await dbContext.cmsRegisteredCases.AddAsync(cmsRegisteredCase);
                    await dbContext.SaveChangesAsync();
                    await _registeredCaseRepository.CreateSubCaseManytoMany(cmsRegisteredCase, _dbContext);
                    await _registeredCaseRepository.SaveAttachmentsBySubCase(cmsRegisteredCase, _dbDmsContext);
                    await _registeredCaseRepository.CopyAttachmentsFromRegisteredCase(cmsRegisteredCase, _dbDmsContext);
                    await _registeredCaseRepository.CopyPartiesFromRegisteredCase(cmsRegisteredCase, _dbContext, _dbDmsContext);
                }
                if (draft.DraftEntityType == (int)DraftEntityTypeEnum.CaseFile || draft.DraftEntityType == (int)DraftEntityTypeEnum.ConsultationFile)
                {
                    if (draft.IsG2GSend == true && draft.Payload == null && draft.DraftedTemplateVersion.StatusId == (int)DraftVersionStatusEnum.Published)
                    {
                        draft.Payload = await CreateSendCommunicationObj(draft, _dbContext);
                    }
                }
                return draft.Payload;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Process Consultation Draft Entity
        private async Task<string> ProcessComsDraftEntity(ComsDraftedTemplate draft, DatabaseContext dbContext, string username)
        {
            try
            {
                if (draft.DraftEntityType == (int)DraftEntityTypeEnum.RequestNeedMoreInfo || draft.DraftEntityType == (int)DraftEntityTypeEnum.FileNeedMoreInfo)
                {
                    SendCommunicationVM sendCommunication = JsonConvert.DeserializeObject<SendCommunicationVM>(draft.Payload);
                    sendCommunication = await UpdateCommunicationOutboxInfoReturnCommunicationDetail(sendCommunication, dbContext, username);

                    // Update Consultation Request Status
                    UpdateEntityStatusVM updateEntity = new UpdateEntityStatusVM();
                    updateEntity.ReferenceId = draft.ReferenceId;

                    if (draft.DraftEntityType == (int)DraftEntityTypeEnum.RequestNeedMoreInfo)
                    {
                        updateEntity.SubModuleId = (int)SubModuleEnum.ConsultationRequest;
                        updateEntity.StatusId = (int)CaseRequestStatusEnum.PendingForGEResponse;
                    }
                    if (draft.DraftEntityType == (int)DraftEntityTypeEnum.FileNeedMoreInfo)
                    {
                        updateEntity.SubModuleId = (int)SubModuleEnum.ConsultationFile;
                        updateEntity.StatusId = (int)CaseFileStatusEnum.PendingForGEResponse;
                    }
                    updateEntity.ModifiedBy = draft.ModifiedBy;
                    updateEntity.ModifiedDate = draft.ModifiedDate;
                    await _commonRepo.UpdateEntityStatus(updateEntity);
                    draft.Payload = JsonConvert.SerializeObject(sendCommunication);
                    draft.UpdateEntity = updateEntity;
                }

                return draft.Payload;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region Update Communication detail
        public async Task<SendCommunicationVM> UpdateCommunicationOutboxInfoReturnCommunicationDetail(SendCommunicationVM sendCommunication, DatabaseContext dbContext, string? outboxNo = null, string? outboxFormat = null)
        {
            try
            {
                //Communication communication = new Communication();

                sendCommunication.Communication.ColorId = (int)CommunicationColorEnum.Green;
                sendCommunication.Communication.OutboxDate = DateTime.Now;
                // CMSCOMSInboxOutboxNumberResult resultOutBoxNumber = new CMSCOMSInboxOutboxNumberResult();
                var resultOutBoxNumber = await _cMSCOMSInboxOutboxPatternNumberRepository.GenerateNumberPattern(0, (int)CmsComsNumPatternTypeEnum.OutboxNumber, dbContext);
                sendCommunication.Communication.OutboxNumber = resultOutBoxNumber.GenerateRequestNumber;
                sendCommunication.Communication.OutBoxNumberFormat = resultOutBoxNumber.FormatRequestNumber;
                sendCommunication.Communication.PatternSequenceResult = resultOutBoxNumber.PatternSequenceResult;
                //sendCommunication.Communication.OutboxShortNum = dbContext.Communications.Any() ? await dbContext.Communications.Select(x => x.OutboxShortNum).MaxAsync() + 1 : 1;
                dbContext.Communications.Update(sendCommunication.Communication);
                sendCommunication.CommunicationTargetLink = dbContext.CommunicationTargetLinks.Where(x => x.CommunicationId == sendCommunication.Communication.CommunicationId).FirstOrDefault();
                sendCommunication.LinkTarget = await dbContext.LinkTargets.Where(x => x.TargetLinkId == sendCommunication.CommunicationTargetLink.TargetLinkId).ToListAsync();
                return sendCommunication;
            }
            catch(Exception ex)
            {
                throw ex;
            }

        }
        public void UpdateCommunicationColor(CmsDraftedTemplate draft, int colorId, DatabaseContext _dbContext)
        {
            List<SendCommunicationVM> sendCommunication = JsonConvert.DeserializeObject<List<SendCommunicationVM>>(draft.Payload);
            foreach (var communicationItem in sendCommunication)
            {

                communicationItem.Communication.ColorId = colorId;
                _dbContext.Communications.Update(communicationItem.Communication);
            }
        }
        public async Task<string> CreateSendCommunicationObj(CmsDraftedTemplate draft, DatabaseContext dbContext)
        {
            try
            {
                sendCommunication.Communication = new Communication();
                sendCommunication.CommunicationResponse = new CommunicationResponse();
                sendCommunication.CommunicationTargetLink = new CommunicationTargetLink();
                List<SendCommunicationVM> communicationList = new List<SendCommunicationVM>();
                LinkTarget linkTarget = new LinkTarget();
                sendCommunication.LinkTarget = new List<LinkTarget>();
                if (draft.DraftEntityType == (int)DraftEntityTypeEnum.CaseFile)
                {
                    var caseFileDetail = await dbContext.CaseFiles.Where(x => x.FileId == draft.ReferenceId).FirstOrDefaultAsync();
                    if (caseFileDetail != null)
                    {
                        caseRequest = await _CMSCaseRequestRepository.GetCMSCaseRequestsDetailById(caseFileDetail.RequestId.ToString(),0);
                        sendCommunication.Communication.ReceivedBy = caseRequest.CreatedBy;
                        sendCommunication.Communication.GovtEntityId = caseRequest.GovtEntityId;
                    }
                }
                if (draft.DraftEntityType == (int)DraftEntityTypeEnum.ConsultationFile)
                {
                    var consultationFileDetail = await dbContext.ConsultationFiles.Where(x => x.FileId == draft.ReferenceId).FirstOrDefaultAsync();
                    if (consultationFileDetail != null)
                    {
                        viewConsultationVM = await _COMSConsultationRequestRepository.GetConsultationRequest(consultationFileDetail.RequestId);
                        sendCommunication.Communication.ReceivedBy = viewConsultationVM.CreatedBy;
                        sendCommunication.Communication.GovtEntityId = viewConsultationVM.GovtEntityId;
                    }
                }
                if (draft.DraftEntityType == (int)DraftEntityTypeEnum.Case)
                {
                    caseRequest = await _CMSCaseRequestRepository.GetCMSCaseRequestsDetailById(draft.ReferenceId.ToString(), 0);
                    sendCommunication.Communication.ReceivedBy = caseRequest.CreatedBy;
                    sendCommunication.Communication.GovtEntityId = caseRequest.GovtEntityId;
                }
                //Communciation Detail
                sendCommunication.Communication.CommunicationId = (Guid)draft.CommunicationId;
                sendCommunication.Communication.CreatedBy = draft.CreatedBy;
                sendCommunication.Communication.CreatedDate = DateTime.Now;
                sendCommunication.Communication.SentBy = draft.CreatedBy;
                sendCommunication.Communication.Title = draft.Name;
                sendCommunication.Communication.Description = draft.Description;
                sendCommunication.Communication.SourceId = (int)CommunicationSourceEnum.FATWA;
                sendCommunication.Communication.CommunicationTypeId = (int)CommunicationTypeEnum.GeneralUpdate;
                sendCommunication.Communication.PreCommunicationId = Guid.Empty;
                sendCommunication.Communication.SectorTypeId = draft.SectorTypeId;
                sendCommunication.Communication.CorrespondenceTypeId = (int)CommunicationCorrespondenceTypeEnum.Outbox;
                sendCommunication.Communication.ColorId = (int)CommunicationColorEnum.Green;
                var resultOutBoxNumber = await _cMSCOMSInboxOutboxPatternNumberRepository.GenerateNumberPattern(0, (int)CmsComsNumPatternTypeEnum.OutboxNumber);
                sendCommunication.Communication.OutboxNumber = resultOutBoxNumber.GenerateRequestNumber;
                sendCommunication.Communication.OutBoxNumberFormat = resultOutBoxNumber.FormatRequestNumber;
                sendCommunication.Communication.PatternSequenceResult = resultOutBoxNumber.PatternSequenceResult;
                sendCommunication.Communication.OutboxShortNum = 0;
                sendCommunication.Communication.OutboxDate = DateTime.Now;
                sendCommunication.Communication.IsDeleted = false;
                //CommunicationResponse Detail
                sendCommunication.CommunicationResponse.CommunicationId = sendCommunication.Communication.CommunicationId;
                sendCommunication.CommunicationResponse.CommunicationResponseId = Guid.NewGuid();
                sendCommunication.CommunicationResponse.CreatedBy = draft.CreatedBy;
                sendCommunication.CommunicationResponse.CreatedDate = DateTime.Now;
                sendCommunication.CommunicationResponse.ResponseDate = DateTime.Now;
                sendCommunication.CommunicationResponse.RequestDate = draft.CreatedDate;
                sendCommunication.CommunicationResponse.IsDeleted = false;
                sendCommunication.CommunicationResponse.FrequencyId = 1;
                sendCommunication.CommunicationResponse.ResponseTypeId = (int)ResponseTypeEnum.RequestForMoreInformation;
                //CommunicationTargetLink Detail
                sendCommunication.CommunicationTargetLink.CommunicationId = sendCommunication.Communication.CommunicationId;
                sendCommunication.CommunicationTargetLink.TargetLinkId = new Guid();
                sendCommunication.CommunicationTargetLink.CreatedBy = draft.CreatedBy;
                sendCommunication.CommunicationTargetLink.CreatedDate = DateTime.Now;
                sendCommunication.CommunicationTargetLink.IsDeleted = false;
                linkTarget = new LinkTarget()
                {
                    LinkTargetId = new Guid(),
                    ReferenceId = sendCommunication.Communication.CommunicationId,
                    IsPrimary = false,
                    LinkTargetTypeId = (int)LinkTargetTypeEnum.Communication
                };
                sendCommunication.LinkTarget.Add(linkTarget);
                linkTarget = new LinkTarget()
                {
                    LinkTargetId = new Guid(),
                    ReferenceId = sendCommunication.CommunicationResponse.CommunicationResponseId,
                    IsPrimary = false,
                    LinkTargetTypeId = (int)LinkTargetTypeEnum.Communication
                };
                sendCommunication.LinkTarget.Add(linkTarget);
                if (draft.DraftEntityType == (int)DraftEntityTypeEnum.ConsultationFile)
                    linkTarget = new LinkTarget()
                    {
                        LinkTargetId = new Guid(),
                        ReferenceId = draft.ReferenceId,
                        IsPrimary = true,
                        LinkTargetTypeId = (int)LinkTargetTypeEnum.ConsultationFile
                    };
                if (draft.DraftEntityType == (int)DraftEntityTypeEnum.CaseFile)
                    linkTarget = new LinkTarget()
                    {
                        LinkTargetId = new Guid(),
                        ReferenceId = draft.ReferenceId,
                        IsPrimary = true,
                        LinkTargetTypeId = (int)LinkTargetTypeEnum.File
                    };
                if (draft.DraftEntityType == (int)DraftEntityTypeEnum.Case)
                    linkTarget = new LinkTarget()
                    {
                        LinkTargetId = new Guid(),
                        ReferenceId = draft.ReferenceId,
                        IsPrimary = true,
                        LinkTargetTypeId = (int)LinkTargetTypeEnum.RegisteredCase
                    };
                sendCommunication.LinkTarget.Add(linkTarget);
                await _communicationRepository.SaveCommunication(sendCommunication.Communication, dbContext);
                await _communicationRepository.SaveCommResponse(sendCommunication.CommunicationResponse, sendCommunication.Communication.CommunicationId, dbContext);
                if (sendCommunication.CommunicationResponse.EntityIds != null)
                {
                    await _communicationRepository.SaveCommResponseGovtEntit(sendCommunication.CommunicationResponse, dbContext);
                }
                await _communicationRepository.SaveCommunicationTargetLink(sendCommunication.CommunicationTargetLink, dbContext);
                await _communicationRepository.SaveLinkTarget(sendCommunication.LinkTarget, sendCommunication.CommunicationTargetLink.TargetLinkId, dbContext);
                communicationList.Add(sendCommunication);
                draft.Payload = JsonConvert.SerializeObject(communicationList);
                return draft.Payload;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion


        #region Cms Transfer Instance
        //<History Author = 'Muhamamd Zaeem' Date='2023-26-11' Version="1.0" Branch="master"> Update approval tracking instance</History>
        public async Task<CmsCaseFileStatusHistory> UpdateApprovalTrackingInstance(CmsApprovalTracking approvalTracking)
        {
            try
            {
                CmsCaseFileStatusHistory historyobj = null;

                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            if (approvalTracking.StatusId == (int)ApprovalStatusEnum.Pending)
                            {
                                var User = await _IAccount.GetUserEmploymentInfoByUserId(approvalTracking.ReviewerUserId);
                                if (User != null)
                                {
                                    approvalTracking.SectorTo = (int)User?.SectorTypeId;
                                }
                                if (approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest)
                                {
                                    await _CMSCaseRequestRepository.UpdateCaseRequestTransferStatus(approvalTracking, (int)ApprovalStatusEnum.Pending, approvalTracking.TransferCaseType);
                                }
                                else if (approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseFile)
                                {
                                    await _cmsCaseFileRepository.UpdateCaseFileTransferStatus(approvalTracking, (int)ApprovalStatusEnum.Pending, approvalTracking.TransferCaseType);
                                }
                                _dbContext.CmsApprovalTracking.Update(approvalTracking);
                                await _dbContext.SaveChangesAsync();
                            }
                            else if (approvalTracking.StatusId == (int)ApprovalStatusEnum.Approved)
                            {

                                if (approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest)
                                {
                                    CaseRequest caseRequest = await _dbContext.CaseRequests.FindAsync(approvalTracking.ReferenceId);
                                    caseRequest.SectorTypeId = approvalTracking.SectorTypeId;
                                    caseRequest.TransferStatusId = (int)ApprovalStatusEnum.Approved;
                                    caseRequest.ModifiedDate = DateTime.Now;
                                    caseRequest.ModifiedBy = approvalTracking.CreatedBy;
                                    _dbContext.CaseRequests.Update(caseRequest);
                                    await _dbContext.SaveChangesAsync();
                                    await _cmsSharedRepository.UpdateApprovalTrackingStatus(caseRequest.RequestId, approvalTracking.CreatedBy, (int)approvalTracking.SectorTypeId, approvalTracking.Remarks, (int)ApprovalProcessTypeEnum.Transfer, approvalTracking.StatusId, approvalTracking.TransferCaseType, _dbContext);
                                }
                                else if (approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseFile)
                                {
                                    CaseFile caseFile = await _dbContext.CaseFiles.FindAsync(approvalTracking.ReferenceId);
                                    if (caseFile.StatusId == (int)CaseFileStatusEnum.RegisteredInMoj)
                                    {
                                        caseFile.StatusId = (int)CaseFileStatusEnum.AssignedToRegionalSector;

                                    }
                                    caseFile.TransferStatusId = (int)ApprovalStatusEnum.Approved;
                                    _dbContext.CaseFiles.Update(caseFile);
                                    await _dbContext.SaveChangesAsync();
                                    CmsApprovalTracking cmsapprovalTracking = await _dbContext.CmsApprovalTracking.Where(x => x.ReferenceId == caseFile.FileId && x.SectorTo == approvalTracking.SectorTypeId && x.ProcessTypeId == (int)ApprovalProcessTypeEnum.FileAssignment).FirstOrDefaultAsync();
                                    if (cmsapprovalTracking != null)
                                    {
                                        await _cmsCaseFileRepository.SaveCaseFileSectorAssignment(caseFile.FileId, (int)approvalTracking.SectorTypeId, approvalTracking.CreatedBy, _dbContext);
                                        historyobj = await _cmsCaseFileRepository.SaveCaseFileStatusHistory(approvalTracking.CreatedBy, caseFile, (int)CaseFileEventEnum.AssignToSector, _dbContext);
                                        await _cmsSharedRepository.UpdateApprovalTrackingStatus(caseFile.FileId, approvalTracking.CreatedBy, (int)approvalTracking.SectorTypeId, approvalTracking.Remarks, (int)ApprovalProcessTypeEnum.FileAssignment, (int)ApprovalStatusEnum.Approved, approvalTracking.TransferCaseType, _dbContext);
                                    }
                                    else
                                    {
                                        await _cmsCaseFileRepository.SaveCaseFileSectorAssignment(caseFile.FileId, (int)approvalTracking.SectorTypeId, approvalTracking.CreatedBy, _dbContext);
                                        await _cmsSharedRepository.UpdateApprovalTrackingStatus(caseFile.FileId, approvalTracking.CreatedBy, (int)approvalTracking.SectorTypeId, approvalTracking.Remarks, (int)ApprovalProcessTypeEnum.Transfer, (int)ApprovalStatusEnum.Approved, approvalTracking.TransferCaseType, _dbContext);

                                    }
                                }
                            }
                            else if (approvalTracking.StatusId == (int)ApprovalTrackingStatusEnum.Transfered)
                            {
                                var User = await _IAccount.GetUserEmploymentInfoByUserId(approvalTracking.ReviewerUserId);
                                //var User = await _dbContext.Users.Where(x => x.Id == approvalTracking.ReviewerUserId).FirstOrDefaultAsync();
                                if (approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest)
                                {
                                    CaseRequest caseRequest = await _dbContext.CaseRequests.FindAsync(approvalTracking.ReferenceId);
                                    if (User != null)
                                    {
                                        caseRequest.SectorTypeId = User?.SectorTypeId;
                                        caseRequest.TransferStatusId = (int)ApprovalStatusEnum.Approved;
                                        caseRequest.ModifiedBy = approvalTracking.CreatedBy;
                                        caseRequest.ModifiedDate = DateTime.Now;
                                    }
                                    _dbContext.CaseRequests.Update(caseRequest);
                                    await _dbContext.SaveChangesAsync();
                                    await _cmsSharedRepository.UpdateApprovalTrackingWorkflow(caseRequest.RequestId, approvalTracking.CreatedBy, (int)User?.SectorTypeId, approvalTracking.Remarks, (int)ApprovalProcessTypeEnum.Transfer, (int)ApprovalStatusEnum.Approved, approvalTracking.TransferCaseType, _dbContext, approvalTracking.SectorFrom);
                                }
                                else if (approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseFile)
                                {
                                    CaseFile caseFile = await _dbContext.CaseFiles.FindAsync(approvalTracking.ReferenceId);
                                    if (caseFile.StatusId == (int)CaseFileStatusEnum.RegisteredInMoj)
                                    {
                                        caseFile.StatusId = (int)CaseFileStatusEnum.AssignedToRegionalSector;
                                    }
                                    caseFile.TransferStatusId = (int)ApprovalStatusEnum.Approved;
                                    _dbContext.CaseFiles.Update(caseFile);
                                    await _dbContext.SaveChangesAsync();
                                    CmsApprovalTracking cmsapprovalTracking = await _dbContext.CmsApprovalTracking.Where(x => x.ReferenceId == caseFile.FileId && x.SectorTo == approvalTracking.SectorTypeId && x.ProcessTypeId == (int)ApprovalProcessTypeEnum.FileAssignment).FirstOrDefaultAsync();
                                    if (cmsapprovalTracking != null)
                                    {
                                        await _cmsCaseFileRepository.SaveCaseFileSectorAssignment(caseFile.FileId, (int)User.SectorTypeId, approvalTracking.CreatedBy, _dbContext);
                                        historyobj = await _cmsCaseFileRepository.SaveCaseFileStatusHistory(approvalTracking.CreatedBy, caseFile, (int)CaseFileEventEnum.AssignToSector, _dbContext);
                                        await _cmsSharedRepository.UpdateApprovalTrackingStatus(caseFile.FileId, approvalTracking.CreatedBy, (int)approvalTracking.SectorTypeId, approvalTracking.Remarks, (int)ApprovalProcessTypeEnum.FileAssignment, (int)ApprovalStatusEnum.Approved, approvalTracking.TransferCaseType, _dbContext);
                                    }
                                    else
                                    {
                                        await _cmsCaseFileRepository.SaveCaseFileSectorAssignment(caseFile.FileId, (int)User?.SectorTypeId, approvalTracking.CreatedBy, _dbContext);
                                        await _cmsSharedRepository.UpdateApprovalTrackingWorkflow(caseFile.FileId, approvalTracking.CreatedBy, (int)User?.SectorTypeId, approvalTracking.Remarks, (int)ApprovalProcessTypeEnum.Transfer, (int)ApprovalStatusEnum.Approved, approvalTracking.TransferCaseType, _dbContext, approvalTracking.SectorFrom);
                                    }
                                }
                            }
                            // For Notification
                            await PrepareNotificationParameter(approvalTracking, _dbContext);
                            //workflow instance
                            var isntance = _dbContext.WorkflowInstance.Where(i => i.ReferenceId == approvalTracking.Id).FirstOrDefault();
                            if (approvalTracking.WorkflowActivityId.HasValue && approvalTracking.WorkflowActivityId > 0)
                            {
                                isntance.WorkflowActivityId = (int)approvalTracking.WorkflowActivityId;
                                isntance.IsSlaExecuted = false;
                                SLA sla = await _dbContext.SLA.Where(s => s.WorkflowActivityId == isntance.WorkflowActivityId).FirstOrDefaultAsync();
                                if (sla != null)
                                {
                                    isntance.ApplySla = true;
                                    isntance.SlaStartDate = DateTime.Now.Date.AddDays(1);
                                    isntance.SlaEndDate = isntance.SlaStartDate.AddDays(sla.Duration - 1);
                                }
                                else
                                {
                                    isntance.ApplySla = false;
                                    isntance.SlaStartDate = DateTime.Now.Date;
                                    isntance.SlaEndDate = DateTime.Now.Date;
                                }
                            }

                            if (approvalTracking.WorkflowInstanceStatusId != null && (int)approvalTracking.WorkflowInstanceStatusId > 0)
                                isntance.StatusId = (int)approvalTracking.WorkflowInstanceStatusId;
                            _dbContext.Entry(isntance).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();

                            //get workflow from current instance
                            Workflow workflow = await _dbContext.Workflow.FindAsync(isntance.WorkflowId);
                            if (workflow.StatusId == (int)WorkflowStatusEnum.Suspended)
                            {
                                //if workflow is suspended check to make it inactive
                                var runningInstances = await _dbContext.WorkflowInstance.Where(i => i.WorkflowId == workflow.WorkflowId && i.StatusId == (int)WorkflowInstanceStatusEnum.InProgress).ToListAsync();

                                //if no instances then inactive workflow
                                if (runningInstances?.Count() <= 0)
                                {
                                    workflow.StatusId = (int)WorkflowStatusEnum.Inactive;
                                    _dbContext.Entry(workflow).State = EntityState.Modified;
                                    await _dbContext.SaveChangesAsync();
                                }
                            }
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                return historyobj;

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Consultation Transfer Instance
        //<History Author = 'Muhamamd Zaeem' Date='2023-26-11' Version="1.0" Branch="master"> Update consultation approval tracking instance</History>
        public async Task UpdateApprovalTrackingConsultationInstance(CmsApprovalTracking approvalTracking)
        {
            try
            {

                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            if (approvalTracking.StatusId == (int)ApprovalStatusEnum.Pending)
                            {
                                var User = await _IAccount.GetUserEmploymentInfoByUserId(approvalTracking.ReviewerUserId);
                                if (User != null)
                                {
                                    approvalTracking.SectorTo = (int)User.SectorTypeId;
                                }
                                if (approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest)
                                {
                                    await _COMSConsultationRequestRepository.UpdateConsultationRequestTransferStatus(approvalTracking, (int)ApprovalStatusEnum.Pending);
                                }
                                if (approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationFile)
                                {
                                    await _COMSConsultationRequestRepository.UpdateConsultationFileTransferStatus(approvalTracking, (int)ApprovalStatusEnum.Pending);
                                }
                                _dbContext.CmsApprovalTracking.Update(approvalTracking);
                                await _dbContext.SaveChangesAsync();

                            }
                            else if (approvalTracking.StatusId == (int)ApprovalStatusEnum.Approved)

                            {
                                if (approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest)
                                {
                                    ConsultationRequest consultationRequest = await _dbContext.ConsultationRequests.FindAsync(approvalTracking.ReferenceId);
                                    consultationRequest.SectorTypeId = approvalTracking.SectorTypeId;
                                    _dbContext.ConsultationRequests.Update(consultationRequest);
                                    await _dbContext.SaveChangesAsync();
                                    await _COMSConsultationRequestRepository.SaveConsultationRequestStatusHistory(approvalTracking.CreatedBy, consultationRequest, (int)CaseRequestEventEnum.Transfer, _dbContext);
                                    await _comsSharedRepository.UpdateApprovalTrackingStatus(consultationRequest.ConsultationRequestId, approvalTracking.CreatedBy, (int)approvalTracking.SectorTypeId, (int)ApprovalProcessTypeEnum.Transfer, (int)ApprovalStatusEnum.Approved, _dbContext, approvalTracking.Remarks, approvalTracking.TransferCaseType);
                                    await _comsSharedRepository.UpdateConsultationRequestTransferStatus(consultationRequest.ConsultationRequestId, approvalTracking.CreatedBy, (int)ApprovalStatusEnum.Approved, _dbContext);

                                }
                                else if (approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationFile)
                                {
                                    ConsultationFile consultationFile = await _dbContext.ConsultationFiles.FindAsync(approvalTracking.ReferenceId);
                                    await _COMSConsultationRequestRepository.SaveConsultationFileSectorAssignment(approvalTracking.ReferenceId, approvalTracking.CreatedBy, (int)approvalTracking.SectorTypeId, _dbContext);
                                    await _COMSConsultationRequestRepository.SaveConsultationFileStatusHistory(consultationFile, (int)CaseFileEventEnum.Transfer, _dbContext);
                                    await _comsSharedRepository.UpdateApprovalTrackingStatus(consultationFile.FileId, approvalTracking.CreatedBy, (int)approvalTracking.SectorTypeId, (int)ApprovalProcessTypeEnum.Transfer, (int)ApprovalStatusEnum.Approved, _dbContext, approvalTracking.Remarks, (int)AssignCaseToLawyerTypeEnum.ConsultationFile);
                                    await _comsSharedRepository.UpdateConsultationFileTransferStatus(consultationFile.FileId, approvalTracking.CreatedBy, (int)ApprovalStatusEnum.Approved, _dbContext);


                                }
                            }
                            else if (approvalTracking.StatusId == (int)ApprovalTrackingStatusEnum.Transfered)
                            {
                                var User = await _IAccount.GetUserEmploymentInfoByUserId(approvalTracking.ReviewerUserId);
                                if (approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest)
                                {
                                    ConsultationRequest consultationRequest = await _dbContext.ConsultationRequests.FindAsync(approvalTracking.ReferenceId);
                                    if (User != null)
                                    {
                                        consultationRequest.SectorTypeId = User.SectorTypeId;
                                        consultationRequest.TransferStatusId = (int)ApprovalStatusEnum.Approved;
                                    }
                                    _dbContext.ConsultationRequests.Update(consultationRequest);
                                    await _dbContext.SaveChangesAsync();
                                    await _cmsSharedRepository.UpdateApprovalTrackingWorkflow(consultationRequest.ConsultationRequestId, approvalTracking.CreatedBy, (int)User.SectorTypeId, approvalTracking.Remarks, (int)ApprovalProcessTypeEnum.Transfer, (int)ApprovalStatusEnum.Approved, approvalTracking.TransferCaseType, _dbContext, approvalTracking.SectorFrom);
                                    await _comsSharedRepository.UpdateConsultationRequestTransferStatus(consultationRequest.ConsultationRequestId, approvalTracking.CreatedBy, (int)ApprovalStatusEnum.Approved, _dbContext);

                                }
                                else if (approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationFile)
                                {
                                    ConsultationFile consultationFile = await _dbContext.ConsultationFiles.FindAsync(approvalTracking.ReferenceId);
                                    await _COMSConsultationRequestRepository.SaveConsultationFileSectorAssignment(approvalTracking.ReferenceId, approvalTracking.CreatedBy, (int)User.SectorTypeId, _dbContext);
                                    await _COMSConsultationRequestRepository.SaveConsultationFileStatusHistory(consultationFile, (int)CaseFileEventEnum.Transfer, _dbContext);
                                    await _cmsSharedRepository.UpdateApprovalTrackingWorkflow(consultationFile.FileId, approvalTracking.CreatedBy, (int)User.SectorTypeId, approvalTracking.Remarks, (int)ApprovalProcessTypeEnum.Transfer, (int)ApprovalStatusEnum.Approved, approvalTracking.TransferCaseType, _dbContext, approvalTracking.SectorFrom);
                                    await _comsSharedRepository.UpdateConsultationFileTransferStatus(consultationFile.FileId, approvalTracking.CreatedBy, (int)ApprovalStatusEnum.Approved, _dbContext);


                                }
                            }
                            // For Notification
                            await PrepareNotificationParameter(approvalTracking, _dbContext);
                            //workflow instance
                            var isntance = _dbContext.WorkflowInstance.Where(i => i.ReferenceId == approvalTracking.Id).FirstOrDefault();
                            if (approvalTracking.WorkflowActivityId.HasValue && approvalTracking.WorkflowActivityId > 0)
                            {
                                isntance.WorkflowActivityId = (int)approvalTracking.WorkflowActivityId;
                                isntance.IsSlaExecuted = false;
                                SLA sla = await _dbContext.SLA.Where(s => s.WorkflowActivityId == isntance.WorkflowActivityId).FirstOrDefaultAsync();
                                if (sla != null)
                                {
                                    isntance.ApplySla = true;
                                    isntance.SlaStartDate = DateTime.Now.Date.AddDays(1);
                                    isntance.SlaEndDate = isntance.SlaStartDate.AddDays(sla.Duration - 1);
                                }
                                else
                                {
                                    isntance.ApplySla = false;
                                    isntance.SlaStartDate = DateTime.Now.Date;
                                    isntance.SlaEndDate = DateTime.Now.Date;
                                }
                            }
                            if (approvalTracking.WorkflowInstanceStatusId != null && (int)approvalTracking.WorkflowInstanceStatusId > 0)
                                isntance.StatusId = (int)approvalTracking.WorkflowInstanceStatusId;
                            _dbContext.Entry(isntance).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();

                            //get workflow from current instance
                            Workflow workflow = await _dbContext.Workflow.FindAsync(isntance.WorkflowId);
                            if (workflow.StatusId == (int)WorkflowStatusEnum.Suspended)
                            {
                                //if workflow is suspended check to make it inactive
                                var runningInstances = await _dbContext.WorkflowInstance.Where(i => i.WorkflowId == workflow.WorkflowId && i.StatusId == (int)WorkflowInstanceStatusEnum.InProgress).ToListAsync();

                                //if no instances then inactive workflow
                                if (runningInstances?.Count() <= 0)
                                {
                                    workflow.StatusId = (int)WorkflowStatusEnum.Inactive;
                                    _dbContext.Entry(workflow).State = EntityState.Modified;
                                    await _dbContext.SaveChangesAsync();
                                }
                            }
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Cms Send Copy Instance
        //<History Author = 'Muhamamd Zaeem' Date='2023-26-11' Version="1.0" Branch="master"> Update approval tracking instance for copy</History>
        public async Task UpdateCopyTrackingInstance(CmsApprovalTracking approvalTracking)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var User = await _accountRepository.GetUserEmploymentInfoByUserId(approvalTracking.ReviewerUserId);
                            if (User != null)
                            {
                                approvalTracking.SectorTo = (int)User.SectorTypeId;
                            }
                            _dbContext.CmsApprovalTracking.Update(approvalTracking);
                            await _dbContext.SaveChangesAsync();
                            await _cmsSharedRepository.SaveCopyHistory(approvalTracking, approvalTracking.StatusId, approvalTracking.TransferCaseType, _dbContext);

                            // For Notification
                            await PrepareNotificationParameter(approvalTracking, _dbContext);
                            //workflow instance
                            var isntance = _dbContext.WorkflowInstance.Where(i => i.ReferenceId == approvalTracking.Id).FirstOrDefault();
                            if (approvalTracking.WorkflowActivityId.HasValue && approvalTracking.WorkflowActivityId > 0)
                            {
                                isntance.WorkflowActivityId = (int)approvalTracking.WorkflowActivityId;
                                isntance.IsSlaExecuted = false;
                                SLA sla = await _dbContext.SLA.Where(s => s.WorkflowActivityId == isntance.WorkflowActivityId).FirstOrDefaultAsync();
                                if (sla != null)
                                {
                                    isntance.ApplySla = true;
                                    isntance.SlaStartDate = DateTime.Now.Date.AddDays(1);
                                    isntance.SlaEndDate = isntance.SlaStartDate.AddDays(sla.Duration - 1);
                                }
                                else
                                {
                                    isntance.ApplySla = false;
                                    isntance.SlaStartDate = DateTime.Now.Date;
                                    isntance.SlaEndDate = DateTime.Now.Date;
                                }
                            }
                            if (approvalTracking.WorkflowInstanceStatusId != null && (int)approvalTracking.WorkflowInstanceStatusId > 0)
                                isntance.StatusId = (int)approvalTracking.WorkflowInstanceStatusId;
                            _dbContext.Entry(isntance).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();

                            //get workflow from current instance
                            Workflow workflow = await _dbContext.Workflow.FindAsync(isntance.WorkflowId);
                            if (workflow.StatusId == (int)WorkflowStatusEnum.Suspended)
                            {
                                //if workflow is suspended check to make it inactive
                                var runningInstances = await _dbContext.WorkflowInstance.Where(i => i.WorkflowId == workflow.WorkflowId && i.StatusId == (int)WorkflowInstanceStatusEnum.InProgress).ToListAsync();

                                //if no instances then inactive workflow
                                if (runningInstances?.Count() <= 0)
                                {
                                    workflow.StatusId = (int)WorkflowStatusEnum.Inactive;
                                    _dbContext.Entry(workflow).State = EntityState.Modified;
                                    await _dbContext.SaveChangesAsync();
                                }
                            }
                            transaction.Commit();

                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw;
                        }


                    }
                }
            }

            catch (Exception)
            {
                throw;
            }
        }
        //<History Author = 'Muhamamd Zaeem' Date='2023-26-11' Version="1.0" Branch="master"> Update approval tracking instance for approved copy</History>
        public async Task<dynamic> UpdateCopyApprovedTrackingInstance(CmsApprovalTracking approvalTracking)
        {
            try
            {
                dynamic entityObject = null;
                using (_dbContext)
                {
                    using (var Transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {

                            var User = await _accountRepository.GetUserEmploymentInfoByUserId(approvalTracking.ReviewerUserId);
                            var userDetail = await _accountRepository.UserDetailByUserId(approvalTracking.ReviewerUserId);
                            if (approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest)
                            {
                                //CaseRequest paramCaseRequest = System.Text.Json.JsonSerializer.Deserialize<CaseRequest>(approvalTracking.Item);
                                CaseRequest paramCaseRequest = _dbContext.CaseRequests.AsNoTracking<CaseRequest>().Where(x => x.RequestId == approvalTracking.ReferenceId).FirstOrDefault();
                                if (User != null)
                                {
                                    if (approvalTracking.CreatedBy != userDetail.UserName)
                                    {
                                        paramCaseRequest.SectorTypeId = User.SectorTypeId;
                                        CaseRequest newCaseRequest = await _CMSCaseRequestRepository.CopyCaseRequest(paramCaseRequest, approvalTracking, _dbContext);
                                        //Copy Case Parties From Source To Destination
                                        var copyCaseParties = await _CMSCaseRequestRepository.CopyCasePartiesFromSourceToDestinationWorkflow(paramCaseRequest.RequestId, newCaseRequest.RequestId, approvalTracking.UserName, _dbContext, _dbDmsContext);
                                        //Copy Case Attachments From Source To Destination
                                        await _CMSCaseRequestRepository.CopyCaseAttachmentsFromSourceToDestinationWorkflow(paramCaseRequest.RequestId, newCaseRequest.RequestId, approvalTracking.UserName, _dbDmsContext);
                                        approvalTracking.SectorTypeId = User.SectorTypeId;
                                        await _cmsSharedRepository.SaveCopyHistory(approvalTracking, (int)ApprovalStatusEnum.Approved, approvalTracking.TransferCaseType, _dbContext);
                                        foreach (var party in copyCaseParties)
                                        {
                                            newCaseRequest.CaseParties.Add(party);

                                        }
                                        entityObject = newCaseRequest;

                                    }
                                    await _CMSCaseRequestRepository.SaveCaseRequestStatusHistory(approvalTracking.UserName, paramCaseRequest, (int)CaseRequestEventEnum.SentCopy, _dbContext);
                                    await _cmsSharedRepository.UpdateWorkflowApprovalTrackingStatus(paramCaseRequest.RequestId, approvalTracking.UserName, (int)User.SectorTypeId, approvalTracking.Remarks, (int)ApprovalProcessTypeEnum.SendaCopy, (int)ApprovalStatusEnum.Approved, _dbContext, approvalTracking.SectorFrom);
                                }
                            }
                            else if (approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseFile)
                            {
                                CaseFile oldCaseFile = await _dbContext.CaseFiles.Where(x => x.FileId == approvalTracking.ReferenceId).FirstOrDefaultAsync();
                                if (User != null)
                                {
                                    if (approvalTracking.CreatedBy != userDetail.UserName)
                                    {
                                        CaseFile newCaseFile = await _cmsCaseFileRepository.CopyCaseFile(oldCaseFile.FileId, oldCaseFile.CreatedBy, _dbContext);
                                        approvalTracking.SectorTypeId = User.SectorTypeId;
                                        await _cmsSharedRepository.SaveCopyHistory(approvalTracking, (int)ApprovalStatusEnum.Approved, approvalTracking.TransferCaseType, _dbContext);
                                        //Copy Case Parties From Source To Destination
                                        //await _caseRequestRepository.CopyCasePartiesFromSourceToDestination(oldCaseFile.FileId, newCaseFile.FileId, paramCaseFile.CreatedBy, _dbContext);
                                        var copyCaseFileParties = await _CMSCaseRequestRepository.CopyCasePartiesFromSourceToDestinationWorkflow(oldCaseFile.FileId, newCaseFile.FileId, approvalTracking.UserName, _dbContext, _dbDmsContext);
                                        //Copy Case Attachments From Source To Destination
                                        await _CMSCaseRequestRepository.CopyCaseAttachmentsFromSourceToDestinationWorkflow(oldCaseFile.FileId, newCaseFile.FileId, approvalTracking.UserName, _dbDmsContext);
                                        await _cmsCaseFileRepository.SaveCaseFileSectorAssignment(newCaseFile.FileId, (int)User.SectorTypeId, approvalTracking.UserName, _dbContext);
                                        foreach (var party in copyCaseFileParties)
                                        {
                                            newCaseFile.PartyLinks.Add(party);

                                        }
                                        entityObject = newCaseFile;
                                    }
                                    await _cmsCaseFileRepository.SaveCaseFileStatusHistory(oldCaseFile.CreatedBy, oldCaseFile, (int)CaseFileEventEnum.SentCopy, _dbContext);
                                    await _cmsSharedRepository.UpdateWorkflowApprovalTrackingStatus(approvalTracking.ReferenceId, approvalTracking.UserName, (int)User.SectorTypeId, approvalTracking.Remarks, (int)ApprovalProcessTypeEnum.SendaCopy, (int)ApprovalStatusEnum.Approved, _dbContext, approvalTracking.SectorFrom);
                                }
                            }
                            // For Notification
                            await PrepareNotificationParameter(approvalTracking, _dbContext);
                            //workflow instance
                            var isntance = _dbContext.WorkflowInstance.Where(i => i.ReferenceId == approvalTracking.Id).FirstOrDefault();
                            if (approvalTracking.WorkflowActivityId.HasValue && approvalTracking.WorkflowActivityId > 0)
                            {
                                isntance.WorkflowActivityId = (int)approvalTracking.WorkflowActivityId;
                                isntance.IsSlaExecuted = false;
                                SLA sla = await _dbContext.SLA.Where(s => s.WorkflowActivityId == isntance.WorkflowActivityId).FirstOrDefaultAsync();
                                if (sla != null)
                                {
                                    isntance.ApplySla = true;
                                    isntance.SlaStartDate = DateTime.Now.Date.AddDays(1);
                                    isntance.SlaEndDate = isntance.SlaStartDate.AddDays(sla.Duration - 1);
                                }
                                else
                                {
                                    isntance.ApplySla = false;
                                    isntance.SlaStartDate = DateTime.Now.Date;
                                    isntance.SlaEndDate = DateTime.Now.Date;
                                }
                            }

                            if (approvalTracking.WorkflowInstanceStatusId != null && (int)approvalTracking.WorkflowInstanceStatusId > 0)
                                isntance.StatusId = (int)approvalTracking.WorkflowInstanceStatusId;
                            _dbContext.Entry(isntance).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();

                            //get workflow from current instance
                            Workflow workflow = await _dbContext.Workflow.FindAsync(isntance.WorkflowId);
                            if (workflow.StatusId == (int)WorkflowStatusEnum.Suspended)
                            {
                                //if workflow is suspended check to make it inactive
                                var runningInstances = await _dbContext.WorkflowInstance.Where(i => i.WorkflowId == workflow.WorkflowId && i.StatusId == (int)WorkflowInstanceStatusEnum.InProgress).ToListAsync();

                                //if no instances then inactive workflow
                                if (runningInstances?.Count() <= 0)
                                {
                                    workflow.StatusId = (int)WorkflowStatusEnum.Inactive;
                                    _dbContext.Entry(workflow).State = EntityState.Modified;
                                    await _dbContext.SaveChangesAsync();
                                }
                            }
                            Transaction.Commit();

                        }
                        catch
                        {
                            Transaction.Rollback();
                            throw;
                        }
                    }
                }
                return entityObject;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Link Entity with Active Workflow
        public async Task LinkEntityWithActiveWorkflow(dynamic entity, DatabaseContext dbContext, int subModuleId = 0, int transferType = 0)
        {
            try
            {
                string StoredProc = null;
                Guid referenceId = new Guid();
                int moduleTriggerId = 0;
                if (transferType == 0)
                {
                    if (subModuleId <= (int)WorkflowSubModuleEnum.InternationalArbitration)
                    {
                        entity = (CmsDraftedTemplate)entity;
                        moduleTriggerId = entity.ModuleId == (int)WorkflowModuleEnum.CaseManagement ?
                        (int)WorkflowModuleTriggerEnum.UserSubmitsCaseDraft : (int)WorkflowModuleTriggerEnum.UserSubmitsConsultationDraft;
                        referenceId = entity.Id;
                        StoredProc = $"exec pWorkflowSelAdvanceSearch @statusId = '{(int)WorkflowStatusEnum.Active}', @moduleId = '{entity.ModuleId}', @moduleTriggerId = '{moduleTriggerId}',@attachmenttypeId ='{entity.AttachmentTypeId}', @submoduleId='{entity.subModuleId}'";
                    }
                    else if (subModuleId == (int)WorkflowSubModuleEnum.LegalLegislations)
                    {
                        entity = (LegalLegislation)entity;
                        referenceId = entity.LegislationId;
                        StoredProc = $"exec pWorkflowSelAdvanceSearch @statusId = '{(int)WorkflowStatusEnum.Active}', @submoduleId = '{(int)WorkflowSubModuleEnum.LegalLegislations}', @moduleTriggerId =  '{(int)WorkflowModuleTriggerEnum.UserSubmitsDocument}'";
                    }
                    else if (subModuleId == (int)WorkflowSubModuleEnum.LegalPrinciples)
                    {
                        entity = (LLSLegalPrincipleSystem)entity;
                        referenceId = entity.PrincipleId;
                        StoredProc = $"exec pWorkflowSelAdvanceSearch @statusId = '{(int)WorkflowStatusEnum.Active}', @submoduleId='{(int)WorkflowSubModuleEnum.LegalPrinciples}', @moduleTriggerId =  '{(int)WorkflowModuleTriggerEnum.UserSubmitsPrinciple}'";
                    }
                }
                else if (transferType != 0)
                {
                    entity = (CmsApprovalTracking)entity;
                    referenceId = entity.Id;
                    if (transferType == (int)AssignCaseToLawyerTypeEnum.CaseRequest || transferType == (int)AssignCaseToLawyerTypeEnum.CaseFile)
                    {
                        if (entity.ProcessTypeId == (int)ApprovalProcessTypeEnum.SendaCopy)
                        {
                            moduleTriggerId = transferType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? (int)WorkflowModuleTriggerEnum.SendCopyCaseRequest : (int)WorkflowModuleTriggerEnum.SendCopyCaseFile;
                        }
                        else
                        {
                            if (entity.IsConfidential && entity.SectorFrom == (int)OperatingSectorTypeEnum.PrivateOperationalSector && transferType == (int)AssignCaseToLawyerTypeEnum.CaseRequest)
                            {
                                moduleTriggerId = (int)WorkflowModuleTriggerEnum.TransferConfidentialCaseRequestPrivateOffice;
                            }
                            else if (entity.IsConfidential)
                            {
                                moduleTriggerId = transferType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? (int)WorkflowModuleTriggerEnum.TransferConfidentialCaseRequest : (int)WorkflowModuleTriggerEnum.TransferConfidentialCaseFile;
                            }
                            else
                            {
                                moduleTriggerId = transferType == (int)AssignCaseToLawyerTypeEnum.CaseRequest ? (int)WorkflowModuleTriggerEnum.TransferCaseRequest : (int)WorkflowModuleTriggerEnum.TransferCaseFile;
                            }
                        }
                        StoredProc = $"exec pWorkflowSelAdvanceSearch @statusId = '{(int)WorkflowStatusEnum.Active}', @moduleId = '{(int)WorkflowModuleEnum.CaseManagement}', @moduleTriggerId = '{moduleTriggerId}', @submoduleId='{entity.SubModuleId}'";
                    }
                    else if (transferType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest || transferType == (int)AssignCaseToLawyerTypeEnum.ConsultationFile)
                    {
                        if (entity.IsConfidential && entity.SectorFrom == (int)OperatingSectorTypeEnum.PrivateOperationalSector && transferType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest)
                        {
                            moduleTriggerId = (int)WorkflowModuleTriggerEnum.TransferConfidentialConsultationRequestPrivateOffice;
                        }
                        else if (entity.IsConfidential)
                        {
                            moduleTriggerId = transferType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest ? (int)WorkflowModuleTriggerEnum.TransferConfidentialConsultationRequest : (int)WorkflowModuleTriggerEnum.TransferConfidentialConsultationFile;
                        }
                        else
                        {
                            moduleTriggerId = transferType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest ? (int)WorkflowModuleTriggerEnum.TransferConsultationRequest : (int)WorkflowModuleTriggerEnum.TransferConsultationFile;
                        }
                        StoredProc = $"exec pWorkflowSelAdvanceSearch @statusId = '{(int)WorkflowStatusEnum.Active}', @moduleId = '{(int)WorkflowModuleEnum.COMSConsultationManagement}', @moduleTriggerId = '{moduleTriggerId}', @submoduleId='{entity.SubModuleId}'";
                    }
                }
                if (StoredProc != null)
                {
                    var activeWorkflow = await dbContext.WorkflowVM.FromSqlRaw(StoredProc).ToListAsync();
                    if (activeWorkflow?.Count() > 0)
                    {
                        WorkflowActivity firstActivity = await dbContext.WorkflowActivity.Where(a => a.WorkflowId == (int)activeWorkflow.FirstOrDefault().WorkflowId).OrderBy(a => a.SequenceNumber).FirstOrDefaultAsync();
                        WorkflowInstance workflowInstance = new WorkflowInstance { ReferenceId = referenceId, StatusId = (int)WorkflowInstanceStatusEnum.InProgress, WorkflowId = (int)activeWorkflow.FirstOrDefault().WorkflowId, WorkflowActivityId = firstActivity.WorkflowActivityId, IsSlaExecuted = false };
                        SLA sla = await dbContext.SLA.Where(s => s.WorkflowActivityId == workflowInstance.WorkflowActivityId).FirstOrDefaultAsync();
                        if (sla != null)
                        {
                            workflowInstance.ApplySla = true;
                            workflowInstance.SlaStartDate = DateTime.Now.Date.AddDays(1);
                            workflowInstance.SlaEndDate = workflowInstance.SlaStartDate.AddDays(sla.Duration - 1);
                        }
                        else
                        {
                            workflowInstance.ApplySla = false;
                            workflowInstance.SlaStartDate = DateTime.Now.Date;
                            workflowInstance.SlaEndDate = DateTime.Now.Date;
                        }
                        await dbContext.WorkflowInstance.AddAsync(workflowInstance);
                        await dbContext.SaveChangesAsync();
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        public async Task PrepareNotificationParameter(CmsApprovalTracking approvalTracking, DatabaseContext _dbContext)
        {
            // For Notification
            if (approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseRequest)
            {
                approvalTracking.NotificationParameter.Entity = new CaseRequest().GetType().Name;
                approvalTracking.NotificationParameter.ReferenceNumber = _dbContext.CaseRequests.Where(x => x.RequestId == approvalTracking.ReferenceId).FirstOrDefault().RequestNumber;
            }
            else if (approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.CaseFile)
            {
                approvalTracking.NotificationParameter.Entity = new CaseFile().GetType().Name;
                approvalTracking.NotificationParameter.ReferenceNumber = _dbContext.CaseFiles.Where(x => x.FileId == approvalTracking.ReferenceId).FirstOrDefault().FileNumber;
            }
            else if (approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationRequest)
            {
                approvalTracking.NotificationParameter.Entity = new ConsultationRequest().GetType().Name;
                approvalTracking.NotificationParameter.ReferenceNumber = _dbContext.ConsultationRequests.Where(x => x.ConsultationRequestId == approvalTracking.ReferenceId).FirstOrDefault().RequestNumber;
            }
            else if (approvalTracking.TransferCaseType == (int)AssignCaseToLawyerTypeEnum.ConsultationFile)
            {
                approvalTracking.NotificationParameter.Entity = new ConsultationFile().GetType().Name;
                approvalTracking.NotificationParameter.ReferenceNumber = _dbContext.ConsultationFiles.Where(x => x.FileId == approvalTracking.ReferenceId).FirstOrDefault().FileNumber;
            }
            approvalTracking.NotificationParameter.SectorFrom = _dbContext.OperatingSectorType.Where(x => x.Id == approvalTracking.SectorFrom).Select(x => x.Name_En + "/" + x.Name_Ar).FirstOrDefault();
            approvalTracking.NotificationParameter.SectorTo = _dbContext.OperatingSectorType.Where(x => x.Id == approvalTracking.SectorTo).Select(x => x.Name_En + "/" + x.Name_Ar).FirstOrDefault();
        }

        public async Task PrepareNotificationParameter(CmsDraftedTemplate draft, DatabaseContext _dbContext)
        {
            draft.NotificationParameter.DraftName = draft.Name;
            draft.NotificationParameter.DraftNumber = draft.DraftNumber.ToString();
            draft.NotificationParameter.Type = _dbDmsContext.AttachmentType
                                            .Where(x => x.AttachmentTypeId == draft.AttachmentTypeId)
                                            .Select(x => x.Type_En + "/" + x.Type_Ar)
                                            .FirstOrDefault();

            var SubmoduleId = CalculateSubmodule((int)draft.DraftEntityType,draft.ModuleId);

            if (SubmoduleId == (int)SubModuleEnum.CaseRequest)
            {
                draft.NotificationParameter.Entity = new CaseRequest().GetType().Name;
                draft.NotificationParameter.ReferenceNumber = _dbContext.CaseRequests.Where(x => x.RequestId == draft.ReferenceId).FirstOrDefault().RequestNumber;
            }
            else if (SubmoduleId == (int)SubModuleEnum.CaseFile)
            {
                draft.NotificationParameter.Entity = new CaseFile().GetType().Name;
                draft.NotificationParameter.ReferenceNumber = _dbContext.CaseFiles.Where(x => x.FileId == draft.ReferenceId).FirstOrDefault().FileNumber;
            }
            else if (SubmoduleId == (int)SubModuleEnum.ConsultationRequest)
            {
                draft.NotificationParameter.Entity = new ConsultationRequest().GetType().Name;
                draft.NotificationParameter.ReferenceNumber = _dbContext.ConsultationRequests.Where(x => x.ConsultationRequestId == draft.ReferenceId).FirstOrDefault().RequestNumber;
            }
            else if (SubmoduleId == (int)SubModuleEnum.ConsultationFile)
            {
                draft.NotificationParameter.Entity = new ConsultationFile().GetType().Name;
                draft.NotificationParameter.ReferenceNumber = _dbContext.ConsultationFiles.Where(x => x.FileId == draft.ReferenceId).FirstOrDefault().FileNumber;
            }
        }

        private int CalculateSubmodule(int DraftEntityType, int ModuleId)
        {
            int SubmoduleId = 0;
            if (ModuleId == (int)WorkflowModuleEnum.CaseManagement)
            {
                SubmoduleId = DraftEntityType == (int)DraftEntityTypeEnum.RequestNeedMoreInfo
                    ? (int)SubModuleEnum.CaseRequest : ((DraftEntityType == (int)DraftEntityTypeEnum.FileNeedMoreInfo
                    || DraftEntityType == (int)DraftEntityTypeEnum.CaseFile) ? (int)SubModuleEnum.CaseFile : (int)SubModuleEnum.RegisteredCase);
            }
            else if (ModuleId == (int)WorkflowModuleEnum.COMSConsultationManagement)
            {
                SubmoduleId = DraftEntityType == (int)DraftEntityTypeEnum.RequestNeedMoreInfo
                    ? (int)SubModuleEnum.ConsultationRequest : ((DraftEntityType == (int)DraftEntityTypeEnum.FileNeedMoreInfo
                    || DraftEntityType == (int)DraftEntityTypeEnum.ConsultationFile) ? (int)SubModuleEnum.ConsultationFile : (int)SubModuleEnum.ConsultationRequest);
            }
            return SubmoduleId;
        }

        #region Get Vice HOS by Sector and User ID
        public async Task<User> GetViceHOSByUserId(string userId)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                string StoreProc = $"exec pGetViceHosByUserId @userId = '{userId}'";
                var users = await _DbContext.Users.FromSqlRaw(StoreProc).ToListAsync();
                return users.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<User>> GetViceHOSBySectorId(int sectorTypeId)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                string StoreProc = $"exec pGetViceHosBySectorId @sectorTypeId = '{sectorTypeId}'";
                var users = await _DbContext.Users.FromSqlRaw(StoreProc).ToListAsync();
                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<OperatingSectorType> GetViceHosResponsibleDetailBySectorId(int sectorTypeId)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                var resonse = _DbContext.OperatingSectorType.Where(x => x.Id == sectorTypeId).FirstOrDefault();
                return resonse;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion


        public async Task<int> GetNextWorrkflowActivity(Guid draftId)
        {
            try
            {
                string StoredProc = $"exec pWorkflowGetNextActivity @draftId='{draftId}'";
                var WorkflowActivity = await _dbContext.WorkflowActivity.FromSqlRaw(StoredProc).ToListAsync();
                return WorkflowActivity.Any() ? WorkflowActivity.FirstOrDefault().ActivityId : 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task CreateDraftTemplateHistoryLogs(CmsDraftedTemplate document, int actionId, int secondActionId = 0)
        {
            try
            {
                if (actionId == (int)DraftActionIdEnum.Published)
                {
                    var previousVersion = _dbContext.CmsDraftedTemplateVersions.Where(x => x.DraftedTemplateId == document.Id).OrderByDescending(i => i.CreatedDate).Skip(1).Take(1).FirstOrDefault();
                    CmsDraftedTemplateVersionLogs cmsDraftedTemplateVersionLogsPrevious = new CmsDraftedTemplateVersionLogs()
                    {
                        Id = Guid.NewGuid(),
                        VersionId = previousVersion.VersionId,
                        UserId = (Guid)document.UserId,
                        ActionId = secondActionId,
                        CreatedBy = document.userName,
                        CreatedDate = DateTime.Now,
                        ModifiedBy = null,
                        ModifiedDate = null,
                        ReviewerUserId = actionId == (int)DraftActionIdEnum.CreatedAndDraft || actionId == (int)DraftActionIdEnum.EditedAndDraft ? "" :
                    (!string.IsNullOrEmpty(document.DraftedTemplateVersion.ReviewerUserId) ? document.DraftedTemplateVersion.ReviewerUserId : document.DraftedTemplateVersion.ReviewerRoleId)
                    };
                    await _dbContext.CmsDraftedTemplateVersionLogs.AddAsync(cmsDraftedTemplateVersionLogsPrevious);
                    await _dbContext.SaveChangesAsync();
                }
                CmsDraftedTemplateVersionLogs cmsDraftedTemplateVersionLogs = new CmsDraftedTemplateVersionLogs()
                {
                    Id = Guid.NewGuid(),
                    VersionId = document.DraftedTemplateVersion.VersionId,
                    UserId = (Guid)document.UserId,
                    ActionId = actionId,
                    CreatedBy = document.userName,
                    CreatedDate = DateTime.Now,
                    ModifiedBy = null,
                    ModifiedDate = null,
                    ReviewerUserId = actionId == (int)DraftActionIdEnum.CreatedAndDraft || actionId == (int)DraftActionIdEnum.EditedAndDraft ? "" :
                    (!string.IsNullOrEmpty(document.DraftedTemplateVersion.ReviewerUserId) ? document.DraftedTemplateVersion.ReviewerUserId : document.DraftedTemplateVersion.ReviewerRoleId)
                };
                await _dbContext.CmsDraftedTemplateVersionLogs.AddAsync(cmsDraftedTemplateVersionLogs);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #region Workflow Trigger Condition Options
        public async Task<List<WorkflowTriggerConditionOptionsVM>> GetWorkflowTriggerConditionsOptions(int TriggerConditionId, Guid ReferenceId)
        {
            try
            {
                string StoredProc = $"exec pWorkflowTriggerConditionsOptionsById @TriggerConditionId = '{TriggerConditionId}', @ReferenceId='{ReferenceId}'";
                return await _dbContext.WorkflowTriggerConditionOptionsVM.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        
        #endregion
    }
}