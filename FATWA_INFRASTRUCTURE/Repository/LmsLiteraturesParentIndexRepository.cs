using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using FATWA_INFRASTRUCTURE.Database;
using FATWA_INFRASTRUCTURE.Repository.CaseManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace FATWA_INFRASTRUCTURE.Repository
{
    //<!-- <History Author = 'Umer Zaman' Date='2022-07-06' Version="1.0" Branch="master">implementation of dewery decimal parent classs ystem</History> -->
    public class LmsLiteraturesParentIndexRepository : ILmsLiteratureParentIndex
    {
        #region Variables declared
        private readonly DatabaseContext _dbContext;
        private List<LmsLiteratureParentIndex> _LmsLiteratureParentIndex;
        private LmsLiteratureParentIndex _LmsLiteratureParentIndexDetail;

        private readonly IServiceScopeFactory _serviceScopeFactory;
        #endregion

        public LmsLiteraturesParentIndexRepository(DatabaseContext dbContext, IServiceScopeFactory serviceScopeFactory)
        {
            _dbContext = dbContext;
            _serviceScopeFactory = serviceScopeFactory; 
        }

        #region Get all parent indexes details, ById
        public async Task<List<LmsLiteratureParentIndex>> GetLmsLiteratureParentIndexs()
        {
            if (_LmsLiteratureParentIndex == null)
            {
                _LmsLiteratureParentIndex = await _dbContext.LmsLiteratureParentIndexs.OrderByDescending(u => u.ParentIndexId).Where(x => x.IsDeleted == false).ToListAsync();
            }

            return _LmsLiteratureParentIndex;
        }

        public List<LmsLiteratureParentIndex> GetLmsLiteratureParentIndexSync()
        {
            if (_LmsLiteratureParentIndex == null)
            {
                _LmsLiteratureParentIndex = _dbContext.LmsLiteratureParentIndexs.ToList();
            }

            return _LmsLiteratureParentIndex;
        }
        public async Task<LmsLiteratureParentIndex> GetLiteratureParentIndexDetailById(int parentIndexId)
        {
            try
            {
                LmsLiteratureParentIndex? task = await _dbContext.LmsLiteratureParentIndexs.FindAsync(parentIndexId);
                if (task != null)
                {
                    return task;
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch
            {
                throw;
            }
        }
        public bool CheckLiteratureParentIndexByUsingParentIndexNumber(string parentIndexNumber, string name_En, string name_Ar)
        {
            try
            {
                LmsLiteratureParentIndex? task = _dbContext.LmsLiteratureParentIndexs.FirstOrDefault(x => x.ParentIndexNumber == parentIndexNumber && x.Name_En == name_En && x.Name_Ar == name_Ar);
                if (task != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
        }
        public bool CheckLiteratureParentIndexByUsingParentNumber(string parentIndexNumber)
        {
            try
            {
                LmsLiteratureParentIndex? task = _dbContext.LmsLiteratureParentIndexs.FirstOrDefault(x => x.ParentIndexNumber == parentIndexNumber);
                if (task != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw;
            }
        }
        public async Task<LmsLiteratureParentIndex> GetLmsLiteratureParentIndexDetailByNumber(string parentIndexNumber)
        {
            try
            {
                var result = await _dbContext.LmsLiteratureParentIndexs.Where(u => u.ParentIndexNumber == parentIndexNumber).FirstOrDefaultAsync();
                if (result != null)
                {
                    return result;
                }
                return null;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Create parent index
        public async Task CreateLmsLiteratureParentIndex(LmsLiteratureParentIndex lmsLiteratureParentIndex)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            _dbContext.LmsLiteratureParentIndexs.Add(lmsLiteratureParentIndex);
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

                throw new Exception(ex.Message);
            }

        }
        #endregion

        #region Update parent index

        public async Task UpdateLmsLiteratureParentIndex(LmsLiteratureParentIndex lmsLiteratureParentIndex)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            _dbContext.Entry(lmsLiteratureParentIndex).State = EntityState.Modified;
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

                throw new Exception(ex.Message);
            }
        }
        #endregion

        #region Delete parent index (soft delete)
        public async Task SoftDeleteLiteratureParentIndex(LmsLiteratureParentIndex UniqueLiteratureParentIndex)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            if (UniqueLiteratureParentIndex != null)
                            {
                                _dbContext.Entry(UniqueLiteratureParentIndex).State = EntityState.Modified;
                                await _dbContext.SaveChangesAsync();
                                await DeleteParentIndexChildList(UniqueLiteratureParentIndex.ParentIndexId, UniqueLiteratureParentIndex.ParentIndexNumber, UniqueLiteratureParentIndex.DeletedBy, _dbContext);
                                UniqueLiteratureParentIndex.NotificationParameter.Entity = new LmsLiteratureParentIndex().GetType().Name;  
                                transaction.Commit();
                            }
                        }
                        catch (Exception)
                        {

                            transaction.Rollback();
                        }
                    }
                }
            }
            catch
            {
                _dbContext.Entry(UniqueLiteratureParentIndex).State = EntityState.Unchanged;
                throw;
            }

        }

        private async Task DeleteParentIndexChildList(int parentIndexId, string parentIndexNumber, string deletedBy, DatabaseContext dbContext)
        {
            try
            {
                var IndexChildListResult = await dbContext.LmsLiteraturesIndex.Where(x => x.ParentId == parentIndexId && x.IndexParentNumber == parentIndexNumber && x.IsDeleted == false).ToListAsync();
                if (IndexChildListResult.Count > 0)
                {
                    foreach (var item in IndexChildListResult)
                    {
                        item.DeletedBy = deletedBy;
                        item.DeletedDate = DateTime.Now;
                        item.IsDeleted = true;
                        dbContext.Entry(item).State = EntityState.Modified;
                        await dbContext.SaveChangesAsync();
                        await DeleteParentIndexDivisionList(item.IndexId, item.DeletedBy, _dbContext);
                        await DeleteParentIndexChildList(item.IndexId, item.IndexNumber, item.DeletedBy, dbContext);
                    }
                }
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        private async Task DeleteParentIndexDivisionList(int indexId, string deletedBy, DatabaseContext dbContext)
        {
            try
            {
                var IndexDivisionListResult = await dbContext.LmsLiteratureIndexDivisionAisles.Where(x => x.IndexId == indexId && x.IsDeleted == false).ToListAsync();
                if (IndexDivisionListResult.Count > 0)
                {
                    foreach (var item in IndexDivisionListResult)
                    {
                        item.DeletedBy = deletedBy;
                        item.DeletedDate = DateTime.Now;
                        item.IsDeleted = true;
                        dbContext.Entry(item).State = EntityState.Modified;
                        await dbContext.SaveChangesAsync();
                    }
                }
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        public async Task SoftDeleteLmsLiteratureParentIndex(LmsLiteratureParentIndex lmsLiteratureParentIndex)
        {
            using (_dbContext)
            {
                using (var transaction = _dbContext.Database.BeginTransaction())
                {
                    try
                    {
                        _dbContext.Entry(lmsLiteratureParentIndex).State = EntityState.Modified;
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

        #endregion
        public async Task<string> GetUserIdByName(string Email)
        {

            using var scope = _serviceScopeFactory.CreateScope();
            var _DbContext = scope.ServiceProvider.GetRequiredService<DatabaseContext>();
            var Id = _DbContext.Users.Where(x => x.Email == Email).Select(y => y.Id).FirstOrDefault();
            return Id;
        }
    }
}
