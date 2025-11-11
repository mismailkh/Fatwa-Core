using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.LLSLegalPrinciple
{
    //<History Author = 'Umer Zaman' Date = '2024-04-16' Version = "1.0" Branch = "master">Create new model to manage new principle and category relations</History>

    [Table("LLS_LEGAL_PRINCIPLE_CONTENT_CATEGORY")]
    public class LLSLegalPrincipleContentCategory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid PrincipleContentId { get; set; }
        public int CategoryId { get; set; }
    }
}
