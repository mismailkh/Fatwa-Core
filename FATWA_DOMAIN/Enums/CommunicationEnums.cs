using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace FATWA_DOMAIN.Enums
{
    public class CommunicationEnums
    {
        public enum CommunicationTypeEnum
        {
            [Display(Name = "Send A Message")]
            SendMessage = 1,
            [Display(Name = "Request for Meeting")]
            RequestForMeeting = 2,
            [Display(Name = "Send a Response")]
            SendResponse = 4,
            [Display(Name = "Request For More Info")]
            RequestMoreInfo = 8,
            [Display(Name = "Case Request")]
            CaseRequest = 16,
            [Display(Name = "Consultation Request")]
            ConsultationRequest = 32,
            [Display(Name = "Meeting Scheduled")]
            MeetingScheduled = 64,
            [Display(Name = "Withdraw Requested")]
            WithdrawRequested = 128,
            ContractRequest = 256,
            LegislationRequest = 512,
            LegalAdviceRequest = 1024,
            AdministrativeComplaintRequest = 2048,
            InternationalArbitrationRequest = 4096,
            [Display(Name = "Request For More Information Reminder")]
            RequestForMoreInformationReminder = 8192,
            [Display(Name = "Save And Close File")]
            SaveAndCloseFile = 16384,
            [Display(Name = "Case Registered")]
            CaseRegistered = 32768,
            [Display(Name = "Case File Execution")]
            CaseFileExecution = 65536,
            [Display(Name = "Judgement")]
            InitialJudgement = 131072,
            [Display(Name = "General Update")]
            GeneralUpdate = 262144,
            [Display(Name = "Interrogation Judgement")]
            InterrogationJudgement = 524288,
            [Display(Name = "IncomingReport")]
            IncomingReport = 1048576,
            [Display(Name = "Interpretation of Judgment")]
            InterpretationofJudgment = 134217728,
            [Display(Name = "Invalidity of Judgment")]
            InvalidityofJudgment = 268435456,
            [Display(Name = "Accept Save And Close File")]
            AcceptSaveAndCloseFile = 536870912,
            [Display(Name = "Reject Save And Close File")]
            RejectSaveAndCloseFile = 1073741824,
           //incremented by 1 because when its multiplied by 2 it goes out of range of int type
            FinalDocument = 1073741825,
            StopExecutionOfJudgment = 1073741826,
            StoppingExecutionOfJudgment = 1073741827,
            G2GTarasolCorrespondence = 1073741828,
            FinalJudgement = 1073741829
        }

        public enum CommunicationResponseTypeEnum
        {
            [Display(Name = "On Hold")]
            OnHold = 1,
            [Display(Name = "Rejected")]
            Rejected = 2,
            [Display(Name = "Cancelled")]
            Cancelled = 4,
            [Display(Name = "Submit")]
            Submit = 8
        }

        public enum CommunicationCorrespondenceTypeEnum
        {
            [Display(Name = "Inbox")]
            Inbox = 1,
            [Display(Name = "Outbox")]
            Outbox = 2
		}
		public enum CommunicationSourceEnum
		{
            FATWA = 1,
			G2G = 2,
			Tarasul = 4,
			LegalAnnoucement = 8,
		}

        public enum ResponseTypeEnum
        {
            RequestForMoreInformation = 1,
            RequestForMoreInformationReminder = 2,
            SaveAndCloseFile = 4,
            CaseRegistered = 8,
            CaseFileExecution = 16,
            InitialJudgement=32,
            FinalJudgement=64,
            GeneralUpdate=128,
            InterrogationJudgement=256,
            IncomingReport=512,
            FinalDocument = 1024,
        }
        public enum CommunicationColorEnum 
        {         
            Yellow=1,
            LightGreen=2,
            Green=4,
            Red=8,
        }
        public enum CommunicationHistoryEnum 
        {         
            ForwardToLawyer=1,
            ReturnToHOS=2,
            ForwardToSector=4,
            ReturnToSender=8,
            RecieveFromTarasol = 16,
        }
    }
}
