namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.AutomationMonitoring
{
    public class AutomationMonitoringProcessVM
    {
        public int Id { get; set; }
        public string? ProcessName { get; set; }
        public DateTime? LastRunDate { get; set; }
        public string Status { get; set; }
        public DateTime? LaunchDate { get; set; }
        public string? Description { get; set; }
        public string? Remarks { get; set; }
        public int? Sessions { get; set; }
        public int? WorkQueues { get; set; }
        public string? ProcessCode { get; set; }
        public string? Resources { get; set; }
    }
}
