namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.AutomationMonitoring
{
    public class AMSSessionLogsVM
    {
        public int Id { get; set; }
        public string? LogType { get; set; }
        public string? LogMessage { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
