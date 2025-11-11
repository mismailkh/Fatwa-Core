using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("LMS_LITERATURE_DETAILS_LMS_LITERATURE_AUTHOR")]
    //<History Author = 'Aqeel Altaf' Date='2022-03-16' Version="1.0" Branch="master"> Add barcode entity</History>
    public partial class LmsLiteratureDetailsLmsLiteratureAuthor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id
        {
            get;
            set;
        }
        public int LiteratureId
        {
            get;
            set;
        }
        public int AuthorId
        {
            get;
            set;
        }
    }

}
