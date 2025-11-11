using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("LOG_ERRORLOGEVENT")]
    public class ErrorLogEvent
    {
        [Key]
        public Guid ErrorLogEventId { get; set; }

        [StringLength(50)]
        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(500)]
        public string Description { get; set; }
    }
}