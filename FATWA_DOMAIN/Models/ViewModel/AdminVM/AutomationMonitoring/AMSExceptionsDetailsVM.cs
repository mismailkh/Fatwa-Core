namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.AutomationMonitoring
{
    public class AMSExceptionsDetailsVM
    {
        public int? ItemId { get; set; }
        public string? ItemName { get; set; }
        public DateTime? OccurrenceDate { get; set; }
        public string? ExceptionType { get; set; }
        public string? ExceptionMessage { get; set; }
        public string? ExceptionTraceback { get; set; }
    }
}
