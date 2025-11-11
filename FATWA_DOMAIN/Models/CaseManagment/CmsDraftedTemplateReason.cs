using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    //< History Author = 'Umer Zaman' Date = '2023-01-30' Version = "1.0" Branch = "master" > Drafted Case Draft Reason Model</History>
    [Table("CMS_DRAFTED_TEMPLATE_REASON")]
    public class CmsDraftedTemplateReason : TransactionalBaseModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid DraftedTemplateVersionId { get; set; }
        public decimal VersionNumber { get; set; }
        public string Reason { get; set; }
        public int StatusId { get; set; }

    }
}
