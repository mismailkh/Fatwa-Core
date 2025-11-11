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
    [Table("OC_COMMITTEE_STATUS_LKP")]
    public partial class CommitteeStatus : TransactionalBaseModel
    {
        [Key]
        public int Id { get; set; }
        public string? NameEn { get; set; }
        public string? NameAr { get; set; }
    }
}
