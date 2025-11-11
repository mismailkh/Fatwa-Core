using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Models.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("LMS_LITERATURE_BORROW_DETAILS")]
    //<History Author = 'Zain Ul Islam' Date='2022-03-23' Version="1.0" Branch="master"> Add many to many relation and allow it should save with relation</History>
    public partial class LmsLiteratureBorrowDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BorrowId { get; set; }
        public int LiteratureId { get; set; }
        public string UserId { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime DueDate { get; set; }
        public bool Extended { get; set; }
        public DateTime? ExtendDueDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime? ApplyReturnDate { get; set; }
        public bool Fine { get; set; }
        [Display(Name = "ISBNMapped")]
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Name_Ar")]
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        [Display(Name = "UserNameMapped")]
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        public int ExtensionApprovalStatus { get; set; }
        public int BorrowApprovalStatus { get; set; }
        public string? Comment { get; set; }
        [NotMapped]
        public string? ISBN { get; set; }
        [NotMapped]
        public string? BarCodeNumber { get; set; }
        [NotMapped]
        public string? UserName { get; set; }
        [NotMapped]
        public string? PhoneNumber { get; set; }
        [NotMapped]
        public string? LiteratureName { get; set; }
        public LmsLiterature? LmsLiterature { get; set; }
        [NotMapped]
        public int EligibleCount { get; set; }
        public int BarcodeId { get; set; }
        [NotMapped]
        public IList<LiteratureDetailsForBorrowRequestVM>? literatureDetailsForBorrowRequestModel { get; set; } = new List<LiteratureDetailsForBorrowRequestVM>();
        public int BorrowReturnApprovalStatus { get; set; }
        public DateTime? ApprovalDate { get; set; }
        [NotMapped]
        public int? BorrowReturnDayDuration { get; set; }

        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
        [NotMapped]
        public string? RoleId { get; set; }
    }
    //<History Author = 'Nadia Gull' Date='2022-11-3' Version="1.0" Branch="master"> Add UserBorrowLiterature VM </History>
    public class UserBorrowLiteratureVM
    {
        [Key]
        public int BorrowId { get; set; }
        public string LiteratureName { get; set; }
        public string? BarCodeNumber { get; set; }
        public DateTime IssueDate { get; set; }
        public DateTime? DueDate { get; set; }
		public string? BorrowApprovalStatus { get; set; }
		public string? BorrowApprovalStatusAr { get; set; }
		public int? DecisionId { get; set; }
	}
}
