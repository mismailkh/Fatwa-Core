using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement
{
    //<History Author = 'Umer Zaman' Date='2022-07-29' Version="1.0" Branch="master"> create and use VM for user detail view</History>
    public class UserDetailViewListVM
    {
        [Key]
        public string Id { get; set; }
        public string? FirstName_En { get; set; }
        public string? FirstName_Ar { get; set; }
        public string? SecondName_En { get; set; }
        public string? SecondName_Ar { get; set; }
        public string? LastName_En { get; set; }
        public string? LastName_Ar { get; set; }
        public string? Address_En { get; set; }
        public string? PhoneNumber { get; set; }
        public string? AlternatePhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfJoining { get; set; }
        //public string? FullName_Ar { get; set; }
        //public string? FullName_En { get; set; }
        public string? UserName { get; set; }
        public string? PasswordHash { get; set; }
        //public string? ConfirmPassword { get; set; }

        //Gender
        // public int GenderId { get; set; }
        public string? GenderName_En { get; set; }
        public string? GenderName_Ar { get; set; }

        //Nationality
        // public int NationalityId { get; set; }
        public string? NationalityName_En { get; set; }
        public string? NationalityName_Ar { get; set; }

        //User Type
        //  public int UserTypeId { get; set; }
        public string? TypeName_En { get; set; }
        public string? TypeName_Ar { get; set; }

        //Designation
        //  public int DesignationId { get; set; }
        public string? DesignationName_En { get; set; }
        public string? DesignationName_Ar { get; set; }

        //Department
        //  public int DepartmentId { get; set; }
        public string? DepartmentName_En { get; set; }
        public string? DepartmentName_Ar { get; set; }

        // Profile Pic
        public string? ProfilePicturePath { get; set; }
    }

}
