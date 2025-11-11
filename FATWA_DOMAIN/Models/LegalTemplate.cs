using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("LEGAL_TEMPLATE")]
    public class LegalTemplate
    {
        [Key]
        public Guid TemplateId { get; set; }
        public string Template_Name { get; set; }
        public int Legislation_Type { get; set; }
        public bool IsDefault { get; set; }
        [NotMapped]
        public List<int>? SelectedCheckBoxValues { get; set; } = new List<int>();
	}
}
