using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.WorkerService
{
    [Table("WS_REMINDER_PROCESSLOG")]
    public partial class WSCmsComsReminderProcessLog : TransactionalBaseModel
    {
        [Key]
        public Guid? Id { get; set; }
        public int? WorkerServiceId { get; set; }
        public string? ReceiverId { get; set; }
        public string? Sender { get; set; }
        public int? ProcessTypeId { get; set; }
        public int? ReminderId { get; set; }
        public int? ReminderTypeId { get; set; }
        public int? CommunicationTypeId { get; set; }
        public int? DraftTemplateVersionStatusId { get; set; }
        public int? CmsCaseFileStatusId { get; set; }
        public string? Description { get; set; }
        public bool IsNotification { get; set; }
        public bool IsTask { get; set; }
        public bool IsFirstReminder { get; set; }
        public bool IsSecondReminder { get; set; }
        public bool IsThirdReminder { get; set; }
        public bool IsActive { get; set; }
        public string? ReferenceId { get; set; }
    }
}
