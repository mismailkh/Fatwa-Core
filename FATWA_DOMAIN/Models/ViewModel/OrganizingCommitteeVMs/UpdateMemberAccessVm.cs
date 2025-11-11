using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.OrganizingCommitteeVMs
{
    public class UpdateMemberAccessVm
    {
        public List<CommitteeMembersVMs> FilteredMembers { get; set; } = new List<CommitteeMembersVMs>();
        public List<CommitteeMembersVMs> checkedMembers { get; set; } = new List<CommitteeMembersVMs>();
        public List<CommitteeMembersVMs> UncheckedMembers { get; set; } = new List<CommitteeMembersVMs>();

    }
}
