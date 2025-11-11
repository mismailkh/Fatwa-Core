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
    [Table("CMS_DRAFTED_TEMPLATE_VERSION_LOGS")]
    public class CmsDraftedTemplateVersionLogs : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public Guid VersionId { get; set; }
        public Guid UserId { get; set; }
        public int ActionId { get; set; }
        public string ReviewerUserId { get; set; }
    }
}
