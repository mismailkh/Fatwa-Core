using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.OrganizingCommittee
{
    [Table("OC_COMMITTEE")]
    public partial class Committee : TransactionalBaseModel
    {
        [Key]
        public Guid Id { get; set; }
        public string CommitteeNumber { get; set; }
        public string? Subject { get; set; }
        public string? Description { get; set; }
        public int? Duration { get; set; }
        public DateTime CircularIssueDate { get; set; }
        public int StatusId { get; set; }
        public int? ShortNumber { get; set; }
        public string? CircularReferenceNo { get; set; }
        public DateTime? CommencementDate { get; set; }
        public DateTime? ExpectedEndDate { get; set; }
        [NotMapped]
        public List<int>? DeletedAttachementIds { get; set; } = new List<int>();
    }
}