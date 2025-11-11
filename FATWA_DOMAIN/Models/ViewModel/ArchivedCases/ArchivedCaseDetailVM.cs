
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.ArchivedCases
{
    public class ArchivedCaseDetailVM
    {
        [Key]
        public string? CANNumber { get; set; }
        public string? CaseNumber { get; set; }
        public string? ChamberNumber { get; set; }
        public string? ChamberName_En { get; set; }
        public string? ChamberName_Ar { get; set; }
        public Guid CaseId { get; set; }
        public Guid CANId { get; set; }
        [NotMapped]
        public List<ArchivedCasePartiesVM> CasePartiesList { get; set; } = new List<ArchivedCasePartiesVM>();
        [NotMapped]
        public List<ArchivedCaseDocumentsVM> CaseDocumentsList { get; set; } = new List<ArchivedCaseDocumentsVM>();
    }

    public class ArchivedCasePartiesVM
    {
        public string? PartyName { get; set; }
        public string? PartyRoleName_En { get; set; }
        public string? PartyRoleName_Ar { get; set; }        
    }

    public class ArchivedCaseDocumentsVM
    {
        [Key]
        public Guid DocumentId { get; set; }
        public string CaseNumber { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string? DocumentType_En { get; set; }
        public string? DocumentType_Ar { get; set; }
        public string? StoragePath { get; set; }
        public string DocType { get; set; }
    }
}
