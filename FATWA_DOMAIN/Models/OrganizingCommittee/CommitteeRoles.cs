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
    [Table("OC_COMMITTEE_ROLE_LKP")]
    public partial class CommitteeRoles : TransactionalBaseModel
    {
        [Key]
        public int Id { get; set; }
        public string? NameEn { get; set; }
        public string? NameAr { get; set; }
    }
}
