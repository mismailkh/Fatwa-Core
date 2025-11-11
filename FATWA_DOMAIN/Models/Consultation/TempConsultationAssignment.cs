using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FATWA_DOMAIN.Models.BaseModels;

namespace FATWA_DOMAIN.Models.Consultation
{
    [Table("TEMP_COMS_CONSULTATION_ASSIGNMENT")]
    public class TempConsultationAssignment : TransactionalBaseModel
    {
        [Key]
        public Guid TempAssignmentId { get; set; }
        public Guid ReferenceId { get; set; }
        public string? LawyerId { get; set; }
        public string? SupervisorId { get; set; }
        public string? Remarks { get; set; }
        public bool IsPrimary { get; set; }
    }
}
