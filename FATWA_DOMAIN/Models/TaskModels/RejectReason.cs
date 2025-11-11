using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.TaskModels
{
    [Table("Rejection")]
    public class RejectReason : TransactionalBaseModel
    {
        [Key]
        public Guid RejectionId { get; set; }
        public Guid ReferenceId { get; set; }   
        public string Reason { get; set; }  
    }
}
