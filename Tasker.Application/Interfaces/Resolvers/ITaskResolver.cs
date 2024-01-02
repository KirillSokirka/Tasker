using Tasker.Application.DTOs.Application.ResolvedProperties;
using Tasker.Application.DTOs.Application.Task;
using Task = Tasker.Domain.Entities.Application.Task;

namespace Tasker.Application.Interfaces.Resolvers;

public interface ITaskResolver : IResolver<Task, string>
{
    Task<TaskResolvedPropertiesDto> ResolveAsync(TaskUpdateDto dto);
    Task<Task> ResolveAsync(TaskCreateDto createDto);
    Task<TaskResolvedPropertiesDto> ResolveStatusAsync(TaskUpdateStatusDto dto);
}