using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.EntityFrameworkCore;

namespace FATWA_INFRASTRUCTURE.Repository
{
    //<!-- <History Author = 'Umer Zaman' Date='2022-07-08' Version="1.0" Branch="master">Division Aisle functionality implementation using dewery decimal system</History> -->
    public class LmsLiteraturesIndexDivisionAisleRepository : ILmsLiteratureIndexDivisionAisle
    {
        #region Variables declared
        private readonly DatabaseContext _dbContext;
        private List<LmsLiteratureIndexDivisionAisle> _lmsLiteratureIndexDivisionAisle;
        private LmsLiteratureIndexDivisionAisle _lmsLiteratureIndexDivisionAisleDetail;
        private List<LmsLiterature> _lmsLiteratureResult;
        #endregion

        public LmsLiteraturesIndexDivisionAisleRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        #region Get all division, aisle details & ById
        public async Task<List<LmsLiteratureIndexDivisionAisle>> GetLmsLiteratureIndexDivisions()
        {
            if (_lmsLiteratureIndexDivisionAisle == null)
            {
                _lmsLiteratureIndexDivisionAisle = await _dbContext.LmsLiteratureIndexDivisionAisles.OrderByDescending(u => u.DivisionAisleId).Where(x => x.IsDeleted == false).ToListAsync();
            }

            return _lmsLiteratureIndexDivisionAisle;
        }

        public List<LmsLiteratureIndexDivisionAisle> GetLmsLiteratureIndexDivisionsSync()
        {
            if (_lmsLiteratureIndexDivisionAisle == null)
            {
                _lmsLiteratureIndexDivisionAisle = _dbContext.LmsLiteratureIndexDivisionAisles.ToList();
            }

            return _lmsLiteratureIndexDivisionAisle;
        }
        public async Task<LmsLiteratureIndexDivisionAisle> GetLiteratureIndexDivisionDetail(int divisionAisleId)
        {
            try
            {
                LmsLiteratureIndexDivisionAisle? task = await _dbContext.LmsLiteratureIndexDivisionAisles.FindAsync(divisionAisleId);
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
        public async Task<List<LmsLiteratureIndexDivisionAisle>> GetLmsLiteratureDivisionDetailsByUsingIndexId(int indexId)
        {
            try
            {
                List<LmsLiteratureIndexDivisionAisle> devisionResultList = new List<LmsLiteratureIndexDivisionAisle>();
                if (_lmsLiteratureIndexDivisionAisle == null)
                {
                    _lmsLiteratureIndexDivisionAisle = await _dbContext.LmsLiteratureIndexDivisionAisles.OrderBy(u => u.IndexId).Where(u => u.IndexId == indexId).ToListAsync();
                }
                devisionResultList = GetDistinctRecordsFromDivisionAisleList(_lmsLiteratureIndexDivisionAisle, devisionResultList);
                return devisionResultList;
            }
            catch
            {
                throw;
            }
        }

        private List<LmsLiteratureIndexDivisionAisle> GetDistinctRecordsFromDivisionAisleList(List<LmsLiteratureIndexDivisionAisle> lmsLiteratureIndexDivisionAisle, List<LmsLiteratureIndexDivisionAisle> devisionResultList)
        {
            try
            {
                var DistinctDivisions = _lmsLiteratureIndexDivisionAisle.Select(c => c.DivisionNumber).Distinct().ToList();
                foreach (var uniqueName in DistinctDivisions)
                {
                    var item = _lmsLiteratureIndexDivisionAisle.Where(x => x.DivisionNumber == uniqueName).FirstOrDefault();
                    devisionResultList.Add(item);
                }
                return devisionResultList;
            }
            catch (Exception)
            {

                throw;
            }
        }
        public async Task<List<LmsLiteratureIndexDivisionAisle>> GetLmsLiteratureDivisionDetailsByUsingIndexIdForViewPage(int indexId)
        {
            try
            {
                if (_lmsLiteratureIndexDivisionAisle == null)
                {
                    _lmsLiteratureIndexDivisionAisle = await _dbContext.LmsLiteratureIndexDivisionAisles.OrderBy(u => u.IndexId).Where(u => u.IndexId == indexId).ToListAsync();
                }
                return _lmsLiteratureIndexDivisionAisle;
            }
            catch
            {
                throw;
            }
        }
        public async Task<List<LmsLiteratureIndexDivisionAisle>> GetDivisionDetailsByUsingIndexAndDivisionId(int divisionAisleId, int indexId)
        {
            try
            {
                if (_lmsLiteratureIndexDivisionAisle == null)
                {
                    _lmsLiteratureIndexDivisionAisle = await _dbContext.LmsLiteratureIndexDivisionAisles.OrderBy(u => u.IndexId).Where(u => u.IndexId == indexId && u.DivisionAisleId == divisionAisleId).ToListAsync();
                }

                return _lmsLiteratureIndexDivisionAisle;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<LmsLiteratureIndexDivisionAisle>> GetLmsLiteratureAisleNumberDetailsByUsingIndexAndDivisionNumber(int indexId, string divisionNumber)
        {
            try
            {
                if (_lmsLiteratureIndexDivisionAisle == null)
                {
                    _lmsLiteratureIndexDivisionAisle = await _dbContext.LmsLiteratureIndexDivisionAisles.OrderByDescending(u => u.IndexId).Where(u => u.IndexId == indexId && u.DivisionNumber == divisionNumber).ToListAsync();

                }
                return _lmsLiteratureIndexDivisionAisle;
            }
            catch
            {
                throw;
            }
        }

        public async Task<List<LmsLiterature>> GetLmsLiteratureDivisionDetailByUsingDivisionId(int divisionAisleId)
        {
            try
            {
                _lmsLiteratureResult = await _dbContext.LmsLiteratures.Where(x => x.DivisionAisleId == divisionAisleId).ToListAsync();
                return _lmsLiteratureResult;
            }
            catch
            {
                throw new Exception();
            }
        }
        #endregion

        #region Create Division & Aisle
        public async Task CreateLmsLiteratureIndexDivision(LmsLiteratureIndexDivisionAisle lmsLiteratureIndexDivision)
        {
            try
            {
                _dbContext.LmsLiteratureIndexDivisionAisles.Add(lmsLiteratureIndexDivision);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }
        #endregion

        #region Update Division & Aisle

        public async Task UpdateLmsLiteratureIndexDivision(LmsLiteratureIndexDivisionAisle lmsLiteratureIndexDivision)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            _dbContext.Entry(lmsLiteratureIndexDivision).State = EntityState.Modified;
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

        #region Delete Division & Aisle (Permanent Delete not soft)
        public async Task DeleteLmsLiteratureIndexDivisionAisle(int divisionAisleId)
        {
            try
            {
                LmsLiteratureIndexDivisionAisle? _lmsLiteratureIndexDivisionAisle = await _dbContext.LmsLiteratureIndexDivisionAisles.FindAsync(divisionAisleId);
                if (_lmsLiteratureIndexDivisionAisle != null)
                {
                    _dbContext.LmsLiteratureIndexDivisionAisles.Remove(_lmsLiteratureIndexDivisionAisle);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch
            {
                _dbContext.Entry(_lmsLiteratureIndexDivisionAisle).State = EntityState.Unchanged;
                throw;
            }

        }
        #endregion
    }
}
