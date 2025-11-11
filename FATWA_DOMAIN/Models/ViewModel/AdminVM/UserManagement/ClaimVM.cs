using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement
{
    //<History Author = 'Hassan Abbas' Date='2022-07-20' Version="1.0" Branch="master"> created VM for managing Claims of Roles</History>
    public class ClaimVM
    {
        [Key]
        public int Id { get; set; }
        public string? Title_En { get; set; }
        public string? Title_Ar { get; set; }
        public string? Module { get; set; }
        public string? SubModule { get; set; }
        public string? ClaimType { get; set; }
        public string? ClaimValue { get; set; }
        public bool IsAssigned { get; set; }
        public int? ModuleId { get; set; }
    }
}
