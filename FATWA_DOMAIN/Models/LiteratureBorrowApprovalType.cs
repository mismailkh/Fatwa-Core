using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("LMS_LITERATURE_BORROW_APPROVAL_STATUS")]
    public partial class LiteratureBorrowApprovalType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DecisionId { get; set; }
        public string? Name { get; set; }
        public string? Name_Ar { get; set; }

    }
}
