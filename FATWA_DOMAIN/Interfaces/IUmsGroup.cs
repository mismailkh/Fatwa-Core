using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;

namespace FATWA_DOMAIN.Interfaces
{
    //<!-- <History Author = 'Nabeel Ur Rehman' Date='2022-05-26' Version="1.0" Branch="master">Create interface</History> -->
    public interface IUmsGroup
    {
        Task<List<UserGroupListVM>> GetGroupDetails(UserListAdvanceSearchVM advanceSearchVM);
        Task DeleteSelectedUserGroup(IList<Group> data);
        Task<Group> GetUserGroupDetailById(Guid GroupId);
        Task CreateUmsGroup(Group userGroup);
        Task UpdateUMSUsersGroup(Group usersgroup);
        Task<List<ClaimVM>> GetAllClaims(string groupId);
        Task<List<ClaimSucessResponse>> GetGroupClaims(string userId);
        Task AssignGroupsToUser();
        Task<List<WebSystem>> GetWebSystems();
        Task<WebSystem> GetWebSystemsById(int Id);
        Task SaveWebSystems(WebSystem WebSystem);
        Task UpdateWebSystems(WebSystem WebSystem);
        Task<GroupAccessTypeVM> GetGroupTypeById(int Id);
        Task<List<GroupTypeWebSystemVM>> GetGroupAccessTypes();
        Task<List<Group>> GetGroups();
        Task CreateGroupAccessType(GroupAccessTypeVM groupAccessTypeVM);
        Task UpdateGroupAccessType(GroupAccessTypeVM item);
        Task<List<Group>> GetGroupsByGroupTypeId(int GroupTypeId);
    }
}
