using FATWA_DOMAIN.Interfaces.Notification;
using FATWA_DOMAIN.Interfaces.Tasks;
using FATWA_DOMAIN.Interfaces.TimeInterval;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.WorkerService;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using static FATWA_DOMAIN.Enums.TimeInterval.TimeIntervalEnums;
using FATWA_DOMAIN.Models.ViewModel.TimeIntervalVMs;
using FATWA_DOMAIN.Models.ViewModel.WorkerServiceVMs;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using FATWA_DOMAIN.Models.WorkflowModels;
using FATWA_DOMAIN.Models.ViewModel;
using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Models.RabbitMQ;
using FATWA_DOMAIN.Models.CaseManagment;

namespace FATWA_INFRASTRUCTURE.Repository.WorkerService
{
    public class WorkerServiceRepository : IWorkerService
    {
        #region Variables
        private readonly DatabaseContext _dbContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly INotification _iNotifications;
        private readonly ITask _iTask;
        #endregion

        #region Properties

        private List<TimeIntervalVM> _TimeIntervalsVM;

        #endregion

        #region Constructor
        public WorkerServiceRepository(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            var scope = _serviceScopeFactory.CreateScope();
            _iNotifications = scope.ServiceProvider.GetRequiredService<INotification>();
            //_iTask = scope.ServiceProvider.GetRequiredService<ITask>();

            _dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        }
        #endregion

        #region  Reminder Process and Error Logs
        public async Task<bool> ReminderProcessLogAsync(WSCmsComsReminderProcessLog _cmsComsReminderProcessLog)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using (dbContext)
            {
                try
                {
                    WSCmsComsReminderProcessLog cmsComsReminderProcessLog = new WSCmsComsReminderProcessLog
                    {
                        Id = Guid.NewGuid(),
                        WorkerServiceId = _cmsComsReminderProcessLog.WorkerServiceId,
                        IsNotification = _cmsComsReminderProcessLog.IsNotification,
                        IsTask = _cmsComsReminderProcessLog.IsTask,
                        IsFirstReminder = _cmsComsReminderProcessLog.IsFirstReminder,
                        IsSecondReminder = _cmsComsReminderProcessLog.IsSecondReminder,
                        IsThirdReminder = _cmsComsReminderProcessLog.IsThirdReminder,
                        ReceiverId = _cmsComsReminderProcessLog.ReceiverId,
                        Sender = "SystemGenerated",
                        ReminderId = _cmsComsReminderProcessLog.ReminderId,
                        ReferenceId = _cmsComsReminderProcessLog.ReferenceId,
                        Description = _cmsComsReminderProcessLog.Description,
                        ReminderTypeId = _cmsComsReminderProcessLog.ReminderTypeId,
                        CreatedBy = "SyestemGenerated",
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true,
                    };
                    await dbContext.CmsComsReminderProcessLogs.AddAsync(cmsComsReminderProcessLog);
                    await dbContext.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        public async Task ReminderErrorLogAsync(WSCmsComsReminderErrorLog _cmsComsReminderErrorLog)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using (dbContext)
            {
                try
                {
                    WSCmsComsReminderErrorLog cmsComsReminderErrorLog = new WSCmsComsReminderErrorLog
                    {
                        Id = Guid.NewGuid(),
                        WorkerServiceId = _cmsComsReminderErrorLog.WorkerServiceId,
                        IsNotification = _cmsComsReminderErrorLog.IsNotification,
                        IsTask = _cmsComsReminderErrorLog.IsTask,
                        IsFirstReminder = _cmsComsReminderErrorLog.IsFirstReminder,
                        IsSecondReminder = _cmsComsReminderErrorLog.IsSecondReminder,
                        IsThirdReminder = _cmsComsReminderErrorLog.IsThirdReminder,
                        ReceiverId = _cmsComsReminderErrorLog.ReceiverId,
                        Sender = "SystemGenerated",
                        ReminderId = _cmsComsReminderErrorLog.ReminderId,
                        ReferenceId = _cmsComsReminderErrorLog.ReferenceId,
                        Message = _cmsComsReminderErrorLog.Message,
                        ReminderTypeId = _cmsComsReminderErrorLog.ReminderTypeId,
                        CreatedBy = "SyestemGenerated",
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true,
                    };
                    await dbContext.CmsComsReminderErrorLogs.AddAsync(cmsComsReminderErrorLog);
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        #endregion

        #region  Reminder Process and Error Logs Data Migration Number Pattern
        public async Task<bool> ProcessLogAsync(WSCmsComsReminderProcessLog _cmsComsDataMigrationProcessLog)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using (dbContext)
            {
                try
                {
                    WSCmsComsReminderProcessLog cmsComsDataMigrationProcessLog = new WSCmsComsReminderProcessLog
                    {
                        Id = Guid.NewGuid(),
                        WorkerServiceId = _cmsComsDataMigrationProcessLog.WorkerServiceId,
                        IsNotification = false,
                        IsTask = false,
                        IsFirstReminder = false,
                        IsSecondReminder = false,
                        IsThirdReminder = false,
                        ReceiverId = null,
                        Sender = "SystemGenerated",
                        ReminderId = null,
                        ReferenceId = null,
                        Description = _cmsComsDataMigrationProcessLog.Description,
                        ReminderTypeId = null,
                        CreatedBy = "SyestemGenerated",
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true,
                    };
                    await dbContext.CmsComsReminderProcessLogs.AddAsync(cmsComsDataMigrationProcessLog);
                    await dbContext.SaveChangesAsync();
                    return true;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        public async Task ErrorLogAsync(WSCmsComsReminderErrorLog _cmsComsDataMigrationErrorLog)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using (dbContext)
            {
                try
                {
                    WSCmsComsReminderErrorLog cmsComsDataMigrationErrorLog = new WSCmsComsReminderErrorLog
                    {
                        Id = Guid.NewGuid(),
                        WorkerServiceId = _cmsComsDataMigrationErrorLog.WorkerServiceId,
                        IsNotification = false,
                        IsTask = false,
                        IsFirstReminder = false,
                        IsSecondReminder = false,
                        IsThirdReminder = false,
                        ReceiverId = null,
                        Sender = "SystemGenerated",
                        ReminderId = null,
                        ReferenceId = null,
                        Message = _cmsComsDataMigrationErrorLog.Message,
                        ReminderTypeId = null,
                        CreatedBy = "SyestemGenerated",
                        CreatedDate = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true,
                    };
                    await dbContext.CmsComsReminderErrorLogs.AddAsync(cmsComsDataMigrationErrorLog);
                    await dbContext.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }
        #endregion

        #region Get Reminders from SPs
        public async Task<List<WSReminderToCompleteClaimStatementVM>> GetReminderToCompleteClaimStatement()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            using (_DbContext)
            {
                try
                {
                    string storedProc = $"exec pWSGetCMSCompleteClaimStatementReminder";
                    List<WSReminderToCompleteClaimStatementVM> result = await _DbContext.WSReminderToCompleteClaimStatement.FromSqlRaw(storedProc).ToListAsync();
                    return result.Any() == false ? null : result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public async Task<List<WSDefenseLetterReminderServiceVM>> GetReminderToPrepareDefenseLetter()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            using (_DbContext)
            {
                try
                {
                    string storedProc = $"exec pWSGetCMSDefenseLetterReminder";
                    List<WSDefenseLetterReminderServiceVM> result = await _DbContext.WSDefenseLetterReminderServiceVMs.FromSqlRaw(storedProc).ToListAsync();
                    return result.Any() == false ? null : result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public async Task<List<WSAdditionalInformationCommunication>> GetAdditionalInformationCommunication()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            using (_DbContext)
            {
                try
                {
                    string storedProc = $"exec pWSGetAdditioalInformationCommunication";
                    List<WSAdditionalInformationCommunication> result = await _DbContext.WSAdditionalInformationCommunication.FromSqlRaw(storedProc).ToListAsync();
                    return result.Any() == false ? null : result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public async Task<List<WSRequestForAdditionalInfoVM>> GetReminderForAdditionalInformation()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            using (_DbContext)
            {
                try
                {
                    string storedProc = $"exec pWSGetCMSForAdditionalInformation";
                    List<WSRequestForAdditionalInfoVM> result = await _DbContext.WSRequestForAdditionalInfoVMs.FromSqlRaw(storedProc).ToListAsync();
                    return result.Any() == false ? null : result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public async Task<List<WSRequestForAdditionalInfoReminderVM>> GetReminderForAdditionalInformationReminder()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            using (_DbContext)
            {
                try
                {
                    string storedProc = $"exec pWSGetCMSForAdditionalInformationReminder";
                    List<WSRequestForAdditionalInfoReminderVM> result = await _DbContext.WSRequestForAdditionalInfoReminderVMs.FromSqlRaw(storedProc).ToListAsync();
                    return result.Any() == false ? null : result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public async Task<List<WSCmsMOJMessangerIntervalVM>> GetAllMojReminderByReminder()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using (_DbContext)
            {
                try
                {
                    string storedProc = $"exec pWSGetCMSMOJMessengerReminder";
                    List<WSCmsMOJMessangerIntervalVM> result = await _DbContext.CmsMOJMessangerIntervalVMs.FromSqlRaw(storedProc).ToListAsync();
                    return result.Any() == false ? null : result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public List<WSCmsDraftTemplateIntervalVM> GetCmsDraftTemplateReminder(int statusId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            try
            {
                string storedProc = $"exec pGetCMSDraftTemplateDurationReminder @StatusId = {statusId}";

                var result = _DbContext.WSCmsDraftTemplateIntervalVMs.FromSqlRaw(storedProc).ToList();

                return result.Count == 0 ? null : result;
            }
            catch (Exception)
            {
                throw;
                // You might want to handle the exception or log it appropriately instead of just returning null and re-throwing.
            }
        }

        public List<WSCmsDraftTemplateIntervalVM> GetHOSTemplateReminder(int reminderNo)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            try
            {
                string storedProc = $"exec pGetCMSDraftTemplateDurationReminder @reminderNo = {reminderNo}";

                var result = _DbContext.WSCmsDraftTemplateIntervalVMs.FromSqlRaw(storedProc).ToList();

                return result.Any() == false ? null : result;
            }
            catch (Exception)
            {
                throw;
                // You might want to handle the exception or log it appropriately instead of just returning null and re-throwing.
            }
        }

        //public List<WSCMSCaseFileHOSReminderVM> GetCaseFileHOSReminderRegional()
        //{
        //    using var scope = _serviceScopeFactory.CreateScope();
        //    var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        //    try
        //    {
        //        string storedProc = $"exec pWSGetCaseFileHOSReminderRegional";

        //        var result = _DbContext.WSCMSCaseFileHOSRemindersVMs.FromSqlRaw(storedProc).ToList();

        //        return result.Count == 0 ? null : result;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //        // You might want to handle the exception or log it appropriately instead of just returning null and re-throwing.
        //    }
        //}

        //public List<WSCMSCaseFileHOSReminderVM> GetCaseFileHOSReminderAppeal()
        //{
        //    using var scope = _serviceScopeFactory.CreateScope();
        //    var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        //    try
        //    {
        //        string storedProc = $"exec pWSGetCaseFileHOSReminderAppeal";

        //        var result = _DbContext.WSCMSCaseFileHOSRemindersVMs.FromSqlRaw(storedProc).ToList();

        //        return result.Count == 0 ? null : result;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //        // You might want to handle the exception or log it appropriately instead of just returning null and re-throwing.
        //    }
        //}

        //public List<WSCMSCaseFileHOSReminderVM> GetCaseFileHOSReminderSupreme()
        //{
        //    using var scope = _serviceScopeFactory.CreateScope();
        //    var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        //    try
        //    {
        //        string storedProc = $"exec pWSGetCaseFileHOSReminderSupreme";

        //        var result = _DbContext.WSCMSCaseFileHOSRemindersVMs.FromSqlRaw(storedProc).ToList();

        //        return result.Count == 0 ? null : result;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //        // You might want to handle the exception or log it appropriately instead of just returning null and re-throwing.
        //    }
        //}
        public List<WSCMSCaseFileHOSReminderRegionalORAppealORSupremeVM> GetCaseFileHOSReminderRegionalORAppealORSupreme(int SectorTypeId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            try
            {
                string storedProc = $"exec pWSGetCaseFileHOSReminderRegionalORAppealORSupreme @SectorTypeId = {SectorTypeId}";

                var result = _DbContext.WSCMSCaseFileHOSRemindersRegionalORAppealORSupremeVMs.FromSqlRaw(storedProc).ToList();

                return result.Count == 0 ? null : result;
            }
            catch (Exception)
            {
                throw;
                // Consider handling the exception or logging it appropriately.
            }
        }

        public async Task CloseCaseAndTaskStatusDone(Guid caseId)
        {
            try
            {
                //var LawyerRoleId = SystemRoles.Lawyer;
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                //string StoreProc = $"exec pCMSCloseCase @caseId = '{caseId}', @fileId = '{fileId}'";
                //var users = await _DbContext.CmsCaseFileDetailVM.FromSqlRaw(StoreProc).ToListAsync();
                var a = await _DbContext.Database.ExecuteSqlRawAsync($"exec pCMSCloseCase @caseId = '{caseId}' ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<User> GetHOSBySectorId(int sectorTypeId)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                string StoreProc = $"exec pGetHOSBSectorId @sectorTypeId = '{sectorTypeId}'";
                var users = await _DbContext.Users.FromSqlRaw(StoreProc).ToListAsync();
                return users.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<User>> GetHOSAndViceHOSBySectorId(string username, int sectorTypeId, bool verifyViceHOSResponsibility, bool includeHOS, int chamberNumberId)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                string StoreProc = $"exec pGetHOSAndViceHosBySectorId @sectorTypeId = '{sectorTypeId}', @username = '{username}', @verifyViceHOSResponsibility = '{verifyViceHOSResponsibility}', @returnHOS = '{includeHOS}', @chamberNumberId = '{chamberNumberId}'";
                var users = await _DbContext.Users.FromSqlRaw(StoreProc).ToListAsync();
                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        // This method might be for the perriod to save auto file (Need Verification) this method sp has executed
        public async Task<List<WSCmsComsIntervalVM>> GetAllCaseRequestNumber(int reminderNo)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            try
            {
                string storedProc = $"exec pGetDuration @reminderNo = {reminderNo}";

                var result = await _dbContext.WSCmsComsIntervalVMs.FromSqlRaw(storedProc).ToListAsync();

                return result.Any() == false ? null : result;

            }
            catch (Exception)
            {
                throw;
                // You might want to handle the exception or log it appropriately instead of just returning null and re-throwing.
            }
        }
        public async Task<List<WSReminderToCompleteDraftModificationVMS>> GetReminderToCompleteDraftModification()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            using (_DbContext)
            {
                try
                {
                    string storedProc = $"exec pWSGetCMSDraftModificationReminder";
                    List<WSReminderToCompleteDraftModificationVMS> result = await _DbContext.WSReminderToCompleteDraftModificationVMs.FromSqlRaw(storedProc).ToListAsync();
                    return result.Count == 0 ? null : result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
        public async Task<List<WSReminderForPendingTaskDecisionVM>> GetReminderForPendingTaskDecision(int NumberOfDays)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            using (_DbContext)
            {
                try
                {
                    string storedProc = $"exec pWSGetTaskDetailDecisionReminder  @NumberOfDays = {NumberOfDays}";
                    List<WSReminderForPendingTaskDecisionVM> result = await _DbContext.WSReminderForPendingTaskDecisionVMs.FromSqlRaw(storedProc).ToListAsync();
                    return result.Any() == false ? null : result;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }
            #endregion

            #region Reminder Entry 
            public async Task CmsComsReminderAsync(dynamic entity)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using (dbContext)
            {
                using (var transation = dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        int Id = 0;
                        Id = entity.Id;
                        if (entity.CmsComsReminderTypeId == (int)CmsComsReminderTypeEnums.DefineThePeriodToRegisterACaseAtMOJ)
                            entity = (WSCmsMOJMessangerIntervalVM)entity;
                        else if (entity.CmsComsReminderTypeId == (int)CmsComsReminderTypeEnums.DefineThePeriodToCompleteTheClaimStatement)
                            entity = (WSReminderToCompleteClaimStatementVM)entity;
                        else if (entity.CmsComsReminderTypeId == (int)CmsComsReminderTypeEnums.DefineThePeriodToReplyOnTheOperationConsultant)
                            entity = (WSCmsDraftTemplateIntervalVM)entity;

                        var task = await dbContext.CmsComsReminders.Where(x => x.ID == Id).FirstOrDefaultAsync();

                        if (task != null)
                        {
                            task.ID = entity.Id;
                            await dbContext.CmsComsReminders.AddAsync(task);
                            dbContext.Entry(task).State = EntityState.Modified;
                            await dbContext.SaveChangesAsync();
                            transation.Commit();
                        }
                        else
                        {

                        }
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
            }

        }
        #endregion

        #region Get Reminders
        public List<WSCmsCaseFileForHOSIntervalVM> GetCaseFileForHOSCommunicationResponseReminder(string storedProc)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using (_DbContext)
            {
                try
                {
                    var result = _DbContext.WSCmsCaseFileForHOSIntervalVMs.FromSqlRaw(storedProc).ToList();
                    return result;
                }
                catch (Exception)
                {
                    return null;
                    throw;
                }
            }
        }
        public async Task<List<WSReminderCommunicationResponseVM>> GetCommunicationResponseReminder(int reminderNo, int reminderTypeId, int? comminicationTypeId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            using (_DbContext)
            {
                try
                {
                    string storedProc = $"exec pWSGetCommunicationResponseTypeReminder @reminderNo = {reminderNo},@CmsComsReminderTypeId = {reminderTypeId}, @communicationTypeId = {comminicationTypeId}";

                    List<WSReminderCommunicationResponseVM> result = await _DbContext.WSReminderCommunicationResponseVM.FromSqlRaw(storedProc).ToListAsync();

                    return result.Any() == false ? null : result;

                }
                catch (Exception)
                {
                    throw;
                    // You might want to handle the exception or log it appropriately instead of just returning null and re-throwing.
                }
            }
        }
        public List<WSCmsFinalJudgmentIntervalVM> GetAllFinalJudgmentReminder(string storedProc)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using (_DbContext)
                try
                {
                    var result = _DbContext.WSCmsFinalJudgmentIntervalVMs.FromSqlRaw(storedProc).ToList();
                    return result;
                }
                catch (Exception)
                {
                    return null;
                    throw;
                }
        }
        #endregion

        #region Get User and Sector Ids
        public async Task<string> UserIdByUserEmailAsync(string email)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            using (_DbContext)
            {
                string? _Id = null;
                try
                {
                    _Id = _DbContext.Users.FirstOrDefault(x => x.Email == email).Id;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                return _Id;
            }
        }
        public User GetMojBySectorId(int sectorTypeId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using (_DbContext)
            {
                try
                {
                    string StoreProc = $"exec pGetMojBySectorId @sectorTypeId = '{sectorTypeId}'";
                    var users = _DbContext.Users.FromSqlRaw(StoreProc).ToList();
                    return users.FirstOrDefault();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        public async Task<(string ManagerId, string ManagerName)> GetManagerByUserId(string userId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            try
            {
                var userEmploymentInfo = await dbContext.UserEmploymentInformation.FirstOrDefaultAsync(x => x.UserId == userId);
                if (userEmploymentInfo != null)
                {
                    var managerId = userEmploymentInfo.ManagerId;
                    var managerPersonalInfo = await dbContext.UserPersonalInformation.FirstOrDefaultAsync(x => x.UserId == managerId);
                    if (managerPersonalInfo != null)
                    {
                        var managerName = $"{managerPersonalInfo.FirstName_En} {managerPersonalInfo.LastName_En} / {managerPersonalInfo.FirstName_Ar} {managerPersonalInfo.LastName_Ar}";
                        return (managerId, managerName);
                    }
                }
                return (string.Empty, string.Empty);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<string> GetReviewerByUserId(string userId)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            try
            {
                return await dbContext.UserPersonalInformation.Where(x => x.UserId == userId).Select(x => x.FirstName_En + " " + x.LastName_En + "/" + x.FirstName_Ar + " " + x.LastName_Ar).FirstOrDefaultAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Worker Service Execution
        public async Task<List<WSExecutionStatus>> GetWSExecutionStatuses()
        {
            try
            {
                return await _dbContext.WSExecutionStatus.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<WSWorkerServices>> GetWorkerServices()
        {
            try
            {
                return await _dbContext.WSWorkerServices.ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task AddWorkerServiceExecution(WSWorkerServiceExecution workerServiceExecution)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using (_DbContext)
                try
                {
                    await _DbContext.WSWorkerServiceExecution.AddAsync(workerServiceExecution);
                    await _DbContext.SaveChangesAsync();
                }
                catch (Exception)
                {
                    throw;
                }
        }
        public async Task<List<WSExecutionDetailVM>> GetWorkerServiceExecutionDetail(WSExecutionAdvanceSearchVM wSExecutionAdvanceSearchVM)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using (_DbContext)
                try
                {
                    string fromDate = wSExecutionAdvanceSearchVM.FromDate != null ? Convert.ToDateTime(wSExecutionAdvanceSearchVM.FromDate).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    string toDate = wSExecutionAdvanceSearchVM.ToDate != null ? Convert.ToDateTime(wSExecutionAdvanceSearchVM.ToDate).ToString("yyyy/MM/dd HH:mm").ToString() : null;
                    string StoreProc = $"exec pWSGetWorkerServiceExecutionDetail @StatusId='{wSExecutionAdvanceSearchVM.StatusId}', @WorkerServiceId='{wSExecutionAdvanceSearchVM.WorkerServiceId}'" +
                        $", @FromDate='{fromDate}', @ToDate='{toDate}' ,@PageNumber ='{wSExecutionAdvanceSearchVM.PageNumber}',@PageSize ='{wSExecutionAdvanceSearchVM.PageSize}' ";
                    return await _DbContext.WSExecutionDetailVMs.FromSqlRaw(StoreProc).ToListAsync();

                }
                catch (Exception)
                {
                    throw;
                }
        }
        public async Task UpdateWorkerServiceExecution(WSWorkerServiceExecution workerServiceExecution)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using (_DbContext)
                try
                {
                    var wsExecution = await _DbContext.WSWorkerServiceExecution.Where(x => x.Id == workerServiceExecution.Id).FirstOrDefaultAsync();
                    if (wsExecution != null)
                    {
                        wsExecution.ExecutionDetails = workerServiceExecution.ExecutionDetails;
                        wsExecution.EndDateTime = DateTime.Now;
                        wsExecution.ReAttemptCount = workerServiceExecution.ReAttemptCount;
                        wsExecution.ExecutionStatusId = workerServiceExecution.ExecutionStatusId;
                        _DbContext.Entry(wsExecution).State = EntityState.Modified;
                        await _DbContext.SaveChangesAsync();
                    }
                }
                catch (Exception)
                {
                    throw;
                }
        }
        #endregion

        #region DataMigration SPs
        public async Task<(bool, string, List<CmsComsNumPatternHistory>)> MigratePatternsFromHistory()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                //Fetch the latest records for each PatternTypId within the last 24 hours
                var latestRecords = await dbContext.CmsComsNumPatternHistories
                    .Where(h => h.CreatedDate >= DateTime.Now.AddHours(-24))
                    .GroupBy(h => h.PatternTypId)
                    .Select(g => g.OrderByDescending(h => h.CreatedDate).FirstOrDefault())
                    .ToListAsync();

                if (!latestRecords.Any())
                {
                    return (true, "No records found to be updated", null);
                }

                //  Update the CMS_COMS_NUM_PATTERN table
                foreach (var record in latestRecords)
                {
                    var pattern = await dbContext.CmsComsNumPatterns.FindAsync(record.PatternId);
                    if (pattern != null)
                    {
                        pattern.Day = record.Day;
                        pattern.D_Order = record.D_Order;
                        pattern.Month = record.Month;
                        pattern.M_Order = record.M_Order;
                        pattern.Year = record.Year;
                        pattern.Y_Order = record.Y_Order;
                        pattern.StaticTextPattern = record.StaticTextPattern;
                        pattern.STP_Order = record.STP_Order;
                        pattern.SequanceNumber = record.SequanceNumber;
                        pattern.SN_Order = record.SN_Order;
                        pattern.SequanceResult = record.SequanceResult;
                        pattern.ResetYearly = record.ResetYearly;
                        pattern.ModifiedBy = record.CreatedBy;
                        pattern.ModifiedDate = record.CreatedDate;
                        pattern.DeletedBy = record.DeletedBy;
                        pattern.DeletedDate = record.DeletedDate;
                        pattern.IsDeleted = record.IsDeleted;
                        pattern.IsActive = record.IsActive;
                        pattern.SequanceFormatResult = record.SequanceFormatResult;
                        pattern.IsModified = true;
                    }
                }
                // Update the CMSRequestPatternId for the related government entities
                foreach (var record in latestRecords)
                {
                    var govEntityIds = record.UpdatedGovtEntities?.Split(',').Select(int.Parse).ToList();
                    if (govEntityIds != null)
                    {
                        // Fetch records where PatternId matches and also where GovtEntityId matches any of the to-be-updated govtEntityIds
                        var govEntitiesWithSamePattern = await dbContext.CmsGovtEntityNumPattern
                            .Where(x =>
                            (record.PatternTypId == (int)CmsComsNumPatternTypeEnum.CaseRequestNumber && x.CMSRequestPatternId == (Guid)record.PatternId)
                            || (record.PatternTypId != (int)CmsComsNumPatternTypeEnum.CaseRequestNumber && x.COMSRequestPatternId == (Guid)record.PatternId)
                            || govEntityIds.Contains((int)x.GovtEntityId))
                            .ToListAsync();

                        // Assign the PatterenId to new GovtEntity and remove the patterenId from old govtENtity which is removed by user
                        foreach (var govEntity in govEntitiesWithSamePattern)
                        {
                            if (govEntityIds.Contains((int)govEntity.GovtEntityId))
                            {
                                if (record.PatternTypId == (int)CmsComsNumPatternTypeEnum.CaseRequestNumber)
                                {
                                    govEntity.CMSRequestPatternId = (Guid)record.PatternId;
                                }
                                else
                                {
                                    govEntity.COMSRequestPatternId = (Guid)record.PatternId;
                                }
                            }
                            else if (govEntity.CMSRequestPatternId == record.PatternId)
                            {
                                if (record.PatternTypId == (int)CmsComsNumPatternTypeEnum.CaseRequestNumber)
                                {
                                    govEntity.CMSRequestPatternId = Guid.Empty;
                                }
                                else
                                {
                                    govEntity.COMSRequestPatternId = Guid.Empty;
                                }
                            }
                        }
                    }
                }
                await dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                latestRecords = latestRecords.Where(record => record != null && (record.PatternTypId == (int)CmsComsNumPatternTypeEnum.CaseRequestNumber || record.PatternTypId == (int)CmsComsNumPatternTypeEnum.ConsultationRequestNumber)).ToList();
                return (true, "Update successful", latestRecords);
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, "Update failed: " + ex.Message, null);
            }
        }

        public async Task<List<RMQ_UnpublishMessage>> RabbitMQMessages()
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            var UnpublishedMessages = await dbContext.RMQ_UnpublishMessages.Where(x => !x.IsPublished).ToListAsync();
            return UnpublishedMessages;
        }
        public async Task MarkMessageAsPublished(Guid messageId, bool isPublished)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            var message = await dbContext.RMQ_UnpublishMessages.FindAsync(messageId);
            if (message != null)
            {
                message.IsPublished = isPublished;
                message.ModifiedDate = DateTime.Now;
                if (!isPublished) message.Re_AttemptCount += 1;
                await dbContext.SaveChangesAsync();
            }
        }
        #endregion

        #region Save Pending Task Reminder
        public async Task SavePendingTaskReminder(TaskDecisionReminder args)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                await dbContext.TaskDecisionReminders.AddAsync(args);
                await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}
