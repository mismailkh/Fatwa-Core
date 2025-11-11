using FATWA_DOMAIN.Models.AdminModels.AutomationMonitoring;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.AutomationMonitoring;
using System.Diagnostics;

namespace FATWA_DOMAIN.Interfaces.AutomationMonitoring
{
    public interface IAutomationMonitoring
    {


        Task<AMSWorkQueueItem> AddItemToQueue(AMSAddWorkQueueItemVM queueItem, int StatusId);
        Task AMSCaseDateExtraction(AMSWorkQueueItem queueItem);
        Task<bool> MarkCompleted(int itemId);
        Task<bool> MarkException(AMSMarkExceptionPayLoadVM exceptionPayLoad);
        Task<AMSWorkQueueItem> GetNextItem(AMSNextItemPayLoadVM exceptionPayLoad);
        Task<AMSWorkQueueItem> GetItemId(string Data, string QueueName);
        Task<AMSWorkQueueItem> GetItemData(int ItemId, string QueueName);
        Task<bool> SetPriority(AMSSetPriorityPayLoadVM setPriorityPayLoad);
        Task<List<AutomationMonitoringQueueItemVM>> GetAllPendingItems();
        Task<bool> TagItem(AMSTagItemPayLoadVM tagItemPayLoad);
        Task<bool> UnTagItem(int ItemId);
        Task<AMSSession> CreateSession(CreateSessionPayLoadVM createSessionPayLoad);
        Task<AMSSessionLog> CreateSessionLog(AMSSessionLogsPayLoadVM createSessionPayLoad);
        Task<bool> MarkSessionRunning(int SessionId);
        Task<bool> MarkSessionTerminated(int SessionId);
        Task<bool> MarkSessionStopped(int SessionId);
        Task<bool> MarkSessionCompleted(int SessionId);
        Task<AMSResources> AddResource(AMSAddResourcePayLoadVM addResourcePayLoad);
        Task<bool> MarkResourceIdle(int ResourceId);
        Task<bool> MarkResourceWorking(int ResourceId);
        Task<bool> MarkResourceLoggedOut(int ResourceId);
        Task<List<AutomationMonitoringProcessVM>> GetProcessesList(AdvanceSearchProcessVM advanceSearch);
        Task<AMSProcesses> GetProcessesById(int Id);
        Task<List<AutomationMonitoringQueueVM>> GetQueueList(int ProcessId, AdvanceSearchQueueVM advanceSearch);
        Task<AMSProcesses> SaveProcess(AMSProcesses process);
        Task<AMSWorkQueue> SaveQueue(AMSWorkQueue queue);
        Task<List<AutomationMonitoringQueueItemVM>> GetQueueItemsListByQueueId(AdvanceSearchQueueVM advanceSearch, int QueueId);
        Task<List<AMSCaseDataExtractionVM>> GetCaseDataExtraction(AdvanceSearchQueueVM advanceSearch);
        Task UpdateProcess(AMSProcesses aMProcess);
        Task UpdateSession(AMSSession aMProcess);
        Task UpdateQueueItem(AMSWorkQueueItem aMWorkQueueItem);
        Task<bool> MarkQueueItemStatus(int ItemId, string Queuename, int StatusCode,string ResourceName, int ResourceId);
        Task<AMSWorkQueueLog> CreateQueueLog(AMSQueueLogPayLoadVM queueLog);
        Task<List<AMSQueueItemStatus>> GetQueueItemStatus();
        Task<List<AMSSessionListVM>> GetSessions(AdvanceSearchSessionVM advanceSearch, int ProcessId);
        Task<List<AMSSessionLogsVM>> GetSessionLogs(int SessionId);
        Task<List<AMSItemLogVM>> GetItemLogs(int ItemId);
        Task<AMSExceptionsDetailsVM> GetExceptionDetails(int ItemId);
        Task<List<AMSSessionStatus>> GetSessionStatus();
        Task<bool> CheckIfAlreadyPushedToQueue(string Data);
        Task<AMSQueueItemStatus> GetQueueItemStatusByQueueId(int QueueId);
        Task<AMSWorkQueue> GetQueueById(int id);
        Task<bool> UpdateQueueItemEndDateTime(int ItemId);
        Task<List<AMSResourcesVM>> GetResourcesByProcessId( int ProcessId);
        Task<List<AMSQueueListVM>> GetQueueDetialsByProcessId( int ProcessId);
        Task<List<AutomationMonitoringQueueVM>> GetQueueListByQueueId(int ProcessId);
        Task<string> GetTagItem(int ItemId);
    }
}
