using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.LLSLegalPrinciple
{
    //<History Author = 'Umer Zaman' Date = '2024-04-16' Version = "1.0" Branch = "master">Create new model to manage attached documents with legal principle</History>

    [Table("LLS_LEGAL_PRINCIPLE_CONTENT_SOURCE_DOCUMENT_REFERENCE")]
    public class LLSLegalPrincipleContentSourceDocumentReference
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReferenceId { get; set; }
        public Guid PrincipleContentId { get; set; }
        public int PageNumber { get; set; }
        public int OriginalSourceDocId { get; set; }
        public int CopySourceDocId { get; set; }
		public bool IsMaskedJudgment { get; set; }
        public bool IsDeleted { get; set; }
    }
}
