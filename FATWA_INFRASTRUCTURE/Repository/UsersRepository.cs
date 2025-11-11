using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.BaseModels;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.DirectoryServices.AccountManagement;
using static FATWA_DOMAIN.Enums.GeneralEnums;
using static FATWA_DOMAIN.Enums.UserEnum;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using FATWA_DOMAIN.Models.ViewModel;
using static FATWA_DOMAIN.Enums.TaskEnums;

namespace FATWA_INFRASTRUCTURE.Repository
{
    public class UsersRepository : IUsers
    {
        private readonly DatabaseContext _dbContext;
        private readonly DmsDbContext _dmsdbContext;
        private List<UserTransferVM> _usersTransferVM;
        private UserDetailViewListVM userDetailViewListVMResult;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        private ActiveDirectorySettings _adSettings { get; set; }
        public UsersRepository(DatabaseContext dbContext, DmsDbContext dmsDbContext, UserManager<IdentityUser> userManager, IOptions<ActiveDirectorySettings> ADSettings, IMapper mapper, IServiceScopeFactory serviceScopeFactory)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _mapper = mapper;
            _adSettings = ADSettings.Value;
            _dmsdbContext = dmsDbContext;
            _serviceScopeFactory = serviceScopeFactory;

        }

        public async Task<List<UserTransferVM>> GetUmsUserTransfer(string userId)
        {
            try
            {
                if (_usersTransferVM == null)
                {
                    string StoredProc = $"exec pUserTransferDetaidByUserId @UserId = N'{userId}'";
                    var transferuser = _dbContext.TransferUsers.Where(x => x.Id == userId).ToList();

                    _usersTransferVM = await _dbContext.UserTransferVMs.FromSqlRaw(StoredProc).ToListAsync();
                }
                return _usersTransferVM;

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<List<UserListGroupVM>> GetUmsUser(string GroupId, bool IsView)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
                string StoredProc = "";
                if (GroupId == null)
                    StoredProc = $"exec pUserListSelSearch @IsView='{IsView}'";
                else
                    StoredProc = $"exec pUserListSelSearch @groupId ='{GroupId}',@IsView='{IsView}' ";
                return await _DbContext.UserListGroupVM.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private async Task UpdateAttachementsByUser(User user, DatabaseContext dbContext)
        {
            try
            {
                var attachements = await _dmsdbContext.TempAttachements.Where(x => x.Guid == Guid.Parse(user.Id)).ToListAsync();
                foreach (var file in attachements)
                {
                    UploadedDocument documentObj = new UploadedDocument();
                    documentObj.Description = "User Profile Picture";
                    documentObj.CreatedDateTime = DateTime.Now;
                    documentObj.DocumentDate = DateTime.Now;
                    documentObj.FileName = file.FileName;
                    documentObj.StoragePath = file.StoragePath;
                    documentObj.DocType = file.DocType;
                    documentObj.ReferenceGuid = Guid.Parse(user.Id);
                    documentObj.IsActive = true;
                    documentObj.CreatedBy = user.CreatedBy;
                    documentObj.CreatedAt = file.StoragePath;
                    documentObj.AttachmentTypeId = file.AttachmentTypeId;
                    documentObj.IsDeleted = false;
                    documentObj.CommunicationGuid = new Guid();
                    documentObj.IsMaskedAttachment = false;
                    await _dmsdbContext.UploadedDocuments.AddAsync(documentObj);
                    await _dmsdbContext.SaveChangesAsync();
                    await Task.Delay(200);
                    _dmsdbContext.TempAttachements.Remove(file);
                    await _dmsdbContext.SaveChangesAsync();
                    if (file.AttachmentTypeId == (int)AttachmentTypeEnum.SignatureImage)
                    {
                        User uptuser = new User();
                        uptuser.Id = user.Id;
                        uptuser.HasSignatureImage = true;
                        _dbContext.Entry(uptuser).Property(x => x.HasSignatureImage).IsModified = true;
                        await _dbContext.SaveChangesAsync();
                    }
                }

                foreach (var deletedAttachementId in user.DeletedAttachementIds)
                {
                    var attachement = await _dmsdbContext.UploadedDocuments.FindAsync(deletedAttachementId);
                    if (attachement != null)
                    {
                        if (File.Exists(attachement.StoragePath))
                        {
                            File.Delete(attachement.StoragePath);
                        }
                        _dmsdbContext.UploadedDocuments.Remove(attachement);

                        if (attachement.AttachmentTypeId == (int)AttachmentTypeEnum.SignatureImage)
                        {
                            User uptuser = new User();
                            uptuser.Id = user.Id;
                            uptuser.HasSignatureImage = false;
                            _dbContext.Entry(uptuser).Property(x => x.HasSignatureImage).IsModified = true;
                            await _dbContext.SaveChangesAsync();
                        }
                        await _dmsdbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region Get user detail by id (for Detail View)

        public async Task<UserDetailViewListVM> GetUserDetailsById(string userId)
        {
            try
            {
                if (userDetailViewListVMResult == null)
                {
                    string StoredProc = $"exec pUserDetaiViewList @UserId = N'{userId}'";
                    var result = await _dbContext.UserDetailViewListVMs.FromSqlRaw(StoredProc).ToListAsync();
                    if (result != null)
                    {
                        foreach (var item in result)
                        {
                            userDetailViewListVMResult = new UserDetailViewListVM
                            {
                                Id = item.Id,
                                FirstName_En = item.FirstName_En,
                                FirstName_Ar = item.FirstName_Ar,
                                SecondName_En = item.SecondName_En,
                                SecondName_Ar = item.SecondName_Ar,
                                LastName_En = item.LastName_En,
                                LastName_Ar = item.LastName_Ar,
                                Address_En = item.Address_En,
                                PhoneNumber = item.PhoneNumber,
                                AlternatePhoneNumber = item.AlternatePhoneNumber,
                                Email = item.Email,
                                DateOfJoining = item.DateOfJoining,
                                UserName = item.UserName,
                                PasswordHash = item.PasswordHash,
                                GenderName_En = item.GenderName_En,
                                GenderName_Ar = item.GenderName_Ar,
                                NationalityName_En = item.NationalityName_En,
                                NationalityName_Ar = item.NationalityName_Ar,
                                TypeName_En = item.TypeName_En,
                                TypeName_Ar = item.TypeName_Ar,
                                DesignationName_En = item.DesignationName_En,
                                DesignationName_Ar = item.DesignationName_Ar,
                                DepartmentName_En = item.DepartmentName_En,
                                DepartmentName_Ar = item.DepartmentName_Ar
                            };
                        }
                    }
                }
                return userDetailViewListVMResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region
        private List<EmployeeVM> _usersList;
        public async Task<List<EmployeeVM>> GetEmployeeList(UserListAdvanceSearchVM advanceSearchVM)
        {
            try
            {
                string StoredProc;
                if (_usersList == null)
                {
                    StoredProc = $"exec pEmployeeSelAll @name =N'{advanceSearchVM.Name}', @civilId='{advanceSearchVM.CivilId}',@SectorId='{advanceSearchVM.SectorId}',@designationId='{advanceSearchVM.DesignationId}',@companyId='{advanceSearchVM.CompanyId}',@employeeStatusId='{advanceSearchVM.EmployeeStatusId}',@passportNumber='{advanceSearchVM.PassportNumber}'" +
                        $",@employeeTypeId='{advanceSearchVM.EmployeeTypeId}',@PageNumber ='{advanceSearchVM.PageNumber}',@PageSize ='{advanceSearchVM.PageSize}'";
                    _usersList = await _dbContext.EmployeeVM.FromSqlRaw(StoredProc).ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return _usersList;
        }

        public async Task<List<EmployeeVMForDropDown>> GetEmployeeList(int sectorTypeId, int attachementId, int documentId)
        {
            try
            {
                List<EmployeeVMForDropDown> _usersList = new List<EmployeeVMForDropDown>();

                // Get Users who signed the document
                var signedUsers = await _dmsdbContext.DsSigningRequestTaskLogs.Where(x => x.DocumentId == documentId && x.StatusId == 4).Select(x => x.ReceiverId).ToListAsync();
                var designationIds = await _dmsdbContext.DsAttachmentTypeDesignationMapping.Where(x => x.AttachmentTypeId == attachementId).Select(x => x.DesignationId).ToListAsync();
                var userIds = await _dbContext.UserEmploymentInformation.Where(x => designationIds.Contains(x.DesignationId) && x.SectorTypeId == sectorTypeId).Select(x => x.UserId).ToListAsync();

                var filteredUserIds = userIds.Except(signedUsers).ToList();

                var users = await _dbContext.UserPersonalInformation.Where(x => filteredUserIds.Contains(x.UserId)).ToListAsync();

                var employeeList = users.Select(user => new EmployeeVMForDropDown
                {
                    UserId = user.UserId,
                    EmployeeNameEn = string.Join(" ", new[] { user.FirstName_En, user.SecondName_En, user.LastName_En }.Where(n => !string.IsNullOrEmpty(n))),
                    EmployeeNameAr = string.Join(" ", new[] { user.FirstName_Ar, user.SecondName_Ar, user.LastName_Ar }.Where(n => !string.IsNullOrEmpty(n)))
                }).ToList();

                _usersList.AddRange(employeeList);
                return _usersList;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<UserDataVM>> GetUserData()
        {
            try
            {
                return await _dbContext.UserDataVM.FromSqlRaw("exec pFetchAllUserData").ToListAsync();

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        #region Employee Leave Delegation
        //<History Author='Ammaar Naveed' Date='29-04-2024'>Employee Leave Delegation Implementation for Vice HOS.</History>//  
        public async Task<bool> CheckEmployeeLeaveStatus(string UserId, DateTime? FromDate, DateTime? ToDate)
        {
            try
            {
                bool isEmployeeOnLeave = false;
                if (UserId != null)
                {
                    string LeaveFromDate = Convert.ToDateTime(FromDate).ToString("yyyy-MM-dd").ToString();
                    string LeaveToDate = Convert.ToDateTime(ToDate).ToString("yyyy-MM-dd").ToString();
                    string StoreProc = $"exec pGetEmployeeLeaveDetails @FromDate='{LeaveFromDate}', @ToDate='{LeaveToDate}', @UserId='{UserId}'";
                    var result = await _dbContext.EmployeeLeaveDelegation.FromSqlRaw(StoreProc).ToListAsync();
                    if (result.Count != 0)
                        isEmployeeOnLeave = true;
                }
                return isEmployeeOnLeave;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        private List<EmployeeDelegationVM> AlternateEmployeesList;
        public async Task<List<EmployeeDelegationVM>> GetAlternateEmployeesList(int? SectorTypeId, string RoleId, DateTime? FromDate, DateTime? ToDate)
        {
            try
            {
                string LeaveFromDate = Convert.ToDateTime(FromDate).ToString("yyyy/MM/dd").ToString();
                string LeaveToDate = Convert.ToDateTime(ToDate).ToString("yyyy/MM/dd").ToString();
                string StoreProc = $"exec pGetAlternateUser @SectorTypeId='{SectorTypeId}', @RoleId='{RoleId}',@FromDate='{LeaveFromDate}' ,@ToDate='{LeaveToDate}'";
                AlternateEmployeesList = await _dbContext.EmployeeDelegationVM.FromSqlRaw(StoreProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return AlternateEmployeesList;
        }
        public async Task SaveDelegatedEmployee(EmployeeLeaveDelegationInformation delegatedEmployeeInformation)
        {
            try
            {
                if (delegatedEmployeeInformation.Id == Guid.Empty)
                {
                    delegatedEmployeeInformation.Id = Guid.NewGuid();
                }
                delegatedEmployeeInformation.CreatedDate = DateTime.Today;
                delegatedEmployeeInformation.IsDeleted = false;

                _dbContext.EmployeeLeaveDelegation.Add(delegatedEmployeeInformation);
                await AssignTasksToDelegatedEmployee(delegatedEmployeeInformation);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private async Task AssignTasksToDelegatedEmployee(EmployeeLeaveDelegationInformation delegationInformation)
        {
            var assignedTasks = _dbContext.Tasks
                .Where(x => x.AssignedTo == delegationInformation.UserId &&
                            x.TaskStatusId == (int)TaskStatusEnum.Pending &&
                            (x.Date >= delegationInformation.FromDate && x.Date <= delegationInformation.ToDate));

            foreach (var task in assignedTasks)
            {
                task.AssignedTo = delegationInformation.DelegatedUserId;
                task.ModifiedBy = delegationInformation.CreatedBy;
                task.ModifiedDate = DateTime.Now;
            }

            await _dbContext.SaveChangesAsync();
        }
        public async Task<List<UserDataVM>> GetUsersByBugTypeId(int TypeId)
        {
            try
            {
                string StoredProc = $"exec pGetUsersDataByBugTypeId @BugTypeId  ='{TypeId}'";
                return await _dbContext.UserDataVM.FromSqlRaw(StoredProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        #endregion

        #endregion

        #region Get Employees List For Admin
        private List<EmployeesListVM> EmployeesList;
        private List<EmployeesListDropdownVM> UserGroupEmployeesList;
        public async Task<List<EmployeesListVM>> GetEmployeesListForAdmin(int EmployeeTypeId, int? SectorTypeId, int? DesignationId)
        {
            try
            {
                string StoredProc;
                if (EmployeesList == null)
                {
                    StoredProc = $"exec pGetEmployeesListForAdmin @SectorTypeId='{SectorTypeId}',@DesignationId='{DesignationId}' ,@EmployeeTypeId='{EmployeeTypeId}'";
                    EmployeesList = await _dbContext.EmployeesListVM.FromSqlRaw(StoredProc).AsNoTracking().ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return EmployeesList;
        }
        public async Task<List<EmployeesListDropdownVM>> GetEmployeesListForUserGroup(UserListAdvanceSearchVM advanceSearchVM)
        {
            try
            {
                string StoredProc;
                if (UserGroupEmployeesList == null)
                {
                    StoredProc = $"exec pGetEmployeesListForUserGroup @SectorTypeId='{advanceSearchVM.SectorId}' ,@EmployeeTypeId='{advanceSearchVM.EmployeeTypeId}', @RoleId='{advanceSearchVM.RoleId}'";
                    UserGroupEmployeesList = await _dbContext.UserGroupEmployeesListVM.FromSqlRaw(StoredProc).AsNoTracking().ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return UserGroupEmployeesList;
        }
        #endregion

        #region UMS_EP

        public async Task<List<Department>> Department()
        {
            try
            {
                return await _dbContext.Departments.OrderBy(u => u.Id).AsNoTracking().ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task<List<Nationality>> GetNationality()
        {
            try
            {
                return await _dbContext.Nationalities.Where(x => x.IsActive && !x.IsDeleted).AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<bool> CheckCivilIdExists(string civilId)
        {
            try
            {
                if (civilId != null)
                {
                    return await _dbContext.UserPersonalInformation.AnyAsync(x => x.CivilId == civilId);
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<Gender>> GetGenders()
        {
            try
            {
                return await _dbContext.Genders.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<UserAdress>> GetUserAdress()
        {
            try
            {
                return await _dbContext.UserAdress.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<City>> GetCities()
        {
            try
            {
                return await _dbContext.Cities.Include(x => x.Governorate).ThenInclude(x => x.Country).AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<Country>> GetCountries()
        {
            try
            {
                return await _dbContext.Countries.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<IEnumerable<Designation>> GetDesignations()
        {
            try
            {
                return await _dbContext.Designations.Where(x => !x.IsDeleted).AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<EmployeeType>> GetEmployeeType()
        {
            try
            {
                return await _dbContext.EmployeeTypes.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<Company>> GetCompanies()
        {
            try
            {
                return await _dbContext.Companies.Include(x => x.City).ThenInclude(x => x.Governorate).ThenInclude(x => x.Country).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<Department>> GetEmployeeDepartment()
        {
            try
            {
                return await _dbContext.Departments.Where(x => x.IsActive && !x.IsDeleted).Include(x => x.SectorTypes).AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<OperatingSectorType>> GetEmployeeSectortype()
        {
            try
            {
                return await _dbContext.OperatingSectorType.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<Grade>> GetEmployeeGrade()
        {
            try
            {
                return await _dbContext.UserGrades.Where(x => x.IsActive && !x.IsDeleted).AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<GradeType>> GetGradeTypes()
        {
            try
            {
                var result = await _dbContext.GradeTypes.Where(x => !x.IsDeleted).AsNoTracking().ToListAsync();
                if (result != null)
                {
                    return result;
                }
                else return Enumerable.Empty<GradeType>().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<ContractType>> GetContractTypes()
        {
            try
            {
                var result = await _dbContext.ContractTypes.Where(x => !x.IsDeleted).AsNoTracking().ToListAsync();
                return result ?? Enumerable.Empty<ContractType>().ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<List<EmployeeWorkingTime>> GetWorkingTime()
        {
            try
            {
                return await _dbContext.WorkingTimes.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<EmployeeStatus>> GetEmployeeStatus()
        {
            try
            {
                return await _dbContext.EmployeeStatuses.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<List<Governorate>> GetGovernorates()
        {
            try
            {
                return await _dbContext.Governorates.AsNoTracking().ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        //< History Author = "Ammaar Naveed" Date = "07/05/2024" Version = "1.0" Branch = "master" >Get managers by sector type id and hierarchy definition against role</ History >
        public async Task<List<ManagersListVM>> GetManagersList(int? SectorTypeId, int DesignationId)
        {
            try
            {
                string StoreProc = $"exec pGetManagersBySectorId @SectorTypeId={SectorTypeId}, @DesignationId='{DesignationId}'";
                return await _dbContext.ManagersListVM.FromSqlRaw(StoreProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        //< History Author = "Ammaar Naveed" Date = "06/06/2024" Version = "1.0" Branch = "master" >Get employees list by role and sector for delegation purposes</ History >
        public async Task<IEnumerable<EmployeeDelegationVM>> GetEmployeesByRoleSectorAndDesignation(int SectorTypeId, string RoleId, int DesignationId)
        {
            try
            {
                string StoreProc = $"exec pGetUsersByRoleIdandSectorId @sectorTypeId={SectorTypeId}, @RoleId='{RoleId}', @DesignationId='{DesignationId}'";
                return await _dbContext.EmployeeDelegationVM.FromSqlRaw(StoreProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        //< History Author = "Ammaar Naveed" Date = "08/08/2024" Version = "1.0" Branch = "master" >Get employees list by designation Id</ History >
        public async Task<List<UserClaimsVM>> GetEmployeesByDesignationId(int? DesignationId)
        {
            try
            {
                string StoreProc = $"exec pGetEmployeesByDesignationId @DesignationId='{DesignationId}'";
                return await _dbContext.UserClaimsVM.FromSqlRaw(StoreProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
        //< History Author = "Ammaar Naveed" Date = "08/08/2024" Version = "1.0" Branch = "master" >Get UMS claims list by module Id</ History >
        public async Task<List<UserClaimsVM>> GetUmsClaimsByModuleId(int? moduleId)
        {
            try
            {
                string StoreProc = $"exec pGetUmsClaimsListByModuleId @ModuleId='{moduleId}'";
                return await _dbContext.UserClaimsVM.FromSqlRaw(StoreProc).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        //<History Author='Ammaar Naveed' Date='27-02-2024'>Get employee placement -> floor and building</History>//
        public async Task<AddEmployeeVM> GetEmployeeDetailById(Guid userId)
        {
            try
            {
                if (userId != Guid.Empty)
                {
                    AddEmployeeVM addEmployeeVM = new AddEmployeeVM();
                    addEmployeeVM.UserId = userId;
                    var user = await _dbContext.Users
                                           .Where(u => u.Id == userId.ToString())
                                           .Select(u => new
                                           {
                                               u.Email,
                                               u.CreatedBy,
                                               u.CreatedDate,
                                               u.ModifiedBy,
                                               u.ModifiedDate
                                           })
                                           .FirstOrDefaultAsync();
                    var userDetail = _dbContext.Users
                        .Where(u => u != null)
                        .Select(u => new
                        {
                            CreatedByName_En = _dbContext.Users
                                                .Where(cbu => cbu.Email == user.CreatedBy)
                                                .Select(cbu => _dbContext.UserPersonalInformation
                                                    .Where(epi => epi.UserId == cbu.Id)
                                                    .Select(epi => epi.FirstName_En + " " + epi.SecondName_En + " " + epi.LastName_En)
                                                    .FirstOrDefault())
                                                .FirstOrDefault(),
                            CreatedByName_Ar = _dbContext.Users
                                                .Where(cbu => cbu.Email == user.CreatedBy)
                                                .Select(cbu => _dbContext.UserPersonalInformation
                                                    .Where(epi => epi.UserId == cbu.Id)
                                                    .Select(epi => epi.FirstName_Ar + " " + epi.SecondName_Ar + " " + epi.LastName_Ar)
                                                    .FirstOrDefault())
                                                .FirstOrDefault(),
                            ModifiedByName_En = _dbContext.Users
                                                 .Where(mbu => mbu.Email == user.ModifiedBy)
                                                 .Select(mbu => _dbContext.UserPersonalInformation
                                                     .Where(epi => epi.UserId == mbu.Id)
                                                     .Select(epi => epi.FirstName_En + " " + epi.SecondName_En + " " + epi.LastName_En)
                                                     .FirstOrDefault())
                                                 .FirstOrDefault(),
                            ModifiedByName_Ar = _dbContext.Users
                                                 .Where(mbu => mbu.Email == user.ModifiedBy)
                                                 .Select(mbu => _dbContext.UserPersonalInformation
                                                     .Where(epi => epi.UserId == mbu.Id)
                                                     .Select(epi => epi.FirstName_Ar + " " + epi.SecondName_Ar + " " + epi.LastName_Ar)
                                                     .FirstOrDefault())
                                                 .FirstOrDefault()
                        })
                        .FirstOrDefault();

                    if (user != null)
                    {
                        addEmployeeVM.Email = user.Email;
                        addEmployeeVM.CreatedByName_En = userDetail.CreatedByName_En;
                        addEmployeeVM.CreatedByName_Ar = userDetail.CreatedByName_Ar;
                        addEmployeeVM.CreatedDate = user.CreatedDate;
                        addEmployeeVM.ModifiedByName_En = userDetail.ModifiedByName_En;
                        addEmployeeVM.ModifiedByName_Ar = userDetail.ModifiedByName_Ar;
                        addEmployeeVM.ModifiedDate = user.ModifiedDate;
                    }
                    addEmployeeVM.ActiveDirectoryUserName = await _dbContext.Users.Where(u => u.Id == userId.ToString()).Select(u => u.ADUserName).FirstOrDefaultAsync();
                    addEmployeeVM.userPersonalInformation = await _dbContext.UserPersonalInformation
                                            .Include(x => x.Nationality)
                                            .Include(x => x.Gender)
                                            .Include(x => x.UserAdresses)
                                            .ThenInclude(x => x.City)
                                            .ThenInclude(x => x.Governorate)
                                            .ThenInclude(x => x.Country)
                                            .Where(u => u.UserId == userId.ToString())
                                            .FirstOrDefaultAsync();
                    addEmployeeVM.UserAdresses = addEmployeeVM.userPersonalInformation.UserAdresses?.ToList();
                    addEmployeeVM.UserEmploymentInformation = await _dbContext.UserEmploymentInformation
                                            .Include(x => x.Company)
                                            .ThenInclude(x => x.City)
                                            .ThenInclude(x => x.Governorate)
                                            .ThenInclude(x => x.Country)
                                            .Include(x => x.SectorType)
                                            .ThenInclude(x => x.Department)
                                            .Include(x => x.EmployeeStatus)
                                            .Include(x => x.Grade)
                                            .Include(x => x.WorkingTime)
                                            .Include(x => x.Designation)
                                            .Include(x => x.EmployeeType)
                                            .Include(x => x.ContractType)
                                            .Where(u => u.UserId == userId.ToString())
                                            .FirstOrDefaultAsync();
                    addEmployeeVM.UserEmploymentInformation.DepartmentId = await _dbContext.OperatingSectorType
                                            .Where(s => s.Id == addEmployeeVM.UserEmploymentInformation.SectorTypeId)
                                            .Select(s => s.DepartmentId)
                                            .FirstOrDefaultAsync();

                    addEmployeeVM.UserEmploymentInformation.Supervisor = await _dbContext.UserPersonalInformation.Where(x => x.UserId == addEmployeeVM.UserEmploymentInformation.SupervisorId).FirstOrDefaultAsync();
                    addEmployeeVM.UserEmploymentInformation.Manager = await _dbContext.UserPersonalInformation.Where(x => x.UserId == addEmployeeVM.UserEmploymentInformation.ManagerId).FirstOrDefaultAsync();
                    var delegatedUserId = await _dbContext.UserEmploymentInformation
                                            .Where(ei => ei.UserId == userId.ToString())
                                            .Select(emp => emp.DelegatedUserId)
                                            .FirstOrDefaultAsync();

                    if (delegatedUserId != null)
                    {
                        addEmployeeVM.DelegatedEmployeeName_En = await _dbContext.UserPersonalInformation
                            .Where(epi => epi.UserId == delegatedUserId)
                            .Select(epi => epi.FirstName_En + " " + epi.SecondName_En + " " + epi.LastName_En)
                            .FirstOrDefaultAsync();
                        addEmployeeVM.DelegatedEmployeeName_Ar = await _dbContext.UserPersonalInformation
                            .Where(epi => epi.UserId == delegatedUserId)
                            .Select(epi => epi.FirstName_Ar + " " + epi.SecondName_Ar + " " + epi.LastName_Ar)
                            .FirstOrDefaultAsync();
                    }
                    addEmployeeVM.GroupId = await _dbContext.UserGroups.Where(x => x.UserId == userId.ToString()).Select(x => x.GroupId).FirstOrDefaultAsync();
                    if (addEmployeeVM.GroupId != Guid.Empty)
                    {
                        addEmployeeVM.Group = await _dbContext.Groups.Where(x => x.GroupId == addEmployeeVM.GroupId).FirstOrDefaultAsync();
                        addEmployeeVM.GroupTypeId = addEmployeeVM.Group.GroupTypeId;
                    }
                    addEmployeeVM.RoleId = await _dbContext.UserRoles.Where(x => x.UserId == userId.ToString()).Select(x => x.RoleId).FirstOrDefaultAsync();
                    if (addEmployeeVM.RoleId != null)
                        addEmployeeVM.Role = await _dbContext.Roles.Where(x => x.Id == addEmployeeVM.RoleId).FirstOrDefaultAsync();
                    addEmployeeVM.UserEducationalInformation = await _dbContext.UserEducationalInformation.Where(u => u.UserId == userId.ToString()).ToListAsync();
                    addEmployeeVM.UserWorkExperiences = await _dbContext.UserWorkExperience.Where(u => u.UserId == userId.ToString()).ToListAsync();
                    addEmployeeVM.userTrainingAttendeds = await _dbContext.UserTrainingAttended.Where(u => u.UserId == userId.ToString()).ToListAsync();
                    addEmployeeVM.UserContactInformationList = await _dbContext.UserContactInformation.Include(x => x.ContactType).Where(u => u.UserId == userId.ToString()).ToListAsync();

                    if (_dbContext.Tasks.Any(x => x.AssignedTo == userId.ToString()))
                        addEmployeeVM.IsUserHasAnyTask = true;
                    return addEmployeeVM;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public async Task EditEmployee(AddEmployeeVM updatedEmployee)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        string UserId = updatedEmployee.UserId.ToString();
                        if (updatedEmployee.IsEmailModified)
                            await UpdateUMSUSER(_dbContext, updatedEmployee.Email, UserId, updatedEmployee.ModifiedBy);
                        await UpdateUserPersonalInformation(_dbContext, updatedEmployee.userPersonalInformation, UserId);
                        if (updatedEmployee.ActiveDirectoryUserName != null)
                            await UpdateUserInActiveDirectory(updatedEmployee.userPersonalInformation, updatedEmployee.ActiveDirectoryUserName, updatedEmployee.Email);
                        await UpdateUserEmployementformation(_dbContext, updatedEmployee.UserEmploymentInformation, UserId, updatedEmployee);
                        //await AddUpdateLawyerSupervisorManager(updatedEmployee.UserEmploymentInformation, updatedEmployee.ModifiedBy);
                        await updateUserAddresses(_dbContext, updatedEmployee.UserAdresses, UserId);
                        await updateUserEducationInfo(_dbContext, updatedEmployee.UserEducationalInformation, UserId);
                        await updateUserWorkExperience(_dbContext, updatedEmployee.UserWorkExperiences, UserId);
                        await updateUserTrainings(_dbContext, updatedEmployee.userTrainingAttendeds, UserId);
                        await updateUserContacts(_dbContext, updatedEmployee.UserContactInformationList, UserId, updatedEmployee.ModifiedBy);

                        #region Update User Group
                        //<History Author='Ammaar Naveed' Date='12-02-2024'>Edit Employee Group. Remove the existing group and create a new entry on every edit.</History>//
                        /*var existingUserGroup = await _dbContext.UserGroups
                            .FirstOrDefaultAsync(x => x.UserId == UserId);*/
                        //Execute if and only if existing GroupId doesn't matches the updated GroupId.
                        /*if (existingUserGroup != null && (existingUserGroup.GroupId != updatedEmployee.GroupId))
                        {
                            _dbContext.UserGroups.Remove(existingUserGroup);

                            var newUserGroup = new UserGroup
                            {
                                UserId = UserId,
                                GroupId = updatedEmployee.GroupId,
                                CreatedDate = DateTime.Now,
                                CreatedBy = updatedEmployee.ModifiedBy
                            };
                            _dbContext.UserGroups.Add(newUserGroup);
                            await _dbContext.SaveChangesAsync();
                        }*/
                        #endregion

                        // For Document update
                        #region Document update
                        var user = new User()
                        {
                            DeletedAttachementIds = updatedEmployee.DeletedAttachementIds,
                            Id = UserId,
                            CreatedBy = updatedEmployee.UserId.ToString()
                        };
                        await UpdateAttachementsByUser(user, _dbContext);
                        #endregion

                        #region Update Security Stamp
                        var IdentityUser = await _userManager.FindByEmailAsync(updatedEmployee.Email);
                        await _userManager.UpdateSecurityStampAsync(IdentityUser);
                        #endregion

                        await _dbContext.SaveChangesAsync();

                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception(ex.Message);
                    }
                }
            }
        }

        #region Update User Additional Informations
        //<History Author='AttiqueRehman' Date='20-02-2024'> Update User Addresses </History>
        protected async Task updateUserAddresses(DatabaseContext _dbContext, List<UserAdress> userAddress, string UserId)
        {
            try
            {
                var userAdresses = await _dbContext.UserAdress.Where(e => e.UserId == UserId).ToListAsync();
                if (userAdresses.Any())
                {
                    _dbContext.UserAdress.RemoveRange(userAdresses);
                    await _dbContext.SaveChangesAsync();
                }
                foreach (var updateAddress in userAddress)
                {
                    var newAddress = new UserAdress
                    {
                        UserId = UserId,
                        Address = updateAddress.Address,
                        CityId = updateAddress.CityId,
                    };
                    await _dbContext.UserAdress.AddAsync(newAddress);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
        //<History Author='AttiqueRehman' Date='20-02-2024'> Update User Educations </History>
        protected async Task updateUserEducationInfo(DatabaseContext _dbContext, List<UserEducationalInformation> userEducation, string UserId)
        {
            try
            {
                var userEducations = await _dbContext.UserEducationalInformation.Where(e => e.UserId == UserId).ToListAsync();
                if (userEducations.Any())
                {
                    _dbContext.UserEducationalInformation.RemoveRange(userEducations);
                    await _dbContext.SaveChangesAsync();
                }
                foreach (var item in userEducation)
                {
                    var newEducation = new UserEducationalInformation
                    {
                        UserId = UserId,
                        EducationId = item.EducationId,
                        Percentage_Grade = item.Percentage_Grade,
                        MajoringName = item.MajoringName,
                        Comments = item.Comments,
                        UniversityCity = item.UniversityCity,
                        UniversityCountry = item.UniversityCountry,
                        UniversityName = item.UniversityName,
                    };
                    _dbContext.UserEducationalInformation.Add(newEducation);
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        //<History Author='AttiqueRehman' Date='20-02-2024'> Update User Work Experiences </History>
        protected async Task updateUserWorkExperience(DatabaseContext _dbContext, List<UserWorkExperience> userWorkExperiences, string UserId)
        {
            try
            {
                var userWorkExp = await _dbContext.UserWorkExperience.Where(e => e.UserId == UserId).ToListAsync();
                if (userWorkExp.Any())
                {
                    _dbContext.UserWorkExperience.RemoveRange(userWorkExp);
                    await _dbContext.SaveChangesAsync();
                }
                foreach (var item in userWorkExperiences)
                {
                    var newWorkExp = new UserWorkExperience
                    {
                        UserId = UserId,
                        ExperienceId = item.ExperienceId,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                        CompanyName = item.CompanyName,
                        JobTitle = item.JobTitle,
                        JobExperience = item.JobExperience,
                    };
                    _dbContext.UserWorkExperience.Add(newWorkExp);
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        //<History Author='AttiqueRehman' Date='20-02-2024'> Update User Trainings </History>
        protected async Task updateUserTrainings(DatabaseContext _dbContext, List<UserTrainingAttended> userTrainings, string UserId)
        {
            try
            {
                var userTrainingExp = await _dbContext.UserTrainingAttended.Where(e => e.UserId == UserId).ToListAsync();
                if (userTrainingExp.Any())
                {
                    _dbContext.UserTrainingAttended.RemoveRange(userTrainingExp);
                    await _dbContext.SaveChangesAsync();
                }
                foreach (var item in userTrainings)
                {
                    var newTraining = new UserTrainingAttended
                    {
                        UserId = UserId,
                        TrainingLocation = item.TrainingLocation,
                        TrainingName = item.TrainingName,
                        TrainingCenterName = item.TrainingCenterName,
                        Comments = item.Comments,
                        Percentage_Grade = item.Percentage_Grade,
                        StartDate = item.StartDate,
                        EndDate = item.EndDate,
                    };
                    _dbContext.UserTrainingAttended.Add(newTraining);
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {

                throw;
            }
        }
        //<History Author='Ammaar Naveed' Date='02-02-2024'>Updating and Deleting User Contact Information In Database.</History>//
        //<History Modified By >>>> Author='AttiqueRehman' Date='20-02-2024'> Update User Contact Information </History>
        protected async Task updateUserContacts(DatabaseContext _dbContext, List<UserContactInformation> userContact, string UserId, string CreatedBy)
        {
            try
            {
                var userContacts = await _dbContext.UserContactInformation.Where(e => e.UserId == UserId).ToListAsync();
                if (userContacts.Any())
                {
                    _dbContext.UserContactInformation.RemoveRange(userContacts);
                    await _dbContext.SaveChangesAsync();
                }
                foreach (var item in userContact)
                {
                    var newContact = new UserContactInformation
                    {
                        Id = item.Id,
                        UserId = UserId,
                        ContactNumber = item.ContactNumber,
                        ContactTypeId = item.ContactTypeId,
                        IsPrimary = item.IsPrimary,
                        IsActive = item.IsActive,
                        CreatedDate = DateTime.Now,
                        CreatedBy = CreatedBy
                    };
                    _dbContext.UserContactInformation.Add(newContact);
                }
                await _dbContext.SaveChangesAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }
        //<History Author='AttiqueRehman' Date='20-02-2024'> Update User Trainings </History>
        protected async Task UpdateUMSUSER(DatabaseContext _dbContext, string Email, string UserId, string ModifiedBy)
        {
            try
            {
                User uptuser = new User();
                uptuser.Id = UserId;
                uptuser.Email = Email;
                uptuser.UserName = Email;
                uptuser.NormalizedEmail = Email.ToUpper();
                uptuser.NormalizedUserName = Email.ToUpper();
                uptuser.ModifiedBy = ModifiedBy;
                uptuser.ModifiedDate = DateTime.Now;
                _dbContext.Entry(uptuser).Property(x => x.Email).IsModified = true;
                _dbContext.Entry(uptuser).Property(x => x.UserName).IsModified = true;
                _dbContext.Entry(uptuser).Property(x => x.NormalizedEmail).IsModified = true;
                _dbContext.Entry(uptuser).Property(x => x.NormalizedUserName).IsModified = true;
                _dbContext.Entry(uptuser).Property(x => x.ModifiedBy).IsModified = true;
                _dbContext.Entry(uptuser).Property(x => x.ModifiedDate).IsModified = true;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }
        //<History Author='AttiqueRehman' Date='20-02-2024'> Update User Personal Information </History>
        protected async Task UpdateUserPersonalInformation(DatabaseContext _dbContext, UserPersonalInformation userPersonalInformation, string UserId)
        {
            try
            {
                var updatePersonalInfo = await _dbContext.UserPersonalInformation.Where(u => u.UserId == UserId).FirstOrDefaultAsync();
                //updatePersonalInfo.UserId = UserId;
                updatePersonalInfo.FirstName_En = userPersonalInformation.FirstName_En;
                updatePersonalInfo.SecondName_En = userPersonalInformation.SecondName_En;
                updatePersonalInfo.LastName_En = userPersonalInformation.LastName_En;
                updatePersonalInfo.FirstName_Ar = userPersonalInformation.FirstName_Ar;
                updatePersonalInfo.SecondName_Ar = userPersonalInformation.SecondName_Ar;
                updatePersonalInfo.LastName_Ar = userPersonalInformation.LastName_Ar;
                updatePersonalInfo.DateOfBirth = userPersonalInformation.DateOfBirth;
                updatePersonalInfo.CivilId = userPersonalInformation.CivilId;
                updatePersonalInfo.GenderId = userPersonalInformation.GenderId;
                updatePersonalInfo.NationalityId = userPersonalInformation.NationalityId;
                updatePersonalInfo.PassportNumber = userPersonalInformation.PassportNumber;
                _dbContext.UserPersonalInformation.Update(updatePersonalInfo);
            }
            catch (Exception)
            {
                throw;
            }
        }
        //<History Author='Ammaar Naveed' Date='2024-05-23'>Update specific details of user in Active Directory</History>
        protected async Task UpdateUserInActiveDirectory(UserPersonalInformation userPersonalInformation, string ActiveDirectoryUsername, string Email)
        {
            try
            {
                var context = new PrincipalContext(ContextType.Domain, _adSettings.ServerIPAddress, _adSettings.Container, _adSettings.MachineAccountName, _adSettings.MachineAccountPassword);
                var principal = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, ActiveDirectoryUsername);

                if (principal != null)
                {
                    // Update user details
                    principal.GivenName = userPersonalInformation.FirstName_En;
                    principal.Surname = userPersonalInformation.LastName_En;
                    principal.MiddleName = userPersonalInformation.SecondName_En;
                    principal.DisplayName = userPersonalInformation.FirstName_En + " " + userPersonalInformation.LastName_En;
                    principal.EmailAddress = Email;
                    principal.Save();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //<History Author='AttiqueRehman' Date='20-02-2024'> Update User Employement Information </History>
        protected async Task UpdateUserEmployementformation(DatabaseContext _dbContext, UserEmploymentInformation UserEmploymentInformation, string UserId, AddEmployeeVM updatedEmployee)
        {
            try
            {
                var updateEmpInfo = await _dbContext.UserEmploymentInformation.Where(u => u.UserId == UserId).FirstOrDefaultAsync();

                updateEmpInfo.DesignationId = UserEmploymentInformation.DesignationId;
                updateEmpInfo.DateOfJoining = UserEmploymentInformation.DateOfJoining;
                updateEmpInfo.DepartmentId = UserEmploymentInformation.DepartmentId;
                updateEmpInfo.SectorTypeId = UserEmploymentInformation.SectorTypeId;
                updateEmpInfo.CompanyId = UserEmploymentInformation.CompanyId;
                updateEmpInfo.SupervisorId = UserEmploymentInformation.SupervisorId;
                updateEmpInfo.ManagerId = UserEmploymentInformation.ManagerId;
                updateEmpInfo.GradeId = UserEmploymentInformation.GradeId;
                updateEmpInfo.ResignedTerminationdDate = UserEmploymentInformation.ResignedTerminationdDate;
                updateEmpInfo.WorkingTimeId = UserEmploymentInformation.WorkingTimeId;
                updateEmpInfo.EmployeeStatusId = UserEmploymentInformation.EmployeeStatusId;
                updateEmpInfo.FingerPrintId = UserEmploymentInformation.FingerPrintId;
                updateEmpInfo.ContractTypeId = UserEmploymentInformation.ContractTypeId;
                _dbContext.UserEmploymentInformation.Update(updateEmpInfo);
            }
            catch (Exception)
            {
                throw;
            }
        }
        protected async Task UpdateUserRole(DatabaseContext _dbContext, UserRole ExistinguserRole, string RoleId, string UserId, string ModifiedBy)
        {
            if (ExistinguserRole != null)
            {
                _dbContext.UserRoles.Remove(ExistinguserRole);
            }
            var newUserRole = new UserRole
            {
                UserId = UserId,
                RoleId = RoleId,
                CreatedDate = DateTime.Now,
                CreatedBy = ModifiedBy
            };
            _dbContext.UserRoles.Add(newUserRole);
            await _dbContext.SaveChangesAsync();
        }
        #endregion

        public async Task<EmployeeSuccessVM> AddEmployee(AddEmployeeVM user)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        await CreateUser(user, _dbContext);

                        transaction.Commit();
                        var employeeSuccessData = new EmployeeSuccessVM
                        {
                            //LoginId = user.ActiveDirectoryUserName,
                            userId = user.userPersonalInformation.UserId,
                            UserName = user.Email,
                            EmployeeTypeId = user.UserEmploymentInformation.EmployeeTypeId,
                            EmployeeId = user.UserEmploymentInformation.EmployeeId,
                        };
                        return employeeSuccessData;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        DeleteADUser(user.UserName);
                        throw;
                    }
                }
            }
        }
        private async Task<string> GenereateEmployeeId(DatabaseContext _dbContext)
        {
            string EmployeeId = "1";
            var InternlEmpIdList = await _dbContext.UserEmploymentInformation
                           .Where(x => x.EmployeeTypeId == (int)EmployeeTypeEnum.Internal)
                           .Select(x => x.EmployeeId)
                           .ToListAsync();

            //excluding the string Id from List so then we apply increament on EmpId which will be filter integer values, Incase someone insert with sting EmpId by Db
            List<int> FilterOutEmpIdList = InternlEmpIdList
                .Where(empId => int.TryParse(empId, out _))
                .Select(int.Parse)
                .ToList();

            int startingEmpId = 1;
            if (FilterOutEmpIdList.Exists(EmpId => EmpId == startingEmpId))
            {
                int maxNumber = FilterOutEmpIdList.Max();
                int newEmpid = maxNumber + 1;
                return EmployeeId = newEmpid.ToString();
            }
            else
            {
                return EmployeeId;
            }
        }
        //*******<History Author='AttiqueRehman' Date='31-jan-2024'>Auto Generate civild for BULK insertaion Emp</History>***//
        private async Task CreateUser(AddEmployeeVM user, DatabaseContext _dbContext)
        {
            try
            {
                var UmsUserId = await CreateUMSUser(user);
                if (UmsUserId != null)
                {
                    user.userPersonalInformation.UserId = UmsUserId;
                    await _dbContext.UserPersonalInformation.AddAsync(user.userPersonalInformation);
                    await _dbContext.SaveChangesAsync();

                    #region Employee Contact Information
                    //<History Author='Ammaar Naveed' Date='30-01-2024'>Adding User Contact Information In Database</History>//
                    if (user.UserContactInformationList.Count() > 0)
                    {
                        foreach (var contacts in user.UserContactInformationList)
                        {
                            var employeeContacts = new UserContactInformation
                            {
                                Id = contacts.Id,
                                UserId = UmsUserId,
                                ContactTypeId = contacts.ContactTypeId,
                                ContactNumber = contacts.ContactNumber,
                                IsActive = true,
                                CreatedBy = user.CreatedBy,
                                CreatedDate = DateTime.Now,
                                IsPrimary = contacts.IsPrimary,
                            };
                            await _dbContext.UserContactInformation.AddAsync(employeeContacts);
                        }
                        await _dbContext.SaveChangesAsync();
                    }
                    #endregion

                    if (user.UserAdresses.Count() > 0)
                    {
                        foreach (var ad in user.UserAdresses)
                        {
                            var add = new UserAdress
                            {
                                AddressId = Guid.NewGuid(),
                                Address = ad.Address,
                                UserId = UmsUserId,
                                CityId = ad.CityId,
                            };
                            await _dbContext.UserAdress.AddAsync(add);
                        }
                        await _dbContext.SaveChangesAsync();
                    }
                    // auto Generated Internal Employee Id
                    if (user.UserEmploymentInformation.EmployeeTypeId == (int)EmployeeTypeEnum.Internal && string.IsNullOrEmpty(user.UserEmploymentInformation.EmployeeId))
                    {
                        user.UserEmploymentInformation.EmployeeId = await GenereateEmployeeId(_dbContext);
                    }
                    user.UserEmploymentInformation.UserId = UmsUserId;
                    if (employeeStatusId == (int)EmployeeStatusEnum.InActive)
                        user.UserEmploymentInformation.EmployeeStatusId = employeeStatusId;
                    await _dbContext.UserEmploymentInformation.AddAsync(user.UserEmploymentInformation);
                    await _dbContext.SaveChangesAsync();
                    await InsertAttachmentsByUser(user, UmsUserId);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        int employeeStatusId = 1;
        private async Task<string> CreateUMSUser(AddEmployeeVM user)
        {
            try
            {
                IdentityUser identityUser = new IdentityUser
                {
                    Email = user.Email,
                    UserName = user.Email,
                };
                var createUser = await _userManager.CreateAsync(identityUser, "Fatwa1234!@#$");
                if (!createUser.Succeeded)
                {
                    throw new Exception(createUser.Errors.FirstOrDefault().Code, new Exception(createUser.Errors.FirstOrDefault().Description));
                }
                var userDetail = await _dbContext.Users.Where(x => x.Email == identityUser.Email).Select(x => x.Id).FirstOrDefaultAsync();
                User newuser = new User();
                newuser.Id = userDetail;
                newuser.CreatedBy = user.CreatedBy;
                newuser.CreatedDate = DateTime.Now;

                if (user.UserEmploymentInformation.DesignationId == (int)DesignationEnum.HeadOfSector)
                    await DeactivateMultipleHOSInSameSector(user, newuser);

                _dbContext.Users.Attach(newuser);
                _dbContext.Entry(newuser).Property(x => x.CreatedBy).IsModified = true;
                _dbContext.Entry(newuser).Property(x => x.CreatedDate).IsModified = true;
                _dbContext.Entry(newuser).Property(x => x.IsActive).IsModified = true;
                _dbContext.SaveChanges();
                return userDetail;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #region Deactivate Newly Created HOS In Same Sector
        //<History Author='Ammaar Naveed' Date='01-08-2024'>Handled on the basis of designation Id</History>//
        //<History Author='Ammaar Naveed' Date='15-01-2024'>Preventing HR to create multiple HOS. Deactivating newly created HOS</History>//
        private async Task DeactivateMultipleHOSInSameSector(AddEmployeeVM user, User newuser)
        {
            try
            {
                string StoreProc = $"exec pGetHOSBSectorId @sectorTypeId = '{user.UserEmploymentInformation.SectorTypeId}'";
                var sectorHosUsersWithRoles = await _dbContext.Users.FromSqlRaw(StoreProc).ToListAsync();
                if (sectorHosUsersWithRoles.Count() > 0)
                {
                    newuser.IsActive = false;
                    user.UserEmploymentInformation.EmployeeStatusId = (int)EmployeeStatusEnum.InActive;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        private async Task InsertAttachmentsByUser(AddEmployeeVM user, string UmsUserId)
        {
            try
            {
                var attachements = await _dmsdbContext.TempAttachements.Where(x => x.Guid == user.UserId).ToListAsync();
                foreach (var file in attachements)
                {
                    UploadedDocument documentObj = new UploadedDocument();
                    documentObj.Description = "User Profile Picture";
                    documentObj.CreatedDateTime = DateTime.Now;
                    documentObj.CreatedBy = user.CreatedBy;
                    documentObj.DocumentDate = DateTime.Now;
                    documentObj.FileName = file.FileName;
                    documentObj.StoragePath = file.StoragePath;
                    documentObj.DocType = file.DocType;
                    documentObj.ReferenceGuid = Guid.Parse(UmsUserId);
                    documentObj.CommunicationGuid = Guid.Empty;
                    documentObj.IsActive = true;
                    documentObj.CreatedAt = file.StoragePath;
                    documentObj.AttachmentTypeId = file.AttachmentTypeId;
                    documentObj.IsDeleted = false;
                    documentObj.IsMaskedAttachment = false;
                    await _dmsdbContext.UploadedDocuments.AddAsync(documentObj);
                    await _dmsdbContext.SaveChangesAsync();
                    await Task.Delay(200);
                    _dmsdbContext.TempAttachements.Remove(file);
                    await _dmsdbContext.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private async Task<bool> AssignGroupToUser(Guid GroupId, string UserId, AddEmployeeVM employeeVM)
        {
            try
            {
                UserGroup userGroup = new UserGroup();
                userGroup.GroupId = GroupId;
                userGroup.UserId = UserId;
                userGroup.CreatedDate = DateTime.Now;
                userGroup.CreatedBy = employeeVM.CreatedBy;
                userGroup.IsDeleted = false;
                _dbContext.UserGroups.Add(userGroup);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        private async Task<bool> AssignRoleToUser(string RoleId, string UserId, AddEmployeeVM employeeVM)
        {
            try
            {
                UserRole userRole = new UserRole();
                userRole.RoleId = RoleId;
                userRole.UserId = UserId;
                userRole.CreatedDate = DateTime.Now;
                userRole.CreatedBy = employeeVM.CreatedBy;
                userRole.IsDeleted = false;
                _dbContext.UserRoles.Add(userRole);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<bool> CreateADUser(AddEmployeeVM user, String groupName)
        {
            bool isSaved = false;
            try
            {
                var password = "Fatwa1234!@#$";
                var context = new PrincipalContext(ContextType.Domain, _adSettings.ServerIPAddress, _adSettings.UserCreationContainer, _adSettings.MachineAccountName, _adSettings.MachineAccountPassword);
                UserPrincipal up = new UserPrincipal(context);
                string FullName = user.userPersonalInformation.FirstName_En + " " + user.userPersonalInformation.LastName_En;
                up.GivenName = up.DisplayName = FullName;
                user.ActiveDirectoryUserName = up.SamAccountName = up.Name = GenerateUniqueSamAccountName(user.userPersonalInformation.FirstName_En, user.userPersonalInformation.LastName_En);
                up.EmailAddress = user.Email;
                up.Enabled = true;
                if (user.userPersonalInformation.SecondName_En != null)
                    up.MiddleName = user.userPersonalInformation.SecondName_En;
                up.Surname = user.userPersonalInformation.LastName_En;
                up.SetPassword(password);
                up.Save();
                isSaved = true;
                // user will not be the part of AD Group, code comment by AttiqueRehman 28/12/2023 
                //await AddUserToADGroup(user.UserName, groupName, context);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return isSaved;
        }

        //**************<History Author='AttiqueRehman' Date='29-12-2023'>Creating Unique Ad User Name</History>*******************//
        // Purpose: This code manages user accounts in Active Directory. It fetches existing users, determines the highest
        //          numeric postfix, and creates a new SamAccountName with an incremented postfix. The logic ensures uniqueness
        //          based on existing user SamAccountNames. The pattern is like "AR001," "AR002." If no suffix is found,
        //          it starts with "AB001" for a new user, and increments for subsequent users (e.g., "AB002").
        // remove some prefixes from lastname like al,bin, ibn
        protected string GenerateUniqueSamAccountName(string FirstName_En, string LastName_En)
        {
            try
            {
                // search the user from overall AD by not providing any Container OU
                var context = new PrincipalContext(ContextType.Domain, _adSettings.ServerIPAddress, _adSettings.Container, _adSettings.MachineAccountName, _adSettings.MachineAccountPassword);
                char firstCharFirstName = string.IsNullOrEmpty(FirstName_En) ? '\0' : FirstName_En[0];
                char firstCharLastName = !string.IsNullOrEmpty(LastName_En) ? LastName_En.Contains('-') ? LastName_En[LastName_En.IndexOf('-') + 1] : LastName_En[0] : '\0';
                string combinedLetters = $"{firstCharFirstName}{firstCharLastName}".ToUpper();
                int maxNumberSuffix = 0;
                //List<UserPrincipal> userList = new List<UserPrincipal>();//getting all ad users here
                using (PrincipalSearcher searcher = new PrincipalSearcher(new UserPrincipal(context)))
                {
                    foreach (UserPrincipal aduser in searcher.FindAll())
                    {
                        //userList.Add(aduser);
                        string samAccountName = aduser.SamAccountName;
                        if (samAccountName.StartsWith(combinedLetters))
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
        private async Task AddUserToADGroup(string userId, string groupName, PrincipalContext pc)
        {
            try
            {
                GroupPrincipal group = GroupPrincipal.FindByIdentity(pc, groupName);
                group.Members.Add(pc, IdentityType.SamAccountName, userId);
                group.Save();
            }
            catch (System.DirectoryServices.DirectoryServicesCOMException E)
            {
                //doSomething with E.Message.ToString(); 

            }
        }
        private void DeleteADUser(string userName)
        {
            var context = new PrincipalContext(ContextType.Domain, _adSettings.ServerIPAddress, _adSettings.Container, _adSettings.MachineAccountName, _adSettings.MachineAccountPassword);
            var userPrincipal = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, userName);
            if (userPrincipal != null)
            {
                userPrincipal.Delete();
            }
        }
        #endregion


        #region Deactivating An Employee (Database + Active Directory)
        //<History Author='Ammaar Naveed' Date='01-08-2024'>Handled missing employee status, updated samAccount parameter matching from email to ADUserName</History>//
        //<History Author='Ammaar Naveed' Date='09-06-2024'>Deactivating employee in database and AD then re-assiging tasks</History>//
        public async Task DeactivateEmployee(DeactivateEmployeesVM deactivatedEmployeeDetails, string EmployeeId, string loggedInUser)
        {
            try
            {
                var userIds = deactivatedEmployeeDetails.EmployeesList.Select(x => x.UserId).ToList();

                var userToDeactivate = await _dbContext.UserEmploymentInformation
                        .Where(x => userIds.Contains(x.UserId))
                        .FirstOrDefaultAsync();

                if (userToDeactivate != null)
                {
                    userToDeactivate.EmployeeStatusId = deactivatedEmployeeDetails.DeactivationReason;
                    userToDeactivate.ResignedTerminationdDate = deactivatedEmployeeDetails.StatusDate;
                    userToDeactivate.ResignationTerminationReason = deactivatedEmployeeDetails.StatusReason;

                    var umsUser = await _dbContext.Users
                        .Where(u => userIds.Contains(u.Id))
                        .FirstOrDefaultAsync();

                    if (umsUser != null)
                    {
                        //int atIndex = umsUser.Email.IndexOf('@');
                        //string samAccountName = umsUser.Email.Substring(0, atIndex);
                        string samAccountName = umsUser.ADUserName;
                        if (userToDeactivate.EmployeeStatusId == (int)EmployeeStatusEnum.Active)
                        {
                            umsUser.IsActive = true;
                            UpdateUserStatusInAD(samAccountName, true);
                        }
                        if (userToDeactivate.EmployeeStatusId == (int)EmployeeStatusEnum.InActive
                            || userToDeactivate.EmployeeStatusId == (int)EmployeeStatusEnum.Resigned
                            || userToDeactivate.EmployeeStatusId == (int)EmployeeStatusEnum.Terminated
                            || userToDeactivate.EmployeeStatusId == (int)EmployeeStatusEnum.Suspended
                            )
                        {
                            umsUser.IsActive = false;
                            UpdateUserStatusInAD(samAccountName, false);
                        }
                    }
                    //await DelegateDeactivatedEmployeeTasks(EmployeeId, deactivatedEmployeeDetails.UserId, loggedInUser);
                }

                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void UpdateUserStatusInAD(string samAccountName, bool IsUserActive)
        {
            try
            {
                var context = new PrincipalContext(ContextType.Domain, _adSettings.ServerIPAddress, _adSettings.Container, _adSettings.MachineAccountName, _adSettings.MachineAccountPassword);

                var userPrincipal = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, samAccountName);

                if (userPrincipal != null)
                {
                    userPrincipal.Enabled = IsUserActive;
                    userPrincipal.Save();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region Save Delegated User & Assign Tasks To Delegated Employee
        /*<History Author='Ammaar Naveed' Date='30-09-2024'>Save delegated employee and transfer tasks for deactivated employee</History>*/
        public async Task SaveDelegatedEmployeeForDeactivatedEmployee(EmployeeDelegationVM employeeDelegation)
        {
            try
            {
                var userEmploymentInformation = _dbContext.UserEmploymentInformation.FirstOrDefault(x => x.UserId == employeeDelegation.UserId);
                if (userEmploymentInformation != null)
                {
                    userEmploymentInformation.DelegatedUserId = employeeDelegation.DelegatedUserId;
                    userEmploymentInformation.DelegatedBy = employeeDelegation.LoggedInUsername;
                }
                await AssignTasksToDelegatedEmployee(employeeDelegation);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /*<History Author='Ammaar Naveed' Date='30-09-2024'>Save delegated employee and transfer tasks for deactivated employee</History>*/
        private async Task AssignTasksToDelegatedEmployee(EmployeeDelegationVM employeeDelegation)
        {
            var assignedTasks = _dbContext.Tasks.Where(x => x.AssignedTo == employeeDelegation.UserId &&
                                                        (x.TaskStatusId == (int)TaskStatusEnum.Pending ||
                                                         x.TaskStatusId == (int)TaskStatusEnum.InProgress)).ToList();
            foreach (var task in assignedTasks)
            {
                task.AssignedTo = employeeDelegation.DelegatedUserId;
                task.ModifiedBy = employeeDelegation.LoggedInUsername;
                task.ModifiedDate = DateTime.Now;
            }
            await _dbContext.SaveChangesAsync();
        }
        #endregion
        #endregion

        public async Task<List<string>> AddBulkEmployees(List<ImportEmployeeTemplate> employees, bool cultureEn)
        {
            List<string> errorList = new List<string>();
            using (_dbContext)
            {
                foreach (var employee in employees)
                {
                    List<string> errormessges = new List<string>();
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            IdentityUser identityUser = new IdentityUser
                            {
                                Email = employee.Email,
                                UserName = employee.Email,
                            };
                            var createUser = await _userManager.CreateAsync(identityUser, "Fatwa1234!@#$");
                            if (!createUser.Succeeded)
                            {
                                throw new Exception(createUser.Errors.FirstOrDefault().Code + "/Email");
                            }
                            else
                            {
                                var EmployeeType = int.Parse(employee.LKP_EmployeeType.Split(',')[0]);
                                var UmsUserId = await _dbContext.Users.Where(x => x.Email == identityUser.Email).Select(x => x.Id).FirstOrDefaultAsync();
                                User newuser = new User();
                                newuser.AllowAccess = true;
                                newuser.Id = UmsUserId;
                                newuser.CreatedBy = employee.CreatedBy;
                                newuser.CreatedDate = DateTime.Now;
                                newuser.ADUserName = employee.AD_UserName;
                                _dbContext.Users.Attach(newuser);
                                _dbContext.Entry(newuser).Property(x => x.CreatedBy).IsModified = true;
                                _dbContext.Entry(newuser).Property(x => x.CreatedDate).IsModified = true;
                                _dbContext.Entry(newuser).Property(x => x.ADUserName).IsModified = true;
                                _dbContext.Entry(newuser).Property(x => x.AllowAccess).IsModified = true;
                                _dbContext.SaveChanges();

                                if (UmsUserId != null)
                                {
                                    await AddPersonalInfo(UmsUserId, employee, errormessges, _dbContext);
                                    await AddEmploymentInformation(UmsUserId, employee, errormessges, _dbContext);
                                    //await AddEmployeeIntoGroup(newuser, employee.LKP_Group, _dbContext);
                                    if (EmployeeType == (int)EmployeeTypeEnum.Internal)
                                    {
                                        if (employee.LKP_Role != "")
                                            await AssignRoleToEmployee(newuser, employee.LKP_Role, _dbContext);
                                        else
                                            errormessges.Add("Role");
                                    }
                                }
                            }
                            if (!errormessges.IsNullOrEmpty())
                                throw new Exception(string.Join(",", errormessges) + " is Required.");
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            var email = employee.Email != null ? employee.Email : employee.FirstName_En + " " + employee.LastName_En;
                            string errorMessage = "Insertion failed for " + email + ", Reason: ";
                            if (ex.InnerException == null)
                                errorMessage += ex.Message + "\n";
                            if (ex.InnerException != null && ex.InnerException.Message.Contains("Violation of UNIQUE KEY "))
                                errorMessage += "EmployeeId must be unique" + "\n";
                            errorList.Add(errorMessage);
                        }
                    }
                }
            }
            return errorList;
        }
        private async Task AddPersonalInfo(string UserId, ImportEmployeeTemplate employee, List<string> errormessges, DatabaseContext _dbContext)
        {
            try
            {
                var userPersonalInformation = _mapper.Map<UserPersonalInformation>(employee);
                if (String.IsNullOrEmpty(userPersonalInformation.FirstName_En))
                    errormessges.Add("FirstName_En");
                if (String.IsNullOrEmpty(userPersonalInformation.FirstName_Ar))
                    errormessges.Add("FirstName_Ar");
                if (String.IsNullOrEmpty(employee.LKP_Gender))
                    errormessges.Add("Gender");
                if (String.IsNullOrEmpty(employee.LKP_Nationality))
                    errormessges.Add("Nationality");

                if (errormessges.IsNullOrEmpty())
                {
                    userPersonalInformation.UserId = UserId;
                    userPersonalInformation.CivilId = await GenerateCivilId(_dbContext);
                    userPersonalInformation.GenderId = int.Parse(employee.LKP_Gender.Split(',')[0]);
                    userPersonalInformation.DateOfBirth = new DateTime(1995, 01, 01);
                    userPersonalInformation.NationalityId = int.Parse(employee.LKP_Nationality.Split(',')[0]);
                    await _dbContext.UserPersonalInformation.AddAsync(userPersonalInformation);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
        private async Task AddEmploymentInformation(string UserId, ImportEmployeeTemplate employee, List<string> errormessges, DatabaseContext _dbContext)
        {
            try
            {
                var userEmploymentInformation = _mapper.Map<UserEmploymentInformation>(employee);
                if (String.IsNullOrEmpty(employee.LKP_Grade))
                    errormessges.Add("Grade");
                if (String.IsNullOrEmpty(employee.LKP_Designation))
                    errormessges.Add("Designation");
                if (String.IsNullOrEmpty(employee.LKP_SectorType))
                    errormessges.Add("SectorType");
                if (String.IsNullOrEmpty(employee.LKP_EmployeeType))
                    errormessges.Add("EmployeeType");
                var EmployeeType = int.Parse(employee.LKP_EmployeeType.Split(',')[0]);
                if (EmployeeType == (int)EmployeeTypeEnum.External && String.IsNullOrEmpty(employee.LKP_Company))
                    errormessges.Add("For External Employee Company");
                if (EmployeeType == (int)EmployeeTypeEnum.External && String.IsNullOrEmpty(employee.EmployeeId))
                    errormessges.Add("For External EmployeeId");

                if (errormessges.IsNullOrEmpty())
                {
                    userEmploymentInformation.UserId = UserId;
                    userEmploymentInformation.EmployeeTypeId = int.Parse(employee.LKP_EmployeeType.Split(',')[0]);
                    if (userEmploymentInformation.EmployeeTypeId == (int)EmployeeTypeEnum.Internal)
                        userEmploymentInformation.EmployeeId = await GenereateEmployeeId(_dbContext);
                    else
                        userEmploymentInformation.EmployeeId = employee.EmployeeId;
                    userEmploymentInformation.GradeId = int.Parse(employee.LKP_Grade.Split(',')[0]);
                    userEmploymentInformation.DesignationId = int.Parse(employee.LKP_Designation.Split(',')[0]);
                    userEmploymentInformation.SectorTypeId = int.Parse(employee.LKP_SectorType.Split(',')[0]);
                    userEmploymentInformation.DateOfJoining = new DateTime(2020, 01, 01);
                    if (userEmploymentInformation.EmployeeTypeId == (int)EmployeeTypeEnum.External)
                        userEmploymentInformation.CompanyId = int.Parse(employee.LKP_Company.Split(',')[0]);
                    //userEmploymentInformation.WorkingTimeId = int.Parse(employee.LKP_WorkingTime.Split(',')[0]); employee.LKP_SectorType.Split(',')[0]
                    userEmploymentInformation.ContractTypeId = 1;
                    userEmploymentInformation.WorkingTimeId = 2;
                    userEmploymentInformation.EmployeeStatusId = 1;
                    await _dbContext.UserEmploymentInformation.AddAsync(userEmploymentInformation);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
        private async Task AddEmployeeIntoGroup(User user, string GroupName, DatabaseContext _dbContext)
        {
            try
            {
                var GroupId = await _dbContext.Groups.Where(x => x.Name_En == GroupName).Select(x => x.GroupId).FirstOrDefaultAsync();
                UserGroup userGroup = new UserGroup();
                userGroup.GroupId = GroupId;
                userGroup.UserId = user.Id;
                userGroup.CreatedBy = user.CreatedBy;
                userGroup.CreatedDate = user.CreatedDate;
                await _dbContext.UserGroups.AddAsync(userGroup);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw;
            }
        }
        private async Task AssignRoleToEmployee(User user, string RoleName, DatabaseContext _dbContext)
        {
            try
            {
                RoleName = RoleName.Split(',')[1];
                var RoleId = await _dbContext.Roles.Where(x => x.NameAr == RoleName).Select(x => x.Id).FirstOrDefaultAsync();
                UserRole userRole = new UserRole();
                userRole.RoleId = RoleId;
                userRole.UserId = user.Id;
                userRole.CreatedBy = user.CreatedBy;
                userRole.CreatedDate = user.CreatedDate;
                await _dbContext.UserRoles.AddAsync(userRole);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw;
            }
        }
        //*******<History Author='AttiqueRehman' Date='31-jan-2024'>Auto Generate civild for BULK insertaion Emp</History>***//
        private async Task<string> GenerateCivilId(DatabaseContext _dbContext)
        {
            var civilIdList = await _dbContext.UserPersonalInformation.Select(x => x.CivilId).ToListAsync();
            List<double> filterlist = civilIdList.Where(x => double.TryParse(x, out _)).Select(double.Parse).ToList();
            double startingCivilIdId = 111111111119;
            if (filterlist.Exists(x => x == startingCivilIdId))
            {
                double maxNumber = filterlist.Max();
                double newCivilId = maxNumber + 1;
                return newCivilId.ToString();
            }
            else
            {
                return startingCivilIdId.ToString();
            }
        }
        //<History Author='Ammaar Naveed' Date='26-01-2024'>Getting contact types from lookup table.</History>//
        public async Task<List<ContactType>> GetContactTypes()
        {
            try
            {
                return await _dbContext.EmployeeContactTypes.ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //<History Author='Ammaar Naveed' Date='18-03-2024'>Store User Logout Activity</History>//  
        public async Task RecordUserLogoutActivity(string username, string userId)
        {
            try
            {
                var userActivityId = await _dbContext.UserActivities
                    .Where(u => u.UserId == userId)
                    .OrderByDescending(a => a.ActivityId)
                    .Select(a => a.ActivityId)
                    .FirstOrDefaultAsync();

                var userActivity = await _dbContext.UserActivities.FindAsync(userActivityId);

                userActivity.IsLoggedOut = true;
                userActivity.LogoutDateTime = DateTime.Now;
                userActivity.IPAddress = GetIPAddress();
                userActivity.ComputerName = Environment.MachineName;
                userActivity.UserName = username;

                _dbContext.UserActivities.Update(userActivity);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //<History Author='Ammaar Naveed' Date='02-10-2024'>Created SP to return records for both tempoorary and permanent deactivations</History>//  
        //<History Author='Ammaar Naveed' Date='01-05-2024'>Get employee leave delegation information.</History>//  
        private List<EmployeeDelegationHistoryVM> EmployeeDelegationRecords;
        public async Task<List<EmployeeDelegationHistoryVM>> GetEmployeeDelegationsInformation(string userId)
        {
            try
            {
                if (userId != null)
                {
                    string StoreProc = $"exec pGetEmployeeDelegationHistory @UserId='{userId}'";
                    EmployeeDelegationRecords = await _dbContext.EmployeeDelegationHistoryVM.FromSqlRaw(StoreProc).AsNoTracking().ToListAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return EmployeeDelegationRecords;
        }
        //<History Author='Ammaar Naveed' Date='29-07-2024'>Add and update user role</History>//  
        public async Task AddEditEmployeeRole(UserRoleAssignmentVM userRoleAssignmentVM)
        {
            try
            {
                if (userRoleAssignmentVM.SelectedUserId != null)
                {
                    var existingUserRole = await _dbContext.UserRoles.FirstOrDefaultAsync(x => x.UserId == userRoleAssignmentVM.SelectedUserId);

                    if (existingUserRole != null)
                    {
                        _dbContext.UserRoles.Remove(existingUserRole);
                    }
                    var newUserRole = new UserRole
                    {
                        UserId = userRoleAssignmentVM.SelectedUserId,
                        RoleId = userRoleAssignmentVM.SelectedRoleId,
                        CreatedDate = DateTime.Now,
                        CreatedBy = userRoleAssignmentVM.CreatedBy,
                    };
                    _dbContext.UserRoles.Add(newUserRole);
                    await _dbContext.SaveChangesAsync();
                }

                else if (userRoleAssignmentVM.SelectedUsersIdsList.Count > 0)
                {
                    var userIds = userRoleAssignmentVM.SelectedUsersIdsList.Select(userId => userId.ToString()).ToList();
                    var existingUserRoles = await _dbContext.UserRoles
                                                            .Where(x => userIds.Contains(x.UserId))
                                                            .ToListAsync();

                    _dbContext.UserRoles.RemoveRange(existingUserRoles);

                    foreach (var userId in userIds)
                    {
                        var newUserRole = new UserRole
                        {
                            UserId = userId,
                            RoleId = userRoleAssignmentVM.SelectedRoleId,
                            CreatedDate = DateTime.Now,
                            CreatedBy = userRoleAssignmentVM.CreatedBy,
                        };
                        _dbContext.UserRoles.Add(newUserRole);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        #region Save UMS User Clais
        //<History Author='Ammaar Naveed' Date='08-08-2024'>Save user claims</History>//  
        public async Task AllowBulkDigitalSign(EmployeeVMForDropDown data)
        {
            try
            {
                var usersToUpdate = await _dbContext.Users.Where(u => data.UserIds.Keys.ToList().Contains(u.Id)).ToListAsync();
                foreach (var user in usersToUpdate)
                {
                    if (data.UserIds.TryGetValue(user.Id, out bool allowDigitalSign))
                    {
                        user.AllowDigitalSign = allowDigitalSign;
                    }
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task SaveUserClaims(UserClaimsVM userClaimsVM)
        {
            if (userClaimsVM.SelectedUserId == null)
                await SaveClaimsOfBulkUsers(userClaimsVM);
            else
                await SaveClaimsOfIndividualUser(userClaimsVM);
        }

        #region Assign Claims To Bulk Users
        //<History Author='Ammaar Naveed' Date='18-08-2024'>Save user claims of bulk users</History>//  
        private async Task SaveClaimsOfBulkUsers(UserClaimsVM userClaimsVM)
        {
            try
            {
                foreach (var userId in userClaimsVM.SelectedUserIds)
                {
                    var existingUserClaims = _dbContext.UmsUserClaims.Where(c => c.UserId == userId).ToList();

                    if (existingUserClaims.Any())
                    {
                        _dbContext.UmsUserClaims.RemoveRange(existingUserClaims);
                    }

                    foreach (var claimValue in userClaimsVM.SelectedUserClaims)
                    {
                        var UmsUserClaim = new UserClaims
                        {
                            UserId = userId,
                            ClaimType = "Permission",
                            ClaimValue = claimValue,
                            ModuleId = userClaimsVM.ModuleId,
                            CreatedBy = userClaimsVM.CreatedBy,
                            CreatedDate = DateTime.Now
                        };
                        _dbContext.UmsUserClaims.Add(UmsUserClaim);
                    }
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Assign Claims To Individual User
        //<History Author='Ammaar Naveed' Date='08-08-2024'>Save user claims against individual user</History>//  
        private async Task SaveClaimsOfIndividualUser(UserClaimsVM userClaimsVM)
        {
            try
            {
                var existingUserClaims = _dbContext.UmsUserClaims.Where(c => c.UserId == userClaimsVM.SelectedUserId).ToList();

                if (existingUserClaims.Any())
                {
                    _dbContext.UmsUserClaims.RemoveRange(existingUserClaims);
                }

                foreach (var claimValue in userClaimsVM.SelectedUserClaims)
                {
                    var UmsUserClaim = new UserClaims
                    {
                        UserId = userClaimsVM.SelectedUserId,
                        ClaimType = "Permission",
                        ClaimValue = claimValue,
                        ModuleId = userClaimsVM.ModuleId,
                        CreatedBy = userClaimsVM.CreatedBy,
                        CreatedDate = DateTime.Now
                    };
                    _dbContext.UmsUserClaims.Add(UmsUserClaim);
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        #endregion

        #region Get IP Address
        private string GetIPAddress()
        {
            try
            {
                string hostName = Dns.GetHostName();
                IPAddress[] localIPs = Dns.GetHostAddresses(hostName);

                foreach (IPAddress ip in localIPs)
                {
                    if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                return "Unknown";
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get Users List By Sector Id 
        public async Task<List<CommitteeUserDataVM>> GetCommitteeUsersListBySectorId(int SectorTypeId)
        {
            try
            {
                var StoreProc = $"exec pGetCommitteeUsersListBySectorId @SectorId= '{SectorTypeId}'";
                var result = await _dbContext.CommitteeUserDataVMs.FromSqlRaw(StoreProc).ToListAsync();
                if (result != null)
                {
                    return result;
                }
                else
                {
                    return new List<CommitteeUserDataVM>();
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
        #endregion

        #region Update Default Correspondence Receiver Status
        //<History Author='Ammaar Naveed' Date='30-07-2024'>Update default correspondence receiver status.</History>//  
        public async Task UpdateDefaultReceiverStatus(bool isDefaultCorrespondenceReceiver, string userId)
        {
            try
            {
                var userEmploymentInfo = await _dbContext.UserEmploymentInformation
                    .Where(u => u.UserId == userId).FirstOrDefaultAsync();

                if (userEmploymentInfo != null)
                {
                    userEmploymentInfo.IsDefaultCorrespondenceReceiver = isDefaultCorrespondenceReceiver;

                    _dbContext.UserEmploymentInformation.Update(userEmploymentInfo);
                    await _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Get User Working Hours 
        public async Task<WorkingHour> GetEmployeeWorkingHours(string userId)
        {
            try
            {
                var StoredProc = $"exec pGetEmployeeTypeByEmpId @empId ='{userId}'";
                var result = await _dbContext.WorkingHours.FromSqlRaw(StoredProc).ToListAsync();
                var workingHours = result.FirstOrDefault();
                return workingHours;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion

        /// <summary>
        /// This method will return manager Id based on role (Manager) and sector
        ///     Assuming that ther is only one manager for that sector
        /// </summary>
        /// <param name="SectorTypeId"></param>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<string?> GetManagerByRoleAndSector(int SectorTypeId, string RoleId)
        {
            try
            {
                string StoreProc = $"exec pGetUsersByRoleIdandSectorId @sectorTypeId={SectorTypeId}, @RoleId='{RoleId}'";
                var manager = await _dbContext.DeactivateEmployeesVM.FromSqlRaw(StoreProc).ToListAsync();
                return manager.Count > 0 ? manager.FirstOrDefault().UserId : null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// <summary>
        /// This method will return the manager id of user from EP_EMPLOYMENT_INFORMATION table
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>

        public async Task<string?> GetManagerByuserId(string userId)
        {
            try
            {
                var response = await _dbContext.UserEmploymentInformation.Where(x => x.UserId == userId).FirstOrDefaultAsync();
                if (response is null) return null;
                return response.ManagerId;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #region Get Active Users By SectorTypeId

        public async Task<List<UserBasicDetailVM>> GetActiveUsersBySectorTypeId(int? sectorTypeId)
        {
            try
            {
                string storeProc = $"exec pGetActiveEmployees @sectorTypeId = '{sectorTypeId}'";
                var result = await _dbContext.UserBasicDetailVMs.FromSqlRaw(storeProc).ToListAsync();
                return result.Count > 0 ? result : null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        #endregion

        #region Get Users By RoleId and SectorId
        public async Task<List<UserDataVM>> GetUsersByRoleIdandSectorId(string RoleId, int SectorTypeId)
        {
            try
            {
                List<UserDataVM> usersData = new List<UserDataVM>();
                string StoreProc = $"exec pGetUserListByRoleandSector @RoleId='{RoleId}',@SectorTypeId = '{SectorTypeId}'";
                usersData = await _dbContext.UserDataVM.FromSqlRaw(StoreProc).ToListAsync();
                return usersData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        #region Get Groups By RoleId and SectorId
        public async Task<List<UserGroupVM>> GetGroupsByRoleIdandSectorId(string RoleId, int SectorTypeId)
        {
            try
            {
                List<UserGroupVM> groupsData = new List<UserGroupVM>();
                string StoreProc = $"exec GetGroupsBySectorAndRole @RoleId='{RoleId}',@SectorTypeId = '{SectorTypeId}'";
                groupsData = await _dbContext.UserGroupVMs.FromSqlRaw(StoreProc).ToListAsync();
                return groupsData;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
        #region
        public async Task<List<UserListMentionVM>> GetUsersListForMention(Guid TicketId, string LoggedInUserEmail)
        {
            try
            {
                string storProc = $"exec pGetUsersListForMention @TicketId ='{TicketId}' ,@LoggedInUserEmail ='{LoggedInUserEmail}'";
                return await _dbContext.UserListMentionVMs.FromSqlRaw(storProc).ToListAsync() ?? new List<UserListMentionVM>();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        #endregion
    }
}

