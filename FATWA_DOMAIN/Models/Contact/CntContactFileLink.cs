using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.Contact
{
    [Table("CNT_CONTACT_FILE_LINK")]
    public class CntContactFileLink : TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
		public Guid ContactId { get; set; }
		public Guid ReferenceId { get; set; }
		public int ContactLinkId { get; set; }
		public int ModuleId { get; set; }
    }
}
