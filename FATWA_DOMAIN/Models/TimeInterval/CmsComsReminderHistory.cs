using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using FATWA_DOMAIN.Models.BaseModels;

namespace FATWA_DOMAIN.Models.TimeInterval
{
    [Table("WS_CMS_COMS_REMINDER_HISTORY")]
    public class CmsComsReminderHistory : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public int CmsComsReminderId { get; set; }
        public int CmsComsReminderTypeId { get; set; }
        public int? SLAInterval { get; set; }
        public int? CouttypeId { get; set; }
        public int? FirstReminderDuration { get; set; }
        public int? SecondReminderDuration { get; set; }
        public int? ThirdReminderDuration { get; set; }
        public DateTime? ExecutionTime { get; set; }
        public bool IsNotification { get; set; } = true;
        public bool IsTask { get; set; }
        public bool IsActive { get; set; }
        public int StatusId { get; set; }
        public int? CommunicationTypeId { get; set; }
        public int? DraftTemplateVersionStatusId { get; set; }
        public int? CmsCaseFileStatusId { get; set; }
    }
}
