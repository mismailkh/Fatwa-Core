using System.ComponentModel.DataAnnotations.Schema;
using FATWA_DOMAIN.Models.TaskModels;

namespace FATWA_DOMAIN.Models.ViewModel.TaskVMs
{
    public class SaveTaskVM
    {
        public UserTask Task { get; set; }
        public List<TaskAction> TaskActions { get; set; }
        public List<Guid>? DeletedTaskActionIds { get; set; } = new List<Guid>();
        public string Action {  get; set; }
        public string EntityName {  get; set; }
        public string EntityId {  get; set; }
		[NotMapped]
        public List<int>? DeletedAttachementIds { get; set; } = new List<int>();
    }
}
