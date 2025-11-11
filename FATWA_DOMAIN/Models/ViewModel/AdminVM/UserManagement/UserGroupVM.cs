using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement
{
    public class UserGroupVM
    {
        [Key]
        // [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid GroupId { get; set; }
        public string Name_En { get; set; }
        public string Name_Ar { get; set; }
        public string Description_En { get; set; }
        public string Description_Ar { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; } = false;

        [NotMapped]
        public ICollection<Role>? Roles { get; set; }
        [NotMapped]
        public ICollection<UserVM>? Users { get; set; }
        //public string? GroupNameitleAr { get; set; }
        //public string? GroupNameitleEn { get; set; }
        //public string? GroupDescriptionEn { get; set; }
        //public string? GroupDescriptionAr { get; set; }
        //public DateTime? GroupCreationDate { get; set; }
        //public string? CreatedBy { get; set; }
        //public string? PhoneNumber { get; set; }

    }
}
