using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("LPS_PRINCIPLE_COMMENTS")]
    public partial class LpsPrincipleComment
    {
       
        [Key]
        public Guid CommentId { get; set; }
        public Guid PrincipleId { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }
        public string? Reason { get; set; }

        public string Createdby { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
