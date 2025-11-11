using FATWA_DOMAIN.Models.AdminModels.UserManagement;

namespace FATWA_DOMAIN.Interfaces
{
    //<History Author = 'Umer Zaman' Date='2022-08-09' Version="1.0" Branch="master"> interface for system configuration operations</History>
    public interface ISystemConfiguration
    {
        Task<List<SystemConfiguration>> GetSystemConfigurationDetails();
        Task<List<SystemOption>> GetSystemOptionDetails();
        Task<User> GetUserDetailByUsingEmail(string email);
        Task<SystemConfiguration> GetUserGroupDetailByUsingGroupId();
        Task LockUserAccount(string email);
        Task UserAccountAccessFailCount(string email, int wrongCount);
    }
}
