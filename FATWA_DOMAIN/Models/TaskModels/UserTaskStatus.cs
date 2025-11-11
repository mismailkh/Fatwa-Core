using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.TaskModels
{
	[Table("TSK_TASK_STATUS")] 
	public partial class UserTaskStatus
	{
		[Key]

		public int TaskStatusId { get; set; }
		public string NameEn { get; set; }
		public string NameAr { get; set; }
	}
} 