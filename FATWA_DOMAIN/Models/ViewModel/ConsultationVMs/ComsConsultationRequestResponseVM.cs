using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.ConsultationVMs
{
    public class ComsConsultationRequestResponseVM
    {
        
        public int? ResponseTypeId { get; set; }       
        public string? Number { get; set; }
        public DateTime? RequestOrFileDate { get; set; }
        public DateTime? RequestDate { get; set; }
        public string? ResponseTypeAr { get; set; }
        public string? ResponseTypeEn { get; set; }
        public DateTime? ResponseDate { get; set; }
        public string? Reason { get; set; }
        public string? Other { get; set; }
        public DateTime? DueDate { get; set; }
        public string? PriorityNameEN { get; set; }
        public string? PriorityNameAr { get; set; }
        public string? FrequencyNameEn { get; set; }
        public string? FrequencyNameAr { get; set; }
        public string? AdditionalGEUserEn { get; set; }
        public string? AdditionalGEUserAr { get; set; }
        public bool? IsUrgent { get; set; }
    }

}
