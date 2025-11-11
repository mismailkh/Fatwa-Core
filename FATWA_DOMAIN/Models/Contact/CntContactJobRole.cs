using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.Contact
{
    [Table("CNT_CONTACT_JOB_ROLE_LKP")]
    public class CntContactJobRole
    {
        [Key]
        public int RoleId { get; set; }
        public string NameEn { get; set; }
        public string NameAr { get; set; }
    }
}
