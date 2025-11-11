using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.Dms
{
    [Table("DS_ATTACHMENT_TYPE_SIGNING_METHODS")]
    public class DSAttachmentTypeSigningMethods : TransactionalBaseModel
    {
        [Key]
        public int Id { get; set; }
        public int AttachmentTypeId { get; set; }
        public int MethodId { get; set; }
    }
}
