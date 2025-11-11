using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.AdminModels.UserManagement
{
    [Table("UMS_USER")]
    public class User : TransactionalBaseModel
    {
        [Key]
        public string Id { get; set; }
        public string? UserName { get; set; }
        public string? NormalizedUserName { get; set; }
        public string? Email { get; set; }
        public string? NormalizedEmail { get; set; }
        [Required]
        public bool? EmailConfirmed { get; set; } = false;
        public string? SecurityStamp { get; set; }
        [StringLength(256)]
        public string? PasswordHash { get; set; }
        public string? ConcurrencyStamp { get; set; }
        [Required]
        public bool TwoFactorEnabled { get; set; } = false;
        public DateTimeOffset? LockoutEnd { get; set; }
        [Required]
        public bool LockoutEnabled { get; set; }
        [Required]
        public int AccessFailedCount { get; set; }
        //For literature Borrow detail
        public int EligibleCount { get; set; }
        public bool IsActive { get; set; } = true;
        public bool AllowAccess { get; set; } = true;
        public bool IsLocked { get; set; } = false;
        [NotMapped]
        public List<string>? RoleIds { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password should be between 8 to 100 characters long")]
        public string? Password { get; set; }
        [NotMapped]
        [Required(ErrorMessage = "Confirm Password is required")]
        [Compare(nameof(Password), ErrorMessage = "Passwords don't match.")]
        public string? ConfirmPassword { get; set; }
        [NotMapped]
        public List<int>? DeletedAttachementIds { get; set; } = new List<int>();
        [NotMapped]
        public IList<ClaimVM>? UserClaims { get; set; }
        public ICollection<UserGroup> GroupUsers { get; set; }
        public ICollection<UserRole> UserRoles { get; set; }
        [NotMapped]
        public int? SectorTypeId { get; set; }
        public string? ADUserName { get; set; }
        public bool IsPasswordReset { get; set; } = false;
        public bool HasSignatureImage { get; set; }
        public bool AllowDigitalSign { get; set; }


    }
}
