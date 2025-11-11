using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("LMS_LITERATURE_BARCODE")]
    //<History Author = 'Aqeel Altaf' Date='2022-03-16' Version="1.0" Branch="master"> Add barcode entity</History>
    public partial class LmsLiteratureBarcode
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BarcodeId
        {
            get;
            set;
        }

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

        public string BarCodeNumber
        {
            get;
            set;
        }


        public string? CreatedBy
        {
            get;
            set;
        }

        public DateTime? CreatedDate
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

        public bool? IsDeleted
        {
            get;
            set;
        }

        public int? LiteratureId
        {
            get;
            set;
        }
        public bool Active
        {
            get;
            set;
        }
        public bool IsBorrowed
        {
            get;
            set;
        }
        public string? RFIDValue 
        {
            get;
            set;
        }
        //public LmsLiterature? LmsLiterature { get; set; }
        [NotMapped]
        public long? RFID { get; set; } 
    }

}
