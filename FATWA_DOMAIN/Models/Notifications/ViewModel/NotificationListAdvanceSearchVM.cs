using FATWA_DOMAIN.Models.BaseModels;

namespace FATWA_DOMAIN.Models.Notifications.ViewModel
{
    public class NotificationListAdvanceSearchVM : GridPagination
    {
        public int? ModuleId { get; set; }
        public int? EventId { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
        public string UserId { get; set; }
        public bool IsLatest { get; set; } = true;
        
    }
}
