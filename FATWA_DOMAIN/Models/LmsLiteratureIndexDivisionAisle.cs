using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    //<History Author = 'Umer Zaman' Date='2022-07-06' Version="1.0" Branch="own">add properties</History>

    [Table("LMS_LITERATURE_INDEX_DIVISION_AISLE")]
    public partial class LmsLiteratureIndexDivisionAisle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DivisionAisleId
        {
            get;
            set;
        }
        public int IndexId
        {
            get;
            set;
        }
        public string DivisionNumber
        {
            get;
            set;
        }
        public string AisleNumber
        {
            get;
            set;
        }
        public DateTime DivisionCreationDate
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
