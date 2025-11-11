using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Enums
{
	public partial class TaskEnums
	{
		public enum TaskStatusEnum
		{
			[Display(Name = "Pending")]
			Pending = 1,
			[Display(Name = "Approved")]
			Approved = 2,
			[Display(Name = "Rejected")]
			Rejected = 4,
			[Display(Name = "In Progress")]
			InProgress = 8,
			[Display(Name = "Done")]
			Done = 16
		}

		public enum TaskTypeEnum
		{
			[Display(Name = "Task")]
			Task = 1,
			[Display(Name = "Request")]
			Request = 2,
            [Display(Name = "Assignment")]
            Assignment = 4,
			[Display(Name = "Transfer")]
			Transfer = 8,
		}
		 
		public enum TaskSubTypeEnum
		{
			[Display(Name = "General Request")]
			GeneralRequest = 1,
			[Display(Name = "Request for Appointment with Medical Council")]
			RequestforAppointmentwithMedicalCouncil = 2,
			[Display(Name = "Request for Leave")]
			RequestforLeave = 4,
			[Display(Name = "Request for Sick Leave")]
			RequestforSickLeave = 8,
			[Display(Name = "Request for Reducing Working Hours")]
			RequestforReducingWorkingHours = 16,
			[Display(Name = "Request for Fingerprint Exemption")]
			RequestforFingerprintExemption = 32,
			[Display(Name = "Submission of Employee Permissions")]
			SubmissionofEmployeePermissions = 64,
			[Display(Name = "Request for Renewing Residency of Non-Kuwaiti Employees")]
			RequestforRenewingResidencyofNonKuwaitiEmployees = 128,
			[Display(Name = "Request for Issuing benefits for Wife and Children")]
			RequestforIssuingbenefitsforWifeandChildren = 256,
			[Display(Name = "Request for Revoking benefits for Wife and Children")]
			RequestforRevokingbenefitsforWifeandChildren = 512,
			[Display(Name = "Request for Internal Transfer")]
			RequestforInternalTransfer = 1024,
			[Display(Name = "Request for External Transfer")]
			RequestforExternalTransfer = 2048,
			[Display(Name = "Request for Special Needs Benefits")]
			RequestforSpecialNeedsBenefits = 4096,
		}
		 
		public enum TaskResponseStatusEnum
		{
			[Display(Name = "In-Progress")]
			InProgress = 1,
			[Display(Name = "On-Hold")]
			OnHold = 2,
			[Display(Name = "Completed")]
			Completed = 4,
			[Display(Name = "Rejected")]
			Rejected = 8 
		} 

		public enum TaskSystemGenTypeEnum
		{
            
            CaseRequestTransfer = 1,
			CaseRequestSendCopy = 2,
			CaseRequestDraftDocumentReview = 4,
            CaseFileAssignToLawyer = 8,
            RegisteredCaseAssignToLawyer = 16,
            CaseFileTransfer = 32,
			CaseFileSendCopy = 64,
			CaseFileAssignToSector = 128,
			DraftDocumentReview = 256,
			CaseMergeRequest = 512,
			ExecutionRequest = 1024,
            CreateCaseRequest = 2048,
            WithdrawCaseRequest = 4096,
            ConsultationRequestTransfer = 8192,
			ConsultationRequestDraftDocumentReview = 16384,
			ConsultationFileAssignToLawyer = 32768,
			ConsultationRequestAssignToLawyer = 65536,
			ConsultationFileTransfer = 131072,
			ConsultationFileAssignToSector = 1048576,
			ConsultationDraftDocumentReview = 2097152,
            CreateConsultationRequest = 4194304,
			WithdrawConsultationRequest = 8388608,
            Communication = 16777216,
            RegisteredCaseToMoj = 33554432,
            documentportfolioRequest = 67108864,
			CaseFileRejectTransfer = 134217728,
			ConsultationFileRejectTransfer = 268435456,
			Meeting = 536870912,
			DMSReviewDocument= 1073741824,
			//Incremented by 1 due to limit of int
			MeetingSendToHos = 1073741825,
			Hearing= 1073741826,
            CaseMigratedFromMOJ= 1073741827,
			LeaveAttendanceRequest = 1073741828,
            CaseFileAssignmentApproval = 1073741829,
            CaseFileTranferRequestToSector = 1073741830,
            RegisteredCaseTranferRequest = 1073741831,
            InventoryManagement = 1073741832,
            FinalJudgementIssued = 1073741833,
        }   
	}
}
