using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    [Table("UMS_GROUP", Schema = "dbo")]
    public class Group : TransactionalBaseModel
    {
        [Key]
        public Guid GroupId { get; set; }
        public int GroupTypeId { get; set; }
        public string Name_En { get; set; }
        public string Name_Ar { get; set; }
        public string Description_En { get; set; }
        public string Description_Ar { get; set; }
        [NotMapped]
        public IList<ClaimVM> GroupClaims { get; set; }
        [NotMapped]
        public IList<UserListGroupVM>? Users { get; set; }=new List<UserListGroupVM>();
        [NotMapped]
        public IList<UserListGroupVM>? UsersWithExistingGroups { get; set; }=new List<UserListGroupVM>();
        //public GroupTypeWebSystem GroupAccessType { get; set; }
        public ICollection<UserGroup> GroupUsers { get; set; }
        public GroupType GroupType { get; set; }
		[NotMapped]
        public string GroupTypeEn { get; set; }
		[NotMapped]
		public string GroupTypeAr { get; set; }

	}
}
