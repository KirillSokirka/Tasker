using Tasker.Application.DTOs;

namespace Tasker.Application.Interfaces.Repositories
{
    public interface ITaskStatusRepository
    {
        Task<TaskStatusDto?> CreateAsync(TaskStatusDto statusDto);
        Task<TaskStatusDto?> UpdateAsync(TaskStatusDto statusDto);
        Task<bool> DeleteAsync(string id);
        Task<TaskStatusDto?> GetAsync(string id);
        Task<List<TaskStatusDto>> GetAllAsync();
    }
}
