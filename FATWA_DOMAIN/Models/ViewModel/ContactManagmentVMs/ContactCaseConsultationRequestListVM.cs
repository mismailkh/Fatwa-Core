using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.ContactManagmentVMs
{
    public partial class ContactCaseConsultationRequestListVM
    {
        public Guid? RequestId { get; set; }
        public string? RequestNumber { get; set; }
        public string? Subject { get; set; }
        public DateTime? RequestDate { get; set; }
        public string? CreatedBy { get; set; }
    }
}
