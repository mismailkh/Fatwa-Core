using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.EntityFrameworkCore;

namespace FATWA_INFRASTRUCTURE.Repository
{
    //<History Author = 'Umer Zaman' Date='2022-09-10" Version="1.0" Branch="master"> Repository for managing system setting functionality</History>
    public class SystemSettingRepository : ISystemSetting
    {
        #region Constructor
        public SystemSettingRepository(DatabaseContext dbContext, DmsDbContext dmsdbContext)
        {
            _dbContext = dbContext;
            _dmsdbContext = dmsdbContext;
        }
        #endregion

        #region Variable Declaration
        private readonly DatabaseContext _dbContext;
        private readonly DmsDbContext _dmsdbContext;
        private List<SystemSetting> _SystemSettingDetails;

        #endregion

        #region Get System sertting
        public async Task<SystemSetting> GetSystemSetting()
        {
            try
            {
                var systemSetting = await _dbContext.SystemSettings.FirstOrDefaultAsync();
                systemSetting.SigningMethods =  await _dmsdbContext.DsSigningMethods.ToListAsync();
                return systemSetting;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Update system setting

        //<History Author = 'Umer Zaman' Date='2022-08-09' Version="1.0" Branch="master"> Save system configuration details</History>
        public async Task UpdateSystemSetting(SystemSetting systemsetting)
        {
            try
            {
                    using (_dbContext)
                    {
                        using (var transaction = _dbContext.Database.BeginTransaction())
                        {
                            try
                            {
                                _dbContext.Entry(systemsetting).State = EntityState.Modified;
                                await _dbContext.SaveChangesAsync();
                            foreach(var signingMethod in systemsetting.SigningMethods)
                                {
                                    _dmsdbContext.Entry(signingMethod).State = EntityState.Modified;
                                    await _dmsdbContext.SaveChangesAsync();
                                }

                                transaction.Commit();
                            }
                            catch (Exception ex)
                            {
                                transaction.Rollback();
                                throw;
                            }
                        }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion


    }
}
