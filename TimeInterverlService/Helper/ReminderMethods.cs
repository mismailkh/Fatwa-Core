using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Enums.WorkerService;
using FATWA_DOMAIN.Interfaces.Notification;
using FATWA_DOMAIN.Interfaces.Tasks;
using FATWA_DOMAIN.Interfaces.WorkerService;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.Notifications;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Models.TimeInterval;
using FATWA_DOMAIN.Models.ViewModel.TaskVMs;
using FATWA_DOMAIN.Models.ViewModel.TimeIntervalVMs;
using FATWA_DOMAIN.Models.WorkerService;


namespace FATWATIMEINTERVALSERVICES.Helper
{
    public class ReminderMethods
    {

        #region Variables
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly INotification _iNotifications;
        //private readonly ITask _iTask;
        private readonly ITimeIntervals _iTimeIntervals;

        #endregion

        #region Constructor
        public ReminderMethods(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            var scope = _serviceScopeFactory.CreateScope();
            _iNotifications = scope.ServiceProvider.GetRequiredService<INotification>();
            //_iTask = scope.ServiceProvider.GetRequiredService<ITask>();
            _iTimeIntervals = scope.ServiceProvider.GetRequiredService<ITimeIntervals>();

        }
        #endregion

        #region Send Reminder and Create Tasks 

        public int SetIntervalNumber(CmsComsReminder reminderInterval)
        {
            int interval = 1;
            if (reminderInterval.FirstReminderDuration != null && reminderInterval.SecondReminderDuration == null)
                interval = 1;
            else if (reminderInterval.FirstReminderDuration != null && reminderInterval.SecondReminderDuration != null && reminderInterval.ThirdReminderDuration == null)
                interval = 2;
            else if (reminderInterval.FirstReminderDuration != null && reminderInterval.SecondReminderDuration != null && reminderInterval.ThirdReminderDuration != null)
                interval = 3;
            return interval;
        }

        public WSCmsComsReminderProcessLog SetReminderNo(WSCmsComsReminderProcessLog cmsComsReminderProcessLog, int reminderNo)
        {
            switch (reminderNo)
            {
                case 1:
                    cmsComsReminderProcessLog.IsFirstReminder = true;
                    break;
                case 2:
                    cmsComsReminderProcessLog.IsSecondReminder = true;
                    break;
                case 3:
                    cmsComsReminderProcessLog.IsThirdReminder = true;
                    break;
            }
            return cmsComsReminderProcessLog;
        }
        public int SetIntervalReminderNo(CmsComsReminder cmsComsReminder, int reminderNo)
        {
            switch (reminderNo)
            {
                case 1:
                    reminderNo = (int)cmsComsReminder.SLAInterval - (int)cmsComsReminder.FirstReminderDuration;
                    break;
                case 2:
                    reminderNo = (int)cmsComsReminder.SLAInterval - (int)cmsComsReminder.SecondReminderDuration;
                    break;
                case 3:
                    reminderNo = (int)cmsComsReminder.SLAInterval - (int)cmsComsReminder.ThirdReminderDuration;
                    break;
            }
            return reminderNo;
        }
        public async Task<bool> SendReminderAsync(string receiverId, int ModuleId, int NotificationEventId, string action, string entityName, string entityId, NotificationParameter notificationParameter,string? taskUrl = null)
        {
            try
            {
                Guid notificationId = Guid.NewGuid();
                var res = await _iNotifications.SendNotification(new Notification
                {
                    NotificationId = notificationId,
                    DueDate = DateTime.Now.AddDays(5),
                    CreatedBy = "System_Generated",
                    CreatedDate = DateTime.Now,
                    IsDeleted = false,
                    ReceiverId = receiverId,
                    ModuleId = ModuleId,
                },
                NotificationEventId,
                action,
                entityName,
                entityId,
                notificationParameter);
                if(!string.IsNullOrEmpty(taskUrl))
                await _iNotifications.UpdateNotificationUrl(notificationId, taskUrl);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //public async Task<bool> CreateTaskAsync(string taskName, string assignTo, int sectorTypeId, Guid? referenceId, string action, string entityName, string? entityId,string actionName)
        //{
        //    try
        //    {
        //        var taskId = Guid.NewGuid();
        //        var taskResult = await _iTask.AddTask(new SaveTaskVM
        //        {
        //            Task = new UserTask
        //            {
        //                TaskId = taskId,
        //                Name = taskName,
        //                Date = DateTime.Now.Date,
        //                AssignedBy = "hosunderfilling@fatwa.com",
        //                AssignedTo = assignTo,
        //                IsSystemGenerated = true,
        //                TaskStatusId = (int)TaskStatusEnum.Pending,
        //                ModuleId = (int)WorkflowModuleEnum.CaseManagement,
        //                SectorId = (int)sectorTypeId,
        //                DepartmentId = (int)DepartmentEnum.Operational,
        //                TypeId = (int)TaskTypeEnum.Transfer,
        //                RoleId = SystemRoles.FatwaAdmin, //FATWA ADMIN
        //                CreatedBy = "SystemGenerated",
        //                CreatedDate = DateTime.Now,
        //                IsDeleted = false,
        //                ReferenceId = referenceId,
        //                SubModuleId = (int)SubModuleEnum.RegisteredCase,
        //                SystemGenTypeId = (int)TaskSystemGenTypeEnum.RegisteredCaseToMoj,
        //            },
        //            TaskActions = new List<TaskAction>()
        //            {
        //                new TaskAction()
        //                {
        //                    ActionName = actionName,
        //                    TaskId = taskId,
        //                }
        //            }
        //        },
        //        action,
        //        entityName,
        //        entityId);
        //        return taskResult;
        //    }
        //    catch (Exception e)
        //    {
        //        return false;
        //    }

        //}

        public async Task<(bool, List<PublicHolidaysVM>, List<WeekdaysSetting>)> CheckValidTimeToRunService()
        {
            try
            {
                var currentDate = DateTime.Now;
                var publicHolidays = await _iTimeIntervals.GetPublicHolidays();
                var weekends = await _iTimeIntervals.GetWeekdays();
                if (weekends.Any())
                {
                    weekends.Where(x => x.IsWeekend).ToList();
                }

                if (publicHolidays != null && publicHolidays.Any(x => currentDate.Date >= x.FromDate.Date && currentDate.Date <= x.ToDate.Date))
                {
                    return (false, new List<PublicHolidaysVM>(), weekends);
                }
                return (true, publicHolidays ?? new List<PublicHolidaysVM>(), weekends);
            }
            catch (Exception)
            {
                return (false, new List<PublicHolidaysVM>(), new List<WeekdaysSetting>());
            }
        }
        // The Main Purpose of this function is that we calculate the working days from weekends + Holidays, so we only send reminder in working days
        public int FetchNumberOfHolidaysAndWeekends(List<PublicHolidaysVM> holidays, DateTime fromDate, List<WeekdaysSetting> weekends)
        {
            //DateTime fromDat = new DateTime(2024, 8, 2, 12, 5, 16, 307).Date;
            DateTime todayDate = DateTime.Today.Date;
            int totalHolidayDays = 0;
            int totalWeekendDays = 0;
            if (holidays.Any())
            {
                foreach (var holiday in holidays)
                {
                    if (holiday.FromDate.Date <= todayDate && holiday.ToDate.Date >= fromDate)
                    {
                        var holidayDays = (holiday.ToDate.Date - holiday.FromDate.Date).Days + 1;
                        for (int i = 0; i < holidayDays; i++)
                        {
                            var day = holiday.FromDate.AddDays(i);
                            var weekday = weekends.FirstOrDefault(x => x.NameEn == day.DayOfWeek.ToString());
                            if (weekday != null && !weekday.IsWeekend)
                            {
                                totalWeekendDays++;
                            }
                            else
                            {
                                totalHolidayDays++;
                            }
                        }
                    }
                }
            }
            else
            {
                var weekDays = (todayDate - fromDate.Date).Days + 1;
                for (int i = 0; i < weekDays; i++)
                {
                    var day = fromDate.AddDays(i);
                    var weekday = weekends.FirstOrDefault(x => x.NameEn == day.DayOfWeek.ToString());
                    if (weekday != null && weekday.IsWeekend)
                    {
                        totalWeekendDays++;
                    }
                }
            }
            return totalHolidayDays + totalWeekendDays;
        }
        #endregion
    }
}

