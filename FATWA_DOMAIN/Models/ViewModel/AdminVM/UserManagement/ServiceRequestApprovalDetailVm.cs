using FATWA_DOMAIN.Models.BaseModels;

namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement
{
    public class ServiceRequestApprovalDetailVm : TransactionalBaseModel
    {
        public int Id { get; set; }
        public int ServiceRequestTypeId { get; set; }
        public int NoOfApprovals { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentNameEn { get; set; }
        public string DepartmentNameAr { get; set; }
        public string RoleIds { get; set; }
        public string RoleNameEn { get; set; }
        public string RoleNameAr { get; set; }
        public string ApprovalSequenceNo { get; set; }
        public string ServiceRequestNameEn { get; set; }
        public string ServiceRequestNameAr { get; set; }
        public string SectorIds { get; set; }
        public string SectorNameEn { get; set; }
        public string SectorNameAr { get; set; }
        public string ApprovalFlowDetailsEn { get; set; }
        public string ApprovalFlowDetailsAr { get; set; }
    }
    public class ServiceRequestApprovalHistoryVm
    {
        public int Id { get; set; }
        public int NoOfApprovals { get; set; }
        public int ApprovalSequenceNo { get; set; }
        public int VersionId { get; set; }
        public string RoleNameEn { get; set; }
        public string RoleNameAr { get; set; }
        public string SectorNameEn { get; set; }
        public string SectorNameAr { get; set; }
    }
}
