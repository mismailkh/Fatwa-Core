using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.LLSLegalPrinciple
{
    //<History Author = 'Umer Zaman' Date = '2024-04-16' Version = "1.0" Branch = "master">Create new model to manage category lookups values</History>

    [Table("LLS_LEGAL_PRINCIPLE_CATEGORY_FTW_LKP")]
    public class LLSLegalPrincipleCategory : TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }
        public int? ParentId { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }

        [NotMapped]
        public bool Expanded { get; set; } = true;
        [NotMapped]
        public bool HasSubFolders { get; set; } = true;
        
    }
}
