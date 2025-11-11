using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.Consultation
{
    [Table("COMS_CONSULTATION_ASSIGNMENT_HISTORY")]
    public class ConsultationAssignmentHistory :TransactionalBaseModel
    {
        [Key]
        public Guid HistoryId { get; set; }
        public Guid ReferenceId { get; set; }
        public string? AssigneeId { get; set; }
        public string? Remarks { get; set; }
    }
}
