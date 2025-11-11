using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;

namespace FATWA_GENERAL.Helper
{
    public class Logging
    {
        private readonly IErrorLogs _errorLogs;
        private readonly IProcessLogs _processLogs;

        public Logging(IErrorLogs errorLogs, IProcessLogs processLogs)
        {
            _errorLogs = errorLogs;
            _processLogs = processLogs;
        }

        #region Add Error Log
        public void AddErrorLog(int errorLogEventId, string subject, string body, string category, string source,
              string type, string computer, string logData)
        {

            ErrorLog eobj = new ErrorLog();
            eobj.ErrorLogId = Guid.NewGuid();
            eobj.ErrorLogTypeId = Guid.NewGuid();
            eobj.ErrorLogEventId = errorLogEventId;
            eobj.Subject = subject;
            eobj.Body = body;
            eobj.LogDate = DateTime.Now;
            eobj.Category = category;
            eobj.Source = source;
            eobj.Type = type;
            eobj.Computer = computer;


            _errorLogs.CreateErrorLog(eobj);
        }
        #endregion

        #region Add Process Log
        public void AddProcessLog(string process, string task, string description, string computer,
            string logData, int processLogEventId, int processLogTypeId)
        {

            ProcessLog pobj = new ProcessLog();
            pobj.ProcessLogId = Guid.NewGuid();
            pobj.StartDate = DateTime.Now;
            pobj.EndDate = DateTime.Now;

            pobj.Process = process;
            pobj.Task = task;
            pobj.Description = description;
            pobj.Computer = computer;
            pobj.ProcessLogEventId = processLogEventId;
            pobj.ProcessLogTypeId = Guid.NewGuid();

            _processLogs.CreateProcessLog(pobj);
        }

        public async Task AddProcessLogAsync(string process, string task, string description, string computer,
           string logData, int processLogEventId, int processLogTypeId)
        {

            ProcessLog pobj = new ProcessLog();
            pobj.ProcessLogId = Guid.NewGuid();
            pobj.StartDate = DateTime.Now;
            pobj.EndDate = DateTime.Now;

            pobj.Process = process;
            pobj.Task = task;
            pobj.Description = description;
            pobj.Computer = computer;
            pobj.ProcessLogEventId = processLogEventId;
            pobj.ProcessLogTypeId = Guid.NewGuid();
            await _processLogs.CreateProcessLog(pobj);
        }
        #endregion
    }
}
