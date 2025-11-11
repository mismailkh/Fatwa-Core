using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.TaskModels
{
	[Table("TSK_TASK_RESPONSE_STATUS")] 
	public partial class TaskResponseStatus
    {
		[Key]

		public int TaskResponeStatusId { get; set; }
		public string NameEn { get; set; }
		public string NameAr { get; set; }
	}
} 