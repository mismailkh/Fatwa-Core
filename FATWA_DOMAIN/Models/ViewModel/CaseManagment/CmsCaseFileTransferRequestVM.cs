using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    public class CmsCaseFileTransferRequestVM
    {
        [Key]
        public Guid Id { get; set; }
        public string? RequestNo { get; set; }
        public string? SectorToNameEn { get; set; }
        public string? SectorToNameAr { get; set; }
        public string? Description { get; set; }
        public string? StatusNameEn { get; set; }
        public string? StatusNameAr { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
