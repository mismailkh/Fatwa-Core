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
    [Table("CMS_DRAFTED_TEMPLATE_VERSIONS_VICEHOS")]
    public class CmsDraftedTemplateVersionVicehos : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid VersionId { get; set; }
        public string ReviewerUserId { get; set; }
    }
}
