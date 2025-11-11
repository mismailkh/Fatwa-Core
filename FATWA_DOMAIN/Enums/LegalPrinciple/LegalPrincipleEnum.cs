using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Enums.LegalPrinciple
{
    public class LegalPrincipleEnum
    {
        public enum PrincipleFlowStatusEnum
        {
            [Display(Name = "Partially Completed")]
            PartiallyCompleted = 1,
            [Display(Name = "In Review")]
            InReview = 2,
            [Display(Name = "Approved")]
            Approve = 4,
            [Display(Name = "Rejected")]
            Reject = 8,
            [Display(Name = "Need To Modify")]
            NeedToModify = 16,
            [Display(Name = "Send A Comment")]
            SendAComment = 32,
            [Display(Name = "Published")]
            Publish = 64,
            [Display(Name = "Unpublished")]
            Unpublished = 128,
            [Display(Name = "New")]
            New = 256,
            [Display(Name = "Need Modification")]
            NeedModification = 512,

        }     
        public enum LLSLegalPrincipleSourceFileTypeEnum
        {
            AppealJudgements = 1, // Under GeneralEnum class -> AttachmentTypeEnum -> CmsAppealJudgement = 60
			SupremeJudgements = 2, // Under GeneralEnum class -> AttachmentTypeEnum -> CmsAppealJudgement = 61
			LegalAdvice = 4, // Under GeneralEnum class -> AttachmentTypeEnum -> CmsAppealJudgement = 63
			KuwaitAlYawm = 16, // Under GeneralEnum class -> AttachmentTypeEnum -> CmsAppealJudgement = 23

			//LegalPublications = 16,
		}

        public enum JudgementsTabsEnums
        {
            AppealJudgements,
            SupremeJudgements,
            LegalAdvice,
            KuwaitAlYawm,
            Others
        }
    }
}
