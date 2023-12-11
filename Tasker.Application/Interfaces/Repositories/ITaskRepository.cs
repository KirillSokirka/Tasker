using Tasker.Application.DTOs.Application;
using Tasker.Application.DTOs.Application.Task;

namespace Tasker.Application.Interfaces.Repositories;

public interface ITaskRepository
{
    Task<TaskDto?> CreateAsync(TaskDto dto);
    Task<TaskDto?> UpdateAsync(TaskUpdateDto dto);
    Task<bool> DeleteAsync(string id);
    Task<TaskDto?> GetAsync(string id);
}