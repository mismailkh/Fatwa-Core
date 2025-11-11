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
    public class CmsCaseFileTransferRequestDetailVM
    {
        [Key]
        public Guid Id { get; set; }
        public string? SectorFromNameEn { get; set; }
        public string? SectorFromNameAr { get; set; }
        public string? SectorToNameEn { get; set; }
        public string? SectorToNameAr { get; set; }
        public string? StatusNameEn { get; set; }
        public string? StatusNameAr { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? Description { get; set; }
        public int? StatusId { get; set; }
        [NotMapped]
        public string? RejectionReason { get; set; }
        [NotMapped]
        public string? UserName { get; set; }
        [NotMapped]
        public RejectReason rejectReason { get; set; } = new RejectReason();
    }
}
