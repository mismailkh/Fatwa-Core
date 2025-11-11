using AutoMapper.Internal;
using FATWA_DOMAIN.Common;
using FATWA_DOMAIN.Enums.Common;
using FATWA_DOMAIN.Extensions;
using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.CommonModels;
using FATWA_DOMAIN.Models.CommunicationModels;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.Lookups;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_DOMAIN.Models.ViewModel.CommunicationVMs;
using FATWA_DOMAIN.Models.ViewModel.RolesVM;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System.Security.Claims;

namespace FATWA_INFRASTRUCTURE.Repository
{
    //<History Author = 'Hassan Abbas' Date='2022-07-20' Version="1.0" Branch="master"> Repository for managing Roles and Claims</History>
    public class RoleRepository : IRole
    {
        private readonly DatabaseContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private List<ClaimVM> _ClaimVM;
        private List<Role> _userRoleDetails;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RoleRepository(DatabaseContext dbContext, UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IServiceScopeFactory serviceScopeFactory)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _roleManager = roleManager;
            _serviceScopeFactory = serviceScopeFactory;
        }

        #region Get Role

        //<History Author = 'Hassan Abbas' Date='2022-07-21' Version="1.0" Branch="master"> Get role by id</History>
        public async Task<Role> GetRoleById(string roleId)
        {
            try
            {
                return await _dbContext.Roles.Where(x => x.Id == roleId && x.IsDeleted == false).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Claims

        //<History Author = 'Hassan Abbas' Date='2022-07-20' Version="1.0" Branch="master"> Get list of All Claims both if RoleId is null and not null</History>
        public async Task<List<ClaimVM>> GetAllClaims(string roleId)
        {
            try
            {
                if (_ClaimVM == null)
                {
                    string StoredProc = "";

                    if (roleId == null)
                        StoredProc = "exec pClaimsList";
                    else
                        StoredProc = $"exec pClaimsList @roleId ='{roleId}'";

                    _ClaimVM = await _dbContext.ClaimVM.FromSqlRaw(StoredProc).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _ClaimVM;
        }

        #endregion

        #region Role

        //<History Author = 'Hassan Abbas' Date='2022-07-21' Version="1.0" Branch="master"> Create Role and insert claims if any</History>
        public async Task CreateRole(Role role)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            role.NormalizedName = role.Name.ToUpper();
                            await _dbContext.Roles.AddAsync(role);
                            await _dbContext.SaveChangesAsync();
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        //<History Author = 'Hassan Abbas' Date='2022-07-21' Version="1.0" Branch="master"> Update Role and claims</History>
        //<History Author = 'Aqeel Altaf' Date='2022-07-26' Version="1.0" Branch="master">update security stamp to force logout specific user</History>
        public async Task UpdateRole(Role role)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            role.NormalizedName = role.Name.ToUpper();
                            _dbContext.Entry(role).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            /*var svs = _dbContext.RoleClaims.Where(c => c.RoleId == role.Id);
                            _dbContext.RoleClaims.RemoveRange(_dbContext.RoleClaims.Where(c => c.RoleId == role.Id));
                            await _dbContext.SaveChangesAsync();*/
                            await UpdateUserSecurityStampByRole(role, _dbContext);
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex);
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        #endregion

        #region Role DDL

        public async Task<List<Role>> GetRoleData()
        {
            List<Role>? result = null;
            try
            {
                result = _dbContext.Roles.Where(x => x.IsDeleted == false).ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return result;
        }

        #endregion

        //<History Author = 'Umer Zaman' Date='2022-07-22' Version="1.0" Branch="master">Get all user rols details </History>
        #region Get Roles Details
        public async Task<List<Role>> GetRoleDetails()
        {
            if (_userRoleDetails == null)
            {
                _userRoleDetails = await _dbContext.Roles.OrderByDescending(u => u.Id).Where(x => x.IsDeleted == false).OrderByDescending(u => u.CreatedDate).ToListAsync();
            }

            return _userRoleDetails;
        }
        #endregion

        #region Get Role, User, Group Claims


        //<History Author = 'Hassan Abbas' Date='2022-07-29' Version="1.0" Branch="master">Get allclaims </History>
        public async Task<List<ClaimSucessResponse>> GetRoleClaims(string userId)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(userId);
                var userRoles = await _userManager.GetRolesAsync(user);
                List<ClaimSucessResponse> claimlistforResult = new List<ClaimSucessResponse>();
                IList<TranslationSucessResponse> AddedTranslations = new List<TranslationSucessResponse>();

                //Get user specific claims
                var userclaimsList = await _userManager.GetClaimsAsync(user);
                foreach (var item in userclaimsList)
                {
                    ClaimSucessResponse claimsobj = new ClaimSucessResponse();
                    claimsobj.Type = item.Type;
                    claimsobj.Value = item.Value;
                    claimlistforResult.Add(claimsobj);
                }

                //Get User group specific roles
                var userGroups = await _dbContext.UserGroups.Where(g => g.UserId == user.Id).ToListAsync();

                //List<UserGroupUserRoles> groupRoles = new List<UserGroupUserRoles>();
                //foreach (var group in userGroups)
                //{
                //    groupRoles = groupRoles.Concat(_dbContext.UserGroupUserRole.Where(g => g.GroupId == group.GroupId).ToList()).ToList();
                //}

                ////Populate role list and merge in main Roles list
                //foreach (var groupRole in groupRoles)
                //{
                //    userRoles.Add(_roleManager.FindByIdAsync(groupRole.Id).Result.Name);
                //}

                //Get User Roles specific claims
                IdentityRole? role = null;

                foreach (var userRole in userRoles)
                {
                    IList<Claim> claimsList = new List<Claim>();

                    role = await _roleManager.FindByNameAsync(userRole);
                    claimsList = await _roleManager.GetClaimsAsync(role);
                    //claimsList = claimsList.Concat(userclaimsList).ToList();
                    foreach (var item in claimsList)
                    {
                        ClaimSucessResponse claimsobj = new ClaimSucessResponse();
                        claimsobj.Type = item.Type;
                        claimsobj.Value = item.Value;
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

        #endregion

        //<History Author = 'Umer Zaman' Date='2022-08-01' Version="1.0" Branch="master">Soft delete Role</History>
        //<History Author = 'Aqeel Altaf' Date='2022-07-26' Version="1.0" Branch="master">update security stamp to force logout specific user</History>
        #region Delete Role (Soft delete status change)
        public async Task DeleteRole(Role role)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            await UpdateUserSecurityStampByRole(role, _dbContext);
                            _dbContext.Entry(role).State = EntityState.Modified;
                            await _dbContext.SaveChangesAsync();
                            transaction.Commit();
                        }
                        catch (Exception)
                        {
                            transaction.Rollback();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        //<History Author = 'Aqeel Altaf' Date='2022-07-26' Version="1.0" Branch="master">update security stamp to force logout specific user</History>
        private async Task UpdateUserSecurityStampByRole(Role role, DatabaseContext dbContext)
        {
            List<UserRole> userRoles = new List<UserRole>();
            userRoles = dbContext.UserRoles.Where(x => x.RoleId == role.Id).ToList();
            foreach (UserRole userRole in userRoles)
            {
                if (userRole.UserId != null)
                {
                    User userObj = dbContext.Users.Find(userRole.UserId);
                    var identityuser = await _userManager.FindByEmailAsync(userObj.Email);
                    await _userManager.UpdateSecurityStampAsync(identityuser);
                    //userObj.SecurityStamp = identityuser.SecurityStamp;
                    //dbContext.Entry(userObj).State = EntityState.Modified;
                    //await dbContext.SaveChangesAsync();
                }
            }
        }


        #endregion

        #region Get Sector HOS
        //<History Author = 'Hassan Abbas' Date='2022-12-21' Version="1.0" Branch="master"> Get HOS By Sector</History>
        public async Task<User> GetHOSByFileAndLinkTargetTypeId(LinkTarget linkTarget)
        {
            try
            {

                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                int sectorCheck = 0;
                if (linkTarget.LinkTargetTypeId == (int)LinkTargetTypeEnum.ConsultationRequest || linkTarget.LinkTargetTypeId == (int)LinkTargetTypeEnum.ConsultationFile)
                {
                    var sector = _DbContext.ComsConsultationFileSectorAssignments.Where(x => x.FileId == linkTarget.ReferenceId).Select(y => y.SectorTypeId).FirstOrDefault();
                    sectorCheck = (int)sector;
                    return await GetHOSBySectorId(sectorCheck);
                }
                else
                {
                    var sector = await _DbContext.CmsCaseFileSectorAssignment.Where(x => x.FileId == linkTarget.ReferenceId).Select(y => y.SectorTypeId).FirstOrDefaultAsync();
                    sectorCheck = (int)sector;
                    return await GetHOSBySectorId(sectorCheck);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<User> GetHOSBySectorId(int sectorTypeId)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                string StoreProc = $"exec pGetHOSBSectorId @sectorTypeId = '{sectorTypeId}'";
                var users = await _DbContext.Users.FromSqlRaw(StoreProc).ToListAsync();
                return users.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Sector MOJ
        //<History Author = 'Hassan Abbas' Date='2022-12-21' Version="1.0" Branch="master"> Get HOS By Sector</History>
        public async Task<User> GetMojBySectorId(int sectorTypeId)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                string StoreProc = $"exec pGetMojBySectorId @sectorTypeId = '{sectorTypeId}'";
                var users = await _DbContext.Users.FromSqlRaw(StoreProc).ToListAsync();
                return users.FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region Get Vice Sector HOS
        public async Task<List<User>> GetViceHOSBySectorId(int sectorTypeId)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

                string StoreProc = $"exec pGetViceHosBySectorId @sectorTypeId = '{sectorTypeId}'";
                var users = await _DbContext.Users.FromSqlRaw(StoreProc).ToListAsync();
                return users;
            }
            catch (Exception ex)
            {
                throw ex;

            }
            #endregion
        }

        #region Get Vice Sector HOS
        public async Task<List<User>> GetViceHOSOrManagerBySectorUserId(int sectorTypeId, string userName)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                List<User> User = new();
                var responsibleForAll = _DbContext.OperatingSectorType.Where(x => x.Id == sectorTypeId).Select(y => y.IsViceHosResponsibleForAllLawyers).FirstOrDefault();
                if (!responsibleForAll)
                {


                    var result = (from uei in _DbContext.UserEmploymentInformation
                                  join uu in _DbContext.Users on uei.UserId equals uu.Id
                                  where uu.UserName == userName
                                  select new
                                  {
                                      User = uei

                                  }).FirstOrDefault();

                    User = _DbContext.Users.Where(x => x.Id == result.User.ManagerId).ToList();
                    if (User.Count > 0)
                    {
                        return User;
                    }
                }
                else
                {
                    string StoreProc = $"exec pGetViceHosBySectorId @sectorTypeId = '{sectorTypeId}'";
                    User = await _DbContext.Users.FromSqlRaw(StoreProc).ToListAsync();
                    return User;
                }
                return User;

            }
            catch (Exception ex)
            {
                throw ex;

            }

        }
        //<History Author = 'Hassan Abbas' Date='2024-05-01' Version="1.0" Branch="master"> Get HOS and Vice HOS By Sector based on Parameters and Sector Configuration if required all or only Manager</History>
        public async Task<List<User>> GetHOSAndViceHOSBySectorId(int sectorTypeId, string username, bool verifyViceHOSResponsibility, bool returnHOS, int chamberNumberId)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                string StoreProc = $"exec pGetHOSAndViceHosBySectorId @sectorTypeId = '{sectorTypeId}', @username = '{username}', @verifyViceHOSResponsibility = '{verifyViceHOSResponsibility}', @returnHOS = '{returnHOS}', @chamberNumberId = '{chamberNumberId}'";
                var users = await _DbContext.Users.FromSqlRaw(StoreProc).ToListAsync();
                return users;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
        #region     Get Departments By GE Id
        //public async Task<List<GEDepartments>> GetDepartmentsByGEId(int entityId, string UserName)
        //      {
        //          List<GEDepartments> gEDepartments = new List<GEDepartments>();
        //	var a = _dbContext.Users.Where(x => x.UserName == UserName).FirstOrDefault();
        //          var b = _dbContext.UserRoles.Where(y => y.UserId == a.Id).Select(z => z.RoleId).FirstOrDefault();
        //          if (SystemRoles.CaseRoles.Contains(b))
        //          {
        //              var c = _dbContext.CmsSectorTypeGEDepartments.Where(x => x.SectorTypeId == (int)FatwaSectorTypeEnum.Case).Select(y => y.DepartmentId).ToList();
        //              if (c != null)
        //              {
        //			gEDepartments = _dbContext.GEDepartments.Where(x => c.Contains(x.Id) && x.EntityId == entityId).ToList();
        //		}
        //	}
        //          return gEDepartments;
        //}

        public async Task<List<GEDepartments>> GetDepartmentsByGEId(List<SendCommunicationVM> sendCommunication)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();

            var partyGEs = sendCommunication.Select(x => x.Communication.GovtEntityId)
                         .Where(id => id.HasValue)
                         .Select(id => id.Value)
                         .ToList();

            var additionalGEs = sendCommunication.SelectMany(x => x.CommunicationResponse.EntityIds).ToList();

            var fatwaSectorTypeId = CaseConsultationExtension.GetFatwaSectorTypeBasedOnSectorId(sendCommunication.FirstOrDefault().Communication.SectorTypeId);

            List<int> AllEntities = partyGEs.Concat(additionalGEs).ToList();
            var result = new List<GEDepartments>();

            foreach (var entityId in AllEntities)
            {
                var gEDepartments = await (
                    from dept in _DbContext.GEDepartments
                    join cmsDept in _DbContext.CmsSectorTypeGEDepartments on dept.Id equals cmsDept.DepartmentId
                    where cmsDept.SectorTypeId == fatwaSectorTypeId && dept.EntityId == entityId
                    select dept
                ).ToListAsync();

                if (!gEDepartments.Any())
                {
                    var defaultDepatment = await _DbContext.GEDepartments
                    .Where(x => x.EntityId == entityId && x.DefaultReceiver == true)
                    .ToListAsync();
                    if (defaultDepatment.Any())
                    {
                        gEDepartments.AddRange(defaultDepatment);
                    }
                }
                if (gEDepartments.Any())
                {
                    var userSect = await _DbContext.OperatingSectorType.Where(x => x.Id == sendCommunication.FirstOrDefault().Communication.SectorTypeId).FirstOrDefaultAsync();
                    var recieverSiteName = await _DbContext.GovernmentEntity.Where(x => x.EntityId == entityId).Select(y => y.Name_Ar).FirstOrDefaultAsync();

                    foreach (var dept in gEDepartments)
                    {
                        dept.SenderBranchId = userSect.G2GBRSiteID;
                        dept.RecieverSiteName = recieverSiteName;

                    }
                }
                result.AddRange(gEDepartments);
            }

            return result;
        }

        #endregion
    }
}

