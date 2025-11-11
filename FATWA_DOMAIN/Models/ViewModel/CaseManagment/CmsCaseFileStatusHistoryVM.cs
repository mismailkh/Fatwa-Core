using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    public class CmsCaseFileStatusHistoryVM : TransactionalBaseModel
    {
        [Key]
        public Guid HistoryId { get; set; } 
        public int StatusId { get; set; } 
        public int EventId { get; set; }
        public Guid? FileId { get; set; } 
        public string? Remarks { get; set; } 
        public string? StatusNameEn { get; set; }
        public string? StatusNameAr { get; set; }
        public string? EventNameEn { get; set; }
        public string? EventNameAr { get; set; }
        public string? UserNameEn { get; set; }
        public string? UserNameAr { get; set; } 
    }
}
