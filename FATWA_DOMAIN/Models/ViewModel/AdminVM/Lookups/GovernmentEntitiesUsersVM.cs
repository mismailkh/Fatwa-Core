using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups
{
    public class GovernmentEntitiesUsersVM :TransactionalBaseModel
    {
        [Key]
        public string Id { get; set; }
        public string? SecurityStamp { get; set; }

        [StringLength(256)]
        public string? PasswordHash { get; set; }
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

        public bool? PhoneNumberConfirmed { get; set; } = false;

        public string? AlternatePhoneNumber { get; set; }

        public string? Email { get; set; }

        public string? NormalizedEmail { get; set; }

        [Required]
        public bool EmailConfirmed { get; set; } = false;

        public string? UserName { get; set; }

        public string? NormalizedUserName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public string? Address { get; set; }

        public DateTime? DateOfJoining { get; set; }

        public Guid GroupId { get; set; }

        public int NationalityId { get; set; }

        public int GenderId { get; set; }

        public int UserTypeId { get; set; }

        public int GradeId { get; set; }

        public int DepartmentId { get; set; }

        public int DesignationId { get; set; }

        public string? ManagerId { get; set; }

        public bool IsActive { get; set; } = true;
        public bool AllowAccess { get; set; } = true;
        public bool IsLocked { get; set; } = false;
        public DateTime? AbsentFrom { get; set; }
        public DateTime? AbsentTo { get; set; }
        public string? ReplacementId { get; set; }
        public Guid ProfilePicReferenceGuid { get; set; }
        public int FloorId { get; set; }
    }
}
