using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    //<History Author = 'Umer Zaman' Date='2022-08-09' Version="1.0" Branch="master"> Create model for system configuration</History>
    [Table("SYSTEM_CONFIGURATION", Schema = "dbo")]
    public class SystemConfiguration
    {
        [Key]
        public Guid ConfigurationId { get; set; }
        public Guid GroupId { get; set; }
        public DateTime Password_Period { get; set; }
        public int Session_Timeout_Period { get; set; }
        public int Wrong_Password_Attempts { get; set; }
        public int Invalid_Login_Attempts { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }

        [NotMapped]
        public ICollection<SystemPasswordPolicy>? SystemPasswordPolicyGet { get; set; }
    }
}
