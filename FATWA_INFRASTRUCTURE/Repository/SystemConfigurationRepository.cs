using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.EntityFrameworkCore;

namespace FATWA_INFRASTRUCTURE.Repository
{
    //<History Author = 'Umer Zaman' Date='2022-08-09" Version="1.0" Branch="master"> Repository for managing system configuration functionality</History>
    public class SystemConfigurationRepository : ISystemConfiguration
    {
        #region Constructor
        public SystemConfigurationRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Variable Declaration
        private readonly DatabaseContext _dbContext;
        private List<SystemConfiguration> _SystemConfigDetails;
        private List<SystemOption> _SystemOptions;

        #endregion
        #region Login validations variables
        private User _UserDetail = null;
        private SystemConfiguration _UserGroupDetail;
        #endregion

        //<History Author = 'Umer Zaman' Date='2022-08-09' Version="1.0" Branch="master">Get system configuration details </History>
        #region Get system configuration details

        public async Task<List<SystemConfiguration>> GetSystemConfigurationDetails()
        {
            if (_SystemConfigDetails == null)
            {
                _SystemConfigDetails = await _dbContext.SystemConfigurations.OrderByDescending(u => u.ConfigurationId).ToListAsync();
            }
            return _SystemConfigDetails;
        }
        #endregion

        #region Get system option details
        public async Task<List<SystemOption>> GetSystemOptionDetails()
        {
            if (_SystemOptions == null)
            {
                _SystemOptions = await _dbContext.SystemOptions.OrderByDescending(u => u.OptionId).ToListAsync();
            }
            return _SystemOptions;
        }
        #endregion

   

        #region Login validation
        public async Task<User> GetUserDetailByUsingEmail(string email)
        {
            try
            {
                if (_UserDetail == null)
                {
                    _UserDetail = await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
                }
                return _UserDetail;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task<SystemConfiguration> GetUserGroupDetailByUsingGroupId()
        {
            if (_UserGroupDetail == null)
            {
                _UserGroupDetail = await _dbContext.SystemConfigurations.FirstOrDefaultAsync();
            }
            return _UserGroupDetail;
        }
        #endregion

        #region User account lock

        //<History Author = 'Umer Zaman' Date='2022-08-23' Version="1.0" Branch="master"> lock user account after wrong password attempts count completed</History>
        public async Task LockUserAccount(string email)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var result = await _dbContext.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
                            if (result != null)
                            {
                                result.IsLocked = true;
                                _dbContext.Entry(result).State = EntityState.Modified;
                                await _dbContext.SaveChangesAsync();
                                transaction.Commit();
                            }
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

        #region User account access fail count manage

        //<History Author = 'Umer Zaman' Date='2022-08-23' Version="1.0" Branch="master"> lock user account after wrong password attempts count completed</History>
        public async Task UserAccountAccessFailCount(string email, int wrongCount)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            var result = await _dbContext.Users.Where(x => x.Email == email).FirstOrDefaultAsync();
                            if (result != null)
                            {
                                result.AccessFailedCount = wrongCount;
                                _dbContext.Entry(result).State = EntityState.Modified;
                                await _dbContext.SaveChangesAsync();
                                transaction.Commit();
                            }
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
