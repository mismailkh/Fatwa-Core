using System.ComponentModel.DataAnnotations;
namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement
{
    public class UserListGroupVM
    {
        [Key]
        public string Id { get; set; }
        public Guid? GroupId { get; set; }
        public string FirstNameEnglish { get; set; }
        public string? SecondNameEnglish { get; set; }
		public string LastNameEnglish { get; set; }
		public string FirstNameArabic { get; set; }
		public string? SecondNameArabic { get; set; }
        public string LastNameArabic { get; set; }
        public string? FullName_En { get; set; }
        public string? FullName_Ar { get; set; }
        public string PhoneNumber { get; set; }
        public int EligibleCount { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentArabic { get; set; }
        public string DepartmentEnglish { get; set; }
        public string UserTypeArabic { get; set; }
        public string UserTypeEnglish { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public int? SectorTypeId { get; set; } = 0;
        public string? SectorTypeName_En { get; set;}
        public string? SectorTypeName_Ar { get; set;}
        public string? Designation_En { get; set; }
        public string? Designation_Ar { get; set; }
        public string? Role_En { get; set; }
        public string? Role_Ar { get; set; }
        public string? ADUsername { get; set; }
        public bool IsUserHasAnyTask { get; set; }
        public bool IsActiveUser { get; set; }
    }

}
