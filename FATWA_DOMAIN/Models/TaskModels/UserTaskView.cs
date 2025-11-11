using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.TaskModels
{
    [Table("USER_TASK_VIEW")]
    public class UserTaskView
    {
        [Key]
        public int Id { get; set; }
        public DateTime? LastViewTime { get; set; }
        public Guid? UserId { get; set; }
        public Guid? ReferenceId { get; set; }

    }
}
