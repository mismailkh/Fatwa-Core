using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CommunicationModels
{
    [Table("COMM_STOP_EXECUTION_REJECTION_REASON")]
    public class StopExecutionRejectionReason
    {
        [Key]
        public Guid Id { get; set; }
        public string CommunicationId { get; set; }
        public string? Reason { get; set; }
        [NotMapped]
        public string? Payload { get; set; }
        [NotMapped]
        public string? CreatedBy { get; set; }
        [NotMapped]
        public int? SectorTypeId { get; set; } 
        [NotMapped]
        public string? AssignById { get; set; }


    }
}
