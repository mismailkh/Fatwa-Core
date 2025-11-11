using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.OrganizingCommitteeVMs
{
    public partial class CommitteeDecisionStatusVM
    {
        public Guid ReferenceId { get; set; }
        public int StatusId { get; set; }
        public string? Reason { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        [NotMapped]
        public string LoginUserId { get; set; }
    }
}
