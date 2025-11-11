using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("LMS_LITERATURE_PURCHASE")]
    //<History Author = 'Aqeel Altaf' Date='2022-03-16' Version="1.0" Branch="master"> Add purchase entity</History>
    public partial class LmsLiteraturePurchase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurchaseId
        {
            get;
            set;
        }

        public int LiteratureId
        {
            get;
            set;
        }
        public LmsLiterature? LmsLiterature { get; set; }

        public DateTime? Date
        {
            get;
            set;
        }

        public string Location
        {
            get;
            set;
        }

        public decimal Price
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
