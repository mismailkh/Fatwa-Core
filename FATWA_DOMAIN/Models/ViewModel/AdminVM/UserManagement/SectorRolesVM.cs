namespace FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement
{
    public class SectorRolesVM
    {
        public Guid Id { get; set; }
        public int SectorId { get; set; }
        public string SectorNameEn { get; set; }
        public string SectorNameAr { get; set; }
        public string RolesNameEn { get; set; }
        public string RolesNameAr { get; set; }
        public string RoleId { get; set; }
        public string SectorAndRoleNameEn { get; set; }
        public string SectorAndRoleNameAr { get; set; }

    }
}
