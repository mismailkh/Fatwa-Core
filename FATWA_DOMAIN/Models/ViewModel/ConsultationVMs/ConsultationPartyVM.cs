using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.ConsultationVMs
{

    public class ConsultationPartyVM : TransactionalBaseModel
    {
        [Key]
        public Guid PartyId { get; set; }
        public Guid ConsultationRequestId { get; set; }
        public int PartyTypeId { get; set; }
        public string? PartyTypeNameEn { get; set; }
        public string? PartyTypeNameAr { get; set; }
        public string RepresentativeName { get; set; }
        public string Designation { get; set; }
        public string? CivilID_CRN { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Fax { get; set; }
        public string? POBox { get; set; }
        public string? POCode { get; set; }
        public string? CompanyName { get; set; }
        public string Address { get; set; }


    }
}
