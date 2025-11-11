namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{

	public class ListLawyerSupervisorAssignmentVM
    { 
		public string Id { get; set; }
		public string UserNameEn { get; set; }
		public string UserNameAr { get; set; }
		public int TotalTasks { get; set; }
		public DateTime? LastActivityDate { get; set; }
		public int? TotalCases { get; set; }
		public int? CurrentCases { get; set; }
		public string? SupervisorNameEn { get; set; }
		public string? SupervisorNameAr { get; set; }
        public string? SupervisorId { get; set; }
		public string? ManagerNameEn { get; set; }
		public string? ManagerNameAr { get; set; }
		public string? ManagerId { get; set; }
    }
}
