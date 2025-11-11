using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{

    [Table("UMS_USER")]
    public class User
    {
        [Key]
        public string Id { get; set; } 
        public string? SecurityStamp { get; set; }
        public string? ConcurrencyStamp { get; set; } 
        [Required]
        public bool TwoFactorEnabled { get; set; } = false; 
        public DateTime? LockoutEnd { get; set; }
        [Required]
        public bool LockoutEnabled { get; set; } 
        [Required]
        public int AccessFailedCount { get; set; } 
        
        //For literature Borrow detail
        public int EligibleCount { get; set; }


        //User Details
        public string? FirstName_En { get; set; }
        public string? FirstName_Ar { get; set; }
        public string? SecondName_En { get; set; }
        public string? SecondName_Ar { get; set; }
        public string? LastName_En { get; set; }
        public string? LastName_Ar { get; set; }
        public string? PhoneNumber { get; set; }
        [Required]
        public bool PhoneNumberConfirmed { get; set; } = false;
        public string? AlternatePhoneNumber { get; set; }
        [StringLength(256)]
        public string? Email { get; set; } 
        public string? NormalizedEmail { get; set; }
        [Required]
        public bool EmailConfirmed { get; set; } = false;

        [Required(ErrorMessage = "Username is required")]
        [StringLength(256)]
        public string UserName { get; set; }
        public string? NormalizedUserName { get; set; }

        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public DateTime? DOJ { get; set; }

        public Guid? GroupId { get; set; }
        public int? Nationality { get; set; }
        public int? GenderId { get; set; }
        public int? RoleId { get; set; }
        public int? UserTypeId { get; set; }
        public int? GradeId { get; set; }
        public int? DepartmentId { get; set; }
        public int? DesignationId { get; set; }
        public int? ManagerId { get; set; }
        public bool IsActive { get; set; }
        public bool AllowAccess { get; set; }
        public bool IsLocked { get; set; }
        public DateTime? AbsentFrom { get; set; }
        public DateTime? AbsentTo { get; set; }
        public int? ReplacementId { get; set; }
        public string? ProfilePicUrl { get; set; }
        [Required(ErrorMessage = "Password is required")]
        [StringLength(256)] 
        public string? PasswordHash { get; set; }

        [Required]
        [StringLength(256)] 
        public string ConfirmPassword { get; set; }


    }
}
