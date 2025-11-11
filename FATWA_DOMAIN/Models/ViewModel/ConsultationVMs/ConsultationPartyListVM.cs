using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.ConsultationVMs
{
    public class ConsultationPartyListVM
    {
        public string? RepresentativeName { get; set; }
        public Guid? ConsultationRequestId { get; set; }
        public string? CivilID_CRN { get; set; }
        public string? Designation { get; set; }
        public string? PartyTypeEn { get; set; }
        public string? PartyTypeAr { get; set; }
        public string? Address { get; set; }
        public string? CompanyName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Fax { get; set; }
        public string? POBox { get; set; } 
        public string? POCode { get; set; }
       


    }
}
