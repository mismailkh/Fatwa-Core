using FATWA_DOMAIN.Models.BaseModels;

namespace FATWA_DOMAIN.Models.ViewModel.CaseManagment
{
    public class CmsLawyerSupervisorVM : TransactionalBaseModel
    {
        public string? SupervisorId { get; set; }
        public string? ManagerId { get; set; }
        public IList<ListLawyerSupervisorAssignmentVM>? SelectedUsers { get; set; } = new List<ListLawyerSupervisorAssignmentVM>();
    }
}
