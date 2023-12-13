using Tasker.Application.DTOs.Application.TaskStatus;

namespace Tasker.Application.Interfaces.Repositories
{
    public interface ITaskStatusRepository
    {
        Task<TaskStatusDto?> CreateAsync(TaskStatusCreateDto statusDto);
        Task<TaskStatusDto?> UpdateAsync(TaskStatusUpdateDto statusDto);
        Task<bool> DeleteAsync(string id);
        Task<TaskStatusDto?> GetAsync(string id);
        Task<List<TaskStatusDto>> GetAllAsync();
    }
}
