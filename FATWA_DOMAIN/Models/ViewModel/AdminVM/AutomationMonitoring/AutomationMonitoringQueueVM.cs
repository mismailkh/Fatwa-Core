namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.AutomationMonitoring
{
    public class AutomationMonitoringQueueVM
    {
        public int QueueId { get; set; }
        public string? QueueName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string StatusNameEn { get; set; }
        public string StatusNameAr { get; set; }
        public string? ProcessName { get; set; }
        public int? ItemCount { get; set; }
        public int? PendingItems { get; set; }
        public int? LockedItems { get; set; }
        public int? CompletedItems { get; set; }
        public int? ExceptionItems { get; set; }
    }
}
