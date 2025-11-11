using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FATWA_DOMAIN.Enums
{
    //<History Author = 'Hassan Abbas' Date='2022-07-27' Version="1.0" Branch="master"> General enum class</History>
    public class GeneralEnums
    {
        //<History Author = 'Hassan Abbas' Date='2022-07-27' Version="1.0" Branch="master"> Permissions Select all type enum</History>
        public enum SelectPermissionType
        {
            All = 0,
            Module = 1,
            Submodule = 2,
        }

        public enum AttachmentTypeEnum
        {
            BookDigitalCopy = 1,
            InternationalArbitration = 2,
            CatalogueFile = 3,
            EpProfilePicture = 4,
            BookCoverImage = 5,
            CourtsDecisions = 6,
            LpsLegalAdvice = 7,
            Legislation = 8,
            AuthorityLetter = 9,
            DefendentCivilID = 10,
            Administrative = 11,
            Other = 12,
            Communication = 13,
            CivilId = 14,
            MOCICertificate = 15,
            MOMAttachment = 16,
            SignedMOMAttachment = 17,
            ClaimStatement = 18,
            LpsAppealJudgments = 19,
            LpsSupremeJudgments = 20,
            LpsAdministrativeComplaints = 21,
            KuwaitAlYawm = 23,
            LegalPublications = 24,
            Task = 25,
            TaskResponse = 26,
            ContractReview = 27,
            ExecutionFile = 28,
            CmsLegalNotification = 29,
            MainAuthorityLetter = 30,
            MainDefendentCivilId = 31,
            WithdrawRequest = 32,
            DocumentPortfolio = 33,
            LegalDocument = 34,
            Complaint = 35,
            PublicProsecutionDecision = 36,
            AnouncementPorcecutionGE = 37,
            AnouncementPorcecutionCouncilMinisters = 38,
            StampedDebtStatement = 39,
            AnouncementPorcecutionCOMDescision = 40,
            ContactDocument = 41,
            OpinionDocument = 42,
            OfficialLetter = 43,
            PresentationNotes = 44,
            OpenPleadingRequest = 45,
            InitialJudgementNotification = 46,
            GeneralUpdateNotification = 47,
            CaseRegisteredNotification = 48,
            AdditionalInformationNotification = 49,
            SavingFileNotification = 50,
            AdditionalInformationReminderNotification = 51,
            CaseClosingDocument = 52,
            FinalJudgementNotification = 53,
            HearingDocument = 54,
            DefenseDocument = 55,
            ExecutionAdditionalInformationNotification = 56,
            ExecutionFileOpened = 57,
            PostponeHearingDocument = 58,
            WithdrawConsultationRequest = 59,
            CmsAppealJudgement = 60,
            CmsSupremeJudgement = 61,   
            ComsAdministrativeComplaints = 62,
            ComsLegalAdvice = 63,
            LegalNotificationResponse = 64,
            LpsExternalSource = 70,
            ComsLegisltation = 71,
            comsInternationArbitration =72,
            RequestForMeeting = 73,
            EpCivilId = 74,
            EpEducationalDoc = 75,
            EpWorkExperience = 76,
            EpTraining = 77,
            TenderContract = 78,
            BiddingContract = 79,
            MomarasaContract = 80,
            DirectContract = 81,
            ExtensionorRenewalContract = 82,
            DesignandBuildContract = 83,
            PPPContract = 84,
            ComsLegalNotification= 85,
            ReplytoMeetingRequest = 86,
            RequestingFulfillment = 87,
            MailingList = 88,
            Others = 89,
            OrderOnPetitionNotes = 90,
            PerformOrderNotes = 91,
            Bug = 92,
            OrderOnPetitionCourtDecision = 93,
            PerformOrderCourtDecision = 94,
            LdsExternalSource=95,
            RequestForStopExecutionOfJudgment = 96, 
            MeetingMinutes = 97,
            StopExecutionOfJudgment = 98,
            G2GTarasolCorrespondenceDocument = 99,
            HearingRollDocument = 100,
			AppealJudgmentCopyOriginal = 101,
			CounselJudgment = 102,
			CorrectionJudgment = 103,
			AppealUrgentJudgment = 104,
			JudgmentStatement = 105,
			InitialJudgment = 106,
			SupremeJudgmentCopy = 107,
			SupremeJudgmentCopyOriginal = 108,
			SupremeInitialJudgment = 109,
			CourtJudgment = 110,
			AppealPartialJudgment = 111,
            OthersPrinciple = 112,
            VendorContract = 113,
            ComplaintSR = 114,
            CommitteeAgenda = 115,
            Reports = 116,
            FatwaCircular = 117,
            MeetingMinutesCommittee = 118,
            OthersCommittee = 119,
            OtherVendorContract = 121,
            OtherLeaveAttendance = 122,
            CleanerAttendance = 123,
            SignatureImage = 124,
            StockTakingReport = 125,
            ReturnInventory = 126,
        }

        public enum PriorityEnum
        {
            
            Low = 1,
            
            Medium = 2,
            
            High = 3,
        }

        public enum ApprovalStatusEnum
        {
            Pending = 1,
            Approved = 2,
            Rejected = 4,   
        }

        public enum ApprovalTrackingStatusEnum
        {
            AssignedToAdministrativeUnderFilingSector = 1,
            AssignedToAdministrativeRegionalSector = 2,
            AssignedToAdministrativeAppealSector = 3,
            AssignedToAdministrativeSupremeSector = 4,
            AssignedToCivilCommercialUnderFilingSector = 5,
            AssignedToCivilCommercialRegionalSector = 6,
            AssignedToCivilCommercialAppealSector = 7,
            AssignedToCivilCommercialSupremeSector = 8,
            AssignedToCivilCommercialPartialUrgentSector = 9,
            AssignedToAdministrativeGeneralSupervisor = 10,
            AssignedToCivilCommercialGeneralSupervisor = 11,
            AssignedToExecutionSector = 12,
            AssignedToLegalAdviceSector = 13,
            AssignedToLegislationsSector = 14,
            AssignedToAdministrativeComplaintsSector = 15,
            AssignedToContractsSector = 16,
            AssignedToInternationalArbitrationSector = 17,
            AssignedToPrivateOperationalSector = 18,
            AssignedToPublicOperationalSector = 19,
            AssignedToFatwaPresidentOffice = 20,
            Transfered = 21,
            Rejected = 22,
            RejectedByFatwaPresident = 23,
            Pending = 24
        }

        public enum SubModuleEnum
        {
            CaseRequest = 1,
            CaseFile = 2,
            RegisteredCase = 4,
            ConsultationRequest = 8,
            //16
            //32 has been skipped because we have to match these values with link target type enum 
            ConsultationFile = 64,
            DMSReviewDocument=128,
            OrganizingCommittee = 256,
            LeaveAndAttendance = 512
        }
        public enum LookupsTablesEnum
        {
            CMS_GOVERNMENT_ENTITY_G2G_LKP =1,
            CMS_COURT_G2G_LKP=2,
            CMS_CHAMBER_G2G_LKP=3,
            COMS_CONSULTATION_Legislation_FILE_TYPE_G2G_LKP = 4,
            LEGAL_LEGISLATION_TYPE=5,
            LEGAL_PRINCIPLE_TYPE=6,
            LEGAL_PUBLICATION_SOURCE_NAME=7,
            CMS_CASE_FILE_STATUS_G2G_LKP = 8,
            Department=9,
            CMS_REQUEST_TYPE_G2G_LKP=10,
            COMS_CONSULTATION_INTERNATIONAL_ARBITRATION_TYPE_G2G_LKP = 11,
            LMS_LITERATURE_TAG = 12,
            CMS_SUBTYPE_G2G_LKP = 13,
            CMS_GOVERNMENT_ENTITY_DEPARTMENT_G2G_LKP = 14,
            CMS_OPERATING_SECTOR_TYPE_G2G_LKP = 15

        }
        public enum IntervalTypeEnum
        {
            Define_The_Period_To_Appeal_Case_File = 1,
            Communication_Response_Reminder = 2,
            Define_The_Period_To_Supreme_Case_File = 3,
            Define_The_Period_To_Request_For_Additional_Information = 4,
            Define_The_Period_For_Reminder_Request_For_Addition_Information = 5,
            Define_The_Period_To_Auto_Save_The_File = 6,
            Define_The_Period_To_Register_A_Case_At_MOJ = 7,
            Define_The_Period_To_Complete_The_Claim_Statement = 8,
            Define_The_Period_To_Reply_On_The_Operation_Consultant = 9,
            Define_The_Period_To_Prepare_Defense_Letter = 10,
        }

		public enum WebSystemEnum
		{
			CoreApp = 1,
			ActiveDirectory = 2,
            ActiveTracking = 3
		}
        public enum MobileChannelEnum
        {
            Android = 1,
            Ios = 2,
            Huawei = 3,
        }
        public enum MobileTaskTypeEnum
        {
            CaseManagement = 1,
            Consultation = 2,
            Generic = 3,
        }
        public enum CaseFileTransferRequestStatusEnum
        {
            [Display(Name = "Approved")]
            Approved = 1,
            [Display(Name = "Rejected")]
            Rejected = 2,
            [Display(Name = "Submitted")]
            Submitted = 3,
        }
        public enum RegisteredCaseTransferRequestStatusEnum
        {
            Approved = 1,
            Rejected = 2,
            Submitted = 3,
        }
    }
}
