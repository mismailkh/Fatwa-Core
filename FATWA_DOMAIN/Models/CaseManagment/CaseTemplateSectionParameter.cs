
//< History Author = 'Hassan Abbas' Date = '2023-08-21' Version = "1.0" Branch = "master" > Case Template Sections Model</History>

using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    [Table("CMS_TEMPLATE_SECTION_PARAMETER")]
    public class CaseTemplateSectionParameter
    {
        [Key]
        public Guid Id { get; set; }
        public Guid TemplateSectionId { get; set; }
        public int ParameterId { get; set; }
    }
}
