using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Serilog;
using System.DirectoryServices.AccountManagement;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using Group = FATWA_DOMAIN.Models.AdminModels.UserManagement.Group;


namespace FATWA_INFRASTRUCTURE.Repository
{
    //<!-- <History Author = 'Nabeel Ur Rehman' Date='2022-07-20' Version="1.0" Branch="master">Create class and add functionality</History> -->

    public class UmsGroupsRepository : IUmsGroup
    {
        #region Variables declaration

        private List<WebSystem> _websystemslist;
        private List<GroupAccessType> groupAccessTypeClasses;
        private WebSystem _websystems;

        private readonly DatabaseContext _dbContext;
        //private ICollection<UserVM> _UserResults = new List<UserVM>();
        //private ICollection<Role> _roleResult = new List<Role>();
        private readonly UserManager<IdentityUser> _userManager;
        private List<ClaimVM> _ClaimVM;
        private ActiveDirectorySettings _adSettings { get; set; }

        #endregion Variables declaration

        #region Constructor

        public UmsGroupsRepository(DatabaseContext dbContext, UserManager<IdentityUser> userManager, IOptions<ActiveDirectorySettings> ADSettings)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _adSettings = ADSettings.Value;
        }

        #endregion Constructor

        //<History Author = 'Umer Zaman' Date='2022-07-22' Version="1.0" Branch="master">Get all group details </History>
        //<History Author = 'AttiqueRehman' Date='09-apr-2025' Version="1.0" Branch="master">Get all group details with SP along with advance search </History>

        #region Get Group Details
        public async Task<List<UserGroupListVM>> GetGroupDetails(UserListAdvanceSearchVM advanceSearchVM)
        {
            try
            {
                string StoredProc = $"EXEC pUserGroupList @Name = '{advanceSearchVM.Name}', @SectorTypeId = '{advanceSearchVM.SectorId}', " +
                        $"@EmployeeTypeId = '{advanceSearchVM.EmployeeTypeId}', " +
                        $"@RoleId = '{advanceSearchVM.RoleId}', " +
                        $"@UserId = '{advanceSearchVM.UserId}'";
                return await _dbContext.UserGroupListVM.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Get Group Details

        //<History Author = 'Umer Zaman' Date='2022-07-22' Version="1.0" Branch="master">Delete selected user group and all dependent (roles & users) </History>

        #region Delete selected groups

        public async Task DeleteSelectedUserGroup(IList<Group> data)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        foreach (var item in data)
                        {
                            item.DeletedDate = DateTime.Now;
                            item.IsDeleted = true;

                            _dbContext.Entry(item).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            await UpdateUserSecurityStampByGroup(item, _dbContext);
                        }
                        await DeleteSelectedManytoManyUserGroupsUsers(data, _dbContext);
                        await DeleteSelectedManytoManyUserGroupsRoles(data, _dbContext);
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }
        }

        //<History Author = 'Aqeel Altaf' Date='2022-07-26' Version="1.0" Branch="master">update security stamp to force logout specific user</History>
        private async Task UpdateUserSecurityStampByGroup(Group group, DatabaseContext dbContext)
        {
            try
            {
                List<UserGroup> userGroups = new List<UserGroup>();
                userGroups = dbContext.UserGroups.Where(x => x.GroupId == group.GroupId).ToList();
                foreach (UserGroup userGroup in userGroups)
                {
                    if (userGroup.GroupId != null)
                    {
                        User userObj = dbContext.Users.Find(userGroup.UserId);
                        var identityuser = await _userManager.FindByEmailAsync(userObj.Email);
                        await _userManager.UpdateSecurityStampAsync(identityuser);
                        userObj.SecurityStamp = identityuser.SecurityStamp;
                        dbContext.Entry(userObj).State = EntityState.Modified;
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task DeleteSelectedManytoManyUserGroupsUsers(IList<Group> data, DatabaseContext dbContext)
        {
            try
            {
                if (data.Count() != 0)
                {
                    foreach (var item in data)
                    {
                        UserGroup? userGroupUserResult = dbContext.UserGroups.Where(x => x.GroupId == item.GroupId).FirstOrDefault();
                        if (userGroupUserResult != null)
                        {
                            dbContext.UserGroups.Remove(userGroupUserResult);
                            dbContext.SaveChanges();
                            userGroupUserResult = null;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private async Task DeleteSelectedManytoManyUserGroupsRoles(IList<Group> data, DatabaseContext dbContext)
        {
            try
            {
                //if (data.Count() != 0)
                //{
                //    foreach (var item in data)
                //    {
                //        UserGroupUserRoles? userGroupRoleResult = dbContext.UserGroupUserRole.Where(x => x.GroupId == item.GroupId).FirstOrDefault();
                //        if (userGroupRoleResult != null)
                //        {
                //            dbContext.UserGroupUserRole.Remove(userGroupRoleResult);
                //            dbContext.SaveChanges();
                //            userGroupRoleResult = null;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion Delete selected groups

        //<History Author = 'Nabeel ur Rehman' Date='2022-07-22' Version="1.0" Branch="master">Get selected user group and all dependent (roles & users) </History>

        #region Get User Group with all dependents

        public async Task<Group> GetUserGroupDetailById(Guid GroupId)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        Group? userGroupDetail = await _dbContext.Groups.FindAsync(GroupId);

                        if (userGroupDetail != null)
                        {
                            string StoredProc = $"exec pUserSelByGroupId @groupId='{GroupId}'";
                            userGroupDetail.Users = await _dbContext.UserListGroupVM.FromSqlRaw(StoredProc).ToListAsync();
                            return userGroupDetail;
                        }
                        else
                        {
                            throw new ArgumentNullException();
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        return null;
                    }
                }
            }
        }

        //private async Task GetManytoManyUserGroupUser(Guid GroupId, DatabaseContext dbContext)
        //{
        //	try
        //	{
        //		List<UserGroup>? referenceIdResults = dbContext.UserGroups.Where(r => r.GroupId == GroupId).ToList();
        //		if (referenceIdResults.Count() > 0)
        //		{
        //			foreach (var item in referenceIdResults)
        //			{
        //				User? results = dbContext.Users.Where(r => r.Id == item.UserId).FirstOrDefault();
        //				string StoredProc = $"exec pUserSelByGroupId @userId={item.UserId}";
        //				var usersList = await _dbContext.UserVM.FromSqlRaw(StoredProc).ToListAsync();
        //				_UserResults.Add(usersList.FirstOrDefault());
        //			}
        //		}
        //	}
        //	catch (Exception)
        //	{
        //		throw;
        //	}
        //}

        #endregion Get User Group with all dependents

        #region Create UMS Group

        //<History Author = 'Ammaar Naveed' Date='2024-07-23' Version="1.0" Branch="master">Find, and Create users in active directory while creating a new group</History>
        //<History Author = 'Nabeel ur Rehman' Date='2022-07-22' Version="1.0" Branch="master">Create User Group</History>
        public async Task CreateUmsGroup(Group group)
        {
            using (_dbContext)
            {
                //var context = new PrincipalContext(ContextType.Domain, _adSettings.ServerIPAddress, _adSettings.Container, _adSettings.MachineAccountName, _adSettings.MachineAccountPassword);
                //GroupPrincipal groupPrincipal = new GroupPrincipal(context);
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        //var groupId = await CreateADGroup(groupPrincipal, group, context);
                        //  group.GroupId = groupId;
                        _dbContext.Groups.Add(group);
                        await _dbContext.SaveChangesAsync();
                        if (group.Users != null)
                        {
                            await InsertManyToManyUsersGroup(group, _dbContext);
                            await FindAndCreateUserInActiveDirectory(group);
                        }
                        await InsertManytoManyGroupClaims(group, _dbContext);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        //groupPrincipal.Delete();
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        #region Create Users In Active Directory

        protected async Task FindAndCreateUserInActiveDirectory(Group group)
        {
            var webSystemsIds = await _dbContext.GroupAccessType.Where(x => x.GroupTypeId == group.GroupTypeId).Select(x => x.WebSystemId).ToListAsync();

            if ((webSystemsIds.Any(value => value == (int)WebSystemEnum.ActiveDirectory)))
            {
                foreach (var user in group.Users)
                {
                    var userDetails = new UserListGroupVM { FirstNameEnglish = user.FirstNameEnglish, LastNameEnglish = user.LastNameEnglish, Email = user.Email, IsActiveUser = user.IsActiveUser };
                    if (string.IsNullOrEmpty(user.ADUsername))
                    {
                        Log.Information("ADUsername is null creating user now");
                        await CreateUsersInActiveDirectory(userDetails, group);
                    }

                }
            }
        }

        private async Task CreateUsersInActiveDirectory(UserListGroupVM userDetails, Group group)
        {
            try
            {
                Log.Information("Create Users In ActiveDirectory Start");
                var defaultPassword = "Fatwa1234!@#$";
                Log.Information("User Creation Container " + _adSettings.UserCreationContainer);
                var principalContext = new PrincipalContext(ContextType.Domain, _adSettings.ServerIPAddress, _adSettings.UserCreationContainer, _adSettings.MachineAccountName, _adSettings.MachineAccountPassword);

                Log.Information("principal Context " + principalContext);

                var userPrincipal = new UserPrincipal(principalContext);

                userPrincipal.GivenName = userDetails.FirstNameEnglish;
                userPrincipal.MiddleName = userDetails.SecondNameEnglish;
                userPrincipal.Surname = userDetails.LastNameEnglish;
                userPrincipal.DisplayName = $"{userDetails.FirstNameEnglish} {userDetails.LastNameEnglish}";
                userPrincipal.EmailAddress = userDetails.Email;
                userPrincipal.Enabled = userDetails.IsActiveUser;
                userPrincipal.SetPassword(defaultPassword);

                Log.Information("Generate Unique SamAccountName start");
                string samAccountName = GenerateUniqueSamAccountName(userDetails.FirstNameEnglish, userDetails.LastNameEnglish);
                Log.Information("Generate Unique SamAccountName end and SamAccount Name is" + samAccountName + "and email is " + userDetails.Email);

                userPrincipal.SamAccountName = samAccountName;
                userPrincipal.Name = samAccountName;

                Log.Information("Save ADUsername In Database start");
                await SaveADUsernameInDatabase(userDetails, samAccountName, group);
                Log.Information("Save ADUsername In Database end");

                Log.Information("Started saving user in AD");
                userPrincipal.Save();
                Log.Information("Saving user in AD end");
            }
            catch (Exception ex)
            {
                Log.Error("Exception " + ex.Message);
                throw new Exception(ex.Message);
            }
        }

        protected async Task SaveADUsernameInDatabase(UserListGroupVM userDetails, string samAccountName, Group group)
        {
            var user = await _dbContext.Users.Where(un => un.Email == userDetails.Email).FirstOrDefaultAsync();
            try
            {
                if (user != null)
                {
                    user.ADUserName = samAccountName;
                    user.ModifiedDate = DateTime.Now;
                    user.ModifiedBy = group.ModifiedBy;
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        protected string GenerateUniqueSamAccountName(string FirstName_En, string LastName_En)
        {
            try
            {
                // search the user from overall AD by not providing any Container OU
                var Newcontext = new PrincipalContext(ContextType.Domain, _adSettings.ServerIPAddress, _adSettings.MachineAccountName, _adSettings.MachineAccountPassword);
                char firstCharFirstName = string.IsNullOrEmpty(FirstName_En) ? '\0' : FirstName_En[0];
                char firstCharLastName = !string.IsNullOrEmpty(LastName_En) ? LastName_En.Contains('-') ? LastName_En[LastName_En.IndexOf('-') + 1] : LastName_En[0] : '\0';
                string combinedLetters = $"{firstCharFirstName}{firstCharLastName}".ToUpper();
                int maxNumberSuffix = 0;
                //List<UserPrincipal> userList = new List<UserPrincipal>();//getting all ad users here
                using (PrincipalSearcher searcher = new PrincipalSearcher(new UserPrincipal(Newcontext)))
                {
                    foreach (UserPrincipal aduser in searcher.FindAll())
                    {
                        //userList.Add(aduser);
                        string samAccountName = aduser.SamAccountName;

                        Log.Information("Users available " + samAccountName);
                        if (samAccountName.StartsWith(combinedLetters, StringComparison.OrdinalIgnoreCase))
                        {
                            string numberPart = samAccountName.Substring(2);
                            if (int.TryParse(numberPart, out int number))
                            {
                                maxNumberSuffix = Math.Max(maxNumberSuffix, number);
                            }
                        }
                    }
                    // Increment maxNumber by 1 if it's greater than 0; otherwise, for the first, it will be 1.
                    maxNumberSuffix = (maxNumberSuffix > 0) ? maxNumberSuffix + 1 : 1;
                    string newSamAccountName = string.Empty;
                    if (maxNumberSuffix < 1000)
                    {
                        newSamAccountName = $"{firstCharFirstName}{firstCharLastName}{maxNumberSuffix:D3}";
                    }
                    else
                    {
                        newSamAccountName = $"{firstCharFirstName}{firstCharLastName}{maxNumberSuffix:D4}";
                    }
                    //Finally, Generated SameAccountName, with Patteren of 3 digit PostFix, incase it reached to AR999, then it will exceed to AR1000 4 digit postfix
                    return newSamAccountName.ToUpper();
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        #endregion Create Users In Active Directory

        public async Task<Guid> CreateADGroup(GroupPrincipal groupPrincipal, Group group, PrincipalContext context)
        {
            try
            {
                groupPrincipal.SamAccountName = group.Name_En;
                groupPrincipal.Description = group.Description_En;
                groupPrincipal.Save();

                if (group.Users != null)
                {
                    foreach (var user in group.Users)
                    {
                        groupPrincipal.Members.Add(context, IdentityType.SamAccountName, user.UserName);
                        groupPrincipal.Save();
                    }
                }
                return (Guid)groupPrincipal.Guid;
            }
            catch (Exception ex)
            {
                groupPrincipal.Delete();
                throw new Exception(ex.Message);
            }
        }

        private async Task InsertManyToManyUsersGroup(Group Group, DatabaseContext dbContext)
        {
            try
            {
                UserGroup userGroupUserobj = new UserGroup();
                foreach (var user in Group.Users)
                {
                    userGroupUserobj.GroupId = Group.GroupId;
                    userGroupUserobj.UserId = user.Id;
                    userGroupUserobj.CreatedDate = DateTime.Now;
                    userGroupUserobj.CreatedBy = Group.CreatedBy;
                    await dbContext.UserGroups.AddAsync(userGroupUserobj);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #endregion Create UMS Group

        #region update User Group

        //<History Author = 'Aqeel Altaf' Date='2022-07-26' Version="1.0" Branch="master">update security stamp to force logout specific user</History>
        public async Task UpdateUMSUsersGroup(Group group)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        _dbContext.Entry(group).State = EntityState.Modified;
                        await _dbContext.SaveChangesAsync();
                        await UpdateUserSecurityStampByGroup(group, _dbContext);
                        if (group.Users != null || group.UsersWithExistingGroups != null)
                            await UpdateManytoManyUmsGroupsUsers(group, _dbContext);
                        await UpdateManytoManyGroupClaims(group, _dbContext);
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Log.Error("UpdateUMSUsersGroup completely fail" + ex.Message);
                        transaction.Rollback();
                    }
                }
            }
        }

        //<History Author = 'Ammaar Naveed' Date='2024-09-03' Version="1.0" Branch="master">Removed unnecessary checks</History>
        //<History Author = 'Attique Rehman' Date='2024-02-18' Version="1.0" Branch="master">Update users existing group / modified  group</History>
        private async Task UpdateManytoManyUmsGroupsUsers(Group Group, DatabaseContext dbContext)
        {
            await UsersWithExistingGroupsDelete(Group, dbContext);
            if (Group.Users != null && Group.Users.Count > 0)
            {
                await InsertManyToManyUsersGroup(Group, dbContext);
                await FindAndCreateUserInActiveDirectory(Group);
            }
        }

        //<History Author = 'Attique Rehman' Date='2024-02-18' Version="1.0" Branch="master"> remove users from group base of GroupId that we unchecked from list </History>
        private Task UsersWithExistingGroupsDelete(Group Group, DatabaseContext dbContext)
        {
            var checkedUserList = Group.UsersWithExistingGroups.Select(u => u.Id).ToList();
            var uncheckedUserlist = dbContext.UserGroups.Where(x => x.GroupId == Group.GroupId && !checkedUserList.Contains(x.UserId)).ToList();
            dbContext.UserGroups.RemoveRange(uncheckedUserlist);
            dbContext.SaveChanges();
            return Task.CompletedTask;
        }

        private async Task UpdateManytoManyGroupClaims(Group Group, DatabaseContext dbContext)
        {
            await ExistingGroupClaimsDelete(Group.GroupId, dbContext);
            await InsertManytoManyGroupClaims(Group, dbContext);
        }

        private Task ExistingGroupClaimsDelete(Guid GroupId, DatabaseContext dbContext)
        {
            var References = dbContext.UmsGroupClaims.Where(x => x.GroupId == GroupId).ToList();
            dbContext.UmsGroupClaims.RemoveRange(References);
            dbContext.SaveChanges();
            return Task.CompletedTask;
        }

        private async Task InsertManytoManyGroupClaims(Group Group, DatabaseContext dbContext)
        {
            try
            {
                foreach (var claim in Group.GroupClaims)
                {
                    GroupClaims groupClaim = new GroupClaims();
                    groupClaim.GroupId = Group.GroupId;
                    groupClaim.ClaimType = claim.ClaimType;
                    groupClaim.ClaimValue = claim.ClaimValue;
                    await dbContext.UmsGroupClaims.AddAsync(groupClaim);
                    await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task UpdateManytoManyUserRole(Guid GroupId, ICollection<Role>? roleGet, DatabaseContext dbContext)
        {
            await ExistingUserGropRoleDelete(GroupId, roleGet, dbContext);
            await InsertManytoManyUserRole(GroupId, roleGet, dbContext);
        }

        private async Task ExistingUserGropRoleDelete(Guid GroupId, ICollection<Role>? roleGet, DatabaseContext dbContext)
        {
            //var Tags = dbContext.UserGroupUserRole.Where(x => x.GroupId == GroupId).ToList();
            //foreach (var item in Tags)
            //{
            //    dbContext.UserGroupUserRole.Remove(item);
            //    dbContext.SaveChanges();
            //}
        }

        private async Task InsertManytoManyUserRole(Guid GroupId, ICollection<Role>? Userroles, DatabaseContext dbContext)
        {
            try
            {
                //UserGroupUserRoles UserGroupUserRoleObj = new UserGroupUserRoles();
                //foreach (var role in Userroles)
                //{
                //    UserGroupUserRoleObj.GroupId = GroupId;
                //    UserGroupUserRoleObj.Id = role.Id;
                //    dbContext.UserGroupUserRole.AddAsync(UserGroupUserRoleObj);
                //    dbContext.SaveChanges();
                //}
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion update User Group

        #region Claims

        //<History Author = 'Danish' Date='2023-02-27' Version="1.0" Branch="master"> Get list of All Claims both if GroupId is null and not null</History>
        public async Task<List<ClaimVM>> GetAllClaims(string groupId)
        {
            try
            {
                if (_ClaimVM == null)
                {
                    string StoredProc = "";

                    if (groupId == null)
                        StoredProc = "exec pGroupClaimsList";
                    else
                        StoredProc = $"exec pGroupClaimsList @groupId ='{groupId}'";

                    _ClaimVM = await _dbContext.ClaimVM.FromSqlRaw(StoredProc).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _ClaimVM;
        }

        #endregion Claims

        #region Get User and Group Claims

        //<History Author = 'Nadia Gull' Date='2023-05-03' Version="1.0" Branch="master"> Return User and Group Claims </History>
        public async Task<List<ClaimSucessResponse>> GetGroupClaims(string userId)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userId);
                List<ClaimSucessResponse> claimlistforResult = new List<ClaimSucessResponse>();

                //Get User Specific Claims
                var userclaimsList = await _userManager.GetClaimsAsync(user);
                foreach (var item in userclaimsList)
                {
                    ClaimSucessResponse claimsobj = new ClaimSucessResponse();
                    claimsobj.Type = item.Type;
                    claimsobj.Value = item.Value;
                    claimlistforResult.Add(claimsobj);
                }

                //Get Group Specific Claims
                var userGroups = await _dbContext.UserGroups.Where(g => g.UserId == user.Id).ToListAsync();

                foreach (var userGroup in userGroups)
                {
                    IList<GroupClaims> claimsList = new List<GroupClaims>();

                    claimsList = await _dbContext.UmsGroupClaims.Where(x => x.GroupId == userGroup.GroupId).ToListAsync();
                    foreach (var item in claimsList)
                    {
                        ClaimSucessResponse claimsobj = new ClaimSucessResponse();
                        claimsobj.Type = item.ClaimType;
                        claimsobj.Value = item.ClaimValue;
                        claimlistforResult.Add(claimsobj);
                    }
                }
                return claimlistforResult;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion Get User and Group Claims

        public async Task AssignGroupsToUser()
        {
            try
            {
                var context = new PrincipalContext(ContextType.Domain, _adSettings.ServerIPAddress, _adSettings.Container, _adSettings.MachineAccountName, _adSettings.MachineAccountPassword);
                var searcher = new PrincipalSearcher(new UserPrincipal(context));

                foreach (var result in searcher.FindAll())
                {
                    var user = (UserPrincipal)result;
                    var userExsist = await _dbContext.Users.Where(x => x.Email == user.EmailAddress).FirstOrDefaultAsync();
                    if (userExsist != null)
                    {
                        foreach (var group in user.GetGroups(context))
                        {
                            if (_dbContext.Groups.Where(x => x.GroupId == group.Guid).Any())
                            {
                                if (!_dbContext.UserGroups.Where(x => x.GroupId == group.Guid && x.UserId == userExsist.Id).Any())
                                {
                                    UserGroup userGroup = new UserGroup();
                                    userGroup.GroupId = (Guid)group.Guid;
                                    userGroup.UserId = userExsist.Id;
                                    _dbContext.UserGroups.Add(userGroup);
                                    await _dbContext.SaveChangesAsync();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #region Get WebSystems list

        public async Task<List<WebSystem>> GetWebSystems()
        {
            try
            {
                if (_websystemslist == null)
                {
                    _websystemslist = await _dbContext.WebSystems.ToListAsync();
                }
                return _websystemslist;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<WebSystem> GetWebSystemsById(int Id)
        {
            try
            {
                if (_websystems == null)
                {
                    _websystems = await _dbContext.WebSystems.Where(x => x.WebSystemId == Id).FirstOrDefaultAsync();
                }
                return _websystems;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task SaveWebSystems(WebSystem WebSystem)
        {
            try
            {
                await _dbContext.WebSystems.AddAsync(WebSystem);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task UpdateWebSystems(WebSystem WebSystem)
        {
            try
            {
                _dbContext.WebSystems.Update(WebSystem);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion Get WebSystems list

        #region Group Access Type

        public async Task<GroupAccessTypeVM> GetGroupTypeById(int Id)
        {
            try
            {
                GroupAccessTypeVM _groupAccessTypeVM = new GroupAccessTypeVM();
                var _groupType = await _dbContext.GroupType
                    .Include(x => x.GroupTypes)
                    .Where(x => x.GroupTypeId == Id).FirstOrDefaultAsync();
                _groupAccessTypeVM.Name = _groupType.Name;
                _groupAccessTypeVM.Description = _groupType.Description;
                _groupAccessTypeVM.SelectedIdz = _groupType.GroupTypes.Select(x => x.WebSystemId).ToList();

                return _groupAccessTypeVM;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<GroupTypeWebSystemVM>> GetGroupAccessTypes()
        {
            try
            {
                if (groupAccessTypeClasses == null)
                {
                    var query = _dbContext.GroupAccessType
                        .Include(x => x.GroupType)
                        .Select(x => new GroupAccessType
                        {
                            GroupType = new GroupType
                            {
                                GroupTypeId = x.GroupTypeId,
                                Name = x.GroupType.Name,
                                Description = x.GroupType.Description
                            },
                            WebSystem = new WebSystem
                            {
                                WebSystemId = x.WebSystemId,
                                NameEn = x.WebSystem.NameEn,
                                NameAr = x.WebSystem.NameAr
                            }
                        });
                    var result1 = await query.ToListAsync();
                    var results = result1.GroupBy(item => new { item.GroupType.GroupTypeId, item.GroupType.Name, item.GroupType.Description })
                        .Select(group => new GroupTypeWebSystemVM
                        {
                            GroupTypeId = group.Key.GroupTypeId,
                            Name = group.Key.Name,
                            Description = group.Key.Description,
                            WebSystemsEn = string.Join(", ", group.Select(item => item.WebSystem.NameEn)),
                            WebSystemsAr = string.Join(", ", group.Select(item => item.WebSystem.NameAr)),
                            WebSystemsIds = group.Select(item => item.WebSystem.WebSystemId).ToList()
                        })
                        .ToList();

                    return results.OrderBy(x => x.GroupTypeId).ToList();
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task CreateGroupAccessType(GroupAccessTypeVM item)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            var groupType = new GroupType
                            {
                                Name = item.Name,
                                Description = item.Description,
                                CreatedBy = item.CreatedBy,
                                CreatedDate = item.CreatedDate,
                            };
                            _dbContext.GroupType.Add(groupType);
                            await _dbContext.SaveChangesAsync();
                            int grpId = groupType.GroupTypeId;
                            foreach (var WebSystemId in item.SelectedIdz)
                            {
                                var groupAccessType = new GroupAccessType
                                {
                                    GroupTypeId = grpId,
                                    WebSystemId = WebSystemId
                                };
                                _dbContext.GroupAccessType.Add(groupAccessType);
                            }
                            await _dbContext.SaveChangesAsync();
                            await transaction.CommitAsync();
                        }
                        catch (Exception)
                        {
                            await transaction.RollbackAsync();
                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task UpdateGroupAccessType(GroupAccessTypeVM item)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = await _dbContext.Database.BeginTransactionAsync())
                    {
                        try
                        {
                            // get GroupType
                            var groupType = _dbContext.GroupType.Find(item.GroupTypeId);
                            // Update GroupType
                            groupType.Name = item.Name;
                            groupType.Description = item.Description;
                            groupType.ModifiedBy = item.ModifiedBy;
                            groupType.ModifiedDate = item.ModifiedDate;
                            _dbContext.GroupType.Update(groupType);
                            await _dbContext.SaveChangesAsync();
                            // delete existing GroupAccessType
                            var References = _dbContext.GroupAccessType.Where(x => x.GroupTypeId == item.GroupTypeId).ToList();
                            _dbContext.GroupAccessType.RemoveRange(References);
                            _dbContext.SaveChanges();
                            // Add new GroupAccessType
                            foreach (var WebSystemId in item.SelectedIdz)
                            {
                                var groupAccessType = new GroupAccessType
                                {
                                    GroupTypeId = item.GroupTypeId,
                                    WebSystemId = WebSystemId
                                };
                                _dbContext.GroupAccessType.Add(groupAccessType);
                            }
                            await _dbContext.SaveChangesAsync();
                            await transaction.CommitAsync();
                        }
                        catch (Exception)
                        {
                            await transaction.RollbackAsync();
                            throw;
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion Group Access Type

        public async Task<List<Group>> GetGroups()
        {
            try
            {
                return _dbContext.Groups.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Group>> GetGroupsByGroupTypeId(int GroupTypeId)
        {
            try
            {
                return _dbContext.Groups.Where(x => x.GroupTypeId == GroupTypeId).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}