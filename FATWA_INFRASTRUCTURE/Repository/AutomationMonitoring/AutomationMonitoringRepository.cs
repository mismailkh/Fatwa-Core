using FATWA_DOMAIN.Interfaces.AutomationMonitoring;
using FATWA_DOMAIN.Models.AdminModels.AutomationMonitoring;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.AutomationMonitoring;
using FATWA_INFRASTRUCTURE.Database;
using Google.Api.Gax;
using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using static FATWA_DOMAIN.Enums.AutomationMonitoringInterface.AutomationMonitoringInterfaceEnum;

namespace FATWA_INFRASTRUCTURE.Repository.AutomationMonitoring
{
    public class AutomationMonitoringRepository : IAutomationMonitoring
    {
        private readonly AutoMonInterfaceDbContext _autoMonInterfaceDbContext;
        private readonly IConfiguration _config;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private List<AutomationMonitoringProcessVM> _automationMonitoringProcessVMs;
        private List<AutomationMonitoringQueueVM> _automationMonitoringQueueVMs;
        private List<AutomationMonitoringQueueItemVM> _automationMonitoringQueueItemVMs;
        private List<AMSCaseDataExtractionVM> _caseDataExtractionVMs;
        private List<AMSSessionListVM> sessionListVMs;
        private List<AMSSessionLogsVM> sessionLogsVMs;
        private List<AMSItemLogVM> itemLogVMs;
        private List<AMSExceptionsDetailsVM> ExceptionsDetailsVM;
        private List<AMSResourcesVM> _aMSResourcesVMs;
        private List<AMSQueueListVM> _aMSQueueListVMs;
        public AutomationMonitoringRepository(AutoMonInterfaceDbContext autoMonInterfaceDbContext, IConfiguration configuration, IServiceScopeFactory serviceScopeFactory)
        {
            _autoMonInterfaceDbContext = autoMonInterfaceDbContext;
            _config = configuration;
            _serviceScopeFactory = serviceScopeFactory;
            using var scope = _serviceScopeFactory.CreateScope();
        }
        public async Task<AMSWorkQueueItem> AddItemToQueue(AMSAddWorkQueueItemVM queueItem, int StatusId)
        {

            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _dbContext = scope.ServiceProvider.GetRequiredService<AutoMonInterfaceDbContext>();

                using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var item = new AMSWorkQueueItem
                        {
                            QueueId = queueItem.QueueId,
                            Data = queueItem.Data,
                            ItemName = queueItem.ItemName,
                            StatusId = StatusId,
                            Attempts = 0,
                            CreatedAt = DateTime.Now,
                            CreatedBy = queueItem.ResourceName,
                            ResourceId = queueItem.ResourceId,
                            IsFatwaManual = true,
                        };

                        await _autoMonInterfaceDbContext.WorkQueueItems.AddAsync(item);
                        await _autoMonInterfaceDbContext.SaveChangesAsync();
                        var execptionlog = new AMSWorkQueueLog
                        {
                            ItemId = item.Id,
                            EventTime = DateTime.Now,
                            Description = "item created in  queue"
                        };
                        await _autoMonInterfaceDbContext.WorkQueueLogs.AddAsync(execptionlog);
                        await _autoMonInterfaceDbContext.SaveChangesAsync();
                        return item;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }

        }

        public async Task AMSCaseDateExtraction(AMSWorkQueueItem queueItem)
        {
            try
            {
                queueItem.CreatedAt = DateTime.Now;
                queueItem.StatusId = 1;// for pending
                queueItem.Attempts = 0;
                queueItem.QueueId = 9; //Ad - Hoc queue.
                queueItem.IsFatwaManual = true;
                await _autoMonInterfaceDbContext.WorkQueueItems.AddAsync(queueItem);
                await _autoMonInterfaceDbContext.SaveChangesAsync();
                var execptionlog = new AMSWorkQueueLog
                {
                    ItemId = queueItem.Id,
                    EventTime = DateTime.Now,
                    Description = "Case Data Extraction"
                };
                await _autoMonInterfaceDbContext.WorkQueueLogs.AddAsync(execptionlog);
                await _autoMonInterfaceDbContext.SaveChangesAsync();


            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> MarkCompleted(int ItemId)
        {
            try
            {
                bool IsUpdated = false;
                var queueitem = await _autoMonInterfaceDbContext.WorkQueueItems.Where(x => x.Id == ItemId).FirstOrDefaultAsync();
                if (queueitem != null)
                {
                    var queuestatuscode = await _autoMonInterfaceDbContext.QueueItemStatuses.Where(x => x.QueueId == queueitem.QueueId && x.StatusCode == (int)QueueItemsEnum.Completed).FirstOrDefaultAsync();
                    if (queueitem != null && queuestatuscode != null)
                    {
                        queueitem.StatusId = queuestatuscode.Id;//3 for completed 
                        _autoMonInterfaceDbContext.Update(queueitem);
                        await _autoMonInterfaceDbContext.SaveChangesAsync();
                        IsUpdated = true;
                    }
                }
                return IsUpdated;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> MarkException(AMSMarkExceptionPayLoadVM markExceptionPayload)
        {
            try
            {
                bool IsUpdated = false;
                var queueItem = await _autoMonInterfaceDbContext.WorkQueueItems
                    .Where(x => x.Id == markExceptionPayload.ItemId)
                    .FirstOrDefaultAsync();

                if (queueItem != null)
                {
                    var exception = new AMSExeceptions
                    {
                        ItemId = markExceptionPayload.ItemId,
                        ExceptionType = markExceptionPayload.ExceptionType,
                        ExceptionMessage = markExceptionPayload.ExceptionMessage,
                        ExceptionTraceback = markExceptionPayload.ExceptionTraceback,
                        RecordedDatetime = DateTime.Now,
                        Viewed = false,
                        ViewedBy = null,
                        ViewedOn = null
                    };

                    await _autoMonInterfaceDbContext.Execeptions.AddAsync(exception);
                    await _autoMonInterfaceDbContext.SaveChangesAsync();
                    IsUpdated = true;
                }
                return IsUpdated;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<AMSWorkQueueItem> GetItemId(string Data, string QueueName)
        {
            try
            {
                return await _autoMonInterfaceDbContext.WorkQueueItems
              .Join(_autoMonInterfaceDbContext.WorkQueues, wqi => wqi.QueueId, q => q.Id, (wqi, q) => new { wqi, q })
              .Where(x => x.wqi.Data == Data && x.q.Name == QueueName)
              .Select(x => x.wqi)
              .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        public async Task<AMSWorkQueueItem> GetItemData(int ItemId, string QueueName)
        {
            try
            {
                return await _autoMonInterfaceDbContext.WorkQueueItems
              .Join(_autoMonInterfaceDbContext.WorkQueues, wqi => wqi.QueueId, q => q.Id, (wqi, q) => new { wqi, q })
              .Where(x => x.wqi.Id == ItemId && x.q.Name == QueueName)
              .Select(x => x.wqi)
              .FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> MarkQueueItemStatus(int ItemId, string QueueName, int StatusCode, string ResourceName, int ResourceId)
        {
            try
            {
                bool IsUpdated = false;
                var workqueueStatusId = await _autoMonInterfaceDbContext.WorkQueueItems.Where(x => x.Id == ItemId).FirstOrDefaultAsync();
                var FromStatusId = workqueueStatusId.StatusId;
                var queueItem = await _autoMonInterfaceDbContext.WorkQueueItems
                    .Join(_autoMonInterfaceDbContext.WorkQueues, wqi => wqi.QueueId, q => q.Id, (wqi, q) => new { wqi, q })
                    .Where(x => x.wqi.Id == ItemId && x.q.Name == QueueName)
                    .Select(x => x.wqi)
                    .FirstOrDefaultAsync();

                if (queueItem != null)
                {
                    queueItem.StatusId = GetStatusId(StatusCode, queueItem.QueueId);
                    if (queueItem.StatusId != null && queueItem.StatusId > 0)
                    {
                        queueItem.UpdatedBy = ResourceName;
                        queueItem.ResourceId = ResourceId;
                        queueItem.UpdatedAt = DateTime.Now;
                        _autoMonInterfaceDbContext.WorkQueueItems.Update(queueItem);
                        await _autoMonInterfaceDbContext.SaveChangesAsync();

                        var statusHistory = new AMSItemStatusHistory
                        {
                            ItemId = ItemId,
                            FromStatusId = FromStatusId,
                            ToStatusId = queueItem.StatusId,
                            CreatedBy = ResourceName,
                            CreatedDate = DateTime.Now,

                        };
                        _autoMonInterfaceDbContext.AMSItemStatusHistories.Add(statusHistory);
                        await _autoMonInterfaceDbContext.SaveChangesAsync();
                        IsUpdated = true;
                    }

                }
                return IsUpdated;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public int GetStatusId(int StatusCode, int? QueueId)
        {
            try
            {
                var status = _autoMonInterfaceDbContext.QueueItemStatuses.FirstOrDefault(s => s.StatusCode == StatusCode && s.QueueId == QueueId);
                return status?.Id ?? 0;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        public async Task<AMSWorkQueueItem> GetNextItem(AMSNextItemPayLoadVM nextItemPayLoad)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _dbContext = scope.ServiceProvider.GetRequiredService<AutoMonInterfaceDbContext>();

                using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var queueId = await _dbContext.WorkQueues.Where(wq => wq.Name == nextItemPayLoad.QueueName)
                            .Select(wq => wq.Id)
                            .FirstOrDefaultAsync();


                        if (queueId == 0 || queueId == null)
                        {
                            return null;
                        }
                        var queuestatus = await _autoMonInterfaceDbContext.QueueItemStatuses.Where(x => x.QueueId == queueId && x.StatusCode == (int)QueueItemsEnum.Pending).FirstOrDefaultAsync();
                        if (queuestatus == null)
                        {
                            return null;
                        }
                        var query = _dbContext.WorkQueueItems.Where(wqi => wqi.QueueId == queueId && wqi.StatusId == queuestatus.Id);
                        if (query.Count() == 0 || query == null)
                        {
                            return null;
                        }
                        if (nextItemPayLoad.Priority != 0 && nextItemPayLoad.Priority != null)
                        {
                            query = query.Where(wqi => wqi.Priority == nextItemPayLoad.Priority.Value);
                        }

                        var queueItem = await query.OrderBy(wqi => wqi.CreatedAt).FirstOrDefaultAsync();

                        if (queueItem != null)
                        {
                            queueItem.StatusId = await _autoMonInterfaceDbContext.QueueItemStatuses.Where(x => x.QueueId == queueId && x.StatusCode == (int)QueueItemsEnum.Locked)
                                .Select(x => x.Id).FirstOrDefaultAsync(); ;
                            queueItem.Attempts += 1;
                            queueItem.DateStarted = DateTime.Now;
                            queueItem.UpdatedBy = nextItemPayLoad.ResourceName;
                            queueItem.ResourceId = nextItemPayLoad.ResourceId;
                            _dbContext.WorkQueueItems.Update(queueItem);
                            await _dbContext.SaveChangesAsync();

                            // Log the locking action
                            var workQueueLog = new AMSWorkQueueLog
                            {
                                ItemId = queueItem.Id,
                                Description = "Item locked and retrieved from queue",
                                EventTime = DateTime.Now
                            };

                            _dbContext.WorkQueueLogs.Add(workQueueLog);
                            await _dbContext.SaveChangesAsync();

                            var statusHistory = new AMSItemStatusHistory
                            {
                                ItemId = queueItem.Id,
                                FromStatusId = queuestatus.Id,
                                ToStatusId = queueItem.StatusId,
                                CreatedBy = nextItemPayLoad.ResourceName,
                                CreatedDate = DateTime.Now,

                            };
                            _autoMonInterfaceDbContext.AMSItemStatusHistories.Add(statusHistory);
                            await _autoMonInterfaceDbContext.SaveChangesAsync();
                            await transaction.CommitAsync();

                            return queueItem;
                        }
                        else
                        {
                            await transaction.RollbackAsync();
                            return null;
                        }
                    }
                    catch (Exception ex)
                    {
                        await transaction.RollbackAsync();
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        public async Task<bool> SetPriority(AMSSetPriorityPayLoadVM setPriorityPayLoad)
        {
            try
            {
                bool IsUpdated = false;
                var queueItem = await _autoMonInterfaceDbContext.WorkQueueItems
                    .Where(x => x.Id == setPriorityPayLoad.ItemId)
                    .FirstOrDefaultAsync();

                if (queueItem != null)
                {
                    queueItem.Priority = setPriorityPayLoad.Priority;
                    queueItem.UpdatedAt = DateTime.Now;

                    _autoMonInterfaceDbContext.Update(queueItem);
                    await _autoMonInterfaceDbContext.SaveChangesAsync();
                    IsUpdated = true;
                }
                return IsUpdated;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<AutomationMonitoringQueueItemVM>> GetAllPendingItems()
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _dbContext = scope.ServiceProvider.GetRequiredService<AutoMonInterfaceDbContext>();
                using (_dbContext)
                {
                    try
                    {
                        if (_automationMonitoringQueueItemVMs == null)
                        {
                            string StoreProc = $"exec GetPendingQueueItems";
                            _automationMonitoringQueueItemVMs = await _autoMonInterfaceDbContext.AutomationMonitoringQueueItemVMs.FromSqlRaw(StoreProc).ToListAsync();
                        }
                        return _automationMonitoringQueueItemVMs;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> TagItem(AMSTagItemPayLoadVM tagItemPayLoad)
        {
            try
            {
                bool IsUpdated = false;
                var queueItem = await _autoMonInterfaceDbContext.WorkQueueItems
                    .FirstOrDefaultAsync(x => x.Id == tagItemPayLoad.ItemId);

                if (queueItem != null)
                {
                    queueItem.Tag = tagItemPayLoad.Tag;

                    _autoMonInterfaceDbContext.WorkQueueItems.Update(queueItem);
                    await _autoMonInterfaceDbContext.SaveChangesAsync();
                    IsUpdated = true;
                }
                return IsUpdated;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> UnTagItem(int Item)
        {
            try
            {
                bool IsUpdated = false;
                var queueItem = await _autoMonInterfaceDbContext.WorkQueueItems
                    .FirstOrDefaultAsync(x => x.Id == Item);

                if (queueItem != null)
                {
                    queueItem.Tag = null;

                    _autoMonInterfaceDbContext.WorkQueueItems.Update(queueItem);
                    await _autoMonInterfaceDbContext.SaveChangesAsync();
                    IsUpdated = true;
                }
                return IsUpdated;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<AMSSession> CreateSession(CreateSessionPayLoadVM createSessionPayLoad)
        {
            using (var scope = _serviceScopeFactory.CreateScope())
            {
                var _dbContext = scope.ServiceProvider.GetRequiredService<AutoMonInterfaceDbContext>();

                using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                {
                    try
                    {
                        var newSession = new AMSSession
                        {
                            ProcessId = createSessionPayLoad.ProcessId,
                            ResourceId = createSessionPayLoad.ResourceId,
                            StartTime = DateTime.Now,
                            EndTime = null,
                            StatusId = (int)AMSQueueSessionEnum.Pending
                        };

                        await _autoMonInterfaceDbContext.Sessions.AddAsync(newSession);
                        await _autoMonInterfaceDbContext.SaveChangesAsync();

                        var newSessionLog = new AMSSessionLog
                        {
                            SessionId = newSession.SessionId,
                            Description = "New Session Created ",
                            EventTime = DateTime.Now,
                        };

                        await _autoMonInterfaceDbContext.SessionLogs.AddAsync(newSessionLog);
                        await _autoMonInterfaceDbContext.SaveChangesAsync();

                        return newSession;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }

        }

        public async Task<bool> MarkSessionTerminated(int sessionId)
        {
            try
            {
                bool IsUpdated = false;
                var session = await _autoMonInterfaceDbContext.Sessions
                    .FirstOrDefaultAsync(s => s.SessionId == sessionId);

                if (session != null)
                {
                    session.StatusId = (int)AMSQueueSessionEnum.Terminated;
                    _autoMonInterfaceDbContext.Sessions.Update(session);
                    await _autoMonInterfaceDbContext.SaveChangesAsync();
                    IsUpdated = true;
                }
                return IsUpdated;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> MarkSessionRunning(int sessionId)
        {
            try
            {
                bool IsUpdated = false;
                var session = await _autoMonInterfaceDbContext.Sessions
                    .FirstOrDefaultAsync(s => s.SessionId == sessionId);

                if (session != null)
                {
                    session.StatusId = (int)AMSQueueSessionEnum.Running;
                    session.StartTime = DateTime.Now;
                    _autoMonInterfaceDbContext.Sessions.Update(session);
                    await _autoMonInterfaceDbContext.SaveChangesAsync();
                    IsUpdated = true;
                }
                return IsUpdated;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> MarkSessionStopped(int sessionId)
        {
            try
            {
                bool IsUpdate = false;
                var session = await _autoMonInterfaceDbContext.Sessions
                    .FirstOrDefaultAsync(s => s.SessionId == sessionId);

                if (session != null)
                {
                    session.StatusId = (int)AMSQueueSessionEnum.Stop;
                    session.EndTime = DateTime.Now;
                    _autoMonInterfaceDbContext.Sessions.Update(session);
                    await _autoMonInterfaceDbContext.SaveChangesAsync();
                    IsUpdate = true;
                }
                return IsUpdate;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> MarkSessionCompleted(int sessionId)
        {
            try
            {
                bool IsUpdated = false;
                var session = await _autoMonInterfaceDbContext.Sessions
                    .FirstOrDefaultAsync(s => s.SessionId == sessionId);

                if (session != null)
                {
                    session.StatusId = (int)AMSQueueSessionEnum.Completed; // 3  for Session Status Completed
                    session.EndTime = DateTime.Now;
                    _autoMonInterfaceDbContext.Sessions.Update(session);
                    await _autoMonInterfaceDbContext.SaveChangesAsync();
                    IsUpdated = true;
                }
                return IsUpdated;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<AMSResources> AddResource(AMSAddResourcePayLoadVM addResourcePayLoad)
        {
            try
            {
                var resource = new AMSResources
                {
                    ResourceName = addResourcePayLoad.ResourceName,
                    ResourceType = addResourcePayLoad.ResourceType,
                    ProcessId = addResourcePayLoad.ProcessId,
                    CreatedOn = DateTime.Now,
                    CreatedBy = "System Generated"
                };

                await _autoMonInterfaceDbContext.Resources.AddAsync(resource);
                await _autoMonInterfaceDbContext.SaveChangesAsync();

                return resource;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> MarkResourceIdle(int resourceId)
        {
            try
            {
                bool IsUpdated = false;
                var resource = await _autoMonInterfaceDbContext.Resources.FindAsync(resourceId);
                if (resource != null)
                {

                    // Update the state to Idle
                    resource.StateId = (int)AMSResourceEnum.ResourceIdle;
                    resource.UpdatedOn = DateTime.Now;
                    _autoMonInterfaceDbContext.Resources.Update(resource);
                    await _autoMonInterfaceDbContext.SaveChangesAsync();
                    IsUpdated = true;
                }
                return IsUpdated;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> MarkResourceWorking(int resourceId)
        {
            try
            {
                bool IsUpdated = false;
                var resource = await _autoMonInterfaceDbContext.Resources.FindAsync(resourceId);
                if (resource != null)
                {

                    // Update the state to Working
                    resource.StateId = (int)AMSResourceEnum.ResourceWorking;
                    resource.UpdatedOn = DateTime.Now;
                    _autoMonInterfaceDbContext.Resources.Update(resource);
                    await _autoMonInterfaceDbContext.SaveChangesAsync();
                    IsUpdated = true;
                }
                return IsUpdated;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> MarkResourceLoggedOut(int resourceId)
        {
            try
            {
                bool IsUpdated = false;
                var resource = await _autoMonInterfaceDbContext.Resources.FindAsync(resourceId);
                if (resource != null)
                {

                    // Update the state to LoggedOut
                    resource.StateId = (int)AMSResourceEnum.ResourceLoggedOut;
                    resource.UpdatedOn = DateTime.Now;
                    _autoMonInterfaceDbContext.Resources.Update(resource);
                    await _autoMonInterfaceDbContext.SaveChangesAsync();
                    IsUpdated = true;
                }
                return IsUpdated;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<AutomationMonitoringProcessVM>> GetProcessesList(AdvanceSearchProcessVM advanceSearch)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<AutoMonInterfaceDbContext>();
            using (_dbContext)
            {
                try
                {
                    if (_automationMonitoringProcessVMs == null)
                    {
                        string CreatedfromDate = advanceSearch.CreatedFrom != null ? Convert.ToDateTime(advanceSearch.CreatedFrom).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                        string CreatedtoDate = advanceSearch.CreatedTo != null ? Convert.ToDateTime(advanceSearch.CreatedTo).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                        string StoreProc = $"exec pGetProcessList @IsActive = '{advanceSearch.IsActive}'  , @createdFrom = '{CreatedfromDate}' , @createdTo = '{CreatedtoDate}'";
                        _automationMonitoringProcessVMs = await _autoMonInterfaceDbContext.AutomationMonitoringProcessVMs.FromSqlRaw(StoreProc).ToListAsync();
                    }
                    return _automationMonitoringProcessVMs;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        public async Task<AMSProcesses> GetProcessesById(int Id)
        {
            return await _autoMonInterfaceDbContext.AMProcesses.Where(x => x.Id == Id).FirstOrDefaultAsync();
        }
        public async Task<List<AutomationMonitoringQueueVM>> GetQueueList(int ProcessId, AdvanceSearchQueueVM advanceSearch)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<AutoMonInterfaceDbContext>();
            using (_dbContext)
            {
                try
                {
                    if (_automationMonitoringQueueVMs == null)
                    {
                        string CreatedfromDate = advanceSearch.CreatedFrom != null ? Convert.ToDateTime(advanceSearch.CreatedFrom).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                        string CreatedtoDate = advanceSearch.CreatedTo != null ? Convert.ToDateTime(advanceSearch.CreatedTo).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                        string StoreProc = $"exec pGetQueueList @ProcessId = '{ProcessId}',@StatusId = '{advanceSearch.StatusId}'  , @createdFrom = '{CreatedfromDate}' , @createdTo = '{CreatedtoDate}'";
                        _automationMonitoringQueueVMs = await _autoMonInterfaceDbContext.AutomationMonitoringQueueVMs.FromSqlRaw(StoreProc).ToListAsync();
                    }
                    return _automationMonitoringQueueVMs;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        public async Task<List<AutomationMonitoringQueueVM>> GetQueueListByQueueId(int QueueId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<AutoMonInterfaceDbContext>();
            using (_dbContext)
            {
                try
                {
                    if (_automationMonitoringQueueVMs == null)
                    {
                        string StoreProc = $"exec pGetQueueListByQueueId @QueueId = '{QueueId}'";
                        _automationMonitoringQueueVMs = await _autoMonInterfaceDbContext.AutomationMonitoringQueueVMs.FromSqlRaw(StoreProc).ToListAsync();
                    }
                    return _automationMonitoringQueueVMs;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        public async Task<List<AutomationMonitoringQueueItemVM>> GetQueueItemsListByQueueId(AdvanceSearchQueueVM advanceSearch, int QueueId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<AutoMonInterfaceDbContext>();
            using (_dbContext)
            {
                try
                {
                    if (_automationMonitoringQueueItemVMs == null)
                    {
                        string CreatedfromDate = advanceSearch.CreatedFrom != null ? Convert.ToDateTime(advanceSearch.CreatedFrom).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                        string CreatedtoDate = advanceSearch.CreatedTo != null ? Convert.ToDateTime(advanceSearch.CreatedTo).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                        string StoreProc = $"exec pGetQueueItemsListByQueueId @QueueId='{QueueId}', @StatusId = '{advanceSearch.StatusId}', @createdFrom = '{CreatedfromDate}', @createdTo = '{CreatedtoDate}'";
                        _automationMonitoringQueueItemVMs = await _autoMonInterfaceDbContext.AutomationMonitoringQueueItemVMs.FromSqlRaw(StoreProc).ToListAsync();
                    }
                    return _automationMonitoringQueueItemVMs;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        public async Task<List<AMSCaseDataExtractionVM>> GetCaseDataExtraction(AdvanceSearchQueueVM advanceSearch)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _dbContext = scope.ServiceProvider.GetRequiredService<AutoMonInterfaceDbContext>();
            using (_dbContext)
            {
                try
                {
                    if (_caseDataExtractionVMs == null)
                    {
                        string CreatedfromDate = advanceSearch.CreatedFrom != null ? Convert.ToDateTime(advanceSearch.CreatedFrom).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                        string CreatedtoDate = advanceSearch.CreatedTo != null ? Convert.ToDateTime(advanceSearch.CreatedTo).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                        string StoreProc = $"exec pGetCaseDataExtraction  @StatusId = '{advanceSearch.StatusId}', @createdFrom = '{CreatedfromDate}', @createdTo = '{CreatedtoDate}'";
                        _caseDataExtractionVMs = await _autoMonInterfaceDbContext.AMSCaseDataExtractionVMs.FromSqlRaw(StoreProc).ToListAsync();
                    }
                    return _caseDataExtractionVMs;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        public async Task UpdateProcess(AMSProcesses aMProcess)
        {

            try
            {
                var process = await _autoMonInterfaceDbContext.AMProcesses.Where(x => x.Id == aMProcess.Id).FirstOrDefaultAsync();
                if (process != null)
                {
                    process.IsActive = aMProcess.IsActive;
                    process.Remarks = aMProcess.Remarks;
                    _autoMonInterfaceDbContext.Update(process);
                    await _autoMonInterfaceDbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task UpdateSession(AMSSession aMProcess)
        {

            try
            {
                var process = await _autoMonInterfaceDbContext.Sessions.Where(x => x.SessionId == aMProcess.SessionId).FirstOrDefaultAsync();
                if (process != null)
                {
                    process.StatusId = aMProcess.StatusId;
                    process.Remarks = aMProcess.Remarks;
                    _autoMonInterfaceDbContext.Update(process);
                    await _autoMonInterfaceDbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task UpdateQueueItem(AMSWorkQueueItem aMWorkQueueItem)
        {
            try
            {
                var queueitem = await _autoMonInterfaceDbContext.WorkQueueItems.Where(x => x.Id == aMWorkQueueItem.Id).FirstOrDefaultAsync();
                var queuestatus = await _autoMonInterfaceDbContext.QueueItemStatuses.
                    Where(x => x.QueueId == queueitem.QueueId && x.StatusCode == (int)QueueItemsEnum.ReattemptException).FirstOrDefaultAsync();

                if (queueitem != null && queuestatus != null)
                {
                    queueitem.StatusId = queuestatus.Id;
                    queueitem.UpdatedAt = DateTime.Now;
                    queueitem.UpdatedBy = aMWorkQueueItem.UpdatedBy;
                    queueitem.ExceptionComment = aMWorkQueueItem.ExceptionComment;
                    _autoMonInterfaceDbContext.Update(queueitem);
                    await _autoMonInterfaceDbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<AMSProcesses> SaveProcess(AMSProcesses process)
        {
            try
            {
                if (process != null)
                {
                    await _autoMonInterfaceDbContext.AMProcesses.AddAsync(process);
                    await _autoMonInterfaceDbContext.SaveChangesAsync();
                }
                return process;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<AMSWorkQueue> SaveQueue(AMSWorkQueue queue)
        {
            try
            {
                if (queue != null)
                {
                    await _autoMonInterfaceDbContext.WorkQueues.AddAsync(queue);
                    await _autoMonInterfaceDbContext.SaveChangesAsync();
                }
                return queue;
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<AMSWorkQueueItem> AddItemToQueue(AMSWorkQueueItem queueItem)
        {
            try
            {
                if (queueItem != null)
                {
                    await _autoMonInterfaceDbContext.WorkQueueItems.AddAsync(queueItem);
                    await _autoMonInterfaceDbContext.SaveChangesAsync();
                }
                return queueItem;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<AMSWorkQueueLog> CreateQueueLog(AMSQueueLogPayLoadVM queueLog)
        {
            try
            {
                var queueItem = await _autoMonInterfaceDbContext.WorkQueueItems.Where(x => x.Id == queueLog.ItemId).FirstOrDefaultAsync();
                if (queueItem != null)
                {
                    var workqueuelog = new AMSWorkQueueLog
                    {
                        ItemId = queueLog.ItemId,
                        Description = queueLog.Description,
                        EventTime = DateTime.Now,
                        LogType = queueLog.LogType,
                    };
                    await _autoMonInterfaceDbContext.WorkQueueLogs.AddAsync(workqueuelog);
                    await _autoMonInterfaceDbContext.SaveChangesAsync();
                    return workqueuelog;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //<History Author = 'Hassan Abbas' Date='2022-09-29' Version="1.0" Branch="master"> Get Case Request Statuses</History>
        public async Task<List<AMSQueueItemStatus>> GetQueueItemStatus()
        {
            try
            {
                return await _autoMonInterfaceDbContext.QueueItemStatuses
                 .OrderBy(u => u.Id)
                 .Select(u => new AMSQueueItemStatus
                 {
                     Id = u.Id,
                     Name_En = u.Name_En,
                     Name_Ar = u.Name_Ar
                 })
                 .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<AMSSessionListVM>> GetSessions(AdvanceSearchSessionVM advanceSearch, int ProcessId)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _dbContext = scope.ServiceProvider.GetRequiredService<AutoMonInterfaceDbContext>();
                using (_dbContext)
                {
                    try
                    {
                        if (sessionListVMs == null)
                        {
                            string CreatedfromDate = advanceSearch.CreatedFrom != null ? Convert.ToDateTime(advanceSearch.CreatedFrom).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                            string CreatedtoDate = advanceSearch.CreatedTo != null ? Convert.ToDateTime(advanceSearch.CreatedTo).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                            string StoreProc = $"exec pGetSessionsList @ProcessId = '{ProcessId}',@StatusId = '{advanceSearch.StatusId}'  , @createdFrom = '{CreatedfromDate}' , @createdTo = '{CreatedtoDate}'";
                            sessionListVMs = await _autoMonInterfaceDbContext.SessionListVMs.FromSqlRaw(StoreProc).ToListAsync();
                        }
                        return sessionListVMs;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<AMSSessionLogsVM>> GetSessionLogs(int SessionId)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _dbContext = scope.ServiceProvider.GetRequiredService<AutoMonInterfaceDbContext>();
                using (_dbContext)
                {
                    try
                    {
                        if (sessionLogsVMs == null)
                        {
                            string StoreProc = $"exec GetSessionLogsBySessionId @SessionId='{SessionId}'";
                            sessionLogsVMs = await _autoMonInterfaceDbContext.SessionLogsVMs.FromSqlRaw(StoreProc).ToListAsync();
                        }
                        return sessionLogsVMs;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<AMSItemLogVM>> GetItemLogs(int ItemId)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _dbContext = scope.ServiceProvider.GetRequiredService<AutoMonInterfaceDbContext>();
                using (_dbContext)
                {
                    try
                    {
                        if (itemLogVMs == null)
                        {
                            string StoreProc = $"exec GetItemLogsList @ItemId='{ItemId}'";
                            itemLogVMs = await _autoMonInterfaceDbContext.ItemLogVMs.FromSqlRaw(StoreProc).ToListAsync();
                        }
                        return itemLogVMs;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<AMSExceptionsDetailsVM> GetExceptionDetails(int ItemId)
        {

            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _dbContext = scope.ServiceProvider.GetRequiredService<AutoMonInterfaceDbContext>();
                using (_dbContext)
                {
                    try
                    {
                        if (ExceptionsDetailsVM == null)
                        {
                            string StoreProc = $"exec pGetExceptionDetails @ItemId='{ItemId}'";
                            ExceptionsDetailsVM = await _autoMonInterfaceDbContext.ExceptionsDetailsVMs.FromSqlRaw(StoreProc).ToListAsync();
                        }
                        var ExceptionsDetails = ExceptionsDetailsVM.FirstOrDefault();
                        return ExceptionsDetails;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<AMSSessionStatus>> GetSessionStatus()
        {
            try
            {
                return await _autoMonInterfaceDbContext.SessionStatuses
                 .OrderBy(u => u.Id)
                 .Select(u => new AMSSessionStatus
                 {
                     Id = u.Id,
                     Name_En = u.Name_En,
                     Name_Ar = u.Name_Ar
                 })
                 .ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> CheckIfAlreadyPushedToQueue(string Data)
        {
            return await _autoMonInterfaceDbContext.WorkQueueItems.AnyAsync(x => x.Data == Data);
        }

        public async Task<AMSQueueItemStatus> GetQueueItemStatusByQueueId(int QueueId)
        {
            return await _autoMonInterfaceDbContext.QueueItemStatuses.Where(x => x.QueueId == QueueId).FirstOrDefaultAsync();
        }
        public async Task<AMSWorkQueue> GetQueueById(int Id)
        {
            return await _autoMonInterfaceDbContext.WorkQueues.Where(x => x.Id == Id).FirstOrDefaultAsync();
        }

        public async Task<AMSSessionLog> CreateSessionLog(AMSSessionLogsPayLoadVM aMSSessionLogsPayLoadVM)
        {
            try
            {
                var session = await _autoMonInterfaceDbContext.Sessions.Where(x => x.SessionId == aMSSessionLogsPayLoadVM.SessionId).FirstOrDefaultAsync();
                if (session != null)
                {
                    var newSessionLog = new AMSSessionLog
                    {
                        SessionId = (int)aMSSessionLogsPayLoadVM.SessionId,
                        Description = aMSSessionLogsPayLoadVM.Description,
                        ItemId = aMSSessionLogsPayLoadVM.ItemId,
                        EventTime = DateTime.Now,
                    };

                    await _autoMonInterfaceDbContext.SessionLogs.AddAsync(newSessionLog);
                    await _autoMonInterfaceDbContext.SaveChangesAsync();
                    return newSessionLog;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> UpdateQueueItemEndDateTime(int ItemId)
        {
            try
            {
                bool IsUpdated = false;
                var queueitem = await _autoMonInterfaceDbContext.WorkQueueItems.Where(x => x.Id == ItemId).FirstOrDefaultAsync();
                if (queueitem != null)
                {

                    queueitem.EndDateTime = DateTime.Now;
                    TimeSpan duration = (TimeSpan)(queueitem.EndDateTime - queueitem.DateStarted);
                    queueitem.CompletedDuration = duration;
                    _autoMonInterfaceDbContext.Update(queueitem);
                    await _autoMonInterfaceDbContext.SaveChangesAsync();
                    IsUpdated = true;
                }
                return IsUpdated;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<AMSResourcesVM>> GetResourcesByProcessId(int ProcessId)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _dbContext = scope.ServiceProvider.GetRequiredService<AutoMonInterfaceDbContext>();
                using (_dbContext)
                {
                    try
                    {
                        if (_aMSResourcesVMs == null)
                        {
                            string StoreProc = $"exec pGetResourcesByProcessesId @ProcessId = '{ProcessId}'";
                            _aMSResourcesVMs = await _autoMonInterfaceDbContext.AMSResourcesVMs.FromSqlRaw(StoreProc).ToListAsync();
                        }
                        return _aMSResourcesVMs;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<AMSQueueListVM>> GetQueueDetialsByProcessId(int ProcessId)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _dbContext = scope.ServiceProvider.GetRequiredService<AutoMonInterfaceDbContext>();
                using (_dbContext)
                {
                    try
                    {
                        var queueList = await _dbContext.WorkQueues
                            .Where(wq => wq.ProcessId == ProcessId)
                            .Select(wq => new AMSQueueListVM
                            {
                                QueueId = wq.Id,
                                QueueName = wq.Name,

                            })
                            .ToListAsync();

                        return queueList;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> GetTagItem(int ItemId)
        {
            try
            {
                var result = await _autoMonInterfaceDbContext.WorkQueueItems
                            .Where(x => x.Id == ItemId)
                            .Select(x => x.Tag)
                            .FirstOrDefaultAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }


}
