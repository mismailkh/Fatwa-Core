using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.LLSLegalPrinciple
{
    [Table("LLS_Legal_PRINCIPLE_COMMENTS")]
    public  class LLSLegalPrincipleComment
    {
        [Key]
        public Guid CommentId { get; set; }
        public Guid PrincipleId { get; set; }
        public string? Comment { get; set; }
        public string Status { get; set; }
        public string? Reason { get; set; }
        public string Createdby { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
