using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    //<History Author = 'Umer Zaman' Date='2022-08-13' Version="1.0" Branch="master"> Create model for system configuration</History>
    [Table("SYSTEM_CONFIGURATION_SYSTEM_PASSWORD_POLICY")]
    public class SystemConfigurationSystemPasswordPolicy
    {
        [Key]
        public Guid Id { get; set; }
        public Guid ConfigurationId { get; set; }
        public Guid PasswordPolicyId { get; set; }
    }
}
