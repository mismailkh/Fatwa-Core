using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.Dms
{
    [Table("DS_SIGNING_REQUEST_TASK_LOG")]
    public class DsSigningRequestTaskLog : TransactionalBaseModel
    {
        [Key]
        public Guid SigningTaskId { get; set; }
        public int DocumentId { get; set; }
        public string ReferenceId { get; set; }
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string Remarks { get; set; }
        public int ModuleId { get; set; }
        public int StatusId { get; set; }
        public string RejectionReason { get; set; }
        public int SigningMethodId { get; set; }
        [NotMapped]
        public int SectorTypeId { get; set; }
        [NotMapped]
        public int SubModuleId { get; set; }
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
        [NotMapped]
        public string? SenderName_En { get; set; }
        [NotMapped]
        public string? SenderName_Ar { get; set; }
    }
}
