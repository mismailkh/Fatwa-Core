using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.WorkflowModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using Telerik.Blazor.Components;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.WorkflowEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;

namespace FATWA_WEB.Pages.Workflows
{
    //<History Author = 'Muhamamd Zaeem' Date='2023-11-30' Version="1.0" Branch="master"> Razor Component for update workflow</History>
    public partial class UpdateWorkflow : ComponentBase
    {
        #region Paramters
        [Parameter]
        public int WorkflowId { get; set; }
        #endregion

        #region Variable Declaration
        public TelerikDropDownList<ModuleTrigger, int> TriggersDDRef { get; set; }
        public TelerikDropDownList<WorkflowSubModule, int> SubModuleTriggersDDRef { get; set; }

        protected Workflow workflow { get; set; } = new Workflow();
        protected int ModuleId { get; set; }
        public MainWorkflowModuleEnum MainWorkflowModuleId { get; set; }
        public int SelectedTab { get; set; }
        protected bool ShowWorkflowDetail { get; set; }
        protected bool ShowAddActivityDialog { get; set; }
        public bool AddConfirmVisible { get; set; }
        public bool SaveDraftConfirmVisible { get; set; }
        public bool CancelConfirmVisible { get; set; }
        public bool CreateAnother { get; set; } = false;
        protected List<MainWorkflowModuleEnumTemp> ModuleResult { get; set; } = new List<MainWorkflowModuleEnumTemp>();
        public class WorkflowControlEnum
        {
            public WorkflowControl WorkflowControlEnumValue { get; set; }
            public string WorkflowControlEnumName { get; set; }
        }
        protected List<object> WorkflowControlOptions { get; set; } = new List<object>();
        protected List<object> SlaActionOptions { get; set; } = new List<object>();
        protected IList<WorkflowCondition> WorkflowConditions { get; set; } = new List<WorkflowCondition>();

        public class WorkflowBranchEnum
        {
            public WorkflowBranch WorkflowBranchEnumValue { get; set; }
            public string WorkflowBranchEnumName { get; set; }
        }
        public class SlaActionEnum
        {
            public SlaAction SlaActionEnumValue { get; set; }
            public string SlaActionEnumName { get; set; }
        }
        public class MainWorkflowModuleEnumTemp
        {
            public MainWorkflowModuleEnum mainWorkflowModuleEnumValue { get; set; }
            public string mainWorkflowModuleEnumName { get; set; }
        }
        protected List<object> WorkflowBranchOptions { get; set; } = new List<object>();
        protected List<ModuleCondition> ModuleConditions { get; set; } = new List<ModuleCondition>();
        protected List<ModuleConditionOptions> ModuleOptions { get; set; } = new List<ModuleConditionOptions>();
        protected List<ModuleActivity> ModuleActivities { get; set; } = new List<ModuleActivity>();
        protected List<WorkflowActivity> WorkflowActivities { get; set; } = new List<WorkflowActivity>();
        protected List<OperatingSectorType> SectorTypes { get; set; } = new List<OperatingSectorType>();

        protected RadzenDataGrid<WorkflowCondition> triggerConditionsGrid;
        protected RadzenDataGrid<WorkflowTriggerSectorOptions> triggerSectorOptionGrid;
        protected RadzenDataGrid<WorkflowConditionOptions> conditionsOptionGrid;
        protected RadzenDataGrid<WorkflowOption> OptionGrid;

        protected Dictionary<int, RadzenDataGrid<WorkflowCondition>> myComponents = new Dictionary<int, RadzenDataGrid<WorkflowCondition>>();
        protected Dictionary<int, RadzenDataGrid<WorkflowOption>> myComponentsOption = new Dictionary<int, RadzenDataGrid<WorkflowOption>>();
        protected Dictionary<Guid?, RadzenDataGrid<WorkflowConditionOptions>> myOptionComponents = new Dictionary<Guid?, RadzenDataGrid<WorkflowConditionOptions>>();

        protected Dictionary<int, RadzenDataGrid<SLA>> mySLAs = new Dictionary<int, RadzenDataGrid<SLA>>();
        public List<AttachmentType> draftTypeResult { get; set; } = new List<AttachmentType>();
        public List<WorkflowSubModule> workflowResultList = new List<WorkflowSubModule>();
        public List<ModuleTrigger> TriggerResultList { get; set; } = new List<ModuleTrigger>();
        protected WorkflowCondition conditionToInsert;
        protected WorkflowConditionOptions optionToInsert;
        protected WorkflowOption activityoptionToInsert;
        protected WorkflowTriggerSectorOptions triggerSectorOptionToInsert;
        protected SLA slaToInsert;
        protected int SelectedActivity { get; set; }
        protected WorkflowBranch SelectedBranch { get; set; }
        #endregion

        #region On Component Load

        //<History Author = 'Hassan Abbas' Date='2022-05-11' Version="1.0" Branch="master"> Initialzie Component -> Show Advance Search For Create / Hide Advance Search and Parse Content For Editing Masking</History>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                spinnerService.Show();
                await PopulateModules();
                workflow.WorkflowActivities = new List<WorkflowActivity>();
                var response = await workflowService.GetWorkflowDetailById(WorkflowId);
                if (response.IsSuccessStatusCode)
                {
                    workflow = (Workflow)response.ResultData;
                    workflow.WorkflowTrigger = await workflowService.GetWorkflowTriggerByWorkflowId(WorkflowId);
                    workflow.ModuleTriggerVM = await workflowService.GetModuleTriggerById((int)workflow.WorkflowTrigger.ModuleTriggerId);
                    workflow.AttachmentTypesList = await workflowService.GetAttachementTypesById(WorkflowId);
                    workflow.AttachmentTypeId = workflow.AttachmentTypesList.Select(x => x.AttachmentTypeId).Cast<int>().AsEnumerable();
                    ModuleId = (int)workflow.ModuleTriggerVM.ModuleId;
                    var Resultresponse = await workflowService.GetSubModuleItems(ModuleId);
                    if (Resultresponse.IsSuccessStatusCode)
                    {
                        workflowResultList = (List<WorkflowSubModule>)Resultresponse.ResultData;

                    }
                    else
                    {
                        customNotificationService.DisplayNotification(Resultresponse);
                    }
                    MainWorkflowModuleId = (MainWorkflowModuleEnum)ModuleId;
                    workflow.WorkflowTrigger = await workflowService.GetWorkflowTriggerByWorkflowId(WorkflowId);
                    await PopulateTriggers();
                    await Task.Delay(100);
                    ShowWorkflowDetail = true;
                    StateHasChanged();

                    workflow.WorkflowActivities = await workflowService.GetWorkflowActivitiesByWorkflowId(WorkflowId);
                    workflow.WorkflowActivities.ForEach(a => { a.ActivityName = "(" + a.SequenceNumber.ToString() + ") " + ModuleActivities.Where(m => m.ActivityId == a.ActivityId).FirstOrDefault().Name; a.UniqueIdentity = Guid.NewGuid(); });
                    foreach (var activity in workflow.WorkflowActivities)
                    {
                        activity.Parameters = await workflowService.GetWorkflowActivityParametersForUpdate(activity.WorkflowActivityId);
                        activity.ModuleActivity = ModuleActivities.Where(m => m.ActivityId == activity.ActivityId).FirstOrDefault();
                        activity.WorkflowConditions = await workflowService.GetWorkflowConditionsForUpdate(activity.WorkflowActivityId);
                        activity.WorkflowOptions = await workflowService.GetWorkflowOptionsForUpdate(activity.WorkflowActivityId);
                        await PopulateActivtySLAsData(activity);
                        foreach (var condition in activity.WorkflowConditions)
                        {
                            condition.IsActivityCondition = true;
                            condition.SequenceNumber = activity.SequenceNumber;
                            condition.MKey = condition.ModuleConditionId > 0 ? ModuleConditions.Where(c => c.ModuleConditionId == condition.ModuleConditionId).FirstOrDefault().MKey : null;
                            condition.ConditionName = condition.ModuleConditionId > 0 ? ModuleConditions.Where(c => c.ModuleConditionId == condition.ModuleConditionId).FirstOrDefault().Name : null;
                            condition.WorkflowConditionOptionLists = await workflowService.GetWorkflowConditionsOptionsList(condition.WorkflowConditionId);
                            if (condition.TrueCaseActivityNo > 0)
                            {
                                condition.TrueCaseActivityName = workflow.WorkflowActivities.Where(c => c.SequenceNumber == condition.TrueCaseActivityNo).FirstOrDefault().ActivityName;
                                condition.UniqueIdentity = workflow.WorkflowActivities.Where(c => c.SequenceNumber == condition.TrueCaseActivityNo).FirstOrDefault().UniqueIdentity;
                            }
                            if (condition.FalseCaseActivityNo > 0)
                                condition.FalseCaseActivityName = workflow.WorkflowActivities.Where(c => c.SequenceNumber == condition.FalseCaseActivityNo).FirstOrDefault().ActivityName;
                            foreach (var conditionOptions in condition.WorkflowConditionOptionLists)
                            {
                                condition.workflowConditionOptions.Add(new WorkflowConditionOptions
                                {
                                    WorkflowOptionId = (int)conditionOptions.WorkflowOptionId,
                                    ModuleOptionId = (int)conditionOptions.ModuleOptionId,
                                    WorkflowConditionId = condition.WorkflowConditionId,
                                    TrueCaseFlowControlId = (WorkflowControl)conditionOptions.TrueCaseFlowControlId,
                                    TrueCaseActivityNo = (int)conditionOptions.TrueCaseActivityNo,
                                    TrueCaseActivityName = (conditionOptions.TrueCaseActivityNo > 0) ? workflow.WorkflowActivities.Where(c => c.SequenceNumber == conditionOptions.TrueCaseActivityNo).FirstOrDefault().ActivityName : null,
                                    OptionName = ModuleOptions.Where(c => c.ModuleOptionId == conditionOptions.ModuleOptionId).FirstOrDefault().Name,
                                    ConditionGuid = condition.ConditionGuid,
                                    SequenceNumber= condition.SequenceNumber,
                                });

                            }
                        }
                        foreach (var option in activity.WorkflowOptions)
                        {
                            option.IsActivityOption = true;
                            option.SequenceNumber = activity.SequenceNumber;
                            //option.MKey = ModuleOptions.Where(c => c.ModuleOptionId == option.ModuleOptionId).FirstOrDefault().MKey;
                            option.OptionName = ModuleOptions.Where(c => c.ModuleOptionId == option.ModuleOptionId).FirstOrDefault().Name;
                            if (option.TrueCaseActivityNo > 0)
                            {
                                option.TrueCaseActivityName = workflow.WorkflowActivities.Where(c => c.SequenceNumber == option.TrueCaseActivityNo).FirstOrDefault().ActivityName;
                                option.UniqueIdentity = workflow.WorkflowActivities.Where(c => c.SequenceNumber == option.TrueCaseActivityNo).FirstOrDefault().UniqueIdentity;
                            }
                        }
                    }
                    await PopulateWorkflowTriggerCondtions();
                    await GetTriggerDetails();
                    await PopulateWorkflowTriggerSectorOptions();
                    StateHasChanged();
                }

                foreach (WorkflowControl item in Enum.GetValues(typeof(WorkflowControl)))
                {
                    WorkflowControlOptions.Add(new WorkflowControlEnum { WorkflowControlEnumName = translationState.Translate(item.ToString()), WorkflowControlEnumValue = item });
                }
                foreach (WorkflowBranch item in Enum.GetValues(typeof(WorkflowBranch)))
                {
                    WorkflowBranchOptions.Add(new WorkflowBranchEnum { WorkflowBranchEnumName = translationState.Translate(item.ToString()), WorkflowBranchEnumValue = item });
                }
                foreach (SlaAction item in Enum.GetValues(typeof(SlaAction)))
                {
                    SlaActionOptions.Add(new SlaActionEnum { SlaActionEnumName = translationState.Translate(item.ToString()), SlaActionEnumValue = item });
                }
                spinnerService.Hide();

            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        #endregion

        #region Virtual Dropdown (Modules, Triggers) Functions

        //<History Author = 'Hassan Abbas' Date='2022-04-23' Version="1.0" Branch="master"> Function for getting subject records for the drop down dynamically from the API</History>
        protected async Task PopulateWorkflowTriggerCondtions()
        {
            try
            {
                var response = await workflowService.GetWorkflowTriggerConditionsByTriggerId(workflow.WorkflowTrigger.WorkflowTriggerId);
                if (response.IsSuccessStatusCode)
                {
                    workflow.WorkflowTrigger.workflowTriggerConditions = (List<WorkflowTriggerCondition>)response.ResultData;
                    if (workflow.WorkflowTrigger.workflowTriggerConditions.Count() > 0)
                    {
                        workflow.WorkflowTrigger.HasConditions = true;
                        foreach (var condition in workflow.WorkflowTrigger.workflowTriggerConditions)
                        {
                            var triggerCondition = new WorkflowCondition
                            {
                                ModuleConditionId = condition.ConditionId,
                                TrueCaseFlowControlId = condition.TrueCaseFlowControlId,
                                TrueCaseActivityNo = condition.TrueCaseActivityNo,
                                ConditionName = condition.ConditionId > 0 ? ModuleConditions.Where(c => c.ModuleConditionId == condition.ConditionId).FirstOrDefault().Name : null,
                                TrueCaseActivityName = condition.TrueCaseActivityNo > 0 ? workflow.WorkflowActivities.Where(c => c.SequenceNumber == condition.TrueCaseActivityNo).FirstOrDefault().ActivityName : null,
                                IsOption = condition.IsOption
                            };
                            triggerCondition.workflowConditionOptions = condition.workflowTriggerConditionOptions.Select(option => new WorkflowConditionOptions
                            {
                                WorkflowOptionId = option.WorkflowOptionId,
                                ModuleOptionId = option.ModuleOptionId,
                                WorkflowConditionId = option.TriggerConditionId,
                                TrueCaseFlowControlId = option.TrueCaseFlowControlId,
                                TrueCaseActivityNo = option.TrueCaseActivityNo,
                                TrueCaseActivityName = (option.TrueCaseActivityNo > 0) ? workflow.WorkflowActivities.Where(c => c.SequenceNumber == option.TrueCaseActivityNo).FirstOrDefault().ActivityName : null,
                                OptionName = ModuleOptions.Where(c => c.ModuleOptionId == option.ModuleOptionId).FirstOrDefault().Name,
                                ConditionGuid = triggerCondition.ConditionGuid,
                            }).ToList();
                            workflow.WorkflowTrigger.WorkflowConditions.Add(triggerCondition);
                        }
                    }
                }
                else
                {
                    customNotificationService.DisplayNotification(response);

                }

            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        protected async Task PopulateWorkflowTriggerSectorOptions()
        {
            try
            {
                var response = await workflowService.GetWorkflowTriggerSectorOptions(workflow.WorkflowTrigger.WorkflowTriggerId);
                if (response.IsSuccessStatusCode)
                {
                    workflow.WorkflowTrigger.WorkflowTriggerSectorOptions = (List<WorkflowTriggerSectorOptions>)response.ResultData;
                    foreach (var sectorOptions in workflow.WorkflowTrigger.WorkflowTriggerSectorOptions)
                    {
                        sectorOptions.SectorFrom.Id = sectorOptions.SectorFromId;
                        sectorOptions.SectorFrom.Name_En = sectorOptions.SectorFromId > 0 ? SectorTypes.Where(x => x.Id == sectorOptions.SectorFromId).FirstOrDefault().Name_En : null;
                        sectorOptions.SectorFrom.Name_Ar = sectorOptions.SectorFromId > 0 ? SectorTypes.Where(x => x.Id == sectorOptions.SectorFromId).FirstOrDefault().Name_Ar : null;
                        var TransferOptionresponse = await workflowService.GetWorkflowTriggerSectorTransferOptions(sectorOptions.TriggerOptionId);
                        if (TransferOptionresponse.IsSuccessStatusCode)
                        {
                            var SectorToId = (List<int>)TransferOptionresponse.ResultData;
                            sectorOptions.SectorToIds = SectorToId.Select(id => new OperatingSectorType { Id = id, Name_En = id > 0 ? SectorTypes.Where(x => x.Id == id).FirstOrDefault().Name_En : null, Name_Ar = id > 0 ? SectorTypes.Where(x => x.Id == id).FirstOrDefault().Name_Ar : null });
                        }
                    }
                }
                else
                {
                    customNotificationService.DisplayNotification(response);

                }

            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        protected async Task PopulateModules()
        {
            ModuleResult = new List<MainWorkflowModuleEnumTemp>();
            foreach (MainWorkflowModuleEnum item in Enum.GetValues(typeof(MainWorkflowModuleEnum)))
            {
                MainWorkflowModuleEnumTemp mainWorkflowModuleEnumTemp = new MainWorkflowModuleEnumTemp();
                mainWorkflowModuleEnumTemp = new MainWorkflowModuleEnumTemp { mainWorkflowModuleEnumName = translationState.Translate(item.ToString()), mainWorkflowModuleEnumValue = item };
                ModuleResult.Add(mainWorkflowModuleEnumTemp);
            }
            await ResetWorkflowConfiguration(false, true);
            await ReinitializeActivityAndConditionLists();
            ShowWorkflowDetail = false;
        }

        protected async Task ReinitializeActivityAndConditionLists()
        {
            try
            {
                ModuleConditions = new List<ModuleCondition>();
                ModuleActivities = new List<ModuleActivity>();
            }
            catch (Exception ex)
            {

            }
        }
        protected async Task ResetWorkflowConfiguration(bool resetTriggerAndActivities, bool resetSubmodule)
        {
            try
            {
                ShowWorkflowDetail = false;
                if (resetTriggerAndActivities)
                {
                    workflow.WorkflowTrigger = new WorkflowTrigger();
                    workflow.ModuleTriggerVM = new ModuleTriggerVM();
                    workflow.WorkflowTrigger.ModuleTriggerId = 0;
                    workflow.WorkflowActivities = new List<WorkflowActivity>();
                }
                if (resetSubmodule)
                {
                    workflow.SubModuleId = 0;
                }
                workflow.AttachmentTypeId = null;
                conditionToInsert = null;
            }
            catch (Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        public async Task GetSubModuleDataList()
        {
            try
            {
                ModuleId = (int)MainWorkflowModuleId;
                if (ModuleId > 0)
                {
                    var response = await workflowService.GetSubModuleItems(ModuleId);
                    if (response.IsSuccessStatusCode)
                    {
                        workflowResultList = (List<WorkflowSubModule>)response.ResultData;
                        workflow.WorkflowActivities = new List<WorkflowActivity>();
                        workflow.ModuleTriggerVM = new ModuleTriggerVM();
                        workflow.WorkflowTrigger = new WorkflowTrigger();
                        workflow.WorkflowTrigger.ModuleTriggerId = 0;
                        workflow.SubModuleId = 0;
                        workflow.AttachmentTypeId = null;
                        await ReinitializeActivityAndConditionLists();
                        ShowWorkflowDetail = false;
                        TriggerResultList = new List<ModuleTrigger>();
                    }
                    else
                    {
                        customNotificationService.DisplayNotification(response);
                    }
                }
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }

        protected async Task PopulateSectorTypes()
        {
            var response = await lookupService.GetOperatingSectorTypes();
            if (response.IsSuccessStatusCode)
            {
                SectorTypes = (List<OperatingSectorType>)response.ResultData;
                if (workflow.SubModuleId == (int)WorkflowSubModuleEnum.Administrative)
                {
                    SectorTypes = SectorTypes.Where(s => s.Id <= (int)OperatingSectorTypeEnum.AdministrativeSupremeCases || (s.Id >= (int)OperatingSectorTypeEnum.PrivateOperationalSector && s.Id <= (int)OperatingSectorTypeEnum.FatwaPresidentOffice) || s.Id == (int)OperatingSectorTypeEnum.AdministrativeGeneralSupervisor).ToList();
                }
                else if (workflow.SubModuleId == (int)WorkflowSubModuleEnum.CivilCommercial)
                {
                    SectorTypes = SectorTypes.Where(s => (s.Id >= (int)OperatingSectorTypeEnum.CivilCommercialUnderFilingCases && s.Id <= (int)OperatingSectorTypeEnum.CivilCommercialPartialUrgentCases) || (s.Id >= (int)OperatingSectorTypeEnum.PrivateOperationalSector && s.Id <= (int)OperatingSectorTypeEnum.PublicOperationalSector) || s.Id == (int)OperatingSectorTypeEnum.CivilCommercialGeneralSupervisor).ToList();
                }
                else
                {
                    SectorTypes = SectorTypes.Where(s => (s.Id >= (int)OperatingSectorTypeEnum.LegalAdvice && s.Id <= (int)OperatingSectorTypeEnum.PublicOperationalSector)).ToList();
                }
            }
            else
            {
                await invalidRequestHandlerService.ReturnBadRequestNotification(response);
            }
            StateHasChanged();
        }

        //<History Author = 'Hassan Abbas' Date='2022-04-23' Version="1.0" Branch="master"> Function for getting subject records for the drop down dynamically from the API</History>
        public async Task GetRemoteTriggersData()
        {
            if (workflow.SubModuleId > 0)
            {
                var response = await workflowService.GetTriggerItemsData(workflow.SubModuleId);
                if (response.IsSuccessStatusCode)
                {
                    TriggerResultList = (List<ModuleTrigger>)response.ResultData;
                }
                else
                {
                    customNotificationService.DisplayNotification(response);
                }
            }
        }

        public async Task GetAttachmentTypeofDraft()
        {
            try
            {
                ModuleId = (int)MainWorkflowModuleId;
                if (ModuleId > 0 && workflow.SubModuleId > 0)
                {
                    if (MainWorkflowModuleId == MainWorkflowModuleEnum.LegalLibrarySystem && workflow.SubModuleId == (int)WorkflowSubModuleEnum.Legislations)
                    {
                        ModuleId = (int)WorkflowModuleEnum.LDSDocument;
                    }
                    if (MainWorkflowModuleId == MainWorkflowModuleEnum.LegalLibrarySystem && workflow.SubModuleId == (int)WorkflowSubModuleEnum.LegalPrinciples)
                    {
                        ModuleId = (int)WorkflowModuleEnum.LPSPrinciple;
                    }
                    var response = await lookupService.GetAttachmentTypes(ModuleId, true);
                    if (response.IsSuccessStatusCode)
                    {
                        draftTypeResult = (List<AttachmentType>)response.ResultData;
                    }
                    else
                    {
                        customNotificationService.DisplayNotification(response);
                    }
                }
            }
            catch (Exception)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_Went_Wrong"),
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
        }
        protected async Task PopulateTriggers()
        {
            if (ModuleId > 0 && workflow.SubModuleId > 0)
            {
                ModuleConditions = await workflowService.GetModuleConditions(workflow.WorkflowTrigger.ModuleTriggerId);
                ModuleActivities = await workflowService.GetModuleActvities(workflow.WorkflowTrigger.ModuleTriggerId);
                var response = await workflowService.GetModuleOptionsByTriggerId(workflow.WorkflowTrigger.ModuleTriggerId);
                ModuleOptions = (List<ModuleConditionOptions>)response.ResultData;
                StateHasChanged();

                await GetRemoteTriggersData();
                await GetAttachmentTypeofDraft();
            }
            else
            {
                await ResetWorkflowConfiguration(true, false);
                await ReinitializeActivityAndConditionLists();
                StateHasChanged();
            }
        }

        protected async Task GetTriggerDetails()
        {
            spinnerService.Show();
            ShowWorkflowDetail = false;
            if (workflow.WorkflowTrigger.ModuleTriggerId != null && (workflow.WorkflowTrigger.ModuleTriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsCaseDraft || workflow.WorkflowTrigger.ModuleTriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsConsultationDraft ||
            workflow.WorkflowTrigger.ModuleTriggerId == (int)WorkflowModuleTriggerEnum.UserSubmitsDMSDocument))
            {
                if ((int)workflow.WorkflowTrigger.ModuleTriggerId > 0 && (workflow.AttachmentTypeId != null && workflow.AttachmentTypeId.Count() > 0) && (int)workflow.SubModuleId > 0)
                {
                    workflow.ModuleTriggerVM = await workflowService.GetModuleTriggerById((int)workflow.WorkflowTrigger.ModuleTriggerId);
                    ShowWorkflowDetail = true;
                }
            }
            else
            {
                if ((int)workflow.WorkflowTrigger.ModuleTriggerId > 0 && (int)workflow.SubModuleId > 0)
                {
                    workflow.ModuleTriggerVM = await workflowService.GetModuleTriggerById((int)workflow.WorkflowTrigger.ModuleTriggerId);
                    ShowWorkflowDetail = true;
                }
                else
                {
                    ShowWorkflowDetail = false;
                }
            }
            ModuleConditions = await workflowService.GetModuleConditions(workflow.WorkflowTrigger.ModuleTriggerId);
            ModuleActivities = await workflowService.GetModuleActvities(workflow.WorkflowTrigger.ModuleTriggerId);
            await PopulateModuleConditionOptionData();
            await PopulateSectorTypes();
            spinnerService.Hide();
        }
        protected async Task PopulateModuleConditionOptionData()
        {
            try
            {
                var response = await workflowService.GetModuleOptionsByTriggerId(workflow.WorkflowTrigger.ModuleTriggerId);
                if (response.IsSuccessStatusCode)
                {
                    ModuleOptions = (List<ModuleConditionOptions>)response.ResultData;
                }
                else
                {
                    customNotificationService.DisplayNotification(response);
                }

            }
            catch (Exception ex)
            {

            }
        }
        protected async Task PopulateActivtySLAsData(WorkflowActivity workflowActivity)
        {
            try
            {
                var slaResponse = await workflowService.GetActivtySlAsByActivityId(workflowActivity.WorkflowActivityId);
                if (slaResponse.IsSuccessStatusCode)
                {
                    workflowActivity.SLAs = (List<SLA>)slaResponse.ResultData;
                    if (workflowActivity.SLAs.Count() > 0)
                    {
                        workflowActivity.HasSLA = true;

                    }
                    foreach (var sla in workflowActivity.SLAs)
                    {
                        sla.Parameters = await workflowService.GetSlaActionParameters((int)Convert.ChangeType(sla.ActionId, sla.ActionId.GetTypeCode()));

                        var slaParamresponse = await workflowService.GetActivtySLAsActionParameterBySLAId(sla.WorkflowSLAId);
                        if (slaParamresponse.IsSuccessStatusCode)
                        {
                            var slaParameter = (List<SLAActionParameters>)slaParamresponse.ResultData;
                            sla.Parameters = sla.Parameters
                                            .Select(parameter =>
                                            {
                                                var slaActionParameter = slaParameter.FirstOrDefault(sla => sla.ParameterId == parameter.ParameterId);
                                                if (slaActionParameter != null)
                                                {
                                                    parameter.Value = slaActionParameter.Value;
                                                }
                                                return parameter;
                                            })
                                            .ToList();
                        }
                        else
                        {
                            customNotificationService.DisplayNotification(slaParamresponse);

                        }
                    }

                }
                else
                {
                    customNotificationService.DisplayNotification(slaResponse);

                }

            }
            catch (Exception ex)
            {

            }
        }
        #endregion
        #region Workflow Trigger Condition options
        protected void OnTriggerConditionOptionCreateRow(WorkflowConditionOptions option)
        {
            option.OptionName = ModuleOptions.Where(c => c.ModuleOptionId == option.ModuleOptionId).FirstOrDefault().Name;
            if (option.TrueCaseActivityNo > 0)
            {
                option.TrueCaseActivityName = workflow.WorkflowActivities.Where(c => c.SequenceNumber == option.TrueCaseActivityNo).FirstOrDefault().ActivityName;
                option.UniqueIdentity = workflow.WorkflowActivities.Where(c => c.SequenceNumber == option.TrueCaseActivityNo).FirstOrDefault().UniqueIdentity;
            }
            workflow.WorkflowTrigger.WorkflowConditions.Where(a => a.ConditionGuid == option.ConditionGuid).FirstOrDefault().workflowConditionOptions.Add(option);
        }
        protected void OnTriggerConditionOptionUpdateRow(WorkflowConditionOptions option)
        {
            if (option == optionToInsert)
            {
                optionToInsert = null;
            }
            workflow.WorkflowTrigger.WorkflowConditions.Where(a => a.ConditionGuid == option.ConditionGuid).FirstOrDefault().workflowConditionOptions.Remove(option);
            if (option.TrueCaseFlowControlId == WorkflowControl.EndofFlow)
            {
                option.TrueCaseActivityNo = 0;
                option.TrueCaseActivityName = null;
            }
            option.OptionName = ModuleOptions.Where(c => c.ModuleOptionId == option.ModuleOptionId).FirstOrDefault().Name;
            if (option.TrueCaseActivityNo > 0)
            {
                option.TrueCaseActivityName = workflow.WorkflowActivities.Where(c => c.SequenceNumber == option.TrueCaseActivityNo).FirstOrDefault().ActivityName;
                option.UniqueIdentity = workflow.WorkflowActivities.Where(c => c.SequenceNumber == option.TrueCaseActivityNo).FirstOrDefault().UniqueIdentity;
            }
            workflow.WorkflowTrigger.WorkflowConditions.Where(a => a.ConditionGuid == option.ConditionGuid).FirstOrDefault().workflowConditionOptions.Add(option);

        }
        #endregion
        #region Workflow Trigger Conditions
        protected async Task InsertRow(bool isActivityCondition, int sequenceNumber)
        {
            try
            {
                if (!isActivityCondition)
                {
                    conditionToInsert = new WorkflowCondition();
                    await triggerConditionsGrid.InsertRow(conditionToInsert);
                }
                else
                {
                    conditionToInsert = new WorkflowCondition { IsActivityCondition = true, SequenceNumber = sequenceNumber };
                    await myComponents[sequenceNumber].InsertRow(conditionToInsert);
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void OnConditionChange(WorkflowCondition condition, WorkflowActivity? activity)
        {
            condition.MKey = ModuleConditions.Where(c => c.ModuleConditionId == condition.ModuleConditionId).FirstOrDefault().MKey;
            condition.IsLawyerTask = false;
            if (activity != null && condition.TrueCaseFlowControlId == WorkflowControl.EndofFlow && (condition.MKey == "CmsDraftStatusRejectedBySupervisor" || condition.MKey == "CmsDraftStatusRejectedByHOS" || condition.MKey == "ComsDraftStatusRejectedBySupervisor" || condition.MKey == "ComsDraftStatusRejectedByHOS"))
            {
                activity.isColVisible = true;

            }

        }
        protected void OnFlowControlChange(WorkflowCondition condition, WorkflowActivity activity)
        {
            condition.IsLawyerTask = false;
            if (condition.TrueCaseFlowControlId == WorkflowControl.EndofFlow && (condition.MKey == "CmsDraftStatusRejectedBySupervisor" || condition.MKey == "CmsDraftStatusRejectedByHOS" || condition.MKey == "ComsDraftStatusRejectedBySupervisor" || condition.MKey == "ComsDraftStatusRejectedByHOS"))
            {
                activity.isColVisible = true;

            }
        }
        protected void OnCreateRow(WorkflowCondition condition)
        {
            if (!condition.IsActivityCondition)
            {
                condition.ConditionName = ModuleConditions.Where(c => c.ModuleConditionId == condition.ModuleConditionId).FirstOrDefault().Name;
                condition.ConditionGuid = Guid.NewGuid();
                if (condition.TrueCaseActivityNo > 0)
                {
                    condition.TrueCaseActivityName = workflow.WorkflowActivities.Where(c => c.SequenceNumber == condition.TrueCaseActivityNo).FirstOrDefault().ActivityName;
                    condition.UniqueIdentity = workflow.WorkflowActivities.Where(a => a.SequenceNumber == condition.TrueCaseActivityNo).FirstOrDefault().UniqueIdentity;
                }

                if (condition.FalseCaseActivityNo > 0)
                    condition.FalseCaseActivityName = workflow.WorkflowActivities.Where(c => c.SequenceNumber == condition.FalseCaseActivityNo).FirstOrDefault().ActivityName;
                workflow.WorkflowTrigger.WorkflowConditions.Add(condition);
            }
            else
            {
                condition.ConditionName = ModuleConditions.Where(c => c.ModuleConditionId == condition.ModuleConditionId).FirstOrDefault().Name;
                condition.ConditionGuid = Guid.NewGuid();
                if (condition.TrueCaseActivityNo > 0)
                {
                    condition.TrueCaseActivityName = workflow.WorkflowActivities.Where(c => c.SequenceNumber == condition.TrueCaseActivityNo).FirstOrDefault().ActivityName;
                    condition.UniqueIdentity = workflow.WorkflowActivities.Where(a => a.SequenceNumber == condition.TrueCaseActivityNo).FirstOrDefault().UniqueIdentity;
                }


                if (condition.FalseCaseActivityNo > 0)
                    condition.FalseCaseActivityName = workflow.WorkflowActivities.Where(c => c.SequenceNumber == condition.FalseCaseActivityNo).FirstOrDefault().ActivityName;
                workflow.WorkflowActivities.Where(a => a.SequenceNumber == condition.SequenceNumber).FirstOrDefault().WorkflowConditions.Add(condition);
            }
        }
        protected void OnUpdateRow(WorkflowCondition condition)
        {
            if (condition == conditionToInsert)
            {
                conditionToInsert = null;
            }
            if (!condition.IsActivityCondition)
            {
                workflow.WorkflowTrigger.WorkflowConditions.Remove(condition);

                if (condition.TrueCaseFlowControlId == WorkflowControl.EndofFlow)
                {
                    condition.TrueCaseActivityNo = 0;
                    condition.TrueCaseActivityName = null;
                }
                if (condition.FalseCaseFlowControlId == WorkflowControl.EndofFlow)
                {
                    condition.FalseCaseActivityNo = 0;
                    condition.FalseCaseActivityName = null;
                }
                condition.ConditionName = ModuleConditions.Where(c => c.ModuleConditionId == condition.ModuleConditionId).FirstOrDefault().Name;
                //condition.ConditionGuid = Guid.NewGuid();
                if (condition.TrueCaseActivityNo > 0)
                {
                    condition.TrueCaseActivityName = workflow.WorkflowActivities.Where(c => c.SequenceNumber == condition.TrueCaseActivityNo).FirstOrDefault().ActivityName;
                    condition.UniqueIdentity = workflow.WorkflowActivities.Where(a => a.SequenceNumber == condition.TrueCaseActivityNo).FirstOrDefault().UniqueIdentity;
                }
                if (condition.FalseCaseActivityNo > 0)
                    condition.FalseCaseActivityName = workflow.WorkflowActivities.Where(c => c.SequenceNumber == condition.FalseCaseActivityNo).FirstOrDefault().ActivityName;
                workflow.WorkflowTrigger.WorkflowConditions.Add(condition);
            }
            else
            {
                workflow.WorkflowActivities.Where(a => a.SequenceNumber == condition.SequenceNumber).FirstOrDefault().WorkflowConditions.Remove(condition);

                if (condition.TrueCaseFlowControlId == WorkflowControl.EndofFlow)
                {
                    condition.TrueCaseActivityNo = 0;
                    condition.TrueCaseActivityName = null;
                }
                if (condition.FalseCaseFlowControlId == WorkflowControl.EndofFlow)
                {
                    condition.FalseCaseActivityNo = 0;
                    condition.FalseCaseActivityName = null;
                }

                condition.ConditionName = ModuleConditions.Where(c => c.ModuleConditionId == condition.ModuleConditionId).FirstOrDefault().Name;
                //condition.ConditionGuid = Guid.NewGuid();
                if (condition.TrueCaseActivityNo > 0)
                {
                    condition.TrueCaseActivityName = workflow.WorkflowActivities.Where(c => c.SequenceNumber == condition.TrueCaseActivityNo).FirstOrDefault().ActivityName;
                    condition.UniqueIdentity = workflow.WorkflowActivities.Where(a => a.SequenceNumber == condition.TrueCaseActivityNo).FirstOrDefault().UniqueIdentity;
                }
                if (condition.FalseCaseActivityNo > 0)
                    condition.FalseCaseActivityName = workflow.WorkflowActivities.Where(c => c.SequenceNumber == condition.FalseCaseActivityNo).FirstOrDefault().ActivityName;
                workflow.WorkflowActivities.Where(a => a.SequenceNumber == condition.SequenceNumber).FirstOrDefault().WorkflowConditions.Add(condition);
            }
        }

        protected async Task EditRow(WorkflowCondition condition)
        {
            if (!condition.IsActivityCondition)
            {
                await triggerConditionsGrid.EditRow(condition);
            }
            else
            {
                await myComponents[condition.SequenceNumber].EditRow(condition);
            }
        }

        protected async Task SaveRow(WorkflowCondition condition)
        {
            if (condition == conditionToInsert)
            {
                conditionToInsert = null;
            }

            if (!condition.IsActivityCondition)
            {
                await triggerConditionsGrid.UpdateRow(condition);
            }
            else
            {
                await myComponents[condition.SequenceNumber].UpdateRow(condition);
            }
        }

        protected void CancelEdit(WorkflowCondition condition)
        {
            if (condition == conditionToInsert)
            {
                conditionToInsert = null;
            }

            if (!condition.IsActivityCondition)
            {
                triggerConditionsGrid.CancelEditRow(condition);
            }
            else
            {
                myComponents[condition.SequenceNumber].CancelEditRow(condition);
            }
        }

        protected async Task DeleteRow(WorkflowCondition condition, WorkflowActivity? activity)
        {
            if (condition == conditionToInsert || conditionToInsert != null)
            {
                conditionToInsert = null;
            }

            if (!condition.IsActivityCondition)
            {
                if (workflow.WorkflowTrigger.WorkflowConditions.Contains(condition))
                {
                    workflow.WorkflowTrigger.WorkflowConditions.Remove(condition);
                    await triggerConditionsGrid.Reload();
                }
                else
                {
                    triggerConditionsGrid.CancelEditRow(condition);
                }
            }
            else
            {
                if (workflow.WorkflowActivities.Where(a => a.SequenceNumber == condition.SequenceNumber).FirstOrDefault().WorkflowConditions.Contains(condition))
                {
                    workflow.WorkflowActivities.Where(a => a.SequenceNumber == condition.SequenceNumber).FirstOrDefault().WorkflowConditions.Remove(condition);
                    await myComponents[condition.SequenceNumber].Reload();
                }
                else
                {
                    myComponents[condition.SequenceNumber].CancelEditRow(condition);
                }
            }
            if (activity != null)
                activity.isColVisible = false;
        }

        protected void OnRowCollapse()
        {
            if (optionToInsert != null)
                optionToInsert = null;
        }
        #endregion

        #region Trigger Transfer Option

        protected async Task InsertRowTriggerSectorOption()
        {
            try
            {
                triggerSectorOptionToInsert = new WorkflowTriggerSectorOptions();
                await triggerSectorOptionGrid.InsertRow(triggerSectorOptionToInsert);
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }

        protected async Task OnCreateRowTriggerSectorOption(WorkflowTriggerSectorOptions triggerSectorOption)
        {
            workflow.WorkflowTrigger.WorkflowTriggerSectorOptions.Add(triggerSectorOption);
        }
        protected async Task OnUpdateRowTriggerSectorOption(WorkflowTriggerSectorOptions triggerSectorOption)
        {
            if (triggerSectorOption == triggerSectorOptionToInsert)
            {
                triggerSectorOptionToInsert = null;
            }
            workflow.WorkflowTrigger.WorkflowTriggerSectorOptions.Remove(triggerSectorOption);

            //condition.ConditionName = ModuleConditions.Where(c => c.ModuleConditionId == condition.ModuleConditionId).FirstOrDefault().Name;
            //condition.ConditionGuid = Guid.NewGuid();
            //if (condition.TrueCaseActivityNo > 0)
            //    condition.TrueCaseActivityName = workflow.WorkflowActivities.Where(c => c.SequenceNumber == condition.TrueCaseActivityNo).FirstOrDefault().ActivityName;
            //if (condition.FalseCaseActivityNo > 0)
            //    condition.FalseCaseActivityName = workflow.WorkflowActivities.Where(c => c.SequenceNumber == condition.FalseCaseActivityNo).FirstOrDefault().ActivityName;
            //triggerSectorOption.SectorName = Enum.GetName(typeof(OperatingSectorTypeEnum), triggerSectorOption.SectorId);
            workflow.WorkflowTrigger.WorkflowTriggerSectorOptions.Add(triggerSectorOption);
        }

        protected async Task EditRowTriggerSectorOption(WorkflowTriggerSectorOptions triggerSectorOption)
        {
            triggerSectorOptionToInsert = triggerSectorOption;
            await triggerSectorOptionGrid.EditRow(triggerSectorOption);
        }

        protected async Task SaveRowTriggerSectorOption(WorkflowTriggerSectorOptions triggerSectorOption)
        {
            if (triggerSectorOption == triggerSectorOptionToInsert)
            {
                triggerSectorOptionToInsert = null;
            }

            await triggerSectorOptionGrid.UpdateRow(triggerSectorOption);
        }

        protected void CancelEditRowTriggerSectorOption(WorkflowTriggerSectorOptions triggerSectorOption)
        {
            if (triggerSectorOption == triggerSectorOptionToInsert)
            {
                triggerSectorOptionToInsert = null;
            }

            triggerSectorOptionGrid.CancelEditRow(triggerSectorOption);
        }

        protected async Task DeleteRowTriggerSectorOption(WorkflowTriggerSectorOptions triggerSectorOption)
        {
            if (triggerSectorOption == triggerSectorOptionToInsert || triggerSectorOptionToInsert != null)
            {
                triggerSectorOptionToInsert = null;
            }

            if (workflow.WorkflowTrigger.WorkflowTriggerSectorOptions.Contains(triggerSectorOption))
            {
                workflow.WorkflowTrigger.WorkflowTriggerSectorOptions.Remove(triggerSectorOption);
                await triggerSectorOptionGrid.Reload();
            }
            else
            {
                triggerSectorOptionGrid.CancelEditRow(triggerSectorOption);
            }
        }

        #endregion

        #region SLAs
        protected async Task InsertRowSLA(int sequenceNumber)
        {
            try
            {
                slaToInsert = new SLA { SequenceNumber = sequenceNumber };
                await mySLAs[sequenceNumber].InsertRow(slaToInsert);
                StateHasChanged();
            }
            catch (Exception ex)
            {

            }
        }

        protected async Task OnCreateRowSLA(SLA sla)
        {
            if (sla.Duration == 0)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = "Invalid SLA duration!",
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                return;
            }
            else
            {
                sla.Parameters = await workflowService.GetSlaActionParameters((int)Convert.ChangeType(sla.ActionId, sla.ActionId.GetTypeCode()));
                workflow.WorkflowActivities.Where(a => a.SequenceNumber == sla.SequenceNumber).FirstOrDefault().SLAs.Add(sla);
                if (!sla.IsExpanded)
                {
                    mySLAs[sla.SequenceNumber].ExpandRow(sla);
                    sla.IsExpanded = true;
                    StateHasChanged();
                }
            }
        }
        protected async Task OnUpdateRowSLA(SLA sla)
        {
            if (sla == slaToInsert)
            {
                slaToInsert = null;
            }
            workflow.WorkflowActivities.Where(a => a.SequenceNumber == sla.SequenceNumber).FirstOrDefault().SLAs.Remove(sla);
            sla.Parameters = await workflowService.GetSlaActionParameters((int)Convert.ChangeType(sla.ActionId, sla.ActionId.GetTypeCode()));
            workflow.WorkflowActivities.Where(a => a.SequenceNumber == sla.SequenceNumber).FirstOrDefault().SLAs.Add(sla);
            if (!sla.IsExpanded)
            {
                mySLAs[sla.SequenceNumber].ExpandRow(sla);
                sla.IsExpanded = true;
                StateHasChanged();
            }
        }

        protected async Task EditRowSLA(SLA sla)
        {
            await mySLAs[sla.SequenceNumber].EditRow(sla);
        }

        protected async Task SaveRowSLA(SLA sla)
        {
            if (sla == slaToInsert)
            {
                slaToInsert = null;
            }

            await mySLAs[sla.SequenceNumber].UpdateRow(sla);
        }

        protected void CancelEditSLA(SLA sla)
        {
            if (sla == slaToInsert)
            {
                slaToInsert = null;
            }

            mySLAs[sla.SequenceNumber].CancelEditRow(sla);
        }

        protected async Task DeleteRowSLA(SLA sla)
        {
            if (sla == slaToInsert)
            {
                slaToInsert = null;
            }

            if (workflow.WorkflowActivities.Where(a => a.SequenceNumber == sla.SequenceNumber).FirstOrDefault().SLAs.Contains(sla))
            {
                workflow.WorkflowActivities.Where(a => a.SequenceNumber == sla.SequenceNumber).FirstOrDefault().SLAs.Remove(sla);
                await mySLAs[sla.SequenceNumber].Reload();
            }
            else
            {
                mySLAs[sla.SequenceNumber].CancelEditRow(sla);
            }
        }
        #endregion

        #region Activities

        protected async Task AddActivity(int sequenceNumber, bool insertAtEnd)
        {
            if (workflow.WorkflowActivities.Any() && insertAtEnd == true)
            {
                sequenceNumber = workflow.WorkflowActivities.LastOrDefault().SequenceNumber;
            }

            var result = await dialogService.OpenAsync<AddActivity>(translationState.Translate("Choose_Activity"),
                   new Dictionary<string, object>()
                   {
                       { "TriggerId", workflow.WorkflowTrigger.ModuleTriggerId },
                       { "isFirstActivity", workflow.WorkflowActivities.Any() ? false : true },
                       { "isPreviousActivityGeneral", workflow.WorkflowActivities.Any() ? workflow.WorkflowActivities.Find(a => a.SequenceNumber == sequenceNumber).ModuleActivity.CategoryId == (int)WorkflowActivityCategory.GeneralControls ? true : false : true },
                       { "IsOptional", workflow.ModuleTriggerVM.IsOptional }

                   },
                   new DialogOptions() { Width = "400px", Resizable = true, CloseDialogOnOverlayClick = true });

            bool isClose = false;
            bool isActivity = false;
            bool isBranch = false;
            try
            {
                if (!(bool)result)
                {
                    isClose = true;
                    isActivity = false;
                    isBranch = false;
                }
            }
            catch (Exception ex)
            { }
            try
            {
                if ((ModuleActivity)result != null)
                {
                    isClose = false;
                    isActivity = true;
                    isBranch = false;
                }
            }
            catch (Exception ex1)
            { }
            try
            {
                if ((WorkflowBranch)result != null)
                {
                    isClose = false;
                    isActivity = false;
                    isBranch = true;
                }
            }
            catch (Exception ex2)
            { }

            if (isClose)
            {
            }
            else if (isActivity)
            {
                var activity = (ModuleActivity)result;
                if (sequenceNumber == 0)
                {
                    var parames = await workflowService.GetModuleActivityParameters(activity.ActivityId);
                    workflow.WorkflowActivities.Add(new WorkflowActivity { ActivityId = activity.ActivityId, ActivityName = "(" + (sequenceNumber + 1).ToString() + ") " + activity.Name, SequenceNumber = sequenceNumber + 1, ModuleActivity = activity, WorkflowConditions = new List<WorkflowCondition>(), Parameters = parames, UniqueIdentity = Guid.NewGuid() });
                }
                else
                {
                    var parames = await workflowService.GetModuleActivityParameters(activity.ActivityId);
                    workflow.WorkflowActivities.Insert(workflow.WorkflowActivities.FindIndex(a => a.SequenceNumber == sequenceNumber) + 1, new WorkflowActivity { ActivityId = activity.ActivityId, ActivityName = "(" + (sequenceNumber + 1).ToString() + ") " + activity.Name, SequenceNumber = workflow.WorkflowActivities.Count() + 1, ModuleActivity = activity, WorkflowConditions = new List<WorkflowCondition>(), Parameters = parames, UniqueIdentity = Guid.NewGuid() });
                    await ReorderWorkflowActivities();
                }
            }
            else if (isBranch)
            {
                var branch = (WorkflowBranch)result;
                workflow.WorkflowActivities.Find(a => a.SequenceNumber == sequenceNumber).BranchId = branch;
            }
            //var activity = ModuleActivities.Where(a => a.ActivityId == SelectedActivity).FirstOrDefault();
            //workflow.WorkflowActivities.Add(new WorkflowActivity { ActivityId = activity.ActivityId, SequenceNumber = workflow.WorkflowActivities.Count() + 1, ModuleActivity = activity, WorkflowConditions = new List<WorkflowCondition>() });
            //SelectedActivity = 0;
        }

        protected async Task RemoveActivity(WorkflowActivity activity)
        {

            if (workflow.WorkflowTrigger.WorkflowConditions.Any(a => a.TrueCaseActivityNo.Equals(activity.SequenceNumber)) ||
                workflow.WorkflowTrigger.WorkflowOptions.Any(a => a.TrueCaseActivityNo.Equals(activity.SequenceNumber)) ||
                workflow.WorkflowActivities.Any(a => a.WorkflowConditions.Any(a => a.TrueCaseActivityNo.Equals(activity.SequenceNumber)) || a.WorkflowOptions.Any(a => a.TrueCaseActivityNo.Equals(activity.SequenceNumber)))
                )
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = "Activity_Selected_JumpTo",
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
            }
            else
            {
                workflow.WorkflowActivities.Remove(activity);
                await ReorderWorkflowActivities();
                conditionToInsert = null;
                activityoptionToInsert = null;
                optionToInsert = null;
            }
        }

        protected async Task ReorderWorkflowActivities()
        {
            int orderNo = 1;
            foreach (var activity in workflow.WorkflowActivities)
            {

                activity.SequenceNumber = orderNo;
                activity.ActivityName = "(" + activity.SequenceNumber.ToString() + ") " + translationState.Translate(activity.ModuleActivity.Name);

                foreach (var sla in activity.SLAs)
                {
                    sla.SequenceNumber = orderNo;
                }
                foreach (var conditionsitem in activity.WorkflowConditions)
                {
                    conditionsitem.SequenceNumber = orderNo;
                }
                foreach (var optionsitem in activity.WorkflowOptions)
                {
                    optionsitem.SequenceNumber = orderNo;
                }
                if (workflow.WorkflowActivities.Any(a => a.WorkflowConditions.Any(a => a.UniqueIdentity == activity.UniqueIdentity)))
                {
                    var conditions = workflow.WorkflowActivities.SelectMany(a => a.WorkflowConditions.Where((a => a.UniqueIdentity == activity.UniqueIdentity)));
                    foreach (var condition in conditions)
                    {
                        condition.TrueCaseActivityNo = orderNo;
                        condition.TrueCaseActivityName = activity.ActivityName;
                    }
                }

                if (workflow.WorkflowTrigger.WorkflowConditions.Any(a => a.UniqueIdentity == activity.UniqueIdentity))
                {
                    var conditions = workflow.WorkflowTrigger.WorkflowConditions.Where(a => a.UniqueIdentity == activity.UniqueIdentity).ToList();
                    foreach (var condition in conditions)
                    {
                        condition.TrueCaseActivityNo = orderNo;
                        condition.TrueCaseActivityName = activity.ActivityName;
                    }
                }

                if (workflow.WorkflowActivities.Any(a => a.WorkflowConditions.Any(a => a.workflowConditionOptions.Any(a => a.UniqueIdentity == activity.UniqueIdentity))))
                {
                    var options = workflow.WorkflowActivities.SelectMany(a => a.WorkflowConditions.SelectMany(a => a.workflowConditionOptions.Where(a => a.UniqueIdentity == activity.UniqueIdentity)));
                    foreach (var option in options)
                    {
                        option.TrueCaseActivityNo = orderNo;
                        option.TrueCaseActivityName = activity.ActivityName;
                    }
                }
                if (workflow.WorkflowActivities.Any(a => a.WorkflowOptions.Any(a => a.UniqueIdentity == activity.UniqueIdentity)))
                {
                    var options = workflow.WorkflowActivities.SelectMany(a => a.WorkflowOptions.Where(a => a.UniqueIdentity == activity.UniqueIdentity));
                    foreach (var option in options)
                    {
                        option.TrueCaseActivityNo = orderNo;
                        option.TrueCaseActivityName = activity.ActivityName;
                    }
                }
                orderNo++;
            }
            StateHasChanged();
        }


        protected async Task AddBranch()
        {
            workflow.WorkflowActivities.LastOrDefault().BranchId = SelectedBranch;
            SelectedBranch = 0;
        }

        protected async Task RemoveActivity(int sequenceNumber)
        {

        }
        #endregion

        #region Workflow Activity Options
        protected async Task InsertActivityOptionRow(bool isActivityOption, int sequenceNumber)
        {
            try
            {
                if (!isActivityOption)
                {
                    activityoptionToInsert = new WorkflowOption();
                    await OptionGrid.InsertRow(activityoptionToInsert);
                }
                else
                {
                    activityoptionToInsert = new WorkflowOption { IsActivityOption = true, SequenceNumber = sequenceNumber };
                    await myComponentsOption[sequenceNumber].InsertRow(activityoptionToInsert);
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void OnActivityOptionUpdateRow(WorkflowOption option)
        {
            if (option == activityoptionToInsert)
            {
                activityoptionToInsert = null;
            }
            if (!option.IsActivityOption)
            {
                workflow.WorkflowTrigger.WorkflowOptions.Remove(option);

                if (option.TrueCaseFlowControlId == WorkflowControl.EndofFlow)
                {
                    option.TrueCaseActivityNo = 0;
                    option.TrueCaseActivityName = null;
                }

                option.OptionName = ModuleOptions.Where(c => c.ModuleOptionId == option.ModuleOptionId).FirstOrDefault().Name;
                option.ConditionGuid = Guid.NewGuid();
                if (option.TrueCaseActivityNo > 0)
                {
                    option.TrueCaseActivityName = workflow.WorkflowActivities.Where(c => c.SequenceNumber == option.TrueCaseActivityNo).FirstOrDefault().ActivityName;
                    option.UniqueIdentity = workflow.WorkflowActivities.Where(c => c.SequenceNumber == option.TrueCaseActivityNo).FirstOrDefault().UniqueIdentity;
                }
                workflow.WorkflowTrigger.WorkflowOptions.Add(option);
            }
            else
            {
                workflow.WorkflowActivities.Where(a => a.SequenceNumber == option.SequenceNumber).FirstOrDefault().WorkflowOptions.Remove(option);

                if (option.TrueCaseFlowControlId == WorkflowControl.EndofFlow)
                {
                    option.TrueCaseActivityNo = 0;
                    option.TrueCaseActivityName = null;
                }

                option.OptionName = ModuleOptions.Where(c => c.ModuleOptionId == option.ModuleOptionId).FirstOrDefault().Name;
                if (option.TrueCaseActivityNo > 0)
                {
                    option.TrueCaseActivityName = workflow.WorkflowActivities.Where(c => c.SequenceNumber == option.TrueCaseActivityNo).FirstOrDefault().ActivityName;
                    option.UniqueIdentity = workflow.WorkflowActivities.Where(c => c.SequenceNumber == option.TrueCaseActivityNo).FirstOrDefault().UniqueIdentity;
                }
                workflow.WorkflowActivities.Where(a => a.SequenceNumber == option.SequenceNumber).FirstOrDefault().WorkflowOptions.Add(option);
            }
        }
        protected void OnActivityOptionCreateRow(WorkflowOption option)
        {
            if (!option.IsActivityOption)
            {
                option.OptionName = ModuleOptions.Where(c => c.ModuleOptionId == option.ModuleOptionId).FirstOrDefault().Name;
                if (option.TrueCaseActivityNo > 0)
                {
                    option.TrueCaseActivityName = workflow.WorkflowActivities.Where(c => c.SequenceNumber == option.TrueCaseActivityNo).FirstOrDefault().ActivityName;
                    option.UniqueIdentity = workflow.WorkflowActivities.Where(c => c.SequenceNumber == option.TrueCaseActivityNo).FirstOrDefault().UniqueIdentity;
                }
                workflow.WorkflowTrigger.WorkflowOptions.Add(option);
            }
            else
            {
                option.OptionName = ModuleOptions.Where(c => c.ModuleOptionId == option.ModuleOptionId).FirstOrDefault().Name;
                if (option.TrueCaseActivityNo > 0)
                {
                    option.TrueCaseActivityName = workflow.WorkflowActivities.Where(c => c.SequenceNumber == option.TrueCaseActivityNo).FirstOrDefault().ActivityName;
                    option.UniqueIdentity = workflow.WorkflowActivities.Where(c => c.SequenceNumber == option.TrueCaseActivityNo).FirstOrDefault().UniqueIdentity;
                }
                workflow.WorkflowActivities.Where(a => a.SequenceNumber == option.SequenceNumber).FirstOrDefault().WorkflowOptions.Add(option);
            }
        }
        protected async Task EditActivityOptionRow(WorkflowOption option)
        {

            activityoptionToInsert = option;
            if (!option.IsActivityOption)
            {
                await OptionGrid.EditRow(option);
            }
            else
            {
                await myComponentsOption[option.SequenceNumber].EditRow(option);
            }
        }
        protected async Task SaveActivityOptionRow(WorkflowOption option)
        {
            if (option == activityoptionToInsert)
            {
                activityoptionToInsert = null;
            }

            if (!option.IsActivityOption)
            {
                await OptionGrid.UpdateRow(option);
            }
            else
            {
                await myComponentsOption[option.SequenceNumber].UpdateRow(option);
            }
        }
        protected void CancelActivtityOptionEdit(WorkflowOption option)
        {
            if (option == activityoptionToInsert)
            {
                activityoptionToInsert = null;
            }

            if (!option.IsActivityOption)
            {
                OptionGrid.CancelEditRow(option);
            }
            else
            {
                myComponentsOption[option.SequenceNumber].CancelEditRow(option);
            }
        }
        protected async Task DeleteActivityOptionRow(WorkflowOption option, WorkflowActivity activity)
        {
            if (option == activityoptionToInsert || activityoptionToInsert != null)
            {
                activityoptionToInsert = null;
            }

            if (!option.IsActivityOption)
            {
                if (workflow.WorkflowTrigger.WorkflowOptions.Contains(option))
                {
                    workflow.WorkflowTrigger.WorkflowOptions.Remove(option);
                    await OptionGrid.Reload();
                }
                else
                {
                    OptionGrid.CancelEditRow(option);
                }
            }
            else
            {
                if (workflow.WorkflowActivities.Where(a => a.SequenceNumber == option.SequenceNumber).FirstOrDefault().WorkflowOptions.Contains(option))
                {
                    workflow.WorkflowActivities.Where(a => a.SequenceNumber == option.SequenceNumber).FirstOrDefault().WorkflowOptions.Remove(option);
                    await myComponentsOption[option.SequenceNumber].Reload();
                }
                else
                {
                    myComponentsOption[option.SequenceNumber].CancelEditRow(option);
                }
            }
        }

        #endregion

        #region SLA On Change

        protected async Task OnSLAChange(WorkflowActivity activity)
        {
            workflow.WorkflowActivities.Where(a => a.WorkflowActivityId == activity.ActivityId).FirstOrDefault().HasSLA = activity.HasSLA;
        }
        #endregion

        #region Workflow Buttons
        protected async Task SubmitWorkflow()
        {
            if (String.IsNullOrEmpty(workflow.Name))
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = "Please provide workflow name",
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });

                return;
            }

            if (workflow.WorkflowActivities.Count() <= 0)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = "No Activities found for the workflow. Please try again.",
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });

                return;
            }

            bool hasEndofFlow = false;
            foreach (var activity in workflow.WorkflowActivities)
            {
                foreach (var condition in activity.WorkflowConditions)
                {
                    if (condition.TrueCaseFlowControlId == WorkflowControl.EndofFlow || condition.FalseCaseFlowControlId == WorkflowControl.EndofFlow)
                    {
                        hasEndofFlow = true;
                    }
                    if (hasEndofFlow)
                    {
                        break;
                    }
                }
                foreach (var options in activity.WorkflowOptions)
                {
                    if (options.TrueCaseFlowControlId == WorkflowControl.EndofFlow)
                    {
                        hasEndofFlow = true;
                    }
                    if (hasEndofFlow)
                    {
                        break;
                    }
                }
                if (activity.ModuleActivity.IsEndofFlow)
                {
                    hasEndofFlow = true;
                }


                if (hasEndofFlow)
                {
                    break;
                }
            }

            if (!hasEndofFlow)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = "No EndofFlow found for the workflow. Please try again.",
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                return;
            }
            bool mandatoryParamsFilled = true;
            foreach (var ac in workflow.WorkflowActivities)
            {
                foreach (var pa in ac.Parameters)
                {
                    if (pa.Mandatory && String.IsNullOrEmpty(pa.Value))
                    {
                        pa.Class = "k-invalid";
                        mandatoryParamsFilled = false;
                    }
                    else
                    {
                        pa.Class = "k-valid";
                    }
                }
            }

            if (!mandatoryParamsFilled)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = "Please fill required activity parameters.",
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                return;
            }
            bool? dialogResponse = await dialogService.Confirm(
            translationState.Translate("Are_you_sure_you_want_to_submit_workflow?"),
            translationState.Translate("Submit_Workflow"),
            new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("No")
            });
            if (dialogResponse == true)
            {
                spinnerService.Show();
                workflow.StatusId = (int)WorkflowStatusEnum.InReview;
                var response = await workflowService.CreateWorkflow(workflow);
                if(response.IsSuccessStatusCode)
                {
                    spinnerService.Hide();
                    notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = "Workflow has been submitted successfully",
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        if (CreateAnother)
                        {
                            await JSRuntime.InvokeVoidAsync("window.location.reload");
                        }
                        else
                        {
                            navigationManager.NavigateTo("workflows");
                        }
                }
                else
                {
                    spinnerService.Hide();
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                dialogService.Close();
            }
            else
            {
                dialogService.Close();
            }

        }
        protected async Task SaveWorkflow()
        {
            if (String.IsNullOrEmpty(workflow.Name))
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = "Please provide workflow name",
                    Style = "position: fixed !important; left: 0; margin: auto; "
                });
                SaveDraftConfirmVisible = false;
                return;
            }
            bool? dialogResponse = await dialogService.Confirm(
            translationState.Translate("Sure_Save_As_Draft"),
            translationState.Translate("Save_Draft"),
            new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("No")
            });

            if (dialogResponse == true)
            {
                workflow.StatusId = (int)WorkflowStatusEnum.Draft;
                Workflow result;
                var response = await workflowService.CreateWorkflow(workflow);
                if(response.IsSuccessStatusCode)
                {
                        notificationService.Notify(new NotificationMessage()
                        {
                            Severity = NotificationSeverity.Success,
                            Detail = "Draft has been saved successfully",
                            Style = "position: fixed !important; left: 0; margin: auto; "
                        });
                        if (CreateAnother)
                        {
                            await JSRuntime.InvokeVoidAsync("window.location.reload");
                        }
                        else
                        {
                            navigationManager.NavigateTo("workflows");
                        }

                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
              
            }
            else
            {
                dialogService.Close();
            }
        }
        protected async Task CancelWorkflow()
        {
            bool? dialogResponse = await dialogService.Confirm(
            translationState.Translate("Sure_Cancel"),
            translationState.Translate("Cancel"),
            new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("No")
            });
            if (dialogResponse == true)
            {
                navigationManager.NavigateTo("workflows");
            }
            else
            {
                dialogService.Close();
            }
        }
        #endregion

        #region Save Draft Dialog
        //<History Author = 'Hassan Abbas' Date='2022-03-30' Version="1.0" Branch="master">Change Confirm Dialog Visibility</History>
        protected void VisibleChangedHandlerDraft(bool currVisible)
        {
            SaveDraftConfirmVisible = currVisible;
        }

        //<History Author = 'Hassan Abbas' Date='2022-04-23' Version="1.0" Branch="master">Submit Document on Comfirmation og Dialog</History>

        //<History Author = 'Hassan Abbas' Date='2022-04-23' Version="1.0" Branch="master">Close Add Document Confirm Dialog on Click of Cancel Button</History>

        #endregion

        #region Add Workflow Dialog
        //<History Author = 'Hassan Abbas' Date='2022-03-30' Version="1.0" Branch="master">Change Confirm Dialog Visibility</History>
        //protected void VisibleChangedHandlerAdd(bool currVisible)
        //{
        //    AddConfirmVisible = currVisible;
        //}

        //<History Author = 'Hassan Abbas' Date='2022-04-23' Version="1.0" Branch="master">Submit Document on Comfirmation og Dialog</History>

        //<History Author = 'Hassan Abbas' Date='2022-04-23' Version="1.0" Branch="master">Close Add Document Confirm Dialog on Click of Cancel Button</History>

        #endregion

        #region Cancel Creating Workflow Dialog

        //<History Author = 'Hassan Abbas' Date='2022-04-23' Version="1.0" Branch="master">Change Confirm Dialog Visibility</History>

        //<History Author = 'Hassan Abbas' Date='2022-03-30' Version="1.0" Branch="master">redirect to document list on Cancel Dialog Confirmation</History>

        //<History Author = 'Hassan Abbas' Date='2022-03-30' Version="1.0" Branch="master">Close cancel adding document details Confirm Dialog</History>

        #endregion

        #region Redirect Events

        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        #endregion

        #region Option Change Handler
        private void ChangeHandler(WorkflowCondition condition)
        {
            if (condition.IsOption == false)
            {
                condition.IsCollapse = false;
                condition.IsOption = true;
                condition.TrueCaseFlowControlId = 0;
                condition.TrueCaseActivityNo = 0;
                condition.TrueCaseActivityName = null;
                SaveRow(condition);
                if (!condition.IsActivityCondition)
                {
                    triggerConditionsGrid.ExpandRow(condition);
                }
                else
                {
                    myComponents[condition.SequenceNumber].ExpandRow(condition);
                }
            }
            else
            {
                condition.IsCollapse = true;
                condition.IsOption = false;
                conditionToInsert = condition;
                EditRow(condition);
                condition.workflowConditionOptions = new List<WorkflowConditionOptions>();
                if (!condition.IsActivityCondition)
                {
                    triggerConditionsGrid.ExpandRow(condition);
                }
                else
                {
                    myComponents[condition.SequenceNumber].ExpandRow(condition);
                }
                optionToInsert = null;
            }
        }
        private async Task DeleteConditionOptions(WorkflowCondition condition)
        {
            bool? dialogResponse = await dialogService.Confirm(
            translationState.Translate("Sure_Delete"),
            translationState.Translate("Confirm"),
            new ConfirmOptions()
            {
                OkButtonText = translationState.Translate("OK"),
                CancelButtonText = translationState.Translate("No"),
                Width = "25%",
                Resizable = true,
                CloseDialogOnOverlayClick = true
            });
            if (dialogResponse == true)
            {
                ChangeHandler(condition);
            }
        }
        public void ConditionOptionsDetail(WorkflowCondition condition)
        {
            myComponents[condition.SequenceNumber].ExpandRow(condition);
            condition.IsCollapse = false;
        }
        #endregion

        #region Workflow Condtion Options
        protected void OnOptionCreateRow(WorkflowConditionOptions option)
        {

            option.OptionName = ModuleOptions.Where(c => c.ModuleOptionId == option.ModuleOptionId).FirstOrDefault().Name;
            if (option.TrueCaseActivityNo > 0)
            {
                option.TrueCaseActivityName = workflow.WorkflowActivities.Where(c => c.SequenceNumber == option.TrueCaseActivityNo).FirstOrDefault().ActivityName;
                option.UniqueIdentity = workflow.WorkflowActivities.Where(c => c.SequenceNumber == option.TrueCaseActivityNo).FirstOrDefault().UniqueIdentity;
            }

            workflow.WorkflowActivities.Where(a => a.SequenceNumber == option.SequenceNumber).FirstOrDefault().WorkflowConditions.Where(c => c.ConditionGuid == option.ConditionGuid).FirstOrDefault().workflowConditionOptions.Add(option);

        }

        protected async Task InsertOptionRow(bool isOptionCondition, Guid? ConditionGuid, int SequenceNumber)
        {
            try
            {
                if (!isOptionCondition)
                {
                    optionToInsert = new WorkflowConditionOptions();
                    await conditionsOptionGrid.InsertRow(optionToInsert);
                }
                else
                {
                    optionToInsert = new WorkflowConditionOptions { IsOptionCondition = true, ConditionGuid = ConditionGuid, SequenceNumber = SequenceNumber };
                    await myOptionComponents[ConditionGuid].InsertRow(optionToInsert);
                }
            }
            catch (Exception ex)
            {

            }
        }
        protected void OnOptionUpdateRow(WorkflowConditionOptions option)
        {
            if (option == optionToInsert)
            {
                optionToInsert = null;
            }
            workflow.WorkflowActivities.Where(a => a.SequenceNumber == option.SequenceNumber).FirstOrDefault().WorkflowConditions.Where(c => c.ConditionGuid == option.ConditionGuid).FirstOrDefault().workflowConditionOptions.Remove(option);
            if (option.TrueCaseFlowControlId == WorkflowControl.EndofFlow)
            {
                option.TrueCaseActivityNo = 0;
                option.TrueCaseActivityName = null;
            }
            option.OptionName = ModuleOptions.Where(c => c.ModuleOptionId == option.ModuleOptionId).FirstOrDefault().Name;
            if (option.TrueCaseActivityNo > 0)
            {
                option.TrueCaseActivityName = workflow.WorkflowActivities.Where(c => c.SequenceNumber == option.TrueCaseActivityNo).FirstOrDefault().ActivityName;
                option.UniqueIdentity = workflow.WorkflowActivities.Where(c => c.SequenceNumber == option.TrueCaseActivityNo).FirstOrDefault().UniqueIdentity;
            }
            workflow.WorkflowActivities.Where(a => a.SequenceNumber == option.SequenceNumber).FirstOrDefault().WorkflowConditions.Where(c => c.ConditionGuid == option.ConditionGuid).FirstOrDefault().workflowConditionOptions.Add(option);

        }
        protected async Task EditOptionRow(WorkflowConditionOptions option)
        {
            await myOptionComponents[option.ConditionGuid].EditRow(option);

        }
        protected async Task SaveOptionRow(WorkflowConditionOptions option)
        {
            if (option == optionToInsert)
            {
                optionToInsert = null;
            }
            option.OptionName = ModuleOptions.Where(c => c.ModuleOptionId == option.ModuleOptionId).FirstOrDefault().Name;
            await myOptionComponents[option.ConditionGuid].UpdateRow(option);

        }
        protected void CancelOptionEdit(WorkflowConditionOptions option)
        {
            if (option == optionToInsert)
            {
                conditionToInsert = null;
            }
            myOptionComponents[option.ConditionGuid].CancelEditRow(option);

        }
        protected async Task DeleteOptionRow(WorkflowCondition condition, WorkflowConditionOptions option)
        {
            if (option == optionToInsert || optionToInsert != null)
            {
                optionToInsert = null;
            }
            if (!condition.IsActivityCondition)
            {
                if (condition.workflowConditionOptions.Contains(option))
                {
                    condition.workflowConditionOptions.Remove(option);
                    await myOptionComponents[option.ConditionGuid].Reload();
                }
                else
                {
                    myOptionComponents[option.ConditionGuid].CancelEditRow(option);
                }
            }
            else
            {
                if (workflow.WorkflowActivities.Where(a => a.SequenceNumber == condition.SequenceNumber).FirstOrDefault().WorkflowConditions.Where(c => c.ConditionGuid == option.ConditionGuid).FirstOrDefault().workflowConditionOptions.Contains(option))
                {
                    workflow.WorkflowActivities.Where(a => a.SequenceNumber == condition.SequenceNumber).FirstOrDefault().WorkflowConditions.Where(c => c.ConditionGuid == option.ConditionGuid).FirstOrDefault().workflowConditionOptions.Remove(option);
                    await myOptionComponents[option.ConditionGuid].Reload();
                }
                else
                {
                    myOptionComponents[option.ConditionGuid].CancelEditRow(option);
                }
            }
        }

        #endregion

    }
}
