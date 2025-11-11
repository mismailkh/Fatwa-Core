using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.BugReporting
{
    [Table("BUG_TYPE_MODULE_ASSIGNMENT")]
    public class BugModuleTypeAssignment:TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public int ModuleId { get; set; }
        public int BugTypeId { get; set; }
        public int? PriorityId { get; set; }
        public int? SeverityId { get; set; }
        public int? ApplicationId { get; set; }
        [NotMapped]
        public IEnumerable<int> SelectedModule { get; set; }
    }
}
