using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FATWA_DOMAIN.Models.ViewModel.Shared
{
    public class CaseTemplateParametersVM
    {
        [Key]
        public Guid Id { get; set; }
        public int? ParameterId { get; set; }
        public int? TemplateId { get; set; }
        public int? SectionId { get; set; }
        public string? Name { get; set; }
        public string? PKey { get; set; }
        public bool IsAutoPopulated { get; set; }
        public string? Value { get; set; } = "";
    }
}
