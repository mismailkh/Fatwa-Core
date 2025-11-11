using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.WorkflowModels;

//< History Author = 'Hassan Abbas' Date = '2022-11-01' Version = "1.0" Branch = "master" > Case Template Model</History>

namespace FATWA_DOMAIN.Models.CaseManagment
{
    [Table("CMS_TEMPLATE")]
    public class CaseTemplate : TransactionalBaseModel
    {
        [Key]
        public int Id { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
        public string Content { get; set; }
        public bool IsActive { get; set; }
        public int AttachmentTypeId { get; set; }
        public bool IsG2GStamp { get; set; }
        public bool IsTimeStamp { get; set; }
        [NotMapped]
        public List<CaseTemplateParameter> Parameters { get; set; } = new List<CaseTemplateParameter>();
    }
}
