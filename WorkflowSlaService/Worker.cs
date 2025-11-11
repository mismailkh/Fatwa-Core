using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.WorkflowModels;
using FATWA_GENERAL.Helper;
using FATWA_INFRASTRUCTURE.Database;
using FATWA_WEB.Services;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using static FATWA_DOMAIN.Enums.WorkflowEnums;
using static FATWA_DOMAIN.Enums.WorkflowModuleAndActivitiesEnum;
using static FATWA_DOMAIN.Enums.WorkflowParameterEnums;
using static FATWA_GENERAL.Helper.Enum;
using static FATWA_GENERAL.Helper.Response;

namespace WorkflowSlaService
{
    //<History Author = 'Hassan Abbas' Date='2022-05-30' Version="1.0" Branch="master"> Background Service for executing Actions on Expired Workflow Instances</History>
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly IAuditLog _IAuditLog;

        System.Timers.Timer _timer;
        DateTime _scheduleTime;
        public Worker(ILogger<Worker> logger, IServiceScopeFactory serviceScopeFactory)
        {
            _logger = logger;
            _serviceScopeFactory = serviceScopeFactory;

            using var scope = _serviceScopeFactory.CreateScope();

            _IAuditLog = scope.ServiceProvider.GetRequiredService<IAuditLog>();

            _timer = new System.Timers.Timer();
            _scheduleTime = DateTime.Today.AddDays(1).AddHours(2); // Schedule to run everyday at 2:00 a.m.
            //_scheduleTime = DateTime.Today.AddDays(0).AddHours(12).AddMinutes(56); // Schedule to run everyday at 2:00 a.m.
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();

                var _dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                    await Task.Delay(1000, stoppingToken);
                    if (!_timer.Enabled)
                    {
                        _timer.Enabled = true;
                        _timer.Interval = _scheduleTime.Subtract(DateTime.Now).TotalSeconds * 1000;
                        _timer.Elapsed += new System.Timers.ElapsedEventHandler(Timer_Elapsed);

                        ProcessLog pobj = new ProcessLog();
                        pobj.ProcessLogId = Guid.NewGuid();
                        pobj.StartDate = DateTime.Now;
                        pobj.EndDate = DateTime.Now;
                        pobj.Process = "Workflow Sla Timer";
                        pobj.Task = "Timer Enabled";
                        pobj.Description = "Scheduled Timer enabled to run the service at specified time";
                        pobj.Message = "Scheduled Timer enabled to run the service at specified time";
                        pobj.Computer = Environment.MachineName.ToString();
                        pobj.ProcessLogEventId = (int)ProcessLogEnum.Processed;
                        pobj.ProcessLogTypeId = Guid.NewGuid();
                        pobj.IPDetails = "";
                        pobj.ApplicationID = (int)PortalEnum.FatwaPortal;
                        pobj.ModuleID = (int)WorkflowModuleEnum.LMSLiterature;
                        pobj.UserName = "";
                        pobj.TerminalID = "C0-B6-F9-1A-D8-89/ hostname/MAC address";
                        pobj.ChannelName = "Web";

                        _dbContext.ProcessLogs.Add(pobj);
                        await _dbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-05-30' Version="1.0" Branch="master"> Execute function when timer elapses</History>
        protected void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            CheckSLAs();

            //If tick for the first time, reset next run to every 24 hours
            if (_timer.Interval != 24 * 60 * 60 * 1000)
            {
                _timer.Interval = 24 * 60 * 60 * 1000;
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-05-30' Version="1.0" Branch="master"> Check expired SLAs and perform specified Actions for it</History>
        protected async Task CheckSLAs()
        {
            using var scope = _serviceScopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            try
            {

                //get list of Expired Instances which has not been executed yet
                List<WorkflowInstance> expiredInstances = await dbContext.WorkflowInstance.Where(s => s.ApplySla == true && s.StatusId == (int)WorkflowInstanceStatusEnum.InProgress && s.SlaEndDate < DateTime.Now.Date).ToListAsync();

                foreach (var instance in expiredInstances)
                {
                    using var scope2 = _serviceScopeFactory.CreateScope();

                    var _dbContext = scope2.ServiceProvider.GetRequiredService<DatabaseContext>();
                    using (_dbContext)
                    {
                        using (var transaction = _dbContext.Database.BeginTransaction())
                        {
                            try
                            {
                                if (instance.IsSlaExecuted == false)
                                {
                                    SLA sla = await _dbContext.SLA.Where(s => s.WorkflowActivityId == instance.WorkflowActivityId).FirstOrDefaultAsync();
                                    string StoredProc = $"exec pWorkflowSlaActionParametersList @workflowSLAId = '{sla.WorkflowSLAId}'";
                                    List<WorkflowActivityParametersVM> parames = await _dbContext.WorkflowActivityParametersVM.FromSqlRaw(StoredProc).ToListAsync();

                                    //check sla expiry action
                                    if (sla.ActionId == SlaAction.DoNothing)
                                    {
                                        //update workflow instance expired
                                        instance.IsSlaExecuted = true;
                                        instance.StatusId = (int)WorkflowInstanceStatusEnum.Expired;
                                        _dbContext.Entry(instance).State = EntityState.Modified;
                                        await _dbContext.SaveChangesAsync();
                                    }
                                    else if (sla.ActionId == SlaAction.AssignTo)
                                    {
                                        //update workflow instance expired
                                        instance.IsSlaExecuted = true;
                                        _dbContext.Entry(instance).State = EntityState.Modified;
                                        await _dbContext.SaveChangesAsync();

                                        //get triggers from workflow instance
                                        WorkflowTrigger workflowTrigger = await _dbContext.WorkflowTrigger.Where(t => t.WorkflowId == instance.WorkflowId).FirstOrDefaultAsync();
                                        ModuleTrigger moduleTrigger = await _dbContext.ModuleTrigger.FindAsync(workflowTrigger.ModuleTriggerId);

                                        //check instance module and update instance
                                        if (moduleTrigger.ModuleId == (int)WorkflowModuleEnum.LDSDocument)
                                        {
                                            LdsDocument document = await _dbContext.LdsDocument.FindAsync(instance.ReferenceId);

                                            foreach (var par in parames)
                                            {
                                                //check parameter type
                                                if (WorkflowParams.Sla_UserRole == (WorkflowParams)System.Enum.Parse(typeof(WorkflowParams), par.PKey))
                                                    document.RoleId = par.Value;
                                            }
                                            document.UserId = "";
                                            _dbContext.Entry(document).State = EntityState.Modified;
                                            await _dbContext.SaveChangesAsync();
                                        }
                                    }
                                    else if (sla.ActionId == SlaAction.SendEmail)
                                    {
                                        //send Email
                                        Type type = typeof(WorkflowImplementationService);
                                        MethodInfo methodInfo = type.GetMethod("SendEmail");
                                        object[] methodParams = new object[parames.Count()];
                                        for (int i = 0; i < parames.Count(); i++)
                                        {
                                            methodParams[i] = parames.ToArray()[i];
                                        }
                                        var response3 = (Task<WorkflowActivityResponse>)methodInfo.Invoke(methodInfo, methodParams);

                                        //update workflow instance expired
                                        instance.IsSlaExecuted = true;
                                        instance.StatusId = (int)WorkflowInstanceStatusEnum.Expired;
                                        _dbContext.Entry(instance).State = EntityState.Modified;
                                        await _dbContext.SaveChangesAsync();
                                    }

                                    //get workflow from current instance
                                    Workflow workflow = await _dbContext.Workflow.FindAsync(instance.WorkflowId);
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

                                    ProcessLog pobj = new ProcessLog();
                                    pobj.ProcessLogId = Guid.NewGuid();
                                    pobj.StartDate = DateTime.Now;
                                    pobj.EndDate = DateTime.Now;
                                    pobj.Process = "Workflow SLA Execution";
                                    pobj.Task = sla.ActionId.ToString();
                                    pobj.Description = "SLA executed Successfully";
                                    pobj.Message = "SLA executed Successfully";
                                    pobj.Computer = Environment.MachineName.ToString();
                                    pobj.ProcessLogEventId = (int)ProcessLogEnum.Processed;
                                    pobj.ProcessLogTypeId = Guid.NewGuid();
                                    pobj.UserName = "";
                                    pobj.TerminalID = "C0-B6-F9-1A-D8-89/ hostname/MAC address";
                                    pobj.IPDetails = "";
                                    pobj.ChannelName = "Web";
                                    pobj.ApplicationID = (int)PortalEnum.FatwaPortal;
                                    pobj.ModuleID = (int)WorkflowModuleEnum.LMSLiterature;

                                    _dbContext.ProcessLogs.Add(pobj);
                                    await _dbContext.SaveChangesAsync();
                                    transaction.Commit();
                                }
                                else
                                {
                                    if (DateOnly.FromDateTime(instance.SlaEndDate.AddDays(4)) < DateOnly.FromDateTime(DateTime.Now))
                                    {
                                        //update workflow instance expired
                                        instance.StatusId = (int)WorkflowInstanceStatusEnum.Expired;
                                        _dbContext.Entry(instance).State = EntityState.Modified;
                                        await _dbContext.SaveChangesAsync();

                                        //get workflow from current instance
                                        Workflow workflow = await _dbContext.Workflow.FindAsync(instance.WorkflowId);
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

                                        ProcessLog pobj = new ProcessLog();
                                        pobj.ProcessLogId = Guid.NewGuid();
                                        pobj.StartDate = DateTime.Now;
                                        pobj.EndDate = DateTime.Now;
                                        pobj.Process = "Workflow SLA Execution";
                                        pobj.Task = "Default Sla Expiry";
                                        pobj.Description = "Default SLA executed and instance expired successfully";
                                        pobj.Message = "Default SLA executed and instance expired successfully";
                                        pobj.Computer = Environment.MachineName.ToString();
                                        pobj.ProcessLogEventId = (int)ProcessLogEnum.Processed;
                                        pobj.ProcessLogTypeId = Guid.NewGuid();
                                        pobj.UserName = "";
                                        pobj.TerminalID = "C0-B6-F9-1A-D8-89/ hostname/MAC address";
                                        pobj.IPDetails = "";
                                        pobj.ChannelName = "Web";
                                        pobj.ApplicationID = (int)PortalEnum.FatwaPortal;
                                        pobj.ModuleID = (int)WorkflowModuleEnum.LMSLiterature;

                                        _dbContext.ProcessLogs.Add(pobj);
                                        await _dbContext.SaveChangesAsync();
                                        transaction.Commit();
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();

                                ErrorLog eobj = new ErrorLog();
                                eobj.ErrorLogId = Guid.NewGuid();
                                eobj.ErrorLogTypeId = Guid.NewGuid();
                                eobj.ErrorLogEventId = (int)ErrorLogEnum.Error;
                                eobj.Subject = "Workflow Sla Service failed for an instance";
                                eobj.Message = "Workflow Sla Service failed for an instance";
                                eobj.Body = ex.Message;
                                eobj.LogDate = DateTime.Now;
                                eobj.Category = "Workflow Sla Service";
                                eobj.Source = ex.Source;
                                eobj.Type = ex.GetType().Name;
                                eobj.Computer = Environment.MachineName.ToString();
                                eobj.TerminalID = "C0-B6-F9-1A-D8-89/ hostname/MAC address";
                                eobj.ChannelName = "Web";
                                eobj.IPDetails = "";
                                eobj.UserName = "";
                                eobj.ApplicationID = (int)PortalEnum.FatwaPortal;
                                eobj.ModuleID = (int)WorkflowModuleEnum.LMSLiterature;

                                _dbContext.ErrorLogs.Add(eobj);
                                await _dbContext.SaveChangesAsync();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorLog eobj = new ErrorLog();
                eobj.ErrorLogId = Guid.NewGuid();
                eobj.ErrorLogTypeId = Guid.NewGuid();
                eobj.ErrorLogEventId = (int)ErrorLogEnum.Error;
                eobj.Subject = "Workflow SLA Service Failed to get list of expired instances";
                eobj.Body = ex.Message;
                eobj.LogDate = DateTime.Now;
                eobj.Category = "Workflow Sla Service";
                eobj.Source = ex.Source;
                eobj.Type = ex.GetType().Name;
                eobj.Computer = Environment.MachineName.ToString();
                eobj.Message = "Workflow SLA Service Failed";
                eobj.UserName = "";
                eobj.TerminalID = "C0-B6-F9-1A-D8-89/ hostname/MAC address";
                eobj.IPDetails = "";
                eobj.ChannelName = "Web";
                eobj.ApplicationID = (int)PortalEnum.FatwaPortal;
                eobj.ModuleID = (int)WorkflowModuleEnum.LMSLiterature;

                dbContext.ErrorLogs.Add(eobj);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}