using Itenso.TimePeriod;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
	public class LawyerVM
	{ 
		public string Id { get; set; }
		public string UserNameEn { get; set; }
		public string UserNameAr { get; set; }
		public int TotalTasks { get; set; }
		public DateTime? LastActivityDate { get; set; }
		public int? TotalCases { get; set; }
		public int? CurrentCases { get; set; }
		public DateTime? AbsentTillDate { get; set; }
    }
}
