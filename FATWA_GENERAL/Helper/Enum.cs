using FATWA_DOMAIN.Enums;
using System.ComponentModel.DataAnnotations;

namespace FATWA_GENERAL.Helper
{
    public class Enum
    {
        public enum Roles
        {
            SuperAdmin = UserEnum.Roles.SuperAdmin,
            Admin = UserEnum.Roles.SuperAdmin,
            Basic = UserEnum.Roles.Basic,
            HOS = UserEnum.Roles.HOS,
            Lawyer = UserEnum.Roles.Lawyer,
            Supervisor = UserEnum.Roles.Supervisor,
            Messenger = UserEnum.Roles.Messenger
        }
        public enum ErrorLogEnum
        {
            Critical = 1,
            Debug = 2,
            Error = 4,
            Information = 8,
            None = 16,
            Trace = 32,
            Warning = 64
        }

        public enum ErrorTypeEnum
        {
            Fatal = 1,
            Critical = 2,
            Minor = 4,
            Warning = 8,
            Harmless = 16
        }

        public enum ProcessLogEnum
        {
            CalculationProcess = 1,
            DependencyProcess = 2,
            InvalidExpression = 4,
            Processed = 8,
            UnProcessed = 16
        }

        public enum ProcessTypeEnum
        {
            CalculationProcess = 1,
            DependencyProcess = 2,
            InvalidExpression = 4,
            Processed = 8,
            UnProcessed = 16
        }

        public enum PortalEnum
        {
            FatwaPortal = 1,
            WorkerServices = 2,
            OSSPortal = 3,

        }

        public enum ModuleEnum
        {
            LegalLibrarySystem = 1,// 
            CaseManagement = 2, //
            ConsultationManagement = 4, //
            InventoryManagement = 8,//
            DocumentManagement = 16,//
            Workflow = 32, //
            EmployeeManagement = 64,//
            CMSCOMS = 128, // for the purpose of the Private Operational Sector, which will handle the Side Menu exclusively for POO. //
            ODRP = 256, //On Demand Portals //
            LibraryManagement = 512,//
            LegislationManagement = 1024,//
            LegalPrinciplesManagement = 2048,//
            ContactManagement = 4096,
            UserManagement = 8192,//
            Meeting = 16384,
            Communication = 32768,
            Notification = 65536,//
            Task = 131072, //
            AuditLogs = 262144, //
            TimeLog = 524288,//
            FatwaDashboard = 1048576, //
            SideMenu = 2097152, //
            VendorContractManagement = 4194304,
            BugReporting = 8388608,
            OrganizingCommittee = 16777216,
            ServiceRequest = 33554432,
            DigitalSignature = 67108864,
            PlanningAndTraining = 134217728,
            ArchivedCases = 536870912,
            AdminPortal= 1073741824,
        }

        public enum ProjectEnum
        {
            FATWA_WEB,
            FATWA_ADMIN,
            OSS_WEB
        }

        #region Lms

        public enum BorrowExtensionApprovalStatus
        {
            Approve = 1,
            Reject = 2
        }

        #endregion

        #region CMS (Case Mgt)

        public enum CMSResponseType
        {
            [Display(Name = "Draft")]
            Draft = 1,
            [Display(Name = "Submitted")]
            Submitted = 2,
            [Display(Name = "ReSubmitted")]
            ReSubmitted = 4,
            [Display(Name = "In Review")]
            InReview = 8,
            [Display(Name = "Withdraw")]
            Withdraw = 16,
            [Display(Name = "On Hold")]
            OnHold = 32,
            [Display(Name = "Rejected")]
            Rejected = 64,
            [Display(Name = "Approved")]
            Approved = 128,
            [Display(Name = "Cancelled")]
            Cancelled = 256,
            [Display(Name = "Other")]
            Other = 512,
        }

        #endregion

        #region Print
        public enum PrintCommandEnum
        {
            Character = 1,
            Character82 = 2,
            Barcode = 3,
            BarcodeAll = 4,
        }
        #endregion

        #region Notification

        public enum NotificationChannelEnum
        {
            [Display(Name = "Email")]
            Email = 1,
            [Display(Name = "Mobile")]
            Mobile = 2,
            [Display(Name = "Browser")]
            Browser = 4,
        }

        public enum NotificationCategoryEnum
        {
            [Display(Name = "Urgent")]
            Urgent = 1,
            [Display(Name = "Normal")]
            Normal = 2,
            [Display(Name = "Important")]
            Important = 4,
            [Display(Name = "Do Not Reply")]
            DontReply = 8,
        }

        public enum NotificationEventEnum
        {
            [Display(Name = "New Case Request")]
            NewRequest = 1,
            [Display(Name = "Assign To Lawyer")]
            AssignToLawyer = 2,
            [Display(Name = "Share Document")]
            ShareDocument = 3,
            [Display(Name = "Receive Legal Notification")]
            ReceiveLegalNotification = 4,
            [Display(Name = "Case Registered")]
            CaseRegistered = 5,
            [Display(Name = "Open a File")]
            OpenFile = 6,
            [Display(Name = "Create Execution Request")]
            CreateExecutionRequest = 7,
            [Display(Name = "Send Execution Request To MOJ Execution")]
            SendExecutionRequestToMojExecution = 8,
            [Display(Name = "Approve Execution Request")]
            ApproveExecutionRequest = 9,
            [Display(Name = "Reject Execution Request")]
            RejectExecutionRequest = 10,
            [Display(Name = "Submit Request")]
            SubmitRequest = 11,
            [Display(Name = "Send A Copy Review")]
            SendACopyReview = 12,
            [Display(Name = "Send A Copy Approved")]
            SendACopyApproved = 13,
            [Display(Name = "Transfer Of Sector")]
            TransferOfSector = 14,
            [Display(Name = "Assign Back To Hos")]
            AssignBackToHos = 15,
            [Display(Name = "Request For Meeting")]
            RequestForMeeting = 16,
            [Display(Name = "Create Merge Request")]
            CreateMergeRequest = 17,
            [Display(Name = "Approve Merge Request")]
            ApproveMergeRequest = 18,
            [Display(Name = "New Consultation Request")]
            NewConsultationRequest = 19,
            [Display(Name = "Add Judgement")]
            AddJudgement = 20,
            [Display(Name = "Attendee Decision For Meeting")]
            AttendeeDecisionForMeeting = 21,
            [Display(Name = "Add Judgment Execution")]
            AddJudgmentExecution = 22,
            [Display(Name = "Review Principle")]
            ReviewPrinciple = 23,
            [Display(Name = "Modify Draft")]
            ModifyDraft = 24,
            [Display(Name = "Review Draft")]
            ReviewDraft = 25,
            [Display(Name = "Published Draft")]
            PublishedDraft = 26,
            [Display(Name = "Modify Document")]
            ModifyDocument = 27,
            [Display(Name = "Review Document")]
            ReviewDocument = 28,
            [Display(Name = "Published Document")]
            PublishedDocument = 29,
            [Display(Name = "Add Contact")]
            AddContact = 30,
            [Display(Name = "Send to MOJ")]
            SendToMOJ = 31,
            [Display(Name = "Case Data Pushed From RPA")]
            CaseDataPushedFromRPA = 32,
            [Display(Name = "Delete Attendee From Meeeting")]
            DeleteAttendeeFromMeeting = 33,
            [Display(Name = "Attendee Reject Meeeting Invite")]
            AttendeeAcceptMeetingInvite = 34,
            [Display(Name = "Attendee Accept Meeeting Invite")]
            AttendeeRejectMeetingInvite = 35,
            [Display(Name = "Meeeting Decision of HOS For Approval")]
            MeeetingDecisionOfHOSForApproval = 36,
            [Display(Name = "Save Legal Legislation")]
            SaveLegalLegislation = 37,
            [Display(Name = "Update Legal Legislation")]
            UpdateLegalLegislation = 38,
            [Display(Name = "Soft Delete Legal Legislation")]
            SoftDeleteLegalLegislation = 39,
            [Display(Name = "Add Meeting Success")]
            AddMeetingSuccess = 40,
            [Display(Name = "Edit Meeting Success")]
            EditMeetingSuccess = 41,
            [Display(Name = "Revoke Delete Legal Legislation")]
            RevokeDeleteLegalLegislation = 42,
            [Display(Name = "Add MOM Of Meeting")]
            AddMOMOfMeeting = 43,
            [Display(Name = "Edit MOM Of Meeting")]
            EditMOMOfMeeting = 44,
            [Display(Name = "Save Legal Principle")]
            SaveLegalPrinciple = 45,
            [Display(Name = "Update Legal Principle")]
            UpdateLegalPrinciple = 46,
            [Display(Name = "Soft Delete Legal Principle")]
            SoftDeleteLegalPrinciple = 47,
            [Display(Name = "Revoke Delete Legal Principle")]
            RevokeDeleteLegalPrinciple = 48,
            [Display(Name = "GE Reject Meeeting Invite")]
            GERejectMeeetingInvite = 49,
            [Display(Name = "GE Accept Meeeting Invite")]
            GEAcceptMeeetingInvite = 50,
            [Display(Name = "MOM Created Successfully")]
            MOMCreatedSuccessfully = 51,
            [Display(Name = "Assig/Unassigned Cases to Sector")]
            AssigUnassignedCasestoSector = 52,
            [Display(Name = "ClaimStatement & Defense Letter Reminder")]
            ClaimStatementandDefenseLetterReminder = 53,
            [Display(Name = "Assign To MOJ Reminder")]
            AssignToMOJReminder = 54,
            //[Display(Name = "Communication Response Reminder")]
            //CommunicationResponseReminder = 55,
            [Display(Name = "Opinion Notes Reminder")]
            OpinionNotesReminder = 56,
            [Display(Name = "Draft Modification Reminder")]
            DraftModificationReminder = 57,
            [Display(Name = "Case Close Reminder")]
            CaseCloseReminder = 58,
            [Display(Name = "Legal Notification Reminder")]
            LegalNotificationReminder = 59,
            //[Display(Name = "HOS Supreme Reminder")]
            //HOSSupremeReminder = 60,
            [Display(Name = "Legal Notification Response Reminder")]
            LegalNotificationResponseReminder = 61,
            [Display(Name = "Review Draft Reminder")]
            ReviewDraftReminder = 62,
            [Display(Name = "Delete Parent Index Literature")]
            DeleteParentIndexLiterature = 63,
            [Display(Name = "Add Literature")]
            AddLiterature = 64,
            [Display(Name = "Delete Literature")]
            DeleteLiterature = 65,
            [Display(Name = "Assign to Sector")]
            AssigntoSector = 66,
            [Display(Name = "Hearing Data Pushed From RPA")]
            HearingDataPushedFromRPA = 67,
            [Display(Name = "Reject To Accept Assign File")]
            RejectToAcceptAssignFile = 68,
            [Display(Name = "Add Sub Case")]
            AddSubCase = 69,
            [Display(Name = "Create Lms Literature Borrow Request")]
            CreateLmsLiteratureBorrowRequest = 70,
            [Display(Name = "Received Lms Literature Borrow Request")]
            ReceivedLmsLiteratureBorrowRequest = 71,
            [Display(Name = "Lms Literature Borrow Request For Return")]
            LmsLiteratureBorrowRequestForReturn = 72,
            [Display(Name = "Update Lms Literature Retun")]
            UpdateLmsLiteratureRetun = 73,
            [Display(Name = "Lms Literature Borrow Request For Approval")]
            LmsLiteratureBorrowRequestForApproval = 74,
            [Display(Name = "Lms Literature Borrow Request For Rejection")]
            LmsLiteratureBorrowRequestForRejection = 75,
            [Display(Name = "Lms Literature Borrow Request Approved")]
            LmsLiteratureBorrowRequestApproved = 76,
            [Display(Name = "Lms Literature Borrow Request Rejected")]
            LmsLiteratureBorrowRequestRejected = 77,
            [Display(Name = "Lms Literature Borrow Request For Extension")]
            LmsLiteratureBorrowRequestForExtension = 78,
            [Display(Name = "Review Legislation")]
            ReviewLegislation = 82,
            [Display(Name = "Bug Ticket Added")]
            SaveBugTicket = 83,
            [Display(Name = "Correspondence Forward To Lawyer")]
            CorrespondenceForwardToLawyer = 84,
            [Display(Name = "Correspondence Send Back To HOS")]
            CorrespondenceSendBackToHOS = 85,
            [Display(Name = "Correspondence Forward To Sector")]
            CorrespondenceForwardToSector = 86,
            [Display(Name = "Receive From Tarassol")]
            ReceiveFromTarassol = 87,
            //[Display(Name = "Create Organizing Committee")]
            //ReceiveFromOrganizingCommittee = 88,
            [Display(Name = "Comment Added")]
            AddComment = 88,
            [Display(Name = "Feedback Added")]
            AddFeedback = 89,
            [Display(Name = "Assign Ticket")]
            AssignTicket = 90,
            [Display(Name = "Reject Ticket")]
            RejectTicket = 91,
            [Display(Name = "Resolve Ticket")]
            ResolveTicket = 92,
            [Display(Name = "ReOpen Ticket")]
            ReOpenTicket = 93,
            [Display(Name = "Close Ticket")]
            CloseTicket = 94,
            [Display(Name = "Delete Request")]
            DeleteRequest = 97,
            [Display(Name = "Return Request")]
            ReturnRequest = 98,
            [Display(Name = "Submit Service Request")]
            SubmitServiceRequest = 99,
            [Display(Name = "Decision Service Request")]
            DecisionServiceRequest = 100,
            [Display(Name = "Rejected Service Request")]
            RejectedServiceRequest = 101,
            [Display(Name = "Add Member Task")]
            AddMemberTask = 102,
            [Display(Name = "Service Request Need Modification")]
            ServiceRequestNeedModification = 105,
            [Display(Name = "Service Request Approved By HOS")]
            ServiceRequestApproved = 106,
            [Display(Name = "Service Request Rejected By LeaveAndDuty Department")]
            ServiceRequestRejectedByLeaveAndDutyDepartment = 107,
            [Display(Name = "Service Request Rejected By Administrative Affair")]
            ServiceRequestRejectedByAdministrativeAffair = 108,
            [Display(Name = "Request Approved By LeaveAndDuty Dept")]
            RequestApprovedByLeaveAndDutyDept = 109,
            [Display(Name = "Request Approved By LeaveAndDuty Dept")]
            RequestApprovedByAdministrativeAffair = 110,
            [Display(Name = "Leave Request submitted")]
            LeaveRequestSubmitted = 111,
            [Display(Name = "Leave Request Rejected")]
            LeaveRequestRejected = 112,
            [Display(Name = "Service Request Resubmitted")]
            ServiceRequestResubmitted = 113,
            [Display(Name = "Service Permission Request Handover To User")]
            ServicePermissionRequestHandoverToUser = 114,
            [Display(Name = "FingerPrint Exemption Request submitted")]
            FingerPrintExemptionRequestSubmitted = 115,
            [Display(Name = "FingerPrint Exemption Request Rejected")]
            FingerPrintExemptionRequestRejected = 116,
            [Display(Name = "Permission Request Submitted")]
            PermissionRequestSubmitted = 117,
            [Display(Name = "Permission Request Rejected")]
            PermissionRequestRejected = 118,
            [Display(Name = "Medical Appointment Request Submitted")]
            MedicalAppointmentRequestSubmitted = 119,
            [Display(Name = "Medical Appointment Request Rejected")]
            MedicalAppointmentRequestRejected = 120,
            [Display(Name = "Leave Request Approved")]
            LeaveRequestApproved = 121,
            [Display(Name = "Medical Appointment Request Need Modification")]
            MedicalAppointmentRequestNeedModification = 122,
            [Display(Name = "Leave Request Need Modification")]
            LeaveRequestNeedModification = 123,
            [Display(Name = "Lms Literature Borrow Request Returned")]
            LmsLiteratureBorrowRequestReturned = 124,
            [Display(Name = "Lms Literature Borrow Request For Extended")]
            LmsLiteratureBorrowRequestForExtended = 125,
            [Display(Name = "Lms Literature Borrow Extension Request Approved")]
            LmsLiteratureBorrowExtensionRequestApproved = 127,
            [Display(Name = "Lms Literature Borrow Extension Request Rejected")]
            LmsLiteratureBorrowExtensionRequestRejected = 128,
            [Display(Name = "Request for Transfer to Sector")]
            RequestForTransferToSector = 126,
            [Display(Name = "Reject To Accept Transfer Request")]
            RejectToAcceptTransferRequest = 129,
            [Display(Name = "Approved Request For Transfer")]
            ApprovedRequestForTransfer = 130,
            [Display(Name = "Decision Form Re-Uploaded")]
            DecisionFormReUploaded = 131,
            [Display(Name = "Document Send For Signing")]
            DocumentSendForSigning = 133,
            [Display(Name = "Document Receive After Signing")]
            DocumentReceiveAfterSigning = 134,
            [Display(Name = "Registered Case Transfer Request")]
            RegisteredCaseTransferRequest = 132,
            [Display(Name = "Reject To Accept Case Transfer Request")]
            RejectToAcceptCaseTransferRequest = 135,
            [Display(Name = "Approved Case Transfer Request")]
            ApprovedCaseTransferRequest = 136,
            [Display(Name = "Case Close reminder for Appeal/Supreme")]
            CaseClosereminderforAppealSupreme = 137,
            [Display(Name = "Add Store")]
            AddStore = 138,
            [Display(Name = "Add Items")]
            AddItems = 139,
            [Display(Name = "Opinion Notes Reminder For Manager")]
            OpinionNotesReminderForManager = 140,
            [Display(Name = "ClaimStatement & Defense Letter Reminder For Manager")]
            ClaimStatementandDefenseLetterReminderForManager = 141,
            [Display(Name = "Legal Notification Response Reminder For Manager")]
            LegalNotificationResponseReminderForManager = 142,
            [Display(Name = "Legal Notification Reminder For Manager")]
            LegalNotificationReminderForManager = 143,
            [Display(Name = "Review Draft Reminder For Manager")]
            ReviewDraftReminderForManager = 144,
            [Display(Name = "Draft Modification Reminder For Manager")]
            DraftModificationReminderForManager = 145,
            [Display(Name = "Assign To MOJ Reminder For Manager")]
            AssignToMOJReminderForManager = 146,
            [Display(Name = "Transfer Items")]
            TransferItems = 147,
            [Display(Name = "Approve Transfer Items")]
            ApproveTransferItems = 148,
            [Display(Name = "Reject Transfer Items")]
            RejectTransferItems = 149,
            [Display(Name = "Final Judgement Issued")]
            FinalJudgementIssued = 150,
            [Display(Name = "Return Item Request")]
            ReturnItemRequest = 151,
            [Display(Name = "Pending Task Reminder For Manager")]
            PendingTaskReminderForManager = 165,
            [Display(Name = "Comment Reply Added")]
            AddCommentReply = 187,
            [Display(Name = "G2G Comment Added")]
            AddG2GComment = 188,
            [Display(Name = "G2G Comment Reply Added")]
            AddG2GCommentReply = 189,
            [Display(Name = "G2G Bug Ticket Added")]
            G2GSaveBugTicket = 192,
            [Display(Name = "G2G Feedback Added")]
            G2GAddFeedback = 193,
            [Display(Name = "G2G ReOpen Ticket")]
            G2GReOpenTicket = 197,
            [Display(Name = "G2GCloseTicket")]
            G2GCloseTicket = 198,
            [Display(Name = "Mention User Notification")]
            MentionUserNotification = 203,
            [Display(Name = "G2G Mention User Notification")]
            G2GMentionUserNotification = 204,
            [Display(Name = "Adding A New Employee")]
            AddingNewEmployee = 205
        }

        public enum NotificationTypeEnum
        {
            [Display(Name = "Synchronous")]
            Synchronous = 1,
            [Display(Name = "Asynchronous")]
            Asynchronous = 2
        }

        public enum NotificationStatusEnum
        {
            [Display(Name = "Read")]
            Read = 1,
            [Display(Name = "Unread")]
            Unread = 2,
            [Display(Name = "Sent")]
            Sent = 4,
            [Display(Name = "Received")]
            Received = 8
        }

        public enum NotificationReceiverTypeEnum
        {
            [Display(Name = "User")]
            User = 1,
            [Display(Name = "Role")]
            Role = 2,
            [Display(Name = "Group")]
            Group = 4,
        }

        public enum NotificationPlaceholderEnum
        {
            [Display(Name = "#Request Number#")]
            RequestNumber = 1,
            [Display(Name = "#File Number#")]
            FileNumber = 2,
            [Display(Name = "#Case Number#")]
            CaseNumber = 3,
            [Display(Name = "#Sender Name#")]
            SenderName = 4,
            [Display(Name = "#Receiver Name#")]
            ReceiverName = 5,
            [Display(Name = "#Created Date#")]
            CreatedDate = 6,
            [Display(Name = "#Entity#")]
            Entity = 7,
            [Display(Name = "#Document Name#")]
            DocumentName = 8,
            [Display(Name = "#Reference Number#")]
            ReferenceNumber = 9,
            [Display(Name = "#Sector From#")]
            SectorFrom = 10,
            [Display(Name = "#Sector To#")]
            SectorTo = 11,
            [Display(Name = "#Type#")]
            Type = 12,
            [Display(Name = "#Status#")]
            Status = 13,
            [Display(Name = "#Request Type#")]
            RequestType = 14,
            [Display(Name = "#Legislation Number#")]
            LegislationNumber = 15,
            [Display(Name = "#Primary Case Number#")]
            PrimaryCaseNumber = 16,
            [Display(Name = "#CAN Number#")]
            CANNumber = 17,
            [Display(Name = "#Name#")]
            Name = 18,
            [Display(Name = "#Principle Number#")]
            PrincipleNumber = 19,
            [Display(Name = "#Duration#")]
            Duration = 20,
            [Display(Name = "#Correspondence Number#")]
            CorrespondenceNumber = 21,
            [Display(Name = "#GE Name#")]
            GEName = 22,
            [Display(Name = "#Updated Date#")]
            UpdatedDate = 23,
            [Display(Name = "#Document Number#")]
            DocumentNumber = 24,
            [Display(Name = "#Draft Number#")]
            DraftNumber = 25,
            [Display(Name = "#Draft Name#")]
            DraftName = 26,
            ServiceRequestTitle = 27,
            [Display(Name = "#Service Request Number#")]
            ServiceRequestNumber = 28,
            [Display(Name = "#Start Date#")]
            StartDate = 29,
            [Display(Name = "#End Date#")]
            EndDate = 30,
            [Display(Name = "#Start Time#")]
            StartTime = 31,
            [Display(Name = "#End Time#")]
            EndTime = 32,
            [Display(Name = "#Permission Date#")]
            PermissionDate = 33,
            [Display(Name = "#Reviewer Name#")]
            ReviewerName = 34,
            [Display(Name = "#Store Incharge#")]
            StoreIncharge = 35,
            [Display(Name = "#Assignee NameEn#")]
            AssigneNameEn = 36,
            [Display(Name = "#Assignee NameAr#")]
            AssigneNameAr = 37,
            [Display(Name = "#Assignor NameEn#")]
            AssigorNameEn = 38,
            [Display(Name = "#Assignor NameAr#")]
            AssigorNameAr = 39,
            [Display(Name = "#SubjectEn#")]
            SubjectEn = 40,
            [Display(Name = "#SubjectAr#")]
            SubjectAr = 41,
            [Display(Name = "#Employee Name#")]
            EmployeeName = 42,
        }

        #endregion

        #region PACI
        public enum RequirdDataEnum
        {
            [Display(Name = "Residential_Address")]
            Residential = 1,
            [Display(Name = "Work_address")]
            WorkAddress = 2,
            [Display(Name = "Others")]
            Others = 3,

        }
        public enum DataEnum
        {
            Cases = 0,
            UnderFilling = 1

        }
        public enum EmailStatusEnum
        {
            [Display(Name = "Sent")]
            Sent = 1,
            [Display(Name = "Not Sent")]
            NotSent = 2,

        }
        public enum RequestStatusEnum
        {
            [Display(Name = "Request Submitted")]
            RequestSubmitted = 1,
            [Display(Name = "Request Picked by RPA")]
            RequestPickedbyRPA = 2,
            [Display(Name = "Request Form generated")]
            RequestFormgenerated = 3,
            [Display(Name = "RequestFormEmailed")]
            RequestFormEmailed = 4,
            [Display(Name = "PACI Response Received")]
            PACIResponseReceived = 5,
            [Display(Name = "PDF Data Extraction Complete")]
            PDFDataExtraction = 6,
            [Display(Name = "Exception")]
            Exception = 7,
            [Display(Name = "Retry Exception")]
            RetryException = 8,

        }
        #endregion

        #region MOJ Rolls
        public enum MOJRollsRequestStatusEnum
        {
            [Display(Name = "Pending")]
            Pending = 1,
            [Display(Name = "Completede")]
            Completed = 2,
            [Display(Name = "Exception")]
            Exception = 3,
            [Display(Name = "Retry Extraction")]
            RetryExtraction = 4,
            [Display(Name = "Under Process")]
            UnderProcess = 5
        }
        public enum MOJRollsEnum
        {
            [Display(Name = "Regional")]
            Regional = 3,
            [Display(Name = "Appeal")]
            Appeal = 10,
            [Display(Name = "Supreme")]
            Supreme = 21,

        }
        #endregion

    }
}
