using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.OrganizingCommitteeVMs
{
    public class CommitteeListVm : TransactionalBaseModel
    {

        public Guid Id { get; set; }
        public string CommitteeNumber { get; set; }
        public string? Subject { get; set; }
        public string? Description { get; set; }
        public int? Duration { get; set; }
        public DateTime CircularIssueDate { get; set; }
        public int StatusId { get; set; }
        public string? StatusEn { get; set; }
        public string? StatusAr { get; set; }
        public string? RoleNameEn { get; set; }
        public string? RoleNameAr { get; set; }
        public int TotalCount { get; set; } = 0;
    }

}
