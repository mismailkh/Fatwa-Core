using FATWA_DOMAIN.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.BugReporting
{
    [Table("BUG_STATUS_G2G_LKP")]
    public class BugStatus:TransactionalBaseModel
    {
        [Key]
        public int Id { get; set; }
        public string Value_En { get; set; }
        public string Value_Ar { get; set; }
    }
}
