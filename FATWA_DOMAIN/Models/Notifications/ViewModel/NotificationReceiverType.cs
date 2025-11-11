using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.Notifications.ViewModel
{
    [Table("NOTIF_NOTIFICATION_RECEIVER_TYPE_LKP")]
    public class NotificationReceiverType
    {
        [Key]
        public int ReceiverTypeId { get; set; }
        public string ReceiverTypeName { get; set; }
    }
}
