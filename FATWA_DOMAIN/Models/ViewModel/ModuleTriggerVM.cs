using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel
{
    public class ModuleTriggerVM
    {
        [Key]
        public int? ModuleTriggerId { get; set; }
        public int? ModuleId { get; set; }
        public string? ModuleNameAr { get; set; }
        public string? ModuleNameEn { get; set; }
        public string? Name { get; set; }
        public bool IsConditional { get; set; }
        public bool IsOptional { get; set; }
    }
}
