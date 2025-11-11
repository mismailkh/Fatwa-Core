using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.WorkflowModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;
using static FATWA_DOMAIN.Enums.WorkflowEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_WEB.Pages.Workflows.CreateWorkflow;

namespace FATWA_WEB.Pages.Workflows
{
    public partial class DetailWorkflow : ComponentBase
    {
        #region Parameter
        [Parameter]
        public int WorkflowId { get; set; }
        #endregion

        #region Variaible Declaration
        protected Workflow workflow { get; set; } = new Workflow();
        protected List<MainWorkflowModuleEnumTemp> ModuleResult { get; set; } = new List<MainWorkflowModuleEnumTemp>();
        protected List<ModuleCondition> ModuleConditions { get; set; } = new List<ModuleCondition>();
        protected List<ModuleActivity> ModuleActivities { get; set; } = new List<ModuleActivity>();
        protected List<ModuleConditionOptions> ModuleOptions { get; set; } = new List<ModuleConditionOptions>();
        protected bool ShowWorkflowDetail { get; set; }
        protected int ModuleId { get; set; }
        public MainWorkflowModuleEnum MainWorkflowModuleId { get; set; }
        public List<ModuleTrigger> TriggerResultList { get; set; } = new List<ModuleTrigger>();
        public List<AttachmentType> draftTypeResult { get; set; } = new List<AttachmentType>();
        protected List<object> WorkflowControlOptions { get; set; } = new List<object>();
        protected List<object> SlaActionOptions { get; set; } = new List<object>();
        protected List<object> WorkflowBranchOptions { get; set; } = new List<object>();
        protected SLA slaToInsert;
        protected Dictionary<int, RadzenDataGrid<SLA>> mySLAs = new Dictionary<int, RadzenDataGrid<SLA>>();
        protected Dictionary<int, RadzenDataGrid<WorkflowCondition>> myComponents = new Dictionary<int, RadzenDataGrid<WorkflowCondition>>();
        protected Dictionary<int, RadzenDataGrid<WorkflowOption>> myComponentsOption = new Dictionary<int, RadzenDataGrid<WorkflowOption>>();
        protected Dictionary<Guid?, RadzenDataGrid<WorkflowConditionOptions>> myOptionComponents = new Dictionary<Guid?, RadzenDataGrid<WorkflowConditionOptions>>();
        protected Dictionary<Guid?, RadzenDataGrid<WorkflowConditionsOptionsListVM>> myOptionComponentslist = new Dictionary<Guid?, RadzenDataGrid<WorkflowConditionsOptionsListVM>>();
        protected RadzenDataGrid<WorkflowCondition> triggerConditionsGrid;
        protected RadzenDataGrid<WorkflowTriggerSectorOptions> triggerSectorOptionGrid;
        protected RadzenDataGrid<WorkflowConditionOptions> conditionsOptionGrid;
        protected RadzenDataGrid<WorkflowOption> OptionGrid;
        string attachmentTypesEn = string.Empty;
        string attachmentTypesAr = string.Empty;
        protected List<OperatingSectorType> SectorTypes { get; set; } = new List<OperatingSectorType>();

        #endregion

        #region On Component Load

        //<History Author = 'Hassan Abbas' Date='2022-05-11' Version="1.0" Branch="master"> Initialzie Component -> Show Advance Search For Create / Hide Advance Search and Parse Content For Editing Masking</History>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                spinnerService.Show();
                await Load();
                spinnerService.Hide();
            }
            catch (Exception ex)
            {

            }
        }
        protected async Task Load()
        {
            try
            {
                GetRemoteModulesData();
                var response = await workflowService.GetWorkflowDetailById(WorkflowId);
                if (response.IsSuccessStatusCode)
                {
                    workflow = (Workflow)response.ResultData;
                    workflow.WorkflowTrigger = await workflowService.GetWorkflowTriggerByWorkflowId(WorkflowId);
                    workflow.ModuleTriggerVM = await workflowService.GetModuleTriggerById((int)workflow.WorkflowTrigger.ModuleTriggerId);
                    workflow.AttachmentTypesList = await workflowService.GetAttachementTypesById(WorkflowId);
                    ModuleId = (int)workflow.ModuleTriggerVM.ModuleId;
                    workflow.WorkflowTrigger = await workflowService.GetWorkflowTriggerByWorkflowId(WorkflowId);
                    await PopulateTriggers();
                    await Task.Delay(100);
                    ShowWorkflowDetail = true;
                    StateHasChanged();

                    workflow.WorkflowActivities = await workflowService.GetWorkflowActivitiesByWorkflowId(WorkflowId);
                    workflow.WorkflowActivities.ForEach(a => a.ActivityName = "(" + a.SequenceNumber.ToString() + ") " + @translationState.Translate(ModuleActivities.Where(m => m.ActivityId == a.ActivityId).FirstOrDefault().Name));

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
                            condition.MKey = ModuleConditions.Where(c => c.ModuleConditionId == condition.ModuleConditionId).FirstOrDefault().MKey;
                            condition.ConditionName = ModuleConditions.Where(c => c.ModuleConditionId == condition.ModuleConditionId).FirstOrDefault().Name;
                            condition.WorkflowConditionOptionLists = await workflowService.GetWorkflowConditionsOptionsList(condition.WorkflowConditionId);
                            if (condition.TrueCaseActivityNo > 0)
                                condition.TrueCaseActivityName = workflow.WorkflowActivities.Where(c => c.SequenceNumber == condition.TrueCaseActivityNo).FirstOrDefault().ActivityName;
                            if (condition.FalseCaseActivityNo > 0)
                                condition.FalseCaseActivityName = workflow.WorkflowActivities.Where(c => c.SequenceNumber == condition.FalseCaseActivityNo).FirstOrDefault().ActivityName;
                            foreach (var conditionOptions in condition.WorkflowConditionOptionLists)
                            {
                                if (conditionOptions.TrueCaseActivityNo > 0)
                                {
                                    conditionOptions.TrueCaseActivityName = workflow.WorkflowActivities.Where(c => c.SequenceNumber == conditionOptions.TrueCaseActivityNo).FirstOrDefault().ActivityName;
                                }
                            }
                        }
                        foreach (var option in activity.WorkflowOptions)
                        {
                            option.IsActivityOption = true;
                            option.SequenceNumber = activity.SequenceNumber;
                            option.OptionName = ModuleOptions.Where(c => c.ModuleOptionId == option.ModuleOptionId).FirstOrDefault().Name;
                            if (option.TrueCaseActivityNo > 0)
                                option.TrueCaseActivityName = workflow.WorkflowActivities.Where(c => c.SequenceNumber == option.TrueCaseActivityNo).FirstOrDefault().ActivityName;
                        }
                    }
                    attachmentTypesEn = string.Join(", ", workflow.AttachmentTypesList.Select(item => item.Type_En));
                    attachmentTypesAr = string.Join(", ", workflow.AttachmentTypesList.Select(item => item.Type_Ar));
                    await PopulateWorkflowTriggerCondtions();
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
            }
            catch(Exception ex)
            {
                notificationService.Notify(new NotificationMessage()
                {
                    Severity = NotificationSeverity.Error,
                    Detail = translationState.Translate("Something_went_wrong_Please_try_again"),
                    Style = "position: fixed !important; left: 0; margin: auto;"
                });
            }
        }
        #endregion

        #region Populate Functions
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
                            workflow.WorkflowTrigger.WorkflowConditions.Add(new WorkflowCondition
                            {
                                ModuleConditionId = condition.ConditionId,
                                TrueCaseFlowControlId = condition.TrueCaseFlowControlId,
                                TrueCaseActivityNo = condition.TrueCaseActivityNo,
                                ConditionName = condition.ConditionId > 0 ? ModuleConditions.Where(c => c.ModuleConditionId == condition.ConditionId).FirstOrDefault().Name : null,
                                TrueCaseActivityName = condition.TrueCaseActivityNo > 0 ? workflow.WorkflowActivities.Where(c => c.SequenceNumber == condition.TrueCaseActivityNo).FirstOrDefault().ActivityName : null,
                                IsOption = condition.IsOption,
                                workflowConditionOptions = condition.workflowTriggerConditionOptions.Select(option => new WorkflowConditionOptions
                                {
                                    WorkflowOptionId = option.WorkflowOptionId,
                                    ModuleOptionId = option.ModuleOptionId,
                                    WorkflowConditionId = option.TriggerConditionId,
                                    TrueCaseFlowControlId = option.TrueCaseFlowControlId,
                                    TrueCaseActivityNo = option.TrueCaseActivityNo,
                                    TrueCaseActivityName = (option.TrueCaseActivityNo > 0) ? workflow.WorkflowActivities.Where(c => c.SequenceNumber == option.TrueCaseActivityNo).FirstOrDefault().ActivityName : null,
                                    OptionName = ModuleOptions.Where(c => c.ModuleOptionId == option.ModuleOptionId).FirstOrDefault().Name,
                                }).ToList(),
                            });
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

        protected async Task GetRemoteModulesData()
        {
            ModuleResult = new List<MainWorkflowModuleEnumTemp>();
            foreach (MainWorkflowModuleEnum item in Enum.GetValues(typeof(MainWorkflowModuleEnum)))
            {
                MainWorkflowModuleEnumTemp mainWorkflowModuleEnumTemp = new MainWorkflowModuleEnumTemp();
                mainWorkflowModuleEnumTemp = new MainWorkflowModuleEnumTemp { mainWorkflowModuleEnumName = translationState.Translate(item.ToString()), mainWorkflowModuleEnumValue = item };
                ModuleResult.Add(mainWorkflowModuleEnumTemp);
            }
            workflow.WorkflowTrigger = new WorkflowTrigger();
            workflow.WorkflowTrigger.WorkflowConditions = new List<WorkflowCondition>();
            workflow.WorkflowActivities = new List<WorkflowActivity>();
            workflow.ModuleTriggerVM = new ModuleTriggerVM();
            workflow.WorkflowTrigger.ModuleTriggerId = 0;
            workflow.SubModuleId = 0;
            workflow.AttachmentTypeId = null;
            ModuleConditions = new List<ModuleCondition>();
            ModuleActivities = new List<ModuleActivity>();
            ShowWorkflowDetail = false;
        }
        protected async Task PopulateTriggers()
        {
            if (ModuleId > 0 && workflow.SubModuleId > 0)
            {
                ModuleConditions = await workflowService.GetModuleConditions(workflow.WorkflowTrigger.ModuleTriggerId);
                ModuleActivities = await workflowService.GetModuleActvities(workflow.WorkflowTrigger.ModuleTriggerId);
                var response = await workflowService.GetModuleOptionsByTriggerId(workflow.WorkflowTrigger.ModuleTriggerId);
                if (response.IsSuccessStatusCode)
                {
                    ModuleOptions = (List<ModuleConditionOptions>)response.ResultData;
                }
                else
                {
                    await invalidRequestHandlerService.ReturnBadRequestNotification(response);
                }
                StateHasChanged();

                await GetRemoteTriggersData();
                await GetAttachmentTypeofDraft();
                await PopulateSectorTypes();

            }
            else
            {
                workflow.WorkflowTrigger.ModuleTriggerId = 0;
                ShowWorkflowDetail = false;
                workflow.WorkflowTrigger = new WorkflowTrigger();
                workflow.WorkflowTrigger.WorkflowConditions = new List<WorkflowCondition>();
                workflow.WorkflowActivities = new List<WorkflowActivity>();
                ModuleConditions = new List<ModuleCondition>();
                ModuleActivities = new List<ModuleActivity>();
                ModuleOptions = new List<ModuleConditionOptions>();
                StateHasChanged();
            }
        }

        public async Task GetRemoteTriggersData()
        {
            if (workflow.SubModuleId > 0)
            {
                var result = await workflowService.GetTriggerItemsData(workflow.SubModuleId);
                TriggerResultList = (List<ModuleTrigger>)result.ResultData;
            }
        }
        public async Task GetAttachmentTypeofDraft()
        {
            try
            {
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
                    draftTypeResult = (List<AttachmentType>)response.ResultData;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion


        #region Workflow Condition
        public void RowRender(RowRenderEventArgs<WorkflowCondition> args)
        {
            if (args.Data.WorkflowConditionOptionLists.Count == 0)
            {
                args.Attributes.Add("class", "no-withdraw-linked");
            }
        }
        #endregion

        #region Redirect Function
        protected async Task RedirectBack()
        {
            await JsInterop.InvokeVoidAsync("RedirectToPreviousPage");
        }
        private void GoBackDashboard()
        {
            navigationManager.NavigateTo("/dashboard");
        }
        private void GoBackHomeScreen()
        {
            navigationManager.NavigateTo("/index");
        }
        protected Task EditWorkflow()
        {
            navigationManager.NavigateTo("/create-workflow/" + WorkflowId + "/" + false);
            return Task.CompletedTask;
        }
        #endregion
    }
}
