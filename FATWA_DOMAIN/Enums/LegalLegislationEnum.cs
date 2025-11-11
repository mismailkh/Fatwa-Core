using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Enums
{
    public class LegalLegislationEnum
    {
        public enum LegalArticleSourceEnum
        {
            [Display(Name = "New")]
            New = 1,
            [Display(Name = "Existing")]
            Existing = 2,
            [Display(Name = "Repeated")]
            Repeated = 4,
        }
        public enum LegalArticleStatusEnum
        {
            [Display(Name = "Active")]
            Active = 1,
            [Display(Name = "Modified")]
            Modified = 2,
            [Display(Name = "Expired")]
            Expired = 4,
        }
        public enum LegislationFlowStatusEnum
        {
            [Display(Name = "Partially Completed")]
            PartiallyCompleted = 1,
            [Display(Name = "In Review")]
            InReview = 2,
            [Display(Name = "Approved")]
            Approved = 4,
            [Display(Name = "Rejected")]
            Rejected = 8,
            [Display(Name = "Need To Modify")]
            NeedToModify = 16,
            [Display(Name = "Send A Comment")]
            SendAComment = 32,
            [Display(Name = "Published")]
            Published = 64,
            [Display(Name = "Unpublished")]
            Unpublished = 128,
            [Display(Name = "New")]
            New = 256,
            [Display(Name = "Need Modification")]
            NeedModification = 512,
        }
        public enum LegislationStatus
        {
            [Display(Name = "Active")]
            Active = 1,
            [Display(Name = "Modified")]
            Modified = 2,
            [Display(Name = "Expired")]
            Expired = 4,
        }
        public enum LegalTemplateSettingEnum
        {
            [Display(Name = "Legislation Number")]
            Legislation_Number = 1,
            [Display(Name = "Legislation Issue Date")]
            Legislation_Issue_Date = 2,
            [Display(Name = "Legislation Start Date")]
            Legislation_Start_Date = 4,
            [Display(Name = "Legislation Subject")]
            Legislation_Subject = 8,
            [Display(Name = "Introduction with relation")]
            Introduction_with_relation = 16,
            [Display(Name = "Introduction without relation")]
            Introduction_without_relation = 32,
            [Display(Name = "Publication details")]
            Publication_details = 64,
            [Display(Name = "Articles with sections")]
            Articles_with_sections = 128,
            [Display(Name = "Articles without sections")]
            Articles_without_sections = 256,
            [Display(Name = "Clauses with sections")]
            Clauses_with_sections = 512,
            [Display(Name = "Clauses without sections")]
            Clauses_without_sections = 1024,
            [Display(Name = "Explanatory Note")]
            Explanatory_Note = 2048,
            [Display(Name = "Note")]
            Note = 4096,
        }
        public enum LegalLegislationTypeEnum
        {
            [Display(Name = "Act")]
            Act = 1,
            [Display(Name = "Amiri Decree")]
            Amiri_Decree = 2,
            [Display(Name = "Decree")]
            Decree = 4,
            [Display(Name = "Law")]
            Law = 8,
        }
    }
}