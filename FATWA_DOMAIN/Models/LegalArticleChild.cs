using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("LEGAL_ARTICLE_CHILD", Schema = "dbo")]
    public partial class LegalArticleChild
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid ParentId { get; set; }
        public Guid ArticleChildId { get; set; }
    }
}
