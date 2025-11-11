using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.Lms
{
    public class UserBorrowedHistoryVM
    {
        public int? BorrowId { get; set; }
        public int? LiteratureId { get; set; }
        public string? BorrowerUserId { get; set; }
        public int DecisionId { get; set; }
        public string? Barcode { get; set; }
        public bool? Extended { get; set; }
        public string? BookName { get; set; }
        public string? AuthorNameEn { get; set; }
        public string? AuthorNameAr { get; set; }
        public DateTime? BarrowedDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? ExtendDueDate { get; set; }
        public DateTime? ApplyReturnDate { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? BorrowReturnApprovalStatus { get; set; }
        public int? ExtensionApprovalStatus { get; set; }
        public string? EventEn { get; set; }
        public string? EventAr { get; set; }
    }
}
