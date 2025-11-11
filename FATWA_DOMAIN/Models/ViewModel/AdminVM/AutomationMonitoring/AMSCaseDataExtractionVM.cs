namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.AutomationMonitoring
{
    public class AMSCaseDataExtractionVM
    {

        public int ItemId { get; set; }
        public string? StatusEn { get; set; }
        public string? StatusAr { get; set; }
        public int? StatusId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public string? CANNumber { get; set; }
        public string? CaseNumber { get; set; }
        public string? Data { get; set; }
        public string? CreatedBy { get; set; }
        public int? StatusCode { get; set; }

    }
}
