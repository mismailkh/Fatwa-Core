using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.TaskModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    public class CmsRegisteredCaseTransferRequestVM
    {
        [Key]
        public Guid Id { get; set; }
        public Guid OutcomeId { get; set; }
        public Guid CaseId { get; set; }
        public int ChamberFromId { get; set; }
        public int ChamberToId { get; set; }
        public int ChamberNumberFromId { get; set; }
        public int ChamberNumberToId { get; set; }
        public int StatusId { get; set; }
        public string? ChamberFromNameEn { get; set; }
        public string? ChamberFromNameAr { get; set; }
        public string? ChamberToNameEn { get; set; }
        public string? ChamberToNameAr { get; set; }
        public string? ChamberNumberFrom { get; set; }
        public string? ChamberNumberTo{ get; set; }
        public string? Remarks { get; set; }
        public string? StatusNameEn { get; set; }
        public string? StatusNameAr { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        [NotMapped]
        public int SectorTypeId { get; set; }
        [NotMapped]
        public string? RejectionReason { get; set; }
        [NotMapped]
        public string? UserName { get; set; }
        [NotMapped]
        public RejectReason rejectReason { get; set; } = new RejectReason();
        [NotMapped]
        public CMSRegisteredCaseTransferHistoryVM caseTransferHistoryVM { get; set; } = new CMSRegisteredCaseTransferHistoryVM();
        [NotMapped]
        public bool? IsAlreadyExist { get; set; } = false;
    }
}
