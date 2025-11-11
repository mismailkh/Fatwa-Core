using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.OrganizingCommitteeVMs
{
    public class AdvanceSearchCommitteListVm : GridPagination
    {
        public string? CommitteeNumber { get; set; }
        public string? Subject { get; set; }
        public int? Duration { get; set; }
        public int? StatusId { get; set; }
        public DateTime? CreatedDateFrom { get; set; }
        public DateTime? CreatedDateTo { get; set; }
        public string? UserId { get; set; }
    }

}
