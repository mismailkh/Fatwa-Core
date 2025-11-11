namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.AutomationMonitoring
{
    public class AMSItemLogVM
    {
        public int Id { get; set; }
        public int ItemId { get; set; }
        public string? LogType { get; set; }
        public string? LogMessage { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string? CANumber { get; set; }
        public string? CaseNumber { get; set; }
        public string? Data { get; set; }

    }
}
