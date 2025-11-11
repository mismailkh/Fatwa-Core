namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.AutomationMonitoring
{
    public class AutomationMonitoringQueueItemVM
    {
        public int ItemId { get; set; }
        public string? ItemName { get; set; }
        public string? StatusEn { get; set; }
        public string? StatusAr { get; set; }
        public int? StatusId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? StartDate { get; set; }
        public string? CompletedDuration { get; set; }
        public string? ExceptionDetails { get; set; }
        public string? QueueName { get; set; }
        public string? Tag { get; set; }
        public string? ExceptionComment { get; set; }
        public int? StatusCode { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string? ResourceName { get; set; }

    }
}
