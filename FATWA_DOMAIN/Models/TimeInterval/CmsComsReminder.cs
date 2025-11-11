using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.TimeInterval
{
    [Table("WS_CMS_COMS_REMINDER")]
    public partial class CmsComsReminder : TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ID { get; set; }
        public int CmsComsReminderTypeId { get; set; }
        public int? CouttypeId { get; set; }
        public int? FirstReminderDuration { get; set; }
        public int? SecondReminderDuration { get; set; }
        public int? SLAInterval { get; set; }
        public int? ThirdReminderDuration { get; set; }
        public bool IsNotification { get; set; } = true;
        public DateTime? ExecutionTime { get; set; } = null;
        public bool? IsTask { get; set; }
        public bool IsActive { get; set; }
        // Reminder End Table Type Id
        public int? CommunicationTypeId { get; set; }
        public int? DraftTemplateVersionStatusId { get; set; }
        public int? CmsCaseFileStatusId { get; set; }
        [NotMapped]
        public string? WorkerServiceAppSettingPath { get; set; } // For writing CRONS into app setting, directly 
    }
}
