using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    //< History Author = 'Hassan Abbas' Date = '2024-03-11' Version = "1.0" Branch = "master" > Registered Case Announcements</History>
    [Table("CMS_CASE_ANNOUNCEMENT")]
    public partial class CaseAnnouncement : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CaseId { get; set; }
        public Guid PartyId { get; set; }
        public string? AnouncementNumber { get; set; }
        public string? AnouncementType { get; set; }
        public string? PersonToBeanounced { get; set; }
        public string? DistributionStatus { get; set; }
        public string? AnouncementResult { get; set; }
        public string? Reason { get; set; }
        public DateTime? HearingDate { get; set; }
        public DateTime? AnouncementMakingDate { get; set; }
        public DateTime? AnouncementGoOutDate { get; set; }
        public DateTime? ActualAnouncementDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
    }
}
