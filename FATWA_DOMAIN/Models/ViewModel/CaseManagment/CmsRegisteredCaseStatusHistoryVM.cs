using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    public class CmsRegisteredCaseStatusHistoryVM : TransactionalBaseModel
    {
        [Key]
        public Guid HistoryId { get; set; }
        public Guid? FileId { get; set; }
        public Guid? CaseId { get; set; }
        public int? StatusId { get; set; }
        public int? EventId { get; set; }
        public string? Remarks { get; set; }
        public string? StatusEn { get; set; }
        public string? StatusAr { get; set; }
        public string? EventEn { get; set; }
        public string? EventAr { get; set; }
        public string? UserNameEn { get; set; }
        public string? UserNameAr { get; set; }
    }
}
