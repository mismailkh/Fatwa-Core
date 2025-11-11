using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("Submodule")]
    //<History Author = 'Muhammad Zaeem' Date='2023-05-22' Version="1.0" Branch="master"> Add SubModule</History>
    public partial class SubModule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name_Ar { get; set; }
        public string Name_En { get; set; }
    }
}
