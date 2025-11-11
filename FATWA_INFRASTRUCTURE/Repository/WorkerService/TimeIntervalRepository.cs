using FATWA_DOMAIN.Interfaces.Notification;
using FATWA_DOMAIN.Interfaces.Tasks;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FATWA_DOMAIN.Interfaces.WorkerService;
using FATWA_DOMAIN.Models.TimeInterval;
using FATWA_DOMAIN.Models.WorkerService;
using FATWA_DOMAIN.Models.ViewModel.TimeIntervalVMs;
using FATWA_DOMAIN.Models.LegalPrinciple;
using FATWA_DOMAIN.Models;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.TimeInterval.TimeIntervalEnums;
using static FATWA_DOMAIN.Enums.WorkerService.WorkerServiceEnums;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;

namespace FATWA_INFRASTRUCTURE.Repository.WorkerService
{
    public class TimeIntervalRepository : ITimeIntervals
    {
        private readonly DatabaseContext _dbContext;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly INotification _iNotifications;
        private readonly ITask _iTask;
        #region Properties

        private List<TimeIntervalVM> _TimeIntervalsVM;

        #endregion

        #region Constructor
        public TimeIntervalRepository(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            var scope = _serviceScopeFactory.CreateScope();
            _iNotifications = scope.ServiceProvider.GetRequiredService<INotification>();
            // _iTask = scope.ServiceProvider.GetRequiredService<ITask>();
            _dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
        }

        #endregion

        #region  GetTimeInttervals
        public async Task<List<TimeIntervalVM>> GetTimeIntervals()
        {
            try
            {
                if (_TimeIntervalsVM == null)
                {
                    string StoredProc = "exec pWSTimeIntervals";
                    _TimeIntervalsVM = await _dbContext.TimeIntervalVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _TimeIntervalsVM;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        public async Task<List<TimeIntervalHistoryVM>> GetTimeIntervalHistoryList()
        {
            try
            {
                string StoredProc = "exec pWSGetCmsComsReminderHistory";
                var result = await _dbContext.TimeIntervalHistoryVM.FromSqlRaw(StoredProc).ToListAsync();

                return result;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get CmsComsReminder by Id 
        public async Task<CmsComsReminder> GetCmsComsReminderById(int Id)
        {
            try
            {
                var scope = _serviceScopeFactory.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                using (dbContext)
                {
                    var result = await dbContext.CmsComsReminders.Where(x => x.CmsComsReminderTypeId == Id).FirstOrDefaultAsync();
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<CmsComsReminder>> GetCmsComsReminder(int Id)
        {
            try
            {
                var result = await _dbContext.CmsComsReminders.Where(x => x.CmsComsReminderTypeId == Id && x.IsActive).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<CmsComsReminder> GetReminderIntervalById(int Id)
        {
            try
            {
                var result = await _dbContext.CmsComsReminders.Where(x => x.ID == Id).FirstOrDefaultAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<CmsComsReminder>> GetCmsComsCommunicationReminder(int id)
        {
            try
            {
                var result = await _dbContext.CmsComsReminders.Where(x => x.CmsComsReminderTypeId == id && x.IsActive).ToListAsync();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region SaveCmsComsReminder
        public async Task<CmsComsReminder> SaveCmsComsReminder(CmsComsReminder cmsComsReminder)
        {
            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        await _dbContext.CmsComsReminders.AddAsync(cmsComsReminder);
                        await _dbContext.SaveChangesAsync();

                        CmsComsReminderHistory cmsComsReminderHistory = new CmsComsReminderHistory()
                        {
                            Id = Guid.NewGuid(),
                            CmsComsReminderId = cmsComsReminder.ID,
                            CmsComsReminderTypeId = cmsComsReminder.CmsComsReminderTypeId,
                            SLAInterval = cmsComsReminder.SLAInterval,
                            IsActive = cmsComsReminder.IsActive,
                            CommunicationTypeId = cmsComsReminder.CommunicationTypeId,
                            CreatedBy = cmsComsReminder.CreatedBy,
                            CreatedDate = cmsComsReminder.CreatedDate,
                            FirstReminderDuration = cmsComsReminder.FirstReminderDuration,
                            SecondReminderDuration = cmsComsReminder.SecondReminderDuration,
                            ThirdReminderDuration = cmsComsReminder.ThirdReminderDuration,
                            IsNotification = cmsComsReminder.IsNotification,
                            DraftTemplateVersionStatusId = cmsComsReminder.DraftTemplateVersionStatusId,
                            StatusId = (int)CmsComsReminderHistoryStatusEnums.Added
                        };
                        await SaveCmsComsReminderHistory(cmsComsReminderHistory);
                        transation.Commit();
                        return cmsComsReminder;
                    }
                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw new Exception(ex.Message, ex.InnerException);
                    }
                }
            }
        }
        #endregion

        #region SaveCmsComsReminderHistory
        public async Task<CmsComsReminderHistory> SaveCmsComsReminderHistory(CmsComsReminderHistory cmsComsReminderHistory)
        {
            try
            {
                await _dbContext.CmsComsReminderHistory.AddAsync(cmsComsReminderHistory);
                await _dbContext.SaveChangesAsync();
                return cmsComsReminderHistory;
            }
            catch (Exception ex)
            {
                throw;
            }

        }


        #endregion

        #region UpdateCmsComsReminder
        public async Task<CmsComsReminder> UpdateCmsComsReminder(CmsComsReminder cmsComsReminder)
        {

            using (var dbContext = _dbContext)
            {
                using (var transation = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        var reminder = await _dbContext.CmsComsReminders.Where(x => x.ID == cmsComsReminder.ID).FirstOrDefaultAsync();
                        if (reminder != null)
                        {
                            CmsComsReminderHistory cmsComsReminderHistory = new CmsComsReminderHistory()
                            {
                                Id = Guid.NewGuid(),
                                CmsComsReminderId = reminder.ID,
                                CmsComsReminderTypeId = reminder.CmsComsReminderTypeId,
                                SLAInterval = reminder.SLAInterval,
                                IsActive = reminder.IsActive,
                                CommunicationTypeId = reminder.CommunicationTypeId,
                                CreatedBy = reminder.CreatedBy,
                                CreatedDate = DateTime.Now,
                                FirstReminderDuration = reminder.FirstReminderDuration,
                                SecondReminderDuration = reminder.SecondReminderDuration,
                                ThirdReminderDuration = reminder.ThirdReminderDuration,
                                IsNotification = reminder.IsNotification,
                                DraftTemplateVersionStatusId = reminder.DraftTemplateVersionStatusId,
                                StatusId = (int)CmsComsReminderHistoryStatusEnums.Updated,
                                ExecutionTime = reminder.ExecutionTime,
                            };
                            await SaveCmsComsReminderHistory(cmsComsReminderHistory);

                            reminder.FirstReminderDuration = cmsComsReminder.FirstReminderDuration;
                            reminder.SecondReminderDuration = cmsComsReminder.SecondReminderDuration;
                            reminder.ThirdReminderDuration = cmsComsReminder.ThirdReminderDuration;
                            reminder.CommunicationTypeId = cmsComsReminder.CommunicationTypeId;
                            reminder.ModifiedBy = cmsComsReminderHistory.CreatedBy;
                            reminder.ModifiedDate = cmsComsReminderHistory.CreatedDate;
                            reminder.IsTask = cmsComsReminder.IsTask;
                            reminder.IsNotification = cmsComsReminder.IsNotification;
                            reminder.SLAInterval = cmsComsReminder.SLAInterval;
                            reminder.ExecutionTime = cmsComsReminder.ExecutionTime;
                            _dbContext.Entry(reminder).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transation.Commit();

                            string appSettingsPath = cmsComsReminder.WorkerServiceAppSettingPath;
                            if (!string.IsNullOrEmpty(appSettingsPath))
                            {
                                var json = File.ReadAllText(appSettingsPath);
                                dynamic jsonObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                                var executionTime = reminder.ExecutionTime.Value;
                                //("0 7 15 ? * SUN,MON,TUE,WED,THU *")//  at 03:15 PM only on Sunday to Tursday , evry Month
                                //("0 4 15 * 1-12 ?")) // at 03:04 pm every day from jan to dec

                                var minutes = executionTime.Minute;
                                var hours = executionTime.Hour;
                                var cronExpression = $"0 {minutes} {hours} ? * SUN,MON,TUE,WED,THU *";

                                switch (reminder.CmsComsReminderTypeId)
                                {
                                    case (int)WorkerServiceTypeEnums.DefineThePeriodToRegisterACaseAtMOJ:
                                        jsonObj["WorkerServiceCronsExpression"]["MOJReminderJob"] = cronExpression;
                                        break;
                                    case (int)WorkerServiceTypeEnums.DataMigrationFromHistoryToMain:
                                        jsonObj["WorkerServiceCronsExpression"]["DataPopulationJob"] = cronExpression;
                                        break;
                                    case (int)WorkerServiceTypeEnums.DefineThePeriodToPrepareDefenseLetter:
                                        jsonObj["WorkerServiceCronsExpression"]["OpinionLetterReminderJob"] = cronExpression;
                                        break;
                                    case (int)WorkerServiceTypeEnums.DefineThePeriodToRegionalORAppealORSupreme:
                                        jsonObj["WorkerServiceCronsExpression"]["HOSReminderServiceRegionalAppealSupremeJob"] = cronExpression;
                                        break;
                                    case (int)WorkerServiceTypeEnums.DefineThePeriodToCompleteTheClaimStatement:
                                        jsonObj["WorkerServiceCronsExpression"]["CompleteClaimStatement"] = cronExpression;
                                        break;
                                    case (int)WorkerServiceTypeEnums.DefineThePeriodForReminderRequestForAdditionInformation:
                                        jsonObj["WorkerServiceCronsExpression"]["RequestForAdditionalInfoReminderServiceJob"] = cronExpression;
                                        break;
                                    case (int)WorkerServiceTypeEnums.DefineThePeriodToRequestForAdditionalInformation:
                                        jsonObj["WorkerServiceCronsExpression"]["RequestForAdditionalInfoServiceJob"] = cronExpression;
                                        break;
                                    case (int)WorkerServiceTypeEnums.DefineThePeriodToReplyOnTheOperationConsultant:
                                        jsonObj["WorkerServiceCronsExpression"]["ReviewDraftReminderServiceJob"] = cronExpression;
                                        break; 
                                    case (int)WorkerServiceTypeEnums.DefineThePeriodForPendingTaskDecisionReminder:
                                        jsonObj["WorkerServiceCronsExpression"]["TaskDecisionPendingReminderServiceJob"] = cronExpression;
                                        break;
                                }
                                string output = Newtonsoft.Json.JsonConvert.SerializeObject(jsonObj, Newtonsoft.Json.Formatting.Indented);
                                File.WriteAllText(appSettingsPath, output);
                            }
                        }
                        return cmsComsReminder;
                    }

                    catch (Exception ex)
                    {
                        transation.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
            }
        }
        public async Task<bool> UpdateIntervalStatus(bool isActive, int id)
        {
            try
            {
                var interval = await _dbContext.CmsComsReminders.Where(x => x.ID == id).FirstOrDefaultAsync();
                if (interval != null)
                {
                    CmsComsReminderHistory cmsComsReminderHistory = new CmsComsReminderHistory()
                    {
                        Id = Guid.NewGuid(),
                        CmsComsReminderId = interval.ID,
                        CmsComsReminderTypeId = interval.CmsComsReminderTypeId,
                        SLAInterval = interval.SLAInterval,
                        IsActive = interval.IsActive,
                        CommunicationTypeId = interval.CommunicationTypeId,
                        CreatedBy = interval.CreatedBy,
                        CreatedDate = DateTime.Now,
                        FirstReminderDuration = interval.FirstReminderDuration,
                        SecondReminderDuration = interval.SecondReminderDuration,
                        ThirdReminderDuration = interval.ThirdReminderDuration,
                        IsNotification = interval.IsNotification,
                        DraftTemplateVersionStatusId = interval.DraftTemplateVersionStatusId,
                        StatusId = (int)CmsComsReminderHistoryStatusEnums.Updated
                    };
                    await SaveCmsComsReminderHistory(cmsComsReminderHistory);
                    interval.IsActive = isActive;
                    interval.ModifiedDate = cmsComsReminderHistory.CreatedDate;
                    interval.ModifiedBy = cmsComsReminderHistory.CreatedBy;

                    _dbContext.Entry(interval).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        #endregion

        public async Task<List<CmsComsReminderType>> GetCmsComsReminderType()
        {
            try
            {
                return await _dbContext.CmsComsReminderTypes.OrderBy(u => u.Id).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }



        #region Public Holidays CRUD

        public async Task<PublicHoliday> AddPublicHoliday(PublicHoliday publicHolidays)
        {
            try
            {
                var res = await _dbContext.PublicHolidays.AddAsync(publicHolidays);
                await _dbContext.SaveChangesAsync();
                return publicHolidays;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PublicHolidaysVM> UpdatePublicHoliday(PublicHolidaysVM publicHoliday)
        {
            try
            {
                var holiday = await _dbContext.PublicHolidays.Where(x => x.Id == publicHoliday.Id).FirstOrDefaultAsync();
                holiday.FromDate = publicHoliday.FromDate;
                holiday.ToDate = publicHoliday.ToDate;
                holiday.Description = publicHoliday.Description;
                holiday.ModifiedDate = publicHoliday.ModifiedDate;
                holiday.ModifiedBy = publicHoliday.ModifiedBy;
                _dbContext.Entry(holiday).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
                return publicHoliday;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<PublicHolidaysVM>> GetPublicHolidays()
        {
            try
            {
                string proc = "exec WSGetPublicHolidays";
                var publicHolidays = await _dbContext.PublicHolidaysVMs.FromSqlRaw(proc).ToListAsync();
                return publicHolidays;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<WeekdaysSetting>> GetWeekdays()
        {
            try
            {
                return await _dbContext.WeekdaysSettings.AsNoTracking().ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        } 
        public async Task<PublicHoliday> GetPublicHolidayById(int id)
        {
            try
            {
                var res = await _dbContext.PublicHolidays.Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefaultAsync();
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PublicHolidaysVM> DeletePublicHoliday(PublicHolidaysVM publicHoliday)
        {
            var holiday = await _dbContext.PublicHolidays.Where(x => x.Id == publicHoliday.Id).FirstOrDefaultAsync();
            holiday.IsDeleted = publicHoliday.IsDeleted;
            holiday.DeletedBy = publicHoliday.DeletedBy;
            holiday.DeletedDate = publicHoliday.DeletedDate;
            _dbContext.Entry(holiday).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
            return publicHoliday;
        }
        #endregion


    }
}
