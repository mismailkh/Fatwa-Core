using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.EntityFrameworkCore;

namespace FATWA_INFRASTRUCTURE.Repository
{
    //<History Author = 'Umer Zaman' Date='2022-07-26' Version="1.0" Branch="master"> Repository for managing transfering user functionality</History>
    public class TransferUserRepository : ITransferUser
    {
        #region Constructor
        public TransferUserRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region Variable Declaration
        private readonly DatabaseContext _dbContext;
        private List<Department> _departmentDetails;
        #endregion

        //<History Author = 'Umer Zaman' Date='2022-07-27' Version="1.0" Branch="master">Get all department details </History>
        #region Get All Department List
        public async Task<List<Department>> GetAllDepartmentList()
        {
            if (_departmentDetails == null)
            {
                _departmentDetails = await _dbContext.Departments.OrderByDescending(u => u.Id).ToListAsync();
            }
            return _departmentDetails;
        }
        #endregion

        #region Save Transfer User

        //<History Author = 'Umer Zaman' Date='2022-07-28' Version="1.0" Branch="master"> Save transfer user details</History>
        public async Task SaveTransferUser(TransferUser transferUser)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            _dbContext.TransferUsers.Add(transferUser);
                            await _dbContext.SaveChangesAsync();
                           // await UpdateUserDepartmentIdDetail(transferUser.Id, transferUser.Previous_DepartmentId, transferUser.Current_DepartmentId, _dbContext);
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

        //private async Task UpdateUserDepartmentIdDetail(string id, int previous_DepartmentId, int current_DepartmentId, DatabaseContext dbContext)
        //{
        //    try
        //    {
        //        UserEmploymentInformation? user = dbContext.UserEmploymentInformation.Where(x => x.UserId == id).FirstOrDefault();
        //        if (user != null)
        //        {
        //            if (user.DepartmentId == previous_DepartmentId)
        //            {
        //                user.DepartmentId = current_DepartmentId;
        //                dbContext.Entry(user).State = EntityState.Modified;
        //                await _dbContext.SaveChangesAsync();
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        throw new Exception(ex.Message);
        //    }
        //}


        #endregion
    }
}
