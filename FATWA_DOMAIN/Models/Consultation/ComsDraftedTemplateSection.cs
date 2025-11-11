using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.ViewModel.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.Consultation
{
    //< History Author = 'Muhammad Zaeem' Date = '2023-02-02' Version = "1.0" Branch = "master" > Consultation Draft Sections Model</History>
    [Table("COMS_DRAFTED_TEMPLATE_SECTION")]
    public class ComsDraftedTemplateSection
    {
        [Key]
        public Guid Id { get; set; }
        public Guid DraftedTemplateId { get; set; }
        public int SectionId { get; set; }
        public string AdditionalName { get; set; }
        public int SequenceNumber { get; set; }

    }
}
