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
    [Table("BUG_ISSUE_TYPE_G2G_LKP")]
    public class BugIssueType : TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Type_En { get; set; }
        public string Type_Ar { get; set; }
        public string? Description { get; set; }
        public bool IsSystemGenerated { get; set; }
    }
}
