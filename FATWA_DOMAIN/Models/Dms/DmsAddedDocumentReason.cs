using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.Dms
{
    //< History Author = 'Hassan Abbas' Date = '2023-06-20' Version = "1.0" Branch = "master" > Added Document Reason Model</History>
    [Table("DMS_ADDED_DOCUMENT_REASON")]
    public class DmsAddedDocumentReason : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid AddedDocumentVersionId { get; set; }
        public string? Reason { get; set; }
    }
}
