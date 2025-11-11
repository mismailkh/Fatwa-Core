using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Models.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static FATWA_DOMAIN.Enums.WorkflowEnums;

namespace FATWA_DOMAIN.Models
{
    [Table("LEGAL_LEGISLATION", Schema = "dbo")]
    public partial class LegalLegislation
    {
        [Key]
        public Guid LegislationId { get; set; }
        public int Legislation_Type { get; set; }   
        public string? Legislation_Number { get; set; } = string.Empty;
        public string? Introduction { get; set; } = string.Empty;
        public DateTime? IssueDate { get; set; } = null;
        public DateTime? IssueDate_Hijri { get; set; }
        public string? LegislationTitle { get; set; } = string.Empty;
        public string? Legislation_Comment { get; set; } = string.Empty;
        public string? LegislationYear { get; set; } = string.Empty;
        public string? LegislationRemark { get; set; } = string.Empty;
        public int Legislation_Status { get; set; }
        public int Legislation_Flow_Status { get; set; }
        public DateTime? StartDate { get; set; } = null;
        public string AddedBy { get; set; }
        public DateTime AddedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? CanceledBy { get; set; }
        public DateTime? CanceledDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string? UserId { get; set; }
        public string? RoleId { get; set; }

        [NotMapped]
        public Guid? ExistingLegislationIdForNewLegislation { get; set; } = Guid.Empty;
        [NotMapped]
        public DateTime? OldLegislation_EndDate { get; set; } = null;
        [NotMapped]
        public int? OldLegislation_Status { get; set; } = 0;
        [NotMapped]
        public int? NumberofArticle { get; set; } = 0;
        [NotMapped]
        public int? NumberofClause { get; set; } = 0;
        [NotMapped]
        public string? EditCaseLegislationNumber { get; set; } = string.Empty;

        [NotMapped]
        public List<int>? LegalLegislationTags { get; set; } = new List<int>();
        [NotMapped]
        public List<LegalPublicationSource> LegalPublicationSources { get; set; } = new List<LegalPublicationSource>();

        [NotMapped]
        public List<LegalLegislationType>? LegalLegislationTypes { get; set; } = new List<LegalLegislationType>();
        [NotMapped]
        public List<LegalLegislationSignature>? LegalLegislationSignatures { get; set; } = new List<LegalLegislationSignature>();
        [NotMapped]
        public List<LegalLegislationReference>? LegalLegislationReferences { get; set; } = new List<LegalLegislationReference>();
        [NotMapped]
        public List<LegalSection>? LegalSections { get; set; } = new List<LegalSection>();
        [NotMapped]
        public List<LegalArticle>? LegalArticles { get; set; } = new List<LegalArticle>();
        [NotMapped]
        public List<LegalClause>? LegalClauses { get; set; } = new List<LegalClause>();
        [NotMapped]
        public List<LegalExplanatoryNote>? LegalExplanatoryNotes { get; set; } = new List<LegalExplanatoryNote>();
        [NotMapped]
        public List<LegalNote>? legalNotes { get; set; } = new List<LegalNote>();
        [NotMapped]
        public LegalTemplate? LegalTemplates { get; set; } = new LegalTemplate();
        [NotMapped]
        public List<Guid>? NewLegislationAddedId { get; set; } = new List<Guid>();
        [NotMapped]
        public List<Guid>? OldTemplateDetails { get; set; } = new List<Guid>();
        [NotMapped]
        public int? WorkflowActivityId { get; set; }
        [NotMapped]
        public WorkflowInstanceStatusEnum? WorkflowInstanceStatusId { get; set; }
        [NotMapped]
        public string? Token { get; set; }
        [NotMapped]
        public List<TempAttachementVM>? SelectedSourceDocumentForDelete { get; set; } = new List<TempAttachementVM>();
        [NotMapped]
        public List<int>? DeletedAttachementIds { get; set; } = new List<int>();
        [NotMapped]
        public List<LegalLegislationArticleEffectHistory>? LegalLegislationArticleEffectHistorys { get; set; } = new List<LegalLegislationArticleEffectHistory>();
        [NotMapped]
        public List<int?>? MaskedDocumentAttachmentIdList { get; set; } = new List<int?>();
        [NotMapped]
        public List<TempAttachementVM>? AttachedDocumentList { get; set; } = new List<TempAttachementVM>();
        [NotMapped]
        public string SenderEmail { get; set; }
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
    }
}
