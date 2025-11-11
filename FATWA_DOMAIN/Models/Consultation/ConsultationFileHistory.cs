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
    [Table("COMS_CONSULTATION_FILE_STATUS_HISTORY")]
    public class ConsultationFileHistory : TransactionalBaseModel
    {
        [Key]
        public Guid HistoryId { get; set; }
        public Guid FileId { get; set; }
        public int EventId { get; set; }
        public string? Remarks { get; set; }
        public int StatusId { get; set; }

    }
}
