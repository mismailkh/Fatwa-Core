using FATWA_DOMAIN.Models.AdminModels.UserManagement;

namespace FATWA_DOMAIN.Interfaces
{
    public interface ISystemSetting
    {
        Task<SystemSetting> GetSystemSetting();
        Task UpdateSystemSetting(SystemSetting systemsetting);
    }
}
