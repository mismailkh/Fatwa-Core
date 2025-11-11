using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    public class CmsRegisteredCasesAssigneeVM
    {
        [Key]
        public Guid? CaseId { get; set; }
        public string? CaseNumber { get; set; }
        public int? CourtTypeId { get; set; }
        public bool? IsFinalJudgement { get; set; }
    }
}
