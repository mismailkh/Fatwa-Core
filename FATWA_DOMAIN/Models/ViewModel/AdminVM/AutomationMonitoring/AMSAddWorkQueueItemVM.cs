namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.AutomationMonitoring
{
    public class AMSAddWorkQueueItemVM
    {
        public int QueueId { get; set; }
        public string? Data { get; set; }
        public string? ItemName { get; set; }
        public int? ResourceId { get; set; }
        public string? ResourceName { get; set; }
    }
}
