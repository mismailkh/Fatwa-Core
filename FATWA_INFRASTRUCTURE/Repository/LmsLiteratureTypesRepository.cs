using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.EntityFrameworkCore;

namespace FATWA_INFRASTRUCTURE.Repository
{
    //<History Author = 'Umer Zaman' Date='2022-03-15' Version="1.0" Branch="own"> create class & add functionality</History>
    public class LmsLiteratureTypesRepository : ILiteratureTypes
    {
        private readonly DatabaseContext _dbContext;

        private List<LmsLiteratureType>? _lmsLiteratureTypes;

        public LmsLiteratureTypesRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<LmsLiteratureType>> GetLmsLiteratureTypes()
        {
            try
            {
                if (_lmsLiteratureTypes == null)
                {
                    _lmsLiteratureTypes = await _dbContext.LmsLiteratureTypes.OrderByDescending(u => u.TypeId).ToListAsync();
                }

                return _lmsLiteratureTypes;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<LmsLiteratureType> GetLmsLiteratureTypesSync()
        {
            try
            {
                if (_lmsLiteratureTypes == null)
                {
                    _lmsLiteratureTypes = _dbContext.LmsLiteratureTypes.ToList();
                }
                return _lmsLiteratureTypes;
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task CreateLmsLiteratureType(LmsLiteratureType lmsLiteratureType)
        {
            try
            {
                _dbContext.LmsLiteratureTypes.Add(lmsLiteratureType);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }

        }

        public async Task<LmsLiteratureType> GetLiteratureTypeDetails(int id)
        {
            try
            {
                LmsLiteratureType? task = await _dbContext.LmsLiteratureTypes.FindAsync(id);
                if (task != null)
                {
                    return task;
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task UpdateLmsLiteratureType(LmsLiteratureType lmsLiterature)
        {
            try
            {
                _dbContext.Entry(lmsLiterature).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public async Task<int> DeleteLmsLiteratureType(int id)
        {
            try
            {
                LmsLiteratureType? _lmsLiteratureTypes = await _dbContext.LmsLiteratureTypes.FindAsync(id);
                var lmsliteraturelist = _dbContext.LmsLiteratures.Where(x => x.TypeId == id).ToList();
                if (lmsliteraturelist != null && lmsliteraturelist.Count > 0)
                {
                    // throw new Exception("Connected type could'nt be deleted");
                    var result = lmsliteraturelist.Count;
                    return result;
                }
                if (_lmsLiteratureTypes != null)
                {
                    _dbContext.LmsLiteratureTypes.Remove(_lmsLiteratureTypes);
                    await _dbContext.SaveChangesAsync();
                    return 0;
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

    }
}
