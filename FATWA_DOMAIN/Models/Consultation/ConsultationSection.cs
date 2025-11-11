using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

//< History Author = 'Muhammad Zaeem' Date = '2023-1-2' Version = "1.0" Branch = "master" >Consultation section </History>

namespace FATWA_DOMAIN.Models.Consultation
{
    [Table("COMS_CONSULTATION_SECTION")]
    public partial class ConsultationSection
    {
        [Key]
        public Guid SectionId { get; set; }
        public Guid ConsultationRequestId { get; set; }
        public Guid? SectionParentId { get; set; } = Guid.Empty;
        public int SectionNumber { get; set; }
        public int? ParentId { get; set; } = 0;
        public string SectionTitle { get; set; }
        public string? SectionParentTitle { get; set; } = string.Empty;
    }
}
