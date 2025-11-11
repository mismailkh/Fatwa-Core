using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Enums
{
    public class LiteratureEnum
    {
        public enum BorrowApprovalStatus
        {
            [Display(Name = "Approve")]
            Approved = 1,
            [Display(Name = "Reject")]
            Rejected = 2,
            [Display(Name = "Borrowing Period Expired")]
            BorrowingPeriodExpired = 4,
            [Display(Name = "Pending For Approval")]
            PendingForApproval = 8,
            [Display(Name = "Borrowed")]
            Borrowed = 16,
            [Display(Name = "Return")]
            Returned = 32,
			[Display(Name = "Pending For Extension Approval")]
			PendingForExtensionApproval = 64,
			[Display(Name = "Extended")]
			Extended = 128,
			[Display(Name = "Extension Rejected")]
			ExtensionRejected = 256,
            [Display(Name = "Extending Period Expired")]
            ExtendingPeriodExpired = 512,
        }
        public enum BookStatus
        {
            [Display(Name = "Borrowable")]
            Borrowable = 1,
            [Display(Name = "Reserved")]
            Reserved = 2,
            [Display(Name = "Borrowed")]
            Borrowed = 4,
            [Display(Name = "UnAvailable")]
            UnAvailable = 8,
            [Display(Name = "Draft")]
            Draft = 16,
            //Used for getting data in literature list when book is unavailable (Not borrowable)
            [Display(Name = "Not BorrowAble")]
            NotBorrowAble = 32,
        }
        public enum BorrowReturnApprovalStatus
        {
			[Display(Name = "Default")]
			Default = 1,
			[Display(Name = "Return")]
            Returned = 2,
            [Display(Name = "Reject")]
            Rejected = 4,
            [Display(Name = "Pending For Return Book Approval")]
            PendingForReturnBookApproval = 8
        }

        // used for set sequence order of Dewey Book Number pattern
        public enum OrderSequenceNumber
        {
            First = 1,
            Second = 2
        }
        public enum StockTakingStatusEnum
        {
            New = 1,
            InProgress = 2,
            Completed = 4
        }
        public enum BorrowedLiteratureEvent
        {

            [Display(Name = "Borrow Request Approve")]
            BorrowRequestApproved = 1,
            [Display(Name = "Borrow Request Reject")]
            BorrowRequestRejected = 2,
            [Display(Name = "Borrowing Period Expired")]
            BorrowingPeriodExpired = 4,
            [Display(Name = "Borrow Request Pending For Approval")]
            BorrowRequestPendingForApproval = 8,
            [Display(Name = "Borrowed")]
            Borrowed = 16,
            [Display(Name = "Return")]
            Returned = 32,
            [Display(Name = "Pending For Extension Approval")]
            PendingForExtensionApproval = 64,
            [Display(Name = "Extended")]
            Extended = 128,
            [Display(Name = "Extension Rejected")]
            ExtensionRejected = 256,
            [Display(Name = "Extending Period Expired")]
            ExtendingPeriodExpired = 512,
            [Display(Name = "literature Request Return Reject")]
            LiteratureRequestReturnReject = 1024,
            [Display(Name = "Pending For Return Book Approval")]
            PendingForReturnBookApproval = 2048,
        }
        public enum LmsStockTakingEventEnum
        {

            [Display(Name = "In Progress")]
            InProgress = 1,
            [Display(Name = "Completed")]
            Completed = 2,
            [Display(Name = "Deleted")]
            Deleted = 4,
            
        }
    }
}
