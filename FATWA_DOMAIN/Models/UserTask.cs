using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models
{
    [Table("tTask")]
    public class UserTask
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaskId { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(250)]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        public string Description { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedDate { get; set; }

        [Required(ErrorMessage = "Assigned Date is required")]
        [DataType(DataType.DateTime)]
        public DateTime? AssignedDate { get; set; }

        [Required(ErrorMessage = "Due Date is required")]
        [DataType(DataType.DateTime)]
        public DateTime? DueDate { get; set; }
    }
}
