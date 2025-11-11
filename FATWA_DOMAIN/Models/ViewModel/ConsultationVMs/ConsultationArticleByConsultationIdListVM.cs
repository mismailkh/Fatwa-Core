using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.ConsultationVMs
{
    public class ConsultationArticleByConsultationIdListVM
    {

        
        public string? SectionName { get; set; }    
        public int? ArticleNumber { get; set;} 
        public string? ArticleTitle { get; set;}
        public string? StatusEn { get; set;}
        public string? StatusAr { get; set;}
    }
}
