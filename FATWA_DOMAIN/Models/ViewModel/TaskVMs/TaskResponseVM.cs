using FATWA_DOMAIN.Models.TaskModels;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.TaskVMs
{
	public partial class TaskResponseVM
    {
        public TaskResponse TaskResponse { get; set; } 
        public List<TaskAction> TaskActions { get; set; } 
         
        [NotMapped]
        public List<int>? DeletedAttachementIds { get; set; } = new List<int>();

    }
}
