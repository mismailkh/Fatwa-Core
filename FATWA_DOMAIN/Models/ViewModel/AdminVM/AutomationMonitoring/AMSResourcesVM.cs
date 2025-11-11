using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.AutomationMonitoring
{
    public class AMSResourcesVM
    {
        public int Id { get; set; }
        public string? ResourceName { get; set; }
        public DateTime? CurrentSessionDate { get; set; }
        public string? StatusNameAr { get; set; }
        public string? StatusNameEn { get; set; }
        public int? SessionId { get; set; }
        [NotMapped]
        public IList<AMSSessionLogsVM> aMSSessionLogs { get; set; } = new List<AMSSessionLogsVM>();
    }
}
