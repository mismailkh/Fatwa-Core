using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using FATWA_INFRASTRUCTURE.Database;
using Microsoft.EntityFrameworkCore;

namespace FATWA_INFRASTRUCTURE.Repository
{
    public class TasksRepository : ITasks
    {
        private readonly DatabaseContext _dbContext;
        public TasksRepository(DatabaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<UserTask>> GetAllTasks()
        {
            try
            {
                return await _dbContext.Tasks.ToListAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task AddTask(UserTask task)
        {
            try
            {
                await _dbContext.Tasks.AddAsync(task);
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task UpdateTask(UserTask task)
        {
            try
            {
                _dbContext.Entry(task).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }
            catch
            {
                throw;
            }
        }

        public async Task DeleteTask(int taskID)
        {
            try
            {
                UserTask? task = await _dbContext.Tasks.FindAsync(taskID);
                if (task != null)
                {
                    _dbContext.Tasks.Remove(task);
                    await _dbContext.SaveChangesAsync();
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

        public async Task<UserTask> GetTaskDetail(int taskID)
        {
            try
            {
                UserTask? task = await _dbContext.Tasks.FindAsync(taskID);
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
    }
}
