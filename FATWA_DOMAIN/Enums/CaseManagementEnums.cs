using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Enums
{
    //<History Author = 'Hassan Abbas' Date='2022-09-30' Version="1.0" Branch="master"> Case Management Enums</History>
    public class CaseManagementEnums
    {
        public enum CasePrioritiesEnum
        {
            Low = 1,
            Medium = 2,
            High = 3,
        }
        public enum CasePartyCategoryEnum
        {
            Defendant = 1,
            Plaintiff = 2,
            ThirdParty = 3
        }
        public enum CasePartyTypeEnum
        {
            Individual = 1,
            Company = 2,
            GovernmentEntity = 3
        }

        public enum CaseRequestStatusEnum
        {
            Draft = 1,
            Submitted = 2,
            Resubmitted = 4,
            ConvertedToFile = 8,
            Rejected = 16,
            WithdrawRequested = 32,
            WithdrawnByGE = 64,
            Archived = 128,
            RegisteredInMOJ = 256,
            AssignedToRegionalSector = 512,
            PendingForGEResponse = 1024,
            AssignedToContractSector = 2048,
            AssignedToLegalAdviceSector = 4096,
            AssignedToLegislationSector = 8192,
            AssignedToInternationalArbitrationSector = 16384,
            AssignedToAdministrativeComplaintsSector = 32768,
        }

        public enum RequestTypeEnum
        {
            Administrative = 1,
            CivilCommercial = 2,
            LegalAdvice = 4,
            Legislations = 8,
            AdministrativeComplaints = 16,
            Contracts = 32,
            InternationalArbitration = 64,
        }



        public enum OperatingSectorTypeEnum
        {
            AdministrativeUnderFilingCases = 1,
            AdministrativeRegionalCases = 2,
            AdministrativeAppealCases = 3,
            AdministrativeSupremeCases = 4,
            CivilCommercialUnderFilingCases = 5,
            CivilCommercialRegionalCases = 6,
            CivilCommercialAppealCases = 7,
            CivilCommercialSupremeCases = 8,
            CivilCommercialPartialUrgentCases = 9,
            AdministrativeGeneralSupervisor = 10,
            CivilCommercialGeneralSupervisor = 11,
            Execution = 12,
            LegalAdvice = 13,
            Legislations = 14,
            AdministrativeComplaints = 15,
            Contracts = 16,
            InternationalArbitration = 17,
            PrivateOperationalSector = 18,
            PublicOperationalSector = 19,
            FatwaPresidentOffice = 20,
            AssistantUndersecretaryforFinancialandAdministrativeAffairs = 21,
            HRDepartment = 22,
            GeneralServices = 23,
            InformationTechnology = 24,
            ConsultationGeneralSupervisor = 25,
            ServicesDepartment = 26,
            MaintenanceANDEngineeringAffairDepartment = 27,
            PublicRecordDepartment = 28,
            TechnicalSupportController = 29,
            SystemDevelopmentController = 30,
            OperationController = 31,
            DatabaseDepartment = 32,
            TechnicalDevelopmentDapartment = 33,
            ProjectPlanningDepartment = 34,
            ProjectExecutionAndFollowUpDapartment = 35,
            OperationExecutaionANDPlanningDepartment = 36,
            OperationControllAndDocumentProcessingDeaprtment = 37,
            AdministrativeAffairsDepartment=51,
            EmploymentAffairsDepartment=52,
            AdministrativeServicesDepartment=53,
            LeaveAndDutyDepartment=54,
            LegalCulturalCenter = 55
        }
        public enum RequestSubTypeEnum
        {
            Lawsuit = 5,
            ComplaintAgainstDecision = 6,
            PerformOrderRequest = 7,
            OrderOnPetitionRequest = 8,
            Momarasa = 10,
            Tender = 11,
            Bidding = 12,
            DirectContract = 13,
            ExtensionorRenewal = 14,
            DesignandBuild = 15,
            PPP = 16
        }

        public enum CaseRequestEventEnum
        {
            Created = 1,
            Edited = 2,
            Withdrawn = 3,
            Transfer = 4,
            AssignToLawyer = 5,
            SentCopy = 6,
            ReceivedCopy = 7,
            Linked = 8,
            RegisteredInMOJ = 9,
            LegalNotificationSend = 10,
            ReceiveCommunication = 11,
            Withdraw = 12,
            WithdrawRejected = 13,
        }

        public enum CaseFileEventEnum
        {
            Created = 1,
            Edited = 2,
            SentToMojTeam = 4,
            RegisteredAtMoj = 8,
            Linked = 16,
            Transfer = 32,
            SentCopy = 64,
            ReceivedCopy = 128,
            AssignToLawyer = 256,
            AssignedBackToHos = 512,
            AssignToSector = 1024,
            Withdraw = 2048,
            Withdrawn = 4096,
            WithdrawRejected = 8192,
            SaveAndClose = 16384,
            MigratedFromMOJ = 32768,
            AcceptedByLawyer = 65536,
        }

        public enum RegisteredCaseEventEnum
        {
            Created = 1,
            Edited = 2,
            Withdrawn = 3,
            CaseTransferedToAnotherChamber = 4,
            AssignToLawyer = 5,
            Linked = 6,
            Closed = 7,
            HearingScheduled = 8,
            HearingAttended = 9,
            HearingCancelled = 10,
            OutcomeAdded = 11,
            JudgementAdded = 12,
            ExecutionRequestSent = 13,
            ExecutionAdded = 14,
            FinalJudgementAdded = 15,
            MigratedFromMOJ = 16,
        }

        public enum CaseTemplateEnum
        {
            NoTemplate = 1,
            BlankTemplate = 2,
            Footer = 3,
            HeaderEn = 4,
            HeaderAr = 5,
            ContractReviewRequest = 6
        }
        public enum CaseTemplateParamsEnum
        {
            CmsTempOutboxDate = 1,
            CmsTempOutboxNumber = 2,
            CmsTempName = 3,
            CmsTempCaseNumber = 4,
            CmsTempStartDateforLawsuit = 5,
            CmsTempPlaintiffName = 6,
            CmsTempDefendantName = 7,
            CmsTempChamberName = 8,
            CmsTempHearingDate = 9,
            CmsTempLawywerName = 10,
            CmsTempPreviousHearingDate = 11,
            CmsTempGovernmentEntityName = 12,
            CmsTempExpertReportNumber = 13,
            CmsTempExpertReportDate = 14,
            CmsTempPostponedHearingDate = 15,
            CmsTempExpectedHearingDate = 16,
            CmsTempCourtName = 17,
            CmsTempNoTemplateContent = 18,
            CmsTempAdditionalSectionContent = 19,
            CmsTempAttachments = 20,
            ComsTempContractAmount = 21,
            ComsTempContractDuration = 22,
            ComsTempContractName = 23,
            ComsTempContractContent = 24,
            ComsTempParty1Name = 25,
            ComsTempParty2Name = 26,
            ComsTempFineAmount = 27,
            ComsTempTitle = 28,
            ComsTempIntroduction = 29,
            ComsTempParty = 30,
            ComsTempArticle = 31,
            ComsTempOutboxDate = 32,
            ComsTempOutboxNumber = 33,
            ComsTempLawywerName = 34,
            CmsTempChamberNumber = 35,
            CmsTempRepresentativeName = 36,
        }


        public enum CaseTemplateSectionEnum
        {
            NoTemplateSection = 1,
            AdditionalSection = 2,
            OpeningStatement = 3,
            Body = 4,
            ClosingStatement = 5,
            ContractTitle = 6,
            ContractIntroduction = 7,
            ContractParty = 8,
            ContractArticle = 9,
        }

        public enum CaseDraftDocumentStatusEnum
        {
            InReview = 1,
            NeedModification = 2,
            Reject = 4,
            ApproveBySupervisor = 8,
            ApproveByHOS = 16,
            Draft = 32,
            SendToMOJ = 64,
            RegisteredInMOJ = 128,
            RejectedBySupervisor = 256,
            RejectedByHOS = 512,
            ApproveByGS = 1024,
            RejectedByGS = 2048,
            ApprovedByPOO = 4096,
            RejectedByPOO = 8196,
            ApproveByLawyer = 16384,
            RejectedByLawyer = 32768
        }
        public enum DraftVersionStatusEnum
        {
            InReview = 1,
            Draft = 2,
            Reject = 4,
            Approve = 8,
            Published = 16,
            SendToMOJ = 32,
            RegisteredInMOJ = 64,
        }

        public enum BlankTemplateSectionPositionEnum
        {
            Before = 1,
            After = 2
        }

        public enum MergeRequestStatusEnum
        {
            OnHold = 1,
            SentForApproval = 2,
            Approved = 3,
            Rejected = 4
        }

        public enum HearingStatusEnum
        {
            HearingScheduled = 1,
            HearingCancelled = 2,
            HearingAttended = 4,
            HearingAdded = 8,
        }

        public enum AssignCaseToLawyerTypeEnum
        {
            CaseRequest = 1,
            CaseFile = 2,
            RegisteredCase = 3,
            ConsultationRequest = 4,
            ConsultationFile = 5,


        }

        public enum CaseFileStatusEnum
        {
            AssignToLawyer = 1,
            WithdrawRequested = 2,
            WithdrawnByGE = 4,
            Archived = 8,
            InProgress = 16,
            PendingForGEResponse = 32,
            PendingForRegistrationAtMoj = 64,
            RegisteredInMoj = 128,
            AssignedToRegionalSector = 256,
            RejectedByLawyer = 512,
            //1024  Anyone can use this ID
            AssignedToContractSector = 2048,
            AssignedToLegalAdviceSector = 4096,
            AssignedToLegislationSector = 8192,
            AssignedToInternationalArbitrationSector = 16384,
            AssignedToAdministrativeComplaintsSector = 32768,
            Rejected = 65536,
            SaveAndCloseCaseFile = 131072,
            FileIsClosed = 262144,
            AssignedToPartialUrgentSector = 524288
        }

        public enum RegisteredCaseStatusEnum
        {
            Open = 1,
            JudgementReceived = 2,
            HearingAttended = 4,
            Closed = 8,
            PendingForGEResponse = 16
        }

        public enum JudgementTypeEnum
        {
            AgainstTheState = 1,
            InFavorOfState = 2,
            ByVirtueOfPartOfTheRequests = 4,
            JudgementInConfrontation = 8,
            ByVirtueOfLackOfJurisdication = 16,
            ByVirtueOfLackOfSpecialJurisdication = 32,
            ByVirtueOfMyLackOfJurisdication = 64,
        }
        public enum SaveAndCloseCaseFileEnum
        {
            NeedMoreInformation = 1,
            SaveAndCloseCaseFile = 2,
            SaveAndCloseConsultationFile = 4

        }

        public enum ApprovalProcessTypeEnum
        {
            Transfer = 1,
            SendaCopy = 2,
            FileAssignment = 4,
            ExecutionRequest = 8,
        }

        public enum DraftEntityTypeEnum
        {
            RequestNeedMoreInfo = 1,
            FileNeedMoreInfo = 2,
            CaseFile = 4,
            Case = 8,
            SubCase = 16,
            LegalNotification = 32,
            RequestForDocument = 64,
            PostPoneHearing = 128,
            SchedulingCourtVisit = 256,
            HearingSchedulingCourtVisit = 256,
            ExecutionFile = 512,
            HearingDocument = 1024,
            ConsultationFile = 2048,
            CaseNeedMoreInfo = 4096,
            StopExecutionOfJudgment = 8192
        }

        public enum CourtTypeEnum
        {
            Regional = 1,
            Appeal = 2,
            Supreme = 4,
            PartialUrgent = 8,
        }

        public enum WithdrawRequestStatusEnum
        {
            WithdrawRequested = 1,
            WithdrawnByGE = 2,
            Rejected = 4,
        }
        public enum CaseManagementModuleEnum
        {
            CaseRequest = 1,
            CaseFile = 2,
            RegisteredCase = 4,
        }
        public enum CaseScreensEnum
        {
            ListRequest = 1,
            ListUnderFilePendingRequests = 2,
            ListUnderFilePendingCaseFiles = 4,
            ListCaseFilePendingRequests = 8,
            ListCaseFilePendingCaseFiles = 16
        }
        public enum CaseDecisionTypeEnum
        {
            InterpretationofJudgement = 1,
            InvalidityofJudgementExecution = 2,
            StopJudgementExecution = 4,
            RegenerationOfJudgmentExecution = 8,
        }
        public enum CaseDecisionStatusEnum
        {
            Approved = 1,
            Rejected = 2,
            AssignedToLawyer = 4,
            SendToMoj = 8,
            MojReplied = 16,
        }
        public enum CaseOutcomePartyActionEnum
        {
            Added = 1,
            Deleted = 2,
        }
        public enum ExecutionStatusEnum
        {
            Open = 1,
            Closed = 2,
        }
        public enum DraftActionIdEnum
        {
            [Display(Name = "CreatedAndDraft")]
            CreatedAndDraft = 1,
            [Display(Name = "EditedAndDraft")]
            EditedAndDraft = 2,
            [Display(Name = "Submitted")]
            Submitted = 4,
            [Display(Name = "Approved")]
            Approved = 8,
            [Display(Name = "Rejected")]
            Rejected = 16,
            [Display(Name = "EditedAndSubmitted")]
            EditedAndSubmitted =32,
            [Display(Name = "Published")]
            Published = 64,
        }
    }
}
