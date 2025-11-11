using System.ComponentModel.DataAnnotations;

namespace FATWA_DOMAIN.Models
{
    public class TasksModel
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(50)]
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
