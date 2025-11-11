using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.OrganizingCommitteeVMs
{
    public class RejectionReasonVm
    {
        [Key]
        public Guid RejectionId { get; set; }
        public Guid ReferenceId { get; set; }
        public string Reason { get; set; }
        public string? StatusEn { get; set; }
        public string? StatusAr { get; set; }
    }

}
