using FATWA_DOMAIN.Models.Notifications.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.Lms
{
    public class UserAndLiteratureVM
    {
        public List<UserDetailsVM> UserDetail { get; set; } = new();
        public List<BorrowedLiteratureVM> Literature { get; set; } = new();

    }
    public class UserDetailsVM
    {
        public string? UserId { get; set; }
        public string? DesignationEn { get; set; }
        public string? DesignationAr { get; set; }
        public string? SectorNameEn { get; set; }
        public int? DepartmentId { get; set; }
        public string? SectorNameAr { get; set; }
        public string? MobileNumber { get; set; }
        public int? EligibleCount { get; set; }
        public string? Extension { get; set; }
        public string? CivilId { get; set; }
        public int? BookReturnDuration { get; set; }    
    }
    public class BorrowedLiteratureVM
    {
        public int? BorrowId { get; set; }
        public string? BookName { get; set; }
        public int? LiteratureId { get; set; }
        public string? AuthorNameEn { get; set; }
        public string? AuthorNameAr { get; set; }
        public string? Barcode { get; set; }
        public int? DecisionId { get; set; }
        public DateTime? BarrowedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime? ExtendDueDate { get; set; }
        public string? BorrowerUserId { get; set; }
        public int? BarcodeId { get; set; }
        public string? ISBN { get; set; }

        public int? BorrowReturnApprovalStatus { get; set; }
        public int? ExtensionApprovalStatus { get; set; }
        public DateTime? ApplyReturnDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public bool? Extended { get; set; }
        [NotMapped]
        public string? LoggedInUser { get; set; }
        [NotMapped]
        public bool IsNew { get; set;}
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();

    }

}
