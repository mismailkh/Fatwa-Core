using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("LOG_PROCESSLOGEVENT")]
    public class ProcessLogEvent
    {
        [Key]
        public Guid ProcessLogEventId { get; set; }

        [StringLength(50)]
        [Required]
        public string? Name { get; set; }

        [Required]
        [StringLength(50)]
        public string Description { get; set; }
    }
}
