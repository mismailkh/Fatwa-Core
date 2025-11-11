using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    //<History Author = 'Umer Zaman' Date='2022-03-18' Version="1.0" Branch="own">add properties</History>

    [Table("LMS_LITERATURE_INDEX")]
    public partial class LmsLiteratureIndex
    {
        public ICollection<LmsLiterature>? LmsLiterature { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IndexId
        {
            get;
            set;
        }
        public int ParentId
        {
            get;
            set;
        }
        public string IndexParentNumber
        {
            get;
            set;
        }
        public string Name_En
        {
            get;
            set;
        }
        public string Name_Ar
        {
            get;
            set;
        }
        public string IndexNumber
        {
            get;
            set;
        }
        public DateTime IndexCreationDate
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
