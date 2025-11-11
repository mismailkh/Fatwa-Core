using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("LEGAL_NOTE", Schema = "dbo")]
    public partial class LegalNote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int NoteId { get; set; }
        public Guid ParentId { get; set; }
        public string? Note_Text { get; set; }
        public string Note_Location { get; set; }
        public DateTime Note_Date { get; set; }
    }
}
