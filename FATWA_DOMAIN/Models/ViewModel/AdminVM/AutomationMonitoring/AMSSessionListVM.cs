namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.AutomationMonitoring
{
    public class AMSSessionListVM
    {
        public int SessionId { get; set; }
        public string ProcessName { get; set; }
        public string ResourceName { get; set; }
        public string Status { get; set; }
        public DateTime? DateStarted { get; set; }
        public DateTime? DateUpdated { get; set; }
    }
}
