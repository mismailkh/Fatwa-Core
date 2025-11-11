using FATWA_DOMAIN.Models.BaseModels;

namespace FATWA_DOMAIN.Models.ViewModel
{
    public class AdvanceSearchCmsCaseFileVM : GridPagination
    {
        public string? FileNumber { get; set; }
        public int? StatusId { get; set; }
        public int? SectorTypeId { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
        public DateTime? ModifiedFrom { get; set; }
        public DateTime? ModifiedTo { get; set; }
        public string? UserId { get; set; }
        public int? GovEntityId { get; set; }
        public string? CANNumber { get; set; }
        public string? CaseNumber { get; set; }
        public int? CourtId { get; set; }
        public int? ChamberId { get; set; }
        public int? ChamberNumberId { get; set; }
        public bool IsImpportant { get; set; } = false;
        public string? LawyerId { get; set; }
        public bool isFinalJudgment { get; set; } = false;
        public string? PlaintiffName { get; set; }
        public string? DefendantName { get; set; }
        
    }
}
