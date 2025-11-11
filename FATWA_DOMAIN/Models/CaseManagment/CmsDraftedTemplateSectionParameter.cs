using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CaseManagment
{

    //< History Author = 'Hassan Abbas' Date = '2022-10-20' Version = "1.0" Branch = "master" > Case Draft Section Parameter Model</History>
    [Table("CMS_DRAFTED_TEMPLATE_SECTION_PARAMETER")]
    public class CmsDraftedTemplateSectionParameter
    {
        [Key]
        public Guid Id { get; set; }
        public Guid DraftedTemplateSectionId { get; set; }
        public int ParameterId { get; set; }
        public string Value { get; set; }

    }
}
