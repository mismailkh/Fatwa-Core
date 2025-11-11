using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static FATWA_DOMAIN.Enums.CaseManagementEnums;

namespace FATWA_DOMAIN.Models.ViewModel.Shared
{
    public class CaseTemplateSectionsVM
    {
        [Key]
        public Guid Id { get; set; }
        public int TemplateId { get; set; }
        public int SectionId { get; set; }
        public string SectionNameEn { get; set; }
        public string SectionNameAr { get; set; }
        [NotMapped]
        public IList<CaseTemplateParametersVM> SectionParameters { get; set; } = new List<CaseTemplateParametersVM>();
        [NotMapped]
        public BlankTemplateSectionPositionEnum BlankSectionPosition { get; set; }
        [NotMapped]
        public int SequenceNumber { get; set; }

    }
}
