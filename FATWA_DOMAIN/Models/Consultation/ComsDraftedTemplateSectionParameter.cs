using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.Consultation
{

    //< History Author = 'Muhammad Zaeem' Date = '2023-02-02' Version = "1.0" Branch = "master" > COnsultation Draft Section Parameter Model</History>
    [Table("COMS_DRAFTED_TEMPLATE_SECTION_PARAMETER")]
    public class ComsDraftedTemplateSectionParameter
    {
        [Key]
        public Guid Id { get; set; }
        public Guid DraftedTemplateSectionId { get; set; }
        public int ParameterId { get; set; }
        public string Value { get; set; }

    }
}
