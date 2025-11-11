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
    [Table("COMS_CONSULTATION_REQUEST_STATUS_HISTORY")]

    public class ComsConsultationRequestHistory : TransactionalBaseModel
    {
        [Key]
        public Guid HistoryId { get; set; }
        public Guid ConsultationRequestId { get; set; }
        public int EventId { get; set; }
        public string? Remarks { get; set; }
        public int StatusId { get; set; }

    }
}
