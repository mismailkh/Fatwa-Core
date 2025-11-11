using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.OrganizingCommitteeVMs
{
    public class CommitteeTaskVm
    {
        [Key]
        public Guid TaskId { get; set; }
        public int? TaskNumber { get; set; }
        public string? Name { get; set; }
        public DateTime? TaskDeadline { get; set; }
        public string? RoleNameEn { get; set; }
        public string? RoleNameAr { get; set; }
        public string? MemberNameEn { get; set; }
        public string? MemberNameAr { get; set; }
        public string? StatusEn { get; set; }
        public string? StatusAr { get; set; }
        public string? Description { get; set; }
        [NotMapped]
        public Guid ReferenceId { get; set; }
        public string? MemberId { get; set; }

    }
}
