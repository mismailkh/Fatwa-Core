using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FATWA_DOMAIN.Models.BaseModels;


namespace FATWA_DOMAIN.Models.TaskModels
{
	[Table("TSK_TASK")]
	public partial class UserTask : TransactionalBaseModel
	{
		[Key]
		public Guid TaskId { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public DateTime Date { get; set; }
		public DateTime? DueDate { get; set; }
		public string AssignedBy { get; set; }
		public string AssignedTo { get; set; }
		public string Url { get; set; }
		public int TaskNumber { get; set; } 
		public bool IsSystemGenerated { get; set; }
		
		#region Foreign Keys

		public int? TaskStatusId { get; set; }
		public int ModuleId { get; set; }
		public int SectorId { get; set; }
		public int? DepartmentId { get; set; }
		public int TypeId { get; set; }
		public int? SubTypeId { get; set; } 
		public int? PriorityId { get; set; }
		public string RoleId { get; set; }
		public Guid? ReferenceId { get; set; }
        public int? SubModuleId { get; set; }
        public int? SystemGenTypeId { get; set; }
		public int? EntityId { get; set; }

        #endregion

    }
}
