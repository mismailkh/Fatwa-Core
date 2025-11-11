using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.ViewModel.Shared;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.CaseManagment
{
    //< History Author = 'Hassan Abbas' Date = '2022-10-20' Version = "1.0" Branch = "master" > Case Draft Sections Model</History>
    [Table("CMS_DRAFTED_TEMPLATE_SECTION")]
    public class CmsDraftedTemplateSection
    {
        [Key]
        public Guid Id { get; set; }
        public Guid DraftedTemplateVersionId { get; set; }
        public int SectionId { get; set; }
        public string AdditionalName { get; set; }
        public int SequenceNumber { get; set; }

    }
}
