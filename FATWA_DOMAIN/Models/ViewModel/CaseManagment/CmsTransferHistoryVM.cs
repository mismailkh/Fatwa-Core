using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    public class CmsTransferHistoryVM : TransactionalBaseModel
    {
        [Key]
        public Guid TransferHistoryId { get; set; }
        //public Guid? RequestId { get; set; }
        public int StatusId { get; set; }
        public string? StatusEn { get; set; }
        public string? StatusAr { get; set; }
        public int SectorFrom { get; set; }
        public string? SectorFromEn { get; set; }
        public string? SectorFromAr { get; set; }
        public int SectorTo { get; set; }
        public string? SectorToEn { get; set; }
        public string? SectorToAr { get; set; }
        public string? Reason { get; set; }
        public string? UserName_En { get; set; }
        public string? UserName_Ar { get; set; }
        
    }
}
