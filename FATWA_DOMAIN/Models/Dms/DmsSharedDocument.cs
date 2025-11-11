using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.Dms
{
    [Table("DMS_SHARED_DOCUMENT")]
    public class DmsSharedDocument : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public int DocumentId { get; set; }
        public string SenderId { get; set; }
        public string RecieverId { get; set; }
        public string Notes { get; set; }
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
    }
}
