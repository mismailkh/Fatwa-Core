using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.Consultation
{
    //< History Author = 'Muhammad Zaeem' Date = '2023-02-02' Version = "1.0" Branch = "master" > Drafted consultation Draft Reason Model</History>
    [Table("COMS_DRAFTED_TEMPLATE_REASON")]
    public class ComsDraftedTemplateReason : TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid DraftedTemplateId { get; set; }
        public double VersionNumber { get; set; }
        public string Reason { get; set; }
        public int StatusId { get; set; }

    }
}
