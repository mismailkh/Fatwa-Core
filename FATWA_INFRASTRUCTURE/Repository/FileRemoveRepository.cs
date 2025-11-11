using FATWA_DOMAIN.Interfaces;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.EntityFrameworkCore;

namespace FATWA_INFRASTRUCTURE.Repository
{
    public class FileRemoveRepository<T> : IFileRemove<T> where T : class
    {
        private readonly DatabaseContext _dbContext;
        private DbSet<T> table = null;
        public FileRemoveRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
            table = _dbContext.Set<T>();
        }

        public async Task<List<T>> GetAttachements(Object Id)
        {
            try
            {
                return await table.ToListAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task DeleteAttachement(Object id)
        {
            try
            {
                T existing = await table.FindAsync(id);
                table.Remove(existing);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
