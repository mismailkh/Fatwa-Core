using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("LMS_LITERATURE_AUTHOR")]
    //<History Author = 'Aqeel Altaf' Date='2022-03-16' Version="1.0" Branch="master"> Add barcode entity</History>
    public partial class LmsLiteratureAuthor:TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AuthorId
        {
            get;
            set;
        }

        public string FullName_En
        {
            get;
            set;
        }

        public string FullName_Ar
        {
            get;
            set;
        }

        public string? FirstName_En
        {
            get;
            set;
        }

        public string? FirstName_Ar
        {
            get;
            set;
        }

        public string? SecondName_En
        {
            get;
            set;
        }

        public string? SecondName_Ar
        {
            get;
            set;
        }

        public string? ThirdName_En
        {
            get;
            set;
        }

        public string? ThirdName_Ar
        {
            get;
            set;
        }

        public string Address_En
        {
            get;
            set;
        }

        public string Address_Ar
        {
            get;
            set;
        }

    }

}
