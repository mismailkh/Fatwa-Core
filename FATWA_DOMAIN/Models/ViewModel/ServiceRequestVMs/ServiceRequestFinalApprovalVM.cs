using FATWA_DOMAIN.Models.BaseModels;

namespace FATWA_DOMAIN.Models.ViewModel.ServiceRequestVMs
{
    public class ServiceRequestFinalApprovalVM : TransactionalBaseModel
    {
        public int ApprovalId { get; set; }
        public IEnumerable<int> ServiceRequestTypesId { get; set; }
        public int DepartmentId { get; set; }
        public IEnumerable<int> SectorIds { get; set; }
        public int NoOfApproval { get; set; }
        public IEnumerable<string> SelectedSectorAndRoles { get; set; }
    }
}
