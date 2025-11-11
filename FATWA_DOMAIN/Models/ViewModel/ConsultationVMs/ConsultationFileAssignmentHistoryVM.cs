using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.ConsultationVMs
{
    public class ConsultationFileAssignmentHistoryVM
    {
        public Guid HistoryId { get; set; }
        public Guid ReferenceId { get; set; }
        public string? AssigneeId { get; set; }
        public string? Remarks { get; set; }
        public string? AssigneeNameEn { get; set; }
        public string? AssigneeNameAr { get; set; }
        public string? AssignorNameEn { get; set; }
        public string? AssignorNameAr { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
