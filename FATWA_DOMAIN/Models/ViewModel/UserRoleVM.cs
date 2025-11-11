namespace FATWA_DOMAIN.Models.ViewModel
{
    public class ManageUserRolesViewModel
    {
        public string UserId { get; set; }
        public IList<UserRolesViewModel> UserRoles { get; set; }
    }
    public class UserRolesViewModel
    {
        public string RoleName { get; set; }
        public bool Selected { get; set; }
    }
    public class UserDetailVM
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public string SecurityStamp { get; set; }
        public int? SectorTypeId { get; set; }
        public bool SectorOnlyViceHOSApprovalEnough { get; set; }
        public string? FullNameEn { get; set; }
        public string? FullNameAr { get; set; }
        public int? UserTypeId { get; set; }
        public bool IsPasswordReset { get; set; }
        public string? ActiveDirectoryUserName { get; set; }
        public int BuildingId { get; set; }
        public int FloorId { get; set; }
        public int BorrowReturnDayDuration { get; set; }
        public int DepartmentId { get; set; }
        public string DepartmentNameEn { get; set; }
        public string DepartmentNameAr { get; set; }
        public string EmployeeId { get; set; }
        public string CivilId { get; set; }
        public int GenderId { get; set; }
        public int DesignationId { get; set; }
        public bool HasSignatureImage  { get; set; }
        public bool CanSignDocument  { get; set; }
        public Guid? GroupId { get; set; }
    }
}
