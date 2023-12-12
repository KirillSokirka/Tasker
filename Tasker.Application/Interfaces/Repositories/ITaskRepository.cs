using Tasker.Application.DTOs.Application.Task;

namespace Tasker.Application.Interfaces.Repositories;

public interface ITaskRepository
{
    Task<TaskDto?> CreateAsync(TaskCreateDto dto);
    Task<TaskDto?> UpdateAsync(TaskUpdateDto dto);
    Task<bool> DeleteAsync(string id);
    Task<TaskDto?> GetAsync(string id);
    Task<List<TaskDto>> GetAllAsync();
}