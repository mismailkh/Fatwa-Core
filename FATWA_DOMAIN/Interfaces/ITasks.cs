using FATWA_DOMAIN.Models;

namespace FATWA_DOMAIN.Interfaces
{
    public interface ITasks
    {
        Task<List<UserTask>> GetAllTasks();
        Task AddTask(UserTask Task);
        Task UpdateTask(UserTask Task);
        Task<UserTask> GetTaskDetail(int taskID);
        Task DeleteTask(int taskID);
    }
}
