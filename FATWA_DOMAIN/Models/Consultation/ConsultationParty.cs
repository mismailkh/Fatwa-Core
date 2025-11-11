
using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.Consultation
{
    [Table("COMS_CONSULTATION_PARTY")]
    public class ConsultationParty : TransactionalBaseModel
    {
        [Key]
        public Guid PartyId { get; set; }
        public Guid ConsultationRequestId { get; set; }
        public int PartyTypeId { get; set; }
        public string RepresentativeName { get; set; }
        public string Designation { get; set; }
        public string? CivilID_CRN { get; set; }
        public string? Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Fax { get; set; }
        public string? POBox { get; set; }
        public string? POCode { get; set; }
        public string? CompanyName { get; set; }
        public string Address { get; set; }
        [NotMapped]
        public string? PartyTypeNameEn { get; set; }
        [NotMapped]
        public string? PartyTypeNameAr { get; set; }
        [NotMapped]
        public UploadedDocument? UploadedDocument { get; set; }
		[NotMapped]
		public bool IsDetailView { get; set; } = false;
	}
}
