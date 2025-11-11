using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.EntityFrameworkCore;

namespace FATWA_INFRASTRUCTURE.Repository
{
    public class LmsLiteratureClassificationRepository : ILmsLiteratureClassification
    {
        private readonly DatabaseContext _dbContext;

        private List<LmsLiteratureClassification> _LmsLiteratureClassification;

        public LmsLiteratureClassificationRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<LmsLiteratureClassification>> GetLmsLiteratureClassifications()
        {
            try
            {
                if (_LmsLiteratureClassification == null)
                {
                    _LmsLiteratureClassification = await _dbContext.LmsLiteratureClassification.OrderByDescending(u => u.ClassificationId).ToListAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return _LmsLiteratureClassification;
        }

        public List<LmsLiteratureClassification> GetLmsLiteratureClassificationsSync()
        {
            if (_LmsLiteratureClassification == null)
            {
                _LmsLiteratureClassification = _dbContext.LmsLiteratureClassification.ToList();
            }

            return _LmsLiteratureClassification;
        }

        public Task CreateLmsLiteratureClassification(LmsLiteratureClassification LmsLiteratureClassification)
        {
            try
            {
                _dbContext.LmsLiteratureClassification.Add(LmsLiteratureClassification);
                _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return Task.CompletedTask;
        }

        public async Task<LmsLiteratureClassification> GetLiteratureClassificationDetailById(int Id)
        {
            try
            {
                LmsLiteratureClassification? entity = await _dbContext.LmsLiteratureClassification.FindAsync(Id);
                if (entity == null)
                {
                    throw new ArgumentNullException();
                }
                return entity;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Task UpdateLmsLiteratureClassification(LmsLiteratureClassification LmsLiteratureClassification)
        {
            try
            {
                
                LmsLiteratureClassification entity = _dbContext.LmsLiteratureClassification.Where( x => x.ClassificationId == LmsLiteratureClassification.ClassificationId).AsNoTracking().FirstOrDefault();
                if (entity == null)
                {
                    throw new ArgumentNullException();
                }
                else
                {
                    _dbContext.Update(LmsLiteratureClassification);
                    _dbContext.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return Task.CompletedTask;
        }

        public async Task<int> DeleteLmsLiteratureClassification(int Id)
        {
            try
            {
                var lmsliteraturelist = _dbContext.LmsLiteratures.Where(x => x.ClassificationId == Id).ToList();
                if (lmsliteraturelist != null && lmsliteraturelist.Count > 0)
                {
                    // throw new Exception("Connected classifications could'nt be deleted");
                    var result = lmsliteraturelist.Count;
                    return result;
                }

                LmsLiteratureClassification? _LmsLiteratureClassification = _dbContext.LmsLiteratureClassification.FirstOrDefault(x => x.ClassificationId == Id);
                if (_LmsLiteratureClassification != null)
                {
                    _dbContext.LmsLiteratureClassification.Remove(_LmsLiteratureClassification);
                    _dbContext.SaveChangesAsync();
                    return 0;
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch (Exception ex)
            {
                _dbContext.Entry(_LmsLiteratureClassification).State = EntityState.Unchanged;
                throw new Exception(ex.Message);
            }
        }
    }
}
