using Tasker.Application.DTOs.Application.Task;

namespace Tasker.Application.Interfaces.Services;

public interface ITaskService : IEntityService<TaskDto>
{
    Task<TaskDto> CreateAsync(TaskCreateDto createDto);
    Task<TaskDto?> UpdateAsync(TaskUpdateDto dto);
    Task<TaskDto?> UpdateTaskStatusAsync(TaskUpdateStatusDto dto);
}