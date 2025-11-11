using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.Dms
{
    [Table("MOJ_IMAGE_DOCUMENT")]
    public class MojDocument:TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string CANNumber { get; set; }
        public string CaseNumber { get; set; }
        public int AttachmentTypeId { get; set; }
        public DateTime DocumentDate { get; set; }
        public string FileName { get; set; }
        public string StoragePath { get; set; }
    }
}
