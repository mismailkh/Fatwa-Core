using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    public class CmsAnnouncementVM
    {
        [Key]
        public Guid Id { get; set; }
        public Guid? CaseId { get; set; }
        public string? PartyName { get; set; }
        public int? TypeId { get; set; }
        public string? TypeName_En { get; set; }
        public string? TypeName_Ar { get; set; }
        public string? GovtEntity_En { get; set; }
        public string? GovtEntity_Ar { get; set; }
        public string? RepresentativeEn { get; set; }
        public string? RepresentativeAr { get; set; }
        public string? AnouncementNumber { get; set; }
        public string? AnouncementType { get; set; }
        public string? PersonToBeanounced { get; set; }
        public DateTime? HearingDate { get; set; }
        public string? DistributionStatus { get; set; }
        public DateTime? AnouncementMakingDate { get; set; }
        public DateTime? AnouncementGoOutDate { get; set; }
        public DateTime? ActualAnouncementDate { get; set; }
        public DateTime? LastUpdateDate { get; set; }
        public string? AnouncementResult { get; set; }
        public string? Reason { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        [NotMapped]
        public CasePartyTypeEnum CasePartyType { get; set; }



    }
}
