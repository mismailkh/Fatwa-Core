using FATWA_DOMAIN.Models.BaseModels;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FATWA_DOMAIN.Models.BaseModels;

namespace FATWA_DOMAIN.Models.TaskModels
{
	[Table("TSK_TODO_LIST")]
	public partial class UserTodoList : TransactionalBaseModel
	{
		[Key]
		public Guid TodoItemId { get; set; } 
		public string Description { get; set; }  

		#region Foreign Keys

		public string UserId { get; set; } 

		#endregion

	}
}
