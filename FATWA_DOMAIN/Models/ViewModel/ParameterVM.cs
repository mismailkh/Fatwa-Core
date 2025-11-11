using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models.ViewModel
{
    public class ParameterVM
    {
        [Key]
        public int? ParameterId { get; set; }
        public string? Name { get; set; }
        public string? PKey { get; set; }
        public bool? Mandatory { get; set; }
    }
}
