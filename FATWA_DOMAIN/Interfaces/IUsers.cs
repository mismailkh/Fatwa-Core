using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_DOMAIN.Models.CaseManagment;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.UserManagement;

namespace FATWA_DOMAIN.Interfaces
{
    public interface IUsers
    {
        Task<List<UserListGroupVM>> GetUmsUser(string GroupId, bool IsView);
        Task<List<UserTransferVM>> GetUmsUserTransfer(string userId);
        Task<UserDetailViewListVM> GetUserDetailsById(string userId);
        Task<List<Nationality>> GetNationality();
        Task<bool> CheckCivilIdExists(string civilId);
        Task<List<Gender>> GetGenders();
        Task<List<UserAdress>> GetUserAdress();
        Task<List<City>> GetCities();
        Task<List<Country>> GetCountries();
        Task<IEnumerable<Designation>> GetDesignations();
        Task<List<EmployeeType>> GetEmployeeType();
        Task<List<Company>> GetCompanies();
        Task<List<Department>> GetEmployeeDepartment();
        Task<List<Department>> Department();
        Task<List<OperatingSectorType>> GetEmployeeSectortype();
        Task<List<Grade>> GetEmployeeGrade();
        Task<List<GradeType>> GetGradeTypes();
        Task<List<ContractType>> GetContractTypes();
        Task<List<EmployeeWorkingTime>> GetWorkingTime();
        Task<List<EmployeeStatus>> GetEmployeeStatus();
        Task<List<Governorate>> GetGovernorates();
        Task<AddEmployeeVM> GetEmployeeDetailById(Guid Id);
        Task EditEmployee(AddEmployeeVM updatedEmployee);
        Task<EmployeeSuccessVM> AddEmployee(AddEmployeeVM employeeVM);
        Task<List<string>> AddBulkEmployees(List<ImportEmployeeTemplate> employees, bool cultureEn);
        Task<List<EmployeeVM>> GetEmployeeList(UserListAdvanceSearchVM advanceSearchVM);
        Task<List<EmployeesListVM>> GetEmployeesListForAdmin(int EmployeeTypeId, int? SectorTypeId, int? DesignationId);
        Task<List<EmployeesListDropdownVM>> GetEmployeesListForUserGroup(UserListAdvanceSearchVM advanceSearchVM);
        Task<List<EmployeeVMForDropDown>> GetEmployeeList(int sectorTypeId, int attachementId, int documentId);
        Task<List<UserDataVM>> GetUserData();
        Task<List<EmployeeDelegationVM>> GetAlternateEmployeesList(int? SectorTypeId, string RoleId, DateTime? FromDate, DateTime? ToDate);
        Task DeactivateEmployee(DeactivateEmployeesVM deactivateEmployeesVMList, string EmployeeId, string loggedInUser);
        Task<List<ContactType>> GetContactTypes();
        Task RecordUserLogoutActivity(string username, string userId);
        Task<List<EmployeeDelegationHistoryVM>> GetEmployeeDelegationsInformation(string userId);
        Task SaveDelegatedEmployee(EmployeeLeaveDelegationInformation delegatedEmployeeInformation);
        Task SaveDelegatedEmployeeForDeactivatedEmployee(EmployeeDelegationVM employeeDelegation);
        Task<bool> CheckEmployeeLeaveStatus(string UserId, DateTime? FromDate, DateTime? ToDate);
        Task<List<ManagersListVM>> GetManagersList(int? SectorTypeId, int DesignationId);
        Task<IEnumerable<EmployeeDelegationVM>> GetEmployeesByRoleSectorAndDesignation(int SectorTypeId, string RoleId, int DesignationId);
        Task AddEditEmployeeRole(UserRoleAssignmentVM userRoleAssignmentVM);
        Task<List<CommitteeUserDataVM>> GetCommitteeUsersListBySectorId(int SectorTypeId);

        Task<WorkingHour> GetEmployeeWorkingHours(string userId);
        Task UpdateDefaultReceiverStatus(bool isDefaultCorrespondenceReceiver, string userId);

        /// <summary>
        ///     This method will return manager Id based on role (Manager) and sector
        ///     Assuming that ther is only one manager for that sector
        /// </summary>
        /// <param name="SectorTypeId"></param>
        /// <param name="RoleId"></param>
        /// <returns></returns>
        Task<string> GetManagerByRoleAndSector(int SectorTypeId, string RoleId);
        Task<List<UserClaimsVM>> GetEmployeesByDesignationId(int? DesignationId);
        Task<List<UserClaimsVM>> GetUmsClaimsByModuleId(int? moduleId);
        Task SaveUserClaims(UserClaimsVM userClaimsVM);
        Task AllowBulkDigitalSign(EmployeeVMForDropDown data);
        Task<string?> GetManagerByuserId(string userId);
        Task<List<UserBasicDetailVM>> GetActiveUsersBySectorTypeId(int? sectorTypeId);

        Task<List<UserDataVM>> GetUsersByRoleIdandSectorId(string RoleId, int SectorTypeId);
        Task<List<UserGroupVM>> GetGroupsByRoleIdandSectorId(string RoleId, int SectorTypeId);
        Task<List<UserDataVM>> GetUsersByBugTypeId(int TypeId);
        Task<List<UserListMentionVM>> GetUsersListForMention(Guid TicketId, string LoggedInUserEmail);
    }
}
