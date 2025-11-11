using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models
{
    [Table("LEGAL_ARTICLE_SOURCE", Schema = "dbo")]
    public partial class  LegalArticleSource
    {
        [Key]
        public int Id { get; set; }
        public string Name_En { get; set; }
        public string Name_Ar { get; set; }
    }
}
