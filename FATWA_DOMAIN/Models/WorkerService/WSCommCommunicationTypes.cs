using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.WorkerService
{
    [Table("WS_COMM_COMMUNICATION_TYPES")]
    public partial class WSCommCommunicationTypes : TransactionalBaseModel
    {
        [Key]
        public int CommunicationTypeId { get; set; }
        public string NameAr { get; set; }
        public string NameEn { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool IsActive { get; set; }

    }
}
