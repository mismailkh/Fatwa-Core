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
    [Table("OC_COMMITTEE_MEMBERS")]
    public partial class CommitteeMembers : TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid? CommitteeId { get; set; }
        public string? MemberId { get; set; }
        public int? CommitteeRoleId { get; set; }
        public bool IsReadOnly { get; set; }
        [NotMapped]
        public string? CommitteeNumber { get; set; }
        [NotMapped]
        public bool IsUpdate { get; set; }
    }
}
