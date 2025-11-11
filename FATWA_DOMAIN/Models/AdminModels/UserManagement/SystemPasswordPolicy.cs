using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    //<History Author = 'Umer Zaman' Date='2022-08-11' Version="1.0" Branch="master"> Create model for system configuration</History>
    [Table("SYSTEM_USER_PASSWORD_POLICY", Schema = "dbo")]
    public class SystemPasswordPolicy
    {
        [Key]
        public Guid PasswordPolicyId { get; set; }
        public int PasswordRuleId { get; set; }
        public string Rule_Name_En { get; set; }
        public string Rule_Name_Ar { get; set; }
        public string Rule_Value { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
    }
}
