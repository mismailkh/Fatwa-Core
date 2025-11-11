using FATWA_DOMAIN.Models.OrganizingCommittee;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.OrganizingCommitteeVMs
{
    public partial class SaveCommitteeVM
    {
        public Committee committee { get; set; } = new Committee();
        public List<TempCommitteeTaskVm> committeeTasks { get; set; } = new List<TempCommitteeTaskVm>();
        public List<TempCommitteeTaskVm> deletedTasks { get; set; } = new List<TempCommitteeTaskVm>();
        public List<CommitteeMembersVMs> committeeMembers { get; set; } = new List<CommitteeMembersVMs>();
        public List<CommitteeMembersVMs> deletedMembers { get; set; } = new List<CommitteeMembersVMs>();
        public List<TempCommitteeTask> tempCommitteeTasks { get; set; } = new List<TempCommitteeTask>();
        public List<CommitteeMembersVMs> AllMembers { get; set; } = new List<CommitteeMembersVMs>();

        public string? LoginUserId { get; set; }
        public int? SectorTypeId { get; set; }
        [NotMapped]
        public ObservableCollection<TempAttachementVM> AdditionalTempFiles { get; set; } = new ObservableCollection<TempAttachementVM>();
        [NotMapped]
        public ObservableCollection<TempAttachementVM> MandatoryTempFiles { get; set; } = new ObservableCollection<TempAttachementVM>();
        [NotMapped]
        public List<int> DeletedAttachementIds { get; set; } = new List<int>();
    }

}
