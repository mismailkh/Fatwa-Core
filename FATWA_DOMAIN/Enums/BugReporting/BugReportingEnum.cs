using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Enums.BugReporting
{
    public class BugReportingEnum
    {
        public enum BugStatusEnum
        {
            New = 1,
            Draft = 2,
            Assigned = 4,
            Closed = 8,
            Resolved = 16,
            InProgress = 32,
            Reopened = 64,
            Cancelled = 128,
            Verified = 256,
            Rejected = 512
        }
        public enum RemarksTypeEnum
        {
            Feedback = 1,
            Comment = 2,
            Reason = 3,
            RejectReason = 4,
            Resolution = 5,
        }
        public enum IssueTypeAssignmentEnum
        {
            Assign = 1,
            UnAssign = 2,
        }
        public enum CommentFeedbackFromTypeEnum
        {
            Ticket = 1,
            Bug = 2,
        }
        public enum AssingmentOptionEnums
        {
            User = 1,
            Group = 2,
        }
        public enum DecisionFromOptionEnum
        {
            FromTicket = 1,
            FromBug = 2,
        }
        public enum IssueTypeEnum
        {
            SystemCrashing = 1,
            DataNotLoading = 2,
            DataNotFound = 3,
            UnableToLogin = 4,
            ButtonNotWorking = 5,
            StatusNotChanging = 6,
            Other = 7,
        }
        public enum SeverityEnum
        {

            Major = 1,
            Minor = 2,
            Critical = 4,
            ShowStopper = 8,
        }
        public enum TicketEvenEnum
        {

            TicketDrafted = 1,
            TicketRaised = 2,
            TicketAssigned = 4,
            TicketAccepted = 8,
            TicketRejected = 16,
            ResolutionAdded = 32,
            TicketClosed = 64,
            TicketReOpened = 128,
            CommentAdded = 256,
            FeedbackAdded = 512,
            BugReported = 1024,
            CommentReplyAdded = 2048,
            CommentDeleted = 4096,
            CommentEdited = 8192
        }
        public enum ApplicationEnums
        {
            FatwaPortal = 1,
            FatwaAdminPortal = 2,
            DocumentManagementSystem = 4,
            G2GPortal = 8,
            G2GAdminPortal = 16,
            OperationsSupportSystem = 32,
        }
        public enum MentionUserEnum
        {
            [Display(Name ="GE_User")]
            GEUser = 1,
            [Display(Name = "IT_Support_Team")]
            FatwaUser = 2,
        }
    }
}
