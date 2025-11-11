using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Enums.Common
{
    public enum LinkTargetTypeEnum
    {
        [Display(Name = "Case Request")]
        CaseRequest = 1,
        [Display(Name = "File")]
        File = 2,
        [Display(Name = "Registered Case")]
        RegisteredCase = 4,
        [Display(Name = "Consultation Request")]
        ConsultationRequest = 8,
        [Display(Name = "Communication")]
        Communication = 16,
        [Display(Name = "Meeting")]
        Meeting = 32,
        [Display(Name = "Consultation File")]
        ConsultationFile = 64,
        [Display(Name = "Withdraw Request")]
        WithdrawRequest = 128,


    }
    public enum CmsComsNumPatternTypeEnum
    {
        [Display(Name = "Case Request Number")]
        CaseRequestNumber = 1,
        [Display(Name = "Consultation Request Number")]
        ConsultationRequestNumber = 2,
        [Display(Name = "Case File Number")]
        CaseFileNumber = 3,
        [Display(Name = "Consultation File Number")]
        ConsultationFileNumber = 4,
        [Display(Name = "Inbox Number")]
        InboxNumber = 5,
        [Display(Name = "Outbox Number")]
        OutboxNumber = 6,


    }
    public enum LookupHistoryEnums
    {
        [Display(Name = "Added")]
        Added = 1,
        [Display(Name = "Updated")]
        Updated = 2,
    }
    public enum FatwaSectorTypeEnum
    {
        [Display(Name = "Case")]
        Case = 1,
        [Display(Name = "Consultation")]
        Consultation = 2,
        [Display(Name = "Others")]
        Others = 3,
    }
    public enum literatureDeweyNumberPatternEnum
    {
        [Display(Name = "Literature Dewey Number Pattern")]
        LiteratureDeweyNumberPattern = 1,
        [Display(Name = "Others")]
        Others = 2,
    }
    // CRUD crudEnum CrudTypeEnum
    public enum CrudOperationEnum
    {
        [Display(Name = "Add")]
        Add = 1,
        [Display(Name = "Update")]
        Update = 2,
        [Display(Name = "Delete")]
        Delete = 3,
    }

    public enum EmailBodyTypeEnum
    {
        [Display(Name = "Text")]
        Text = 1,
        [Display(Name = "Html")]
        Html = 2
    }

}
