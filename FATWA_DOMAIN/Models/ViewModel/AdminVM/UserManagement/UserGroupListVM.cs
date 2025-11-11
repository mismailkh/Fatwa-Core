using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement
{
    public class UserGroupListVM
    {
        [Key]
        public Guid GroupId { get; set; }
        public int GroupTypeId { get; set; }
        public string GroupNameEn { get; set; }
        public string GroupNameAr { get; set; }
        public string DescriptionEn { get; set; }
        public string DescriptionAr { get; set; }
        public string? GroupTypeNameEn { get; set; }
        public string? GroupTypeNameAr { get; set; }
    }
}


