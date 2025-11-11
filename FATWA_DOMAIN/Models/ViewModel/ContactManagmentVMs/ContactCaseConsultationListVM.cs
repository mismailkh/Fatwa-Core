using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.ContactManagmentVMs
{
    public partial class ContactCaseConsultationListVM
    {
        public Guid? FileId { get; set; }   
        public string? FileNumber { get; set; } 
        public string? FileName { get; set; } 
        public DateTime? CreatedDate { get; set; }
        public string? StatusEn { get; set; }
        public string? StatusAr { get; set; }
        public string? RequestTypeEn { get; set; }
        public string? RequestTypeAr { get; set; }
    }
}
