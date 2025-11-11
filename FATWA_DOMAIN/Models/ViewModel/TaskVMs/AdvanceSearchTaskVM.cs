using FATWA_DOMAIN.Models.BaseModels;

namespace FATWA_DOMAIN.Models.ViewModel.TaskVMs
{
	public class AdvanceSearchTaskVM : GridPagination
	{
		public string? UserId { get; set; } 
		public string? Name { get; set; }
		public string? Description { get; set; }
		public DateTime? FromDate { get; set; }
		public DateTime? ToDate { get; set; }
		public string? AssignedBy { get; set; } 
		public int? TaskStatusId { get; set; } 
		public int? SubModuleId { get; set; } 
		public int? ScreenId { get; set; }
		public bool IsImpportant { get; set; } = false;
		public int? GovermentEntityId { get; set; } 
		public string? ReferenceNumber { get; set; }
        public DateTime? FromHearingDate { get; set; }

        public DateTime? ToHearingDate { get; set; }
        public int? TaskTypeId { get; set; }
        public int? SelectedIndex { get; set; }
    }

    public class AdvanceSearchTaskMobileAppVM
    {
        public string? UserId { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime? FromDate { get; set; }
        public DateTime? ToDate { get; set; }
        public string? AssignedBy { get; set; }
        public int? TaskStatusId { get; set; }
        public int? SubModuleId { get; set; }
        public int? ScreenId { get; set; }
        public bool IsImpportant { get; set; } = false;
        public int? GovermentEntityId { get; set; }
        public string? ReferenceNumber { get; set; }
        public DateTime? FromHearingDate { get; set; }

        public DateTime? ToHearingDate { get; set; }
        public int? TaskTypeId { get; set; }
    }
}
