using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.AutomationMonitoring
{
    public class AMSQueueLogPayLoadVM
    {
        public int ItemId { get; set; }
        public string? Description { get; set; }
        public string? LogType { get; set; }
    }
}
