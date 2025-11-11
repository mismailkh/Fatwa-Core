using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FATWA_DOMAIN.Models.Notifications.ViewModel;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    //< History Author = 'Hassan Abbas' Date = '2022-12-10' Version = "1.0" Branch = "master" > Judgement model</History
    [Table("CMS_JUDGEMENT")]
    public class Judgement : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid OutcomeId { get; set; }
        public Guid CaseId { get; set; }
        public DateTime HearingDate { get; set; }
        public DateTime JudgementDate { get; set; }
        public int TypeId { get; set; }
        public int CategoryId { get; set; }
        public int StatusId { get; set; }
        public decimal? Amount { get; set; }
        public decimal? AmountCollected { get; set; }
        public string Remarks { get; set; }
        public string SerialNumber { get; set; } = "";
        public bool IsFinal { get; set; } = false;
        public bool OpenExecutionFile { get; set; } = false;
        public int ExecutionFileLevelId { get; set; }
        [NotMapped]
        public int? SectorTypeId { get; set; }
        [NotMapped]
        public bool IsUpdated { get; set; }
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
    }
}
