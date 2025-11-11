using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.ViewModel;
using FATWA_DOMAIN.Models.ViewModel.AdminVM.CaseManagment;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.EntityFrameworkCore;

namespace FATWA_INFRASTRUCTURE.Repository
{
    //<!-- <History Author = 'Umer Zaman' Date='2022-03-25' Version="1.0" Branch="master">Create class for manage index</History> -->

    //<!-- <History Author = 'Umer Zaman' Date='2022-07-06' Version="1.0" Branch="master">implementation of dewery decimal system, also modified the current index implementation</History> -->
    public class LmsLiteratureIndexRepository : ILmsLiteratureIndex
    {
        #region Variables declared
        private readonly DatabaseContext _dbContext;
        private List<LmsLiteratureIndex> _lmsLiteratureIndex;
        private LmsLiteratureIndex _literaturebyindexId;
        private LmsLiteratureIndex _lmsLiteratureIndexDetail;
        private LmsLiteratureParentIndexVM _lmsLiteratureVMDetails;
        #endregion

        public LmsLiteratureIndexRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Get all indexes details, ById
        public async Task<List<LmsLiteratureIndex>> GetLmsLiteratureIndexs()
        {
            if (_lmsLiteratureIndex == null)
            {
                _lmsLiteratureIndex = await _dbContext.LmsLiteraturesIndex.OrderByDescending(u => u.IndexId).Where(x => x.IsDeleted == false).ToListAsync();
            }

            return _lmsLiteratureIndex;
        }

        public List<LmsLiteratureIndex> GetLmsLiteratureIndexSync()
        {
            if (_lmsLiteratureIndex == null)
            {
                _lmsLiteratureIndex = _dbContext.LmsLiteraturesIndex.Where(x => x.IsDeleted == false).ToList();
            }

            return _lmsLiteratureIndex;
        }
        public async Task<LmsLiteratureIndex> GetLiteratureIndexDetail(int indexId)
        {
            try
            {
                LmsLiteratureIndex? task = await _dbContext.LmsLiteraturesIndex.FindAsync(indexId);
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
        public async Task<List<LmsLiteratureIndex>> GetLmsLiteratureIndexesIdByIndexNumber(string indexNumber)
        {
            try
            {
                if (_lmsLiteratureIndex == null)
                {
                    _lmsLiteratureIndex = await _dbContext.LmsLiteraturesIndex.OrderBy(u => u.IndexId).Where(u => u.IndexNumber == indexNumber).ToListAsync();
                }

                return _lmsLiteratureIndex;
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<LmsLiteratureIndex>> GetLmsLiteratureIndexDetailByUsingNameAndIndexNumber(string indexNumber, string name_en, string name_ar)
        {
            try
            {
                if (_lmsLiteratureIndex == null)
                {
                    _lmsLiteratureIndex = await _dbContext.LmsLiteraturesIndex.OrderBy(u => u.IndexId).Where(u => u.IndexNumber == indexNumber && u.Name_En == name_en && u.Name_Ar == name_ar).ToListAsync();
                }

                return _lmsLiteratureIndex;
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<LmsLiteratureIndex>> GetLmsLiteratureIndexDetilsByUsingParentIdAndNumber(int parentId, string parentIndexNumber)
        {
            try
            {
                List<LmsLiteratureIndex> _lmsLiteratureIndex = new List<LmsLiteratureIndex>();
                _lmsLiteratureIndex = await _dbContext.LmsLiteraturesIndex.OrderBy(u => u.IndexId).Where(u => u.ParentId == parentId && u.IndexParentNumber == parentIndexNumber && u.IsDeleted == false).ToListAsync();
                if (_lmsLiteratureIndex.Count() != 0)
                {
                    return _lmsLiteratureIndex;
                }
                return new List<LmsLiteratureIndex>();
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<LmsLiteratureIndexDivisionAisle>> GetLmsLiteratureIndexDivisions(int indexId)
        {
            try
            {
                var _lmsLiteratureIndexDivision = await _dbContext.LmsLiteratureIndexDivisionAisles.OrderByDescending(u => u.IndexId).Where(u => u.IndexId == indexId).ToListAsync();
                return _lmsLiteratureIndexDivision;
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<LmsLiteratureIndex>> GetLmsLiteratureIndexNumberDetailsForAddDivision(string indexNumber)
        {
            try
            {
                if (_lmsLiteratureIndex == null)
                {
                    _lmsLiteratureIndex = await _dbContext.LmsLiteraturesIndex.OrderByDescending(u => u.IndexId).Where(u => u.IndexNumber == indexNumber).ToListAsync();

                }
                return _lmsLiteratureIndex;
            }
            catch
            {
                throw;
            }
        }

        public async Task<LmsLiteratureIndex> GetLiteratureIndexDetailByUsingIndexId(int indexId)
        {
            try
            {
                if (_lmsLiteratureIndexDetail == null)
                {
                    _lmsLiteratureIndexDetail = await _dbContext.LmsLiteraturesIndex.FirstOrDefaultAsync(u => u.IndexId == indexId);
                }

                return _lmsLiteratureIndexDetail;
            }
            catch
            {
                throw;
            }
        }

        public List<LmsLiterature> CheckLmsLiteratureIndexIdAssociatedWithLiteratures(int indexId)
        {
            try
            {
                var _lmsLiteratureIndex = _dbContext.LmsLiteratures.Where(x => x.IndexId == indexId && x.IsDeleted == false).ToList();
                return _lmsLiteratureIndex;
            }
            catch
            {
                throw new Exception();
            }
        }
        //Author:Zaeem
        public async Task<LmsLiteratureParentIndexVM> GetLiteratureIndexByIndexIdAndNumber(int indexId, string indexNumber)
        {
            try
            {
                _lmsLiteratureVMDetails = new LmsLiteratureParentIndexVM();
                if (_literaturebyindexId == null)
                {
                    _literaturebyindexId = await _dbContext.LmsLiteraturesIndex.OrderBy(u => u.IndexId).Where(u => u.IndexId == indexId && u.IndexNumber == indexNumber && u.IsDeleted == false).FirstOrDefaultAsync();
                }
                if (_literaturebyindexId != null)
                {
                    _lmsLiteratureVMDetails.ParentIndexId = _literaturebyindexId.IndexId;
                    _lmsLiteratureVMDetails.IndexParentNumber = _literaturebyindexId.IndexNumber;
                    _lmsLiteratureVMDetails.Parent_Name_En = _literaturebyindexId.Name_En;
                    _lmsLiteratureVMDetails.Parent_Name_Ar = _literaturebyindexId.Name_Ar;
                }
                else
                {
                    var result = await _dbContext.LmsLiteratureParentIndexs.OrderBy(x => x.ParentIndexId).Where(x => x.ParentIndexId == indexId && x.ParentIndexNumber == indexNumber).FirstOrDefaultAsync();
                    if (result != null)
                    {
                        _lmsLiteratureVMDetails.ParentIndexId = result.ParentIndexId;
                        _lmsLiteratureVMDetails.IndexParentNumber = result.ParentIndexNumber;
                        _lmsLiteratureVMDetails.Parent_Name_En = result.Name_En;
                        _lmsLiteratureVMDetails.Parent_Name_Ar = result.Name_Ar;
                    }
                }
                return _lmsLiteratureVMDetails;
            }
            catch
            {
                throw;
            }
        }
        #endregion




        #region Create index
        public async Task CreateLmsLiteratureIndex(LmsLiteratureIndex lmsLiteratureIndex)
        {
            try
            {
                _dbContext.LmsLiteraturesIndex.Add(lmsLiteratureIndex);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {


                throw new Exception(ex.Message);
            }

        }
        #endregion

        #region Update index

        public async Task UpdateLmsLiteratureIndex(LmsLiteratureIndex lmsLiteratureIndex)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            _dbContext.Entry(lmsLiteratureIndex).State = EntityState.Modified;
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

        #region Delete Index (Soft Delete, all dependents child indexes and divisions)
        public async Task DeleteLmsLiteratureIndex(LmsLiteratureIndex UniqueLiteratureIndex)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            if (UniqueLiteratureIndex != null)
                            {
                                _dbContext.Entry(UniqueLiteratureIndex).State = EntityState.Modified;
                                await _dbContext.SaveChangesAsync();
                                await DeleteIndexChildList(UniqueLiteratureIndex, _dbContext);
                                await DeleteIndexDivisionList(UniqueLiteratureIndex, _dbContext);
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
                _dbContext.Entry(UniqueLiteratureIndex).State = EntityState.Unchanged;
                throw;
            }

        }

        private async Task DeleteIndexDivisionList(LmsLiteratureIndex uniqueLiteratureIndex, DatabaseContext dbContext)
        {
            try
            {
                var IndexDivisionListResult = await dbContext.LmsLiteratureIndexDivisionAisles.Where(x => x.IndexId == uniqueLiteratureIndex.IndexId).ToListAsync();
                if (IndexDivisionListResult.Count > 0)
                {
                    foreach (var item in IndexDivisionListResult)
                    {
                        item.DeletedBy = uniqueLiteratureIndex.DeletedBy;
                        item.DeletedDate = uniqueLiteratureIndex.DeletedDate;
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

        private async Task DeleteIndexChildList(LmsLiteratureIndex uniqueLiteratureIndex, DatabaseContext dbContext)
        {

            try
            {
                var IndexChildListResult = await dbContext.LmsLiteraturesIndex.Where(x => x.ParentId == uniqueLiteratureIndex.IndexId && x.IndexParentNumber == uniqueLiteratureIndex.IndexNumber && x.IsDeleted == false).ToListAsync();
                if (IndexChildListResult.Count > 0)
                {
                    foreach (var item in IndexChildListResult)
                    {
                        item.DeletedBy = uniqueLiteratureIndex.DeletedBy;
                        item.DeletedDate = uniqueLiteratureIndex.DeletedDate;
                        item.IsDeleted = true;
                        dbContext.Entry(item).State = EntityState.Modified;
                        await dbContext.SaveChangesAsync();
                        await DeleteIndexChildList(item, dbContext);
                    }
                }
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }
        #endregion
    }
}
