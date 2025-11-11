using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.TaskVMs
{
    public class DraftListVM
    {
        public int DraftNumber { get; set; }
        public string Name { get; set; }
        public string StatementTypeEn { get; set; }
        public string StatementTypeAr { get; set; }
        public string FileNumber { get; set; }
        public string StatusEn { get; set; }
        public string StatusAr { get; set; }
        public DateTime Date { get; set; }
        public DateTime? DeletedDate { get; set; }
        public String? DeletedBy { get; set; }
        public bool IsDeleted { get; set; } 
        public Guid DraftId { get; set; }
    }
}


