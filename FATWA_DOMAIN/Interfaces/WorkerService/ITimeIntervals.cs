using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FATWA_DOMAIN.Models.TimeInterval;
using FATWA_DOMAIN.Models.WorkerService;
using FATWA_DOMAIN.Models.ViewModel.TimeIntervalVMs;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;

namespace FATWA_DOMAIN.Interfaces.WorkerService
{
    public interface ITimeIntervals
    {
        #region  GetTimeIntervals
        Task<List<TimeIntervalVM>> GetTimeIntervals();
        Task<List<TimeIntervalHistoryVM>> GetTimeIntervalHistoryList();
        Task<CmsComsReminder> SaveCmsComsReminder(CmsComsReminder chamber);
        Task<CmsComsReminder> UpdateCmsComsReminder(CmsComsReminder chamber);
        Task<CmsComsReminder> GetCmsComsReminderById(int Id);
        Task<List<CmsComsReminder>> GetCmsComsReminder(int Id);
        Task<CmsComsReminder> GetReminderIntervalById(int Id);
        Task<List<CmsComsReminderType>> GetCmsComsReminderType();
        Task<List<CmsComsReminder>> GetCmsComsCommunicationReminder(int id);
        Task<bool> UpdateIntervalStatus(bool isActive, int id);
        #endregion

        #region Public Holidays CRUD
        Task<List<PublicHolidaysVM>> GetPublicHolidays();
        Task<List<WeekdaysSetting>> GetWeekdays();
        Task<PublicHoliday> AddPublicHoliday(PublicHoliday publicHolidays);
        Task<PublicHolidaysVM> UpdatePublicHoliday(PublicHolidaysVM publicHolidays);
        Task<PublicHolidaysVM> DeletePublicHoliday(PublicHolidaysVM publicHoliday);
        Task<PublicHoliday> GetPublicHolidayById(int id);
        #endregion
    }
}
