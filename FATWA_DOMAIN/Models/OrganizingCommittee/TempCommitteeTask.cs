using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.OrganizingCommittee
{
    [Table("OC_TEMP_COMMITTEE_TASKS")]
    public partial class TempCommitteeTask
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CommitteeId { get; set; }
        public string? MemberId { get; set; }
        public string? TaskName { get; set; }
        public string? Description { get; set; }
        public DateTime? TaskDeadline { get; set; }

    }
}
