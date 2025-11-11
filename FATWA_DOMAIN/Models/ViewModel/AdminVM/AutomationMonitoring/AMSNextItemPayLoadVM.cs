namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.AutomationMonitoring
{
    public class AMSNextItemPayLoadVM
    {
        public string QueueName { get; set; }
        public int? Priority { get; set; }
        public string ResourceName { get; set; }
        public int ResourceId { get; set; }
    }
}
