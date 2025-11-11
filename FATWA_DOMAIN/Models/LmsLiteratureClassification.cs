using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("LMS_LITERATURE_CLASSIFICATION")]
    //<History Author = 'Aqeel Altaf' Date='2022-03-16' Version="1.0" Branch="master"> Add many to many relation and allow it should save with relation</History>
    //<History Author = 'Zain Ul Islam' Date='2022-08-01' Version="1.0" Branch="master"> Lms Literature Classification Service Generic Solution Reverted</History> 
    public partial class LmsLiteratureClassification
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ClassificationId { get; set; }
        [NotMapped]
        public ICollection<LmsLiterature>? LmsLiterature { get; set; }
        public string? Name_En { get; set; }
        public string? Name_Ar { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public bool IsDeleted { get; set; }
      
    }

    public class DataEnvelope<T>
    {
        public int Total { get; set; }
        public List<T> Data { get; set; }
    }
}
