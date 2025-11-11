using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ArchivedCasesModels
{
    [Table("ARC_ARCHIVED_CASE_DOCUMENT_TYPE_LKP")]
    public class ArchivedCaseDocumentTypes : TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string? Type_En { get; set; }
        public string? Type_Ar { get; set; }
        public bool IsActive { get; set; }
    }
}
