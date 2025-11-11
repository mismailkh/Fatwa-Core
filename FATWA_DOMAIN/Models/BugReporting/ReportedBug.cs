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
    [Table("BUG_REPORTED")]
    public class ReportedBug : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public string PrimaryBugId { get; set; }
        public int? ApplicationId { get; set; }
        public int? ModuleId { get; set; }
        public string? ScreenReference { get; set; }
        public int? TypeId { get; set; }
        public string? Description { get; set; }
        public int? ShortNumber { get; set; }
        public int? StatusId { get; set; }
        public string? Subject { get; set; }

        public DateTime? ResolutionDate { get; set; }
        [NotMapped]
        public string Type_En { get; set; }
        [NotMapped]
        public string Type_Ar { get; set; }
        [NotMapped]
        public List<int>? DeletedAttachementIds { get; set; } = new List<int>();

    }
}
