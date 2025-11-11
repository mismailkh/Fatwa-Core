
//< History Author = 'Hassan Abbas' Date = '2023-08-21' Version = "1.0" Branch = "master" > Case Template Sections Model</History>

using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    [Table("CMS_TEMPLATE_SECTION")]
    public class CaseTemplateSection
    {
        [Key]
        public Guid Id { get; set; }
        public int TemplateId { get; set; }
        public int SectionId { get; set; }
    }
}
