using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.AutomationMonitoring
{
    public class AMSQueueListVM
    {
        [Key]
        public int QueueId { get; set; }
        public string? QueueName { get; set; }
    }
}
