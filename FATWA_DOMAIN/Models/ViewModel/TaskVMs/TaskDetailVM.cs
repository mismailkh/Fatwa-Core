using System.ComponentModel.DataAnnotations.Schema;

namespace FATWA_DOMAIN.Models.ViewModel.TaskVMs
{
	public partial class TaskDetailVM
	{
		public Guid TaskId { get; set; } 
		public int TaskNumber { get; set; }
		public DateTime TaskDate { get; set; }
		public int? TypeId { get; set; }
		public string TypeAr { get; set; }
		public string TypeEn { get; set; }
		public string? SubTypeAr { get; set; }
		public string? SubTypeEn { get; set; }
		public string? PriorityAr { get; set; }
		public string? PriorityEn { get; set; }
		public DateTime? DueDate { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }
		public string? FileNumber { get; set; }
		public string? ConsultationFileNo { get; set; }
		public string SectorAr { get; set; }
		public string SectorEn { get; set; }
		public string DepartmentAr { get; set; } 
		public string DepartmentEn { get; set; }
        public int? ModuleId { get; set; } 
        public string ModuleAr { get; set; }
		public string ModuleEn { get; set; }
		public string Role { get; set; }
		public string AssignedTo { get; set; } 
		public string AssignedToName { get; set; } 
		public string AssignedBy { get; set; } 
		public string Url { get; set; } 
		public string ModifiedBy { get; set; } 
		public int TaskStatusId { get; set; }
		public string? TaskStatusEn { get; set; } 
		public string? TaskStatusAr { get; set; } 
		public string? Reason { get; set; }
        public Guid? ReferenceId { get; set; }  
		public int? SystemGenTypeId { get; set; }
        [NotMapped]
        public Guid? VersionId { get; set; }
        [NotMapped]
        public int? SectorId { get; set; }
        [NotMapped]
        public bool IsMultipleTaskUpdateForSameEntity { get; set; }
		[NotMapped]
		public List<int> SystemGenTypeIdsToComplete { get; set; } = new List<int>();
        [NotMapped]
        public bool IsFinalJudgementTrue { get; set; }
    }
}
