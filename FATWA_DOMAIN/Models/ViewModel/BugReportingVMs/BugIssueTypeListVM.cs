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
    public class BugIssueTypeListVM : TransactionalBaseModel
    {
        [Key]
        public int Id { get; set; }
        public string Type_En { get; set; }
        public string Type_Ar { get; set; }
        public bool IsSystemGenerated { get; set; }
        public string? CreatedByEn {  get; set; }
        public string? CreatedByAr {  get; set; }
        public string? Description {  get; set; }

    }
}
