using FATWA_DOMAIN.Models.Notifications.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel
{
    public class BorrowDetailVM
    {
        public enum SearchViewBy
        {
            //[Display("لا أحد")]
            None = 0,
            //[Display("تاريخ العودة")] 
            ReturnDate = 1,
            //[Display("تمديد التاريخ")] 
            ExtendDate = 2,
            //[Display("تاريخ الاقتراض")] 
            BorrowDate = 3,
            //[Display("تاريخ العودة")] 
            OverdueDate = 4,
            //[Display("Book Name")]  
            BookName = 5,
            UserName = 6,
            IssueDate= 7,
        }
        [Key]
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

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string? ISBN { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? LiteratureName { get; set; }
        public int EligibleCount { get; set; }
        public string? FullName_En { get; set; }
        public string? FullName_Ar { get; set; }
        //public string? TypeName_Ar { get; set; }
        //public string? TypeName_En { get; set; }
        public string? BorrowApprovalStatus { get; set; }
        public string? BorrowApprovalStatusAr { get; set; }
        public int? DecisionId { get; set; }
        public string? BarCodeNumber { get; set; }
        public int? ExtensionApprovalStatus { get; set; }
        [NotMapped]
        public string? Comment { get; set; }
        public string? ReturnApprovalNameEn { get; set; }
        public string? ReturnApprovalNameAr { get; set; }
        public int? ReturnApprovalId { get; set; }
        public DateTime? ApprovalDate { get; set; }

        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
        [NotMapped]
        public string? RoleId { get; set; }
        [NotMapped]
        public string? LoggedInUser { get; set; }
    }

}
