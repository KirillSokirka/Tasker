using Tasker.Application.DTOs.Application.Task;
using Tasker.Application.DTOs.Application.TaskStatus;
using TaskStatus = Tasker.Domain.Entities.Application.TaskStatus;

namespace Tasker.Application.Interfaces.Services;

public interface ITaskStatusService : IEntityService<TaskStatusDto>
{
    Task<TaskStatusDto> CreateAsync(TaskStatusCreateDto dto);
    Task<TaskStatusDto> UpdateAsync(TaskStatusUpdateDto dto);
}