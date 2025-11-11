using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;
using FATWA_DOMAIN.Models.Notifications.ViewModel;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    //<History Author = 'Danish' Date='2022-01-03' Version="1.0" Branch="master">CmsJudgmentExecution</History>
    [Table("CMS_EXECUTION")]
    public class CmsJudgmentExecution : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? CaseId { get; set; }
        public Guid ExecutionRequestId { get; set; }
        //public string? ExecutorNumber { get; set; }
        public string? ExecutionFileNumber { get; set; }
        public int? FileStatusId { get; set; }      
        public DateTime? FileOpeningDate { get; set; }
        //public DateTime? FileAttachDate { get; set; }       
        public decimal? FileBalance { get; set; }
        //public string? AttachedFileNo { get; set; }
        //public string? AttachedFileSection { get; set; }
        public string? Remarks { get; set; }
        public decimal? Amount { get; set; }
        public decimal? PaidAmount { get; set; }
 
        public Guid PayerId { get; set; }
        public Guid ReceiverId { get; set; }
        [NotMapped]
        public string? LawyerUserName { get; set; }
        [NotMapped]
        public MojExecutionRequest? ExecutionRequest { get; set; }
        [NotMapped]
        public Guid? DecisionId { get; set; }

        [NotMapped]
        public string TaskUserId { get; set; }
        [NotMapped]
        public Guid PayerIdGuid { get; set; }
        [NotMapped]
        public Guid ReceiverIdGuid { get; set; }
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();

        [NotMapped]
        public bool IsUpdated { get; set; }
    }
}
