using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("LOG_PROCESSLOGTYPE")]
    public class ProcessLogType
    {
        [Key]
        public Guid ClientId { get; set; }

        [StringLength(50)]
        [Required]
        public string? Field { get; set; }

        [Required]
        [StringLength(50)]
        public string Value { get; set; }
    }
}
