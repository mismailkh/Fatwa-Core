using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    //<History Author = 'Umer Zaman' Date='2022-03-15' Version="1.0" Branch="own"> create class & add properties</History>
    [Table("LMS_LITERATURE_TYPE")]
    public partial class LmsLiteratureType
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TypeId
        {
            get;
            set;
        }
        [NotMapped]
        public ICollection<LmsLiterature>? LmsLiterature { get; set; }
        public string? Name_En
        {
            get;
            set;
        }
        public string? Name_Ar
        {
            get;
            set;
        }
        public string CreatedBy
        {
            get;
            set;
        }
        public DateTime CreatedDate
        {
            get;
            set;
        }
        public string? ModifiedBy
        {
            get;
            set;
        }
        public DateTime? ModifiedDate
        {
            get;
            set;
        }
        public string? DeletedBy
        {
            get;
            set;
        }
        public DateTime? DeletedDate
        {
            get;
            set;
        }
        public bool IsDeleted
        {
            get;
            set;
        }
    }
}
