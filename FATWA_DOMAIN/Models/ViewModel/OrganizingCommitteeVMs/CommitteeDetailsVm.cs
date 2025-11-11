using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.OrganizingCommitteeVMs
{
    public class CommitteeDetailsVm : TransactionalBaseModel
    {
        public Guid Id { get; set; }
        public string? CommitteeNumber { get; set; }
        public string? Subject { get; set; }
        public string? Description { get; set; }
        public int? Duration { get; set; }
        public DateTime CircularIssueDate { get; set; }
        public int StatusId { get; set; }
        public string? StatusEn { get; set; }
        public string? StatusAr { get; set; }
        public string? CircularReferenceNo { get; set; }
        public DateTime? CommencementDate { get; set; }
        public DateTime? ExpectedEndDate { get; set; }
        [NotMapped]
        public string? MemeberNameEn { get; set; }
        [NotMapped]
        public string? MemeberNameAr { get; set; }
    }

}