using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.MeetModels;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.CommunicationModels;

using FATWA_DOMAIN.Models.ViewModel.Meet;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.ViewModel.RolesVM;
using FATWA_DOMAIN.Models.CaseManagment;

namespace FATWA_DOMAIN.Interfaces
{
    //<History Author = 'Hassan Abbas' Date='2022-07-20' Version="1.0" Branch="master"> interface for role and claims operations</History>
    public interface IRole
    {
        Task<List<ClaimVM>> GetAllClaims(string roleId);
        Task<Role> GetRoleById(string roleId);
        Task CreateRole(Role role);
        Task UpdateRole(Role role);
        Task<List<Role>> GetRoleData();
        Task<List<Role>> GetRoleDetails();
        Task<List<ClaimSucessResponse>> GetRoleClaims(string userId);
        Task DeleteRole(Role role);
        Task<User> GetHOSBySectorId(int sectorTypeId);
        Task<User> GetMojBySectorId(int sectorTypeId);
        Task<List<User>> GetViceHOSBySectorId(int sectorTypeId);
        Task<List<User>> GetViceHOSOrManagerBySectorUserId(int sectorTypeId, string userName);
        Task<List<User>> GetHOSAndViceHOSBySectorId(int sectorTypeId, string username, bool verifyViceHOSResponsibility, bool returnHOS, int chamberNumberId);

        Task<List<GEDepartments>> GetDepartmentsByGEId(List<SendCommunicationVM> sendCommunication);
        Task<User> GetHOSByFileAndLinkTargetTypeId(LinkTarget linkTarget);
    }
}
