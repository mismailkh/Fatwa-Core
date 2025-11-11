using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.Notifications.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.DmsVMs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    //< History Author = 'Hassan Abbas' Date = '2022-12-05' Version = "1.0" Branch = "master">Outcome of hearing</History>
    [Table("CMS_OUTCOME_HEARING")]
    public class OutcomeHearing : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid HearingId { get; set; }
        public DateTime HearingDate { get; set; }
        public DateTime? NextHearingDate { get; set; }
        public TimeSpan HearingTime { get; set; }
        public string LawyerId { get; set; }
        public string Remarks { get; set; }
        [NotMapped]
        public DateTime Time { get; set; }
        [NotMapped]
        public List<int>? DeletedAttachementIds { get; set; } = new List<int>();
        [NotMapped]
        public Guid CaseId { get; set; }
        [NotMapped]
        public List<CopyAttachmentVM>? CopyAttachmentVMs { get; set; } = new List<CopyAttachmentVM>();
        [NotMapped]
        public List<CasePartyLinkVM> caseParties { get; set; } = new List<CasePartyLinkVM>();
         [NotMapped]
        public List<CasePartyLinkVM> CasePartyLinks { get; set; } = new List<CasePartyLinkVM>();
        [NotMapped]
        public List<CasePartyLinkVM> DeletedParties { get; set; } = new List<CasePartyLinkVM>();
        [NotMapped]
        public int? SectorTypeId { get; set; }
        [NotMapped]
        public bool IsExist { get; set; }
        [NotMapped]
        public List<JudgementVM> outcomeJudgement { get; set; } = new List<JudgementVM>();
        [NotMapped]
        public List<MojExecutionRequest> mojExecutionRequest { get; set; } = new List<MojExecutionRequest>();
        [NotMapped]
        public NotificationParameter NotificationParameter { get; set; } = new NotificationParameter();
        [NotMapped]
        public List<CmsRegisteredCaseTransferRequestVM> caseTransferRequestsVM { get; set; } = new List<CmsRegisteredCaseTransferRequestVM>();
    }
}
