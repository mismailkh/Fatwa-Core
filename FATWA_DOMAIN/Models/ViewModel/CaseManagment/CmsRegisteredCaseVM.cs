using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    //<History Author = 'Danish' Date='2022-11-29' Version="1.0" Branch="master"> CmsRegisteredCaseVM </History>
    public class CmsRegisteredCaseVM : TransactionalBaseModel
    {
        [Key]
        public Guid CaseId { get; set; }
        public Guid? FileId { get; set; }
        public string? CANNumber { get; set; }
        public string? CaseNumber { get; set; }
        public DateTime? CaseDate { get; set; }
        public int? CourtId { get; set; }
        public int? CourtTypeId { get; set; }
        public int? ChamberId { get; set; }
        public int? StatusId { get; set; }
        public int? GovtEntityId { get; set; }
        public bool? IsConfidential { get; set; }
        public int? SubcaseCount { get; set; }
        public string? CaseRequirements { get; set; }
        public string? GovtEntityNameEn { get; set; }
        public string? GovtEntityNameAr { get; set; }
        public string? StatusNameEn { get; set; }
        public string? StatusNameAr { get; set; }
        public string? CourtTypeNameEn { get; set; }
        public string? CourtTypeNameAr { get; set; }
        public string? CourtNameEn { get; set; }
        public string? CourtNameAr { get; set; }
        public string? CourtNumber { get; set; }
        public string? CourtDistrict { get; set; }
        public string? CourtLocation { get; set; }
        public string? ChamberNameEn { get; set; }
        public string? ChamberNameAr { get; set; }
        public string? ChamberNumber { get; set; }
        public string? ChamberAddress { get; set; }
        public string FileNumber { get; set; }

        [NotMapped]
        public IList<CmsRegisteredCaseVM> RegisteredCases { get; set; } = new List<CmsRegisteredCaseVM>();
         [NotMapped]
        public IList<CmsJugdmentDecisionVM> jugdmentDecisionVMs { get; set; } = new List<CmsJugdmentDecisionVM>();

    }
    public class CmsRegisteredCaseDmsVM : TransactionalBaseModel
    {
        [Key]
        public Guid CaseId { get; set; }
        public Guid? FileId { get; set; }
        public string? CANNumber { get; set; }
        public string? CaseNumber { get; set; }
        public DateTime? CaseDate { get; set; }
        public int? CourtId { get; set; }
        public int? CourtTypeId { get; set; }
        public int? ChamberId { get; set; }
        public int? StatusId { get; set; }
        public int? GovtEntityId { get; set; }
        public bool? IsConfidential { get; set; }
        public int? SubcaseCount { get; set; }
        public string? CaseRequirements { get; set; }
        public string? GovtEntityNameEn { get; set; }
        public string? GovtEntityNameAr { get; set; }
        public string? StatusNameEn { get; set; }
        public string? StatusNameAr { get; set; }
        public string? CourtTypeNameEn { get; set; }
        public string? CourtTypeNameAr { get; set; }
        public string? CourtNameEn { get; set; }
        public string? CourtNameAr { get; set; }
        public string? CourtNumber { get; set; }
        public string? CourtDistrict { get; set; }
        public string? CourtLocation { get; set; }
        public string? ChamberNameEn { get; set; }
        public string? ChamberNameAr { get; set; }
        public string? ChamberNumber { get; set; }
        public string? ChamberAddress { get; set; }
        public string FileNumber { get; set; }
    }
    public class CmsRegisteredCaseFileDetailVM : TransactionalBaseModel
    {
        [Key]
        public Guid CaseId { get; set; }
        public Guid? FileId { get; set; }
        public string? CANNumber { get; set; }
        public string? CaseNumber { get; set; }
        public DateTime? CaseDate { get; set; }
        public int? CourtId { get; set; }
        public int? CourtTypeId { get; set; }
        public int? ChamberId { get; set; }
        public int? StatusId { get; set; }
        public int? GovtEntityId { get; set; }
        public bool? IsConfidential { get; set; }
        public int? SubcaseCount { get; set; }
        public string? CaseRequirements { get; set; }
        public string? GovtEntityNameEn { get; set; }
        public string? GovtEntityNameAr { get; set; }
        public string? StatusNameEn { get; set; }
        public string? StatusNameAr { get; set; }
        public string? CourtTypeNameEn { get; set; }
        public string? CourtTypeNameAr { get; set; }
        public string? CourtNameEn { get; set; }
        public string? CourtNameAr { get; set; }
        public string? CourtNumber { get; set; }
        public string? CourtDistrict { get; set; }
        public string? CourtLocation { get; set; }
        public string? ChamberNameEn { get; set; }
        public string? ChamberNameAr { get; set; }
        public string? ChamberNumber { get; set; }
        public string? ChamberAddress { get; set; }
        public string FileNumber { get; set; }
        public bool IsFinalJudgement { get; set; }

        [NotMapped]
        public IList<CmsRegisteredCaseVM> RegisteredCaseFileDetails { get; set; } = new List<CmsRegisteredCaseVM>();
        [NotMapped]
        public IList<CmsJugdmentDecisionVM> jugdmentDecisionVMs { get; set; } = new List<CmsJugdmentDecisionVM>();

    }
}
