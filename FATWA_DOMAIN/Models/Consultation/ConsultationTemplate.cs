using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.Consultation
{
    [Table("COMS_TEMPLATE")]
    public class ConsultationTemplate : TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TemplateId { get; set; }
        public string Name { get; set; }
        public int? AttachmentTypeId { get; set; }
        public bool IsActive { get; set; }

    }
}
