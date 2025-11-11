using FATWA_DOMAIN.Interfaces;
using FATWA_DOMAIN.Models;
using Microsoft.AspNetCore.Mvc;

namespace FATWA_API.Controllers.V1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITasks _ITasks;
        public TasksController(ITasks itasks)
        {
            this._ITasks = itasks;
        }
        [HttpGet("GetAllTasks")]
        [MapToApiVersion("1.0")]
        public async Task<List<UserTask>> GetAllTasks()
        {
            return await _ITasks.GetAllTasks();
        }

        [HttpGet("{id}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Get(int id)
        {
            UserTask task = await _ITasks.GetTaskDetail((int)id);
            if (task != null)
            {
                return Ok(task);
            }
            return NotFound();
        }

        [HttpPost("AddTask")]
        [MapToApiVersion("1.0")]
        public async System.Threading.Tasks.Task AddTask(FATWA_DOMAIN.Models.UserTask task)
        {
            await _ITasks.AddTask(task);
        }

        [HttpPost("UpdateTask")]
        [MapToApiVersion("1.0")]
        public async System.Threading.Tasks.Task UpdateTask(FATWA_DOMAIN.Models.UserTask task)
        {
            await _ITasks.UpdateTask(task);
        }

        [HttpDelete("{id}")]
        [MapToApiVersion("1.0")]
        public async Task<IActionResult> Delete(int id)
        {
            await _ITasks.DeleteTask(id);
            return Ok();
        }
    }
}
