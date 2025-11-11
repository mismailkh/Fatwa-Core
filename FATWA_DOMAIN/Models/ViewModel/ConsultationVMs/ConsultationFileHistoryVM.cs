using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.ConsultationVMs
{
    public class ConsultationFileHistoryVM : TransactionalBaseModel
    {
        public Guid? FileId {get; set;}
        public Guid? HistoryId {get; set;}
        public string? Remarks { get; set; }
        public int? RequestTypeId { get; set; }
        public string? StatusNameEn { get; set; }
        public string? StatusNameAr { get; set; }
        public string? EventNameEn { get; set; }
        public string? EventNameAr { get; set; }
        public string? UserNameEn { get; set; }
        public string? UserNameAr { get; set; }

    }
}
