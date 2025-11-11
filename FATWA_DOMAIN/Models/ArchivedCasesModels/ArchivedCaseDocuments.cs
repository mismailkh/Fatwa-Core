using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//< History Author = 'Ammaar Naveed' Date = '2024-12-10' Version = "1.0" Branch = "master" >Created Case Documents Model for FATWA Archving System</History>

namespace FATWA_DOMAIN.Models.ArchivedCasesModels
{
    [Table("ARC_CASE_DOCUMENTS")]
    public class ArchivedCaseDocuments : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? CaseId { get; set; }
        public int DocumentTypeId { get; set; }
        public string? DocumentTitle { get; set; }
        public DateTime DocumentDate { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public int NumberOfPages { get; set; }
        public string DocType { get; set; }
        public string? ScannedBy { get; set; }
        public DateTime? ScannedOn { get; set; }
    }
}
