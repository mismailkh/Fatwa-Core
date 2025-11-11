using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("LDS_DOCUMENT_COMMENTS")]
    public partial class LdsDocumentComment
    {
        [Key]
        public Guid CommentId { get; set; }
        public Guid DocumentId { get; set; }
        public string Comment { get; set; }
        public string Status { get; set; }
        public string? Reason { get; set; }

        public string Createdby { get; set; }
        public DateTime CreatedDate { get; set; }
        public bool IsDeleted { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
