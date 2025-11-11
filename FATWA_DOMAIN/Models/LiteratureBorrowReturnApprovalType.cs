using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("LMS_LITERATURE_BORROW_RETURN_APPROVAL_STATUS")]
    public partial class LiteratureBorrowReturnApprovalType
    {
        [Key]
        public int Id { get; set; }
        public string? NameEn { get; set; }
        public string? NameAr { get; set; }

    }
}
