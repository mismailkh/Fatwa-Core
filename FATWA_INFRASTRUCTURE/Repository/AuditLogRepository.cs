using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_INFRASTRUCTURE.Repository
{
    //<History Author = 'Hassan Abbas' Date='2022-09-02' Version="1.0" Branch="master"> Repo for Performing DB operations related to Process Logs, Error Logs</History>
    public class AuditLogRepository : IAuditLog
    {
        private readonly DatabaseContext _dbContext;

        private List<ProcessLog> _ProcessLogs;
        private ProcessLog _GetProcessLogDetail;
        private ErrorLog _ErrorLogsDetail;
        private List<ErrorLog> _ErrorLogs;
        private List<ErrorLogVM> _ErrorLogsVM;
        private List<ProcessLogVM> _ProcessLogsVM;
        
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public AuditLogRepository(DatabaseContext dbContext, IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _dbContext = dbContext;
        }

        #region Process Logs
        public async Task<ProcessLog> GetProcessLogDetailById(Guid ProcessLogId)
        {
            if (_GetProcessLogDetail == null)
            {
                _GetProcessLogDetail = await _dbContext.ProcessLogs.OrderByDescending(u => u.StartDate).Where(x => x.ProcessLogId == ProcessLogId).FirstOrDefaultAsync();
            }

            return _GetProcessLogDetail;
        }
       
        public async Task<List<ProcessLog>> GetProcessLogs()
        {
            if (_ProcessLogs == null)
            {
                _ProcessLogs = await _dbContext.ProcessLogs.OrderByDescending(u => u.StartDate).ToListAsync();
            }

            return _ProcessLogs;
        }

        public List<ProcessLog> GetProcessLogsSync()
        {
            if (_ProcessLogs == null)
            {
                _ProcessLogs = _dbContext.ProcessLogs.ToList();
            }

            return _ProcessLogs;
        }

        public void CreateProcessLog(ProcessLog processLog)
        {
            try
            {
                if (!String.IsNullOrEmpty(processLog.Token))
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(processLog.Token);
                    var tokenS = jsonToken as JwtSecurityToken;
                    processLog.UserName = tokenS.Subject;
                }
                else
                {
                    processLog.UserName = "Anonymous";
                }

                processLog.ProcessLogId = Guid.NewGuid();
                processLog.StartDate = DateTime.Now;
                processLog.EndDate = DateTime.Now;
                processLog.Computer = Environment.MachineName.ToString();
                processLog.ProcessLogTypeId = Guid.NewGuid();
                processLog.TerminalID = "C0-B6-F9-1A-D8-89/ hostname/MAC address";
                processLog.ChannelName = "Web";

                using var scope = _serviceScopeFactory.CreateScope();
                var databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                databaseContext.ProcessLogs.Add(processLog);
                databaseContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void UpdateProcessLog(ProcessLog ProcessLog)
        {
            var target = _ProcessLogs.FirstOrDefault(p => p.ProcessLogId == ProcessLog.ProcessLogId);
            if (target != null)
            {
                target.StartDate = ProcessLog.StartDate;
                target.EndDate = ProcessLog.EndDate;
                target.Description = ProcessLog.Description;
                target.Computer = ProcessLog.Computer;
            }
            _dbContext.Entry(ProcessLog).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public void DeleteProcessLog(ProcessLog ProcessLog)
        {
            _dbContext.ProcessLogs.Remove(ProcessLog);
            _dbContext.SaveChanges();
        }

        #endregion

        #region Error Logs


        public async Task<List<ErrorLogVM>> GetErrorLogAdvanceSearch(ErrorLogAdvanceSearchVM errorLogAdvanceSearchVM)
        {
            try
            {
                if (_ErrorLogsVM == null)
                {
                    string fromDate = errorLogAdvanceSearchVM.FromDate != null ? Convert.ToDateTime(errorLogAdvanceSearchVM.FromDate).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    string toDate = errorLogAdvanceSearchVM.ToDate != null ? Convert.ToDateTime(errorLogAdvanceSearchVM.ToDate).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    string StoredProc = $"exec pErrorLogSelAdvanceSearch @Subject='{errorLogAdvanceSearchVM.Subject}' ,@From ='{fromDate}',@To ='{toDate}', @Category='{errorLogAdvanceSearchVM.Category}', @ComputerName='{errorLogAdvanceSearchVM.ComputerName}',@UserName ='{errorLogAdvanceSearchVM.UserName}',@PageNumber ='{errorLogAdvanceSearchVM.PageNumber}',@PageSize ='{errorLogAdvanceSearchVM.PageSize}'";
                    _ErrorLogsVM = await _dbContext.ErrorLogVM.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _ErrorLogsVM;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<ProcessLogVM>> GetProcessLogAdvanceSearch(ProcessLogAdvanceSearchVM processLogAdvanceSearchVM)
        {
            try
            {


                if (_ProcessLogsVM == null)
                {
                    string fromDate = processLogAdvanceSearchVM.StartDate != null ? Convert.ToDateTime(processLogAdvanceSearchVM.StartDate).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    string toDate = processLogAdvanceSearchVM.EndDate != null ? Convert.ToDateTime(processLogAdvanceSearchVM.EndDate).ToString("yyyy/MM/dd HH:mm").ToString() : null;


                    string StoredProc = $"exec pProcessLogSelAdvanceSearch @Task='{processLogAdvanceSearchVM.Task}' ,@Process='{processLogAdvanceSearchVM.Process}' ,@StartDate ='{fromDate}',@EndDate ='{toDate}', @ComputerName='{processLogAdvanceSearchVM.ComputerName}',@UserName ='{processLogAdvanceSearchVM.UserName}', @PageNumber='{processLogAdvanceSearchVM.PageNumber}', @PageSize='{processLogAdvanceSearchVM.PageSize}'";
                    _ProcessLogsVM = await _dbContext.ProcessLogVM.FromSqlRaw(StoredProc).ToListAsync();
                }

                return _ProcessLogsVM;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }


        public async Task<List<ErrorLogVM>> GetErrorLogThroughProc()
        {
            try
            {


                if (_ErrorLogsVM == null)
                {
                    string StoredProc = "exec pErrorLogSelAdvanceSearch";
                    _ErrorLogsVM = await _dbContext.ErrorLogVM.FromSqlRaw(StoredProc).ToListAsync();
                }

                return _ErrorLogsVM;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public async Task<List<ProcessLogVM>> GetProcessLogThroughProc()
        {
            try
            {


                if (_ProcessLogsVM == null)
                {
                    string StoredProc = "exec pProcessLogSelAdvanceSearch";
                    _ProcessLogsVM = await _dbContext.ProcessLogVM.FromSqlRaw(StoredProc).ToListAsync();
                }

                return _ProcessLogsVM;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<ErrorLog>> GetErrorLogs()
        {
            if (_ErrorLogs == null)
            {
                _ErrorLogs = await _dbContext.ErrorLogs.OrderByDescending(u => u.LogDate).ToListAsync();
            }

            return _ErrorLogs;
        }
        public async Task<ErrorLog> GetErrorLogDetailById(Guid ErrorLogId)
        {
            if (_ErrorLogsDetail == null)
            {
                _ErrorLogsDetail = await _dbContext.ErrorLogs.OrderByDescending(u => u.LogDate).Where(x => x.ErrorLogId == ErrorLogId).FirstOrDefaultAsync();
            }

            return _ErrorLogsDetail;
        }

        public List<ErrorLog> GetErrorLogsSync()
        {
            if (_ErrorLogs == null)
            {
                _ErrorLogs = _dbContext.ErrorLogs.ToList();
            }

            return _ErrorLogs;
        }

        public void CreateErrorLog(ErrorLog errorLog)
        {
            try
            {
                if(!String.IsNullOrEmpty(errorLog.Token))
                {
                    var handler = new JwtSecurityTokenHandler();
                    var jsonToken = handler.ReadToken(errorLog.Token);
                    var tokenS = jsonToken as JwtSecurityToken;
                    errorLog.UserName = tokenS.Subject;
                }
                else
                {
                    errorLog.UserName = "Anonymous";
                }

                errorLog.ErrorLogId = Guid.NewGuid();
                errorLog.ErrorLogTypeId = Guid.NewGuid();
                errorLog.LogDate = DateTime.Now;
                errorLog.Computer = Environment.MachineName.ToString();
                errorLog.TerminalID = "C0-B6-F9-1A-D8-89/ hostname/MAC address";
                errorLog.ChannelName = "Web";

                using var scope = _serviceScopeFactory.CreateScope();
                var databaseContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                databaseContext.ErrorLogs.Add(errorLog);
                databaseContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void UpdateErrorLog(ErrorLog ErrorLog)
        {
            var target = _ErrorLogs.FirstOrDefault(p => p.ErrorLogId == ErrorLog.ErrorLogId);
            if (target != null)
            {
                target.Subject = ErrorLog.Subject;
                target.Body = ErrorLog.Body;
                target.Type = ErrorLog.Type;
                target.Category = ErrorLog.Category;
                target.Computer = ErrorLog.Computer;
            }
            _dbContext.Entry(ErrorLog).State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public void DeleteErrorLog(ErrorLog ErrorLog)
        {
            _dbContext.ErrorLogs.Remove(ErrorLog);
            _dbContext.SaveChanges();
        }

        #endregion
    }
}
