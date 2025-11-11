using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("LOG_ERRORLOGTYPE")]
    public class ErrorLogType
    {
        [Key]
        public Guid ErrorLogTypeId { get; set; }

        [StringLength(50)]
        [Required]
        public string Field { get; set; }

        [Required]
        [StringLength(500)]
        public string Value { get; set; }
    }
}