using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using FATWA_DOMAIN.Models.AdminModels.UserManagement;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.EntityFrameworkCore;

namespace FATWA_INFRASTRUCTURE.Repository
{
    //<History Author = 'Muhammad Zaeem' Date='2022-09-29' Version="1.0" Branch="master"> Repo for Performing DB operations related to UMS CLAIMS</History>

    public class ClaimUmsRepository : IClaims
    {
        private readonly DatabaseContext _dbContext;
        private List<ClaimUms> _ClaimUms;
        public ClaimUmsRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<ClaimUms>> GetClaimUms()
        {
            try
            {
                if (_ClaimUms == null)
                {
                    _ClaimUms = await _dbContext.Claims.OrderByDescending(u => u.Id).Where(x => x.IsDeleted == false).ToListAsync();
                }

                return _ClaimUms;
            }
            catch (Exception)
            {

                throw;
            }
            
        }
        public List<ClaimUms> GetClaimUmsSync()
        {
            if (_ClaimUms == null)
            {
                _ClaimUms = _dbContext.Claims.ToList();
            }

            return _ClaimUms;
        }

        public async Task<ClaimUms> GetClaimsById(int Id)
        {
            try
            {
                ClaimUms? task = await _dbContext.Claims.FindAsync(Id);
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

        public async Task CreateClaims(ClaimUms claimUms)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            _dbContext.Claims.Add(claimUms);
                            await _dbContext.SaveChangesAsync();
                            transaction.Commit();
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            throw ex;
                        }
                    }
                }
            }
            catch (Exception Mainex)
            {

                throw Mainex;
            }

        }

        #region update claims
        public async Task UpdateClaims(ClaimUms ClaimUms)
        {
            try
            {
                using (_dbContext)
                {
                    using (var transaction = _dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            _dbContext.Entry(ClaimUms).State = EntityState.Modified;
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
            catch (Exception)
            {

                throw;
            }
        }
        #endregion

        #region delete claims
        public async Task DeleteClaims(int Id)
        {
            try
            {
                ClaimUms? ClaimUms = await _dbContext.Claims.FindAsync(Id);
                if (ClaimUms != null)
                {
                    _dbContext.Claims.Remove(ClaimUms);
                    var success = await _dbContext.SaveChangesAsync();
                }
                else
                {
                    throw new ArgumentNullException();
                }
            }
            catch
            {
                _dbContext.Entry(_ClaimUms).State = EntityState.Unchanged;
                throw;
            }
        }
        #endregion

    }
}
