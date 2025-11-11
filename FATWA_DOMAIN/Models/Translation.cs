using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("tTranslation")]
    //<History Author = 'Aqeel Altaf' Date='2022-03-23' Version="1.0" Branch="master"> Add many to many relation and allow it should save with relation</History>
    public partial class Translation
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TranslationId { get; set; }
        public string? Url { get; set; }
        public string TranslationKey { get; set; }
        public int? TranslationType { get; set; }
        public string Value_Ar { get; set; }
        public string? Value_En { get; set; }
        public string? ReferenceScreen { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }

    }
}
