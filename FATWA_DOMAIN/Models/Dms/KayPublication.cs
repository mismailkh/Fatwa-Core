using FATWA_DOMAIN.Models.BaseModels; 
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; 

namespace FATWA_DOMAIN.Models.Dms
{
    [Table("KAY_PUBLICATION_STG")]
    public class KayPublication: TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string EditionNumber { get; set; }
        public string EditionType { get; set; }
        public DateTime? PublicationDate { get; set; }
        public string FileTitle { get; set; }
        public string DocumentTitle { get; set; }
        public string StoragePath { get; set; }
        public Guid? ReferenceGuid { get; set; }
        public int? StartPage { get; set; }
        public int? EndPage { get; set; }
        public string PublicationDateHijri { get; set; }
        public bool IsFullEdition { get; set; }
    }
}
