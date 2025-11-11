using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;

namespace FATWA_DOMAIN.Interfaces
{
    //<History Author = 'Hassan Abbas' Date='2022-09-02' Version="1.0" Branch="master"> Interface for Process Logs, Error Logs processes</History>
    public interface IAuditLog
    {
        #region Process Logs

        Task<List<ProcessLog>> GetProcessLogs();
        List<ProcessLog> GetProcessLogsSync();
        void CreateProcessLog(ProcessLog processlog);
        void UpdateProcessLog(ProcessLog processlog);
        void DeleteProcessLog(ProcessLog processlog);
        Task<ProcessLog> GetProcessLogDetailById(Guid ProcessLogId);
        Task<ErrorLog> GetErrorLogDetailById(Guid ErrorLogId);
        
        #endregion

        #region Error Logs
        Task<List<ErrorLogVM>> GetErrorLogThroughProc();
        Task<List<ProcessLogVM>> GetProcessLogThroughProc();
        Task<List<ErrorLogVM>> GetErrorLogAdvanceSearch(ErrorLogAdvanceSearchVM errorLogAdvanceSearchVM);
        Task<List<ProcessLogVM>> GetProcessLogAdvanceSearch(ProcessLogAdvanceSearchVM processLogAdvanceSearchVM);
        Task<List<ErrorLog>> GetErrorLogs();
        List<ErrorLog> GetErrorLogsSync();
        void CreateErrorLog(ErrorLog errorlog);
        void UpdateErrorLog(ErrorLog errorlog);
        void DeleteErrorLog(ErrorLog errorlog);

        #endregion
    }
}
