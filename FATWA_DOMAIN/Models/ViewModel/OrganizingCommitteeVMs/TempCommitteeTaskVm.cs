using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.OrganizingCommitteeVMs
{
    public class TempCommitteeTaskVm
    {
        [Key]
        public Guid Id { get; set; }
        public Guid CommitteeId { get; set; }
        public string? MemberId { get; set; }
        public string? TaskName { get; set; }
        public DateTime? TaskDeadline { get; set; } = DateTime.Now;
        public string? MemeberNameEn { get; set; }
        public string? MemeberNameAr { get; set; }
        public string? Description { get; set; }


    }
}
