using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.OrganizingCommitteeVMs
{
    public class CommitteeMembersVMs
    {
        public int Id { get; set; }
        public Guid? CommitteeId { get; set; }
        public string? MemberId { get; set; }
        public int? CommitteeRoleId { get; set; } = 0;
        public bool? IsReadOnly { get; set; }
        public string? MemeberNameEn { get; set; }
        public string? MemeberNameAr { get; set; }
        public string? RoleNameEn { get; set; }
        public string? RoleNameAr { get; set; }
        public string? SectorTypeEn { get; set; }
        public string? SectorTypeAr { get; set; }
        public int? TotalCommittee { get; set; }
        public int? TotalTasks { get; set; }
        public DateTime? LastActivityDate { get; set; }
        [NotMapped]
        public IEnumerable<int> SelectedMembers { get; set; } = new List<int>();
        [NotMapped]
        public string? CreatedBy { get; set; }


    }

}
