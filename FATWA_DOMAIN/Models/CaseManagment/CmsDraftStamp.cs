using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    [Table("CMS_DRAFT_STAMP")]
    public class CmsDraftStamp : TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Content { get; set; }
        [NotMapped]
        public List<CaseTemplateParameter> Parameters { get; set; } = new List<CaseTemplateParameter>();
    }
}
